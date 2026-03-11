<%@ WebHandler Language="C#" Class="InputStockResult" %>

using System;
using System.Web;
using System.Text;
using System.Data;
using System.Collections;
using Com.ValuePlus.Web;
using Com.ValuePlus.DAL;

public class InputStockResult : HandlerBase, IHttpHandler, System.Web.SessionState.IRequiresSessionState
{

    public void ProcessRequest(HttpContext context)
    {
        Hashtable hsTableUrlQuery = this.GetUrlAnalyse(context);
        string strParam = hsTableUrlQuery["param"] == null ? string.Empty : hsTableUrlQuery["param"].ToString();//param
        string strIsStocked = hsTableUrlQuery["stocked"] == null ? string.Empty : hsTableUrlQuery["stocked"].ToString();//stocked

        if (strParam.Equals("queryAssetsList"))
        {
            this.QueryAssetsList(context, strIsStocked,hsTableUrlQuery);
        }
        if (strParam.Equals("saveResult"))
        {
            this.SaveStockResult(context, strIsStocked);
        }
    }


    /// <summary>
    /// 保存盘点结果数据
    /// </summary>
    /// <param name="context"></param>
    /// <param name="context"></param>
    /// <param name="strIsStocked"></param>
    private void SaveStockResult(HttpContext context, String strIsStocked)
    {
        String strStockPlan = context.Request.Form["sel_StockPlan"].ToString();
        String strSelectedSAcode = context.Request.Form["txt_SelectSaveSACODE"].ToString();
        int iReturn = -1;
        String strSpName = "USP_AM_AMPLAN_SaveInputResult";
        Hashtable hsTableParam = new Hashtable();
        hsTableParam.Add("AMPLAN", strStockPlan);
        hsTableParam.Add("StockAssetCodes", strSelectedSAcode);
        hsTableParam.Add("IsStocked", strIsStocked.ToString().ToLower());
        hsTableParam.Add("SUSERID", this.GetUserCode());
        try
        {
            log.Error("资产盘点计划" + strStockPlan + "是否为删除盘点数据：" + strIsStocked + "，手工盘点录入时传入后台的数据:" + strSelectedSAcode);
            if (!String.IsNullOrEmpty(strSpName))
            {
                iReturn = SqlParamDao.ExcuteSP(strSpName, hsTableParam);
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
            log.Error("操作合同后,执行存储过程失败: SPNAME:" + strSpName);
        }
        context.Response.Write(iReturn);

    }


    /// <summary>
    /// 查询资产列表
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strIsStocked"></param>
    private void QueryAssetsList(HttpContext context,String strIsStocked,Hashtable hsTableUrlQuery)
    {
        try
        {
            String strStockPlan = context.Request.Form["sel_StockPlan"].ToString();
            String strDept = context.Request.Form["sel_Dept"].ToString();
            String strLocation = context.Request.Form["sel_Location"].ToString();
            String strCategory = context.Request.Form["sel_Category"].ToString();
            String strACode = context.Request.Form["txt_ACode"].ToString();
            String strAName = context.Request.Form["txt_AName"].ToString();
            String strAModel = context.Request.Form["txt_AModel"].ToString();
            String strLabelType = context.Request.Form["sel_LabelType"].ToString();
            String strIsJoinStock = context.Request.Form["sel_IsJoinStock"].ToString();
            //String strStockPlan = hsTableUrlQuery["stockplan"].ToString();
            //String strDept = hsTableUrlQuery["dept"].ToString();
            //String strLocation = hsTableUrlQuery["location"].ToString();
            //String strCategory = hsTableUrlQuery["category"].ToString();
            //String strACode = hsTableUrlQuery["acode"].ToString();
            //String strAName = hsTableUrlQuery["aname"].ToString();
            //String strAModel = hsTableUrlQuery["amodel"].ToString();

            StringBuilder sbSql = new StringBuilder();
            String strColumnNames = "SACODE,SANAMECHS,CLASSNAMECN,GuigeXinghao,LOCATIONNAMECN,LABELTYPENAMECHS,SUSEDEPTNAMECN,SUSER,SBARCODE,NVALUE";
            if (strIsStocked.ToLower().Equals("true"))
            {
                //获取尚未盘点到的资产
                sbSql.Append("select "+strColumnNames+" from VW_AssetDetail where SBARCODE IN (select SEPCID from AMPLAN_2 WHERE PCODE = '" + strStockPlan + "')");
            }
            else
            {
                //取未盘点数据
                sbSql.Append("select "+strColumnNames+" from VW_AssetDetail where SSTATE IN (select AssetsStatus from dbo.[Fun_AM_GetNormalStatus]()) ");
                //获取尚未盘点到的资产
                sbSql.Append(" and SBARCODE NOT IN (select SEPCID from AMPLAN_2 WHERE PCODE = '" + strStockPlan + "')");
            }

            if (!string.IsNullOrEmpty(strDept))
            {
                sbSql.Append(" and (SUSEDEPT = '" + strDept + "' or SUSEDEPT in (SELECT SubCode from dbo.[Fun_AM_GetSubDept]('" + strDept + "','1')))");
            }
            if (!string.IsNullOrEmpty(strLocation))
            {
                //sbSql.Append(" and SLCODE = '" + strLocation + "'");
                sbSql.Append(" and (SLCODE = '" + strLocation + "' or SLCODE in (SELECT SubCode from dbo.[Fun_AM_GetSubLocation]('" + strLocation + "','1')))");
            }
            if (!string.IsNullOrEmpty(strCategory))
            {
                //sbSql.Append(" and STYPE = '" + strCategory + "'");
                sbSql.Append(" and (STYPE = '" + strCategory + "' or STYPE in (SELECT SubCode from dbo.[Fun_AM_GetSubAMClass]('" + strCategory + "','1')))");
            }
            if (!string.IsNullOrEmpty(strACode))
            {
                sbSql.Append(" and SACODE like '%" + strACode + "%'");
            }
            if (!string.IsNullOrEmpty(strAName))
            {
                sbSql.Append(" and (SANAME like '%" + strAName + "%' or SANAMECHS like '%" + strAName + "%')");
            }
            if (!string.IsNullOrEmpty(strAModel))
            {
                sbSql.Append(" and SMODEL like '%" + strAModel + "%'");
            }
            if (!string.IsNullOrEmpty(strLabelType))
            {
                sbSql.Append(" and LABELTYPE = '" + strLabelType + "'");
            }
            if (!string.IsNullOrEmpty(strIsJoinStock))
            {
                sbSql.Append(" and BISSTOKE = '" + strIsJoinStock + "'");
            }
            
            sbSql.Append(" ORDER BY SACODE");

            String strSql = sbSql.ToString();

            if (strIsStocked.ToLower().Equals("true"))
            {
                log.Error("查询可删除盘点数据的资产集合SQL:" + strSql);
            }
            else
            {
                log.Error("查询可录入盘点数据的资产集合SQL:" + strSql);
            }

            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
            int iRowsCount = 0;
            if (dt != null)
            {
                iRowsCount = dt.Rows.Count;
            }

            //int iColCount = dt.Columns.Count;
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

    public bool IsReusable {
        get {
            return false;
        }
    }

}