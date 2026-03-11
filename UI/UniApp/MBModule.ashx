<%@ WebHandler Language="C#" Class="MBModule" %>

using System;
using System.Web;
using System.Text;
using System.IO;
using System.Data;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Web;

public class MBModule : HandlerBase,IHttpHandler, System.Web.SessionState.IRequiresSessionState
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
        string strCulture = WebCommon.GetJsonValue(strParamJson,"culture").ToString();//文化编码[0中文1英文]
        string strModuleCode = WebCommon.GetJsonValue(strParamJson,"modulecode").ToString();//模块编码
        string strIsValid = WebCommon.GetJsonValue(strParamJson,"isvalid").ToString();//是否有效

        String strRequestLanguage = strCulture.Equals("0") ? "zh-cn" : "en-us";
        switch (param.ToLower().ToString())
        {
            case "getmbmoduledatalist"://获取数据库中MBModule表中的数据信息
                this.GetMBModuleDataList(context, strModuleCode);
                break;
            case "getmymbmoduledata"://根据登录账号获取FLUser_5对应MBModule表中的功能权限
                this.GetMyMBModuleData(context, strUserCode,strIsValid);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 获取数据库中MBModule表中的数据信息
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strUserCode"></param>
    public void GetMBModuleDataList(HttpContext context,String strModuleCode)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        String strMethodDesc = "获取数据库中MBModule表中的数据信息";

        StringBuilder sbResult = new StringBuilder();
        try
        {
            String strSql = "select * from TREE_MBModule WHERE 1=1 ";
            if(!String.IsNullOrEmpty(strModuleCode)){
                strSql = strSql + " and ModuleCode = '"+strModuleCode+"'";
            }
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);

            strReturnCode = "1";
            strReturnMsg = strMethodDesc+"成功";
            sbResultData.Append(WebCommon.GetJsonStringByDataTable(dt, "\"ResultData\"", false));
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
    /// 根据登录账号或者员工工号获取FLUser_5对应MBModule表中的功能权限
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strUserCode"></param>
    public void GetMyMBModuleData(HttpContext context,String strUserCode,String strIsValid)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        String strMethodDesc = "根据登录账号或者员工工号获取FLUser_5对应MBModule表中的功能权限";

        StringBuilder sbResult = new StringBuilder();
        try
        {
            String strSql = "select A.*,B.SUSERID from TREE_MBModule A INNER JOIN FLUser_5 B ON A.ModuleCode = B.MCODE WHERE 1=1 AND SUSERID = '"+strUserCode+"' ";
            if(!String.IsNullOrEmpty(strIsValid)){
                strSql = strSql + " and BISSTOP = '"+(strIsValid.Equals("0")?"1":"2")+"'";
            }
            if(strUserCode.ToLower().Equals("admin")){
                strSql = "select A.*,'"+strUserCode+"' as SUSERID from TREE_MBModule A where TREELEVEL = '2' and BISSTOP = '"+(strIsValid.Equals("0")?"1":"2")+"'";
            }
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);

            strReturnCode = "1";
            strReturnMsg = strMethodDesc+"成功";
            sbResultData.Append(WebCommon.GetJsonStringByDataTable(dt, "\"ResultData\"", false));
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