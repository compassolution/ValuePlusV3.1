<%@ WebHandler Language="C#" Class="EmplyeeHandbook" %>

using System;
using System.Web;
using System.Text;
using System.Data;
using System.IO;
using System.Collections;
using Com.ValuePlus.Web;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Weixin;
using Com.ValuePlus.Utils.Serializable;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

public class EmplyeeHandbook : HandlerBase, IHttpHandler, System.Web.SessionState.IRequiresSessionState {

    public void ProcessRequest (HttpContext context) {
        //解析客户端传递过来的json data
        StreamReader reader = new StreamReader(context.Request.InputStream);
        String strParamJson = HttpUtility.UrlDecode(reader.ReadToEnd());
        log.Error("传入参数的Json data字符串:" + strParamJson.ToString());

        string strParam = WebCommon.GetJsonValue(strParamJson,"param").ToString();//请求类型参数
        string strLanguage = WebCommon.GetJsonValue(strParamJson,"language").ToString();
        string strMobileNo = WebCommon.GetJsonValue(strParamJson,"mobileno").ToString();
        string strProjectId = WebCommon.GetJsonValue(strParamJson,"projectid").ToString();
        string strYearMonthType = WebCommon.GetJsonValue(strParamJson,"yearmonthtype").ToString();
        string strYearMonth = WebCommon.GetJsonValue(strParamJson,"yearmonth").ToString();

        log.Error("EmplyeeHandbook.ashx,MobileNo:"+strMobileNo+";Language:"+strLanguage);
        if (strParam.Equals("getondutyprojectid"))
        {
            context.Response.Write(this.GetOndutyProjectId(context, strMobileNo, strLanguage));
        }
        else if (strParam.Equals("getpreviewconfirmsecond"))
        {
            //获取预览确认提醒时间间隔
            context.Response.Write(this.GetPreviewConfirmSecond(context, strProjectId));
        }
        else if (strParam.Equals("judgeisconfimrhandbook"))
        {
            //判断是否已确认员工手册
            context.Response.Write(this.JudgeIsConfimrHandbook(context, strMobileNo, strProjectId));
        }
        else if (strParam.Equals("confirmemployeehandbook"))
        {
            //确认员工手册已读
            context.Response.Write(this.ConfirmEmployeeHandbook(context, strMobileNo, strProjectId));
        }
    }
    
