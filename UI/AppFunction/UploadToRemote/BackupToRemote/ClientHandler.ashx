<%@ WebHandler Language="C#" Class="ClientHandler" %>

using System;
using System.Web;
using System.Text;
using System.Data;
using System.IO;
using System.Collections;
using Com.ValuePlus.Web;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Utils.Serializable;
using Ionic.Zip;

public class ClientHandler : HandlerBase, IHttpHandler, System.Web.SessionState.IRequiresSessionState {

    private string strFileSaveRootPath = "~/UserFile/BackupToRemoteFiles/";
    public void ProcessRequest (HttpContext context) {
        Hashtable hsTableUrlQuery = this.GetUrlAnalyse(context);
        string strCallback = hsTableUrlQuery["callback"] == null ? string.Empty : hsTableUrlQuery["callback"].ToString();//param
        string strParam = hsTableUrlQuery["param"] == null ? string.Empty : hsTableUrlQuery["param"].ToString();//param
        string strProjectId = hsTableUrlQuery["projectid"] == null ? string.Empty : hsTableUrlQuery["projectid"].ToString();//param
        string strBKTR = hsTableUrlQuery["bktr"] == null ? string.Empty : hsTableUrlQuery["bktr"].ToString();//bktr
        string strBKTRName = hsTableUrlQuery["bktrname"] == null ? string.Empty : hsTableUrlQuery["bktrname"].ToString();//bktrname
        string strBKTRType = hsTableUrlQuery["bktrtype"] == null ? string.Empty : hsTableUrlQuery["bktrtype"].ToString();//bktrtype
        string strBKTRFolder = hsTableUrlQuery["bktrfolder"] == null ? string.Empty : hsTableUrlQuery["bktrfolder"].ToString();//bktr
        string strObjectType = hsTableUrlQuery["objecttype"] == null ? string.Empty : hsTableUrlQuery["objecttype"].ToString();//param

        log.Error("(BackupToRemote/ClientHandler)"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"---Param:"+strParam);

        if (strParam.Equals("getbackuplist"))
        {
            this.GetBackupList(context);
        }
        else if (strParam.Equals("backupobject"))
        {
            this.BuildDataObjectFile(context,strBKTR,strBKTRName,strBKTRType,strBKTRFolder,this.GetUserCode());
        }
        else if (strParam.Equals("setsendrecord"))
        {
            this.SetSendRecord(context,strBKTR,this.GetUserCode());
        }
    }


