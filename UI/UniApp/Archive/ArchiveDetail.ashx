<%@ WebHandler Language="C#" Class="ArchiveDetail" %>

using System;
using System.Web;
using System.Collections;
using System.Text;
using System.IO;
using System.Data;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Web;
using Com.ValuePlus.Archive.BLL;

public class ArchiveDetail : HandlerBase,IHttpHandler, System.Web.SessionState.IRequiresSessionState
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
        string strTID = WebCommon.GetJsonValue(strParamJson,"tid").ToString();
        string strRID = WebCommon.GetJsonValue(strParamJson,"rid").ToString();
        string strSID = WebCommon.GetJsonValue(strParamJson,"sid").ToString();
        string strGID = WebCommon.GetJsonValue(strParamJson,"gid").ToString();
        string strPID = WebCommon.GetJsonValue(strParamJson,"pid").ToString();
        string strAID = WebCommon.GetJsonValue(strParamJson,"aid").ToString();
        string strEID = WebCommon.GetJsonValue(strParamJson,"eid").ToString();
        string strKEY = WebCommon.GetJsonValue(strParamJson,"key").ToString();
        string strKEYVALUE = WebCommon.GetJsonValue(strParamJson,"keyvalue").ToString();
        string strGRIDKEY = WebCommon.GetJsonValue(strParamJson,"gridkey").ToString();
        string strGRIDKEYVALUE = WebCommon.GetJsonValue(strParamJson,"gridkeyvalue").ToString();
        string strIsLoadAllRole = WebCommon.GetJsonValue(strParamJson,"isloadallrole").ToString();
        string strActionLocation = WebCommon.GetJsonValue(strParamJson,"actionlocation").ToString();
        string strCondition = WebCommon.GetJsonValue(strParamJson,"condition").ToString();
        string strMastValue = WebCommon.GetJsonValue(strParamJson,"mastvalue").ToString();
        string strIsInsert = WebCommon.GetJsonValue(strParamJson,"isinsert").ToString();
        string strSql = WebCommon.GetJsonObjectValue(strParamJson,"sql").ToString();
        //log.Error("传入参数的strSql.1:" + strSql.ToString());
        strSql = Microsoft.JScript.GlobalObject.decodeURIComponent(strSql);
        //log.Error("传入参数的strSql.2:" + strSql.ToString());

        string strSaveData = WebCommon.GetJsonObjectValue(strParamJson,"savedata").ToString();

        String strPageSize = WebCommon.GetJsonValue(strParamJson,"pagesize").ToString();
        String strPageIndex = WebCommon.GetJsonValue(strParamJson,"pageindex").ToString();
        int iPageSize = String.IsNullOrEmpty(strPageSize)? 10 : int.Parse(strPageSize);
        int iPageIndex = String.IsNullOrEmpty(strPageSize)? 0 : int.Parse(strPageIndex);

        String strRequestLanguage = strCulture.Equals("0") ? "zh-cn" : "en-us";
        switch (param.ToLower().ToString())
        {
            case "getarchivedetaildatalist"://获取模板页面相关业务明细数据
                this.GetArchiveDetailDataList(context,strTID,strRID,strSID,strGID,strKEY,strKEYVALUE,strGRIDKEY,strGRIDKEYVALUE,strRequestLanguage,strUserCode);
                break;
            case "getonegroupdetaildata"://获取模板页面单个分组的字段及其相关业务明细数据
                this.GetOneGroupDetailData(context,strTID,strRID,strSID,strGID,strKEY,strKEYVALUE,strGRIDKEY,strGRIDKEYVALUE,strRequestLanguage,strUserCode);
                break;
            case "getonelistgrouplistdata"://获取模板列表型分组的业务数据
                this.GetOneListGroupListData(context,strTID,strRID,strSID,strGID,strKEY,strKEYVALUE,strCondition,iPageSize,iPageIndex,strRequestLanguage,strUserCode);
                break;
            case "getarchiveeventdatalist"://add by sammen 20260305 获取模板场景分组下的有效事件数据
                this.GetArchiveEventDataList(context,strTID,strSID,strGID,strPID,strEID,strUserCode);
                break;
            case "getdatabysql"://通过SQL获取数据jason
                this.GetDataBySQL(context,strSql,strCondition,strMastValue,strRequestLanguage);
                break;
            case "savearchivedetail"://保存模板明细信息
                this.SaveArchiveDetail(context, strTID, strRID, strSID, strGID, strKEY, strKEYVALUE, strGRIDKEY, strGRIDKEYVALUE, strIsInsert, strSaveData, strRequestLanguage, strUserCode);
                break;
            case "deletearchivedetail"://获取模板页面相关业务明细数据
                this.DeleteArchiveDetail(context,strTID,strRID,strSID,strGID,strKEY,strKEYVALUE,strGRIDKEY,strGRIDKEYVALUE,strRequestLanguage,strUserCode);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 获取模板页面相关业务明细数据
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strTID"></param>
    /// <param name="strRID"></param>
    /// <param name="strSID"></param>
    /// <param name="strGID"></param>
    /// <param name="strKey"></param>
    /// <param name="strKeyValue"></param>
    /// <param name="strGridKey"></param>
    /// <param name="strGridKeyValue"></param>
    /// <param name="strRequestLanguage"></param>
    /// <param name="strUserCode"></param>
    public void GetArchiveDetailDataList(HttpContext context,String strTID,String strRID,String strSID,String strGID,String strKey,String strKeyValue
        ,String strGridKey,String strGridKeyValue,String strRequestLanguage,String strUserCode)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        String strMethodDesc = "获取模板页面相关业务明细数据";
        StringBuilder sbResult = new StringBuilder();

        try
        {
            MobileArchive mobileArchive = new MobileArchive();
            String strJsonData = mobileArchive.GetArchiveDetailMain(strUserCode,strTID,strRID,strSID,strGID,strKey,strKeyValue,strGridKey,strGridKeyValue,strRequestLanguage);
            //log.Error(strMethodDesc+":"+strJsonData);

            if(!String.IsNullOrEmpty(strJsonData)){
                strReturnCode = "1";
                strReturnMsg = strMethodDesc+"成功";
            }else{
                strReturnCode = "-1";
                strReturnMsg = strMethodDesc+"获取数据失败";
            }
            sbResultData.Append(strJsonData);
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
                sbResult.Append(",\"ResultData\":{"+sbResultData.ToString()+"}");
            }
            sbResult.Append("}");
        }
        //log.Error(strMethodDesc+"时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }

    /// <summary>
    /// 获取模板页面单个分组的字段及其相关业务明细数据
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strTID"></param>
    /// <param name="strRID"></param>
    /// <param name="strSID"></param>
    /// <param name="strGID"></param>
    /// <param name="strKey"></param>
    /// <param name="strKeyValue"></param>
    /// <param name="strGridKey"></param>
    /// <param name="strGridKeyValue"></param>
    /// <param name="strRequestLanguage"></param>
    /// <param name="strUserCode"></param>
    public void GetOneGroupDetailData(HttpContext context,String strTID,String strRID,String strSID,String strGID,String strKey,String strKeyValue
        ,String strGridKey,String strGridKeyValue,String strRequestLanguage,String strUserCode)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        String strMethodDesc = "获取模板页面单个分组的字段及其相关业务明细数据";
        StringBuilder sbResult = new StringBuilder();

        try
        {
            MobileArchive mobileArchive = new MobileArchive();
            String strJsonData = mobileArchive.GetOneGroupDetailData(strUserCode,strTID,strRID,strSID,strGID,strKey,strKeyValue,strGridKey,strGridKeyValue,strRequestLanguage);
            //log.Error(strMethodDesc+":"+strJsonData);

            if(!String.IsNullOrEmpty(strJsonData)){
                strReturnCode = "1";
                strReturnMsg = strMethodDesc+"成功";
            }else{
                strReturnCode = "-1";
                strReturnMsg = strMethodDesc+"获取数据失败";
            }
            sbResultData.Append(strJsonData);
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
                sbResult.Append(",\"ResultData\":{"+sbResultData.ToString()+"}");
            }
            sbResult.Append("}");
        }
        //log.Error(strMethodDesc+"时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }

    /// <summary>
    /// 获取模板列表型分组的业务数据
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strTID"></param>
    /// <param name="strRID"></param>
    /// <param name="strSID"></param>
    /// <param name="strGID"></param>
    /// <param name="strKey"></param>
    /// <param name="strKeyValue"></param>
    /// <param name="strCondition"></param>
    /// <param name="iPageSize"></param>
    /// <param name="iPageIndex"></param>
    /// <param name="strRequestLanguage"></param>
    /// <param name="strUserCode"></param>
    public void GetOneListGroupListData(HttpContext context,String strTID,String strRID,String strSID,String strGID,String strKey,String strKeyValue
        ,String strCondition,int iPageSize,int iPageIndex,String strRequestLanguage,String strUserCode)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        String strMethodDesc = "获取模板列表型分组的业务数据";
        StringBuilder sbResult = new StringBuilder();

        try
        {
            MobileArchive mobileArchive = new MobileArchive();
            String strJsonData = mobileArchive.GetOneListGroupListData(strUserCode,strTID,strRID,strSID,strGID,strKey,strKeyValue,strCondition,iPageSize,iPageIndex,strRequestLanguage);
            //log.Error(strMethodDesc+":"+strJsonData);

            if(!String.IsNullOrEmpty(strJsonData)){
                strReturnCode = "1";
                strReturnMsg = strMethodDesc+"成功";
            }else{
                strReturnCode = "-1";
                strReturnMsg = strMethodDesc+"获取数据失败";
            }
            sbResultData.Append(strJsonData);
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
                sbResult.Append(",\"ResultData\":{"+sbResultData.ToString()+"}");
            }
            sbResult.Append("}");
        }
        //log.Error(strMethodDesc+"时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }
    
    /// <summary>
    /// 获取模板场景分组下的有效事件数据
    /// //add by sammen 20260305
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strTID"></param>
    /// <param name="strSID"></param>
    /// <param name="strGID"></param>
    /// <param name="strPID"></param>
    /// <param name="strEID"></param>
    /// <param name="strUserCode"></param>
    public void GetArchiveEventDataList(HttpContext context,String strTID,String strSID,String strGID,String strPID,String strEID,String strUserCode)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        String strMethodDesc = "获取模板场景分组下的有效事件数据";
        StringBuilder sbResult = new StringBuilder();

        try
        {
            MobileArchive mobileArchive = new MobileArchive();
            String strJsonData = mobileArchive.GetArchiveEventData(strTID,strSID,strGID,strPID,strEID,strUserCode);
            //log.Error(strMethodDesc+":"+strJsonData);

            if(!String.IsNullOrEmpty(strJsonData)){
                strReturnCode = "1";
                strReturnMsg = strMethodDesc+"成功";
            }else{
                strReturnCode = "-1";
                strReturnMsg = strMethodDesc+"获取数据失败";
            }
            sbResultData.Append(strJsonData);
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
                sbResult.Append(",\"ResultData\":{"+sbResultData.ToString()+"}");
            }
            sbResult.Append("}");
        }
        //log.Error(strMethodDesc+"时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }

    /// <summary>
    /// 通过SQL获取数据jason
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strSql"></param>
    /// <param name="strCondition"></param>
    /// <param name="strMastValue"></param>
    /// <param name="strRequestLanguage"></param>
    public void GetDataBySQL(HttpContext context,String strSql,String strCondition,String strMastValue,String strRequestLanguage)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        String strMethodDesc = "通过SQL获取数据jason";
        StringBuilder sbResult = new StringBuilder();

        try
        {
            //客户端进行的关键字替换，服务端替换回来
            strSql = strSql.Replace("#", "'").Replace("S1E2L3E4C5T","select").Replace("F1R2O3M","from");
            if(!String.IsNullOrEmpty(strSql)){
                strSql = strSql.Replace("@MastValue@",strMastValue);
            }
            log.Error("GetDataBySQL的strSql:" + strSql.ToString());

            MobileArchive mobileArchive = new MobileArchive();
            String strJsonData = mobileArchive.GetDataBySQL(strSql,strCondition,strRequestLanguage);
            //log.Error(strMethodDesc+":"+strJsonData);

            if(!String.IsNullOrEmpty(strJsonData)){
                strReturnCode = "1";
                strReturnMsg = strMethodDesc+"成功";
            }else{
                strReturnCode = "-1";
                strReturnMsg = strMethodDesc+"获取数据失败";
            }
            sbResultData.Append(strJsonData);
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
                sbResult.Append(",\"ResultData\":{"+sbResultData.ToString()+"}");
            }
            sbResult.Append("}");
        }
        //log.Error(strMethodDesc+"时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }


    /// <summary>
    /// 保存档案数据
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strTID"></param>
    /// <param name="strRID"></param>
    /// <param name="strSID"></param>
    /// <param name="strGID"></param>
    /// <param name="strKey"></param>
    /// <param name="strKeyValue"></param>
    /// <param name="strGridKey"></param>
    /// <param name="strGridKeyValue"></param>
    /// <param name="strIsInsert"></param>
    /// <param name="strSaveData"></param>
    /// <param name="strRequestLanguage"></param>
    /// <param name="strUserCode"></param>
    public void SaveArchiveDetail(HttpContext context,String strTID,String strRID,String strSID,String strGID,String strKey,String strKeyValue
        ,String strGridKey,String strGridKeyValue,String strIsInsert,String strSaveData,String strRequestLanguage,String strUserCode)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        String strMethodDesc = "保存档案数据";
        StringBuilder sbResult = new StringBuilder();

        try
        {
            MobileArchive mobileArchive = new MobileArchive();
            String strJsonData = mobileArchive.SaveArchiveDetailData(strUserCode, strTID, strRID, strSID, strKey, strKeyValue
                , strGridKey, strGridKeyValue, strIsInsert, strSaveData, strRequestLanguage);
            //log.Error(strMethodDesc+":"+strJsonData);

            //String strJsonData = strSaveData;
            if(!String.IsNullOrEmpty(strJsonData)){
                strReturnCode = "1";
                strReturnMsg = strMethodDesc+"成功";
            }else{
                strReturnCode = "-1";
                strReturnMsg = strMethodDesc+"获取数据失败";
            }
            sbResultData.Append(strJsonData);
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
                sbResult.Append(",");
                sbResult.Append(sbResultData.ToString());
            }
            sbResult.Append("}");
        }
        //log.Error(strMethodDesc+"时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }

    /// <summary>
    /// 删除模板页面相关业务明细数据
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strTID"></param>
    /// <param name="strRID"></param>
    /// <param name="strSID"></param>
    /// <param name="strGID"></param>
    /// <param name="strKey"></param>
    /// <param name="strKeyValue"></param>
    /// <param name="strGridKey"></param>
    /// <param name="strGridKeyValue"></param>
    /// <param name="strRequestLanguage"></param>
    /// <param name="strUserCode"></param>
    public void DeleteArchiveDetail(HttpContext context,String strTID,String strRID,String strSID,String strGID,String strKey,String strKeyValue
        ,String strGridKey,String strGridKeyValue,String strRequestLanguage,String strUserCode)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        String strMethodDesc = "删除模板页面相关业务明细数据";
        StringBuilder sbResult = new StringBuilder();

        try
        {
            MobileArchive mobileArchive = new MobileArchive();
            String strJsonData = "";
            if (!String.IsNullOrEmpty(strGridKey)&& !String.IsNullOrEmpty(strGID))
            {
                //针对Grid分组的删除
                strJsonData = mobileArchive.DeleteArchiveGridData(strUserCode,strTID,strRID,strSID,strGID,strKey,strKeyValue,strGridKey,strGridKeyValue,strRequestLanguage);
            }else{
                //针对主信息表的删除
                strJsonData = mobileArchive.DeleteArchiveListData(strUserCode,strTID,strRID,strSID,strGID,strKey,strKeyValue,strRequestLanguage);
            }
            //log.Error(strMethodDesc+":"+strJsonData);

            if(!String.IsNullOrEmpty(strJsonData)){
                strReturnCode = "1";
                strReturnMsg = strMethodDesc+"成功";
            }else{
                strReturnCode = "-1";
                strReturnMsg = strMethodDesc+"获取数据失败";
            }
            sbResultData.Append(strJsonData);
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
                sbResult.Append(",\"ResultData\":"+sbResultData.ToString()+"");
            }
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