    /// <summary>
    /// 获取当前在职单位的ProjectId
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strMobileNo"></param>
    /// <param name="strLanguage"></param>
    private String GetOndutyProjectId(HttpContext context,String strMobileNo,String strLanguage)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "获取当前在职单位的ProjectId";
        try
        {
            log.Error("获取当前在职单位的ProjectId,MobileNo:"+strMobileNo+";Language:"+strLanguage);
            //String strUrlHandbook = this.GetRemoteServer() + "/UserFile/EmployeeHandbook/";
            String strUrlHandbook = "/UserFile/EmployeeHandbook/";
            //String strUrlHandbook = "/static/EmployeeHandbook/";//无法跨域时，只能放在本应用目录下
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select TOP 1 ProjectId ");
            if (strLanguage.Equals("0"))
            {
                sbSql.Append(",ProjectNameChs as ProjectName");
            }else
            {
                sbSql.Append(",ProjectName");
            }
            sbSql.Append(",'"+strUrlHandbook+"'+ProjectId+'.pdf' as UrlHandbook");
            sbSql.Append(",'"+this.GetRemoteServer()+"' as PdfFileServer");
            sbSql.Append(" from [TB_Remote_StaffBasicData] where DCMOBILE = '"+strMobileNo+"' and ISNULL(DCSTATUS,'') = '1' order by DCJOIN DESC");
            String strSql = sbSql.ToString();
            log.Error(strMethodDesc+"SQL:"+strSql);

            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
            sbResultData.Append(WebCommon.GetJsonStringByDataTable(dt,"\"ResultData\"",true));

            strReturnCode = "1";
            strReturnMsg = strMethodDesc+"成功";
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = strMethodDesc+"出错";
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
                sbResult.Append(","+sbResultData.ToString());
            }
            sbResult.Append("}");
            log.Error(strMethodDesc+"Return Json:"+sbResult.ToString());
        }
        return sbResult.ToString();
    }

    /// <summary>
    /// 获取预览确认提醒时间间隔
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strProjectId"></param>
    private String GetPreviewConfirmSecond(HttpContext context,String strProjectId)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "获取预览确认提醒时间间隔,strProjectId:"+strProjectId;
        try
        {
            log.Error(strMethodDesc);
            int iSecond = 20;
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select ISNULL(ConfirmSecond060,20) AS ConfirmSecond060 from WXProjectAuth_1 where 1=1 and ProjectId = '"+strProjectId+"'");
            String strSql = sbSql.ToString();
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
            if (dt!=null && dt.Rows.Count==1){
                iSecond = int.Parse(dt.Rows[0]["ConfirmSecond060"].ToString());
            }
            sbResultData.Append(" \"ResultData\":"+"\""+iSecond.ToString()+"\" ");

            strReturnCode = "1";
            strReturnMsg = strMethodDesc+"成功";
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = strMethodDesc+"出错";
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
                sbResult.Append(","+sbResultData.ToString());
            }
            sbResult.Append("}");
            log.Error(strMethodDesc+"Return Json:"+sbResult.ToString());
        }
        return sbResult.ToString();
    }

    /// <summary>
    /// 判断是否已确认员工手册
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strMobileNo"></param>
    /// <param name="strProjectId"></param>
    private String JudgeIsConfimrHandbook(HttpContext context,String strMobileNo,String strProjectId)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "判断是否已确认员工手册,MobileNo:"+strMobileNo;
        try
        {
            log.Error("判断是否已确认员工手册,MobileNo:"+strMobileNo+";strProjectId:"+strProjectId);
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select top 1 * from TB_Remote_HandBookConfirmData where ProjectId = '"+strProjectId+"' AND DCMOBILE = '"+strMobileNo+"' ");
            String strSql = sbSql.ToString();
            log.Error(strMethodDesc+"SQL:"+strSql);

            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
            sbResultData.Append(WebCommon.GetJsonStringByDataTable(dt,"\"ResultData\"",true));

            strReturnCode = "1";
            strReturnMsg = strMethodDesc+"成功";
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = strMethodDesc+"出错";
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
                sbResult.Append(","+sbResultData.ToString());
            }
            sbResult.Append("}");
            log.Error(strMethodDesc+"Return Json:"+sbResult.ToString());
        }
        return sbResult.ToString();
    }

    /// <summary>
    /// 确认员工手册已读
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strMobileNo"></param>
    /// <param name="strProjectId"></param>
    private String ConfirmEmployeeHandbook(HttpContext context,String strMobileNo,String strProjectId)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "确认员工手册已读,MobileNo:"+strMobileNo;
        try
        {
            log.Error("确认员工手册已读,MobileNo:"+strMobileNo+";strProjectId:"+strProjectId);
            String strSQL_ExecuteSP = "USP_Remote_Handbook_EmployeeConfirm";
            Hashtable hsTableParam = new Hashtable();
            hsTableParam.Add("ProjectId", strProjectId);
            hsTableParam.Add("MobileNo", strMobileNo);
            hsTableParam.Add("UserId", this.GetUserCode());
            int iReturnValue = SqlParamDao.ExcuteSP(strSQL_ExecuteSP, hsTableParam);

            strReturnCode = "1";
            strReturnMsg = strMethodDesc+"成功";
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = strMethodDesc+"出错";
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
                sbResult.Append(","+sbResultData.ToString());
            }
            sbResult.Append("}");
            log.Error(strMethodDesc+"Return Json:"+sbResult.ToString());
        }
        return sbResult.ToString();
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}