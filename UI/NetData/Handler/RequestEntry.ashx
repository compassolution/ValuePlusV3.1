<%@ WebHandler Language="C#" Class="RequestEntry" %>

using System;
using System.Web;
using Com.ValuePlus.Common.Security;

public class RequestEntry : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        this.InterfaceTest();
    }

    public void InterfaceTest()
    {
        string token = "WeiXinNetData";
        if (string.IsNullOrEmpty(token))
        {
            return;
        }
        string echoString = HttpContext.Current.Request.QueryString["echoStr"];
        string signature = HttpContext.Current.Request.QueryString["signature"];
        string timestamp = HttpContext.Current.Request.QueryString["timestamp"];
        string nonce = HttpContext.Current.Request.QueryString["nonce"];
        
        //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
        echoString = SQLInjectionDefense.ReplaceSQLReservedKeyword(echoString);
        signature = SQLInjectionDefense.ReplaceSQLReservedKeyword(signature);
        timestamp = SQLInjectionDefense.ReplaceSQLReservedKeyword(timestamp);
        nonce = SQLInjectionDefense.ReplaceSQLReservedKeyword(nonce);

        if (!string.IsNullOrEmpty(echoString))
        {
            HttpContext.Current.Response.Write(echoString);
            HttpContext.Current.Response.End();
        }
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}