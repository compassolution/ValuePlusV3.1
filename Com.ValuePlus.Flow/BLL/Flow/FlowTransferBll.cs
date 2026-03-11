using System;
using System.Text;
using Com.ValuePlus.Flow.DAL.Flow;
using System.Data;
using Com.ValuePlus.Flow.Entity;
using Com.ValuePlus.Entity;
using Com.ValuePlus.Flow.Config;

namespace Com.ValuePlus.Flow.BLL.Flow
{
    public class FlowTransferBll
    {
        /// <summary>
        /// 实现流程转岗
        /// 首先当前流程实例实体，需更新到数据库
        /// 然后将转岗前的最后一条流转记录，需更新到数据库
        /// 最后将转岗后的最后一条流转记录，新增到数据库
        /// </summary>
        /// <param name="strFlowId"></param>
        /// <param name="strPathCode"></param>
        /// <param name="strTransferIdea"></param>
        /// <param name="entityNextPost"></param>
        /// <param name="entityCurUser"></param>
        /// <param name="entityNextUser"></param>
        /// <returns></returns>
        public int TransferFlowToNextPost(String strFlowId,String strPathCode,String strTransferIdea,Entity_TB_FLOW_POST_DEFINE entityNextPost,UserInfo entityCurUser,UserInfo entityNextUser)
        {
            int iCount = 0;
            if (!String.IsNullOrEmpty(strFlowId))
            {
                //当前流程实例实体，需更新到数据库
                Entity_TB_FLOW_WORK_INSTANCE entityInstance = this.GetFlowInstanceInfoEntityByFlowId(strFlowId);
                entityInstance.SPENDINGUSERID = entityNextUser.SUSERID;//所处待办用户
                entityInstance.SFLOWMOVECODE = Com.ValuePlus.SysParams.FlowConfigParamGetter.GetFlowConfigParamValue("FlowMoveState_NotAccepted");//流程接收移交状态编码设置为“已转出未接收”
                entityInstance.SWKSCODE = Com.ValuePlus.SysParams.FlowConfigParamGetter.GetFlowConfigParamValue("FlowState_InUse");//流程状态编码设置为“流转中”

                //转岗前的最后一条流转记录，需更新到数据库
                Entity_TB_FLOW_WORKFLOW_DETAIL entityLastDetail = this.GetLastTrasferInfoEntityByFlowId(strFlowId);
                int iNumberLast = System.Convert.ToInt32(entityLastDetail.NNUMBER);

                entityLastDetail.SDESTPOSTCODE = entityNextPost.SPOSTCODE;
                entityLastDetail.SDESTPOSTNAME = entityNextPost.SPOSTNAME;
                entityLastDetail.SDESTPOSTNAMECN = entityNextPost.SPOSTNAMECN;
                entityLastDetail.SDESTDEPTID = entityNextUser.SDEPT;
                entityLastDetail.SDESTUSERID = entityNextUser.SUSERID;
                entityLastDetail.SDESTUSERNAME = entityNextUser.SUSERNAME;
                entityLastDetail.SDESTUSERNAMECN = entityNextUser.SUSERNAMECN;
                entityLastDetail.DTDESTDATE = DateTime.Now;
                entityLastDetail.SMEMO = strTransferIdea;
                entityLastDetail.SFLOWPATHCODE = strPathCode;
                entityLastDetail.SFLOWMOVECODE = Com.ValuePlus.SysParams.FlowConfigParamGetter.GetFlowConfigParamValue("FlowMoveState_HadTransferred");//流程接收移交状态编码设置为“已接收已转出”
                entityLastDetail.SWKSCODE = Com.ValuePlus.SysParams.FlowConfigParamGetter.GetFlowConfigParamValue("FlowState_InUse");
                entityLastDetail.BISCURSTEP = "0";

                //转岗后的最后一条流转记录，需新增到数据库
                Entity_TB_FLOW_WORKFLOW_DETAIL entityDetail_Add = new Entity_TB_FLOW_WORKFLOW_DETAIL();
                entityDetail_Add.SWFDCODE = System.Guid.NewGuid().ToString();
                entityDetail_Add.SWORKFLOWCODE = strFlowId;
                entityDetail_Add.NNUMBER = iNumberLast+1;
                entityDetail_Add.SSOURPOSTCODE = entityNextPost.SPOSTCODE;
                entityDetail_Add.SSOURPOSTNAME = entityNextPost.SPOSTNAME;
                entityDetail_Add.SSOURPOSTNAMECN = entityNextPost.SPOSTNAMECN;
                entityDetail_Add.SSOURDEPTID = entityNextUser.SDEPT;
                entityDetail_Add.SSOURUSERID=entityNextUser.SUSERID;
                entityDetail_Add.SSOURUSERNAME = entityNextUser.SUSERNAME;
                entityDetail_Add.SSOURUSERNAMECN = entityNextUser.SUSERNAMECN;
                entityDetail_Add.DTSOURDATE = DateTime.Now;
                entityDetail_Add.SDESTPOSTCODE = null;
                entityDetail_Add.SDESTPOSTNAME = null;
                entityDetail_Add.SDESTPOSTNAMECN = null;
                entityDetail_Add.SDESTDEPTID = null;
                entityDetail_Add.SDESTUSERID = null;
                entityDetail_Add.SDESTUSERNAME = null;
                entityDetail_Add.SDESTUSERNAMECN = null;
                entityDetail_Add.DTDESTDATE = null;
                entityDetail_Add.SMEMO = null;
                entityDetail_Add.SFLOWPATHCODE = null;
                entityDetail_Add.NMOVETOTALDAY = null;
                entityDetail_Add.NWORKPROCESSDAY = null;
                entityDetail_Add.BPROMPT = "0";//是否工作超时
                entityDetail_Add.SBIZSTATUS = Com.ValuePlus.SysParams.FlowConfigParamGetter.GetFlowConfigParamValue("defaultBizState");
                entityDetail_Add.SWORKFLOWNAME = entityLastDetail.SWORKFLOWNAME;
                entityDetail_Add.SWORKFLOWNAMECN = entityLastDetail.SWORKFLOWNAMECN;
                entityDetail_Add.SFLOWCODE = entityLastDetail.SFLOWCODE;
                entityDetail_Add.SENTITYID = entityLastDetail.SENTITYID;
                entityDetail_Add.SFLOWMOVECODE = Com.ValuePlus.SysParams.FlowConfigParamGetter.GetFlowConfigParamValue("FlowMoveState_NotAccepted");//流程接收移交状态编码设置为“已转出未接收”
                entityDetail_Add.SWKSCODE = Com.ValuePlus.SysParams.FlowConfigParamGetter.GetFlowConfigParamValue("FlowState_InUse");
                entityDetail_Add.SENTITYNAME = entityLastDetail.SENTITYNAME;
                entityDetail_Add.SENTITYNAMECN = entityLastDetail.SENTITYNAMECN;
                entityDetail_Add.BISCURSTEP = "1";

                FlowDetailDao dao = new FlowDetailDao();
                iCount = dao.transferFlowPost(entityInstance, entityDetail_Add, entityLastDetail);
                
            }
            return iCount;
        }

