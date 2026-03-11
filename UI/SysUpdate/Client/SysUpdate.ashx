<%@ WebHandler Language="C#" Class="SysUpdate" %>

using System;
using System.Web;
using System.Text;
using System.Data;
using System.Collections;
using Com.ValuePlus.Web;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Utils.Serializable;
using System.IO;
using Ionic.Zip;
using Com.ValuePlus.Common.Security;

public class SysUpdate : HandlerBase, IHttpHandler, System.Web.SessionState.IRequiresSessionState
{

    public void ProcessRequest (HttpContext context) {
        Hashtable hsTableUrlQuery = this.GetUrlAnalyse(context);
        string strParam = hsTableUrlQuery["param"] == null ? string.Empty : hsTableUrlQuery["param"].ToString();//param
        string strDeployNo = hsTableUrlQuery["deployno"] == null ? string.Empty : hsTableUrlQuery["deployno"].ToString();//param
        string strProjectId = hsTableUrlQuery["projectid"] == null ? string.Empty : hsTableUrlQuery["projectid"].ToString();//param

        if (strParam.Equals("update"))
        {
            this.DoUpdateOperation(context, strDeployNo,strProjectId);
        }
        else if (strParam.Equals("getupdatedlist"))
        {
            this.GetUpdateList(context);
        }
    }

    /// <summary>
    /// 执行更新操作
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strDeployNo"></param>
    /// <param name="strProjectId"></param>
    private void DoUpdateOperation(HttpContext context,String strDeployNo,String strProjectId)
    {
        try
        {
            String strFileBase64String = context.Request["txtUpdateFile"].ToString();
            //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
            strFileBase64String = SQLInjectionDefense.ReplaceSQLReservedKeyword(strFileBase64String);

            string strFileSavePath = "~/UserFile/UpdatedFiles/" + strDeployNo;
            string strFileSaveName = strDeployNo + ".zip";
            string strCurrentSaveFolderPath = HttpContext.Current.Server.MapPath(strFileSavePath);
            string strCurrentSaveFilePath = strCurrentSaveFolderPath + "\\" + strFileSaveName;

            if (!Directory.Exists(strCurrentSaveFolderPath))
            {
                Directory.CreateDirectory(strCurrentSaveFolderPath);
            }


            byte[] fileByte = Convert.FromBase64String(strFileBase64String);
            String strFileType = "";
            if (fileByte != null && fileByte.Length > 0)//数据是否为空
            {
                //可以获取文件类型
                strFileType = fileByte[0].ToString() + fileByte[1].ToString();

                System.IO.File.WriteAllBytes(strCurrentSaveFilePath, fileByte);//your_bytes就是byte数组

                bool IsSuccess = this.UnzipAllZipFile(strCurrentSaveFolderPath, strCurrentSaveFilePath);

                //不管更新是否成功,记录更新记录
                this.RecordUpdateHistory(context, strDeployNo, strProjectId, IsSuccess);
                if (IsSuccess)
                {
                    //完成更新后，设置服务器端标识
                    context.Response.Write("1");
                }
                else
                {
                    context.Response.Write("0");
                }
            }
        }
        catch (Exception ex)
        {
            context.Response.Write("-1");
            log.Error(ex);
        }
        //context.Response.Write("strDeployNo:"+strDeployNo+";strProjectId:"+strProjectId);
    }

    /// <summary>
    /// 从zip文件中解压全部文件
    /// 使用控件Ionic.Zip压缩解压文件
    /// </summary>
    /// <param name="strFolder"></param>
    /// <param name="strFileName"></param>
    private bool UnzipAllZipFile(string strFolder,string strFileName)
    {
        bool IsSuccess = true;
        try
        {
            strFolder = strFolder + "\\Unzip";
            if (Directory.Exists(strFolder))
            {
                Directory.Delete(strFolder, true);
            }
            //加上System.Text.Encoding.Default，解决中文乱码问题
            using (ZipFile zip = ZipFile.Read(strFileName, System.Text.Encoding.Default))
            {
                //zip.Password = "123456";//密码解压  
                foreach (ZipEntry entry in zip)
                {
                    //Extract解压zip文件包的方法，参数是保存解压后文件的路基
                    entry.Extract(strFolder);
                }
            }

            //解压后更新App文件执行数据库脚本
            IsSuccess = this.OperationFiles(strFolder);
        }
        catch (Exception ex)
        {
            //脚本文件执行失败
            IsSuccess = false;
            log.Error("自动更新程序时,解压更新包出错。\r\n");
            log.Error(ex.ToString());
        }
        return IsSuccess;
    }

