<%@ WebHandler Language="C#" Class="ServerHandler" %>

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

public class ServerHandler : HandlerBase, IHttpHandler, System.Web.SessionState.IRequiresSessionState {

    public void ProcessRequest (HttpContext context) {
        //跨域提交表单，前端ajax不用做任何修改
        context.Response.AddHeader("Access-Control-Allow-Origin", "*");//支持全域名访问，不安全，部署后需要固定限制为客户端网址

        Hashtable hsTableUrlQuery = this.GetUrlAnalyse(context);
        string strCallback = hsTableUrlQuery["callback"] == null ? string.Empty : hsTableUrlQuery["callback"].ToString();//
        string strParam = hsTableUrlQuery["param"] == null ? string.Empty : hsTableUrlQuery["param"].ToString();//param
        string strProjectId = hsTableUrlQuery["projectid"] == null ? string.Empty : hsTableUrlQuery["projectid"].ToString();//

        if (strParam.Equals("getselfentryinputdata"))
        {
            //获取某ProjectId的自助录入信息数据
            context.Response.Write(this.GetSelfEntryInputData(strProjectId));
        }

    }


    /// <summary>
    /// 获取某ProjectId的自助录入信息数据
    /// </summary>
    /// <param name="strProjectId"></param>
    private String GetSelfEntryInputData(String strProjectId)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "获取某ProjectId的自助录入信息数据";
        try
        {
            //TB_Remote_SelfInputData是每个字段数据是一行数据，所以需先将行数据转化成录入字段为列的数据，语句范例如下
            //select ProjectID,BusinessType,MobileNo,
            // max(case PID when 'DCNO' then PVALUE else '' end) as DCNO,
            // max(case PID when 'DCID' then PVALUE else '' end) as DCID
            //from TB_Remote_SelfInputData group by ProjectID,BusinessType,MobileNo

            StringBuilder sbSql_TMPD = new StringBuilder();
            sbSql_TMPD.Append("select * from TB_Remote_HRTMPD where ProjectId = '"+strProjectId+"' ");
            sbSql_TMPD.Append(" and BusinessType = 'SelfEntry' and TID = 'OANEW' AND GID = '1' ORDER BY PORDER");
            String strSql_TMPD = sbSql_TMPD.ToString();
            log.Error(strMethodDesc+"SQL:"+strSql_TMPD);

            //sbResultData.Append("\"ResultData\":[");
            DataTable dt_TMPD = SqlParamDao.GetDataTableBySql(strSql_TMPD);
            DataTable dt_InputData = new DataTable();
            if(dt_TMPD!=null&& dt_TMPD.Rows.Count>0){
                StringBuilder sbSql_InputData = new StringBuilder();
                sbSql_InputData.Append("select (ProjectId+BusinessType+MobileNo+TID+GID) as RowKey \r\n");
                for(int i=0;i<dt_TMPD.Rows.Count;i++){
                    String strPID = dt_TMPD.Rows[i]["PID"].ToString();
                    String strPDESC = dt_TMPD.Rows[i]["PDESCCHS"].ToString();
                    String strPCTRL = dt_TMPD.Rows[i]["PCTRL"].ToString();
                    String strPCTRLID = dt_TMPD.Rows[i]["PCTRLID"].ToString();
                    if (!strPCTRL.Equals("1"))
                    {
                        sbSql_InputData.Append(",max(case PID when '"+strPID+"' then PVALUE else '' end) as ["+strPID+"]  \r\n");
                    }
                    else
                    {
                        sbSql_InputData.Append(",max(case PID when '"+strPID+"' then PVALUE else '' end) + '-' ");
                        sbSql_InputData.Append(" +ISNULL((select CDESCCHS FROM TB_Remote_HRLSTD WHERE ProjectId = '"+strProjectId+"' and LID = '"+strPCTRLID+"' ");
                        sbSql_InputData.Append(" AND CID = (max(case PID when '"+strPID+"' then PVALUE else '' end)) ");
                        sbSql_InputData.Append(" ),'') as ["+strPID+"] \r\n");
                    }

                }
                //增加显示录入时间字段列
                sbSql_InputData.Append(",max(InputTime) as [InputTime]  \r\n");
                sbSql_InputData.Append(" from TB_Remote_SelfInputData where ProjectId = '"+strProjectId+"' and BusinessType = 'SelfEntry' ");
                sbSql_InputData.Append(" group by ProjectId,BusinessType,MobileNo,TID,GID");
                sbSql_InputData.Append(" order by max(InputTime) desc");
                dt_InputData = SqlParamDao.GetDataTableBySql(sbSql_InputData.ToString());

            }

            //sbResultData.Append("]");

            sbResultData.Append(WebCommon.GetJsonStringByDataTable(dt_InputData, "\"ResultData\"", true));
            sbResultData.Append(",");
            sbResultData.Append(WebCommon.GetJsonStringByDataTable(dt_TMPD, "\"ColumnsData\"", true));

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