        /// <summary>
        /// 实现流程接收前的退回操作
        /// 首先更新当前流程实例的状态及当前待办用户信息
        /// 然后将回退前的最后一条流转记录删除
        /// 最后将回退前的倒数第二条流转记录，更新成为回退后的最后一条记录
        /// </summary>
        /// <param name="strFlowId"></param>
        /// <returns></returns>
        public int ReturnFlowToPrePost(String strFlowId)
        {
            int iCount = 0;
            if (!String.IsNullOrEmpty(strFlowId))
            {
                //回退前的最后一条流转记录，需删除数据库
                Entity_TB_FLOW_WORKFLOW_DETAIL entityDetail_Delete = this.GetLastTrasferInfoEntityByFlowId(strFlowId);

                //回退前的倒数第二条流转记录，将被更新成为回退后的最后一条记录
                Entity_TB_FLOW_WORKFLOW_DETAIL entityDetail_Update = this.GetPreviosTrasferInfoEntityByFlowId(strFlowId);
                //entityDetail_Update.SWFDCODE = System.Guid.NewGuid().ToString();
                //entityDetail_Update.SWORKFLOWCODE = strFlowId;
                //entityDetail_Update.NNUMBER = iNumberLast + 1;
                //entityDetail_Update.SSOURPOSTCODE = entityNextPost.SPOSTCODE;
                //entityDetail_Update.SSOURPOSTNAME = entityNextPost.SPOSTNAME;
                //entityDetail_Update.SSOURPOSTNAMECN = entityNextPost.SPOSTNAMECN;
                //entityDetail_Update.SSOURDEPTID = entityNextUser.SDEPT;
                //entityDetail_Update.SSOURUSERID = entityNextUser.SUSERID;
                //entityDetail_Update.SSOURUSERNAME = entityNextUser.SUSERNAME;
                //entityDetail_Update.SSOURUSERNAMECN = entityNextUser.SUSERNAMECN;
                //entityDetail_Update.DTSOURDATE = DateTime.Now;
                //entityDetail_Update.SDESTPOSTCODE = null;
                //entityDetail_Update.SDESTPOSTNAME = null;
                //entityDetail_Update.SDESTPOSTNAMECN = null;
                //entityDetail_Update.SDESTDEPTID = null;
                //entityDetail_Update.SDESTUSERID = null;
                //entityDetail_Update.SDESTUSERNAME = null;
                //entityDetail_Update.SDESTUSERNAMECN = null;
                //entityDetail_Update.DTDESTDATE = null;
                //entityDetail_Update.SMEMO = null;
                //entityDetail_Update.SFLOWPATHCODE = null;
                //entityDetail_Update.NMOVETOTALDAY = null;
                //entityDetail_Update.NWORKPROCESSDAY = null;
                //entityDetail_Update.BPROMPT = "0";//是否工作超时
                //entityDetail_Update.SBIZSTATUS = Com.ValuePlus.SysParams.FlowConfigParamGetter.GetFlowConfigParamValue("defaultBizState");
                //entityDetail_Update.SWORKFLOWNAME = entityLastDetail.SWORKFLOWNAME;
                //entityDetail_Update.SWORKFLOWNAMECN = entityLastDetail.SWORKFLOWNAMECN;
                //entityDetail_Update.SFLOWCODE = entityLastDetail.SFLOWCODE;
                //entityDetail_Update.SENTITYID = entityLastDetail.SENTITYID;
                entityDetail_Update.SFLOWMOVECODE = Com.ValuePlus.SysParams.FlowConfigParamGetter.GetFlowConfigParamValue("FlowMoveState_BeReturned");//流程接收移交状态编码设置为“已转出被退回”
                //entityDetail_Update.SWKSCODE = Com.ValuePlus.SysParams.FlowConfigParamGetter.GetFlowConfigParamValue("FlowState_InUse");
                //entityDetail_Update.SENTITYNAME = entityLastDetail.SENTITYNAME;
                //entityDetail_Update.SENTITYNAMECN = entityLastDetail.SENTITYNAMECN;
                entityDetail_Update.BISCURSTEP = "1";
                String strPreUserId = entityDetail_Update.SSOURUSERID;//上岗位处理人员

                //当前流程实例实体，需更新到数据库
                Entity_TB_FLOW_WORK_INSTANCE entityInstance = this.GetFlowInstanceInfoEntityByFlowId(strFlowId);
                entityInstance.SPENDINGUSERID = strPreUserId;//所处待办用户
                entityInstance.SFLOWMOVECODE = Com.ValuePlus.SysParams.FlowConfigParamGetter.GetFlowConfigParamValue("FlowMoveState_BeReturned");//流程接收移交状态编码设置为“已转出被退回”


                FlowDetailDao dao = new FlowDetailDao();
                iCount = dao.returnFlowPost(entityInstance, entityDetail_Delete, entityDetail_Update);

            }
            return iCount;
        }

