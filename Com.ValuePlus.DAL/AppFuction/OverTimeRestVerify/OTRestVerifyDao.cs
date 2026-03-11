using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Com.ValuePlus.Database;
using Com.ValuePlus.Common.Config;
using System.Collections;
using Com.ValuePlus.Utils;

namespace Com.ValuePlus.DAL.AppFuction.OverTimeRestVerify
{
    public class OTRestVerifyDao
    {

        /// <summary>
        /// 日志声明
        /// </summary>
        protected static Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region 通过当前用户ID查询视图【VW_PAIBAN_STAFF_FILTER】中对应的员工记录，返回DataSet记录集
        /// <summary>
        /// 通过当前用户ID查询视图【VW_PAIBAN_STAFF_FILTER】中对应的员工记录，返回DataSet记录集
        /// <param name="strUserId"></param>
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet findStuffInfoByUserId(String strUserId)
        {
            DataSet ds = new DataSet();
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(SqlConfig_wsm.Instance.GetSql_VW_PAIBAN_STAFF_FILTER_select_byUserId());
                param[0].Value = strUserId;
                ds = dao.ExecuteDataSet(SqlConfig_wsm.Instance.GetSql_VW_PAIBAN_STAFF_FILTER_select_byUserId(), param);

            }
            return ds;
        }
        #endregion

        #region 通过当前员工编号查询表【KQOVTM_1】中对应的加班记录，返回DataSet记录集
        /// <summary>
        /// 通过当前员工编号查询表【KQOVTM_1】中对应的加班记录，返回DataSet记录集
        /// <param name="strStuffId"></param>
        /// <param name="dtFrom"></param>
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet findOverTimeInfoByStuffId(String strStuffId, DateTime dtFrom)
        {
            DataSet ds = new DataSet();
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(SqlConfig_wsm.Instance.GetSql_KQOVTM_1_select_byStuffId());
                param[0].Value = strStuffId;
                param[1].Value = dtFrom;
                ds = dao.ExecuteDataSet(SqlConfig_wsm.Instance.GetSql_KQOVTM_1_select_byStuffId(), param);

            }
            return ds;
        }
        #endregion

        #region 通过当前员工编号查询表【KQLV_1】中对应的调休记录，返回DataSet记录集
        /// <summary>
        /// 通过当前员工编号查询表【KQLV_1】中对应的加班记录，返回DataSet记录集
        /// <param name="strStuffId"></param>
        /// <param name="dtFrom"></param>
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet findLvInfoByStuffId(String strStuffId, DateTime dtFrom)
        {
            DataSet ds = new DataSet();
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(SqlConfig_wsm.Instance.GetSql_KQLV_1_select_byStuffId());
                param[0].Value = strStuffId;
                param[1].Value = dtFrom;
                ds = dao.ExecuteDataSet(SqlConfig_wsm.Instance.GetSql_KQLV_1_select_byStuffId(), param);

            }
            return ds;
        }
        #endregion

        #region 根据存储过程名称及其参数执行存储过程，返回影响记录数
        /// <summary>
        /// 根据存储过程名称及其参数执行存储过程，返回影响记录数
        /// </summary>
        /// <param name="strSpName"></param>
        /// <param name="hsTableParam"></param>
        /// <returns>DataSet</returns>
        public int ExcuteSP(String strSpName, Hashtable hsTableParam)
        {
            int iCount = 0;
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                if ((hsTableParam != null) && (hsTableParam.Count > 0))
                    foreach (System.Collections.DictionaryEntry entity in hsTableParam)
                    {
                        String strParamName = entity.Key.ToString();
                        String strParamValue = hsTableParam[strParamName].ToString();
                        dao.AddParameter("@" + strParamName, strParamValue, TypeDao.VarChar, 100);
                    }
                DbParameter[] param = dao.GetParameters();
                iCount = dao.ExecuteNonQuery(CommandType.StoredProcedure, strSpName, param);
            }
            return iCount;
        }
        #endregion


    }
}
