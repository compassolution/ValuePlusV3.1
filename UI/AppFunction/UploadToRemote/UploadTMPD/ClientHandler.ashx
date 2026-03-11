<%@ WebHandler Language="C#" Class="ClientHandler" %>

using System;
using System.Web;
using System.Text;
using System.Data;
using System.IO;
using System.Collections;
using Com.ValuePlus.Web;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Utils.Serializable;

public class ClientHandler : HandlerBase, IHttpHandler, System.Web.SessionState.IRequiresSessionState {

    public void ProcessRequest (HttpContext context) {
        Hashtable hsTableUrlQuery = this.GetUrlAnalyse(context);
        string strCallback = hsTableUrlQuery["callback"] == null ? string.Empty : hsTableUrlQuery["callback"].ToString();//param
        string strParam = hsTableUrlQuery["param"] == null ? string.Empty : hsTableUrlQuery["param"].ToString();//param
        string strMobileNo = hsTableUrlQuery["mobileno"] == null ? string.Empty : hsTableUrlQuery["mobileno"].ToString();//param
        string strProjectId = hsTableUrlQuery["projectid"] == null ? string.Empty : hsTableUrlQuery["projectid"].ToString();//param
        string strBusinessType = hsTableUrlQuery["businesstype"] == null ? string.Empty : hsTableUrlQuery["businesstype"].ToString();//param

        string strTIDRange = hsTableUrlQuery["tidrange"] == null ? string.Empty : hsTableUrlQuery["tidrange"].ToString();//需decodeURIComponent
        string strGIDRange = hsTableUrlQuery["gidrange"] == null ? string.Empty : hsTableUrlQuery["gidrange"].ToString();//需decodeURIComponent
        string strPIDRange = hsTableUrlQuery["pidrange"] == null ? string.Empty : hsTableUrlQuery["pidrange"].ToString();//需decodeURIComponent
        string strTID = hsTableUrlQuery["tid"] == null ? string.Empty : hsTableUrlQuery["tid"].ToString();//tid
        string strGID = hsTableUrlQuery["gid"] == null ? string.Empty : hsTableUrlQuery["gid"].ToString();//gid
        string strPID = hsTableUrlQuery["pid"] == null ? string.Empty : hsTableUrlQuery["pid"].ToString();//pid

        string strIsOnJob = hsTableUrlQuery["isonjob"] == null ? string.Empty : hsTableUrlQuery["isonjob"].ToString();//params


        log.Error("(AppFunction/UploadToRemote/UploadTMPD/ClientHandler.ashx)"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"---Param:"+strParam);
        if (strParam.Equals("gettmphdata"))
        {
            this.GetTMPHData(context,strTIDRange,strBusinessType);
        }
        else if (strParam.Equals("gettmpgdata"))
        {
            this.GetTMPGData(context,strTID,strGIDRange,strBusinessType);
        }
        else if (strParam.Equals("gettmpddata"))
        {
            this.GetTMPDData(context,strTID,strGID,strBusinessType);
        }
        else if (strParam.Equals("gettmpddndlstddata"))
        {
            this.GetTMPDAndLSTDData(context,strTID,strGID,strPIDRange);
        }
    }

    /// <summary>
    /// 通过TID范围获取模板档案的数据信息
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strTIDRange">范围，以逗号,隔开</param>
    /// <param name="strBusinessType"></param>
    private void GetTMPHData(HttpContext context,String strTIDRange,String strBusinessType)
    {
        try
        {
            StringBuilder sbSql = new StringBuilder();
            if (!String.IsNullOrEmpty(strTIDRange)){
                strTIDRange = Microsoft.JScript.GlobalObject.decodeURIComponent(strTIDRange);
                strTIDRange = strTIDRange.Replace("[", "").Replace("]", "").Replace("\"", "");
                if (!String.IsNullOrEmpty(strTIDRange))
                {
                    String strSqlInTID = "'" + strTIDRange.Replace(",", "','") + "'";
                    sbSql.Append("select * from TB_HRTMPH where TID in (" + strSqlInTID + ")");
                }else{
                    sbSql.Append("select * from TB_HRTMPH ORDER BY TORDER");
                }
            }else{
                sbSql.Append("select * from TB_HRTMPH ORDER BY TORDER");
            }
            DataTable dt = SqlParamDao.GetDataTableBySql(sbSql.ToString());
            StringBuilder sBuilder = new StringBuilder();
            sBuilder.Append("{");
            sBuilder.Append(WebCommon.GetJsonStringByDataTable(dt,"\"ResultData\"",false));
            sBuilder.Append("}");

            string json = sBuilder.ToString();

            context.Response.Write(json);

        }
        catch (Exception ex)
        {
            log.Error(ex);
            context.Response.Write("-1");
        }
    }

