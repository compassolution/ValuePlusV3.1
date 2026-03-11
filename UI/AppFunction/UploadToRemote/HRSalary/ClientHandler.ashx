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
using Com.ValuePlus.Common.Security;

public class ClientHandler : HandlerBase, IHttpHandler, System.Web.SessionState.IRequiresSessionState {

    public void ProcessRequest (HttpContext context) {
        Hashtable hsTableUrlQuery = this.GetUrlAnalyse(context);
        string strCallback = hsTableUrlQuery["callback"] == null ? string.Empty : hsTableUrlQuery["callback"].ToString();//param
        string strParam = hsTableUrlQuery["param"] == null ? string.Empty : hsTableUrlQuery["param"].ToString();//param
        string strYearMonthType = hsTableUrlQuery["yearmonthtype"] == null ? string.Empty : hsTableUrlQuery["yearmonthtype"].ToString();//param
        string strProjectId = hsTableUrlQuery["projectid"] == null ? string.Empty : hsTableUrlQuery["projectid"].ToString();//param
        string strYearMonthDCNO = hsTableUrlQuery["yearmonthdcno"] == null ? string.Empty : hsTableUrlQuery["yearmonthdcno"].ToString();//params


        log.Error("(UpdateServerHandler)"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"---Param:"+strParam);

        if (strParam.Equals("getyearmonth"))
        {
            this.GetYearMonthList(context,strYearMonthType);
        }
        else if (strParam.Equals("getstafflist"))
        {
            this.GetStaffSalaryList(context);
        }
        else if (strParam.Equals("getstaffsalary"))
        {
            this.GetStaffSalaryData(context,strYearMonthDCNO);
        }
        else if (strParam.Equals("recordsendhistory"))
        {
            this.SetSendRecord(context,strYearMonthDCNO,this.GetUserCode());
        }
    }

    /// <summary>
    /// 获取薪资期间列表
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strYearMonthType"></param>
    private void GetYearMonthList(HttpContext context,String strYearMonthType)
    {
        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select * from KQPERD_1 where 1=1");
            sbSql.Append(" and PID IN (select YearMonth from PREMPL_1 union all select YearMonth from PREMPL_1_H)");
            sbSql.Append(" ORDER BY PID DESC");
            String strSql = sbSql.ToString();

            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);

            int iColCount = dt.Columns.Count;
            StringBuilder sBuilder = new StringBuilder();
            sBuilder.Append("{ResultData:[ ");
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
            String strClientInfo = GetCurClientInfo();
            if (!String.IsNullOrEmpty(strClientInfo))
            {
                sBuilder.Append("," + strClientInfo);
            }
            sBuilder.Append("}");

            string json = sBuilder.ToString();

