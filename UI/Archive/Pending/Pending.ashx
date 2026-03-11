<%@ WebHandler Language="C#" Class="Pending" %>

using System;
using System.Web;
using System.Text;
using System.Data;
using System.IO;
using System.Collections;
using Com.ValuePlus.Web;
using Com.ValuePlus.Archive.BLL;

public class Pending : HandlerBase, IHttpHandler, System.Web.SessionState.IRequiresSessionState {

    public void ProcessRequest (HttpContext context) {
        Hashtable hsTableUrlQuery = this.GetUrlAnalyse(context);
        string strParam = hsTableUrlQuery["param"] == null ? string.Empty : hsTableUrlQuery["param"].ToString();//param
        String strCurUserId = this.GetUserCode();

        if (strParam.Equals("getcuruserpendingcount"))
        {
            this.GetCurUserPendingCount(context,strCurUserId);
        }
    }

    /// <summary>
    /// 即时获取当前用户的待办事项条数
    /// </summary>
    /// <returns></returns>
    private void GetCurUserPendingCount(HttpContext context,String strCurUserId)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbReturnData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "即时获取当前用户的待办事项条数";

        try
        {
            int iCount = ArchiveAlertBll.GetAlertDetailCount(strCurUserId, "zh-cn", false);
            sbReturnData.Append("{\"UserId\":\"" + strCurUserId + "\",\"iPendingCount\":\"" + iCount.ToString() + "\"}");

            strReturnCode = "1";
            strReturnMsg = strMethodDesc + "成功";
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = strMethodDesc + "出错";
            log.Error(ex);
        }
        finally
        {
            sbResult.Append("{");
            sbResult.Append("\"ReturnCode\":\"" + strReturnCode.ToString() + "\"");
            sbResult.Append(",\"ReturnMsg\":\"" + strReturnMsg.ToString() + "\"");
            if (!String.IsNullOrEmpty(sbReturnData.ToString()))
            {
                sbResult.Append(",\"ReturnData\":" + sbReturnData.ToString());
            }
            sbResult.Append("}");
            //log.Error(strMethodDesc + "Return Json:" + sbResult.ToString());
        }
        context.Response.Write(sbResult.ToString());
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}