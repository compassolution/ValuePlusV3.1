<%@ WebHandler Language="C#" Class="PrintLabel" %>

using System;
using System.Web;
using System.Collections;
using System.Text;
using System.IO;
using System.Data;
using Com.ValuePlus.BLL.SysManager;
using Com.ValuePlus.BLL.User;
using Com.ValuePlus.DataLog;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Entity;
using Com.ValuePlus.Web;
using Com.ValuePlus.Utils.Serializable;

public class PrintLabel : IHttpHandler {

    /// <summary>
    /// 日志声明
    /// </summary>
    protected static Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    public void ProcessRequest (HttpContext context) {
        //解析客户端传递过来的json data
        //StreamReader reader = new StreamReader(context.Request.InputStream);
        //String strParamJson = HttpUtility.UrlDecode(reader.ReadToEnd());
        //log.Error("传入参数的Json data字符串:" + strParamJson.ToString());

        //string strParam = WebCommon.GetJsonValue(strParamJson,"param").ToString();//请求类型参数

        String strUrlQueryString = context.Server.UrlDecode(context.Request.Url.Query.ToString());
        Hashtable hsTableUrlQuery = WebCommon.GetUrlAnalyse(strUrlQueryString);
        string strParam = hsTableUrlQuery["param"] == null ? string.Empty : hsTableUrlQuery["param"].ToString();//param
        string strAccountId = hsTableUrlQuery["accountid"] == null ? string.Empty : hsTableUrlQuery["accountid"].ToString();

        string strSACODE = hsTableUrlQuery["sacode"] == null ? string.Empty : hsTableUrlQuery["sacode"].ToString();
        string strLabelTypeCode = hsTableUrlQuery["typecode"] == null ? string.Empty : hsTableUrlQuery["typecode"].ToString();

        switch (strParam.ToLower().ToString())
        {
            case "getprintassetslist":
                context.Response.Write(this.GetPrintAssetsList(strSACODE,strAccountId).ToString());
                break;
            case "getlabelprintersetting":
                context.Response.Write(this.GetLabelPrinterSetting(strLabelTypeCode).ToString());
                break;
            case "getlabelfieldsetting":
                context.Response.Write(this.GetLabelFieldSetting(strLabelTypeCode).ToString());
                break;
        }
    }

    /// <summary>
    /// 根据条件获取需打印的资产数据记录
    /// </summary>
    /// <param name="strSACODE"></param>
    /// <param name="strAccountId"></param>
    /// <returns></returns>
    private String GetPrintAssetsList(String strSACODE,String strAccountId)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbReturnData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "根据条件获取需打印的资产数据记录";

        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select * from [TB_AM_PrintAssetsList] A where 1=1 ");
            //String strSql = "select * from [VW_OE_AssetDetail_ForLabelPrint] where SIMAGE IS NOT NULL order by SACODE,SLCODE";
            
            if (!String.IsNullOrEmpty(strAccountId))
            {
                sbSql.Append(" and [OPUserId] = '" + strAccountId + "'");
            }
            if (!String.IsNullOrEmpty(strSACODE))
            {
                sbSql.Append(" and [SACODE] = '" + strSACODE + "'");
            }
            sbSql.Append(" order by PrintOrder DESC");
            
            log.Error(strMethodDesc + "GetData Sql:" + sbSql.ToString());
            DataTable dt = SqlParamDao.GetDataTableBySql(sbSql.ToString());
            //将DataTable转为Byte[]
            DataSet ds = dt.DataSet;
            byte[] db = WinCeDataSetHelper.GetZipBytesByDataSet(ds);
            sbReturnData.Append(Convert.ToBase64String(db));

