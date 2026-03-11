using System;
using System.IO;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Com.ValuePlus.Web;
using System.Resources;

public partial class MenuManager_SelectImage : PageBase
{
    public string strPath;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //自定义设置页面文字显示的中英文字符串
            ResourceManager rmLocResourceManager = base.GetResourceManager("MenuManager");
            this.Label1.Text = rmLocResourceManager.GetString("lbSelectImage");

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
        childImage.Height = 0x55;
        childImage.Width = 0x55;
        childImage.Attributes.Add("ondblclick", "ImageDbClick('"+this.strPath +"','" + filename + "');window.close();");
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
