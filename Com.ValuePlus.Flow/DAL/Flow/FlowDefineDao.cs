using System;
using System.Text;
using Com.ValuePlus.Database;
using Com.ValuePlus.Flow.Config;
using System.Data;
using System.Data.Common;

namespace Com.ValuePlus.Flow.DAL.Flow
{
    public class FlowDefineDao
    {
        /// <summary>
        /// 日志声明
        /// </summary>
        protected static Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region 查询表TB_FLOW_DEFINE所有记录，返回dateset记录集
        /// <summary>
        /// 查询表TB_FLOW_DEFINE所有记录，返回dateset记录集
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet findAll()
        {
            DataSet ds = new DataSet();
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                ds = dao.ExecuteDataSet(FlowSqlConfig.Instance.GetSqlForTB_FLOW_DEFINE_selectAll(), null);
            }
            return ds;
        }
        #endregion

        #region 根据主键查询TB_FLOW_DEFINE的相应记录，返回dateset记录集
        /// <summary>
        /// 根据主键查询TB_FLOW_DEFINE的相应记录，返回dateset记录集
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet findByKey(String strKeyValue)
        {
            DataSet ds = new DataSet();
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(FlowSqlConfig.Instance.GetSqlForTB_FLOW_DEFINE_selectByKey());
                param[0].Value = strKeyValue;
                ds = dao.ExecuteDataSet(FlowSqlConfig.Instance.GetSqlForTB_FLOW_DEFINE_selectByKey(), param);
            }
            return ds;
        }
        #endregion

        #region 新增一条记录到表TB_FLOW_DEFINE中，返回成功新增记录数
        /// <summary>
        /// 新增一条记录到表TB_FLOW_DEFINE中，返回成功新增记录数
        /// </summary>
        /// <returns>DataSet</returns>
        public int insertOneRow(String strSFLOWCODE, String strSFLOWNAME, String strSFLOWNAMECN, String strSFLOWDESC, String strSFLOWDESCCN, String strNWORKCOUNTDAY, String strBISNEEDACCEPT, String strBISACTIVEWITHSUB, String strBSTOP)
        {
            int count = 0;
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(FlowSqlConfig.Instance.GetSqlForTB_FLOW_DEFINE_insert());
                param[0].Value = strSFLOWCODE;
                param[1].Value = strSFLOWNAME;
                param[2].Value = strSFLOWNAMECN;
                param[3].Value = strSFLOWDESC;
                param[4].Value = strSFLOWDESCCN;
                param[5].Value = strNWORKCOUNTDAY;
                param[6].Value = strBISNEEDACCEPT;
                param[7].Value = strBISACTIVEWITHSUB;
                param[8].Value = strBSTOP;
                object obj = dao.ExecuteScalar(FlowSqlConfig.Instance.GetSqlForTB_FLOW_DEFINE_insert(), param);
                if (obj != null)
                {
                    count = Convert.ToInt32(obj); ;
                }
            }
            return count;
        }
        #endregion

        #region 根据主键更新表TB_FLOW_DEFINE一条记录，返回成功更新记录数
        /// <summary>
        /// 根据主键更新表TB_FLOW_DEFINE一条记录，返回成功更新记录数
        /// </summary>
        /// <returns>DataSet</returns>
        public int updateByKey(String strSFLOWCODE, String strSFLOWNAME, String strSFLOWNAMECN, String strSFLOWDESC, String strSFLOWDESCCN, String strNWORKCOUNTDAY, String strBISNEEDACCEPT, String strBISACTIVEWITHSUB, String strBSTOP)
        {
            int count = 0;
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(FlowSqlConfig.Instance.GetSqlForTB_FLOW_DEFINE_updateByKey());
                param[0].Value = strSFLOWCODE;
                param[1].Value = strSFLOWNAME;
                param[2].Value = strSFLOWNAMECN;
                param[3].Value = strSFLOWDESC;
                param[4].Value = strSFLOWDESCCN;
                param[5].Value = strNWORKCOUNTDAY;
                param[6].Value = strBISNEEDACCEPT;
                param[7].Value = strBISACTIVEWITHSUB;
                param[8].Value = strBSTOP;
                object obj = dao.ExecuteNonQuery(FlowSqlConfig.Instance.GetSqlForTB_FLOW_DEFINE_updateByKey(), param);
                if (obj != null)
                {
                    count = Convert.ToInt32(obj); ;
                }
            }
            return count;
        }
        #endregion

        #region 根据主键删除表TB_FLOW_DEFINE的相应记录，返回成功删除记录数
        /// <summary>
        /// 根据主键删除表TB_FLOW_DEFINE的相应记录，返回成功删除记录数
        /// </summary>
        /// <returns>DataSet</returns>
        public int deleteByKey(String strKeyValue)
        {
            int count = 0;
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(FlowSqlConfig.Instance.GetSqlForTB_FLOW_DEFINE_deleteByKey());
                param[0].Value = strKeyValue;
                object obj = dao.ExecuteNonQuery(FlowSqlConfig.Instance.GetSqlForTB_FLOW_DEFINE_deleteByKey(), param);
                if (obj != null)
                {
                    count = Convert.ToInt32(obj); ;
                }
            }
            return count;
        }
        #endregion

        #region 根据流程实例ID获取表TB_FLOW_DEFINE的一条记录，返回dateset记录集
        /// <summary>
        /// 根据流程实例ID获取表TB_FLOW_DEFINE的一条记录，返回dateset记录集
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet findDefineByWorkFlowCode(String strWorkFlowCode)
        {
            DataSet ds = new DataSet();
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(FlowSqlConfig.Instance.GetSqlForTB_FLOW_DEFINE_selectByWorkFlowCode());
                param[0].Value = strWorkFlowCode;
                ds = dao.ExecuteDataSet(FlowSqlConfig.Instance.GetSqlForTB_FLOW_DEFINE_selectByWorkFlowCode(), param);
            }
            return ds;
        }
        #endregion

    }
}
