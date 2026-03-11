<%@ WebHandler Language="C#" Class="ArrangeShiftHandler" %>

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
/// 排班操作的处理类
/// </summary>
public class ArrangeShiftHandler : HandlerBase, IHttpHandler, System.Web.SessionState.IRequiresSessionState {

    /// <summary>
    /// 日志声明
    /// </summary>
    protected static Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    //获取配置中的周锁定是否针对全部部门的设置
    private String strIsLockWeekForCurSection = Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("Switch_IsLockWeekForCurSection");

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
        string strUserType = hsTableUrlQuery["usertype"] == null ? "" : hsTableUrlQuery["usertype"].ToString();//排班操作用户类型（0或者空为小部门用户,1为人事部）
        strUserType = String.IsNullOrEmpty(strUserType) ? "0" : strUserType;
        string strTotalwidth = hsTableUrlQuery["totalwidth"] == null ? "1000" : hsTableUrlQuery["totalwidth"].ToString();//终端最大宽度
        string strStaffNo = hsTableUrlQuery["staffno"] == null ? string.Empty : hsTableUrlQuery["staffno"].ToString();//员工工号
        string strOneDay = hsTableUrlQuery["oneday"] == null ? string.Empty : hsTableUrlQuery["oneday"].ToString();//日期
        string strSqlCondition = hsTableUrlQuery["sqlcondition"] == null ? string.Empty : hsTableUrlQuery["sqlcondition"].ToString();//Sql条件语句
        string strSaveShiftData = hsTableUrlQuery["saveshiftdata"] == null ? string.Empty : hsTableUrlQuery["saveshiftdata"].ToString();//需保存的排班信息数据
        //string strSaveShiftData = WebCommon.GetJsonValue(strParamJson, "saveshiftdata").ToString();//需保存的排班信息数据
        string strTableName = hsTableUrlQuery["tablename"] == null ? string.Empty : hsTableUrlQuery["tablename"].ToString();//表名
        string strColName = hsTableUrlQuery["colname"] == null ? string.Empty : hsTableUrlQuery["colname"].ToString();//列名
        string strColValue = hsTableUrlQuery["colvalue"] == null ? string.Empty : hsTableUrlQuery["colvalue"].ToString();//列值
        string strIsModify = hsTableUrlQuery["ismodify"] == null ? string.Empty : hsTableUrlQuery["ismodify"].ToString();//是否可以考勤结果进行修改操作

        if (String.IsNullOrEmpty(strUserId)){
            strUserId = this.GetUserCode();
        }

