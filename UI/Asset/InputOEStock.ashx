<%@ WebHandler Language="C#" Class="InputOEStock" %>

using System;
using System.Web;
using System.Text;
using System.Data;
using System.Collections;
using Com.ValuePlus.Web;
using Com.ValuePlus.DAL;

public class InputOEStock : HandlerBase, IHttpHandler, System.Web.SessionState.IRequiresSessionState
{

    public void ProcessRequest(HttpContext context)
    {
        Hashtable hsTableUrlQuery = this.GetUrlAnalyse(context);
        string strParam = hsTableUrlQuery["param"] == null ? string.Empty : hsTableUrlQuery["param"].ToString();//param
        String strStockPlan = hsTableUrlQuery["stockplan"] == null ? string.Empty : hsTableUrlQuery["stockplan"].ToString();
        String strDept = hsTableUrlQuery["dept"] == null ? string.Empty : hsTableUrlQuery["dept"].ToString();
        String strLocation = hsTableUrlQuery["location"] == null ? string.Empty : hsTableUrlQuery["location"].ToString();
        String strCategory = hsTableUrlQuery["category"] == null ? string.Empty : hsTableUrlQuery["category"].ToString();
        String strACode = hsTableUrlQuery["acode"] == null ? string.Empty : hsTableUrlQuery["acode"].ToString();
        String strAName = hsTableUrlQuery["aname"] == null ? string.Empty : hsTableUrlQuery["aname"].ToString();
        String strAModel = hsTableUrlQuery["amodel"] == null ? string.Empty : hsTableUrlQuery["amodel"].ToString();
        String strSelectedSAcode = hsTableUrlQuery["selectedsacodes"] == null ? string.Empty : hsTableUrlQuery["selectedsacodes"].ToString();

        if (strParam.Equals("queryAssetsList"))
        {
            this.QueryAssetsList(context, strStockPlan, strDept, strLocation, strCategory, strACode, strAName, strAModel);
        }
        if (strParam.Equals("saveResult"))
        {
            this.SaveStockResult(context);
        }
    }


    /// <summary>
    /// 保存盘点结果数据
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strStockPlan"></param>
    /// <param name="strStockPlan"></param>
    private void SaveStockResult(HttpContext context)
    {
        String strSql = "";
        try
        {
            String strStockPlan = context.Request.Form["sel_StockPlan"].ToString();

            StringBuilder sbSql = new StringBuilder();
            //添加执行存储过程的语句
            sbSql.Append("; ");
            sbSql.Append(this.GetSqlString_StockQty(context));

            if (!string.IsNullOrEmpty(sbSql.ToString()))
            {
                String strFlagId = Guid.NewGuid().ToString();
                sbSql.Append("exec USP_OE_OEPHY_SaveInputResult '" + strStockPlan + "','" + this.GetUserCode() + "','" + strFlagId + "'");

                strSql = sbSql.ToString();
                int iResult = SqlParamDao.ExecuteNonQueryBySql(strSql);
                if (iResult >= 0)
                {
                    context.Response.Write("1");
                    return;
                }
                else
                {
                    context.Response.Write("0");
                    return;
                }
            }
            else
            {
                context.Response.Write("0");
                return;
            }


        }
        catch (Exception ex)
        {
            context.Response.Write("-1");
            log.Error(ex);
            log.Error("error Sql:" + strSql);
        }

    }