            strReturnCode = "1";
            strReturnMsg = strMethodDesc + "成功";
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = strMethodDesc + "出错";
            log.Error(ex);
        }
        finally
        {
            StringBuilder sbResultStatus = new StringBuilder();
            sbResultStatus.Append("{");
            sbResultStatus.Append("\"ReturnCode\":\"" + strReturnCode.ToString() + "\"");
            sbResultStatus.Append(",\"ReturnMsg\":\"" + strReturnMsg.ToString() + "\"");

            sbResult.Append(sbResultStatus.ToString());
            if (!String.IsNullOrEmpty(sbReturnData.ToString()))
            {
                sbResult.Append(",\"ReturnData\":\"" + sbReturnData.ToString()+"\"");
            }
            sbResult.Append("}");
            log.Error(strMethodDesc + "Return Json:" + sbResult.ToString());
        }
        return sbResult.ToString();
    }
    
    /// <summary>
    /// 根据标签类型获取打印主设置
    /// </summary>
    /// <param name="strLabelTypeCode"></param>
    /// <returns></returns>
    private String GetLabelPrinterSetting(String strLabelTypeCode)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbReturnData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "根据标签类型获取打印设置";

        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select * from [LabelPrint_1] A where 1=1 ");
            //String strSql = "select * from [VW_OE_AssetDetail_ForLabelPrint] where SIMAGE IS NOT NULL order by SACODE,SLCODE";
            
            if (!String.IsNullOrEmpty(strLabelTypeCode))
            {
                sbSql.Append(" and [TypeCode] = '" + strLabelTypeCode + "'");
            }
            sbSql.Append(" order by TypeCode");
            
            log.Error(strMethodDesc + "GetData Sql:" + sbSql.ToString());
            DataTable dt = SqlParamDao.GetDataTableBySql(sbSql.ToString());
            //将DataTable转为Byte[]
            DataSet ds = dt.DataSet;
            byte[] db = WinCeDataSetHelper.GetZipBytesByDataSet(ds);
            sbReturnData.Append(Convert.ToBase64String(db));

            strReturnCode = "1";
            strReturnMsg = strMethodDesc + "成功";
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = strMethodDesc + "出错";
            log.Error(ex);
        }
        finally
        {
            StringBuilder sbResultStatus = new StringBuilder();
            sbResultStatus.Append("{");
            sbResultStatus.Append("\"ReturnCode\":\"" + strReturnCode.ToString() + "\"");
            sbResultStatus.Append(",\"ReturnMsg\":\"" + strReturnMsg.ToString() + "\"");

            sbResult.Append(sbResultStatus.ToString());
            if (!String.IsNullOrEmpty(sbReturnData.ToString()))
            {
                sbResult.Append(",\"ReturnData\":\"" + sbReturnData.ToString()+"\"");
            }
            sbResult.Append("}");
            log.Error(strMethodDesc + "Return Json:" + sbResult.ToString());
        }
        return sbResult.ToString();
    }
    
    /// <summary>
    /// 根据标签类型获取打印字段的设置
    /// </summary>
    /// <param name="strLabelTypeCode"></param>
    /// <returns></returns>
    private String GetLabelFieldSetting(String strLabelTypeCode)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbReturnData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "根据标签类型获取打印设置";

        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select * from [LabelPrint_2] A where 1=1 and IsShow = '1' ");
            //String strSql = "select * from [VW_OE_AssetDetail_ForLabelPrint] where SIMAGE IS NOT NULL order by SACODE,SLCODE";
            
            if (!String.IsNullOrEmpty(strLabelTypeCode))
            {
                sbSql.Append(" and [TypeCode] = '" + strLabelTypeCode + "'");
            }
            sbSql.Append(" order by [TypeCode],[SEQNO]");
            
            log.Error(strMethodDesc + "GetData Sql:" + sbSql.ToString());
            DataTable dt = SqlParamDao.GetDataTableBySql(sbSql.ToString());
            //将DataTable转为Byte[]
            DataSet ds = dt.DataSet;
            byte[] db = WinCeDataSetHelper.GetZipBytesByDataSet(ds);
            sbReturnData.Append(Convert.ToBase64String(db));

            strReturnCode = "1";
            strReturnMsg = strMethodDesc + "成功";
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = strMethodDesc + "出错";
            log.Error(ex);
        }
        finally
        {
            StringBuilder sbResultStatus = new StringBuilder();
            sbResultStatus.Append("{");
            sbResultStatus.Append("\"ReturnCode\":\"" + strReturnCode.ToString() + "\"");
            sbResultStatus.Append(",\"ReturnMsg\":\"" + strReturnMsg.ToString() + "\"");

            sbResult.Append(sbResultStatus.ToString());
            if (!String.IsNullOrEmpty(sbReturnData.ToString()))
            {
                sbResult.Append(",\"ReturnData\":\"" + sbReturnData.ToString()+"\"");
            }
            sbResult.Append("}");
            log.Error(strMethodDesc + "Return Json:" + sbResult.ToString());
        }
        return sbResult.ToString();
    }
    public bool IsReusable {
        get {
            return false;
        }
    }

}