<%@ WebHandler Language="C#" Class="StaffLeaveOverview_NWGZU" %>

using System;
using System.Web;
using System.Text;
using System.Data;
using System.IO;
using System.Collections;
using Com.ValuePlus.Web;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Utils.Serializable;

public class StaffLeaveOverview_NWGZU : HandlerBase, IHttpHandler, System.Web.SessionState.IRequiresSessionState {

    public void ProcessRequest (HttpContext context) {
        Hashtable hsTableUrlQuery = this.GetUrlAnalyse(context);
        string strParam = hsTableUrlQuery["param"] == null ? string.Empty : hsTableUrlQuery["param"].ToString();//param
        string strKeyValue = hsTableUrlQuery["keyvalue"] == null ? string.Empty : hsTableUrlQuery["keyvalue"].ToString();//param
        string strYearMonth = hsTableUrlQuery["yearmonth"] == null ? string.Empty : hsTableUrlQuery["yearmonth"].ToString();//param
        string strTID = hsTableUrlQuery["tid"] == null ? string.Empty : hsTableUrlQuery["tid"].ToString();//param
        string strDCNO = hsTableUrlQuery["dcno"] == null ? string.Empty : hsTableUrlQuery["dcno"].ToString();//param
        string strLVTYPE = hsTableUrlQuery["lvtype"] == null ? string.Empty : hsTableUrlQuery["lvtype"].ToString();//param
        string strLVStatus = hsTableUrlQuery["lvstatus"] == null ? string.Empty : hsTableUrlQuery["lvstatus"].ToString();//param
        string strDateFrom = hsTableUrlQuery["datefrom"] == null ? string.Empty : hsTableUrlQuery["datefrom"].ToString();//param
        string strDateTo = hsTableUrlQuery["dateto"] == null ? string.Empty : hsTableUrlQuery["dateto"].ToString();//param
        string strOTTYPE = hsTableUrlQuery["ottype"] == null ? string.Empty : hsTableUrlQuery["ottype"].ToString();//param
        string strPayType = hsTableUrlQuery["paytype"] == null ? string.Empty : hsTableUrlQuery["paytype"].ToString();//param
        string strOTStatus = hsTableUrlQuery["otstatus"] == null ? string.Empty : hsTableUrlQuery["otstatus"].ToString();//param
        string strIsShowPre = hsTableUrlQuery["isshowpre"] == null ? string.Empty : hsTableUrlQuery["isshowpre"].ToString();//是否是显示界面中的前几年的年假信息 add by sammen 20250224

        //根据输入的TID和KeyValue获取对应员工工号
        if ((!String.IsNullOrEmpty(strTID)) && (!String.IsNullOrEmpty(strKeyValue)))
        {
            StringBuilder sbSql = new StringBuilder();
            try
            {
                sbSql.Append("select CASE WHEN '"+strTID+"' = 'HRDOCU' THEN '"+strKeyValue+"'");
                sbSql.Append("  WHEN '"+strTID+"' = 'KQOVTM' THEN (select top 1 EMPLOYEE FROM KQOVTM_1 WHERE OTNO = '"+strKeyValue+"')");
                sbSql.Append("  WHEN '"+strTID+"' = 'KQLV' THEN (select top 1 EMPLOYEE FROM KQLV_1 WHERE LVNO = '"+strKeyValue+"')");
                sbSql.Append("  WHEN '"+strTID+"' = 'FLLV' THEN (select top 1 EMPLOYEE FROM FLLV_1 WHERE FlowCode = '"+strKeyValue+"')");
                sbSql.Append("  ELSE '"+strKeyValue+"' END AS DCNO");
                DataTable dt = SqlParamDao.GetDataTableBySql(sbSql.ToString());
                strDCNO = dt.Rows[0]["DCNO"].ToString();
            }
            catch (Exception ex)
            {
                log.Error("StaffLeaveOverview_Load该项目OA审批流程FLLV未启用");
                sbSql = new StringBuilder();
                sbSql.Append("select CASE WHEN '"+strTID+"' = 'HRDOCU' THEN '"+strKeyValue+"'");
                sbSql.Append("  WHEN '"+strTID+"' = 'KQOVTM' THEN (select top 1 EMPLOYEE FROM KQOVTM_1 WHERE OTNO = '"+strKeyValue+"')");
                sbSql.Append("  WHEN '"+strTID+"' = 'KQLV' THEN (select top 1 EMPLOYEE FROM KQLV_1 WHERE LVNO = '"+strKeyValue+"')");
                sbSql.Append("  ELSE '"+strKeyValue+"' END AS DCNO");
                DataTable dt = SqlParamDao.GetDataTableBySql(sbSql.ToString());
                strDCNO = dt.Rows[0]["DCNO"].ToString();
            }
        }

        if (strParam.Equals("getyearmonth"))
        {
            this.GetYearMonthList(context);
        }else if (strParam.Equals("getqueryyear"))
        {
            this.GetQueryYearList(context);
        }
        else if (strParam.Equals("getannualleaveinfo"))
        {
            this.GetAnnualLeaveInfo(context,strDCNO,strYearMonth,strIsShowPre);
        }
        else if (strParam.Equals("getlvtyplist"))
        {
            this.GetLVTYPEList(context);
        }
        else if (strParam.Equals("getotlvbalance"))
        {
            this.GetOTLVBalance(context,strDCNO,strYearMonth);
        }
        else if (strParam.Equals("getlvrecord"))
        {
            this.GetLeaveRecordData(context,strDCNO,strLVTYPE,strLVStatus,strYearMonth,strDateFrom,strDateTo);
        }
        else if (strParam.Equals("getlvsummary"))
        {
            this.GetLeaveSummaryData(context,strDCNO,strYearMonth);
        }
        else if (strParam.Equals("getlvstatus"))
        {
            this.GetLVApproveStatusList(context);
        }
        //加班数据
        else if (strParam.Equals("getotstatus"))
        {
            this.GetOTApproveStatusList(context);
        }
        else if (strParam.Equals("getotsummary"))
        {
            this.GetOvertimeSummaryData(context,strDCNO,strYearMonth);
        }
        else if (strParam.Equals("getottyplist"))
        {
            this.GetOTTYPEList(context);
        }
        else if (strParam.Equals("getotrecord"))
        {
            this.GetOvertimeRecordData(context,strDCNO,strOTTYPE,strPayType,strOTStatus,strYearMonth,strDateFrom,strDateTo);
        }
        else if (strParam.Equals("getflotstafflist"))
        {
            GetFLOTStaffList(context,strKeyValue);
        }

    }

