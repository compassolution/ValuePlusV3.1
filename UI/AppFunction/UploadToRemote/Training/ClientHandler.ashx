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
        string strProjectId = hsTableUrlQuery["projectid"] == null ? string.Empty : hsTableUrlQuery["projectid"].ToString();//param
        string strTSEQ = hsTableUrlQuery["tseq"] == null ? string.Empty : hsTableUrlQuery["tseq"].ToString();//params


        log.Error("(UpdateServerHandler)"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"---Param:"+strParam);

        if (strParam.Equals("getclientinfo"))
        {
            this.GetClientInfo(context);
        }
        else if (strParam.Equals("gettrainninglist"))
        {
            this.GetTrainingList(context);
        }
        else if (strParam.Equals("getonetrainingdata"))
        {
            this.GetOneTraningData(context,strTSEQ);
        }
        else if (strParam.Equals("recordsendhistory"))
        {
            this.SetSendRecord(context, strTSEQ, this.GetUserCode());
        }
    }

    /// <summary>
    /// 获取当前客户信息
    /// </summary>
    /// <returns></returns>
    public void GetClientInfo(HttpContext context)
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
            sBuilder_ClientInfo.Append("{ResultData:[ ");
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
            sBuilder_ClientInfo.Append("]}");
            String strClientInfo = sBuilder_ClientInfo.ToString();
            context.Response.Write(strClientInfo);
        }
        catch (Exception ex)
        {
            log.Error(ex);
            context.Response.Write("");
        }
    }

    /// <summary>
    /// 获取培训报告列表
    /// </summary>
    /// <param name="context"></param>
    private void GetTrainingList(HttpContext context)
    {
        try
        {
            String strQueryFlag = System.Guid.NewGuid().ToString();
            String strSQL_ExecuteSP = "USP_ToRemote_TRN_QRY_QueryTraining";
            String strTableName = "_"+strSQL_ExecuteSP;
            Hashtable hsTableParam = new Hashtable();
            hsTableParam.Add("TSEQ", "");
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
            StringBuilder sBuilder = new StringBuilder();
            sBuilder.Append("{");
            sBuilder.Append(WebCommon.GetJsonStringByDataTable(dt,"\"ResultData\"",false));
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
    /// 获取对应某一个培训记录的数据信息
    /// 返回数据到客户端后，再传递到远程服务器端的服务
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strTSEQ"></param>
    private void GetOneTraningData(HttpContext context,String strTSEQ)
    {
        try
        {
            String strOpFlag = context.Request["txt_OpFlag"].ToString();
            String strSelectedTSEQ = context.Request["txt_SelectedTSEQ"].ToString();
            //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
            strOpFlag = SQLInjectionDefense.ReplaceSQLReservedKeyword(strOpFlag);
            strSelectedTSEQ = SQLInjectionDefense.ReplaceSQLReservedKeyword(strSelectedTSEQ);

            String strSQL_ExecuteSP = "USP_ToRemote_TRN_GetTrainingData";
            String strTableName = "_"+strSQL_ExecuteSP;
            Hashtable hsTableParam = new Hashtable();
            if (!String.IsNullOrEmpty(strTSEQ))
            {
                hsTableParam.Add("TSEQ", strTSEQ);
            }else
            {
                hsTableParam.Add("TSEQ", strSelectedTSEQ);//使用分号;隔开的字符串
            }
            hsTableParam.Add("Language", "zh-cn");
            hsTableParam.Add("OpFlag", strOpFlag);
            hsTableParam.Add("UserId", this.GetUserCode());
            int iReturnValue = SqlParamDao.ExcuteSP(strSQL_ExecuteSP, hsTableParam);

            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select * from ["+strTableName+"] where OpFlag = '"+strOpFlag+"'");
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
    /// 设置推送记录
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strTSEQ"></param>
    /// <param name="strUserId"></param>
    public void SetSendRecord(HttpContext context,String strTSEQ,String strUserId)
    {
        try
        {
            String strNow = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("update [HRTRN_1] set RemotePushTime = '"+strNow+"' where [TSEQ] = '"+strTSEQ+"';");
            sbSql.Append("update [HRTRN_1] set RemoteGetTime = '"+strNow+"' where [TSEQ] = '"+strTSEQ+"' and (SELECT COUNT(1) FROM HRTRN_4 WHERE TSEQ = '"+strTSEQ+"')>0;");

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