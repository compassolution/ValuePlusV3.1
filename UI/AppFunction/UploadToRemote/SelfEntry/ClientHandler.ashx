<%@ WebHandler Language="C#" Class="ClientHandler" %>

using System;
using System.Web;
using System.Text;
using System.Data;
using System.IO;
using System.Collections;
using Com.ValuePlus.Web;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Weixin;
using Com.ValuePlus.Utils.Serializable;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

public class ClientHandler : HandlerBase, IHttpHandler, System.Web.SessionState.IRequiresSessionState {

    public void ProcessRequest (HttpContext context) {
        Hashtable hsTableUrlQuery = this.GetUrlAnalyse(context);
        string strParam = hsTableUrlQuery["param"] == null ? string.Empty : hsTableUrlQuery["param"].ToString();//param

        if (strParam.Equals("getstaffdcidlist"))
        {
            //获取系统已入职的员工身份证号
            context.Response.Write(this.GetStaffDCIDList());
        }
    }

    /// <summary>
    /// 获取系统已入职的员工身份证号
    /// </summary>
    /// <param name="strProjectId"></param>
    private String GetStaffDCIDList()
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "获取系统已入职的员工身份证号";
        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select DCID from HRDOCU_1 ");
            String strSql = sbSql.ToString();

            sbResultData.Append("\"ResultData\":[");
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
            for(int i=0;i<dt.Rows.Count;i++){
                String strDCID = dt.Rows[i]["DCID"].ToString();
                sbResultData.Append((i==0?"":",")+"\""+strDCID+"\"");

            }
            sbResultData.Append("]");

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
        return sbResult.ToString();
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}