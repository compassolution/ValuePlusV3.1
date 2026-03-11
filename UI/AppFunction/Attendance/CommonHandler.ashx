<%@ WebHandler Language="C#" Class="CommonHandler" %>

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

/// <summary>
/// 考勤排班页面的公共处理类
/// </summary>
public class CommonHandler : HandlerBase, IHttpHandler, System.Web.SessionState.IRequiresSessionState {

    /// <summary>
    /// 日志声明
    /// </summary>
    protected static Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    //获取配置中的周锁定是否针对全部部门的设置
    private String strIsLockWeekForCurSection = Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("Switch_IsLockWeekForCurSection");
    //是否能锁定解锁已支付的月度期间
    private String strIsCanLockPreYearMonth = Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("Switch_IsCanLockPreYearMonth");
    //是否能锁定解锁已成薪资依据的月度期间
    private String strIsCanLockYearMonthWhenTRS090 = Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("Switch_IsCanLockYearMonthWhenTRS090");

    public void ProcessRequest (HttpContext context) {
        //解析客户端传递过来的json data
        //StreamReader reader = new StreamReader(context.Request.InputStream);
        //String strParamJson = HttpUtility.UrlDecode(reader.ReadToEnd());
        //log.Error("传入参数的Json data字符串:" + strParamJson.ToString());

        //string strParam = WebCommon.GetJsonValue(strParamJson,"param").ToString();//请求类型参数

        String strUrlQueryString = context.Server.UrlDecode(context.Request.Url.Query.ToString());
        Hashtable hsTableUrlQuery = WebCommon.GetUrlAnalyse(strUrlQueryString);
        string strParam = hsTableUrlQuery["param"] == null ? string.Empty : hsTableUrlQuery["param"].ToString();//param
        string strPostUserId = hsTableUrlQuery["userid"] == null ? string.Empty : hsTableUrlQuery["userid"].ToString();////排班操作用户ID
        string strYearMonth = hsTableUrlQuery["yearmonth"] == null ? string.Empty : hsTableUrlQuery["yearmonth"].ToString();//考勤周期期间
        string strMonthWeekNo = hsTableUrlQuery["weekno"] == null ? string.Empty : hsTableUrlQuery["weekno"].ToString();//考勤周期周数
        string strUserType = hsTableUrlQuery["usertype"] == null ? "" : hsTableUrlQuery["usertype"].ToString();//排班操作用户类型（0或者空为小部门用户,1为人事部）
        strUserType = String.IsNullOrEmpty(strUserType) ? "0" : strUserType;
        string strTotalwidth = hsTableUrlQuery["totalwidth"] == null ? "1000" : hsTableUrlQuery["totalwidth"].ToString();//终端最大宽度
        string strSqlCondition = hsTableUrlQuery["sqlcondition"] == null ? string.Empty : hsTableUrlQuery["sqlcondition"].ToString();//Sql条件语句
        string strGRNO = hsTableUrlQuery["grno"] == null ? string.Empty : hsTableUrlQuery["grno"].ToString();//GRROTO_3权限组编码
        string strLockStatus = hsTableUrlQuery["lockstatus"] == null ? string.Empty : hsTableUrlQuery["lockstatus"].ToString();//当前锁定状态
        string strOpType = hsTableUrlQuery["optype"] == null ? string.Empty : hsTableUrlQuery["optype"].ToString();//审核操作类型

        if (String.IsNullOrEmpty(strPostUserId)){
            strPostUserId = this.GetUserCode();
        }

        switch (strParam.ToLower().ToString())
        {
            case "getyearmonthlist":
                context.Response.Write(this.GetYearMonthList(strPostUserId,strUserType).ToString());
                break;
            case "getmonthweeklist":
                context.Response.Write(this.GetMonthWeekList(strYearMonth,strPostUserId).ToString());
                break;
            case "getshiftlist":
                context.Response.Write(this.GetShiftList(strPostUserId,strSqlCondition,strTotalwidth).ToString());
                break;
            case "getdeptlistbyuserid":
                context.Response.Write(this.GetDeptListByUserId(strPostUserId,strYearMonth).ToString());
                break;
            case "getinchargeuserlist":
                context.Response.Write(this.GetInchargeUserList(strGRNO).ToString());
                break;
            case "updatestafflistbyyearmonth":
                context.Response.Write(this.UpdateStaffListByYearMonth(strYearMonth).ToString());
                break;
            case "getlockedyearmonth"://获取在表KQPERD_1中的PLOCK标志被锁定的年月
                context.Response.Write(this.GetLockedYearMonth().ToString());
                break;
            case "getdaylockedbyyearmonth"://获取某月份下在表KQPERD_3中的PLOCK标志被锁定的日期
                context.Response.Write(this.GetDayLockedByYearMonth(strYearMonth).ToString());
                break;
            case "lockbyyearmonth":
                context.Response.Write(this.LockByYearMonth(strYearMonth,strLockStatus).ToString());
                break;
            case "lockbymonthweek":
                context.Response.Write(this.LockByMonthWeek(strYearMonth,strMonthWeekNo,strLockStatus,strPostUserId).ToString());
                break;
            case "verifydeptattendance":
                context.Response.Write(this.VerifyDeptAttendance(strYearMonth,strPostUserId,strOpType).ToString());
                break;

        }
    }


