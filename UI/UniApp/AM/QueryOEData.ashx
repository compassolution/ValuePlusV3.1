<%@ WebHandler Language="C#" Class="QueryOEData" %>

using System;
using System.Web;
using System.Collections;
using System.Text;
using System.IO;
using System.Data;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Web;
using Com.ValuePlus.Archive.BLL;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Com.ValuePlus.Archive.Utils;

public class QueryOEData : HandlerBase,IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    #region 日志声明
    /// <summary>
    /// 日志声明
    /// </summary>
    private Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    #endregion


    public void ProcessRequest (HttpContext context) {
        //解析客户端传递过来的json data
        StreamReader reader = new StreamReader(context.Request.InputStream);
        String strParamJson = HttpUtility.UrlDecode(reader.ReadToEnd());
        //log.Error("传入参数的Json data字符串:" + strParamJson.ToString());

        string param = WebCommon.GetJsonValue(strParamJson,"param").ToString();//请求类型参数
        string strUserCode = WebCommon.GetJsonValue(strParamJson,"usercode").ToString();//用户编码
        string strCulture = WebCommon.GetJsonValue(strParamJson,"culture").ToString();//文化编码[0中文1英文]
        string strIsIncludeOE = WebCommon.GetJsonValue(strParamJson,"isincludeoe").ToString();//是否需要包含OE[0否1是]
        string strPlanCode = WebCommon.GetJsonValue(strParamJson,"plancode").ToString();//盘点计划编码
        string strLocationCode = WebCommon.GetJsonValue(strParamJson,"locationcode").ToString();//地址编码
        string strIsTakedByUser = WebCommon.GetJsonValue(strParamJson,"istakedbyuser").ToString();//是否盘点区分用户
        string strGetScope = WebCommon.GetJsonValue(strParamJson,"getscope").ToString();//获取范围（all/taked/lost/profit）
        string strPageSize = WebCommon.GetJsonValue(strParamJson,"pagesize").ToString();//每页条数
        int iPageSize = (String.IsNullOrEmpty(strPageSize)?20:int.Parse(strPageSize));//每页条数
        string strPageIndex = WebCommon.GetJsonValue(strParamJson,"pageindex").ToString();//分页起始index
        int iPageIndex = (String.IsNullOrEmpty(strPageIndex)?0:int.Parse(strPageIndex));//分页起始index

        string strQueryCondition = WebCommon.GetJsonObjectValue(strParamJson,"querycondition").ToString();//查询条件的JSON字符串

        object objTakedData = context.Request["datataked"] == null ? string.Empty : context.Request["datataked"];//上传数据对象

        String strRequestLanguage = strCulture.Equals("0") ? "zh-cn" : "en-us";
        switch (param.ToLower().ToString())
        {
            case "getdata_oeplanlist"://获取OE资产盘点计划信息数据
                this.GetData_OEPlanList(context,strRequestLanguage);
                break;
            case "getdata_oeplanlocationqty"://获取固定资产某计划下各种数量【包括总数，已盘到，盘亏数，盘盈数】
                this.GetData_OEPlanLocationQty(context,strPlanCode,strLocationCode,strUserCode,strIsTakedByUser);
                break;
            case "getdata_inventoryoeinfo"://根据条件获取对应盘点计划对应地址下的OE资产详细信息【服务器端分页获取】
                this.GetData_InventoryOEInfo(context,strGetScope,strQueryCondition,iPageSize,iPageIndex);
                break;
            case "getdata_oeinfonotpaging"://根据条件获取OE资产详细信息【不分页】
                this.GetData_OEInfoNotPaging(context,strQueryCondition);
                break;
            case "getdata_oelistbypaging"://根据条件获取OE资产详细信息【服务器端分页获取】
                this.GetData_OEListByPaging(context,strQueryCondition,iPageSize,iPageIndex);
                break;
            case "getdata_oelocationnotpaging"://根据条件获取OE资产地址分布信息【不分页】
                this.GetData_OELocationNotPaging(context,strQueryCondition);
                break;
            default:
                break;

        }
    }

    /// <summary>
    /// 获取OE资产盘点计划信息数据
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strRequestLanguage"></param>
    public void GetData_OEPlanList(HttpContext context,String strRequestLanguage)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        String strMethodDesc = "获取OE资产盘点计划信息数据";
        StringBuilder sbResult = new StringBuilder();

        try
        {
            DataTable dt = new DataTable();
            String strSql = "";
            //获取OE资产盘点计划信息数据
            strSql = "select * from [VW_Moblie_OEPlan]";
            sbResultData.Append("," + WebCommon.GetJsonStringByDataTable(SqlParamDao.GetDataTableBySql(strSql), "\"ResultData\"", true));
            strReturnCode = "1";
            strReturnMsg = strMethodDesc+"成功";

        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = strMethodDesc+"出错,请稍候重试";
            log.Error(ex);
        }
        finally{
            StringBuilder sbResultStatus = new StringBuilder();
            sbResultStatus.Append("\"ReturnStatus\":{");
            sbResultStatus.Append("\"ReturnCode\":\""+strReturnCode.ToString()+"\"");
            sbResultStatus.Append(",\"ReturnMsg\":\""+strReturnMsg.ToString()+"\"");
            sbResultStatus.Append("}");

            sbResult.Append("{");
            sbResult.Append(sbResultStatus.ToString());
            if (!String.IsNullOrEmpty(sbResultData.ToString()))
            {
                sbResult.Append(sbResultData.ToString()+"");
            }
            sbResult.Append("}");
        }
        //log.Error(strMethodDesc+"时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }

    /// <summary>
    /// 获取OE资产某计划下各种数量【包括总数，已盘到，盘亏数，盘盈数】
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strPlanCode"></param>
    /// <param name="strLocationCode"></param>
    /// <param name="strUserCode"></param>
    /// <param name="strIsTakedByUser"></param>
    public void GetData_OEPlanLocationQty(HttpContext context,String strPlanCode,String strLocationCode,String strUserCode,String strIsTakedByUser)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        String strMethodDesc = "获取OE资产某计划下各种数量【包括总数，已盘到，盘亏数，盘盈数】";
        StringBuilder sbResult = new StringBuilder();

        try
        {
            DataTable dt = new DataTable();
            String strSql = "";
            //系统存放地址信息数据
            strSql = "select * from dbo.[Fun_OE_GetPlanLocationQty]('"+strPlanCode+"','"+strLocationCode+"','"+strUserCode+"','"+strIsTakedByUser+"')";
            sbResultData.Append("," + WebCommon.GetJsonStringByDataTable(SqlParamDao.GetDataTableBySql(strSql), "\"ResultData\"", true));

            strReturnCode = "1";
            strReturnMsg = strMethodDesc+"成功";

        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = strMethodDesc+"出错,请稍候重试";
            log.Error(ex);
        }
        finally{
            StringBuilder sbResultStatus = new StringBuilder();
            sbResultStatus.Append("\"ReturnStatus\":{");
            sbResultStatus.Append("\"ReturnCode\":\""+strReturnCode.ToString()+"\"");
            sbResultStatus.Append(",\"ReturnMsg\":\""+strReturnMsg.ToString()+"\"");
            sbResultStatus.Append("}");

            sbResult.Append("{");
            sbResult.Append(sbResultStatus.ToString());
            if (!String.IsNullOrEmpty(sbResultData.ToString()))
            {
                sbResult.Append(sbResultData.ToString()+"");
            }
            sbResult.Append("}");
        }
        //log.Error(strMethodDesc+"时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }

    /// <summary>
    /// 根据条件获取对应计划对应地址下的OE资产详细信息【服务器端分页获取】
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strGetScope">获取范围（all/taked/lost/profit）</param>
    /// <param name="iPageSize"></param>
    /// <param name="iPageIndex"></param>
    public void GetData_InventoryOEInfo(HttpContext context,String strGetScope,String strQueryCondition,int iPageSize,int iPageIndex)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        String strMethodDesc = "根据条件获取对应计划对应地址下的OE资产详细信息【服务器端分页获取】";
        StringBuilder sbResult = new StringBuilder();
        int iTotalRecord = 0;

        try
        {

            //json字符串转json的JObject对象
            if(!String.IsNullOrEmpty(strQueryCondition))
            {
                JObject joCondition = (JObject)JsonConvert.DeserializeObject(strQueryCondition);
                String strPlanCode = joCondition["planCode"].ToString().Replace("'","''");
                String strLocationCode = joCondition["locationCode"].ToString().Replace("'","''");
                String strUserCode = joCondition["userId"].ToString().Replace("'","''");
                String strIsTakedByUser = joCondition["isTakedByUser"].ToString().Replace("'","''");
                String strSearchText = joCondition["searchText"].ToString().Replace("'","''");

                DataTable dt = new DataTable();
                StringBuilder sbSql = new StringBuilder();
                StringBuilder sbTakedQtySql = new StringBuilder();
                sbTakedQtySql.Append("(select ISNULL(SUM(NQUANTITY),0.00) from OEPHY_3 WHERE OEPID = '"+strPlanCode+"' AND SACODE = A.SACODE AND SLCODE = A.SLCODE \r\n");
                sbTakedQtySql.Append("  and ('"+strIsTakedByUser+"' = '0' or ADUSER = '"+strUserCode+"')  \r\n");
                sbTakedQtySql.Append("  )  \r\n");
                String strTakedQtySql = sbTakedQtySql.ToString();

                sbSql.Append("select *  \r\n");
                sbSql.Append(","+strTakedQtySql+" AS NTAKEDQTY  \r\n");
                sbSql.Append("from VW_Moblie_OE_Location A \r\n");
                sbSql.Append("WHERE (A.SLCODE in (SELECT SubCode from dbo.[Fun_AM_GetSubLocation]('"+strLocationCode+"','1')) or A.SLCODE = '"+strLocationCode+"') \r\n");

                if(!String.IsNullOrEmpty(strSearchText)){
                    sbSql.Append("and (A.SACODE LIKE '%"+strSearchText+"%' \r\n");
                    sbSql.Append("or A.SANAME LIKE '%"+strSearchText+"%' \r\n");
                    sbSql.Append("or A.SANAMECN LIKE '%"+strSearchText+"%' \r\n");
                    sbSql.Append("or A.SMODEL LIKE '%"+strSearchText+"%' \r\n");
                    sbSql.Append(") \r\n");
                }
                switch (strGetScope.ToLower()){
                    case "taked":
                        sbSql.Append("and ("+strTakedQtySql+" > 0) \r\n");
                        break;
                    case "none":
                        sbSql.Append("and ("+strTakedQtySql+" <= 0) \r\n");
                        break;
                    case "lost":
                        sbSql.Append("and ("+strTakedQtySql+" < A.NLOCATIONQTY) \r\n");
                        break;
                    case "profit":
                        sbSql.Append("and ("+strTakedQtySql+" > A.NLOCATIONQTY) \r\n");
                        break;
                    case "finish":
                        sbSql.Append("and ("+strTakedQtySql+" = A.NLOCATIONQTY) \r\n");
                        break;
                    default :
                        break;
                }

                String strCountSql = PagingSqlUtil.GetCountSql(sbSql.ToString());
                iTotalRecord = SqlParamDao.ExecuteScalarBySql(strCountSql);
                String strSql = PagingSqlUtil.GetPagingSql(sbSql.ToString(), iPageSize, iPageIndex, "SACODE", "SLCODE,SACODE");

                DataTable dtResultData = SqlParamDao.GetDataTableBySql(strSql);
                sbResultData.Append("," + WebCommon.GetJsonStringByDataTable(dtResultData, "\"ResultData\"", true));

            }else{
                sbResultData.Append("\"ResultData\":[]");
            }
            strReturnCode = "1";
            strReturnMsg = strMethodDesc+"成功";

        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = strMethodDesc+"出错,请稍候重试";
            log.Error(ex);
        }
        finally{
            StringBuilder sbResultStatus = new StringBuilder();
            sbResultStatus.Append("\"ReturnStatus\":{");
            sbResultStatus.Append("\"ReturnCode\":\""+strReturnCode.ToString()+"\"");
            sbResultStatus.Append(",\"ReturnMsg\":\""+strReturnMsg.ToString()+"\"");
            sbResultStatus.Append("}");

            sbResult.Append("{");
            sbResult.Append(sbResultStatus.ToString());
            if (!String.IsNullOrEmpty(sbResultData.ToString()))
            {
                sbResult.Append(sbResultData.ToString()+"");
            }
            sbResult.Append(",\"TotalRecord\":"+iTotalRecord.ToString()+"");
            sbResult.Append("}");
        }
        //log.Error(strMethodDesc+"时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }

    /// <summary>
    /// 根据条件获取OE资产详细信息【不分页】
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strQueryCondition"></param>
    public void GetData_OEInfoNotPaging(HttpContext context,String strQueryCondition)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        String strMethodDesc = "根据条件获取OE资产详细信息【不分页】";
        StringBuilder sbResult = new StringBuilder();
        try
        {

            //json字符串转json的JObject对象
            if(!String.IsNullOrEmpty(strQueryCondition))
            {
                JObject joCondition = (JObject)JsonConvert.DeserializeObject(strQueryCondition);
                String strAssetCode = joCondition["assetCode"] == null?"":joCondition["assetCode"].ToString().Replace("'","''");
                String strEpcId = joCondition["epcId"] == null?"":joCondition["epcId"].ToString().Replace("'","''");
                String strLocationCode = joCondition["locationCode"] == null?"":joCondition["locationCode"].ToString().Replace("'","''");
                String strSearchText = joCondition["searchText"] == null?"":joCondition["searchText"].ToString().Replace("'","''");

                DataTable dt = new DataTable();
                StringBuilder sbSql = new StringBuilder();

                sbSql.Append("select A.* from VW_Moblie_OE_Location A where 1 =1\r\n");
                if (!String.IsNullOrEmpty(strAssetCode)) {
                    sbSql.Append("and A.SACODE = '"+strAssetCode+"' \r\n");
                }
                if (!String.IsNullOrEmpty(strEpcId)) {
                    sbSql.Append("and A.SBARCODE = '"+strEpcId+"' \r\n");
                }
                if (!String.IsNullOrEmpty(strLocationCode)) {
                    sbSql.Append("and (A.SLCODE in (SELECT SubCode from dbo.[Fun_AM_GetSubLocation]('"+strLocationCode+"','1')) or A.SLCODE = '"+strLocationCode+"') \r\n");
                }

                if(!String.IsNullOrEmpty(strSearchText)){
                    sbSql.Append("and (A.SACODE LIKE '%"+strSearchText+"%' \r\n");
                    sbSql.Append("or A.SBARCODE LIKE '%"+strSearchText+"%' \r\n");
                    sbSql.Append("or A.SANAME LIKE '%"+strSearchText+"%' \r\n");
                    sbSql.Append("or A.SANAMECN LIKE '%"+strSearchText+"%' \r\n");
                    sbSql.Append("or A.SMODEL LIKE '%"+strSearchText+"%' \r\n");
                    sbSql.Append(") \r\n");
                }

                DataTable dtResultData = SqlParamDao.GetDataTableBySql(sbSql.ToString());
                sbResultData.Append("," + WebCommon.GetJsonStringByDataTable(dtResultData, "\"ResultData\"", true));

            }else{
                sbResultData.Append("\"ResultData\":[]");
            }
            strReturnCode = "1";
            strReturnMsg = strMethodDesc+"成功";

        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = strMethodDesc+"出错,请稍候重试";
            log.Error(ex);
        }
        finally{
            StringBuilder sbResultStatus = new StringBuilder();
            sbResultStatus.Append("\"ReturnStatus\":{");
            sbResultStatus.Append("\"ReturnCode\":\""+strReturnCode.ToString()+"\"");
            sbResultStatus.Append(",\"ReturnMsg\":\""+strReturnMsg.ToString()+"\"");
            sbResultStatus.Append("}");

            sbResult.Append("{");
            sbResult.Append(sbResultStatus.ToString());
            if (!String.IsNullOrEmpty(sbResultData.ToString()))
            {
                sbResult.Append(sbResultData.ToString()+"");
            }
            sbResult.Append("}");
        }
        //log.Error(strMethodDesc+"时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }


    /// <summary>
    /// 根据条件获取OE资产详细信息【分页获取】
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strQueryCondition"></param>
    /// <param name="iPageSize"></param>
    /// <param name="iPageIndex"></param>
    public void GetData_OEListByPaging(HttpContext context,String strQueryCondition,int iPageSize,int iPageIndex)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        String strMethodDesc = "根据条件获取固定资产详细信息【分页获取】";
        StringBuilder sbResult = new StringBuilder();
        int iTotalRecord = 0;
        try
        {
            //json字符串转json的JObject对象
            if(!String.IsNullOrEmpty(strQueryCondition))
            {
                JObject joCondition = (JObject)JsonConvert.DeserializeObject(strQueryCondition);
                String strSearchText = joCondition["searchText"] == null?"":joCondition["searchText"].ToString().Replace("'","''");

                DataTable dt = new DataTable();
                StringBuilder sbSql = new StringBuilder();

                sbSql.Append("select A.* from VW_Moblie_OE_Item A where 1 =1\r\n");

                if(!String.IsNullOrEmpty(strSearchText)){
                    sbSql.Append("and (A.SACODE LIKE '%"+strSearchText+"%' \r\n");
                    sbSql.Append("or A.SBARCODE LIKE '%"+strSearchText+"%' \r\n");
                    sbSql.Append("or A.SANAME LIKE '%"+strSearchText+"%' \r\n");
                    sbSql.Append("or A.SANAMECN LIKE '%"+strSearchText+"%' \r\n");
                    sbSql.Append("or A.SMODEL LIKE '%"+strSearchText+"%' \r\n");
                    sbSql.Append("or A.STYPENAME LIKE '%"+strSearchText+"%' \r\n");
                    sbSql.Append("or A.STYPENAMECN LIKE '%"+strSearchText+"%' \r\n");
                    sbSql.Append("or A.LABELTYPENAME LIKE '%"+strSearchText+"%' \r\n");
                    sbSql.Append("or A.LABELTYPENAMECN LIKE '%"+strSearchText+"%' \r\n");
                    sbSql.Append(") \r\n");
                }

                String strCountSql = PagingSqlUtil.GetCountSql(sbSql.ToString());
                iTotalRecord = SqlParamDao.ExecuteScalarBySql(strCountSql);
                String strSql = PagingSqlUtil.GetPagingSql(sbSql.ToString(), iPageSize, iPageIndex, "SACODE", "SACODE");

                DataTable dtResultData = SqlParamDao.GetDataTableBySql(strSql);
                sbResultData.Append("," + WebCommon.GetJsonStringByDataTable(dtResultData, "\"ResultData\"", true));

            }else{
                sbResultData.Append("\"ResultData\":[]");
            }
            strReturnCode = "1";
            strReturnMsg = strMethodDesc+"成功";

        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = strMethodDesc+"出错,请稍候重试";
            log.Error(ex);
        }
        finally{
            StringBuilder sbResultStatus = new StringBuilder();
            sbResultStatus.Append("\"ReturnStatus\":{");
            sbResultStatus.Append("\"ReturnCode\":\""+strReturnCode.ToString()+"\"");
            sbResultStatus.Append(",\"ReturnMsg\":\""+strReturnMsg.ToString()+"\"");
            sbResultStatus.Append("}");

            sbResult.Append("{");
            sbResult.Append(sbResultStatus.ToString());
            if (!String.IsNullOrEmpty(sbResultData.ToString()))
            {
                sbResult.Append(sbResultData.ToString()+"");
            }
            sbResult.Append(",\"TotalRecord\":"+iTotalRecord.ToString()+"");
            sbResult.Append("}");
        }
        //log.Error(strMethodDesc+"时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }

    /// <summary>
    /// 根据条件获取OE资产地址分布信息【不分页】
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strQueryCondition"></param>
    /// <param name="iPageSize"></param>
    /// <param name="iPageIndex"></param>
    public void GetData_OELocationNotPaging(HttpContext context,String strQueryCondition)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        String strMethodDesc = "根据条件获取OE资产地址分布信息【不分页】";
        StringBuilder sbResult = new StringBuilder();
        int iTotalRecord = 0;
        try
        {
            //json字符串转json的JObject对象
            if(!String.IsNullOrEmpty(strQueryCondition))
            {
                JObject joCondition = (JObject)JsonConvert.DeserializeObject(strQueryCondition);
                String strAssetCode = joCondition["assetCode"] == null?"":joCondition["assetCode"].ToString().Replace("'","''");

                DataTable dt = new DataTable();
                StringBuilder sbSql = new StringBuilder();

                sbSql.Append("select A.* from VW_Moblie_OE_Location A where 1 =1\r\n");

                if(!String.IsNullOrEmpty(strAssetCode)){
                    sbSql.Append(" and A.SACODE = '"+strAssetCode+"' \r\n");
                }
                sbSql.Append(" order by A.SACODE,A.SLCODE\r\n");

                String strSql = sbSql.ToString();

                DataTable dtResultData = SqlParamDao.GetDataTableBySql(strSql);
                sbResultData.Append("," + WebCommon.GetJsonStringByDataTable(dtResultData, "\"ResultData\"", true));

            }else{
                sbResultData.Append("\"ResultData\":[]");
            }
            strReturnCode = "1";
            strReturnMsg = strMethodDesc+"成功";

        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = strMethodDesc+"出错,请稍候重试";
            log.Error(ex);
        }
        finally{
            StringBuilder sbResultStatus = new StringBuilder();
            sbResultStatus.Append("\"ReturnStatus\":{");
            sbResultStatus.Append("\"ReturnCode\":\""+strReturnCode.ToString()+"\"");
            sbResultStatus.Append(",\"ReturnMsg\":\""+strReturnMsg.ToString()+"\"");
            sbResultStatus.Append("}");

            sbResult.Append("{");
            sbResult.Append(sbResultStatus.ToString());
            if (!String.IsNullOrEmpty(sbResultData.ToString()))
            {
                sbResult.Append(sbResultData.ToString()+"");
            }
            sbResult.Append(",\"TotalRecord\":"+iTotalRecord.ToString()+"");
            sbResult.Append("}");
        }
        //log.Error(strMethodDesc+"时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }


    public bool IsReusable {
        get {
            return false;
        }
    }

}