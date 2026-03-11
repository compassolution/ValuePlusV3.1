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
using System.IO;
using System.Text;
using System.Xml;
using Com.ValuePlus.BLL.UpDownLoad;
using Com.ValuePlus.Entity.UpDownLoad;
using Com.ValuePlus.Common;
using Com.ValuePlus.Common.Security;

public partial class UpDownLoad_DownFileList : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            //设置中英文
            ResourceManager rm = base.GetResourceManager("UpDownLoad");
            this.lbTitle.Text = rm.GetString("lbExsitFileList");
            this.strTipDownload = rm.GetString("btnDownload");

            this.hf_strSureDelete.Value = rm.GetString("lbSureDelete");
            this.hf_strDeleteSuccess.Value = rm.GetString("lbDeleteSuccess");
            this.hf_strLbFile.Value = rm.GetString("lbFile");

            try
            {
                if (Request.Params["folder"] != null)
                {
                    if (Request.Params["edit"] != null)
                    {
                        this.strIsEdit = Request.Params["edit"].ToString();
                    }
                    this.strFileFolder = Request.Params["folder"].ToString();

                    //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
                    this.strFileFolder = SQLInjectionDefense.ReplaceSQLReservedKeyword(this.strFileFolder);
                    this.strIsEdit = SQLInjectionDefense.ReplaceSQLReservedKeyword(this.strIsEdit);

                    ////获取配置文件信息
                    //this.hsXmlConfigInfo = this.GetXmlFileConfig();
                    //if ((this.hsXmlConfigInfo != null) && (this.hsXmlConfigInfo.Count > 0))
                    //{
                    //    if (this.hsXmlConfigInfo.ContainsKey(strFileFolder))
                    //    {
                    //        this.entityXml = (UpDownLoadXmlEntity)this.hsXmlConfigInfo[strFileFolder];
                    //        if (this.Language.Equals("en-us"))
                    //        {
                    //            this.lbTitle.Text = rm.GetString("lbDownLoadTitle").Replace("{0}", this.entityXml.NAME);
                    //        }
                    //        else
                    //        {
                    //            this.lbTitle.Text = rm.GetString("lbDownLoadTitle").Replace("{0}", this.entityXml.NAME_CN);
                    //        }
                    //        this.strFilePath = this.entityXml.PATH;
                    //        // 得到源目录的文件列表，该里面是包含文件以及目录路径的一个数组
                    //        this.GetDownloadFileList(this.strFilePath);
                    //    }
                    //    else
                    //    {
                    //        base.AlertMessageBox(this.Page, rm.GetString("msgNoNodeErr") + strFileFolder);
                    //        return;
                    //    }
                    //}
                    //else
                    //{
                    //    base.AlertMessageBox(this.Page, rm.GetString("msgUploadPathErr"));
                    //    return;
                    //}

                    this.entityXml = ReadConfigDataBll.GetUpLoadConfigData(strFileFolder);
                    if (this.entityXml != null)
                    {
                        if (this.Language.Equals("en-us"))
                        {
                            this.lbTitle.Text = rm.GetString("lbDownLoadTitle").Replace("{0}", this.entityXml.NAME);
                        }
                        else
                        {
                            this.lbTitle.Text = rm.GetString("lbDownLoadTitle").Replace("{0}", this.entityXml.NAME_CN);
                        }
                        this.strFilePath = this.entityXml.PATH;
                        // 得到源目录的文件列表，该里面是包含文件以及目录路径的一个数组
                        this.GetDownloadFileList(this.strFilePath);

                        //将路径中的文件列表写入数据库中
                        ReadConfigDataBll.WriteUploadFileListToDB(this.entityXml);
                    }

                    //add by sammane by sammen 20230328
                    //新增文件操作token,防止恶意操作，UpDownLoad/DownLoadFile.aspx.cs中进行解析
                    this.strToken = this.strFileFolder + "＃" + DateTime.Now.ToString("yyyyMMddHHmmss");
                    this.strToken = Microsoft.JScript.GlobalObject.encodeURIComponent(this.strToken);//加密
                    //this.strToken = UrlParamEncryption.Encrypt3des(this.strToken, System.Text.Encoding.UTF8);//加密
                }
                else
                {
                    base.AlertMessageBox(this.Page, rm.GetString("msgUploadParamErr"));
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
    public string strIsEdit
    {
        get
        {
            if (ViewState["strIsEdit"] == null)
            {
                return "0";
            }
            else
            {
                return ViewState["strIsEdit"] as string;
            }
        }
        set
        {
            ViewState["strIsEdit"] = value;
        }
    }
    private string strTipDownload
    {
        get
        {
            return ViewState["strTipDownload"] as string;
        }
        set
        {
            ViewState["strTipDownload"] = value;
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

    #region 读取上传下载相应xml配置文件（作废）
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

    #region 获取源目录的文件列表
    /// <summary>
    /// 得到源目录的文件列表，该里面是包含文件以及目录路径的一个数组
    /// </summary>
    /// <param name="strFilePath"></param>
    private void GetDownloadFileList(String strFileListPath)
    {
        if (Directory.Exists(strFileListPath))
        {
            String strEncodePath =  HttpUtility.UrlEncode(strFileListPath) ;
            DirectoryInfo thisOne = new DirectoryInfo(strFileListPath);
            FileInfo[] fileInfo = thisOne.GetFiles();
            StringBuilder sb = new StringBuilder("");
            sb.Append("\r\n");
            int i = 0;
            sb.Append("     <tr align=\"center\" style=\"width:80%\">\r\n");
            sb.Append("         <td  style=\"width:10px\"  valign=\"top\" align=\"left\" class=\"tableTitle\"><font color=red>No.</font></td>\r\n");
            sb.Append("         <td  style=\"width:80%\"  valign=\"top\" align=\"left\" class=\"tableTitle\"><font color=red>FileName</font></td>\r\n");
            sb.Append("         <td  style=\"width:10%\"  valign=\"top\" align=\"right\" class=\"tableTitle\"><font color=red>Size</font></td>\r\n");
            sb.Append("         <td  style=\"width:10%\"  valign=\"top\" align=\"right\" class=\"tableTitle\"><font color=red>Down</font></td>\r\n");
            sb.Append("     </tr>\r\n");
            // 遍历所有的文件和目录
            foreach (FileInfo file in fileInfo)
            {
                String strFileName = file.Name;
                String strFileLength = file.Length.ToString();
                String strEncodeFileName = HttpUtility.UrlEncode(strFileName);
                String strOnclickDown = "\"javascript:downthisfile('" + strEncodePath + "','" + strEncodeFileName + "');\"";
                String strOnclickDel = "\"javascript:deletethisfile('" + strEncodePath + "','" + strEncodeFileName + "');\"";
                //if (i % 2 == 0)
                //{
                //    sb.Append("     <tr align=\"center\" style=\"width:80%\">\r\n");
                //    sb.Append("         <td  style=\"width:5px\"  valign=\"top\" align=\"left\" class=\"edit_label\"><font color=red>" + (i + 1).ToString() + "：</font></td>\r\n");
                //    sb.Append("         <td  style=\"width:40%\"  valign=\"top\" align=\"left\">" + strFileName + "</td>\r\n");
                //    sb.Append("         <td  style=\"width:5px\"  valign=\"top\" align=\"left\"><img title=\"" + this.strTipDownload + "：" + strFileName + "\" onclick=" + strOnclickDown + " style=\"cursor:hand;\" src=\"../common/images/down_list.gif\"></td>\r\n");
                //    sb.Append("         <td  style=\"width:5px\"  valign=\"top\" align=\"left\"><img  title=\"Delete File：" + strFileName + "\" onclick=" + strOnclickDel + " style=\"cursor:hand;\" src=\"../common/images/treeIcon/icon-delete.gif\"></td>\r\n");
                //}
                //else
                //{
                //    sb.Append("         <td  style=\"width:5px\"  valign=\"top\" align=\"left\" class=\"edit_label\"><font color=red>" + (i + 1).ToString() + "：</font></td>\r\n");
                //    sb.Append("         <td  style=\"width:40%\"  valign=\"top\" align=\"left\">" + strFileName + "</td>\r\n");
                //    sb.Append("         <td  style=\"width:5px\"  valign=\"top\" align=\"left\"><img title=\"" + this.strTipDownload + "：" + strFileName + "\" onclick=" + strOnclickDown + " style=\"cursor:hand;\" src=\"../common/images/down_list.gif\"></td>\r\n");
                //    sb.Append("         <td  style=\"width:5px\"  valign=\"top\" align=\"left\"><img title=\"Delete File：" + strFileName + "\" onclick=" + strOnclickDel + " style=\"cursor:hand;\" src=\"../common/images/treeIcon/icon-delete.gif\"></td>\r\n");
                //    sb.Append("     </tr>\r\n");
                //}
                
                sb.Append("     <tr align=\"center\" style=\"width:80%\">\r\n");
                sb.Append("         <td  style=\"width:10px\"  valign=\"top\" align=\"left\" class=\"edit_label\"><font color=red>" + (i + 1).ToString() + "：</font></td>\r\n");
                sb.Append("         <td  style=\"width:80%\"  valign=\"top\" align=\"left\">" + strFileName + "</td>\r\n");
                sb.Append("         <td  style=\"width:10%\"  valign=\"top\" align=\"right\">" + strFileLength + " KB</td>\r\n");
                sb.Append("         <td  style=\"width:10%\"  valign=\"top\" align=\"right\">\r\n");
                sb.Append("             <img title=\"" + this.strTipDownload + "：" + strFileName + "\" onclick=" + strOnclickDown + " style=\"cursor:hand;\" src=\"../common/images/down_list.gif\">\r\n");
                sb.Append("             &nbsp;&nbsp;&nbsp\r\n");
                if (strIsEdit.Equals("1"))
                {
                    sb.Append("             <img  title=\"Delete File：" + strFileName + "\" onclick=" + strOnclickDel + " style=\"cursor:hand;\" src=\"../common/images/treeIcon/icon-delete.gif\">\r\n");
                }
                sb.Append("         </td>\r\n");
                sb.Append("     </tr>\r\n");
                i++;
            }
            //if (i % 2 == 0)
            //{

            //}
            //else
            //{
            //    sb.Append("         <td style=\"width:5px\" class=\"edit_label\"></td>\r\n");
            //    sb.Append("         <td style=\"width:40%\"  valign=\"top\" align=\"center\"></td>\r\n");
            //    sb.Append("         <td style=\"width:5px\"  valign=\"top\" align=\"center\"></td>\r\n");
            //    sb.Append("     </tr>\r\n");
            //}

            this.divFileListArea.InnerHtml = sb.ToString();
        }
    }
    #endregion


}
