using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Com.ValuePlus.Database;
using Com.ValuePlus.Common.Config;

namespace Com.ValuePlus.DAL.WinForm
{
    public class KQPerdDao
    {
        /// <summary>
        /// 日志声明
        /// </summary>
        protected static Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region 查询考勤日期设定表所有记录，返回DataSet记录集
        /// <summary>
        /// 查询考勤日期设定表所有记录，返回DataSet记录集
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet findAll()
        {
            DataSet ds = new DataSet();
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                ds = dao.ExecuteDataSet(SqlConfig_wsm.Instance.GetSql_KQPERD_1_selectAll(), null);

            }
            return ds;
        }
        #endregion

        #region 根据PID获取数据表【KQPERD_1】相应记录集，返回DataSet记录集
        /// <summary>
        /// 根据PID获取数据表【KQPERD_1】相应记录集，返回DataSet记录集
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet findById(String strPid)
        {
            DataSet ds = new DataSet();
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(SqlConfig_wsm.Instance.GetSql_KQPERD_1_select_byId());
                param[0].Value = strPid;
                ds = dao.ExecuteDataSet(SqlConfig_wsm.Instance.GetSql_KQPERD_1_select_byId(), param);

            }
            return ds;
        }
        #endregion
    }
}
