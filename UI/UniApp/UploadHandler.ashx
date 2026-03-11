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
using Microsoft.JScript;
using Com.ValuePlus.Common.Security;

public class UploadHandler : HandlerBase,IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    public void ProcessRequest (HttpContext context) {
        //上传的文件信息通过此种方式获取
        //解析客户端传递过来的json data
        //log.Error("传入参数的Json data字符串:" + context.Request["file"].ToString());
        StreamReader reader = new StreamReader(context.Request.InputStream);
        String strParamJson = HttpUtility.UrlDecode(reader.ReadToEnd());
        //log.Error("传入参数的Json data字符串:" + strParamJson.ToString());

        string param = context.Request["param"] == null ? string.Empty : context.Request["param"].ToString();//请求类型参数
        //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
        param = SQLInjectionDefense.ReplaceSQLReservedKeyword(param);
        if(String.IsNullOrEmpty(param)){
            param = WebCommon.GetJsonValue(strParamJson, "param").ToString();//请求类型参数
        }
        string strObjectType = WebCommon.GetJsonValue(strParamJson, "objecttype").ToString();//对象类型
        string strTID = WebCommon.GetJsonValue(strParamJson, "tid").ToString();//TID
        string strSID = WebCommon.GetJsonValue(strParamJson, "sid").ToString();//SID
        string strGID = WebCommon.GetJsonValue(strParamJson, "gid").ToString();//GID
        string strPID = WebCommon.GetJsonValue(strParamJson, "pid").ToString();//PID
        string strPCTRLD = WebCommon.GetJsonValue(strParamJson, "pctrld").ToString();//pctrld
        string strKEY = WebCommon.GetJsonValue(strParamJson, "key").ToString();//key
        string strKEYVALUE = WebCommon.GetJsonValue(strParamJson, "keyvalue").ToString();//keyvalue
        string strGRIDKEY = WebCommon.GetJsonValue(strParamJson, "gridkey").ToString();//gridkey
        string strGRIDKEYVALUE = WebCommon.GetJsonValue(strParamJson, "gridkeyvalue").ToString();//gridkeyvalue
        string strFileName = WebCommon.GetJsonValue(strParamJson, "filename").ToString();//filename
        string strUserCode = WebCommon.GetJsonValue(strParamJson, "usercode").ToString();//登录名


        StringBuilder sbImportParam = new StringBuilder();
        sbImportParam.Append("param:" + param);
        sbImportParam.Append(",strObjectType:" + strObjectType);
        sbImportParam.Append(",strUserCode:" + strUserCode);
        //sbImportParam.Append(",strPostDataObject:" + strPostDataObject);
        //log.Error("UniApp/UploadHandler.ashx传入参数:" + sbImportParam.ToString());

