<%@ WebHandler Language="C#" Class="AnalyseHandler" %>

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
/// 考勤分析的处理类
/// </summary>
public class AnalyseHandler : HandlerBase, IHttpHandler, System.Web.SessionState.IRequiresSessionState {

    /// <summary>
    /// 日志声明
    /// </summary>
    protected static Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    public void ProcessRequest (HttpContext context) {
        //解析客户端传递过来的json data
        StreamReader reader = new StreamReader(context.Request.InputStream);
        String strParamJson = HttpUtility.UrlDecode(reader.ReadToEnd());
        //log.Error("传入参数的Json data字符串:" + strParamJson.ToString());

        String strUrlQueryString = context.Server.UrlDecode(context.Request.Url.Query.ToString());
        Hashtable hsTableUrlQuery = WebCommon.GetUrlAnalyse(strUrlQueryString);
        string strParam = hsTableUrlQuery["param"] == null ? string.Empty : hsTableUrlQuery["param"].ToString();//param
        string strUserId = hsTableUrlQuery["userid"] == null ? string.Empty : hsTableUrlQuery["userid"].ToString();////分析操作用户ID
        string strYearMonth = hsTableUrlQuery["yearmonth"] == null ? string.Empty : hsTableUrlQuery["yearmonth"].ToString();//考勤周期期间
        string strStartDay = hsTableUrlQuery["startday"] == null ? string.Empty : hsTableUrlQuery["startday"].ToString();//分析开始日期
        string strEndDay = hsTableUrlQuery["endday"] == null ? string.Empty : hsTableUrlQuery["endday"].ToString();//分析结束日期
        string strStaffNo = hsTableUrlQuery["staffno"] == null ? string.Empty : hsTableUrlQuery["staffno"].ToString();//分析对象的员工工号
        string strBatchNo = hsTableUrlQuery["batchno"] == null ? string.Empty : hsTableUrlQuery["batchno"].ToString();//分析批次号

        if (String.IsNullOrEmpty(strUserId)){
            strUserId = this.GetUserCode();
        }

        switch (strParam.ToLower().ToString())
        {
            ////按员工执行分析前的数据初始化准备
            case "analyse_doinit":
                context.Response.Write(this.Analyse_DoInit(strYearMonth,strStartDay,strEndDay,strUserId).ToString());
                break;
            ////执行考勤分析存储过程(按员工执行)
            case "analyse_dobystaff":
                context.Response.Write(this.Analyse_DoBySataff(strStaffNo,strYearMonth,strStartDay,strEndDay,strUserId,strBatchNo).ToString());
                break;
            ////按员工执行分析后的汇总分析
            case "analyse_dosummary":
                context.Response.Write(this.Analyse_DoSummary(strYearMonth,strStartDay,strEndDay,strUserId,strBatchNo).ToString());
                break;
        }
    }

