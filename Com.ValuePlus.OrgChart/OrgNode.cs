using System;
using System.ComponentModel;
using System.Text;
using System.Web.UI;

namespace Com.ValuePlus.OrgChart
{
    public class OrgNode
    {
        private string Descript;
        private string ImageAddress;
        private string ImageUrlAddress;
        private string NavigateUrlAddress;
        private object ObjectDemo;
        internal OrgNodeCollection OrgNodeCollectionDemo;
        private string TipText;
        private string TypeStyle;
        private LabelLayoutFlow lableLayOutFlow;

        public OrgNode()
        {
            this.OrgNodeCollectionDemo = new OrgNodeCollection(this);
        }

        public string GetNodeIndex()
        {
            return "";
        }

        protected OrgNode GetPreviousSibling()
        {
            return null;
        }

        protected virtual bool OnSelectedIndexChange(EventArgs e)
        {
            return true;
        }

        public virtual string OrgNodeHtml()
        {
            return this.OrgNodeHtml(this);
        }

        public virtual string OrgNodeHtml(OrgNode node)
        {
            return this.OrgNodeHtml("", node);
        }

        public virtual string OrgNodeHtml(string templateHtml, OrgNode xNode)
        {
            string strHtmlOrgNode;
            if ((templateHtml == null) || (templateHtml == ""))
            {
                StringBuilder strBuilderT = new StringBuilder();
                strBuilderT.Append("\r\n");
                strBuilderT.Append("<TABLE Width={6} height={7} align=center border=0>\r\n");
                strBuilderT.Append("    <TR>\r\n");
                strBuilderT.Append("        <TD align=center style=\"font-size: 12px;font-family: Verdana, Arial;layout-flow :{5};padding : 5px 5px 5px 5px;border:thin solid  orange;background-color: lightgrey\" title='{1}'>\r\n");
                strBuilderT.Append("            {3}<a href='{4}'>{0}</a>\r\n");
                strBuilderT.Append("        </TD>\r\n");
                strBuilderT.Append("    </TR>\r\n");
                strBuilderT.Append("</TABLE>\r\n");
                strHtmlOrgNode = strBuilderT.ToString();

                //str = "<TABLE Width=100 align=center border=0><TR><TD align=center style=\"font-size: 12px;font-family: Verdana, Arial;layout-flow :vertical-ideographic;padding : 5px 5px 5px 5px;border:thin solid  orange;background-color: lightgrey\" title='{1}'>{3}<a href='{4}'>{0}</a></TD></TR></TABLE>";
            }
            else
            {
                strHtmlOrgNode = templateHtml;
            }
            StringBuilder builderOrgNode = new StringBuilder();
            string[] strArray = new string[] 
            { 
                (String.IsNullOrEmpty(xNode.Text)) ? "" : xNode.Text, 
                (String.IsNullOrEmpty(xNode.Description)) ? "" : xNode.Description, 
                (String.IsNullOrEmpty(xNode.Type)) ? "" : xNode.Type, 
                (String.IsNullOrEmpty(xNode.ImageUrl)) ? "" : ("<img border=0 src='" + xNode.ImageUrl + "'>"), 
                (String.IsNullOrEmpty(xNode.NavigateUrl)) ? "#" : xNode.NavigateUrl,
                (xNode.LayoutFlow.Equals(LabelLayoutFlow.Vertical))?"vertical-ideographic":"horizontal",
                (xNode.LayoutFlow.Equals(LabelLayoutFlow.Vertical))?"30":"200",
                (xNode.LayoutFlow.Equals(LabelLayoutFlow.Vertical))?"160":"30"
            };

            //string imageUrl = xNode.ImageUrl;
            //if ((imageUrl != null) && !(imageUrl == ""))
            //{
            //    strArray[3] = (imageUrl == "") ? "" : ("<img border=0 src='" + imageUrl + "'>");
            //}
            //string str3 = xNode.Type.ToUpper();
            //if (str3 != null)
            //{
            //    str3 = string.IsInterned(str3);
            //    if (str3 == "ROOT")
            //    {
            //        imageUrl = this.ImageFolder + "x1root.gif";
            //    }
            //    else if (str3 == "GROUP")
            //    {
            //        imageUrl = this.ImageFolder + "X1Group.gif";
            //    }
            //    else if (str3 == "ROLES")
            //    {
            //        imageUrl = this.ImageFolder + "X1Roles.gif";
            //    }
            //    else
            //    {
            //        if (!(str3 == "LOGIN"))
            //        {
            //            goto Label_017B;
            //        }
            //        imageUrl = this.ImageFolder + "X1Login.gif";
            //    }
            //    goto Label_0181;
            //}
        //Label_017B:
        //    imageUrl = "";
        //Label_0181:
        //    strArray[3] = (imageUrl == "") ? "" : ("<img border=0 src='" + imageUrl + "'>");
            builderOrgNode.AppendFormat(strHtmlOrgNode, (object[])strArray);
            return builderOrgNode.ToString();
        }

