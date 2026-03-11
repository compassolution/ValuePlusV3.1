<%@ WebHandler Language="C#" Class="OANEW" %>

using System;
using System.Web;
using System.Collections;
using System.Text;
using System.Data;
using Com.ValuePlus.BLL.SysManager;
using Com.ValuePlus.BLL.User;
using Com.ValuePlus.DataLog;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Entity;
using Com.ValuePlus.Web;
using Com.ValuePlus.Archive.Utils;
using Com.ValuePlus.Common.Security;

public class OANEW : HandlerBase, IHttpHandler, System.Web.SessionState.IRequiresSessionState
{

    #region 日志声明
    /// <summary>
    /// 日志声明
    /// </summary>
    private Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    #endregion

    public void ProcessRequest(HttpContext context)
    {

        Hashtable hsTableUrlQuery = this.GetUrlAnalyse(context);
        string strCondition = hsTableUrlQuery["condition"] == null ? string.Empty : hsTableUrlQuery["condition"].ToString();//查询条件

        string param = context.Request["param"] == null ? string.Empty : context.Request["param"].ToString();//请求类型参数
        string strRequestLanguage = context.Request["language"] == null ? string.Empty : context.Request["language"].ToString();//终端请求时的语言
        string strRequestProjectId = context.Request["projectid"] == null ? string.Empty : context.Request["projectid"].ToString();//请求类型参数
        string strRequestMobile = context.Request["mobile"] == null ? string.Empty : context.Request["mobile"].ToString();//请求类型参数
        string strRequestPost = context.Request["post"] == null ? string.Empty : context.Request["post"].ToString();//终端请求时的语言
        string strQueryDCNO = context.Request["dcno"] == null ? string.Empty : context.Request["dcno"].ToString();//请求类型参数
        string strQueryStatus = context.Request["dcno"] == null ? string.Empty : context.Request["dcno"].ToString();//请求类型参数
        
        //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
        param = SQLInjectionDefense.ReplaceSQLReservedKeyword(param);
        strRequestLanguage = SQLInjectionDefense.ReplaceSQLReservedKeyword(strRequestLanguage);
        strRequestProjectId = SQLInjectionDefense.ReplaceSQLReservedKeyword(strRequestProjectId);
        strRequestMobile = SQLInjectionDefense.ReplaceSQLReservedKeyword(strRequestMobile);
        strRequestPost = SQLInjectionDefense.ReplaceSQLReservedKeyword(strRequestPost);
        strQueryDCNO = SQLInjectionDefense.ReplaceSQLReservedKeyword(strQueryDCNO);
        strQueryStatus = SQLInjectionDefense.ReplaceSQLReservedKeyword(strQueryStatus);

        //switch (param.ToLower().ToString())
        //{
        //case "pendinglist":
        //    this.GetPendingList(context, strRequestProjectId, strRequestMobile,strRequestPost, strRequestLanguage);
        //    break;
        //case "checkedlist":
        //    this.GetCheckedList(context, strRequestProjectId, strRequestMobile,strRequestPost, strRequestLanguage);
        //    break;
        //case "finishedlist":
        //    this.GetFinishedList(context, strRequestProjectId, strRequestMobile,strRequestPost, strRequestLanguage);
        //    break;
        //default:
        //    this.GetOEInfo(context, iPageSize, iPageIndex, strRequestLanguage, strQueryLocation, strQueryDept, strQueryOECode, strQueryOEName);
        //break;
        //}
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}