    /// <summary>
    /// 获取保存盘点结果更新数据库的脚本
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    private String GetSqlString_StockQty(HttpContext context)
    {
        StringBuilder sbSql = new StringBuilder();

        try
        {
            String strStockPlan = context.Request.Form["sel_StockPlan"].ToString();
            String strStockSaveData = context.Request.Form["txt_SelectSaveSACODE"].ToString();
            String[] strArray1 = strStockSaveData.Split(',');
            int iItemCount = strArray1.Length;
            if (iItemCount > 0)
            {
                for (int i = 0; i < iItemCount; i++)
                {
                    String strOneResultData = strArray1[i];
                    String[] strArray2 = strOneResultData.Split('*');
                    if (strArray2.Length == 3)
                    {
                        String strSACODE = strArray2[0].ToString();
                        String strLCODE = strArray2[1].ToString();
                        String strStockQty = strArray2[2];
                        String strStockTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        sbSql.Append(" delete from OEPHY_3 where OEPID = '" + strStockPlan + "' AND SACODE = '" + strSACODE + "' AND SLCODE = '" + strLCODE + "';");
                        try {
                            if (float.Parse(strStockQty) != 0)
                            {
                                sbSql.Append("INSERT INTO OEPHY_3(OEPID,SEQNO,SLCODE,SACODE,NQUANTITY,ADUSER,ADDATE)values");
                                sbSql.Append("('" + strStockPlan + "','" + System.Guid.NewGuid() + "','" + strLCODE + "','" + strSACODE + "','" + strStockQty + "'");
                                sbSql.Append(",'" + this.GetUserCode() + "',dbo.Fun_AM_GetCurMonthlyLastDatetime(CONVERT(varchar(30),getdate(),120)));");
                            }
                        }
                        catch (Exception ex)
                        {
                            //log.Error(ex);
                            //log.Error("strSACODE："+strSACODE+"strStockQty:" + strStockQty);
                        }
                    }

                }
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
            log.Error("获取保存盘点结果更新数据库的脚本出错 Sql:" + sbSql.ToString());
        }

        return sbSql.ToString();

    }


    /// <summary>
    /// 查询资产列表
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strStockPlan"></param>
    /// <param name="strDept"></param>
    /// <param name="strLocation"></param>
    /// <param name="strCategory"></param>
    /// <param name="strACode"></param>
    /// <param name="strAName"></param>
    /// <param name="strAModel"></param>
    private void QueryAssetsList(HttpContext context, String strStockPlan, String strDept, String strLocation, String strCategory, String strACode, String strAName, String strAModel)
    {
        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select row_number() over(ORDER BY A.SACODE,ISNULL(A.SUSEDEPT,''),ISNULL(A.SLCODE,'')) AS ID");
            sbSql.Append(" ,A.*,isnull((select NQUANTITY from OEPHY_3 where OEPID = '" + strStockPlan + "' and SLCODE = A.SLCODE AND SACODE = A.SACODE),0) AS StockQty ");
            sbSql.Append(" from [VW_OE_AssetDetail_Location] A where 1=1 ");

            if (!string.IsNullOrEmpty(strDept))
            {
                //sbSql.Append(" and ISNULL(A.SUSEDEPT,'') = '" + strDept + "'");
                sbSql.Append(" and (ISNULL(A.SUSEDEPT,'') = '" + strDept + "' or ISNULL(A.SUSEDEPT,'') in (SELECT SubCode from dbo.[Fun_AM_GetSubDept]('" + strDept + "','1')))");
            }
            if (!string.IsNullOrEmpty(strLocation))
            {
                //sbSql.Append(" and ISNULL(A.SLCODE,'') = '" + strLocation + "'");
                sbSql.Append(" and (ISNULL(A.SLCODE,'') = '" + strLocation + "' or ISNULL(A.SLCODE,'') in (SELECT SubCode from dbo.[Fun_AM_GetSubLocation]('" + strLocation + "','1')))");
            }
            if (!string.IsNullOrEmpty(strCategory))
            {
                //sbSql.Append(" and ISNULL(A.STYPE,'') = '" + strCategory + "'");
                sbSql.Append(" and (ISNULL(A.STYPE,'') = '" + strCategory + "' or ISNULL(A.STYPE,'') in (SELECT SubCode from dbo.[Fun_OE_GetSubOEClass]('" + strCategory + "','1')))");
            }
            if (!string.IsNullOrEmpty(strACode))
            {
                sbSql.Append(" and ISNULL(A.SACODE,'') like '%" + strACode + "%'");
            }
            if (!string.IsNullOrEmpty(strAName))
            {
                sbSql.Append(" and (ISNULL(A.SANAME,'') like '%" + strAName + "%' or ISNULL(A.SANAMECHS,'') like '%" + strAName + "%')");
            }
            if (!string.IsNullOrEmpty(strAModel))
            {
                sbSql.Append(" and ISNULL(A.SMODEL,'') like '%" + strAModel + "%'");
            }
            ////获取尚未盘点到的资产
            //if (!string.IsNullOrEmpty(strStockPlan))
            //{
            //    sbSql.Append(" and ISNULL(A.SBARCODE,'') NOT IN (select SACODE from OEPHY_3 WHERE OEPID = '" + strStockPlan + "')");
            //}
            sbSql.Append(" AND A.NQUANTITY>0");
            sbSql.Append(" ORDER BY ISNULL(A.SUSEDEPT,''),ISNULL(A.SLCODE,''),A.STYPE,A.SACODE");

            String strSql = sbSql.ToString();
            //log.Error("OE手工盘点页面查询列表:"+strSql);

            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
            int iRowsCount = 0;
            if (dt != null)
            {
                iRowsCount = dt.Rows.Count;
            }

            int iColCount = dt.Columns.Count;
            StringBuilder sBuilder = new StringBuilder();

            sBuilder.Append("{");
            sBuilder.Append("\"totalCount\":"+iRowsCount.ToString());
            sBuilder.Append(",");
            sBuilder.Append(WebCommon.GetJsonStringByDataTable(dt,"\"ResultData\"",true));
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

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}