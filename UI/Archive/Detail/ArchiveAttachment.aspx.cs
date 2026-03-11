using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Com.ValuePlus.Web;
using Com.ValuePlus.Common;
using Com.ValuePlus.Archive.DAL;
using Com.ValuePlus.Archive.BLL;
using System.IO;
using System.Drawing;
using Com.ValuePlus.Common.Config;
using System.Resources;

public partial class Archive_Detail_ArchiveAttachment : ArchivePageBase
{
    protected String strAttFileRelaTivePath = BaseConfig.Instance.GetConfigValueByKey("PATH_ArchiveAttFile");//用户在模板明细页面中上传文件的相对路径

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            try
            {
                ////解密传递字符串并获取对应参数值
                Hashtable htUrlQuery = UrlParamEncryption.DecryptionUrlParam(base.Request.Url.Query.ToString());
                this.TID = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "TID");
                this.SID = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "SID");
                this.GID = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "GID");
                this.KEY = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "KEY");
                this.KEYVALUE = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "KEYVALUE");
                this.GRIDKEY = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "GRIDKEY");
                this.GRIDKEYVALUE = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "GRIDKEYVALUE");
                this.strCurPID = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "PID");
                this.OPTYPE = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "OPTYPE");
                this.strFilePath = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "path");
                this.strFilePath = strAttFileRelaTivePath + "/" + this.strFilePath;
                
                this.FileUpload1.Visible = false;
                this.btnOK.Visible = false;

                if (this.Language.Equals("zh-cn"))
                {
                    this.Page.Title = Session["ArchiveDesc"] + "附件列表";
                }
                else
                {
                    this.Page.Title = Session["ArchiveDesc"] + " Attachment List";
                }

                ResourceManager rmLocResourceManager = base.GetResourceManager("UpDownLoad");
                this.hf_strSureDelete.Value = rmLocResourceManager.GetString("lbSureDelete");
                this.hf_strDeleteSuccess.Value = rmLocResourceManager.GetString("lbDeleteSuccess");
                this.hf_strLbFile.Value = rmLocResourceManager.GetString("lbFile");

                //在存在主键且是编辑状态下可以上传
                if ((this.OPTYPE.ToLower().Equals("edit")) || (this.OPTYPE.ToLower().Equals("add")))
                {
                    if (!String.IsNullOrEmpty(this.GRIDKEY))
                    {
                        //非列表分组界面时
                        if (!String.IsNullOrEmpty(this.GRIDKEYVALUE))
                        {
                            this.FileUpload1.Visible = true;
                            this.btnOK.Visible = true;
                        }
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(this.KEYVALUE))
                        {
                            this.FileUpload1.Visible = true;
                            this.btnOK.Visible = true;
                        }
                    }

                }

                this.GetLimitedConfigInfo();
                this.strFileList = this.GetAttachmentListFromDB(this.TID,this.GID,this.KEY,this.KEYVALUE,this.GRIDKEY,this.GRIDKEYVALUE,this.strCurPID);
                this.CreateAttachRows(this.strFileList);
                this.FileUpload1.Width = Unit.Percentage(50);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                base.AlertMessageBox(this, "Load Attachment Error！");
            }
        }
    }

    #region viewstate初始化区域
    private int iLimitedCount
    {
        get
        {
            if (ViewState["iLimitedCount"] != null)
            {
                return (int)ViewState["iLimitedCount"];
            }
            else
            {
                return 1000;
            }
        }
        set
        {
            ViewState["iLimitedCount"] = value;
        }
    }
    private int iFileCount
    {
        get
        {
            if (ViewState["iFileCount"] != null)
            {
                return (int)ViewState["iFileCount"];
            }
            else
            {
                return 0;
            }
        }
        set
        {
            ViewState["iFileCount"] = value;
        }
    }
    private string strCurPID
    {
        get
        {
            return ViewState["strPID_ViewState"] as string;
        }
        set
        {
            ViewState["strPID_ViewState"] = value;
        }
    }
    private string strFilePath
    {
        get
        {
            return ViewState["strCurTID_ViewState"] as string;
        }
        set
        {
            ViewState["strCurTID_ViewState"] = value;
        }
    }
    private string strFileList
    {
        get
        {
            return ViewState["strFileList_ViewState"] as string;
        }
        set
        {
            ViewState["strFileList_ViewState"] = value;
        }
    }
    private string strCurTableName
    {
        get
        {
            return ViewState["strCurTableName_ViewState"] as string;
        }
        set
        {
            ViewState["strCurTableName_ViewState"] = value;
        }
    }
    #endregion

    #region 数据库操作

    /// <summary>
    ///  获取字段配置中的上传文件个数上限
    /// </summary>
    private void GetLimitedConfigInfo()
    {
        DataTable dt = new DataTable();
        string strSql = "select * from TB_HRTMPSD where  TID='" + this.TID + "' and SID = '" + this.SID + "' and PID = '" + this.strCurPID + "'";
        dt = SqlParamDao.GetDataTableBySql(strSql);
        if (dt != null && dt.Rows.Count > 0)
        {
            String strLimit = dt.Rows[0]["PDEFAULT"].ToString();
            try
            {
                this.iLimitedCount = int.Parse(strLimit);
            }
            catch (Exception ex)
            {
                this.iLimitedCount = int.Parse("1000"); 
            }
        }
    }

    /// <summary>
    /// 获取附件列表信息
    /// </summary>
    /// <param name="strTID"></param>
    /// <param name="strGID"></param>
    /// <param name="strKey"></param>
    /// <param name="strKeyValue"></param>
    /// <param name="strGridKey"></param>
    /// <param name="strGridKeyValue"></param>
    /// <param name="strPID"></param>
    /// <returns></returns>
    private String GetAttachmentListFromDB(String strTID, String strGID, String strKey,String strKeyValue, String strGridKey, String strGridKeyValue, String strPID)
    {
        String strFileList = "";
        String strTableName = strTID + "_" + strGID;
        this.strCurTableName = strTableName;
        DataTable dt = new DataTable();
        string strSql = "select * from " + strTableName + " where  " + strKey + "='" + strKeyValue + "'";
        if (!String.IsNullOrEmpty(strGridKey))
        {
            strSql = strSql + " and "+ strGridKey +"='"+ strGridKeyValue + "'";
        }
        try
        {
            dt = SqlParamDao.GetDataTableBySql(strSql);
            if (dt != null && dt.Rows.Count > 0)
            {
                strFileList = dt.Rows[0][strPID].ToString();
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
            log.Error("\r\n");
            log.Error("获取附件列表信息的操作失败，SQL：" + strSql);
        }
        return strFileList;
    }

    /// <summary>
    /// 更新附件列表字符串到数据库中
    /// </summary>
    /// <param name="strTableName"></param>
    /// <param name="strKey"></param>
    /// <param name="strKeyValue"></param>
    /// <param name="strGridKey"></param>
    /// <param name="strGridKeyValue"></param>
    /// <param name="strPID"></param>
    /// <param name="strList"></param>
    /// <returns></returns>
    private void UpdateAttachmentList(String strTableName, String strKey, String strKeyValue, String strGridKey, String strGridKeyValue, String strPID,String strList)
    {
        string strSql = "update " + strTableName + " set "+strPID+"='" + strList + "' where  " + strKey + "='" + strKeyValue + "'";
        if (!String.IsNullOrEmpty(strGridKey))
        {
            strSql = strSql + " and " + strGridKey + "='" + strGridKeyValue + "'";
        }
        try
        {
            int iCount = 0;
            Object obj = SqlParamDao.ExecuteNonQueryBySql(strSql);
            if (obj != null)
            {
                iCount = Convert.ToInt32(obj);
                if (iCount <= 0)
                {
                    this.AlertMessageBox(this.Page,"上传更新失败！");
                }
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
            log.Error("\r\n");
            log.Error("更新附件列表字符串到数据库中的操作失败，SQL：" + strSql);
        }
    }

    #endregion

    /// <summary>
    /// 创建附件文件列表
    /// </summary>
    /// <param name="strFileList"></param>
    private void CreateAttachRows(string strFileList)
    {
        if (!String.IsNullOrEmpty(strFileList))
        {
            this.divFileList.Controls.Clear();
            string[] strArray = strFileList.Split(new char[] { ";".ToCharArray()[0] });
            for (int i = 0; i < strArray.Length; i++)
            {
                this.AddAttachRow(strArray[i].ToString(), strFileList);
            }
            this.iFileCount = strArray.Length;
        }
        else
        {
            this.iFileCount = 0;
        }
    }

    /// <summary>
    /// 添加一条附件记录到页面
    /// </summary>
    /// <param name="strFileName"></param>
    private void AddAttachRow(String strFileName,String strFileList)
    {
        TableRow row = new TableRow();
        //文件列
        TableCell cellFile = new TableCell();
        cellFile.HorizontalAlign = HorizontalAlign.Left;
        cellFile.Width = Unit.Percentage(94);
        cellFile.Controls.Add(new LiteralControl(strFileName));
        //判断文件是否存在
        String strFile = this.Page.MapPath(this.strFilePath) + "\\" + strFileName;
        if (!File.Exists(strFile))
        {
            Label lb = new Label();
            lb.Text = "(The file may be removed!)";
            lb.ForeColor = Color.Red;
            cellFile.Controls.Add(lb);
        }
        //查看列
        TableCell cellView = new TableCell();
        ImageButton childView = new ImageButton();
        childView.ImageUrl = "../../common/images/icon/icon_query1.gif";
        childView.ToolTip = "View";
        //String strFilePathAndName = this.strFilePath.Replace("../","").Replace("./","") + "/" + strFileName;
        //strFilePathAndName = String.Format(this.GetSiteSchema()+"://{0}/", Com.ValuePlus.Utils.RequestUtils.GetCurrentFullHost()) + strFilePathAndName;

        //childView.Attributes.Add("onclick", "return ShowFile(this,'');");

        if (String.IsNullOrEmpty(this.strFilePath))
        {
            childView.Attributes.Add("onclick", "return ShowFile(this,'');");
        }
        else
        {
            ////根据文件名判断是否是图片类型文件，此处增加几个参数传到第客户端 add by sammen 20220208
            childView.Attributes.Add("onclick", "return ShowFile(this,'" + this.strFilePath + "/','" + strFileName + "','" + strFileList + "');");
        }

        cellView.Controls.Add(childView);
        cellView.Width = Unit.Percentage(3);
        //删除列
        TableCell cellDel = new TableCell();
        ImageButton childDel = new ImageButton();
        childDel.ImageUrl = "../../common/images/icon/icon-delete.gif";
        childDel.ID = "del_" + strFileName;
        childDel.ToolTip = "Delete";
        //childDel.Click += new ImageClickEventHandler(this.Delete_Click);
        childDel.Attributes.Add("onclick", "DeleteFile('" + strFileName + "'); return false;");
        cellDel.Controls.Add(childDel);
        cellDel.Width = Unit.Percentage(3);

        row.Cells.Add(cellFile);
        row.Cells.Add(cellView);
        if ((!String.IsNullOrEmpty(this.OPTYPE)) && (!this.OPTYPE.ToLower().Equals("readonly")))
        {
            row.Cells.Add(cellDel);
        }
        this.divFileList.Controls.Add(row);
    }


    #region 按钮点击操作
    /// <summary>
    /// 关闭操作(测试用)
    /// </summary>
    protected void aClose_Click(object sender, EventArgs e)
    {
        String strParam = "TID=HRTRNP&GID=1&KEY=PSEQ&KEYVALUE=100900013&PID=TDETAIL&path=../../common/temp";
        String strUrl = "ArchiveAttachment.aspx?" + UrlParamEncryption.EncryptionUrlParam(strParam);

        Response.Redirect(strUrl, false);
    }
    /// <summary>
    /// 确定操作
    /// </summary>
    protected void OK_Click(object sender, EventArgs e)
    {
        Boolean bIsContinue = true;
        try
        {
            Object fileUpload = this.FileUpload1.PostedFile;
        }
        catch (Exception ex)
        {
            bIsContinue = false;
            log.Error(ex);
            base.AlertMessageBox(this, "Error File Name can not Contain special character！文件名不能保存有风险的特殊字符");
            return;
        }

        if (bIsContinue && this.FileUpload1.PostedFile != null)
        {
            string strFileName = this.FileUpload1.PostedFile.FileName;
            //add by sammen 20220125 上传文件名中特殊字符的处理
            strFileName = strFileName.Replace("&", "＆");
            strFileName = strFileName.Replace("%", "％");
            strFileName = strFileName.Replace("+", "＋");
            strFileName = strFileName.Replace("#", "＃");
            strFileName = strFileName.Replace(";", "；");
            //strFileName = strFileName.Replace(".", "。");
            if (!JudgeIsCanUpload())
            {
                base.AlertMessageBox(this.Page, "You had upload too much files！The limited:" + this.iLimitedCount.ToString());
                this.CreateAttachRows(this.strFileList);
                return;
            }
            //判断是否设置上传路径
            if (String.IsNullOrEmpty(this.strFilePath))
            {
                base.AlertMessageBox(this.Page, "未设置上传文件存放路径，请联系系统管理员！");
                this.CreateAttachRows(this.strFileList);
                return;
            }
            //首先判断是否选择了文件
            if (String.IsNullOrEmpty(strFileName))
            {
                this.CreateAttachRows(this.strFileList);
                return;
            }
            int startIndex = 0;
            if (strFileName.LastIndexOf(@"\") > 0)
            {
                startIndex = strFileName.LastIndexOf(@"\");
                strFileName = strFileName.Substring(startIndex + 1);
            }
            //判断此文件名在数据库中是否存在
            if (this.IsExistFile(strFileName))
            {
                base.AlertMessageBox(this.Page,"The file you had Uploaded!");
                this.CreateAttachRows(this.strFileList);
            }
            else
            {
                //fileName = this.Page.MapPath(this.HomeDir) + fileName.Substring(startIndex);
                String strServerFilePath = this.Page.MapPath(this.strFilePath);
                String strFilePathAndName = strServerFilePath + "\\" + strFileName;
                if (!Directory.Exists(strServerFilePath))
                {
                    Directory.CreateDirectory(strServerFilePath);
                }
                //如果存在文件则覆盖
                if (File.Exists(strFilePathAndName))
                {
                    base.AlertMessageBox(this.Page, "The file is Exsit!Will be Recover it!");
                    File.Delete(strFilePathAndName);
                }
                
                try
                {
                    this.FileUpload1.PostedFile.SaveAs(strFilePathAndName);
                    if (this.strFileList.Length > 0)
                    {
                        this.strFileList = this.strFileList + ";";
                    }
                    this.strFileList = this.strFileList + strFileName;
                    this.UpdateAttachmentList(this.strCurTableName, this.KEY, this.KEYVALUE,this.GRIDKEY,this.GRIDKEYVALUE, this.strCurPID, this.strFileList);
                    this.CreateAttachRows(this.strFileList);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    base.AlertMessageBox(this, "UpLoad Attachment Error！");
                    return;
                }
            }
        }
    }

    /// <summary>
    /// 删除某一附件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Delete_Click(object sender, EventArgs e)
    {
        try
        {
            string strFileName = this.hfDeleteFile.Value;
            if (!String.IsNullOrEmpty(strFileName))
            {
                string strPathAName = this.Page.MapPath(this.strFilePath) + @"\" + strFileName;
                if (File.Exists(strPathAName))
                {
                    File.Delete(strPathAName);
                }
                else
                {
                    base.AlertMessageBox(this, "The file may be removed!Delete Failed！");
                    this.UpdateAttachmentList(this.strCurTableName, this.KEY, this.KEYVALUE, this.GRIDKEY, this.GRIDKEYVALUE, this.strCurPID, this.strFileList);
                    this.CreateAttachRows(this.strFileList);
                }
                if (!String.IsNullOrEmpty(this.strFileList))
                {
                    this.strFileList = this.strFileList.Replace(";" + strFileName, "").Replace(strFileName, "");
                    if (this.strFileList.StartsWith(";"))
                    {
                        this.strFileList = this.strFileList.Substring(1, this.strFileList.Length-1);
                    }
                    this.UpdateAttachmentList(this.strCurTableName, this.KEY, this.KEYVALUE, this.GRIDKEY, this.GRIDKEYVALUE, this.strCurPID, this.strFileList);
                }
                this.CreateAttachRows(this.strFileList);
            }
            else
            {
                this.CreateAttachRows(this.strFileList);
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
            base.AlertMessageBox(this, "Delete Attachment Error！");
            return;
        }
    }

    /// <summary>
    /// 判断某文件在数据库中是否存在
    /// </summary>
    /// <param name="filename"></param>
    /// <returns></returns>
    private bool IsExistFile(string filename)
    {
        bool bIs = false;
        if (!String.IsNullOrEmpty(this.strFileList))
        {
            string[] strArray = this.strFileList.Split(new char[] { ";".ToCharArray()[0] });
            for (int i = 0; i < strArray.Length; i++)
            {
                if (strArray[i] == filename)
                {
                    bIs = true;
                }
            }
        }
        return bIs;
    }

    #endregion

    /// <summary>
    /// 根据附件个数字段（PDEFAULT）的配置判断是否可以继续上传附件
    /// </summary>
    /// <returns></returns>
    private bool JudgeIsCanUpload()
    {
        bool bIsCan = true;
        if (this.iFileCount >= this.iLimitedCount)
        {
            bIsCan = false;
        }
        return bIsCan;
    }

}
