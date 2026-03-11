<%@ WebHandler Language="C#" Class="Overtime" %>

using System;
using System.Web;
using System.Collections;
using System.Text;
using System.IO;
using System.Data;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Web;
using Com.ValuePlus.Archive.BLL;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

public class Overtime : HandlerBase,IHttpHandler, System.Web.SessionState.IRequiresSessionState
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
        string strYearMonth = WebCommon.GetJsonValue(strParamJson,"yearmonth").ToString();
        string strDCNO = WebCommon.GetJsonValue(strParamJson,"dcno").ToString();
        string strOneDate = WebCommon.GetJsonValue(strParamJson,"ondate").ToString();
        string strDateFrom = WebCommon.GetJsonValue(strParamJson,"datefrom").ToString();
        string strDateTo = WebCommon.GetJsonValue(strParamJson,"dateto").ToString();
        string strOTType = WebCommon.GetJsonValue(strParamJson,"ottype").ToString();//加班类型
        string strOTPayType = WebCommon.GetJsonValue(strParamJson,"otpaytype").ToString();//加班单支付类型
        string strOTStatus = WebCommon.GetJsonValue(strParamJson,"otstatus").ToString();//加班单状态
        string strSaveShiftData = WebCommon.GetJsonObjectValue(strParamJson,"saveshiftdata").ToString();//需保存的排班信息数据
        
        String strPageSize = WebCommon.GetJsonValue(strParamJson,"pagesize").ToString();
        String strPageIndex = WebCommon.GetJsonValue(strParamJson,"pageindex").ToString();
        int iPageSize = String.IsNullOrEmpty(strPageSize)? 10 : int.Parse(strPageSize);
        int iPageIndex = String.IsNullOrEmpty(strPageSize)? 0 : int.Parse(strPageIndex);

        String strRequestLanguage = strCulture.Equals("0") ? "zh-cn" : "en-us";
        switch (param.ToLower().ToString())
        {
            case "getovertimetypelist"://获取加班类型
                this.GetOvertimeTypeList(context,strUserCode);
                break;
            case "getovertimelist"://获取加班单列表
                this.GetOvertimeList(context,strDCNO,strDateFrom,strDateTo,strOTType,strOTStatus,strOTPayType,strUserCode);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 获取加班类型
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strUserCode"></param>
    public void GetOvertimeTypeList(HttpContext context,String strUserCode)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        String strMethodDesc = "获取加班类型";
        StringBuilder sbResult = new StringBuilder();

        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select * from OTTYPE_1 WHERE BISSTOP <> '1' ORDER BY OTTYPE ");
            String strSql = sbSql.ToString();

            //log.Error(strMethodDesc+":"+strSql);

            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);

            strReturnCode = "1";
            strReturnMsg = strMethodDesc+"成功";
            sbResultData.Append(WebCommon.GetJsonStringByDataTable(dt, "\"OTTypeInfo\"", false));
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
                sbResult.Append(",\"ResultData\":{"+sbResultData.ToString()+"}");
            }
            sbResult.Append("}");
        }
        //log.Error(strMethodDesc+"时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }

    /// <summary>
    /// 获取加班单列表
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strDCNO"></param>
    /// <param name="strDateFrom"></param>
    /// <param name="strDateTo"></param>
    /// <param name="strOTType"></param>
    /// <param name="strOTStatus"></param>
    /// <param name="strOTPayType"></param>
    /// <param name="strUserCode"></param>
    public void GetOvertimeList(HttpContext context,String strDCNO,String strDateFrom,String strDateTo,String strOTType,String strOTStatus,String strOTPayType,String strUserCode)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        String strMethodDesc = "获取加班单列表";
        StringBuilder sbResult = new StringBuilder();

        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("SELECT * from [VW_MB_KQOVTM] where 1=1 ");
            if(!String.IsNullOrEmpty(strDCNO)){
                sbSql.Append(" and StaffNo = '"+strDCNO+"'");
            }
            if(!String.IsNullOrEmpty(strDateFrom)){
                sbSql.Append(" and OTDate >= '"+strDateFrom+"'");
            }
            if(!String.IsNullOrEmpty(strDateTo)){
                sbSql.Append(" and OTDate <= '"+strDateTo+"'");
            }
            if(!String.IsNullOrEmpty(strOTType)){
                sbSql.Append(" and OTTYPE = '"+strOTType+"'");
            }
            if(!String.IsNullOrEmpty(strOTPayType)){
                sbSql.Append(" and OTPTYPE = '"+strOTPayType+"'");
            }
            if(!String.IsNullOrEmpty(strOTStatus)){
                sbSql.Append(" and OTStatus = '"+strOTStatus+"'");
            }
            String strSql = sbSql.ToString();

            //log.Error(strMethodDesc+":"+strSql);

            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);

            strReturnCode = "1";
            strReturnMsg = strMethodDesc+"成功";
            sbResultData.Append(WebCommon.GetJsonStringByDataTable(dt, "\"OTRecordInfo\"", false));
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
                sbResult.Append(",\"ResultData\":{"+sbResultData.ToString()+"}");
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