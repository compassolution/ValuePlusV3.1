<%@ WebHandler Language="C#" Class="DataSyncIndex" %>

using System;
using System.Web;
using System.Text;
using System.Data;
using System.IO;
using System.Collections;
using Com.ValuePlus.Web;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Utils.Serializable;
using Com.ValuePlus.Common.Security;
using Com.ValuePlus.Archive.Admin;

public class DataSyncIndex : HandlerBase, IHttpHandler, System.Web.SessionState.IRequiresSessionState {

    public void ProcessRequest (HttpContext context) {
        Hashtable hsTableUrlQuery = this.GetUrlAnalyse(context);
        string strCallback = hsTableUrlQuery["callback"] == null ? string.Empty : hsTableUrlQuery["callback"].ToString();//param
        string strParam = hsTableUrlQuery["param"] == null ? string.Empty : hsTableUrlQuery["param"].ToString();//param
        string strDSCode = hsTableUrlQuery["dscode"] == null ? string.Empty : hsTableUrlQuery["dscode"].ToString();//param
        string strUploadOrDownload = hsTableUrlQuery["uploadordownload"] == null ? string.Empty : hsTableUrlQuery["uploadordownload"].ToString();//uploadordownload

        log.Error("(DataSyncIndex)"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"---Param:"+strParam);

        if (strParam.Equals("getclientinfo"))
        {
            this.GetCurClientInfo(context);
        }
        else if (strParam.Equals("getarchivedata_oadatasync"))
        {
            //获取模版OADataSync的配置数据及表结构及表数据
            this.GetArchiveData_OADataSync(context);
        }
        else if (strParam.Equals("getoadatasync1configdata"))
        {
            //获取OADataSync_1的配置数据
            this.GetOADataSync1ConfigData(context);
        }
        else if (strParam.Equals("getoadatasync2configdata"))
        {
            //获取OADataSync_2的配置数据
            this.GetOADataSync2ConfigData(context,strDSCode);
        }
        else if (strParam.Equals("updateoadatasynclastoperation"))
        {
            //更新OADataSync_1表中的最后插入时间及用户信息
            this.UpdateOADataSyncLastOperation(context,strDSCode,strUploadOrDownload);
        }

    }

