<%@ WebHandler Language="C#" Class="TimelineOTLV" %>

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

public class TimelineOTLV : HandlerBase,IHttpHandler, System.Web.SessionState.IRequiresSessionState
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
        string strYear = WebCommon.GetJsonValue(strParamJson,"year").ToString();
        string strYearMonth = WebCommon.GetJsonValue(strParamJson,"yearmonth").ToString();
        string strDCNO = WebCommon.GetJsonValue(strParamJson,"dcno").ToString();
        string strOneDate = WebCommon.GetJsonValue(strParamJson,"ondate").ToString();
        string strDateFrom = WebCommon.GetJsonValue(strParamJson,"datefrom").ToString();
        string strDateTo = WebCommon.GetJsonValue(strParamJson,"dateto").ToString();
        string strOTType = WebCommon.GetJsonValue(strParamJson,"ottype").ToString();//加班类型
        string strOTPayType = WebCommon.GetJsonValue(strParamJson,"otpaytype").ToString();//加班单支付类型
        string strOTStatus = WebCommon.GetJsonValue(strParamJson,"otstatus").ToString();//加班单状态
        string strIsLast = WebCommon.GetJsonValue(strParamJson,"islast").ToString();//是否只是最后一条[1是0否]
        string strSaveShiftData = WebCommon.GetJsonObjectValue(strParamJson,"saveshiftdata").ToString();//需保存的排班信息数据

        String strPageSize = WebCommon.GetJsonValue(strParamJson,"pagesize").ToString();
        String strPageIndex = WebCommon.GetJsonValue(strParamJson,"pageindex").ToString();
        int iPageSize = String.IsNullOrEmpty(strPageSize)? 10 : int.Parse(strPageSize);
        int iPageIndex = String.IsNullOrEmpty(strPageSize)? 0 : int.Parse(strPageIndex);

        String strRequestLanguage = strCulture.Equals("0") ? "zh-cn" : "en-us";
        switch (param.ToLower().ToString())
        {
            case "getotlvbalancetimeline"://获取加班休假余额时间线记录
                this.GetOTLVBalanceTimeline(context,strDCNO,strYear,strIsLast,strUserCode);
                break;
            case "getalbalancetimeline"://获取年级假余额时间线记录
                this.GetALBalanceTimeline(context,strDCNO,strIsLast,strUserCode);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 获取加班休假余额时间线记录
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strDCNO"></param>
    /// <param name="strYear"></param>
    /// <param name="strIsLast">是否只是最后一条[1是0否]</param>
    /// <param name="strUserCode"></param>
    public void GetOTLVBalanceTimeline(HttpContext context,String strDCNO,String strYear,String strIsLast,String strUserCode)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        String strMethodDesc = "获取加班休假余额时间线记录";
        StringBuilder sbResult = new StringBuilder();

        try
        {
            //add by sammen 20240325 获取时间线数据前先重新计算仅几个月的余额数据【首页Index已经计算，此处先取消】
            //String strSQL_ExecuteSP = "USP_MB_ComputeOLBalance_ByStaff";
            //Hashtable hsTableParam = new Hashtable();
            //hsTableParam.Add("StaffNo", strDCNO);
            //hsTableParam.Add("UserCode", strUserCode);
            //int iReturnValue = SqlParamDao.ExcuteSP(strSQL_ExecuteSP, hsTableParam);

            //计算完成后再获取数据
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("SELECT "+(strIsLast.Equals("1")?" top 1 ":""));
            sbSql.Append(" * from [VW_MB_Timeline_OTLVBalance] where 1=1 ");
            if(!String.IsNullOrEmpty(strDCNO)){
                sbSql.Append(" and StaffNo = '"+strDCNO+"'");
            }
            if(!String.IsNullOrEmpty(strYear)){
                sbSql.Append(" and '20'+left(YearMonth,2) = '"+strYear+"'");
            }
            sbSql.Append(" order by StaffNo,YearMonth desc");
            String strSql = sbSql.ToString();

            //log.Error(strMethodDesc+":"+strSql);

            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);

            strReturnCode = "1";
            strReturnMsg = strMethodDesc+"成功";
            sbResultData.Append(WebCommon.GetJsonStringByDataTable(dt, "\"OTLVBalanceTimeline\"", false));
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
    /// 获取年假余额时间线记录
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strDCNO"></param>
    /// <param name="strIsLast">是否只是最后一条[1是0否]</param>
    /// <param name="strUserCode"></param>
    public void GetALBalanceTimeline(HttpContext context,String strDCNO,String strIsLast,String strUserCode)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        String strMethodDesc = "获取年假余额时间线记录";
        StringBuilder sbResult = new StringBuilder();

        try
        {
            //add by sammen 20240325 获取时间线数据前先重新计算年假余额数据【首页Index已经计算，此处先取消】
            //String strSQL_ExecuteSP = "USP_MB_ComputeAnnualLeave_ByStaff";
            //Hashtable hsTableParam = new Hashtable();
            //hsTableParam.Add("StaffNo", strDCNO);
            //hsTableParam.Add("UserCode", strUserCode);
            //hsTableParam.Add("InputDate", "");
            //int iReturnValue = SqlParamDao.ExcuteSP(strSQL_ExecuteSP, hsTableParam);

            //计算完成后再获取数据
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("SELECT "+(strIsLast.Equals("1")?" top 1 ":""));
            sbSql.Append(" * from [VW_MB_Timeline_ALBalance] where 1=1 ");
            if(!String.IsNullOrEmpty(strDCNO)){
                sbSql.Append(" and StaffNo = '"+strDCNO+"'");
            }
            sbSql.Append(" order by StaffNo,AYEAR desc");
            String strSql = sbSql.ToString();

            //log.Error(strMethodDesc+":"+strSql);

            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);

            strReturnCode = "1";
            strReturnMsg = strMethodDesc+"成功";
            sbResultData.Append(WebCommon.GetJsonStringByDataTable(dt, "\"ALBalanceTimeline\"", false));
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