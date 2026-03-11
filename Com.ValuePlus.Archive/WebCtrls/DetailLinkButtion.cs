using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.ComponentModel;
using Com.ValuePlus.Common;
using System.Data;
using Com.ValuePlus.Archive.DAL;
using Com.ValuePlus.Archive.Config;

namespace Com.ValuePlus.Archive.WebCtrls
{
    [ToolboxData("<{0}:AttachmentCtrl runat=server></{0}:AttachmentCtrl>"), DefaultProperty("Text")]
    public class DetailLinkButtion : System.Web.UI.Control
    {
        public System.Web.UI.WebControls.LinkButton linkButton = new System.Web.UI.WebControls.LinkButton();

        private string _TID;
        private string _RID;
        private string _SID;
        private string _KEY;
        private string _KEYVALUE;
        private string _Name;
        private string _NameCn;
        private string _IsSave;

        /// <summary>
        /// 模板ID
        /// </summary>
        public string DTID
        {
            get { return _TID; }
            set { _TID = value; }
        }
        /// <summary>
        /// 角色ID
        /// </summary>
        public string DRID
        {
            get { return _RID; }
            set { _RID = value; }
        }
        /// <summary>
        /// 场景ID
        /// </summary>
        public string DSID
        {
            get { return _SID; }
            set { _SID = value; }
        }
        /// <summary>
        /// 键字段
        /// </summary>
        public string DKEY
        {
            get { return _KEY; }
            set { _KEY = value; }
        }
        /// <summary>
        /// 键值
        /// </summary>
        public string DKEYVALUE
        {
            get { return _KEYVALUE; }
            set { _KEYVALUE = value; }
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
        /// 是否加密（1是0否）
        /// </summary>
        public string IsSave
        {
            get { return _IsSave; }
            set { _IsSave = value; }
        }


        public DetailLinkButtion()
        {
            this.linkButton.Style.Add("cursor","hand");
            this.linkButton.CssClass = "a_Left";
            this.linkButton.Text = this.DKEYVALUE;
            this.Controls.Add(this.linkButton);
        }

        /// <summary>
        /// 设置点击事件
        /// </summary>
        public void SetClickFunction()
        {
            this.linkButton.Text = this.DKEYVALUE;
            String strUrlParam = "TID=" + this.DTID + "&SID=" + this.DSID + "&RID=" + this.DRID + "&KEY=" + this.DKEY + "&KEYVALUE=" + this.DKEYVALUE;
            //String strUrl = "ArchiveCtrlDBList.aspx?" + UrlParamEncryption.EncryptionUrlParam(strUrlParam);
            String strUrl = "../ViewArchiveDetail.aspx?" + strUrlParam;
            String strWindowTarget = "window" + this.DTID + this.DKEYVALUE;
            strWindowTarget = strWindowTarget.Replace("-", "");
            String strOpenFunction = "window.open('" + strUrl + "', '" + strWindowTarget + "', 'width=820,height=500,top=100,left=200, center:1, toolbar=no, menubar=no, scrollbars=yes,resizable=yes,location=no, status=no');return false;";

            this.linkButton.Attributes.Remove("onclick");
            this.linkButton.Attributes.Add("onclick", strOpenFunction);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            this.linkButton.RenderControl(writer);
        }
    }
}
