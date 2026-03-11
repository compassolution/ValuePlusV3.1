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
        string strWXMPModule = hsTableUrlQuery["wxmpmodule"] == null ? string.Empty : hsTableUrlQuery["wxmpmodule"].ToString();//param
        
        if (strParam.Equals("uploaddatatoremote"))
        {
            //上传员工基础数据到远程服务器
            this.DoUpdateDataToRemote(context, strProjectId);
        }
        else if (strParam.Equals("get_reg_mobile"))
        {
            GetRegistedMobileList(context,strProjectId);
        }
    }
    
    /// <summary>
    /// 上传员工基础数据到远程服务器
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strProjectId"></param>i
    private void DoUpdateDataToRemote(HttpContext context,String strProjectId)
    {
        int iReturnResult = -1;
        StringBuilder sBuilder = new StringBuilder();
        try
        {
            String strTableName = "TB_Remote_StaffBasicData";
            StringBuilder sbSqlDelete = new StringBuilder();
            StringBuilder sbSqlInsert = new StringBuilder();
            String strResultData = context.Request["txt_StaffBasicData"].ToString();
            
            //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
            strResultData = SQLInjectionDefense.ReplaceSQLReservedKeyword(strResultData);

            JArray jsonArray = (JArray)JsonConvert.DeserializeObject(strResultData);
            int iCount = jsonArray.Count;
            log.Error("上传员工基础数据到远程服务器DoUpdateDataToRemote(ProjectId:"+strProjectId+"),本次记录数："+iCount.ToString());
            
            String strDCNO ="";
            foreach (JObject itemJArray in jsonArray)
            {
                strDCNO =Microsoft.JScript.GlobalObject.unescape(itemJArray["DCNO"].ToString()).Replace("'","''");
                log.Error("上传员工基础数据到远程服务器DoUpdateDataToRemote(itemJArray："+itemJArray.ToString());
                sbSqlDelete.Append("delete from "+strTableName+" where ProjectId = '"+strProjectId+"' and DCNO = '"+strDCNO+"';");

                sbSqlInsert.Append("insert into "+strTableName+" ([ProjectId],[ProjectName],[ProjectNameChs],[DCNO],[DCNAME],[DCNAMECHS],[DCGENDER],[DCID]");
                sbSqlInsert.Append(",[DCDDESCCHS],[DeptName],[DeptNameChs],[DCHRPOSI],[PosiName],[PosiNameChs],[DCPLEVEL]");
                sbSqlInsert.Append(",[DCJOIN],[DCDMACT],[DCMOBILE],[DCSTATUS],[StatusName]");
                sbSqlInsert.Append(",[OpFlag],[OpUser],[OpTime])");
                sbSqlInsert.Append(" values (");
                sbSqlInsert.Append(" '"+Microsoft.JScript.GlobalObject.unescape(itemJArray["ProjectId"].ToString()).Replace("'","''")+"'");
                sbSqlInsert.Append(", '"+Microsoft.JScript.GlobalObject.unescape(itemJArray["ProjectName"].ToString()).Replace("'","''")+"'");
                sbSqlInsert.Append(", '"+Microsoft.JScript.GlobalObject.unescape(itemJArray["ProjectNameChs"].ToString()).Replace("'","''")+"'");
                sbSqlInsert.Append(", '"+strDCNO+"'");
                sbSqlInsert.Append(", '"+Microsoft.JScript.GlobalObject.unescape(itemJArray["DCNAME"].ToString()).Replace("'","''")+"'");
                sbSqlInsert.Append(", '"+Microsoft.JScript.GlobalObject.unescape(itemJArray["DCNAMECHS"].ToString()).Replace("'","''")+"'");
                sbSqlInsert.Append(", '"+Microsoft.JScript.GlobalObject.unescape(itemJArray["DCGENDER"].ToString()).Replace("'","''")+"'");

                sbSqlInsert.Append(", '"+Microsoft.JScript.GlobalObject.unescape(itemJArray["DCID"].ToString()).Replace("'","''")+"'");
                sbSqlInsert.Append(", '"+Microsoft.JScript.GlobalObject.unescape(itemJArray["DCDDESCCHS"].ToString()).Replace("'","''")+"'");

                sbSqlInsert.Append(", '"+Microsoft.JScript.GlobalObject.unescape(itemJArray["DeptName"].ToString()).Replace("'","''")+"'");
                sbSqlInsert.Append(", '"+Microsoft.JScript.GlobalObject.unescape(itemJArray["DeptNameChs"].ToString()).Replace("'","''")+"'");
                sbSqlInsert.Append(", '"+Microsoft.JScript.GlobalObject.unescape(itemJArray["DCHRPOSI"].ToString()).Replace("'","''")+"'");

                sbSqlInsert.Append(", '"+Microsoft.JScript.GlobalObject.unescape(itemJArray["PosiName"].ToString()).Replace("'","''")+"'");
                sbSqlInsert.Append(", '"+Microsoft.JScript.GlobalObject.unescape(itemJArray["PosiNameChs"].ToString()).Replace("'","''")+"'");
                sbSqlInsert.Append(", '"+Microsoft.JScript.GlobalObject.unescape(itemJArray["DCPLEVEL"].ToString()).Replace("'","''")+"'");
                
                sbSqlInsert.Append(", '"+Microsoft.JScript.GlobalObject.unescape(itemJArray["DCJOIN"].ToString()).Replace("'","''")+"'");
                sbSqlInsert.Append(", '"+Microsoft.JScript.GlobalObject.unescape(itemJArray["DCDMACT"].ToString()).Replace("'","''")+"'");
                sbSqlInsert.Append(", '"+Microsoft.JScript.GlobalObject.unescape(itemJArray["DCMOBILE"].ToString()).Replace("'","''")+"'");
                sbSqlInsert.Append(", '"+Microsoft.JScript.GlobalObject.unescape(itemJArray["DCSTATUS"].ToString()).Replace("'","''")+"'");
                sbSqlInsert.Append(", '"+Microsoft.JScript.GlobalObject.unescape(itemJArray["StatusName"].ToString()).Replace("'","''")+"'");

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
                        
                        //推送至服务器成功后的后续处理
                        String strOpFlag = context.Request["txt_OpFlag"].ToString();
                        //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
                        strOpFlag = SQLInjectionDefense.ReplaceSQLReservedKeyword(strOpFlag);

                        String strSQL_ExecuteSP = "USP_Remote_HR_AfterGetStaffBasicData";
                        Hashtable hsTableParam = new Hashtable();
                        hsTableParam.Add("ProjectId", strProjectId);
                        hsTableParam.Add("OpFlag", strOpFlag);
                        hsTableParam.Add("UserId", this.GetUserCode());
                        int iReturnValue = SqlParamDao.ExcuteSP(strSQL_ExecuteSP, hsTableParam);
                    }
                }
                catch (Exception ex)
                {
                    log.Error("上传员工基础数据到远程服务器出错，SQL语句执行失败\r\n");
                    log.Error("SQL语句:"+strInsertSql+"\r\n");
                    log.Error("错误信息："+ex.ToString());
                }
            }
        }
        catch (Exception ex)
        {
            log.Error("上传员工基础数据到远程服务器出错，解析薪资数据json失败\r\n");
            log.Error("错误信息："+ex.ToString());
        }
        finally
        {
            context.Response.Write(iReturnResult);
        }
    }


    /// <summary>
    /// 从远程服务器上获取已注册微信公众号的手机号列表
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strProjectId"></param>
    private void GetRegistedMobileList(HttpContext context,String strProjectId)
    {
        try
        {
            StringBuilder sbSql = new StringBuilder();
            log.Error("获取已注册微信公众号的手机号列表，获取表单数据前记录");
            //String strStaffMobileList = HttpUtility.UrlDecode(context.Request["StaffMobileList"].ToString());
            String strStaffMobileList = HttpUtility.UrlDecode(context.Request["txt_StaffMobileList"].ToString());
            //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
            strStaffMobileList = SQLInjectionDefense.ReplaceSQLReservedKeyword(strStaffMobileList);
            //string strStaffMobileList = "13920394023;23242344";
            log.Error("获取已注册微信公众号的手机号列表:" + strStaffMobileList);

            String strSQL_ExecuteSP = "USP_WX_QRY_RegistMobileNo";
            String strTableName = "_" + strSQL_ExecuteSP;
            Hashtable hsTableParam = new Hashtable();
            hsTableParam.Add("ProjectId", strProjectId);
            hsTableParam.Add("StaffMobileList", strStaffMobileList);
            hsTableParam.Add("UserId", this.GetUserCode());
            int iReturnValue = SqlParamDao.ExcuteSP(strSQL_ExecuteSP, hsTableParam);

            sbSql.Append("select * from "+strTableName);

            DataTable dt = SqlParamDao.GetDataTableBySql(sbSql.ToString());
            int iColCount = dt.Columns.Count;
            StringBuilder sBuilder = new StringBuilder();
            //sBuilder.Append("jsonpCallback( ");
            sBuilder.Append("{");
            sBuilder.Append(WebCommon.GetJsonStringByDataTable(dt,"ResultData",true));
            sBuilder.Append("}");
            //sBuilder.Append(")");

            string json = sBuilder.ToString();
            log.Error("获取已注册微信公众号的手机号列表返回值:" + json);

            context.Response.Write(json);

        }
        catch (Exception ex)
        {
            log.Error("客户端页面StaffMobile调用，从远程服务器上获取已注册微信公众号的手机号列表出错:" + ex);
            //context.Response.Write("jsonpCallback({'ResultData':'error'})");
            context.Response.Write("error");
        }
    }


    public bool IsReusable {
        get {
            return false;
        }
    }

}