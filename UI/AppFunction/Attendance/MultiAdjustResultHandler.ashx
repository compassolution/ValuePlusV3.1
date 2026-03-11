<%@ WebHandler Language="C#" Class="MultiAdjustResultHandler" %>

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
/// 批量修改考勤结果的操作类
/// </summary>
public class MultiAdjustResultHandler : HandlerBase, IHttpHandler, System.Web.SessionState.IRequiresSessionState {

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
        string strSecUserId = hsTableUrlQuery["sectionuserid"] == null ? string.Empty : hsTableUrlQuery["sectionuserid"].ToString();////排班操作用户ID
        string strYearMonth = hsTableUrlQuery["yearmonth"] == null ? string.Empty : hsTableUrlQuery["yearmonth"].ToString();//考勤周期期间
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

        if (String.IsNullOrEmpty(strSecUserId)){
            strSecUserId = this.GetUserCode();
        }

        switch (strParam.ToLower().ToString())
        {
            case "getexceptionresultdatalist":
                context.Response.Write(this.GetExceptionResultDataList(strSecUserId,strYearMonth,strTotalwidth).ToString());
                break;
        }
    }

    /// <summary>
    /// 获取某个月某个考勤员管辖下处于异常或者调整为正常的考勤结果列表
    /// </summary>
    /// <param name="strSecUserId"></param>
    /// <param name="strYearMonth"></param>
    /// <param name="strTotalwidth"></param>
    /// <returns></returns>
    private String GetExceptionResultDataList(String strSecUserId,String strYearMonth,String strTotalwidth)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbReturnRowData = new StringBuilder();
        StringBuilder sbReturnColumnData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "获取某个月某个考勤员管辖下处于异常或者调整为正常的考勤结果列表";
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
                int iRemarkWidth = 150;
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
                            sbReturnColumnData.Append("{\"type\": \"dropdown\", \"title\": \""+strColumnDesc_Config_4+"\", \"width\": \""+iColWidth.ToString()+"\"");
                            sbReturnColumnData.Append(" , \"tableName\": \""+strTableName_Config_4+"\"");
                            sbReturnColumnData.Append(" , \"colName\": \""+strColumnName_Config_4+"\"");
                            sbReturnColumnData.Append(" , \"isListShow\": \""+strPLIST_Config_4+"\"");
                            sbReturnColumnData.Append(" , \"readOnly\": false");
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
                            sbReturnColumnData.Append(" , \"readOnly\": false");
                            sbReturnColumnData.Append(" },");
                            break;
                        default:
                            double iTempColWidth = iColWidth;
                            switch (strColumnName_Config_4.ToUpper()) {
                                case "EM_NO":
                                case "SAVEDSHIFT":
                                    iTempColWidth = iTempColWidth / 2;
                                    break;
                                default:
                                    break;
                            }
                            sbReturnColumnData.Append("{\"type\": \"text\", \"title\": \""+strColumnDesc_Config_4+"\", \"width\": "+iTempColWidth.ToString()+"");
                            sbReturnColumnData.Append(" , \"tableName\": \""+strTableName_Config_4+"\"");
                            sbReturnColumnData.Append(" , \"colName\": \""+strColumnName_Config_4+"\"");
                            sbReturnColumnData.Append(" , \"isListShow\": \""+strPLIST_Config_4+"\"");
                            if (strPEDIT_Config_4.Equals("1"))
                            {
                                sbReturnColumnData.Append(" , \"readOnly\": false");
                            }else
                            {
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
            sbSql.Append(" FROM KQRSSZ_2 KQRSSZ_2 INNER JOIN FUN_VW_PAIBAN_STAFF_FILTER_BYUSERID('" + strYearMonth + "','" + strSecUserId + "') B  \r\n");
            sbSql.Append(" ON KQRSSZ_2.SEQNO = '"+strYearMonth+"'+B.STAFFID  \r\n");
            sbSql.Append(" INNER JOIN KQRSSZ_1 KQRSSZ_1 ON KQRSSZ_2.SEQNO = KQRSSZ_1.SEQNO \r\n");
            sbSql.Append(" where ISNULL(KQRSSZ_2.BISTONORMAL,'0') <> '0' \r\n");
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

    public bool IsReusable {
        get {
            return false;
        }
    }

}