using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Com.ValuePlus.Database;
using Com.ValuePlus.Common.Config;

namespace Com.ValuePlus.DAL.Report
{
    public class ReportMainDao
    {
        #region 查询视图VW_HR_UP1所有信息,返回dataset记录
        /// <summary>
        /// 查询视图VW_HR_UP1所有信息,返回dataset记录
        /// </summary>
        /// <param name="strUserCode"></param>
        /// <returns>DataSet</returns>
        public DataSet selectViewHrUp1All(String strUserCode)
        {
            DataSet ds = new DataSet();
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(SqlConfig_report.Instance.GetSql_VW_HR_UP1_select_byUserCode());
                param[0].Value = strUserCode;
                ds = dao.ExecuteDataSet(SqlConfig_report.Instance.GetSql_VW_HR_UP1_select_byUserCode(), param);

            }
            return ds;
        }
        #endregion

        #region 根据主键查询数据表【TB_HRTMPSD】的一条记录,返回dataset记录
        /// <summary>
        /// 根据主键查询数据表【TB_HRTMPSD】的一条记录,返回dataset记录
        /// </summary>
        /// <param name="strTid"></param>
        /// <param name="strGid"></param>
        /// <param name="strSid"></param>
        /// <param name="strPid"></param>
        /// <returns></returns>
        public DataSet selectTB_HRTMPSD_ByKey(String strTid, String strGid, String strSid, String strPid)
        {
            DataSet ds = new DataSet();
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(SqlConfig_report.Instance.GetSql_TB_HRTMPSD_select_byKey());
                param[0].Value = strTid;
                param[1].Value = strGid;
                param[2].Value = strSid;
                param[3].Value = strPid;
                ds = dao.ExecuteDataSet(SqlConfig_report.Instance.GetSql_TB_HRTMPSD_select_byKey(), param);

            }
            return ds;
        }
        #endregion

    }
}
