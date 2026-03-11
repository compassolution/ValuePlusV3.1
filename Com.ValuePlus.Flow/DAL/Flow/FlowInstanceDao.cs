using System;
using System.Text;
using Com.ValuePlus.Database;
using Com.ValuePlus.Flow.Config;
using System.Data;
using System.Data.Common;
using Com.ValuePlus.Flow.Entity;
using Com.ValuePlus.Entity;

namespace Com.ValuePlus.Flow.DAL.Flow
{
    public class FlowInstanceDao
    {
        /// <summary>
        /// 日志声明
        /// </summary>
        protected static Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region 查询表TB_FLOW_WORK_INSTANCE所有记录，返回dateset记录集
        /// <summary>
        /// 查询表TB_FLOW_WORK_INSTANCE所有记录，返回dateset记录集
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet findAll()
        {
            DataSet ds = new DataSet();
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                ds = dao.ExecuteDataSet(FlowSqlConfig.Instance.GetSqlForTB_FLOW_WORK_INSTANCE_selectAll(), null);
            }
            return ds;
        }
        #endregion

        #region 根据主键查询TB_FLOW_WORK_INSTANCE的相应记录，返回dateset记录集
        /// <summary>
        /// 根据主键查询TB_FLOW_WORK_INSTANCE的相应记录，返回dateset记录集
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet findByKey(String strKeyValue)
        {
            DataSet ds = new DataSet();
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(FlowSqlConfig.Instance.GetSqlForTB_FLOW_WORK_INSTANCE_selectByKey());
                param[0].Value = strKeyValue;
                ds = dao.ExecuteDataSet(FlowSqlConfig.Instance.GetSqlForTB_FLOW_WORK_INSTANCE_selectByKey(), param);
            }
            return ds;
        }
        #endregion

        #region 新增一条记录到表TB_FLOW_WORK_INSTANCE中，返回成功新增记录数
        /// <summary>
        /// 新增一条记录到表TB_FLOW_WORK_INSTANCE中，返回成功新增记录数
        /// </summary>
        /// <returns>DataSet</returns>
        public int insertOneRow(string strSWORKFLOWCODE, string strSFLOWCODE, string strSWORKFLOWNAME, string strSWORKFLOWNAMECN, string strSENTITYID, string strSENTITYNAME, string strSENTITYNAMECN, string strSFLOWACCEPTNO, DateTime dtDTSTARTDATE, string strSUSERID, string strSDEPTID, string strSFLOWMOVECODE, string strSWKSCODE, decimal nNCOUNTWORKDAY, decimal nNSUBNUMBER, string strSBIZSTATUS, string strBISSUBFLOW, string strSPARENTCODE, string strSPENDINGUSERID)
        {
            int count = 0;
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(FlowSqlConfig.Instance.GetSqlForTB_FLOW_WORK_INSTANCE_insert());
                param[0].Value = strSWORKFLOWCODE;
                param[1].Value = strSFLOWCODE;
                param[2].Value = strSWORKFLOWNAME;
                param[3].Value = strSWORKFLOWNAMECN;
                param[4].Value = strSENTITYID;
                param[5].Value = strSENTITYNAME;
                param[6].Value = strSENTITYNAMECN;
                param[7].Value = strSFLOWACCEPTNO;
                param[8].Value = dtDTSTARTDATE;
                param[9].Value = strSUSERID;
                param[10].Value = strSDEPTID;
                param[11].Value = strSFLOWMOVECODE;
                param[12].Value = strSWKSCODE;
                param[13].Value = nNCOUNTWORKDAY;
                param[14].Value = nNSUBNUMBER;
                param[15].Value = strSBIZSTATUS;
                param[16].Value = strBISSUBFLOW;
                param[17].Value = strSPARENTCODE;
                param[18].Value = strSPENDINGUSERID;
                object obj = dao.ExecuteScalar(FlowSqlConfig.Instance.GetSqlForTB_FLOW_WORK_INSTANCE_insert(), param);
                if (obj != null)
                {
                    count = Convert.ToInt32(obj); ;
                }
            }
            return count;
        }
        #endregion

        #region 根据主键更新表TB_FLOW_WORK_INSTANCE一条记录，返回成功更新记录数
        /// <summary>
        /// 根据主键更新表TB_FLOW_WORK_INSTANCE一条记录，返回成功更新记录数
        /// </summary>
        /// <returns>DataSet</returns>
        public int updateByKey(string strSWORKFLOWCODE, string strSFLOWCODE, string strSWORKFLOWNAME, string strSWORKFLOWNAMECN, string strSENTITYID, string strSENTITYNAME, string strSENTITYNAMECN, string strSFLOWACCEPTNO, DateTime dtDTSTARTDATE, string strSUSERID, string strSDEPTID, string strSFLOWMOVECODE, string strSWKSCODE, decimal nNCOUNTWORKDAY, decimal nNSUBNUMBER, string strSBIZSTATUS, string strBISSUBFLOW, string strSPARENTCODE, string strSPENDINGUSERID)
        {
            int count = 0;
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(FlowSqlConfig.Instance.GetSqlForTB_FLOW_WORK_INSTANCE_updateByKey());
                param[0].Value = strSWORKFLOWCODE;
                param[1].Value = strSFLOWCODE;
                param[2].Value = strSWORKFLOWNAME;
                param[3].Value = strSWORKFLOWNAMECN;
                param[4].Value = strSENTITYID;
                param[5].Value = strSENTITYNAME;
                param[6].Value = strSENTITYNAMECN;
                param[7].Value = strSFLOWACCEPTNO;
                param[8].Value = dtDTSTARTDATE;
                param[9].Value = strSUSERID;
                param[10].Value = strSDEPTID;
                param[11].Value = strSFLOWMOVECODE;
                param[12].Value = strSWKSCODE;
                param[13].Value = nNCOUNTWORKDAY;
                param[14].Value = nNSUBNUMBER;
                param[15].Value = strSBIZSTATUS;
                param[16].Value = strBISSUBFLOW;
                param[17].Value = strSPARENTCODE;
                param[18].Value = strSPENDINGUSERID;
                object obj = dao.ExecuteNonQuery(FlowSqlConfig.Instance.GetSqlForTB_FLOW_WORK_INSTANCE_updateByKey(), param);
                if (obj != null)
                {
                    count = Convert.ToInt32(obj); ;
                }
            }
            return count;
        }
        #endregion
        
        #region 根据主键删除表TB_FLOW_WORK_INSTANCE的相应记录，返回成功删除记录数
        /// <summary>
        /// 根据主键删除表TB_FLOW_WORK_INSTANCE的相应记录，返回成功删除记录数
        /// </summary>
        /// <returns>DataSet</returns>
        public int deleteByKey(String strKeyValue)
        {
            int count = 0;
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(FlowSqlConfig.Instance.GetSqlForTB_FLOW_WORK_INSTANCE_deleteByKey());
                param[0].Value = strKeyValue;
                object obj = dao.ExecuteNonQuery(FlowSqlConfig.Instance.GetSqlForTB_FLOW_WORK_INSTANCE_deleteByKey(), param);
                if (obj != null)
                {
                    count = Convert.ToInt32(obj); ;
                }
            }
            return count;
        }
        #endregion

        #region 对数据表【TB_FLOW_WORK_INSTANCE】和【TB_FLOW_WORKFLOW_DETAIL】的selectByUserid操作相关配置文件（待办事项查询），返回dateset记录集
        /// <summary>
        /// 对数据表【TB_FLOW_WORK_INSTANCE】和【TB_FLOW_WORKFLOW_DETAIL】的selectByUserid操作相关配置文件（待办事项查询），返回dateset记录集
        /// </summary>
        /// <param name="strUserId"></param>
        /// <param name="iPageSize"></param>
        /// <param name="iStartIndex"></param>
        /// <param name="strCondition"></param>
        /// <param name="iRecordCount"></param>
        /// <returns></returns>
        public DataSet findPendingListByUserId(String strUserId, int iPageSize, int iStartIndex, String strCondition, ref int iRecordCount)
        {
            DataSet ds = new DataSet();
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                //DbParameter[] param = dao.MakeParameter(FlowSqlConfig.Instance.GetSqlForTB_FLOW_WORK_INSTANCE_selectPendingByUserId());
                //param[0].Value = iPageSize;
                //param[1].Value = iStartIndex;
                //param[2].Value = strUserId;
                //param[3].Value = strCondition;
                //ds = dao.ExecuteDataSet(FlowSqlConfig.Instance.GetSqlForTB_FLOW_WORK_INSTANCE_selectPendingByUserId(), param);

                ////获取全部数据
                //DbParameter[] param_All = dao.MakeParameter(FlowSqlConfig.Instance.GetSqlForTB_FLOW_WORK_INSTANCE_selectPendingByUserId());
                //param_All[0].Value = 1000;
                //param_All[1].Value = 0;
                //param_All[2].Value = strUserId;
                //param_All[3].Value = strCondition;
                //DataSet ds_All = dao.ExecuteDataSet(FlowSqlConfig.Instance.GetSqlForTB_FLOW_WORK_INSTANCE_selectPendingByUserId(), param_All);

                //分页查询数据集
                Com.ValuePlus.Utils.SqlBasicMetaData sqlData = FlowSqlConfig.Instance.GetSqlForTB_FLOW_WORK_INSTANCE_selectPendingByUserId();
                String sqlBase = sqlData.CommandSql;
                DbParameter[] param = dao.MakeParameter(FlowSqlConfig.Instance.GetSqlForTB_FLOW_WORK_INSTANCE_selectPendingByUserId());
                if (param != null)
                {
                    for (int i = 0; i < param.Length; i++)
                    {
                        DbParameter p = (DbParameter)param[i];
                        if (p != null)
                        {
                            String strParamName = p.ParameterName;
                            switch (i)
                            {
                                case 0:
                                    sqlBase = sqlBase.Replace(strParamName, iPageSize.ToString());
                                    break;
                                case 1:
                                    sqlBase = sqlBase.Replace(strParamName, iStartIndex.ToString());
                                    break;
                                case 2:
                                    sqlBase = sqlBase.Replace(strParamName, "'"+strUserId+"'");
                                    break;
                                case 3:
                                    sqlBase = sqlBase.Replace(strParamName, strCondition);
                                    break;
                            }
                        }
                    }
                }
                ds = dao.ExecuteDataSet(CommandType.Text, sqlBase, null);

                //查询数据总数
                String sqlBase_All = sqlData.CommandSql;
                DbParameter[] param_All = dao.MakeParameter(FlowSqlConfig.Instance.GetSqlForTB_FLOW_WORK_INSTANCE_selectPendingByUserId());
                if (param_All != null)
                {
                    for (int i = 0; i < param_All.Length; i++)
                    {
                        DbParameter p = (DbParameter)param_All[i];
                        if (p != null)
                        {
                            String strParamName = p.ParameterName;
                            switch (i)
                            {
                                case 0:
                                    sqlBase_All = sqlBase_All.Replace(strParamName, System.Convert.ToString(1000));
                                    break;
                                case 1:
                                    sqlBase_All = sqlBase_All.Replace(strParamName, System.Convert.ToString(0));
                                    break;
                                case 2:
                                    sqlBase_All = sqlBase_All.Replace(strParamName, "'" + strUserId + "'");
                                    break;
                                case 3:
                                    sqlBase_All = sqlBase_All.Replace(strParamName, strCondition);
                                    break;
                            }
                        }
                    }
                }
                DataSet ds_All = dao.ExecuteDataSet(CommandType.Text, sqlBase_All, null);
                if (ds_All != null)
                {
                    iRecordCount = ds_All.Tables[0].Rows.Count;
                }
            }
            return ds;
        }
        #endregion

        #region 创建流程实例，分别在TB_FLOW_WORK_INSTANCE以及TB_FLOW_WORKFLOW_DETAIL表中新增一条记录，返回成功新增记录数
        /// <summary>
        /// 创建流程实例，分别在TB_FLOW_WORK_INSTANCE以及TB_FLOW_WORKFLOW_DETAIL表中新增一条记录，返回成功新增记录数
        /// </summary>
        /// <returns>int</returns>
        public int createFlowInstance(Entity_TB_FLOW_WORK_INSTANCE entityInstance, Entity_TB_FLOW_POST_DEFINE entityPostCode, UserInfo entityUser)
        {
            int iCount = 0;
            IDatabaseDAO dao = DALFactory.CreateSqlServerDAO();
            try
            {
                //事务开始
                dao.BeginTransaction();

                String strSWORKFLOWCODE = entityInstance.SWORKFLOWCODE;
                String strSFLOWCODE = entityInstance.SFLOWCODE;
                String strSWORKFLOWNAME = entityInstance.SWORKFLOWNAME;
                String strSWORKFLOWNAMECN = entityInstance.SWORKFLOWNAMECN;
                String strSENTITYID = entityInstance.SENTITYID;
                String strSENTITYNAME = entityInstance.SENTITYNAME;
                String strSENTITYNAMECN = entityInstance.SENTITYNAMECN;
                String strSFLOWACCEPTNO = entityInstance.SFLOWACCEPTNO;
                DateTime dtDTSTARTDATE = DateTime.Now;
                String strSUSERID = entityInstance.SUSERID;
                String strSDEPTID = entityInstance.SDEPTID;
                String strSFLOWMOVECODE = entityInstance.SFLOWMOVECODE;
                String strSWKSCODE = entityInstance.SWKSCODE;
                Decimal nNCOUNTWORKDAY = entityInstance.NCOUNTWORKDAY;
                Decimal nNSUBNUMBER = entityInstance.NSUBNUMBER;
                String strSBIZSTATUS = entityInstance.SBIZSTATUS;
                String strBISSUBFLOW = entityInstance.BISSUBFLOW;
                String strSPARENTCODE = entityInstance.SPARENTCODE;
                String strSPENDINGUSERID = entityInstance.SPENDINGUSERID;

                //首先新增TB_FLOW_WORK_INSTANCE一条记录
                DbParameter[] param1 = dao.MakeParameter(FlowSqlConfig.Instance.GetSqlForTB_FLOW_WORK_INSTANCE_insert());
                param1[0].Value = strSWORKFLOWCODE;
                param1[1].Value = strSFLOWCODE;
                param1[2].Value = strSWORKFLOWNAME;
                param1[3].Value = strSWORKFLOWNAMECN;
                param1[4].Value = strSENTITYID;
                param1[5].Value = strSENTITYNAME;
                param1[6].Value = strSENTITYNAMECN;
                param1[7].Value = strSFLOWACCEPTNO;
                param1[8].Value = dtDTSTARTDATE;
                param1[9].Value = strSUSERID;
                param1[10].Value = strSDEPTID;
                param1[11].Value = strSFLOWMOVECODE;
                param1[12].Value = strSWKSCODE;
                param1[13].Value = nNCOUNTWORKDAY;
                param1[14].Value = nNSUBNUMBER;
                param1[15].Value = strSBIZSTATUS;
                param1[16].Value = strBISSUBFLOW;
                param1[17].Value = strSPARENTCODE;
                param1[18].Value = strSPENDINGUSERID;
                object obj = dao.ExecuteNonQuery(FlowSqlConfig.Instance.GetSqlForTB_FLOW_WORK_INSTANCE_insert(), param1);
                if (obj != null)
                {
                    //然后新增TB_FLOW_WORKFLOW_DETAIL一条记录
                    DbParameter[] param2 = dao.MakeParameter(FlowSqlConfig.Instance.GetSqlForTB_FLOW_WORKFLOW_DETAIL_insert());
                    param2[0].Value = System.Guid.NewGuid().ToString();
                    param2[1].Value = strSWORKFLOWCODE;
                    param2[2].Value = 1;
                    param2[3].Value = entityPostCode.SPOSTCODE;
                    param2[4].Value = entityPostCode.SPOSTNAME;
                    param2[5].Value = entityPostCode.SPOSTNAMECN;
                    param2[6].Value = entityUser.SDEPT;
                    param2[7].Value = entityUser.SUSERID;
                    param2[8].Value = entityUser.SUSERNAME;
                    param2[9].Value = entityUser.SUSERNAMECN;
                    param2[10].Value = dtDTSTARTDATE;
                    param2[11].Value = null;
                    param2[12].Value = null;
                    param2[13].Value = null;
                    param2[14].Value = null;
                    param2[15].Value = null;
                    param2[16].Value = null;
                    param2[17].Value = null;
                    param2[18].Value = null;
                    param2[19].Value = null;
                    param2[20].Value = null;
                    param2[21].Value = null;
                    param2[22].Value = null;
                    param2[23].Value = "0";//是否工作超时
                    param2[24].Value = Com.ValuePlus.SysParams.FlowConfigParamGetter.GetFlowConfigParamValue("defaultBizState");//流程业务状态，控制活动是否出现
                    param2[25].Value = strSWORKFLOWNAME;
                    param2[26].Value = strSWORKFLOWNAMECN;
                    param2[27].Value = strSFLOWCODE;
                    param2[28].Value = strSENTITYID;
                    param2[29].Value = strSFLOWMOVECODE;
                    param2[30].Value = strSWKSCODE;
                    param2[31].Value = strSENTITYNAME;
                    param2[32].Value = strSENTITYNAMECN;
                    param2[33].Value = "1";//是否当前岗位
                    int iReturn = dao.ExecuteNonQuery(FlowSqlConfig.Instance.GetSqlForTB_FLOW_WORKFLOW_DETAIL_insert(), param2);
                    if (iReturn > 0)
                    {
                        iCount++;
                    }
                    else
                    {
                        throw new Exception();
                    }

                    //事务提交

                    dao.Commit();
                }
                else
                {
                    dao.RoolBack();
                }
            }
            catch (Exception ex)
            {
                dao.RoolBack();
                log.Error(ex);
                iCount = 0;
            }
            finally
            {
                dao.Dispose();
            }
            return iCount;
        }
        #endregion

        #region 根据流程实例编码更新当前流程的流转状态，返回更新记录数
        /// <summary>
        /// 根据流程实例编码更新当前流程的流转状态，返回更新记录数
        /// </summary>
        /// <param name="strFlowId"></param>
        /// <param name="strMoveState"></param>
        /// <returns>int</returns>
        public int updateFlowMoveStateByFlowId(String strFlowId,String strMoveState)
        {
            int count = 0;
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(FlowSqlConfig.Instance.GetSqlForTB_FLOW_WORK_INSTANCE_TB_FLOW_WORKFLOW_DETAIL_updateMoveStateByFlowId());
                param[0].Value = strMoveState;
                param[1].Value = strFlowId;
                object obj = dao.ExecuteNonQuery(FlowSqlConfig.Instance.GetSqlForTB_FLOW_WORK_INSTANCE_TB_FLOW_WORKFLOW_DETAIL_updateMoveStateByFlowId(), param);
                if (obj != null)
                {
                    count = Convert.ToInt32(obj); ;
                }
            }
            return count;
        }
        #endregion

        #region 根据流程实例编码更新当前流程的流程状态，返回更新记录数
        /// <summary>
        /// 根据流程实例编码更新当前流程的流程状态，返回更新记录数
        /// </summary>
        /// <param name="strFlowId"></param>
        /// <param name="strFlowState"></param>
        /// <returns>int</returns>
        public int updateFlowStateByFlowId(String strFlowId, String strFlowState)
        {
            int count = 0;
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(FlowSqlConfig.Instance.GetSqlForTB_FLOW_WORK_INSTANCE_TB_FLOW_WORKFLOW_DETAIL_updateFlowStateByFlowId());
                param[0].Value = strFlowState;
                param[1].Value = strFlowId;
                object obj = dao.ExecuteNonQuery(FlowSqlConfig.Instance.GetSqlForTB_FLOW_WORK_INSTANCE_TB_FLOW_WORKFLOW_DETAIL_updateFlowStateByFlowId(), param);
                if (obj != null)
                {
                    count = Convert.ToInt32(obj); ;
                }
            }
            return count;
        }
        #endregion

        #region 根据流程实例编码更新当前流程的业务状态，返回更新记录数
        /// <summary>
        /// 根据流程实例编码更新当前流程的业务状态，返回更新记录数
        /// </summary>
        /// <param name="strFlowId"></param>
        /// <param name="strBizState"></param>
        /// <returns>int</returns>
        public int updateFlowBizStateByFlowId(String strFlowId, String strBizState)
        {
            int count = 0; 
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(FlowSqlConfig.Instance.GetSqlForTB_FLOW_WORK_INSTANCE_TB_FLOW_WORKFLOW_DETAIL_updateBizStateByFlowId());
                param[0].Value = strBizState;
                param[1].Value = strFlowId;
                object obj = dao.ExecuteNonQuery(FlowSqlConfig.Instance.GetSqlForTB_FLOW_WORK_INSTANCE_TB_FLOW_WORKFLOW_DETAIL_updateBizStateByFlowId(), param);
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
