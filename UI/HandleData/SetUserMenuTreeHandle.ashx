<%@ WebHandler Language="C#" Class="SetUserMenuTreeHandle" %>

using System;
using System.Web;
using Com.ValuePlus.BLL.SysManager;
using Com.ValuePlus.BLL.User;
using Com.ValuePlus.Common.Security;

public class SetUserMenuTreeHandle : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{

    #region 日志声明
    /// <summary>
    /// 日志声明
    /// </summary>
    private Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    #endregion

    public void ProcessRequest(HttpContext context)
    {
        String strUserId = context.Request.Params["userId"].ToString();//获取传进的系统用户ID
        //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
        strUserId = SQLInjectionDefense.ReplaceSQLReservedKeyword(strUserId);
        context.Response.Clear();
        context.Response.Charset = "UTF-8";
        context.Response.ContentEncoding = System.Text.Encoding.UTF8;
        context.Response.ContentType = "text/xml";
        try
        {
            UserManagerBll userMenuManager = new UserManagerBll();
            context.Response.Write(userMenuManager.GetMenuTreeAll(UserLoginBll.Language, strUserId));
        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
        context.Response.Flush();
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}