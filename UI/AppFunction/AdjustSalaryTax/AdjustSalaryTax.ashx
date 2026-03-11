<%@ WebHandler Language="C#" Class="AdjustSalaryTax" %>

using System;
using System.Web;
using System.Text;
using System.Data;
using System.Collections;
using Com.ValuePlus.Web;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Common.Security;

public class AdjustSalaryTax : HandlerBase, IHttpHandler, System.Web.SessionState.IRequiresSessionState {

    public void ProcessRequest (HttpContext context) {
        Hashtable hsTableUrlQuery = this.GetUrlAnalyse(context);
        string strParam = hsTableUrlQuery["param"] == null ? string.Empty : hsTableUrlQuery["param"].ToString();//param
        String strKeyValue = hsTableUrlQuery["keyvalue"] == null ? string.Empty : hsTableUrlQuery["keyvalue"].ToString();
        String strTaxYear = hsTableUrlQuery["taxyear"] == null ? string.Empty : hsTableUrlQuery["taxyear"].ToString();
        String strYearMonth = hsTableUrlQuery["yearmonth"] == null ? string.Empty : hsTableUrlQuery["yearmonth"].ToString();
        String strDCNO = hsTableUrlQuery["dcno"] == null ? string.Empty : hsTableUrlQuery["dcno"].ToString();
        String strCtrlId = hsTableUrlQuery["ctrlid"] == null ? string.Empty : hsTableUrlQuery["ctrlid"].ToString();
        String strCtrlValue = hsTableUrlQuery["ctrlvalue"] == null ? string.Empty : hsTableUrlQuery["ctrlvalue"].ToString();

        if (String.IsNullOrEmpty(this.GetUserCode()))
        {
            context.Response.Write("-9999");
        }else
        {
            if (strParam.Equals("taxyearlist"))
            {
                this.QueryTaxYearList(context);
            }else if (strParam.Equals("stafflist"))
            {
                this.QueryStaffListByTaxYear(context,strTaxYear);
            }else if (strParam.Equals("stafftaxdata"))
            {
                this.QueryTaxDataList(context,strTaxYear,strDCNO);
            }else if (strParam.Equals("savetaxdata"))
            {
                this.SaveTaxData(context);
            }else if (strParam.Equals("calculatetaxdata"))
            {
                this.CalculateTaxData(context);
            }
        }
    }

    /// <summary>
    /// 查询纳税年度列表数据
    /// </summary>
    /// <param name="context"></param>
    private void QueryTaxYearList(HttpContext context)
    {
        StringBuilder sbSql = new StringBuilder();
        try
        {
            sbSql.Append("SELECT distinct ISNULL(TAXYEAR,'20'+LEFT(PID,2)) AS TaxYear ");
            sbSql.Append(", (case when convert(varchar(20),year(getdate())) = ISNULL(TAXYEAR,'20'+LEFT(PID,2)) then '1' else '0' end) as IsNow ");
            sbSql.Append(" FROM KQPERD_1 WHERE ISNULL(TAXYEAR,'20'+LEFT(PID,2))>='2019'");
            String strSql = sbSql.ToString();

            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
            int iRowsCount = 0;
            if (dt != null)
            {
                iRowsCount = dt.Rows.Count;
            }

            int iColCount = dt.Columns.Count;
            StringBuilder sBuilder = new StringBuilder();

            sBuilder.Append("{");
            sBuilder.Append(WebCommon.GetJsonStringByDataTable(dt,"\"ResultData\"",true));
            sBuilder.Append("}");

            string json = sBuilder.ToString();

            //同时设置当前用户对历史调整数据的角色
            this.SetUserRole(context);

            context.Response.Write(json);
        }
        catch (Exception ex)
        {
            context.Response.Write("-1");
            log.Error(ex);
            log.Error("查询纳税年度列表数据,SQL："+sbSql.ToString());
        }
    }

