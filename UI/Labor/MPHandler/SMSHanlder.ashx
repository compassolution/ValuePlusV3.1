<%@ WebHandler Language="C#" Class="SMSHanlder" %>

using System;
using System.Web;
using System.Collections;
using System.Text;
using System.Data;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using MSXML2;
using Com.ValuePlus.Labor;
using Com.ValuePlus.Web;

public class SMSHanlder : HandlerBase,IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    #region 日志声明
    /// <summary>
    /// 日志声明
    /// </summary>
    private Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    #endregion

    private string strSMS_UID = "SMSLabor";
    private string strSMS_PWD = "SMSEasyLabor";

    public void ProcessRequest (HttpContext context) {
        //解析客户端传递过来的json data
        StreamReader reader = new StreamReader(context.Request.InputStream);
        String strParamJson = HttpUtility.UrlDecode(reader.ReadToEnd());
        log.Error("传入参数的Json data字符串:" + strParamJson.ToString());

        string param = WebCommon.GetJsonValue(strParamJson,"param").ToString();//请求类型参数
        string strMobileNo = WebCommon.GetJsonValue(strParamJson,"sms_mobileno").ToString();//登录名

        log.Error("短信发送-接收手机号:"+strMobileNo);

        switch (param.ToLower().ToString())
        {
            case "sendverifycode":
                this.DoSendVerifyCode(context,strMobileNo);
                break;
                //default:
                //    this.JudgeLogin(context, strUserID, strPassword);
                //    break;
        }
    }

    /// <summary>
    /// 发送验证码短信
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strRequestLanguage"></param>
    /// <param name="strRequestLanguage"></param>
    public void DoSendVerifyCode(HttpContext context,String strMobileNo)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResult = new StringBuilder();
        StringBuilder sbResultData = new StringBuilder();
        try
        {
            Random rad = new Random();//实例化随机数产生器rad；
            String strSendVerifyCode = rad.Next(100000, 1000000).ToString();
            String strSendMsg = "【达易帮】您的手机号码本次进行注册操作，验证码为:"+strSendVerifyCode+"，请勿告知他人！";
            String strSMSParam = "uid=" + strSMS_UID + "&pwd=" + strSMS_PWD + "&tos=" + strMobileNo + "&msg=" + strSendMsg + "&otime=";
                
            string backinfo = LSMS.PostData(strSMSParam);

            //记录发送验证码后的记录,返回记录主键
            string strSMSKey = LSMS.WriteSMSSendRecord(strMobileNo, strSendVerifyCode);
                
            strReturnCode = "1";
            strReturnMsg = "发送验证码短信成功:"+strSendVerifyCode;
            sbResultData.Append("\"ResultData\":{\"VerifyCode\":\""+strSendVerifyCode+"\",\"SMSKey\":\""+strSMSKey+"\"}");
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = "发送验证码短信出错,请稍候重试";
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
        log.Error("发送验证码短信返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }


    public bool IsReusable {
        get {
            return false;
        }
    }

}