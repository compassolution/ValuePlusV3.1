<%@ WebHandler Language="C#" Class="FileSyncData" %>

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

public class FileSyncData : IHttpHandler {

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
        string strSEQ = hsTableUrlQuery["seq"] == null ? string.Empty : hsTableUrlQuery["seq"].ToString();
        string strPCODE = hsTableUrlQuery["pcode"] == null ? string.Empty : hsTableUrlQuery["pcode"].ToString();
        string strObjectType = hsTableUrlQuery["objecttype"] == null ? string.Empty : hsTableUrlQuery["objecttype"].ToString();
        string strBarCode = hsTableUrlQuery["barcode"] == null ? string.Empty : hsTableUrlQuery["barcode"].ToString();
        string strLocationCode = hsTableUrlQuery["locationcode"] == null ? string.Empty : hsTableUrlQuery["locationcode"].ToString();
        string strQty = hsTableUrlQuery["qty"] == null ? string.Empty : hsTableUrlQuery["qty"].ToString();
        string strUserId = hsTableUrlQuery["userid"] == null ? string.Empty : hsTableUrlQuery["userid"].ToString();
        string strSystime = hsTableUrlQuery["systime"] == null ? string.Empty : hsTableUrlQuery["systime"].ToString();
        int iPageIndex = hsTableUrlQuery["pageindex"] == null ? 0 : int.Parse(hsTableUrlQuery["pageindex"].ToString());
        int iPageSize = hsTableUrlQuery["pagesize"] == null ? 0 : int.Parse(hsTableUrlQuery["pagesize"].ToString());

