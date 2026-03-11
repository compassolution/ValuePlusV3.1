<%@ WebHandler Language="C#" Class="ChangeStockLocation" %>

using System;
using System.Web;
using System.Text;
using System.Data;
using System.Collections;
using Com.ValuePlus.Web;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Utils.Serializable;
using System.IO;
using Com.ValuePlus.Common.Security;

public class ChangeStockLocation : HandlerBase, IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    public void ProcessRequest (HttpContext context) {
        Hashtable hsTableUrlQuery = this.GetUrlAnalyse(context);
        string strParam = hsTableUrlQuery["param"] == null ? string.Empty : hsTableUrlQuery["param"].ToString();//param
        string strStockPlan = hsTableUrlQuery["stockplan"] == null ? string.Empty : hsTableUrlQuery["stockplan"].ToString();//stockplan
        string strHoldDeptNative = hsTableUrlQuery["deptnative"] == null ? string.Empty : hsTableUrlQuery["holddeptnative"].ToString();//holddeptnative
        string strUseDeptNative = hsTableUrlQuery["deptnative"] == null ? string.Empty : hsTableUrlQuery["usedeptnative"].ToString();//usedeptnative
        string strDeptStocked = hsTableUrlQuery["deptstocked"] == null ? string.Empty : hsTableUrlQuery["deptstocked"].ToString();//deptstocked
        string strLocationNative = hsTableUrlQuery["locationnative"] == null ? string.Empty : hsTableUrlQuery["locationnative"].ToString();//locationnative
        string strLocationStocked = hsTableUrlQuery["locationstocked"] == null ? string.Empty : hsTableUrlQuery["locationstocked"].ToString();//locationstocked
        string strAssetsCode = hsTableUrlQuery["sacode"] == null ? string.Empty : hsTableUrlQuery["sacode"].ToString();//sacode
        string strEpcId = hsTableUrlQuery["barcode"] == null ? string.Empty : hsTableUrlQuery["barcode"].ToString();//epcid
        string strAssetsName = hsTableUrlQuery["saname"] == null ? string.Empty : hsTableUrlQuery["saname"].ToString();//saname

