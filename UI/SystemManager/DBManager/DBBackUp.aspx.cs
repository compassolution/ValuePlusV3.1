using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Com.ValuePlus.Web;
using Com.ValuePlus.DAL;
using Com.ValuePlus.BLL.Report;
using Ionic.Zip;
using System.Text;

public partial class SystemManager_DBManager_DBBackUp : PageBase
{
    protected String strFileRelaTivePath = "../../DB/BackUp";//数据库备份文件相对路径

    protected void Page_Load(object sender, EventArgs e)
    {
        this.btnBackUp.Attributes.Add("onclick", "javascript:if(confirm('Are you Sure?')){return true;}else{return false;}");
        //this.btnBackUp.Attributes.Add("onclick","if(confirm('Are you sure?') return  ture;)");
    }

    #region 备份当前数据库
    /// <summary>
    /// 备份当前数据库
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnBackUp_Click(object sender, EventArgs e)
    {
        try
        {
            DBConSortStr dbConnection = new DBConSortStr();
            String strDbDatabase = dbConnection.Database;

            String strFilePath = base.MapPath(strFileRelaTivePath);
            if (!Directory.Exists(strFilePath))
            {
                Directory.CreateDirectory(strFilePath);
            }
            strFilePath = strFilePath.Replace("\\", "/") + "/";
            String strTime = string.Format("{0:yyyyMMddHHmmss}", DateTime.Now);
            String strFileName = strDbDatabase + "_" + strTime + ".bak";
            String strPathAndFileName = strFilePath + strFileName;

            String strSqlBuckUp = @"backup database " + strDbDatabase + " to disk='" + strPathAndFileName+"'";
            strSqlBuckUp = strSqlBuckUp + " WITH NOFORMAT, NOINIT,  NAME = N'ValuePlus-Full-Backup', SKIP, NOREWIND, NOUNLOAD,  STATS = 10";
            //备份数据库到指定文件
            int iCount = SqlParamDao.ExecuteNonQueryBySql(strSqlBuckUp);

            //压缩数据库备份文件
            try
            {
                //待压缩文件
                String fileToZip = strPathAndFileName;
                //想要压成zip的文件名
                String zipedFile = strDbDatabase + "_" + strTime + ".zip";
                String zipedPathAndFile = strFilePath + zipedFile;


                //log.Error("压缩数据库备份文件源文件：" + strPathAndFileName);
                //log.Error("压缩数据库备份文件目标文件：" + zipedPathAndFile);

                if (System.IO.File.Exists(zipedPathAndFile))
                {
                    System.IO.File.Delete(zipedPathAndFile);
                }
                using (ZipFile zip = new ZipFile(zipedFile))
                {
                    Directory.SetCurrentDirectory(strFilePath);
                    zip.AddFile(fileToZip);
                    zip.Save();
                }
                //删除备份文件
                System.IO.File.Delete(fileToZip);

            }
            catch (Exception ex)
            {
                log.Error("压缩数据库备份文件时出错：" + ex);
            }

            this.AlertMessageBox(this.Page, "Backup successed!");
            Response.Redirect("DBBackUp.aspx", false);
        }
        catch (Exception ex)
        {
            log.Error(ex);
            this.AlertMessageBox(this.Page, "Backup Failed!");
        }
    }
    #endregion


}
