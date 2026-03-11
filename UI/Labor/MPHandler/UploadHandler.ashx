<%@ WebHandler Language="C#" Class="UploadHandler" %>

using System;
using System.Web;
using System.Collections;
using System.Text;
using System.IO;
using System.Data;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Com.ValuePlus.BLL.SysManager;
using Com.ValuePlus.BLL.User;
using Com.ValuePlus.DataLog;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Labor;
using Com.ValuePlus.Web;
using Com.ValuePlus.Common.Security;

public class UploadHandler : HandlerBase,IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    public void ProcessRequest (HttpContext context) {
        //uni.uploadfile组件上传，字符串参数通过get方式传值，则通过此种方式获取
        Hashtable hsTableUrlQuery = this.GetUrlAnalyse(context);
        string param = context.Request["param"] == null ? string.Empty : context.Request["param"].ToString();//请求类型参数
        string strObjectType = context.Request["objecttype"] == null ? string.Empty : context.Request["objecttype"].ToString();//用户类型
        string strUserType = context.Request["usertype"] == null ? string.Empty : context.Request["usertype"].ToString();//用户类型
        string strUserCode = context.Request["usercode"] == null ? string.Empty : context.Request["usercode"].ToString();//登录名
        string strCompanyCode = context.Request["companycode"] == null ? string.Empty : context.Request["companycode"].ToString();//公司编码
        
        //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
        param = SQLInjectionDefense.ReplaceSQLReservedKeyword(param);
        strObjectType = SQLInjectionDefense.ReplaceSQLReservedKeyword(strObjectType);
        strUserType = SQLInjectionDefense.ReplaceSQLReservedKeyword(strUserType);
        strUserCode = SQLInjectionDefense.ReplaceSQLReservedKeyword(strUserCode);
        strCompanyCode = SQLInjectionDefense.ReplaceSQLReservedKeyword(strCompanyCode);

        //上传的文件信息通过此种方式获取
        //解析客户端传递过来的json data
        //log.Error("传入参数的Json data字符串:" + context.Request["file"].ToString());
        StreamReader reader = new StreamReader(context.Request.InputStream);
        String strParamJson = HttpUtility.UrlDecode(reader.ReadToEnd());
        //log.Error("传入参数的Json data字符串:" + strParamJson.ToString());

        //string param = WebCommon.GetJsonValue(strParamJson,"param").ToString();//请求类型参数
        //string strUserType = WebCommon.GetJsonValue(strParamJson,"usertype").ToString();//用户类型
        //string strUserCode = WebCommon.GetJsonValue(strParamJson,"usercode").ToString();//登录名
        //string strCompanyCode = WebCommon.GetJsonValue(strParamJson,"companycode").ToString();//公司编码
        //string strPostDataObject = WebCommon.GetJsonObjectValue(strParamJson,"postdataobj").ToString();

        StringBuilder sbImportParam = new StringBuilder();
        sbImportParam.Append("param:" + param);
        sbImportParam.Append(",strObjectType:" + strObjectType);
        sbImportParam.Append(",strUserCode:" + strUserCode);
        sbImportParam.Append(",strUserType:" + strUserType);
        sbImportParam.Append(",strCompanyCode:" + strCompanyCode);
        //sbImportParam.Append(",strPostDataObject:" + strPostDataObject);
        log.Error("Labor/MPHandler/UploadHandler.ashx传入参数:" + sbImportParam.ToString());

        switch (param.ToLower().ToString())
        {
            case "uploadlogo":
                this.UploadLogo(context,strObjectType,strUserCode, strCompanyCode);
                break;
        }
    }


    /// <summary>
    /// 上传Logo
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strObjectType">logo头像对象类型【1为用户、2位公司】</param>
    /// <param name="strUserCode"></param>
    /// <param name="strCompanyCode"></param>
    public void UploadLogo(HttpContext context,String strObjectType,String strUserCode,String strCompanyCode)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();

        StringBuilder sbResult = new StringBuilder();
        try
        {
            if (String.IsNullOrEmpty(strObjectType) && String.IsNullOrEmpty(strUserCode) && String.IsNullOrEmpty(strCompanyCode))
            {
                strReturnCode = "-1";
                strReturnMsg = "上传是参数有误";
            }else
            {
                HttpPostedFile postFile = HttpContext.Current.Request.Files[0];
                //服务器硬盘物理目录
                String strSitePath = "Labor/Resource/Logo/";
                String strServerFilePath = context.Server.MapPath("../Resource/Logo/");

                //文件夹不存在则先创建
                if (!Directory.Exists(strServerFilePath))
                {
                    Directory.CreateDirectory(strServerFilePath);
                }

                //文件后缀名
                var strFileExt = Path.GetExtension(postFile.FileName);
                //根据规则重命名文件名
                var strFileName = DateTime.Now.ToString("yyyyMMddHHmmss");
                if (strObjectType.Equals("1"))
                {
                    strFileName = strUserCode + DateTime.Now.ToString("yyyyMMddHHmmss");
                }else if (strObjectType.Equals("2"))
                {
                    strFileName = strCompanyCode + DateTime.Now.ToString("yyyyMMddHHmmss");
                }
                //var strFileName = strUserCode+"-"+DateTime.Now.ToString("yyyyMMdd");
                //硬盘物理路径
                var strFilePathAndName = strServerFilePath + strFileName + strFileExt;
                //网站相对路径路径
                var strSitePathAndFileName = strSitePath + strFileName + strFileExt;

                //如果存在文件则删除覆盖
                if (File.Exists(strFilePathAndName))
                {
                    File.Delete(strFilePathAndName);
                }
                //保存文件
                postFile.SaveAs(strFilePathAndName);

                //同时更新数据库的Logo字段
                int iCount = 0;
                if (strObjectType.Equals("1"))
                {
                    iCount = LUser.UpdateUserLogoImage(strUserCode,strSitePathAndFileName);
                }else if (strObjectType.Equals("2"))
                {
                    iCount = LCompany.UpdateCompanyLogoImage(strCompanyCode,strSitePathAndFileName);
                }
                if (iCount == 1)
                {
                    strReturnCode = "1";
                    strReturnMsg = "上传Logo成功";
                    //sbResultData.Append(WebCommon.GetJsonStringByDataTable(dt,"\"ResultData\"",false));
                    sbResultData.Append("\"ResultData\":{");
                    sbResultData.Append("\"LogoPath\":\""+strSitePathAndFileName+"\"");
                    sbResultData.Append("}");
                }else
                {
                    strReturnCode = "-2";
                    strReturnMsg = "上传Logo失败，请重试，上傳文件："+strSitePathAndFileName;
                }


            }
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = "上传Logo出错,请稍候重试";
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
                sbResult.Append(","+sbResultData.ToString());
            }
            sbResult.Append("}");
        }
        log.Error("上传Logo时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}