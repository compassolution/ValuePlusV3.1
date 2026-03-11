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
using Com.ValuePlus.Labor;

public class CommonHandler : HandlerBase,IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    public void ProcessRequest (HttpContext context) {
        //解析客户端传递过来的json data
        StreamReader reader = new StreamReader(context.Request.InputStream);
        String strParamJson = HttpUtility.UrlDecode(reader.ReadToEnd());
        log.Error("传入参数的Json data字符串:" + strParamJson.ToString());

        string strParam = WebCommon.GetJsonValue(strParamJson,"param").ToString();//请求类型参数
        string strProjectId = WebCommon.GetJsonValue(strParamJson,"projectid").ToString();
        string strWXMPModule = WebCommon.GetJsonValue(strParamJson,"wxmpmodule").ToString();
        string strDicLid = WebCommon.GetJsonValue(strParamJson,"lid").ToString();
        string strDicIsContentStopped = WebCommon.GetJsonValue(strParamJson,"isincludeinvalid").ToString();//是否包含已停用的
        string strIsEscape = WebCommon.GetJsonValue(strParamJson,"isescape").ToString();//是否
        bool isEscape = strIsEscape.Equals("false") ? false : true;

        string strTableName = WebCommon.GetJsonValue(strParamJson,"tablename").ToString();//Json数据库对象
        string strPostDataObject = WebCommon.GetJsonObjectValue(strParamJson,"postdataobj").ToString();//Json数据库对象
        string strPostKeyObject = WebCommon.GetJsonObjectValue(strParamJson,"postkeyobj").ToString();//表主键键值对Json数据库对象

        StringBuilder sbImportParam = new StringBuilder();
        sbImportParam.Append("param:" + strParam);
        sbImportParam.Append(",strDicLid:" + strDicLid);
        sbImportParam.Append(",strDicIsContentStopped:" + strDicIsContentStopped);
        log.Error("Labor/MPHandler/Login.ashx传入参数:" + sbImportParam.ToString());

        switch (strParam.ToLower().ToString())
        {
            case "judgeishavalicense":
                context.Response.Write(this.GetIsHaveLicenseByProjectId(context, strProjectId, strWXMPModule));
                break;
            case "getdicdetail":
                this.GetDicDetail(context, strDicLid, bool.Parse(strDicIsContentStopped),isEscape);
                break;
            case "judgerecordisexists":
                //判断在某表中是否存在某些记录
                //strPostDataObject为条件的json对象
                context.Response.Write(JObjectToDB.JudgeRecordIsExistsReturnJson(strTableName,strPostDataObject,strPostKeyObject));
                break;
        }

    }

    /// <summary>
    /// 获取某就职单位是否获取的某功能模块的授权
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strProjectId"></param>
    /// <param name="strWXMPModule">LID=WXMPModule</param>
    private String GetIsHaveLicenseByProjectId(HttpContext context,String strProjectId,String strWXMPModule)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "获取某就职单位是否获取的某功能模块的授权";
        try
        {

            //context.Response.Write(ProjectWXAuth.JudgeWXAuthIsValidByProjectId(strProjectId,strWXMPModule,"").ToString().ToLower());

            String strNow = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select * from WXProjectAuth_1 where 1=1");
            sbSql.Append(" and ProjectId = '"+strProjectId+"'");
            sbSql.Append(" and '"+strNow+"' between convert(varchar(20),[PeriodFrom"+strWXMPModule+"],120) and convert(varchar(20),[PeriodTo"+strWXMPModule+"],120)");
            String strSql = sbSql.ToString();
            log.Error(strMethodDesc+"SQL:"+strSql);

            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
            sbResultData.Append(WebCommon.GetJsonStringByDataTable(dt,"\"ResultData\"",false));

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
    ///获取字典明细
    /// </summary>
    public void GetDicDetail(HttpContext context, String strDicLid, bool isContentStopped,bool isEscape)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();

        StringBuilder sbResult = new StringBuilder();
        try
        {
            sbResultData.Append("\"ResultData\":");
            sbResultData.Append(DicGetter.GetDictionaryList(strDicLid,isContentStopped,isEscape));
            sbResultData.Append("");
            strReturnCode = "1";
            strReturnMsg = "获取字典明细成功";
            log.Error("获取字典明细成功:" + sbResultData.ToString());
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = "获取字典明细出错";
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
        context.Response.Write(sbResult.ToString());
    }


    public bool IsReusable {
        get {
            return false;
        }
    }

}