        /// <summary>
        /// 根据流程实例获取其最后一条流转信息,返回Entity_TB_FLOW_WORKFLOW_DETAIL实体
        /// </summary>
        /// <param name="strFlowId"></param>
        /// <returns>Entity_TB_FLOW_WORKFLOW_DETAIL</returns>
        private Entity_TB_FLOW_WORKFLOW_DETAIL GetLastTrasferInfoEntityByFlowId(String strFlowId)
        {
            FlowInstanceBll bll = new FlowInstanceBll();
            return bll.GetLastTrasferInfoEntityByFlowId(strFlowId);
        }

        /// <summary>
        /// 根据流程实例获取其倒数第二条(即前一条)流转信息,返回Entity_TB_FLOW_WORKFLOW_DETAIL实体
        /// </summary>
        /// <param name="strFlowId"></param>
        /// <returns>Entity_TB_FLOW_WORKFLOW_DETAIL</returns>
        private Entity_TB_FLOW_WORKFLOW_DETAIL GetPreviosTrasferInfoEntityByFlowId(String strFlowId)
        {
            FlowInstanceBll bll = new FlowInstanceBll();
            return bll.GetPreviousTrasferInfoEntityByFlowId(strFlowId);
        }

        /// <summary>
        /// 根据流程实例ID获取一条TB_FLOW_WORK_INSTANCE,返回Entity_TB_FLOW_WORK_INSTANCE实体
        /// </summary>
        /// <param name="strFlowId"></param>
        /// <returns>Entity_TB_FLOW_WORK_INSTANCE</returns>
        private Entity_TB_FLOW_WORK_INSTANCE GetFlowInstanceInfoEntityByFlowId(String strFlowId)
        {
            FlowInstanceBll bll = new FlowInstanceBll();
            return bll.GetFlowInstanceInfoEntityByFlowId(strFlowId);
        }

        /// <summary>
        /// 根据流程实例ID获取该流程实例所有流转信息，返回dateset记录集
        /// </summary>
        /// <param name="strFlowId"></param>
        /// <returns>DataSet</returns>
        public DataSet GetAllTransferInfoByFlowId(String strFlowId)
        {
            FlowDetailDao dao = new FlowDetailDao();
            return dao.findAllByFlowId(strFlowId);
        }
    }
}
