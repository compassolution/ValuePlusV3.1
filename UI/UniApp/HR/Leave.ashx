<%@ WebHandler Language="C#" Class="Leave" %>

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

public class Leave : HandlerBase,IHttpHandler, System.Web.SessionState.IRequiresSessionState
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
        string strOneDate = WebCommon.GetJsonValue(strParamJson,"ondate").ToString();
        string strDateFrom = WebCommon.GetJsonValue(strParamJson,"datefrom").ToString();
        string strDateTo = WebCommon.GetJsonValue(strParamJson,"dateto").ToString();
        string strLVType = WebCommon.GetJsonValue(strParamJson,"lvtype").ToString();//休假类型
        string strLVStatus = WebCommon.GetJsonValue(strParamJson,"lvstatus").ToString();//休假单状态
        string strIsLVWithSalary = WebCommon.GetJsonValue(strParamJson,"islvwithsalary").ToString();//休假单是否带薪状态
        string strSaveShiftData = WebCommon.GetJsonObjectValue(strParamJson,"saveshiftdata").ToString();//需保存的排班信息数据

        String strPageSize = WebCommon.GetJsonValue(strParamJson,"pagesize").ToString();
        String strPageIndex = WebCommon.GetJsonValue(strParamJson,"pageindex").ToString();
        int iPageSize = String.IsNullOrEmpty(strPageSize)? 10 : int.Parse(strPageSize);
        int iPageIndex = String.IsNullOrEmpty(strPageSize)? 0 : int.Parse(strPageIndex);

        String strRequestLanguage = strCulture.Equals("0") ? "zh-cn" : "en-us";
        switch (param.ToLower().ToString())
        {
            case "getleavetypelist"://获取休假类型
                this.GetLeaveTypeList(context,strUserCode);
                break;
            case "getleavelist"://获取休假单列表
                this.GetLeaveList(context,strDCNO,strDateFrom,strDateTo,strLVType,strLVStatus,strIsLVWithSalary,strUserCode);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 获取休假类型
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strUserCode"></param>
    public void GetLeaveTypeList(HttpContext context,String strUserCode)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        String strMethodDesc = "获取休假类型";
        StringBuilder sbResult = new StringBuilder();

        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select * from LVTYPE_1 WHERE BISSTOP <> '1' ORDER BY TYPENO ");
            String strSql = sbSql.ToString();

            //log.Error(strMethodDesc+":"+strSql);

            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);

            strReturnCode = "1";
            strReturnMsg = strMethodDesc+"成功";
            sbResultData.Append(WebCommon.GetJsonStringByDataTable(dt, "\"LVTypeInfo\"", false));
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
    /// 获取休假单列表
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strDCNO"></param>
    /// <param name="strDateFrom"></param>
    /// <param name="strDateTo"></param>
    /// <param name="strLVType"></param>
    /// <param name="strLVStatus"></param>
    /// <param name="strIsLVWithSalary"></param>
    /// <param name="strUserCode"></param>
    public void GetLeaveList(HttpContext context,String strDCNO,String strDateFrom,String strDateTo,String strLVType,String strLVStatus,String strIsLVWithSalary,String strUserCode)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        String strMethodDesc = "获取休假单列表";
        StringBuilder sbResult = new StringBuilder();

        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("SELECT * from [VW_MB_KQLV] where 1=1 ");
            if(!String.IsNullOrEmpty(strDCNO)){
                sbSql.Append(" and StaffNo = '"+strDCNO+"'");
            }
            if(!String.IsNullOrEmpty(strDateFrom)){
                sbSql.Append(" and LEFT(LVDateFrom,10) >= '"+strDateFrom+"'");
            }
            if(!String.IsNullOrEmpty(strDateTo)){
                sbSql.Append(" and LEFT(LVDateTo,10) <= '"+strDateTo+"'");
            }
            if(!String.IsNullOrEmpty(strLVType)){
                sbSql.Append(" and LVTYPE = '"+strLVType+"'");
            }
            if(!String.IsNullOrEmpty(strLVStatus)){
                sbSql.Append(" and LVStatus = '"+strLVStatus+"'");
            }
            if(!String.IsNullOrEmpty(strIsLVWithSalary)){
                sbSql.Append(" and ISSALARY = '"+strIsLVWithSalary+"'");
            }
            String strSql = sbSql.ToString();

            //log.Error(strMethodDesc+":"+strSql);

            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);

            strReturnCode = "1";
            strReturnMsg = strMethodDesc+"成功";
            sbResultData.Append(WebCommon.GetJsonStringByDataTable(dt, "\"LVRecordInfo\"", false));
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