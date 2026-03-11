<%@ WebHandler Language="C#" Class="ModifyDesign_2" %>

using System;
using System.Web;
using System.Text;
using System.Data;
using System.Collections;
using Com.ValuePlus.Web;
using Com.ValuePlus.DAL;

public class ModifyDesign_2 : HandlerBase, IHttpHandler, System.Web.SessionState.IRequiresSessionState {

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
        if (strParam.Equals("deletelist"))
        {
            this.DeleteDataList(context,strKeyValue);
        }
        if (strParam.Equals("savedataonectrl"))
        {
            this.SaveDataOneCtrl(context,strKeyValue,strCtrlId,strCtrlValue);
        }
    }

    /// <summary>
    /// 即时保存单独每个字段控件的数据
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strKeyValue"></param>
    /// <param name="strCtrlId">控件ID支持多个ID，中间用*</param>
    /// <param name="strCtrlValue">控件值支持多个，中间用*，数量需跟CtrlId匹配</param>
    private void SaveDataOneCtrl(HttpContext context, String strKeyValue,String strCtrlId,String strCtrlValue)
    {
        int iReturn = -1;
        try
        {
            StringBuilder sbSql = new StringBuilder();
            if (!String.IsNullOrEmpty(strCtrlId))
            {
                String[] CtralIdArrary = strCtrlId.Split('*');
                String[] CtralValueArrary = strCtrlValue.Split('*');
                if (CtralIdArrary.Length > 0 && CtralIdArrary.Length == CtralValueArrary.Length)
                {
                    for(int i = 0; i < CtralIdArrary.Length; i++)
                    {
                        string[] strSelectCtrlId = CtralIdArrary[i].Split('_');
                        string[] strSelectCtrlValue = CtralValueArrary[i].Split('_');
                        String strOrderNum = strSelectCtrlId[2].ToString();
                        String strColumnName = strSelectCtrlId[3].ToString();

                        String strColumnValue = CtralValueArrary[i].ToString();
                        sbSql.Append("UPDATE DESIGN_2 SET "+strColumnName+" = '"+strColumnValue+"' WHERE DNO = '" + strKeyValue + "' and ORDERNUM = '"+strOrderNum+"';");
                        //从预算中获取成本单价并计算成本总价此逻辑在[USP_Archive_Grid_AfterEdit]中实现
                        sbSql.Append("exec [USP_PBMS_AfterModify_DESIGN_2] '" + strKeyValue + "','" + strOrderNum + "','edit';");
                    }
                }

            }
            String strSql = sbSql.ToString();
            log.Error("即时保存单独每个字段控件的数据:"+strSql);
            if (!String.IsNullOrEmpty(strSql))
            {
                int iCount = SqlParamDao.ExecuteNonQueryBySql(strSql);
                iReturn = iCount;
            }

            context.Response.Write(iReturn);
        }
        catch (Exception ex)
        {
            context.Response.Write(iReturn);
            log.Error(ex);
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
            sbSql.Append("select * from DESIGN_2 WHERE 1=1 ");
            sbSql.Append(" AND DNO = '" + strKeyValue + "'");
            sbSql.Append(" order by DNO,ORDERNUM ");
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

    /// <summary>
    /// 删除列表数据
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strKeyValue"></param>
    private void DeleteDataList(HttpContext context, String strKeyValue)
    {
        int iReturn = -1;
        try
        {
            String strSelectedOrderNum = context.Request.Form["txt_SelectedOrderNum"].ToString();

            StringBuilder sbSql = new StringBuilder();
            if (!String.IsNullOrEmpty(strSelectedOrderNum))
            {
                string[] strSelectArray = strSelectedOrderNum.Split(',');
                for (int i= 0; i < strSelectArray.Length; i++){
                    String strOrderNum = strSelectArray[i].ToString();
                    sbSql.Append("delete from DESIGN_2 WHERE DNO = '" + strKeyValue + "' and ORDERNUM = '"+strOrderNum+"';");
                }
            }
            String strSql = sbSql.ToString();
            log.Error("批量删除列表数据:"+strSql);
            if (!String.IsNullOrEmpty(strSql))
            {
                int iCount = SqlParamDao.ExecuteNonQueryBySql(strSql);
                iReturn = iCount;
            }

            context.Response.Write(iReturn);
        }
        catch (Exception ex)
        {
            context.Response.Write(iReturn);
            log.Error(ex);
        }
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}