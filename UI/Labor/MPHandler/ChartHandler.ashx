<%@ WebHandler Language="C#" Class="ChartHandler" %>

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
using Com.ValuePlus.Labor;

public class ChartHandler : HandlerBase,IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    public void ProcessRequest (HttpContext context) {
        //解析客户端传递过来的json data
        StreamReader reader = new StreamReader(context.Request.InputStream);
        String strParamJson = HttpUtility.UrlDecode(reader.ReadToEnd());
        log.Error("传入参数的Json data字符串:" + strParamJson.ToString());

        string param = WebCommon.GetJsonValue(strParamJson,"param").ToString();//请求类型参数
        string strUserCode = WebCommon.GetJsonValue(strParamJson,"usercode").ToString();//用户名
        string strUserType = WebCommon.GetJsonValue(strParamJson,"usertype").ToString();//用户类型
        string strCompanyCode = WebCommon.GetJsonValue(strParamJson,"companycode").ToString();//公司编码

        string strTimeSummaryType = WebCommon.GetJsonValue(strParamJson,"timesummarytype").ToString();//统计时间类型【day/month/year】
        string strTimeRange = WebCommon.GetJsonValue(strParamJson,"timerange").ToString();//统计时间范围幅度
        string strTimeScope = WebCommon.GetJsonValue(strParamJson,"timescope").ToString();//统计时间范围数组
        string strTimeFrom = WebCommon.GetJsonValue(strParamJson,"timefrom").ToString();//统计时间起始
        string strTimeTo = WebCommon.GetJsonValue(strParamJson,"timeto").ToString();//统计时间结束

        string strPostDataObject = WebCommon.GetJsonObjectValue(strParamJson,"postdataobj").ToString();//Json数据库对象
        string strIsEscape = WebCommon.GetJsonValue(strParamJson,"isescape").ToString();//是否
        bool isEscape = strIsEscape.Equals("false") ? false : true;

        ///根据类型及其对应的起始结束时间点获取规范格式的起始结束日期yyyy-MM-dd
        String strDateFrom = strTimeFrom;
        String strDateTo = strTimeTo;
        switch (strTimeSummaryType) {
            case "day"://格式为2020-03-12
                strDateFrom = strTimeFrom;
                strDateTo = strTimeTo;
                break;
            case "month"://格式为2020-03
                strDateFrom = strTimeFrom+"-01";
                strDateTo = DateTime.Parse(strTimeTo+"-01").AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
                break;
            case "year"://格式为2020
                strDateFrom = strTimeFrom+"-01-01";
                strDateTo = strTimeTo+"-12-31";
                break;
        }
        ///根据类型及其对应的起始结束时间点获取规范格式的起始结束日期yyyy-MM-dd