    /// <summary>
    /// 查询某计税年度月份下的员工列表数据
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strTaxYear"></param>
    private void QueryStaffListByTaxYear(HttpContext context, String strTaxYear)
    {
        StringBuilder sbSql = new StringBuilder();
        try
        {
            sbSql.Append("select distinct DCNO,DCNAMECHS from TB_HR_MonthStaffTax  ");
            sbSql.Append(" where [Year] = '"+strTaxYear+"'");
            sbSql.Append(" order by DCNO ");
            String strSql = sbSql.ToString();

            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
            int iRowsCount = 0;
            if (dt != null)
            {
                iRowsCount = dt.Rows.Count;
            }

            int iColCount = dt.Columns.Count;
            StringBuilder sBuilder = new StringBuilder();

            sBuilder.Append("{");
            sBuilder.Append(WebCommon.GetJsonStringByDataTable(dt,"\"ResultData\"",true));
            sBuilder.Append("}");

            string json = sBuilder.ToString();

            context.Response.Write(json);
        }
        catch (Exception ex)
        {
            context.Response.Write("-1");
            log.Error(ex);
            log.Error("查询某计税年度月份下的员工列表数据,SQL："+sbSql.ToString());
        }
    }


    /// <summary>
    /// 查询某员工某计税年度个税情况列表数据
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strYearMonth"></param>
    /// <param name="strTaxYear"></param>
    private void QueryTaxDataList(HttpContext context, String strTaxYear,String strDCNO)
    {
        StringBuilder sbSql = new StringBuilder();
        try
        {
            //首先执行一个查询存储过程获得数据集
            String strExecuteSql = "exec [USP_HR_QRY_PR_StaffYearTax] '"+strTaxYear+"','"+strDCNO+"'";
            SqlParamDao.ExecuteNonQueryBySql(strExecuteSql);

            String strTableName = "_USP_HR_QRY_PR_StaffYearTax";
            sbSql.Append("select * from "+strTableName);
            String strSql = sbSql.ToString();
            DataTable dt_Data = SqlParamDao.GetDataTableBySql(strSql);

            StringBuilder sBuilder = new StringBuilder();
            sBuilder.Append("{");
            sBuilder.Append(WebCommon.GetJsonStringByDataTable(dt_Data,"\"ResultData\"",true));
            sBuilder.Append("}");

            string json = sBuilder.ToString();

            context.Response.Write(json);
        }
        catch (Exception ex)
        {
            context.Response.Write("-1");
            log.Error(ex);
            log.Error("查询某员工某计税年度个税情况列表数据,SQL："+sbSql.ToString());
        }
    }


    /// <summary>
    /// 保存个税数据
    /// </summary>
    /// <param name="context"></param>
    private void SaveTaxData(HttpContext context)
    {
        int iReturn = -1;
        StringBuilder sbSql = new StringBuilder();
        try
        {
            String strModifyData = context.Request["txt_ModifyData"].ToString();
            //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
            strModifyData = SQLInjectionDefense.ReplaceSQLReservedKeyword(strModifyData);

            if (!String.IsNullOrEmpty(strModifyData))
            {
                String[] AllDataArrary = strModifyData.Split('；');
                if (AllDataArrary.Length > 0 )
                {
                    for (int i = 0; i < AllDataArrary.Length; i++)
                    {
                        string[] OneDataArrary = AllDataArrary[i].Split('+');
                        String strTaxYear = OneDataArrary[0].ToString();
                        String strYearMonth = OneDataArrary[1].ToString();
                        String strDCNO = OneDataArrary[2].ToString();
                        String strItemCode = OneDataArrary[3].ToString();
                        String strOldValue = OneDataArrary[4].ToString();
                        String strNewValue = OneDataArrary[5].ToString();
                        String strStaffName = OneDataArrary[6].ToString();
                        String strItemName = OneDataArrary[7].ToString();

                        //保存到表TB_HR_MonthStaffTax中
                        sbSql.Append("UPDATE TB_HR_MonthStaffTax SET " + strItemCode + " = '" + strNewValue + "' WHERE DCNO = '" + strDCNO + "' and YEARMONTH = '" + strYearMonth + "';");

                        //同时保存在调整历史表中        
                        String strSystemTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        String strKey = System.DateTime.Now.ToString("yyyyMMddHHmmss")+strYearMonth+strDCNO;
                        sbSql.Append("INSERT INTO [AdjustTaxData_1]([SKEY],[TaxYear],[YearMonth],[StaffNo],[StaffName],[ItemCode],[ItemName],[OldValue],[NewValue],[UserId],[SystemTime])");
                        sbSql.Append(" values ('" + strKey + "','" + strTaxYear + "','" + strYearMonth + "','" + strDCNO + "','" + strStaffName + "','" + strItemCode+"'");
                        sbSql.Append(",'" + strItemName + "','" + strOldValue + "','" + strNewValue + "','" + this.GetUserCode() + "','" + strSystemTime + "');");
                    }
                }

            }
            String strSql = sbSql.ToString();
            if (!String.IsNullOrEmpty(strSql))
            {
                int iCount = SqlParamDao.ExecuteNonQueryBySql(strSql);
                iReturn = iCount;
            }

            context.Response.Write(iReturn);
        }
        catch (Exception ex)
        {
            log.Error(ex);
            log.Error("保存个税数据失败:" + sbSql.ToString());
            context.Response.Write(iReturn);
        }
    }



