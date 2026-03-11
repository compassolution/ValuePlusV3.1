<%@ WebHandler Language="C#" Class="ServerHandler" %>

using System;
using System.Web;
using System.Text;
using System.Data;
using System.IO;
using System.Collections;
using Com.ValuePlus.Web;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Utils.Serializable;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Com.ValuePlus.Weixin;
using Com.ValuePlus.Common.Security;

public class ServerHandler : HandlerBase, IHttpHandler, System.Web.SessionState.IRequiresSessionState {

    public void ProcessRequest (HttpContext context) {
        //跨域提交表单，前端ajax不用做任何修改
        context.Response.AddHeader("Access-Control-Allow-Origin", "*");//支持全域名访问，不安全，部署后需要固定限制为客户端网址

        Hashtable hsTableUrlQuery = this.GetUrlAnalyse(context);
        string strCallback = hsTableUrlQuery["callback"] == null ? string.Empty : hsTableUrlQuery["callback"].ToString();//param
        string strParam = hsTableUrlQuery["param"] == null ? string.Empty : hsTableUrlQuery["param"].ToString();//param
        string strProjectId = hsTableUrlQuery["projectid"] == null ? string.Empty : hsTableUrlQuery["projectid"].ToString();//param
            
        //log.Error("(UpdateServerHandler)"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"---Param:"+strParam);
        if (strParam.Equals("uploadlicensedata"))
        {
            //上传Lic数据到远程服务器
            this.DoUpdateLicenseToRemote(context, strProjectId);
        }
    }

    /// <summary>
    /// 上传Lic数据到远程服务器
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strProjectId"></param>i
    private void DoUpdateLicenseToRemote(HttpContext context,String strProjectId)
    {
        int iReturnResult = -1;
        try
        {
            StringBuilder sbSqlInsert = new StringBuilder();
            String strLicenseData = context.Request["txt_LicenseData"].ToString();
            //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
            strLicenseData = SQLInjectionDefense.ReplaceSQLReservedKeyword(strLicenseData);
            JObject jsonObject= (JObject)JsonConvert.DeserializeObject(strLicenseData);
            log.Error("上传Lic数据到远程服务器 DoUpdateLicenseToRemote(ProjectId:"+strProjectId+")"+strLicenseData);
            
            String strLICENSECODE = Microsoft.JScript.GlobalObject.unescape(jsonObject["LICENSECODE"].ToString()).Replace("'","''");
            String strCLIENTNAME = Microsoft.JScript.GlobalObject.unescape(jsonObject["CLIENTNAME"].ToString()).Replace("'","''");
            String strCLIENTNAMECHS = Microsoft.JScript.GlobalObject.unescape(jsonObject["CLIENTNAMECHS"].ToString()).Replace("'","''");
            String strPRODUCTORNAME = Microsoft.JScript.GlobalObject.unescape(jsonObject["PRODUCTORNAME"].ToString()).Replace("'","''");
            String strVERSION = Microsoft.JScript.GlobalObject.unescape(jsonObject["VERSION"].ToString()).Replace("'","''");
            String strBOUNDCODE = Microsoft.JScript.GlobalObject.unescape(jsonObject["BOUNDCODE"].ToString()).Replace("'","''");
            String strVALIDDATE = Microsoft.JScript.GlobalObject.unescape(jsonObject["VALIDDATE"].ToString()).Replace("'","''");
            String strMACHINECODE = Microsoft.JScript.GlobalObject.unescape(jsonObject["MACHINECODE"].ToString()).Replace("'","''");
            String strLASTUSERID = Microsoft.JScript.GlobalObject.unescape(jsonObject["LASTUSERID"].ToString()).Replace("'","''");
            String strLASTTIME = Microsoft.JScript.GlobalObject.unescape(jsonObject["LASTTIME"].ToString()).Replace("'","''");
            
            sbSqlInsert.Append("UPDATE PROJECT_1 SET LICVALIDDATE = '"+strVALIDDATE+"' where ProjectId = '"+strProjectId+"'; \r\n");
            sbSqlInsert.Append("DELETE FROM PROJECT_4 where [PRONO] in (select [PRONO] from PROJECT_1 where ProjectId = '"+strProjectId+"'); \r\n");
            sbSqlInsert.Append("INSERT INTO [PROJECT_4]([PRONO],[VALIDDATE],[CLIENTNAME],[CLIENTNAMECHS],[PRODUCTORNAME],[BOUNDCODE],[VERSION],[MACHINECODE],[LASTUSERID],[LASTTIME]) \r\n");
            sbSqlInsert.Append("SELECT PRONO,'"+strVALIDDATE+"','"+strCLIENTNAME+"','"+strCLIENTNAMECHS+"','"+strPRODUCTORNAME+"','"+strBOUNDCODE+"' \r\n");
            sbSqlInsert.Append(",'"+strVERSION+"','"+strMACHINECODE+"','"+strLASTUSERID+"','"+strLASTTIME+"' FROM [PROJECT_1] WHERE ProjectId = '"+strProjectId+"' \r\n");

            String strInsertSql = sbSqlInsert.ToString();
            if (!String.IsNullOrEmpty(strInsertSql))
            {
                try
                {
                    int iReturnCount = SqlParamDao.ExecuteNonQueryBySql(strInsertSql);
                    if (iReturnCount > 0)
                    {
                        iReturnResult = iReturnCount;
                    }
                }
                catch (Exception ex)
                {
                    log.Error("上传Lic数据到远程服务器出错，SQL语句执行失败\r\n");
                    log.Error("SQL语句:"+strInsertSql+"\r\n");
                    log.Error("错误信息："+ex.ToString());
                }
            }
        }
        catch (Exception ex)
        {
            log.Error("上传Lic数据到远程服务器出错，解析薪资数据json失败\r\n");
            log.Error("错误信息："+ex.ToString());
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