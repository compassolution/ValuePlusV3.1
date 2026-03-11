<%@ WebHandler Language="C#" Class="WXWebLogin" %>

using System;
using System.Web;
using System.Collections;
using System.Text;
using System.Data;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Web;
using Com.ValuePlus.Weixin;

/// <summary>
/// 新版HRMobile(即UniApp版本)微信公众号登录时使用
/// </summary>
public class WXWebLogin : HandlerBase,IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    #region 日志声明
    /// <summary>
    /// 日志声明
    /// </summary>
    private Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    #endregion

    //private string Weixin_URL_GetCodeUrl = "https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_userinfo&state=STATE#wechat_redirect";//用户同意授权，获取code
    //private string Weixin_URL_GetAccessTokenByCode = "https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code";//通过code换取网页授权access_token
    //private string Weixin_URL_RefreshToken = "https://api.weixin.qq.com/sns/oauth2/refresh_token?appid={0}&grant_type=refresh_token&refresh_token={1}";//刷新access_token
    //private string Weixin_URL_GetUserInfo = "https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}&lang=zh_CN";//用access_token和openid获取到用户基本信息

    public void ProcessRequest (HttpContext context) {

        //解析客户端传递过来的json data
        StreamReader reader = new StreamReader(context.Request.InputStream);
        String strParamJson = HttpUtility.UrlDecode(reader.ReadToEnd());
        //log.Error("传入参数的Json data字符串:" + strParamJson.ToString());

        string param = WebCommon.GetJsonValue(strParamJson,"param").ToString();//请求类型参数
        string strRedirectUrl = WebCommon.GetJsonValue(strParamJson,"weinxin_redirecturl").ToString();//需定位的URL
        string strLoginCode = WebCommon.GetJsonValue(strParamJson,"weinxin_code").ToString();//登录返回的Code        
        string strOpenId = WebCommon.GetJsonValue(strParamJson,"weinxin_openid").ToString();//登录用户的OpenID
        string strAccesstoken = WebCommon.GetJsonValue(strParamJson,"weinxin_accesstoken").ToString();//登录用户的weinxin_accesstoken
        string strMobileNo = WebCommon.GetJsonValue(strParamJson,"weinxin_mobileno").ToString();//登录用户的OpenID
        string strPassword = WebCommon.GetJsonValue(strParamJson,"weinxin_password").ToString();//登录用户的OpenID

        //strMobileNo = "13924207569";
        //strPassword = "123456";

        log.Error("微信授权-param:"+param);
        switch (param.ToLower().ToString())
        {
            case "weixin_getcode":
                this.GetCodeUrl(context,GetSysParams.GetWeixin_AppId(),strRedirectUrl);
                break;
            case "weixin_getopenid":
                this.GetOpenIdByCode(context,GetSysParams.GetWeixin_AppId(),GetSysParams.GetWeixin_Appsecret(),strLoginCode);
                break;
            case "weixin_getuserinfo":
                this.GetUserInfoByOpenId(context,strAccesstoken,strOpenId);
                break;
            case "weixin_isreg":
                this.JudgeIsRegByOpenId(context,strOpenId);
                break;
            case "weixin_regopenid":
                this.RegisterOpenId(context,strOpenId,strMobileNo,strPassword);
                break;
            case "weixin_mobileisbanding":
                this.JudgeMobileNoIsRegisted(context,strOpenId,strMobileNo);
                break;
            case "weixin_clearmobileno":
                this.ClearMobileNo(context,strOpenId,strMobileNo);
                break;
        }
    }

    /// <summary>
    /// 对页面是否要用授权,获取Code Appid是微信应用id
    /// </summary>
    /// <param name="context"></param>
    /// <param name="Appid"></param>
    /// <param name="redirect_uri"></param>
    /// <returns></returns>
    public void GetCodeUrl(HttpContext context,string Appid, string redirect_uri)
    {
        //对url进行一次编码，防止有包括#在内的特殊字符
        redirect_uri = Microsoft.JScript.GlobalObject.encodeURIComponent(redirect_uri);
        string strReturnURL = string.Format(Const.Weixin_URL_GetCodeUrl, Appid, redirect_uri);
        strReturnURL = Microsoft.JScript.GlobalObject.encodeURIComponent(strReturnURL);
        //strReturnURL = Microsoft.JScript.GlobalObject.escape(strReturnURL);
        log.Error("微信授权-获取Code返回地址:"+strReturnURL);
        context.Response.Write(strReturnURL);
    }

    /// <summary>
    /// 用Code换取Openid
    /// </summary>
    /// <param name="context"></param>
    /// <param name="Appid"></param>
    /// <param name="Appsecret"></param>
    /// <param name="Code"></param>
    public void GetOpenIdByCode(HttpContext context,string Appid, string Appsecret,string Code)
    {
        String strOpenID = "";
        String strAccess_Token = "";
        String strRefreshToken = "";
        StringBuilder sbResult = new StringBuilder();
        sbResult.Append("{\"ResultData\":[{");
        try
        {
            string url = string.Format(Const.Weixin_URL_GetAccessTokenByCode, Appid, Appsecret, Code);
            string ReText = WebRequestPostOrGet(url,"");//post/get方法获取信息 
            JavaScriptSerializer myJson = new JavaScriptSerializer();
            Dictionary<string, object> DicText = (Dictionary<string, object>)myJson.DeserializeObject(ReText);
            if (DicText.ContainsKey("openid"))
            {
                strOpenID = DicText["openid"].ToString();
            }
            if (DicText.ContainsKey("access_token"))
            {
                strAccess_Token = DicText["access_token"].ToString();
            }
            if (DicText.ContainsKey("refresh_token"))
            {
                strRefreshToken = DicText["refresh_token"].ToString();
            }
            if (String.IsNullOrEmpty(strAccess_Token))
            {
                //如果Access_Token为空即失效了，则重新刷新
                string url_RefreshToken = string.Format(Const.Weixin_URL_RefreshToken, Appid, strRefreshToken);
                string ReText_RefreshToken = WebRequestPostOrGet(url_RefreshToken,"");//post/get方法获取信息 
                JavaScriptSerializer myJson_RefreshToken = new JavaScriptSerializer();
                Dictionary<string, object> DicText_RefreshToken = (Dictionary<string, object>)myJson.DeserializeObject(ReText);
                if (DicText_RefreshToken.ContainsKey("access_token"))
                {
                    strAccess_Token = DicText["access_token"].ToString();
                }
                //log.Error("微信授权-刷新Token后，access_token:"+strAccess_Token);
            }
            sbResult.Append("\"OpenId\":\""+Microsoft.JScript.GlobalObject.escape(strOpenID)+"\"");
            sbResult.Append(",\"Access_token\":\""+Microsoft.JScript.GlobalObject.escape(strAccess_Token)+"\"");
        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
        sbResult.Append("}]}");
        //log.Error("微信授权-用Code("+Code+")获取Openid:"+strOpenID+",access_token:"+strAccess_Token);
        log.Error("微信授权-用Code("+Code+")返回Openid和access_token，结果集:"+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }

    /// <summary>
    /// 根据Access_Token和Openid获取用户信息
    /// 并更新到数据库中
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strAccesstoken"></param>
    /// <param name="strOpenId"></param>
    public void GetUserInfoByOpenId(HttpContext context,string strAccesstoken, string strOpenId)
    {
        log.Error("根据Access_Token和Openid获取用户信息并保存到数据，Access_Token：("+strAccesstoken+")，Openid:"+strOpenId);
        StringBuilder sbSql = new StringBuilder();
        try
        {
            string url = string.Format(Const.Weixin_URL_GetUserInfo, strAccesstoken, strOpenId);
            string ReText = WebRequestPostOrGet(url,"");//post/get方法获取信息 
            JavaScriptSerializer myJson = new JavaScriptSerializer();
            Dictionary<string, object> DicText = (Dictionary<string, object>)myJson.DeserializeObject(ReText);
            String strNickName = DicText.ContainsKey("nickname") ? DicText["nickname"].ToString() : "";
            String strSex = DicText.ContainsKey("sex") ? DicText["sex"].ToString() : "";
            String strProvince = DicText.ContainsKey("province") ? DicText["province"].ToString() : "";
            String strCity = DicText.ContainsKey("city") ? DicText["city"].ToString() : "";
            String strCountry = DicText.ContainsKey("country") ? DicText["country"].ToString() : "";
            String strHeadImgUrl = DicText.ContainsKey("headimgurl") ? DicText["headimgurl"].ToString() : "";
            String strPrivilege = DicText.ContainsKey("privilege") ? DicText["privilege"].ToString() : "";
            String strUnionid = DicText.ContainsKey("unionid") ? DicText["unionid"].ToString() : "";

            String strSqlSelect = "select count(1) from [WXUser_1] where [OpenId] = '"+strOpenId+"'";
            int iCount1 = SqlParamDao.ExecuteScalarBySql(strSqlSelect.ToString());

            if (iCount1 <= 0)
            {
                String strNowTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                sbSql.Append("insert into [WXUser_1] ([OpenId],[NickName],[Sex],[Province],[City],[Country],[HeadImgUrl],[Privilege],[WXEntryType],[RegTime],[LastTime]) values (");
                sbSql.Append(" '" + strOpenId + "'");
                sbSql.Append(",'" + strNickName + "'");
                sbSql.Append(",'" + strSex + "'");
                sbSql.Append(",'" + strProvince + "'");
                sbSql.Append(",'" + strCity + "'");
                sbSql.Append(",'" + strCountry + "'");
                sbSql.Append(",'" + strHeadImgUrl + "'");
                sbSql.Append(",'" + strPrivilege + "'");
                sbSql.Append(",'010'");////公众号注册登记的入口标记
                sbSql.Append(",'" + strNowTime + "'");
                sbSql.Append(",'" + strNowTime + "'");
                sbSql.Append(" )");
            }
            else
            {
                sbSql.Append("UPDATE [WXUser_1] SET [OpenId] = '" + strOpenId + "'");
                if (DicText.ContainsKey("nickname"))
                {
                    sbSql.Append(",[NickName] = '" + DicText["nickname"].ToString() + "'");
                }
                if (DicText.ContainsKey("sex"))
                {
                    sbSql.Append(",[Sex] = '" + DicText["sex"].ToString() + "'");
                }
                if (DicText.ContainsKey("province"))
                {
                    sbSql.Append(",[Province] = '" + DicText["province"].ToString() + "'");
                }
                if (DicText.ContainsKey("city"))
                {
                    sbSql.Append(",[City] = '" + DicText["city"].ToString() + "'");
                }
                if (DicText.ContainsKey("country"))
                {
                    sbSql.Append(",[Country] = '" + DicText["country"].ToString() + "'");
                }
                if (DicText.ContainsKey("headimgurl"))
                {
                    sbSql.Append(",[HeadImgUrl] = '" + DicText["headimgurl"].ToString() + "'");
                }
                if (DicText.ContainsKey("privilege"))
                {
                    sbSql.Append(",[Privilege] = '" + DicText["privilege"].ToString() + "'");
                }
                if (DicText.ContainsKey("unionid"))
                {
                    sbSql.Append(",[Unionid] = '" + DicText["unionid"].ToString() + "'");
                }
                //公众号注册登记的入口标记
                sbSql.Append(",[WXEntryType] = '010'");
                sbSql.Append(",[LastTime] = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' WHERE [OpenId] = '" + strOpenId + "'");

            }
            int iCount = SqlParamDao.ExecuteNonQueryBySql(sbSql.ToString());
            log.Error("根据Access_Token和Openid获取用户信息并写入数据库成功，sql:"+sbSql.ToString());

            context.Response.Write(iCount);
        }
        catch (Exception ex)
        {
            log.Error("根据Access_Token和Openid获取用户信息并写入数据库失败，sql:"+sbSql.ToString());
            log.Error(ex);
            context.Response.Write("-1");
        }
    }

    /// <summary>
    /// 根据OpenId判断是否有注册
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strOpenId"></param>
    private void JudgeIsRegByOpenId(HttpContext context,string strOpenId)
    {
        string IsReg = "0";
        StringBuilder sbResult = new StringBuilder();
        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select * from [WXUser_1] where [OpenId] = '"+strOpenId+"'");
            DataTable dtResultData = SqlParamDao.GetDataTableBySql(sbSql.ToString());
            if ((dtResultData != null) && (dtResultData.Rows.Count > 0))
            {
                IsReg = "1";
                sbResult.Append("{\"ResultData\":[{");
                sbResult.Append("\"openId\":\""+Microsoft.JScript.GlobalObject.escape(dtResultData.Rows[0]["OpenId"].ToString())+"\"");
                sbResult.Append(",\"nickName\":\""+Microsoft.JScript.GlobalObject.escape(dtResultData.Rows[0]["NickName"].ToString())+"\"");
                sbResult.Append(",\"gender\":\""+Microsoft.JScript.GlobalObject.escape(dtResultData.Rows[0]["Sex"].ToString())+"\"");
                sbResult.Append(",\"province\":\""+Microsoft.JScript.GlobalObject.escape(dtResultData.Rows[0]["Province"].ToString())+"\"");
                sbResult.Append(",\"city\":\""+Microsoft.JScript.GlobalObject.escape(dtResultData.Rows[0]["City"].ToString())+"\"");
                sbResult.Append(",\"country\":\""+Microsoft.JScript.GlobalObject.escape(dtResultData.Rows[0]["Country"].ToString())+"\"");
                sbResult.Append(",\"headImgUrl\":\""+Microsoft.JScript.GlobalObject.escape(dtResultData.Rows[0]["HeadImgUrl"].ToString())+"\"");
                sbResult.Append(",\"privilege\":\""+Microsoft.JScript.GlobalObject.escape(dtResultData.Rows[0]["Privilege"].ToString())+"\"");
                sbResult.Append(",\"unionid\":\""+Microsoft.JScript.GlobalObject.escape(dtResultData.Rows[0]["Unionid"].ToString())+"\"");
                sbResult.Append(",\"mobileNo\":\""+Microsoft.JScript.GlobalObject.escape(dtResultData.Rows[0]["MobileNo"].ToString())+"\"");
                sbResult.Append(",\"password\":\""+Microsoft.JScript.GlobalObject.escape(dtResultData.Rows[0]["Password"].ToString())+"\"");
                sbResult.Append(",\"smsCount\":\""+Microsoft.JScript.GlobalObject.escape(dtResultData.Rows[0]["SMSCount"].ToString())+"\"");
                sbResult.Append(",\"regTime\":\""+Microsoft.JScript.GlobalObject.escape(dtResultData.Rows[0]["RegTime"].ToString())+"\"");
                sbResult.Append(",\"lastTime\":\""+Microsoft.JScript.GlobalObject.escape(dtResultData.Rows[0]["LastTime"].ToString())+"\"");
                sbResult.Append("}]}");
                context.Response.Write(sbResult.ToString());
            }
            else
            {
                context.Response.Write("0");
            }

        }
        catch (Exception ex)
        {
            log.Error(ex);
            context.Response.Write("-1");
        }
        log.Error("微信授权-判断OpenId:"+strOpenId+"是否有注册，结果为："+IsReg.ToString());
        log.Error("微信授权-判断OpenId:"+strOpenId+"是否有注册，返回结果为："+sbResult.ToString());
    }

    /// <summary>
    /// 注册OpenId到业务系统
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strOpenId"></param>
    private void RegisterOpenId(HttpContext context,string strOpenId,string strMobileNo,string strPassword)
    {
        StringBuilder sbResult = new StringBuilder();
        try
        {
            String strNowTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("if not exists (select * from [WXUser_1] where [OpenId] = '"+strOpenId+"') \r\n");
            sbSql.Append("begin \r\n");
            sbSql.Append("  insert into [WXUser_1] ([OpenId],[MobileNo],[Password],[SMSCount],[RegTime],[LastTime]) values ( \r\n");
            sbSql.Append(" '"+strOpenId+"'");
            sbSql.Append(",'"+strMobileNo+"'");
            sbSql.Append(",'"+strPassword+"'");
            sbSql.Append(",'1'");
            sbSql.Append(",'"+strNowTime+"'");
            sbSql.Append(",'"+strNowTime+"'");
            sbSql.Append(" ) \r\n");
            sbSql.Append("end \r\n");
            sbSql.Append("else \r\n");
            sbSql.Append("begin \r\n");
            sbSql.Append("  UPDATE [WXUser_1] SET [MobileNo] = '"+strMobileNo+"',[Password] = '"+strPassword+"' ");
            sbSql.Append("  ,[SMSCount] = CONVERT(INT,isnull([SMSCount],0))+1,[LastTime] = '"+strNowTime+"' ");
            sbSql.Append("  where [OpenId] = '"+strOpenId+"' \r\n");
            sbSql.Append("end; \r\n");
            sbSql.Append("insert into WXUser_2 (OpenId,LNO,MobileNo,RegTime) \r\n");
            sbSql.Append("select OpenId,isnull((SELECT MAX(LNO)+1 FROM WXUser_2 WHERE OpenId = A.OpenId),1),'"+strMobileNo+"','"+strNowTime+"' from WXUser_1 A where [OpenId] = '"+strOpenId+"' \r\n");
            log.Error("微信授权-注册OpenId到业务系统,注册脚本："+sbSql.ToString());
            int iCount = SqlParamDao.ExecuteNonQueryBySql(sbSql.ToString());
            context.Response.Write(iCount);

        }
        catch (Exception ex)
        {
            log.Error(ex);
            context.Response.Write("-1");
        }
    }

    /// <summary>
    /// 根据OpenId解绑注册的手机号码
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strOpenId"></param>
    private void ClearMobileNo(HttpContext context,string strOpenId,string strMobileNo)
    {
        StringBuilder sbResult = new StringBuilder();
        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("UPDATE [WXUser_1] SET [MobileNo] = '' where [OpenId] = '"+strOpenId+"' \r\n");
            log.Error("微信授权-根据OpenId解绑注册的手机号码,脚本："+sbSql.ToString());
            int iCount = SqlParamDao.ExecuteNonQueryBySql(sbSql.ToString());


            //新增用户访问记录信息表的写入 add by sammen 20190919
            WXUser.RecordWXUser_3(strMobileNo,"","解绑注册的手机号码"+strMobileNo);
            //新增用户访问记录信息表的写入 add by sammen 20190919

            context.Response.Write(iCount);

        }
        catch (Exception ex)
        {
            log.Error(ex);
            context.Response.Write("-1");
        }
    }

    /// <summary>
    /// 判断手机号码是否已经注册
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strOpenId"></param>
    /// <param name="strMobileNo"></param>
    private void JudgeMobileNoIsRegisted(HttpContext context,string strOpenId,string strMobileNo)
    {
        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select count(1) from [WXUser_1] where [MobileNo] = '"+strMobileNo+"' and [OpenId] <> '"+strOpenId+"'");
            log.Error("微信授权-判断手机号码是否已经注册,注册脚本："+sbSql.ToString());
            int iCount = SqlParamDao.ExecuteScalarBySql(sbSql.ToString());
            context.Response.Write(iCount);

        }
        catch (Exception ex)
        {
            log.Error(ex);
            context.Response.Write("-1");
        }
    }

    #region Post/Get提交调用抓取
    /// <summary>
    /// Post/get 提交调用抓取
    /// </summary>
    /// <param name="url">提交地址</param>
    /// <param name="param">参数</param>
    /// <returns>string</returns>
    public static string WebRequestPostOrGet(string sUrl, string sParam)
    {
        byte[] bt = System.Text.Encoding.UTF8.GetBytes(sParam);

        Uri uriurl = new Uri(sUrl);
        HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(uriurl);//HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url + (url.IndexOf("?") > -1 ? "" : "?") + param);
        req.Method = "Post";
        req.Timeout = 120 * 1000;
        req.ContentType = "application/x-www-form-urlencoded;";
        req.ContentLength = bt.Length;

        using (Stream reqStream = req.GetRequestStream())//using 使用可以释放using段内的内存
        {
            reqStream.Write(bt, 0, bt.Length);
            reqStream.Flush();
        }
        try
        {
            using (WebResponse res = req.GetResponse())
            {
                //在这里对接收到的页面内容进行处理 
                Stream resStream = res.GetResponseStream();
                StreamReader resStreamReader = new StreamReader(resStream, System.Text.Encoding.UTF8);
                string resLine;

                System.Text.StringBuilder resStringBuilder = new System.Text.StringBuilder();
                while ((resLine = resStreamReader.ReadLine()) != null)
                {
                    resStringBuilder.Append(resLine + System.Environment.NewLine);
                }
                resStream.Close();
                resStreamReader.Close();
                return resStringBuilder.ToString();
            }
        }
        catch (Exception ex)
        {
            return ex.Message;//url错误时候回报错
        }
    }
    #endregion Post/Get提交调用抓取

    public bool IsReusable {
        get {
            return false;
        }
    }

}