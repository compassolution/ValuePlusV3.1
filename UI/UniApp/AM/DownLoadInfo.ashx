<%@ WebHandler Language="C#" Class="DownLoadInfo" %>

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

public class DownLoadInfo : HandlerBase,IHttpHandler, System.Web.SessionState.IRequiresSessionState
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
        string objTakedDataArray = WebCommon.GetJsonObjectValue(strParamJson,"datatakedarray").ToString();//上传数据对象数据

        String strRequestLanguage = strCulture.Equals("0") ? "zh-cn" : "en-us";
        switch (param.ToLower().ToString())
        {
            case "getallneeddownloadinfo"://获取需下载到终端的所有数据信息
                this.GetAllNeedDownloadInfo(context,strIsIncludeOE,strRequestLanguage);
                break;
            case "uploadtakedassetsinfo"://上传盘点数据【直接带上盘点计划写入】
                this.UploadTakedAssetsInfo(context,objTakedDataArray);
                break;
            case "deletetakedassetsinfo"://删除盘点数据【直接带上盘点计划】
                this.DeleteTakedAssetsInfo(context,objTakedDataArray);
                break;
            case "uploadonephotoimagedata"://上传资产拍摄图片盘点数据
                this.UploadOnePhotoImageData(context,objTakedDataArray);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 获取需下载到终端的所有数据信息
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strIsIncludeOE"></param>
    /// <param name="strRequestLanguage"></param>
    public void GetAllNeedDownloadInfo(HttpContext context,String strIsIncludeOE,String strRequestLanguage)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        String strMethodDesc = "获取需下载到终端的所有数据信息";
        StringBuilder sbResult = new StringBuilder();

        try
        {
            DataTable dt = new DataTable();
            String strSql = "";
            //基本参数数据
            strSql = "select * from [VW_Moblie_BasicParam]";
            sbResultData.Append(WebCommon.GetJsonStringByDataTable(SqlParamDao.GetDataTableBySql(strSql), "\"BasicParamData\"", true) );

            //系统用户数据
            strSql = "select * from [VW_Moblie_User] order by SUSERID";
            sbResultData.Append(","+WebCommon.GetJsonStringByDataTable(SqlParamDao.GetDataTableBySql(strSql), "\"UserData\"", true));

            //系统部门组织架构数据
            strSql = "select * from [VW_Moblie_Dept] order BY OID";
            sbResultData.Append("," + WebCommon.GetJsonStringByDataTable(SqlParamDao.GetDataTableBySql(strSql), "\"DeptData\"", true));

            //系统存放地址信息数据
            strSql = "select * from [VW_Moblie_Location] order by SLCODE";
            sbResultData.Append("," + WebCommon.GetJsonStringByDataTable(SqlParamDao.GetDataTableBySql(strSql), "\"LocationData\"", true));

            //固定资产信息数据
            strSql = "select * from [VW_Moblie_AssetDetail_ForDownload] with(nolock) order by SACODE";
            sbResultData.Append("," + WebCommon.GetJsonStringByDataTable(SqlParamDao.GetDataTableBySql(strSql), "\"AssetsInfoData\"", true));

            //固定资产盘点计划信息数据
            strSql = "select * from [VW_Moblie_AMPlan]";
            sbResultData.Append("," + WebCommon.GetJsonStringByDataTable(SqlParamDao.GetDataTableBySql(strSql), "\"AMPlanData\"", true));

            //固定资产盘点计划的已盘到数据
            strSql = "select * from [VW_Moblie_AMTaked]";
            sbResultData.Append("," + WebCommon.GetJsonStringByDataTable(SqlParamDao.GetDataTableBySql(strSql), "\"AMTakedData\"", true));
            
            //固定资产图片数据
            strSql = "select IMAGENAME,SIMAGE from [VW_Mobile_AssetImageData] where OBJECTTYPE = 'AM' order by [IMAGENAME]";
            sbResultData.Append("," + WebCommon.GetJsonStringByDataTable(SqlParamDao.GetDataTableBySql(strSql), "\"AMImageData\"", true));

            if(strIsIncludeOE.Equals("1")){
                //OE资产物品信息数据
                strSql = "select * from [VW_Moblie_OE_Item] order by SACODE";
                sbResultData.Append("," + WebCommon.GetJsonStringByDataTable(SqlParamDao.GetDataTableBySql(strSql), "\"OEItemData\"", true));

                //OE资产地址分布信息数据
                strSql = "select * from [VW_Moblie_OE_Location] order by SACODE";
                sbResultData.Append("," + WebCommon.GetJsonStringByDataTable(SqlParamDao.GetDataTableBySql(strSql), "\"OELocationData\"", true));

                //OE资产盘点计划信息数据
                strSql = "select * from [VW_Moblie_OEPlan]";
                sbResultData.Append("," + WebCommon.GetJsonStringByDataTable(SqlParamDao.GetDataTableBySql(strSql), "\"OEPlanData\"", true));

                //OE资产盘点计划的已盘到数据
                strSql = "select * from [VW_Moblie_OETaked]";
                sbResultData.Append("," + WebCommon.GetJsonStringByDataTable(SqlParamDao.GetDataTableBySql(strSql), "\"OETakedData\"", true));
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
                sbResult.Append(",\"ResultData\":{"+sbResultData.ToString()+"}");
            }
            sbResult.Append("}");
        }
        //log.Error(strMethodDesc+"时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }

    /// <summary>
    /// 上传盘点数据【直接带上盘点计划写入】
    /// </summary>
    /// <param name="context"></param>
    /// <param name="objTakedAssets"></param>
    public void UploadTakedAssetsInfo(HttpContext context,Object arrayTakedData)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        String strMethodDesc = "上传盘点数据【直接带上盘点计划写入】";
        StringBuilder sbResult = new StringBuilder();

        try
        {
            StringBuilder sbSql = new StringBuilder();

            String strJson = arrayTakedData.ToString();
            Newtonsoft.Json.Linq.JArray jsonArray = (Newtonsoft.Json.Linq.JArray)JsonConvert.DeserializeObject(strJson);

            for (int i = 0; i < jsonArray.Count; i++)
            {
                //首先判断是资产还是OE的盘点数据
                String strSEQ = jsonArray[i]["seq"].ToString();
                String strPCODE = jsonArray[i]["planCode"].ToString();
                String strObjectType = jsonArray[i]["objectType"].ToString();
                String strBarCode = jsonArray[i]["barCode"].ToString();
                String strLocationCode = jsonArray[i]["locationCode"].ToString();
                String strQty = jsonArray[i]["takedQty"].ToString();
                String strUserId = jsonArray[i]["userId"].ToString();
                String strSystime = jsonArray[i]["sysTime"].ToString();

                sbSql.Append("exec [USP_AM_Moblie_SaveOneTaked] '"+strSEQ+"','"+strPCODE+"','"+strObjectType+"','"+strBarCode+"','"+strLocationCode+"','"+strQty+"','"+strUserId+"','"+strSystime+"';\r\n");
            }
            if (!String.IsNullOrEmpty(sbSql.ToString()))
            {
                log.Error(sbSql.ToString());
                SqlParamDao.ExecuteNonQueryBySql(sbSql.ToString());
                sbResultData.Append((jsonArray.Count).ToString());
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
                sbResult.Append(",\"ResultData\":"+sbResultData.ToString());
            }
            sbResult.Append("}");
        }
        //log.Error(strMethodDesc+"时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }

    /// <summary>
    /// 删除已盘到的数据【直接带上盘点计划】
    /// </summary>
    /// <param name="context"></param>
    /// <param name="objTakedAssets"></param>
    public void DeleteTakedAssetsInfo(HttpContext context,Object arrayTakedData)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        String strMethodDesc = "删除已盘到的数据【直接带上盘点计划】";
        StringBuilder sbResult = new StringBuilder();

        try
        {
            StringBuilder sbSql = new StringBuilder();

            String strJson = arrayTakedData.ToString();
            Newtonsoft.Json.Linq.JArray jsonArray = (Newtonsoft.Json.Linq.JArray)JsonConvert.DeserializeObject(strJson);

            int iDeleteCount = 0;
            for (int i = 0; i < jsonArray.Count; i++)
            {
                //首先判断是资产还是OE的盘点数据
                String strPCODE = jsonArray[i]["planCode"].ToString();
                String strObjectType = jsonArray[i]["objectType"].ToString();
                String strBarCode = jsonArray[i]["barCode"].ToString();
                String strLocationCode = jsonArray[i]["locationCode"].ToString();
                String strUserId = jsonArray[i]["userId"].ToString();
                String strSystime = jsonArray[i]["sysTime"].ToString();

                sbSql.Append("exec [USP_AM_Moblie_DeleteOneTaked] '"+strPCODE+"','"+strObjectType+"','"+strBarCode+"','"+strLocationCode+"','"+strUserId+"','"+strSystime+"';\r\n");
            }
            if (!String.IsNullOrEmpty(sbSql.ToString()))
            {
                log.Error(sbSql.ToString());
                iDeleteCount = SqlParamDao.ExecuteNonQueryBySql(sbSql.ToString());
                sbResultData.Append((iDeleteCount).ToString());
            }


            strReturnCode = iDeleteCount.ToString();
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
                sbResult.Append(",\"ResultData\":"+sbResultData.ToString());
            }
            sbResult.Append("}");
        }
        //log.Error(strMethodDesc+"时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }


    /// <summary>
    /// 上传一张资产拍摄图片盘点数据
    /// </summary>
    /// <param name="context"></param>
    /// <param name="objTakedAssets"></param>
    public void UploadOnePhotoImageData(HttpContext context,Object arrarPhotoImageData)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        String strMethodDesc = "上传一张资产拍摄图片盘点数据";
        StringBuilder sbResult = new StringBuilder();

        try
        {
            StringBuilder sbSql = new StringBuilder();

            String strJson = arrarPhotoImageData.ToString();
            Newtonsoft.Json.Linq.JArray jsonArray = (Newtonsoft.Json.Linq.JArray)JsonConvert.DeserializeObject(strJson);

            for (int i = 0; i < jsonArray.Count; i++)
            {
                String strSACODE = jsonArray[i]["assetsCode"].ToString();
                String strPHOTOIMAGE = Microsoft.JScript.GlobalObject.decodeURIComponent(jsonArray[i]["photoImage"].ToString());////base64字符串此处获取后解密
                String strObjectType = jsonArray[i]["objectType"].ToString();
                String strUserId = jsonArray[i]["userId"].ToString();
                String strSystime = jsonArray[i]["sysTime"].ToString();

                //根据SACODE获取对应的资产图片文件名称以及上传到服务器上的路径
                String strSaveFilePath = "";
                String strImageFileName = "";
                StringBuilder sbSqlImageName = new StringBuilder();
                sbSqlImageName.Append("SELECT dbo.[Fun_AM_GetAssetsImageName](SACODE,'"+strObjectType+"') AS FileName ");
                sbSqlImageName.Append(" ,(SELECT FPATH FROM UPDOWNCONFIGPARAM_1 WHERE FID = '"+(strObjectType.ToUpper().Equals("OE")?"OEIMAGE":"AIMAGE")+"' ) AS FilePath");
                sbSqlImageName.Append(" FROM "+(strObjectType.ToUpper().Equals("OE")?"OEITEM_1":"AMASSETS_1"));
                sbSqlImageName.Append(" WHERE SACODE = '"+strSACODE+"'");
                String strSqlImageName = sbSqlImageName.ToString();

                DataTable dtFileName = SqlParamDao.GetDataTableBySql(strSqlImageName);
                if(dtFileName!=null && dtFileName.Rows.Count>0){
                    strImageFileName = dtFileName.Rows[0]["FileName"].ToString();
                    strSaveFilePath = dtFileName.Rows[0]["FilePath"].ToString();

                    String strFilePathAndName = strSaveFilePath + "\\" + strImageFileName;
                    //base64转字节
                    byte[] bytesImage = Convert.FromBase64String(strPHOTOIMAGE);

                    //保存文件到服务器
                    using (FileStream fs = new FileStream(strFilePathAndName, FileMode.Create))
                    {
                        fs.Write(bytesImage, 0, bytesImage.Length);
                    }

                    //同时写入到服务器端图片数据库中
                    String strSqlInsertImageData = "EXEC [USP_AM_InsertAssetImage] '"+strSACODE+"'";
                    if(strObjectType.ToUpper().Equals("OE")){
                        strSqlInsertImageData = "EXEC [USP_OE_InsertOEImage] '"+strSACODE+"'";
                    }
                    int iCount = SqlParamDao.ExecuteNonQueryBySql(strSqlInsertImageData);

                    Console.WriteLine("图像已保存到"+strFilePathAndName);

                }

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
                sbResult.Append(",\"ResultData\":"+sbResultData.ToString());
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