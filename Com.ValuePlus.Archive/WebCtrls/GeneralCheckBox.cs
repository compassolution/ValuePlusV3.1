using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.ComponentModel;

namespace Com.ValuePlus.Archive.WebCtrls
{
    public class GeneralCheckBox:CheckBox
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


        /// <summary>
        /// 根据是否是只读设置控件相关属性【完全只读】
        /// </summary>
        /// <param name="bReadOnly"></param>
        public void SetCtrlReadOnly(bool bReadOnly)
        {
            if (bReadOnly)
            {
                this.Enabled = false;
                //this.Attributes.Add("onfocus", "this.blur()");
                this.CssClass = "CheckBox_readonly_Archive";
            }
            else
            {
                this.Enabled = true;
                //this.Attributes.Remove("onfocus");
                this.CssClass = "CheckBox_edit_Archive";
            }
        }

        public GeneralCheckBox()
        {
        }
            
        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
        }
    }
}
