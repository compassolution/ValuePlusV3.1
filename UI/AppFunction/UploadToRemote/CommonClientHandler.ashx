<%@ WebHandler Language="C#" Class="CommonClientHandler" %>

using System;
using System.Web;
using System.Text;
using System.Data;
using System.IO;
using System.Collections;
using Com.ValuePlus.Web;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Utils.Serializable;

public class CommonClientHandler : HandlerBase, IHttpHandler, System.Web.SessionState.IRequiresSessionState {

    public void ProcessRequest (HttpContext context) {
        Hashtable hsTableUrlQuery = this.GetUrlAnalyse(context);
        string strCallback = hsTableUrlQuery["callback"] == null ? string.Empty : hsTableUrlQuery["callback"].ToString();//param
        string strParam = hsTableUrlQuery["param"] == null ? string.Empty : hsTableUrlQuery["param"].ToString();//param
        string strProjectId = hsTableUrlQuery["projectid"] == null ? string.Empty : hsTableUrlQuery["projectid"].ToString();//param

        if (String.IsNullOrEmpty(this.GetUserCode()))
        {
            context.Response.Write("-9999");
        }else{        
            if (strParam.Equals("getclientinfo"))
            {
                this.GetClientInfo(context);
            }else{
                context.Response.Write(this.GetUserCode());
            }
        }
    }
        

    /// <summary>
    /// 获取客户基本信息
    /// </summary>
    /// <param name="context"></param>
    private void GetClientInfo(HttpContext context)
    {
        try
        {
            StringBuilder sBuilder = new StringBuilder();
            sBuilder.Append("{");
            String strClientInfo = GetCurClientInfo();
            sBuilder.Append(strClientInfo);

            sBuilder.Append("}");

            string json = sBuilder.ToString();

            context.Response.Write(json);
        }
        catch (Exception ex)
        {
            context.Response.Write("error");
            log.Error(ex);
        }
    }

    /// <summary>
    /// 获取当前客户信息
    /// </summary>
    /// <returns></returns>
    public String GetCurClientInfo()
    {
        try
        {
            String strSql_ProjectId = "select top 1 * from BASICPARAM_1 WHERE paramName = 'ProjectId'";
            String strSql_ClientName = "select top 1 * from TB_HR_MENU WHERE SMENUCODE = 'ROOT'";
            String strSql_RemoteServer = "select top 1 * from BASICPARAM_1 WHERE paramName = 'SalaryRemoteServer'";
            DataTable dt_ProjectId = SqlParamDao.GetDataTableBySql(strSql_ProjectId);
            DataTable dt_ClientName = SqlParamDao.GetDataTableBySql(strSql_ClientName);
            DataTable dt_RemoteServer = SqlParamDao.GetDataTableBySql(strSql_RemoteServer);
            StringBuilder sBuilder_ClientInfo = new StringBuilder();
            sBuilder_ClientInfo.Append("ClientInfo:[ ");
            sBuilder_ClientInfo.Append("{");
            if ((dt_ProjectId != null) && (dt_ProjectId.Rows.Count > 0))
            {
                String strParamValue = dt_ProjectId.Rows[0]["ParamValue"].ToString();
                sBuilder_ClientInfo.Append("ProjectId:" + "'" + strParamValue + "'");
            }
            if ((dt_ClientName != null) && (dt_ClientName.Rows.Count > 0))
            {
                String strClientName = dt_ClientName.Rows[0]["SMENUNAMECN"].ToString();
                sBuilder_ClientInfo.Append(",ClientName:" + "'" + strClientName + "'");
            }
            if ((dt_RemoteServer != null) && (dt_RemoteServer.Rows.Count > 0))
            {
                String strRemoteServer = dt_RemoteServer.Rows[0]["ParamValue"].ToString();
                sBuilder_ClientInfo.Append(",RemoteServer:" + "'" + strRemoteServer + "'");
            }
            sBuilder_ClientInfo.Append("}");
            sBuilder_ClientInfo.Append("]");
            String strClientInfo = sBuilder_ClientInfo.ToString();
            return strClientInfo;
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return "";
        }
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}