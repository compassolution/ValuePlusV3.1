<%@ WebHandler Language="C#" Class="DownLoadInfo" %>

using System;
using System.Web;
using System.Collections;
using System.Text;
using System.Data;
using Newtonsoft.Json;
using Com.ValuePlus.BLL.SysManager;
using Com.ValuePlus.BLL.User;
using Com.ValuePlus.DataLog;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Entity;
using Com.ValuePlus.Web;
using Com.ValuePlus.Archive.Utils;
using Com.ValuePlus.Common.Security;

public class DownLoadInfo : HandlerBase, IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    #region 日志声明
    /// <summary>
    /// 日志声明
    /// </summary>
    private Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    #endregion

    public void ProcessRequest(HttpContext context)
    {
        string param = context.Request["param"] == null ? string.Empty : context.Request["param"].ToString();//请求类型参数
        int iPageSize = context.Request["pagesize"] == null ? 20 : int.Parse(context.Request["pagesize"].ToString());//每页条数
        string strRequestLanguage = context.Request["language"] == null ? string.Empty : context.Request["language"].ToString();//终端请求时的语言
        string strAppOldVersion = context.Request["oldversion"] == null ? string.Empty : context.Request["oldversion"].ToString();//当前APP版本号
        object objTakedData = context.Request["datataked"] == null ? string.Empty : context.Request["datataked"];//上传数据对象

        //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
        param = SQLInjectionDefense.ReplaceSQLReservedKeyword(param);
        strRequestLanguage = SQLInjectionDefense.ReplaceSQLReservedKeyword(strRequestLanguage);
        strAppOldVersion = SQLInjectionDefense.ReplaceSQLReservedKeyword(strAppOldVersion);

        switch (param.ToLower().ToString())
        {
            case "getalllocations":
                this.GetAllLoactionsInfo(context, strRequestLanguage);
                break;
            case "getalldepts":
                this.GetAllDeptsInfo(context, strRequestLanguage);
                break;
            case "getallassets":
                this.GetAllAssetsInfo(context, strRequestLanguage);
                break;
            case "getalloeitem":
                this.GetAllOEItemInfo(context, strRequestLanguage);
                break;
            case "getalloelocation":
                this.GetAllOELocationInfo(context, strRequestLanguage);
                break;
            case "getallusers":
                this.GetAllUsersInfo(context, strRequestLanguage);
                break;
            case "getallbasicparams":
                this.GetAllBasicParamsInfo(context, strRequestLanguage);
                break;
            case "getallamplan":
                this.GetAllAMPlanInfo(context, strRequestLanguage);
                break;
            case "getalloeplan":
                this.GetAllOEPlanInfo(context, strRequestLanguage);
                break;
            case "getallamtaked":
                this.GetAllAMTakedInfo(context, strRequestLanguage);
                break;
            case "getalloetaked":
                this.GetAllOETakedInfo(context, strRequestLanguage);
                break;
            case "uploadtaked":
                this.UploadTakedAssetsInfo(context, strRequestLanguage, objTakedData);
                break;
            case "versionupdate":
                this.UpdateVersion(context, strAppOldVersion);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 获取所有地址信息
    /// </summary>
    /// <param name="context"></param>
    public void GetAllLoactionsInfo(HttpContext context, String strRequestLanguage)
    {
        try
        {
            String strSql = "select * from [VW_Moblie_Location] order by SLCODE";
            DataTable dtResultData = SqlParamDao.GetDataTableBySql(strSql);

            string json = WebCommon.GetDataRowJsonString(dtResultData, "ResultData", "");
            //json = "2";
            context.Response.Write(json);
        }
        catch (Exception ex)
        {
            log.Error(ex);
            context.Response.Write("error");
        }
    }

    /// <summary>
    /// 获取所有资产信息
    /// </summary>
    /// <param name="context"></param>
    public void GetAllAssetsInfo(HttpContext context, String strRequestLanguage)
    {
        try
        {
            log.Error("获取所有资产信息,开始时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")) ;
            String strSql = "select * from [VW_Moblie_AssetDetail_ForDownload] with(nolock) order by SACODE";
            DataTable dtResultData = SqlParamDao.GetDataTableBySql(strSql);

            string json = WebCommon.GetDataRowJsonString(dtResultData, "ResultData", "");
            //json = "2";
            log.Error("获取所有资产信息,结束时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")) ;
            context.Response.Write(json);
        }
        catch (Exception ex)
        {
            log.Error(ex);
            context.Response.Write("error");
        }
    }

    /// <summary>
    /// 获取所有OE资产信息
    /// </summary>
    /// <param name="context"></param>
    public void GetAllOEItemInfo(HttpContext context, String strRequestLanguage)
    {
        try
        {
            String strSql = "select * from [VW_Moblie_OE_Item] order by SACODE";
            DataTable dtResultData = SqlParamDao.GetDataTableBySql(strSql);

            string json = WebCommon.GetDataRowJsonString(dtResultData, "ResultData", "");
            //json = "2";
            context.Response.Write(json);
        }
        catch (Exception ex)
        {
            log.Error(ex);
            context.Response.Write("error");
        }
    }

    /// <summary>
    /// 获取所有OE资产地址相应信息
    /// </summary>
    /// <param name="context"></param>
    public void GetAllOELocationInfo(HttpContext context, String strRequestLanguage)
    {
        try
        {
            String strSql = "select * from [VW_Moblie_OE_Location] order by SACODE";
            DataTable dtResultData = SqlParamDao.GetDataTableBySql(strSql);

            string json = WebCommon.GetDataRowJsonString(dtResultData, "ResultData", "");
            //json = "2";
            context.Response.Write(json);
        }
        catch (Exception ex)
        {
            log.Error(ex);
            context.Response.Write("error");
        }
    }

    /// <summary>
    /// 获取所有部门信息
    /// </summary>
    /// <param name="context"></param>
    public void GetAllDeptsInfo(HttpContext context, String strRequestLanguage)
    {
        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select * from [VW_Moblie_Dept] order BY OID");
            String strSql = sbSql.ToString();

            DataTable dtResultData = SqlParamDao.GetDataTableBySql(strSql);

            string json = WebCommon.GetDataRowJsonString(dtResultData, "ResultData", "");
            //json = "2";
            context.Response.Write(json);
        }
        catch (Exception ex)
        {
            log.Error(ex);
            context.Response.Write("error");
        }
    }


    /// <summary>
    /// 获取所有用户信息
    /// </summary>
    /// <param name="context"></param>
    public void GetAllUsersInfo(HttpContext context, String strRequestLanguage)
    {
        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select * from [VW_Moblie_User] order by SUSERID");
            String strSql = sbSql.ToString();

            DataTable dtResultData = SqlParamDao.GetDataTableBySql(strSql);

            string json = WebCommon.GetDataRowJsonString(dtResultData, "ResultData", "");
            //json = "2";
            context.Response.Write(json);
        }
        catch (Exception ex)
        {
            log.Error(ex);
            context.Response.Write("error");
        }
    }

    /// <summary>
    /// 获取所有系统基本参数信息
    /// </summary>
    /// <param name="context"></param>
    public void GetAllBasicParamsInfo(HttpContext context, String strRequestLanguage)
    {
        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select * from [VW_Moblie_BasicParam]");
            String strSql = sbSql.ToString();

            DataTable dtResultData = SqlParamDao.GetDataTableBySql(strSql);

            string json = WebCommon.GetDataRowJsonString(dtResultData, "ResultData", "");
            //json = "2";
            context.Response.Write(json);
        }
        catch (Exception ex)
        {
            log.Error(ex);
            context.Response.Write("error");
        }
    }

    /// <summary>
    /// 获取未完成的固定资产盘点计划信息
    /// </summary>
    /// <param name="context"></param>
    public void GetAllAMPlanInfo(HttpContext context, String strRequestLanguage)
    {
        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select * from [VW_Moblie_AMPlan]");
            String strSql = sbSql.ToString();

            DataTable dtResultData = SqlParamDao.GetDataTableBySql(strSql);

            string json = WebCommon.GetDataRowJsonString(dtResultData, "ResultData", "");
            //json = "2";
            context.Response.Write(json);
        }
        catch (Exception ex)
        {
            log.Error(ex);
            context.Response.Write("error");
        }
    }

    /// <summary>
    /// 获取未完成的OE资产盘点计划信息
    /// </summary>
    /// <param name="context"></param>
    public void GetAllOEPlanInfo(HttpContext context, String strRequestLanguage)
    {
        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select * from [VW_Moblie_OEPlan]");
            String strSql = sbSql.ToString();

            DataTable dtResultData = SqlParamDao.GetDataTableBySql(strSql);

            string json = WebCommon.GetDataRowJsonString(dtResultData, "ResultData", "");
            //json = "2";
            context.Response.Write(json);
        }
        catch (Exception ex)
        {
            log.Error(ex);
            context.Response.Write("error");
        }
    }

    /// <summary>
    /// 获取未完成的固定资产盘点数据信息
    /// </summary>
    /// <param name="context"></param>
    public void GetAllAMTakedInfo(HttpContext context, String strRequestLanguage)
    {
        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select * from [VW_Moblie_AMTaked]");
            String strSql = sbSql.ToString();

            DataTable dtResultData = SqlParamDao.GetDataTableBySql(strSql);

            string json = WebCommon.GetDataRowJsonString(dtResultData, "ResultData", "");
            //json = "2";
            context.Response.Write(json);
        }
        catch (Exception ex)
        {
            log.Error(ex);
            context.Response.Write("error");
        }
    }

    /// <summary>
    /// 获取未完成的OE资产盘点数据信息
    /// </summary>
    /// <param name="context"></param>
    public void GetAllOETakedInfo(HttpContext context, String strRequestLanguage)
    {
        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select * from [VW_Moblie_OETaked]");
            String strSql = sbSql.ToString();

            DataTable dtResultData = SqlParamDao.GetDataTableBySql(strSql);

            string json = WebCommon.GetDataRowJsonString(dtResultData, "ResultData", "");
            //json = "2";
            context.Response.Write(json);
        }
        catch (Exception ex)
        {
            log.Error(ex);
            context.Response.Write("error");
        }
    }

    /// <summary>
    /// 上传盘点数据【直接带上盘点计划写入】
    /// </summary>
    /// <param name="context"></param>
    public void UploadTakedAssetsInfo(HttpContext context, String strRequestLanguage,Object objTakedAssets)
    {
        StringBuilder sbSql = new StringBuilder();
        String strReturn = "0";
        try
        {
            String strJson = objTakedAssets.ToString();
            Newtonsoft.Json.Linq.JArray jsonArray = (Newtonsoft.Json.Linq.JArray)JsonConvert.DeserializeObject(strJson);

            for (int i = 0; i < jsonArray.Count; i++)
            {
                //首先判断是资产还是OE的盘点数据
                String strSEQ = jsonArray[i]["SEQ"].ToString();
                String strPCODE = jsonArray[i]["PCODE"].ToString();
                String strObjectType = jsonArray[i]["OBJECTTYPE"].ToString();
                String strBarCode = jsonArray[i]["SBARCODE"].ToString();
                String strLocationCode = jsonArray[i]["SLCODE"].ToString();
                String strQty = jsonArray[i]["NQTY"].ToString();
                String strUserId = jsonArray[i]["USERID"].ToString();
                String strSystime = jsonArray[i]["SYSTIME"].ToString();

                sbSql.Append("exec [USP_AM_Moblie_SaveOneTaked] '"+strSEQ+"','"+strPCODE+"','"+strObjectType+"','"+strBarCode+"','"+strLocationCode+"','"+strQty+"','"+strUserId+"','"+strSystime+"';\r\n");
            }
            if (!String.IsNullOrEmpty(sbSql.ToString()))
            {
                log.Error(sbSql.ToString());
                SqlParamDao.ExecuteNonQueryBySql(sbSql.ToString());
                strReturn = (jsonArray.Count).ToString();
            }
        }
        catch (Exception ex)
        {
            log.Error("客户端上传盘点信息\r\n" + ex);
            log.Error(sbSql.ToString());
            strReturn = "-1";
        }
        context.Response.Write(strReturn);
    }

    ///// <summary>
    ///// 上传盘点数据【旧版不带盘点计划】
    ///// </summary>
    ///// <param name="context"></param>
    //public void UploadTakedAssetsInfo(HttpContext context, String strRequestLanguage,Object objTakedAssets)
    //{
    //    StringBuilder sbSql = new StringBuilder();
    //    String strReturn = "0";
    //    try
    //    {
    //        String strJson = objTakedAssets.ToString();
    //        Newtonsoft.Json.Linq.JArray jsonArray = (Newtonsoft.Json.Linq.JArray)JsonConvert.DeserializeObject(strJson);

    //        for (int i = 0; i < jsonArray.Count; i++)
    //        {
    //            //首先判断是资产还是OE的盘点数据
    //            String strSEQ = jsonArray[i]["SEQ"].ToString();
    //            String strObjectType = jsonArray[i]["OBJECTTYPE"].ToString();
    //            String strBarCode = jsonArray[i]["SBARCODE"].ToString();
    //            String strLocationCode = jsonArray[i]["SLCODE"].ToString();
    //            String strQty = jsonArray[i]["NQTY"].ToString();
    //            String strUserId = jsonArray[i]["USERID"].ToString();
    //            String strSystime = jsonArray[i]["SYSTIME"].ToString();

    //            sbSql.Append("exec [USP_AM_Moblie_SaveOneTaked] '"+strSEQ+"','"+strObjectType+"','"+strBarCode+"','"+strLocationCode+"','"+strQty+"','"+strUserId+"','"+strSystime+"';\r\n");
    //        }
    //        if (!String.IsNullOrEmpty(sbSql.ToString()))
    //        {
    //            log.Error(sbSql.ToString());
    //            SqlParamDao.ExecuteNonQueryBySql(sbSql.ToString());
    //            strReturn = (jsonArray.Count).ToString();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        log.Error("客户端上传盘点信息\r\n" + ex);
    //        log.Error(sbSql.ToString());
    //        strReturn = "-1";
    //    }
    //    context.Response.Write(strReturn);
    //}


    /// <summary>
    /// 判断并获取客户端app的新版本
    /// </summary>
    /// <param name="context"></param>
    public void UpdateVersion(HttpContext context, String strAppOldVersion)
    {
        try
        {
            String json = "";
            //从服务器本地文件获取新版本文件
            String strPath = "~/SysFile/Download/App/AM";
            String strServerPath = context.Server.MapPath(strPath);

            //是否存在目录 
            if (!System.IO.Directory.Exists(strServerPath))
            {
                //不存在创建文件夹  
                System.IO.Directory.CreateDirectory(strServerPath);
            }
            else
            {
                String strEncodePath = HttpUtility.UrlEncode(strServerPath);
                System.IO.DirectoryInfo thisOne = new System.IO.DirectoryInfo(strServerPath);
                System.IO.FileInfo[] fileInfo = thisOne.GetFiles();

                String strNewVersionFileName = "";//最后版本文件名
                DateTime dtNewVersionTime = DateTime.Parse("2000-01-01 00:00:00");//最后版本文件时间
                String strNewVersionCode = strAppOldVersion;//最后版本号
                // 遍历所有的文件和目录
                foreach (System.IO.FileInfo file in fileInfo)
                {
                    String strFileName = file.Name;
                    DateTime dtFileTime = file.CreationTime;
                    String strExtension = file.Extension;

                    if (DateTime.Compare(dtFileTime, dtNewVersionTime) > 0)
                    {
                        dtNewVersionTime = dtFileTime;
                        strNewVersionFileName = strFileName;
                        String[] strArrary = strFileName.Replace(strExtension, "").Split('_');
                        if (strArrary.Length >= 2)
                        {
                            strNewVersionCode = strArrary[strArrary.Length-1];
                        }
                    }
                }

                //如果新旧版本号不一致则提示更新
                if (!strAppOldVersion.Equals(strNewVersionCode))
                {
                    //且版本号大于当前版本号
                    if (float.Parse(strNewVersionCode.Replace(".", "").Replace("V", "")) > float.Parse(strAppOldVersion.Replace(".", "").Replace("V", "")))
                    {
                        json = "{ResultData:[{versionCode:'" + strNewVersionCode + "',fileName:'" + strNewVersionFileName + "'}]}";
                    }
                    //json = strNewVersionFileName;
                }
            }

            context.Response.Write(json);
        }
        catch (Exception ex)
        {
            log.Error(ex);
            context.Response.Write("");
        }
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}