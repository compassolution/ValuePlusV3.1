using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using Com.ValuePlus.Weixin;

/// <summary>
/// WXAccessToken 的摘要说明
/// 微信公众号获取Access_Token的接口
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
// [System.Web.Script.Services.ScriptService]
public class WXAccessToken : System.Web.Services.WebService
{
    /// <summary>
    /// 日志声明
    /// </summary>
    protected static Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    public WXAccessToken()
    {

        //如果使用设计的组件，请取消注释以下行 
        //InitializeComponent(); 
    }


    /// <summary>
    /// 获取最新的有效的AccessToken
    /// </summary>
    /// <returns></returns>
    [WebMethod]
    public String GetAccessToken(String strAppId,String strAppSecret,String strSourceType)
    {
        String strNowTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        return AccessTokenGetter.GetValidAccessToken(strAppId, strAppSecret, strNowTime, strSourceType);
    }



    [WebMethod]
    public string HelloWorld()
    {
        return "Hello World";
    }

}
