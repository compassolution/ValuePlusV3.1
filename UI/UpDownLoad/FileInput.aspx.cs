using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Com.ValuePlus.Web;
using Com.ValuePlus.Common.Config;
using System.Resources;
using System.Reflection;
using CuteEditor;
using System.IO;
using System.Xml;
using System.Text;
using Com.ValuePlus.BLL.UpDownLoad;
using Com.ValuePlus.Entity.UpDownLoad;
using Com.ValuePlus.DAL;
using Com.ValuePlus.SysParams;
using Com.ValuePlus.BLL.User;
using Com.ValuePlus.Common;

public partial class UpDownLoad_FileInput : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            String strUrlQueryString = Server.UrlDecode(Request.Url.Query.ToString());
            Hashtable hsTableUrlQuery = WebCommon.GetUrlAnalyse(strUrlQueryString);
            string strParam = hsTableUrlQuery["param"] == null ? string.Empty : hsTableUrlQuery["param"].ToString();//param
            string strFolder = hsTableUrlQuery["folder"] == null ? string.Empty : hsTableUrlQuery["folder"].ToString();//folder
            string strFilePath = hsTableUrlQuery["filepath"] == null ? string.Empty : hsTableUrlQuery["filepath"].ToString();//filepath
            string strFileName = hsTableUrlQuery["filename"] == null ? string.Empty : hsTableUrlQuery["filename"].ToString();//filepath
            string strOpType = hsTableUrlQuery["optype"] == null ? string.Empty : hsTableUrlQuery["optype"].ToString();//filepath
            strFilePath = Microsoft.JScript.GlobalObject.unescape(strFilePath);

            this.strFileFolder = strFolder;

            //add by sammane by sammen 20230328
            //新增文件操作token,防止恶意操作，UpDownLoad/DownLoadFile.aspx.cs中进行解析
            this.strToken = this.strFileFolder + "＃" + DateTime.Now.ToString("yyyyMMddHHmmss");
            this.strToken = Microsoft.JScript.GlobalObject.encodeURIComponent(this.strToken);//加密
            //this.strToken = UrlParamEncryption.Encrypt3des(this.strToken, System.Text.Encoding.UTF8);//加密

            switch (strParam.ToLower().ToString())
            {
                case "getpagebasicdata":
                    Response.Write(this.GetPageBasicData(strFolder).ToString());
                    Response.End();
                    break;
                case "savefile":
                    Response.Write(this.SaveInputFile(strFolder,strFilePath).ToString());
                    Response.End();
                    break;
                case "getfilelist":
                    Response.Write(this.GetDownloadFileList(strFolder, strFilePath).ToString());
                    Response.End();
                    break;
                case "operateonefile":
                    Response.Write(this.OperateOneFile(strFolder,strFilePath, strFileName,strOpType).ToString());
                    Response.End();
                    break;
                case "initassetsimagedata":
                    Response.Write(this.InitAssetsImageData(strFolder).ToString());
                    Response.End();
                    break;
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
    }

    #region viewstate初始化区域
    public string strFileFolder
    {
        get
        {
            return ViewState["strFileFolder"] as string;
        }
        set
        {
            ViewState["strFileFolder"] = value;
        }
    }
    public string strToken
    {
        get
        {
            return ViewState["strToken"] as string;
        }
        set
        {
            ViewState["strToken"] = value;
        }
    }
    private UpDownLoadXmlEntity entityXml
    {
        get
        {
            if (this.ViewState["entityXml"] == null)
            {
                return new UpDownLoadXmlEntity();
            }
            return (UpDownLoadXmlEntity)this.ViewState["entityXml"];
        }
        set
        {
            this.ViewState["entityXml"] = value;
        }
    }
    #endregion

    /// <summary>
    /// 获取FileInput页面的基础数据信息
    /// </summary>
    /// <returns></returns>
    private String GetPageBasicData(String strFolder)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        StringBuilder sbReturnRowData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "获取FileInput页面的基础数据信息";

        try
        {
            StringBuilder sbLanguageTips = new StringBuilder();
            sbLanguageTips.Append("{");
            sbLanguageTips.Append("\"TitleTips\":\"" + (this.Language == "zh-cn" ? "文件上传及下载" : "File Upload & Download") + "\"");
            sbLanguageTips.Append(",\"SystemTips\":\"" + (this.Language == "zh-cn" ? "系统提示" : "System Tips") + "\"");
            sbLanguageTips.Append(",\"ConfirmTips\":\"" + (this.Language == "zh-cn" ? "确定" : "Confirm") + "\"");
            sbLanguageTips.Append(",\"CancelTips\":\"" + (this.Language == "zh-cn" ? "取消" : "Cancel") + "\"");
            sbLanguageTips.Append(",\"DeleteTips\":\"" + (this.Language == "zh-cn" ? "删除" : "Delete") + "\"");
            sbLanguageTips.Append(",\"SureDeleteFile\":\"" + (this.Language == "zh-cn" ? "是否删除文件" : "Delete file ") + "\"");
            sbLanguageTips.Append(",\"FileListHadUploadedTips\":\"" + (this.Language == "zh-cn" ? "已上传文件列表" : "The file list had uploaded ") + "\"");
            sbLanguageTips.Append(",\"UoloadSuccessTips\":\"" + (this.Language == "zh-cn" ? "上传文件成功！" : "Upload files successfully!") + "\"");
            sbLanguageTips.Append(",\"DownloadTips\":\"" + (this.Language == "zh-cn" ? "下载" : "Download") + "\"");
            sbLanguageTips.Append(",\"DeleteAllFileTips\":\"" + (this.Language == "zh-cn" ? "删除所有文件" : "Delete all files") + "\"");
            sbLanguageTips.Append(",\"NoFileListTips\":\"" + (this.Language == "zh-cn" ? "暂无上传文件" : "No file uploaded") + "\"");
            sbLanguageTips.Append(",\"InitImageDataTips\":\"" + (this.Language == "zh-cn" ? "初始化图片数据" : "Init. Image Data") + "\"");
            sbLanguageTips.Append(",\"ViewUploadRuleTips\":\"" + (this.Language == "zh-cn" ? "查看上传规则" : "View upload rules") + "\"");
            sbLanguageTips.Append("}");
            if (String.IsNullOrEmpty(strFolder))
            {
                strReturnCode = "-1";
                strReturnMsg = (this.Language == "zh-cn" ? "失败，页面需传递folder参数" : "Failed,the page's param must contain the param folder!");
            }
            else
            {
                //设置中英文
                ResourceManager rm = base.GetResourceManager("UpDownLoad");

                //加载Folder对应的上传下载配置数据信息
                String strConfigData = this.GetUploadFolderConfigData(strFolder);

                String strUserName = this.Language == "zh-cn" ? this.GetUserInfo().SUSERNAMECN : this.GetUserInfo().SUSERNAME;


                sbResultData.Append(",\"UserId\":\"" + UserLoginBll.Language.ToString() + "\"");
                sbResultData.Append(",\"UserName\":\"" + strUserName + "\"");
                sbResultData.Append(",\"Language\":\"" + this.Language + "\"");
                sbResultData.Append(",\"ProjectId\":\"" + this.GetProjectId() + "\"");
                sbResultData.Append(",\"RemoteServer\":\"" + this.GetRemoteServer() + "\"");
                sbResultData.Append(",\"Folder\":\"" + strFolder + "\"");
                sbResultData.Append(",\"FolderName\":\"" + (this.Language == "zh-cn" ? this.entityXml.NAME_CN : this.entityXml.NAME) + "\"");
                sbResultData.Append(",\"FilePath\":\"" + Microsoft.JScript.GlobalObject.escape(this.entityXml.PATH) + "\"");

                // add by sammen 20230328 增加上传文件支持类型字段
                String strAllowType = this.entityXml.ALLOWTYPE==null?"":this.entityXml.ALLOWTYPE.Replace(";", ",");
                if(!(strAllowType.Equals("")|| strAllowType.Equals("*")))
                {
                    strAllowType = strAllowType.Replace(",", "\",\"");
                    strAllowType = "[\"" + strAllowType + "\"]";
                }else
                {
                    strAllowType = "[]";
                }
                sbResultData.Append(",\"AllowType\":\"" + Microsoft.JScript.GlobalObject.escape(strAllowType) + "\"");

                if (!String.IsNullOrEmpty(strConfigData))
                {
                    sbReturnRowData.Append(strConfigData);
                    strReturnCode = "1";
                    strReturnMsg = (this.Language == "zh-cn" ? "加载页面成功" : "Load page successfully!");
                }
                else
                {
                    strReturnCode = "-10";
                    strReturnMsg = (this.Language == "zh-cn" ? "加载页面失败，页面传递参数[" + strFolder + "]错误" : "Failed,the page's param [" + strFolder + "] is wrong!");
                }
            }
            sbResultData.Append(",\"LanguageTips\":" + sbLanguageTips.ToString() + "");
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = (this.Language == "zh-cn" ? "加载页面出错!" : "Upload files faild,please reload page and try again");
            log.Error(ex);
        }
        finally
        {

            sbResult.Append("{");
            sbResult.Append("\"ReturnCode\":\"" + strReturnCode.ToString() + "\"");
            sbResult.Append(",\"ReturnMsg\":\"" + strReturnMsg.ToString() + "\"");
            sbResult.Append(sbResultData.ToString());
            if (!String.IsNullOrEmpty(sbReturnRowData.ToString()))
            {
                sbResult.Append(",\"UploadFolderConfigData\"" + sbReturnRowData.ToString() + "");
            }
            sbResult.Append("}");
            log.Error(strMethodDesc + "Return Json:" + sbResult.ToString());
        }
        return sbResult.ToString();
    }

    /// <summary>
    /// 加载Folder对应的上传下载配置数据信息
    /// </summary>
    private string GetUploadFolderConfigData(String strFolder)
    {
        StringBuilder sbResult = new StringBuilder();
        StringBuilder sbSql = new StringBuilder();
        try
        {
            //加载Folder对应的上传下载配置数据信息
            sbSql.Append("SELECT * FROM UPDOWNCONFIGPARAM_1 WHERE FID = '"+ strFolder + "' \r\n");

            String strSql = sbSql.ToString();
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
            if(dt!=null&&(dt.Rows.Count>0))
            {
                DataRow dr = dt.Rows[0];
                this.entityXml = ReadConfigDataBll.GetUpLoadConfigData(strFolder);
                sbResult.Append(WebCommon.GetJsonStringByDataTable(dt, "", true));
            }

        }
        catch (Exception ex)
        {

        }
        return sbResult.ToString();

    }

    /// <summary>
    /// 上传附件操作
    /// </summary>
    private String SaveInputFile(String strFolder,String strFilePath)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        StringBuilder sbReturnRowData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "上传附件操作";

        int iUploadCount = 0;
        try
        {
            UpDownLoadXmlEntity entityXml = ReadConfigDataBll.GetUpLoadConfigData(this.strFileFolder);
            if (String.IsNullOrEmpty(strFolder) || String.IsNullOrEmpty(strFilePath)
                || entityXml == null || entityXml.ID == null || !entityXml.PATH.ToLower().Equals(strFilePath.ToLower()))
            {
                strReturnCode = "-1";
                strReturnMsg = (this.Language == "zh-cn" ? "上传文件失败，页面传递参数错误" : "Upload files Failed,the param config is wrong!");

            }
            else
            {
                HttpFileCollection Files = HttpContext.Current.Request.Files;//该集合是所有fileupload文件的集合。

                if (Files.Count > 0)
                {
                    String strAllowType = entityXml.ALLOWTYPE.Replace(";", ",") + ",";
                    //String strAllowType = "sql,";

                    bool bIsTypeValid = true;
                    //获取服务器端文件件地址
                    //String strServerFilePath = Server.MapPath(strFilePath);
                    String strServerFilePath = strFilePath;
                    log.Error("TempInput.html上传文件，ServerFilePath：" + strServerFilePath);
                    if (!Directory.Exists(strServerFilePath))
                    {
                        Directory.CreateDirectory(strServerFilePath);
                    }
                    for (int i = 0; i < Files.Count; i++)
                    {
                        HttpPostedFile PostedFile = Files[i];
                        if (PostedFile.ContentLength > 0)
                        {
                            string FileName = PostedFile.FileName;//文件名自行处理
                            String[] arrayFileName = FileName.Split('.');
                            int iLength = arrayFileName.Length;
                            String strExtType = iLength >= 2 ? arrayFileName[iLength - 1] : "";
                            if(strAllowType.ToLower().IndexOf(strExtType.ToLower() + ",")>-1)
                            {
                                string FilePathAndName = strServerFilePath + "\\" + FileName;//文件名自行处理
                                PostedFile.SaveAs(FilePathAndName);
                                log.Error("UpDownLoad_FileInput.aspx 上传文件：" + FilePathAndName);
                            }else{
                                bIsTypeValid = false;
                                continue;
                            }

                        }
                        iUploadCount++;
                    }
                    if(bIsTypeValid)
                    {
                        strReturnCode = "1";
                        strReturnMsg = (this.Language == "zh-cn" ? "上传文件成功!" : "Upload files successfully!");
                    }else{
                        strReturnCode = "1";
                        strReturnMsg = (this.Language == "zh-cn" ? "部分文件（文件扩展名支持"+ strAllowType + "）上传成功！" : "Part of files Uploaded successfully!");
                    }

                    //写入成功后将改文件夹下的文件名写入到数据库
                    //UpDownLoadXmlEntity entityXml = ReadConfigDataBll.GetUpLoadConfigData(this.strFileFolder);
                    ReadConfigDataBll.WriteUploadFileListToDB(entityXml);
                }else
                {
                    strReturnCode = "-9";
                    strReturnMsg = (this.Language == "zh-cn" ? "上传文件失败，请选择至少一个文件!" : "Upload files failed,Please select at least one file!");
                }
            }
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = (this.Language == "zh-cn" ? "上传文件出错!" : "Upload files faild,please reload page and try again");
            log.Error(ex);
        }
        finally
        {

            sbResult.Append("{");
            sbResult.Append("\"ReturnCode\":\"" + strReturnCode.ToString() + "\"");
            sbResult.Append(",\"ReturnMsg\":\"" + strReturnMsg.ToString() + "\"");
            sbResult.Append(",\"ReturnCount\":\"" + strReturnMsg.ToString() + "\"");
            sbResult.Append("}");
            log.Error(strMethodDesc + "Return Json:" + sbResult.ToString());
        }
        return sbResult.ToString();
    }

    /// <summary>
    /// 读取文件夹路径下的文件
    /// </summary>
    private String GetDownloadFileList(String strFolder, String strFilePath)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        StringBuilder sbReturnRowData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "读取文件夹路径下的文件";

        int iCount = 0;
        try
        {
            //加载Folder对应的上传下载配置数据信息
            UpDownLoadXmlEntity entityXml = ReadConfigDataBll.GetUpLoadConfigData(strFolder);

            if (String.IsNullOrEmpty(strFolder) || String.IsNullOrEmpty(strFilePath)
                || entityXml==null || entityXml.ID == null || !entityXml.PATH.ToLower().Equals(strFilePath.ToLower()))
            {
                strReturnCode = "-1";
                strReturnMsg = (this.Language == "zh-cn" ? "读取文件失败，页面传递参数错误" : "Read files Failed,the param config is wrong!");
            }
            else
            {
                //获取服务器端文件件地址
                //String strServerFilePath = Server.MapPath(strFilePath);
                String strServerFilePath = strFilePath;
                if (Directory.Exists(strServerFilePath))
                {
                    String strEncodePath = HttpUtility.UrlEncode(strServerFilePath);
                    DirectoryInfo thisOne = new DirectoryInfo(strServerFilePath);
                    FileInfo[] fileInfo = thisOne.GetFiles();
                    sbReturnRowData.Append("[");
                    // 遍历所有的文件和目录
                    foreach (FileInfo file in fileInfo)
                    {
                        String strFileName = file.Name;
                        String strFileLength = (file.Length / 1000.00).ToString();//kb;
                        String strEncodeFileName = Microsoft.JScript.GlobalObject.escape(strFileName);
                        String strCreateTime = file.CreationTime.ToString("yyyy-MM-dd HH:mm:ss");
                        String strLastWriteTime = file.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss");

                        if(iCount>0)
                        {
                            sbReturnRowData.Append(",");
                        }
                        sbReturnRowData.Append("{");
                        sbReturnRowData.Append("\"FileName\":\""+ strEncodeFileName + "\"");
                        sbReturnRowData.Append(",\"FileLength\":\"" + strFileLength + "\"");
                        sbReturnRowData.Append(",\"CreateTime\":\"" + strCreateTime + "\"");
                        sbReturnRowData.Append(",\"LastWriteTime\":\"" + strLastWriteTime + "\"");

                        sbReturnRowData.Append("}");
                        iCount++;
                    }
                    sbReturnRowData.Append("]");

                    strReturnCode = "1";
                    strReturnMsg = (this.Language == "zh-cn" ? "读取文件成功!" : "Read files successfully!");
                }else
                {
                    strReturnCode = "-10";
                    strReturnMsg = (this.Language == "zh-cn" ? "读取文件失败，文件夹不存在!" : "Read files failed,the file foler is not exists!");
                }
            }
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = (this.Language == "zh-cn" ? "读取文件出错!" : "Read files faild,please reload page and try again");
            log.Error(ex);
        }
        finally
        {

            sbResult.Append("{");
            sbResult.Append("\"ReturnCode\":\"" + strReturnCode.ToString() + "\"");
            sbResult.Append(",\"ReturnMsg\":\"" + strReturnMsg.ToString() + "\"");
            sbResult.Append(",\"ReturnCount\":\"" + iCount.ToString() + "\"");
            if (!String.IsNullOrEmpty(sbReturnRowData.ToString()))
            {
                sbResult.Append(",\"FileListData\":" + sbReturnRowData.ToString() + "");
            }
            sbResult.Append("}");
            log.Error(strMethodDesc + "Return Json:" + sbResult.ToString());
        }
        return sbResult.ToString();
    }

    /// <summary>
    /// 针对单个文件的操作
    /// </summary>
    private String OperateOneFile(String strFolder,String strPath, String strFileName,String strOpType)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        StringBuilder sbReturnRowData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "针对单个文件的操作";

        int iCount = 0;
        try
        {
            //加载Folder对应的上传下载配置数据信息
            UpDownLoadXmlEntity entityXml = ReadConfigDataBll.GetUpLoadConfigData(strFolder);

            if (String.IsNullOrEmpty(strFolder) || String.IsNullOrEmpty(strPath) || String.IsNullOrEmpty(strFileName)
                || entityXml == null || entityXml.ID == null || !entityXml.PATH.ToLower().Equals(strPath.ToLower()))
            {
                strReturnCode = "-1";
                strReturnMsg = (this.Language == "zh-cn" ? "操作文件失败，页面传递参数错误" : "Operate files Failed,the param config is wrong!");

            }
            else
            {
                strPath = System.IO.Path.Combine(strPath, strFileName);
                if (File.Exists(strPath))
                {
                    if (strOpType.ToLower().Equals("down"))
                    {
                        System.IO.FileInfo fileInfo = new System.IO.FileInfo(strPath);
                        if (fileInfo.Length <= 0)
                        {
                            strReturnCode = "-2";
                            strReturnMsg = (this.Language == "zh-cn" ? "操作文件失败，文件大小为0，无法下载!" : "The File Size is Zero,Cannot DownLoad!");
                        }
                        else
                        {
                            try
                            {
                                Com.ValuePlus.Utils.RequestUtils.DownLoadFile(strFileName, strPath);
                                strReturnCode = "1";
                                strReturnMsg = (this.Language == "zh-cn" ? "文件下载成功!" : "Download files successfully!");
                            }
                            catch (Exception ex)
                            {
                                strReturnCode = "-3";
                                strReturnMsg = (this.Language == "zh-cn" ? "文件正在使用中,下载失败，!" : "Failed,The is Using,Please try again later!");
                                log.Error(ex);
                            }
                        }
                    }
                    else if (strOpType.ToLower().Equals("delete"))
                    {
                        try
                        {
                            System.IO.File.Delete(strPath);
                            strReturnCode = "1";
                            strReturnMsg = (this.Language == "zh-cn" ? "文件删除成功!" : "Download files successfully!");
                        }
                        catch (Exception ex)
                        {
                            strReturnCode = "-4";
                            strReturnMsg = (this.Language == "zh-cn" ? "文件正在使用中,删除失败，!" : "Failed,The is Using,Please try again later!");
                            log.Error(ex);
                        }
                    }
                }
                else
                {
                    strReturnCode = "-4";
                    strReturnMsg = (this.Language == "zh-cn" ? "指定文件不存在或已被删除!" : "The File is not Exsits or had be deleted!");
                }
            }
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = (this.Language == "zh-cn" ? "操作文件出错!" : "Read files faild,please reload page and try again");
            log.Error(ex);
        }
        finally
        {

            sbResult.Append("{");
            sbResult.Append("\"ReturnCode\":\"" + strReturnCode.ToString() + "\"");
            sbResult.Append(",\"ReturnMsg\":\"" + strReturnMsg.ToString() + "\"");
            sbResult.Append(",\"ReturnCount\":\"" + iCount.ToString() + "\"");
            if (!String.IsNullOrEmpty(sbReturnRowData.ToString()))
            {
                sbResult.Append(",\"FileListData\":" + sbReturnRowData.ToString() + "");
            }
            sbResult.Append("}");
            log.Error(strMethodDesc + "Return Json:" + sbResult.ToString());
        }
        return sbResult.ToString();
    }

    /// <summary>
    /// 初始化资产图片数据
    /// </summary>
    private String InitAssetsImageData(String strFolder)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        StringBuilder sbReturnRowData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "初始化资产图片数据";

        int iCount = 0;
        try
        {
            String strSpName = "";
            if (strFolder.ToUpper().Equals("AIMAGE")){
                strSpName = "USP_AM_InsertAssetImage_Batch";
            }else if (strFolder.ToUpper().Equals("OEIMAGE"))
            {
                strSpName = "USP_OE_InsertOEImage_Batch";
            }
            if(!String.IsNullOrEmpty(strSpName))
            {
                Hashtable hsTableParam = new Hashtable();
                SqlParamDao.ExcuteSPReturnStr(strSpName, hsTableParam);

                strReturnCode = "1";
                strReturnMsg = (this.Language == "zh-cn" ? "初始化资产图片数据成功!" : "Init. images data successfully!");
            }else{
                strReturnCode = "-1";
                strReturnMsg = (this.Language == "zh-cn" ? "初始化资产图片数据失败!" : "Init. images data failed!");
            }

        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = (this.Language == "zh-cn" ? "初始化资产图片数据出错!" : "Init. images data faild,please reload page and try again");
            log.Error(ex);
        }
        finally
        {

            sbResult.Append("{");
            sbResult.Append("\"ReturnCode\":\"" + strReturnCode.ToString() + "\"");
            sbResult.Append(",\"ReturnMsg\":\"" + strReturnMsg.ToString() + "\"");
            sbResult.Append(",\"ReturnCount\":\"" + iCount.ToString() + "\"");
            if (!String.IsNullOrEmpty(sbReturnRowData.ToString()))
            {
                sbResult.Append(",\"FileListData\":" + sbReturnRowData.ToString() + "");
            }
            sbResult.Append("}");
            log.Error(strMethodDesc + "Return Json:" + sbResult.ToString());
        }
        return sbResult.ToString();
    }


}