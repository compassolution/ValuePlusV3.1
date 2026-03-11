<%@ WebHandler Language="C#" Class="ServerHandler" %>

using System;
using System.Web;
using System.Text;
using System.Net;
using System.Data;
using System.Collections;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Com.ValuePlus.Web;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Weixin;
using Com.ValuePlus.Common.Security;

public class ServerHandler : HandlerBase, IHttpHandler, System.Web.SessionState.IRequiresSessionState {

    public void ProcessRequest (HttpContext context) {
        //跨域提交表单，前端ajax不用做任何修改
        context.Response.AddHeader("Access-Control-Allow-Origin", "*");//支持全域名访问，不安全，部署后需要固定限制为客户端网址

        Hashtable hsTableUrlQuery = this.GetUrlAnalyse(context);
        string strCallback = hsTableUrlQuery["callback"] == null ? string.Empty : hsTableUrlQuery["callback"].ToString();//param
        string strParam = hsTableUrlQuery["param"] == null ? string.Empty : hsTableUrlQuery["param"].ToString();//param
        string strUserId = hsTableUrlQuery["userid"] == null ? string.Empty : hsTableUrlQuery["userid"].ToString();//param
        string strProjectId = hsTableUrlQuery["projectid"] == null ? string.Empty : hsTableUrlQuery["projectid"].ToString();//param
        string strMessageType = hsTableUrlQuery["msgtype"] == null ? string.Empty : hsTableUrlQuery["msgtype"].ToString();//param
        string strWXMPModule = hsTableUrlQuery["wxmpmodule"] == null ? string.Empty : hsTableUrlQuery["wxmpmodule"].ToString();//param

        //log.Error("(UpdateServerHandler)"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"---Param:"+strParam);
        if (strParam.Equals("savemessagedata"))
        {
            SaveMessage(context,strProjectId,strMessageType,strUserId);
        }else if (strParam.Equals("pushmessagedata"))
        {
            SendTemplateMessage(context);
        }
    }