        switch (strParam.ToLower().ToString())
        {
            case "getstafflist":
                context.Response.Write(this.GetStaffList(strUserId,strYearMonth,strSqlCondition,strTotalwidth).ToString());
                break;
            case "getcontextmenulist":
                context.Response.Write(this.GetContextMenuList(strUserType).ToString());
                break;
            case "getunnormaldata":
                context.Response.Write(this.GetUnNormalData(strUserId,strYearMonth).ToString());
                break;
            case "getonedayresultdata":
                context.Response.Write(this.GetOneDayResultData(strUserId,strYearMonth,strStaffNo,strOneDay,strTotalwidth,strIsModify).ToString());
                break;
            case "savestaffshiftdata":
                //由于此参数字符可能会过长，不适合使用get方式，而使用post方式
                strSaveShiftData = context.Request.Form["txt_SaveShiftData"]!=null?context.Request.Form["txt_SaveShiftData"].ToString():"";
                context.Response.Write(this.SaveStaffShiftData(this.GetUserCode(), strYearMonth, strSaveShiftData).ToString());
                break;
            case "savestaffresultdata":
                context.Response.Write(this.SaveStaffResultData(strUserId,strYearMonth,strStaffNo,strOneDay,strColName,strColValue).ToString());
                break;
        }
    }

    /// <summary>
    /// 获取员工排班列表信息
    /// </summary>
    /// <param name="strSecUserId"></param>
    /// <param name="strYearMonth"></param>
    /// <param name="strSqlCondition"></param>
    /// <param name="strTotalwidth"></param>
    /// <returns></returns>
    private String GetStaffList(String strSecUserId,String strYearMonth ,String strSqlCondition,String strTotalwidth)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbReturnRowData = new StringBuilder();
        StringBuilder sbReturnColumnData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "获取员工排班列表信息";
        int iColCount = 0;
        int iRowCount = 0;

        int iFixedColumnCount = 0;
        int iTotalFixedColumnWidth = 0;
        int iKQPERD_3ColumnCount = 0;
        try
        {
            sbReturnColumnData.Append("[");
            //获取配置表中需要显示的固定字段
            String strSqlConfig_3 = "SELECT * FROM KQASConfig_3 WHERE ConfigCode in (select top 1 ConfigCode from KQASConfig_1 order by ConfigCode) order by SORDER";
            DataTable dt_Config_3 = SqlParamDao.GetDataTableBySql(strSqlConfig_3);
            StringBuilder sbStaffListFixedColumns = new StringBuilder();
            if(dt_Config_3!=null && dt_Config_3.Rows.Count>0){
                iFixedColumnCount = dt_Config_3.Rows.Count;
                for(int i=0;i<iFixedColumnCount;i++){
                    String strTableName_Config_3 = dt_Config_3.Rows[i]["TableName"].ToString();
                    String strColumnName_Config_3 = dt_Config_3.Rows[i]["ColumnName"].ToString();
                    String strColumnDesc_Config_3 = dt_Config_3.Rows[i]["ColumnDesc"].ToString();
                    String strColumnWidth_Config_3 = dt_Config_3.Rows[i]["ColumnWidth"].ToString();

                    iTotalFixedColumnWidth = iTotalFixedColumnWidth + Convert.ToInt32(double.Parse(strColumnWidth_Config_3));

                    String strTableColumnName = strTableName_Config_3 + "." + strColumnName_Config_3;
                    switch (strColumnName_Config_3.ToUpper()) {
                        case "DCDDESCCHS":
                            strTableColumnName = "(select ODESCCHS FROM CSORGA_1 WHERE OID = " + strTableColumnName + ")";
                            break;
                        case "DCHRPOSI":
                            strTableColumnName = "(select PDESCCHS FROM CSPOSI_1 WHERE PID = " + strTableColumnName + ")";
                            break;
                        case "DCPLEVEL":
                            strTableColumnName = "(select CDESCCHS FROM TB_HRLSTD WHERE LID = 'PLEVEL' AND CID = " + strTableColumnName + ")";
                            break;
                        case "DCJOIN":
                        case "DCDMACT":
                            strTableColumnName = "(convert(varchar(20)," + strTableColumnName + ",23))";
                            break;
                        default:
                            break;
                    }
                    sbStaffListFixedColumns.Append(i==0?"":",");
                    sbStaffListFixedColumns.Append(strTableColumnName +" as ["+strColumnName_Config_3+"]\r\n");

                    //固定列数组
                    sbReturnColumnData.Append("{");
                    sbReturnColumnData.Append("\"type\": \"text\"");
                    sbReturnColumnData.Append(" , \"title\": \""+strColumnDesc_Config_3+"\"");
                    sbReturnColumnData.Append(" , \"width\": \""+strColumnWidth_Config_3+"\"");
                    sbReturnColumnData.Append(" , \"readOnly\": true ");
                    sbReturnColumnData.Append("},");
                }
            }

            //获取特定月份特定小部门的考勤分析状态位
            String strAnalysStatus = "1";
            String strSqlKQPERD_4 = "SELECT * FROM KQPERD_4 WHERE PID = '" + strYearMonth + "' AND SECTIONCODE in (select SEPNO from KQDL_2 where DCNO = '"+strSecUserId+"')";
            DataTable dt_KQPERD_4 = SqlParamDao.GetDataTableBySql(strSqlKQPERD_4);
            if (dt_KQPERD_4 != null && dt_KQPERD_4.Rows.Count > 0)
            {
                DataRow dr = dt_KQPERD_4.Rows[0];
                strAnalysStatus = dr["KQSTATUS"].ToString();
            }

            //获取每月的日期列表
            String strSqlKQPERD_3 = "select * from KQPERD_3 WHERE PID = '"+strYearMonth+"' order by [date]";
            DataTable dt_KQPERD_3 = SqlParamDao.GetDataTableBySql(strSqlKQPERD_3);
            StringBuilder sbKQPERD_3Columns = new StringBuilder();
            if (dt_KQPERD_3 != null && dt_KQPERD_3.Rows.Count > 0)
            {
                iKQPERD_3ColumnCount = dt_KQPERD_3.Rows.Count;
                //全表格的宽度
                int iTableWidth = Convert.ToInt32(double.Parse(strTotalwidth));
                //每一列日期列的宽度
                double iColWidth = Math.Round((double)(iTableWidth-iTotalFixedColumnWidth-20) / (double)iKQPERD_3ColumnCount,2);
                iColWidth = iColWidth < 40 ? 40 : iColWidth;


                for (int i = 0; i < iKQPERD_3ColumnCount; i++)
                {
                    String strDayNum = dt_KQPERD_3.Rows[i]["DNUM"].ToString();
                    String strDayDate = dt_KQPERD_3.Rows[i]["DATE"].ToString();
                    String strDayLock = dt_KQPERD_3.Rows[i]["PLOCK"].ToString();
                    String strLockedSections = dt_KQPERD_3.Rows[i]["LockedSections"].ToString();
                    String strTableColumnName = "KQTOE_2." + strDayNum;
                    String strDate = DateTime.Parse(dt_KQPERD_3.Rows[i]["DATE"].ToString()).ToString("yyyy-MM-dd");
                    String strColShowName = "D"+strDate.Substring(8,2);

                    sbKQPERD_3Columns.Append(i==0?"":",");
                    sbKQPERD_3Columns.Append(strTableColumnName+" as ["+strColShowName+"]\r\n");

                    //日期列数组
                    sbReturnColumnData.Append("{\"type\": \"text\", \"title\": \""+strColShowName+"\", \"width\": \""+iColWidth.ToString()+"\"");
                    //如果该日期被锁定，或者人事部审核或者审核完毕，则只读
                    //if (strDayLock.Equals("1") || (!strAnalysStatus.Equals("1")))
                    //{
                    //if (strDayLock.Equals("1")){
                    //    sbReturnColumnData.Append(", \"readOnly\": true");
                    //}                        
                    //只针对当前部门锁定
                    if(strIsLockWeekForCurSection.Equals("1")){
                        //锁定标志位为1且锁定用户也存在
                        if(strDayLock.Equals("1") && strLockedSections.ToUpper().IndexOf(strSecUserId.ToUpper()+",")>-1){
                            strDayLock = "1";
                        }else{
                            strDayLock = "0";
                        }
                    }
                    sbReturnColumnData.Append(", \"plock\": \""+strDayLock+"\"");
                    sbReturnColumnData.Append(", \"date\": \""+strDate+"\"");
                    sbReturnColumnData.Append(", \"day\":\""+strDayNum.Replace("D","")+"\"");
                    sbReturnColumnData.Append(", \"render\": \"square\"},");
                }
            }
            sbReturnColumnData.Append("]");
            iColCount = iFixedColumnCount + iKQPERD_3ColumnCount;


            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("SELECT "+sbStaffListFixedColumns.ToString()+","+sbKQPERD_3Columns.ToString());
            sbSql.Append(" FROM FUN_VW_PAIBAN_STAFF_FILTER_BYUSERID('"+strYearMonth+"','"+strSecUserId+"') A  \r\n");
            sbSql.Append(" inner join (SELECT * FROM TB_HR_MonthStaffList WHERE YEARMONTH = '"+strYearMonth+"') TB_HR_MonthStaffList on A.STAFFID = TB_HR_MonthStaffList.DCNO \r\n");
            sbSql.Append(" inner join KQTOE_2 KQTOE_2 on A.STAFFID = KQTOE_2.EMPLOYEE AND KQTOE_2.YEARMONTH = TB_HR_MonthStaffList.YEARMONTH\r\n");
            sbSql.Append(" WHERE A.SUSERID = '"+strSecUserId+"' AND TB_HR_MonthStaffList.YEARMONTH = '"+strYearMonth+"'\r\n");
            if(!String.IsNullOrEmpty(strSqlCondition)){
                sbSql.Append(" and "+strSqlCondition);
            }
            sbSql.Append(" ORDER BY TB_HR_MonthStaffList.DCDDESCCHS,TB_HR_MonthStaffList.DCPLEVEL,TB_HR_MonthStaffList.DCNO\r\n");
            //log.Error("StaffList SQL:" + sbSql.ToString());
            String strSql = sbSql.ToString();
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
            iRowCount = dt.Rows.Count;

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
            sbResultStatus.Append(",\"IsLockWeekForCurSection\":\"" + strIsLockWeekForCurSection + "\""); // add by sammen 20260209 兼顾到周锁定是否可以针对特定部门而不是全部部门
            sbResultStatus.Append(",\"ReturnColCount\":\"" + iColCount.ToString() + "\"");
            sbResultStatus.Append(",\"ReturnRowCount\":\"" + iRowCount.ToString() + "\"");
            sbResultStatus.Append(",\"ReturnFixedColumnCount\":\"" + iFixedColumnCount.ToString() + "\"");
            sbResultStatus.Append(",\"ReturnTotalFixedColumnWidth\":\"" + iTotalFixedColumnWidth.ToString() + "\"");
            sbResultStatus.Append(",\"ReturnMonthDayColumnCount\":\"" + iKQPERD_3ColumnCount.ToString() + "\"");


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
    /// 获取排班表中右键功能菜单配置
    /// </summary>
    /// <returns></returns>
    private String GetContextMenuList(String strUserType)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbReturnRowData = new StringBuilder();
        StringBuilder sbReturnColumnData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "获取排班表中右键功能菜单配置";

        try
        {
            StringBuilder sbSql = new StringBuilder();
            String strSqlConfig_5 = "SELECT * FROM KQASConfig_5 WHERE IsShow = '1' and ConfigCode in (select top 1 ConfigCode from KQASConfig_1 order by ConfigCode) ";
            if(!String.IsNullOrEmpty(strUserType)){
                strSqlConfig_5 = strSqlConfig_5 + " and (isnull(PaibanUserType,'') in ('','"+strUserType+"')) ";
            }
            strSqlConfig_5 = strSqlConfig_5+ " order by ShowOrder";
            DataTable dt_Config_5 = SqlParamDao.GetDataTableBySql(strSqlConfig_5);

            sbReturnRowData.Append(WebCommon.GetJsonStringByDataTable(dt_Config_5,"",false));

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
                sbResult.Append(",\"ReturnRowData\"" + sbReturnRowData.ToString()+"");
            }
            sbResult.Append("}");
            //log.Error(strMethodDesc + "Return Json:" + sbResult.ToString());
        }
        return sbResult.ToString();
    }

    /// <summary>
    /// 获取员工某一月份考勤非正常结果信息数据
    /// </summary>
    /// <param name="strSecUserId"></param>
    /// <param name="strYearMonth"></param>
    /// <returns></returns>
    private String GetUnNormalData(String strSecUserId,String strYearMonth)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbReturnRowData = new StringBuilder();
        StringBuilder sbReturnColumnData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "获取员工某一月份考勤非正常结果信息数据";
        int iRowCount = 0;

        try
        {
            StringBuilder sbSql = new StringBuilder();
            //sbSql.Append("SELECT A.* FROM VW_HR_KQ_UNNORMAL A LEFT JOIN FUN_VW_PAIBAN_STAFF_FILTER_BYUSERID('" + strYearMonth + "','" + strSecUserId + "') B ON A.SSTAFFNO = B.STAFFID \r\n");
            //sbSql.Append(" WHERE [SYEARMONTH]='" + strYearMonth + "' and B.SUSERID = '" + strSecUserId + "' order by SSTAFFNO, convert(int,[sday]) \r\n");

            //modify by sammen 20230224 由于VW_HR_KQ_UNNORMAL关联FUN_VW_PAIBAN_STAFF_FILTER_BYUSERID后速度比较慢，考虑取出全部异常数据
            sbSql.Append("SELECT A.* FROM VW_HR_KQ_UNNORMAL A  \r\n");
            sbSql.Append(" WHERE [SYEARMONTH]='" + strYearMonth + "' order by SSTAFFNO, convert(int,[sday]) \r\n");

            String strSql = sbSql.ToString();
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
            iRowCount = dt.Rows.Count;

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

    /// <summary>
    /// 保存排班数据
    /// </summary>
    /// <param name="strSecUserId"></param>
    /// <param name="strYearMonth"></param>
    /// <param name="strSaveShiftData"></param>
    /// <returns></returns>
    private String SaveStaffShiftData(String strSecUserId,String strYearMonth,String strSaveShiftData)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbReturnRowData = new StringBuilder();
        StringBuilder sbReturnColumnData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "保存排班数据";

        String strIsLockStaffDateAtt = Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("Switch_IsLockStaffDateAtt");
        strIsLockStaffDateAtt = String.IsNullOrEmpty(strIsLockStaffDateAtt) ? "0" : strIsLockStaffDateAtt;
        try
        {
            //获取每月的日期列表
            String strSqlKQPERD_3 = "select * from KQPERD_3 WHERE PID = '"+strYearMonth+"' order by [date]";
            DataTable dt_KQPERD_3 = SqlParamDao.GetDataTableBySql(strSqlKQPERD_3);

            if (!String.IsNullOrEmpty(strSaveShiftData))
            {
                JArray jsonArray = (JArray)JsonConvert.DeserializeObject(strSaveShiftData);
                int iCount = jsonArray.Count;
                if(iCount>0){
                    foreach (JObject itemJArray in jsonArray)
                    {
                        StringBuilder sbSql = new StringBuilder();
                        String strDCNO = itemJArray["DCNO"].ToString();
                        String strDNUM = itemJArray["DNUM"].ToString();
                        String strSHCODE = itemJArray["SHCODE"].ToString();
                        strDNUM = "D" + strDNUM;
                        String strYearMonthDCNO = strYearMonth + strDCNO;

                        //获取DNUM和YearMonth获取对应的日期
                        DataRow dr = dt_KQPERD_3.Select("DNUM='" + strDNUM + "'")[0];
                        String strDate = DateTime.Parse(dr["DATE"].ToString()).ToString("yyyy-MM-dd");


                        //获取修改前的排班班次值
                        String strOldValue = "";
                        String strSql_OldValue = "select isnull(SavedShift,'') as SavedShift from KQRSSZ_2 WHERE EM_NO = '" + strDCNO + "' AND SEQNO = '" + strYearMonthDCNO + "' AND CONVERT(varchar(20),r_Date,23) = '" + strDate + "'";
                        DataTable dt_OldValue = SqlParamDao.GetDataTableBySql(strSql_OldValue);
                        if(dt_OldValue!=null && dt_OldValue.Rows.Count==1){
                            strOldValue = dt_OldValue.Rows[0]["SavedShift"].ToString();
                        }

                        sbSql.Append("update KQTOE_2 set "+strDNUM+" = '"+strSHCODE+"' WHERE EMPLOYEE = '"+strDCNO+"' AND YEARMONTH = '"+strYearMonth+"' \r\n");
                        sbSql.Append("update KQRSSZ_2 set SavedShift = '"+strSHCODE+"' WHERE EM_NO = '"+strDCNO+"' AND SEQNO = '"+strYearMonthDCNO+"' AND CONVERT(varchar(20),r_Date,23) = '"+strDate+"' \r\n");

                        //add by sammen 排班保存后即锁定
                        try{
                            strIsLockStaffDateAtt = strIsLockStaffDateAtt.Equals("0") ? "2" : strIsLockStaffDateAtt;
                            sbSql.Append("update A set A.IsAttLocked = '"+strIsLockStaffDateAtt+"' \r\n");
                            sbSql.Append(" from KQRSSZ_2 A \r\n");
                            sbSql.Append(" WHERE A.EM_NO = '"+strDCNO+"' AND A.SEQNO = '"+strYearMonthDCNO+"' AND CONVERT(varchar(20),A.r_Date,23) = '"+strDate+"' \r\n");
                        }
                        catch (Exception ex){
                        }

                        log.Error(strMethodDesc + " Execute Sql :" + sbSql.ToString());
                        if (!string.IsNullOrEmpty(sbSql.ToString()))
                        {
                            int iUpdateCount = SqlParamDao.ExecuteNonQueryBySql(sbSql.ToString());
                        }

                        ///保存某人某天排班班次后的后续日志操作
                        this.SaveShiftRecord(strYearMonthDCNO,strDate,"SavedShift",strOldValue,strSHCODE,strSecUserId);
                    }
                    strReturnCode = "1";
                    strReturnMsg = strMethodDesc + "成功";
                }else{
                    strReturnCode = "1";
                    strReturnMsg = strMethodDesc + "失败";
                }

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
            if (!String.IsNullOrEmpty(sbReturnRowData.ToString()))
            {
                sbResult.Append(",\"ReturnRowData\"" + sbReturnRowData.ToString()+"");
            }
            sbResult.Append("}");
            //log.Error(strMethodDesc + "Return Json:" + sbResult.ToString());
        }
        return sbResult.ToString();
    }

    /// <summary>
    /// 保存某人某天排班班次后的后续日志操作
    /// </summary>
    /// <param name="strYearMonth"></param>
    /// <param name="strStartDay"></param>
    /// <param name="strEndDay"></param>
    /// <param name="strUserId"></param>
    /// <returns></returns>
    private void SaveShiftRecord(String strYearMonthDCNO, String strDate, String strPID,String strOldValue,String strNewValue, String strUserId)
    {
        String strMethodDesc = "保存某人某天排班班次后的后续日志操作";
        String strCurTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        String strBatchNo = DateTime.Now.ToString("yyyyMMddHHmmssffff")+strUserId;
        try
        {
            String strSpName = "USP_HR_KQ_WriteModifyLog";
            Hashtable hsTableParam = new Hashtable();
            hsTableParam.Add("YearMonthDCNO", strYearMonthDCNO);
            hsTableParam.Add("Date", strDate);
            hsTableParam.Add("PID", strPID);
            hsTableParam.Add("OldValue", strOldValue);
            hsTableParam.Add("NewValue", strNewValue);
            hsTableParam.Add("SUSERID", strUserId);
            //log.Error("保存某人某天排班班次后的后续日志操作：exec " + strSpName + " '" + strYearMonthDCNO + "', '" + strDate + "','" + strPID + "','" + strOldValue + "','" + strNewValue + "','" + strUserId + "'");
            int iSPReturn = SqlParamDao.ExcuteSP(strSpName, hsTableParam);

        }
        catch (Exception ex)
        {
            log.Error(strMethodDesc+ex);
        }
    }



    /// <summary>
    /// 获取某员工某天的考勤结果信息
    /// </summary>
    /// <param name="strAdminUserId"></param>
    /// <param name="strYearMonth"></param>
    /// <param name="strStaffNo"></param>
    /// <param name="strOneDay"></param>
    /// <param name="strTotalwidth"></param>
    /// <param name="strIsModify"></param>
    /// <returns></returns>
    private String GetOneDayResultData(String strAdminUserId,String strYearMonth,String strStaffNo,String strOneDay,String strTotalwidth,String strIsModify)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbReturnRowData = new StringBuilder();
        StringBuilder sbReturnColumnData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "获取某员工某天的考勤结果信息";
        int iColCount = 0;
        int iColCount_ListShow = 0;
        int iRowCount = 0;

        try
        {
            sbReturnColumnData.Append("[");
            //获取配置表中需要显示的固定字段
            String strSqlConfig_4 = "SELECT * FROM KQASConfig_4 WHERE ConfigCode in (select top 1 ConfigCode from KQASConfig_1 order by ConfigCode) order by SORDER";
            DataTable dt_Config_4 = SqlParamDao.GetDataTableBySql(strSqlConfig_4);

            //获取只在列表中显示的字段记录集
            DataView dv_ListShow = new DataView();
            dv_ListShow.Table = dt_Config_4;
            dv_ListShow.RowFilter = "PLIST = '1'";
            DataTable dt_Config_4_ListShow = dv_ListShow.ToTable();

            StringBuilder sbStaffListFixedColumns = new StringBuilder();
            if(dt_Config_4!=null && dt_Config_4.Rows.Count>0){
                iColCount = dt_Config_4.Rows.Count;
                iColCount_ListShow = dt_Config_4_ListShow.Rows.Count;


                //全表格的宽度
                int iTableWidth = Convert.ToInt32(double.Parse(strTotalwidth)-20);
                int iRemarkWidth = 350;
                //每一列的宽度
                double iColWidth = Math.Round((double)((iTableWidth-iRemarkWidth) / (iColCount_ListShow-1)),2);

                for(int i=0;i<iColCount;i++){
                    String strTableName_Config_4 = dt_Config_4.Rows[i]["TableName"].ToString();
                    String strColumnName_Config_4 = dt_Config_4.Rows[i]["ColumnName"].ToString();
                    String strColumnDesc_Config_4 = dt_Config_4.Rows[i]["ColumnDesc"].ToString();
                    String strPLIST_Config_4 = dt_Config_4.Rows[i]["PLIST"].ToString();
                    String strPEDIT_Config_4 = (!dt_Config_4.Columns.Contains("PEDIT"))?"2":dt_Config_4.Rows[i]["PEDIT"].ToString();


                    String strTableColumnName = strTableName_Config_4 + "." + strColumnName_Config_4;
                    sbStaffListFixedColumns.Append(i==0?"":",");
                    sbStaffListFixedColumns.Append(strTableColumnName +" as ["+strColumnDesc_Config_4+"]\r\n");

                    switch (strColumnName_Config_4.ToUpper()) {
                        case "BISTONORMAL":
                            sbReturnColumnData.Append("{\"type\": \"dropdown\", \"title\": \""+strColumnDesc_Config_4+"\", \"width\": "+iColWidth.ToString()+"");
                            sbReturnColumnData.Append(" , \"tableName\": \""+strTableName_Config_4+"\"");
                            sbReturnColumnData.Append(" , \"colName\": \""+strColumnName_Config_4+"\"");
                            sbReturnColumnData.Append(" , \"isListShow\": \""+strPLIST_Config_4+"\"");
                            if (strIsModify.Equals("1") && strPEDIT_Config_4.Equals("1"))
                            {
                                sbReturnColumnData.Append(" , \"readOnly\": false");
                            }
                            else { 
                                sbReturnColumnData.Append(" , \"readOnly\": true");
                            }
                            sbReturnColumnData.Append(" ,\"source\":[");
                            sbReturnColumnData.Append(" {\"id\":\"0\",\"name\":\"考勤正常\"}");
                            sbReturnColumnData.Append(" ,{\"id\":\"1\",\"name\":\"异常调整为正常\"}");
                            sbReturnColumnData.Append(" ,{\"id\":\"2\",\"name\":\"考勤异常\"}");
                            sbReturnColumnData.Append(" ]},");
                            break;
                        case "SREMARK":
                            sbReturnColumnData.Append("{\"type\": \"text\", \"title\": \""+strColumnDesc_Config_4+"\", \"width\": "+iRemarkWidth.ToString()+"");
                            sbReturnColumnData.Append(" , \"tableName\": \""+strTableName_Config_4+"\"");
                            sbReturnColumnData.Append(" , \"colName\": \""+strColumnName_Config_4+"\"");
                            sbReturnColumnData.Append(" , \"isListShow\": \""+strPLIST_Config_4+"\"");
                            sbReturnColumnData.Append(" , \"richText\": \"true\"");
                            if (strIsModify.Equals("1") && strPEDIT_Config_4.Equals("1"))
                            {
                                sbReturnColumnData.Append(" , \"readOnly\": false");
                            }
                            else { 
                                sbReturnColumnData.Append(" , \"readOnly\": true");
                            }
                            sbReturnColumnData.Append(" },");
                            break;
                        default:
                            sbReturnColumnData.Append("{\"type\": \"text\", \"title\": \""+strColumnDesc_Config_4+"\", \"width\": "+iColWidth.ToString()+"");
                            sbReturnColumnData.Append(" , \"tableName\": \""+strTableName_Config_4+"\"");
                            sbReturnColumnData.Append(" , \"colName\": \""+strColumnName_Config_4+"\"");
                            sbReturnColumnData.Append(" , \"isListShow\": \""+strPLIST_Config_4+"\"");
                            if (strIsModify.Equals("1") && strPEDIT_Config_4.Equals("1"))
                            {
                                sbReturnColumnData.Append(" , \"readOnly\": false");
                            }
                            else { 
                                sbReturnColumnData.Append(" , \"readOnly\": true");
                            }
                            sbReturnColumnData.Append(" },");
                            break;
                    }
                }
            }

            sbReturnColumnData.Append("]");

            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("SELECT "+sbStaffListFixedColumns.ToString());
            sbSql.Append(" FROM KQRSSZ_2 KQRSSZ_2 \r\n");
            sbSql.Append(" INNER JOIN KQRSSZ_1 KQRSSZ_1 ON KQRSSZ_2.SEQNO = KQRSSZ_1.SEQNO \r\n");
            sbSql.Append(" WHERE KQRSSZ_2.SEQNO = '"+strYearMonth+strStaffNo+"' AND convert(varchar(20),KQRSSZ_2.r_Date,23) = '"+strOneDay+"'\r\n");
            sbSql.Append(" ORDER BY KQRSSZ_2.SEQNO,convert(varchar(20),KQRSSZ_2.r_Date,23)\r\n");

            String strSql = sbSql.ToString();
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
            iRowCount = dt.Rows.Count;

            sbReturnRowData.Append(WebCommon.GetJsonStringByDataTable(dt,"",true));

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
            sbResultStatus.Append(",\"ReturnColCountListShow\":\"" + iColCount_ListShow.ToString() + "\"");
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
    /// 修改结果数据
    /// </summary>
    /// <param name="strSecUserId"></param>
    /// <param name="strYearMonth"></param>
    /// <param name="strSaveShiftData"></param>
    /// <returns></returns>
    private String SaveStaffResultData(String strSecUserId,String strYearMonth,String strStaffNo,String strOneDay,String strColName,String strColValue)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbReturnRowData = new StringBuilder();
        StringBuilder sbReturnColumnData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        StringBuilder sbSql = new StringBuilder();
        String strMethodDesc = "修改结果数据";

        try
        {
            String strYearMonthDCNO = strYearMonth + strStaffNo;
            strColValue = strColValue.Replace("'", "''");

            sbSql.Append("update KQRSSZ_2 set "+strColName+" = '"+strColValue+"' WHERE EM_NO = '"+strStaffNo+"' AND SEQNO = '"+strYearMonthDCNO+"' AND CONVERT(varchar(20),r_Date,23) = '"+strOneDay+"' \r\n");
            //log.Error(strMethodDesc + " Execute Sql :" + sbSql.ToString());
            int iUpdateCount = SqlParamDao.ExecuteNonQueryBySql(sbSql.ToString());

            if(iUpdateCount>0){
                ///保存某人某天排班考勤结果修改后的后续日志操作
                this.SaveShiftRecord(strYearMonthDCNO,strOneDay,strColName,"",strColValue,strSecUserId);
            }

            strReturnCode = "1";
            strReturnMsg = strMethodDesc + "成功";
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = strMethodDesc + "出错，Sql语句为："+sbSql.ToString();
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