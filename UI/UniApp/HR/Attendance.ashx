<%@ WebHandler Language="C#" Class="Attendance" %>

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

public class Attendance : HandlerBase,IHttpHandler, System.Web.SessionState.IRequiresSessionState
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
        string strClockTime = WebCommon.GetJsonValue(strParamJson,"clocktime").ToString();//打卡时间
        string strClockType = WebCommon.GetJsonValue(strParamJson,"clocktype").ToString();//打卡类型
        string strSaveShiftData = WebCommon.GetJsonObjectValue(strParamJson,"saveshiftdata").ToString();//需保存的排班信息数据

        String strPageSize = WebCommon.GetJsonValue(strParamJson,"pagesize").ToString();
        String strPageIndex = WebCommon.GetJsonValue(strParamJson,"pageindex").ToString();
        int iPageSize = String.IsNullOrEmpty(strPageSize)? 10 : int.Parse(strPageSize);
        int iPageIndex = String.IsNullOrEmpty(strPageSize)? 0 : int.Parse(strPageIndex);

        String strRequestLanguage = strCulture.Equals("0") ? "zh-cn" : "en-us";
        switch (param.ToLower().ToString())
        {
            case "getshiftinfobystaffdate"://通过员工编号及日期获取排班班次信息
                this.GetShiftInfoByStaffDate(context,strDCNO,strOneDate,strUserCode);
                break;
            case "getleavebalancebystaffdate"://通过员工编号及日期获取假期余额数据信息
                this.GetLeaveBalanceByStaffDate(context,strDCNO,strOneDate,strUserCode);
                break;
            case "getotlvsummarybystaffyearmonth"://通过员工编号及期间获取加班休假小时数汇总数据
                this.GetOTLVSummaryByStaffYearMonth(context,strDCNO,strYearMonth,strUserCode);
                break;
            case "getclockrecordbystaff"://通过员工编号及期间范围获取打卡记录
                this.GetClockRecordByStaff(context,strDCNO,strDateFrom,strDateTo,strClockType,strUserCode);
                break;
            case "getstaffonedayisattlocked"://通过工号和日期获取当天考勤是否被锁定
                this.GetStaffOneDayIsAttLocked(context,strDCNO,strYearMonth,strOneDate,strUserCode);
                break;
            case "recordstaffclock"://记录员工打卡记录
                this.RecordStaffClock(context,strDCNO,strClockTime,strClockType,strUserCode);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 通过员工编号及日期获取排班班次信息
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strDCNO"></param>
    /// <param name="strOneDate"></param>
    /// <param name="strUserCode"></param>
    public void GetShiftInfoByStaffDate(HttpContext context,String strDCNO,String strOneDate,String strUserCode)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        String strMethodDesc = "通过员工编号及日期获取排班班次信息";
        StringBuilder sbResult = new StringBuilder();

        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("SELECT * from [dbo].[Fun_MB_HR_GetShiftInfoByStaffDate]('"+strDCNO+"','"+strOneDate+"','"+strUserCode+"') ");
            String strSql = sbSql.ToString();

            //log.Error(strMethodDesc+":"+strSql);

            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);

            strReturnCode = "1";
            strReturnMsg = strMethodDesc+"成功";
            sbResultData.Append(WebCommon.GetJsonStringByDataTable(dt, "\"ShiftInfo\"", false));
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
    /// 通过员工编号及日期获取假期余额数据信息
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strDCNO"></param>
    /// <param name="strOneDate"></param>
    /// <param name="strUserCode"></param>
    public void GetLeaveBalanceByStaffDate(HttpContext context,String strDCNO,String strOneDate,String strUserCode)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        String strMethodDesc = "通过员工编号及日期获取假期余额数据信息";
        StringBuilder sbResult = new StringBuilder();

        try
        {
            //add by sammen 20240325 先重新计算仅几个月的加班休假余额数据
            String strSQL_ExecuteSP = "USP_MB_ComputeOLBalance_ByStaff";
            Hashtable hsTableParam = new Hashtable();
            hsTableParam.Add("StaffNo", strDCNO);
            hsTableParam.Add("UserCode", strUserCode);
            int iReturnValue = SqlParamDao.ExcuteSP(strSQL_ExecuteSP, hsTableParam);
            //add by sammen 20240325 先重新计算年假余额数据
            strSQL_ExecuteSP = "USP_MB_ComputeAnnualLeave_ByStaff";
            hsTableParam = new Hashtable();
            hsTableParam.Add("StaffNo", strDCNO);
            hsTableParam.Add("UserCode", strUserCode);
            hsTableParam.Add("InputDate", "");
            iReturnValue = SqlParamDao.ExcuteSP(strSQL_ExecuteSP, hsTableParam);

            //计算完成后再获取数据
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("SELECT * from [dbo].[Fun_MB_HR_GetLeaveBalanceByStaff]('"+strDCNO+"','"+strOneDate+"','"+strUserCode+"') ");
            String strSql = sbSql.ToString();

            //log.Error(strMethodDesc+":"+strSql);

            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);

            strReturnCode = "1";
            strReturnMsg = strMethodDesc+"成功";
            sbResultData.Append(WebCommon.GetJsonStringByDataTable(dt, "\"LeaveBalanceInfo\"", false));
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
    /// 通过员工编号及期间获取加班休假小时数汇总数据
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strDCNO"></param>
    /// <param name="strOneDate"></param>
    /// <param name="strUserCode"></param>
    public void GetOTLVSummaryByStaffYearMonth(HttpContext context,String strDCNO,String strYearMonth,String strUserCode)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        String strMethodDesc = "通过员工编号及期间获取加班休假小时数汇总数据";
        StringBuilder sbResult = new StringBuilder();

        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("SELECT * from [dbo].[Fun_MB_HR_GetOTLVSummaryByStaff]('"+strDCNO+"','"+strYearMonth+"','"+strUserCode+"') ");
            String strSql = sbSql.ToString();

            //log.Error(strMethodDesc+":"+strSql);

            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);

            strReturnCode = "1";
            strReturnMsg = strMethodDesc+"成功";
            sbResultData.Append(WebCommon.GetJsonStringByDataTable(dt, "\"OTLVSummaryInfo\"", false));
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
    /// 通过员工编号及期间范围获取打卡记录
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strDCNO"></param>
    /// <param name="strDateFrom"></param>
    /// <param name="strDateTo"></param>
    /// <param name="strClockType"></param>
    /// <param name="strUserCode"></param>
    public void GetClockRecordByStaff(HttpContext context,String strDCNO,String strDateFrom,String strDateTo,String strClockType,String strUserCode)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        String strMethodDesc = "通过员工编号及期间范围获取打卡记录";
        StringBuilder sbResult = new StringBuilder();

        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("SELECT * from [dbo].[Fun_MB_HR_GetClockRecordByStaff]('"+strDCNO+"','"+strClockType+"','"+strDateFrom+"','"+strDateTo+"','"+strUserCode+"') ");
            String strSql = sbSql.ToString();

            //log.Error(strMethodDesc+":"+strSql);

            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);

            strReturnCode = "1";
            strReturnMsg = strMethodDesc+"成功";
            sbResultData.Append(WebCommon.GetJsonStringByDataTable(dt, "\"ClockRecordInfo\"", false));
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
    /// 通过工号和日期获取当天考勤是否被锁定
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strDCNO"></param>
    /// <param name="strOneDate"></param>
    /// <param name="strUserCode"></param>
    public void GetStaffOneDayIsAttLocked(HttpContext context,String strDCNO,String strYearMonth,String strOneDate,String strUserCode)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        String strMethodDesc = "通过工号和日期获取当天考勤是否被锁定";
        StringBuilder sbResult = new StringBuilder();

        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("SELECT * from [dbo].[Fun_MB_HR_GetStaffOneDayIsAttLocked]('"+strDCNO+"','"+strYearMonth+"','"+strOneDate+"','"+strUserCode+"') ");
            String strSql = sbSql.ToString();

            //log.Error(strMethodDesc+":"+strSql);

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
                sbResult.Append(","+sbResultData.ToString()+"");
            }
            sbResult.Append("}");
        }
        //log.Error(strMethodDesc+"时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }

    /// <summary>
    /// 记录员工打卡记录
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strDCNO"></param>
    /// <param name="strClockTime"></param>
    /// <param name="strUserCode"></param>
    public void RecordStaffClock(HttpContext context,String strDCNO,String strClockTime,String strClockType,String strUserCode)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        String strMethodDesc = "记录员工打卡记录";
        StringBuilder sbResult = new StringBuilder();

        try
        {
            String strSpName = "USP_MB_RecordStaffClock";
            Hashtable hsTableParam = new Hashtable();
            hsTableParam.Add("StaffNo", strDCNO);
            hsTableParam.Add("ClockTime", strClockTime);
            hsTableParam.Add("ClockType", strClockType);
            hsTableParam.Add("UserCode", strUserCode);
            //log.Error("记录员工打卡记录：exec " + strSpName + " '" + strDCNO + "', '" + strClockTime + "','" + strClockType + "','" + strUserCode + "'");
            int iSPReturn = SqlParamDao.ExcuteSP(strSpName, hsTableParam);

            strReturnCode = "1";
            strReturnMsg = strMethodDesc+"成功";
            //sbResultData.Append("");
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