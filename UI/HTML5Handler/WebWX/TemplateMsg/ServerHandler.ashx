<%@ WebHandler Language="C#" Class="ServerHandler" %>

using System;
using System.Web;
using System.Text;
using System.IO;
using System.Net;
using System.Data;
using System.Collections;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Com.ValuePlus.Web;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Weixin;
using Com.ValuePlus.Common.Security;
using Microsoft.JScript;
using Com.ValuePlus.Archive.BLL;

public class ServerHandler : HandlerBase, IHttpHandler, System.Web.SessionState.IRequiresSessionState {

    public void ProcessRequest (HttpContext context) {
        Hashtable hsTableUrlQuery = this.GetUrlAnalyse(context);
        string strParam = hsTableUrlQuery["param"] == null ? string.Empty : hsTableUrlQuery["param"].ToString();//param
        string strKeyValue = hsTableUrlQuery["keyvalue"] == null ? string.Empty : hsTableUrlQuery["keyvalue"].ToString();//
        string strJsonPushAimData = hsTableUrlQuery["aimdata"] == null ? string.Empty : hsTableUrlQuery["aimdata"].ToString();//param
        string strProjectId = hsTableUrlQuery["projectid"] == null ? string.Empty : hsTableUrlQuery["projectid"].ToString();//param
        string strUserCode = hsTableUrlQuery["usercode"] == null ? string.Empty : hsTableUrlQuery["usercode"].ToString();//param
        bool bIsFromMobile = false;//是否手机端入口
        if(String.IsNullOrEmpty(strParam)){
            //手机端标记
            bIsFromMobile = true;
            //如果传参为空，则考虑是Uniapp手机端的入口，则采用另外一种获取参数的模式            
            //解析客户端传递过来的json data
            StreamReader reader = new StreamReader(context.Request.InputStream);
            String strParamJson = HttpUtility.UrlDecode(reader.ReadToEnd());
            //log.Error("传入参数的Json data字符串:" + strParamJson.ToString());

            strParam = WebCommon.GetJsonValue(strParamJson,"param").ToString();//请求类型参数
            strKeyValue = WebCommon.GetJsonValue(strParamJson,"keyvalue").ToString();//
            strProjectId = WebCommon.GetJsonValue(strParamJson,"projectid").ToString();//
            strUserCode = WebCommon.GetJsonValue(strParamJson,"usercode").ToString();//
            strJsonPushAimData = WebCommon.GetJsonObjectValue(strParamJson,"aimdata").ToString();//

            //log.Error("(HTML5Handler/WebWX/TemplateMsg/ServerHandler.ashx:)" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "---移动手机端执行完成动作:" + this.GetUserCode());
        }else{
            //log.Error("(HTML5Handler/WebWX/TemplateMsg/ServerHandler.ashx:)" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "---PC端执行完成动作:" + this.GetUserCode());
        }

        //log.Error("(HTML5Handler/WebWX/TemplateMsg/ServerHandler.ashx:)" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "---Param:" + strParam);
        //log.Error("(HTML5Handler/WebWX/TemplateMsg/ServerHandler.ashx:)" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "---keyvalue:" + strKeyValue);
        //log.Error("(HTML5Handler/WebWX/TemplateMsg/ServerHandler.ashx:)" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "---JsonPushAimData:" + strJsonPushAimData);
        if (strParam.Equals("pushtemplatedata"))
        {
            PushTemplateData(context,strJsonPushAimData,strProjectId,strUserCode,bIsFromMobile);
        }
    }

