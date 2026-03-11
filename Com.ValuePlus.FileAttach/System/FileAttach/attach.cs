namespace Com.ValuePlus.FileAttach
{
    using Com.ValuePlus.FileAttach.localhost;
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [ToolboxData("<{0}:attach runat=server></{0}:attach>"), DefaultProperty("Text")]
    public class attach : Control
    {
        private ImageButton btnScanner = new ImageButton();
        private ImageButton Button1 = new ImageButton();
        private HtmlInputFile File1 = new HtmlInputFile();
        private Label Label1 = new Label();
        private Table myTable = new Table();
        private string myUrl;
        private string strname;
        private TextBox txtFilename = new TextBox();

        private void Add1Row(string _Filename)
        {
            TableRow row = new TableRow();
            this.myTable.Rows.Add(row);
            TableCell cell = new TableCell();
            row.Cells.Add(cell);
            cell.Controls.Add(new LiteralControl(_Filename));
            cell = new TableCell();
            row.Cells.Add(cell);
            ImageButton child = new ImageButton();
            child.ImageUrl = "../../common/images/icon/icon_query1.gif";
            if (this.HomeDir == "")
            {
                child.Attributes.Add("onclick", "return Open_Click(this,'');");
            }
            else
            {
                child.Attributes.Add("onclick", "return Open_Click(this,'" + this.HomeDir + "/');");
            }
            cell.Controls.Add(child);
            cell = new TableCell();
            row.Cells.Add(cell);
            child = new ImageButton();
            child.ImageUrl = "../../common/images/icon/icon-delete.gif";
            child.ID = "attdel" + _Filename;
            child.Click += new ImageClickEventHandler(this.Delete_Click);
            cell.Controls.Add(child);
        }

        private void btnScanner_Click(object sender, ImageClickEventArgs e)
        {
            this.strname = this.txtFilename.Text.ToString().Trim();
            if (this.strname != "")
            {
                this.strname = this.strname + ".jpg";
                if (!this.isexist(this.strname))
                {
                    this.Controls.Add(new LiteralControl("<script language=javascript>alert('这个文件名已存在！');</script>"));
                }
                else
                {
                    Upload upload = new Upload(this.myUrl);
                    string strpath = "";
                    if (upload.isExist(this.strname, strpath))
                    {
                        this.Controls.Add(new LiteralControl("<script language=javascript>alert('服务器上已经存在相同文件名的文件!');</script>"));
                    }
                }
            }
        }

        private void Button1_Click(object sender, ImageClickEventArgs e)
        {
            if (this.File1.PostedFile != null)
            {
                string fileName = this.File1.PostedFile.FileName;
                //int startIndex = fileName.LastIndexOf(@"\");//delete by wsm
                //string filename = fileName.Substring(startIndex + 1);//delete by wsm

                //add by wsm start
                int startIndex = 0;
                string filename = fileName;
                if (fileName.LastIndexOf(@"\") > 0)
                {
                    startIndex = fileName.LastIndexOf(@"\");
                    filename = fileName.Substring(startIndex + 1);
                }
                //add by wsm end

                if (!this.isexist(filename))
                {
                    this.Controls.Add(new LiteralControl("<script language=javascript>alert('The file is exists!');</script>"));
                }
                else
                {
                    //fileName = this.Page.MapPath(this.HomeDir) + fileName.Substring(startIndex);
                    fileName = this.Page.MapPath(this.HomeDir) +"\\"+ fileName.Substring(startIndex);
                    if (File.Exists(fileName))
                    {
                        this.Controls.Add(new LiteralControl("<script language=javascript>alert('The file is exists!');</script>"));
                    }
                    else
                    {
                        try
                        {
                            this.File1.PostedFile.SaveAs(fileName);
                        }
                        catch (Exception exception)
                        {
                            this.Controls.Add(new LiteralControl("<script language=javascript>alert('" + exception.Message.Replace("'", "\"") + "');</script>"));
                            return;
                        }
                        if (this.Text.Length > 0)
                        {
                            this.Text = this.Text + ";";
                        }
                        this.Text = this.Text + filename;
                        this.Add1Row(filename);
                    }
                }
            }
        }

        protected override void CreateChildControls()
        {
        }

        private void Delete_Click(object sender, ImageClickEventArgs e)
        {
            string str = (sender as ImageButton).ID.Substring(6);
            int intPos = 0;
            string[] strArray = this.Text.Split(new char[] { ";".ToCharArray()[0] });
            for (int i = 0; i < strArray.Length; i++)
            {
                if (strArray[i] == str)
                {
                    intPos = i;
                    break;
                }
            }
            this.deletefromtext(str, intPos);
            string path = this.Page.MapPath(this.HomeDir) + @"\" + str;
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            this.myTable.Rows.RemoveAt(intPos);
        }

        private void deletefromtext(string _filename, int intPos)
        {
            if (intPos == 0)
            {
                if (this.Text.Length == _filename.Length)
                {
                    this.Text = "";
                }
                else
                {
                    this.Text = this.Text.Replace(_filename + ";", "");
                }
            }
            else
            {
                this.Text = this.Text.Replace(";" + _filename, "");
            }
        }

        private bool isexist(string filename)
        {
            if (this.Text != "")
            {
                string[] strArray = this.Text.Split(new char[] { ";".ToCharArray()[0] });
                for (int i = 0; i < strArray.Length; i++)
                {
                    if (strArray[i] == filename)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private void ListFiles()
        {
            this.Controls.Add(new LiteralControl("<P>"));
            this.Controls.Add(this.myTable);
            if (this.Text != "")
            {
                string[] strArray = this.Text.Split(new char[] { ";".ToCharArray()[0] });
                for (int i = 0; i < strArray.Length; i++)
                {
                    this.Add1Row(strArray[i]);
                }
            }
        }

        protected override void OnInit(EventArgs e)
        {
            this.myTable.EnableViewState = false;
            this.Controls.Add(new LiteralControl("<script src='ldAttacher.js'></script>"));
            this.Button1.ID = "Button1";
            this.Button1.ImageUrl = "../../common/images/icon/ToolbaruploadEnabled.gif";
            this.Button1.ToolTip = "Upload";
            this.Button1.Attributes.Add("onclick", "return uploadclick(File1);");
            this.Button1.Click += new ImageClickEventHandler(this.Button1_Click);
            this.File1.ID = "File1";
            this.myUrl = this.Page.Request.Url.ToString();
            this.myUrl = this.myUrl.Substring(0, this.myUrl.ToLower().LastIndexOf("/edit"));
            this.myUrl = this.myUrl + "/service/Upload.asmx";
            this.btnScanner.ID = "btnScanner";
            this.btnScanner.ImageUrl = "../../common/images/icon/EasyImage.gif";
            this.btnScanner.ToolTip = "Scanner";
            this.btnScanner.Click += new ImageClickEventHandler(this.btnScanner_Click);
            this.txtFilename.ID = "txtFilename";
            this.Label1.ID = "Label1";
            this.Label1.Text = ".JPG";
            this.Controls.Add(this.File1);
            this.Controls.Add(this.Button1);
            //this.Controls.Add(this.txtFilename);
            //this.Controls.Add(this.Label1);
            //this.Controls.Add(this.btnScanner);
            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            this.ListFiles();
        }

        protected override void OnPreRender(EventArgs e)
        {
            this.btnScanner.Attributes.Add("onclick", "return easyimageclick(txtFilename,'" + this.myUrl + "','" + this.Text + "','" + this.HomeDir + "','" + this.Key + "');");
            this.strname = "";
            this.Page.RegisterHiddenField(this.UniqueID + "Hide", this.Text);
            this.Page.RegisterHiddenField(this.UniqueID + "Key", this.Key);
            base.OnPreRender(e);
        }

        public string HomeDir
        {
            get
            {
                if (this.ViewState["HomeDir"] == null)
                {
                    return "";
                }
                return this.ViewState["HomeDir"].ToString();
            }
            set
            {
                this.ViewState["HomeDir"] = value;
            }
        }

        public string Key
        {
            get
            {
                if (this.ViewState["Key"] == null)
                {
                    return "";
                }
                return this.ViewState["Key"].ToString();
            }
            set
            {
                this.ViewState["Key"] = value;
            }
        }

        [DefaultValue(""), Category("Appearance"), Bindable(true)]
        public string Text
        {
            get
            {
                if (this.ViewState["text"] == null)
                {
                    return "";
                }
                return this.ViewState["text"].ToString();
            }
            set
            {
                this.ViewState["text"] = value;
            }
        }
    }
}