        switch (param.ToLower().ToString())
        {
            case "getchartdataorderamount":
                this.GetChartDataOrderAmount(context, strUserCode, strTimeSummaryType,strTimeRange,strDateFrom,strDateTo);
                break;
            case "getchartdataorderamount_allitem":
                this.GetChartDataDeptOrderAmount_AllItem(context, strUserCode, strTimeSummaryType,strCompanyCode,strDateFrom,strDateTo);
                break;
            case "getchartdataorderamount_alldept":
                this.GetChartDataDeptOrderAmount_AllDept(context, strUserCode, strTimeSummaryType,strCompanyCode,strDateFrom,strDateTo);
                break;
        }

    }

    /// <summary>
    /// 获取日期范围内的工单费用集合
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strUserCode"></param>
    /// <param name="strTimeSummaryType"></param>
    /// <param name="strTimeRange"></param>
    /// <param name="strDateFrom"></param>
    /// <param name="strDateTo"></param>
    public void GetChartDataOrderAmount(HttpContext context, String strUserCode,String strTimeSummaryType,String strTimeRange,String strDateFrom,String strDateTo)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();

        StringBuilder sbResult = new StringBuilder();
        StringBuilder sbSql = new StringBuilder();
        try
        {
            Hashtable hsTableParams = new Hashtable();
            hsTableParams.Add("UserCode",strUserCode);
            hsTableParams.Add("EmployCompany","");
            hsTableParams.Add("DeptCode","");
            hsTableParams.Add("EmployItem","");
            hsTableParams.Add("ServiceCompany","");
            hsTableParams.Add("WorkOrderDateFrom",strDateFrom);
            hsTableParams.Add("WorkOrderDateTo",strDateTo);
            log.Error("获取日期范围内" + strTimeSummaryType + "的工单费用集合strDateFrom:" + strDateFrom + "到strDateTo:" + strDateTo);

            decimal nMaxValue = 0;
            Hashtable hsTableReturn = (Hashtable)this.GetSeriesDataPartArray(hsTableParams,strDateFrom,strDateTo,strUserCode, strTimeSummaryType,ref nMaxValue);
            String strData = hsTableReturn["SummaryTotalCost"].ToString();

            sbResultData.Append("\"ResultData\":{");
            sbResultData.Append("\"categories\": []");
            sbResultData.Append(",\"series\":[{");
            sbResultData.Append("   \"name\":\"工单费用\"");
            sbResultData.Append("   ,\"data\":"+strData+"");
            sbResultData.Append("}]");
            sbResultData.Append(",\"maxValue\": "+nMaxValue.ToString()+"");
            sbResultData.Append("}");


            strReturnCode = "1";
            strReturnMsg = "获取日期范围内的工单费用集合成功";
            log.Error("获取日期范围内的工单费用集合成功:" + sbResultData.ToString());
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = "获取日期范围内的工单费用集合出错";
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
        ////格式如下：
        //ChartData_OrderAmount:{//column饼状图数据格式
        //	"categories": ["03", "04", "05", "06", "07", "08", "09", "10", "11"],
        //	"series": [{
        //		"name": "工单费用",
        //		"data": [1500,2000, 4500, 3700, 4300, 3400, 3700, 4300, 3400],
        //		},
        //	],
        //	"maxValue":5000,
        //},
    }
        
    /// <summary>
    /// 获取日期范围内各种用工类型的工单费用集合
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strUserCode"></param>
    /// <param name="strTimeSummaryType"></param>
    /// <param name="strCompanyCode"></param>
    /// <param name="strDateFrom"></param>
    /// <param name="strDateTo"></param>
    public void GetChartDataDeptOrderAmount_AllItem(HttpContext context, String strUserCode,String strTimeSummaryType,String strCompanyCode,String strDateFrom,String strDateTo)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();

        StringBuilder sbResult = new StringBuilder();
        StringBuilder sbSql = new StringBuilder();
        try
        {
            Hashtable hsTableParams = new Hashtable();
            hsTableParams.Add("UserCode",strUserCode);
            hsTableParams.Add("EmployCompany","");
            hsTableParams.Add("DeptCode","");
            hsTableParams.Add("EmployItem","");
            hsTableParams.Add("ServiceCompany","");
            hsTableParams.Add("WorkOrderDateFrom",strDateFrom);
            hsTableParams.Add("WorkOrderDateTo",strDateTo);
            log.Error("获取日期范围内" + strTimeSummaryType + "的各用工类型的工单费用集合strDateFrom:" + strDateFrom + "到strDateTo:" + strDateTo);

            decimal nMaxValue = 0;
            //获取当前单位的部门列表
            String strSql_DeptList = "select * from TB_HRLSTD WHERE LID = 'EmployItem'";
            DataTable dt_DeptList = SqlParamDao.GetDataTableBySql(strSql_DeptList);
            StringBuilder sbSeries_SummaryTotalCost = new StringBuilder();
            StringBuilder sbSeries_SummaryWorkTimeQty = new StringBuilder();
            sbSeries_SummaryTotalCost.Append("\"series_TotalCost\":[");
            sbSeries_SummaryWorkTimeQty.Append("\"series_WorkTimeQty\":[");

            if (dt_DeptList != null && dt_DeptList.Rows.Count > 0)
            {
                for(int i = 0; i < dt_DeptList.Rows.Count; i++)
                {
                    DataRow dr = dt_DeptList.Rows[i];
                    String strCID = dr["CID"].ToString();
                    String strCDESCCHS = dr["CDESCCHS"].ToString();

                    hsTableParams.Remove("EmployItem");
                    hsTableParams.Add("EmployItem",strCID);
                    DataTable dt = LWorkOrder.GetWorkOrderSummaryData(strUserCode,hsTableParams);
                    Decimal nSummaryTotalCost = (Decimal)dt.Rows[0]["SummaryTotalCost"];
                    int nSummaryWorkTimeQty = (int)dt.Rows[0]["SummaryWorkTimeQty"];

                    nMaxValue = nMaxValue < nSummaryTotalCost ? nSummaryTotalCost : nMaxValue;

                    Hashtable hsTableReturn = (Hashtable)this.GetSeriesDataPartArray(hsTableParams,strDateFrom,strDateTo,strUserCode, strTimeSummaryType,ref nMaxValue);
                    String strData_SummaryTotalCost = hsTableReturn["SummaryTotalCost"].ToString();
                    String strData_SummaryWorkTimeQty = hsTableReturn["SummaryWorkTimeQty"].ToString();

                    sbSeries_SummaryTotalCost.Append(i==0?"{":",{");
                    sbSeries_SummaryTotalCost.Append("   \"name\":\""+strCDESCCHS+"\"");
                    sbSeries_SummaryTotalCost.Append("   ,\"data\":"+strData_SummaryTotalCost+"");
                    sbSeries_SummaryTotalCost.Append("   }");

                    sbSeries_SummaryWorkTimeQty.Append(i == 0 ? "{" : ",{");
                    sbSeries_SummaryWorkTimeQty.Append("   \"name\":\"" + strCDESCCHS + "\"");
                    sbSeries_SummaryWorkTimeQty.Append("   ,\"data\":" + strData_SummaryWorkTimeQty + "");
                    sbSeries_SummaryWorkTimeQty.Append("   }");
                }
            }
            sbSeries_SummaryTotalCost.Append("]");
            sbSeries_SummaryWorkTimeQty.Append("]");


            sbResultData.Append("\"ResultData\":{");
            sbResultData.Append("\"categories\": []");
            sbResultData.Append("," + sbSeries_SummaryTotalCost.ToString());
            sbResultData.Append("," + sbSeries_SummaryWorkTimeQty.ToString());
            sbResultData.Append(",\"maxValue\": "+nMaxValue.ToString()+"");
            sbResultData.Append("}");


            strReturnCode = "1";
            strReturnMsg = "获取日期范围内各种用工类型的工单费用集合成功";
            log.Error("获取日期范围内各种用工类型的工单费用集合成功:" + sbResultData.ToString());
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = "获取日期范围内各种用工类型的工单费用集合出错";
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
        log.Error("获取日期范围内各种用工类型的工单费用集合时返回json数据:" + sbResult.ToString());
    }

    /// <summary>
    /// 获取日期范围内各部门的工单费用集合
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strUserCode"></param>
    /// <param name="strTimeSummaryType"></param>
    /// <param name="strCompanyCode"></param>
    /// <param name="strDateFrom"></param>
    /// <param name="strDateTo"></param>
    public void GetChartDataDeptOrderAmount_AllDept(HttpContext context, String strUserCode,String strTimeSummaryType,String strCompanyCode,String strDateFrom,String strDateTo)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();

        StringBuilder sbResult = new StringBuilder();
        StringBuilder sbSql = new StringBuilder();
        try
        {
            Hashtable hsTableParams = new Hashtable();
            hsTableParams.Add("UserCode",strUserCode);
            hsTableParams.Add("EmployCompany","");
            hsTableParams.Add("DeptCode","");
            hsTableParams.Add("EmployItem","");
            hsTableParams.Add("ServiceCompany","");
            hsTableParams.Add("WorkOrderDateFrom",strDateFrom);
            hsTableParams.Add("WorkOrderDateTo",strDateTo);
            log.Error("获取日期范围内" + strTimeSummaryType + "的各部门的工单费用集合strDateFrom:" + strDateFrom + "到strDateTo:" + strDateTo);

            decimal nMaxValue = 0;
            //获取当前单位的部门列表
            String strSql_DeptList = "select * from LCompany_2 where CompanyCode = '"+strCompanyCode+"'";
            DataTable dt_DeptList = SqlParamDao.GetDataTableBySql(strSql_DeptList);
            StringBuilder sbSeries_SummaryTotalCost = new StringBuilder();
            StringBuilder sbSeries_SummaryWorkTimeQty = new StringBuilder();
            sbSeries_SummaryTotalCost.Append("\"series_TotalCost\":[");
            sbSeries_SummaryWorkTimeQty.Append("\"series_WorkTimeQty\":[");

            if (dt_DeptList != null && dt_DeptList.Rows.Count > 0)
            {
                for(int i = 0; i < dt_DeptList.Rows.Count; i++)
                {
                    DataRow dr = dt_DeptList.Rows[i];
                    String strDeptCode = dr["DeptCode"].ToString();
                    String strDeptName = dr["DeptName"].ToString();

                    hsTableParams.Remove("DeptCode");
                    hsTableParams.Add("DeptCode",strDeptCode);
                    DataTable dt = LWorkOrder.GetWorkOrderSummaryData(strUserCode,hsTableParams);
                    Decimal nSummaryTotalCost = (Decimal)dt.Rows[0]["SummaryTotalCost"];
                    int nSummaryWorkTimeQty = (int)dt.Rows[0]["SummaryWorkTimeQty"];

                    nMaxValue = nMaxValue < nSummaryTotalCost ? nSummaryTotalCost : nMaxValue;

                    Hashtable hsTableReturn = (Hashtable)this.GetSeriesDataPartArray(hsTableParams,strDateFrom,strDateTo,strUserCode, strTimeSummaryType,ref nMaxValue);
                    String strData_SummaryTotalCost = hsTableReturn["SummaryTotalCost"].ToString();
                    String strData_SummaryWorkTimeQty = hsTableReturn["SummaryWorkTimeQty"].ToString();

                    sbSeries_SummaryTotalCost.Append(i==0?"{":",{");
                    sbSeries_SummaryTotalCost.Append("   \"name\":\""+strDeptName+"\"");
                    sbSeries_SummaryTotalCost.Append("   ,\"data\":"+strData_SummaryTotalCost+"");
                    sbSeries_SummaryTotalCost.Append("   }");

                    sbSeries_SummaryWorkTimeQty.Append(i == 0 ? "{" : ",{");
                    sbSeries_SummaryWorkTimeQty.Append("   \"name\":\"" + strDeptName + "\"");
                    sbSeries_SummaryWorkTimeQty.Append("   ,\"data\":" + strData_SummaryWorkTimeQty + "");
                    sbSeries_SummaryWorkTimeQty.Append("   }");
                }
            }
            sbSeries_SummaryTotalCost.Append("]");
            sbSeries_SummaryWorkTimeQty.Append("]");


            sbResultData.Append("\"ResultData\":{");
            sbResultData.Append("\"categories\": []");
            sbResultData.Append("," + sbSeries_SummaryTotalCost.ToString());
            sbResultData.Append("," + sbSeries_SummaryWorkTimeQty.ToString());
            sbResultData.Append(",\"maxValue\": "+nMaxValue.ToString()+"");
            sbResultData.Append("}");


            strReturnCode = "1";
            strReturnMsg = "获取日期范围内各部门的工单费用集合成功";
            log.Error("获取日期范围内各部门的工单费用集合成功:" + sbResultData.ToString());
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = "获取日期范围内各部门的工单费用集合出错";
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
        log.Error("获取日期范围内各部门的工单费用集合时返回json数据:" + sbResult.ToString());
    }

    /// <summary>
    /// 遍历起始结束时间点获取对应的值数值
    /// </summary>
    /// <param name="strDateFrom"></param>
    /// <param name="strDateTo"></param>
    /// <param name="strUserCode"></param>
    /// <param name="strTimeSummaryType"></param>
    /// <param name="nMaxValue"></param>
    /// <returns></returns>
    private Hashtable GetSeriesDataPartArray(Hashtable hsTableParams,String strDateFrom,String strDateTo,String strUserCode,String strTimeSummaryType, ref decimal nMaxValue)
    {
        Hashtable hsTableReturn = new Hashtable();
        log.Error("遍历起始结束时间点获取对应的值数值，起始时间:" + strDateFrom+"---结束时间："+strDateTo+"");
        //从起始时间遍历到结束时间
        int i = 0;
        String strTempDateFrom = strDateFrom;
        String strTempDateTo = "";
        String strData_SummaryTotalCost = "[";//格式[100,2003,223,222]
        String strData_SummaryWorkTimeQty = "[";

        while (DateTime.Parse(strTempDateFrom) <= DateTime.Parse(strDateTo))
        {
            switch (strTimeSummaryType) {
                case "day":
                    strTempDateTo = strTempDateFrom;
                    break;
                case "month":
                    strTempDateTo = DateTime.Parse(strTempDateFrom).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
                    break;
                case "year":
                    strTempDateTo = strTempDateFrom.Substring(0,4)+"-12-31";
                    break;
            }

            //log.Error("获取日期范围内"+strTimeSummaryType+"的工单费用集合strTempDateFrom:"+strTempDateFrom+"到strTempDateTo:"+strTempDateTo);

            hsTableParams.Remove("WorkOrderDateFrom");
            hsTableParams.Remove("WorkOrderDateTo");
            hsTableParams.Add("WorkOrderDateFrom",strTempDateFrom);
            hsTableParams.Add("WorkOrderDateTo",strTempDateTo);
            DataTable dt = LWorkOrder.GetWorkOrderSummaryData(strUserCode,hsTableParams);
            DataRow dr = dt.Rows[0];
            Decimal nSummaryTotalCost = (Decimal)dr["SummaryTotalCost"];
            int nSummaryWorkTimeQty = (int)dr["SummaryWorkTimeQty"];

            strData_SummaryTotalCost = i == 0 ? strData_SummaryTotalCost + nSummaryTotalCost.ToString() : strData_SummaryTotalCost + "," + nSummaryTotalCost.ToString();
            strData_SummaryWorkTimeQty = i == 0 ? strData_SummaryWorkTimeQty + nSummaryWorkTimeQty.ToString() : strData_SummaryWorkTimeQty + "," + nSummaryWorkTimeQty.ToString();

            nMaxValue = nMaxValue < nSummaryTotalCost ? nSummaryTotalCost : nMaxValue;

            switch (strTimeSummaryType) {
                case "day":
                    strTempDateFrom = DateTime.Parse(strTempDateFrom).AddDays(1).ToString("yyyy-MM-dd");
                    break;
                case "month":
                    strTempDateFrom = DateTime.Parse(strTempDateFrom).AddMonths(1).ToString("yyyy-MM-dd");
                    break;
                case "year":
                    strTempDateFrom = DateTime.Parse(strTempDateFrom).AddYears(1).ToString("yyyy-MM-dd");
                    break;
            }
            i++;
        }
        strData_SummaryTotalCost = strData_SummaryTotalCost + "]";
        strData_SummaryWorkTimeQty = strData_SummaryWorkTimeQty + "]";

        hsTableReturn.Remove("SummaryTotalCost");
        hsTableReturn.Remove("SummaryWorkTimeQty");
        hsTableReturn.Add("SummaryTotalCost",strData_SummaryTotalCost);
        hsTableReturn.Add("SummaryWorkTimeQty", strData_SummaryWorkTimeQty);

        log.Error("遍历起始结束时间点获取对应的SummaryTotalCost数值:" + strData_SummaryTotalCost.ToString());
        log.Error("遍历起始结束时间点获取对应的SummaryWorkTimeQty数值:" + strData_SummaryWorkTimeQty.ToString());
        return hsTableReturn;

    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}