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

        string strWXMPModule = hsTableUrlQuery["wxmpmodule"] == null ? string.Empty : hsTableUrlQuery["wxmpmodule"].ToString();//param

        string strMobileNo = hsTableUrlQuery["mobileno"] == null ? string.Empty : hsTableUrlQuery["mobileno"].ToString();//param
        string strProjectId = hsTableUrlQuery["projectid"] == null ? string.Empty : hsTableUrlQuery["projectid"].ToString();//param
        string strBusinessType = hsTableUrlQuery["businesstype"] == null ? string.Empty : hsTableUrlQuery["businesstype"].ToString();//param
        string strTID = hsTableUrlQuery["tid"] == null ? string.Empty : hsTableUrlQuery["tid"].ToString();//tid
        string strGID = hsTableUrlQuery["gid"] == null ? string.Empty : hsTableUrlQuery["gid"].ToString();//gid
        string strPID = hsTableUrlQuery["pid"] == null ? string.Empty : hsTableUrlQuery["pid"].ToString();//gid

        if (strParam.Equals("getremotetmpd"))
        {
            this.GetRemoteTMPD(context,strProjectId,strBusinessType,strTID,strGID);
        }else if (strParam.Equals("uploaddatatoremote"))
        {
            //上传TMPD及对应的LSTD数据到远程服务器
            this.DoUpdateDataToRemote(context, strProjectId,strBusinessType);
        }else if (strParam.Equals("deleteremotetmpd"))
        {
            //删除远程服务器TMPD中的数据
            this.DeleteRemoteTMPD(context, strProjectId,strBusinessType,strTID,strGID,strPID);
        }
    }


    /// <summary>
    /// 从远程服务器上获取对应ProjectId及对应业务的TMPD列表
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strProjectId"></param>
    /// <param name="strBusinessType"></param>
    /// <param name="strTID"></param>
    /// <param name="strGID"></param>
    private void GetRemoteTMPD(HttpContext context,String strProjectId,String strBusinessType,String strTID,String strGID)
    {
        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select * from TB_Remote_HRTMPD where ProjectId = '"+strProjectId+"' and BusinessType = '"+strBusinessType+"';");
            if(!String.IsNullOrEmpty(strTID)){
                sbSql.Append(" and TID = '"+strTID+"';");
            }if(!String.IsNullOrEmpty(strTID)){
                sbSql.Append(" and GID = '"+strGID+"';");
            }

            DataTable dt = SqlParamDao.GetDataTableBySql(sbSql.ToString());
            int iColCount = dt.Columns.Count;
            StringBuilder sBuilder = new StringBuilder();
            //sBuilder.Append("jsonpCallback( ");
            sBuilder.Append("{");
            sBuilder.Append(WebCommon.GetJsonStringByDataTable(dt,"ResultData",true));
            sBuilder.Append("}");
            //sBuilder.Append(")");

            string json = sBuilder.ToString();
            log.Error("从远程服务器上获取对应ProjectId及对应业务的TMPD列表返回值:" + json);

            context.Response.Write(json);

        }
        catch (Exception ex)
        {
            log.Error("客户端页面从远程服务器上获取对应ProjectId及对应业务的TMPD列表列表出错:" + ex);
            //context.Response.Write("jsonpCallback({'ResultData':'error'})");
            context.Response.Write("error");
        }
    }

    /// <summary>
    /// 上传TMPD及对应的LSTD数据到远程服务器
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strProjectId"></param>
    /// <param name="strBusinessType"></param>
    private void DoUpdateDataToRemote(HttpContext context,String strProjectId,String strBusinessType)
    {
        int iReturnResult = -1;
        StringBuilder sBuilder = new StringBuilder();
        try
        {
            String strResultData_TMPD = context.Request["txt_LocalTMPDData"].ToString();
            String strResultData_LSTD = context.Request["txt_LocalLSTDData"].ToString();
            //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
            strResultData_TMPD = SQLInjectionDefense.ReplaceSQLReservedKeyword(strResultData_TMPD);
            strResultData_LSTD = SQLInjectionDefense.ReplaceSQLReservedKeyword(strResultData_LSTD);

            if (!String.IsNullOrEmpty(strResultData_TMPD))
            {
                String strTableName_TMPD = "TB_Remote_HRTMPD";
                String strTableName_LSTD = "TB_Remote_HRLSTD";

                StringBuilder sbSqlDelete_TMPD = new StringBuilder();
                StringBuilder sbSqlInsert_TMPD = new StringBuilder();
                StringBuilder sbSqlDelete_LSTD = new StringBuilder();
                StringBuilder sbSqlInsert_LSTD = new StringBuilder();

                JArray jsonArray_TMPD = (JArray)JsonConvert.DeserializeObject(strResultData_TMPD);
                JArray jsonArray_LSTD = (JArray)JsonConvert.DeserializeObject(strResultData_LSTD);

                int iCount_TMPD = jsonArray_TMPD.Count;
                int iCount_LSTD = jsonArray_LSTD.Count;
                log.Error("上传TMPD数据到远程服务器DoUpdateDataToRemote(ProjectId:" + strProjectId + "),本次记录数：" + iCount_TMPD.ToString());
                log.Error("上传LSTD数据到远程服务器DoUpdateDataToRemote(ProjectId:" + strProjectId + "),本次记录数：" + iCount_LSTD.ToString());

                //先上传TMPD数据
                foreach (JObject itemJArray_TMPD in jsonArray_TMPD)
                {
                    String strTID = "";
                    String strGID = "";
                    StringBuilder sbInsertColumnNames = new StringBuilder();
                    StringBuilder sbInsertColumnValues = new StringBuilder();
                    sbInsertColumnNames.Append("[ProjectId],[BusinessType]");
                    sbInsertColumnValues.Append("'" + strProjectId + "','" + strBusinessType + "'");
                    foreach (var item in itemJArray_TMPD)
                    {
                        string strColumnName = Microsoft.JScript.GlobalObject.unescape(item.Key);
                        string strColumnValue = Microsoft.JScript.GlobalObject.unescape(item.Value.ToString());
                        sbInsertColumnNames.Append(",[" + strColumnName + "]");
                        sbInsertColumnValues.Append(",'" + strColumnValue + "'");
                        if (strColumnName.ToUpper().Equals("TID"))
                        {
                            strTID = strColumnValue;
                        }
                        if (strColumnName.ToUpper().Equals("GID"))
                        {
                            strGID = strColumnValue;
                        }
                    }
                    sbSqlDelete_TMPD.Append("delete from " + strTableName_TMPD + " where ProjectId = '" + strProjectId + "' and BusinessType = '" + strBusinessType + "' ");
                    sbSqlDelete_TMPD.Append(" AND TID = '" + strTID + "' and GID = '" + strGID + "'; \r\n ");

                    sbSqlInsert_TMPD.Append("insert into " + strTableName_TMPD + " (" + sbInsertColumnNames.ToString() + ")");
                    sbSqlInsert_TMPD.Append(" values (");
                    sbSqlInsert_TMPD.Append(sbInsertColumnValues.ToString());
                    sbSqlInsert_TMPD.Append("); \r\n");
                }
                sbSqlDelete_TMPD.Append(sbSqlInsert_TMPD.ToString());
                
                //先上传LSTD数据
                foreach (JObject itemJArray_LSTD in jsonArray_LSTD)
                {
                    String strLID = "";
                    StringBuilder sbInsertColumnNames = new StringBuilder();
                    StringBuilder sbInsertColumnValues = new StringBuilder();
                    sbInsertColumnNames.Append("[ProjectId]");
                    sbInsertColumnValues.Append("'" + strProjectId + "'");
                    foreach (var item in itemJArray_LSTD)
                    {
                        string strColumnName = Microsoft.JScript.GlobalObject.unescape(item.Key);
                        string strColumnValue = Microsoft.JScript.GlobalObject.unescape(item.Value.ToString());
                        sbInsertColumnNames.Append(",[" + strColumnName + "]");
                        sbInsertColumnValues.Append(",'" + strColumnValue + "'");
                        if (strColumnName.ToUpper().Equals("LID"))
                        {
                            strLID = strColumnValue;
                        }
                    }
                    sbSqlDelete_LSTD.Append("delete from " + strTableName_LSTD + " where ProjectId = '" + strProjectId + "' ");
                    sbSqlDelete_LSTD.Append(" AND LID = '" + strLID + "'; \r\n ");

                    sbSqlInsert_LSTD.Append("insert into " + strTableName_LSTD + " (" + sbInsertColumnNames.ToString() + ")");
                    sbSqlInsert_LSTD.Append(" values (");
                    sbSqlInsert_LSTD.Append(sbInsertColumnValues.ToString());
                    sbSqlInsert_LSTD.Append("); \r\n");
                }
                sbSqlDelete_LSTD.Append(sbSqlInsert_LSTD.ToString());

                String strInsertSql = sbSqlDelete_TMPD.ToString()+"; \r\n \r\n"+sbSqlDelete_LSTD.ToString();

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
                        log.Error("上传TMPD及对应的LSTD数据到远程服务器出错，SQL语句执行失败\r\n");
                        log.Error("SQL语句:" + strInsertSql + "\r\n");
                        log.Error("错误信息：" + ex.ToString());
                    }
                }
            }else{
                iReturnResult = -9;
            }
        }
        catch (Exception ex)
        {
            log.Error("上传TMPD及对应的LSTD数据到远程服务器出错，解析薪资数据json失败\r\n");
            log.Error("错误信息："+ex.ToString());
        }
        finally
        {
            context.Response.Write(iReturnResult);
        }
    }
    
    /// <summary>
    /// 删除远程服务器TMPD中的数据
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strProjectId"></param>
    /// <param name="strBusinessType"></param>
    /// <param name="strTID"></param>
    /// <param name="strGID"></param>
    /// <param name="strPID"></param>
    private void DeleteRemoteTMPD(HttpContext context,String strProjectId,String strBusinessType,String strTID,String strGID,String strPID)
    {
        int iReturnResult = -1;
        StringBuilder sBuilder = new StringBuilder();
        try
        {
            String strTableName_TMPD = "TB_Remote_HRTMPD";
            StringBuilder sbSqlDelete_TMPD = new StringBuilder();
        
            sbSqlDelete_TMPD.Append("delete from " + strTableName_TMPD + " where ProjectId = '" + strProjectId + "' and BusinessType = '" + strBusinessType + "' ");
            if(!String.IsNullOrEmpty(strTID)){
                sbSqlDelete_TMPD.Append(" AND TID = '" + strTID + "' \r\n ");
                if(!String.IsNullOrEmpty(strGID)){
                    sbSqlDelete_TMPD.Append(" AND GID = '" + strGID + "' \r\n ");
                    if(!String.IsNullOrEmpty(strPID)){
                        sbSqlDelete_TMPD.Append(" AND PID = '" + strPID + "' \r\n ");
                    }
                }
            }
             
            try
            {
                int iReturnCount = SqlParamDao.ExecuteNonQueryBySql(sbSqlDelete_TMPD.ToString());
                if (iReturnCount >= 0)
                {
                    iReturnResult = iReturnCount;
                }
            }
            catch (Exception ex)
            {
                log.Error("删除远程服务器TMPD中的数据出错，SQL语句执行失败\r\n");
                log.Error("SQL语句:" + sbSqlDelete_TMPD.ToString() + "\r\n");
                log.Error("错误信息：" + ex.ToString());
            }
        }
        catch (Exception ex)
        {
            log.Error("删除远程服务器TMPD中的数据出错出错，解析薪资数据json失败\r\n");
            log.Error("错误信息："+ex.ToString());
        }
        finally
        {
            context.Response.Write(iReturnResult);
        }
    }


    public bool IsReusable {
        get {
            return false;
        }
    }

}