    /// <summary>
    /// 解压后更新App文件执行数据库脚本
    /// </summary>
    /// <param name="strFolder"></param>
    private bool OperationFiles(string strFolder)
    {
        bool IsSuccess = true;
        try
        {
            bool IsSuccess_CopyApp = true;
            bool IsSuccess_ExecuteScripts = true;
            if (Directory.Exists(strFolder))
            {
                String strEncodePath = HttpUtility.UrlEncode(strFolder);
                DirectoryInfo thisOne = new DirectoryInfo(strFolder);
                FileInfo[] fileInfo = thisOne.GetFiles();
                DirectoryInfo[] directoryInfo = thisOne.GetDirectories();
                foreach (DirectoryInfo folder in directoryInfo)
                {
                    DirectoryInfo[] directoryInfo_sub = folder.GetDirectories();
                    foreach (DirectoryInfo folder_sub in directoryInfo_sub)
                    {
                        String strFloderName = folder_sub.Name;
                        //App目錄下則覆蓋拷貝到目標目錄
                        if (strFloderName.ToString().ToLower().Equals("app"))
                        {
                            String strSrcPath_App = folder_sub.FullName;
                            //String strAimPath = "d:\\Temp";
                            String strAimPath = HttpContext.Current.Server.MapPath("~");
                            //如果有特殊文件夹需要删除 add by sammen 20240115
                            DeleteSpecialFolder(strSrcPath_App,strAimPath);
                            //先删除后，再拷贝覆盖
                            IsSuccess_CopyApp = CopyDir(strSrcPath_App, strAimPath);
                            continue;
                        }
                        else if (strFloderName.ToString().ToLower().Equals("scripts"))
                        {
                            String strSrcPath_Scripts = folder_sub.FullName;
                            //执行数据库脚本
                            IsSuccess_ExecuteScripts = this.ExecuteScript(strSrcPath_Scripts);
                            //IsSuccess_ExecuteScripts = true;
                            continue;
                        }
                    }
                }
            }
            if ((!IsSuccess_CopyApp)||(!IsSuccess_ExecuteScripts))
            {
                IsSuccess = false;
            }
        }
        catch (Exception ex)
        {
            //脚本文件执行失败
            IsSuccess = false;
            log.Error("自动更新程序时,操作更新包内文件出错。\r\n");
            log.Error(ex.ToString());
        }
        return IsSuccess;

    }

