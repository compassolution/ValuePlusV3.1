using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using Com.ValuePlus.Common.Security;
//using Com.ValuePlus.Weixin;

public partial class NetData_Test : System.Web.UI.Page
{
    const string token = "WeiXinNetData";//定义一个局部变量不可以被修改，这里定义的变量要与接口配置信息中填写的token一致

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(token))
        {
            Valid();

            string strFilePath = System.Web.HttpContext.Current.Server.MapPath("TokenFile.xml");
            //String strToken = PublicApi.GetAccess_Token("wx6f2c4b315e434172", "411176cdcbc6be49a2e8c9302aa36e3c", strFilePath);

        }
    }

    private void Valid()
    {
        if (Request.QueryString["echoStr"] != null)
        {
            string echoStr = Request.QueryString["echoStr"].ToString();
            //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
            echoStr = SQLInjectionDefense.ReplaceSQLReservedKeyword(echoStr);
            if (CheckSignature())
            {
                if (!string.IsNullOrEmpty(echoStr))
                {
                    Response.Write(echoStr);
                    Response.End();
                }
            }
        }
    }


    /// <summary>
    /// 验证微信签名
    /// </summary>
    /// <returns></returns>
    private bool CheckSignature()
    {
        string signature = Request.QueryString["signature"].ToString();
        string timestamp = Request.QueryString["timestamp"].ToString();
        string nonce = Request.QueryString["nonce"].ToString();

        //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
        signature = SQLInjectionDefense.ReplaceSQLReservedKeyword(signature);
        timestamp = SQLInjectionDefense.ReplaceSQLReservedKeyword(timestamp);
        nonce = SQLInjectionDefense.ReplaceSQLReservedKeyword(nonce);

        string[] ArrTmp = { token, timestamp, nonce };
        Array.Sort(ArrTmp);//字典排序
        string tmpStr = string.Join("", ArrTmp);
        tmpStr = FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");//对该字符串进行sha1加密
        tmpStr = tmpStr.ToLower();//对字符串中的字母部分进行小写转换，非字母字符不作处理

        //开发者获得加密后的字符串可与signature对比，标识该请求来源于微信。
        //开发者通过检验signature对请求进行校验，若确认此次GET请求来自微信服务器，请原样返回echostr参数内容，则接入生效，否则接入失败
        if (tmpStr == signature)
        {
            return true;
        }
        else
        {
            return false;
        }
    }   
}