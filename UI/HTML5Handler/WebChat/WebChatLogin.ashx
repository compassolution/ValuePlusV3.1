<%@ WebHandler Language="C#" Class="WebChatLogin" %>

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
using Com.ValuePlus.Common.Security;

/// <summary>
/// 旧版HRMobile微信公众号VP登录时使用，包括个人中心/微信工资条
/// </summary>
public class WebChatLogin : HandlerBase,IHttpHandler, System.Web.SessionState.IRequiresSessionState
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
        //context.Response.ContentType = "text/plain";
        //context.Response.Write("GetWeixin_AppId:"+Const.Weixin_URL_GetCodeUrl+";");
        //context.Response.Write("GetWeixin_Appsecret:"+GetSysParams.GetWeixin_Appsecret());

        Hashtable hsTableUrlQuery = this.GetUrlAnalyse(context);

        string param = context.Request["param"] == null ? string.Empty : context.Request["param"].ToString();//请求类型参数
        string strRedirectUrl = context.Request["weinxin_redirecturl"] == null ? string.Empty : context.Request["weinxin_redirecturl"].ToString();//需定位的URL
        string strLoginCode = context.Request["weinxin_code"] == null ? string.Empty : context.Request["weinxin_code"].ToString();//登录返回的Code
        string strOpenId = context.Request["weinxin_openid"] == null ? string.Empty : context.Request["weinxin_openid"].ToString();//登录用户的OpenID
        string strAccesstoken = context.Request["weinxin_accesstoken"] == null ? string.Empty : context.Request["weinxin_accesstoken"].ToString();//登录用户的weinxin_accesstoken
        string strMobileNo = context.Request["weinxin_mobileno"] == null ? string.Empty : context.Request["weinxin_mobileno"].ToString();//登录用户的OpenID
        string strPassword = context.Request["weinxin_password"] == null ? string.Empty : context.Request["weinxin_password"].ToString();//登录用户的OpenID
        
        //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
        param = SQLInjectionDefense.ReplaceSQLReservedKeyword(param);
        strRedirectUrl = SQLInjectionDefense.ReplaceSQLReservedKeyword(strRedirectUrl);
        strLoginCode = SQLInjectionDefense.ReplaceSQLReservedKeyword(strLoginCode);
        strOpenId = SQLInjectionDefense.ReplaceSQLReservedKeyword(strOpenId);
        strAccesstoken = SQLInjectionDefense.ReplaceSQLReservedKeyword(strAccesstoken);
        strMobileNo = SQLInjectionDefense.ReplaceSQLReservedKeyword(strMobileNo);
        strPassword = SQLInjectionDefense.ReplaceSQLReservedKeyword(strPassword);
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
        string strReturnURL = string.Format(Const.Weixin_URL_GetCodeUrl, Appid, redirect_uri);
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
                sbResult.Append("\"OpenId\":\""+Microsoft.JScript.GlobalObject.escape(strOpenID)+"\"");
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
                log.Error("微信授权-刷新Token后，access_token:"+strAccess_Token);
            }
            sbResult.Append(",\"Access_token\":\""+Microsoft.JScript.GlobalObject.escape(strAccess_Token)+"\"");
        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
        sbResult.Append("}]}");
        log.Error("微信授权-用Code("+Code+")获取Openid:"+strOpenID+",access_token:"+strAccess_Token);
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
            sbSql.Append("UPDATE [WXUser_1] SET [OpenId] = '"+strOpenId+"'");
            string url = string.Format(Const.Weixin_URL_GetUserInfo, strAccesstoken, strOpenId);
            string ReText = WebRequestPostOrGet(url,"");//post/get方法获取信息 
            JavaScriptSerializer myJson = new JavaScriptSerializer();
            Dictionary<string, object> DicText = (Dictionary<string, object>)myJson.DeserializeObject(ReText);
            if (DicText.ContainsKey("nickname"))
            {
                sbSql.Append(",[NickName] = '"+DicText["nickname"].ToString()+"'");
            }
            if (DicText.ContainsKey("sex"))
            {
                sbSql.Append(",[Sex] = '"+DicText["sex"].ToString()+"'");
            }
            if (DicText.ContainsKey("province"))
            {
                sbSql.Append(",[Province] = '"+DicText["province"].ToString()+"'");
            }
            if (DicText.ContainsKey("city"))
            {
                sbSql.Append(",[City] = '"+DicText["city"].ToString()+"'");
            }
            if (DicText.ContainsKey("country"))
            {
                sbSql.Append(",[Country] = '"+DicText["country"].ToString()+"'");
            }
            if (DicText.ContainsKey("headimgurl"))
            {
                sbSql.Append(",[HeadImgUrl] = '"+DicText["headimgurl"].ToString()+"'");
            }
            if (DicText.ContainsKey("privilege"))
            {
                sbSql.Append(",[Privilege] = '"+DicText["privilege"].ToString()+"'");
            }
            if (DicText.ContainsKey("unionid"))
            {
                sbSql.Append(",[Unionid] = '"+DicText["unionid"].ToString()+"'");
            }
            //公众号注册登记的入口标记
            sbSql.Append(",[WXEntryType] = '010'");
            sbSql.Append(",[LastTime] = '"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"' WHERE [OpenId] = '"+strOpenId+"'");
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
                sbResult.Append("\"OpenId\":\""+Microsoft.JScript.GlobalObject.escape(dtResultData.Rows[0]["OpenId"].ToString())+"\"");
                sbResult.Append(",\"NickName\":\""+Microsoft.JScript.GlobalObject.escape(dtResultData.Rows[0]["NickName"].ToString())+"\"");
                sbResult.Append(",\"Sex\":\""+Microsoft.JScript.GlobalObject.escape(dtResultData.Rows[0]["Sex"].ToString())+"\"");
                sbResult.Append(",\"Province\":\""+Microsoft.JScript.GlobalObject.escape(dtResultData.Rows[0]["Province"].ToString())+"\"");
                sbResult.Append(",\"City\":\""+Microsoft.JScript.GlobalObject.escape(dtResultData.Rows[0]["City"].ToString())+"\"");
                sbResult.Append(",\"Country\":\""+Microsoft.JScript.GlobalObject.escape(dtResultData.Rows[0]["Country"].ToString())+"\"");
                sbResult.Append(",\"HeadImgUrl\":\""+Microsoft.JScript.GlobalObject.escape(dtResultData.Rows[0]["HeadImgUrl"].ToString())+"\"");
                sbResult.Append(",\"Privilege\":\""+Microsoft.JScript.GlobalObject.escape(dtResultData.Rows[0]["Privilege"].ToString())+"\"");
                sbResult.Append(",\"Unionid\":\""+Microsoft.JScript.GlobalObject.escape(dtResultData.Rows[0]["Unionid"].ToString())+"\"");
                sbResult.Append(",\"MobileNo\":\""+Microsoft.JScript.GlobalObject.escape(dtResultData.Rows[0]["MobileNo"].ToString())+"\"");
                sbResult.Append(",\"Password\":\""+Microsoft.JScript.GlobalObject.escape(dtResultData.Rows[0]["Password"].ToString())+"\"");
                sbResult.Append(",\"SMSCount\":\""+Microsoft.JScript.GlobalObject.escape(dtResultData.Rows[0]["SMSCount"].ToString())+"\"");
                sbResult.Append(",\"RegTime\":\""+Microsoft.JScript.GlobalObject.escape(dtResultData.Rows[0]["RegTime"].ToString())+"\"");
                sbResult.Append(",\"LastTime\":\""+Microsoft.JScript.GlobalObject.escape(dtResultData.Rows[0]["LastTime"].ToString())+"\"");
                sbResult.Append("}]}");
                context.Response.Write(sbResult.ToString());
            }
            else
            {
                context.Response.Write("0");
            }

            string json = WebCommon.GetDataRowJsonString(dtResultData, "ResultData", "");

        }
        catch (Exception ex)
        {
            log.Error(ex);
            context.Response.Write("0");
        }
        log.Error("微信授权-判断OpenId:"+strOpenId+"是否有注册，结果为："+IsReg.ToString());
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
            sbSql.Append("insert into [WXUser_1] ([OpenId],[MobileNo],[Password],[SMSCount],[RegTime],[LastTime]) values (");
            sbSql.Append(" '"+strOpenId+"'");
            sbSql.Append(",'"+strMobileNo+"'");
            sbSql.Append(",'"+strPassword+"'");
            sbSql.Append(",'1'");
            sbSql.Append(",'"+strNowTime+"'");
            sbSql.Append(",'"+strNowTime+"'");
            sbSql.Append(" )");
            sbSql.Append("end \r\n");
            sbSql.Append("else \r\n");
            sbSql.Append("begin \r\n");
            sbSql.Append(" UPDATE [WXUser_1] SET [MobileNo] = '"+strMobileNo+"',[Password] = '"+strPassword+"' ,[SMSCount] = CONVERT(INT,[SMSCount])+1,[LastTime] = '"+strNowTime+"' where [OpenId] = '"+strOpenId+"' \r\n");
            sbSql.Append("end; \r\n");
            sbSql.Append(" insert into WXUser_2 (OpenId,LNO,MobileNo,RegTime) \r\n");
            sbSql.Append(" select OpenId,isnull((SELECT MAX(LNO)+1 FROM WXUser_2 WHERE OpenId = A.OpenId),1),'"+strMobileNo+"','"+strNowTime+"' from WXUser_1 A where [OpenId] = '"+strOpenId+"' \r\n");
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