    /// <summary>
    /// 保存推送消息数据到数据库
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strProjectId"></param>
    /// <param name="strMessageType"></param>
    /// <param name="strUserId"></param>
    private void SaveMessage(HttpContext context,String strProjectId,String strMessageType,String strUserId)
    {
        String IsPushSuccess = "0";
        int iNeedPushCount = 0;
        int iHadPushCount = 0;
        try
        {
            String strCurDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            String strMSGCODE = DateTime.Now.ToString("yyyyMMddHHmmss")+strProjectId;
            String strIsSuccess = "2";//默认未发送成功

            //消息模板短ID集合
            Hashtable hsTableTemplateShortId = new Hashtable();
            hsTableTemplateShortId.Add("010", "OPENTM409879450");//生日祝福类型

            StringBuilder sbSqlDelete = new StringBuilder();
            StringBuilder sbSqlInsert1 = new StringBuilder();
            StringBuilder sbSqlInsert2 = new StringBuilder();
            StringBuilder sbSqlInsert3 = new StringBuilder();
            StringBuilder sbSqlUpdate = new StringBuilder();
            String strMainStaff = context.Request["txt_MainStaff"].ToString();
            String strCCStaff = context.Request["txt_CCStaff"].ToString();
            String strMessageTitle = context.Request["txt_MessageTitle"].ToString();
            String strMessageContent = context.Request["txt_MessageContent"].ToString();
            strMessageContent = strMessageContent.Replace("'", "''");
            String strMessageBadging = context.Request["txt_MessageBadging"].ToString();
            String strWXTemplateShortId = hsTableTemplateShortId[strMessageType].ToString();
            
            //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
            strMainStaff = SQLInjectionDefense.ReplaceSQLReservedKeyword(strMainStaff);
            strCCStaff = SQLInjectionDefense.ReplaceSQLReservedKeyword(strCCStaff);
            strMessageTitle = SQLInjectionDefense.ReplaceSQLReservedKeyword(strMessageTitle);
            strMessageContent = SQLInjectionDefense.ReplaceSQLReservedKeyword(strMessageContent);
            strMessageBadging = SQLInjectionDefense.ReplaceSQLReservedKeyword(strMessageBadging);
            strWXTemplateShortId = SQLInjectionDefense.ReplaceSQLReservedKeyword(strWXTemplateShortId);

            //String strMainStaff = "13423668826+00026+王伟华+工程部+值班工程师,15915781288+00073+陈立志+保安部+消防主管";
            //String strCCStaff = "13423668826+00026+王伟华+工程部+值班工程师,15915781288+00073+陈立志+保安部+消防主管";
            //String strMessageTitle = "Test";
            //String strMessageContent = "13423668826+00026+王伟华+工程部+值班工程师,15915781288+00073+陈立志+保安部+消防主管";
            //String strMessageBadging = "Test";

            //log.Error("保存推送消息数据到数据库(ProjectId:" + strProjectId);
            //log.Error("保存推送消息数据到数据库(strMessageType:" + strMessageType);
            //log.Error("保存推送消息数据到数据库(strMainStaff:" + strMainStaff);
            //log.Error("保存推送消息数据到数据库(strCCStaff:" + strCCStaff);
            //log.Error("保存推送消息数据到数据库(strCCStaff:" + strCCStaff);
            //log.Error("保存推送消息数据到数据库(strMessageTitle:" + strMessageTitle);
            //log.Error("保存推送消息数据到数据库(strMessageContent:" + strMessageContent);
            //log.Error("保存推送消息数据到数据库(strMessageBadging:" + strMessageBadging);

            //删除现有的表数据
            sbSqlDelete.Append("delete from WXPushMessage_1 where MSGCODE = '"+strMSGCODE+"';");
            sbSqlDelete.Append("delete from WXPushMessage_2 where MSGCODE = '"+strMSGCODE+"';");
            sbSqlDelete.Append("delete from WXPushMessage_3 where MSGCODE = '"+strMSGCODE+"';");
            sbSqlDelete.Append("delete from WXPushMessage_4 where MSGCODE = '"+strMSGCODE+"';");
            //插入主表的数据脚本（WXPushMessage_1）
            sbSqlInsert1.Append("INSERT INTO [WXPushMessage_1]([MSGCODE],[MsgType],[MsgTitle],[MsgContent],[MsgBadging],[WXTemplateCode],[ProjectId],[IsSuccess],[MsgPushTime],[MsgUserId])");
            sbSqlInsert1.Append("values('"+strMSGCODE+"','"+strMessageType+"','"+strMessageTitle+"','"+strMessageContent+"','"+strMessageBadging+"'");
            sbSqlInsert1.Append(",'"+strWXTemplateShortId+"','"+strProjectId+"','"+strIsSuccess+"','"+strCurDateTime+"','"+strUserId+"');");
            sbSqlInsert1.Append("");
                
            //插入主接收人表的数据脚本（WXPushMessage_2）
            if (!String.IsNullOrEmpty(strMainStaff))
            {
                String[] strMainStaffArrary = strMainStaff.Split(',');
                if (strMainStaffArrary != null && strMainStaffArrary.Length > 0)
                {
                    for(int i = 0; i < strMainStaffArrary.Length; i++)
                    {
                        String[] strMainStaffArrary1 = strMainStaffArrary[i].Split('+');
                        sbSqlInsert2.Append("INSERT INTO [WXPushMessage_2]([MSGCODE],[MobileNo],[StaffNo],[StaffName],[StaffNameChs],[DeptCode],[DeptName],[PosiCode],[PosiName],[IsPushed])");
                        sbSqlInsert2.Append("values('"+strMSGCODE+"','"+strMainStaffArrary1[0]+"','"+strMainStaffArrary1[1]+"',null,'"+strMainStaffArrary1[2]+"'");
                        sbSqlInsert2.Append(",null,'"+strMainStaffArrary1[3]+"',null,'"+strMainStaffArrary1[4]+"','2');");
                        iNeedPushCount++;
                    }
                }
            }
            //插入次接收人表的数据脚本（WXPushMessage_3）
            if (!String.IsNullOrEmpty(strCCStaff))
            {
                String[] strCCStaffArrary = strCCStaff.Split(',');
                if (strCCStaffArrary != null && strCCStaffArrary.Length > 0)
                {
                    for(int i = 0; i < strCCStaffArrary.Length; i++)
                    {
                        String[] strCCStaffArrary1 = strCCStaffArrary[i].Split('+');
                        sbSqlInsert3.Append("INSERT INTO [WXPushMessage_3]([MSGCODE],[MobileNo],[StaffNo],[StaffName],[StaffNameChs],[DeptCode],[DeptName],[PosiCode],[PosiName],[IsPushed])");
                        sbSqlInsert3.Append("values('"+strMSGCODE+"','"+strCCStaffArrary1[0]+"','"+strCCStaffArrary1[1]+"',null,'"+strCCStaffArrary1[2]+"'");
                        sbSqlInsert3.Append(",null,'"+strCCStaffArrary1[3]+"',null,'"+strCCStaffArrary1[4]+"','2');");
                        iNeedPushCount++;
                    }
                }
            }

            sbSqlDelete.Append(sbSqlInsert1.ToString());
            sbSqlDelete.Append(sbSqlInsert2.ToString());
            sbSqlDelete.Append(sbSqlInsert3.ToString());

            String strInsertSql = sbSqlDelete.ToString();
            if (!String.IsNullOrEmpty(strInsertSql))
            {
                try
                {
                    int iReturnCount = SqlParamDao.ExecuteNonQueryBySql(strInsertSql);
                    if (iReturnCount > 0)
                    {
                        //推送操作
                        iHadPushCount = SendTemplateMessage(strMSGCODE,strMessageType,strWXTemplateShortId);

                        //更新是否发送成功的标记位
                        if (iHadPushCount == iNeedPushCount)
                        {
                            sbSqlUpdate.Append("update [WXPushMessage_1] set [IsSuccess] = '1' where [MSGCODE] = '"+strMSGCODE+"'");
                            int iUpdate = SqlParamDao.ExecuteNonQueryBySql(sbSqlUpdate.ToString());

                            IsPushSuccess = "1";
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error("保存推送消息数据到数据库出错，SQL语句执行失败\r\n");
                    log.Error("SQL语句:" + strInsertSql + "\r\n");
                    log.Error("错误信息：" + ex.ToString());
                    IsPushSuccess = "-1";
                }
            }
        }
        catch (Exception ex)
        {
            log.Error("推送消息数据到数据库失败\r\n");
            log.Error("错误信息：" + ex.ToString());
            IsPushSuccess = "-1";
        }
        finally
        {
            StringBuilder sBuilder = new StringBuilder();
            sBuilder.Append("{");
            sBuilder.Append("\"IsPushSuccess\":\""+IsPushSuccess+"\"");
            sBuilder.Append(",\"iNeedPushCount\":\""+iNeedPushCount.ToString()+"\"");
            sBuilder.Append(",\"iHadPushCount\":\""+iHadPushCount.ToString()+"\"");
            sBuilder.Append("}");
            context.Response.Write(sBuilder.ToString());
        }
    }

    /// <summary>
    /// 发送模板消息推送(生日祝福)
    /// </summary>
    /// <param name="strMSGCODE"></param>
    /// <param name="strWXTemplateShortId"></param>
    private int SendTemplateMessage(String strMSGCODE,String strMessageType,String strWXTemplateShortId)
    {
        int iReturnResult = 0;
        try
        {
            String strNowTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //获取AccessToken的接口(生日祝福的入口编码为020)
            string strGetToken = AccessTokenGetter.GetValidAccessToken(GetSysParams.GetWeixin_AppId(),GetSysParams.GetWeixin_Appsecret(),strNowTime,"020");
            //log.Error("获取AccessToken的请求地址为："+strReturnURL);
            log.Error("----开始发送模板消息推送");
            log.Error("当前AppId:"+GetSysParams.GetWeixin_AppId());
            log.Error("当前AppSecret:"+GetSysParams.GetWeixin_Appsecret());
            log.Error("获取AccessToken的结果为："+strGetToken);
            if (!String.IsNullOrEmpty(strGetToken))
            {
                //第一步：获取模版ID
                String strShortId = strWXTemplateShortId;
                String jsonPostData = "{\"template_id_short\":\""+strShortId+"\"}";
                String strTemplateId_ReturnJson = HttpRequestHelper.RequestPostData(string.Format(Const.Weixin_URL_GetTemplateId, strGetToken),jsonPostData);
                String strTemplateId = JsonHelper.GetJsonValue(strTemplateId_ReturnJson, "template_id");
                log.Error("使用模板为:"+strShortId);
                log.Error("获取模板id接口返回值为:"+strTemplateId_ReturnJson);
                log.Error("使用模板的TemplateId为:"+strTemplateId);

                ////第二步：遍历接收人发送模板消息
                ////发送文本消息
                StringBuilder sbSqlSelect = new StringBuilder();
                sbSqlSelect.Append("select * from [VW_WX_PushMessageInfo] where MSGCODE = '"+strMSGCODE+"' order by SOrder");
                DataTable dtSelect = SqlParamDao.GetDataTableBySql(sbSqlSelect.ToString());
                switch (strMessageType)
                {
                    case "010"://生日祝福
                        iReturnResult = SendTemplateMessage010(strMSGCODE, strTemplateId, dtSelect, strGetToken);
                        break;
                    default:
                        break;
                }

                //第三步：删除模版ID（因为超过25个就不能调用了)
                String jsonPostData_Delete = "{\"template_id\":\""+strTemplateId+"\"}";
                String strTemplateId_ReturnJson_Delete = HttpRequestHelper.RequestPostData(string.Format(Const.Weixin_URL_DeleteTemplate, strGetToken),jsonPostData_Delete);
                String strErrcode_Delete = JsonHelper.GetJsonValue(strTemplateId_ReturnJson_Delete, "errcode");
                log.Error("删除模板TemplateId<"+strTemplateId+">时返回值：" + strTemplateId_ReturnJson_Delete);
                log.Error("删除模板TemplateId<"+strTemplateId+">时返回值中的errcode:"+strErrcode_Delete);

            }
        }
        catch (Exception ex)
        {
            log.Error("发送模板消息推送失败\r\n");
            log.Error("错误信息："+ex.ToString());
        }
        return iReturnResult;
    }

    /// <summary>
    /// 发送模板消息推送(生日祝福)
    /// </summary>
    /// <param name="strMSGCODE"></param>
    /// <param name="strTemplateId"></param>
    /// <param name="dtSelect"></param>
    /// <param name="strGetToken"></param>
    /// <returns></returns>
    private int SendTemplateMessage010(String strMSGCODE,String strTemplateId,DataTable dtSelect,String strGetToken)
    {
        int iReturnResult = 0;
        try
        {
            if (dtSelect != null && dtSelect.Rows.Count > 0)
            {
                for(int i=0;i<dtSelect.Rows.Count; i++)
                {
                    String strOpenId = dtSelect.Rows[i]["OpenId"].ToString();
                    String strMobileNo = dtSelect.Rows[i]["MobileNo"].ToString();
                    String strStaffNo = dtSelect.Rows[i]["StaffNo"].ToString();
                    String strStaffName = dtSelect.Rows[i]["StaffName"].ToString();
                    String strStaffNameChs = dtSelect.Rows[i]["StaffNameChs"].ToString();
                    String strDeptName = dtSelect.Rows[i]["DeptName"].ToString();
                    String strPosiName = dtSelect.Rows[i]["PosiName"].ToString();
                    String strMsgTitle = dtSelect.Rows[i]["MsgTitle"].ToString();
                    String strMsgContent = Microsoft.JScript.GlobalObject.escape(dtSelect.Rows[i]["MsgContent"].ToString().Replace("''","'"));
                    String strMsgBadging = dtSelect.Rows[i]["MsgBadging"].ToString();
                    String strMsgPushTime = dtSelect.Rows[i]["MsgPushTime"].ToString();

                    StringBuilder sBuilderJson = new StringBuilder();
                    sBuilderJson.Append("{");
                    sBuilderJson.Append("	\"touser\": \"" + strOpenId + "\",");
                    sBuilderJson.Append("	\"template_id\": \""+strTemplateId+"\",");
                    sBuilderJson.Append("	\"url\": \"www.baidu.com\",");
                    sBuilderJson.Append("	\"data\": {");
                    sBuilderJson.Append("		\"first\": {");
                    sBuilderJson.Append("			\"value\":\""+strMsgTitle+"\",");
                    sBuilderJson.Append("			\"color\":\"#173177\"");
                    sBuilderJson.Append("		},");
                    sBuilderJson.Append("		\"keyword1\":{");
                    sBuilderJson.Append("			\"value\":\""+strStaffNameChs+"\",");
                    sBuilderJson.Append("			\"color\":\"#173177\"");
                    sBuilderJson.Append("		},");
                    sBuilderJson.Append("		\"keyword2\": {");
                    sBuilderJson.Append("			\"value\":\""+strMsgPushTime+"\",");
                    sBuilderJson.Append("			\"color\":\"#173177\"");
                    sBuilderJson.Append("		},");
                    sBuilderJson.Append("		\"keyword3\": {");
                    sBuilderJson.Append("			\"value\":\""+strMsgBadging+"\",");
                    sBuilderJson.Append("			\"color\":\"#173177\"");
                    sBuilderJson.Append("		},");
                    sBuilderJson.Append("		\"remark\":{");
                    sBuilderJson.Append("			\"value\":\""+strMsgContent+"\",");
                    sBuilderJson.Append("			\"color\":\"#173177\"");
                    sBuilderJson.Append("		}");
                    sBuilderJson.Append("	}");
                    sBuilderJson.Append("}");
                    String json = sBuilderJson.ToString();

                    byte[] byteArray = Encoding.UTF8.GetBytes(json);
                    String ps = HttpRequestHelper.RequestPostData(string.Format(Const.Weixin_URL_SendTemplateMessage, strGetToken), json);
                    String strMsg_id = JsonHelper.GetJsonValue(ps, "msg_id");
                    String strErrcode = JsonHelper.GetJsonValue(ps, "errcode");
                    log.Error("发送模板消息推送(生日祝福)对象OpenId：" + strOpenId);
                    log.Error("发送模板消息推送(生日祝福)数据：" + json);
                    log.Error("发送模板消息推送(生日祝福)返回值：" + ps);
                    log.Error("发送模板消息推送(生日祝福)返回值中的msg_id：" + strMsg_id);
                    log.Error("发送模板消息推送(生日祝福)返回值中的errcode：" + strErrcode);

                    //推送成功则设置成功标志位
                    if (strErrcode.Equals("0"))
                    {
                        StringBuilder sbSqlUpdate = new StringBuilder();
                        sbSqlUpdate.Append("update [WXPushMessage_2] set [IsPushed] = '1' where [MSGCODE] = '"+strMSGCODE+"' and [MobileNo] = '"+strMobileNo+"';");
                        sbSqlUpdate.Append("update [WXPushMessage_3] set [IsPushed] = '1' where [MSGCODE] = '"+strMSGCODE+"' and [MobileNo] = '"+strMobileNo+"';");
                        int iUpdate = SqlParamDao.ExecuteNonQueryBySql(sbSqlUpdate.ToString());

                        iReturnResult++;
                    }
                }
            }
            return iReturnResult;
        }
        catch (Exception ex)
        {
            log.Error("发送模板消息推送(生日祝福)失败\r\n");
            log.Error("错误信息："+ex.ToString());
            return iReturnResult;
        }
    }

    /// <summary>
    /// 发送模板消息推送【测试用】
    /// </summary>
    /// <param name="context"></param>
    private void SendTemplateMessage(HttpContext context)
    {
        int iReturnResult = -1;
        try
        {
            String strNowTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string strGetToken = AccessTokenGetter.GetValidAccessToken(GetSysParams.GetWeixin_AppId(),GetSysParams.GetWeixin_Appsecret(),strNowTime,"020");
            //log.Error("获取AccessToken的请求地址为："+strReturnURL);
            log.Error("----开始发送模板消息推送");
            log.Error("当前AppId:"+GetSysParams.GetWeixin_AppId());
            log.Error("当前AppSecret:"+GetSysParams.GetWeixin_Appsecret());
            log.Error("获取AccessToken的结果为："+strGetToken);

            if (!String.IsNullOrEmpty(strGetToken))
            {
                //第一步：获取模版ID
                String strShortId = "TM00461";
                String jsonPostData = "{\"template_id_short\":\""+strShortId+"\"}";
                String strTemplateId_ReturnJson = HttpRequestHelper.RequestPostData(string.Format(Const.Weixin_URL_GetTemplateId, strGetToken),jsonPostData);
                String strTemplateId = JsonHelper.GetJsonValue(strTemplateId_ReturnJson, "template_id");
                log.Error("使用模板为:"+strShortId);
                log.Error("获取模板id接口返回值为:"+strTemplateId_ReturnJson);
                log.Error("使用模板的TemplateId为:"+strTemplateId);

                ////第二步：发送模板消息
                ////发送文本消息
                String toUser1 = "oKw3jvmsKp0xRFZSgqYeq_zIJ7w4";
                String toUser2="oKw3jvs6QW7UDfFTubeIHkCqgv8Y";
                //String content="【VP平台群发消息测试】我曾经跨过山和大海，也穿过人山人海。。。";
                StringBuilder sBuilderJson = new StringBuilder();
                sBuilderJson.Append("{");
                //sBuilderJson.Append("	\"touser\": [\"" + toUser1 + "\", \""+toUser2+"\"],");
                sBuilderJson.Append("	\"touser\": \"" + toUser1 + "\",");
                sBuilderJson.Append("	\"template_id\": \""+strTemplateId+"\",");
                sBuilderJson.Append("	\"url\": \"www.baidu.com\",");
                sBuilderJson.Append("	\"data\": {");
                sBuilderJson.Append("		\"first\": {");
                sBuilderJson.Append("			\"value\":\"模板消息测试！\",");
                sBuilderJson.Append("			\"color\":\"#173177\"");
                sBuilderJson.Append("		},");
                sBuilderJson.Append("		\"keynote1\":{");
                sBuilderJson.Append("			\"value\":\"广州达宬\",");
                sBuilderJson.Append("			\"color\":\"#173177\"");
                sBuilderJson.Append("		},");
                sBuilderJson.Append("		\"keynote2\": {");
                sBuilderJson.Append("			\"value\":\"2019年9月26日\",");
                sBuilderJson.Append("			\"color\":\"#173177\"");
                sBuilderJson.Append("		},");
                sBuilderJson.Append("		\"keynote3\": {");
                sBuilderJson.Append("			\"value\":\"2\",");
                sBuilderJson.Append("			\"color\":\"#173177\"");
                sBuilderJson.Append("		},");
                sBuilderJson.Append("		\"keynote4\": {");
                sBuilderJson.Append("			\"value\":\"模板消息\",");
                sBuilderJson.Append("			\"color\":\"#173177\"");
                sBuilderJson.Append("		},");
                sBuilderJson.Append("		\"keynote5\": {");
                sBuilderJson.Append("			\"value\":\"2014年9月22日\",");
                sBuilderJson.Append("			\"color\":\"#173177\"");
                sBuilderJson.Append("		},");
                sBuilderJson.Append("		\"remark\":{");
                sBuilderJson.Append("			\"value\":\"模板消息测试！\",");
                sBuilderJson.Append("			\"color\":\"#173177\"");
                sBuilderJson.Append("		}");
                sBuilderJson.Append("	}");
                sBuilderJson.Append("}");
                String json = sBuilderJson.ToString();

                byte[] byteArray = Encoding.UTF8.GetBytes(json);
                String ps = HttpRequestHelper.RequestPostData(string.Format(Const.Weixin_URL_SendTemplateMessage, strGetToken), json);

                String strMsg_id = JsonHelper.GetJsonValue(ps, "msg_id");
                String strErrcode = JsonHelper.GetJsonValue(ps, "errcode");
                log.Error("发送模板消息测试对象OpenId：" + toUser1);
                log.Error("发送模板消息测试数据：" + json);
                log.Error("发送模板消息测试返回值：" + ps);
                log.Error("发送模板消息测试返回值中的msg_id：" + strMsg_id);
                log.Error("发送模板消息测试返回值中的errcode：" + strErrcode);
                iReturnResult = 1;
            }
            else
            {
                iReturnResult = -19;
            }
        }
        catch (Exception ex)
        {
            log.Error("发送模板消息测试失败\r\n");
            log.Error("错误信息："+ex.ToString());
            iReturnResult = -9;
        }
        finally
        {
            context.Response.Write(iReturnResult.ToString());
        }
    }

    /// <summary>
    /// 群发推送【测试用】
    /// </summary>
    /// <param name="context"></param>
    private void MassMessage(HttpContext context)
    {
        int iReturnResult = -1;
        try
        {
            //string strGetToken = "25_D4E2BX532tj7oOJJqyUn0LYNgAoHNfozDBue8OuGnsWWIljiDsg5y5ddEjmpxvlDs_91k-Tex-sVllyhytZx22xvWytvfUDmBvzbPV_mYOS3ITg7mYdwKUx82n7FW2Df5qdcYOj_-5iXrxr3ONDfACATSI";

            String strNowTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string strGetToken = AccessTokenGetter.GetValidAccessToken(GetSysParams.GetWeixin_AppId(),GetSysParams.GetWeixin_Appsecret(),strNowTime,"020");
            //log.Error("获取AccessToken的请求地址为："+strReturnURL);
            log.Error("当前AppId:"+GetSysParams.GetWeixin_AppId());
            log.Error("当前AppSecret:"+GetSysParams.GetWeixin_Appsecret());
            log.Error("获取AccessToken的结果为："+strGetToken);

            if (!String.IsNullOrEmpty(strGetToken))
            {
                //发送文本消息
                String toUser1="oKw3jvmsKp0xRFZSgqYeq_zIJ7w4";
                String toUser2="oKw3jvs6QW7UDfFTubeIHkCqgv8Y";
                String content="【VP平台群发消息测试】我曾经跨过山和大海，也穿过人山人海。。。";
                StringBuilder sBuilderJson = new StringBuilder();
                sBuilderJson.Append("{");
                sBuilderJson.Append("	\"touser\": [\""+toUser1+"\", \""+toUser2+"\"],");
                sBuilderJson.Append("	\"msgtype\": \"text\",");
                sBuilderJson.Append("	\"text\": {");
                sBuilderJson.Append("		\"content\": \""+content+"\"");
                sBuilderJson.Append("	}");
                sBuilderJson.Append("}");
                String json = sBuilderJson.ToString();

                byte[] byteArray = Encoding.UTF8.GetBytes(json);
                String ps = HttpRequestHelper.RequestPostData(string.Format(Const.Weixin_URL_SendMassMessage, strGetToken), json);

                String strMsg_id = JsonHelper.GetJsonValue(ps, "msg_id");
                String strErrcode = JsonHelper.GetJsonValue(ps, "errcode");
                log.Error("群发公众号信息测试对象OpenId："+toUser1+";"+toUser2);
                log.Error("群发公众号信息测试数据："+json);
                log.Error("群发公众号信息测试返回值："+ps);
                log.Error("群发公众号信息测试返回值中的msg_id："+strMsg_id);
                log.Error("群发公众号信息测试返回值中的errcode："+strErrcode);
                iReturnResult = 1;
            }
            else
            {
                iReturnResult = -19;
            }
        }
        catch (Exception ex)
        {
            log.Error("群发公众号信息测试失败\r\n");
            log.Error("错误信息："+ex.ToString());
            iReturnResult = -9;
        }
        finally
        {
            context.Response.Write(iReturnResult.ToString());
        }
    }


    public bool IsReusable {
        get {
            return false;
        }
    }

}