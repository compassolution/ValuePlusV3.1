<%@ WebHandler Language="C#" Class="OE" %>

using System;
using System.Web;
using System.Collections;
using System.Text;
using System.Data;
using Com.ValuePlus.BLL.SysManager;
using Com.ValuePlus.BLL.User;
using Com.ValuePlus.DataLog;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Entity;
using Com.ValuePlus.Web;
using Com.ValuePlus.Archive.Utils;
using Com.ValuePlus.Common.Security;

public class OE : HandlerBase, IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    #region 日志声明
    /// <summary>
    /// 日志声明
    /// </summary>
    private Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    #endregion

    public void ProcessRequest (HttpContext context) {

        Hashtable hsTableUrlQuery = this.GetUrlAnalyse(context);
        string strCondition = hsTableUrlQuery["condition"] == null ? string.Empty : hsTableUrlQuery["condition"].ToString();//查询条件


        string param = context.Request["param"] == null ? string.Empty : context.Request["param"].ToString();//请求类型参数
        int iPageSize = context.Request["pagesize"] == null ? 20 : int.Parse(context.Request["pagesize"].ToString());//每页条数
        string strRequestLanguage = context.Request["language"] == null ? string.Empty : context.Request["language"].ToString();//终端请求时的语言
        int iPageIndex = context.Request["pageindex"] == null ? 0 : int.Parse(context.Request["pageindex"].ToString());//分页起始index
        string strQueryLocation = context.Request["queryLocation"] == null ? string.Empty : context.Request["queryLocation"].ToString();//请求类型参数
        string strQueryDept = context.Request["queryDept"] == null ? string.Empty : context.Request["queryDept"].ToString();//请求类型参数
        string strQueryOECode = context.Request["queryOECode"] == null ? string.Empty : context.Request["queryOECode"].ToString();//请求类型参数
        string strQueryOEName = context.Request["queryOEName"] == null ? string.Empty : context.Request["queryOEName"].ToString();//请求类型参数
        
        //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
        param = SQLInjectionDefense.ReplaceSQLReservedKeyword(param);
        strRequestLanguage = SQLInjectionDefense.ReplaceSQLReservedKeyword(strRequestLanguage);
        strQueryLocation = SQLInjectionDefense.ReplaceSQLReservedKeyword(strQueryLocation);
        strQueryDept = SQLInjectionDefense.ReplaceSQLReservedKeyword(strQueryDept);
        strQueryOECode = SQLInjectionDefense.ReplaceSQLReservedKeyword(strQueryOECode);
        strQueryOEName = SQLInjectionDefense.ReplaceSQLReservedKeyword(strQueryOEName);

        switch (param.ToLower().ToString())
        {
            case "queryoe":
                this.GetOEInfo(context, iPageSize, iPageIndex, strRequestLanguage, strQueryLocation, strQueryDept, strQueryOECode, strQueryOEName);
                break;
            case "queryoeloationqty":
                this.GetOELoactionQtyInfo(context,strRequestLanguage,strQueryOECode);
                break;
            default:
                this.GetOEInfo(context, iPageSize, iPageIndex, strRequestLanguage, strQueryLocation, strQueryDept, strQueryOECode, strQueryOEName);
                break;
        }
    }
        
    /// <summary>
    /// 获取OE资产信息
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strRequestLanguage"></param>
    public void GetOEInfo(HttpContext context,int iPageSize,int iPageIndex, String strRequestLanguage,String  strQueryLocation, String strQueryDept, String strQueryOECode, String strQueryOEName)
    {
        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select * from [VW_Moblie_OE_Item] where 1=1");
            if (!String.IsNullOrEmpty(strQueryLocation))
            {
                sbSql.Append(" AND SACODE IN (SELECT SACODE FROM [VW_Moblie_OE_Location] WHERE SLCODE = '" + strQueryLocation + "')");
            }
            if (!String.IsNullOrEmpty(strQueryDept))
            {
                sbSql.Append(" AND SACODE IN (SELECT SACODE FROM [VW_Moblie_OE_Location] WHERE SUSEDEPT = '" + strQueryDept + "')");
            }
            if (!String.IsNullOrEmpty(strQueryOECode))
            {
                sbSql.Append(" AND SACODE = '" + strQueryOECode + "'");
            }
            if (!String.IsNullOrEmpty(strQueryOEName))
            {
                if (strRequestLanguage.Equals("en-us"))
                {
                    sbSql.Append(" AND SANAME like '%" + strQueryOEName + "%'");
                }
                else
                {
                    sbSql.Append(" AND SANAMECN like '%" + strQueryOEName + "%'");
                }
            }

            //log.Error(sbSql.ToString());
            String strCountSql = PagingSqlUtil.GetCountSql(sbSql.ToString());
            int iRecordCount = SqlParamDao.ExecuteScalarBySql(strCountSql);
            String strSql = PagingSqlUtil.GetPagingSql(sbSql.ToString(), iPageSize, iPageIndex, "SACODE", "SACODE");
            log.Error(strSql);

            DataTable dtResultData = SqlParamDao.GetDataTableBySql(strSql);

            string json = WebCommon.GetDataRowJsonString(dtResultData, "ResultData", "{ResultCount:" + iRecordCount.ToString()+ "}");

            //json = "2";
            context.Response.Write(json);
        }
        catch (Exception ex)
        {
            log.Error(ex);
            context.Response.Write("error");
        }
    }
         
    /// <summary>
    /// 获取OE资产信息
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strRequestLanguage"></param>
    /// <param name="strQueryOECode"></param>
    public void GetOELoactionQtyInfo(HttpContext context,String strRequestLanguage, String strQueryOECode)
    {
        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select top 100 percent * from [VW_Moblie_OE_Location] where SACODE = '"+strQueryOECode+"' ORDER BY SUSEDEPT,SLCODE");
                
            //log.Error(sbSql.ToString());
            String strCountSql = PagingSqlUtil.GetCountSql(sbSql.ToString());
            int iRecordCount = SqlParamDao.ExecuteScalarBySql(strCountSql);
            String strSql = sbSql.ToString();
            log.Error(strSql);

            DataTable dtResultData = SqlParamDao.GetDataTableBySql(strSql);

            string json = WebCommon.GetDataRowJsonString(dtResultData, "ResultData", "{ResultCount:" + iRecordCount.ToString()+ "}");

            //json = "2";
            context.Response.Write(json);
        }
        catch (Exception ex)
        {
            log.Error(ex);
            context.Response.Write("error");
        }
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}