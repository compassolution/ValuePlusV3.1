<%@ WebHandler Language="C#" Class="DeptTreeHandle" %>

using System;
using System.Web;
using Com.ValuePlus.BLL.SysManager;
using Com.ValuePlus.BLL.User;

public class DeptTreeHandle : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{

    #region 日志声明
    /// <summary>
    /// 日志声明
    /// </summary>
    private Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    #endregion


    public void ProcessRequest(HttpContext context)
    {
        context.Response.Clear();
        context.Response.Charset = "UTF-8";
        context.Response.ContentEncoding = System.Text.Encoding.UTF8;
        context.Response.ContentType = "text/xml";
        try
        {
            DeptManagerBll bllMenuManager = new DeptManagerBll();
            context.Response.Write(bllMenuManager.GetDeptTreeAll(UserLoginBll.Language));
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