using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Text;
using System.Web.UI;
using System.ComponentModel;
using System.Drawing;
using Com.ValuePlus.Common;
using Com.ValuePlus.Archive.BLL;


namespace Com.ValuePlus.Archive.WebCtrls
{
    /// <summary>
    /// 数据列表文本框控件
    /// </summary>
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:TextBox　runat=server></{0}:TextBox>")]
    public class DBTextBox : System.Web.UI.WebControls.TextBox
    {
        public ImageButton image = new ImageButton();//定义点击进入列表页面的图标控件

        /// <summary>
        /// 调用改动态服务器断控件时，主页面中必须增加如下脚本行
                //function OpenDbTextBoxWindow(url, ctrlId) {
                //    var keyValue = document.getElementById(ctrlId).value;
                //    var varUrl = url + "&KEYVALUE="+keyValue;
                //    window.open(varUrl, 'newwindow', 'width=620,height=500,top=100,left=200, center:1, toolbar=no, menubar=no, scrollbars=yes,resizable=no,location=no, status=no');
                //}
        /// </summary>

        #region 变量定义
        private string _ImageUrl;
        private string _TID;
        private string _RID; 
        private string _SID;
        private string _GID;
        private string _PID;
        private string _MASTVALUE;
        private string _OPENFUNCTION;
        private Boolean _CTRLREADONLY;
        private string _isKey;
        private string _isNull;
        private string _groupType;
        private string _dataType;
        private string _Name;
        private string _NameCn;
        private string _OldValue;
        private string _WindowUrl;
        private string _TxtCtrlId;
        private string _IsSave;

        /// <summary>
        /// 图片按钮链接
        /// </summary>
        public string ImageUrl
        {
            get { return _ImageUrl; }
            set { _ImageUrl = value; }
        }
        /// <summary>
        /// 模板ID
        /// </summary>
        public string TID
        {
            get { return _TID; }
            set { _TID = value; }
        }
        /// <summary>
        /// 角色ID
        /// </summary>
        public string RID
        {
            get { return _RID; }
            set { _RID = value; }
        }
        /// <summary>
        /// 场景ID
        /// </summary>
        public string SID
        {
            get { return _SID; }
            set { _SID = value; }
        }
        /// <summary>
        /// 分组ID
        /// </summary>
        public string GID
        {
            get { return _GID; }
            set { _GID = value; }
        }
        /// <summary>
        /// 字段ID
        /// </summary>
        public string PID
        {
            get { return _PID; }
            set { _PID = value; }
        }
        /// <summary>
        /// 主控本控件的控制值
        /// </summary>
        public string MASTVALUE
        {
            get { return _MASTVALUE; }
            set { _MASTVALUE = value; }
        }
        /// <summary>
        /// 图片按钮控件点击相应的js方法
        /// </summary>
        public string OPENFUNCTION
        {
            get { return _OPENFUNCTION; }
            set { _OPENFUNCTION = value; }
        }
        /// <summary>
        /// 控件只读属性
        /// </summary>
        public Boolean CtrlReadOnly
        {
            get { return _CTRLREADONLY; }
            set { _CTRLREADONLY = value; }
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
        /// 点击按钮弹出窗口链接
        /// </summary>
        public string WindowUrl
        {
            get { return _WindowUrl; }
            set { _WindowUrl = value; }
        }
        /// <summary>
        /// 控件ID
        /// </summary>
        public string TxtCtrlId 
        {
            get { return _TxtCtrlId;  }
            set { _TxtCtrlId = value; }
        }
        /// <summary>
        /// 是否加密（1是0否）
        /// </summary>
        public string IsSave
        {
            get { return _IsSave; }
            set { _IsSave = value; }
        }
        #endregion

        public DBTextBox()
        {
            this.SetImageClickFunction();
            this.Controls.Add(this.image);
            
        }

        ///// <summary>
        ///// 设置控制只读状态的显示属性【只读显示，文本框可编辑】
        ///// </summary>
        //public void SetCtrlReadOnlyStyle(bool bCtrlReadOnly)
        //{
        //    if (bCtrlReadOnly)
        //    {
        //        this.image.Visible = false;
        //        this.CssClass = "Text_readonly_Archive";
        //    }
        //    else
        //    {
        //        this.image.Visible = true;
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
                this.CtrlReadOnly = true;
                this.Attributes.Add("readonly", "true");
                this.CssClass = "Text_readonly_Archive";
                this.image.Visible = false;
                this.Attributes.Remove("onfocus");
                this.Attributes.Remove("onblur");
                this.Attributes.Remove("onclick");
            }
            else
            {
                //this.ReadOnly = false;
                //this.Attributes.Add("readonly", "false");
                this.CtrlReadOnly = false;
                this.CssClass = "Text_edit_Archive";
                this.image.Visible = true;
            }
        }

        /// <summary>
        /// 设置图片点击事件
        /// </summary>
        public void SetImageClickFunction()
        {
            if (!this.CtrlReadOnly)//add by sammen 20120821
            {
                String strUrlParam = "TID=" + this.TID + "&SID=" + this.SID + "&RID=" + this.RID + "&GID=" + this.GID + "&PID=" + this.PID;
                this.TxtCtrlId = ServerCtrlIDGetterBll.GetCtrlID_DBTextBox(this.TID, this.GID, this.PID);
                this.image.Attributes.Remove("Params");
                this.image.Attributes.Add("Params", strUrlParam);//为了返回到js获取
                strUrlParam = strUrlParam + "&MASTVALUE=" + this.MASTVALUE;
                //String strUrl = "ArchiveCtrlDBList.aspx?" + UrlParamEncryption.EncryptionUrlParam(strUrlParam);
                this.WindowUrl = "ArchiveCtrlDBList.aspx?" + strUrlParam;//不加密

                image.Attributes.Remove("onclick");
                image.Attributes.Add("onclick", "OpenDbTextBoxWindow('" + this.WindowUrl + "','" + this.TxtCtrlId + "');return false;");

                this.Attributes.Remove("onblur");
                String strOnBlurUrl = this.WindowUrl + "&ISOK=1";//增加默认确定关闭的参数
                //delete by sammen 20210604
                this.Attributes.Add("onblur", "OpenDbTextBoxWindow('" + strOnBlurUrl + "','" + this.TxtCtrlId + "'); return false");
                this.Attributes.Add("curValue", "");

                ////add by sammen 20231228 禁止控件可人工输入，仅提供放大镜选择 delete by sammen 20230105
                //this.Attributes.Add("ReadOnly", "true");
                //this.Attributes.Remove("onblur");
            }
        }

      //[Bindable(true)]
      //[Category("Appearance")]
      //[DefaultValue("")]
      //[Localizable(true)]
      //public　override　string　Text
      //{
      //    get
      //    {
      //        if　(base.Text.ToLower()　==　Tip.ToLower())
      //            return　string.Empty;
      //        return　base.Text;
      //    }
      //    set
      //    {
      //        base.Text　=　value;
      //    }
      //}

        protected override void Render(HtmlTextWriter writer)
        {

            base.Render(writer);
            this.image.RenderControl(writer);
        }

    }


}
