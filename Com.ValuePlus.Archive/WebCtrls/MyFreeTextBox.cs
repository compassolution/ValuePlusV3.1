using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using FreeTextBoxControls;


namespace Com.ValuePlus.Archive.WebCtrls
{
    [ToolboxData(@"<{0}:FreeTextBox runat=""server""></{0}:FreeTextBox>")]
    [ValidationProperty("Text")]
    [DefaultProperty("Text")]
    public class MyFreeTextBox : FreeTextBox
    {
        private string _isKey;
        private string _isNull;
        private string _groupType;
        private string _dataType;
        private string _Name;
        private string _NameCn;
        private string _OldValue;
        private string _IsSave;

        /// <summary>
        /// 是否是主键（1是0否）
        /// </summary>
        public string IsKey
        {
            get { return _isKey; }
            set { _isKey = value; }
        }
        /// <summary>
        /// 是否可为空（1是0否）
        /// </summary>
        public string IsNull
        {
            get { return _isNull; }
            set { _isNull = value; }
        }
        /// <summary>
        /// 所在分组类型）
        /// </summary>
        public string GroupType
        {
            get { return _groupType; }
            set { _groupType = value; }
        }
        /// <summary>
        /// 字段类型
        /// </summary>
        public string DataType
        {
            get { return _dataType; }
            set { _dataType = value; }
        }
        /// <summary>
        /// 英文名称
        /// </summary>
        public string NameEn
        {
            get { return _Name; }
            set { _Name = value; }
        }
        /// <summary>
        /// 中文名称
        /// </summary>
        public string NameCn
        {
            get { return _NameCn; }
            set { _NameCn = value; }
        }
        /// <summary>
        /// 初始值
        /// </summary>
        public string OldValue
        {
            get { return _OldValue; }
            set { _OldValue = value; }
        }
        /// <summary>
        /// 是否加密（1是0否）
        /// </summary>
        public string IsSave
        {
            get { return _IsSave; }
            set { _IsSave = value; }
        }

        public MyFreeTextBox()
        {
            this.Language = "zh-cn";
            this.ToolbarStyleConfiguration = FreeTextBoxControls.ToolbarStyleConfiguration.OfficeMac;
            this.Width = Unit.Percentage(100);
            this.Height = Unit.Pixel(300);

            //this.AutoGenerateToolbarsFromString = true; //由字符串自动生成工具栏按钮=false
            this.SupportFolder = "~/aspnet_client/FreeTextBox/"; //源代码
            this.ImageGalleryUrl = "ftb.imagegallery.aspx?rif={0}&cif={0}"; //指定选择图片的aspx文件
            this.JavaScriptLocation = ResourceLocation.ExternalFile; //设java脚本为外部文件
            this.ButtonImagesLocation = ResourceLocation.ExternalFile;//设按钮图片外部文件
            this.ToolbarImagesLocation = ResourceLocation.ExternalFile;//设按钮图片外部文件
            //this.BreakMode = BreakMode.LineBreak;//断行模式
            //this.StripAllScripting = true; //自动移除Java脚本.!!!非常重要!!!!
            this.ButtonSet = ToolbarStyleConfiguration.OfficeXP; //按钮样式
            this.ToolbarStyleConfiguration = ToolbarStyleConfiguration.OfficeXP;
            this.DesignModeCss = "~/css/ftbdesign.css"; //设计模式时样式,很重要
            //this.RemoveServerNameFromUrls = false;
            //this.BackColor = Color.FromArgb(229, 240, 253);
            //this.GutterBackColor = Color.FromArgb(229, 240, 253);

            this.ToolbarLayout = "ParagraphMenu, FontFacesMenu, FontSizesMenu,FontForeColorsMenu,FontBackColorsMenu, FontForeColorPicker, FontBackColorPicker";
            this.ToolbarLayout = this.ToolbarLayout + "| Bold, Italic, Underline,Strikethrough, Superscript, Subscript, RemoveFormat";
            this.ToolbarLayout = this.ToolbarLayout + "| JustifyLeft, JustifyRight, JustifyCenter,JustifyFull; BulletedList, NumberedList, Indent, Outdent; CreateLink, Unlink";
            this.ToolbarLayout = this.ToolbarLayout + "| Cut,Copy, Paste, Delete, Undo, Redo, Print, Save| SymbolsMenu, StyleMenu";
            this.ToolbarLayout = this.ToolbarLayout + "| InsertRule, InsertDate, InsertTime";
            this.ToolbarLayout = this.ToolbarLayout + "| InsertTable, EditTable; InsertTableRowBefore, InsertTableRowAfter, DeleteTableRow; InsertTableColumnBefore, InsertTableColumnAfter, DeleteTableColumn";
            this.ToolbarLayout = this.ToolbarLayout + "| InsertForm, InsertDiv, InsertTextBox, InsertTextArea, InsertRadioButton, InsertCheckBox, InsertDropDownList, InsertButton";
            this.ToolbarLayout = this.ToolbarLayout + "| InsertImageFromGallery, Preview, SelectAll, WordClean, EditStyle";
            if (!String.IsNullOrEmpty(Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("FreeTextBox_ToolbarLayout")))
            {
                this.ToolbarLayout = Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("FreeTextBox_ToolbarLayout");
            }
            

        }

