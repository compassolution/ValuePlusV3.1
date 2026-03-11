using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Com.ValuePlus.Web;
using Com.ValuePlus.Common;
using System.IO;
using System.Text;

public partial class Archive_Detail_ArchiveImageSelect : ArchivePageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ////解密传递字符串并获取对应参数值
        Hashtable htUrlQuery = UrlParamEncryption.DecryptionUrlParam(base.Request.Url.Query.ToString());
        this.strPath = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "path");
        this.strCtrlId = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "ctrlId");
        this.strSize = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "size") == "" ? "30" : UrlParamEncryption.GetUrlParamValue(htUrlQuery, "size");

        this.getImagePath(base.MapPath(this.strPath));

        if (!base.IsPostBack)
        {
            if (this.Language.Equals("zh-cn"))
            {
                this.Page.Title = Session["ArchiveDesc"] + "图片选择列表";
            }
            else
            {
                this.Page.Title = Session["ArchiveDesc"] + " Image Select List";
            }

            HtmlGenericControl control = (HtmlGenericControl)this.Page.FindControl("HEAD1");
            StringBuilder strB = new StringBuilder();
            strB.Append("<script>\r\n");
            strB.Append("   function dbclick(selectedIMG)\r\n");
            strB.Append("   {\r\n");
            strB.Append("       var filename=selectedIMG.src.substr(selectedIMG.src.lastIndexOf('/')+1);\r\n");
            strB.Append("       window.opener.document.all['" + this.strCtrlId + "'].value=decodeURI(filename);\r\n");
            strB.Append("       if(window.opener.document.all['" + "img_" + this.strCtrlId + "']!=null)\r\n");
            strB.Append("       {\r\n");
            strB.Append("           window.opener.document.all['" + "img_" + this.strCtrlId + "'].src=decodeURI(selectedIMG.src);\r\n");
            strB.Append("       }\r\n");
            strB.Append("       close();\r\n");
            strB.Append("   }\r\n");
            strB.Append("</script>\r\n");

            control.Controls.Add(new LiteralControl(strB.ToString()));
        }
    }

    #region viewstate初始化区域
    private string strPath
    {
        get
        {
            return ViewState["strPath_ViewState"] as string;
        }
        set
        {
            ViewState["strPath_ViewState"] = value;
        }
    }
    private string strPhotoName
    {
        get
        {
            return ViewState["strPhotoName_ViewState"] as string;
        }
        set
        {
            ViewState["strPhotoName_ViewState"] = value;
        }
    }
    private string strCtrlId
    {
        get
        {
            return ViewState["strCtrlId_ViewState"] as string;
        }
        set
        {
            ViewState["strCtrlId_ViewState"] = value;
        }
    }
    private string strSize
    {
        get
        {
            return ViewState["strSize_ViewState"] as string;
        }
        set
        {
            ViewState["strSize_ViewState"] = value;
        }
    }
    #endregion

    private void getImagePath(string strPath)
    {
        int iSize_Height = int.Parse(this.strSize);
        Double dec = (Double)iSize_Height * 0.8;
        int iSize_Width = int.Parse(Math.Round(dec).ToString());
        if (Directory.Exists(strPath))
        {
            string[] files = Directory.GetFiles(strPath);
            for (int i = 0; i < files.Length; i++)
            {
                string strFileName = files[i].Substring(files[i].LastIndexOf(@"\") + 1);
                this.getImage(strFileName, iSize_Height, iSize_Width);
            }
        }
    }

    private void getImage(string strFileName, int iSize_Height, int iSize_Width)
    {
        Image child = new Image();
        child.ImageUrl = this.strPath + "/" + strFileName;
        child.Height = Unit.Pixel(iSize_Height);
        child.Width = Unit.Pixel(iSize_Width);
        child.Attributes.Add("ondblclick", "dbclick(this);");
        child.ToolTip = strFileName;
        this.Controls.Add(child);
    }
}
