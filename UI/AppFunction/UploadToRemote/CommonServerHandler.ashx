<%@ WebHandler Language="C#" Class="CommonServerHandler" %>

using System;
using System.Web;
using System.Text;
using System.Net;
using System.Data;
using System.Collections;
using Com.ValuePlus.Web;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Weixin;

public class CommonServerHandler : HandlerBase, IHttpHandler, System.Web.SessionState.IRequiresSessionState {

    public void ProcessRequest (HttpContext context) {
        //跨域提交表单，前端ajax不用做任何修改
        context.Response.AddHeader("Access-Control-Allow-Origin", "*");//支持全域名访问，不安全，部署后需要固定限制为客户端网址

        Hashtable hsTableUrlQuery = this.GetUrlAnalyse(context);
        string strCallback = hsTableUrlQuery["callback"] == null ? string.Empty : hsTableUrlQuery["callback"].ToString();//param
        string strParam = hsTableUrlQuery["param"] == null ? string.Empty : hsTableUrlQuery["param"].ToString();//param
        string strProjectId = hsTableUrlQuery["projectid"] == null ? string.Empty : hsTableUrlQuery["projectid"].ToString();//param
        string strWXMPModule = hsTableUrlQuery["wxmpmodule"] == null ? string.Empty : hsTableUrlQuery["wxmpmodule"].ToString();//param

        //log.Error("(UpdateServerHandler)"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"---Param:"+strParam);
        if (strParam.Equals("wxauthisvalid"))
        {
            context.Response.Write(ProjectWXAuth.JudgeWXAuthIsValidByProjectId(strProjectId,strWXMPModule,"").ToString().ToLower());
        }else if (strParam.Equals("getmessagetypeinfo"))
        {
            GetMessageTypeInfo(context);
        } 
    }

    /// <summary>
    /// 获取推送消息类型数据
    /// </summary>
    /// <returns></returns>
    public void GetMessageTypeInfo(HttpContext context)
    {
        try
        {
            StringBuilder sBuilder = new StringBuilder();
            sBuilder.Append("{");
            String strSql_MessageType = "select * from TB_HRLSTD WHERE LID = 'WXPushMessageType' and isnull(BISSTOP,'2')<> '1' order by isnull(P9,CID)";
            DataTable dt_MessageType = SqlParamDao.GetDataTableBySql(strSql_MessageType);
            String strClientInfo = WebCommon.GetJsonStringByDataTable(dt_MessageType,"MessageTypeInfo",true);
             
            sBuilder.Append(strClientInfo);   
            sBuilder.Append("}");

            string json = sBuilder.ToString();

            context.Response.Write(json);
        }
        catch (Exception ex)
        {
            log.Error(ex);
            context.Response.Write("error");
        }
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}