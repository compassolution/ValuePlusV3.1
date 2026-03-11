<%@ WebHandler Language="C#" Class="DataGetter" %>

using System;
using System.Web;
using System.Collections;
using System.Xml;
using System.Net;
using System.IO;
using System.Text;
using System.Data;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Web;
using Com.ValuePlus.Common.Security;

public class DataGetter : HandlerBase, IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    #region 日志声明
    /// <summary>
    /// 日志声明
    /// </summary>
    private Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    #endregion

    public void ProcessRequest(HttpContext context)
    {
        Hashtable hsTableUrlQuery = this.GetUrlAnalyse(context);
        string param = context.Request["param"] == null ? string.Empty : context.Request["param"].ToString();//param
        string strDataScope = context.Request["datascope"] == null ? string.Empty : context.Request["datascope"].ToString();//
        string strCompanyType = context.Request["companytype"] == null ? string.Empty : context.Request["companytype"].ToString();//
        
        //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
        param = SQLInjectionDefense.ReplaceSQLReservedKeyword(param);
        strDataScope = SQLInjectionDefense.ReplaceSQLReservedKeyword(strDataScope);
        strCompanyType = SQLInjectionDefense.ReplaceSQLReservedKeyword(strCompanyType);

        //log.Error(strMobileNo);
        switch (param.ToLower().ToString())
        {
            case "querynationdata":
                this.QueryNationDataList(context, strDataScope, strCompanyType);
                break;
            case "getprovincelist":
                this.QueryProvinceList(context);
                break;
        }
    }

    /// <summary>
    /// 查询全国流量包
    /// </summary>
    /// <param name="context"></param>
    private void QueryProvinceList(HttpContext context)
    {
        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select * from TB_HRLSTD WHERE LID = 'NATIVE' and BISSTOP <> '1' ORDER BY CID");
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
    /// 查询全国流量包
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strDataScope"></param>
    /// <param name="strCompanyType"></param>
    private void QueryNationDataList(HttpContext context, String strDataScope, String strCompanyType)
    {
        try
        {
            StringBuilder sbSql = new StringBuilder();

            if (strDataScope.ToLower().Equals("nation"))
            {
                sbSql.Append("select * from NationData_1 where IsValid = 'true' and Operator = '"+strCompanyType+"' order by DataValue,SalePrice");
            }

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
    
        
    public bool IsReusable {
        get {
            return false;
        }
    }

}