        switch (strParam.ToLower().ToString())
        {
            case "getexportdata":
                //从服务器导出全部基础数据(图片除外)
                context.Response.Write(this.GetExportData().ToString());
                break;
            case "getexportimagedatarecordcount":
                // 从服务器获取全部资产图片数据的记录数
                context.Response.Write(this.GetExportImageDataRecordCount(iPageSize).ToString());
                break;
            case "getexportimagedata":
                // 从服务器分页导出全部资产图片数据
                context.Response.Write(this.GetExportImageData(iPageIndex,iPageSize).ToString());
                break;
            case "importstockdata":
                //导入盘点数据
                context.Response.Write(this.ImportStockData(strSEQ,strPCODE,strObjectType,strBarCode,strLocationCode,strQty,strUserId,strSystime).ToString());
                break;
        }
    }

    /// <summary>
    ///从服务器导出全部基础数据(图片除外)
    /// </summary>
    private String GetExportData()
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbReturnData_BasicParam = new StringBuilder();
        StringBuilder sbReturnData_DeptInfo = new StringBuilder();
        StringBuilder sbReturnData_LocationInfo = new StringBuilder();
        StringBuilder sbReturnData_UserInfo = new StringBuilder();
        StringBuilder sbReturnData_AssetsInfo = new StringBuilder();
        StringBuilder sbReturnData_AMPlan = new StringBuilder();
        StringBuilder sbReturnData_AMTaked = new StringBuilder();
        StringBuilder sbReturnData_AMImageData = new StringBuilder();
        StringBuilder sbReturnData_OEItem = new StringBuilder();
        StringBuilder sbReturnData_OELocation = new StringBuilder();
        StringBuilder sbReturnData_OEPlan = new StringBuilder();
        StringBuilder sbReturnData_OETaked = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "从服务器导出全部同步的数据";
        try
        {
            DataSet ds_BasicParam = SqlParamDao.GetDataSetBySql("select * from VW_Moblie_BasicParam");
            DataSet ds_DeptInfo = SqlParamDao.GetDataSetBySql("select * from VW_Moblie_Dept");
            DataSet ds_LocationInfo = SqlParamDao.GetDataSetBySql("select * from VW_Moblie_Location");
            DataSet ds_UserInfo = SqlParamDao.GetDataSetBySql("select * from VW_Moblie_User");
            DataSet ds_AssetsInfo = SqlParamDao.GetDataSetBySql("select * from VW_Moblie_AssetDetail_ForDownload");
            DataSet ds_AMPlan = SqlParamDao.GetDataSetBySql("select * from VW_Moblie_AMPlan");
            DataSet ds_AMTaked = SqlParamDao.GetDataSetBySql("select * from VW_Moblie_AMTaked");
            DataSet ds_OEItem = SqlParamDao.GetDataSetBySql("select * from VW_Moblie_OE_Item");
            DataSet ds_OELocation = SqlParamDao.GetDataSetBySql("select * from VW_Moblie_OE_Location");
            DataSet ds_OEPlan = SqlParamDao.GetDataSetBySql("select * from VW_Moblie_OEPlan");
            DataSet ds_OETaked = SqlParamDao.GetDataSetBySql("select * from VW_Moblie_OETaked");

            //将DataTable转为Byte[]
            sbReturnData_BasicParam.Append(Convert.ToBase64String(WinCeDataSetHelper.GetZipBytesByDataSet(ds_BasicParam)));
            sbReturnData_DeptInfo.Append(Convert.ToBase64String(WinCeDataSetHelper.GetZipBytesByDataSet(ds_DeptInfo)));
            sbReturnData_LocationInfo.Append(Convert.ToBase64String(WinCeDataSetHelper.GetZipBytesByDataSet(ds_LocationInfo)));
            sbReturnData_UserInfo.Append(Convert.ToBase64String(WinCeDataSetHelper.GetZipBytesByDataSet(ds_UserInfo)));
            sbReturnData_AssetsInfo.Append(Convert.ToBase64String(WinCeDataSetHelper.GetZipBytesByDataSet(ds_AssetsInfo)));
            sbReturnData_AMPlan.Append(Convert.ToBase64String(WinCeDataSetHelper.GetZipBytesByDataSet(ds_AMPlan)));
            sbReturnData_AMTaked.Append(Convert.ToBase64String(WinCeDataSetHelper.GetZipBytesByDataSet(ds_AMTaked)));
            sbReturnData_OEItem.Append(Convert.ToBase64String(WinCeDataSetHelper.GetZipBytesByDataSet(ds_OEItem)));
            sbReturnData_OELocation.Append(Convert.ToBase64String(WinCeDataSetHelper.GetZipBytesByDataSet(ds_OELocation)));
            sbReturnData_OEPlan.Append(Convert.ToBase64String(WinCeDataSetHelper.GetZipBytesByDataSet(ds_OEPlan)));
            sbReturnData_OETaked.Append(Convert.ToBase64String(WinCeDataSetHelper.GetZipBytesByDataSet(ds_OETaked)));

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

            sbResult.Append(",\"ReturnData\":{");
            sbResult.Append("\"BasicParam\":\"" + sbReturnData_BasicParam.ToString()+"\"");
            sbResult.Append(",\"DeptInfo\":\"" + sbReturnData_DeptInfo.ToString() + "\"");
            sbResult.Append(",\"LocationInfo\":\"" + sbReturnData_LocationInfo.ToString() + "\"");
            sbResult.Append(",\"UserInfo\":\"" + sbReturnData_UserInfo.ToString() + "\"");
            sbResult.Append(",\"AssetsInfo\":\"" + sbReturnData_AssetsInfo.ToString() + "\"");
            sbResult.Append(",\"AMPlan\":\"" + sbReturnData_AMPlan.ToString() + "\"");
            sbResult.Append(",\"AMTaked\":\"" + sbReturnData_AMTaked.ToString() + "\"");
            sbResult.Append(",\"OEItem\":\"" + sbReturnData_OEItem.ToString() + "\"");
            sbResult.Append(",\"OELocation\":\"" + sbReturnData_OELocation.ToString() + "\"");
            sbResult.Append(",\"OEPlan\":\"" + sbReturnData_OEPlan.ToString() + "\"");
            sbResult.Append(",\"OETaked\":\"" + sbReturnData_OETaked.ToString() + "\"");
            sbResult.Append("}");

            sbResult.Append("}");
            log.Error(strMethodDesc + "Return Json:" + sbResult.ToString());
        }
        return sbResult.ToString();
    }
    
    /// <summary>
    /// 从服务器获取全部资产图片数据的记录数
    /// </summary>
    /// <param name="iPageSize"></param>
    /// <returns></returns>
    private String GetExportImageDataRecordCount(int iPageSize)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbReturnData_AMImageData = new StringBuilder();
        StringBuilder sbReturnData_OEImageData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "从服务器获取全部资产图片数据的记录数";
        int iRecordCount = 0;
        int iPageCount = 0;
        try
        {
            String strSqlAllCount = "select count(1) as count from VW_Mobile_AssetImageData";
            iRecordCount = SqlParamDao.ExecuteScalarBySql(strSqlAllCount);
            //返回分页信息
            iPageCount = iRecordCount / iPageSize;
            if (iRecordCount % iPageSize != 0) { iPageCount = iPageCount + 1; }

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

            sbResult.Append(",\"ReturnData\":{");
            sbResult.Append("\"RecordCount\":\"" + iRecordCount.ToString() + "\"");
            sbResult.Append(",\"PageCount\":\"" + iPageCount.ToString() + "\"");
            sbResult.Append(",\"PageSize\":\"" + iPageSize.ToString() + "\"");
            sbResult.Append("}");

            sbResult.Append("}");
            log.Error(strMethodDesc + "Return Json:" + sbResult.ToString());
        }
        return sbResult.ToString();
    }

    /// <summary>
    /// 从服务器分页导出全部资产图片数据
    /// </summary>
    /// <param name="iPageIndex"></param>
    /// <param name="iPageSize"></param>
    /// <returns></returns>
    private String GetExportImageData(int iPageIndex,int iPageSize)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbReturnData_AMImageData = new StringBuilder();
        StringBuilder sbReturnData_OEImageData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "从服务器导出全部资产图片数据";
        try
        {
            // 计算跳过的记录数
            int iSkipRows = iPageIndex * iPageSize;
            // 使用OFFSET FETCH进行分页，适用于SQL Server 2012及以上版本
            string strSql = " SELECT *  FROM VW_Mobile_AssetImageData ORDER BY IMAGENAME OFFSET "+iSkipRows.ToString()+" ROWS FETCH NEXT "+iPageSize.ToString()+" ROWS ONLY";

            DataSet ds_AMImageData = SqlParamDao.GetDataSetBySql(strSql);

            //将DataTable转为Byte[]
            sbReturnData_AMImageData.Append(Convert.ToBase64String(WinCeDataSetHelper.GetZipBytesByDataSet(ds_AMImageData)));

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

            sbResult.Append(",\"ReturnData\":{");
            sbResult.Append("\"AMImageData\":\"" + sbReturnData_AMImageData.ToString() + "\"");
            sbResult.Append(",\"PageIndex\":\"" + iPageIndex.ToString() + "\"");
            sbResult.Append(",\"PageSize\":\"" + iPageSize.ToString() + "\"");
            sbResult.Append("}");

            sbResult.Append("}");
            log.Error(strMethodDesc + "Return Json:" + sbResult.ToString());
        }
        return sbResult.ToString();
    }

    /// <summary>
    ///导入盘点数据
    /// </summary>
    private String ImportStockData(String strSEQ,String strPCODE,String strObjectType,String strBarCode,String strLocationCode,String strQty,String strUserId,String strSystime)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbReturnData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "设置某些资产为待打印列表";
        try
        {
            String strSPName = "USP_AM_Moblie_SaveOneTaked";
            Hashtable hsTableParam = new Hashtable();
            hsTableParam.Add("SEQ", strSEQ);
            hsTableParam.Add("PCODE", strPCODE);
            hsTableParam.Add("ObjectType", strObjectType);
            hsTableParam.Add("SBARCODE", strBarCode);
            hsTableParam.Add("SLCODE", strLocationCode);
            hsTableParam.Add("NQTY", strQty);
            hsTableParam.Add("USERID", strUserId);
            hsTableParam.Add("SYSTIME", strSystime);


            //sbSql.Append("exec [USP_AM_Moblie_SaveOneTaked] '" + strSEQ + "','" + strPCODE + "','" + strObjectType + "','" + strBarCode + "','" + strLocationCode + "','" + strQty + "','" + strUserId + "','" + strSystime + "';\r\n");

            int iCount = SqlParamDao.ExcuteSP(strSPName, hsTableParam);

            if (iCount>0)
            {
                strReturnCode = "1";
                strReturnMsg = strMethodDesc + "成功";
            }else{
                strReturnCode = iCount.ToString();
                strReturnMsg = strMethodDesc + "失败";
            }
            sbReturnData.Append(iCount.ToString());
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