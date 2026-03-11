using System;
using System.Text;
using Com.ValuePlus.Flow.DAL.Flow;
using System.Data;
using Com.ValuePlus.Flow.Entity;

namespace Com.ValuePlus.Flow.BLL.Flow
{
    public class FlowPostBll
    {
        /// <summary>
        /// 根据流程定义获取该流程定义的起始岗位
        /// </summary>
        /// <param name="strFlowCode"></param>
        /// <returns></returns>
        public DataSet GetStartPostInfoByFlowCode(String strFlowCode)
        {
            FlowPostDao dao = new FlowPostDao();
            DataSet ds = dao.findStartPostByFlowCode(strFlowCode);

            return ds;
        }

        /// <summary>
        /// 根据流程定义获取该流程定义的起始岗位实体
        /// </summary>
        /// <param name="strFlowCode"></param>
        /// <returns></returns>
        public Entity_TB_FLOW_POST_DEFINE GetStartPostEntityByFlowCode(String strFlowCode)
        {
            DataSet ds = this.GetStartPostInfoByFlowCode(strFlowCode);
            Entity_TB_FLOW_POST_DEFINE entityPostCode = new Entity_TB_FLOW_POST_DEFINE();
            if ((ds != null) && (ds.Tables.Count > 0)&&(ds.Tables[0].Rows.Count>0))
            {
                entityPostCode.SPOSTCODE = ds.Tables[0].Rows[0]["SPOSTCODE"].ToString();
                entityPostCode.SFLOWCODE = ds.Tables[0].Rows[0]["SFLOWCODE"].ToString();
                entityPostCode.SPOSTNAME = ds.Tables[0].Rows[0]["SPOSTNAME"].ToString();
                entityPostCode.SPOSTNAMECN = ds.Tables[0].Rows[0]["SPOSTNAMECN"].ToString();
                if (ds.Tables[0].Rows[0]["NWORKDAY"] != DBNull.Value)
                {
                    entityPostCode.NWORKDAY = System.Convert.ToDecimal(ds.Tables[0].Rows[0]["NWORKDAY"].ToString());
                }
                entityPostCode.SPLUGIN_PRE = ds.Tables[0].Rows[0]["SPLUGIN_PRE"].ToString();
                entityPostCode.SPLUGIN_AFTER = ds.Tables[0].Rows[0]["SPLUGIN_AFTER"].ToString();
                entityPostCode.BISSTARTPOST = ds.Tables[0].Rows[0]["BISSTARTPOST"].ToString();
            }
            return entityPostCode;
        }

        /// <summary>
        /// 根据岗位编码查询TB_FLOW_POST_DEFINE相应记录，返回dateset记录集
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet GetPostDefineInfoByPostCode(String strPostCode)
        {
            FlowPostDao dao = new FlowPostDao();
            return dao.findPostInfoByPostCode(strPostCode);
        }

        /// <summary>
        /// 根据岗位编码查询TB_FLOW_POST_DEFINE相应记录，返回岗位定义实体信息
        /// </summary>
        /// <param name="strPostCode"></param>
        /// <returns></returns>
        public Entity_TB_FLOW_POST_DEFINE GetPostDefineEntityByPostCode(String strPostCode)
        {
            DataSet ds = this.GetPostDefineInfoByPostCode(strPostCode); 
            Entity_TB_FLOW_POST_DEFINE entityPostCode = new Entity_TB_FLOW_POST_DEFINE();
            if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
            {
                entityPostCode.SPOSTCODE = ds.Tables[0].Rows[0]["SPOSTCODE"].ToString();
                entityPostCode.SFLOWCODE = ds.Tables[0].Rows[0]["SFLOWCODE"].ToString();
                entityPostCode.SPOSTNAME = ds.Tables[0].Rows[0]["SPOSTNAME"].ToString();
                entityPostCode.SPOSTNAMECN = ds.Tables[0].Rows[0]["SPOSTNAMECN"].ToString();
                if (ds.Tables[0].Rows[0]["NWORKDAY"] != DBNull.Value)
                {
                    entityPostCode.NWORKDAY = System.Convert.ToDecimal(ds.Tables[0].Rows[0]["NWORKDAY"].ToString());
                }
                entityPostCode.SPLUGIN_PRE = ds.Tables[0].Rows[0]["SPLUGIN_PRE"].ToString();
                entityPostCode.SPLUGIN_AFTER = ds.Tables[0].Rows[0]["SPLUGIN_AFTER"].ToString();
                entityPostCode.BISSTARTPOST = ds.Tables[0].Rows[0]["BISSTARTPOST"].ToString();
            }
            return entityPostCode;
        }

        /// <summary>
        /// 根据岗位编码查询相应名称，返回字符串
        /// </summary>
        /// <returns>String</returns>
        public String GetPostNameByPostCode(String strPostCode,String strLanguage)
        {
            DataSet ds = this.GetPostDefineInfoByPostCode(strPostCode);
            String strPostName = strPostCode;
            if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
            {
                if (strLanguage.Equals("en-us"))
                {
                    strPostName = ds.Tables[0].Rows[0]["SPOSTNAME"].ToString();
                }
                else
                {
                    strPostName = ds.Tables[0].Rows[0]["SPOSTNAMECN"].ToString();
                }
            }
            return strPostName;
        }

        /// <summary>
        /// 根据岗位编码查询TB_FLOW_POST_ACTION的相应记录，返回dateset记录集
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet GetActionInfoByPostCode(String strPostCode)
        {
            FlowPostDao dao = new FlowPostDao(); 
            return dao.findActionByPostCode(strPostCode);
        }

        /// <summary>
        /// 根据前岗位编码查询TB_FLOW_PATH中对应下岗位信息的相应记录，返回dateset记录集
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet GetNextPostInfoByPrePostCode(String strPostCode)
        {
            FlowPostDao dao = new FlowPostDao();
            return dao.findNextPostInfoByPrePostCode(strPostCode);
        }

        /// <summary>
        /// 根据岗位编码查询TB_FLOW_POST_ACTOR的相应记录，返回dateset记录集
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet GetActorInfoByPostCode(String strPostCode)
        {
            FlowPostDao dao = new FlowPostDao();
            return dao.findActorByPostCode(strPostCode);
        }

        /// <summary>
        /// 根据路径编码获取表TB_FLOW_RESERVED_MEMO相应记录，返回dateset记录集
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet GetReservedMemoByPathCode(String strPathCode)
        {
            FlowPostDao dao = new FlowPostDao();
            return dao.findReservedMemoByPathCode(strPathCode);
        }

    }
}
