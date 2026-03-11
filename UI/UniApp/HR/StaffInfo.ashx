<%@ WebHandler Language="C#" Class="StaffInfo" %>

using System;
using System.Web;
using System.Collections;
using System.Text;
using System.IO;
using System.Data;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Web;
using Com.ValuePlus.Archive.BLL;

public class StaffInfo : HandlerBase,IHttpHandler, System.Web.SessionState.IRequiresSessionState
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
        string strDCNO = WebCommon.GetJsonValue(strParamJson,"dcno").ToString();
        string strYearMonth = WebCommon.GetJsonValue(strParamJson,"yearmonth").ToString();

        String strPageSize = WebCommon.GetJsonValue(strParamJson,"pagesize").ToString();
        String strPageIndex = WebCommon.GetJsonValue(strParamJson,"pageindex").ToString();
        int iPageSize = String.IsNullOrEmpty(strPageSize)? 10 : int.Parse(strPageSize);
        int iPageIndex = String.IsNullOrEmpty(strPageSize)? 0 : int.Parse(strPageIndex);

        String strRequestLanguage = strCulture.Equals("0") ? "zh-cn" : "en-us";
        switch (param.ToLower().ToString())
        {
            case "getonestaffbasicinfo"://获取某员工基本信息
                this.GetOneStaffBasicInfo(context,strDCNO,strUserCode);
                break;
            case "getinchargestafflistbyusercode"://根据账号获取管辖员工列表
                this.GetInchargeStaffListByUserCode(context,strYearMonth,strUserCode);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 获取某员工基本信息
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strYearMonth"></param>
    /// <param name="strUserCode"></param>
    public void GetOneStaffBasicInfo(HttpContext context,String strDCNO,String strUserCode)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        String strMethodDesc = "获取某员工基本信息";
        StringBuilder sbResult = new StringBuilder();

        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("SELECT * FROM VW_HRDOCU_DEPARTMENT A WHERE 1=1 ");
            if(!String.IsNullOrEmpty(strDCNO)){
                sbSql.Append(" AND A.DCNO = '"+strDCNO+"'");
            }
            sbSql.Append(" ORDER BY A.DEPTCODE1,A.DEPTCODE2,A.DEPTCODE3,A.DCNO");
            String strSql = sbSql.ToString();

            //log.Error(strMethodDesc+":"+strSql);

            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);

            strReturnCode = "1";
            strReturnMsg = strMethodDesc+"成功";
            sbResultData.Append(WebCommon.GetJsonStringByDataTable(dt, "\"StaffInfoData\"", false));
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
    /// 根据账号获取管辖员工列表
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strUserCode"></param>
    public void GetInchargeStaffListByUserCode(HttpContext context,String strYearMonth,String strUserCode)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        String strMethodDesc = "根据账号获取管辖员工列表";
        StringBuilder sbResult = new StringBuilder();

        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select A.* from VW_HRDOCU_DEPARTMENT A INNER JOIN KQDL_2 B ON A.DEPTCODE = B.SEPNO WHERE B.DCNO = '"+strUserCode+"'");
            sbSql.Append(" AND A.DCNO IN (select DCNO from TB_HR_MonthStaffList where YEARMONTH = '" + strYearMonth + "')");
            sbSql.Append(" ORDER BY A.DEPTCODE1,A.DEPTCODE2,A.DEPTCODE3,A.DCNO");
            String strSql = sbSql.ToString();

            //log.Error(strMethodDesc+":"+strSql);

            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);

            strReturnCode = "1";
            strReturnMsg = strMethodDesc+"成功";
            sbResultData.Append(WebCommon.GetJsonStringByDataTable(dt, "\"InchargeStaffList\"", false));
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