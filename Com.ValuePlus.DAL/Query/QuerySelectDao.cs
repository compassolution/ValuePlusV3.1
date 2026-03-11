using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Com.ValuePlus.Database;
using Com.ValuePlus.Common.Config;

namespace Com.ValuePlus.DAL.Query
{
    public class QuerySelectDao
    {
        /// <summary>
        /// 日志声明
        /// </summary>
        protected static Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region 通过sql语句，获取对应数据集
        /// <summary>
        /// 通过sql语句，获取对应数据集
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns>DataTable</returns>
        public DataTable selectDataBySql(String strSql)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                ds = dao.ExecuteDataSet(CommandType.Text,strSql, null);
                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                }
            }
            return dt;
        }
        #endregion
    }
}
