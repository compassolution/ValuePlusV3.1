<%@ WebHandler Language="C#" Class="YearMonth" %>

using System;
using System.Web;
using System.Collections;
using System.Text;
using System.IO;
using System.Data;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Web;
using Com.ValuePlus.Archive.BLL;

public class YearMonth : HandlerBase,IHttpHandler, System.Web.SessionState.IRequiresSessionState
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
        string strOneDate = WebCommon.GetJsonValue(strParamJson,"onedate").ToString();

        String strPageSize = WebCommon.GetJsonValue(strParamJson,"pagesize").ToString();
        String strPageIndex = WebCommon.GetJsonValue(strParamJson,"pageindex").ToString();
        int iPageSize = String.IsNullOrEmpty(strPageSize)? 10 : int.Parse(strPageSize);
        int iPageIndex = String.IsNullOrEmpty(strPageSize)? 0 : int.Parse(strPageIndex);

        String strRequestLanguage = strCulture.Equals("0") ? "zh-cn" : "en-us";
        switch (param.ToLower().ToString())
        {
            case "getyearmonthdata"://获取年月周期信息数据
                this.GetYearMonthData(context,strYearMonth,strUserCode);
                break;
            case "gettodayyearmonthdata"://获取今天所在的年月周期信息数据
                this.GetTodayYearMonthData(context,strOneDate,strUserCode);
                break;
            case "getattendanceyeardata"://获取所有考勤年份
                this.GetAttendanceYearData(context,strUserCode);
                break;
            case "getsalaryyeardata"://获取所有薪资年份
                this.GetSalaryYearData(context,strUserCode);
                break;
            default:
                break;
        }
    }


    /// <summary>
    /// 获取今天所在的年月周期信息数据
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strOneDate"></param>
    /// <param name="strUserCode"></param>
    public void GetTodayYearMonthData(HttpContext context,String strOneDate,String strUserCode)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        String strMethodDesc = "获取今天所在的年月周期信息数据";
        StringBuilder sbResult = new StringBuilder();

        try
        {
            strOneDate = String.IsNullOrEmpty(strOneDate) ? DateTime.Now.ToString("yyyy-MM-dd") : strOneDate;
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("SELECT *  FROM KQPERD_1 A WHERE 1=1 ");
            sbSql.Append(" and '"+strOneDate+"' BETWEEN CONVERT(VARCHAR(20),PSTART,23) AND CONVERT(VARCHAR(20),PEND,23)");

            String strSql = sbSql.ToString();

            //log.Error(strMethodDesc+":"+strSql);

            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);

            strReturnCode = "1";
            strReturnMsg = strMethodDesc+"成功";
            sbResultData.Append(WebCommon.GetJsonStringByDataTable(dt, "\"YearMonthData\"", false));
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
    /// 获取年月周期信息数据
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strYearMonth"></param>
    /// <param name="strUserCode"></param>
    public void GetYearMonthData(HttpContext context,String strYearMonth,String strUserCode)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        String strMethodDesc = "获取年月周期信息数据";
        StringBuilder sbResult = new StringBuilder();

        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("SELECT *  ");
            sbSql.Append(",(CASE WHEN CONVERT(VARCHAR(20),GETDATE(),23) BETWEEN CONVERT(VARCHAR(20),PSTART,23) AND CONVERT(VARCHAR(20),PEND,23) THEN '1' ELSE '0' END) AS IsTodayPeriod");
            sbSql.Append(" FROM KQPERD_1 A WHERE 1=1 ");
            if(!String.IsNullOrEmpty(strYearMonth)){
                sbSql.Append(" AND A.PID = '"+strYearMonth+"'");
            }
            //暂时只显示今天往前12个月的那天所在期间之后的记录
            sbSql.Append(" and PID>(SELECT PID FROM KQPERD_1 WHERE CONVERT(VARCHAR(20),dateadd(MONTH,-12,getdate()),23) BETWEEN CONVERT(VARCHAR(20),PSTART,23) and CONVERT(VARCHAR(20),PEND,23))");
            sbSql.Append(" ORDER BY A.PID DESC");
            String strSql = sbSql.ToString();

            //log.Error(strMethodDesc+":"+strSql);

            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);

            strReturnCode = "1";
            strReturnMsg = strMethodDesc+"成功";
            sbResultData.Append(WebCommon.GetJsonStringByDataTable(dt, "\"YearMonthData\"", false));
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
    /// 获取所有考勤年份
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strUserCode"></param>
    public void GetAttendanceYearData(HttpContext context,String strUserCode)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        String strMethodDesc = "获取所有考勤年份";
        StringBuilder sbResult = new StringBuilder();

        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select distinct LEFT(CONVERT(varchar(20),B.PSTART,23),4) AS Year from KQRSSZ_1 a inner join KQPERD_1 b on A.Ymonth = B.PID order by Year DESC");

            String strSql = sbSql.ToString();

            //log.Error(strMethodDesc+":"+strSql);

            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);

            strReturnCode = "1";
            strReturnMsg = strMethodDesc+"成功";
            sbResultData.Append(WebCommon.GetJsonStringByDataTable(dt, "\"YearListData\"", false));
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
    /// 获取所有薪资年份
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strUserCode"></param>
    public void GetSalaryYearData(HttpContext context,String strUserCode)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        String strMethodDesc = "获取所有薪资年份";
        StringBuilder sbResult = new StringBuilder();

        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select distinct LEFT(CONVERT(varchar(20),B.PSTART,23),4) AS Year from ");
            sbSql.Append("(SELECT DISTINCT YearMonth FROM PREMPL_1 UNION ALL SELECT DISTINCT YearMonth FROM PREMPL_1_H) a  ");
            sbSql.Append("inner join KQPERD_1 b on A.YearMonth = B.PID  order by Year DESC");

            String strSql = sbSql.ToString();

            //log.Error(strMethodDesc+":"+strSql);

            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);

            strReturnCode = "1";
            strReturnMsg = strMethodDesc+"成功";
            sbResultData.Append(WebCommon.GetJsonStringByDataTable(dt, "\"YearListData\"", false));
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