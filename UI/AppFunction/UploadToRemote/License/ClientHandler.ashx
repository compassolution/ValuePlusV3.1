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
        string strYearMonthType = hsTableUrlQuery["yearmonthtype"] == null ? string.Empty : hsTableUrlQuery["yearmonthtype"].ToString();//param
        string strProjectId = hsTableUrlQuery["projectid"] == null ? string.Empty : hsTableUrlQuery["projectid"].ToString();//param
        string strYearMonthDCNO = hsTableUrlQuery["yearmonthdcno"] == null ? string.Empty : hsTableUrlQuery["yearmonthdcno"].ToString();//params


        if (strParam.Equals("getlicenseinfo"))
        {
            this.GetLicenseInfo(context);
        }
    }

    /// <summary>
    /// 从本地数据库中获取Lic数据
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strProjectId"></param>
    private void GetLicenseInfo(HttpContext context)
    {
        try
        {
            String strProjectId = this.GetProjectId();
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select * from TB_LICENSE where LICENSECODE = '"+strProjectId+"'");
            String strSql = sbSql.ToString();

            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);

            int iColCount = dt.Columns.Count;
            StringBuilder sBuilder = new StringBuilder();
            sBuilder.Append("{ \"ProjectId\":\""+strProjectId+"\" ");
            sBuilder.Append(",\"RemoteServer\":\""+this.GetRemoteServer()+"\" ");
            sBuilder.Append(", \"ResultData\":[ ");
            if ((dt != null) && (dt.Rows.Count > 0))
            {
                //sBuilder.Append("   " + this.ulHeaderMenu.ClientID + ".innerHTML =\"\r\n");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i > 0)
                    {
                        sBuilder.Append(",{");
                    }
                    else
                    {
                        sBuilder.Append("{");
                    }
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        String strColName = dt.Columns[j].ColumnName;
                        //对值进行编码处理特殊字符，如引号等
                        //String strColValue = Microsoft.JScript.GlobalObject.escape(dt.Rows[i][dt.Columns[j].ColumnName].ToString());
                        String strColValue = dt.Rows[i][dt.Columns[j].ColumnName].ToString();
                        if (j == 0)
                        {
                            sBuilder.Append(strColName + ":'" + strColValue + "'");
                        }
                        else
                        {
                            sBuilder.Append("," + strColName + ":'" + strColValue + "'");
                        }
                    }

                    sBuilder.Append("}");
                }

            }
            sBuilder.Append("]");
            sBuilder.Append("}");

            string json = sBuilder.ToString();

            context.Response.Write(json);
        }
        catch (Exception ex)
        {
            context.Response.Write("");
            log.Error(ex);
        }
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}