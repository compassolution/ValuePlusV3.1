using System;
using System.Text;
using Com.ValuePlus.Database;
using System.Data;
using System.Collections;
using System.Data.Common;

namespace Com.ValuePlus.Archive.DAL
{
    public class SqlParamDao
    {
        /// <summary>
        /// 日志声明
        /// </summary>
        protected static Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
                String strReturn = dao.ExecuteProcedure(strSpName,hsTableParam);
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

        #region 通过数据表或者视图名称，服务器端分页获取该对象的每页记录，返回dataset
        /// <summary>
        /// 通过数据表或者视图名称，服务器端分页获取该对象的每页记录，返回dataset
        /// </summary>
        /// <param name="strTableName"></param>
        /// <param name="iPageIndex">当前页数</param>
        /// <param name="iPageSize">每页显示数</param>
        /// <param name="iRecordCount">返回的总记录数</param>
        /// <returns>DataSet</returns>
        public static DataSet GetDataSetByPage(String strTableName,String strKeyName, int iPageIndex, int iPageSize, ref int iRecordCount)
        {
            DataSet ds = new DataSet();
            int iStartIndex = iPageIndex * iPageSize;
            String strSql = "select top " + iPageSize.ToString() + " * from " + strTableName + " where " + strKeyName + " NOT IN (SELECT TOP " + iStartIndex.ToString() + " " + strKeyName + " FROM " + strTableName + " ORDER BY " + strKeyName + ")" + " ORDER BY " + strKeyName;
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                ds = dao.ExecuteDataSet(CommandType.Text, strSql, null);
            }
            //获取全部记录数
            iRecordCount = 0;
            String strSql1 = "select count(*) from  " + strTableName;
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                Object obj = dao.ExecuteScalar(CommandType.Text, strSql1, null);
                if (obj != null)
                {
                    iRecordCount = Convert.ToInt32(obj);
                }
            }

            return ds;
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
                if ((ds!=null)&&(ds.Tables.Count>0)&&(ds.Tables[0].Rows.Count>0))
                {
                    bIsExsit = true;
                }
            }
            return bIsExsit;
        }
        #endregion

    }
}
