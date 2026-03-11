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
        log.Error("传入参数的Json data字符串:" + strParamJson.ToString());

        string strParam = WebCommon.GetJsonValue(strParamJson,"param").ToString();//请求类型参数
        string strLanguage = WebCommon.GetJsonValue(strParamJson,"language").ToString();
        string strMobileNo = WebCommon.GetJsonValue(strParamJson,"mobileno").ToString();
        string strProjectId = WebCommon.GetJsonValue(strParamJson,"projectid").ToString();
        string strYearMonthType = WebCommon.GetJsonValue(strParamJson,"yearmonthtype").ToString();
        string strYearMonth = WebCommon.GetJsonValue(strParamJson,"yearmonth").ToString();

        log.Error("Payroll.ashx,MobileNo:"+strMobileNo+";Language:"+strLanguage);
        if (strParam.Equals("getlastoneprojectmonth"))
        {
            context.Response.Write(this.GetLastOneProjectMonth(context, strMobileNo, strLanguage));
        }
        else if (strParam.Equals("getprojectidlist"))
        {
            context.Response.Write(this.GetProjectList(context,strMobileNo,strLanguage));
        }
        else if (strParam.Equals("getyearmonth"))
        {
            context.Response.Write(this.GetYearMonthList(context,strMobileNo,strProjectId));
        }
        else if (strParam.Equals("getcurstaffdutyinfo"))
        {
            context.Response.Write(this.GetCurStaffDutyInfo(context,strMobileNo,strProjectId,strYearMonth));
        }
        else if (strParam.Equals("getstaffpayrolldetail"))
        {
            context.Response.Write(this.GetStaffPayrollDetail(context,strMobileNo,strProjectId,strYearMonth));
        }
    }

    /// <summary>
    /// 获取最后一个就职单位的最后一个月份信息
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strMobileNo"></param>
    /// <param name="strLanguage"></param>
    private String GetLastOneProjectMonth(HttpContext context,String strMobileNo,String strLanguage)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "获取最后一个就职单位的最后一个月份信息";
        try
        {
            log.Error("获取最后一个就职单位的最后一个月份信息,MobileNo:"+strMobileNo+";Language:"+strLanguage);
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select TOP 1 ProjectId ");
            if (strLanguage.Equals("0"))
            {
                sbSql.Append(",ProjectNameChs as ProjectName");
            }else
            {
                sbSql.Append(",ProjectName");
            }
            sbSql.Append(",YearMonth ");
            sbSql.Append(",PSTART+'/'+PEND as YearMonthScope ");
            sbSql.Append(" from [TB_Remote_StaffSalaryData] where StaffMobile = '"+strMobileNo+"' order by YEARMONTH DESC");
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
    /// 获取就职单位列表
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strMobileNo"></param>
    /// <param name="strLanguage"></param>
    private String GetProjectList(HttpContext context,String strMobileNo,String strLanguage)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "获取就职单位列表";
        try
        {
            log.Error("获取就职单位列表,MobileNo:"+strMobileNo+";Language:"+strLanguage);
            StringBuilder sbSql = new StringBuilder();
            if (strLanguage.Equals("0"))
            {
                sbSql.Append("select distinct ProjectId,ProjectNameChs as ProjectName from [TB_Remote_StaffSalaryData] where StaffMobile = '"+strMobileNo+"'");
            }else
            {
                sbSql.Append("select distinct ProjectId,ProjectName from [TB_Remote_StaffSalaryData] where StaffMobile = '"+strMobileNo+"'");
            }
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
        }
        return sbResult.ToString();
    }

    /// <summary>
    /// 获取薪资期间列表
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strMobileNo"></param>
    /// <param name="strProjectId"></param>
    private String GetYearMonthList(HttpContext context,String strMobileNo,String strProjectId)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "获取薪资期间列表";
        try
        {
            String strCurYearMonth = "";
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select distinct YearMonth,PSTART,PEND from [TB_Remote_StaffSalaryData] where 1=1  ");
            sbSql.Append(" AND StaffMobile = '"+strMobileNo+"'");
            sbSql.Append(" AND ProjectId = '"+strProjectId+"'");
            sbSql.Append(" order by YEARMONTH desc");
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
        }
        return sbResult.ToString();
    }

    /// <summary>
    /// 获取当前登录用户当月在职信息
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strMobileNo"></param>
    /// <param name="strProjectId"></param>
    /// <param name="strYearMonth"></param>
    /// <returns></returns>
    public String GetCurStaffDutyInfo(HttpContext context,String strMobileNo,String strProjectId,String strYearMonth)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "获取当前登录用户当月在职信息";
        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select top 1 * from [TB_Remote_StaffSalaryData] where 1=1  ");
            sbSql.Append(" AND StaffMobile = '"+strMobileNo+"'");
            sbSql.Append(" AND ProjectId = '"+strProjectId+"'");
            sbSql.Append(" AND YEARMONTH = '"+strYearMonth+"'");
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
        }
        return sbResult.ToString();
    }

    /// <summary>
    /// 获取员工对应薪资期间的员工薪资条明细
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strMobileNo"></param>
    /// <param name="strProjectId"></param>
    /// <param name="strYearMonth"></param>
    private String GetStaffPayrollDetail(HttpContext context,String strMobileNo,String strProjectId,String strYearMonth)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "获取员工对应薪资期间的员工薪资条明细";
        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select * from [TB_Remote_StaffSalaryData] where 1=1  ");
            sbSql.Append(" AND StaffMobile = '"+strMobileNo+"'");
            sbSql.Append(" AND ProjectId = '"+strProjectId+"'");
            sbSql.Append(" AND YEARMONTH = '"+strYearMonth+"'");
            sbSql.Append(" and ISSHOW = 'true' order by ITEMPORDER");
            String strSql = sbSql.ToString();
            log.Error(strMethodDesc+"SQL:"+strSql);

            DataTable dt = SqlParamDao.GetDataTableBySql(sbSql.ToString());
            sbResultData.Append(WebCommon.GetJsonStringByDataTable(dt,"\"ResultData\"",false));

            strReturnCode = "1";
            strReturnMsg = strMethodDesc+"成功";

            //新增用户访问记录信息表的写入 add by sammen 20190919
            WXUser.RecordWXUser_3(strMobileNo,strProjectId,"查看员工薪资条");
            //新增用户访问记录信息表的写入 add by sammen 20190919

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
        return sbResult.ToString();
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}