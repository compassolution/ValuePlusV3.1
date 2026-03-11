using System;
using System.Text;
using Com.ValuePlus.Database;
using System.Data;
using System.Collections;
using System.Data.Common;

namespace Com.ValuePlus.DataLog.DAL
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

    }
}
