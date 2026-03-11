<%@ WebHandler Language="C#" Class="QueryCommon" %>

using System;
using System.Web;
using System.Collections;
using System.Text;
using System.IO;
using System.Data;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Web;
using Com.ValuePlus.Archive.BLL;
using Newtonsoft.Json;

public class QueryCommon : HandlerBase,IHttpHandler, System.Web.SessionState.IRequiresSessionState
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
        string strLocationCode = WebCommon.GetJsonValue(strParamJson,"locationcode").ToString();//地址编码
        string strParamName = WebCommon.GetJsonValue(strParamJson,"paramname").ToString();//基本参数名称

        object objTakedData = context.Request["datataked"] == null ? string.Empty : context.Request["datataked"];//上传数据对象

        String strRequestLanguage = strCulture.Equals("0") ? "zh-cn" : "en-us";
        switch (param.ToLower().ToString())
        {
            case "getdata_basicparamlist"://获取基本参数数据列表
                this.GetData_BasicParamList(context,strRequestLanguage);
                break;
            case "getdata_locationlist"://获取存放地址信息数据列表
                this.GetData_LocationList(context,strRequestLanguage);
                break;
            case "getdata_maxlocationlevellist"://获取存放地址最大级别的数据列表
                this.GetData_MaxLocationLevelList(context,strRequestLanguage);
                break;
            default:
                break;
        }
    }
    
    /// <summary>
    /// 获取基本参数数据列表
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strParamName"></param>
    public void GetData_BasicParamList(HttpContext context,String strParamName)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        String strMethodDesc = "获取基本参数数据列表";
        StringBuilder sbResult = new StringBuilder();

        try
        {
            DataTable dt = new DataTable();
            String strSql = "select * from [VW_Moblie_BasicParam] where 1=1";
            if(!String.IsNullOrEmpty(strParamName)){
                strSql = strSql + " and ParamName = '"+strParamName+"'";
            }
            strSql = strSql + " order by ParamName";
            sbResultData.Append("," + WebCommon.GetJsonStringByDataTable(SqlParamDao.GetDataTableBySql(strSql), "\"ResultData\"", true));

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
                sbResult.Append(sbResultData.ToString()+"");
            }
            sbResult.Append("}");
        }
        //log.Error(strMethodDesc+"时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }

    /// <summary>
    /// 获取存放地址信息数据列表
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strRequestLanguage"></param>
    public void GetData_LocationList(HttpContext context,String strRequestLanguage)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        String strMethodDesc = "获取存放地址信息数据列表";
        StringBuilder sbResult = new StringBuilder();

        try
        {
            DataTable dt = new DataTable();
            String strSql = "";
            //系统存放地址信息数据
            strSql = "select * from [VW_Moblie_Location] order by SLCODE";
            sbResultData.Append("," + WebCommon.GetJsonStringByDataTable(SqlParamDao.GetDataTableBySql(strSql), "\"ResultData\"", true));

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
                sbResult.Append(sbResultData.ToString()+"");
            }
            sbResult.Append("}");
        }
        //log.Error(strMethodDesc+"时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }
    
    /// <summary>
    /// 获取存放地址最大级别的数据列表
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strRequestLanguage"></param>
    public void GetData_MaxLocationLevelList(HttpContext context,String strRequestLanguage)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        String strMethodDesc = "获取存放地址最大级别的数据列表";
        StringBuilder sbResult = new StringBuilder();

        try
        {
            DataTable dt = new DataTable();
            String strSql = "";
            //系统存放地址信息数据
            strSql = "select * from [VW_Moblie_Location] where ISLASTLEVEL = '1' order by SLCODE";
            sbResultData.Append("," + WebCommon.GetJsonStringByDataTable(SqlParamDao.GetDataTableBySql(strSql), "\"ResultData\"", true));

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
                sbResult.Append(sbResultData.ToString()+"");
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