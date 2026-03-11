<%@ WebHandler Language="C#" Class="MultiApprove" %>
using System;
using System.Web;
using System.Text;
using System.IO;
using System.Data;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Web;
using System.Collections;
using Com.ValuePlus.Archive.BLL;
using Com.ValuePlus.Archive.Flow;

public class MultiApprove : HandlerBase,IHttpHandler, System.Web.SessionState.IRequiresSessionState
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

        Hashtable hsTableUrlQuery = this.GetUrlAnalyse(context);
        string strParam1 = hsTableUrlQuery["param"] == null ? string.Empty : hsTableUrlQuery["param"].ToString();//param
        string param = hsTableUrlQuery["param"] == null ? string.Empty : hsTableUrlQuery["param"].ToString();//请求类型参数
        string strTID = hsTableUrlQuery["tid"] == null ? string.Empty : hsTableUrlQuery["tid"].ToString();//
        string strRID = hsTableUrlQuery["rid"] == null ? string.Empty : hsTableUrlQuery["rid"].ToString();//
        string strSID = hsTableUrlQuery["sid"] == null ? string.Empty : hsTableUrlQuery["sid"].ToString();//
        string strAID = hsTableUrlQuery["aid"] == null ? string.Empty : hsTableUrlQuery["aid"].ToString();//
        string strKeyValue = hsTableUrlQuery["keyvalue"] == null ? string.Empty : hsTableUrlQuery["keyvalue"].ToString();//
        string strOpFlag = hsTableUrlQuery["opflag"] == null ? string.Empty : hsTableUrlQuery["opflag"].ToString();//agree/reject
        string strUserCode = hsTableUrlQuery["usercode"] == null ? string.Empty : hsTableUrlQuery["usercode"].ToString();//


        string strDicLid = WebCommon.GetJsonValue(strParamJson,"lid").ToString();
        string strCulture = WebCommon.GetJsonValue(strParamJson,"culture").ToString();//文化编码[0中文1英文]
        string strProjectId = WebCommon.GetJsonValue(strParamJson,"projectid").ToString();
        string strWXMPModule = WebCommon.GetJsonValue(strParamJson,"wxmpmodule").ToString();
        string strSql = WebCommon.GetJsonObjectValue(strParamJson,"sql").ToString();
        //log.Error("传入参数的strSql.1:" + strSql.ToString());
        strSql = Microsoft.JScript.GlobalObject.decodeURIComponent(strSql);

        string strDicIsIncludeInvalid = WebCommon.GetJsonValue(strParamJson,"isincludeinvalid").ToString();//是否包含已停用的
        string strIsEscape = WebCommon.GetJsonValue(strParamJson,"isescape").ToString();//是否
        strDicIsIncludeInvalid = String.IsNullOrEmpty(strDicIsIncludeInvalid) ? "0" : "1";
        Boolean isIncludeInvalid = strDicIsIncludeInvalid.Equals("1") ? true : false;
        strIsEscape = String.IsNullOrEmpty(strIsEscape) ? "0" : "1";
        Boolean isEscape = strIsEscape.Equals("1") ? true : false;

        String strRequestLanguage = strCulture.Equals("0") ? "zh-cn" : "en-us";
        switch (param.ToLower().ToString())
        {
            case "getarchivedesc"://获取模版相应描述
                this.GetArchiveDesc(context,strTID,strRID,strSID,strUserCode);
                break;
            case "getpendingapprovelist"://获取待办处理的审批列表
                this.GetPendingApproveList(context,strTID,strRID,strSID,strAID,strKeyValue,strUserCode);
                break;
            case "getarchivedetailurl"://根据FlowCode获取进行Detail模版页面的链接url
                this.GetArchiveDetailUrl(context,strTID,strRID,strSID,strAID,strKeyValue,strUserCode);
                break;
            case "doagreeorrejectoneflow"://针对某条流程进行同意或者决绝操作
                this.DoAgreeOrRejectOneFlow(context,strTID,strRID,strSID,strAID,strKeyValue,strOpFlag,strUserCode);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 获取模版相应描述
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strTID"></param>
    /// <param name="strRID"></param>
    /// <param name="strSID"></param>
    /// <param name="strAID"></param>
    /// <param name="strKeyValue"></param>
    /// <param name="strUserCode"></param>
    public void GetArchiveDesc(HttpContext context,String strTID,String strRID,String strSID,String strUserCode)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        String strMethodDesc = "获取模版相应描述";

        StringBuilder sbResult = new StringBuilder();
        try
        {
            String strSql_TID = "select * from TB_HRTMPH WHERE TID = '"+strTID+"'";
            String strSql_RID = "select * from TB_HRTMPR WHERE TID = '"+strTID+"' AND RID = '"+strRID+"'";
            String strSql_SID = "select * from TB_HRTMPS WHERE TID = '"+strTID+"' AND SID = '"+strSID+"'";

            DataTable dt_TID = SqlParamDao.GetDataTableBySql(strSql_TID);
            DataTable dt_RID = SqlParamDao.GetDataTableBySql(strSql_RID);
            DataTable dt_SID = SqlParamDao.GetDataTableBySql(strSql_SID);
            sbResultData.Append(WebCommon.GetJsonStringByDataTable(dt_TID,"\"ArchiveData\"",false,true));
            sbResultData.Append(","+WebCommon.GetJsonStringByDataTable(dt_RID,"\"RoleData\"",false,true));
            sbResultData.Append(","+WebCommon.GetJsonStringByDataTable(dt_SID,"\"SceneData\"",false,true));

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
                sbResult.Append(","+sbResultData.ToString());
            }
            sbResult.Append("}");
        }
        //log.Error(strMethodDesc+"时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }

    /// <summary>
    /// 获取待办处理的审批列表
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strTID"></param>
    /// <param name="strRID"></param>
    /// <param name="strSID"></param>
    /// <param name="strAID"></param>
    /// <param name="strKeyValue"></param>
    /// <param name="strUserCode"></param>
    public void GetPendingApproveList(HttpContext context,String strTID,String strRID,String strSID,String strAID,String strKeyValue,String strUserCode)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        String strMethodDesc = "获取待办处理的审批列表";

        StringBuilder sbResult = new StringBuilder();
        try
        {
            //获取列表行数据
            DataTable dt = MultiApproveBll.GetArchiveSSLCTByFilter(strTID,strRID,strSID,"",strUserCode,this.Language);
            sbResultData.Append(WebCommon.GetJsonStringByDataTable(dt,"\"ResultData\"",false,true));

            //获取列表列数据
            DataTable dt_Column = MultiApproveBll.GetListShowColumns(strTID,strRID,strSID,this.Language);
            sbResultData.Append(","+WebCommon.GetJsonStringByDataTable(dt_Column,"\"ColumnData\"",false,false));

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
                sbResult.Append(","+sbResultData.ToString());
            }
            sbResult.Append("}");
        }
        //log.Error(strMethodDesc+"时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }

    /// <summary>
    /// 根据FlowCode获取进行Detail模版页面的链接url
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strTID"></param>
    /// <param name="strRID"></param>
    /// <param name="strSID"></param>
    /// <param name="strAID"></param>
    /// <param name="strKeyValue"></param>
    /// <param name="strUserCode"></param>
    public void GetArchiveDetailUrl(HttpContext context,String strTID,String strRID,String strSID,String strAID,String strKeyValue,String strUserCode)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        String strMethodDesc = "根据FlowCode获取进行Detail模版页面的链接url";

        StringBuilder sbResult = new StringBuilder();
        try
        {
            String strURL = MultiApproveBll.GetArchiveDetailUrl(strTID,strRID,strSID,strKeyValue,2) ;

            sbResultData.Append("\"ResultData\":\""+strURL+"\"");
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
                sbResult.Append(","+sbResultData.ToString());
            }
            sbResult.Append("}");
        }
        //log.Error(strMethodDesc+"时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }


    /// <summary>
    /// 针对某条流程进行同意/退回操作
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strTID"></param>
    /// <param name="strRID"></param>
    /// <param name="strSID"></param>
    /// <param name="strAID"></param>
    /// <param name="strKeyValue"></param>
    /// <param name="strOpFlag"></param>
    /// <param name="strUserCode"></param>
    public void DoAgreeOrRejectOneFlow(HttpContext context,String strTID,String strRID,String strSID,String strAID,String strKeyValue,String strOpFlag,String strUserCode)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        String strMethodDesc = strTID+"流程"+strKeyValue+"同意进行下一步";

        StringBuilder sbResult = new StringBuilder();
        try
        {
            //针对某条流程进行同意/退回操作
            strReturnMsg = MultiApproveBll.ApproveOneFlowInstance(strTID,strRID,strSID,strKeyValue,strOpFlag,strUserCode,this.Language,ref strReturnCode) ;
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
                sbResult.Append(","+sbResultData.ToString());
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