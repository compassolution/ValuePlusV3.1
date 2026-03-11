<%@ WebHandler Language="C#" Class="Payroll" %>

using System;
using System.Web;
using System.Text;
using System.Data;
using System.IO;
using System.Collections;
using Com.ValuePlus.Web;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Weixin;
using Com.ValuePlus.Utils.Serializable;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Com.ValuePlus.Common.Security;

public class Payroll : HandlerBase, IHttpHandler, System.Web.SessionState.IRequiresSessionState {

    public void ProcessRequest (HttpContext context) {
        Hashtable hsTableUrlQuery = this.GetUrlAnalyse(context);
        string strParam = context.Request["param"] == null ? string.Empty : context.Request["param"].ToString();//请求类型参数
        string strLanguage = context.Request["language"] == null ? string.Empty : context.Request["language"].ToString();//请求类型参数
        string strMobileNo = context.Request["mobileno"] == null ? string.Empty : context.Request["mobileno"].ToString();//请求类型参数
        string strProjectId = context.Request["projectid"] == null ? string.Empty : context.Request["projectid"].ToString();//请求类型参数
        string strYearMonthType = context.Request["yearmonthtype"] == null ? string.Empty : context.Request["yearmonthtype"].ToString();//请求类型参数
        string strYearMonth = context.Request["yearmonth"] == null ? string.Empty : context.Request["yearmonth"].ToString();//请求类型参数
        
        //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
        strParam = SQLInjectionDefense.ReplaceSQLReservedKeyword(strParam);
        strLanguage = SQLInjectionDefense.ReplaceSQLReservedKeyword(strLanguage);
        strMobileNo = SQLInjectionDefense.ReplaceSQLReservedKeyword(strMobileNo);
        strProjectId = SQLInjectionDefense.ReplaceSQLReservedKeyword(strProjectId);
        strYearMonthType = SQLInjectionDefense.ReplaceSQLReservedKeyword(strYearMonthType);
        strYearMonth = SQLInjectionDefense.ReplaceSQLReservedKeyword(strYearMonth);

        log.Error("Payroll.ashx,MobileNo:"+strMobileNo+";Language:"+strLanguage);
        if (strParam.Equals("getlastoneprojectmonth"))
        {
            this.GetLastOneProjectMonth(context,strMobileNo,strLanguage);
        }
        else if (strParam.Equals("getprojectidlist"))
        {
            this.GetProjectList(context,strMobileNo,strLanguage);
        }
        else if (strParam.Equals("getyearmonth"))
        {
            this.GetYearMonthList(context,strMobileNo,strProjectId);
        }
        else if (strParam.Equals("getstaffpayrolldetail"))
        {
            this.GetStaffPayrollDetail(context,strMobileNo,strProjectId,strYearMonth);
        }
        else if (strParam.Equals("setfirstviewtime"))
        {
            //设置员工查看对应薪资期间的员工薪资条的第一次查看时间
            this.SetFirstViewTime(context,strMobileNo,strProjectId,strYearMonth);
        }
        else if (strParam.Equals("staffconfirmpayroll"))
        {
            //设置员工确认对应薪资期间的员工薪资条的确认时间
            this.StaffConfirmPayroll(context,strMobileNo,strProjectId,strYearMonth);
        }
    }

