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
using Com.ValuePlus.Common.Security;

public class ClientHandler : HandlerBase, IHttpHandler, System.Web.SessionState.IRequiresSessionState {

    public void ProcessRequest (HttpContext context) {
        Hashtable hsTableUrlQuery = this.GetUrlAnalyse(context);
        string strCallback = hsTableUrlQuery["callback"] == null ? string.Empty : hsTableUrlQuery["callback"].ToString();//param
        string strParam = hsTableUrlQuery["param"] == null ? string.Empty : hsTableUrlQuery["param"].ToString();//param
        string strYearMonthType = hsTableUrlQuery["yearmonthtype"] == null ? string.Empty : hsTableUrlQuery["yearmonthtype"].ToString();//param
        string strProjectId = hsTableUrlQuery["projectid"] == null ? string.Empty : hsTableUrlQuery["projectid"].ToString();//param
        string strIsOnJob = hsTableUrlQuery["isonjob"] == null ? string.Empty : hsTableUrlQuery["isonjob"].ToString();//params


        log.Error("(AppFunction/UploadToRemote/StaffMobile/ClientHandler.ashx)"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"---Param:"+strParam);
        if (strParam.Equals("getclientinfo"))
        {
            this.GetClientInfo(context);
        }
        else if (strParam.Equals("getstaffbasicdata"))
        {
            this.GetStaffBasicData(context);
        }
        else if (strParam.Equals("getstaffmobilelist"))
        {
            this.GetStaffMobileList(context,strIsOnJob);
        }
        else if (strParam.Equals("saveverifymoblielist"))
        {
            this.SaveVerifyMobileListToLocal(context);
        }
        else if (strParam.Equals("getstafflist"))
        {
            this.GetStaffInfoList(context,strIsOnJob);
        }

    }

    /// <summary>
    /// 获取档案库的员工基本数据信息
    /// 返回数据到客户端后，再传递到远程服务器端的服务
    /// </summary>
    /// <param name="context"></param>
    private void GetStaffBasicData(HttpContext context)
    {
        try
        {
            String strOpFlag = context.Request["txt_OpFlag"].ToString();
            //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
            strOpFlag = SQLInjectionDefense.ReplaceSQLReservedKeyword(strOpFlag);

            String strSQL_ExecuteSP = "USP_ToRemote_HR_GetStaffBasicData";
            String strTableName = "_"+strSQL_ExecuteSP;
            Hashtable hsTableParam = new Hashtable();
            hsTableParam.Add("Language", "zh-cn");
            hsTableParam.Add("OpFlag", strOpFlag);
            hsTableParam.Add("UserId", this.GetUserCode());
            int iReturnValue = SqlParamDao.ExcuteSP(strSQL_ExecuteSP, hsTableParam);

            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select * from ["+strTableName+"] where OpFlag = '"+strOpFlag+"'");
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
            context.Response.Write("-1");
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

    /// <summary>
    /// 从本地数据库中获取在职员工电话号码列表写入到本地数据库微信号码表中
    /// 并获取数据准备提交给远程数据库进行是否注册的验证
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strIsOnJob"></param>
    private void GetStaffMobileList(HttpContext context,String strIsOnJob)
    {
        try
        {
            String strQueryFlag = System.Guid.NewGuid().ToString();
            StringBuilder sbSql_Insert = new StringBuilder();
            sbSql_Insert.Append("delete from TB_WX_RegistMobile;");
            sbSql_Insert.Append("insert into [TB_WX_RegistMobile]([MobileNo]) select LTRIM(RTRIM(isnull(DCMOBILE,''))) as DCMOBILE from HRDOCU_1 WHERE 1=1");
            if (strIsOnJob.Equals("1"))
            {
                sbSql_Insert.Append(" and DCSTATUS NOT IN ('3')");
            }else if (strIsOnJob.Equals("2"))
            {
                sbSql_Insert.Append(" and DCSTATUS IN ('3')");
            }
            log.Error("获取在职员工电话号码列表写入微信号码表中:"+sbSql_Insert.ToString());
            int iCount = SqlParamDao.ExecuteNonQueryBySql(sbSql_Insert.ToString());

            ///获取电话号码信息
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select * from TB_WX_RegistMobile");
            String strSql = sbSql.ToString();
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
            int iColCount = dt.Columns.Count;
            StringBuilder sBuilder = new StringBuilder();
            sBuilder.Append("{");
            sBuilder.Append(WebCommon.GetJsonStringByDataTable(dt,"ResultData",true));
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

    /// <summary>
    /// 将通过远程服务器验证过的手机号码更新到本地数据库微信号码表中
    /// </summary>
    /// <param name="context"></param>
    private void SaveVerifyMobileListToLocal(HttpContext context)
    {
        try
        {
            String strQueryFlag = System.Guid.NewGuid().ToString();
            String strVerifyStaffMobileList = HttpUtility.UrlDecode(context.Request["txt_VerifyMobileData"].ToString());
            //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
            strVerifyStaffMobileList = SQLInjectionDefense.ReplaceSQLReservedKeyword(strVerifyStaffMobileList);

            StringBuilder sbSql_Insert = new StringBuilder();
            sbSql_Insert.Append("delete from TB_WX_RegistMobile;");
            String[] strArray = strVerifyStaffMobileList.Split(';');
            log.Error("将通过远程服务器验证过的手机号码更新到本地数据库微信号码表中,strVerifyStaffMobileList:"+strVerifyStaffMobileList.ToString());
            for(int i=0;i<strArray.Length; i++)
            {
                if (!String.IsNullOrEmpty(strArray[i]))
                {
                    sbSql_Insert.Append("insert into [TB_WX_RegistMobile]([MobileNo],[IsRegistedWX],[IsRegistedMPHR]) values ");
                    sbSql_Insert.Append("("+strArray[i]+")");
                }
            }
            log.Error("将通过远程服务器验证过的手机号码更新到本地数据库微信号码表中,SQL语句:"+sbSql_Insert.ToString());
            //int iCount = 1;
            int iCount = SqlParamDao.ExecuteNonQueryBySql(sbSql_Insert.ToString());

            context.Response.Write(iCount.ToString());

        }
        catch (Exception ex)
        {
            log.Error(ex);
            context.Response.Write("-1");
        }
    }

    /// <summary>
    /// 获取员工列表
    /// </summary>
    /// <param name="context"></param>
    private void GetStaffInfoList(HttpContext context,String strIsOnJob)
    {
        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select A.* from VW_WX_StaffMobileRegistList A INNER JOIN HRDOCU_1 B ON A.员工编号 = B.DCNO where 1=1 ");
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
            int iColCount = dt.Columns.Count;
            StringBuilder sBuilder = new StringBuilder();
            sBuilder.Append("{");
            sBuilder.Append(WebCommon.GetJsonStringByDataTable(dt,"ResultData",true));
            //if (!String.IsNullOrEmpty(strColNames))
            //{
            //    sBuilder.Append("," + strColNames);
            //}
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