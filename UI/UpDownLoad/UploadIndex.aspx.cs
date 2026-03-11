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
using Com.ValuePlus.Common.Security;

public partial class UpDownLoad_UploadIndex : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {

        //设置最大上传文件大小
        this.SetUploadFileSize();
        if (!Page.IsPostBack)
        {
            //设置中英文
            ResourceManager rm = base.GetResourceManager("UpDownLoad");
            //this.Uploader1.InsertText = rm.GetString("btnInsertText");
            //this.Uploader1.CancelText = rm.GetString("btnCancelText");
            //this.Uploader1.CancelAllMsg = rm.GetString("msgCancelAllFile");
            //this.Uploader1.CancelUploadMsg = rm.GetString("msgCancelUploadMsg");
            //this.Uploader1.FileTooLargeMsg = rm.GetString("msgFileTooLarge");
            //this.Uploader1.UploadingMsg = rm.GetString("msgUploading");
            this.SubmitButton.Text = rm.GetString("btnUpload");
            this.strMsgSuccessUp = rm.GetString("msgSuccessUp");
            this.hfPleaseSelectFile.Value = rm.GetString("msgPleaseSelectFile");

            try
            {
                if (Request.Params["folder"] != null)
                {
                    this.strFileFolder = Request.Params["folder"].ToString();
                    //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
                    this.strFileFolder = SQLInjectionDefense.ReplaceSQLReservedKeyword(this.strFileFolder);

                    if (this.strFileFolder.ToUpper().Equals("AIMAGE"))
                    {
                        this.btnQueryAssetsImageName.Visible = true;
                        this.btnInitAssetsImageData.Visible = true;
                        this.btnQueryOEImageName.Visible = false;
                        this.btnInitOEImageData.Visible = false;
                    }
                    else  if (this.strFileFolder.ToUpper().Equals("OEIMAGE"))
                    {
                        this.btnQueryAssetsImageName.Visible = false;
                        this.btnInitAssetsImageData.Visible = false;
                        this.btnQueryOEImageName.Visible = true;
                        this.btnInitOEImageData.Visible = true;
                    }
                    else
                    {
                        this.btnQueryOEImageName.Visible = false;
                        this.btnInitAssetsImageData.Visible = false;
                        this.btnQueryAssetsImageName.Visible = false;
                        this.btnInitOEImageData.Visible = false;
                    }

                    ////获取配置文件信息
                    //this.hsXmlConfigInfo = this.GetXmlFileConfig();
                    //if ((this.hsXmlConfigInfo != null) && (this.hsXmlConfigInfo.Count > 0))
                    //{
                    //    if (this.hsXmlConfigInfo.ContainsKey(this.strFileFolder))
                    //    {
                    //        this.entityXml = (UpDownLoadXmlEntity)this.hsXmlConfigInfo[this.strFileFolder];
                    //        if (this.Language.Equals("en-us"))
                    //        {
                    //            this.lbTitle.Text = rm.GetString("lbUpLoadTitle").Replace("{0}", this.entityXml.NAME);
                    //        }
                    //        else
                    //        {
                    //            this.lbTitle.Text = rm.GetString("lbUpLoadTitle").Replace("{0}", this.entityXml.NAME_CN);
                    //        }
                    //        this.strFilePath = this.entityXml.PATH;
                    //        this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "", "<script language=javascript>redirectFrame('" + this.strFileFolder + "')</script>");
                    //    }
                    //    else
                    //    {
                    //        base.AlertMessageBox(this.Page, rm.GetString("msgNoNodeErr") + this.strFileFolder);
                    //        this.Uploader1.Visible = false;
                    //        this.SubmitButton.Visible = false;
                    //        return;
                    //    }
                    //}
                    //else
                    //{
                    //    base.AlertMessageBox(this.Page, rm.GetString("msgUploadPathErr"));
                    //    this.Uploader1.Visible = false;
                    //    this.SubmitButton.Visible = false;
                    //    return;
                    //}

                    this.entityXml = ReadConfigDataBll.GetUpLoadConfigData(this.strFileFolder);
                    if (this.entityXml != null)
                    {
                        if (this.Language.Equals("en-us"))
                        {
                            this.lbTitle.Text = rm.GetString("lbUpLoadTitle").Replace("{0}", this.entityXml.NAME);
                        }
                        else
                        {
                            this.lbTitle.Text = rm.GetString("lbUpLoadTitle").Replace("{0}", this.entityXml.NAME_CN);
                        }
                        this.strFilePath = this.entityXml.PATH;
                        this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "", "<script language=javascript>redirectFrame('" + this.strFileFolder + "')</script>");
                    }

                }
                else
                {
                    base.AlertMessageBox(this.Page, rm.GetString("msgUploadParamErr"));
                    //this.Uploader1.Visible = false;
                    this.SubmitButton.Visible = false;
                    return;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
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
    private string strFilePath
    {
        get
        {
            return ViewState["strFilePath"] as string;
        }
        set
        {
            ViewState["strFilePath"] = value;
        }
    }
    private string strMsgSuccessUp
    {
        get
        {
            return ViewState["strMsgSuccessUp"] as string;
        }
        set
        {
            ViewState["strMsgSuccessUp"] = value;
        }
    }
    private Hashtable hsXmlConfigInfo
    {
        get
        {
            if (this.ViewState["hsXmlConfigInfo"] == null)
            {
                return new Hashtable();
            }
            return (Hashtable)this.ViewState["hsXmlConfigInfo"];
        }
        set
        {
            this.ViewState["hsXmlConfigInfo"] = value;
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

    #region 设置最大上传文件大小
    /// <summary>
    /// 设置最大上传文件大小
    /// </summary>
    private void SetUploadFileSize()
    {
        String strFileMaxSize = Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("UploadFileMaxSize");
        if (!String.IsNullOrEmpty(strFileMaxSize))
        {
            //this.Uploader1.MaxFilesLimit = int.Parse(strFileMaxSize);
            //this.Uploader1.ValidateOption.MaxSizeKB = int.Parse(strFileMaxSize);
        }
        else
        {
            //this.Uploader1.MaxFilesLimit = int.Parse("10240");
            //this.Uploader1.ValidateOption.MaxSizeKB = int.Parse("10240");
        }
    }
    #endregion

    #region 读取上传下载相应xml配置文件(作废)
    ///// <summary>
    ///// 读取上传下载相应xml配置文件
    ///// </summary>
    ///// <returns>Hashtable</returns>
    //private Hashtable GetXmlFileConfig()
    //{
    //    String strPath = Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("UpDownloadConfigFile");
    //    String strXmlFilePathAndName = Server.MapPath(strPath);

    //    Hashtable hsTable = new Hashtable();
    //    try
    //    {
    //        if (Session["sessionUpDownLoadConfig"] != null)
    //        {
    //            hsTable = (Hashtable)Session["sessionUpDownLoadConfig"];
    //        }
    //        else
    //        {
    //            if (File.Exists(strXmlFilePathAndName))
    //            {
    //                hsTable = ReadConfigXmlBll.ReadXmlFile(strXmlFilePathAndName);
    //                if (hsTable != null)
    //                {
    //                    Session["sessionUpDownLoadConfig"] = hsTable;
    //                }
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        log.Error(ex);
    //    }
    //    return hsTable;
    //}
    #endregion

    protected void InsertMsg(string msg)
    {
        ListBoxEvents.Items.Insert(0, msg);
        ListBoxEvents.SelectedIndex = 0;
    }

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        Uploader1.FileUploaded += new UploaderEventHandler(Uploader_FileUploaded);
    }

    protected void ButtonPostBack_Click(object sender, EventArgs e)
    {
        //InsertMsg("您已经单击了一个回调按钮！");
    }

    protected void SubmitButton_Click(object sender, EventArgs e)
    {
        //InsertMsg("您已经单击了一个回调按钮！");
        //InsertMsg("您已经上传了 " + uploadcount + "/" + Uploader1.Items.Count + " 文件.");
    }

    int uploadcount = 0;

    protected void Uploader_FileUploaded(object sender, UploaderEventArgs args)
    {
        try
        {
            if (!Directory.Exists(this.strFilePath))
            {
                Directory.CreateDirectory(this.strFilePath);
            }

            uploadcount++;

            Uploader uploader = (Uploader)sender;
            InsertMsg(this.strMsgSuccessUp +"--->"+ args.FileName + ", " + args.FileSize + " bytes.");

            args.CopyTo(this.strFilePath.TrimEnd('\\') + @"\" + args.FileName);
            args.Delete();

            //将路径中的文件列表写入数据库中
            ReadConfigDataBll.WriteUploadFileListToDB(this.entityXml);

            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "", "<script language=javascript>redirectFrame('" + this.strFileFolder + "')</script>");
        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
    }

    #region 批量初始化图片数据
    /// <summary>
    /// 批量初始化资产图片数据
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void InitAssetsImageData_Click(object sender, EventArgs e)
    {
        StringBuilder sbReturnScript = new StringBuilder();
        try
        {
            Hashtable hsTableParam = new Hashtable();
            SqlParamDao.ExcuteSPReturnStr("USP_AM_InsertAssetImage_Batch", hsTableParam);
            //SqlParamDao.ExecuteNonQueryBySql("exec USP_AM_InsertAssetImage_Batch");
            sbReturnScript.Append("alert('初始化图片数据成功');");
        }
        catch (Exception ex)
        {
            log.Error(ex);
            sbReturnScript.Append("alert('初始化图片数据失败');");
        }
        finally
        {
            sbReturnScript.Append("redirectFrame('" + this.strFileFolder + "')");
            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "", "<script language=javascript>" + sbReturnScript.ToString() + "</script>");
        }
    }

    /// <summary>
    /// 批量初始化OE图片数据
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void InitOEImageData_Click(object sender, EventArgs e)
    {
        StringBuilder sbReturnScript = new StringBuilder();
        try
        {
            Hashtable hsTableParam = new Hashtable();
            SqlParamDao.ExcuteSPReturnStr("USP_OE_InsertOEImage_Batch", hsTableParam);
            //SqlParamDao.ExecuteNonQueryBySql("exec USP_OE_InsertOEImage_Batch");
            sbReturnScript.Append("alert('初始化图片数据成功');");
        }
        catch (Exception ex)
        {
            log.Error(ex);
            sbReturnScript.Append("alert('初始化图片数据失败');");
        }
        finally
        {
            sbReturnScript.Append("redirectFrame('" + this.strFileFolder + "')");
            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "", "<script language=javascript>"+ sbReturnScript.ToString() + "</script>");
        }
    }
    #endregion

    protected override void OnPreRender(EventArgs e)
    {
        //SubmitButton.Attributes["itemcount"] = Uploader1.Items.Count.ToString();

        base.OnPreRender(e);
    }

}