    /// <summary>
    /// 根据目标信息及用户推送模版数据
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strJsonPushAimData"></param>
    /// <param name="strProjectId"></param>
    /// <param name="strUserCode"></param>
    /// <param name="bIsFromMobile"></param>
    private void PushTemplateData(HttpContext context,String strJsonPushAimData,String strProjectId,String strUserCode,bool bIsFromMobile)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        String strMethodDesc = "根据目标信息及用户推送模版数据";
        StringBuilder sbResult = new StringBuilder();
        try
        {
            strJsonPushAimData = Microsoft.JScript.GlobalObject.decodeURIComponent(strJsonPushAimData);
            JObject jsonObject = (JObject)JsonConvert.DeserializeObject(strJsonPushAimData);
            String strAimTID = jsonObject["FCODE"].ToString();
            String strAimRID = jsonObject["PostCode"].ToString();
            String strAimSID = jsonObject["SceneCode"].ToString();
            String strAimUserId = jsonObject["SUSERID"].ToString();
            String strAimMobileNo = jsonObject["MOBILENO"].ToString();
            String strAimStaffNo = jsonObject["STAFFNO"].ToString();
            String strAimProjectId = jsonObject["ProjectId"].ToString();
            String strAimPendingQty = jsonObject["PendingQty"].ToString();

            String strAimFNAME = jsonObject["FNAMECHS"].ToString();
            String strAimPostName = jsonObject["PostNameChs"].ToString();
            String strAimSceneName = jsonObject["SceneNameChs"].ToString();

            StringBuilder sbLog1 = new StringBuilder();
            sbLog1.Append("【HTML5Handler/WebWX/TemplateMsg/ServerHandler.ashx.PushTemplateData 模版消息推送】 strAimProjectId:"+strAimProjectId);
            sbLog1.Append("；strAimTID:"+strAimTID+"；strAimRID:"+strAimRID+"；strAimSID:"+strAimSID);
            //log.Error(sbLog1.ToString());
            //从CS服务器中的【微信推送模版管理】配置中获取此项目此角色场景适用哪种模版【只取其中第一个模版】
            StringBuilder sb_GetTemplate = new StringBuilder();
            sb_GetTemplate.Append("select top 1 A.*,B.TEMPID from WXPushTemplate_2 A INNER JOIN WXPushTemplate_1 B ON A.TEMPCODE = B.TEMPCODE ");
            sb_GetTemplate.Append(" WHERE A.ProjectId = '"+strAimProjectId+"' and A.FCODE = '"+strAimTID+"' AND A.PostCode = '"+strAimRID+"' AND A.SceneCode = '"+strAimSID+"' order by A.SEQNO DESC");
            String strGetTemplate = sb_GetTemplate.ToString();
            DataTable dtGetTemplate = SqlParamDao.GetDataTableBySql(strGetTemplate);
            if(dtGetTemplate!=null && dtGetTemplate.Rows.Count == 1)
            {
                DataRow dr = dtGetTemplate.Rows[0];
                String strTemplateCode = dr["TEMPCODE"].ToString();
                String strTemplateId = dr["TEMPID"].ToString();
                //log.Error("【HTML5Handler/WebWX/TemplateMsg/ServerHandler.ashx.PushTemplateData模版消息推送】当前 strTemplateCode:"+strTemplateCode);
                //log.Error("【HTML5Handler/WebWX/TemplateMsg/ServerHandler.ashx.PushTemplateData模版消息推送】当前 strTemplateId:"+strTemplateId);

                String strNowTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                String strAppId = GetSysParams.GetWeixin_AppId();
                String strAppsecret = GetSysParams.GetWeixin_Appsecret();
                string strGetToken = AccessTokenGetter.GetValidAccessToken(strAppId,strAppsecret,strNowTime,"050");

                //根据MobileNo获取其在此公众号中绑定的OpenId
                String strSql_GetOpenId = "select top 1 * from WXUser_1 WHERE MobileNo = '"+strAimMobileNo+"' ORDER BY RegTime desc";
                String strOpenId = "";
                DataTable dt_GetOpenId = SqlParamDao.GetDataTableBySql(strSql_GetOpenId);
                if (dt_GetOpenId != null && dt_GetOpenId.Rows.Count == 1)
                {
                    strOpenId = dt_GetOpenId.Rows[0]["OpenId"].ToString();
                }
                StringBuilder sbLog2 = new StringBuilder();
                sbLog2.Append("【HTML5Handler/WebWX/TemplateMsg/ServerHandler.ashx.PushTemplateData 模版消息推送】 strTemplateCode:"+strTemplateCode);
                sbLog2.Append("；strTemplateCode:"+strTemplateCode+"；strTemplateId:"+strTemplateId+"；AppId:"+strAppId);
                sbLog2.Append("；AppSecret:"+strAppsecret+"；AccessToke："+strGetToken+"；strOpenId："+strOpenId);
                //log.Error(sbLog2.ToString());

                if(!String.IsNullOrEmpty(strOpenId))
                {
                    String strPushResult = "";
                    switch (strTemplateCode) {
                        case "46435":
                            strPushResult = this.PushTemplateData_46435(strGetToken,strTemplateId,strOpenId,strAimFNAME,strAimPostName,strAimSceneName,strAimPendingQty,strUserCode);
                            /// 记录推送记录
                            this.WritePushRecord(strTemplateCode, strProjectId, strAimTID, strAimRID, strAimSID, strAimUserId,strPushResult, strUserCode,bIsFromMobile);
                            break;
                    }
                }
            }

            strReturnCode = "1";
            strReturnMsg = strMethodDesc+"成功";

            //sbResultData.Append("\"ResultData\":"+sbResult_NextInfo.ToString());
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = strMethodDesc+"出错,请稍候重试 ";
            log.Error(strReturnMsg + "：\r\n" + ex.ToString());
        }
        finally{
            StringBuilder sbResultStatus = new StringBuilder();
            sbResultStatus.Append("\"ReturnStatus\":{");
            sbResultStatus.Append("\"ReturnCode\":\""+strReturnCode.ToString()+"\"");
            sbResultStatus.Append(",\"ReturnMsg\":\""+strReturnMsg.ToString()+"\"");
            sbResultStatus.Append("}");

            sbResult.Append("{");
            sbResult.Append(sbResultStatus.ToString());
            if (!String.IsNullOrEmpty(sbResultData.ToString()))
            {
                sbResult.Append(","+sbResultData.ToString());
            }
            sbResult.Append("}");
        }
        //log.Error(strMethodDesc + "时返回数据：" + sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }

