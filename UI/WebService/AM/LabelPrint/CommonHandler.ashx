<%@ WebHandler Language="C#" Class="CommonHandler" %>

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

public class CommonHandler : IHttpHandler {

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
        string strPassword = hsTableUrlQuery["password"] == null ? string.Empty : hsTableUrlQuery["password"].ToString();
        string strLid = hsTableUrlQuery["lid"] == null ? string.Empty : hsTableUrlQuery["lid"].ToString();

        switch (strParam.ToLower().ToString())
        {
            case "testconnect":
                context.Response.Write(this.TestConnetct().ToString());
                break;
            case "dologin":
                context.Response.Write(this.DoLogin(strAccountId,strPassword).ToString());
                break;
            case "getalldiclist":
                context.Response.Write(this.GetAllDicListByLid(strLid).ToString());
                break;
            case "getalldeptlist":
                context.Response.Write(this.GetAllDeptList().ToString());
                break;
            case "getalllocationlist":
                context.Response.Write(this.GetAllLocationList().ToString());
                break;
            case "getallcategorylist":
                context.Response.Write(this.GetAllAssetCategoryList().ToString());
                break;
            case "getalloeclasslist":
                context.Response.Write(this.GetAllOEClassList().ToString());
                break;
        }
    }


    /// <summary>
    ///测试远程服务链接
    /// </summary>
    private String TestConnetct()
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbReturnData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "测试远程服务链接";
        try
        {
            
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
    ///账号密码登录
    /// </summary>
    private String DoLogin(String strAccountId,String strPassword)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbReturnData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "账号密码登录";
        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append(" SELECT COUNT(*) FROM TB_HR_USER WHERE SUSERID = '" + strAccountId + "' AND dbo.fun_Decode_Password(SPWD) = '" + strPassword + "'");
            int iCount = SqlParamDao.ExecuteScalarBySql(sbSql.ToString());

            if (iCount > 0)
            {
                strReturnCode = "1";
                strReturnMsg = strMethodDesc + "成功";
            }
            else
            {
                strReturnCode = "-1";
                strReturnMsg = strMethodDesc + "失败,请确认账号密码的正确性!";
            }
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
    ///根据LID获取全部字典数据信息
    /// </summary>
    private String GetAllDicListByLid(String strLid)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbReturnData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "根据LID获取全部字典数据信息";
        try
        {
            DataTable dt = DicGetter.GetDictionaryListDataTable(strLid,true);
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
    ///获取全部固定格式的使用部门信息
    /// </summary>
    private String GetAllDeptList()
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbReturnData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "获取全部固定格式的使用部门信息";
        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select * from VW_Sys_TREE_CSORGA where LID = 'CSORGA1'");
            sbSql.Append(" ORDER BY P9");
            String strSql = sbSql.ToString();
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
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
    ///获取全部固定格式的存放地址信息
    /// </summary>
    private String GetAllLocationList()
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbReturnData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "获取全部固定格式的存放地址信息";
        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select * from VW_Sys_TREE_AMLOCATION where LID = 'AMLOCATION1'");
            sbSql.Append(" ORDER BY P9");
            String strSql = sbSql.ToString();
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
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
    ///获取全部固定格式的资产分类信息
    /// </summary>
    private String GetAllAssetCategoryList()
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbReturnData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "获取全部固定格式的资产分类信息";
        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select * from VW_Sys_TREE_ASSETSCLASS where LID = 'ASSETSCLASS1'");
            sbSql.Append(" ORDER BY P9");
            String strSql = sbSql.ToString();
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
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
    ///获取全部固定格式的OE资产分类信息
    /// </summary>
    private String GetAllOEClassList()
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbReturnData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "获取全部固定格式的OE资产分类信息";
        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select * from VW_Sys_TREE_OECLASS where LID = 'OECLASS1'");
            sbSql.Append(" ORDER BY P9");
            String strSql = sbSql.ToString();
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
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