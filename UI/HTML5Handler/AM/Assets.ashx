<%@ WebHandler Language="C#" Class="Assets" %>

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

public class Assets : HandlerBase, IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    #region 日志声明
    /// <summary>
    /// 日志声明
    /// </summary>
    private Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    #endregion

    public void ProcessRequest (HttpContext context) {

        Hashtable hsTableUrlQuery = this.GetUrlAnalyse(context);

        //string param = hsTableUrlQuery["param"] == null ? string.Empty : hsTableUrlQuery["param"].ToString();//请求类型参数
        //string strUserID = hsTableUrlQuery["userid"] == null ? string.Empty : hsTableUrlQuery["userid"].ToString();//登录名
        //string strPassword = hsTableUrlQuery["password"] == null ? string.Empty : hsTableUrlQuery["password"].ToString();//密码
        //string strRequestLanguage = hsTableUrlQuery["language"] == null ? string.Empty : hsTableUrlQuery["language"].ToString();//终端请求时的语言
        //int iPageSize = hsTableUrlQuery["pagesize"] == null ? 20 : int.Parse(hsTableUrlQuery["pagesize"].ToString());//每页条数
        //int iPageIndex = hsTableUrlQuery["pageindex"] == null ? 0 : int.Parse(hsTableUrlQuery["pageindex"].ToString());//分页起始index
        string strCondition = hsTableUrlQuery["condition"] == null ? string.Empty : hsTableUrlQuery["condition"].ToString();//查询条件


        string param = context.Request["param"] == null ? string.Empty : context.Request["param"].ToString();//请求类型参数
        int iPageSize = context.Request["pagesize"] == null ? 20 : int.Parse(context.Request["pagesize"].ToString());//每页条数
        string strRequestLanguage = context.Request["language"] == null ? string.Empty : context.Request["language"].ToString();//终端请求时的语言
        int iPageIndex = context.Request["pageindex"] == null ? 0 : int.Parse(context.Request["pageindex"].ToString());//分页起始index
        string strQueryLocation = context.Request["queryLocation"] == null ? string.Empty : context.Request["queryLocation"].ToString();//请求类型参数
        string strQueryDept = context.Request["queryDept"] == null ? string.Empty : context.Request["queryDept"].ToString();//请求类型参数
        string strQueryAssetsCode = context.Request["queryAssetsCode"] == null ? string.Empty : context.Request["queryAssetsCode"].ToString();//请求类型参数
        string strQueryAssetsName = context.Request["queryAssetsName"] == null ? string.Empty : context.Request["queryAssetsName"].ToString();//请求类型参数
        
        //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
        param = SQLInjectionDefense.ReplaceSQLReservedKeyword(param);
        strRequestLanguage = SQLInjectionDefense.ReplaceSQLReservedKeyword(strRequestLanguage);
        strQueryLocation = SQLInjectionDefense.ReplaceSQLReservedKeyword(strQueryLocation);
        strQueryDept = SQLInjectionDefense.ReplaceSQLReservedKeyword(strQueryDept);
        strQueryAssetsCode = SQLInjectionDefense.ReplaceSQLReservedKeyword(strQueryAssetsCode);
        strQueryAssetsName = SQLInjectionDefense.ReplaceSQLReservedKeyword(strQueryAssetsName);

        switch (param.ToLower().ToString())
        {
            case "queryassets":
                this.GetAssetsInfo(context, iPageSize, iPageIndex, strRequestLanguage, strQueryLocation, strQueryDept, strQueryAssetsCode, strQueryAssetsName);
                break;
            case "getlocations":
                this.GetLocationsInfo(context, strRequestLanguage, strCondition);
                break;
            case "getdepts":
                this.GetDeptsInfo(context, strRequestLanguage, strCondition);
                break;
            default:
                this.GetAssetsInfo(context, iPageSize, iPageIndex, strRequestLanguage, strQueryLocation, strQueryDept, strQueryAssetsCode, strQueryAssetsName);
                break;
        }
    }


    /// <summary>
    /// 获取资产信息
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strRequestLanguage"></param>
    public void GetAssetsInfo(HttpContext context,int iPageSize,int iPageIndex, String strRequestLanguage,String  strQueryLocation, String strQueryDept, String strQueryAssetsCode, String strQueryAssetsName)
    {
        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select * from [VW_Moblie_AssetDetail] where 1=1");
            if (!String.IsNullOrEmpty(strQueryLocation))
            {
                sbSql.Append(" AND SLCODE = '" + strQueryLocation + "'");
            }
            if (!String.IsNullOrEmpty(strQueryDept))
            {
                sbSql.Append(" AND SUSEDEPT = '" + strQueryDept + "'");
            }
            if (!String.IsNullOrEmpty(strQueryAssetsCode))
            {
                sbSql.Append(" AND SACODE = '" + strQueryAssetsCode + "'");
            }
            if (!String.IsNullOrEmpty(strQueryAssetsName))
            {
                if (strRequestLanguage.Equals("en-us"))
                {
                    sbSql.Append(" AND SANAME like '%" + strQueryAssetsName + "%'");
                }
                else
                {
                    sbSql.Append(" AND SANAMECN like '%" + strQueryAssetsName + "%'");
                }
            }

            log.Error(sbSql.ToString());
            String strCountSql = PagingSqlUtil.GetCountSql(sbSql.ToString());
            int iRecordCount = SqlParamDao.ExecuteScalarBySql(strCountSql);
            String strSql = PagingSqlUtil.GetPagingSql(sbSql.ToString(), iPageSize, iPageIndex, "SACODE", "SACODE");

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
    /// 获取存放地址信息
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strRequestLanguage"></param>
    public void GetLocationsInfo(HttpContext context,String strRequestLanguage, String strCondition)
    {
        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select *");
            //if (strRequestLanguage.Equals("zh-cn"))
            //{
            //    sbSql.Append(", SNAMECN AS LocationName");
            //}
            //else
            //{
            //    sbSql.Append(", [SNAME] AS LocationName");
            //}
            sbSql.Append(" from [VW_Moblie_Location] WHERE BISUSE = '1' and [ISLASTLEVEL] = '1' ORDER BY LORDER");
            if (!String.IsNullOrEmpty(strCondition))
            {
                sbSql.Append(" AND " + strCondition);
            }

            DataTable dtResultData = SqlParamDao.GetDataTableBySql(sbSql.ToString());

            string json = WebCommon.GetDataRowJsonString(dtResultData, "ResultData", "");

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
    /// 获取部门信息
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strRequestLanguage"></param>
    public void GetDeptsInfo(HttpContext context, String strRequestLanguage, String strCondition)
    {
        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select *");
            //if (strRequestLanguage.Equals("en-us"))
            //{
            //    sbSql.Append(", ODESC AS 'DepartmentName'");
            //}
            //else
            //{
            //    sbSql.Append(", ODESCCHS AS 'DepartmentName'");
            //}
            sbSql.Append(" from [VW_Moblie_Dept] WHERE BISSTOP <> '1' and [ISLASTLEVEL] = '1' ORDER BY OORDER");
            if (!String.IsNullOrEmpty(strCondition))
            {
                sbSql.Append(" AND " + strCondition);
            }

            DataTable dtResultData = SqlParamDao.GetDataTableBySql(sbSql.ToString());

            string json = WebCommon.GetDataRowJsonString(dtResultData, "ResultData", "");

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