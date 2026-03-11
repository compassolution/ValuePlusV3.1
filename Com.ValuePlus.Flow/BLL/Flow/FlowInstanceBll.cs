using System;
using System.Text;
using Com.ValuePlus.Flow.DAL.Flow;
using System.Data;
using Com.ValuePlus.Flow.Entity;
using Com.ValuePlus.Entity;

namespace Com.ValuePlus.Flow.BLL.Flow
{
    public class FlowInstanceBll
    {
        /// <summary>
        /// 转岗后的最后一条流转记录，需新增到数据库
        /// </summary>
        /// <param name="entityInstance"></param>
        /// <param name="entityUser"></param>
        /// <returns></returns>
        public String CreateWorkFlow(Entity_TB_FLOW_WORK_INSTANCE entityInstance,UserInfo entityUser)
        {
            if (entityInstance != null)
            {
                String strFlowDefineCode = entityInstance.SFLOWCODE;//流程定义编码
                String strSWORKFLOWCODE = entityInstance.SWORKFLOWCODE;//流程实例编码
                //首先获取该流程的起始岗位
                FlowPostBll bllPost = new FlowPostBll();
                Entity_TB_FLOW_POST_DEFINE entityPostCode = (Entity_TB_FLOW_POST_DEFINE)bllPost.GetStartPostEntityByFlowCode(strFlowDefineCode);

                //然后写入表TB_FLOW_WORK_INSTANCE一条数据,同时写入表TB_FLOW_WORKFLOW_DETAIL一条起始记录
                FlowInstanceDao dao = new FlowInstanceDao();
                int iCount = dao.createFlowInstance(entityInstance, entityPostCode, entityUser);
                if (iCount > 0)
                {
                    return strSWORKFLOWCODE;
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 根据用户获取其待办事项列表
        /// </summary>
        /// <param name="strUserId"></param>
        /// <param name="iPageSize"></param>
        /// <param name="iStartIndex"></param>
        /// <param name="iRecordCount"></param>
        /// <param name="strCondition"></param>
        /// <returns></returns>
        public DataSet GetPendingFlowListByUserId(String strUserId, int iPageSize, int iPageIndex, String strCondition, ref int iRecordCount)
        {
            FlowInstanceDao dao = new FlowInstanceDao();
            int iStartIndex = iPageIndex * iPageSize;
            DataSet ds = dao.findPendingListByUserId(strUserId, iPageSize, iStartIndex, strCondition, ref iRecordCount);

            return ds;
        }

        /// <summary>
        /// 根据流程实例获取其最后一条流转信息,返回DS数据集
        /// </summary>
        /// <param name="strFlowId"></param>
        /// <returns>DataSet</returns>
        public DataSet GetLastTrasferInfoDsByFlowId(String strFlowId)
        {
            FlowDetailDao dao = new FlowDetailDao();
            DataSet ds = dao.findLastTrasferInByFlowId(strFlowId);

            return ds;
        }

        /// <summary>
        /// 根据流程实例获取其最后一条流转信息,返回Entity_TB_FLOW_WORKFLOW_DETAIL实体
        /// </summary>
        /// <param name="strFlowId"></param>
        /// <returns>Entity_TB_FLOW_WORKFLOW_DETAIL</returns>
        public Entity_TB_FLOW_WORKFLOW_DETAIL GetLastTrasferInfoEntityByFlowId(String strFlowId)
        {
            FlowDetailDao dao = new FlowDetailDao();
            DataSet ds = dao.findLastTrasferInByFlowId(strFlowId);
            return this.FillDsToEntity_TB_FLOW_WORKFLOW_DETAIL(ds);
        }
        
        /// <summary>
        /// 根据流程实例ID获取表TB_FLOW_WORKFLOW_DETAIL倒数第二条（即上一条）流转信息,返回DS数据集
        /// </summary>
        /// <param name="strFlowId"></param>
        /// <returns>DataSet</returns>
        public DataSet GetPreviousTrasferInfoDsByFlowId(String strFlowId)
        {
            FlowDetailDao dao = new FlowDetailDao();
            DataSet ds = dao.findPreviousTrasferInByFlowId(strFlowId);

            return ds;
        }

        /// <summary>
        /// 根据流程实例ID获取表TB_FLOW_WORKFLOW_DETAIL倒数第二条（即上一条）流转信息,返回Entity_TB_FLOW_WORKFLOW_DETAIL实体
        /// </summary>
        /// <param name="strFlowId"></param>
        /// <returns>Entity_TB_FLOW_WORKFLOW_DETAIL</returns>
        public Entity_TB_FLOW_WORKFLOW_DETAIL GetPreviousTrasferInfoEntityByFlowId(String strFlowId)
        {
            DataSet ds = GetPreviousTrasferInfoDsByFlowId(strFlowId);
            return this.FillDsToEntity_TB_FLOW_WORKFLOW_DETAIL(ds);
        }

        /// <summary>
        /// 根据数据集填充到实体Entity_TB_FLOW_WORKFLOW_DETAIL中
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        private Entity_TB_FLOW_WORKFLOW_DETAIL FillDsToEntity_TB_FLOW_WORKFLOW_DETAIL(DataSet ds)
        {
            Entity_TB_FLOW_WORKFLOW_DETAIL entity = new Entity_TB_FLOW_WORKFLOW_DETAIL(); 
            if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
            {
                entity.SWFDCODE = ds.Tables[0].Rows[0]["SWFDCODE"].ToString();
                entity.SWORKFLOWCODE = ds.Tables[0].Rows[0]["SWORKFLOWCODE"].ToString();
                if (ds.Tables[0].Rows[0]["NNUMBER"] != DBNull.Value)
                {
                    entity.NNUMBER = System.Convert.ToDecimal(ds.Tables[0].Rows[0]["NNUMBER"]);
                }
                entity.SSOURPOSTCODE = ds.Tables[0].Rows[0]["SSOURPOSTCODE"].ToString();
                entity.SSOURPOSTNAME = ds.Tables[0].Rows[0]["SSOURPOSTNAME"].ToString();
                entity.SSOURPOSTNAMECN = ds.Tables[0].Rows[0]["SSOURPOSTNAMECN"].ToString();
                entity.SSOURDEPTID = ds.Tables[0].Rows[0]["SSOURDEPTID"].ToString();
                entity.SSOURUSERID = ds.Tables[0].Rows[0]["SSOURUSERID"].ToString();
                entity.SSOURUSERNAME = ds.Tables[0].Rows[0]["SSOURUSERNAME"].ToString();
                entity.SSOURUSERNAMECN = ds.Tables[0].Rows[0]["SSOURUSERNAMECN"].ToString();
                if (ds.Tables[0].Rows[0]["DTSOURDATE"] != DBNull.Value)
                {
                    entity.DTSOURDATE = System.Convert.ToDateTime(ds.Tables[0].Rows[0]["DTSOURDATE"]);
                }
                entity.SDESTPOSTCODE = ds.Tables[0].Rows[0]["SDESTPOSTCODE"].ToString();
                entity.SDESTPOSTNAME = ds.Tables[0].Rows[0]["SDESTPOSTNAME"].ToString();
                entity.SDESTPOSTNAMECN = ds.Tables[0].Rows[0]["SDESTPOSTNAMECN"].ToString();
                entity.SDESTDEPTID = ds.Tables[0].Rows[0]["SDESTDEPTID"].ToString();
                entity.SDESTUSERID = ds.Tables[0].Rows[0]["SDESTUSERID"].ToString();
                entity.SDESTUSERNAME = ds.Tables[0].Rows[0]["SDESTUSERNAME"].ToString();
                entity.SDESTUSERNAMECN = ds.Tables[0].Rows[0]["SDESTUSERNAMECN"].ToString();
                if (ds.Tables[0].Rows[0]["DTDESTDATE"] != DBNull.Value)
                {
                    entity.DTDESTDATE = System.Convert.ToDateTime(ds.Tables[0].Rows[0]["DTDESTDATE"]);
                }
                entity.SMEMO = ds.Tables[0].Rows[0]["SMEMO"].ToString();
                entity.SFLOWPATHCODE = ds.Tables[0].Rows[0]["SFLOWPATHCODE"].ToString();
                if (ds.Tables[0].Rows[0]["NMOVETOTALDAY"] != DBNull.Value)
                {
                    entity.NMOVETOTALDAY = System.Convert.ToDecimal(ds.Tables[0].Rows[0]["NMOVETOTALDAY"]);
                }
                if (ds.Tables[0].Rows[0]["NWORKPROCESSDAY"] != DBNull.Value)
                {
                    entity.NWORKPROCESSDAY = System.Convert.ToDecimal(ds.Tables[0].Rows[0]["NWORKPROCESSDAY"]);
                }
                entity.BPROMPT = ds.Tables[0].Rows[0]["BPROMPT"].ToString();
                entity.SBIZSTATUS = ds.Tables[0].Rows[0]["SBIZSTATUS"].ToString();
                entity.SWORKFLOWNAME = ds.Tables[0].Rows[0]["SWORKFLOWNAME"].ToString();
                entity.SWORKFLOWNAMECN = ds.Tables[0].Rows[0]["SWORKFLOWNAMECN"].ToString();
                entity.SFLOWCODE = ds.Tables[0].Rows[0]["SFLOWCODE"].ToString();
                entity.SENTITYID = ds.Tables[0].Rows[0]["SENTITYID"].ToString();
                entity.SFLOWMOVECODE = ds.Tables[0].Rows[0]["SFLOWMOVECODE"].ToString();
                entity.SWKSCODE = ds.Tables[0].Rows[0]["SWKSCODE"].ToString();
                entity.SENTITYNAME = ds.Tables[0].Rows[0]["SENTITYNAME"].ToString();
                entity.SENTITYNAMECN = ds.Tables[0].Rows[0]["SENTITYNAMECN"].ToString();
                entity.BISCURSTEP = ds.Tables[0].Rows[0]["BISCURSTEP"].ToString();

            }
            return entity;
        }

        /// <summary>
        /// 根据流程实例ID获取一条TB_FLOW_WORK_INSTANCE,返回DS数据集
        /// </summary>
        /// <param name="strFlowId"></param>
        /// <returns></returns>
        public DataSet GetFlowInstanceInfoDsByFlowId(String strFlowId)
        {
            FlowInstanceDao dao = new FlowInstanceDao();
            DataSet ds = dao.findByKey(strFlowId);

            return ds;
        }

        /// <summary>
        /// 根据流程实例ID获取一条TB_FLOW_WORK_INSTANCE,返回Entity_TB_FLOW_WORK_INSTANCE实体
        /// </summary>
        /// <param name="strFlowId"></param>
        /// <returns></returns>
        public Entity_TB_FLOW_WORK_INSTANCE GetFlowInstanceInfoEntityByFlowId(String strFlowId)
        {
            DataSet ds = GetFlowInstanceInfoDsByFlowId(strFlowId);
            return this.FillDsToEntity_TB_FLOW_WORK_INSTANCE(ds);
        }

        /// <summary>
        /// 根据数据集填充到实体Entity_TB_FLOW_WORK_INSTANCE中
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        private Entity_TB_FLOW_WORK_INSTANCE FillDsToEntity_TB_FLOW_WORK_INSTANCE(DataSet ds)
        {
            Entity_TB_FLOW_WORK_INSTANCE entityInstance = new Entity_TB_FLOW_WORK_INSTANCE();
            if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
            {
                entityInstance.SWORKFLOWCODE = ds.Tables[0].Rows[0]["SWORKFLOWCODE"].ToString();
                entityInstance.SFLOWCODE = ds.Tables[0].Rows[0]["SFLOWCODE"].ToString();
                entityInstance.SWORKFLOWNAME = ds.Tables[0].Rows[0]["SWORKFLOWNAME"].ToString();
                entityInstance.SWORKFLOWNAMECN = ds.Tables[0].Rows[0]["SWORKFLOWNAMECN"].ToString();
                entityInstance.SENTITYID = ds.Tables[0].Rows[0]["SENTITYID"].ToString();
                entityInstance.SENTITYNAME = ds.Tables[0].Rows[0]["SENTITYNAME"].ToString();
                entityInstance.SENTITYNAMECN = ds.Tables[0].Rows[0]["SENTITYNAMECN"].ToString();
                entityInstance.SFLOWACCEPTNO = ds.Tables[0].Rows[0]["SFLOWACCEPTNO"].ToString();
                if (ds.Tables[0].Rows[0]["DTSTARTDATE"] != DBNull.Value)
                {
                    entityInstance.DTSTARTDATE = System.Convert.ToDateTime(ds.Tables[0].Rows[0]["DTSTARTDATE"]);
                }
                entityInstance.SUSERID = ds.Tables[0].Rows[0]["SUSERID"].ToString();
                entityInstance.SDEPTID = ds.Tables[0].Rows[0]["SDEPTID"].ToString();
                entityInstance.SFLOWMOVECODE = ds.Tables[0].Rows[0]["SFLOWMOVECODE"].ToString();
                entityInstance.SWKSCODE = ds.Tables[0].Rows[0]["SWKSCODE"].ToString();
                if (ds.Tables[0].Rows[0]["NCOUNTWORKDAY"] != DBNull.Value)
                {
                    entityInstance.NCOUNTWORKDAY = System.Convert.ToDecimal(ds.Tables[0].Rows[0]["NCOUNTWORKDAY"]);
                }
                if (ds.Tables[0].Rows[0]["NSUBNUMBER"] != DBNull.Value)
                {
                    entityInstance.NSUBNUMBER = System.Convert.ToDecimal(ds.Tables[0].Rows[0]["NSUBNUMBER"]);
                }
                entityInstance.SBIZSTATUS = ds.Tables[0].Rows[0]["SBIZSTATUS"].ToString();
                entityInstance.BISSUBFLOW = ds.Tables[0].Rows[0]["BISSUBFLOW"].ToString();
                entityInstance.SPARENTCODE = ds.Tables[0].Rows[0]["SPARENTCODE"].ToString();
                entityInstance.SPENDINGUSERID = ds.Tables[0].Rows[0]["SPENDINGUSERID"].ToString();
            }
            return entityInstance;
        }

        /// <summary>
        /// 根据流程实例ID获取当前流程的业务状态,返回字符串
        /// </summary>
        /// <param name="strFlowId"></param>
        /// <returns></returns>
        public String GetFlowBizStateByFlowId(String strFlowId)
        {
            DataSet ds = GetFlowInstanceInfoDsByFlowId(strFlowId);
            String strCurBizState = "0";
            if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
            {
                strCurBizState = ds.Tables[0].Rows[0]["SBIZSTATUS"].ToString();
            }

            return strCurBizState;
        }

        /// <summary>
        /// 根据流程实例ID获取当前流程的业务状态,返回字符串
        /// </summary>
        /// <param name="strFlowId"></param>
        /// <param name="strLanguage"></param>
        /// <returns></returns>
        public String GetFlowNameByFlowId(String strFlowId,String strLanguage)
        {
            DataSet ds = GetFlowInstanceInfoDsByFlowId(strFlowId);
            String strFlowName = strFlowId;
            if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
            {
                if (strLanguage.Equals("en-us"))
                {
                    strFlowName = ds.Tables[0].Rows[0]["SWORKFLOWNAME"].ToString();
                }
                else
                {
                    strFlowName = ds.Tables[0].Rows[0]["SWORKFLOWNAMECN"].ToString();
                }
                  
            }

            return strFlowName;
        }

        /// <summary>
        /// 根据流程实例ID获取表TB_FLOW_DEFINE的一条记录,返回DS数据集
        /// </summary>
        /// <param name="strFlowId"></param>
        /// <returns></returns>
        public DataSet GetFlowDefineInfoDsByFlowId(String strFlowId)
        {
            FlowDefineDao dao = new FlowDefineDao();
            DataSet ds = dao.findDefineByWorkFlowCode(strFlowId);

            return ds;
        }

        /// <summary>
        /// 根据流程实例ID获取表TB_FLOW_DEFINE的一条记录,返回Entity_TB_FLOW_DEFINE实体
        /// </summary>
        /// <param name="strFlowId"></param>
        /// <returns></returns>
        public Entity_TB_FLOW_DEFINE GetFlowDefineInfoEntityByFlowId(String strFlowId)
        {
            DataSet ds = GetFlowDefineInfoDsByFlowId(strFlowId);
            Entity_TB_FLOW_DEFINE entityDefine = this.FillDsToEntity_TB_FLOW_DEFINE(ds);
            return entityDefine;

        }

        /// <summary>
        /// 根据数据集填充到实体Entity_TB_FLOW_DEFINE中
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        private Entity_TB_FLOW_DEFINE FillDsToEntity_TB_FLOW_DEFINE(DataSet ds)
        {
            Entity_TB_FLOW_DEFINE entityDefine = new Entity_TB_FLOW_DEFINE();
            if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
            {
                entityDefine.SFLOWCODE = ds.Tables[0].Rows[0]["SFLOWCODE"].ToString();
                entityDefine.SFLOWNAME = ds.Tables[0].Rows[0]["SFLOWNAME"].ToString();
                entityDefine.SFLOWNAMECN = ds.Tables[0].Rows[0]["SFLOWNAMECN"].ToString();
                entityDefine.SFLOWDESC = ds.Tables[0].Rows[0]["SFLOWDESC"].ToString();
                entityDefine.SFLOWDESCCN = ds.Tables[0].Rows[0]["SFLOWDESCCN"].ToString();
                entityDefine.BISNEEDACCEPT = ds.Tables[0].Rows[0]["BISNEEDACCEPT"].ToString();
                entityDefine.BISACTIVEWITHSUB = ds.Tables[0].Rows[0]["BISACTIVEWITHSUB"].ToString();
                if (ds.Tables[0].Rows[0]["NWORKCOUNTDAY"] != DBNull.Value)
                {
                    entityDefine.NWORKCOUNTDAY = System.Convert.ToDecimal(ds.Tables[0].Rows[0]["NWORKCOUNTDAY"]);
                }
                entityDefine.BSTOP = ds.Tables[0].Rows[0]["BSTOP"].ToString();
            }
            return entityDefine;
        }

        /// <summary>
        /// 根据流程实例编码更新当前流程的流转状态，返回更新记录数
        /// </summary>
        /// <param name="strFlowId"></param>
        /// <param name="strMoveStateCode"></param>
        /// <returns></returns>
        public int SetFlowMoveStateByFlowId(String strFlowId, String strMoveStateCode)
        {
            int iCount = 0;
            FlowInstanceDao dao = new FlowInstanceDao(); 
            iCount = dao.updateFlowMoveStateByFlowId(strFlowId, strMoveStateCode);

            return iCount;
        }

        /// <summary>
        /// 根据流程实例编码更新当前流程的流程状态，返回更新记录数
        /// </summary>
        /// <param name="strFlowId"></param>
        /// <param name="strFlowStateCode"></param>
        /// <returns></returns>
        public int SetFlowStateByFlowId(String strFlowId, String strFlowStateCode)
        {
            int iCount = 0;
            FlowInstanceDao dao = new FlowInstanceDao();
            iCount = dao.updateFlowStateByFlowId(strFlowId, strFlowStateCode);

            return iCount;
        }

        /// <summary>
        /// 根据流程实例编码更新当前流程的业务状态，返回更新记录数
        /// </summary>
        /// <param name="strFlowId"></param>
        /// <param name="strBizStateCode"></param>
        /// <returns></returns>
        public int SetFlowBizStateByFlowId(String strFlowId, String strBizStateCode)
        {
            int iCount = 0;
            FlowInstanceDao dao = new FlowInstanceDao();
            iCount = dao.updateFlowBizStateByFlowId(strFlowId, strBizStateCode);

            return iCount;
        }

    }
}
