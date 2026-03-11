<%@ WebHandler Language="C#" Class="ClientHandler" %>

using System;
using System.Web;
using System.Text;
using System.Data;
using System.IO;
using System.Collections;
using Com.ValuePlus.Web;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Utils.Serializable;

public class ClientHandler : HandlerBase, IHttpHandler, System.Web.SessionState.IRequiresSessionState {

    public void ProcessRequest (HttpContext context) {
        Hashtable hsTableUrlQuery = this.GetUrlAnalyse(context);
        string strCallback = hsTableUrlQuery["callback"] == null ? string.Empty : hsTableUrlQuery["callback"].ToString();//param
        string strParam = hsTableUrlQuery["param"] == null ? string.Empty : hsTableUrlQuery["param"].ToString();//param
        string strYearMonthType = hsTableUrlQuery["yearmonthtype"] == null ? string.Empty : hsTableUrlQuery["yearmonthtype"].ToString();//param
        string strProjectId = hsTableUrlQuery["projectid"] == null ? string.Empty : hsTableUrlQuery["projectid"].ToString();//param
        string strIsOnJob = hsTableUrlQuery["isonjob"] == null ? string.Empty : hsTableUrlQuery["isonjob"].ToString();//params
            
        log.Error("(AppFunction/UploadToRemote/PushMessage/ClientHandler.ashx)"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"---Param:"+strParam);
        if (strParam.Equals("getclientinfo"))
        {
            this.GetClientInfo(context);
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
            //String strClientInfo = GetCurClientInfo();
            sBuilder.Append(GetCurClientInfo());
            sBuilder.Append(","+GetStaffMobileList("1"));

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
                sBuilder_ClientInfo.Append(",UserId:" + "'" + this.GetUserCode() + "'");
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


    /// <summary>
    /// 获取员工列表
    /// </summary>
    /// <param name="strIsOnJob"></param>
    private String GetStaffMobileList(String strIsOnJob)
    {
        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select A.* from VW_WX_StaffMobileRegistList A INNER JOIN HRDOCU_1 B ON A.员工编号 = B.DCNO where 1=1 ");
            //sbSql.Append(" and [是否已注册] = '已注册'");
            if (strIsOnJob.Equals("1"))
            {
                sbSql.Append(" and B.DCSTATUS NOT IN ('3')");
            }else if (strIsOnJob.Equals("2"))
            {
                sbSql.Append(" and B.DCSTATUS IN ('3')");
            }
            sbSql.Append(" order by 序号");
            log.Error("获取员工列表:"+sbSql.ToString());
            DataTable dt = SqlParamDao.GetDataTableBySql(sbSql.ToString());
            String strStaffMobileListInfo = WebCommon.GetJsonStringByDataTable(dt,"StaffMobileListInfo",true);

            return strStaffMobileListInfo;

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