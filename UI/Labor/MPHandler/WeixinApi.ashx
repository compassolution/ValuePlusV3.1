<%@ WebHandler Language="C#" Class="WeixinApi" %>

using System;
using System.Web;
using System.Collections;
using System.Text;
using System.IO;
using System.Data;
using System.Security.Cryptography;
using Com.ValuePlus.BLL.SysManager;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Web;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using Com.ValuePlus.Weixin;
using Com.ValuePlus.Labor;

public class WeixinApi : HandlerBase,IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    #region 日志声明
    /// <summary>
    /// 日志声明
    /// </summary>
    private Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    #endregion

    //【小程序登录】调用 auth.code2Session 接口，换取 用户唯一标识 OpenID 和 会话密钥 session_key。
    //private String WeixinMP_URL_GetAccessTokenByCode = "https://api.weixin.qq.com/sns/jscode2session?appid={0}&secret={1}&js_code={2}&grant_type=authorization_code";

    //【小程序登录】调用 sendTemplateMessage发送模板消息
    //private String WeixinMP_URL_sendTemplateMessage = "https://api.weixin.qq.com/cgi-bin/message/wxopen/template/send?access_token=ACCESS_TOKEN";

    private String strAppId = Com.ValuePlus.Labor.GetSysParams.GetWeixin_AppId();
    private String strAppsecret = Com.ValuePlus.Labor.GetSysParams.GetWeixin_Appsecret();

    public void ProcessRequest (HttpContext context) {
        //解析客户端传递过来的json data
        StreamReader reader = new StreamReader(context.Request.InputStream);
        String strParamJson = HttpUtility.UrlDecode(reader.ReadToEnd());
        log.Error("传入参数的Json data字符串:" + strParamJson.ToString());

        string param = WebCommon.GetJsonValue(strParamJson,"param").ToString();//请求类型参数
        string strUserCode = WebCommon.GetJsonValue(strParamJson,"usercode").ToString();//用户编码
        string strWeixinCode = WebCommon.GetJsonValue(strParamJson,"weixin_code").ToString();//weixin_code
        string strOpenId = WebCommon.GetJsonValue(strParamJson,"weixin_openid").ToString();//weixin_openid
        string strAccesstoken = WebCommon.GetJsonValue(strParamJson,"weixin_accesstoken").ToString();//weixin_accesstoken

        string strSessionKey = WebCommon.GetJsonValue(strParamJson,"weixin_session_key").ToString();//weixin_session_key
        string strEncryptedData = WebCommon.GetJsonValue(strParamJson, "weixin_encryptedData").ToString();//weixin_encryptedData
        string strIv = WebCommon.GetJsonValue(strParamJson, "weixin_iv").ToString();//weixin_iv
        strSessionKey = strSessionKey.Replace(" ","+");
        strEncryptedData = strEncryptedData.Replace(" ","+");
        strIv = strIv.Replace(" ","+");

        log.Error("传入参数的weixin_code:" + strWeixinCode);
        log.Error("传入参数的weixin_openid:" + strOpenId);
        log.Error("传入参数的weixin_encryptedData:" + strEncryptedData);
        log.Error("传入参数的weixin_iv:" + strIv);
        switch (param.ToLower().ToString())
        {
            case "weixin_getopenid":
                this.GetOpenIdByCode(context, strAppId,strAppsecret,strWeixinCode,strEncryptedData,strIv);
                break;
            case "weixin_getuserencrypteddata":
                this.GetUserEncryptedData(context, strEncryptedData,strIv,strSessionKey);
                break;
            case "weixin_getaccesstoken":
                this.GetAccessToken(context, strAppId, strAppsecret);
                break;
            case "weixin_getuserinfo":
                this.GetUserInfoByOpenId(context,strAccesstoken,strOpenId);
                break;
            case "weixin_sendtestnotice":
                this.SendTestServiceNotice(context);
                break;

            default:
                //this.JudgeLogin(context, strAccount, strPassword);
                break;
        }
    }

    /// <summary>
    /// 小程序发送测试订阅消息
    /// </summary>
    /// <param name="context"></param>
    public void SendTestServiceNotice(HttpContext context)
    {
        log.Error("小程序发送订阅消息");

        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();

        StringBuilder sbResult = new StringBuilder();
        try
        {
            String strTemplateId_WorkOrderPending = Com.ValuePlus.Labor.GetSysParams.GetWeixin_TemplateId_WorkOrderPending();
            //获取AccessToken的接口(外包工的入口编码为030)
            String strNow = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            String strrAccessToken = AccessTokenGetter.GetValidAccessToken(strAppId, strAppsecret, strNow, "030");
            //请求地址
            //string apiUrl = Com.ValuePlus.Weixin.Const.WeixinMP_URL_sendSubscribeMessage;
            string apiUrl = "https://api.weixin.qq.com/cgi-bin/message/subscribe/send?access_token={0}";
            string requestUrl = string.Format(apiUrl, strrAccessToken);

            String strOpenId = "optuf4kwK8y1tJBM9Mgkd5yClWmM";
            String strNowZH = DateTime.Now.ToString("yyyy年M月dd日 HH:mm");
            StringBuilder sbJsonData = new StringBuilder();
            sbJsonData.Append("{");
            sbJsonData.Append("     \"touser\": \""+strOpenId+"\",");
            sbJsonData.Append("     \"template_id\": \""+strTemplateId_WorkOrderPending+"\",");
            sbJsonData.Append("     \"page\": \"/pages/WorkOrder/WorkOrderList\",");
            sbJsonData.Append("     \"miniprogram_state\": \"formal\",");
            sbJsonData.Append("     \"lang\": \"zh_CN\",");//string 	否 	模板需要放大的关键词，不填则默认无放大
            sbJsonData.Append("     \"data\": {");
            sbJsonData.Append("         \"character_string1\": {");
            sbJsonData.Append("             \"value\": \"WO20120******0000001\"");
            sbJsonData.Append("         },");
            sbJsonData.Append("         \"thing2\": {");
            sbJsonData.Append("             \"value\": \"******公司\"");
            sbJsonData.Append("         },");
            sbJsonData.Append("         \"name3\": {");
            sbJsonData.Append("             \"value\": \"张*三\"");
            sbJsonData.Append("         },");
            sbJsonData.Append("         \"thing4\": {");
            sbJsonData.Append("             \"value\": \"工单发布\"");
            sbJsonData.Append("         },");
            sbJsonData.Append("         \"date5\": {");
            sbJsonData.Append("             \"value\": \""+strNowZH+"\"");
            sbJsonData.Append("         }");
            sbJsonData.Append("     }");
            sbJsonData.Append("}");
            log.Error("小程序发送订阅消息时发送的数据："+sbJsonData.ToString());
            ;
            string ReText = PublicHelper.WebRequestPostOrGet(requestUrl, sbJsonData.ToString());//post/get方法获取信息 
            JavaScriptSerializer myJson = new JavaScriptSerializer();
            Dictionary<string, object> DicText = (Dictionary<string, object>)myJson.DeserializeObject(ReText);
            String strErrCode = DicText["errcode"].ToString();
            String strErrMsg = DicText["errmsg"].ToString();

            sbResultData.Append("\"ResultData\":{\"1\":\"1\"");
            sbResultData.Append("   ,\"openid\":\"" + strOpenId + "\"");
            sbResultData.Append("   ,\"access_token\":\"" + strrAccessToken + "\"");
            sbResultData.Append("   ,\"errcode\":\"" + strErrCode + "\"");
            sbResultData.Append("   ,\"errmsg\":\"" + strErrMsg + "\"");
            //遍历key
            foreach (string key in DicText.Keys)
            {
                if (!key.Equals("watermark"))
                {
                    sbResultData.Append("   ,\"" + key + "\":\"" + DicText[key].ToString() + "\"");
                }
            }

            sbResultData.Append("}");
            strReturnCode = "1";
            strReturnMsg = "小程序发送订阅消息成功";
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = "小程序发送订阅消息出错";
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
        log.Error("小程序发送订阅消息时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }

    /// <summary>
    /// 微信登录授权获取AccessToken
    /// </summary>
    public void GetAccessToken(HttpContext context, string Appid, string Appsecret)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();

        StringBuilder sbResult = new StringBuilder();
        try
        {
            String strNow = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //获取AccessToken的接口(外包工的入口编码为030)
            String strrAccessToken = AccessTokenGetter.GetValidAccessToken(Appid,Appsecret,strNow,"030");
            sbResultData.Append("\"ResultData\":{");
            sbResultData.Append("   \"appid\":\""+Appid+"\"");
            sbResultData.Append("   ,\"appsecret\":\""+Appsecret+"\"");
            sbResultData.Append("   ,\"access_token\":\""+strrAccessToken+"\"");

            sbResultData.Append("}");
            strReturnCode = "1";
            strReturnMsg = "微信登录授权获取AccessToken成功";
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = "微信登录授权获取AccessToken出错";
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
        log.Error("微信登录授权获取AccessToken时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }

    /// <summary>
    /// 微信登录授权用Code获取Openid
    /// </summary>
    /// <param name="context"></param>
    /// <param name="Appid"></param>
    /// <param name="Appsecret"></param>
    /// <param name="Code"></param>
    /// <param name="strEncryptedData">加密的手机号码信息</param>
    /// <param name="strIv">加密算法的初始向量</param>
    public void GetOpenIdByCode(HttpContext context, string Appid, string Appsecret,string Code,String strEncryptedData, String strIv)
    {
        log.Error("微信登录授权用Code获取Openid，Appid:" + Appid);
        log.Error("微信登录授权用Code获取Openid，Appsecret:" + Appsecret);
        log.Error("微信登录授权用Code获取Openid，Code:" + Code);
        log.Error("微信登录授权用Code获取Openid，strEncryptedData:" + strEncryptedData);
        log.Error("微信登录授权用Code获取Openid，strIv:" + strIv);
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();

        StringBuilder sbResult = new StringBuilder();
        try
        {
            String strOpenId = "";
            String strSession_Key = "";
            String strUnionId = "";

            string url = string.Format(Com.ValuePlus.Weixin.Const.WeixinMP_URL_GetAccessTokenByCode, Appid, Appsecret, Code);
            string ReText = PublicHelper.WebRequestPostOrGet(url,"");//post/get方法获取信息 
            JavaScriptSerializer myJson = new JavaScriptSerializer();
            Dictionary<string, object> DicText = (Dictionary<string, object>)myJson.DeserializeObject(ReText);
            if (DicText.ContainsKey("openid"))
            {
                strOpenId = DicText["openid"].ToString();
            }
            if (DicText.ContainsKey("session_key"))
            {
                strSession_Key = DicText["session_key"].ToString();
            }
            if (DicText.ContainsKey("unionid"))
            {
                strUnionId = DicText["unionid"].ToString();
            }
            sbResultData.Append("\"ResultData\":{\"1\":\"1\"");
            //sbResultData.Append("   ,\"openid\":\"" + strOpenId + "\"");
            //sbResultData.Append("   ,\"session_key\":\"" + strSession_Key + "\"");
            //遍历key
            foreach (string key in DicText.Keys)
            {
                if (!key.Equals("watermark"))
                {
                    sbResultData.Append("   ,\"" + key + "\":\"" + DicText[key].ToString() + "\"");
                }
            }

            //解析手机号码
            String strPhoneNumbers = this.getPhoneNumber(strEncryptedData, strIv, strSession_Key);
            if (!String.IsNullOrEmpty(strPhoneNumbers))
            {
                sbResultData.Append(" ,"+strPhoneNumbers);
            }

            sbResultData.Append("}");
            strReturnCode = "1";
            strReturnMsg = "微信登录授权用Code获取Openid成功";
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = "微信登录授权用Code获取Openid出错";
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
        log.Error("微信授权-用Code获取Openid和access_token时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }

    /// <summary>
    /// 微信登录后获取UnionId等加密信息[主要是unionId]
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strEncryptedData">加密的手机号码信息</param>
    /// <param name="strIv">加密算法的初始向量</param>
    /// <param name="strSessionKey">加密算法的初始向量</param>
    public void GetUserEncryptedData(HttpContext context, String strEncryptedData, String strIv,String strSessionKey)
    {
        log.Error("微信登录后获取UnionId等加密信息，strEncryptedData:" + strEncryptedData);
        log.Error("微信登录后获取UnionId等加密信息，strIv:" + strIv);
        log.Error("微信登录后获取UnionId等加密信息，strSessionKey:" + strSessionKey);
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();

        StringBuilder sbResult = new StringBuilder();
        try
        {
            String strDecodedData = this.DecodeData(strEncryptedData,strIv,strSessionKey);
            if (!String.IsNullOrEmpty(strDecodedData))
            {
                sbResultData.Append("\"ResultData\":{\"1\":\"1\"");
                JavaScriptSerializer myJson = new JavaScriptSerializer();
                Dictionary<string, object> DicText = (Dictionary<string, object>)myJson.DeserializeObject(strDecodedData);

                ////遍历key
                foreach (string key in DicText.Keys)
                {
                    //if (key.Equals("unionId"))
                    //{
                    //    sbResultData.Append(",\"" + key + "\":\"" + DicText[key].ToString() + "\"");
                    //    log.Error("微信授权-解析加密信息 key[" + key + "] ：" + DicText[key].ToString());
                    //}

                    if (!key.Equals("watermark"))
                    {
                        sbResultData.Append(",\"" + key + "\":\"" + DicText[key].ToString() + "\"");
                        log.Error("微信授权-解析加密信息 key[" + key + "] ：" + DicText[key].ToString());
                    }
                }

                sbResultData.Append("}");
                strReturnCode = "1";
                strReturnMsg = "微信登录后获取UnionId等加密信息成功";
            }else
            {
                strReturnCode = "0";
                strReturnMsg = "微信登录后获取UnionId等加密信息失败";
            }
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = "微信登录后获取UnionId等加密信息出错";
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
        log.Error("微信授权-微信登录后获取UnionId等加密信息时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }



    /// <summary>
    /// AES解密：从小程序中 getPhoneNumber 返回值中，解析手机号码
    /// </summary>
    /// <param name="encryptedData">包括敏感数据在内的完整用户信息的加密数据，详细见加密数据解密算法</param>
    /// <param name="IV">加密算法的初始向量</param>
    /// <param name="Session_key"></param>
    /// <returns>手机号码</returns>
    private String getPhoneNumber(string encryptedDataStr, string IV, string Session_key)
    {
        StringBuilder sbPhoneNumber = new StringBuilder();
        try
        {
            String strDecodedData = this.DecodeData(encryptedDataStr,IV,Session_key);
            if (!String.IsNullOrEmpty(strDecodedData))
            {
                JavaScriptSerializer myJson = new JavaScriptSerializer();
                Dictionary<string, object> DicText = (Dictionary<string, object>)myJson.DeserializeObject(strDecodedData);

                ////遍历key
                int iIndex = 0;
                foreach (string key in DicText.Keys)
                {
                    if (key.Equals("phoneNumber") || key.Equals("purePhoneNumber") || key.Equals("countryCode"))
                    {
                        if (iIndex == 0)
                        {
                            sbPhoneNumber.Append("\"" + key + "\":\"" + DicText[key].ToString() + "\"");
                        }else
                        {
                            sbPhoneNumber.Append(",\"" + key + "\":\"" + DicText[key].ToString() + "\"");
                        }
                        log.Error("微信授权-getPhoneNumber 返回值中，解析手机号码 key[" + key + "] ：" + DicText[key].ToString());
                        iIndex++;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            log.Error("getPhoneNumber 错误:"+ex);
        }
        return sbPhoneNumber.ToString();
    }

    /// <summary>
    /// 微信加密数据解密
    /// </summary>
    /// <param name="encryptedDataStr"></param>
    /// <param name="IV"></param>
    /// <param name="Session_key"></param>
    /// <returns></returns>
    public string DecodeData(string encryptedDataStr, string IV, string Session_key)
    {
        StringBuilder sbDecodedData = new StringBuilder();
        try
        {
            log.Error("DecodeData 加密的手机号码信息："+encryptedDataStr);
            log.Error("DecodeData 加密算法的初始向量："+IV);
            log.Error("DecodeData Session_key："+Session_key);

            if (!String.IsNullOrEmpty(encryptedDataStr) && !String.IsNullOrEmpty(IV))
            {
                //#####第一种方法
                byte[] encryData = Convert.FromBase64String(encryptedDataStr);  // strToToHexByte(text);
                RijndaelManaged rijndaelCipher = new RijndaelManaged();
                rijndaelCipher.KeySize = 128;
                rijndaelCipher.Key = Convert.FromBase64String(Session_key); // Encoding.UTF8.GetBytes(AesKey);
                rijndaelCipher.IV = Convert.FromBase64String(IV);// Encoding.UTF8.GetBytes(AesIV);
                rijndaelCipher.Mode = CipherMode.CBC;
                rijndaelCipher.Padding = PaddingMode.PKCS7;
                ICryptoTransform transform = rijndaelCipher.CreateDecryptor();
                byte[] plainText = transform.TransformFinalBlock(encryData, 0, encryData.Length);
                sbDecodedData.Append(Encoding.Default.GetString(plainText));
                log.Error("微信授权-解密数据 result：" + sbDecodedData.ToString());
            }
        }
        catch (Exception ex)
        {
            log.Error("微信加密数据解密 错误:"+ex);
        }
        return sbDecodedData.ToString();
    }

    /// <summary>
    /// 微信登录授权用Openid获取用户信息
    /// </summary>
    public void GetUserInfoByOpenId(HttpContext context, string strAccesstoken, string strOpenId)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();

        StringBuilder sbResult = new StringBuilder();
        try
        {
            string url = string.Format(Com.ValuePlus.Weixin.Const.Weixin_URL_GetUserInfo, strAccesstoken, strOpenId);
            string ReText = PublicHelper.WebRequestPostOrGet(url,"");//post/get方法获取信息 
            JavaScriptSerializer myJson = new JavaScriptSerializer();
            Dictionary<string, object> DicText = (Dictionary<string, object>)myJson.DeserializeObject(ReText);

            sbResultData.Append("\"ResultData\":{");
            sbResultData.Append("   \"Openid\":\""+strOpenId+"\"");
            sbResultData.Append("   ,\"Access_token\":\""+strAccesstoken+"\"");
            //遍历key
            foreach (string key in DicText.Keys)
            {
                sbResultData.Append("   ,\""+key+"\":\""+DicText[key].ToString()+"\"");
            }
            sbResultData.Append("}");
            strReturnCode = "1";
            strReturnMsg = "微信登录授权用Openid获取用户信息成功";
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = "微信登录授权用Openid获取用户信息出错";
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
        log.Error("微信登录授权用Openid获取用户信息时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }



    public bool IsReusable {
        get {
            return false;
        }
    }

}