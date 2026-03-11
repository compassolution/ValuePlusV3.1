<%@ WebHandler Language="C#" Class="Common" %>

using System;
using System.Web;
using System.Text;
using System.IO;
using System.Data;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Web;
using Microsoft.JScript;

public class Common : HandlerBase,IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    #region 日志声明
    /// <summary>
    /// 日志声明
    /// </summary>
    private Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    #endregion


    public void ProcessRequest (HttpContext context) {
        //解析客户端传递过来的json data
        StreamReader reader = new StreamReader(context.Request.InputStream);
        String strParamJson = HttpUtility.UrlDecode(reader.ReadToEnd());
        //log.Error("传入参数的Json data字符串:" + strParamJson.ToString());

        string param = WebCommon.GetJsonValue(strParamJson,"param").ToString();//请求类型参数
        string strUserCode = WebCommon.GetJsonValue(strParamJson,"usercode").ToString();//用户编码
        string strDicLid = WebCommon.GetJsonValue(strParamJson,"lid").ToString();
        string strCulture = WebCommon.GetJsonValue(strParamJson,"culture").ToString();//文化编码[0中文1英文]
        string strProjectId = WebCommon.GetJsonValue(strParamJson,"projectid").ToString();
        string strWXMPModule = WebCommon.GetJsonValue(strParamJson,"wxmpmodule").ToString();        
        string strSql = WebCommon.GetJsonObjectValue(strParamJson,"sql").ToString();
        //log.Error("传入参数的strSql.1:" + strSql.ToString());
        strSql = Microsoft.JScript.GlobalObject.decodeURIComponent(strSql);

        string strDicIsIncludeInvalid = WebCommon.GetJsonValue(strParamJson,"isincludeinvalid").ToString();//是否包含已停用的
        string strIsEscape = WebCommon.GetJsonValue(strParamJson,"isescape").ToString();//是否
        strDicIsIncludeInvalid = String.IsNullOrEmpty(strDicIsIncludeInvalid) ? "0" : "1";
        Boolean isIncludeInvalid = strDicIsIncludeInvalid.Equals("1") ? true : false;
        strIsEscape = String.IsNullOrEmpty(strIsEscape) ? "0" : "1";
        Boolean isEscape = strIsEscape.Equals("1") ? true : false;

        String strRequestLanguage = strCulture.Equals("0") ? "zh-cn" : "en-us";
        switch (param.ToLower().ToString())
        {
            case "testconnect"://测试网络链接是否正常
                this.TestConnect(context);
                break;
            case "getapiwebsitehostyrl"://获取应用VP平台中的基本参数WebSiteHostUrl
                this.GetAPIWebSiteHostUrl(context, strUserCode);
                break;
            case "getapisysupdateserver"://获取应用VP平台中的基本参数SysUpdateServer
                this.GetAPISysUpdateServer(context, strUserCode);
                break;
            case "judgeishavalicense"://获取某就职单位是否获取的某功能模块的授权
                context.Response.Write(this.GetIsHaveLicenseByProjectId(context, strProjectId, strWXMPModule));
                break;
            case "getdicdetail"://获取特定Lid的字典数据
                this.GetDicDetail(context, strDicLid, isIncludeInvalid,isEscape);
                break;
            case "getdatabysql"://通过SQL获取数据json
                this.GetDataBySQL(context,strSql,isEscape);
                break;
            default:
                break;
        }
    }
    
    /// <summary>
    /// 测试网络链接是否正常
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strUserCode"></param>
    public void TestConnect(HttpContext context)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        String strMethodDesc = "测试网络链接是否正常";

        StringBuilder sbResult = new StringBuilder();
        try
        {
            String strSql = "select top 1 * from BASICPARAM_1";
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
            strReturnCode = "1";
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = strMethodDesc+"出错,请稍候重试";
            log.Error(ex);
        }
        context.Response.Write(strReturnCode);
    }

    /// <summary>
    /// 获取应用VP平台中的基本参数WebSiteHostUrl
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strUserCode"></param>
    public void GetAPIWebSiteHostUrl(HttpContext context,String strUserCode)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        String strMethodDesc = "获取应用VP平台中的基本参数WebSiteHostUrl";

        StringBuilder sbResult = new StringBuilder();
        try
        {
            String strSql = "select * from BASICPARAM_1 WHERE paramName = 'WebSiteHostUrl'";
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
            strReturnCode = "1";
            strReturnMsg = strMethodDesc+"成功";
            sbResultData.Append("\"ResultData\":\""+dt.Rows[0]["paramValue"]+"\"");
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = strMethodDesc+"出错,请稍候重试";
            log.Error(ex);
        }
        finally{
            StringBuilder sbResultStatus = new StringBuilder();
            sbResultStatus.Append("\"ReturnStatus\":{");
            sbResultStatus.Append("\"ReturnCode\":\""+strReturnCode.ToString()+"\"");
            sbResultStatus.Append(",\"ReturnMsg\":\""+strReturnMsg.ToString()+"\"");
            sbResultStatus.Append("}");

            sbResult.Append("{");
            sbResult.Append(sbResultStatus.ToString());
            if (!String.IsNullOrEmpty(sbResultData.ToString()))
            {
                sbResult.Append(","+sbResultData.ToString());
            }
            sbResult.Append("}");
        }
        //log.Error(strMethodDesc+"时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }
    
    /// <summary>
    /// 获取应用VP平台中的基本参数SysUpdateServer
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strUserCode"></param>
    public void GetAPISysUpdateServer(HttpContext context,String strUserCode)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        String strMethodDesc = "获取应用VP平台中的基本参数SysUpdateServer";

        StringBuilder sbResult = new StringBuilder();
        try
        {
            String strSql = "select * from BASICPARAM_1 WHERE paramName = 'SysUpdateServer'";
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
            strReturnCode = "1";
            strReturnMsg = strMethodDesc+"成功";
            sbResultData.Append("\"ResultData\":\""+dt.Rows[0]["paramValue"]+"\"");
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = strMethodDesc+"出错,请稍候重试";
            log.Error(ex);
        }
        finally{
            StringBuilder sbResultStatus = new StringBuilder();
            sbResultStatus.Append("\"ReturnStatus\":{");
            sbResultStatus.Append("\"ReturnCode\":\""+strReturnCode.ToString()+"\"");
            sbResultStatus.Append(",\"ReturnMsg\":\""+strReturnMsg.ToString()+"\"");
            sbResultStatus.Append("}");

            sbResult.Append("{");
            sbResult.Append(sbResultStatus.ToString());
            if (!String.IsNullOrEmpty(sbResultData.ToString()))
            {
                sbResult.Append(","+sbResultData.ToString());
            }
            sbResult.Append("}");
        }
        //log.Error(strMethodDesc+"时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }
    
    /// <summary>
    /// 获取某就职单位是否获取的某功能模块的授权
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strProjectId"></param>
    /// <param name="strWXMPModule">LID=WXMPModule</param>
    private String GetIsHaveLicenseByProjectId(HttpContext context,String strProjectId,String strWXMPModule)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "获取某就职单位是否获取的某功能模块的授权";
        try
        {

            //context.Response.Write(ProjectWXAuth.JudgeWXAuthIsValidByProjectId(strProjectId,strWXMPModule,"").ToString().ToLower());

            String strNow = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select * from WXProjectAuth_1 where 1=1");
            sbSql.Append(" and ProjectId = '"+strProjectId+"'");
            sbSql.Append(" and '"+strNow+"' between convert(varchar(20),[PeriodFrom"+strWXMPModule+"],120) and convert(varchar(20),[PeriodTo"+strWXMPModule+"],120)");
            String strSql = sbSql.ToString();
            log.Error(strMethodDesc+"SQL:"+strSql);

            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
            sbResultData.Append(WebCommon.GetJsonStringByDataTable(dt,"\"ResultData\"",false));

            strReturnCode = "1";
            strReturnMsg = strMethodDesc+"成功";
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = strMethodDesc+"出错";
            log.Error(ex);
        }
        finally{
            StringBuilder sbResultStatus = new StringBuilder();
            sbResultStatus.Append("\"ReturnStatus\":{");
            sbResultStatus.Append("\"ReturnCode\":\""+strReturnCode.ToString()+"\"");
            sbResultStatus.Append(",\"ReturnMsg\":\""+strReturnMsg.ToString()+"\"");
            sbResultStatus.Append("}");

            sbResult.Append("{");
            sbResult.Append(sbResultStatus.ToString());
            if (!String.IsNullOrEmpty(sbResultData.ToString()))
            {
                sbResult.Append(","+sbResultData.ToString());
            }
            sbResult.Append("}");
            log.Error(strMethodDesc+"Return Json:"+sbResult.ToString());
        }
        return sbResult.ToString();
    }



    /// <summary>
    ///获取字典明细
    /// </summary>
    public void GetDicDetail(HttpContext context, String strDicLid, bool isContentStopped,bool isEscape)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();

        StringBuilder sbResult = new StringBuilder();
        try
        {
            sbResultData.Append("\"ResultData\":");
            sbResultData.Append(DicGetter.GetDictionaryList(strDicLid,isContentStopped,isEscape));
            sbResultData.Append("");
            strReturnCode = "1";
            strReturnMsg = "获取字典明细成功";
            //log.Error("获取字典明细成功:" + sbResultData.ToString());
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = "获取字典明细出错";
            log.Error(ex);
        }
        finally{
            StringBuilder sbResultStatus = new StringBuilder();
            sbResultStatus.Append("\"ReturnStatus\":{");
            sbResultStatus.Append("\"ReturnCode\":\""+strReturnCode.ToString()+"\"");
            sbResultStatus.Append(",\"ReturnMsg\":\""+strReturnMsg.ToString()+"\"");
            sbResultStatus.Append("}");

            sbResult.Append("{");
            sbResult.Append(sbResultStatus.ToString());
            if (!String.IsNullOrEmpty(sbResultData.ToString()))
            {
                sbResult.Append(","+sbResultData.ToString());
            }
            sbResult.Append("}");
        }
        context.Response.Write(sbResult.ToString());
    }

    /// <summary>
    /// 通过SQL获取数据json
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strSql"></param>
    /// <param name="isEscape"></param>
    public void GetDataBySQL(HttpContext context,String strSql,bool isEscape)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        String strMethodDesc = "通过SQL获取数据jason";
        StringBuilder sbResult = new StringBuilder();

        try
        {
            //客户端进行的关键字替换，服务端替换回来
            strSql = strSql.Replace("#", "'").Replace("S1E2L3E4C5T","select").Replace("F1R2O3M","from");
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
            sbResultData.Append(WebCommon.GetJsonStringByDataTable(dt, "\"ResultData\"", isEscape));
            strReturnCode = "1";
            strReturnMsg = strMethodDesc+"成功";
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = strMethodDesc+"出错,请稍候重试";
            log.Error(ex);
        }
        finally{
            StringBuilder sbResultStatus = new StringBuilder();
            sbResultStatus.Append("\"ReturnStatus\":{");
            sbResultStatus.Append("\"ReturnCode\":\""+strReturnCode.ToString()+"\"");
            sbResultStatus.Append(",\"ReturnMsg\":\""+strReturnMsg.ToString()+"\"");
            sbResultStatus.Append("}");

            sbResult.Append("{");
            sbResult.Append(sbResultStatus.ToString());
            if (!String.IsNullOrEmpty(sbResultData.ToString()))
            {
                sbResult.Append(","+sbResultData.ToString());
            }
            sbResult.Append("}");
        }
        //log.Error(strMethodDesc+"时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}