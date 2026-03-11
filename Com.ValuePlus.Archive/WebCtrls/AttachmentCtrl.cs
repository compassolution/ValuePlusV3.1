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
    public class AttachmentCtrl : System.Web.UI.Control
    {
        public System.Web.UI.WebControls.Image image = new System.Web.UI.WebControls.Image();//定义图片控件

        private string _ImagePath;
        private string _isKey;
        private string _isNull;
        private string _groupType;
        private string _dataType;
        private string _fileUrl;
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
        /// 字段类型
        /// </summary>
        public string FilePath
        {
            get { return _fileUrl; }
            set { _fileUrl = value; }
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

        public AttachmentCtrl()
        {
            this.image.Style.Add("cursor","hand");
            this.Controls.Add(this.image);
        }

        /// <summary>
        /// 设置图片显示
        /// </summary>
        public void SetImageUrl(String strImageUrl)
        {
            this.image.ImageUrl = strImageUrl;
        }

        /// <summary>
        /// 设置图标按钮点击事件
        /// </summary>
        public void SetCtrlClick(String strFileUrl)
        {
            if (!String.IsNullOrEmpty(strFileUrl))
            {
                this.FilePath = strFileUrl;
                String strUrlParam = "path=" + strFileUrl;
                this.SetImageClickAttribute(strUrlParam);
            }
        }

        /// <summary>
        /// 设置图标按钮点击事件
        /// </summary>
        public void SetCtrlClick(String strTID, String strSID, String strGID,String strKey,String strKeyValue, String strGridKey, String strGridKeyValue, String strPID,String strOpType)
        {
            String strFileUrl = "";
            String strSql = "select * from TB_HRTMPSD WHERE TID = '" + strTID + "' AND SID ='" + strSID + "' AND GID = '" + strGID + "' AND PID = '" + strPID + "'";
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
            if ((dt != null) && (dt.Rows.Count > 0))
            {
                strFileUrl = dt.Rows[0]["PCTRLD"].ToString();
                //获取附件操作权限
                String strPright = dt.Rows[0]["PRIGHT"].ToString();
                if (!strPright.Equals("0"))
                {
                    strOpType = "readonly";
                }
            }
            if (!String.IsNullOrEmpty(strFileUrl))
            {
                if ((Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("ArchiveAttachment_FloderIsRelateTID") != null) && (Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("ArchiveAttachment_FloderIsRelateTID").Equals("1")))
                {
                    //设置目录下自动创建与模板及分组相关的文件夹
                    strFileUrl = strFileUrl + "/" + strTID + "-" + strGID;
                }
                strFileUrl = strFileUrl + "/" + strKeyValue;
                String strUrlParam = "TID=" + strTID + "&SID=" + strSID + "&GID=" + strGID + "&KEY=" + strKey + "&KEYVALUE=" + strKeyValue + "&PID=" + strPID + "&OPTYPE=" + strOpType;
                if (!String.IsNullOrEmpty(strGridKey))
                {
                    strFileUrl = strFileUrl + "-" + strGridKeyValue;
                    strUrlParam = strUrlParam + "&GRIDKEY=" + strGridKey + "&GRIDKEYVALUE=" + strGridKeyValue;
                }
                strUrlParam = strUrlParam + "&path=" + strFileUrl;

                this.SetImageClickAttribute(strUrlParam);
            }
        }

        /// <summary>
        /// 设置图标按钮点击事件
        /// </summary>
        private void SetImageClickAttribute(String strUrlParam)
        {
            String strUrl = "ArchiveAttachment.aspx?" + UrlParamEncryption.EncryptionUrlParam(strUrlParam); 
            String strOpenFunction = "window.open('" + strUrl + "', 'newwindow', 'width=900,height=700,top=100,left=200, center:1, toolbar=no, menubar=no, scrollbars=yes,resizable=yes,location=no, status=no');return false;";

            this.image.Attributes.Remove("onclick");
            this.image.Attributes.Add("onclick", strOpenFunction);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            this.image.RenderControl(writer);
        }
    }
}
