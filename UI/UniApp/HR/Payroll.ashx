<%@ WebHandler Language="C#" Class="Payroll" %>

using System;
using System.Web;
using System.Text;
using System.Data;
using System.IO;
using System.Collections;
using Com.ValuePlus.Web;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Weixin;
using Com.ValuePlus.Utils.Serializable;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

public class Payroll : HandlerBase, IHttpHandler, System.Web.SessionState.IRequiresSessionState {

    public void ProcessRequest (HttpContext context) {
        //解析客户端传递过来的json data
        StreamReader reader = new StreamReader(context.Request.InputStream);
        String strParamJson = HttpUtility.UrlDecode(reader.ReadToEnd());
        //log.Error("传入参数的Json data字符串:" + strParamJson.ToString());

        string param = WebCommon.GetJsonValue(strParamJson,"param").ToString();//请求类型参数
        string strCulture = WebCommon.GetJsonValue(strParamJson,"culture").ToString();//文化编码[0中文1英文]
        string strStaffNo = WebCommon.GetJsonValue(strParamJson,"dcno").ToString();
        string strYearMonth = WebCommon.GetJsonValue(strParamJson,"yearmonth").ToString();

        String strRequestLanguage = strCulture.Equals("0") ? "zh-cn" : "en-us";

        switch (param.ToLower().ToString())
        {
            case "getstaffpayrolldetail"://根据工号和年月获取薪资明细信息
                this.GetStaffPayrollDetail(context, strStaffNo, strYearMonth);
                break;
            case "getstaffpayrollyearmonth"://根据工号获取发放工资的月份
                this.GetStaffPayrollYearMonth(context,strStaffNo);
                break;
            default:
                break;
        }
    }
    
    /// <summary>
    /// 根据工号获取发放工资的月份
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strStaffNo"></param>
    /// <param name="strProjectId"></param>
    private void GetStaffPayrollYearMonth(HttpContext context,String strStaffNo)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "根据工号获取发放工资的月份";
        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select distinct YearMonth,StartDate,EndDate from [VW_MB_Payroll_ItemList] where 1=1  ");
            sbSql.Append(" AND StaffNo = '"+strStaffNo+"'");
            sbSql.Append(" order by YearMonth desc");
            String strSql = sbSql.ToString();
            //log.Error(strMethodDesc+"SQL:"+strSql);

            DataTable dt = SqlParamDao.GetDataTableBySql(sbSql.ToString());
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
        }
        context.Response.Write(sbResult.ToString());
    }

    /// <summary>
    /// 获取员工对应薪资期间的员工薪资条明细
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strStaffNo"></param>
    /// <param name="strProjectId"></param>
    /// <param name="strYearMonth"></param>
    private void GetStaffPayrollDetail(HttpContext context,String strStaffNo,String strYearMonth)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "获取员工对应薪资期间的员工薪资条明细";
        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select * from [VW_MB_Payroll_ItemList] where 1=1  ");
            sbSql.Append(" AND StaffNo = '"+strStaffNo+"'");
            sbSql.Append(" AND YearMonth = '"+strYearMonth+"'");
            sbSql.Append(" order by ITEMPORDER desc");
            String strSql = sbSql.ToString();
            //log.Error(strMethodDesc+"SQL:"+strSql);

            DataTable dt = SqlParamDao.GetDataTableBySql(sbSql.ToString());
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
        }
        context.Response.Write(sbResult.ToString());
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}