    /// <summary>
    /// 获取当前年假信息
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strDCNO"></param>
    /// <param name="strYearMonth"></param>
    /// <param name="strIsShowPre">是否是显示界面中的前几年的年假信息[广州新世界的要求同时显示近三年年假信息] add by sammen 20250224</param>
    private void GetAnnualLeaveInfo(HttpContext context,String strDCNO,String strYearMonth,String strIsShowPre)
    {
        try
        {
            if (!strIsShowPre.Equals("1"))
            {   
                //如果只是为了同时显示之前年份的年假，则不重新计算 add by sammen 20250224
                try
                {
                    //add by Michael 20200319 add auto refresh al
                    String strSQL_ExecuteSP = "USP_HR_AHOL_Calculate";
                    String strCalcuteDate = DateTime.Now.ToString("yyyy-MM-dd");
                    String strInputYear = "20" + strYearMonth.Substring(0, 2);
                    if (!(strInputYear).Equals(DateTime.Now.ToString("yyyy")))
                    {
                        strCalcuteDate = strInputYear + "-12-31";
                    }
                    Hashtable hsTableParam = new Hashtable();
                    hsTableParam.Add("P1", strDCNO);
                    hsTableParam.Add("P2", strCalcuteDate);
                    hsTableParam.Add("P0", "HRDOCU");
                    int iiReturnValue = SqlParamDao.ExcuteSP(strSQL_ExecuteSP, hsTableParam);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    context.Response.Write("error");
                }
            }

            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select * from [VW_QRY_AHOL_ForLeaveOverviewPage] where 1=1");
            sbSql.Append(" and [员工编号] = '"+strDCNO+"' and [年度] = (select LEFT(CONVERT(VARCHAR(20),pend,23),4) from KQPERD_1 WHERE PID = '"+strYearMonth+"')");
            sbSql.Append(" ORDER BY 员工编号,[年度] desc");
            String strSql = sbSql.ToString();

            //获取是否显示福利假区域
            String strIsShowFLJArea = Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("IsShowFLJArea_StaffLeaveOverview");
            //获取员工假期一览表里是否显示福利假过期作废天数
            String strIsShowFLJDue = Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("IsShowFLJDue_StaffLeaveOverview");

            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);

            bool BisTest = dt.Columns.Contains("员工编号");
            bool BisTest1 = dt.Columns.Contains("员工编号1");


            int iCount = dt.Rows.Count;
            StringBuilder sBuilder_ClientInfo = new StringBuilder();
            sBuilder_ClientInfo.Append("{ResultData:[{ ");
            sBuilder_ClientInfo.Append("DCNO:'" + (iCount<=0 ?"":dt.Rows[0]["员工编号"].ToString())+ "'");
            sBuilder_ClientInfo.Append(",StaffName:'" + (iCount<=0  ?"":dt.Rows[0]["英文名"].ToString())+ "'");
            sBuilder_ClientInfo.Append(",StaffNameChs:'" + (iCount<=0  ?"":dt.Rows[0]["中文名"].ToString())+ "'");
            sBuilder_ClientInfo.Append(",Year:'" + (iCount<=0 ?"":dt.Rows[0]["年度"].ToString())+ "'");
            sBuilder_ClientInfo.Append(",DeptName:'" + (iCount<=0 ?"":dt.Rows[0]["部门"].ToString())+ "'");
            sBuilder_ClientInfo.Append(",PosiName:'" + (iCount<=0 ?"":dt.Rows[0]["职位"].ToString())+ "'");
            sBuilder_ClientInfo.Append(",PosiLevel:'" + (iCount<=0 ?"":dt.Rows[0]["员工级别"].ToString())+ "'");
            sBuilder_ClientInfo.Append(",JoinDate:'" + (iCount<=0 ?"":dt.Rows[0]["入职日期"].ToString())+ "'");
            sBuilder_ClientInfo.Append(",LeaveDate:'" + (iCount<=0 ?"":dt.Rows[0]["离职日期"].ToString())+ "'");
            sBuilder_ClientInfo.Append(",ServiceYear:'" + (iCount<=0 ?"":dt.Rows[0]["服务年限"].ToString())+ "'");
            sBuilder_ClientInfo.Append(",STDDAY:'" + (iCount<=0 ?"":dt.Rows[0]["标准天数"].ToString())+ "'");
            sBuilder_ClientInfo.Append(",BUILDDAY:'" + (iCount<=0 ?"":dt.Rows[0]["生成天数"].ToString())+ "'");
            sBuilder_ClientInfo.Append(",USEDAY:'" + (iCount<=0 ?"":dt.Rows[0]["已用天数"].ToString())+ "'");
            sBuilder_ClientInfo.Append(",PREREMAIN:'" + (iCount<=0 ?"":dt.Rows[0]["往年余额"].ToString())+ "'");
            sBuilder_ClientInfo.Append(",CHANGEOFF:'" + (iCount<=0 ?"":dt.Rows[0]["核销天数"].ToString())+ "'"); //ADD BY SAMMEN 20250223
            sBuilder_ClientInfo.Append(",REMAINDAY:'" + (iCount<=0 ?"":dt.Rows[0]["剩余天数"].ToString())+ "'");
            ///add by sammen 20210831新增福利假字段
            sBuilder_ClientInfo.Append(",FLServiceYear:'" + (iCount<=0 ?"":dt.Columns.Contains("福利年限")?dt.Rows[0]["福利年限"].ToString():dt.Rows[0]["服务年限"].ToString())+ "'");
            sBuilder_ClientInfo.Append(",FLSTDDAY:'" + (iCount<=0 ?"":dt.Columns.Contains("福利假标准天数")?dt.Rows[0]["福利假标准天数"].ToString():dt.Rows[0]["标准天数"].ToString())+ "'");
            sBuilder_ClientInfo.Append(",FLBUILDDAY:'" + (iCount<=0 ?"":dt.Columns.Contains("福利假生成天数")?dt.Rows[0]["福利假生成天数"].ToString():dt.Rows[0]["生成天数"].ToString())+ "'");
            sBuilder_ClientInfo.Append(",FLUSEDAY:'" + (iCount<=0 ?"":dt.Columns.Contains("福利假已用天数")?dt.Rows[0]["福利假已用天数"].ToString():dt.Rows[0]["已用天数"].ToString())+ "'");
            sBuilder_ClientInfo.Append(",FLPREREMAIN:'" + (iCount<=0 ?"":dt.Columns.Contains("福利假往年余额")?dt.Rows[0]["福利假往年余额"].ToString():dt.Rows[0]["往年余额"].ToString())+ "'");
            sBuilder_ClientInfo.Append(",FLREMAINDAY:'" + (iCount<=0 ?"":dt.Columns.Contains("福利假剩余天数")?dt.Rows[0]["福利假剩余天数"].ToString():dt.Rows[0]["剩余天数"].ToString())+ "'");
            ///add by sammen 20210831新增福利假字段

            ///add by sammen 20221121新增福利假过期作废字段
            sBuilder_ClientInfo.Append(",FLDUEDAY:'" + (iCount<=0 ?"":dt.Columns.Contains("福利假过期作废天数")?dt.Rows[0]["福利假过期作废天数"].ToString():"0.0")+ "'");


            sBuilder_ClientInfo.Append(",DeadLine:'" + (iCount<=0 ?"":dt.Rows[0]["年假截止日期"].ToString())+ "'");
            sBuilder_ClientInfo.Append(",LastCalDate:'" + (iCount<=0 ?"":dt.Rows[0]["最后计算日期"].ToString())+ "'");
            sBuilder_ClientInfo.Append("}]");
            sBuilder_ClientInfo.Append(",IsShowFLJArea:'"+strIsShowFLJArea+"'");
            sBuilder_ClientInfo.Append(",IsShowFLJDue:'"+strIsShowFLJDue+"'");
            sBuilder_ClientInfo.Append("}");
            String strClientInfo = sBuilder_ClientInfo.ToString();
            context.Response.Write(strClientInfo);
        }
        catch (Exception ex)
        {
            context.Response.Write("error");
            log.Error(ex);
        }
    }

    /// <summary>
    /// 获取考勤期间列表
    /// </summary>
    /// <param name="context"></param>
    private void GetYearMonthList(HttpContext context)
    {
        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select PID,CONVERT(VARCHAR(20),PSTART,23) AS PSTART,CONVERT(VARCHAR(20),PEND,23) AS PEND,PADAYS,PDAYS,PLOCK,PKQISNOW,PISNOW from KQPERD_1 where 1=1");
            sbSql.Append(" and PID <= (SELECT PID FROM KQPERD_1 WHERE CONVERT(VARCHAR(20),GETDATE(),23) BETWEEN CONVERT(VARCHAR(20),PSTART,23) AND CONVERT(VARCHAR(20),PEND,23))");
            sbSql.Append(" ORDER BY PID DESC");
            String strSql = sbSql.ToString();

            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);

            int iColCount = dt.Columns.Count;
            StringBuilder sBuilder = new StringBuilder();
            sBuilder.Append("{");
            sBuilder.Append(WebCommon.GetJsonStringByDataTable(dt, "\"ResultData\"",true));
            sBuilder.Append("}");
            string json = sBuilder.ToString();

            context.Response.Write(json);
        }
        catch (Exception ex)
        {
            context.Response.Write("error");
            log.Error(ex);
        }
    }

    /// <summary>
    /// 获取查询年度列表
    /// </summary>
    /// <param name="context"></param>
    private void GetQueryYearList(HttpContext context)
    {
        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select distinct '20'+left(Ymonth,2) AS QueryYear from KQRSSZ_1 ORDER BY '20'+left(Ymonth,2) desc");
            String strSql = sbSql.ToString();

            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);

            int iColCount = dt.Columns.Count;
            StringBuilder sBuilder = new StringBuilder();
            sBuilder.Append("{");
            sBuilder.Append(WebCommon.GetJsonStringByDataTable(dt, "\"ResultData\"",true));
            sBuilder.Append("}");
            string json = sBuilder.ToString();

            context.Response.Write(json);
        }
        catch (Exception ex)
        {
            context.Response.Write("error");
            log.Error(ex);
        }
    }


    /// <summary>
    /// 获取员工考勤期间的各假期汇总列表
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strDCNO"></param>
    /// <param name="strYearMonth"></param>
    private void GetLeaveSummaryData(HttpContext context,String strDCNO,String strYearMonth)
    {
        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select A.TYPESHORT,A.TYPENAMECHS");
            sbSql.Append(" ,(select isnull(SUM(J.LVDAYS),0.00) from KQLV_1 I INNER JOIN KQLV_2 J ON I.LVNO = J.LVNO");
            sbSql.Append("      WHERE I.LVAUDTSTAT = '090' AND I.EMPLOYEE = '"+strDCNO+"' AND I.LVTYPE = A.TYPENO AND J.YearMonth = '"+strYearMonth+"'");
            sbSql.Append("  ) AS LVDAYS");
            sbSql.Append(" from LVTYPE_1 A WHERE BISSTOP <> '1' ORDER BY TYPENO");
            String strSql = sbSql.ToString();

            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);

            int iColCount = dt.Columns.Count;
            StringBuilder sBuilder = new StringBuilder();
            sBuilder.Append("{");
            sBuilder.Append(WebCommon.GetJsonStringByDataTable(dt, "\"ResultData\"",true));
            sBuilder.Append("}");

            string json = sBuilder.ToString();

            context.Response.Write(json);
        }
        catch (Exception ex)
        {
            context.Response.Write("error");
            log.Error(ex);
        }
    }

    /// <summary>
    /// 获取当前月度加班休假余额信息
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strDCNO"></param>
    /// <param name="strYearMonth"></param>
    public void GetOTLVBalance(HttpContext context,String strDCNO,String strYearMonth)
    {
        try
        {
            try
            {
                //add by Michael 20200319 add auto refresh otlvbal
                //获取前自动计算加班休假余额的最新数据，20201109新增独立存储过程USP_KQ_ComputeOLBalance_ByStaff进行计算
                String strSQL_ExecuteSP = "USP_KQ_ComputeOLBalance_ByStaff";
                Hashtable hsTableParam = new Hashtable();
                hsTableParam.Add("DCNO", strDCNO);
                hsTableParam.Add("YearMonth", strYearMonth);
                hsTableParam.Add("UserId", this.GetUserCode());
                int iReturnValue = SqlParamDao.ExcuteSP(strSQL_ExecuteSP, hsTableParam);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                context.Response.Write("error");
            }

            StringBuilder sbSql = new StringBuilder();
            StringBuilder sbSql_HIS = new StringBuilder();
            sbSql.Append("select SEQNO,EM_NO AS StaffNo, Ymonth as YearMonth,A.LOTLVB AS [LimitBegin],A.UPOTHR AS [LimitOT],A.CPLHR AS [LimitLV],A.OTLVT AS [LimitEnd] from KQRSSZ_1 A ");
            sbSql.Append(" WHERE EM_NO = '"+strDCNO+"' AND Ymonth = '"+strYearMonth+"' ");

            sbSql_HIS.Append("select SEQNO,EM_NO AS StaffNo,Ymonth as YearMonth,A.LOTLVB AS [LimitBegin],A.UPOTHR AS [LimitOT],A.CPLHR AS [LimitLV],A.OTLVT AS [LimitEnd] ");
            sbSql_HIS.Append(" ,A.Ymonth+'#('+CONVERT(VARCHAR(20),B.PSTART,23)+'至'+CONVERT(VARCHAR(20),B.PEND,23)+')' AS YearMonthScope");
            sbSql_HIS.Append(" from KQRSSZ_1 A inner join KQPERD_1 B ON A.Ymonth = B.PID WHERE EM_NO = '"+strDCNO+"' ORDER BY Ymonth DESC ");

            DataTable dt = SqlParamDao.GetDataTableBySql(sbSql.ToString());
            DataTable dtHIS = SqlParamDao.GetDataTableBySql(sbSql_HIS.ToString());

            int iColCount = dt.Columns.Count;

            //获取今天的调休余额数据
            StringBuilder sBuilderTodayBalance = new StringBuilder();
            try{

                //获取是否显示截止到今天的余额区域
                String strIsShowTodayBalance = Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("IsShowTodayBalance_StaffLeaveOverview");

                sBuilderTodayBalance.Append("\"isShowTodayBalance\":\"" + strIsShowTodayBalance + "\"");
                if(strIsShowTodayBalance.Equals("1")){
                    String strTodayDate = DateTime.Now.ToString("yyyy-MM-dd");
                    String strSql_Today = "select *,CONVERT(VARCHAR(20),PSTART,23) as StartDate from KQPERD_1 WHERE '" + strTodayDate + "' BETWEEN CONVERT(VARCHAR(20),PSTART,23) AND CONVERT(VARCHAR(20),PEND,23) ";
                    DataTable dt_Today = SqlParamDao.GetDataTableBySql(strSql_Today.ToString());
                    String strPID_Today = dt_Today.Rows[0]["PID"].ToString();
                    String strPID_Today_StartDate = dt_Today.Rows[0]["StartDate"].ToString();
                    //今天在所选中的期间才去计算
                    if (strPID_Today.Equals(strYearMonth)) {
                        StringBuilder sbSql_LimitToday = new StringBuilder();
                        //计算截止到今天的余额=本期间的期初余额+本期第一天到今天的调休加班-本期第一天到今天的调休休假
                        ////期间的期初余额
                        //float fLimitBegin = float.Parse(dt.Rows[0]["LimitBegin"].ToString());
                        //sbSql_LimitToday.Append("select (ISNULL((SELECT sum(I.OTTIME) FROM KQOVTM_1 I  WHERE OTPTYPE = '1' AND OTAUDTSTAT = '090' \r\n");
                        //sbSql_LimitToday.Append("		AND EMPLOYEE = A.DCNO AND (CONVERT(VARCHAR(20),I.OTDATEF,23) BETWEEN '"+strPID_Today_StartDate+"' AND '"+strTodayDate+"' ))\r\n");
                        //sbSql_LimitToday.Append("		,0.00) \r\n");
                        //sbSql_LimitToday.Append("	 -ISNULL((SELECT sum(J.LVTIME) FROM KQLV_1 I INNER JOIN KQLV_4 J ON I.LVNO = J.LVNO WHERE LVTYPE = 'LV080' AND LVAUDTSTAT = '090' \r\n");
                        //sbSql_LimitToday.Append("		AND EMPLOYEE = A.DCNO AND (CONVERT(VARCHAR(20),J.LVDATEF,23) BETWEEN '"+strPID_Today_StartDate+"' AND '"+strTodayDate+"' )) \r\n");
                        //sbSql_LimitToday.Append("		,0.00) \r\n");
                        //sbSql_LimitToday.Append("	) AS LimitToday\r\n");
                        //sbSql_LimitToday.Append(" from HRDOCU_1 A where DCNO = '"+strDCNO+"'\r\n");
                        //DataTable dt_LimitToday = SqlParamDao.GetDataTableBySql(sbSql_LimitToday.ToString());
                        ////本期第一天到今天的调休发生额
                        //float fLimitToday = float.Parse(dt_LimitToday.Rows[0]["LimitToday"].ToString());
                        //sBuilderTodayBalance.Append("\"todayDate\":\"" + strTodayDate + "\"");
                        //sBuilderTodayBalance.Append(",\"todayBalance\":\"" + (fLimitBegin+fLimitToday).ToString() + "\"");

                        //通过数据库函数获取 modify by sammen 20210915
                        sbSql_LimitToday.Append("select dbo.Fun_HR_QRY_OTLVBal_ByDate('','"+strTodayDate+"','"+strDCNO+"') \r\n");
                        DataTable dt_LimitToday = SqlParamDao.GetDataTableBySql(sbSql_LimitToday.ToString());
                        float fLimitToday = float.Parse(dt_LimitToday.Rows[0][0].ToString());

                        sBuilderTodayBalance.Append(",\"todayDate\":\"" + strTodayDate + "\"");
                        sBuilderTodayBalance.Append(",\"todayBalance\":\"" + (fLimitToday).ToString() + "\"");
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("获取今天的调休余额数据失败："+ex);
            }

            StringBuilder sBuilder = new StringBuilder();
            sBuilder.Append("{");
            sBuilder.Append(WebCommon.GetJsonStringByDataTable(dt, "\"ResultData\"",true));
            sBuilder.Append(","+WebCommon.GetJsonStringByDataTable(dtHIS, "ResultData_HIS",true));
            if(!String.IsNullOrEmpty(sBuilderTodayBalance.ToString())){
                sBuilder.Append(","+sBuilderTodayBalance.ToString());
            }
            sBuilder.Append("}");

            string json = sBuilder.ToString();

            context.Response.Write(json);
        }
        catch (Exception ex)
        {
            log.Error(ex);
            context.Response.Write("error");
        }
    }

    /// <summary>
    /// 获取休假记录信息
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strDCNO"></param>
    /// <param name="strLVTYPE"></param>
    /// <param name="strLVStatus"></param>
    /// <param name="strYearMonth"></param>
    /// <param name="strDateFrom"></param>
    /// <param name="strDateTo"></param>
    public void GetLeaveRecordData(HttpContext context,String strDCNO,String strLVTYPE,String strLVStatus ,String strYearMonth, String strDateFrom, String strDateTo)
    {
        try
        {
            if (String.IsNullOrEmpty(strDateFrom))
            {
                strDateFrom = "1900-01-01";
            }
            if (String.IsNullOrEmpty(strDateTo))
            {
                strDateTo = "2999-12-31";
            }
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select A.LVNO,A.EMPLOYEE,A.CNAME,A.ENAME,A.LVTYPE,CONVERT(VARCHAR(30),A.LVDATEF,120) AS LVDATEF,CONVERT(VARCHAR(30),A.LVDATET,120) AS LVDATET,A.LVDAYS,A.LVTIME,A.ISSALARY,A.WHRTYPE");
            sbSql.Append(" ,(select TYPENAMECHS from LVTYPE_1 WHERE TYPENO = A.LVTYPE ) AS LVTYPE_DESC");
            sbSql.Append(" ,(select CDESCCHS from TB_HRLSTD WHERE LID = 'BOOL' AND CID = A.ISSALARY ) AS ISSALARY_DESC");
            sbSql.Append(" ,(select CDESCCHS from TB_HRLSTD WHERE LID = 'LVWHTYPE' AND CID = A.WHRTYPE ) AS WHRTYPE_DESC");
            sbSql.Append(" ,(select CDESCCHS from TB_HRLSTD WHERE LID = 'KQLVSTATUS' AND CID = A.LVAUDTSTAT ) AS LVAUDTSTAT_DESC");
            sbSql.Append(" from KQLV_1 A WHERE A.EMPLOYEE = '"+strDCNO+"' ");
            sbSql.Append(" AND ((CONVERT(VARCHAR(20),LVDATEF,23) BETWEEN '"+strDateFrom+"' AND '"+strDateTo+"') OR (CONVERT(VARCHAR(20),LVDATET,23) BETWEEN '"+strDateFrom+"' AND '"+strDateTo+"'))");
            if (!String.IsNullOrEmpty(strLVTYPE))
            {
                sbSql.Append(" AND A.LVTYPE = '"+strLVTYPE+"'");
            }
            //sbSql.Append(" AND A.LVAUDTSTAT = '090'");
            if (!String.IsNullOrEmpty(strLVStatus))
            {
                sbSql.Append(" AND A.LVAUDTSTAT = '"+strLVStatus+"'");
            }
            sbSql.Append(" order by CONVERT(VARCHAR(30),A.LVDATEF,120) DESC,CONVERT(VARCHAR(30),A.LVDATET,120) DESC ");

            DataTable dt = SqlParamDao.GetDataTableBySql(sbSql.ToString());
            int iColCount = dt.Columns.Count;
            StringBuilder sBuilder = new StringBuilder();
            sBuilder.Append("{");
            sBuilder.Append(WebCommon.GetJsonStringByDataTable(dt, "\"ResultData\"",true));
            sBuilder.Append("}");

            string json = sBuilder.ToString();

            context.Response.Write(json);
        }
        catch (Exception ex)
        {
            log.Error(ex);
            context.Response.Write("error");
        }
    }
    /// <summary>
    /// 获取假期类型列表
    /// </summary>
    /// <param name="context"></param>
    private void GetLVTYPEList(HttpContext context)
    {
        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select * from LVTYPE_1 WHERE BISSTOP <> '1' ORDER BY TYPENO ");
            DataTable dt = SqlParamDao.GetDataTableBySql(sbSql.ToString());
            int iColCount = dt.Columns.Count;
            StringBuilder sBuilder = new StringBuilder();
            sBuilder.Append("{");
            sBuilder.Append(WebCommon.GetJsonStringByDataTable(dt, "\"ResultData\"",true));
            sBuilder.Append("}");

            string json = sBuilder.ToString();

            context.Response.Write(json);

        }
        catch (Exception ex)
        {
            log.Error(ex);
            context.Response.Write("error");
        }
    }

    /// <summary>
    /// 获取KQLV审批状态列表
    /// </summary>
    /// <param name="context"></param>
    private void GetLVApproveStatusList(HttpContext context)
    {
        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select * from TB_HRLSTD WHERE LID = 'KQLVSTATUS' AND BISSTOP <> '1' ORDER BY CID ");
            DataTable dt = SqlParamDao.GetDataTableBySql(sbSql.ToString());
            int iColCount = dt.Columns.Count;
            StringBuilder sBuilder = new StringBuilder();
            sBuilder.Append("{");
            sBuilder.Append(WebCommon.GetJsonStringByDataTable(dt, "\"ResultData\"",true));
            sBuilder.Append("}");

            string json = sBuilder.ToString();

            context.Response.Write(json);

        }
        catch (Exception ex)
        {
            log.Error(ex);
            context.Response.Write("error");
        }
    }

    /// <summary>
    /// 获取KQLV审批状态列表
    /// </summary>
    /// <param name="context"></param>
    private void GetOTApproveStatusList(HttpContext context)
    {
        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select * from TB_HRLSTD WHERE LID = 'KQOVTMSTATUS' AND BISSTOP <> '1' ORDER BY CID ");
            DataTable dt = SqlParamDao.GetDataTableBySql(sbSql.ToString());
            int iColCount = dt.Columns.Count;
            StringBuilder sBuilder = new StringBuilder();
            sBuilder.Append("{");
            sBuilder.Append(WebCommon.GetJsonStringByDataTable(dt, "\"ResultData\"",true));
            sBuilder.Append("}");

            string json = sBuilder.ToString();

            context.Response.Write(json);

        }
        catch (Exception ex)
        {
            log.Error(ex);
            context.Response.Write("error");
        }
    }

    /// <summary>
    /// 获取员工考勤期间的各加班单汇总列表
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strDCNO"></param>
    /// <param name="strYearMonth"></param>
    private void GetOvertimeSummaryData(HttpContext context,String strDCNO,String strYearMonth)
    {
        try
        {
            //加班类型
            StringBuilder sbSql_OTTYPE = new StringBuilder();
            sbSql_OTTYPE.Append("select A.OTTYPE,A.TYPENAMECHS");
            sbSql_OTTYPE.Append(" ,(SELECT isnull(SUM(OTDAYS),0.00) FROM KQOVTM_1 WHERE OTAUDTSTAT = '090' AND EMPLOYEE = '"+strDCNO+"' AND OTTYPE = A.OTTYPE AND YearMonth = '"+strYearMonth+"'");
            sbSql_OTTYPE.Append("  ) AS OTDAYS");
            sbSql_OTTYPE.Append(" from OTTYPE_1 A WHERE BISSTOP <> '1' ORDER BY OTTYPE");
            String strSql_OTTYPE = sbSql_OTTYPE.ToString();
            DataTable dt_OTTYPE = SqlParamDao.GetDataTableBySql(strSql_OTTYPE);

            //支付类型
            StringBuilder sbSql_PayType = new StringBuilder();
            sbSql_PayType.Append("select A.CID,A.CDESCCHS");
            sbSql_PayType.Append(" ,(SELECT isnull(SUM(OTDAYS),0.00) FROM KQOVTM_1 WHERE OTAUDTSTAT = '090' AND EMPLOYEE = '"+strDCNO+"' AND OTPTYPE = A.CID AND YearMonth = '"+strYearMonth+"'");
            sbSql_PayType.Append("  ) AS OTDAYS");
            sbSql_PayType.Append(" from TB_HRLSTD A WHERE LID = 'OTPTYPE' and BISSTOP <> '1' ORDER BY CID");
            String strSql_PayType = sbSql_PayType.ToString();

            DataTable dt_PayType = SqlParamDao.GetDataTableBySql(strSql_PayType);

            StringBuilder sBuilder = new StringBuilder();
            sBuilder.Append("{");
            sBuilder.Append(WebCommon.GetJsonStringByDataTable(dt_OTTYPE, "\"ResultData_OTTYPE\"",true));
            sBuilder.Append(",");
            sBuilder.Append(WebCommon.GetJsonStringByDataTable(dt_PayType, "\"ResultData_PayType\"",true));
            sBuilder.Append("}");

            string json = sBuilder.ToString();

            context.Response.Write(json);
        }
        catch (Exception ex)
        {
            context.Response.Write("error");
            log.Error(ex);
        }
    }

    /// <summary>
    /// 获取加班类型列表
    /// </summary>
    /// <param name="context"></param>
    private void GetOTTYPEList(HttpContext context)
    {
        try
        {
            StringBuilder sbSql_OTTYPE = new StringBuilder();
            sbSql_OTTYPE.Append("select * from OTTYPE_1 WHERE BISSTOP <> '1' ORDER BY OTTYPE ");
            DataTable dt_OTTYPE = SqlParamDao.GetDataTableBySql(sbSql_OTTYPE.ToString());

            StringBuilder sbSql_PayType = new StringBuilder();
            sbSql_PayType.Append("select * from TB_HRLSTD WHERE LID = 'OTPTYPE' and BISSTOP <> '1' ORDER BY CID ");
            DataTable dt_PayType = SqlParamDao.GetDataTableBySql(sbSql_PayType.ToString());

            StringBuilder sBuilder = new StringBuilder();
            sBuilder.Append("{");
            sBuilder.Append(WebCommon.GetJsonStringByDataTable(dt_OTTYPE, "\"ResultData_OTTYPE\"",true));
            sBuilder.Append(",");
            sBuilder.Append(WebCommon.GetJsonStringByDataTable(dt_PayType, "\"ResultData_PayType\"",true));
            sBuilder.Append("}");

            string json = sBuilder.ToString();

            context.Response.Write(json);

        }
        catch (Exception ex)
        {
            log.Error(ex);
            context.Response.Write("error");
        }
    }

    /// <summary>
    /// 获取加班记录信息
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strDCNO"></param>
    /// <param name="strOTTYPE"></param>
    /// <param name="strPayType"></param>
    /// <param name="strOTStatus"></param>
    /// <param name="strYearMonth"></param>
    /// <param name="strDateFrom"></param>
    /// <param name="strDateTo"></param>
    public void GetOvertimeRecordData(HttpContext context,String strDCNO,String strOTTYPE,String strPayType,String strOTStatus,String strYearMonth, String strDateFrom, String strDateTo)
    {
        try
        {
            if (String.IsNullOrEmpty(strDateFrom))
            {
                strDateFrom = "1900-01-01";
            }
            if (String.IsNullOrEmpty(strDateTo))
            {
                strDateTo = "2999-12-31";
            }
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select A.OTNO,A.EMPLOYEE,A.CNAME,A.ENAME,A.OTTYPE,CONVERT(VARCHAR(30),A.OTDATEF,23) AS OTDATEF,A.OTTMF,A.OTTMT,A.OTDAYS,A.OTTIME,A.OTPTYPE,A.WHRTYPE,A.OTMEMO");
            sbSql.Append(" ,(select TYPENAMECHS from OTTYPE_1 WHERE OTTYPE = A.OTTYPE ) AS OTTYPE_DESC");
            sbSql.Append(" ,(select CDESCCHS from TB_HRLSTD WHERE LID = 'OTPTYPE' AND CID = A.OTPTYPE ) AS OTPTYPE_DESC");
            sbSql.Append(" ,(select CDESCCHS from TB_HRLSTD WHERE LID = 'OTWHTYPE' AND CID = A.WHRTYPE ) AS WHRTYPE_DESC");
            sbSql.Append(" ,(select CDESCCHS from TB_HRLSTD WHERE LID = 'KQOVTMSTATUS' AND CID = A.OTAUDTSTAT ) AS OTAUDTSTAT_DESC");
            sbSql.Append(" from KQOVTM_1 A WHERE A.EMPLOYEE = '"+strDCNO+"' ");
            //sbSql.Append(" AND A.OTAUDTSTAT = '090'");
            if (!String.IsNullOrEmpty(strOTStatus))
            {
                sbSql.Append(" AND A.OTAUDTSTAT = '"+strOTStatus+"'");
            }

            if (!String.IsNullOrEmpty(strDateFrom) && !String.IsNullOrEmpty(strDateTo))
            {
                sbSql.Append(" AND (CONVERT(VARCHAR(20),OTDATEF,23) BETWEEN '"+strDateFrom+"' AND '"+strDateTo+"')");
            }else if (!String.IsNullOrEmpty(strYearMonth))
            {
                sbSql.Append(" AND A.YEARMONTH = '"+strYearMonth+"'");
            }
            if (!String.IsNullOrEmpty(strOTTYPE))
            {
                sbSql.Append(" AND A.OTTYPE = '"+strOTTYPE+"'");
            }
            if (!String.IsNullOrEmpty(strPayType))
            {
                sbSql.Append(" AND A.OTPTYPE = '"+strPayType+"'");
            }
            sbSql.Append(" order by CONVERT(VARCHAR(30),A.OTDATEF,23) DESC ");

            log.Error("获取加班记录信息,SQL:"+sbSql.ToString());
            DataTable dt = SqlParamDao.GetDataTableBySql(sbSql.ToString());
            int iColCount = dt.Columns.Count;
            StringBuilder sBuilder = new StringBuilder();
            sBuilder.Append("{");
            sBuilder.Append(WebCommon.GetJsonStringByDataTable(dt, "\"ResultData\"",true));
            sBuilder.Append("}");

            string json = sBuilder.ToString();

            context.Response.Write(json);
        }
        catch (Exception ex)
        {
            log.Error(ex);
            context.Response.Write("error");
        }
    }

    /// <summary>
    /// 获取加班单FLOT中的人员列表
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strFlowCode"></param>
    private void GetFLOTStaffList(HttpContext context,String strFlowCode)
    {
        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select A.*,B.DEPTNAMECHS1,B.DEPTNAMECHS2,B.DEPTNAMECHS3,B.POSINAMECHS from FLOT_2 A INNER JOIN VW_HRDOCU_DEPARTMENT B ON A.EMPLOYEE = B.DCNO");
            sbSql.Append(" where A.FlowCode = '"+strFlowCode+"'");
            sbSql.Append(" ORDER BY A.EMPLOYEE");
            String strSql = sbSql.ToString();

            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);

            int iColCount = dt.Columns.Count;
            StringBuilder sBuilder = new StringBuilder();
            sBuilder.Append("{");
            sBuilder.Append(WebCommon.GetJsonStringByDataTable(dt, "\"ResultData\"",true));
            sBuilder.Append("}");
            string json = sBuilder.ToString();

            context.Response.Write(json);
        }
        catch (Exception ex)
        {
            context.Response.Write("error");
            log.Error(ex);
        }
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}