            context.Response.Write(json);
        }
        catch (Exception ex)
        {
            context.Response.Write("error");
            log.Error(ex);
        }
    }

    /// <summary>
    /// 获取当前客户信息
    /// </summary>
    /// <returns></returns>
    public String GetCurClientInfo()
    {
        try
        {
            String strSql_ProjectId = "select top 1 * from BASICPARAM_1 WHERE paramName = 'ProjectId'";
            String strSql_ClientName = "select top 1 * from TB_HR_MENU WHERE SMENUCODE = 'ROOT'";
            String strSql_RemoteServer = "select top 1 * from BASICPARAM_1 WHERE paramName = 'SalaryRemoteServer'";
            DataTable dt_ProjectId = SqlParamDao.GetDataTableBySql(strSql_ProjectId);
            DataTable dt_ClientName = SqlParamDao.GetDataTableBySql(strSql_ClientName);
            DataTable dt_RemoteServer = SqlParamDao.GetDataTableBySql(strSql_RemoteServer);
            StringBuilder sBuilder_ClientInfo = new StringBuilder();
            sBuilder_ClientInfo.Append("ClientInfo:[ ");
            sBuilder_ClientInfo.Append("{");
            if ((dt_ProjectId != null) && (dt_ProjectId.Rows.Count > 0))
            {
                String strParamValue = dt_ProjectId.Rows[0]["ParamValue"].ToString();
                sBuilder_ClientInfo.Append("ProjectId:" + "'" + strParamValue + "'");
            }
            if ((dt_ClientName != null) && (dt_ClientName.Rows.Count > 0))
            {
                String strClientName = dt_ClientName.Rows[0]["SMENUNAMECN"].ToString();
                sBuilder_ClientInfo.Append(",ClientName:" + "'" + strClientName + "'");
            }
            if ((dt_RemoteServer != null) && (dt_RemoteServer.Rows.Count > 0))
            {
                String strRemoteServer = dt_RemoteServer.Rows[0]["ParamValue"].ToString();
                sBuilder_ClientInfo.Append(",RemoteServer:" + "'" + strRemoteServer + "'");
            }
            sBuilder_ClientInfo.Append("}");
            sBuilder_ClientInfo.Append("]");
            String strClientInfo = sBuilder_ClientInfo.ToString();
            return strClientInfo;
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return "";
        }
    }

    /// <summary>
    /// 获取对应薪资期间的发放员工列表
    /// </summary>
    /// <param name="context"></param>
    private void GetStaffSalaryList(HttpContext context)
    {
        try
        {
            String strQueryFlag = System.Guid.NewGuid().ToString();
            String strSQL_ExecuteSP = "USP_ToRemote_HR_QRY_QuerySalary";
            String strTableName = "_"+strSQL_ExecuteSP;

            String strYearMonth = context.Request["sel_YearMonth"].ToString();
            //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
            strYearMonth = SQLInjectionDefense.ReplaceSQLReservedKeyword(strYearMonth);

            Hashtable hsTableParam = new Hashtable();
            hsTableParam.Add("YearMonth", strYearMonth);
            hsTableParam.Add("Language", "zh-cn");
            hsTableParam.Add("QueryFlag", strQueryFlag);
            hsTableParam.Add("UserId", this.GetUserCode());
            int iReturnValue = SqlParamDao.ExcuteSP(strSQL_ExecuteSP, hsTableParam);

            String strSql_Cols = "select a.* from syscolumns a inner join sysobjects b on a.id = b.id where b.name = '"+strTableName+"' order by colorder";
            DataTable dt_Cols = SqlParamDao.GetDataTableBySql(strSql_Cols);
            StringBuilder sBuilder_ColName = new StringBuilder();
            sBuilder_ColName.Append("colNames:[ ");
            sBuilder_ColName.Append("{");
            if ((dt_Cols != null) && (dt_Cols.Rows.Count > 0))
            {
                for(int i = 0; i < dt_Cols.Rows.Count; i++)
                {
                    String strColName = dt_Cols.Rows[i]["name"].ToString();
                    if (i == 0)
                    {
                        sBuilder_ColName.Append(strColName+":"+"'"+strColName+"'");
                    }else
                    {
                        sBuilder_ColName.Append(","+strColName+":"+"'"+strColName+"'");
                    }
                }
            }
            sBuilder_ColName.Append("}");
            sBuilder_ColName.Append("]");
            String strColNames = sBuilder_ColName.ToString();

            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select * from ["+strTableName+"] where QueryFlag = '"+strQueryFlag+"'");
            DataTable dt = SqlParamDao.GetDataTableBySql(sbSql.ToString());
            int iColCount = dt.Columns.Count;
            StringBuilder sBuilder = new StringBuilder();
            sBuilder.Append("{ResultData:[ ");
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
                        String strColValue = Microsoft.JScript.GlobalObject.escape(dt.Rows[i][dt.Columns[j].ColumnName].ToString());
                        //String strColValue = dt.Rows[i][dt.Columns[j].ColumnName].ToString();
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
            if (!String.IsNullOrEmpty(strColNames))
            {
                sBuilder.Append("," + strColNames);
            }
            sBuilder.Append("}");

            string json = sBuilder.ToString();

            context.Response.Write(json);

        }
        catch (Exception ex)
        {
            log.Error(ex);
            context.Response.Write("error");
        }
    }

    /// <summary>
    /// 获取对应薪资期间的员工薪资数据信息
    /// 返回数据到客户端后，再传递到远程服务器端的服务
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strYearMonthDCNO"></param>
    private void GetStaffSalaryData(HttpContext context,String strYearMonthDCNO)
    {
        try
        {
            String strOpFlag = context.Request["txt_OpFlag"].ToString();
            String strYearMonth = context.Request["sel_YearMonth"].ToString();
            String strSelectedYearMonthDCNO = context.Request["txt_SelectedYearMonthDCNO"].ToString();
            //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
            strOpFlag = SQLInjectionDefense.ReplaceSQLReservedKeyword(strOpFlag);
            strYearMonth = SQLInjectionDefense.ReplaceSQLReservedKeyword(strYearMonth);
            strSelectedYearMonthDCNO = SQLInjectionDefense.ReplaceSQLReservedKeyword(strSelectedYearMonthDCNO);

            String strSQL_ExecuteSP = "USP_ToRemote_HR_GetSalaryData";
            String strTableName = "_"+strSQL_ExecuteSP;

            Hashtable hsTableParam = new Hashtable();
            hsTableParam.Add("YearMonth", strYearMonth);
            if (!String.IsNullOrEmpty(strYearMonthDCNO))
            {
                hsTableParam.Add("YearMonthDCNO", strYearMonthDCNO);
            }else
            {
                hsTableParam.Add("YearMonthDCNO", strSelectedYearMonthDCNO);//使用分号;隔开的字符串
            }
            hsTableParam.Add("Language", "zh-cn");
            hsTableParam.Add("OpFlag", strOpFlag);
            hsTableParam.Add("UserId", this.GetUserCode());
            int iReturnValue = SqlParamDao.ExcuteSP(strSQL_ExecuteSP, hsTableParam);

            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select * from ["+strTableName+"] where OpFlag = '"+strOpFlag+"'");
            DataTable dt = SqlParamDao.GetDataTableBySql(sbSql.ToString());
            int iColCount = dt.Columns.Count;
            StringBuilder sBuilder = new StringBuilder();
            //sBuilder.Append("{ResultData:[ ");
            sBuilder.Append("[ ");
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
                        String strColValue = Microsoft.JScript.GlobalObject.escape(dt.Rows[i][dt.Columns[j].ColumnName].ToString());
                        //String strColValue = dt.Rows[i][dt.Columns[j].ColumnName].ToString();
                        if (j == 0)
                        {
                            sBuilder.Append("\""+strColName + "\":\"" + strColValue + "\"");
                        }
                        else
                        {
                            sBuilder.Append(",\"" + strColName + "\":\"" + strColValue + "\"");
                        }
                    }

                    sBuilder.Append("}");
                }

            }
            sBuilder.Append("]");
            //sBuilder.Append("}");

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
    /// 设置推送记录
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strYearMonthDCNO"></param>
    /// <param name="strUserId"></param>
    public void SetSendRecord(HttpContext context,String strYearMonthDCNO,String strUserId)
    {
        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("delete from [PREMPL_SendWX] where [YEARMONTHDCNO] = '"+strYearMonthDCNO+"';");
            sbSql.Append("insert into [PREMPL_SendWX]([YEARMONTHDCNO],[SendUserId],[SendTime])values");
            sbSql.Append("('"+strYearMonthDCNO+"','"+strUserId+"','"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"')");

            log.Error("设置推送记录，脚本语句："+sbSql.ToString());
            int iReturnValue = SqlParamDao.ExecuteNonQueryBySql(sbSql.ToString());

            context.Response.Write(iReturnValue);

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