        if (strParam.Equals("submit"))
        {
            this.SaveChanged(context);
        }
        else if (strParam.Equals("getlist"))
        {
            this.GetDiffLocationAssetsList(context);
        }
    }

    /// <summary>
    /// 保存盘到地址变更
    /// </summary>
    /// <param name="context"></param>
    private void SaveChanged(HttpContext context)
    {
        String strSql = "";
        try
        {
            String strSelectedPlan = context.Request["txt_SelectedPlanCode"].ToString();
            String strSelectedBarCodes = context.Request["txt_SelectedBarCodes"].ToString();
            //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
            strSelectedPlan = SQLInjectionDefense.ReplaceSQLReservedKeyword(strSelectedPlan);
            strSelectedBarCodes = SQLInjectionDefense.ReplaceSQLReservedKeyword(strSelectedBarCodes);

            if (!String.IsNullOrEmpty(strSelectedBarCodes))
            {
                StringBuilder sbSql = new StringBuilder();
                String[] strArrary = strSelectedBarCodes.Split(';');
                for(int i = 0; i < strArrary.Length; i++)
                {
                    String strBarCode = strArrary[i];
                    if (!String.IsNullOrEmpty(strBarCode))
                    {
                        sbSql.Append("UPDATE A SET A.SLCODE = B.SLCODE,A.SUSEDEPT = B.SUSEDEPT ");
                        sbSql.Append(" FROM AMPLAN_2 A INNER JOIN VW_AssetDetail_ForReport B ON ISNULL(A.SEPCID,'') = ISNULL(B.SBARCODE ,'')");
                        sbSql.Append(" WHERE ISNULL(A.SEPCID,'') = '"+strBarCode+"' AND A.PCODE = '"+strSelectedPlan+"';");
                    }
                }

                strSql = sbSql.ToString();
                int iResult = SqlParamDao.ExecuteNonQueryBySql(strSql);
                context.Response.Write(iResult.ToString());
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
    /// 获取地址差异的列表
    /// </summary>
    /// <param name="context"></param>
    private void GetDiffLocationAssetsList(HttpContext context)
    {
        try
        {
            String strSQL_ExecuteSP = "USP_AM_QRY_StockLocationDiff";
            String strTableName = "_"+strSQL_ExecuteSP;
            
            String strAMPLAN = context.Request["sel_StockPlan"].ToString();
            String strSACODE = context.Request["txt_SACODE"].ToString();
            String strSBARCODE = context.Request["txt_BARCODE"].ToString();
            String strSANAME = context.Request["txt_SANAME"].ToString();
            String strSHOLDDEPT = context.Request["sel_HoldDeptNative"].ToString();
            String strSUSEDEPT = context.Request["sel_UseDeptNative"].ToString();
            String strSLCODE = context.Request["sel_LocationNative"].ToString();
            String strStockDept = context.Request["sel_DeptStocked"].ToString();
            String strStockLocation = context.Request["sel_LocationStocked"].ToString();
			
            //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
            strAMPLAN = SQLInjectionDefense.ReplaceSQLReservedKeyword(strAMPLAN);
            strSACODE = SQLInjectionDefense.ReplaceSQLReservedKeyword(strSACODE);
            strSBARCODE = SQLInjectionDefense.ReplaceSQLReservedKeyword(strSBARCODE);
            strSANAME = SQLInjectionDefense.ReplaceSQLReservedKeyword(strSANAME);
            strSUSEDEPT = SQLInjectionDefense.ReplaceSQLReservedKeyword(strSUSEDEPT);
            strSLCODE = SQLInjectionDefense.ReplaceSQLReservedKeyword(strSLCODE);
            strStockDept = SQLInjectionDefense.ReplaceSQLReservedKeyword(strStockDept);
            strStockLocation = SQLInjectionDefense.ReplaceSQLReservedKeyword(strStockLocation);

            Hashtable hsTableParam = new Hashtable();
            hsTableParam.Add("AMPLAN", strAMPLAN);
            hsTableParam.Add("SACODE", strSACODE);
            hsTableParam.Add("SBARCODE", strSBARCODE);
            hsTableParam.Add("SANAME", strSANAME);
            hsTableParam.Add("SHOLDDEPT", strSHOLDDEPT);
            hsTableParam.Add("SUSEDEPT", strSUSEDEPT);
            hsTableParam.Add("SLCODE", strSLCODE);
            hsTableParam.Add("StockDept", strStockDept);
            hsTableParam.Add("StockLocation", strStockLocation);
            int iReturnValue = SqlParamDao.ExcuteSP(strSQL_ExecuteSP, hsTableParam);

            //String strSQL_ExecuteSP = "EXEC USP_AM_QRY_StockLocationDiff '"+strStockPlan+"','"+strAssetsCode+"','"+strEpcId+"','"+strAssetsName+"','"+strUseDeptNative+"','"+strLocationNative+"','"+strDeptStocked+"','"+strLocationStocked+"'";
            //int iResutlt = SqlParamDao.ExecuteNonQueryBySql(strSQL_ExecuteSP);

            String strSql_Cols = "select a.* from syscolumns a inner join sysobjects b on a.id = b.id where b.name = '_USP_AM_QRY_StockLocationDiff' order by colorder";
            DataTable dt_Cols = SqlParamDao.GetDataTableBySql(strSql_Cols);
            StringBuilder sBuilder_ColName = new StringBuilder();
            sBuilder_ColName.Append("colNames:[ ");
            sBuilder_ColName.Append("{");
            if ((dt_Cols != null) && (dt_Cols.Rows.Count > 0))
            {
                for(int i = 0; i < dt_Cols.Rows.Count; i++)
                {
                    String strColName = dt_Cols.Rows[i]["name"].ToString();
                    if (i == 0)
                    {
                        sBuilder_ColName.Append(strColName+":"+"'"+strColName+"'");
                    }else
                    {
                        sBuilder_ColName.Append(","+strColName+":"+"'"+strColName+"'");
                    }
                }
            }
            sBuilder_ColName.Append("}");
            sBuilder_ColName.Append("]");
            String strColNames = sBuilder_ColName.ToString();

            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select * from ["+strTableName+"]");
            DataTable dt = SqlParamDao.GetDataTableBySql(sbSql.ToString());
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
            if (!String.IsNullOrEmpty(strColNames))
            {
                sBuilder.Append("," + strColNames);
            }
            sBuilder.Append("}");

            string json = sBuilder.ToString();

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

