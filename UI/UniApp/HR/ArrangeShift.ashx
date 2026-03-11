<%@ WebHandler Language="C#" Class="ArrangeShift" %>

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

public class ArrangeShift : HandlerBase,IHttpHandler, System.Web.SessionState.IRequiresSessionState
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
        string strSaveShiftData = WebCommon.GetJsonObjectValue(strParamJson,"saveshiftdata").ToString();//需保存的排班信息数据

        String strPageSize = WebCommon.GetJsonValue(strParamJson,"pagesize").ToString();
        String strPageIndex = WebCommon.GetJsonValue(strParamJson,"pageindex").ToString();
        int iPageSize = String.IsNullOrEmpty(strPageSize)? 10 : int.Parse(strPageSize);
        int iPageIndex = String.IsNullOrEmpty(strPageSize)? 0 : int.Parse(strPageIndex);

        String strRequestLanguage = strCulture.Equals("0") ? "zh-cn" : "en-us";
        switch (param.ToLower().ToString())
        {
            case "getshiftdatabyyearmonthdcno"://根据员工及周期获取排班及考勤数据
                this.GetShiftDataByYearMonthDcno(context,strDCNO,strYearMonth,strUserCode);
                break;
            case "getshiftcodelistbyusercode"://根据账号获取可进行排班的班次列表
                this.GetShiftCodeListByUserCode(context,strYearMonth,strUserCode);
                break;
            case "saveonestaffonedayshiftcode"://修改某人某天的班次
                this.SaveOneStaffOneDayShiftCode(context,strYearMonth,strDCNO,strSaveShiftData,strUserCode);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 根据员工及周期获取排班及考勤数据
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strDCNO"></param>
    /// <param name="strYearMonth"></param>
    /// <param name="strUserCode"></param>
    public void GetShiftDataByYearMonthDcno(HttpContext context,String strDCNO,String strYearMonth,String strUserCode)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        String strMethodDesc = "根据员工及周期获取排班及考勤数据";
        StringBuilder sbResult = new StringBuilder();

        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("SELECT A.PID,CONVERT(varchar(20),A.DATE,23) AS [DATE],replace(A.DNUM,'D','') AS DNUM ");
            sbSql.Append(",right(CONVERT(varchar(20),A.DATE,23),2) AS Day,(datepart(dw,A.DATE)-1) as Week,A.MonthlyWeekNo");
            sbSql.Append(",(select MAX(MonthlyWeekNo) from KQPERD_3 where PID = A.PID ) AS WeeksCount");
            sbSql.Append(",B.*");
            sbSql.Append(",C.EM_NO,C.DCNAMEC,C.DCNAMEE,C.DCDDESCCHS");
            sbSql.Append(" FROM KQPERD_3 A LEFT JOIN (SELECT * FROM KQRSSZ_2 WHERE EM_NO = '"+strDCNO+"') B ");
            sbSql.Append(" ON A.PID = LEFT(B.SEQNO,4) AND CONVERT(varchar(20),A.DATE,23) = CONVERT(varchar(20),B.r_Date,23)");
            sbSql.Append(" INNER JOIN (SELECT * FROM KQRSSZ_1 WHERE EM_NO = '"+strDCNO+"' AND Ymonth = '"+strYearMonth+"') C ON B.SEQNO = C.SEQNO ");
            sbSql.Append(" WHERE A.PID = '"+strYearMonth+"'");
            sbSql.Append(" ORDER BY A.PID,A.DATE");
            String strSql = sbSql.ToString();

            //log.Error(strMethodDesc+":"+strSql);

            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);

            strReturnCode = "1";
            strReturnMsg = strMethodDesc+"成功";
            sbResultData.Append(WebCommon.GetJsonStringByDataTable(dt, "\"ArrangeShiftData\"", false));
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
    /// 根据账号获取可进行排班的班次列表
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strUserCode"></param>
    public void GetShiftCodeListByUserCode(HttpContext context,String strYearMonth,String strUserCode)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        String strMethodDesc = "根据账号获取可进行排班的班次列表";
        StringBuilder sbResult = new StringBuilder();

        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("SELECT A.SHCODE,A.SHNAME,A.IN1,A.OUT1,A.IN2,A.OUT2 ");
            sbSql.Append(" FROM KQSHIF_1 A INNER JOIN VW_PAIBAN_SHIFT_FILTER vwName ON A.SHCODE = vwName.[SHIFTCODE] ");
            sbSql.Append(" where vwName.[SUSERID] = '" + strUserCode + "'");
            sbSql.Append(" order by A.SHCODE");
            String strSql = sbSql.ToString();

            //log.Error(strMethodDesc+":"+strSql);

            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);

            strReturnCode = "1";
            strReturnMsg = strMethodDesc+"成功";
            sbResultData.Append(WebCommon.GetJsonStringByDataTable(dt, "\"ShiftCodeList\"", false));
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
    /// 修改某人某天的班次
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strYearMonth"></param>
    /// <param name="strDCNO"></param>
    /// <param name="strOneDate"></param>
    /// <param name="strShiftCode"></param>
    /// <param name="strUserCode"></param>
    public void SaveOneStaffOneDayShiftCode(HttpContext context,String strYearMonth,String strDCNO,String strSaveShiftData,String strUserCode)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        String strMethodDesc = "修改某人某天的班次";
        StringBuilder sbResult = new StringBuilder();

        //log.Error(strMethodDesc+"--strSaveShiftData:"+strSaveShiftData);

        String strIsLockStaffDateAtt = Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("Switch_IsLockStaffDateAtt");
        strIsLockStaffDateAtt = String.IsNullOrEmpty(strIsLockStaffDateAtt) ? "0" : strIsLockStaffDateAtt;
        try
        {
            //获取每月的日期列表
            String strSqlKQPERD_3 = "select * from KQPERD_3 WHERE PID = '"+strYearMonth+"' order by [date]";
            DataTable dt_KQPERD_3 = SqlParamDao.GetDataTableBySql(strSqlKQPERD_3);

            if (!String.IsNullOrEmpty(strSaveShiftData))
            {
                JObject itemJArray = (JObject)JsonConvert.DeserializeObject(strSaveShiftData);
                StringBuilder sbSql = new StringBuilder();
                String strDNUM = itemJArray["DNUM"].ToString();
                String strSHCODE = itemJArray["SavedShift"].ToString();
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
                sbSql.Append("update KQRSSZ_2 set SavedShift = '"+strSHCODE+"',shift = '"+strSHCODE+"' ");
                sbSql.Append(" WHERE EM_NO = '"+strDCNO+"' AND SEQNO = '"+strYearMonthDCNO+"' AND CONVERT(varchar(20),r_Date,23) = '"+strDate+"' \r\n");

                //add by sammen 排班保存后即锁定
                try{
                    strIsLockStaffDateAtt = strIsLockStaffDateAtt.Equals("0") ? "2" : strIsLockStaffDateAtt;
                    sbSql.Append("update A set A.IsAttLocked = '"+strIsLockStaffDateAtt+"' \r\n");
                    sbSql.Append(" from KQRSSZ_2 A \r\n");
                    sbSql.Append(" WHERE A.EM_NO = '"+strDCNO+"' AND A.SEQNO = '"+strYearMonthDCNO+"' AND CONVERT(varchar(20),A.r_Date,23) = '"+strDate+"' \r\n");
                }
                catch (Exception ex){
                }

                //log.Error(strMethodDesc + " Execute Sql :" + sbSql.ToString());
                if (!string.IsNullOrEmpty(sbSql.ToString()))
                {
                    int iUpdateCount = SqlParamDao.ExecuteNonQueryBySql(sbSql.ToString());
                }

                ///保存某人某天排班班次后的后续日志操作
                this.SaveShiftRecord(strYearMonthDCNO,strDate,"SavedShift",strOldValue,strSHCODE,strUserCode);
            }

            strReturnCode = "1";
            strReturnMsg = strMethodDesc+"成功";
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
            log.Error(ex);
        }
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}