    /// <summary>
    /// 根据模版46435推送数据
    /// </summary>
    /// <param name="strGetToken"></param>
    /// <param name="strTemplateId"></param>
    /// <param name="strOpenId"></param>
    /// <param name="strAimFNAME"></param>
    /// <param name="strAimPostName"></param>
    /// <param name="strAimSceneName"></param>
    /// <param name="strAimPendingQty"></param>
    /// <param name="strUserCode"></param>
    private String PushTemplateData_46435(String strGetToken,String strTemplateId,String strOpenId,String strAimFNAME
        ,String strAimPostName,String strAimSceneName,String strAimPendingQty,String strUserCode)
    {
        String strPushResult = "";
        try
        {
            if (!String.IsNullOrEmpty(strGetToken))
            {
                //发送文本消息
                String strToUrl = "https://mobile.compassolution.com/#/pages/CS/wx/wxAuth?entry=hrRedirect";
                String strKeyWord1 = strAimFNAME;
                String strKeyWord2 = strAimSceneName;
                String strKeyWord3 = strAimPostName;
                String strKeyWord4 = strAimPendingQty;
                StringBuilder sBuilderJson = new StringBuilder();
                sBuilderJson.Append("{");
                sBuilderJson.Append("	\"touser\": \"" + strOpenId + "\",");//接收者openid
                sBuilderJson.Append("	\"template_id\": \""+strTemplateId+"\",");//模板ID
                sBuilderJson.Append("	\"url\": \""+strToUrl+"\",");//模板跳转链接（海外账号没有跳转能力）
                sBuilderJson.Append("	\"client_msg_id\": \"\",");//防重入id。对于同一个openid + client_msg_id, 只发送一条消息,10分钟有效,超过10分钟不保证效果。若无防重入需求，可不填
                //流程名称{{thing7.DATA}}
                //流程状态{{thing10.DATA}}
                //审批事项{{thing18.DATA}}
                //待处理数量{{number3.DATA}}
                sBuilderJson.Append("	\"data\": {");
                sBuilderJson.Append("		\"thing7\":{");
                sBuilderJson.Append("			\"value\":\""+strKeyWord1+"\"");
                sBuilderJson.Append("		}");
                sBuilderJson.Append("		,\"thing10\": {");
                sBuilderJson.Append("			\"value\":\""+strKeyWord2+"\"");
                sBuilderJson.Append("		}");
                sBuilderJson.Append("		,\"thing18\": {");
                sBuilderJson.Append("			\"value\":\""+strKeyWord3+"\"");
                sBuilderJson.Append("		}");
                sBuilderJson.Append("		,\"number3\": {");
                sBuilderJson.Append("			\"value\":\""+strKeyWord4+"\"");
                sBuilderJson.Append("		}");
                sBuilderJson.Append("	}");
                sBuilderJson.Append("}");
                String json = sBuilderJson.ToString();


                byte[] byteArray = Encoding.UTF8.GetBytes(json);
                String ps = HttpRequestHelper.RequestPostData(string.Format(Const.Weixin_URL_SendTemplateMessage, strGetToken), json);

                String strMsg_id = JsonHelper.GetJsonValue(ps, "msg_id");
                String strErrcode = JsonHelper.GetJsonValue(ps, "errcode");

                StringBuilder sbLog1 = new StringBuilder();
                sbLog1.Append("【HTML5Handler/WebWX/TemplateMsg/ServerHandler.ashx.PushTemplateData_46435 模版消息推送】 对象OpenId:"+strOpenId);
                sbLog1.Append("；返回值:"+ps);

                strPushResult = ps+","+sBuilderJson.ToString();
                //log.Error(sbLog1.ToString());
            }
        }
        catch (Exception ex)
        {
            log.Error("根据模版46435推送数据出错："+ex.ToString());
        }
        return strPushResult;
    }