        [Category("Data"), DefaultValue("结点描述"), PersistenceMode(PersistenceMode.Attribute)]
        public string Description
        {
            get
            {
                object descript = this.Descript;
                if (descript == null)
                {
                    return string.Empty;
                }
                return (string)descript;
            }
            set
            {
                this.Descript = value;
            }
        }

        [Bindable(true), Category("Appearance"), DefaultValue(""), Description("存放图片的目录")]
        public string ImageFolder
        {
            get
            {
                if (this.ImageAddress == null)
                {
                    this.ImageAddress = "images/";
                }
                this.ImageAddress = this.ImageAddress.Trim();
                if (!this.ImageAddress.EndsWith(@"\") && !this.ImageAddress.EndsWith("/"))
                {
                    this.ImageAddress = this.ImageAddress + "/";
                }
                return this.ImageAddress;
            }
            set
            {
                this.ImageAddress = value;
            }
        }

        public string ImageUrl
        {
            get
            {
                if (this.ImageUrlAddress != null)
                {
                    return this.ImageUrlAddress;
                }
                return string.Empty;
            }
            set
            {
                this.ImageUrlAddress = value;
            }
        }

        [Category("Behavior"), DefaultValue(""), PersistenceMode(PersistenceMode.Attribute)]
        public string NavigateUrl
        {
            get
            {
                object navigateUrlAddress = this.NavigateUrlAddress;
                if (navigateUrlAddress != null)
                {
                    return (string)navigateUrlAddress;
                }
                return string.Empty;
            }
            set
            {
                this.NavigateUrlAddress = value;
            }
        }

        [Browsable(false), PersistenceMode(PersistenceMode.InnerDefaultProperty), MergableProperty(false), DefaultValue((string)null), Category("Data")]
        public virtual OrgNodeCollection Nodes
        {
            get
            {
                return this.OrgNodeCollectionDemo;
            }
        }

        public object Parent
        {
            get
            {
                return this.ObjectDemo;
            }
            set
            {
                this.ObjectDemo = value;
            }
        }

        [DefaultValue(""), Category("Appearance"), PersistenceMode(PersistenceMode.Attribute)]
        public string Text
        {
            get
            {
                object tipText = this.TipText;
                if (tipText != null)
                {
                    return (string)tipText;
                }
                return string.Empty;
            }
            set
            {
                this.TipText = value;
            }
        }

        [PersistenceMode(PersistenceMode.Attribute), DefaultValue(""), Category("Data")]
        public string Type
        {
            get
            {
                if (this.TypeStyle != null)
                {
                    return this.TypeStyle;
                }
                return string.Empty;
            }
            set
            {
                this.TypeStyle = value;
            }
        }

        [Category("Data"), DefaultValue("文本显示横纵向"), PersistenceMode(PersistenceMode.Attribute)]
        public LabelLayoutFlow LayoutFlow
        {

            get
            {
                return this.lableLayOutFlow;
            }
            set
            {
                this.lableLayOutFlow = value;
            }
        }

        public enum LabelLayoutFlow
        {
            Vertical,
            Horizontal
        }
    }
}