    /// <summary>
    /// 重新计算整年度的个税数据
    /// </summary>
    /// <param name="context"></param>
    private void CalculateTaxData(HttpContext context)
    {
        int iReturn = -1;
        StringBuilder sbSql = new StringBuilder();
        try
        {
            String strTaxYear = context.Request["sel_TaxYear"].ToString();
            String strDCNO = context.Request["sel_StaffNo"].ToString();
            //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
            strTaxYear = SQLInjectionDefense.ReplaceSQLReservedKeyword(strTaxYear);
            strDCNO = SQLInjectionDefense.ReplaceSQLReservedKeyword(strDCNO);

            if (!String.IsNullOrEmpty(strTaxYear)&&!String.IsNullOrEmpty(strDCNO))
            {
                //重新计算个税数据
                sbSql.Append("exec [USP_HR_Update_YearTax_OneStaff] '" + strTaxYear + "', '" + strDCNO + "','"+this.GetUserCode()+"';");
            }
            String strSql = sbSql.ToString();
            if (!String.IsNullOrEmpty(strSql))
            {
                int iCount = SqlParamDao.ExecuteNonQueryBySql(strSql);
                iReturn = iCount;
            }

            context.Response.Write(iReturn);
        }
        catch (Exception ex)
        {
            log.Error(ex);
            log.Error("保存个税数据失败:" + sbSql.ToString());
            context.Response.Write(iReturn);
        }
    }


    /// <summary>
    /// 设置当前用户具有查看调整历史表的权限
    /// </summary>
    /// <param name="context"></param>
    private void SetUserRole(HttpContext context)
    {
        int iReturn = -1;
        StringBuilder sbSql = new StringBuilder();
        try
        {
            sbSql.Append("if not exists (select * from TB_HR_USERROLE WHERE TID = 'AdjustTaxData' AND RID = 'Viewer' and SUSERID = '"+this.GetUserCode()+"')");
            sbSql.Append("begin");
            sbSql.Append(" INSERT INTO TB_HR_USERROLE(SUSERID,TID,RID)VALUES('"+this.GetUserCode()+"','AdjustTaxData','Viewer')");
            sbSql.Append("end");

            String strSql = sbSql.ToString();
            if (!String.IsNullOrEmpty(strSql))
            {
                int iCount = SqlParamDao.ExecuteNonQueryBySql(strSql);
                iReturn = iCount;
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
            log.Error("设置当前用户具有查看调整历史表的权限失败:" + sbSql.ToString());
        }
    }


    public bool IsReusable {
        get {
            return false;
        }
    }

}