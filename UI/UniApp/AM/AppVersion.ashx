<%@ WebHandler Language="C#" Class="AppVersion" %>

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

public class AppVersion : HandlerBase,IHttpHandler, System.Web.SessionState.IRequiresSessionState
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
        string strLocationCode = WebCommon.GetJsonValue(strParamJson,"locationcode").ToString();//地址编码
        string strParamName = WebCommon.GetJsonValue(strParamJson,"paramname").ToString();//基本参数名称

        object objTakedData = context.Request["datataked"] == null ? string.Empty : context.Request["datataked"];//上传数据对象

        String strRequestLanguage = strCulture.Equals("0") ? "zh-cn" : "en-us";
        switch (param.ToLower().ToString())
        {
            case "getappversion":// 获取AppVersion版本
                this.GeAppVersion(context,strRequestLanguage);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 获取AppVersion版本
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strParamName"></param>
    public void GeAppVersion(HttpContext context,String strParamName)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        String strMethodDesc = "获取AppVersion版本";
        StringBuilder sbResult = new StringBuilder();

        try
        {
            FileStream fs = null;
            string strFileName = "";
            byte[] btReturn = new byte[0];
            string strFilePath = "~/UserFile/ReleaseApk/AM";
            string CurrentUploadFolderPath = HttpContext.Current.Server.MapPath(strFilePath);

            if (Directory.Exists(CurrentUploadFolderPath))
            {
                String strEncodePath = HttpUtility.UrlEncode(CurrentUploadFolderPath);
                DirectoryInfo thisOne = new DirectoryInfo(CurrentUploadFolderPath);
                FileInfo[] fileInfo = thisOne.GetFiles();
                StringBuilder sb = new StringBuilder("");
                int i = 0;

                // 遍历所有的文件和目录
                foreach (FileInfo file in fileInfo)
                {
                    //只取第一个文件，只支持一个文件
                    if (i > 0)
                    {
                        break;
                    }
                    strFileName = file.Name;
                    i++;
                }
                sbResultData.Append("\"ResultData\":{");
                sbResultData.Append("\"fileName\":\""+strFileName+"\"");
                sbResultData.Append(",\"fileString\":\""+ Convert.ToBase64String(btReturn) +"\"");
                sbResultData.Append(",\"fileUrl\":\""+Microsoft.JScript.GlobalObject.encodeURIComponent(strFilePath.Replace("~","")+"/"+ strFileName)+"\"");
                sbResultData.Append("}");
                //sbResultData.Append("\"ResultData\":\"\"");
                strReturnCode = "1";
                strReturnMsg = strMethodDesc+"成功";
            }else
            {
                sbResultData.Append("\"ResultData\":{\"fileName\":\"\",\"fileString\":\"\",\"fileUrl\":\"\"}");
                strReturnCode = "-1";
                strReturnMsg = strMethodDesc+"失败";
            }
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
                sbResult.Append(","+sbResultData.ToString()+"");
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