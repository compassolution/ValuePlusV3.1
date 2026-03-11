<%@ WebHandler Language="C#" Class="Menu" %>

using System;
using System.Web;
using System.Collections;
using System.Text;
using System.Data;
using Com.ValuePlus.DataLog;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Web;
using Com.ValuePlus.Common.Security;

public class Menu : HandlerBase,IHttpHandler, System.Web.SessionState.IRequiresSessionState {

    #region 日志声明
    /// <summary>
    /// 日志声明
    /// </summary>
    private Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    #endregion

    public void ProcessRequest (HttpContext context) {
        Hashtable hsTableUrlQuery = this.GetUrlAnalyse(context);

        string param = context.Request["param"] == null ? string.Empty : context.Request["param"].ToString();//请求类型参数
        string strUserID = context.Request["userid"] == null ? string.Empty : context.Request["userid"].ToString();//登录名
        string strParentMenuCode = context.Request["parentcode"] == null ? string.Empty : context.Request["parentcode"].ToString();//父级栏目编码
        string strRequestLanguage = context.Request["language"] == null ? string.Empty : context.Request["language"].ToString();//终端请求时的语言
        
        //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
        param = SQLInjectionDefense.ReplaceSQLReservedKeyword(param);
        strUserID = SQLInjectionDefense.ReplaceSQLReservedKeyword(strUserID);
        strParentMenuCode = SQLInjectionDefense.ReplaceSQLReservedKeyword(strParentMenuCode);
        strRequestLanguage = SQLInjectionDefense.ReplaceSQLReservedKeyword(strRequestLanguage);

        switch (param.ToLower().ToString())
        {
            case "level1menu":
                this.GetMenuInfo(context, strUserID,"1","", false,"",strRequestLanguage);
                break;
            case "mainshowmenu":
                this.GetMenuInfo(context, strUserID,"2","", true,"",strRequestLanguage);
                break;
            case "level2menu":
                this.GetMenuInfo(context, strUserID,"2","", false,strParentMenuCode,strRequestLanguage);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 获取栏目菜单信息
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strUserID"></param>
    /// <param name="strLevel"></param>
    /// <param name="strMenuCode"></param>
    /// <param name="IsMainShow"></param>
    /// <param name="strParentMenuCode"></param>
    /// <param name="strRequestLanguage"></param>
    public void GetMenuInfo(HttpContext context,String strUserID ,String strLevel,String strMenuCode,bool IsMainShow,String strParentMenuCode,String strRequestLanguage)
    {
        try
        {
            StringBuilder sbSql = new StringBuilder();
            string strNameColumn = strRequestLanguage.ToLower().Equals("zh-cn")?"A.TREENAMECHS":"A.TREENAME";

            if (strUserID.ToLower().Equals("admin"))
            {
                sbSql.Append("select A.*,"+strNameColumn+" AS [MenuName] from TREE_MobileMenu A where A.BISSTOP <> '1'");
            }else
            {
                sbSql.Append("select A.*,"+strNameColumn+" AS [MenuName]  from TREE_MobileMenu A INNER JOIN MBUser_2 B ON A.TREECODE = B.TREECODE ");
                sbSql.Append(" INNER JOIN MBUser_1 C ON B.DCNO = C.DCNO");
                sbSql.Append(" where A.BISSTOP <> '1' ");
                sbSql.Append(" AND (B.DCNO = '"+strUserID+"' or C.DCMOBILE = '"+strUserID+"' or C.SUSERID = '"+strUserID+"')");
            }
            if (!String.IsNullOrEmpty(strLevel))
            {
                sbSql.Append(" AND A.TREELEVEL = '"+strLevel+"'");
            }
            if (!String.IsNullOrEmpty(strMenuCode))
            {
                sbSql.Append(" AND A.TREECODE = '"+strMenuCode+"'");
            }
            if (IsMainShow)
            {
                sbSql.Append(" AND A.IsMainShow = '1'");
            }
            if (!String.IsNullOrEmpty(strParentMenuCode))
            {
                sbSql.Append(" AND A.PARENTCODE = '"+strParentMenuCode+"'");
            }

            sbSql.Append(" ORDER BY A.TREEORDER");

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