    /// <summary>
    /// 获取当前客户信息
    /// </summary>
    /// <returns></returns>
    public void GetCurClientInfo(HttpContext context)
    {
        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select (select top 1 paramValue from BASICPARAM_1 WHERE paramName = 'ProjectId') as ProjectId");
            sbSql.Append(",(select top 1 SMENUNAMECN from TB_HR_MENU WHERE SMENUCODE = 'ROOT') as ClientName");
            sbSql.Append(",(select top 1 paramValue from BASICPARAM_1 WHERE paramName = 'SysUpdateServer') as SysUpdateServer");
            sbSql.Append(",(select top 1 paramValue from BASICPARAM_1 WHERE paramName = 'OADataSyncOuterServer') as OADataSyncOuterServer");
            sbSql.Append(",(select top 1 paramValue from BASICPARAM_1 WHERE paramName = 'IsOuterServer_OADataSync') as IsOuterServer_OADataSync");

            DataTable dt = SqlParamDao.GetDataTableBySql(sbSql.ToString());

            StringBuilder sBuilder_ClientInfo = new StringBuilder();
            sBuilder_ClientInfo.Append("[ ");
            sBuilder_ClientInfo.Append("{");
            if ((dt != null) && (dt.Rows.Count > 0))
            {
                sBuilder_ClientInfo.Append("\"ProjectId\":" + "\"" + Microsoft.JScript.GlobalObject.escape( dt.Rows[0]["ProjectId"].ToString()) + "\"");
            }
            if ((dt != null) && (dt.Rows.Count > 0))
            {
                sBuilder_ClientInfo.Append(",\"ClientName\":" + "\"" + Microsoft.JScript.GlobalObject.escape(dt.Rows[0]["ClientName"].ToString()) + "\"");
            }
            if ((dt != null) && (dt.Rows.Count > 0))
            {
                sBuilder_ClientInfo.Append(",\"RemoteServer\":" + "\"" + Microsoft.JScript.GlobalObject.escape(dt.Rows[0]["SysUpdateServer"].ToString()) + "\"");
            }
            if ((dt != null) && (dt.Rows.Count > 0))
            {
                sBuilder_ClientInfo.Append(",\"OADataSyncOuterServer\":" + "\"" + Microsoft.JScript.GlobalObject.escape(dt.Rows[0]["OADataSyncOuterServer"].ToString()) + "\"");
            }
            if ((dt != null) && (dt.Rows.Count > 0))
            {
                sBuilder_ClientInfo.Append(",\"IsOuterServer_OADataSync\":" + "\"" + Microsoft.JScript.GlobalObject.escape(dt.Rows[0]["IsOuterServer_OADataSync"].ToString()) + "\"");
            }
            sBuilder_ClientInfo.Append(",\"UserId\":" + "\"" + this.GetUserCode() + "\"");
            sBuilder_ClientInfo.Append("}");
            sBuilder_ClientInfo.Append("]");
            String strClientInfo = sBuilder_ClientInfo.ToString();

            context.Response.Write(strClientInfo);
        }
        catch (Exception ex)
        {
            log.Error(ex);
            context.Response.Write("");
        }
    }

    /// <summary>
    /// 获取模版OADataSync的配置数据及表结构及表数据
    /// </summary>
    /// <param name="context"></param>
    private void GetArchiveData_OADataSync(HttpContext context)
    {
        try
        {
            String strEnter = "\r\n";
            StringBuilder sbExportSql = new StringBuilder();
            ExportArchiveConfig exportArchiveConfig = new ExportArchiveConfig();
            String strTID = "OADataSync";
            //第一步：获取删除配置的语句
            sbExportSql.Append(exportArchiveConfig.GetClearArchiveConfigSql(strTID)+strEnter+strEnter);
            //第二步：获取配置内容的语句
            sbExportSql.Append(exportArchiveConfig.GetArchiveConfigExportSql(strTID,false)+strEnter+strEnter);
            //第三步：获取创建实体业务表结构的语句
            sbExportSql.Append(exportArchiveConfig.GetTableDropAndCreateSql("OADataSync_1")+strEnter+strEnter);
            sbExportSql.Append(exportArchiveConfig.GetTableDropAndCreateSql("OADataSync_2")+strEnter+strEnter);
            //第四步：获取实体业务表数据的语句
            sbExportSql.Append(exportArchiveConfig.ExportTableData("OADataSync_1")+strEnter+strEnter);
            sbExportSql.Append(exportArchiveConfig.ExportTableData("OADataSync_2")+strEnter+strEnter);

            string json = sbExportSql.ToString();

            context.Response.Write(json);

        }
        catch (Exception ex)
        {
            log.Error(ex);
            context.Response.Write("");
        }
    }

    /// <summary>
    /// 获取OA数据同步配置主表信息【OADataSync_1】
    /// </summary>
    /// <param name="context"></param>
    private void GetOADataSync1ConfigData(HttpContext context)
    {
        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select ROW_NUMBER() over(order by DSORDER) AS ID, * from OADataSync_1 where BISVALID = '1' order by DSORDER");
            DataTable dt = SqlParamDao.GetDataTableBySql(sbSql.ToString());
            StringBuilder sBuilder = new StringBuilder();
            sBuilder.Append("{");
            sBuilder.Append(WebCommon.GetJsonStringByDataTable(dt,"\"ResultData\"",false));
            sBuilder.Append("}");

            string json = sBuilder.ToString();

            context.Response.Write(json);

        }
        catch (Exception ex)
        {
            log.Error(ex);
            context.Response.Write("");
        }
    }

    /// <summary>
    /// 根据同步编码获取OA数据同步配置明细表信息【OADataSync_2】
    /// </summary>
    /// <param name="strDSCode"></param>
    /// <param name="context"></param>
    private void GetOADataSync2ConfigData(HttpContext context,String strDSCode)
    {
        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select * from OADataSync_2 where DSCODE = '"+strDSCode+"' AND BISVALID = '1' ORDER BY SEQNO");
            DataTable dt = SqlParamDao.GetDataTableBySql(sbSql.ToString());
            StringBuilder sBuilder = new StringBuilder();
            sBuilder.Append("{");
            sBuilder.Append(WebCommon.GetJsonStringByDataTable(dt,"\"OADataSync_2\"",true));
            sBuilder.Append("}");

            string json = sBuilder.ToString();
            context.Response.Write(json);

        }
        catch (Exception ex)
        {
            log.Error(ex);
            context.Response.Write("");
        }
    }

    /// <summary>
    /// 更新OADataSync_1表中的最后插入时间及用户信息
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strDSCode"></param>
    /// <param name="strUploadOrDownload"></param>
    private void UpdateOADataSyncLastOperation(HttpContext context,String strDSCode,String strUploadOrDownload)
    {
        try
        {
            StringBuilder sbSql = new StringBuilder();
            String strCurTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            String strUserId = this.GetUserCode();
            if(strUploadOrDownload.ToLower().Equals("upload"))
            {
                sbSql.Append("UPDATE OADataSync_1 SET LASTUPLOADTIME = '"+strCurTime+"',LASTUPLOADUSER = '"+strUserId+"' WHERE DSCODE = '"+strDSCode+"'");
            }else if(strUploadOrDownload.ToLower().Equals("download"))
            {
                sbSql.Append("UPDATE OADataSync_1 SET LASTDOWNTIME = '"+strCurTime+"',LASTDOWNUSER = '"+strUserId+"' WHERE DSCODE = '"+strDSCode+"'");
            }
            int iCount = SqlParamDao.ExecuteNonQueryBySql(sbSql.ToString());

            context.Response.Write(iCount);

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