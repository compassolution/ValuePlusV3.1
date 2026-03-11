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
using Com.ValuePlus.Web;
using Com.ValuePlus.Common.Security;

public class SMSHanlder : HandlerBase,IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    #region 日志声明
    /// <summary>
    /// 日志声明
    /// </summary>
    private Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    #endregion

    private string strSMS_UID = "compassolution";
    private string strSMS_PWD = "Compass2006";

    /// <summary>
    /// sms短信是否进行特别验证(1为特别验证，固定验证码为手机号后4位，0为正常验证
    /// </summary>
    private String strSmsIsSpecialVerify = Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("smsIsSpecialVerify");

    public void ProcessRequest (HttpContext context) {

        Hashtable hsTableUrlQuery = this.GetUrlAnalyse(context);

        string param = context.Request["param"] == null ? string.Empty : context.Request["param"].ToString();//请求类型参数
        string strMobileNo = context.Request["sms_mobileno"] == null ? string.Empty : context.Request["sms_mobileno"].ToString();//注册的手机号
        string strProjectId = context.Request["projectId"] == null ? string.Empty : context.Request["projectId"].ToString();//可能会获取到的ProjectId【如果是入职资料填写时则会获取到】

        //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
        param = SQLInjectionDefense.ReplaceSQLReservedKeyword(param);
        strMobileNo = SQLInjectionDefense.ReplaceSQLReservedKeyword(strMobileNo);

        if(String.IsNullOrEmpty(param)){
            //解析客户端传递过来的json data
            StreamReader reader = new StreamReader(context.Request.InputStream);
            String strParamJson = HttpUtility.UrlDecode(reader.ReadToEnd());
            //log.Error("传入参数的Json data字符串:" + strParamJson.ToString());

            param = WebCommon.GetJsonValue(strParamJson,"param").ToString();//请求类型参数
            strMobileNo = WebCommon.GetJsonValue(strParamJson,"sms_mobileno").ToString();//注册的手机号
            strProjectId = WebCommon.GetJsonValue(strParamJson,"projectId").ToString();//可能会获取到的ProjectId【如果是入职资料填写时则会获取到】
        }

        log.Error("短信发送-传入参数的strProjectId:" + strProjectId);
        log.Error("短信发送-接收手机号:"+strMobileNo);
        log.Error("短信发送参数smsIsSpecialVerify为:"+strSmsIsSpecialVerify);
        //string strSendType = "1";
        //String strSendMsg = "";
        switch (param.ToLower().ToString())
        {
            case "sendverifycode":
                this.DoSendVerifyCode(context,strMobileNo,strProjectId);
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
    /// <param name="strMobileNo"></param>
    /// <param name="strProjectId"></param>
    public void DoSendVerifyCode(HttpContext context,String strMobileNo,String strProjectId)
    {
        try
        {
            Random rad = new Random();//实例化随机数产生器rad；
            String strSendVerifyCode = rad.Next(100000, 1000000).ToString();
            if(strSmsIsSpecialVerify.Equals("1") && strMobileNo.Length > 4)
            {
                strSendVerifyCode = strMobileNo.Substring(strMobileNo.Length - 4);;
            }else{
                String strSIgnature = this.GetSMSSignatureByProjectId(strProjectId);
                String strSendMsg = strSIgnature + "您的手机号码本次进行注册操作，验证码为:"+strSendVerifyCode;
                String strSMSParam = "uid=" + strSMS_UID + "&pwd=" + strSMS_PWD + "&tos=" + strMobileNo + "&msg=" + strSendMsg + "&otime=";

                log.Error("短信发送-参数:"+strSMSParam);
                string backinfo = PostData("http://service2.winic.org/service.asmx/SendMessages?",strSMSParam);

            }
            log.Error("手机号"+strMobileNo+"获取验证码为:"+strSendVerifyCode);
            context.Response.Write(strSendVerifyCode);
        }
        catch (Exception ex)
        {
            log.Error(ex);
            context.Response.Write("");
        }
    }

    /// <summary>
    /// 根据strProjectId来获取短信签名
    /// </summary>
    /// <param name="strProjectId"></param>
    /// <returns></returns>
    public String GetSMSSignatureByProjectId(String strProjectId){
        String strSIgnature = "【上海达晶软件科技有限公司】";//默认签名
        //如果是如下项目，则签名为广州达宬信息科技(达宬自签项目)
        switch(strProjectId){
            case "VP_HR_FSGZ":
            case "VP_HR_CSBJOP":
                strSIgnature = "【广州达宬信息科技】";
                break;
            default:
                break;
        }

        return strSIgnature;
    }

    /// <summary>
    /// 短信接口发送
    /// </summary>
    /// <param name="purl"></param>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string PostData(string purl, string str)
    {
        try
        {
            byte[] data = System.Text.Encoding.GetEncoding("UTF-8").GetBytes(str);
            // 准备请求    
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(purl);
            //设置超时     
            req.Timeout = 30000;
            req.Method = "Post";
            req.ContentType = "application/x-www-form-urlencoded";
            req.ContentLength = data.Length;
            Stream stream = req.GetRequestStream();
            // 发送数据   
            stream.Write(data, 0, data.Length);
            stream.Close();

            HttpWebResponse rep = (HttpWebResponse)req.GetResponse();
            Stream receiveStream = rep.GetResponseStream();
            Encoding encode = System.Text.Encoding.GetEncoding("UTF-8");
            // Pipes the stream to a higher level stream reader with the required encoding format.   
            StreamReader readStream = new StreamReader(receiveStream, encode);

            Char[] read = new Char[256];
            int count = readStream.Read(read, 0, 256);
            StringBuilder sb = new StringBuilder("");
            while (count > 0)
            {
                String readstr = new String(read, 0, count);
                sb.Append(readstr);
                count = readStream.Read(read, 0, 256);
            }

            rep.Close();
            readStream.Close();

            return sb.ToString();
        }
        catch (Exception ex)
        {
            return "posterror";
        }
    }

    ///// <summary>
    ///// 获取客户信息
    ///// </summary>
    ///// <param name="context"></param>
    ///// <param name="strRequestLanguage"></param>
    //public void GetCustomerInfo(HttpContext context,String strRequestLanguage)
    //{
    //    try
    //    {
    //        string Send_URL ="http://service.winic.org:8009/sys_port/gateway/mo.aspid=compassolution&pwd=Compass2006";

    //        //____________________________

    //        MSXML2.XMLHTTP xmlhttp = new MSXML2.XMLHTTP();

    //        xmlhttp.open("GET", Send_URL, false, null, null);
    //        xmlhttp.send("");
    //        MSXML2.XMLDocument dom = new XMLDocument();
    //        Byte[] b = (Byte[])xmlhttp.responseBody;

    //        //string Flag = System.Text.ASCIIEncoding.UTF8.GetString(b, 0, b.Length);
    //        string andy = System.Text.Encoding.GetEncoding("GB2312").GetString(b).Trim();

    //        //json = "2";
    //        context.Response.Write(andy);
    //    }
    //    catch (Exception ex)
    //    {
    //        log.Error(ex);
    //        context.Response.Write("-1");
    //    }
    //}
    public bool IsReusable {
        get {
            return false;
        }
    }

}