    /// <summary>
    /// 创建数据库对象的结构和数据到文件
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strBKTR"></param>
    /// <param name="strBKTRType"></param>
    /// <param name="strBKTRName"></param>
    /// <param name="strBKTRFolder"></param>
    /// <param name="strUserId"></param>
    private void BuildDataObjectFile(HttpContext context,String strBKTR,String strBKTRName,String strBKTRType ,String strBKTRFolder ,String strUserId)
    {
        try
        {
            bool IsCanZipFile = true;
            //本次备份时保存的文件夹路径
            string strFileSaveFolder = DateTime.Now.ToString("yyyyMMdd") + "_" + this.GetProjectId() + "_" + strBKTRName;
            string strFileSavePath = strFileSaveRootPath+"/"+strFileSaveFolder;

            if (strBKTRType.Equals("010"))//010','数据库','数据库
            {
                //获取表对象进行脚本备份
                string strSql = "select * from BKTRConfig_2 where BKTR = '" + strBKTR + "' ORDER BY TableName";
                DataTable dt_Table = SqlParamDao.GetDataTableBySql(strSql);
                if (dt_Table != null && dt_Table.Rows.Count > 0)
                {
                    for(int i = 0; i < dt_Table.Rows.Count; i++)
                    {
                        String strTableName = dt_Table.Rows[i]["TableName"].ToString();
                        StringBuilder sbBuildSql = new StringBuilder();

                        //获取数据表结构脚本
                        sbBuildSql.Append(GetTableCreateScripts(strTableName).ToString());
                        sbBuildSql.Append("\r\n");
                        //获取数据表数据脚本
                        sbBuildSql.Append(GetTableDataScripts(strTableName).ToString());

                        //保存脚本到服务器文件
                        SaveScriptsToFile(strFileSavePath,strTableName, sbBuildSql.ToString());
                    }
                }
                //获取视图对象进行脚本备份
                strSql = "select * from BKTRConfig_3 where BKTR = '" + strBKTR + "' ORDER BY ViewName";
                dt_Table = SqlParamDao.GetDataTableBySql(strSql);
                if (dt_Table != null && dt_Table.Rows.Count > 0)
                {
                    for (int i = 0; i < dt_Table.Rows.Count; i++)
                    {
                        String strViewName = dt_Table.Rows[i]["ViewName"].ToString();
                        StringBuilder sbBuildSql = new StringBuilder();

                        //获取数据视图结构脚本
                        sbBuildSql.Append(GetObjectCreateScripts(strViewName,"view").ToString());

                        //保存脚本到服务器文件
                        SaveScriptsToFile(strFileSavePath,strViewName, sbBuildSql.ToString());
                    }
                }
                //获取存储过程对象进行脚本备份
                strSql = "select * from BKTRConfig_4 where BKTR = '" + strBKTR + "' ORDER BY SPName";
                dt_Table = SqlParamDao.GetDataTableBySql(strSql);
                if (dt_Table != null && dt_Table.Rows.Count > 0)
                {
                    for (int i = 0; i < dt_Table.Rows.Count; i++)
                    {
                        String strSPName = dt_Table.Rows[i]["SPName"].ToString();
                        StringBuilder sbBuildSql = new StringBuilder();

                        //获取数据存储过程结构脚本
                        sbBuildSql.Append(GetObjectCreateScripts(strSPName,"sp").ToString());

                        //保存脚本到服务器文件
                        SaveScriptsToFile(strFileSavePath,strSPName, sbBuildSql.ToString());
                    }
                }
                //获取函数对象进行脚本备份
                strSql = "select * from BKTRConfig_5 where BKTR = '" + strBKTR + "' ORDER BY FunctionName";
                dt_Table = SqlParamDao.GetDataTableBySql(strSql);
                if (dt_Table != null && dt_Table.Rows.Count > 0)
                {
                    for (int i = 0; i < dt_Table.Rows.Count; i++)
                    {
                        String strFunctionName = dt_Table.Rows[i]["FunctionName"].ToString();
                        StringBuilder sbBuildSql = new StringBuilder();

                        //获取数据函数结构脚本
                        sbBuildSql.Append(GetObjectCreateScripts(strFunctionName,"function").ToString());

                        //保存脚本到服务器文件
                        SaveScriptsToFile(strFileSavePath,strFunctionName, sbBuildSql.ToString());
                    }
                }
            }else if (strBKTRType.Equals("020"))//020','源文件','源文件
            {
                //首先负责这个文件夹的文件到制定目录
                String strSourcePath = HttpContext.Current.Server.MapPath("~/"+strBKTRFolder);
                String strAimPath = HttpContext.Current.Server.MapPath(strFileSaveRootPath+"/"+strFileSaveFolder);
                bool IsSuccess_CopyApp = CopyDir(strSourcePath, strAimPath);
                IsCanZipFile = IsSuccess_CopyApp;
            }

            if (IsCanZipFile)
            {
                //用zip文件中压缩全部文件
                if(!String.IsNullOrEmpty(this.ZipAllFile(strFileSaveFolder)))
                {
                    String strZipFileName = strFileSaveFolder + ".zip";
                    byte[] btReturn = this.GetFileByte(strZipFileName);
                    //context.Response.Write(Convert.ToBase64String(btReturn));
                    context.Response.Write("{'fileName':'"+strZipFileName+"','fileString':'" + Convert.ToBase64String(btReturn) + "'}");
                }
                else
                {
                    context.Response.Write("-1");
                }
            }else {
                context.Response.Write("-1");
            }

        }
        catch (Exception ex)
        {
            context.Response.Write("-1");
            log.Error(ex);
        }

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
                    log.Error("复制文件夹下面的所有内容:"+strFilePathAim+"\r\n");

                    //########备份后再覆盖更新
                    System.IO.File.Copy(file, strFilePathAim, true);
                }
            }
        }
        catch (Exception ex)
        {
            //脚本文件执行失败
            IsSuccess = false;
            log.Error("复制文件夹下面的所有内容出错。\r\n");
            log.Error(ex.ToString());
        }
        return IsSuccess;

    }

    /// <summary>
    /// 将压缩文件转化成字节流并返回
    /// </summary>
    /// <param name="strFileName"></param>
    /// <returns></returns>
    private byte[] GetFileByte(String strFileName)
    {
        byte[] btReturn = new byte[0];
        try
        {
            FileStream fs = null;
            string strCurrentSaveFolderPath = HttpContext.Current.Server.MapPath(strFileSaveRootPath);

            String strEncodeFileName = HttpUtility.UrlEncode(strFileName);
            String CurrentUploadFilePath = strCurrentSaveFolderPath + "\\" + strFileName;
            if (File.Exists(CurrentUploadFilePath))
            {
                try
                {
                    ///打开现有文件以进行读取。
                    fs = File.OpenRead(CurrentUploadFilePath);
                    int b1;
                    System.IO.MemoryStream tempStream = new System.IO.MemoryStream();
                    while ((b1 = fs.ReadByte()) != -1)
                    {
                        tempStream.WriteByte(((byte)b1));
                    }
                    btReturn = tempStream.ToArray();
                }
                catch (Exception ex)
                {
                    btReturn = new byte[0];
                    log.Error(ex.ToString());
                }
                finally
                {
                    fs.Close();
                }
            }
            else
            {
                btReturn = new byte[0];
            }

        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
        return btReturn;

    }

    /// <summary>
    /// 用zip文件中压缩某文件夹下的全部文件
    /// 使用控件Ionic.Zip
    /// </summary>
    /// <param name="strFileName"></param>
    private string ZipAllFile(string strNeedZipFolder)
    {
        string strReturnZipFileName = "";
        try
        {
            string strCurrentSaveFolderPath = HttpContext.Current.Server.MapPath(strFileSaveRootPath);
            String strZipFileName = strNeedZipFolder + ".zip";

            //先删除已有的zip文件
            if (File.Exists(strCurrentSaveFolderPath + strZipFileName))
            {
                File.Delete(strCurrentSaveFolderPath + strZipFileName);
            }
            //ZipFile实例化一个压缩文件保存路径的一个对象zip
            using (ZipFile zip = new ZipFile(strCurrentSaveFolderPath + strZipFileName,Encoding.Default))
            {
                //加密压缩
                //zip.Password = "123456";
                //将要压缩的文件夹添加到zip对象中去(要压缩的文件夹路径和名称)
                zip.AddDirectory(strCurrentSaveFolderPath+ strNeedZipFolder);
                //将要压缩的文件添加到zip对象中去,如果文件不存在抛错FileNotFoundExcept
                //zip.AddFile(@"E:\\yangfeizai\\12051214544443\\"+"Jayzai.xml");
                zip.Save();
            }

            #region 删除文件夹，只保留zip文件
            String strDeleteFolder = strCurrentSaveFolderPath + strNeedZipFolder;
            this.DeleteFolder(strDeleteFolder);
            #endregion 删除文件夹，只保留zip文件

            strReturnZipFileName = strZipFileName;
        }
        catch (Exception ex)
        {
            //脚本文件执行失败
            strReturnZipFileName = "";
            log.Error("推送备份时,压缩备份文件时出错。\r\n");
            log.Error(ex.ToString());
        }
        return strReturnZipFileName;
    }

    /// <summary>
    /// 清空文件夹并删除文件夹
    /// </summary>
    /// <param name="strDeleteFolder"></param>
    private void DeleteFolder(String strDeleteFolder)
    {
        foreach (string d in Directory.GetFileSystemEntries(strDeleteFolder))
        {
            if (File.Exists(d))
            {
                FileInfo fi = new FileInfo(d);
                if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                    fi.Attributes = FileAttributes.Normal;
                File.Delete(d);//直接删除其中的文件 
            }else
                DeleteFolder(d);////递归删除子文件夹
        }
        if (Directory.Exists(strDeleteFolder))
        {
            Directory.Delete(strDeleteFolder);
        }
    }

    /// <summary>
    /// 保存脚本到服务器文件
    /// </summary>
    /// <param name="strFileSavePath"></param>
    /// <param name="strObjectName"></param>
    /// <param name="strScripts"></param>
    private void SaveScriptsToFile(String strFileSavePath,String strObjectName,String strScripts)
    {
        try
        {
            String strFileSaveName = strObjectName + ".sql";
            string strCurrentSaveFolderPath = HttpContext.Current.Server.MapPath(strFileSavePath);
            string strCurrentSaveFilePath = strCurrentSaveFolderPath + "\\" + strFileSaveName;

            if (!Directory.Exists(strCurrentSaveFolderPath))
            {
                Directory.CreateDirectory(strCurrentSaveFolderPath);
            }
            StreamWriter sw;
            //先删除文件后再写入
            if (File.Exists(strCurrentSaveFilePath))
            {
                File.Delete(strCurrentSaveFilePath);
            }
            sw = File.CreateText(strCurrentSaveFilePath);

            sw.WriteLine(strScripts.ToString());
            sw.Close();
        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
    }

    /// <summary>
    /// 获取某数据表结构的数据Insert脚本
    /// </summary>
    /// <param name="strTableName"></param>
    /// <returns></returns>
    private StringBuilder GetTableDataScripts(String strTableName)
    {
        StringBuilder sbDataSql = new StringBuilder();
        try
        {
            String strSql = "exec USP_SYS_GetTableData_ToInsertScript '"+strTableName+"','1=1','','15000'";
            int iCount = SqlParamDao.ExecuteScalarBySql(strSql);
            strSql = "select * from [_USP_SYS_GetTableData_ToInsertScript]";
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
            if (dt != null && dt.Rows.Count > 0)
            {
                sbDataSql.Append("delete from "+strTableName);
                foreach (DataRow dr in dt.Rows)
                {
                    if (sbDataSql.ToString().Length > 0) sbDataSql.Append("\r\n");
                    object[] objRow = dr.ItemArray;
                    for(int i = 0; i < objRow.Length; i++)
                    {
                        sbDataSql.Append(objRow[i]==DBNull.Value?"null":objRow[i].ToString());
                    }
                }
            }
        }
        catch (Exception ex)
        {
            sbDataSql = null;
            log.Error(ex);
        }
        String strRetrun = sbDataSql.ToString();
        return sbDataSql;

    }

    /// <summary>
    /// 获取某数据表结构的创建脚本
    /// </summary>
    /// <param name="strTableName"></param>
    /// <returns></returns>
    private StringBuilder GetTableCreateScripts(String strTableName)
    {
        StringBuilder sbCreateSql = new StringBuilder();
        try
        {

            sbCreateSql.Append("IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'" + strTableName + "') AND type in (N'U'))\r\n");
            sbCreateSql.Append("BEGIN\r\n");
            sbCreateSql.Append("    DROP TABLE " + strTableName + " \r\n");
            sbCreateSql.Append("END \r\n");
            sbCreateSql.Append("go \r\n");
            sbCreateSql.Append("CREATE TABLE " + strTableName + " ( " + "\r\n");

            StringBuilder strSql_GetDetail = new StringBuilder();
            strSql_GetDetail.Append("SELECT TableName = D.name,ColOrder = A.colorder,ColName = A.name,ColDataType = B.name \r\n");
            strSql_GetDetail.Append(",IsKey = Case When exists(SELECT 1 FROM sysobjects Where xtype='PK' and parent_obj=A.id and name in ( \r\n");
            strSql_GetDetail.Append("       SELECT name FROM sysindexes WHERE indid in( SELECT indid FROM sysindexkeys WHERE id = A.id AND colid=A.colid))) then '1' else '0' end \r\n");
            strSql_GetDetail.Append(",ColLength = COLUMNPROPERTY(A.id,A.name,'PRECISION') \r\n");
            strSql_GetDetail.Append(",ColDecimals = isnull(COLUMNPROPERTY(A.id,A.name,'Scale'),0) \r\n");
            strSql_GetDetail.Append(",IsNull = Case When A.isnullable=1 Then '1'Else '0' End \r\n");
            strSql_GetDetail.Append(",DefaultValue = isnull(E.Text,'') \r\n");
            strSql_GetDetail.Append(" FROM syscolumns A Left Join systypes B On A.xusertype=B.xusertype \r\n");
            strSql_GetDetail.Append(" Inner Join sysobjects D On A.id=D.id and D.xtype='U' \r\n");
            strSql_GetDetail.Append(" Left Join syscomments E on A.cdefault=E.id \r\n");
            strSql_GetDetail.Append(" Left Join sys.extended_properties G on A.id=G.major_id and A.colid=G.minor_id \r\n");
            strSql_GetDetail.Append(" Left Join sys.extended_properties F On D.id=F.major_id and F.minor_id=0 \r\n");
            strSql_GetDetail.Append(" where d.name='"+strTableName+"' \r\n");
            strSql_GetDetail.Append(" Order By A.id,A.colorder \r\n");
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql_GetDetail.ToString());
            if ((dt != null) && (dt.Rows.Count > 0))
            {
                int iKeyCount = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    String strColName = dr["ColName"].ToString();
                    String strColDataType = dr["ColDataType"].ToString();
                    String strIsKey = dr["IsKey"].ToString();
                    String strColLength = dr["ColLength"].ToString();
                    String strColDecimals = dr["ColDecimals"].ToString();
                    String strIsNull = dr["IsNull"].ToString();
                    String strDefaultValue = dr["DefaultValue"].ToString();

                    if (strIsKey.Equals("1"))
                    {
                        iKeyCount++;
                    }
                    String strSqlString = string.Concat(new object[] { "", "[" + strColName + "]", " ", strColDataType });
                    switch (strColDataType)
                    {
                        case "numeric":
                        case "decimal":
                            strSqlString = string.Concat(new object[] { strSqlString, "(", strColLength, ",", strColDecimals, ") " });
                            break;
                        case "datetime":
                        case "date":
                        case "int":
                        case "float":
                        case "image":
                        case "text":
                            strSqlString = string.Concat(new object[] { strSqlString, "" });
                            break;
                        default:
                            strSqlString = string.Concat(new object[] { strSqlString, "(", strColLength, ") " });
                            break;

                    }
                    if (strIsNull.Equals("0"))
                    {
                        strSqlString = strSqlString + " NOT NULL ";
                    }else
                    {
                        strSqlString = strSqlString + " NULL ";
                    }
                    if (iKeyCount > 0)
                    {
                        strSqlString = strSqlString + ",";
                    }
                    else
                    {
                        if (i < dt.Rows.Count - 1)
                        {
                            strSqlString = strSqlString + ",";
                        }
                    }

                    sbCreateSql.Append("	" + strSqlString + "\r\n");
                }

                #region 创建表的主键信息
                if (iKeyCount > 0)
                {
                    sbCreateSql.Append("CONSTRAINT [PK_" + strTableName + "] PRIMARY KEY CLUSTERED");
                    sbCreateSql.Append("(\r\n");
                    int iKeyNum = 0;
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        String strIsKey = dt.Rows[j]["IsKey"].ToString();
                        if (strIsKey.Equals("1"))
                        {
                            iKeyNum++;
                            if (iKeyNum < iKeyCount)
                            {
                                sbCreateSql.Append("	[" + dt.Rows[j]["ColName"] + "] ASC,\r\n");
                            }
                            else
                            {
                                sbCreateSql.Append("	[" + dt.Rows[j]["ColName"] + "] ASC\r\n");
                            }
                        }
                    }


                    sbCreateSql.Append(") WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]  \r\n");
                }
                #endregion

                sbCreateSql.Append(") ON [PRIMARY] \r\n ");
            }
        }
        catch (Exception ex)
        {
            sbCreateSql = null;
            log.Error(ex);
        }
        return sbCreateSql;
    }

    /// <summary>
    /// 获取除数据表之外的数据对象的创建脚本
    /// 包括视图，函数，存储过程
    /// </summary>
    /// <param name="strTableName"></param>
    /// <param name="strObjectType"></param>
    /// <returns></returns>
    private StringBuilder GetObjectCreateScripts(String strOjbectName,String strObjectType)
    {
        StringBuilder sbCreateSql = new StringBuilder();
        try
        {
            switch (strObjectType)
            {
                case "view":
                    sbCreateSql.Append("IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].["+strOjbectName+"]'))\r\n");
                    sbCreateSql.Append("DROP VIEW [dbo].["+strOjbectName+"] \r\n");
                    sbCreateSql.Append("go \r\n");
                    sbCreateSql.Append("\r\n");
                    break;
                case "sp":
                    sbCreateSql.Append("IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].["+strOjbectName+"]') AND type in (N'P', N'PC'))\r\n");
                    sbCreateSql.Append("DROP PROCEDURE [dbo].["+strOjbectName+"] \r\n");
                    sbCreateSql.Append("go \r\n");
                    sbCreateSql.Append("\r\n");
                    break;
                case "function":
                    sbCreateSql.Append("IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].["+strOjbectName+"]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))\r\n");
                    sbCreateSql.Append("DROP FUNCTION [dbo].["+strOjbectName+"] \r\n");
                    sbCreateSql.Append("go \r\n");
                    sbCreateSql.Append("\r\n");
                    break;
            }

            String strCommentSql = "SELECT definition FROM sys.sql_modules INNER JOIN sys.objects ON sys.sql_modules.object_id=sys.objects.object_id where sys.objects.name = '" + strOjbectName + "'";
            DataTable dt = SqlParamDao.GetDataTableBySql(strCommentSql);
            if ((dt != null) && (dt.Rows.Count > 0))
            {
                String strComment = dt.Rows[0]["definition"].ToString();
                sbCreateSql.Append(strComment + "\r\n");
                sbCreateSql.Append("go \r\n");
            }
        }
        catch (Exception ex)
        {
            sbCreateSql = null;
            log.Error(ex);
        }
        return sbCreateSql;
    }


    /// <summary>
    /// 获取备份类型列表
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strYearMonthType"></param>
    private void GetBackupList(HttpContext context)
    {
        try
        {
            String strSql_Cols = "select * from TB_HRTMPD WHERE TID = 'BKTRConfig' AND GID = '1' ORDER BY PORDER ";
            DataTable dt_Cols = SqlParamDao.GetDataTableBySql(strSql_Cols);
            StringBuilder sBuilder_ColName = new StringBuilder();
            sBuilder_ColName.Append("colNames:[ ");
            sBuilder_ColName.Append("{");
            if ((dt_Cols != null) && (dt_Cols.Rows.Count > 0))
            {
                for(int i = 0; i < dt_Cols.Rows.Count; i++)
                {
                    String strColCode = dt_Cols.Rows[i]["PID"].ToString();
                    String strColName = dt_Cols.Rows[i]["PDESCCHS"].ToString();
                    if (!this.Language.Equals("zh-cn"))
                    {
                        strColName = dt_Cols.Rows[i]["PDESC"].ToString();
                    }
                    if (i == 0)
                    {
                        sBuilder_ColName.Append(strColCode+":"+"'"+strColName+"'");
                    }else
                    {
                        sBuilder_ColName.Append(","+strColCode+":"+"'"+strColName+"'");
                    }
                }
            }
            sBuilder_ColName.Append(",TableCount:"+"'Tables Count'");
            sBuilder_ColName.Append(",ViewCount:"+"'Views Count'");
            sBuilder_ColName.Append(",SPCount:"+"'SP Count'");
            sBuilder_ColName.Append(",FunctionCount:"+"'Functions Count'");
            sBuilder_ColName.Append("}");
            sBuilder_ColName.Append("]");
            String strColNames = sBuilder_ColName.ToString();

            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select *");
            sbSql.Append(" ,(select count(1) from BKTRConfig_2 WHERE BKTR = A.BKTR) AS TableCount");
            sbSql.Append(" ,(select count(1) from BKTRConfig_3 WHERE BKTR = A.BKTR) AS ViewCount");
            sbSql.Append(" ,(select count(1) from BKTRConfig_4 WHERE BKTR = A.BKTR) AS SPCount");
            sbSql.Append(" ,(select count(1) from BKTRConfig_5 WHERE BKTR = A.BKTR) AS FunctionCount");
            sbSql.Append(" from BKTRConfig_1 A where 1=1");
            sbSql.Append(" ORDER BY SORDER");
            String strSql = sbSql.ToString();

            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);

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
                        //String strColValue = Microsoft.JScript.GlobalObject.escape(dt.Rows[i][dt.Columns[j].ColumnName].ToString());
                        String strColValue = dt.Rows[i][dt.Columns[j].ColumnName].ToString();
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
            if (!String.IsNullOrEmpty(strColNames))
            {
                sBuilder.Append("," + strColNames);
            }

            String strClientInfo = GetCurClientInfo();
            if (!String.IsNullOrEmpty(strClientInfo))
            {
                sBuilder.Append("," + strClientInfo);
            }
            sBuilder.Append("}");

            string json = sBuilder.ToString();

            context.Response.Write(json);
        }
        catch (Exception ex)
        {
            context.Response.Write("error");
            log.Error(ex);
        }
    }

    /// <summary>
    /// 获取当前客户信息
    /// </summary>
    /// <returns></returns>
    public String GetCurClientInfo()
    {
        try
        {
            String strSql_ProjectId = "select top 1 * from BASICPARAM_1 WHERE paramName = 'ProjectId'";
            String strSql_ClientName = "select top 1 * from TB_HR_MENU WHERE SMENUCODE = 'ROOT'";
            String strSql_RemoteServer = "select top 1 * from BASICPARAM_1 WHERE paramName = 'SysUpdateServer'";
            DataTable dt_ProjectId = SqlParamDao.GetDataTableBySql(strSql_ProjectId);
            DataTable dt_ClientName = SqlParamDao.GetDataTableBySql(strSql_ClientName);
            DataTable dt_RemoteServer = SqlParamDao.GetDataTableBySql(strSql_RemoteServer);
            StringBuilder sBuilder_ClientInfo = new StringBuilder();
            sBuilder_ClientInfo.Append("ClientInfo:[ ");
            sBuilder_ClientInfo.Append("{");
            if ((dt_ProjectId != null) && (dt_ProjectId.Rows.Count > 0))
            {
                String strParamValue = dt_ProjectId.Rows[0]["ParamValue"].ToString();
                sBuilder_ClientInfo.Append("ProjectId:" + "'" + strParamValue + "'");
            }
            if ((dt_ClientName != null) && (dt_ClientName.Rows.Count > 0))
            {
                String strClientName = dt_ClientName.Rows[0]["SMENUNAMECN"].ToString();
                sBuilder_ClientInfo.Append(",ClientName:" + "'" + strClientName + "'");
            }
            if ((dt_RemoteServer != null) && (dt_RemoteServer.Rows.Count > 0))
            {
                String strRemoteServer = dt_RemoteServer.Rows[0]["ParamValue"].ToString();
                sBuilder_ClientInfo.Append(",RemoteServer:" + "'" + strRemoteServer + "'");
            }
            sBuilder_ClientInfo.Append("}");
            sBuilder_ClientInfo.Append("]");
            String strClientInfo = sBuilder_ClientInfo.ToString();
            return strClientInfo;
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return "";
        }
    }

    /// <summary>
    /// 设置备份推送的历史记录
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strYearMonthDCNO"></param>
    /// <param name="strUserId"></param>
    public void SetSendRecord(HttpContext context,String strBKTR,String strUserId)
    {
        try
        {
            String strCurTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("UPDATE [BKTRConfig_1] SET UPDATETIME = '"+strCurTime+"' where [BKTR] = '"+strBKTR+"';");
            sbSql.Append("insert into [BKTRConfig_9]([BKTR],[SEQNO],[OPUSER],[OPTIME])values");
            sbSql.Append("('"+strBKTR+"',ISNULL((select MAX(SEQNO) from [BKTRConfig_9] where [BKTR] = '"+strBKTR+"'),0)+1,'"+strUserId+"','"+strCurTime+"')");

            log.Error("设置备份推送的历史记录，脚本语句："+sbSql.ToString());
            int iReturnValue = SqlParamDao.ExecuteNonQueryBySql(sbSql.ToString());

            context.Response.Write(iReturnValue);

        }
        catch (Exception ex)
        {
            log.Error(ex);
            context.Response.Write("-1");
        }
    }

    #region 根据不同数据对象类型获取其结构明细

    #endregion

    public bool IsReusable {
        get {
            return false;
        }
    }

}