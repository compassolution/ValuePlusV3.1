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
            
        //log.Error("(UpdateServerHandler)"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"---Param:"+strParam);
        if (strParam.Equals("uploadsalarydata"))
        {
            //上传薪资数据到远程服务器
            this.DoUpdateSalaryToRemote(context, strProjectId);
        }
        else if (strParam.Equals("afteruploadsuccess"))
        {
            //上传薪资数据到远程服务器成功后的后续处理
            this.DoUpdateSalaryToRemote_AfterSuccess(context, strProjectId);
        }
    }

    /// <summary>
    /// 上传薪资数据到远程服务器
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strProjectId"></param>i
    private void DoUpdateSalaryToRemote(HttpContext context,String strProjectId)
    {
        int iReturnResult = -1;
        try
        {
            String strTableName = "TB_Remote_StaffSalaryData";
            StringBuilder sbSqlDelete = new StringBuilder();
            StringBuilder sbSqlInsert = new StringBuilder();
            String strSalaryData = context.Request["txt_SalaryData"].ToString();
            //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
            strSalaryData = SQLInjectionDefense.ReplaceSQLReservedKeyword(strSalaryData);


            JArray jsonArray = (JArray)JsonConvert.DeserializeObject(strSalaryData);
            int iCount = jsonArray.Count;
            log.Error("上传薪资数据到远程服务器DoUpdateSalaryToRemote(ProjectId:"+strProjectId+"),本次记录数："+iCount.ToString());

            foreach (JObject itemJArray in jsonArray)
            {

                //String strProjectId = itemJArray["ProjectId"].ToString();
                String strYEARMONTHDCNO = itemJArray["YEARMONTHDCNO"].ToString();
                String strITEMCODE = itemJArray["ITEMCODE"].ToString();
                sbSqlDelete.Append("delete from "+strTableName+" where ProjectId = '"+strProjectId+"' and YEARMONTHDCNO = '"+strYEARMONTHDCNO+"';");

                sbSqlInsert.Append("insert into "+strTableName+" ([ProjectId],[ProjectName],[ProjectNameChs],[YEARMONTHDCNO],[EMPNO],[YEARMONTH],[PSTART],[PEND],[EMPNAME],[EMPNAMECHS]");
                sbSqlInsert.Append(",[StaffMobile],[StaffIDCardNo],[StaffEmail]");
                sbSqlInsert.Append(",[DCDDESCCHS],[POSICODE],[DeptName],[DeptNameChs],[PosiName],[PosiNameChs]");
                sbSqlInsert.Append(",[JDATE],[LDATE],[DCEMAIL],[EMPBTYPE],[BankName],[EMPBANK],[EMPEVALUE],[EMPRVALUE],[EMPSC],[SFSB],[EMPHF],[SFGJJ],[DCSTATUS],[YGZT],[PCALSTAT]");
                sbSqlInsert.Append(",[XZJSZT],[CALTIME],[SEQNO],[ITEMCODE],[ITEMNAME],[ITEMNAMECHS],[ITEMEVALUE],[SHOWNAME],[SHOWNAMECHS],[ISSHOW],[ITEMPORDER],[OpFlag],[OpUser],[OpTime])");
                sbSqlInsert.Append(" values (");
                sbSqlInsert.Append(" '"+Microsoft.JScript.GlobalObject.unescape(itemJArray["ProjectId"].ToString()).Replace("'","''")+"'");
                sbSqlInsert.Append(", '"+Microsoft.JScript.GlobalObject.unescape(itemJArray["ProjectName"].ToString()).Replace("'","''")+"'");
                sbSqlInsert.Append(", '"+Microsoft.JScript.GlobalObject.unescape(itemJArray["ProjectNameChs"].ToString()).Replace("'","''")+"'");
                sbSqlInsert.Append(", '"+Microsoft.JScript.GlobalObject.unescape(itemJArray["YEARMONTHDCNO"].ToString()).Replace("'","''")+"'");
                sbSqlInsert.Append(", '"+Microsoft.JScript.GlobalObject.unescape(itemJArray["EMPNO"].ToString()).Replace("'","''")+"'");
                sbSqlInsert.Append(", '"+Microsoft.JScript.GlobalObject.unescape(itemJArray["YEARMONTH"].ToString()).Replace("'","''")+"'");
                sbSqlInsert.Append(", '"+Microsoft.JScript.GlobalObject.unescape(itemJArray["PSTART"].ToString()).Replace("'","''")+"'");
                sbSqlInsert.Append(", '"+Microsoft.JScript.GlobalObject.unescape(itemJArray["PEND"].ToString()).Replace("'","''")+"'");
                sbSqlInsert.Append(", '"+Microsoft.JScript.GlobalObject.unescape(itemJArray["EMPNAME"].ToString()).Replace("'","''")+"'");
                sbSqlInsert.Append(", '"+Microsoft.JScript.GlobalObject.unescape(itemJArray["EMPNAMECHS"].ToString()).Replace("'","''")+"'");

                sbSqlInsert.Append(", '"+Microsoft.JScript.GlobalObject.unescape(itemJArray["StaffMobile"].ToString()).Replace("'","''")+"'");
                sbSqlInsert.Append(", '"+Microsoft.JScript.GlobalObject.unescape(itemJArray["StaffIDCardNo"].ToString()).Replace("'","''")+"'");
                sbSqlInsert.Append(", '"+Microsoft.JScript.GlobalObject.unescape(itemJArray["StaffEmail"].ToString()).Replace("'","''")+"'");

                sbSqlInsert.Append(", '"+Microsoft.JScript.GlobalObject.unescape(itemJArray["DCDDESCCHS"].ToString()).Replace("'","''")+"'");
                sbSqlInsert.Append(", '"+Microsoft.JScript.GlobalObject.unescape(itemJArray["POSICODE"].ToString()).Replace("'","''")+"'");
                sbSqlInsert.Append(", '"+Microsoft.JScript.GlobalObject.unescape(itemJArray["DeptName"].ToString()).Replace("'","''")+"'");
                sbSqlInsert.Append(", '"+Microsoft.JScript.GlobalObject.unescape(itemJArray["DeptNameChs"].ToString()).Replace("'","''")+"'");
                sbSqlInsert.Append(", '"+Microsoft.JScript.GlobalObject.unescape(itemJArray["PosiName"].ToString()).Replace("'","''")+"'");
                sbSqlInsert.Append(", '"+Microsoft.JScript.GlobalObject.unescape(itemJArray["PosiNameChs"].ToString()).Replace("'","''")+"'");

                sbSqlInsert.Append(", '"+Microsoft.JScript.GlobalObject.unescape(itemJArray["JDATE"].ToString()).Replace("'","''")+"'");
                sbSqlInsert.Append(", '"+Microsoft.JScript.GlobalObject.unescape(itemJArray["LDATE"].ToString()).Replace("'","''")+"'");
                sbSqlInsert.Append(", '"+Microsoft.JScript.GlobalObject.unescape(itemJArray["DCEMAIL"].ToString()).Replace("'","''")+"'");
                sbSqlInsert.Append(", '"+Microsoft.JScript.GlobalObject.unescape(itemJArray["EMPBTYPE"].ToString()).Replace("'","''")+"'");
                sbSqlInsert.Append(", '"+Microsoft.JScript.GlobalObject.unescape(itemJArray["BankName"].ToString()).Replace("'","''")+"'");
                sbSqlInsert.Append(", '"+Microsoft.JScript.GlobalObject.unescape(itemJArray["EMPBANK"].ToString()).Replace("'","''")+"'");
                sbSqlInsert.Append(", '"+Microsoft.JScript.GlobalObject.unescape(itemJArray["EMPEVALUE"].ToString()).Replace("'","''")+"'");
                sbSqlInsert.Append(", '"+Microsoft.JScript.GlobalObject.unescape(itemJArray["EMPRVALUE"].ToString()).Replace("'","''")+"'");
                sbSqlInsert.Append(", '"+Microsoft.JScript.GlobalObject.unescape(itemJArray["EMPSC"].ToString()).Replace("'","''")+"'");
                sbSqlInsert.Append(", '"+Microsoft.JScript.GlobalObject.unescape(itemJArray["SFSB"].ToString()).Replace("'","''")+"'");
                sbSqlInsert.Append(", '"+Microsoft.JScript.GlobalObject.unescape(itemJArray["EMPHF"].ToString()).Replace("'","''")+"'");
                sbSqlInsert.Append(", '"+Microsoft.JScript.GlobalObject.unescape(itemJArray["SFGJJ"].ToString()).Replace("'","''")+"'");
                sbSqlInsert.Append(", '"+Microsoft.JScript.GlobalObject.unescape(itemJArray["DCSTATUS"].ToString()).Replace("'","''")+"'");
                sbSqlInsert.Append(", '"+Microsoft.JScript.GlobalObject.unescape(itemJArray["YGZT"].ToString()).Replace("'","''")+"'");
                sbSqlInsert.Append(", '"+Microsoft.JScript.GlobalObject.unescape(itemJArray["PCALSTAT"].ToString()).Replace("'","''")+"'");

                sbSqlInsert.Append(", '"+Microsoft.JScript.GlobalObject.unescape(itemJArray["XZJSZT"].ToString()).Replace("'","''")+"'");
                sbSqlInsert.Append(", '"+Microsoft.JScript.GlobalObject.unescape(itemJArray["CALTIME"].ToString()).Replace("'","''")+"'");
                sbSqlInsert.Append(", '"+Microsoft.JScript.GlobalObject.unescape(itemJArray["SEQNO"].ToString()).Replace("'","''")+"'");
                sbSqlInsert.Append(", '"+Microsoft.JScript.GlobalObject.unescape(itemJArray["ITEMCODE"].ToString()).Replace("'","''")+"'");
                sbSqlInsert.Append(", '"+Microsoft.JScript.GlobalObject.unescape(itemJArray["ITEMNAME"].ToString()).Replace("'","''")+"'");
                sbSqlInsert.Append(", '"+Microsoft.JScript.GlobalObject.unescape(itemJArray["ITEMNAMECHS"].ToString()).Replace("'","''")+"'");
                sbSqlInsert.Append(", '"+Microsoft.JScript.GlobalObject.unescape(itemJArray["ITEMEVALUE"].ToString()).Replace("'","''")+"'");
                sbSqlInsert.Append(", '"+Microsoft.JScript.GlobalObject.unescape(itemJArray["SHOWNAME"].ToString()).Replace("'","''")+"'");
                sbSqlInsert.Append(", '"+Microsoft.JScript.GlobalObject.unescape(itemJArray["SHOWNAMECHS"].ToString()).Replace("'","''")+"'");
                sbSqlInsert.Append(", '"+Microsoft.JScript.GlobalObject.unescape(itemJArray["ISSHOW"].ToString()).Replace("'","''")+"'");
                sbSqlInsert.Append(", '"+Microsoft.JScript.GlobalObject.unescape(itemJArray["ITEMPORDER"].ToString()).Replace("'","''")+"'");
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
                    }
                }
                catch (Exception ex)
                {
                    log.Error("上传薪资数据到远程服务器出错，SQL语句执行失败\r\n");
                    log.Error("SQL语句:"+strInsertSql+"\r\n");
                    log.Error("错误信息："+ex.ToString());
                }
            }
        }
        catch (Exception ex)
        {
            log.Error("上传薪资数据到远程服务器出错，解析薪资数据json失败\r\n");
            log.Error("错误信息："+ex.ToString());
        }
        finally
        {
            context.Response.Write(iReturnResult.ToString());
        }
    }

    /// <summary>
    /// 上传薪资数据到远程服务器成功后的后续处理
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strProjectId"></param>i
    private void DoUpdateSalaryToRemote_AfterSuccess(HttpContext context,String strProjectId)
    {
        int iReturnResult = -1;
        log.Error("上传薪资数据到远程服务器成功后的后续处理:DoUpdateSalaryToRemote_AfterSuccess(ProjectId:"+strProjectId+")");
        try
        {
            //String strProjectId = context.Request["txt_ProjectId"].ToString();
            String strOpFlag = context.Request["txt_OpFlag"].ToString();
            //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
            strOpFlag = SQLInjectionDefense.ReplaceSQLReservedKeyword(strOpFlag);

            String strSQL_ExecuteSP = "USP_Remote_HR_DealAfterGetSalaryData";
            Hashtable hsTableParam = new Hashtable();
            hsTableParam.Add("ProjectId", strProjectId);
            hsTableParam.Add("OpFlag", strOpFlag);
            hsTableParam.Add("UserId", this.GetUserCode());
            int iReturnValue = SqlParamDao.ExcuteSP(strSQL_ExecuteSP, hsTableParam);

            iReturnResult = iReturnValue;
        }
        catch (Exception ex)
        {
            log.Error("上传薪资数据到远程服务器成功后的后续处理出错\r\n");
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