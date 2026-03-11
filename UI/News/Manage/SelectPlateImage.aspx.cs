using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Com.ValuePlus.Web;
using System.IO;

public partial class News_Manage_SelectPlateImage : PageBase
{
    private string strPath;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.strPath = base.Request["PATH"].ToString();
            this.getImages(base.MapPath(this.strPath));
        }

    }

    private void getImage(string filename)
    {
        Label lb = new Label();
        lb.Text = "  ";
        this.Controls.Add(lb);
        Image childImage = new Image();
        childImage.ImageUrl = this.strPath + "" + filename;
        //childImage.Height = Unit.Pixel(100);
        //childImage.Width = Unit.Pixel(100);
        childImage.Attributes.Add("ondblclick", "ImageDbClick('" + this.strPath + "','" + filename + "');window.close();");
        childImage.ToolTip = filename;
        this.Controls.Add(childImage);
    }

    private void getImages(string strPath)
    {
        if (Directory.Exists(strPath))
        {
            string[] files = Directory.GetFiles(strPath);
            for (int i = 0; i < files.Length; i++)
            {
                string strFilename = files[i].Substring(files[i].LastIndexOf(@"\") + 1);
                this.getImage(strFilename);
            }
        }
    }

}

