<%@ WebHandler Language="C#" Class="BindingLabel" %>

using System;
using System.Web;
using System.Collections;
using System.Text;
using System.IO;
using System.Data;
using Com.ValuePlus.BLL.SysManager;
using Com.ValuePlus.BLL.User;
using Com.ValuePlus.DataLog;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Entity;
using Com.ValuePlus.Web;
using Com.ValuePlus.Utils.Serializable;

public class BindingLabel : IHttpHandler {

    /// <summary>
    /// 日志声明
    /// </summary>
    protected static Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    public void ProcessRequest (HttpContext context) {
        //解析客户端传递过来的json data
        //StreamReader reader = new StreamReader(context.Request.InputStream);
        //String strParamJson = HttpUtility.UrlDecode(reader.ReadToEnd());
        //log.Error("传入参数的Json data字符串:" + strParamJson.ToString());

        //string strParam = WebCommon.GetJsonValue(strParamJson,"param").ToString();//请求类型参数

        String strUrlQueryString = context.Server.UrlDecode(context.Request.Url.Query.ToString());
        Hashtable hsTableUrlQuery = WebCommon.GetUrlAnalyse(strUrlQueryString);
        string strParam = hsTableUrlQuery["param"] == null ? string.Empty : hsTableUrlQuery["param"].ToString();//param
        string strSACODE = hsTableUrlQuery["sacode"] == null ? string.Empty : hsTableUrlQuery["sacode"].ToString();
        string strWriteEPCID = hsTableUrlQuery["epcid"] == null ? string.Empty : hsTableUrlQuery["epcid"].ToString();

        switch (strParam.ToLower().ToString())
        {
            case "getnewepcid":
                context.Response.Write(this.GetNewEPCID().ToString());
                break;
            case "bindingtodatabase":
                context.Response.Write(this.BindingToDatabase(strSACODE,strWriteEPCID).ToString());
                break;
        }
    }

    /// <summary>
    /// 获取新的EPCID
    /// </summary>
    private String GetNewEPCID()
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbReturnData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "获取新的EPCID";

        try
        {
            String strWriteEPCID_Prefix = "FA" + DateTime.Now.ToString("yyyyMMdd");//前缀为10位
            String strWriteEPCID_SEQNO = "000001";//每天6位流水号
            String strWriteEPCID = strWriteEPCID_Prefix + strWriteEPCID_SEQNO;

            StringBuilder sbSql_GetSEQNO = new StringBuilder();
            sbSql_GetSEQNO.Append("select ISNULL(right('000000'+convert(varchar(10),convert(int,RIGHT(MAX(SBARCODE),6))+1),6),'" + strWriteEPCID_SEQNO + "') as SEQNO from AMASSETS_1 ");
            sbSql_GetSEQNO.Append(" WHERE ISNULL(SBARCODE,'') <> '' AND SACODE<>ISNULL(SBARCODE,'')");
            sbSql_GetSEQNO.Append(" and LEFT(SBARCODE,10) = '" + strWriteEPCID_Prefix + "'");
            DataTable dtResutl = SqlParamDao.GetDataTableBySql(sbSql_GetSEQNO.ToString());
            if ((dtResutl != null) && (dtResutl.Rows.Count == 1))
            {
                strWriteEPCID = strWriteEPCID_Prefix + dtResutl.Rows[0]["SEQNO"].ToString();
            }
            sbReturnData.Append(strWriteEPCID);

            strReturnCode = "1";
            strReturnMsg = strMethodDesc + "成功";
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = strMethodDesc + "出错";
            log.Error(ex);
        }
        finally
        {
            StringBuilder sbResultStatus = new StringBuilder();
            sbResultStatus.Append("{");
            sbResultStatus.Append("\"ReturnCode\":\"" + strReturnCode.ToString() + "\"");
            sbResultStatus.Append(",\"ReturnMsg\":\"" + strReturnMsg.ToString() + "\"");

            sbResult.Append(sbResultStatus.ToString());
            if (!String.IsNullOrEmpty(sbReturnData.ToString()))
            {
                sbResult.Append(",\"ReturnData\":\"" + sbReturnData.ToString()+"\"");
            }
            sbResult.Append("}");
            log.Error(strMethodDesc + "Return Json:" + sbResult.ToString());
        }
        return sbResult.ToString();
    }
    
    /// <summary>
    ///绑定标签到数据库
    /// </summary>
    private String BindingToDatabase(String strSACODE,String strWriteEPCID)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbReturnData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "绑定标签到数据库";
        try
        { 
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("UPDATE AMASSETS_1 SET SBARCODE = '" + strWriteEPCID + "' WHERE SACODE = '" + strSACODE + "';");
            sbSql.Append("UPDATE AMASSETS_1 SET SBARCODE = SACODE WHERE SBARCODE = '" + strWriteEPCID + "' AND SACODE <> '" + strSACODE + "';");
            String strSql_Update = sbSql.ToString();

            int iSaveCount = SqlParamDao.ExecuteNonQueryBySql(strSql_Update);
            if(iSaveCount>0){
                strReturnCode = "1";
                strReturnMsg = strMethodDesc + "成功";
            }else{
                strReturnCode = "1";
                strReturnMsg = strMethodDesc + "失败";
            } 
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = strMethodDesc + "出错";
            log.Error(ex);
        }
        finally
        {
            StringBuilder sbResultStatus = new StringBuilder();
            sbResultStatus.Append("{");
            sbResultStatus.Append("\"ReturnCode\":\"" + strReturnCode.ToString() + "\"");
            sbResultStatus.Append(",\"ReturnMsg\":\"" + strReturnMsg.ToString() + "\"");

            sbResult.Append(sbResultStatus.ToString());
            if (!String.IsNullOrEmpty(sbReturnData.ToString()))
            {
                sbResult.Append(",\"ReturnData\":\"" + sbReturnData.ToString()+"\"");
            }
            sbResult.Append("}");
            log.Error(strMethodDesc + "Return Json:" + sbResult.ToString());
        }
        return sbResult.ToString();
    }


    public bool IsReusable {
        get {
            return false;
        }
    }

}