    /// <summary>
    /// 记录推送记录
    /// </summary>
    /// <param name="strTemplateCode"></param>
    /// <param name="strProjectId"></param>
    /// <param name="strFCODE"></param>
    /// <param name="strPostCode"></param>
    /// <param name="strSceneCode"></param>
    /// <param name="strAimUserId"></param>
    /// <param name="strPushResult"></param>
    /// <param name="strUserCode"></param>
    /// <param name="bIsFromMobile"></param>
    private void WritePushRecord(String strTemplateCode,String strProjectId,String strFCODE,String strPostCode,String strSceneCode,String strAimUserId,String strPushResult,String strUserCode,bool bIsFromMobile)
    {
        StringBuilder sbInsertSql = new StringBuilder();
        try
        {
            String strIsSuccess = String.IsNullOrEmpty(strPushResult) ? "2" : "1";
            //先获取最新的SEQNO
            String strSql = "select ISNULL(Max(SEQNO),0)+1 AS NextSeqNo from WXPushTemplate_3 where TEMPCODE = '"+strTemplateCode+"' ";
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
            String strNextSeqNo = dt.Rows[0]["NextSeqNo"].ToString();

            ///插入记录
            String strPushTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            sbInsertSql = new StringBuilder();
            sbInsertSql.Append("INSERT INTO [WXPushTemplate_3]([TEMPCODE],[SEQNO],[ProjectId],[FCODE],[PostCode],[SceneCode],[AcceptUserCode],[PushUserCode],[PushTime],[TriggerFrom],[IsSuccess],[PushResult])");
            sbInsertSql.Append("VALUES('"+strTemplateCode+"','"+strNextSeqNo+"','"+strProjectId+"','"+strFCODE+"','"+strPostCode+"','"+strSceneCode+"','"+strAimUserId+"'");
            sbInsertSql.Append(",'"+strUserCode+"','"+strPushTime+"','"+(bIsFromMobile?"手机端":"PC端")+"','"+strIsSuccess+"','"+strPushResult+"')");

            int iCount = SqlParamDao.ExecuteNonQueryBySql(sbInsertSql.ToString());
        }
        catch (Exception ex)
        {
            log.Error("WritePushRecord记录推送记录出错,sql："+sbInsertSql.ToString());
            log.Error("WritePushRecord记录推送记录出错："+ex.ToString());
        }

    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}