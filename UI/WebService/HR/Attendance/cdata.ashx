<%@ WebHandler Language="C#" Class="cdata" %>

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

public class cdata : IHttpHandler {

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
        string strAccountId = hsTableUrlQuery["accountid"] == null ? string.Empty : hsTableUrlQuery["accountid"].ToString();

        string strSACODE = hsTableUrlQuery["sacode"] == null ? string.Empty : hsTableUrlQuery["sacode"].ToString();
        string strLabelType = hsTableUrlQuery["labeltype"] == null ? string.Empty : hsTableUrlQuery["labeltype"].ToString();
        string strTableName = hsTableUrlQuery["tablename"] == null ? string.Empty : hsTableUrlQuery["tablename"].ToString();
        string strFiledName = hsTableUrlQuery["fieldname"] == null ? string.Empty : hsTableUrlQuery["fieldname"].ToString();
        string strFiledValue = hsTableUrlQuery["fieldvalue"] == null ? string.Empty : hsTableUrlQuery["fieldvalue"].ToString();

        switch (strParam.ToLower().ToString())
        {
            case "setassetsprinted":
                context.Response.Write(this.SetAssetsPrinted(strSACODE).ToString());
                break;
        }
    }

    /// <summary>
    ///设置某些资产为已打印
    /// </summary>
    private String SetAssetsPrinted(String strBatchSACODE)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbReturnData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "设置某些资产为已打印";
        try
        {
            if (!String.IsNullOrEmpty(strBatchSACODE))
            {
                String strSql = "UPDATE AMASSETS_1 SET BISPRINTED = '1' WHERE SACODE IN (" + strBatchSACODE + ")";
                int iSaveCount = SqlParamDao.ExecuteNonQueryBySql(strSql);
                if(iSaveCount>0){
                    strReturnCode = "1";
                    strReturnMsg = strMethodDesc + "成功";
                }
                sbReturnData.Append(iSaveCount.ToString());
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