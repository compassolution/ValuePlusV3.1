using System;
using System.Text;
using Com.ValuePlus.Database;
using Com.ValuePlus.Flow.Config;
using System.Data;
using System.Data.Common;
using Com.ValuePlus.Entity;
using Com.ValuePlus.Flow.Entity;

namespace Com.ValuePlus.Flow.DAL.Flow
{
    public class FlowDetailDao
    {
        /// <summary>
        /// 日志声明
        /// </summary>
        protected static Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region 查询表TB_FLOW_WORKFLOW_DETAIL所有记录，返回dateset记录集
        /// <summary>
        /// 查询表TB_FLOW_WORKFLOW_DETAIL所有记录，返回dateset记录集
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet findAll()
        {
            DataSet ds = new DataSet();
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                ds = dao.ExecuteDataSet(FlowSqlConfig.Instance.GetSqlForTB_FLOW_WORKFLOW_DETAIL_selectAll(), null);
            }
            return ds;
        }
        #endregion

        #region 根据主键查询TB_FLOW_WORKFLOW_DETAIL的相应记录，返回dateset记录集
        /// <summary>
        /// 根据主键查询TB_FLOW_WORKFLOW_DETAIL的相应记录，返回dateset记录集
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet findByKey(String strKeyValue)
        {
            DataSet ds = new DataSet();
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(FlowSqlConfig.Instance.GetSqlForTB_FLOW_WORKFLOW_DETAIL_selectByKey());
                param[0].Value = strKeyValue;
                ds = dao.ExecuteDataSet(FlowSqlConfig.Instance.GetSqlForTB_FLOW_WORKFLOW_DETAIL_selectByKey(), param);
            }
            return ds;
        }
        #endregion

        #region 新增一条记录到表TB_FLOW_WORKFLOW_DETAIL中，返回成功新增记录数
        /// <summary>
        /// 新增一条记录到表TB_FLOW_WORKFLOW_DETAIL中，返回成功新增记录数
        /// </summary>
        /// <returns>DataSet</returns>
        public int insertOneRow(string strSWFDCODE, string strSWORKFLOWCODE, decimal nNNUMBER, string strSSOURPOSTCODE, string strSSOURPOSTNAME, string strSSOURPOSTNAMECN, string strSSOURDEPTID, string strSSOURUSERID, string strSSOURUSERNAME, string strSSOURUSERNAMECN, DateTime dtDTSOURDATE, string strSDESTPOSTCODE, string strSDESTPOSTNAME, string strSDESTPOSTNAMECN, string strSDESTDEPTID, string strSDESTUSERID, string strSDESTUSERNAME, string strSDESTUSERNAMECN, DateTime dtDTDESTDATE, string strSMEMO, string strSFLOWPATHCODE, decimal nNMOVETOTALDAY, decimal nNWORKPROCESSDAY, string strBPROMPT, string strSBIZSTATUS, string strSWORKFLOWNAME, string strSWORKFLOWNAMECN, string strSFLOWCODE, string strSENTITYID, string strSFLOWMOVECODE, string strSWKSCODE, string strSENTITYNAME, string strSENTITYNAMECN, string strBISCURSTEP)
        {
            int count = 0;
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(FlowSqlConfig.Instance.GetSqlForTB_FLOW_WORKFLOW_DETAIL_insert());
                param[0].Value = strSWFDCODE;
                param[1].Value = strSWORKFLOWCODE;
                param[2].Value = nNNUMBER;
                param[3].Value = strSSOURPOSTCODE;
                param[4].Value = strSSOURPOSTNAME;
                param[5].Value = strSSOURPOSTNAMECN;
                param[6].Value = strSSOURDEPTID;
                param[7].Value = strSSOURUSERID;
                param[8].Value = strSSOURUSERNAME;
                param[9].Value = strSSOURUSERNAMECN;
                param[10].Value = dtDTSOURDATE;
                param[11].Value = strSDESTPOSTCODE;
                param[12].Value = strSDESTPOSTNAME;
                param[13].Value = strSDESTPOSTNAMECN;
                param[14].Value = strSDESTDEPTID;
                param[15].Value = strSDESTUSERID;
                param[16].Value = strSDESTUSERNAME;
                param[17].Value = strSDESTUSERNAMECN;
                param[18].Value = dtDTDESTDATE;
                param[19].Value = strSMEMO;
                param[20].Value = strSFLOWPATHCODE;
                param[21].Value = nNMOVETOTALDAY;
                param[22].Value = nNWORKPROCESSDAY;
                param[23].Value = strBPROMPT;
                param[24].Value = strSBIZSTATUS;
                param[25].Value = strSWORKFLOWNAME;
                param[26].Value = strSWORKFLOWNAMECN;
                param[27].Value = strSFLOWCODE;
                param[28].Value = strSENTITYID;
                param[29].Value = strSFLOWMOVECODE;
                param[30].Value = strSWKSCODE;
                param[31].Value = strSENTITYNAME;
                param[32].Value = strSENTITYNAMECN;
                param[33].Value = strBISCURSTEP;
                object obj = dao.ExecuteScalar(FlowSqlConfig.Instance.GetSqlForTB_FLOW_WORKFLOW_DETAIL_insert(), param);
                if (obj != null)
                {
                    count = Convert.ToInt32(obj); ;
                }
            }
            return count;
        }
        #endregion

        #region 根据主键更新表TB_FLOW_WORKFLOW_DETAIL一条记录，返回成功更新记录数
        /// <summary>
        /// 根据主键更新表TB_FLOW_WORKFLOW_DETAIL一条记录，返回成功更新记录数
        /// </summary>
        /// <returns>DataSet</returns>
        public int updateByKey(string strSWFDCODE, string strSWORKFLOWCODE, decimal nNNUMBER, string strSSOURPOSTCODE, string strSSOURPOSTNAME, string strSSOURPOSTNAMECN, string strSSOURDEPTID, string strSSOURUSERID, string strSSOURUSERNAME, string strSSOURUSERNAMECN, DateTime dtDTSOURDATE, string strSDESTPOSTCODE, string strSDESTPOSTNAME, string strSDESTPOSTNAMECN, string strSDESTDEPTID, string strSDESTUSERID, string strSDESTUSERNAME, string strSDESTUSERNAMECN, DateTime dtDTDESTDATE, string strSMEMO, string strSFLOWPATHCODE, decimal nNMOVETOTALDAY, decimal nNWORKPROCESSDAY, string strBPROMPT, string strSBIZSTATUS, string strSWORKFLOWNAME, string strSWORKFLOWNAMECN, string strSFLOWCODE, string strSENTITYID, string strSFLOWMOVECODE, string strSWKSCODE, string strSENTITYNAME, string strSENTITYNAMECN, string strBISCURSTEP)
        {
            int count = 0;
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(FlowSqlConfig.Instance.GetSqlForTB_FLOW_WORKFLOW_DETAIL_updateByKey());
                param[0].Value = strSWFDCODE;
                param[1].Value = strSWORKFLOWCODE;
                param[2].Value = nNNUMBER;
                param[3].Value = strSSOURPOSTCODE;
                param[4].Value = strSSOURPOSTNAME;
                param[5].Value = strSSOURPOSTNAMECN;
                param[6].Value = strSSOURDEPTID;
                param[7].Value = strSSOURUSERID;
                param[8].Value = strSSOURUSERNAME;
                param[9].Value = strSSOURUSERNAMECN;
                param[10].Value = dtDTSOURDATE;
                param[11].Value = strSDESTPOSTCODE;
                param[12].Value = strSDESTPOSTNAME;
                param[13].Value = strSDESTPOSTNAMECN;
                param[14].Value = strSDESTDEPTID;
                param[15].Value = strSDESTUSERID;
                param[16].Value = strSDESTUSERNAME;
                param[17].Value = strSDESTUSERNAMECN;
                param[18].Value = dtDTDESTDATE;
                param[19].Value = strSMEMO;
                param[20].Value = strSFLOWPATHCODE;
                param[21].Value = nNMOVETOTALDAY;
                param[22].Value = nNWORKPROCESSDAY;
                param[23].Value = strBPROMPT;
                param[24].Value = strSBIZSTATUS;
                param[25].Value = strSWORKFLOWNAME;
                param[26].Value = strSWORKFLOWNAMECN;
                param[27].Value = strSFLOWCODE;
                param[28].Value = strSENTITYID;
                param[29].Value = strSFLOWMOVECODE;
                param[30].Value = strSWKSCODE;
                param[31].Value = strSENTITYNAME;
                param[32].Value = strSENTITYNAMECN;
                param[33].Value = strBISCURSTEP;
                object obj = dao.ExecuteNonQuery(FlowSqlConfig.Instance.GetSqlForTB_FLOW_WORKFLOW_DETAIL_updateByKey(), param);
                if (obj != null)
                {
                    count = Convert.ToInt32(obj); ;
                }
            }
            return count;
        }
        #endregion

        #region 根据主键删除表TB_FLOW_WORKFLOW_DETAIL的相应记录，返回成功删除记录数
        /// <summary>
        /// 根据主键删除表TB_FLOW_WORKFLOW_DETAIL的相应记录，返回成功删除记录数
        /// </summary>
        /// <returns>DataSet</returns>
        public int deleteByKey(String strKeyValue)
        {
            int count = 0;
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(FlowSqlConfig.Instance.GetSqlForTB_FLOW_WORKFLOW_DETAIL_deleteByKey());
                param[0].Value = strKeyValue;
                object obj = dao.ExecuteNonQuery(FlowSqlConfig.Instance.GetSqlForTB_FLOW_WORKFLOW_DETAIL_deleteByKey(), param);
                if (obj != null)
                {
                    count = Convert.ToInt32(obj); ;
                }
            }
            return count;
        }
        #endregion

        #region 根据流程实例ID获取表TB_FLOW_WORKFLOW_DETAIL最后一条流转信息，返回dateset记录集
        /// <summary>
        /// 根据流程实例ID获取表TB_FLOW_WORKFLOW_DETAIL最后一条流转信息，返回dateset记录集
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet findLastTrasferInByFlowId(String strFlowId)
        {
            DataSet ds = new DataSet();
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(FlowSqlConfig.Instance.GetSqlForTB_FLOW_WORKFLOW_DETAIL_selectLastTrasferByFlowId());
                param[0].Value = strFlowId;
                ds = dao.ExecuteDataSet(FlowSqlConfig.Instance.GetSqlForTB_FLOW_WORKFLOW_DETAIL_selectLastTrasferByFlowId(), param);
            }
            return ds;
        }
        #endregion

        #region 根据流程实例ID获取表TB_FLOW_WORKFLOW_DETAIL倒数第二条（即上一条）流转信息，返回dateset记录集
        /// <summary>
        /// 根据流程实例ID获取表TB_FLOW_WORKFLOW_DETAIL倒数第二条（即上一条）流转信息，返回dateset记录集
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet findPreviousTrasferInByFlowId(String strFlowId)
        {
            DataSet ds = new DataSet();
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO()) 
            {
                DbParameter[] param = dao.MakeParameter(FlowSqlConfig.Instance.GetSqlForTB_FLOW_WORKFLOW_DETAIL_selectPreviousTrasferByFlowId());
                param[0].Value = strFlowId;
                ds = dao.ExecuteDataSet(FlowSqlConfig.Instance.GetSqlForTB_FLOW_WORKFLOW_DETAIL_selectPreviousTrasferByFlowId(), param);
            }
            return ds;
        }
        #endregion

        #region 实现流程转岗，返回成功操作记录数
        /// <summary>
        /// 实现流程转岗，返回成功操作记录数
        /// </summary>
        /// <param name="entityInstance">流程实例实体，需更新到数据库</param>
        /// <param name="entityDetail_Add">转岗后的最后一条流转记录，需新增到数据库</param>
        /// <param name="entityDetail_Update">转岗前的最后一条流转记录，需更新到数据库</param>
        /// <returns>int</returns>
        public int transferFlowPost(Entity_TB_FLOW_WORK_INSTANCE entityInstance,Entity_TB_FLOW_WORKFLOW_DETAIL entityDetail_Add, Entity_TB_FLOW_WORKFLOW_DETAIL entityDetail_Update)
        {
            int iCount = 0;
            IDatabaseDAO dao = DALFactory.CreateSqlServerDAO();
            try
            {
                //事务开始
                dao.BeginTransaction();

                //首先更新TB_FLOW_WORK_INSTANCE的相应记录
                DbParameter[] param_Instance = dao.MakeParameter(FlowSqlConfig.Instance.GetSqlForTB_FLOW_WORK_INSTANCE_updateByKey()); 
                param_Instance[0].Value = entityInstance.SWORKFLOWCODE;
                param_Instance[1].Value = entityInstance.SFLOWCODE;
                param_Instance[2].Value = entityInstance.SWORKFLOWNAME;
                param_Instance[3].Value = entityInstance.SWORKFLOWNAMECN;
                param_Instance[4].Value = entityInstance.SENTITYID;
                param_Instance[5].Value = entityInstance.SENTITYNAME;
                param_Instance[6].Value = entityInstance.SENTITYNAMECN;
                param_Instance[7].Value = entityInstance.SFLOWACCEPTNO;
                param_Instance[8].Value = entityInstance.DTSTARTDATE;
                param_Instance[9].Value = entityInstance.SUSERID;
                param_Instance[10].Value = entityInstance.SDEPTID;
                param_Instance[11].Value = entityInstance.SFLOWMOVECODE;
                param_Instance[12].Value = entityInstance.SWKSCODE;
                param_Instance[13].Value = entityInstance.NCOUNTWORKDAY;
                param_Instance[14].Value = entityInstance.NSUBNUMBER;
                param_Instance[15].Value = entityInstance.SBIZSTATUS;
                param_Instance[16].Value = entityInstance.BISSUBFLOW;
                param_Instance[17].Value = entityInstance.SPARENTCODE;
                param_Instance[18].Value = entityInstance.SPENDINGUSERID;
                object obj = dao.ExecuteNonQuery(FlowSqlConfig.Instance.GetSqlForTB_FLOW_WORK_INSTANCE_updateByKey(), param_Instance);
                if (obj != null)
                {
                    //然后新增TB_FLOW_WORKFLOW_DETAIL一条记录，成为转岗后的最后一条流转记录
                    DbParameter[] param_Add = dao.MakeParameter(FlowSqlConfig.Instance.GetSqlForTB_FLOW_WORKFLOW_DETAIL_insert());
                    param_Add[0].Value = entityDetail_Add.SWFDCODE;
                    param_Add[1].Value = entityDetail_Add.SWORKFLOWCODE;
                    param_Add[2].Value = entityDetail_Add.NNUMBER;
                    param_Add[3].Value = entityDetail_Add.SSOURPOSTCODE;
                    param_Add[4].Value = entityDetail_Add.SSOURPOSTNAME;
                    param_Add[5].Value = entityDetail_Add.SSOURPOSTNAMECN;
                    param_Add[6].Value = entityDetail_Add.SSOURDEPTID;
                    param_Add[7].Value = entityDetail_Add.SSOURUSERID;
                    param_Add[8].Value = entityDetail_Add.SSOURUSERNAME;
                    param_Add[9].Value = entityDetail_Add.SSOURUSERNAMECN;
                    param_Add[10].Value = entityDetail_Add.DTSOURDATE;
                    param_Add[11].Value = entityDetail_Add.SDESTPOSTCODE;
                    param_Add[12].Value = entityDetail_Add.SDESTPOSTNAME;
                    param_Add[13].Value = entityDetail_Add.SDESTPOSTNAMECN;
                    param_Add[14].Value = entityDetail_Add.SDESTDEPTID;
                    param_Add[15].Value = entityDetail_Add.SDESTUSERID;
                    param_Add[16].Value = entityDetail_Add.SDESTUSERNAME;
                    param_Add[17].Value = entityDetail_Add.SDESTUSERNAMECN;
                    param_Add[18].Value = entityDetail_Add.DTDESTDATE;
                    param_Add[19].Value = entityDetail_Add.SMEMO;
                    param_Add[20].Value = entityDetail_Add.SFLOWPATHCODE;
                    param_Add[21].Value = entityDetail_Add.NMOVETOTALDAY;
                    param_Add[22].Value = entityDetail_Add.NWORKPROCESSDAY;
                    param_Add[23].Value = entityDetail_Add.BPROMPT;//是否工作超时
                    param_Add[24].Value = entityDetail_Add.SBIZSTATUS;
                    param_Add[25].Value = entityDetail_Add.SWORKFLOWNAME;
                    param_Add[26].Value = entityDetail_Add.SWORKFLOWNAMECN;
                    param_Add[27].Value = entityDetail_Add.SFLOWCODE;
                    param_Add[28].Value = entityDetail_Add.SENTITYID;
                    param_Add[29].Value = entityDetail_Add.SFLOWMOVECODE;
                    param_Add[30].Value = entityDetail_Add.SWKSCODE;
                    param_Add[31].Value = entityDetail_Add.SENTITYNAME;
                    param_Add[32].Value = entityDetail_Add.SENTITYNAMECN;
                    param_Add[33].Value = entityDetail_Add.BISCURSTEP;
                    iCount++;
                    int iReturn = dao.ExecuteNonQuery(FlowSqlConfig.Instance.GetSqlForTB_FLOW_WORKFLOW_DETAIL_insert(), param_Add);
                    if (iReturn > 0)
                    {
                        iCount++;
                        //最后更新转岗前得最后一条转岗记录
                        DbParameter[] param_Update = dao.MakeParameter(FlowSqlConfig.Instance.GetSqlForTB_FLOW_WORKFLOW_DETAIL_updateByKey());
                        param_Update[0].Value = entityDetail_Update.SWFDCODE;
                        param_Update[1].Value = entityDetail_Update.SWORKFLOWCODE;
                        param_Update[2].Value = entityDetail_Update.NNUMBER;
                        param_Update[3].Value = entityDetail_Update.SSOURPOSTCODE;
                        param_Update[4].Value = entityDetail_Update.SSOURPOSTNAME;
                        param_Update[5].Value = entityDetail_Update.SSOURPOSTNAMECN;
                        param_Update[6].Value = entityDetail_Update.SSOURDEPTID;
                        param_Update[7].Value = entityDetail_Update.SSOURUSERID;
                        param_Update[8].Value = entityDetail_Update.SSOURUSERNAME;
                        param_Update[9].Value = entityDetail_Update.SSOURUSERNAMECN;
                        param_Update[10].Value = entityDetail_Update.DTSOURDATE;
                        param_Update[11].Value = entityDetail_Update.SDESTPOSTCODE;
                        param_Update[12].Value = entityDetail_Update.SDESTPOSTNAME;
                        param_Update[13].Value = entityDetail_Update.SDESTPOSTNAMECN;
                        param_Update[14].Value = entityDetail_Update.SDESTDEPTID;
                        param_Update[15].Value = entityDetail_Update.SDESTUSERID;
                        param_Update[16].Value = entityDetail_Update.SDESTUSERNAME;
                        param_Update[17].Value = entityDetail_Update.SDESTUSERNAMECN;
                        param_Update[18].Value = entityDetail_Update.DTDESTDATE;
                        param_Update[19].Value = entityDetail_Update.SMEMO;
                        param_Update[20].Value = entityDetail_Update.SFLOWPATHCODE;
                        param_Update[21].Value = entityDetail_Update.NMOVETOTALDAY;
                        param_Update[22].Value = entityDetail_Update.NWORKPROCESSDAY;
                        param_Update[23].Value = entityDetail_Update.BPROMPT;//是否工作超时
                        param_Update[24].Value = entityDetail_Update.SBIZSTATUS;
                        param_Update[25].Value = entityDetail_Update.SWORKFLOWNAME;
                        param_Update[26].Value = entityDetail_Update.SWORKFLOWNAMECN;
                        param_Update[27].Value = entityDetail_Update.SFLOWCODE;
                        param_Update[28].Value = entityDetail_Update.SENTITYID;
                        param_Update[29].Value = entityDetail_Update.SFLOWMOVECODE;
                        param_Update[30].Value = entityDetail_Update.SWKSCODE;
                        param_Update[31].Value = entityDetail_Update.SENTITYNAME;
                        param_Update[32].Value = entityDetail_Update.SENTITYNAMECN;
                        param_Update[33].Value = entityDetail_Update.BISCURSTEP;
                        int iReturn_Update = dao.ExecuteNonQuery(FlowSqlConfig.Instance.GetSqlForTB_FLOW_WORKFLOW_DETAIL_updateByKey(), param_Update);
                        if (iReturn_Update > 0)
                        {
                            iCount++;
                        }
                        else
                        {
                            throw new Exception();
                            dao.RoolBack();
                        }
                    }
                    else
                    {
                        throw new Exception();
                        dao.RoolBack();
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

        #region 实现流程回退，返回成功操作记录数
        /// <summary>
        /// 实现流程回退，返回成功操作记录数
        /// </summary>
        /// <param name="entityInstance">流程实例实体，需更新到数据库</param>
        /// <param name="entityDetail_Delete">回退前的最后一条流转记录，需删除数据库</param>
        /// <param name="entityDetail_Update">回退前的倒数第二条流转记录，成为回退后的最后一条记录，需更新到数据库</param>
        /// <returns>int</returns>
        public int returnFlowPost(Entity_TB_FLOW_WORK_INSTANCE entityInstance, Entity_TB_FLOW_WORKFLOW_DETAIL entityDetail_Delete, Entity_TB_FLOW_WORKFLOW_DETAIL entityDetail_Update)
        {
            int iCount = 0;
            IDatabaseDAO dao = DALFactory.CreateSqlServerDAO();
            try
            {
                //事务开始
                dao.BeginTransaction();

                //首先更新TB_FLOW_WORK_INSTANCE的相应记录
                DbParameter[] param_Instance = dao.MakeParameter(FlowSqlConfig.Instance.GetSqlForTB_FLOW_WORK_INSTANCE_updateByKey());
                param_Instance[0].Value = entityInstance.SWORKFLOWCODE;
                param_Instance[1].Value = entityInstance.SFLOWCODE;
                param_Instance[2].Value = entityInstance.SWORKFLOWNAME;
                param_Instance[3].Value = entityInstance.SWORKFLOWNAMECN;
                param_Instance[4].Value = entityInstance.SENTITYID;
                param_Instance[5].Value = entityInstance.SENTITYNAME;
                param_Instance[6].Value = entityInstance.SENTITYNAMECN;
                param_Instance[7].Value = entityInstance.SFLOWACCEPTNO;
                param_Instance[8].Value = entityInstance.DTSTARTDATE;
                param_Instance[9].Value = entityInstance.SUSERID;
                param_Instance[10].Value = entityInstance.SDEPTID;
                param_Instance[11].Value = entityInstance.SFLOWMOVECODE;
                param_Instance[12].Value = entityInstance.SWKSCODE;
                param_Instance[13].Value = entityInstance.NCOUNTWORKDAY;
                param_Instance[14].Value = entityInstance.NSUBNUMBER;
                param_Instance[15].Value = entityInstance.SBIZSTATUS;
                param_Instance[16].Value = entityInstance.BISSUBFLOW;
                param_Instance[17].Value = entityInstance.SPARENTCODE;
                param_Instance[18].Value = entityInstance.SPENDINGUSERID;
                object obj = dao.ExecuteNonQuery(FlowSqlConfig.Instance.GetSqlForTB_FLOW_WORK_INSTANCE_updateByKey(), param_Instance);
                if (obj != null)
                {
                    //回退前的最后一条流转记录，需删除数据库
                    DbParameter[] param_Del = dao.MakeParameter(FlowSqlConfig.Instance.GetSqlForTB_FLOW_WORKFLOW_DETAIL_deleteByKey());
                    param_Del[0].Value = entityDetail_Delete.SWFDCODE;
                    iCount++;
                    int iReturn = dao.ExecuteNonQuery(FlowSqlConfig.Instance.GetSqlForTB_FLOW_WORKFLOW_DETAIL_deleteByKey(), param_Del);
                    if (iReturn > 0)
                    {
                        iCount++;
                        //最后更新回退前的倒数第二条流转记录，成为回退后的最后一条记录
                        DbParameter[] param_Update = dao.MakeParameter(FlowSqlConfig.Instance.GetSqlForTB_FLOW_WORKFLOW_DETAIL_updateByKey());
                        param_Update[0].Value = entityDetail_Update.SWFDCODE;
                        param_Update[1].Value = entityDetail_Update.SWORKFLOWCODE;
                        param_Update[2].Value = entityDetail_Update.NNUMBER;
                        param_Update[3].Value = entityDetail_Update.SSOURPOSTCODE;
                        param_Update[4].Value = entityDetail_Update.SSOURPOSTNAME;
                        param_Update[5].Value = entityDetail_Update.SSOURPOSTNAMECN;
                        param_Update[6].Value = entityDetail_Update.SSOURDEPTID;
                        param_Update[7].Value = entityDetail_Update.SSOURUSERID;
                        param_Update[8].Value = entityDetail_Update.SSOURUSERNAME;
                        param_Update[9].Value = entityDetail_Update.SSOURUSERNAMECN;
                        param_Update[10].Value = entityDetail_Update.DTSOURDATE;
                        param_Update[11].Value = entityDetail_Update.SDESTPOSTCODE;
                        param_Update[12].Value = entityDetail_Update.SDESTPOSTNAME;
                        param_Update[13].Value = entityDetail_Update.SDESTPOSTNAMECN;
                        param_Update[14].Value = entityDetail_Update.SDESTDEPTID;
                        param_Update[15].Value = entityDetail_Update.SDESTUSERID;
                        param_Update[16].Value = entityDetail_Update.SDESTUSERNAME;
                        param_Update[17].Value = entityDetail_Update.SDESTUSERNAMECN;
                        param_Update[18].Value = entityDetail_Update.DTDESTDATE;
                        param_Update[19].Value = entityDetail_Update.SMEMO;
                        param_Update[20].Value = entityDetail_Update.SFLOWPATHCODE;
                        param_Update[21].Value = entityDetail_Update.NMOVETOTALDAY;
                        param_Update[22].Value = entityDetail_Update.NWORKPROCESSDAY;
                        param_Update[23].Value = entityDetail_Update.BPROMPT;//是否工作超时
                        param_Update[24].Value = entityDetail_Update.SBIZSTATUS;
                        param_Update[25].Value = entityDetail_Update.SWORKFLOWNAME;
                        param_Update[26].Value = entityDetail_Update.SWORKFLOWNAMECN;
                        param_Update[27].Value = entityDetail_Update.SFLOWCODE;
                        param_Update[28].Value = entityDetail_Update.SENTITYID;
                        param_Update[29].Value = entityDetail_Update.SFLOWMOVECODE;
                        param_Update[30].Value = entityDetail_Update.SWKSCODE;
                        param_Update[31].Value = entityDetail_Update.SENTITYNAME;
                        param_Update[32].Value = entityDetail_Update.SENTITYNAMECN;
                        param_Update[33].Value = entityDetail_Update.BISCURSTEP;
                        int iReturn_Update = dao.ExecuteNonQuery(FlowSqlConfig.Instance.GetSqlForTB_FLOW_WORKFLOW_DETAIL_updateByKey(), param_Update);
                        if (iReturn_Update > 0)
                        {
                            iCount++;
                        }
                        else
                        {
                            throw new Exception();
                            dao.RoolBack();
                        }
                    }
                    else
                    {
                        throw new Exception();
                        dao.RoolBack();
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


        #region 根据流程实例ID获取该流程实例所有流转信息，返回dateset记录集
        /// <summary>
        /// 根据流程实例ID获取该流程实例所有流转信息，返回dateset记录集
        /// </summary>
        /// <param name="strFlowId"></param>
        /// <returns>DataSet</returns>
        public DataSet findAllByFlowId(String strFlowId)
        {
            DataSet ds = new DataSet();
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(FlowSqlConfig.Instance.GetSqlForTB_FLOW_WORKFLOW_DETAIL_selectAllByFlowId());
                param[0].Value = strFlowId;
                ds = dao.ExecuteDataSet(FlowSqlConfig.Instance.GetSqlForTB_FLOW_WORKFLOW_DETAIL_selectAllByFlowId(), param);
            }
            return ds;
        }
        #endregion

    }
}
