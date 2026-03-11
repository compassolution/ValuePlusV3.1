<%@ WebHandler Language="C#" Class="ExportAssetsImages" %>

using System;
using System.Web;
using System.Text;
using System.Data;
using System.Collections;
using Com.ValuePlus.Web;
using Com.ValuePlus.DAL;

public class ExportAssetsImages : HandlerBase, IHttpHandler, System.Web.SessionState.IRequiresSessionState {

    public void ProcessRequest (HttpContext context) {
        Hashtable hsTableUrlQuery = this.GetUrlAnalyse(context);
        string strParam = hsTableUrlQuery["param"] == null ? string.Empty : hsTableUrlQuery["param"].ToString();//param
        String strKeyValue = hsTableUrlQuery["keyvalue"] == null ? string.Empty : hsTableUrlQuery["keyvalue"].ToString();
        String strCtrlId = hsTableUrlQuery["ctrlid"] == null ? string.Empty : hsTableUrlQuery["ctrlid"].ToString();
        String strCtrlValue = hsTableUrlQuery["ctrlvalue"] == null ? string.Empty : hsTableUrlQuery["ctrlvalue"].ToString();

        if (strParam.Equals("querylist"))
        {
            this.QueryDataList(context,strKeyValue);
        }

    }

    /// <summary>
    /// 查询列表数据
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strKeyValue"></param>
    private void QueryDataList(HttpContext context, String strKeyValue)
    {
        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select top 300 A.*,B.Base64String as ImageBase64String from VW_AssetDetail_ForReport a inner join TB_AMIMAGE b on a.sacode = b.sacode");
            //sbSql.Append("select top 10 A.* from VW_AssetDetail_ForReport a ");
            sbSql.Append(" order by a.sacode ");
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
            sBuilder.Append("\"totalCount\":"+iRowsCount.ToString());
            sBuilder.Append(",");
            sBuilder.Append(WebCommon.GetJsonStringByDataTable(dt,"\"ResultData\"",true));
            sBuilder.Append("}");

            string json = sBuilder.ToString();

            context.Response.Write(json);
        }
        catch (Exception ex)
        {
            context.Response.Write("-1");
            log.Error(ex);
        }
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}