    /// <summary>
    /// 获取最后一个就职单位的最后一个月份信息
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strMobileNo"></param>
    /// <param name="strLanguage"></param>
    private void GetLastOneProjectMonth(HttpContext context,String strMobileNo,String strLanguage)
    {
        try
        {
            log.Error("获取最后一个就职单位的最后一个月份信息,MobileNo:"+strMobileNo+";Language:"+strLanguage);
            StringBuilder sbSql = new StringBuilder();
            if (strLanguage.Equals("zh-cn"))
            {
                sbSql.Append("select TOP 1 ProjectId,ProjectNameChs as ProjectName,YEARMONTH from [TB_Remote_StaffSalaryData] where StaffMobile = '"+strMobileNo+"' order by YEARMONTH DESC");
            }else
            {
                sbSql.Append("select TOP 1 ProjectId,ProjectName,YEARMONTH from [TB_Remote_StaffSalaryData] where StaffMobile = '"+strMobileNo+"' order by YEARMONTH DESC");
            }
            String strSql = sbSql.ToString();
            log.Error("获取最后一个就职单位的最后一个月份信息:"+strSql);

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
    /// 获取就职单位列表
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strMobileNo"></param>
    /// <param name="strLanguage"></param>
    private void GetProjectList(HttpContext context,String strMobileNo,String strLanguage)
    {
        try
        {
            log.Error("获取就职单位列表,MobileNo:"+strMobileNo+";Language:"+strLanguage);
            StringBuilder sbSql = new StringBuilder();
            if (strLanguage.Equals("zh-cn"))
            {
                sbSql.Append("select distinct ProjectId,ProjectNameChs as ProjectName from [TB_Remote_StaffSalaryData] where StaffMobile = '"+strMobileNo+"'");
            }else
            {
                sbSql.Append("select distinct ProjectId,ProjectName from [TB_Remote_StaffSalaryData] where StaffMobile = '"+strMobileNo+"'");
            }
            String strSql = sbSql.ToString();
            log.Error("获取就职单位列表:"+strSql);

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
    /// 获取薪资期间列表
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strMobileNo"></param>
    /// <param name="strProjectId"></param>
    private void GetYearMonthList(HttpContext context,String strMobileNo,String strProjectId)
    {
        try
        {
            String strCurYearMonth = "";
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select distinct YEARMONTH,PSTART,PEND from [TB_Remote_StaffSalaryData] where 1=1  ");
            sbSql.Append(" AND StaffMobile = '"+strMobileNo+"'");
            sbSql.Append(" AND ProjectId = '"+strProjectId+"'");
            sbSql.Append(" order by YEARMONTH desc");
            String strSql = sbSql.ToString();

            log.Error("获取薪资期间列表:"+strSql);
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);

            int iColCount = dt.Columns.Count;
            StringBuilder sBuilder = new StringBuilder();
            sBuilder.Append("{YearMonthList:[ ");
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
                    if (i == 0)
                    {
                        strCurYearMonth = dt.Rows[i]["YEARMONTH"].ToString();
                    }

                    sBuilder.Append("}");
                }

            }
            sBuilder.Append("]");
            //获取当前登录用户当月在职信息
            String strStaffDutyInfo = GetCurStaffDutyInfo(context,strMobileNo,strProjectId,strCurYearMonth);
            if (!String.IsNullOrEmpty(strStaffDutyInfo))
            {
                sBuilder.Append("," + strStaffDutyInfo);
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
    /// 获取当前登录用户当月在职信息
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strMobileNo"></param>
    /// <param name="strProjectId"></param>
    /// <param name="strYearMonth"></param>
    /// <returns></returns>
    public String GetCurStaffDutyInfo(HttpContext context,String strMobileNo,String strProjectId,String strYearMonth)
    {
        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select top 1 * from [TB_Remote_StaffSalaryData] where 1=1  ");
            sbSql.Append(" AND StaffMobile = '"+strMobileNo+"'");
            sbSql.Append(" AND ProjectId = '"+strProjectId+"'");
            sbSql.Append(" AND YEARMONTH = '"+strYearMonth+"'");
            String strSql = sbSql.ToString();
            log.Error("获取当前登录用户当月在职信息:"+strSql);

            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);

            int iColCount = dt.Columns.Count;
            StringBuilder sBuilder = new StringBuilder();
            sBuilder.Append("StaffDutyInfo:[ ");
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

            string json = sBuilder.ToString();

            return json;
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return "";
        }
    }

    /// <summary>
    /// 获取员工对应薪资期间的员工薪资条明细
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strMobileNo"></param>
    /// <param name="strProjectId"></param>
    /// <param name="strYearMonth"></param>
    private void GetStaffPayrollDetail(HttpContext context,String strMobileNo,String strProjectId,String strYearMonth)
    {
        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select * from [TB_Remote_StaffSalaryData] where 1=1  ");
            sbSql.Append(" AND StaffMobile = '"+strMobileNo+"'");
            sbSql.Append(" AND ProjectId = '"+strProjectId+"'");
            sbSql.Append(" AND YEARMONTH = '"+strYearMonth+"'");
            sbSql.Append(" and ISSHOW = 'true' order by ITEMPORDER DESC");
            String strSql = sbSql.ToString();
            log.Error("获取员工对应薪资期间的员工薪资条明细:"+strSql);

            DataTable dt = SqlParamDao.GetDataTableBySql(sbSql.ToString());
            int iColCount = dt.Columns.Count;
            StringBuilder sBuilder = new StringBuilder();
            sBuilder.Append("{ResultData: ");
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
            sBuilder.Append("}");

            string json = sBuilder.ToString();

            //新增用户访问记录信息表的写入 add by sammen 20190919
            WXUser.RecordWXUser_3(strMobileNo,strProjectId,"查看员工薪资条");
            //新增用户访问记录信息表的写入 add by sammen 20190919

            context.Response.Write(json);

        }
        catch (Exception ex)
        {
            log.Error(ex);
            context.Response.Write("-1");
        }
    }

    /// <summary>
    /// 设置员工查看对应薪资期间的员工薪资条的第一次查看时间
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strMobileNo"></param>
    /// <param name="strProjectId"></param>
    /// <param name="strYearMonth"></param>
    private void SetFirstViewTime(HttpContext context,String strMobileNo,String strProjectId,String strYearMonth)
    {
        try
        {
            String strNow = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("UPDATE [TB_Remote_StaffSalaryData] SET FirstViewTime = '"+strNow+"' where 1=1  ");
            sbSql.Append(" AND StaffMobile = '"+strMobileNo+"'");
            sbSql.Append(" AND ProjectId = '"+strProjectId+"'");
            sbSql.Append(" AND YEARMONTH = '"+strYearMonth+"'");
            sbSql.Append(" AND isnull(FirstViewTime,'') = ''");
            String strSql = sbSql.ToString();
            log.Error("设置员工查看对应薪资期间的员工薪资条的第一次查看时间:"+strSql);

            int iCount = SqlParamDao.ExecuteNonQueryBySql(strSql);
            
            string json = "";
            if(iCount>0){
                json = strNow;
            }

            context.Response.Write(json);

        }
        catch (Exception ex)
        {
            log.Error(ex);
            context.Response.Write("");
        }
    }

    /// <summary>
    /// 设置员工确认对应薪资期间的员工薪资条的确认时间
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strMobileNo"></param>
    /// <param name="strProjectId"></param>
    /// <param name="strYearMonth"></param>
    private void StaffConfirmPayroll(HttpContext context,String strMobileNo,String strProjectId,String strYearMonth)
    {
        try
        {
            String strNow = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("UPDATE [TB_Remote_StaffSalaryData] SET StaffConfirmTime = '"+strNow+"' where 1=1  ");
            sbSql.Append(" AND StaffMobile = '"+strMobileNo+"'");
            sbSql.Append(" AND ProjectId = '"+strProjectId+"'");
            sbSql.Append(" AND YEARMONTH = '"+strYearMonth+"'");
            String strSql = sbSql.ToString();
            log.Error("设置员工确认对应薪资期间的员工薪资条的确认时间:"+strSql);

            int iCount = SqlParamDao.ExecuteNonQueryBySql(strSql);
            string json = "";
            if(iCount>0){
                json = strNow;
            }

            context.Response.Write(json);
        }
        catch (Exception ex)
        {
            log.Error(ex);
            context.Response.Write("");
        }
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}