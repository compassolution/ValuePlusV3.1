<%@ WebHandler Language="C#" Class="AttLockedHandler" %>

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
/// 排班人天锁定操作的处理类
/// </summary>
public class AttLockedHandler : HandlerBase, IHttpHandler, System.Web.SessionState.IRequiresSessionState {

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
        string strUserId = hsTableUrlQuery["userid"] == null ? string.Empty : hsTableUrlQuery["userid"].ToString();////排班操作用户ID
        string strYearMonth = hsTableUrlQuery["yearmonth"] == null ? string.Empty : hsTableUrlQuery["yearmonth"].ToString();//考勤周期期间
        string strStaffNo = hsTableUrlQuery["staffno"] == null ? string.Empty : hsTableUrlQuery["staffno"].ToString();//员工工号
        string strOneDay = hsTableUrlQuery["oneday"] == null ? string.Empty : hsTableUrlQuery["oneday"].ToString();//日期
        string strIsToLock = hsTableUrlQuery["istolock"] == null ? string.Empty : hsTableUrlQuery["istolock"].ToString();//是否锁定
        if (String.IsNullOrEmpty(strUserId)){
            strUserId = this.GetUserCode();
        }

        switch (strParam.ToLower().ToString())
        {
            case "getattlockedstaffdays":
                context.Response.Write(this.GetAttLockedStaffDays(strUserId,strYearMonth).ToString());
                break;
            case "getattlockedinfo":
                context.Response.Write(this.GetAttLockedInfo(strUserId,strYearMonth,strStaffNo,strOneDay).ToString());
                break;
            case "lockstaffoneday"://锁定/解锁某员工某一天排班锁定
                context.Response.Write(this.LockStaffOneDay(strUserId,strStaffNo,strOneDay,strIsToLock).ToString());
                break;
            case "lockstaffonemonth"://锁定/解锁某员工某一月排班锁定
                context.Response.Write(this.LockStaffOneMonth(strUserId,strStaffNo,strYearMonth,strIsToLock).ToString());
                break;
        }
    }

    /// <summary>
    /// 获取某月份某管辖用户下被锁定的人天
    /// </summary>
    /// <param name="strUserId"></param>
    /// <param name="strYearMonth"></param>
    /// <returns></returns>
    private String GetAttLockedStaffDays(String strUserId,String strYearMonth)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbReturnRowData = new StringBuilder();
        StringBuilder sbReturnColumnData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "获取某月份某管辖用户下被锁定的人天";
        int iRowCount = 0;

        sbReturnRowData.Append("[");
        try
        {
            //delete by sammen 20230117 屏蔽这个判断，不管是否启用员工某天排版后锁定功能，都获取KQRSSZ_2中IsAttLocked = 1的记录集
            String strIsLockStaffDateAtt = Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("Switch_IsLockStaffDateAtt");
            strIsLockStaffDateAtt = String.IsNullOrEmpty(strIsLockStaffDateAtt) ? "0" : strIsLockStaffDateAtt;

            //if(strIsLockStaffDateAtt.Equals("1")){
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select EM_NO+'#'+CONVERT(VARCHAR(20),r_date,23) as StaffDate from KQRSSZ_2 A  \r\n");
            sbSql.Append(" INNER JOIN FUN_VW_PAIBAN_STAFF_FILTER_BYUSERID('"+strYearMonth+"','"+strUserId+"') B ON A.EM_NO = B.STAFFID \r\n");
            sbSql.Append(" WHERE LEFT(A.SEQNO,4) = '"+strYearMonth+"' AND B.SUSERID = '"+strUserId+"' and ISNULL([IsAttLocked],'0') = '1' \r\n");

            String strSql = sbSql.ToString();
            //log.Error(strMethodDesc + " strSql:" + strSql);
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
            iRowCount = dt.Rows.Count;

            //sbReturnRowData.Append(WebCommon.GetJsonStringByDataTable(dt,"",false));

            if (iRowCount > 0)
            {
                for (int i = 0; i < iRowCount; i++)
                {
                    sbReturnRowData.Append(i > 0 ? "," : "");
                    sbReturnRowData.Append("\"" + dt.Rows[i]["StaffDate"].ToString() + "\"");
                }
            }

            //}
            strReturnCode = "1";
            strReturnMsg = strMethodDesc + "成功";
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = strMethodDesc + "出错";
            log.Error(strReturnMsg+ex);
        }
        finally
        {
            sbReturnRowData.Append("]");

            StringBuilder sbResultStatus = new StringBuilder();
            sbResultStatus.Append("{");
            sbResultStatus.Append("\"ReturnCode\":\"" + strReturnCode.ToString() + "\"");
            sbResultStatus.Append(",\"ReturnMsg\":\"" + strReturnMsg.ToString() + "\"");

            sbResult.Append(sbResultStatus.ToString());
            sbResult.Append(",\"ReturnRowData\":" + sbReturnRowData.ToString() + "");
            sbResult.Append("}");
            //log.Error(strMethodDesc + "Return Json:" + sbResult.ToString());
        }
        return sbResult.ToString();
    }



    /// <summary>
    /// 获取某员工某一天是否排班锁定
    /// </summary>
    /// <param name="strUserId"></param>
    /// <param name="strYearMonth"></param>
    /// <param name="strStaffNo"></param>
    /// <param name="strOneDay"></param>
    /// <returns></returns>
    private String GetAttLockedInfo(String strUserId,String strYearMonth,String strStaffNo,String strOneDay)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbReturnRowData = new StringBuilder();
        StringBuilder sbReturnColumnData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "获取某员工某一天是否排班锁定";
        int iRowCount = 0;

        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select top 1  ISNULL(IsAttLocked,'0') as IsAttLocked from KQRSSZ_2 where EM_NO = '"+strStaffNo+"' AND CONVERT(VARCHAR(20),r_date,23) = '"+strOneDay+"' \r\n");

            String strSql = sbSql.ToString();
            //log.Error(strMethodDesc + " strSql:" + strSql);
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
            iRowCount = dt.Rows.Count;
            if(iRowCount<=0){
                strReturnCode = iRowCount.ToString();
            }else{
                strReturnCode = dt.Rows[0]["IsAttLocked"].ToString();
            }
            strReturnMsg = strMethodDesc + "成功";
        }
        catch (Exception ex)
        {
            strReturnCode = "0";
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
            sbResult.Append("}");
            //log.Error(strMethodDesc + "Return Json:" + sbResult.ToString());
        }
        return sbResult.ToString();
    }

    /// <summary>
    /// 锁定/解锁某员工某一天排班锁定
    /// </summary>
    /// <param name="strUserId"></param>
    /// <param name="strIsToLock"></param>
    /// <param name="strStaffNo"></param>
    /// <param name="strOneDay"></param>
    /// <returns></returns>
    private String LockStaffOneDay(String strUserId,String strStaffNo,String strOneDay,String strIsToLock)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbReturnRowData = new StringBuilder();
        StringBuilder sbReturnColumnData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "锁定/解锁某员工某一天排班锁定";

        try
        {
            StringBuilder sbSql = new StringBuilder();
            String IsAttLocked = strIsToLock.Equals("1") ? "1" : "2";
            sbSql.Append("UPDATE KQRSSZ_2 SET IsAttLocked = '"+IsAttLocked+"' where EM_NO = '"+strStaffNo+"' AND CONVERT(VARCHAR(20),r_date,23) = '"+strOneDay+"' \r\n");

            String strSql = sbSql.ToString();
            //log.Error(strMethodDesc + " strSql:" + strSql);
            int iCount = SqlParamDao.ExecuteNonQueryBySql(strSql);
            if(iCount==1){
                strReturnCode = iCount.ToString();
                strReturnMsg = strMethodDesc + "成功";
            }else{            
                strReturnCode = iCount.ToString();
                strReturnMsg = strMethodDesc + "失败";
            }
        }
        catch (Exception ex)
        {
            strReturnCode = "0";
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
            sbResult.Append("}");
            //log.Error(strMethodDesc + "Return Json:" + sbResult.ToString());
        }
        return sbResult.ToString();
    }
    
    /// <summary>
    /// 锁定/解锁某员工某一月排班锁定
    /// </summary>
    /// <param name="strUserId"></param>
    /// <param name="strIsToLock"></param>
    /// <param name="strStaffNo"></param>
    /// <param name="strOneMonth"></param>
    /// <returns></returns>
    private String LockStaffOneMonth(String strUserId,String strStaffNo,String strOneMonth,String strIsToLock)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbReturnRowData = new StringBuilder();
        StringBuilder sbReturnColumnData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "锁定/解锁某员工某一月排班锁定";

        try
        {
            StringBuilder sbSql = new StringBuilder();
            String IsAttLocked = strIsToLock.Equals("1") ? "1" : "2";
            sbSql.Append("UPDATE KQRSSZ_2 SET IsAttLocked = '"+IsAttLocked+"' where EM_NO = '"+strStaffNo+"' AND LEFT(SEQNO,4) = '"+strOneMonth+"' \r\n");

            String strSql = sbSql.ToString();
            //log.Error(strMethodDesc + " strSql:" + strSql);
            int iCount = SqlParamDao.ExecuteNonQueryBySql(strSql);
            if(iCount>=1){
                strReturnCode = iCount.ToString();
                strReturnMsg = strMethodDesc + "成功";
            }else{            
                strReturnCode = iCount.ToString();
                strReturnMsg = strMethodDesc + "失败";
            }
        }
        catch (Exception ex)
        {
            strReturnCode = "0";
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