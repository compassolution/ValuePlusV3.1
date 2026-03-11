using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Com.ValuePlus.Database;
using Com.ValuePlus.Common.Config;

namespace Com.ValuePlus.DAL.WinForm
{
    public class KQUnNormalResultDao
    {
        /// <summary>
        /// 日志声明
        /// </summary>
        protected static Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        #region 根据考勤月份获取VW_HR_KQ_UNNORMAL记录，返回DataSet记录集
        /// <summary>
        /// 根据考勤月份获取表VW_HR_KQ_UNNORMAL记录，返回DataSet记录集
        /// <param name="strYearMonth"></param>
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet findUnNormalByYearMonth(String strYearMonth)
        {
            DataSet ds = new DataSet();
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(SqlConfig_wsm.Instance.GetSql_VW_HR_KQ_UNNORMAL_select_byYearMonth());
                param[0].Value = strYearMonth;
                ds = dao.ExecuteDataSet(SqlConfig_wsm.Instance.GetSql_VW_HR_KQ_UNNORMAL_select_byYearMonth(), param);
            }
            return ds;
        }
        #endregion

        //#region 根据考勤月份获取表TB_HR_KQ_UNNORMAL记录，返回DataSet记录集
        ///// <summary>
        ///// 根据考勤月份获取表TB_HR_KQ_UNNORMAL记录，返回DataSet记录集
        ///// <param name="strYearMonth"></param>
        ///// </summary>
        ///// <returns>DataSet</returns>
        //public DataSet findUnNormalByYearMonth(String strYearMonth)
        //{
        //    DataSet ds = new DataSet();
        //    using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
        //    {
        //        DbParameter[] param = dao.MakeParameter(SqlConfig_wsm.Instance.GetSql_TB_HR_KQ_UNNORMAL_select_byYearMonth());
        //        param[0].Value = strYearMonth;
        //        ds = dao.ExecuteDataSet(SqlConfig_wsm.Instance.GetSql_TB_HR_KQ_UNNORMAL_select_byYearMonth(), param);
        //    }
        //    return ds;
        //}
        //#endregion

        #region 根据员工编号及考勤日期获取表TB_HR_KQ_UNNORMAL记录，返回DataSet记录集
        /// <summary>
        /// 根据员工编号及考勤日期获取表TB_HR_KQ_UNNORMAL记录，返回DataSet记录集
        /// <param name="strEmNo"></param>
        /// <param name="strYearMonth"></param>
        /// <param name="strDay"></param>
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet findUnNormalByEmnoADay(String strEmNo, String strYearMonth, String strDay)
        {
            DataSet ds = new DataSet();
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(SqlConfig_wsm.Instance.GetSql_TB_HR_KQ_UNNORMAL_select_byNODay());
                param[0].Value = strEmNo;
                param[1].Value = strYearMonth;
                param[2].Value = strDay;
                ds = dao.ExecuteDataSet(SqlConfig_wsm.Instance.GetSql_TB_HR_KQ_UNNORMAL_select_byNODay(), param);
            }
            return ds;
        }
        #endregion

        #region 根据员工编号及考勤日期更新表TB_HR_KQ_UNNORMAL记录，返回记录数
        /// <summary>
        /// 根据员工编号及考勤日期更新表TB_HR_KQ_UNNORMAL记录，返回记录数
        /// <param name="strEmNo"></param>
        /// <param name="strYearMonth"></param>
        /// <param name="strDay"></param>
        /// <param name="strState"></param>
        /// </summary>
        /// <returns>DataSet</returns>
        public int updateUnNormalByEmnoADay(String strEmNo, String strYearMonth, String strDay, String strState)
        {
            DataSet ds = new DataSet();
            int count = 0;
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(SqlConfig_wsm.Instance.GetSql_TB_HR_KQ_UNNORMAL_update_byNODay());
                param[0].Value = strEmNo;
                param[1].Value = strYearMonth;
                param[2].Value = strDay;
                param[3].Value = strState;
                ds = dao.ExecuteDataSet(SqlConfig_wsm.Instance.GetSql_TB_HR_KQ_UNNORMAL_update_byNODay(), param);
            }
            return count;
        }
        #endregion

    }
}
