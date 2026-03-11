<%@ WebHandler Language="C#" Class="PRItemTree" %>

using System;
using System.Web;
using System.Text;
using System.Data;
using System.Collections;
using Com.ValuePlus.Web;
using Com.ValuePlus.DAL;

public class PRItemTree : HandlerBase, IHttpHandler, System.Web.SessionState.IRequiresSessionState {

    public void ProcessRequest (HttpContext context) {
        Hashtable hsTableUrlQuery = this.GetUrlAnalyse(context);
        string strParam = hsTableUrlQuery["param"] == null ? string.Empty : hsTableUrlQuery["param"].ToString();//param
        String strItemCode = hsTableUrlQuery["itemcode"] == null ? string.Empty : hsTableUrlQuery["itemcode"].ToString();
        String strParentCode = hsTableUrlQuery["parentcode"] == null ? string.Empty : hsTableUrlQuery["parentcode"].ToString();
        String strItemType = hsTableUrlQuery["itemtype"] == null ? string.Empty : hsTableUrlQuery["itemtype"].ToString();
        String strIsStop = hsTableUrlQuery["isstop"] == null ? string.Empty : hsTableUrlQuery["isstop"].ToString();
        String strIsShowPrint = hsTableUrlQuery["isshowprint"] == null ? string.Empty : hsTableUrlQuery["isshowprint"].ToString();
        String strVerifyCode = hsTableUrlQuery["verifycode"] == null ? string.Empty : hsTableUrlQuery["verifycode"].ToString();
        String strCtrlId = hsTableUrlQuery["ctrlid"] == null ? string.Empty : hsTableUrlQuery["ctrlid"].ToString();
        String strCtrlValue = hsTableUrlQuery["ctrlvalue"] == null ? string.Empty : hsTableUrlQuery["ctrlvalue"].ToString();
        
        if (strParam.Equals("queryall030"))
        {
            String strReturnJson = this.QueryDataList(context,"",strItemCode,strItemType,strIsShowPrint,strVerifyCode,strIsStop,"0");
            context.Response.Write("["+strReturnJson+"]");
        }else if (strParam.Equals("querytreelist"))
        {
            String strReturnJson = this.QueryDataList(context,strParentCode,strItemCode,strItemType,strIsShowPrint,"",strIsStop,"1");
            context.Response.Write("["+strReturnJson+"]");
        }
    }

    /// <summary>
    /// 查询列表数据
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strItemCode"></param>
    private String QueryDataList(HttpContext context,String strParentCode, String strItemCode,String strItemType,String strIsShowPrint,String strVerifyCode,String strIsStop,String strIsGetSubLevel)
    {
        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select * ");
            sbSql.Append(" ,(SELECT COUNT(1) FROM PRITEM_4 WHERE ITEMCODE = A.ITEMCODE) AS SubItemCount");
            sbSql.Append(" ,(select CDESCCHS from tb_hrlstd where lid = 'VERIFYSTATUS' and CID = A.ITEMVERIFY) AS ITEMVERIFY_DESC");
            sbSql.Append(" from PRITEM_1 A WHERE 1=1 ");
            if(!String.IsNullOrEmpty(strParentCode))
            {
                sbSql.Append(" AND ITEMCODE IN (select RefItemCode from PRITEM_4 where ITEMCODE = '" + strParentCode+"')" );
            }else{
                sbSql.Append(" AND ITEMECAL = '030' " );
                //sbSql.Append(" AND ITEMCODE NOT IN (select RefItemCode from PRITEM_4) " );
                //sbSql.Append(" AND ITEMCODE = 'FunSFHJ'" );
            }

            if(!String.IsNullOrEmpty(strItemCode))
            {
                sbSql.Append(" AND ITEMCODE = '" + strItemCode+"'" );
            }
            if(!String.IsNullOrEmpty(strItemType))
            {
                sbSql.Append(" AND ITEMECAL = '" + strItemType+"'" );
            }
            if(!String.IsNullOrEmpty(strIsShowPrint))
            {
                sbSql.Append(" AND ITEMPRINT = '" + strIsShowPrint+"'" );
            }
            if(!String.IsNullOrEmpty(strVerifyCode))
            {
                sbSql.Append(" AND ITEMVERIFY = '" + strVerifyCode+"'" );
            }
            if(!String.IsNullOrEmpty(strIsStop))
            {
                sbSql.Append(" AND BISSTOP = '" + strIsStop+"'" );
            }

            sbSql.Append(" order by ITEMCORDER,ITEMPORDER");
            String strSql = sbSql.ToString();
            log.Error("PRItemTree.ashx查询列表数据:"+strSql);

            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
            int iRowsCount = 0;
            if (dt != null)
            {
                iRowsCount = dt.Rows.Count;
            }

            int iColCount = dt.Columns.Count;
            StringBuilder sBuilder = new StringBuilder();

            if ((dt != null) && (dt.Rows.Count > 0))
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    String strRowItemCode = dt.Rows[i]["ITEMCODE"].ToString();
                    String strRowItemName = strRowItemCode+"-"+dt.Rows[i]["SHOWNAMECHS"].ToString().Trim();
                    String strRowParentItemCode = strParentCode;
                    String strRowItemType = dt.Rows[i]["ITEMECAL"].ToString();
                    String strRowItemEPPL = dt.Rows[i]["ITEMEPPL"].ToString();
                    String strRowItemEPPLDesc = dt.Rows[i]["EPPLDESC"].ToString();
                    String strRowItemVerifyDesc = dt.Rows[i]["ITEMVERIFY_DESC"].ToString();
                    int iSubItemCount = int.Parse(dt.Rows[i]["SubItemCount"].ToString());

                    if (i == 0)
                    {
                        sBuilder.Append("{");
                    }
                    else
                    {
                        sBuilder.Append(",{");
                    }

                    sBuilder.Append("\"id\": \"" + strRowItemCode + "\",");
                    sBuilder.Append("\"name\": \"" + strRowItemName + "\",");
                    sBuilder.Append("\"pId\": \"" + Microsoft.JScript.GlobalObject.escape(strParentCode) + "\",");
                    sBuilder.Append("\"open\": \"" + (iSubItemCount>0?false:false).ToString() + "\",");
                    sBuilder.Append("\"eppl\": \"" + Microsoft.JScript.GlobalObject.escape(strRowItemEPPL) + "\",");
                    sBuilder.Append("\"eppldesc\": \"" + Microsoft.JScript.GlobalObject.escape(strRowItemEPPLDesc) + "\",");
                    sBuilder.Append("\"verifydesc\": \"" + Microsoft.JScript.GlobalObject.escape(strRowItemVerifyDesc) + "\",");
                    sBuilder.Append("\"click\": \"" + "GetOneItemInfo('"+strRowItemCode+"','"+strRowItemType+"');" + "\"");
                    sBuilder.Append("}");
                    if (strIsGetSubLevel.Equals("1")&&iSubItemCount>0){
                        sBuilder.Append(",");
                        sBuilder.Append(this.QueryDataList(context,strRowItemCode,strItemCode,strItemType,strIsShowPrint,"",strIsStop,"1"));
                    }
                    
                }
            }
            return sBuilder.ToString();
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return "";
        }
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}