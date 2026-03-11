<%@ WebHandler Language="C#" Class="ServerHandler" %>

using System;
using System.Web;
using System.Text;
using System.Data;
using System.IO;
using System.Collections;
using Com.ValuePlus.Web;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Utils.Serializable;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Com.ValuePlus.Weixin;
using Com.ValuePlus.Common.Security;

public class ServerHandler : HandlerBase, IHttpHandler, System.Web.SessionState.IRequiresSessionState {

    public void ProcessRequest (HttpContext context) {
        //跨域提交表单，前端ajax不用做任何修改
        context.Response.AddHeader("Access-Control-Allow-Origin", "*");//支持全域名访问，不安全，部署后需要固定限制为客户端网址

        Hashtable hsTableUrlQuery = this.GetUrlAnalyse(context);
        string strCallback = hsTableUrlQuery["callback"] == null ? string.Empty : hsTableUrlQuery["callback"].ToString();//param
        string strParam = hsTableUrlQuery["param"] == null ? string.Empty : hsTableUrlQuery["param"].ToString();//param
        string strProjectId = hsTableUrlQuery["projectid"] == null ? string.Empty : hsTableUrlQuery["projectid"].ToString();//param
        string strIsOnJob = hsTableUrlQuery["isonjob"] == null ? string.Empty : hsTableUrlQuery["isonjob"].ToString();//param
        string strIsDimission = hsTableUrlQuery["isdimission"] == null ? string.Empty : hsTableUrlQuery["isdimission"].ToString();//param
        string strIsConfirmed = hsTableUrlQuery["isconfirmed"] == null ? string.Empty : hsTableUrlQuery["isconfirmed"].ToString();//param
        string strIsNoConfirm = hsTableUrlQuery["isnoconfirm"] == null ? string.Empty : hsTableUrlQuery["isnoconfirm"].ToString();//param

        if (strParam.Equals("gethandbookconfirmlist"))
        {
            GetHandbookConfirmList(context,strProjectId,strIsOnJob,strIsDimission,strIsConfirmed,strIsNoConfirm);
        }
    }


    /// <summary>
    /// 从远程服务器上获取员工手册确认信息列表
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strProjectId"></param>
    /// <param name="strIsOnJob"></param>
    /// <param name="strIsDimission"></param>
    /// <param name="strIsConfirmed"></param>
    /// <param name="strIsNoConfirm"></param>
    private void GetHandbookConfirmList(HttpContext context,String strProjectId,String strIsOnJob,String strIsDimission,String strIsConfirmed,String strIsNoConfirm)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "从远程服务器上获取员工手册确认信息列表";
        try
        {
            log.Error("从远程服务器上获取员工手册确认信息列表,strProjectId:"+strProjectId);
            StringBuilder sbWhereStatus = new StringBuilder();
            sbWhereStatus.Append(" AND ( 1=0 ");
            if(strIsOnJob.Equals("1")){
                sbWhereStatus.Append(" OR A.DCSTATUS IN ('1','2')");
            }
            if (strIsDimission.Equals("1"))
            {
                sbWhereStatus.Append(" OR A.DCSTATUS IN ('3')");
            }
            sbWhereStatus.Append(" )");

            StringBuilder sbWhereConfirmed = new StringBuilder();
            sbWhereConfirmed.Append(" AND ( 1=0 ");
            if(strIsConfirmed.Equals("1")){
                sbWhereConfirmed.Append(" OR ISNULL(B.OpTime,'') <> ''");
            }
            if(strIsNoConfirm.Equals("1")){
                sbWhereConfirmed.Append(" OR ISNULL(B.OpTime,'') = ''");
            }
            sbWhereConfirmed.Append(" )");

            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("SELECT A.ProjectId,A.DCNO,A.DCNAME,A.DCNAMECHS,A.DeptName,A.DeptNameChs,A.PosiName,A.PosiNameChs,A.DCPLEVEL,A.DCJOIN,A.DCMOBILE,A.StatusName,B.OpUser,B.OpTime \r\n");
            sbSql.Append(" FROM TB_Remote_StaffBasicData A LEFT JOIN TB_Remote_HandBookConfirmData B ON A.ProjectId = B.ProjectId AND A.DCMOBILE = B.DCMOBILE  \r\n");
            sbSql.Append(" WHERE A.ProjectId = '"+strProjectId+"'  \r\n");
            sbSql.Append(" "+sbWhereStatus.ToString()+"\r\n");
            sbSql.Append(" "+sbWhereConfirmed.ToString()+"\r\n");
            sbSql.Append(" ORDER BY OpTime desc,DCNO \r\n");
            String strSql = sbSql.ToString();
            log.Error(strMethodDesc + "SQL:" + strSql);

            log.Error(strMethodDesc+"Return Json:"+sbResult.ToString());
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
            sbResultData.Append(WebCommon.GetJsonStringByDataTable(dt,"\"ResultData\"",true));

            strReturnCode = "1";
            strReturnMsg = strMethodDesc+"成功";
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = strMethodDesc+"出错";
            log.Error(ex);
        }
        finally{
            StringBuilder sbResultStatus = new StringBuilder();
            sbResultStatus.Append("\"ReturnStatus\":{");
            sbResultStatus.Append("\"ReturnCode\":\""+strReturnCode.ToString()+"\"");
            sbResultStatus.Append(",\"ReturnMsg\":\""+strReturnMsg.ToString()+"\"");
            sbResultStatus.Append("}");

            sbResult.Append("{");
            sbResult.Append(sbResultStatus.ToString());
            if (!String.IsNullOrEmpty(sbResultData.ToString()))
            {
                sbResult.Append(","+sbResultData.ToString());
            }
            sbResult.Append("}");
            log.Error(strMethodDesc+"Return Json:"+sbResult.ToString());
        }
        context.Response.Write( sbResult.ToString());

    }


    public bool IsReusable {
        get {
            return false;
        }
    }

}