    /// <summary>
    /// 如果有特殊文件夹需要删除，则先删除，再拷贝覆盖
    /// </summary>
    /// <param name="srcPath"></param>
    /// <returns></returns>
    private bool DeleteSpecialFolder(String srcPath,String aimPath){
        bool IsSuccess = true;
        try
        {
            // 检查目标目录是否以目录分割字符结束如果不是则添加之
            if (aimPath[aimPath.Length - 1] != System.IO.Path.DirectorySeparatorChar)
            {
                aimPath += System.IO.Path.DirectorySeparatorChar;
            }
            // 得到源目录的文件列表，该里面是包含文件以及目录路径的一个数组
            // 如果你指向copy目标文件下面的文件而不包含目录请使用下面的方法
            string[] fileList = System.IO.Directory.GetFileSystemEntries(srcPath);
            // 遍历更新包中源文件的所有的目录
            foreach (string file in fileList)
            {
                if (System.IO.Directory.Exists(file))
                {
                    //add by sammen 20240115
                    //HRMobile手机端HR应用用的Uniapp版本，是需要打包后进行发布，而每次打包的文件个数和文件名都不尽相同，所有先删除此文件夹下所有内容后，再新建文件夹
                    //更新包解压路径文件夹如以“App\HRMobile”结尾时
                    String strMathHRMobileString = file.Substring(file.Length - 12);
                    //log.Error("获取需删除文件夹的最后几个字符:" + strMathHRMobileString + "\r\n");
                    if (strMathHRMobileString.ToUpper().Equals("App\\HRMobile".ToUpper()))
                    {
                        String strServerAimPath = aimPath + System.IO.Path.GetFileName(file);
                        //log.Error("检测到mobile文件夹:"+strServerAimPath+"\r\n");
                        if (System.IO.Directory.Exists(strServerAimPath))
                        {
                            System.IO.Directory.Delete(strServerAimPath, true);
                            //log.Error("已删除文件夹HRMobile:" + strServerAimPath + "\r\n");
                            break;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            //脚本文件执行失败
            IsSuccess = false;
            log.Error("删除特殊文件夹出错。\r\n");
            log.Error(ex.ToString());
        }
        return IsSuccess;

    }

    // <summary>
    /// 复制文件夹下面的所有内容
    /// </summary>
    /// <param name="srcPath"></param>
    /// <param name="aimPath"></param>
    private bool CopyDir(string srcPath,string aimPath)
    {
        bool IsSuccess = true;
        try
        {
            // 检查目标目录是否以目录分割字符结束如果不是则添加之
            if (aimPath[aimPath.Length - 1] != System.IO.Path.DirectorySeparatorChar)
            {
                aimPath += System.IO.Path.DirectorySeparatorChar;
            }
            // 判断目标目录是否存在如果不存在则新建之
            if (!System.IO.Directory.Exists(aimPath))
            {
                System.IO.Directory.CreateDirectory(aimPath);
            }
            // 得到源目录的文件列表，该里面是包含文件以及目录路径的一个数组
            // 如果你指向copy目标文件下面的文件而不包含目录请使用下面的方法
            // string[] fileList = System.IO.Directory.GetFiles(srcPath);
            string[] fileList = System.IO.Directory.GetFileSystemEntries(srcPath);
            // 遍历所有的文件和目录
            foreach (string file in fileList)
            {
                // 先当作目录处理如果存在这个目录就递归Copy该目录下面的文件
                if (System.IO.Directory.Exists(file))
                {
                    CopyDir(file, aimPath + System.IO.Path.GetFileName(file));
                }
                // 否则直接Copy文件
                else
                {
                    String strFileNameAim = System.IO.Path.GetFileName(file);
                    String strFilePathAim = aimPath + strFileNameAim;
                    log.Error("自动更新应用文件:"+strFilePathAim+"\r\n");

                    //部分路径下的文件在覆盖前先进行一次备份 add by sammen 20170814
                    ////########先备份
                    if ((aimPath.IndexOf("SysFile")>1)
                            ||(aimPath.IndexOf("SysFile\\Action")>1)
                            ||(aimPath.IndexOf("SysFile\\Report")>1)
                            ||(aimPath.IndexOf("SysFile\\SP")>1))
                    {
                        if (System.IO.Directory.Exists(strFilePathAim))
                        {
                            String[] strArrayFileTmp = strFileNameAim.Split('.');
                            String strFileName_Backup = strArrayFileTmp[0] + "_Bakcup_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "." + strArrayFileTmp[1];
                            String strFilePath_Backup = aimPath + strFileName_Backup;
                            System.IO.File.Copy(strFilePathAim, strFilePath_Backup, true);
                        }
                    }

                    //########备份后再覆盖更新
                    System.IO.File.Copy(file, strFilePathAim, true);

                }
            }
        }
        catch (Exception ex)
        {
            //脚本文件执行失败
            IsSuccess = false;
            log.Error("自动更新应用文件出错。\r\n");
            log.Error(ex.ToString());
        }
        return IsSuccess;

    }

    /// <summary>
    /// 执行脚本文件中数据库脚本
    /// </summary>
    /// <param name="strFolder"></param>
    private bool ExecuteScript(String strFolder)
    {
        bool IsSuccess = true;
        if (Directory.Exists(strFolder))
        {
            String strEncodePath = HttpUtility.UrlEncode(strFolder);
            DirectoryInfo thisOne = new DirectoryInfo(strFolder);
            FileInfo[] fileInfo = thisOne.GetFiles();
            int iFileCount = fileInfo.Length;
            if (iFileCount > 0) {
                //按文件名排序（顺序）
                SortAsFileName(ref fileInfo);

                for (int i = 0; i < iFileCount; i++)
                {
                    bool IsSuccess_CurScript = false;
                    FileInfo file = fileInfo[i];
                    String strScriptFileName = file.FullName;
                    StringBuilder sbSql = new StringBuilder();
                    bool bIsSqlValid = false;

                    FileStream stream = new FileStream(strScriptFileName, FileMode.Open);
                    StreamReader reader = new StreamReader(stream, System.Text.Encoding.Default);
                    try
                    {
                        String strLine = "";
                        while ((strLine = reader.ReadLine()) != null)
                        {
                            bIsSqlValid = false;
                            if (strLine.Trim().ToUpper() != @"GO")
                            {
                                sbSql.AppendLine(strLine);
                            }
                            else
                            {
                                if (!String.IsNullOrEmpty(sbSql.ToString()))
                                {
                                    try
                                    {
                                        int iReturn = SqlParamDao.ExecuteNonQueryBySql(sbSql.ToString());
                                        IsSuccess_CurScript = true;
                                        bIsSqlValid = true;
                                        sbSql = new StringBuilder();
                                    }
                                    catch (Exception ex)
                                    {
                                        //脚本文件执行失败
                                        IsSuccess_CurScript = false;
                                        bIsSqlValid = false;
                                        log.Error("自动更新脚本错误，脚本："+sbSql.ToString()+"\r\n");
                                        log.Error(ex.ToString());
                                        break;
                                    }
                                }
                            }
                        }

                    }catch (Exception ex)
                    {
                        //获取脚本文件出错
                        IsSuccess_CurScript = false;
                        log.Error("自动更新脚本错误，获取脚本文件出错\r\n");
                        log.Error(ex.ToString());
                        sbSql = new StringBuilder();
                        break;
                    }
                    finally
                    {
                        reader.Close();
                        reader.Dispose();
                        stream.Close();
                        stream.Dispose();
                    }

                    if (!bIsSqlValid)
                    {
                        //未以GO结束的也可以执行
                        if (!String.IsNullOrEmpty(sbSql.ToString()))
                        {
                            try
                            {
                                int iReturn = SqlParamDao.ExecuteNonQueryBySql(sbSql.ToString());
                                IsSuccess_CurScript = true;
                                sbSql = new StringBuilder();
                            }
                            catch (Exception ex)
                            {
                                //脚本文件执行失败
                                IsSuccess_CurScript = false;
                                log.Error("自动更新脚本错误，脚本："+sbSql.ToString()+"\r\n");
                                log.Error(ex.ToString());
                            }
                        }
                    }

                    IsSuccess = IsSuccess_CurScript;
                    if (!IsSuccess_CurScript)
                    {
                        log.Error("自动更新脚本错误，请检查脚本文件格式，以GO结束,脚本文件：\r\n"+sbSql.ToString());
                        sbSql = new StringBuilder();
                        break;
                    }

                }
            }
        }
        return IsSuccess;
    }

    //从zip文件中解压出一个文件  
    private void UnzipOneFolder(string strFolder,string strFileName)
    {
        strFolder = strFolder + "\\Unzip";
        using (ZipFile zip = ZipFile.Read(strFileName,System.Text.Encoding.Default))
        {
            //zip.Password = "123456";//密码解压  
            foreach (ZipEntry entry in zip)
            {
                //Extract解压zip文件包的方法，参数是保存解压后文件的路基  
                entry.Extract(strFolder);
                //Extract解压zip文件包的方法，参数是保存解压后文件的路基  
                zip["Jayzai.xml"].Extract(strFolder);
            }
        }
    }

    /// <summary>
    /// 成功更新后设置服务器更新标志
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strDeployNo"></param>
    /// <param name="strProjectId"></param>
    /// <param name="bIsSuccess"></param>
    private void RecordUpdateHistory(HttpContext context,String strDeployNo,String strProjectId,bool bIsSuccess)
    {
        StringBuilder sbSql = new StringBuilder();
        try
        {
            String strSEQNO = System.Guid.NewGuid().ToString();
            String strDeployTime = context.Request["txtDeployTime"].ToString();
            String strDEPLOYDESC = context.Request["txtDEPLOYDESC"].ToString().Replace("'","''");
            String strDEPLOYKEY = context.Request["txtDEPLOYKEY"].ToString().Replace("'","''");
            String strUpdateDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
            strDeployTime = SQLInjectionDefense.ReplaceSQLReservedKeyword(strDeployTime);
            strDEPLOYDESC = SQLInjectionDefense.ReplaceSQLReservedKeyword(strDEPLOYDESC);
            strDEPLOYKEY = SQLInjectionDefense.ReplaceSQLReservedKeyword(strDEPLOYKEY);

            sbSql.Append("INSERT INTO [TB_SysUpdateList] ([SEQNO] ,[DEPLOYNO] ,[DeployTime] ,[DEPLOYDESC] ,[DEPLOYKEY] ");
            sbSql.Append(" ,[ProjectId],[UpdateUserId] ,[UpdateTime] ,[UpdateClientIP] ,[AppWebSite] ,[RemoteServer] ,[IsSuccess])");
            sbSql.Append(" values ('"+strSEQNO+"','"+strDeployNo+"','"+strDeployTime+"','"+strDEPLOYDESC+"','"+strDEPLOYKEY+"'");
            sbSql.Append(" ,'"+this.GetProjectId()+"','"+this.GetUserCode()+"','"+strUpdateDateTime+"','"+this.GetClientIPAddress()+"'");
            sbSql.Append(" ,'"+this.GetSiteWebAddress()+"','"+this.GetRemoteServer()+"','"+bIsSuccess.ToString()+"')");

            //log.Error("更新成功后的处理:"+sbSql.ToString());
            int btReturn = SqlParamDao.ExecuteNonQueryBySql(sbSql.ToString());

        }
        catch (Exception ex)
        {
            log.Error(ex.ToString());
        }
    }

    /// <summary>
    /// 获取可更新的更新列表
    /// </summary>
    /// <param name="context"></param>
    private void GetUpdateList(HttpContext context)
    {
        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select * from [TB_SysUpdateList] order by [UpdateTime] desc");

            DataTable dt = SqlParamDao.GetDataTableBySql(sbSql.ToString());
            int iColCount = dt.Columns.Count;
            StringBuilder sBuilder = new StringBuilder();
            sBuilder.Append("{ResultData:[ ");
            if ((dt != null) && (dt.Rows.Count > 0))
            {
                //sBuilder.Append("   " + this.ulHeaderMenu.ClientID + ".innerHTML =\"\r\n");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i > 0)
                    {
                        sBuilder.Append(",{");
                    }
                    else
                    {
                        sBuilder.Append("{");
                    }
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        String strColName = dt.Columns[j].ColumnName;
                        //对值进行编码处理特殊字符，如引号等
                        String strColValue = Microsoft.JScript.GlobalObject.escape(dt.Rows[i][dt.Columns[j].ColumnName].ToString());
                        //String strColValue = dt.Rows[i][dt.Columns[j].ColumnName].ToString();
                        if (j == 0)
                        {
                            sBuilder.Append(strColName + ":'" + strColValue + "'");
                        }
                        else
                        {
                            sBuilder.Append("," + strColName + ":'" + strColValue + "'");
                        }
                    }

                    sBuilder.Append("}");
                }

            }
            sBuilder.Append("]");
            sBuilder.Append("}");

            string json = sBuilder.ToString();

            context.Response.Write(json);

        }
        catch (Exception ex)
        {
            log.Error(ex);
            context.Response.Write("error");
        }
    }

    /// <summary>
    /// 按文件名排序（顺序）
    /// </summary>
    /// <param name="arrFi">待排序数组</param>
    private void SortAsFileName(ref FileInfo[] arrFi)
    {
        Array.Sort(arrFi, delegate(FileInfo x, FileInfo y) { return x.Name.CompareTo(y.Name); });
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}

//常见文件类型对应的byte数据
//199196		sqlite数据库文件
//7076			flv视频文件
//6787			swf视频文件
//7173			gif
//255216		jpg
//13780			png
//6677			bmp
//239187		txt,aspx,asp,sql
//208207		xls.doc.ppt
//6063			xml
//6033			htm,html
//4742			js
//8075			xlsx,zip,pptx,mmap,zip,docx
//8297			rar
//01			accdb,mdb
//7790			exe,dll
//5666			psd
//255254		rdp
//10056			bt种子
//64101			bat
//255254		csv
//3780			pdf