    /// <summary>
    /// 获取考勤周期月份信息
    /// </summary>
    /// <param name="strPostUserId"></param>
    /// <param name="strUserType"></param>
    /// <returns></returns>
    private String GetYearMonthList(String strPostUserId,String strUserType)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbReturnRowData = new StringBuilder();
        StringBuilder sbReturnColumnData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "获取考勤周期月份信息";
        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("SELECT * ");
            sbSql.Append(" ,(case when convert(varchar(20),getdate(),23) between convert(varchar(20),A.PSTART,23) AND convert(varchar(20),A.PEND,23) then '1' else '0' end) as IsCurYearMonth");
            sbSql.Append(" FROM  KQPERD_1 A");
            sbSql.Append(" ORDER BY PID DESC");
            String strSql = sbSql.ToString();
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);

            sbReturnRowData.Append(WebCommon.GetJsonStringByDataTable(dt,"\"ResultData\"",false));

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
            sbResultStatus.Append(",\"ReturnPostUserId\":\"" + strPostUserId + "\"");
            sbResultStatus.Append(",\"ReturnLoginUserId\":\"" + this.GetUserCode().ToString() + "\"");
            sbResultStatus.Append(",\"ReturnUserType\":\"" + strUserType + "\"");

            sbResult.Append(sbResultStatus.ToString());
            if (!String.IsNullOrEmpty(sbReturnRowData.ToString()))
            {
                sbResult.Append("," + sbReturnRowData.ToString());
            }
            sbResult.Append("}");
            //log.Error(strMethodDesc + "Return Json:" + sbResult.ToString());
        }
        return sbResult.ToString();
    }

    /// <summary>
    /// 获取考勤周期月份中的周信息
    /// </summary>
    /// <param name="strYearMonth"></param>
    /// <param name="strUserId"></param>
    /// <returns></returns>
    private String GetMonthWeekList(String strYearMonth,String strUserId)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbReturnRowData = new StringBuilder();
        StringBuilder sbReturnColumnData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "获取考勤周期月份信息";
        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("SELECT * ");
            if (!strIsLockWeekForCurSection.Equals("1"))
            {
                sbSql.Append(" ,(case when exists (select * from KQPERD_3 where PID = A.PID AND MonthlyWeekNo = A.MonthlyWeekNo and PLOCK <> '1') THEN '2' ELSE '1' END) as PLOCK");
            }
            else
            {
                ////新增针对各个部门分别进行周锁定的功能 add by sammen 20181029
                sbSql.Append(" ,(case when exists (select * from KQPERD_3 where PID = A.PID AND MonthlyWeekNo = A.MonthlyWeekNo and PLOCK = '1' ");
                sbSql.Append(" and CHARINDEX(UPPER('" + strUserId + ",'),UPPER(isnull(LockedSections,'')))>0) ");
                sbSql.Append(" THEN '1' ELSE '2' END) as PLOCK");
            }
            sbSql.Append(" FROM (select DISTINCT PID,MonthlyWeekNo from KQPERD_3 where PID = '" + strYearMonth + "' group by PID,MonthlyWeekNo ) A");

            String strSql = sbSql.ToString();
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);

            sbReturnRowData.Append(WebCommon.GetJsonStringByDataTable(dt,"\"ResultData\"",false));

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
            sbResultStatus.Append(",\"ReturnUserId\":\"" + this.GetUserCode().ToString() + "\"");

            sbResult.Append(sbResultStatus.ToString());
            if (!String.IsNullOrEmpty(sbReturnRowData.ToString()))
            {
                sbResult.Append("," + sbReturnRowData.ToString());
            }
            sbResult.Append("}");
            //log.Error(strMethodDesc + "Return Json:" + sbResult.ToString());
        }
        return sbResult.ToString();
    }

    /// <summary>
    /// 获取班次信息
    /// </summary>
    /// <param name="strAdminUserId"></param>
    /// <param name="strSqlCondition"></param>
    /// <param name="strTotalwidth"></param>
    /// <returns></returns>
    private String GetShiftList(String strAdminUserId,String strSqlCondition,String strTotalwidth)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbReturnRowData = new StringBuilder();
        StringBuilder sbReturnColumnData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "获取班次信息";
        int iColCount = 0;
        int iRowCount = 0;
        try
        {

            //获取配置表中需要显示的固定字段
            String strSqlConfig_2 = "SELECT * FROM KQASConfig_2 WHERE ConfigCode in (select top 1 ConfigCode from KQASConfig_1 order by ConfigCode) order by SORDER";
            DataTable dt_Config_2 = SqlParamDao.GetDataTableBySql(strSqlConfig_2);
            StringBuilder sbShiftListFixedColumns = new StringBuilder();
            if(dt_Config_2!=null && dt_Config_2.Rows.Count>0){
                iColCount = dt_Config_2.Rows.Count;
                for(int i=0;i<iColCount;i++){
                    String strTableName_Config_2 = dt_Config_2.Rows[i]["TableName"].ToString();
                    String strColumnName_Config_2 = dt_Config_2.Rows[i]["ColumnName"].ToString();
                    String strColumnDesc_Config_2 = dt_Config_2.Rows[i]["ColumnDesc"].ToString();
                    String strTableColumnName = strTableName_Config_2 + "." + strColumnName_Config_2;

                    switch (strColumnName_Config_2.ToUpper()) {
                        case "STSS":
                        case "SSHIFT":
                        case "SNIGHT":
                        case "SMIDS":
                        case "Breakfast":
                        case "Lunch":
                        case "Dinner":
                        case "meal":
                            strTableColumnName = "(select CDESCCHS FROM TB_HRLSTD WHERE LID = 'BOOL' AND CID = " + strTableColumnName + ")";
                            break;
                        default:
                            break;
                    }

                    sbShiftListFixedColumns.Append(i==0?"":",");
                    sbShiftListFixedColumns.Append(strTableColumnName +" as ["+strColumnDesc_Config_2+"]\r\n");
                }
            }


            //全表格的宽度
            int iTableWidth = Convert.ToInt32(double.Parse(strTotalwidth)-50);
            //每一列的宽度，平均分成6列
            double iColWidth = Math.Round((double)(iTableWidth / iColCount),2);

            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("SELECT "+sbShiftListFixedColumns.ToString());
            sbSql.Append(" FROM KQSHIF_1 KQSHIF_1 inner join VW_PAIBAN_SHIFT_FILTER vwName ON KQSHIF_1.SHCODE = vwName.[SHIFTCODE]\r\n");
            sbSql.Append(" WHERE vwName.[SUSERID] = '"+strAdminUserId+"' and isnull(ISSTOP,'false') = 'false' \r\n");
            if(!String.IsNullOrEmpty(strSqlCondition)){
                sbSql.Append(" and "+strSqlCondition + "\r\n");
            }
            sbSql.Append(" ORDER BY SHCODE,SHNAME");
            String strSql = sbSql.ToString();
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
            iColCount = dt.Columns.Count;
            iRowCount = dt.Rows.Count;

            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0] as DataRow;

                sbReturnColumnData.Append("[");
                foreach (DataColumn col in row.Table.Columns)
                {
                    sbReturnColumnData.Append("{\"type\": \"text\", \"title\": \""+col.ColumnName+"\", \"width\": "+iColWidth.ToString()+", \"readOnly\": true},");
                }
                sbReturnColumnData.Append("]");
            }

            sbReturnRowData.Append(WebCommon.GetJsonStringByDataTable(dt,"",false));

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
            sbResultStatus.Append(",\"ReturnColCount\":\"" + iColCount.ToString() + "\"");
            sbResultStatus.Append(",\"ReturnRowCount\":\"" + iRowCount.ToString() + "\"");

            sbResult.Append(sbResultStatus.ToString());
            if (!String.IsNullOrEmpty(sbReturnRowData.ToString()))
            {
                sbResult.Append(",\"ReturnRowData\"" + sbReturnRowData.ToString()+"");
            }
            if (!String.IsNullOrEmpty(sbReturnColumnData.ToString()))
            {
                sbResult.Append(",\"ReturnColumnData\":" + sbReturnColumnData.ToString());
            }
            sbResult.Append("}");
            //log.Error(strMethodDesc + "Return Json:" + sbResult.ToString());
        }
        return sbResult.ToString();
    }


    /// <summary>
    /// 根据考勤员账号获取管辖的部门列表
    /// </summary>
    /// <param name="strAdminUserId"></param>
    /// <param name="strYearMonth"></param>
    /// <returns></returns>
    private String GetDeptListByUserId(String strAdminUserId,String strYearMonth)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbReturnRowData = new StringBuilder();
        StringBuilder sbReturnColumnData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "根据考勤员账号获取管辖的部门列表";
        int iColCount = 0;
        int iRowCount = 0;
        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("SELECT A.DCNO,B.* ");
            sbSql.Append(" ,(SELECT ISNULL(KQSTATUS,'1') FROM KQPERD_4 I where PID = '"+strYearMonth+"' AND SECTIONCODE = A.SEPNO) AS KQSTATUS");
            sbSql.Append(" ,(SELECT (select CDESCCHS from TB_HRLSTD WHERE LID = 'ANALYSISSTATUS' AND CID = ISNULL(KQSTATUS,'1')) FROM KQPERD_4 I where PID = '"+strYearMonth+"' AND SECTIONCODE = A.SEPNO) AS KQSTATUSDesc");
            sbSql.Append(" FROM KQDL_2 A INNER JOIN CSORGA_1 B ON A.SEPNO = B.OID WHERE A.DCNO = '"+strAdminUserId+"'");
            sbSql.Append(" ORDER BY DCNO,OPID,OID");
            String strSql = sbSql.ToString();
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);

            sbReturnRowData.Append(WebCommon.GetJsonStringByDataTable(dt,"",false));

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
            sbResultStatus.Append(",\"ReturnColCount\":\"" + iColCount.ToString() + "\"");
            sbResultStatus.Append(",\"ReturnRowCount\":\"" + iRowCount.ToString() + "\"");

            sbResult.Append(sbResultStatus.ToString());
            if (!String.IsNullOrEmpty(sbReturnRowData.ToString()))
            {
                sbResult.Append(",\"ReturnRowData\"" + sbReturnRowData.ToString()+"");
            }
            if (!String.IsNullOrEmpty(sbReturnColumnData.ToString()))
            {
                sbResult.Append(",\"ReturnColumnData\":" + sbReturnColumnData.ToString());
            }
            sbResult.Append("}");
            //log.Error(strMethodDesc + "Return Json:" + sbResult.ToString());
        }
        return sbResult.ToString();
    }

    /// <summary>
    ///获取考勤员列表
    /// </summary>
    private String GetInchargeUserList(String strGRNO)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbReturnRowData = new StringBuilder();
        StringBuilder sbReturnColumnData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "获取考勤员列表";
        try
        {
            StringBuilder sbSql = new StringBuilder();
            if (strGRNO.Equals("")) {
                strGRNO = "00001";
            }
            sbSql.Append("SELECT * FROM GRROTO_3 WHERE GRNO = '"+ strGRNO + "' order by DCNO");
            String strSql = sbSql.ToString();
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);

            sbReturnRowData.Append(WebCommon.GetJsonStringByDataTable(dt,"\"ResultData\"",false));

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
            if (!String.IsNullOrEmpty(sbReturnRowData.ToString()))
            {
                sbResult.Append("," + sbReturnRowData.ToString());
            }
            sbResult.Append("}");
            //log.Error(strMethodDesc + "Return Json:" + sbResult.ToString());
        }
        return sbResult.ToString();
    }

    /// <summary>
    /// 根据期间更新本月需做考勤的人员列表
    /// </summary>
    /// <param name="strYearMonth"></param>
    /// <returns></returns>
    private String UpdateStaffListByYearMonth(String strYearMonth)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "根据期间更新本月需做考勤的人员列表";
        try
        {
            //获取员工列表之前先更新月度员工清单表
            String strSpName = "USP_HR_Update_MonthStaffList";
            Hashtable hsTableParam = new Hashtable();
            hsTableParam.Add("YearMonth", strYearMonth);
            //log.Error("更新月度员工清单表：exec " + strSpName + " '" + strYearMonth + "'");
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

            sbResult.Append(sbResultStatus.ToString());
            sbResult.Append("}");
            //log.Error(strMethodDesc + "Return Json:" + sbResult.ToString());
        }
        return sbResult.ToString();
    }

    /// <summary>
    /// 获取在表KQPERD_1中的PLOCK标志被锁定的年月
    /// </summary>
    /// <returns></returns>
    private String GetLockedYearMonth()
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResult = new StringBuilder();
        StringBuilder sbReturnRowData = new StringBuilder();
        String strMethodDesc = "获取在表KQPERD_1中的PLOCK标志被锁定的年月";
        try
        {
            StringBuilder sbSql = new StringBuilder();

            sbReturnRowData.Append("[");
            //获取年月列表
            String strSql = "select * from KQPERD_1 WHERE ISNULL(PLOCK,'2') = '1' order by [PID]";

            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
            int iRowCount = dt.Rows.Count;
            if (iRowCount > 0)
            {
                for (int i = 0; i < iRowCount; i++)
                {
                    sbReturnRowData.Append(i > 0 ? "," : "");
                    sbReturnRowData.Append("\"" + dt.Rows[i]["PID"].ToString() + "\"");
                }
            }

            sbReturnRowData.Append("]");

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
            if (!String.IsNullOrEmpty(sbReturnRowData.ToString()))
            {
                sbResult.Append(",\"ReturnRowData\":" + sbReturnRowData.ToString()+"");
            }
            sbResult.Append("}");
            //log.Error(strMethodDesc + "Return Json:" + sbResult.ToString());
        }
        return sbResult.ToString();
    }

    /// <summary>
    /// 获取某月份下在表KQPERD_3中的PLOCK标志被锁定的日期
    /// </summary>
    /// <param name="strYearMonth"></param>
    /// <returns></returns>
    private String GetDayLockedByYearMonth(String strYearMonth)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResult = new StringBuilder();
        StringBuilder sbReturnRowData = new StringBuilder();
        String strMethodDesc = "获取某月份下在表KQPERD_3中的PLOCK标志被锁定的日期";
        try
        {
            StringBuilder sbSql = new StringBuilder();

            sbReturnRowData.Append("[");
            //获取每月的日期列表
            String strSql = "select convert(varchar(20),DATE,23) AS DayLocked,* from KQPERD_3 WHERE PID = '"+strYearMonth+"' AND ISNULL(PLOCK,'2') = '1' order by [date]";

            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
            int iRowCount = dt.Rows.Count;

            //sbReturnRowData.Append(WebCommon.GetJsonStringByDataTable(dt,"",false));

            if (iRowCount > 0)
            {
                for (int i = 0; i < iRowCount; i++)
                {
                    sbReturnRowData.Append(i > 0 ? "," : "");
                    sbReturnRowData.Append("\"" + dt.Rows[i]["DayLocked"].ToString() + "\"");
                }
            }

            sbReturnRowData.Append("]");

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
            if (!String.IsNullOrEmpty(sbReturnRowData.ToString()))
            {
                sbResult.Append(",\"ReturnRowData\":" + sbReturnRowData.ToString()+"");
            }
            sbResult.Append("}");
            //log.Error(strMethodDesc + "Return Json:" + sbResult.ToString());
        }
        return sbResult.ToString();
    }

    /// <summary>
    /// 根据期间进行锁定或解锁
    /// </summary>
    /// <param name="strYearMonth"></param>
    /// <param name="strCurLockFlag"></param>
    /// <returns></returns>
    private String LockByYearMonth(String strYearMonth,String strCurLockFlag)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "根据期间进行锁定或解锁";
        String strSetToLockFlag = strCurLockFlag == "1" ? "2" : "1";
        strMethodDesc = (strSetToLockFlag == "1" ? "锁定期间":"解锁期间") + strYearMonth;
        try
        {
            //考虑到有些项目中KQPERD_4的部门数据有缺失，此处先进行一次校验，将本期间的缺失部门写入到KQPERD_4中
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("insert into KQPERD_4(PID,SECTIONCODE,KQSTATUS)");
            sbSql.Append("select '"+strYearMonth+"',OID,'1' from CSORGA_1 WHERE OTEST = '3' AND OID NOT IN (SELECT SECTIONCODE FROM KQPERD_4 WHERE PID = '"+strYearMonth+"')");
            String strSql = sbSql.ToString();
            int iCount = SqlParamDao.ExecuteNonQueryBySql(strSql);
            //考虑到有些项目中KQPERD_4的部门数据有缺失，此处先进行一次校验，将本期间的缺失部门写入到KQPERD_4中

            //默认设置可操作
            bool isCanLock = true;
            //是否能锁定解锁已支付的月度期间
            if(strIsCanLockPreYearMonth.Equals("0")){//如果不能
                String strSql_IsCan = "select count(1) from (select max(yearmonth) as yearmonth  from TRSBAS_1_H ) a where yearmonth >= '"+strYearMonth+"'";
                int iCount_IsCan = SqlParamDao.ExecuteScalarBySql(strSql_IsCan);
                if(iCount_IsCan>0){
                    isCanLock = false;//设置为不可操作
                    strReturnCode = "-19";
                    strReturnMsg = "失败,已支付的月度期间禁止进行锁定或解锁操作!";
                }
                //sbSql_Lock1.Append(" and PID > (select max(yearmonth) from TRSBAS_1_H )");
                //sbSql_Lock3.Append(" and PID > (select max(yearmonth) from TRSBAS_1_H )");
            }
            //是否能锁定解锁已成薪资依据的月度期间
            if(isCanLock && strIsCanLockYearMonthWhenTRS090.Equals("0")){//如果不能
                String strSql_IsCan = "select count(1) from TRSBAS_1 where [STATUS]='090' and yearmonth = '"+strYearMonth+"' and convert(varchar(10),isnull(LDATE,'2999-01-01'),120)>=(select convert(varchar(10),pstart,120) from KQPERD_1 where pid='"+strYearMonth+"')";
                int iCount_IsCan = SqlParamDao.ExecuteScalarBySql(strSql_IsCan);
                if(iCount_IsCan>0){
                    isCanLock = false;//设置为不可操作
                    strReturnCode = "-29";
                    strReturnMsg = "失败,已成薪资依据的月度期间禁止进行锁定或解锁操作!";
                }
                //sbSql_Lock1.Append(" and PID not in (select yearmonth from TRSBAS_1 where [STATUS]='090')");
                //sbSql_Lock3.Append(" and PID not in (select yearmonth from TRSBAS_1 where [STATUS]='090')");

            }

            if(isCanLock){
                ///执行解锁或者锁定操作--Start
                StringBuilder sbSql_Lock1 = new StringBuilder();
                sbSql_Lock1.Append("update KQPERD_1 set PLOCK = '" + strSetToLockFlag + "' WHERE PID = '" + strYearMonth + "' ");
                StringBuilder sbSql_Lock3 = new StringBuilder();
                sbSql_Lock3.Append("update KQPERD_3 set PLOCK = '" + strSetToLockFlag + "' WHERE PID = '" + strYearMonth + "'");

                strSql = sbSql_Lock1.ToString() + ";" + sbSql_Lock3.ToString();
                //log.Error(strMethodDesc + "Sql:" + strSql);
                iCount = SqlParamDao.ExecuteNonQueryBySql(strSql);
                ///执行解锁或者锁定操作--End

                strReturnCode = "1";
                strReturnMsg = strMethodDesc + "成功";
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
            sbResult.Append("}");
            //log.Error(strMethodDesc + "Return Json:" + sbResult.ToString());
        }
        return sbResult.ToString();
    }

    /// <summary>
    /// 根据期间周数进行锁定或解锁
    /// </summary>
    /// <param name="strYearMonth"></param>
    /// <param name="strMonthlyWeekNo"></param>
    /// <param name="strCurLockFlag"></param>
    /// <param name="strUserId"></param>
    /// <returns></returns>
    private String LockByMonthWeek(String strYearMonth,String strMonthlyWeekNo,String strCurLockFlag,String strUserId)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "根据期间周数进行锁定或解锁";
        String strSetToLockFlag = strCurLockFlag == "1" ? "2" : "1";
        strMethodDesc = (strIsLockWeekForCurSection.Equals("1") ? "(针对考勤员"+strUserId+"管辖下的部门)" : "(针对所有部门)");
        strMethodDesc = strMethodDesc + "<br />"+ (strSetToLockFlag == "1" ? "锁定期间":"解锁期间") + strYearMonth+"的第"+strMonthlyWeekNo+"周";
        try
        {
            StringBuilder sbSql = new StringBuilder();
            if (!strIsLockWeekForCurSection.Equals("1"))
            {
                //针对所有部门一起
                sbSql.Append("update KQPERD_3 set PLOCK = '" + strSetToLockFlag + "' WHERE PID = '" + strYearMonth + "' AND MonthlyWeekNo = '" + strMonthlyWeekNo + "'");
            }
            else
            {
                String strSectionCodeString = strUserId + ",";
                ////新增针对各个部门分别进行周锁定的功能 add by sammen 20181029
                sbSql.Append("update KQPERD_3 set PLOCK = '" + strSetToLockFlag + "'");
                if (strSetToLockFlag.Equals("1"))
                {
                    //字段新增锁定的部门代码集合
                    sbSql.Append(" ,LockedSections = replace(isnull(LockedSections,''),'" + strSectionCodeString + "','') + '" + strSectionCodeString + "'");
                }
                else
                {
                    //字段删除锁定的部门代码集合
                    sbSql.Append(" ,LockedSections = replace(isnull(LockedSections,''),'" + strSectionCodeString + "','')");
                }
                sbSql.Append("  WHERE PID = '" + strYearMonth + "' AND MonthlyWeekNo = '" + strMonthlyWeekNo + "'");
            }


            String strSql = sbSql.ToString();

            //log.Error(strMethodDesc + "Sql:" + strSql);
            int iCount = SqlParamDao.ExecuteNonQueryBySql(strSql);
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
        }
        return sbResult.ToString();
    }

    /// <summary>
    /// 审核考勤操作
    /// </summary>
    /// <param name="strYearMonth"></param>
    /// <param name="strMonthlyWeekNo"></param>
    /// <param name="strCurLockFlag"></param>
    /// <param name="strUserId"></param>
    /// <returns></returns>
    private String VerifyDeptAttendance(String strYearMonth,String strUserId,String strOpType)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "审核考勤操作"+strOpType;
        try
        {
            StringBuilder sbSql = new StringBuilder();
            //针对单个部门的按钮控制
            String strSql = "";
            switch (strOpType)
            {
                case "verify":
                    strSql = "update KQPERD_4 set KQSTATUS = '2' WHERE PID = '" + strYearMonth + "' AND SECTIONCODE in (select SEPNO from KQDL_2 where DCNO = '"+strUserId+"')";
                    strMethodDesc = "审核此部门考勤操作";
                    break;
                case "finish":
                    strSql = "update KQPERD_4 set KQSTATUS = '3' WHERE PID = '" + strYearMonth + "' AND SECTIONCODE in (select SEPNO from KQDL_2 where DCNO = '"+strUserId+"')";
                    strMethodDesc = "完成此部门的考勤审核";
                    break;
                case "return":
                    strSql = "update KQPERD_4 set KQSTATUS = '1' WHERE PID = '" + strYearMonth + "' AND SECTIONCODE in (select SEPNO from KQDL_2 where DCNO = '"+strUserId+"')";
                    strMethodDesc = "退回此部门的考勤";
                    break;
                case "verifyall":
                    strSql = "update KQPERD_4 set KQSTATUS = '2' WHERE PID = '" + strYearMonth + "'";
                    strMethodDesc = "审核所有部门考勤操作";
                    break;
                case "finishall":
                    strSql = "update KQPERD_4 set KQSTATUS = '3' WHERE PID = '" + strYearMonth + "'";
                    strMethodDesc = "完成所有部门的考勤审核";
                    break;
                case "returnall":
                    strSql = "update KQPERD_4 set KQSTATUS = '1' WHERE PID = '" + strYearMonth + "'";
                    strMethodDesc = "退回所有部门的考勤";
                    break;
            }
            if (!String.IsNullOrEmpty(strSql))
            {
                //log.Error(strMethodDesc + "Sql:" + strSql);
                int iCount = SqlParamDao.ExecuteNonQueryBySql(strSql);
                strReturnCode = "1";
                strReturnMsg = strMethodDesc + "成功";
            }else{
                strReturnCode = "-1";
                strReturnMsg = strMethodDesc + "失败";
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