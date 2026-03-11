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

        log.Error("(UploadToRemote/Training/ServerHandler)" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "---Param:" + strParam);
        if (strParam.Equals("uploaddatatoremote"))
        {
            //上传数据到远程服务器
            this.DoUpdateDataToRemote(context, strProjectId);
        }
        else if (strParam.Equals("afteruploadsuccess"))
        {
            //上传培训记录数据到远程服务器成功后的后续处理
            this.DoUpdateDataToRemote_AfterSuccess(context, strProjectId);
        }
    }

    /// <summary>
    /// 上传数据到远程服务器,同时也返回签到记录到本地
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strProjectId"></param>i
    private void DoUpdateDataToRemote(HttpContext context,String strProjectId)
    {
        int iReturnResult = -1;
        StringBuilder sBuilder = new StringBuilder();
        try
        {
            String strTableName = "TB_Remote_TrainingData";
            StringBuilder sbSqlDelete = new StringBuilder();
            StringBuilder sbSqlInsert = new StringBuilder();
            String strResultData = context.Request["txt_ResultData"].ToString();
            //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
            strResultData = SQLInjectionDefense.ReplaceSQLReservedKeyword(strResultData);

            JArray jsonArray = (JArray)JsonConvert.DeserializeObject(strResultData);
            int iCount = jsonArray.Count;
            log.Error("上传培训记录数据到远程服务器DoUpdateDataToRemote(ProjectId:"+strProjectId+"),本次记录数："+iCount.ToString());
            
            String strTSEQ ="";
            foreach (JObject itemJArray in jsonArray)
            {
                strTSEQ =Microsoft.JScript.GlobalObject.unescape(itemJArray["TSEQ"].ToString()).Replace("'","''");
                log.Error("上传培训记录数据到远程服务器DoUpdateDataToRemote(itemJArray："+itemJArray.ToString());
                sbSqlDelete.Append("delete from "+strTableName+" where ProjectId = '"+strProjectId+"' and TSEQ = '"+strTSEQ+"';");

                sbSqlInsert.Append("insert into "+strTableName+" ([ProjectId],[ProjectName],[ProjectNameChs],[TSEQ],[COURSENAME],[COURSEDESC],[LOCATION],[TDSECT]");
                sbSqlInsert.Append(",[TDSECTNAME],[TRAINER],[TTIMEFROM],[TTIMETO],[NOTES],[State],[StateName],[OpFlag],[OpUser],[OpTime])");
                sbSqlInsert.Append(" values (");
                sbSqlInsert.Append(" '"+Microsoft.JScript.GlobalObject.unescape(itemJArray["ProjectId"].ToString()).Replace("'","''")+"'");
                sbSqlInsert.Append(", '"+Microsoft.JScript.GlobalObject.unescape(itemJArray["ProjectName"].ToString()).Replace("'","''")+"'");
                sbSqlInsert.Append(", '"+Microsoft.JScript.GlobalObject.unescape(itemJArray["ProjectNameChs"].ToString()).Replace("'","''")+"'");
                sbSqlInsert.Append(", '"+strTSEQ+"'");
                sbSqlInsert.Append(", '"+Microsoft.JScript.GlobalObject.unescape(itemJArray["COURSENAME"].ToString()).Replace("'","''")+"'");
                sbSqlInsert.Append(", '"+Microsoft.JScript.GlobalObject.unescape(itemJArray["COURSEDESC"].ToString()).Replace("'","''")+"'");
                sbSqlInsert.Append(", '"+Microsoft.JScript.GlobalObject.unescape(itemJArray["LOCATION"].ToString()).Replace("'","''")+"'");

                sbSqlInsert.Append(", '"+Microsoft.JScript.GlobalObject.unescape(itemJArray["TDSECT"].ToString()).Replace("'","''")+"'");
                sbSqlInsert.Append(", '"+Microsoft.JScript.GlobalObject.unescape(itemJArray["TDSECTNAME"].ToString()).Replace("'","''")+"'");

                sbSqlInsert.Append(", '"+Microsoft.JScript.GlobalObject.unescape(itemJArray["TRAINER"].ToString()).Replace("'","''")+"'");
                sbSqlInsert.Append(", '"+Microsoft.JScript.GlobalObject.unescape(itemJArray["TTIMEFROM"].ToString()).Replace("'","''")+"'");
                sbSqlInsert.Append(", '"+Microsoft.JScript.GlobalObject.unescape(itemJArray["TTIMETO"].ToString()).Replace("'","''")+"'");

                sbSqlInsert.Append(", '"+Microsoft.JScript.GlobalObject.unescape(itemJArray["NOTES"].ToString()).Replace("'","''")+"'");
                sbSqlInsert.Append(", '"+Microsoft.JScript.GlobalObject.unescape(itemJArray["State"].ToString()).Replace("'","''")+"'");
                sbSqlInsert.Append(", '"+Microsoft.JScript.GlobalObject.unescape(itemJArray["StateName"].ToString()).Replace("'","''")+"'");

                sbSqlInsert.Append(", '"+Microsoft.JScript.GlobalObject.unescape(itemJArray["OpFlag"].ToString()).Replace("'","''")+"'");
                sbSqlInsert.Append(", '"+Microsoft.JScript.GlobalObject.unescape(itemJArray["OpUser"].ToString()).Replace("'","''")+"'");
                sbSqlInsert.Append(", '"+Microsoft.JScript.GlobalObject.unescape(itemJArray["OpTime"].ToString()).Replace("'","''")+"'");
                sbSqlInsert.Append(");");
            }
            sbSqlDelete.Append(sbSqlInsert.ToString());

            String strInsertSql = sbSqlDelete.ToString();
            if (!String.IsNullOrEmpty(strInsertSql))
            {
                try
                {
                    int iReturnCount = SqlParamDao.ExecuteNonQueryBySql(strInsertSql);
                    if (iReturnCount > 0)
                    {
                        iReturnResult = iReturnCount;

                        //同时获取云服务器上的签到记录并返回                        
                        StringBuilder sbSql = new StringBuilder();
                        sbSql.Append("select * from [TB_Remote_TrainingData_SignIn] where [ProjectId] = '"+strProjectId+"' and [TSEQ] = '"+strTSEQ+"'");
                        DataTable dt = SqlParamDao.GetDataTableBySql(sbSql.ToString());
                        sBuilder.Append("{");
                        sBuilder.Append(WebCommon.GetJsonStringByDataTable(dt,"\"ResultData\"",false));
                        sBuilder.Append("}");
                    }
                }
                catch (Exception ex)
                {
                    log.Error("上传培训记录数据到远程服务器出错，SQL语句执行失败\r\n");
                    log.Error("SQL语句:"+strInsertSql+"\r\n");
                    log.Error("错误信息："+ex.ToString());
                }
            }
        }
        catch (Exception ex)
        {
            log.Error("上传培训记录数据到远程服务器出错，解析薪资数据json失败\r\n");
            log.Error("错误信息："+ex.ToString());
        }
        finally
        {
            context.Response.Write(sBuilder.ToString());
        }
    }

    /// <summary>
    /// 上传培训记录数据到远程服务器成功后的后续处理
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strProjectId"></param>i
    private void DoUpdateDataToRemote_AfterSuccess(HttpContext context,String strProjectId)
    {
        int iReturnResult = -1;
        log.Error("上传培训记录数据到远程服务器成功后的后续处理:DoUpdateDataToRemote_AfterSuccess(ProjectId:"+strProjectId+")");
        try
        {
            //String strProjectId = context.Request["txt_ProjectId"].ToString();
            String strOpFlag = context.Request["txt_OpFlag"].ToString();
            //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
            strOpFlag = SQLInjectionDefense.ReplaceSQLReservedKeyword(strOpFlag);

            String strSQL_ExecuteSP = "USP_Remote_TRN_AfterDealTrainingData";
            Hashtable hsTableParam = new Hashtable();
            hsTableParam.Add("ProjectId", strProjectId);
            hsTableParam.Add("OpFlag", strOpFlag);
            hsTableParam.Add("UserId", this.GetUserCode());
            int iReturnValue = SqlParamDao.ExcuteSP(strSQL_ExecuteSP, hsTableParam);

            iReturnResult = iReturnValue;
        }
        catch (Exception ex)
        {
            log.Error("上传培训记录数据到远程服务器成功后的后续处理出错\r\n");
            log.Error("错误信息："+ex.ToString());
        }
        finally
        {
            context.Response.Write(iReturnResult.ToString());
        }
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}