    /// <summary>
    /// 按员工执行分析前的数据初始化准备
    /// </summary>
    /// <param name="strYearMonth"></param>
    /// <param name="strStartDay"></param>
    /// <param name="strEndDay"></param>
    /// <param name="strUserId"></param>
    /// <returns></returns>
    private String Analyse_DoInit(String strYearMonth, String strStartDay, String strEndDay, String strUserId)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "按员工执行分析前的数据初始化准备";
        String strCurTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        String strBatchNo = DateTime.Now.ToString("yyyyMMddHHmmssffff")+strUserId;
        StringBuilder sbReturnStaffListData = new StringBuilder();
        try
        {
            String strSpName = "USP_KQ_ByStaff_ANALY_Init";
            Hashtable hsTableParam = new Hashtable();
            hsTableParam.Add("YearMonth", strYearMonth);
            hsTableParam.Add("DATEF", strStartDay);
            hsTableParam.Add("DATET", strEndDay);
            hsTableParam.Add("SUSERID", strUserId);
            //log.Error("按员工依次执行作考勤分析前的数据初始化操作：exec " + strSpName + " '" + strYearMonth + "', '" + strStartDay + "','" + strEndDay + "','" + strUserId + "'");
            int iSPReturn = SqlParamDao.ExcuteSP(strSpName, hsTableParam);

             //add by sammen 20220303 如果新月份从未分析过，月度员工清单为空，所以在分析初始化时，同时先获取员工清单
            StringBuilder sbSql_GetStaffList = new StringBuilder();
            sbSql_GetStaffList.Append("SELECT B.*");
            sbSql_GetStaffList.Append(" FROM FUN_VW_PAIBAN_STAFF_FILTER_BYUSERID('"+strYearMonth+"','"+strUserId+"') A \r\n");
            sbSql_GetStaffList.Append(" inner join (SELECT * FROM TB_HR_MonthStaffList WHERE YEARMONTH = '"+strYearMonth+"') B on A.STAFFID = B.DCNO\r\n");
            sbSql_GetStaffList.Append(" WHERE A.SUSERID = '"+strUserId+"' AND B.YEARMONTH = '"+strYearMonth+"'\r\n");
            String strSql_GetStaffList = sbSql_GetStaffList.ToString();
            DataTable dt_GetStaffList = SqlParamDao.GetDataTableBySql(strSql_GetStaffList);
            sbReturnStaffListData.Append(WebCommon.GetJsonStringByDataTable(dt_GetStaffList,"",false));
             //add by sammen 20220303 如果新月份从未分析过，月度员工清单为空，所以在分析初始化时，同时先获取员工清单

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
            StringBuilder sbResultStatus = new StringBuilder();
            sbResultStatus.Append("{");
            sbResultStatus.Append("\"ReturnCode\":\"" + strReturnCode.ToString() + "\"");
            sbResultStatus.Append(",\"ReturnMsg\":\"" + strReturnMsg.ToString() + "\"");
            sbResultStatus.Append(",\"ReturnBatchNo\":\"" + strBatchNo + "\"");

            sbResult.Append(sbResultStatus.ToString());
             //add by sammen 20220303 如果新月份从未分析过，月度员工清单为空，所以在分析初始化时，同时先获取员工清单
            if (!String.IsNullOrEmpty(sbReturnStaffListData.ToString()))
            {
                sbResult.Append(",\"ReturnStaffListData\"" + sbReturnStaffListData.ToString()+"");
            }
            sbResult.Append("}");
            //log.Error(strMethodDesc + "Return Json:" + sbResult.ToString());

            //记录考勤分析记录表主表
            try {
                StringBuilder sbSql_Anylyse = new StringBuilder();
                sbSql_Anylyse.Append("delete from KQANAL_1 where [CSEQ] = '"+strBatchNo+"' \r\n");
                sbSql_Anylyse.Append("delete from KQANAL_2 where [CSEQ] = '"+strBatchNo+"' \r\n");
                sbSql_Anylyse.Append("delete from KQANAL_3 where [CSEQ] = '"+strBatchNo+"' \r\n");
                sbSql_Anylyse.Append("INSERT INTO [KQANAL_1]([CSEQ],[YEARMONTH],[DATEF],[DATET],[InchargeUserId],[OpUserId],[ASTART],[AEND],[ASTATUS]) \r\n");
                sbSql_Anylyse.Append("values ('"+strBatchNo+"','"+strYearMonth+"','"+strStartDay+"','"+strEndDay+"','"+strUserId+"','"+this.GetUserCode()+"','"+strCurTime+"',null,'2') \r\n");
                int iExecuteCount = SqlParamDao.ExecuteNonQueryBySql(sbSql_Anylyse.ToString());

            }catch(Exception ex1){

            }
            //记录考勤分析记录表主表
        }
        return sbResult.ToString();
    }

    /// <summary>
    /// 执行考勤分析存储过程(按员工执行)
    /// </summary>
    /// <param name="strStaffNo"></param>
    /// <param name="strYearMonth"></param>
    /// <param name="strStartDay"></param>
    /// <param name="strEndDay"></param>
    /// <param name="strUserId"></param>
    /// <param name="strBatchNo"></param>
    /// <returns></returns>
    private String Analyse_DoBySataff(String strStaffNo,String strYearMonth, String strStartDay, String strEndDay, String strUserId,String strBatchNo)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "执行考勤分析存储过程(按员工执行)";
        String strCurTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        try
        {
            String strSpName = "USP_KQ_ByStaff_ANALY_DoStaff";
            Hashtable hsTableParam = new Hashtable();
            hsTableParam.Add("em_No", strStaffNo);
            hsTableParam.Add("YearMonth", strYearMonth);
            hsTableParam.Add("DATEF", strStartDay);
            hsTableParam.Add("DATET", strEndDay);
            hsTableParam.Add("SUSERID", strUserId);
            //log.Error("按员工依次执行考勤分析：exec " + strSpName + " '" + strStaffNo + "'," + " '" + strYearMonth +  "', '" + strStartDay + "'," + " '" + strEndDay + "','" + strUserId + "'");
            int iSPReturn = SqlParamDao.ExcuteSP(strSpName, hsTableParam);

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
            StringBuilder sbResultStatus = new StringBuilder();
            sbResultStatus.Append("{");
            sbResultStatus.Append("\"ReturnCode\":\"" + strReturnCode.ToString() + "\"");
            sbResultStatus.Append(",\"ReturnMsg\":\"" + strReturnMsg.ToString() + "\"");
            sbResultStatus.Append(",\"ReturnStaffNo\":\"" + strStaffNo + "\"");

            sbResult.Append(sbResultStatus.ToString());
            sbResult.Append("}");
            //log.Error(strMethodDesc + "Return Json:" + sbResult.ToString());

            //记录考勤分析记录表中的员工列表
            try {
                StringBuilder sbSql_Anylyse = new StringBuilder();
                sbSql_Anylyse.Append("delete from KQANAL_3 where [CSEQ] = '"+strBatchNo+"' and DCNO = '"+strStaffNo+"' \r\n");
                //sbSql_Anylyse.Append("delete from KQANAL_2 where [CSEQ] = '"+strBatchNo+"' \r\n");
                sbSql_Anylyse.Append("INSERT INTO [KQANAL_3]([CSEQ],[DCNO],[DCNAME],[DCNAMECHS],[DCDDESCCHS],[DCHRPOSI],[DCPLEVEL],[YEARMONTH],[DATEF],[DATET],[OpTime]) \r\n");
                sbSql_Anylyse.Append("SELECT '"+strBatchNo+"',[DCNO],[DCNAME],[DCNAMECHS],[DCDDESCCHS],[DCHRPOSI],[DCPLEVEL],'"+strYearMonth+"','"+strStartDay+"','"+strEndDay+"','"+strCurTime+"' \r\n");
                sbSql_Anylyse.Append(" FROM HRDOCU_1 A WHERE DCNO = '"+strStaffNo+"' \r\n");
                int iExecuteCount = SqlParamDao.ExecuteNonQueryBySql(sbSql_Anylyse.ToString());

            }catch(Exception ex1){

            }
            //记录考勤分析记录表中的员工列表
        }
        return sbResult.ToString();
    }

    /// <summary>
    /// 按员工执行分析后的汇总分析
    /// </summary>
    /// <param name="strYearMonth"></param>
    /// <param name="strStartDay"></param>
    /// <param name="strEndDay"></param>
    /// <param name="strUserId"></param>
    /// <param name="strBatchNo"></param>
    /// <returns></returns>
    private String Analyse_DoSummary(String strYearMonth, String strStartDay, String strEndDay, String strUserId,String strBatchNo)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "考勤分析并汇总";
        String strCurTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        try
        {
            String strSpName = "USP_KQ_ByStaff_ANALY_Summary";
            Hashtable hsTableParam = new Hashtable();
            hsTableParam.Add("YearMonth", strYearMonth);
            hsTableParam.Add("DATEF", strStartDay);
            hsTableParam.Add("DATET", strEndDay);
            hsTableParam.Add("SUSERID", strUserId);
            //log.Error("按员工依次执行考勤分析后的汇总分析：exec " + strSpName  + " '" + strYearMonth + "', '" + strStartDay + "'," + " '" + strEndDay + "','" + strUserId + "'");
            int iSPReturn = SqlParamDao.ExcuteSP(strSpName, hsTableParam);

            strCurTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
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
            StringBuilder sbResultStatus = new StringBuilder();
            sbResultStatus.Append("{");
            sbResultStatus.Append("\"ReturnCode\":\"" + strReturnCode.ToString() + "\"");
            sbResultStatus.Append(",\"ReturnMsg\":\"" + strReturnMsg.ToString() + "\"");

            sbResult.Append(sbResultStatus.ToString());
            sbResult.Append("}");
            //log.Error(strMethodDesc + "Return Json:" + sbResult.ToString());

            //记录考勤分析记录表中的部门列表及主表
            try {
                StringBuilder sbSql_Anylyse = new StringBuilder();
                //sbSql_Anylyse.Append("update KQRSSZ_2 set BISTONORMAL = '0' WHERE CONVERT(VARCHAR(20),r_Date,20) BETWEEN '"+strStartDay+"' AND '"+strEndDay+"'  \r\n");
                //sbSql_Anylyse.Append(" and ISNULL(BISTONORMAL,'') = '' AND EM_NO in (select DCNO FROM KQANAL_3 WHERE [CSEQ] = '" + strBatchNo + "') \r\n");
                sbSql_Anylyse.Append("update KQANAL_1 set ARESULT = '"+strReturnMsg+"', ASTATUS = '1',AEND = '"+strCurTime+"' WHERE [CSEQ] = '" + strBatchNo + "' \r\n");
                sbSql_Anylyse.Append("delete from KQANAL_2 where [CSEQ] = '" + strBatchNo + "' \r\n");
                sbSql_Anylyse.Append("INSERT INTO [KQANAL_2]([CSEQ],[DCDDESCCHS],[YEARMONTH],[DATEF],[DATET]) \r\n");
                sbSql_Anylyse.Append("select distinct [CSEQ],[DCDDESCCHS],'" + strYearMonth + "','" + strStartDay + "','" + strEndDay + "' \r\n");
                sbSql_Anylyse.Append(" from [KQANAL_3] where CSEQ = '" + strBatchNo + "' \r\n");
                int iExecuteCount = SqlParamDao.ExecuteNonQueryBySql(sbSql_Anylyse.ToString());

            }
            catch(Exception ex1){

            }
            //记录考勤分析记录表中的部门列表及主表
        }
        return sbResult.ToString();
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}