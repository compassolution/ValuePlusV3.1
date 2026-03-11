<%@ WebHandler Language="C#" Class="PushMessage" %>

using System;
using System.Web;
using System.Text;
using System.Data;
using System.IO;
using System.Collections;
using Com.ValuePlus.Web;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Weixin;
using Com.ValuePlus.Utils.Serializable;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Com.ValuePlus.Common.Security;

public class PushMessage : HandlerBase, IHttpHandler, System.Web.SessionState.IRequiresSessionState {

    /// <summary>
    /// 微信端获取推送消息的处理类
    /// </summary>
    /// <param name="context"></param>
    public void ProcessRequest (HttpContext context) {
        Hashtable hsTableUrlQuery = this.GetUrlAnalyse(context);
        string strParam = context.Request["param"] == null ? string.Empty : context.Request["param"].ToString();//请求类型参数
        string strLanguage = context.Request["language"] == null ? string.Empty : context.Request["language"].ToString();//请求类型参数
        string strMobileNo = context.Request["mobileno"] == null ? string.Empty : context.Request["mobileno"].ToString();//请求类型参数
        string strProjectId = context.Request["projectid"] == null ? string.Empty : context.Request["projectid"].ToString();//请求类型参数
        string strMsgCode = context.Request["msgcode"] == null ? string.Empty : context.Request["msgcode"].ToString();//请求类型参数
        
        //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
        strParam = SQLInjectionDefense.ReplaceSQLReservedKeyword(strParam);
        strLanguage = SQLInjectionDefense.ReplaceSQLReservedKeyword(strLanguage);
        strMobileNo = SQLInjectionDefense.ReplaceSQLReservedKeyword(strMobileNo);
        strProjectId = SQLInjectionDefense.ReplaceSQLReservedKeyword(strProjectId);
        strMsgCode = SQLInjectionDefense.ReplaceSQLReservedKeyword(strMsgCode);

        log.Error("PushMessage.ashx,MobileNo:"+strMobileNo+";strMsgCode:"+strMsgCode);
        if (strParam.Equals("getmsglist"))
        {
            //this.GetPushMessageList(context,strMobileNo,strProjectId);
            this.GetPushMessageDetail(context,strMobileNo,strProjectId,strMsgCode);
        }
        else if (strParam.Equals("getmsgdetail"))
        {
            this.GetPushMessageDetail(context,strMobileNo,strProjectId,strMsgCode);
        }
    }

    /// <summary>
    /// 根据消息编码获取推送消息明细信息
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strMobileNo"></param>
    /// <param name="strProjectId"></param>
    /// <param name="strMsgCode"></param>
    private void GetPushMessageDetail(HttpContext context,String strMobileNo,String strProjectId,String strMsgCode)
    {
        try
        {
            StringBuilder sbSql1 = new StringBuilder();
            StringBuilder sbSql4 = new StringBuilder();

            sbSql1.Append("select * from [WXPushMessage_1] where MSGCODE = ' "+strMsgCode+"'");
            sbSql4.Append("select * from [WXPushMessage_4] where MSGCODE = ' "+strMsgCode+"' order by SEQNO DESC");

            String strSql = sbSql1.ToString();
            log.Error("根据消息编码获取推送消息明细信息:"+sbSql1.ToString());

            DataTable dt1 = SqlParamDao.GetDataTableBySql(sbSql1.ToString());
            DataTable dt4 = SqlParamDao.GetDataTableBySql(sbSql4.ToString());
            int iColCount = dt1.Columns.Count;

            StringBuilder sBuilder = new StringBuilder();
            sBuilder.Append("{");
            sBuilder.Append(WebCommon.GetJsonStringByDataTable(dt1,"ResultData_Main",true));
            sBuilder.Append(",");
            sBuilder.Append(WebCommon.GetJsonStringByDataTable(dt4,"ResultData_Reply",true));

            sBuilder.Append("}");

            string json = sBuilder.ToString();

            context.Response.Write(json);

        }
        catch (Exception ex)
        {
            log.Error(ex);
            context.Response.Write("-1");
        }
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}