using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Text;
using System.Web.UI;
using System.ComponentModel;
using System.Drawing;
using Com.ValuePlus.Common;

namespace Com.ValuePlus.Archive.WebCtrls
{
    /// <summary>
    /// 图片选择浏览文本控件
    /// </summary>
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:TextBox　runat=server></{0}:TextBox>")]
    public class ImageTextBox : System.Web.UI.WebControls.TextBox
    {
        public System.Web.UI.WebControls.Image image = new System.Web.UI.WebControls.Image();//定义图片控件
        public ImageButton imgBtn = new ImageButton();//定义点击进入选择图片的图标控件

        private string _ImagePath;
        private string _isKey;
        private string _isNull;
        private string _groupType;
        private string _dataType;
        private string _Name;
        private string _NameCn;
        private string _OldValue;
        private string _IsSave;

        /// <summary>
        /// 图片选择路径
        /// </summary>
        public string ImagePath
        {
            get { return _ImagePath; }
            set { _ImagePath = value; }
        }
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

        public ImageTextBox()
        {
            this.Controls.Add(this.image);
            this.Controls.Add(this.imgBtn);
            
        }

        /// <summary>
        /// 设置图片显示
        /// </summary>
        public void SetImageStyle()
        {
            String strImageFileName = this.Text;

            this.image.ImageUrl = this.ImagePath + "/" + strImageFileName;

            //add by sammen 20220105 优化新增, 解决Edge浏览器的图片无法放大的问题
            this.image.Attributes.Remove("data-preview-src");
            this.image.Attributes.Add("data-preview-src", "");
        }

        /// <summary>
        /// 设置图标按钮属性
        /// </summary>
        public void SetImgBtnAttributes()
        {
            this.imgBtn.ImageUrl = "../../common/images/search1.png";

            String strUrlParam = "path=" + this.ImagePath + "&ctrlId=" + this.ID + "&size=161";
            String strUrl = "ArchiveImageSelect.aspx?" + UrlParamEncryption.EncryptionUrlParam(strUrlParam);
            //String strOpenFunction = "window.showModalDialog('" + strUrl + "','window','dialogWidth:620px;dialogHeight:500px;location:no;edge:raised;resizable:yes;scroll:auto;status:no;center:yes;help:no;minimize:yes;maximize:yes;');return false;";
            String strOpenFunction = "window.open('" + strUrl + "', 'newwindow', 'width=620,height=500,top=100,left=200, center:1, toolbar=no, menubar=no, scrollbars=yes,resizable=no,location=no, status=no');return false;";

            this.Attributes.Remove("ondblclick");
            this.Attributes.Add("ondblclick", strOpenFunction);
            this.imgBtn.Attributes.Remove("onclick");
            this.imgBtn.Attributes.Add("onclick", strOpenFunction);

        }

        ///// <summary>
        ///// 根据是否是只读设置控件相关属性【只读显示，文本框可编辑】
        ///// </summary>
        ///// <param name="bReadOnly"></param>
        //public void SetCtrlReadOnlyStyle(bool bReadOnly)
        //{
        //    if (bReadOnly)
        //    {
        //        this.Attributes.Remove("ondblclick");
        //        this.imgBtn.Visible = false;
        //        //this.ReadOnly = true;//设置只读后客户端赋值后无法保存成功

        //        this.CssClass = "Text_readonly_Archive";
        //    }
        //    else
        //    {
        //        this.imgBtn.Visible = true;
        //        //this.ReadOnly = false;
        //        this.CssClass = "Text_edit_Archive";
        //    }
        //}

        /// <summary>
        /// 根据是否是只读设置控件相关属性【完全只读】
        /// </summary>
        /// <param name="bReadOnly"></param>
        public void SetCtrlReadOnly(bool bReadOnly)
        {
            if (bReadOnly)
            {
                //this.ReadOnly = true;//此种设置后台程序将获取不到客户端的赋值
                this.Attributes.Add("readonly", "true");
                this.CssClass = "Text_readonly_Archive";
                this.imgBtn.Visible = false;
                this.Attributes.Remove("onfocus");
                this.Attributes.Remove("onclick");
                this.Attributes.Remove("ondblclick");
            }
            else
            {
                //this.ReadOnly = false;
                this.Attributes.Add("readonly", "false");
                this.CssClass = "Text_edit_Archive";
                this.imgBtn.Visible = true;
            }
            //SetCtrlReadOnlyStyle(bReadOnly);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            this.image.RenderControl(writer);
            base.Render(writer);
            this.imgBtn.RenderControl(writer);
        }
    }
}
