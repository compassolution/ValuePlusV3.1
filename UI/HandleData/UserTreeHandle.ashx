<%@ WebHandler Language="C#" Class="UserTreeHandle" %>

using System;
using System.Web;
using System.Web.UI;
using Com.ValuePlus.BLL.User;
using Com.ValuePlus.Entity;
using Com.ValuePlus.Common.Security;

public class UserTreeHandle : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    #region 日志声明
    /// <summary>
    /// 日志声明
    /// </summary>
    private Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    #endregion
    
    public void ProcessRequest (HttpContext context) {
        context.Response.Clear();
        context.Response.Charset = "UTF-8";
        context.Response.ContentEncoding = System.Text.Encoding.UTF8;
        context.Response.ContentType = "text/xml";
        try
        {
            String strMenuCode = context.Request.QueryString["menuId"].ToString();
            //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
            strMenuCode = SQLInjectionDefense.ReplaceSQLReservedKeyword(strMenuCode);
            UserBll userBll = new UserBll();

            //判断是否已经读取过用户菜单【暂时不用缓存】
            //if (Com.ValuePlus.Utils.Session.SessionHelper.GetSession("dtLoginUserMenu") == null)
            //{
                UserInfo userInfo;
                bool isOnline = UserLoginBll.AuthLoginUser(out userInfo);
                if (isOnline)
                {
                    if (Com.ValuePlus.BLL.User.UserLoginBll.IsAdminstratorUser())
                    {
                        context.Response.Write(userBll.GetUserTreeAdmin(UserLoginBll.Language, strMenuCode));
                    }
                    else
                    {
                        context.Response.Write(userBll.GetUserTreeNotAdmin(userInfo.SUSERID, strMenuCode, UserLoginBll.Language));
                    }

                }
                else
                {
                    context.Response.Write(userBll.GetUserTreeNotLogin());
                }
                
            //}
            //else
            //{
            //    System.Data.DataTable dt = (System.Data.DataTable)Com.ValuePlus.Utils.Session.SessionHelper.GetSession("dtLoginUserMenu");
            //    String sResult = string.Empty;
            //    userBll.CreateTreeXmlStructure(dt, strMenuCode, UserLoginBll.Language, ref sResult);
            //    context.Response.Write(sResult);
            //}
        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
        context.Response.Flush();
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}