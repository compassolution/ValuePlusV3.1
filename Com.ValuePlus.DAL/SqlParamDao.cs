using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Com.ValuePlus.Database;
using System.Data.Common;
using System.Collections;
using System.Data.SqlClient;
using Com.ValuePlus.Common.Config;

namespace Com.ValuePlus.DAL
{
    public class SqlParamDao
    {
        /// <summary>
        /// 日志声明
        /// </summary>
        protected static Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected static String strDbConnection = BaseConfig.Instance.GetConnectionString();

        #region 根据SQL语句获取DATATABLE数据集
        /// <summary>
        /// 根据SQL语句获取DATATABLE数据集
        /// </summary>
        /// <param name="Sql"></param>
        /// <returns></returns>
        public static DataTable GetDataTableBySql(String Sql)
        {
            DataTable dt = null;
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DataSet ds = dao.ExecuteDataSet(CommandType.Text, Sql, null);
                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                }
            }
            return dt;
        }
        #endregion

        #region 根据SQL语句获取DATASET数据集
        /// <summary>
        /// 根据SQL语句获取DATASET数据集
        /// </summary>
        /// <param name="Sql"></param>
        /// <returns></returns>
        public static DataSet GetDataSetBySql(String Sql)
        {
            DataSet ds = new DataSet();
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                ds = dao.ExecuteDataSet(CommandType.Text, Sql, null);
            }
            return ds;
        }
        #endregion

        #region 根据SQL语句执行更新操作返回影响记录数
        /// <summary>
        /// 根据SQL语句执行更新操作返回影响记录数
        /// </summary>
        /// <param name="Sql"></param>
        /// <returns></returns>
        public static int ExecuteNonQueryBySql(String Sql)
        {
            int count = 0;
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                object obj = dao.ExecuteNonQuery(CommandType.Text, Sql, null);
                if (obj != null)
                {
                    count = Convert.ToInt32(obj); ;
                }
            }
            return count;
        }
        #endregion

        #region 根据SQL语句执行操作返回结果集的第一行第一列的值
        /// <summary>
        /// 根据SQL语句执行操作返回结果集的第一行第一列的值
        /// </summary>
        /// <param name="Sql"></param>
        /// <returns></returns>
        public static int ExecuteScalarBySql(String Sql)
        {
            int count = 0;
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                object obj = dao.ExecuteScalar(CommandType.Text, Sql, null);
                if (obj != null)
                {
                    count = Convert.ToInt32(obj); ;
                }
            }
            return count;
        }
        #endregion

        #region 根据存储过程名称及其参数执行存储过程，返回存储过程返回值
        /// <summary>
        /// 根据存储过程名称及其参数执行存储过程，返回存储过程返回值(整数值)
        /// </summary>
        /// <param name="strSpName"></param>
        /// <param name="hsTableParam"></param>
        /// <returns>DataSet</returns>
        public static int ExcuteSP(String strSpName, Hashtable hsTableParam)
        {
            int iCount = 0;
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                //if ((hsTableParam != null) && (hsTableParam.Count > 0))
                //{
                //    foreach (System.Collections.DictionaryEntry entity in hsTableParam)
                //    {
                //        String strParamName = entity.Key.ToString();
                //        String strParamValue = hsTableParam[strParamName].ToString();
                //        dao.AddParameter("@" + strParamName, strParamValue, TypeDao.VarChar, 100);
                //    }
                //}
                //DbParameter[] param = dao.GetParameters();
                //Object obj = dao.ExecuteScalar(CommandType.StoredProcedure, strSpName, param);
                String strReturn = dao.ExecuteProcedure(strSpName, hsTableParam);
                if (!String.IsNullOrEmpty(strReturn))
                {
                    try
                    {
                        iCount = Convert.ToInt32(strReturn);
                    }
                    catch
                    {
                        iCount = 0;
                    }
                }
            }
            return iCount;
        }

        /// <summary>
        /// 根据存储过程名称及其参数执行存储过程，返回存储过程返回值(字符串)
        /// </summary>
        /// <param name="strSpName"></param>
        /// <param name="hsTableParam"></param>
        /// <returns>DataSet</returns>
        public static String ExcuteSPReturnStr(String strSpName, Hashtable hsTableParam)
        {
            String strReturn = "";
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                strReturn = dao.ExecuteProcedure(strSpName, hsTableParam);
            }
            return strReturn;
        }
        #endregion

        #region 判断数据库对象是否存在
        /// <summary>
        /// 判断数据库对象是否存在
        /// </summary>
        /// <param name="strObjectName"></param>
        /// <param name="strObjectType"></param>
        /// <returns></returns>
        public static bool IsExsitDbObject(String strObjectName, String strObjectType)
        {
            bool bIsExsit = false;
            String strSql = "SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'" + strObjectName + "') AND type in (N'" + strObjectType + "')";
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DataSet ds = dao.ExecuteDataSet(CommandType.Text, strSql, null);
                if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
                {
                    bIsExsit = true;
                }
            }
            return bIsExsit;
        }
        #endregion

        #region 通过SqlBulkCopy将DataTable数据集写入到数据表中，并返回记录数
        /// <summary>
        /// 将DataTable数据集写入到数据表中，并返回记录数
        /// </summary>
        /// <param name="dtData"></param>
        /// <param name="strDBTableName"></param>
        /// <returns></returns>
        public static int InsertDBFromDataTable(DataTable dtData, String strDBTableName)
        {
            int iRowCount = -1;
            try
            {
                if ((dtData != null) && (dtData.Rows.Count > 0) && !string.IsNullOrEmpty(strDBTableName))
                {
                    iRowCount = dtData.Rows.Count;
                    CreateTable(dtData.Columns, strDBTableName);

                    SqlBulkCopy sqlbulkcopy = new SqlBulkCopy(strDbConnection, SqlBulkCopyOptions.UseInternalTransaction);
                    sqlbulkcopy.DestinationTableName = strDBTableName;//数据库中的表名

                    sqlbulkcopy.WriteToServer(dtData);
                    sqlbulkcopy.Close();
                }
            }
            catch (Exception err)
            {
                log.Error("将DataTable数据集写入到数据表中失败：" + err.ToString());
            }

            return iRowCount;
        }

        /// <summary>
        /// 创建数据表
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="tableName"></param>
        private static void CreateTable(System.Data.DataColumnCollection columns, string tableName)
        {
            String strSql = "";
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[" + tableName + "]') AND type in (N'U'))\r\n");
                sb.Append("DROP TABLE [dbo].[" + tableName + "]\r\n");
                sb.Append("CREATE TABLE [" + tableName + "]\r\n");
                sb.Append("(\r\n");
                int iCount = 1;
                foreach (DataColumn column in columns)
                {
                    if(iCount<columns.Count)
                    {
                        sb.Append("    [" + column.ColumnName + "] " + GetTableColumnType(column.DataType, column.MaxLength) + ",\r\n");
                    }else
                    {
                        
                        sb.Append("    [" + column.ColumnName + "] " + GetTableColumnType(column.DataType, column.MaxLength) + "\r\n");
                    }
                    iCount++;
                }
                sb.Append(")");
                strSql = sb.ToString();
                ExecuteNonQueryBySql(strSql);

            }
            catch (Exception err)
            {
                log.Error("创建数据表失败：" + err.ToString());
                log.Error("创建数据表失败：" + strSql);
            }
        }

        /// <summary>
        /// 创建列
        /// </summary>
        /// <param name="type"></param>
        /// <param name="iLength"></param>
        /// <returns></returns>
        private static string GetTableColumnType(System.Type type, int iLength)
        {
            string result = "varchar(" + iLength.ToString() + ")";
            string sDbType = type.ToString();
            switch (sDbType)
            {
                case "System.String":
                    break;
                case "System.Int16":
                    result = "int";
                    break;
                case "System.Int32":
                    result = "int";
                    break;
                case "System.Int64":
                    result = "float";
                    break;
                case "System.Decimal":
                    result = "decimal(18,4)";
                    break;
                case "System.Double":
                    result = "decimal(18,4)";
                    break;
                case "System.DateTime":
                    result = "datetime";
                    break;
                default:
                    break;
            }
            return result;
        } 


        #endregion

    }
}
