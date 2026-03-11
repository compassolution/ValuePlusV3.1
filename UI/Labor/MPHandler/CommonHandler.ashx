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

        string param = WebCommon.GetJsonValue(strParamJson,"param").ToString();//请求类型参数
        string strDicLid = WebCommon.GetJsonValue(strParamJson,"lid").ToString();
        string strDicIsContentStopped = WebCommon.GetJsonValue(strParamJson,"isincludeinvalid").ToString();//是否包含已停用的
        string strIsEscape = WebCommon.GetJsonValue(strParamJson,"isescape").ToString();//是否
        bool isEscape = strIsEscape.Equals("false") ? false : true;

        string strTableName = WebCommon.GetJsonValue(strParamJson,"tablename").ToString();//Json数据库对象
        string strPostDataObject = WebCommon.GetJsonObjectValue(strParamJson,"postdataobj").ToString();//Json数据库对象
        string strPostKeyObject = WebCommon.GetJsonObjectValue(strParamJson,"postkeyobj").ToString();//表主键键值对Json数据库对象

        StringBuilder sbImportParam = new StringBuilder();
        sbImportParam.Append("param:" + param);
        sbImportParam.Append(",strDicLid:" + strDicLid);
        sbImportParam.Append(",strDicIsContentStopped:" + strDicIsContentStopped);
        log.Error("Labor/MPHandler/Login.ashx传入参数:" + sbImportParam.ToString());

        switch (param.ToLower().ToString())
        {
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