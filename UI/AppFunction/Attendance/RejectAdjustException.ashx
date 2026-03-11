<%@ WebHandler Language="C#" Class="RejectAdjustException" %>

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
using Com.ValuePlus.Utils.Serializable;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

/// <summary>
/// 拒绝异常调整的处理类
/// </summary>
public class RejectAdjustException : HandlerBase, IHttpHandler, System.Web.SessionState.IRequiresSessionState {

    /// <summary>
    /// 日志声明
    /// </summary>
    protected static Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    public void ProcessRequest (HttpContext context) {
        //解析客户端传递过来的json data
        StreamReader reader = new StreamReader(context.Request.InputStream);
        String strParamJson = HttpUtility.UrlDecode(reader.ReadToEnd());
        //log.Error("传入参数的Json data字符串:" + strParamJson.ToString());

        //string strParam = WebCommon.GetJsonValue(strParamJson,"param").ToString();//请求类型参数

        String strUrlQueryString = context.Server.UrlDecode(context.Request.Url.Query.ToString());
        Hashtable hsTableUrlQuery = WebCommon.GetUrlAnalyse(strUrlQueryString);
        string strParam = hsTableUrlQuery["param"] == null ? string.Empty : hsTableUrlQuery["param"].ToString();//param
        string strSectionUserId = hsTableUrlQuery["sectionuserid"] == null ? string.Empty : hsTableUrlQuery["sectionuserid"].ToString();////考勤员用户ID
        string strUserId = hsTableUrlQuery["loginuserid"] == null ? string.Empty : hsTableUrlQuery["loginuserid"].ToString();////操作用户ID
        string strYearMonth = hsTableUrlQuery["yearmonth"] == null ? string.Empty : hsTableUrlQuery["yearmonth"].ToString();//考勤周期期间
        string strUserType = hsTableUrlQuery["usertype"] == null ? "" : hsTableUrlQuery["usertype"].ToString();//排班操作用户类型（0或者空为小部门用户,1为人事部）
        strUserType = String.IsNullOrEmpty(strUserType) ? "0" : strUserType;
        string strStaffNo = hsTableUrlQuery["staffno"] == null ? string.Empty : hsTableUrlQuery["staffno"].ToString();//员工工号
        string strOneDay = hsTableUrlQuery["oneday"] == null ? string.Empty : hsTableUrlQuery["oneday"].ToString();//日期
        string strRejectIdea = hsTableUrlQuery["rejectidea"] == null ? string.Empty : hsTableUrlQuery["rejectidea"].ToString();//不同意的意见
        //string strRejectIdea = WebCommon.GetJsonValue(strParamJson, "rejectidea").ToString();//不同意的意见

        if (String.IsNullOrEmpty(strUserId)){
            strUserId = this.GetUserCode();
        }

        switch (strParam.ToLower().ToString())
        {
            case "saverejectidea":
                context.Response.Write(this.SaveRejectIdea(strSectionUserId,strStaffNo,strOneDay,strYearMonth, strRejectIdea,strUserId).ToString());
                break;
        }
    }

    /// <summary>
    /// 保存不同意异常调整意见
    /// </summary>
    /// <param name="strSectionUserId"></param>
    /// <param name="strStaffNo"></param>
    /// <param name="strOneDay"></param>
    /// <param name="strYearMonth"></param>
    /// <param name="strRejectIdea"></param>
    /// <param name="strUserId"></param>
    /// <returns></returns>
    private string SaveRejectIdea(String strSectionUserId, String strStaffNo, String strOneDay,String strYearMonth,String strRejectIdea,String strUserId)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbReturnRowData = new StringBuilder();
        StringBuilder sbReturnColumnData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "保存不同意异常调整意见";

        try
        {
            strRejectIdea = strRejectIdea.Replace("'","''");

            String strSpName = "USP_KQ_SaveRejectExceptionIdea";
            Hashtable hsTableParam = new Hashtable();
            hsTableParam.Add("LoginUserId", strUserId);
            hsTableParam.Add("SectionUserId", strSectionUserId);
            hsTableParam.Add("StaffNo", strStaffNo);
            hsTableParam.Add("KQDate", strOneDay);
            hsTableParam.Add("YearMonth", strYearMonth);
            hsTableParam.Add("RejectIdea", strRejectIdea);
            int iSPReturn = SqlParamDao.ExcuteSP(strSpName, hsTableParam);

            strReturnCode = iSPReturn.ToString();
            if(iSPReturn == 1){
                strReturnMsg = strMethodDesc + "成功";
            }else{
                strReturnMsg = strMethodDesc + "失败，请稍后重试!";
            }
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = strMethodDesc + "出错";
            log.Error(strReturnMsg+ex);
        }
        finally
        {
            StringBuilder sbResultStatus = new StringBuilder();
            sbResultStatus.Append("{");
            sbResultStatus.Append("\"ReturnCode\":\"" + strReturnCode.ToString() + "\"");
            sbResultStatus.Append(",\"ReturnMsg\":\"" + strReturnMsg.ToString() + "\"");

            sbResult.Append(sbResultStatus.ToString());
            if (!String.IsNullOrEmpty(sbReturnRowData.ToString()))
            {
                sbResult.Append(",\"ReturnRowData\"" + sbReturnRowData.ToString()+"");
            }
            sbResult.Append("}");
            //log.Error(strMethodDesc + "Return Json:" + sbResult.ToString());
        }
        return sbResult.ToString();
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}