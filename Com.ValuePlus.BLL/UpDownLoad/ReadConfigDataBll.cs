using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Com.ValuePlus.Entity.UpDownLoad;
using Com.ValuePlus.DAL;
using System.IO;

namespace Com.ValuePlus.BLL.UpDownLoad
{
    /// <summary>
    /// 上传下载相应配置数据读取类(从数据库中配置读取)
    /// </summary>
    public class ReadConfigDataBll
    {
        /// <summary>
        /// 日志声明
        /// </summary>
        protected static Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region 读取上传下载相应配置数据,存储在hashTable中
        /// <summary>
        /// 读取上传下载相应配置数据,存储在hashTable中
        /// </summary>
        /// <param name="strFolder"></param>
        /// <returns>Hashtable</returns>
        public static UpDownLoadXmlEntity GetUpLoadConfigData(String strFolder)
        {
            UpDownLoadXmlEntity entityUpDown = new UpDownLoadXmlEntity();
            try
            {
                Hashtable hsTable = Com.ValuePlus.SysParams.UpdownParamGetter.GetUpdownParams(strFolder);
                if ((hsTable != null) && (hsTable.Count > 0))
                {
                    entityUpDown.ID = hsTable["FID"].ToString();
                    entityUpDown.NAME = hsTable["FNAME"].ToString();
                    entityUpDown.NAME_CN = hsTable["FNAMECN"].ToString();
                    entityUpDown.DESC = hsTable["FDESC"].ToString();
                    entityUpDown.DESC_CN = hsTable["FDESCCN"].ToString();
                    entityUpDown.PATH = hsTable["FPATH"].ToString();
                    entityUpDown.ALLOWTYPE = hsTable["ALLOWTYPE"].ToString();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return entityUpDown;
        }
        #endregion

        #region 将路径中的文件列表写入数据库中
        /// <summary>
        /// 将路径中的文件列表写入数据库中
        /// </summary>
        /// <param name="entityXml"></param>
        public static void WriteUploadFileListToDB(UpDownLoadXmlEntity entityXml)
        {
            try
            {
                if ((entityXml != null) && (!String.IsNullOrEmpty(entityXml.PATH)))
                {
                    String strTableName = "UploadFileList_" + entityXml.ID.ToString();
                    StringBuilder sb = new StringBuilder();
                    sb.Append("IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[" + strTableName + "]') AND type in (N'U'))  ");
                    sb.Append("DROP TABLE [dbo].[" + strTableName + "]  ");
                    sb.Append("create table " + strTableName + " (sFileName varchar (1000), iLevel int , iIsFile int )  ");
                    //sb.Append("insert into " + strTableName + " exec master..xp_dirtree '" + entityXml.PATH + "',1,1 ");

                    if (Directory.Exists(entityXml.PATH))
                    {
                        DirectoryInfo folder = new DirectoryInfo(entityXml.PATH.ToString());
                        foreach (FileInfo file in folder.GetFiles("*.*"))
                        {
                            sb.Append("insert into " + strTableName + " (sFileName,iLevel,iIsFile) values('" + file.Name + "',1,1 )");
                        }
                    }

                    int iCount = SqlParamDao.ExecuteNonQueryBySql(sb.ToString());
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }
        #endregion

    }
}