        switch (param.ToLower().ToString())
        {
            case "uploadfile":
                this.UploadFile(context);
                break;
            case "deletefile":
                this.DeleteFile(context,strObjectType,strTID,strSID,strGID,strPID,strPCTRLD,strKEY,strKEYVALUE,strGRIDKEY,strGRIDKEYVALUE,strFileName,strUserCode);
                break;
        }
    }

    /// <summary>
    /// 上传文件
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strObjectType"></param>
    /// <param name="strTID"></param>
    /// <param name="strSID"></param>
    /// <param name="strGID"></param>
    /// <param name="strPID"></param>
    /// <param name="strPCTRLD"></param>
    /// <param name="strKEY"></param>
    /// <param name="strKEYVALUE"></param>
    /// <param name="strGRIDKEY"></param>
    /// <param name="strGRIDKEYVALUE"></param>
    /// <param name="strUserCode"></param>
    public void UploadFile(HttpContext context)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();

        StringBuilder sbResult = new StringBuilder();
        try
        {
            //uni.uploadfile组件上传，字符串参数通过get方式传值，则通过此种方式获取
            // 上传附件对象类型[archive模板，]可扩展
            string strObjectType = context.Request["objecttype"] == null ? string.Empty : GlobalObject.unescape(context.Request["objecttype"].ToString());
            //TID
            string strTID = context.Request["tid"] == null ? string.Empty : GlobalObject.unescape(context.Request["tid"].ToString());
            string strSID = context.Request["sid"] == null ? string.Empty : GlobalObject.unescape(context.Request["sid"].ToString());
            string strGID = context.Request["gid"] == null ? string.Empty : GlobalObject.unescape(context.Request["gid"].ToString());
            string strPID = context.Request["pid"] == null ? string.Empty : GlobalObject.unescape(context.Request["pid"].ToString());
            string strPCTRLD = context.Request["pctrld"] == null ? string.Empty : GlobalObject.unescape(context.Request["pctrld"].ToString());
            string strKEY = context.Request["key"] == null ? string.Empty : GlobalObject.unescape(context.Request["key"].ToString());
            string strKEYVALUE = context.Request["keyvalue"] == null ? string.Empty : GlobalObject.unescape(context.Request["keyvalue"].ToString());
            string strGRIDKEY = context.Request["gridkey"] == null ? string.Empty : GlobalObject.unescape(context.Request["gridkey"].ToString());
            string strGRIDKEYVALUE = context.Request["gridkeyvalue"] == null ? string.Empty : GlobalObject.unescape(context.Request["gridkeyvalue"].ToString());
            //string strFileName = context.Request["filename"] == null ? string.Empty : GlobalObject.unescape(context.Request["filename"].ToString());
            //当前用户编码
            string strUserCode = context.Request["usercode"] == null ? string.Empty : GlobalObject.unescape(context.Request["usercode"].ToString());
            string strTerminalEntry = context.Request["terminalentry"] == null ? string.Empty : GlobalObject.unescape(context.Request["terminalentry"].ToString());
                        
            //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
            strObjectType = SQLInjectionDefense.ReplaceSQLReservedKeyword(strObjectType);
            strTID = SQLInjectionDefense.ReplaceSQLReservedKeyword(strTID);
            strSID = SQLInjectionDefense.ReplaceSQLReservedKeyword(strSID);
            strGID = SQLInjectionDefense.ReplaceSQLReservedKeyword(strGID);
            strPID = SQLInjectionDefense.ReplaceSQLReservedKeyword(strPID);
            strPCTRLD = SQLInjectionDefense.ReplaceSQLReservedKeyword(strPCTRLD);
            strKEY = SQLInjectionDefense.ReplaceSQLReservedKeyword(strKEY);
            strKEYVALUE = SQLInjectionDefense.ReplaceSQLReservedKeyword(strKEYVALUE);
            strGRIDKEY = SQLInjectionDefense.ReplaceSQLReservedKeyword(strGRIDKEY);
            strGRIDKEYVALUE = SQLInjectionDefense.ReplaceSQLReservedKeyword(strGRIDKEYVALUE);
            strUserCode = SQLInjectionDefense.ReplaceSQLReservedKeyword(strUserCode);
            strTerminalEntry = SQLInjectionDefense.ReplaceSQLReservedKeyword(strTerminalEntry);

            //log.Error("移动端上传文件时的终端入口类型：" + strTerminalEntry.ToString());

            if (String.IsNullOrEmpty(strObjectType) && String.IsNullOrEmpty(strUserCode))
            {
                strReturnCode = "-1";
                strReturnMsg = "上传是参数有误";
            }else
            {
                HttpPostedFile postFile = HttpContext.Current.Request.Files[0];
                //文件名
                String strFileName = postFile.FileName;

                if(strTerminalEntry.ToUpper().Equals("MP")){
                    //小程序上传时，文件名被特意处理成了较长的随机字符串，太不友好，此处我们自定义处理

                    //文件后缀名
                    string strFileExt = Path.GetExtension(strFileName);
                    //根据规则重命名文件名
                    String strDefineFileName = DateTime.Now.ToString("yyyyMMddHHmmss");
                    if(strObjectType.ToLower().Equals("archive")){
                        strDefineFileName = strUserCode+strDefineFileName;
                    }
                    strFileName = strDefineFileName + strFileExt;
                }

                //文件名特殊字符的处理
                strFileName = strFileName.Replace(";", "；");
                strFileName = strFileName.Replace("'", "’");
                strFileName = strFileName.Replace("+", "#");
                strFileName = strFileName.Replace("*", "X");

                //服务器硬盘物理目录
                String strSitePath = "UserFile/MobileUpload/";
                String strServerFilePath = context.Server.MapPath("../"+strSitePath);

                if(strObjectType.ToLower().Equals("archive")){
                    //如果是模板类型的附件上传
                    strSitePath = "UserFile/ArchiveAtt/" + strPCTRLD + "/" + strTID + "-" + strGID + "/";
                    if(!String.IsNullOrEmpty(strGRIDKEYVALUE)){
                        strSitePath = strSitePath + strGRIDKEYVALUE;
                    }else{
                        strSitePath = strSitePath + strKEYVALUE;
                    }
                    strSitePath = strSitePath + "/";
                    strServerFilePath = context.Server.MapPath("../"+strSitePath);
                }else if(strObjectType.ToLower().Equals("mylogo")){
                    //我的头像的上传
                    String strSql_Config = "select * from MBConfig_1 WHERE paramName = 'Path_MyLogoImage'";
                    DataTable dt = SqlParamDao.GetDataTableBySql(strSql_Config);
                    if(dt!=null&&dt.Rows.Count==0){
                        strSitePath = dt.Rows[0]["paramValue"].ToString();
                    }else{
                        strSitePath = "UserFile/MobileUpload/MyLogo";
                    }
                    strSitePath = strSitePath + "/";
                    strFileName = strUserCode + "-" + strFileName;
                    strServerFilePath = context.Server.MapPath("../"+strSitePath);
                }
                //log.Error("移动端上传文件时的对象类型：" + strObjectType.ToString());
                //log.Error("移动端上传文件时的对象TID：" + strTID.ToString());
                //log.Error("移动端上传文件时文件名：" + strFileName.ToString());
                //log.Error("移动端上传文件时文件路径：" + strServerFilePath.ToString());

                //文件夹不存在则先创建
                if (!Directory.Exists(strServerFilePath))
                {
                    Directory.CreateDirectory(strServerFilePath);
                }

                //硬盘物理路径
                String strFilePathAndName = strServerFilePath + strFileName;
                //网站相对路径路径
                String strSitePathAndFileName = strSitePath + strFileName;

                //如果存在文件则删除覆盖
                if (File.Exists(strFilePathAndName))
                {
                    File.Delete(strFilePathAndName);
                }
                //保存文件
                postFile.SaveAs(strFilePathAndName);

                //同时更新数据库
                StringBuilder sbSql = new StringBuilder();
                int iCount = 0;
                if(strObjectType.ToLower().Equals("archive"))
                {
                    //如果是模板类型的附件上传
                    String strTableName = strTID + "_" + strGID;
                    sbSql.Append("UPDATE "+strTableName+" SET ATTFILE = (case when CHARINDEX('"+strFileName+"',isnull(ATTFILE,'')) >0 then  ATTFILE else \r\n");
                    sbSql.Append(" (CASE when ISNULL(ATTFILE,'') = '' THEN '' ELSE ATTFILE+';' END)+'"+strFileName+"' end)  \r\n");
                    sbSql.Append(" WHERE "+strKEY+" = '"+strKEYVALUE+"'  \r\n");
                    if(!String.IsNullOrEmpty(strGRIDKEY)&& !String.IsNullOrEmpty(strGRIDKEYVALUE)){
                        sbSql.Append(" WHERE "+strGRIDKEY+" = '"+strGRIDKEYVALUE+"'  \r\n");
                    }
                    iCount = SqlParamDao.ExecuteNonQueryBySql(sbSql.ToString());
                }else if(strObjectType.ToLower().Equals("mylogo")){
                    //我的头像的上传
                    sbSql.Append("UPDATE FLUser_1 SET LogoImage = '"+strFileName+"'\r\n");
                    sbSql.Append(" WHERE SUSERID = '"+strUserCode+"'  \r\n");

                    iCount = SqlParamDao.ExecuteNonQueryBySql(sbSql.ToString());
                }

                //log.Error("移动端上传文件时存储sql：" + sbSql.ToString());
                if (iCount == 1)
                {
                    strReturnCode = "1";
                    strReturnMsg = "上传文件成功";
                    sbResultData.Append("\"ResultData\":{");
                    sbResultData.Append("\"fileName\":\""+ Microsoft.JScript.GlobalObject.escape(strFileName)+"\"");
                    sbResultData.Append(",\"filePath\":\""+Microsoft.JScript.GlobalObject.escape(strSitePathAndFileName)+"\"");
                    sbResultData.Append("}");
                }else
                {
                    strReturnCode = "-2";
                    strReturnMsg = "上传文件失败，请重试，文件文件："+strSitePathAndFileName;
                }
            }
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = "上传文件出错,请稍候重试";
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
        //log.Error("移动端上传文件时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }

    /// <summary>
    /// 删除文件
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strObjectType"></param>
    /// <param name="strTID"></param>
    /// <param name="strSID"></param>
    /// <param name="strGID"></param>
    /// <param name="strPID"></param>
    /// <param name="strPCTRLD"></param>
    /// <param name="strKEY"></param>
    /// <param name="strKEYVALUE"></param>
    /// <param name="strGRIDKEY"></param>
    /// <param name="strGRIDKEYVALUE"></param>
    /// <param name="strUserCode"></param>
    public void DeleteFile(HttpContext context,String strObjectType,String strTID,String strSID,String strGID,String strPID,String strPCTRLD
        ,String strKEY,String strKEYVALUE,String strGRIDKEY,String strGRIDKEYVALUE,String strFileName,String strUserCode)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();

        StringBuilder sbResult = new StringBuilder();
        try
        {
            if (String.IsNullOrEmpty(strObjectType) && String.IsNullOrEmpty(strUserCode))
            {
                strReturnCode = "-1";
                strReturnMsg = "上传是参数有误";
            }else
            {
                //服务器硬盘物理目录
                String strSitePath = "UserFile/MobileUpload/";
                String strServerFilePath = context.Server.MapPath("../"+strSitePath);

                if(strObjectType.ToLower().Equals("archive")){
                    //如果是模板类型的附件上传
                    strSitePath = "UserFile/ArchiveAtt/" + strPCTRLD + "/" + strTID + "-" + strGID + "/";
                    if(!String.IsNullOrEmpty(strGRIDKEYVALUE)){
                        strSitePath = strSitePath + strGRIDKEYVALUE;
                    }else{
                        strSitePath = strSitePath + strKEYVALUE;
                    }
                    strSitePath = strSitePath + "/";
                    strServerFilePath = context.Server.MapPath("../"+strSitePath);
                }else if(strObjectType.ToLower().Equals("mylogo")){
                    //我的头像的上传
                    String strSql_Config = "select * from MBConfig_1 WHERE paramName = 'Path_MyLogoImage'";
                    DataTable dt = SqlParamDao.GetDataTableBySql(strSql_Config);
                    if(dt!=null&&dt.Rows.Count==0){
                        strSitePath = dt.Rows[0]["paramValue"].ToString();
                    }else{
                        strSitePath = "UserFile/MobileUpload/MyLogo";
                    }
                    strSitePath = strSitePath + "/";
                    strServerFilePath = context.Server.MapPath("../"+strSitePath);
                }

                //文件夹不存在则先创建
                if (Directory.Exists(strServerFilePath))
                {
                    //硬盘物理路径
                    String strFilePathAndName = strServerFilePath + strFileName;
                    //网站相对路径路径
                    String strSitePathAndFileName = strSitePath + strFileName;

                    //如果存在文件则删除
                    if (File.Exists(strFilePathAndName))
                    {
                        File.Delete(strFilePathAndName);
                    }
                    //同时更新数据库
                    StringBuilder sbSql = new StringBuilder();
                    int iCount = 0;
                    if(strObjectType.ToLower().Equals("archive"))
                    {
                        //如果是模板类型的附件上传
                        String strTableName = strTID + "_" + strGID;
                        sbSql.Append("UPDATE "+strTableName+" SET ATTFILE = (case when CHARINDEX('"+strFileName+";',isnull(ATTFILE,'')) >0 then REPLACE(ATTFILE,'"+strFileName+";','') else \r\n");
                        sbSql.Append(" (case when CHARINDEX(';"+strFileName+"',isnull(ATTFILE,'')) >0 then REPLACE(ATTFILE,';"+strFileName+"','') else REPLACE(ATTFILE,'"+strFileName+"','') end)  end) \r\n");
                        sbSql.Append(" WHERE "+strKEY+" = '"+strKEYVALUE+"'  \r\n");
                        if(!String.IsNullOrEmpty(strGRIDKEY)&& !String.IsNullOrEmpty(strGRIDKEYVALUE)){
                            sbSql.Append(" WHERE "+strGRIDKEY+" = '"+strGRIDKEYVALUE+"'  \r\n");
                        }
                        iCount = SqlParamDao.ExecuteNonQueryBySql(sbSql.ToString());
                    }else if(strObjectType.ToLower().Equals("mylogo")){
                        //删除我的头像
                        sbSql.Append("UPDATE FLUser_1 SET LogoImage = null WHERE SUSERID = '"+strUserCode+"'  \r\n");
                        iCount = SqlParamDao.ExecuteNonQueryBySql(sbSql.ToString());
                    }

                    if (iCount == 1)
                    {
                        strReturnCode = "1";
                        strReturnMsg = "删除文件成功";
                        sbResultData.Append("\"ResultData\":{");
                        sbResultData.Append("\"fileName\":\""+ Microsoft.JScript.GlobalObject.escape(strFileName)+"\"");
                        sbResultData.Append(",\"filePath\":\""+Microsoft.JScript.GlobalObject.escape(strSitePathAndFileName)+"\"");
                        sbResultData.Append("}");
                    }else
                    {
                        strReturnCode = "-2";
                        strReturnMsg = "删除文件失败，请重试，文件："+strSitePathAndFileName;
                    }
                }else{
                    strReturnCode = "-3";
                    strReturnMsg = "删除文件失败，文件路径不存在，请重试";
                }
            }
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = "上传文件出错,请稍候重试";
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
        //log.Error("移动端删除附件时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }


    /// <summary>
    /// 通过上传对象类型获取上传路径
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strObjectType"></param>
    /// <param name="strTID"></param>
    /// <param name="strSID"></param>
    /// <param name="strGID"></param>
    /// <param name="strPID"></param>
    /// <param name="strPCTRLD"></param>
    /// <param name="strKEY"></param>
    /// <param name="strKEYVALUE"></param>
    /// <param name="strGRIDKEY"></param>
    /// <param name="strGRIDKEYVALUE"></param>
    /// <param name="strUserCode"></param>
    public string GetFilePathByObjectType(HttpContext context, String strObjectType, String strTID, String strSID, String strGID, String strPID, String strPCTRLD
        , String strKEY, String strKEYVALUE, String strGRIDKEY, String strGRIDKEYVALUE, String strUserCode,ref String strFileName,ref String strServerFilePath)
    {
        //服务器硬盘物理目录
        String strSitePath = "UserFile/MobileUpload/";
        strServerFilePath = context.Server.MapPath("../" + strSitePath);
        try
        {

            if (strObjectType.ToLower().Equals("archive"))
            {
                //如果是模板类型的附件上传
                strSitePath = "UserFile/ArchiveAtt/" + strPCTRLD + "/" + strTID + "-" + strGID + "/";
                if (!String.IsNullOrEmpty(strGRIDKEYVALUE))
                {
                    strSitePath = strSitePath + strGRIDKEYVALUE;
                }
                else
                {
                    strSitePath = strSitePath + strKEYVALUE;
                }
                strSitePath = strSitePath + "/";
                strServerFilePath = context.Server.MapPath("../" + strSitePath);
            }
            else if (strObjectType.ToLower().Equals("mylogo"))
            {
                //我的头像的上传
                String strSql_Config = "select * from MBConfig_1 WHERE paramName = 'Path_MyLogoImage'";
                DataTable dt = SqlParamDao.GetDataTableBySql(strSql_Config);
                if (dt != null && dt.Rows.Count == 0)
                {
                    strSitePath = dt.Rows[0]["paramValue"].ToString();
                }
                else
                {
                    strSitePath = "UserFile/MobileUpload/MyLogo";
                }
                strSitePath = strSitePath + "/";
                strFileName = strUserCode + "-" + strFileName;
                strServerFilePath = context.Server.MapPath("../" + strSitePath);
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
        return strSitePath;
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}