    /// <summary>
    /// 通过GID范围获取特定模板分组的数据信息
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strGIDRange">范围，以逗号,隔开</param>
    /// <param name="strBusinessType"></param>
    private void GetTMPGData(HttpContext context,String strTID,String strGIDRange,String strBusinessType)
    {
        try
        {
            StringBuilder sbSql = new StringBuilder();
            if (!String.IsNullOrEmpty(strGIDRange)){
                strGIDRange = Microsoft.JScript.GlobalObject.decodeURIComponent(strGIDRange);
                strGIDRange = strGIDRange.Replace("[", "").Replace("]", "").Replace("\"", "");
                if (!String.IsNullOrEmpty(strGIDRange))
                {
                    String strSqlInGID = "'" + strGIDRange.Replace(",", "','") + "'";
                    sbSql.Append("select * from TB_HRTMPG where TID = '"+strTID+"' AND GID in (" + strSqlInGID + ")");
                }else{
                    sbSql.Append("select * from TB_HRTMPG where TID = '"+strTID+"' ORDER BY GORDER");
                }
            }else{
                sbSql.Append("select * from TB_HRTMPG where TID = '"+strTID+"' ORDER BY GORDER");
            }
            DataTable dt = SqlParamDao.GetDataTableBySql(sbSql.ToString());
            StringBuilder sBuilder = new StringBuilder();
            sBuilder.Append("{");
            sBuilder.Append(WebCommon.GetJsonStringByDataTable(dt,"\"ResultData\"",false));
            sBuilder.Append("}");

            string json = sBuilder.ToString();

            context.Response.Write(json);

        }
        catch (Exception ex)
        {
            log.Error(ex);
            context.Response.Write("-1");
        }
    }

    /// <summary>
    /// 通过TID和PID获取对应字段属性的数据信息
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strTID"></param>
    /// <param name="strGID"></param>
    /// <param name="strBusinessType"></param>
    private void GetTMPDData(HttpContext context,String strTID,String strGID,String strBusinessType)
    {
        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select * from TB_HRTMPD where TID = '"+strTID+"' AND GID = '"+strGID+"'");
            if(strBusinessType.Equals("SelfEntry")){
                sbSql.Append(" and PTYPE NOT IN ('CS','CH')");
            }
            sbSql.Append("  ORDER BY PORDER");
            DataTable dt = SqlParamDao.GetDataTableBySql(sbSql.ToString());
            StringBuilder sBuilder = new StringBuilder();
            sBuilder.Append("{");
            sBuilder.Append(WebCommon.GetJsonStringByDataTable(dt,"\"ResultData\"",false));
            sBuilder.Append("}");

            string json = sBuilder.ToString();

            context.Response.Write(json);

        }
        catch (Exception ex)
        {
            log.Error(ex);
            context.Response.Write("-1");
        }
    }

    /// <summary>
    /// 通过TID/GID/PIDS获取对应字段属性的数据及对应字典LSTD的数据信息
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strTID"></param>
    /// <param name="strGID"></param>
    /// <param name="strPIDS"></param>
    private void GetTMPDAndLSTDData(HttpContext context,String strTID,String strGID,String strPIDS)
    {
        try 
        {
            strPIDS = Microsoft.JScript.GlobalObject.decodeURIComponent(strPIDS);
            strPIDS = strPIDS.Replace("[", "").Replace("]", "").Replace("\"", "");
            String strSqlInPID = "'" + strPIDS.Replace(",", "','") + "'";
            StringBuilder sbSql_TMPD = new StringBuilder();
            sbSql_TMPD.Append("select * from TB_HRTMPD where TID = '"+strTID+"' AND GID = '"+strGID+"'");
            sbSql_TMPD.Append(" AND PID IN ("+strSqlInPID+")");
            sbSql_TMPD.Append(" ORDER BY PORDER");
            DataTable dtTMPD = SqlParamDao.GetDataTableBySql(sbSql_TMPD.ToString());

            //遍历以获取这些字段中LID的LSTD数据
            String strLIDS = "";
            if(dtTMPD!=null && dtTMPD.Rows.Count>0){
                for(int i=0;i<dtTMPD.Rows.Count;i++){
                    String strPCTRL = dtTMPD.Rows[i]["PCTRL"].ToString();
                    String strPCTRLID = dtTMPD.Rows[i]["PCTRLID"].ToString();
                    if(strPCTRL.Equals("1")&&!String.IsNullOrEmpty(strPCTRLID)&&(strPCTRLID.IndexOf("@")<0)){
                        String strTempOne = "'" + strPCTRLID + "'";
                        strLIDS = strLIDS+(String.IsNullOrEmpty(strLIDS) ? strTempOne : "," + strTempOne);
                    }
                }
            }
            strLIDS = String.IsNullOrEmpty(strLIDS)?"''":strLIDS;
            StringBuilder sbSql_LSTD = new StringBuilder();
            sbSql_LSTD.Append("select * from TB_HRLSTD where LID  IN ("+strLIDS+")");
            DataTable dtLSTD = SqlParamDao.GetDataTableBySql(sbSql_LSTD.ToString());

            StringBuilder sBuilder = new StringBuilder();
            sBuilder.Append("{");
            sBuilder.Append(WebCommon.GetJsonStringByDataTable(dtTMPD,"\"TMPDData\"",false));
            sBuilder.Append(",");
            sBuilder.Append(WebCommon.GetJsonStringByDataTable(dtLSTD,"\"LSTDData\"",false));
            sBuilder.Append("}");

            string json = sBuilder.ToString();

            context.Response.Write(json);

        }
        catch (Exception ex)
        {
            log.Error(ex);
            context.Response.Write("-1");
        }
    }



    public bool IsReusable {
        get {
            return false;
        }
    }

}