<%@ WebHandler Language="C#" Class="StaffOnDuty" %>

using System;
using System.Web;
using System.Text;
using System.Data;
using System.Collections;
using Com.ValuePlus.Web;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Utils.Serializable;
using System.IO;

public class StaffOnDuty : HandlerBase, IHttpHandler, System.Web.SessionState.IRequiresSessionState
{

    public void ProcessRequest (HttpContext context) {
        Hashtable hsTableUrlQuery = this.GetUrlAnalyse(context);
        string strParam = hsTableUrlQuery["param"] == null ? string.Empty : hsTableUrlQuery["param"].ToString();//param
        string strDate = hsTableUrlQuery["date"] == null ? string.Empty : hsTableUrlQuery["date"].ToString();//param

        if (strParam.Equals("getdata"))
        {
            this.GetChartData(context,strDate);
        }

    }

    /// <summary>
    /// 获取图表数据
    /// </summary>
    /// <param name="context"></param>
    private void GetChartData(HttpContext context,String strDate)
    {
        String strReturn = "";
        try
        {
            StringBuilder sBuilder = new StringBuilder();

            StringBuilder sbSql_ResultData = new StringBuilder();
            sbSql_ResultData.Append("select * ");
            sbSql_ResultData.Append("   ,(select count(*) from HRDOCU_1 A INNER JOIN CSORGA_1 B ON A.DCDDESCCHS = B.OID WHERE A.DCSTATUS = '1' AND B.OPID = VW_DEPT.CID) as OnJobCount");
            sbSql_ResultData.Append("   ,(select count(*) from KQRSSZ_2 C INNER JOIN HRDOCU_1 A ON C.EM_NO = A.DCNO INNER JOIN CSORGA_1 B ON A.DCDDESCCHS = B.OID ");
            sbSql_ResultData.Append("       WHERE B.OPID = VW_DEPT.CID AND CONVERT(VARCHAR(20),r_Date,23) = '"+strDate+"' and (Isnull(InTime,'')<>'' or Isnull(InTime1,'')<>'') ) as OnDutyCount");
            sbSql_ResultData.Append(" from VW_Sys_Department VW_DEPT WHERE LID = 'D2' ORDER BY LID");
                
            String strSql = "select * from VW_Sys_Department WHERE LID = 'D2' ORDER BY LID";
            DataTable dt_Dept = SqlParamDao.GetDataTableBySql(strSql);
            DataTable dt_ResultData = SqlParamDao.GetDataTableBySql(sbSql_ResultData.ToString());

            sBuilder.Append("{");
            sBuilder.Append(WebCommon.GetJsonStringByDataTable(dt_ResultData, "ResultData"));
            sBuilder.Append(",");
            sBuilder.Append(WebCommon.GetJsonStringByDataTable(dt_Dept, "DeptName"));
            sBuilder.Append("}");

            strReturn = sBuilder.ToString();

            context.Response.Write(strReturn);

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