<%@ WebHandler Language="C#" Class="IPAddress" %>

using System;
using System.Web;
using System.Collections;
using System.Text;
using System.IO;
using System.Data;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Web;
using System.Net;

public class IPAddress: HandlerBase,IHttpHandler, System.Web.SessionState.IRequiresSessionState
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

        String strRequestLanguage = strCulture.Equals("0") ? "zh-cn" : "en-us";
        switch (param.ToLower().ToString())
        {
            case "getipaddressisvalid"://获取IP地址及其有效性
                this.GetIPAddressValid(context);
                break;
        }
    }

    /// <summary>
    /// 获取IP地址及其有效性
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strUserCode"></param>
    public void GetIPAddressValid(HttpContext context)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        String strMethodDesc = "获取IP地址及其有效性";

        StringBuilder sbResult = new StringBuilder();
        try
        {
            String IsCanDo = "1";

            string strHostName = System.Net.Dns.GetHostName();
            string strClientIPAddress1 = string.Empty;
            //Return real client IP
            if (context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
            {
                strClientIPAddress1 = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            }
            string strClientIPAddress2 = string.Empty;
            //While it can't get the Client IP, it will return proxy IP.
            if (context.Request.ServerVariables["REMOTE_ADDR"] != null)
            {
                strClientIPAddress2 = context.Request.ServerVariables["REMOTE_ADDR"].ToString();
            }
            String strClientIPAddress3 = HttpContext.Current.Request.UserHostAddress;
            String strServerIPAddress = Com.ValuePlus.Utils.RequestUtils.GetLocalhostIPAddress(true);

            ///配置中需匹配的IP地址
            String strSql = " select * from MBConfig_1 where paramName = 'WifiClockModeIPAddress'";
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
            String strValidIPAddress = "";
            if(dt!=null&&dt.Rows.Count==1){
                IsCanDo = "0";
                strValidIPAddress = dt.Rows[0]["paramValue"].ToString();
                //配置中需匹配的IP地址，多个用;隔开
                String[] strArrary = strValidIPAddress.Split(';');
                for(int i=0;i<strArrary.Length;i++){
                    if(!String.IsNullOrEmpty(strArrary[i])){
                        //先检验IP地址1
                        bool IsMatch = IsIPAddressMatch(strClientIPAddress1,strArrary[i]);
                        if(IsMatch){
                            IsCanDo = "1";
                            break;
                        }
                        //再检验IP地址2
                        IsMatch = IsIPAddressMatch(strClientIPAddress2,strArrary[i]);
                        if(IsMatch){
                            IsCanDo = "1";
                            break;
                        }
                    }
                }
            }


            sbResultData.Append("\"IPAddress\":{");
            sbResultData.Append("\"HostName\":\"" + strHostName + "\"");
            sbResultData.Append(",\"ClientIPAddress1\":\"" + strClientIPAddress1 + "\"");
            sbResultData.Append(",\"ClientIPAddress2\":\""+strClientIPAddress2+"\"");
            sbResultData.Append(",\"ClientIPAddress3\":\""+strClientIPAddress3+"\"");
            sbResultData.Append(",\"ServerIPAddress\":\"" + strServerIPAddress + "\"");
            sbResultData.Append(",\"ValidIPAddress\":\"" + strValidIPAddress + "\"");
            sbResultData.Append(",\"IPAddressIsMatch\":\"" + IsCanDo + "\"");
            sbResultData.Append("}");
            strReturnCode = "1";
            strReturnMsg = strMethodDesc+"成功";
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = strMethodDesc+"出错,请稍候重试";
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

    /// <summary>
    /// 判断IP地址是否能匹配规定的地址
    /// </summary>
    /// <param name="strInputIP"></param>
    /// <param name="strStandardIP"></param>
    /// <returns></returns>
    public bool IsIPAddressMatch(String strInputIP,String strStandardIP){
        bool IsMatch = false;
        try{
            String[] strArrayInputIP = strInputIP.Split('.');
            String[] strArrayStandardIP = strStandardIP.Split('.');

            if (
                (strArrayInputIP[0].Trim() == strArrayStandardIP[0].Trim()|| strArrayStandardIP[0].Trim().Equals("*"))
                && (strArrayInputIP[1].Trim() == strArrayStandardIP[1].Trim() || strArrayStandardIP[1].Trim().Equals("*"))
                && (strArrayInputIP[2].Trim() == strArrayStandardIP[2].Trim() || strArrayStandardIP[2].Trim().Equals("*"))
                && (strArrayInputIP[3].Trim() == strArrayStandardIP[3].Trim() || strArrayStandardIP[3].Trim().Equals("*"))
                )
            {
                IsMatch = true;
            }else{
                IsMatch = false;
            }
        }
        catch (Exception ex)
        {
        }

        return IsMatch;
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}