        /// <summary>
        /// 添加服务器端所有字体
        /// </summary>
        public void AddAllFont()
        {
            Toolbar toolbar1 = this.Toolbars[0];//此处的0表示第一个toolbar。
            FontFacesMenu fontmenu = (FontFacesMenu)toolbar1.Items[1];//freetextbox中加载字体的类为FontFacesMenu ,这是FontFacesMenu 在toolar中的索引。        
            InstalledFontCollection installedFontCollection = new InstalledFontCollection();//   Get   the   array   of   FontFamily   objects        .   
            FontFamily[] fontFamilies = installedFontCollection.Families;
            for (int i = fontFamilies.Length - 1; i > 0; i--)
            {
                fontmenu.Items.Add(new ToolbarListItem(fontFamilies[i].Name.ToString(), fontFamilies[i].Name.ToString()));
            }
        }

        /// <summary>
        /// 添加指定字体
        /// </summary>
        public void AddSpecificFont()
        {
            Toolbar toolbar1 = this.Toolbars[0];//此处的0表示第一个toolbar。
            FontFacesMenu fontmenu = (FontFacesMenu)toolbar1.Items[1];//freetextbox中加载字体的类为FontFacesMenu ,这是FontFacesMenu 在toolar中的索引。        
            fontmenu.Items.Add(new ToolbarListItem("Arial", "Arial"));
            fontmenu.Items.Add(new ToolbarListItem("Courier New", "Courier New"));
            fontmenu.Items.Add(new ToolbarListItem("Garamond"));
            fontmenu.Items.Add(new ToolbarListItem("Georgia", "Georgia"));
            fontmenu.Items.Add(new ToolbarListItem("Tahoma"));
            fontmenu.Items.Add(new ToolbarListItem("宋体", "宋体"));
            fontmenu.Items.Add(new ToolbarListItem("仿宋体", "仿宋体"));
            fontmenu.Items.Add(new ToolbarListItem("楷体", "楷体"));
            fontmenu.Items.Add(new ToolbarListItem("微软雅黑", "微软雅黑"));
            fontmenu.Items.Add(new ToolbarListItem("隶书", "隶书"));
            fontmenu.Items.Add(new ToolbarListItem("黑体", "黑体"));
            fontmenu.Items.Add(new ToolbarListItem("华文行楷", "华文行楷"));
            fontmenu.Items.Add(new ToolbarListItem("Times", "Times New Roman"));
            fontmenu.Items.Add(new ToolbarListItem("Verdana", "Verdana"));
        }

        /// <summary>
        /// 添加指定的字体大小
        /// </summary>
        public void AddFontSize()
        {
            Toolbar toolbar1 = this.Toolbars[0];//此处的0表示第一个toolbar。
            FontSizesMenu fontSizeMenu = (FontSizesMenu)toolbar1.Items[2];//freetextbox中加载字体的类为FontSizesMenu ,这是FontSizesMenu 在toolar中的索引。        
            fontSizeMenu.Items.Add(new ToolbarListItem("1", "1"));
            fontSizeMenu.Items.Add(new ToolbarListItem("2", "2"));
            fontSizeMenu.Items.Add(new ToolbarListItem("3", "3"));
            fontSizeMenu.Items.Add(new ToolbarListItem("4", "4"));
            fontSizeMenu.Items.Add(new ToolbarListItem("5", "5"));
            fontSizeMenu.Items.Add(new ToolbarListItem("6", "6"));
            fontSizeMenu.Items.Add(new ToolbarListItem("7", "7"));
            fontSizeMenu.Items.Add(new ToolbarListItem("8", "8"));
            fontSizeMenu.Items.Add(new ToolbarListItem("9", "9"));
        }

        /// <summary>
        /// 根据是否是只读设置控件相关属性【完全只读】
        /// </summary>
        /// <param name="bReadOnly"></param>
        public void SetCtrlReadOnly(bool bReadOnly)
        {
            if (bReadOnly)
            {
                this.ReadOnly = true;
            }
            else
            {
                this.ReadOnly = false;
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
        }
    }
}
