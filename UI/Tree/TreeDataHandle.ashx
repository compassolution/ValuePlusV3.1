<%@ WebHandler Language="C#" Class="TreeDataHandle" %>

using System;
using System.Web;
using Com.ValuePlus.Archive.Tree;
using Com.ValuePlus.BLL.User;
using Com.ValuePlus.Common.Security;

public class TreeDataHandle : IHttpHandler, System.Web.SessionState.IRequiresSessionState
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
            if (context.Request.Params["TID"] != null)
            {
                String strTid = context.Request.Params["TID"].ToString();
                String strOpType = context.Request.Params["OPTYPE"].ToString();
	            //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
	            strTid = SQLInjectionDefense.ReplaceSQLReservedKeyword(strTid);
	            strOpType = SQLInjectionDefense.ReplaceSQLReservedKeyword(strOpType);

                BiuldTreeDataBll bllTree = new BiuldTreeDataBll();
                context.Response.Write(bllTree.GetTreeDataAll(strTid, strOpType,UserLoginBll.Language));
            }
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