<%@ WebHandler Language="C#" Class="QueryAMData" %>

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

public class QueryAMData : HandlerBase,IHttpHandler, System.Web.SessionState.IRequiresSessionState
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
            case "getdata_amplanlist"://获取固定资产盘点计划信息数据
                this.GetData_AMPlanList(context,strRequestLanguage);
                break;
            case "getdata_amplanlocationqty"://获取固定资产某计划下各种数量【包括总数，已盘到，盘亏数，盘盈数】
                this.GetData_AMPlanLocationQty(context,strPlanCode,strLocationCode,strUserCode,strIsTakedByUser);
                break;
            case "getdata_amplantakedlist"://获取固定资产某计划下已盘到资产
                this.GetData_AMPlanTakedList(context,strPlanCode,strUserCode,strIsTakedByUser);
                break;                
            case "getdata_inventoryassetsinfo"://根据条件获取对应盘点计划对应地址下的固定资产详细信息【服务器端分页获取】
                this.GetData_InventoryAssetsInfo(context,strGetScope,strQueryCondition,iPageSize,iPageIndex);
                break;
            case "getdata_assetsinfonotpaging"://根据条件获取固定资产详细信息【不分页】
                this.GetData_AssetsInfoNotPaging(context,strQueryCondition);
                break;
            case "getdata_assetslistbypaging"://根据条件获取固定资产详细信息【服务器端分页获取】
                this.GetData_AssetsListByPaging(context,strQueryCondition,iPageSize,iPageIndex);
                break;
            default:
                break;

        }
    }

    /// <summary>
    /// 获取固定资产盘点计划信息数据
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strRequestLanguage"></param>
    public void GetData_AMPlanList(HttpContext context,String strRequestLanguage)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        String strMethodDesc = "获取固定资产盘点计划信息数据";
        StringBuilder sbResult = new StringBuilder();

        try
        {
            DataTable dt = new DataTable();
            String strSql = "";
            //固定资产盘点计划信息数据
            strSql = "select * from [VW_Moblie_AMPlan]";
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
    /// 获取固定资产某计划下各种数量【包括总数，已盘到，盘亏数，盘盈数】
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strPlanCode"></param>
    /// <param name="strLocationCode"></param>
    /// <param name="strUserCode"></param>
    /// <param name="strIsTakedByUser"></param>
    public void GetData_AMPlanLocationQty(HttpContext context,String strPlanCode,String strLocationCode,String strUserCode,String strIsTakedByUser)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        String strMethodDesc = "获取固定资产某计划下各种数量【包括总数，已盘到，盘亏数，盘盈数】";
        StringBuilder sbResult = new StringBuilder();

        try
        {
            DataTable dt = new DataTable();
            String strSql = "";
            //系统存放地址信息数据
            strSql = "select * from dbo.[Fun_AM_GetPlanLocationQty]('"+strPlanCode+"','"+strLocationCode+"','"+strUserCode+"','"+strIsTakedByUser+"')";
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
    /// 获取固定资产某计划下已盘到资产
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strPlanCode"></param>
    /// <param name="strUserCode"></param>
    /// <param name="strIsTakedByUser"></param>
    public void GetData_AMPlanTakedList(HttpContext context,String strPlanCode,String strUserCode,String strIsTakedByUser)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        String strMethodDesc = "获取固定资产某计划下已盘到资产";
        StringBuilder sbResult = new StringBuilder();

        try
        {
            DataTable dt = new DataTable();
            String strSql = "";
            //系统存放地址信息数据
            strSql = "select * from dbo.[Fun_AM_GetPlanTakedList]('"+strPlanCode+"','"+strUserCode+"','"+strIsTakedByUser+"')";
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
    /// 根据条件获取对应计划对应地址下的固定资产详细信息【服务器端分页获取】
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strGetScope">获取范围（all/taked/lost/profit）</param>
    /// <param name="iPageSize"></param>
    /// <param name="iPageIndex"></param>
    public void GetData_InventoryAssetsInfo(HttpContext context,String strGetScope,String strQueryCondition,int iPageSize,int iPageIndex)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        String strMethodDesc = "根据条件获取对应计划对应地址下的固定资产详细信息【服务器端分页获取】";
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
                if(strGetScope.ToLower().Equals("taked")){//条件下的已盘到
                    sbSql.Append("select A.* from [VW_Moblie_AMTaked] B inner join VW_Moblie_AssetDetail A ON B.SEPCID = A.SBARCODE \r\n");
                    sbSql.Append("where ('"+strIsTakedByUser+"' = '0' or SUSERID = '"+strUserCode+"') \r\n");
                    sbSql.Append("and PCODE = '"+strPlanCode+"' AND (B.SLCODE in (SELECT SubCode from dbo.[Fun_AM_GetSubLocation]('"+strLocationCode+"','1')) or B.SLCODE = '"+strLocationCode+"')  \r\n");
                    sbSql.Append("and SEPCID IN (SELECT SBARCODE FROM AMASSETS_1 WHERE (SLCODE in (SELECT SubCode from dbo.[Fun_AM_GetSubLocation]('"+strLocationCode+"','1')) or SLCODE = '"+strLocationCode+"'))  \r\n");
                }else if(strGetScope.ToLower().Equals("lost")){//条件下的盘亏
                    sbSql.Append("select * from VW_Moblie_AssetDetail A  \r\n");
                    sbSql.Append("where (A.SLCODE in (SELECT SubCode from dbo.[Fun_AM_GetSubLocation]('"+strLocationCode+"','1')) or A.SLCODE = '"+strLocationCode+"') \r\n");
                    sbSql.Append("and SBARCODE NOT IN (select SEPCID from [VW_Moblie_AMTaked] WHERE PCODE = '"+strPlanCode+"' AND ('"+strIsTakedByUser+"' = '0' or SUSERID = '"+strUserCode+"') \r\n");
                    sbSql.Append("  AND (SLCODE in (SELECT SubCode from dbo.[Fun_AM_GetSubLocation]('"+strLocationCode+"','1')) or SLCODE = '"+strLocationCode+"')  \r\n");
                    sbSql.Append(")  \r\n");
                }else if(strGetScope.ToLower().Equals("profit")){//条件下的盘盈
                    sbSql.Append("select A.* from [VW_Moblie_AMTaked] B inner join VW_Moblie_AssetDetail A ON B.SEPCID = A.SBARCODE \r\n");
                    sbSql.Append("where ('"+strIsTakedByUser+"' = '0' or SUSERID = '"+strUserCode+"')  \r\n");
                    sbSql.Append("and PCODE = '"+strPlanCode+"' AND (B.SLCODE in (SELECT SubCode from dbo.[Fun_AM_GetSubLocation]('"+strLocationCode+"','1')) or B.SLCODE = '"+strLocationCode+"')  \r\n");
                    sbSql.Append("and SEPCID NOT IN (SELECT SBARCODE FROM AMASSETS_1 WHERE (SLCODE in (SELECT SubCode from dbo.[Fun_AM_GetSubLocation]('"+strLocationCode+"','1')) or SLCODE = '"+strLocationCode+"'))  \r\n");
                }else{//条件下的全部
                    sbSql.Append("select A.* from VW_Moblie_AssetDetail A \r\n");
                    sbSql.Append("where (A.SLCODE in (SELECT SubCode from dbo.[Fun_AM_GetSubLocation]('"+strLocationCode+"','1')) or A.SLCODE = '"+strLocationCode+"') \r\n");
                }

                if(!String.IsNullOrEmpty(strSearchText)){
                    sbSql.Append("and (A.SACODE LIKE '%"+strSearchText+"%' \r\n");
                    sbSql.Append("or A.SANAME LIKE '%"+strSearchText+"%' \r\n");
                    sbSql.Append("or A.SANAMECN LIKE '%"+strSearchText+"%' \r\n");
                    sbSql.Append("or A.SMODEL LIKE '%"+strSearchText+"%' \r\n");
                    sbSql.Append(") \r\n");
                }

                String strCountSql = PagingSqlUtil.GetCountSql(sbSql.ToString());
                iTotalRecord = SqlParamDao.ExecuteScalarBySql(strCountSql);
                String strSql = PagingSqlUtil.GetPagingSql(sbSql.ToString(), iPageSize, iPageIndex, "SACODE", "SUSEDEPT,SLCODE,SACODE");

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
    /// 根据条件获取固定资产详细信息【不分页】
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strQueryCondition"></param>
    public void GetData_AssetsInfoNotPaging(HttpContext context,String strQueryCondition)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        String strMethodDesc = "根据条件获取固定资产详细信息【不分页】";
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

                sbSql.Append("select A.* from VW_Moblie_AssetDetail A where 1 =1\r\n");
                if (!String.IsNullOrEmpty(strAssetCode)) {
                    sbSql.Append("and A.SACODE = '"+strAssetCode+"' \r\n");
                }
                if (!String.IsNullOrEmpty(strEpcId)) {
                    sbSql.Append("and (A.SACODE = '"+strEpcId+"' OR A.SBARCODE = '"+strEpcId+"') \r\n");
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
    /// 根据条件获取固定资产详细信息【分页获取】
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strQueryCondition"></param>
    /// <param name="iPageSize"></param>
    /// <param name="iPageIndex"></param>
    public void GetData_AssetsListByPaging(HttpContext context,String strQueryCondition,int iPageSize,int iPageIndex)
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
                //String strAssetCode = joCondition["assetCode"] == null?"":joCondition["assetCode"].ToString().Replace("'","''");
                //String strAssetName = joCondition["assetName"] == null?"":joCondition["assetName"].ToString().Replace("'","''");
                //String strModel = joCondition["model"] == null?"":joCondition["model"].ToString().Replace("'","''");
                //String strEpcId = joCondition["epcId"] == null?"":joCondition["epcId"].ToString().Replace("'","''");
                //String strLocationName = joCondition["locationName"] == null?"":joCondition["locationName"].ToString().Replace("'","''");
                //String strDeptName = joCondition["deptName"] == null?"":joCondition["deptName"].ToString().Replace("'","''");
                //String strTypeName = joCondition["typeName"] == null?"":joCondition["typeName"].ToString().Replace("'","''");
                String strSearchText = joCondition["searchText"] == null?"":joCondition["searchText"].ToString().Replace("'","''");

                DataTable dt = new DataTable();
                StringBuilder sbSql = new StringBuilder();

                sbSql.Append("select A.* from VW_Moblie_AssetDetail A where 1 =1\r\n");
                //if (!String.IsNullOrEmpty(strAssetCode)) {
                //    sbSql.Append("and A.SLCODE like '%"+strAssetCode+"%' \r\n");
                //}
                //if (!String.IsNullOrEmpty(strModel)) {
                //    sbSql.Append("and A.SMODEL like '%"+strModel+"%' \r\n");
                //}
                //if (!String.IsNullOrEmpty(strEpcId)) {
                //    sbSql.Append("and A.SBARCODE like '%"+strEpcId+"%' \r\n");
                //}
                //if (!String.IsNullOrEmpty(strAssetName)) {
                //    sbSql.Append("and (A.SANAME like '%"+strAssetName+"%' OR A.SANAMECN like '%"+strAssetName+"%') \r\n");
                //}
                //if (!String.IsNullOrEmpty(strLocationName)) {
                //    sbSql.Append("and (A.SLOCATIONNAME like '%"+strLocationName+"%' OR A.SLOCATIONNAMECN like '%"+strLocationName+"%') \r\n");
                //}
                //if (!String.IsNullOrEmpty(strDeptName)) {
                //    sbSql.Append("and (A.SUSEDEPTNAME like '%"+strDeptName+"%' OR A.SUSEDEPTNAMECN like '%"+strDeptName+"%') \r\n");
                //}
                //if (!String.IsNullOrEmpty(strTypeName)) {
                //    sbSql.Append("and (A.STYPENAME like '%"+strTypeName+"%' OR A.STYPENAMECN like '%"+strTypeName+"%') \r\n");
                //}

                if(!String.IsNullOrEmpty(strSearchText)){
                    sbSql.Append("and (A.SACODE LIKE '%"+strSearchText+"%' \r\n");
                    sbSql.Append("or A.SBARCODE LIKE '%"+strSearchText+"%' \r\n");
                    sbSql.Append("or A.SANAME LIKE '%"+strSearchText+"%' \r\n");
                    sbSql.Append("or A.SANAMECN LIKE '%"+strSearchText+"%' \r\n");
                    sbSql.Append("or A.SMODEL LIKE '%"+strSearchText+"%' \r\n");
                    sbSql.Append("or A.SLOCATIONNAME LIKE '%"+strSearchText+"%' \r\n");
                    sbSql.Append("or A.SLOCATIONNAMECN LIKE '%"+strSearchText+"%' \r\n");
                    sbSql.Append("or A.SUSEDEPTNAME LIKE '%"+strSearchText+"%' \r\n");
                    sbSql.Append("or A.SUSEDEPTNAMECN LIKE '%"+strSearchText+"%' \r\n");
                    sbSql.Append("or A.STYPENAME LIKE '%"+strSearchText+"%' \r\n");
                    sbSql.Append("or A.STYPENAMECN LIKE '%"+strSearchText+"%' \r\n");
                    sbSql.Append("or A.LABELTYPENAME LIKE '%"+strSearchText+"%' \r\n");
                    sbSql.Append("or A.LABELTYPENAMECN LIKE '%"+strSearchText+"%' \r\n");
                    sbSql.Append(") \r\n");
                }

                String strCountSql = PagingSqlUtil.GetCountSql(sbSql.ToString());
                iTotalRecord = SqlParamDao.ExecuteScalarBySql(strCountSql);
                String strSql = PagingSqlUtil.GetPagingSql(sbSql.ToString(), iPageSize, iPageIndex, "SACODE", "SUSEDEPT,SLCODE,SACODE");

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