<%@ WebHandler Language="C#" Class="PushTestServer" %>

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

public class PushTestServer : HandlerBase, IHttpHandler, System.Web.SessionState.IRequiresSessionState {

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
        if (strParam.Equals("pushtemplatetestdata"))
        {
            PushTemplateTestData(context);
        }
    }

    /// <summary>
    /// 根据某模版推送消息【测试用】
    /// </summary>
    /// <param name="context"></param>
    private void PushTemplateTestData(HttpContext context)
    {
        int iReturnResult = -1;
        StringBuilder sbResultData = new StringBuilder();
        sbResultData.Append("{");
        try
        {
            //string strGetToken = "25_D4E2BX532tj7oOJJqyUn0LYNgAoHNfozDBue8OuGnsWWIljiDsg5y5ddEjmpxvlDs_91k-Tex-sVllyhytZx22xvWytvfUDmBvzbPV_mYOS3ITg7mYdwKUx82n7FW2Df5qdcYOj_-5iXrxr3ONDfACATSI";

            String strNowTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            String strAppId = GetSysParams.GetWeixin_AppId();
            String strAppsecret = GetSysParams.GetWeixin_Appsecret();
            string strGetToken = AccessTokenGetter.GetValidAccessToken(strAppId,strAppsecret,strNowTime,"050");
            //log.Error("获取AccessToken的请求地址为："+strReturnURL);
            log.Error("【PushTest.html.PushTemplateTestData测试模版消息推送】当前AppId:"+strAppId);
            log.Error("【PushTest.html.PushTemplateTestData测试模版消息推送】当前AppSecret:"+strAppsecret);
            log.Error("【PushTest.html.PushTemplateTestData测试模版消息推送】获取AccessToken的结果为："+strGetToken);

            sbResultData.Append("	\"appId\": \"" + strAppId + "\"");
            sbResultData.Append("	,\"appSecret\": \"" + strAppsecret + "\"");
            sbResultData.Append("	,\"accessToken\": \"" + strGetToken + "\"");
            if (!String.IsNullOrEmpty(strGetToken))
            {
                //发送文本消息
                String strOpenId1="oKw3jvmsKp0xRFZSgqYeq_zIJ7w4";
                String strTemplateId = "jkoU5U4FZDEkPbKQ2WFFiH3uHtn-YgHPm3IOB2cZ43Q";
                String strToUrl = "https://mobile.compassolution.com/#/pages/CS/wx/wxAuth?entry=hrRedirect";
                String strKeyWord1 = "加班流程审批";
                String strKeyWord2 = "客房部";
                String strKeyWord3 = "OT2024042600001";
                String strKeyWord4 = "换调休加班";
                String strKeyWord5 = "总经理审批";
                StringBuilder sBuilderJson = new StringBuilder();
                sBuilderJson.Append("{");
                sBuilderJson.Append("	\"touser\": \"" + strOpenId1 + "\",");//接收者openid
                sBuilderJson.Append("	\"template_id\": \""+strTemplateId+"\",");//模板ID
                sBuilderJson.Append("	\"url\": \""+strToUrl+"\",");//模板跳转链接（海外账号没有跳转能力）
                sBuilderJson.Append("	\"client_msg_id\": \"\",");//防重入id。对于同一个openid + client_msg_id, 只发送一条消息,10分钟有效,超过10分钟不保证效果。若无防重入需求，可不填
                sBuilderJson.Append("	\"data\": {");
                sBuilderJson.Append("		\"thing2\":{");
                sBuilderJson.Append("			\"value\":\""+strKeyWord1+"\"");
                sBuilderJson.Append("		},");
                sBuilderJson.Append("		\"thing23\": {");
                sBuilderJson.Append("			\"value\":\""+strKeyWord2+"\"");
                sBuilderJson.Append("		},");
                sBuilderJson.Append("		\"character_string6\": {");
                sBuilderJson.Append("			\"value\":\""+strKeyWord3+"\"");
                sBuilderJson.Append("		},");
                sBuilderJson.Append("		\"thing7\": {");
                sBuilderJson.Append("			\"value\":\""+strKeyWord4+"\"");
                sBuilderJson.Append("		},");
                sBuilderJson.Append("		\"thing10\": {");
                sBuilderJson.Append("			\"value\":\""+strKeyWord5+"\"");
                sBuilderJson.Append("		}");
                sBuilderJson.Append("	}");
                sBuilderJson.Append("}");
                String json = sBuilderJson.ToString();


                byte[] byteArray = Encoding.UTF8.GetBytes(json);
                String ps = HttpRequestHelper.RequestPostData(string.Format(Const.Weixin_URL_SendTemplateMessage, strGetToken), json);

                String strMsg_id = JsonHelper.GetJsonValue(ps, "msg_id");
                String strErrcode = JsonHelper.GetJsonValue(ps, "errcode");
                log.Error("【PushTest.html.PushTemplateTestData测试模版消息推送】对象OpenId："+strOpenId1);
                log.Error("【PushTest.html.PushTemplateTestData测试模版消息推送】数据："+json);
                log.Error("【PushTest.html.PushTemplateTestData测试模版消息推送】返回值："+ps);
                log.Error("【PushTest.html.PushTemplateTestData测试模版消息推送】返回值中的msg_id："+strMsg_id);
                log.Error("【PushTest.html.PushTemplateTestData测试模版消息推送】返回值中的errcode："+strErrcode);
                iReturnResult = 1;

                sbResultData.Append("	,\"pushContent\": " + json + "");
                sbResultData.Append("	,\"pushResult\": " + ps + "");

            }
            else
            {
                iReturnResult = -19;
            }
        }
        catch (Exception ex)
        {
            log.Error("【PushTest.html.PushTemplateTestData测试模版消息推送】失败\r\n");
            log.Error("【PushTest.html.PushTemplateTestData测试模版消息推送】错误信息："+ex.ToString());
            iReturnResult = -9;
        }
        finally
        {
            sbResultData.Append("}");
            //log.Error("【PushTest.html.PushTemplateTestData测试模版消息推送】sbResultData："+sbResultData.ToString());
            context.Response.Write(sbResultData.ToString());
        }
    }


    public bool IsReusable {
        get {
            return false;
        }
    }

}