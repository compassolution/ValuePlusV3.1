using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Com.ValuePlus.Common.Config;
using System.IO;
using Com.ValuePlus.Web;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Entity;
using System.Text;

public partial class Tools_ExecuteSql : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            this.RadioButtonList1.SelectedIndex = 0;
            this.trFile.Visible = true;
            this.trSql.Visible = true;
        }
    }

    protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (this.RadioButtonList1.SelectedIndex == 0)
        {
            this.trFile.Visible = true;
            //this.trSql.Visible = false;
        }
        else
        {
            this.trFile.Visible = false;
            //this.trSql.Visible = true;
        }
    }

    /// <summary>
    /// 执行操作
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Button1_Click(object sender, EventArgs e)
    {
        //this.txtResult.Text = "";
        //if (this.RadioButtonList1.SelectedIndex == 0)
        //{
        //    this.ExecuteSqlFile();
        //}
        //else
        //{
        //    this.ExecuteSql();
        //}
        this.ExecuteSql();
    }

    /// <summary>
    /// 显示sql文件内容
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void previewFile_Click(object sender, EventArgs e)
    {
        this.ShowSqlFileContent();
        this.txtResult.Text = "";
    }

    /// <summary>
    /// 显示sql文件内容
    /// </summary>
    private void ShowSqlFileContent()
    {
        try
        {
            String strLocalFile = this.File1.FileName.ToString();
            String strSqlFile = strLocalFile;

            if (!String.IsNullOrEmpty(strSqlFile))
            {
                //定义保存路径 
                string savePath = "ExcuteSql"; 
         
                //是否存在目录 
                if (!System.IO.Directory.Exists(Server.MapPath(savePath))) 
                { 
                    //不存在创建文件夹  
                    System.IO.Directory.CreateDirectory(Server.MapPath(savePath) ); 
                } 
                strSqlFile = Server.MapPath(savePath) + "\\" + strSqlFile;

                this.File1.SaveAs(strSqlFile); 

                FileStream stream = new FileStream(strSqlFile, FileMode.Open);
                StreamReader reader = new StreamReader(stream, System.Text.Encoding.Default);

                StringBuilder builder = new StringBuilder();
                StringBuilder builderAll = new StringBuilder();
                String strLine = "";
                while ((strLine = reader.ReadLine()) != null)
                {
                    builderAll.AppendLine(strLine);
                }

                reader.Close();
                reader.Dispose();
                stream.Close();
                stream.Dispose();

                this.txtSql.Text = builderAll.ToString();
            }

        }
        catch (Exception ex)
        {
            StringBuilder strB = new StringBuilder();
            strB.Append("--Get Sql scripts failed or Sql file is not exists！\r\n");
            strB.Append(ex.Message.ToString());
            this.txtResult.Text = strB.ToString();
        }
    }

    /// <summary>
    /// 执行界面输入的sql语句
    /// </summary>
    private void ExecuteSql()
    {
        try
        {
            if (!String.IsNullOrEmpty(this.txtSql.Text))
            {
                String strSql = this.txtSql.Text.Trim();

                string[] ContentLines = strSql.Split(new string[] { "\r\n" }, StringSplitOptions.None);//不忽略空行
                int iRowCount = ContentLines.Length;
                StringBuilder builder = new StringBuilder();
                bool bIsValid = false;
                for (int i = 0; i < iRowCount; i++)
                {
                    String strLine = ContentLines[i].ToString();
                    if (strLine.Trim().ToUpper() != @"GO")
                    {
                        builder.AppendLine(strLine);
                    }
                    else
                    {
                        SqlParamDao.ExecuteNonQueryBySql(builder.ToString());
                        builder.Remove(0, builder.Length);
                        bIsValid = true;
                    }
                }

                StringBuilder strB = new StringBuilder();
                //strB.Append("--操作时间:" + DateTime.Now.ToString() + "\r\n");
                strB.Append("--Execute Time:" + string.Format("{0:G}",DateTime.Now)+ "\r\n");
                if (bIsValid)
                {
                    strB.Append("--Execute Scripts Successfully!\r\n");
                    strB.Append(this.WriteOpLog(strSql, "1"));
                }
                else
                {
                    strB.Append("--Execute Scripts failed，the scripts format is invalid（GO）\r\n");
                    strB.Append(this.WriteOpLog(strSql, "0"));
                }
                this.txtResult.Text = strB.ToString();
            }
        }
        catch (Exception ex)
        {
            StringBuilder strB = new StringBuilder();
            strB.Append("--Execute Scripts failed，or the scripts format is invalid!\r\n");
            strB.Append(ex.Message.ToString());
            this.txtResult.Text = strB.ToString();
        }
    }

    /// <summary>
    /// 记录操作日志
    /// </summary>
    /// <param name="strSql"></param>
    /// <param name="strIsSuccess"></param>
    /// <returns></returns>
    private String WriteOpLog(String strSql,String strIsSuccess)
    {
        String strReturn = "";
        try
        {
            String strGUID = System.Guid.NewGuid().ToString();
            String strIp = Com.ValuePlus.Utils.RequestUtils.GetIP();
            UserInfo userInfo = this.GetUserInfo();
            String strUserId = userInfo.SUSERID;
            String strUserName = userInfo.SUSERNAME;
            String strUserNameCN = userInfo.SUSERNAMECN;
            DateTime dtNow = DateTime.Now;

            StringBuilder sbInsertSql = new StringBuilder();
            sbInsertSql.Append("insert into SQLLOG_1 (SLOGID,SUSERID,SUSERNAME,SUSERNAMECN,DTOPTIME,SIP,SSQL,BISSUCCESS) VALUES(");
            sbInsertSql.Append("'" + strGUID + "',");
            sbInsertSql.Append("'" + strUserId + "',");
            sbInsertSql.Append("'" + strUserName + "',");
            sbInsertSql.Append("'" + strUserNameCN + "',");
            sbInsertSql.Append("'" + dtNow + "',");
            sbInsertSql.Append("'" + strIp + "',");
            sbInsertSql.Append("'" + strSql.Replace("'", "\"").Replace("\r\n", "<br>") + "',");
            sbInsertSql.Append("'" + strIsSuccess + "')");

            int iCount = SqlParamDao.ExecuteNonQueryBySql(sbInsertSql.ToString());
            if (iCount > 0)
            {
                //strReturn = "--记录操作日志成功！";
                strReturn = "--Record log successfully！";
            }
            else
            {
                //strReturn = "--记录操作日志失败！";
                strReturn = "--Record log failed！";
            }
        }
        catch (Exception ex)
        {
            strReturn = ex.Message.ToString();
        }
        return strReturn;
    }

}
