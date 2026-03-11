using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections;
using System.Runtime.InteropServices;
namespace Com.ValuePlus.Utils
{
    /// <summary>
    /// FileUtils 的摘要说明。
    /// </summary>
    public class FileUtils
    {
        //#region 创建目录
        ///// <summary>
        ///// 创建目录
        ///// </summary>
        ///// <param name="name">名称</param>
        ///// <returns>创建是否成功</returns>
        //[DllImport("dbgHelp", SetLastError = true)]
        //private static extern bool MakeSureDirectoryPathExists(string name);
        //#endregion

        #region 建立文件夹
        /// <summary>
        /// 建立文件夹
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool CreateDir(string name)
        {
            System.IO.DirectoryInfo dir = new DirectoryInfo(name);
            //不存在，则创建
            if (!dir.Exists)
            {
                dir.Create();
            }
            return true; ;
        }
        #endregion

        #region 备份文件
        /// <summary>
        /// 备份文件
        /// </summary>
        /// <param name="sourceFileName">源文件名</param>
        /// <param name="destFileName">目标文件名</param>
        /// <param name="overwrite">当目标文件存在时是否覆盖</param>
        /// <returns>操作是否成功</returns>
        public static bool BackupFile(string sourceFileName, string destFileName, bool overwrite)
        {
            if (!System.IO.File.Exists(sourceFileName))
            {
                throw new FileNotFoundException(sourceFileName + "文件不存在！");
            }
            if (!overwrite && System.IO.File.Exists(destFileName))
            {
                return false;
            }
            try
            {
                System.IO.File.Copy(sourceFileName, destFileName, true);
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

        #region 恢复文件
        /// <summary>
        /// 恢复文件
        /// </summary>
        /// <param name="backupFileName">备份文件名</param>
        /// <param name="targetFileName">要恢复的文件名</param>
        /// <param name="backupTargetFileName">要恢复文件再次备份的名称,如果为null,则不再备份恢复文件</param>
        /// <returns>操作是否成功</returns>
        public static bool RestoreFile(string backupFileName, string targetFileName, string backupTargetFileName)
        {
            try
            {
                if (!System.IO.File.Exists(backupFileName))
                {
                    throw new FileNotFoundException(backupFileName + "文件不存在！");
                }
                if (backupTargetFileName != null)
                {
                    if (!System.IO.File.Exists(targetFileName))
                    {
                        throw new FileNotFoundException(targetFileName + "文件不存在！无法备份此文件！");
                    }
                    else
                    {
                        System.IO.File.Copy(targetFileName, backupTargetFileName, true);
                    }
                }
                System.IO.File.Delete(targetFileName);
                System.IO.File.Copy(backupFileName, targetFileName);
            }
            catch (Exception e)
            {
                throw e;
            }
            return true;
        }
        #endregion

        #region GetLinesFromTextFile
        /// <summary>
        /// 得到文本文件的数据
        /// 返回IList 每行一个字串
        /// Encoding为当前系统的ANSI
        /// </summary>
        /// <param name="FilePathAll"></param>
        /// <returns></returns>
        public IList GetLinesFromTextFile(string FilePathAll)
        {
            IList Lines = new ArrayList();
            FileStream fs = new FileStream(FilePathAll, FileMode.Open, FileAccess.Read);
            StreamReader din = new StreamReader(fs, System.Text.Encoding.Default);
            string str = "";
            while ((str = din.ReadLine()) != null)
            {
                Lines.Add(str);
            }
            return Lines;

        }
        #endregion

        #region 得到文本文件的数据
        /// <summary>
        /// 得到文本文件的数据
        /// 返回IList 每行一个字串
        /// Encoding为指定的Encoding
        /// </summary>
        /// <param name="FilePathAll"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public IList GetLinesFromTextFile(string FilePathAll, Encoding encoding)
        {
            IList Lines = new ArrayList();
            FileStream fs = new FileStream(FilePathAll, FileMode.Open, FileAccess.Read);
            StreamReader din = new StreamReader(fs, encoding);
            string str = "";
            while ((str = din.ReadLine()) != null)
            {
                Lines.Add(str);
            }
            return Lines;

        }
        #endregion

        #region create 存在就更新
        /// <summary>
        /// 创建文件, 如果文件已经存在, 则更新内容
        /// </summary>
        /// <param name="strFileName">文件名</param>
        /// <param name="strContent">文件内容</param>
        public static void Create(string strFileName, string strContent)
        {
            try
            {
                int iIndex = strFileName.LastIndexOf(@"\");
                if (iIndex > 0)
                {
                    CreateDirectory(strFileName.Substring(0, iIndex));
                }

                StreamWriter streamWriter = File.CreateText(strFileName);
                try
                {
                    streamWriter.Write(strContent);
                    streamWriter.Flush();
                }
                catch (Exception except)
                {
                    throw new Exception(String.Format("创建文件{0}出错\r\n{1}", strFileName, except.Message));
                }
                finally
                {
                    streamWriter.Close();
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region create 存在是否备份
        /// <summary>
        /// 创建文件, 如果已经存在,是否备份原文件(文件名后加.bak)
        /// </summary>
        /// <param name="strFileName">文件名</param>
        /// <param name="strContent">文件内容</param>
        /// <param name="bBackup">是否备份</param>
        public static void Create(string strFileName, string strContent, bool bBackup)
        {
            if (bBackup && File.Exists(strFileName))
            {
                Backup(strFileName);
            }
            Create(strFileName, strContent);
        }
        /// <summary>
        /// 备份指定文件为.bak
        /// </summary>
        /// <param name="strFileName">备份文件</param>
        private static void Backup(string strFileName)
        {
            try
            {
                File.Delete(strFileName + ".bak");
                File.Move(strFileName, strFileName + ".bak");
            }
            catch (Exception except)
            {
                throw new Exception(String.Format("备份指定文件{0}出错\r\n{1}", strFileName, except.Message));
            }
        }

        #endregion

        #region delete
        /// <summary>
        /// 删除指定文件
        /// </summary>
        /// <param name="strFileName">文件名称</param>
        private static void Delete(string strFileName)
        {
            try
            {
                File.Delete(strFileName);
            }
            catch (Exception except)
            {
                throw new Exception(String.Format("删除指定文件{0}出错\r\n{1}", strFileName, except.Message));
            }
        }

        #endregion

        #region 如果文件的目录不存在就建立目录
        /// <summary>
        /// 如果文件的目录不存在就建立目录
        /// </summary>
        /// <param name="strFileName"></param>
        public static void CreateDirectoryOfFileName(string strFileName)
        {
            try
            {
                int iIndex = strFileName.LastIndexOf(@"\");
                if (iIndex > 0)
                {
                    CreateDirectory(strFileName.Substring(0, iIndex));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //Console.WriteLine(strFileName);
                //Console.WriteLine(strContent);
            }
        }
        #endregion

        #region CreateDirectory
        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="strDirectoryName">文件夹名</param>
        public static void CreateDirectory(string strDirectoryName)
        {
            try
            {
                Directory.CreateDirectory(strDirectoryName);
            }
            catch (Exception except)
            {
                throw new System.Exception(String.Format("不能创建文件目录{0},出错为{1}", strDirectoryName, except.Message));
            }
        }
        #endregion

        #region ReadFileLines
        /// <summary>
        /// ReadFileLines
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private ArrayList ReadFileLines(string path)
        {
            StreamReader sr;
            ArrayList array = new ArrayList();
            sr = File.OpenText(path);
            string line = sr.ReadLine();
            while (line != null)
            {
                array.Add(line);
                line = sr.ReadLine();
            }
            sr.Close();
            return array;
        }
        #endregion

        #region 获取某目录下的所有文件(包括子目录下文件)的数量
        /// <summary>
        /// 获取某目录下的所有文件(包括子目录下文件)的数量
        /// </summary>
        /// <param name="srcPath"></param>
        /// <returns></returns>
        public int GetFileNum(string srcPath)
        {
            int fileNum = 0;
            try
            {

                // 得到源目录的文件列表，该里面是包含文件以及目录路径的一个数组
                string[] fileList = System.IO.Directory.GetFileSystemEntries(srcPath);
                // 遍历所有的文件和目录
                foreach (string file in fileList)
                {
                    // 先当作目录处理如果存在这个目录就重新调用GetFileNum(string srcPath)
                    if (System.IO.Directory.Exists(file))
                        GetFileNum(file);
                    else
                        fileNum++;
                }

            }
            catch
            {
                //MessageBox.Show (e.ToString());

            }
            return fileNum;
        }
        #endregion

        #region GetFilesCount
        /// <summary>
        /// GetFilesCount
        /// </summary>
        /// <param name="dirInfo"></param>
        /// <returns></returns>
        public static int GetFilesCount(System.IO.DirectoryInfo dirInfo)
        {
            //System.IO.DirectoryInfo dirInfo = new System.IO.DirectoryInfo(dirPath); 

            int totalFile = 0;
            totalFile += dirInfo.GetFiles().Length;
            foreach (System.IO.DirectoryInfo subdir in dirInfo.GetDirectories())
            {
                totalFile += GetFilesCount(subdir);
            }
            return totalFile;
        }
        #endregion

        #region GetFileName(string FileNamePath)
        /// <summary>
        /// 得到文件名
        /// </summary>
        /// <param name="FileNamePath">包含路径的文件名</param>
        /// <returns>不含路径的文件名</returns>
        public static string GetFileName(string FileNamePath)
        {
            int iIndex = FileNamePath.LastIndexOf(@"\");
            if (iIndex >= 0)
            {
                string fileName = FileNamePath.Substring(iIndex + 1);
                return fileName;
            }
            else
            {
                return FileNamePath;
            }
        }
        #endregion

        #region 返回文件是否存在
        /// <summary>
        /// 返回文件是否存在
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <returns>是否存在</returns>
        public static bool FileExists(string filename)
        {
            return System.IO.File.Exists(filename);
        }
        #endregion

        #region 判断文件名是否为浏览器可以直接显示的图片文件名
        /// <summary>
        /// 判断文件名是否为浏览器可以直接显示的图片文件名
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <returns>是否可以直接显示</returns>
        public static bool IsImgFilename(string filename)
        {
            filename = filename.Trim();
            if (filename.EndsWith(".") || filename.IndexOf(".") == -1)
            {
                return false;
            }
            string extname = filename.Substring(filename.LastIndexOf(".") + 1).ToLower();
            return (extname == "jpg" || extname == "jpeg" || extname == "png" || extname == "bmp" || extname == "gif");
        }
        #endregion

        #region 返回指定目录下的非 UTF8 字符集文件
        /// <summary>
        /// 返回指定目录下的非 UTF8 字符集文件
        /// </summary>
        /// <param name="Path">路径</param>
        /// <returns>文件名的字符串数组</returns>
        public static string[] FindNoUTF8File(string Path)
        {
            //System.IO.StreamReader reader = null;
            StringBuilder filelist = new StringBuilder();
            DirectoryInfo Folder = new DirectoryInfo(Path);
            //System.IO.DirectoryInfo[] subFolders = Folder.GetDirectories(); 
            /*
            for (int i=0;i<subFolders.Length;i++) 
            { 
                FindNoUTF8File(subFolders[i].FullName); 
            }
            */
            FileInfo[] subFiles = Folder.GetFiles();
            for (int j = 0; j < subFiles.Length; j++)
            {
                if (subFiles[j].Extension.ToLower().Equals(".htm"))
                {
                    FileStream fs = new FileStream(subFiles[j].FullName, FileMode.Open, FileAccess.Read);
                    bool bUtf8 = IsUTF8(fs);
                    fs.Close();
                    if (!bUtf8)
                    {
                        filelist.Append(subFiles[j].FullName);
                        filelist.Append("\r\n");
                    }
                }
            }
            return StringUtils.SplitString(filelist.ToString(), "\r\n");

        }
        #endregion

        #region 判断文件流是否为UTF8字符集
        /// <summary>
        /// 判断文件流是否为UTF8字符集
        /// </summary>
        /// <param name="sbInputStream">文件流</param>
        /// <returns>判断结果</returns>
        private static bool IsUTF8(FileStream sbInputStream)
        {
            int i;
            byte cOctets;  // octets to go in this UTF-8 encoded character 
            byte chr;
            bool bAllAscii = true;
            long iLen = sbInputStream.Length;

            cOctets = 0;
            for (i = 0; i < iLen; i++)
            {
                chr = (byte)sbInputStream.ReadByte();

                if ((chr & 0x80) != 0) bAllAscii = false;

                if (cOctets == 0)
                {
                    if (chr >= 0x80)
                    {
                        do
                        {
                            chr <<= 1;
                            cOctets++;
                        }
                        while ((chr & 0x80) != 0);

                        cOctets--;
                        if (cOctets == 0) return false;
                    }
                }
                else
                {
                    if ((chr & 0xC0) != 0x80)
                    {
                        return false;
                    }
                    cOctets--;
                }
            }

            if (cOctets > 0)
            {
                return false;
            }

            if (bAllAscii)
            {
                return false;
            }

            return true;

        }
        #endregion

        #region 格式化字节数字符串
        /// <summary>
        /// 格式化字节数字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string FormatBytesStr(int bytes)
        {
            if (bytes > 1073741824)
            {
                return ((double)(bytes / 1073741824)).ToString("0") + "G";
            }
            if (bytes > 1048576)
            {
                return ((double)(bytes / 1048576)).ToString("0") + "M";
            }
            if (bytes > 1024)
            {
                return ((double)(bytes / 1024)).ToString("0") + "K";
            }
            return bytes.ToString() + "Bytes";
        }
        #endregion

        #region 判断目录是否存在
        /// <summary>
        /// 判断目录是否存在
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool ExistsDirectory(string path)
        {
            if (System.IO.Directory.Exists(path))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region 删除目录
        /// <summary>
        /// 删除目录
        /// </summary>
        /// <param name="path"></param>
        public static void DeleteDirectory(string path)
        {
            System.IO.Directory.Delete(path);
        }
        #endregion

        #region 判断文件是否存在
        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool ExistsFile(string path)
        {
            if (File.Exists(path))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region 删除文件
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="path"></param>
        public static void DeleteFile(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region 保存为文本文件
        /// <summary>
        /// 保存为文本文件
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="content"></param>
        public static void SaveToTxtFile(string filename, string content)
        {
            using (System.IO.StreamWriter streamwriter = new StreamWriter(filename, false))
            {
                streamwriter.Write(content);
            }
        }
        #endregion

        #region 保存为文本文件并创建目录
        /// <summary>
        /// 保存为文本文件并创建目录
        /// </summary>
        /// <param name="path"></param>
        /// <param name="filename"></param>
        /// <param name="html"></param>
        public static void IOSaveTxtHtml(string path, string filename, string html)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            FileUtils.SaveToTxtFile((path.TrimEnd('\\') + @"\" + filename), html);
        }
        #endregion

        #region 读文本文件内容
        /// <summary>
        /// 读文本文件内容
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ReadIOTxtContent(string path)
        {
            string result = "";
            if (File.Exists(path))
            {
                using (System.IO.StreamReader streamreader = new StreamReader(path))
                {
                    result = streamreader.ReadToEnd();
                }
            }
            return result;
        }
        #endregion

    }
}
