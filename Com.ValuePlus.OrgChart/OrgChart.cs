using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Com.ValuePlus.OrgChart
{
    [ParseChildren(true), ToolboxData("<{0}:OrgChart runat=server  ChartStyle=Horizontal></{0}:OrgChart>"), Designer(typeof(OrgChartDesigner))]
    public class OrgChart : WebControl
    {
        private string HttpContent;
        private string ImgFolder;
        private OrgNodeCollection OrgNodeCollectionDemo;
        private Color OrgNodeColor;
        private OrgNode OrgNodeDemo;
        private Orientation OrgNodeOrientation;
        private string OrgNodestring;
        private Unit OrgNodeUnit;

        private void CreatNodeStyle(OrgNode node1, Orientation orientation1)
        {
            this.HttpContent = "";
            try
            {
                object httpContent;
                object[] objArray;
                if (orientation1 == Orientation.Horizontal)
                {
                    this.HttpContent = this.HttpContent + "<table cellspacing=0 cellpadding=0 border=0 Width=100% height=>\r\n";
                    this.HttpContent = this.HttpContent + "    <tr>\r\n";
                    if (node1.Nodes.Count > 0)
                    {
                        httpContent = this.HttpContent;
                        objArray = new object[] { httpContent, "        <td rowspan=", node1.Nodes.Count, " valign=middle align=Center width=1 class=OrgChartCellPadding>\r\n" };
                        this.HttpContent = string.Concat(objArray);
                    }
                    else
                    {
                        this.HttpContent = this.HttpContent + "        <td valign=middle align=Center width=1 class=OrgChartCellPadding>\r\n";
                    }
                    this.HttpContent = this.HttpContent + this.OrgNodeHtmlImg(node1);
                    this.HttpContent = this.HttpContent + "        </td>\r\n";
                    if (node1.Nodes.Count > 0)
                    {
                        if (node1.Nodes.Count == 1)
                        {
                            httpContent = this.HttpContent;
                            objArray = new object[] { httpContent, "        <td rowspan=", node1.Nodes.Count, " align=right width=>", this.CreatTableStyleCenter(40, 1), "</td>\r\n" };
                            this.HttpContent = string.Concat(objArray);
                            httpContent = this.HttpContent;
                            objArray = new object[] { httpContent, "        <td rowspan=", node1.Nodes.Count, " align=left width=>\r\n" };
                            this.HttpContent = string.Concat(objArray);
                        }
                        else
                        {
                            httpContent = this.HttpContent;
                            objArray = new object[] { httpContent, "        <td rowspan=", node1.Nodes.Count, " align=right width=>", this.CreatTableStyleCenter(20, 1), "</td>\r\n" };
                            this.HttpContent = string.Concat(objArray);
                            httpContent = this.HttpContent;
                            objArray = new object[] { httpContent, "        <td rowspan=", node1.Nodes.Count, " align=left width=>\r\n" };
                            this.HttpContent = string.Concat(objArray);
                        }
                        this.StyleSelectStyle(node1, orientation1);
                        this.HttpContent = this.HttpContent + "        </td>\r\n";
                    }
                    this.HttpContent = this.HttpContent + "    </tr>\r\n";
                    this.HttpContent = this.HttpContent + "</table>\r\n";
                }
                else
                {
                    this.HttpContent = this.HttpContent + "<table cellspacing=0 cellpadding=0 border=0 Width=100%>\r\n";
                    this.HttpContent = this.HttpContent + "    <tr>\r\n";
                    if (node1.Nodes.Count > 0)
                    {
                        httpContent = this.HttpContent;
                        objArray = new object[] { httpContent, "        <td colspan=", node1.Nodes.Count, " valign=top align=Center width= class=orgChartCellPadding>\r\n" };
                        this.HttpContent = string.Concat(objArray);
                    }
                    else
                    {
                        this.HttpContent = this.HttpContent + "        <td valign=top align=Center width= class=orgChartCellPadding>\r\n";
                    }
                    this.HttpContent = this.HttpContent + this.OrgNodeHtmlImg(node1);
                    this.HttpContent = this.HttpContent + "        </td>\r\n</tr>\r\n";
                    this.HttpContent = this.HttpContent + "    </tr>\r\n";
                    if (node1.Nodes.Count > 0)
                    {
                        this.HttpContent = this.HttpContent + "    <tr>\r\n";
                        httpContent = this.HttpContent;
                        objArray = new object[] { httpContent, "        <td colspan=", node1.Nodes.Count, " align=Center>\r\n" };
                        this.HttpContent = string.Concat(objArray);
                        this.HttpContent = this.HttpContent + "            " + this.CreatTableStyleCenter(1, 20);
                        this.HttpContent = this.HttpContent + "        \r\n";
                        this.HttpContent = this.HttpContent + "        </td>\r\n";
                        this.HttpContent = this.HttpContent + "    </tr>\r\n";
                        this.StyleSelectStyle(node1, orientation1);
                    }
                    this.HttpContent = this.HttpContent + "</table>\r\n";
                }
            }
            catch (Exception exception)
            {
                string message = exception.Message;
            }
        }

        private string CreatTable(string text3, string text4)
        {
            string name = this.LineColor.Name;
            StringBuilder builder = new StringBuilder();
            string format = "<Table valign=bottom cellspacing=0 cellpadding=0 border=0 width={0} height={1}  style=\"border:1px solid  {2};\" bgcolor={2}><tr><td></td></tr></Table>";
            builder.AppendFormat(format, text3, text4, name);
            return builder.ToString();
        }

        private string CreatTableStyleCenter(int num1, int num2)
        {
            return this.CreatTable(num1.ToString(), num2.ToString());
        }

        private string CreatTableStyleColor(string text3, string text4)
        {
            string name = this.LineColor.Name;
            StringBuilder builder = new StringBuilder();
            string format = "<table cellspacing=0 cellpadding=0 border=0 width={0} height={1} ><tr><td height=50%></td></tr><tr><td width=1 height=50% style=\"border:1px solid  {2};\" bgcolor={2}></td></tr></Table>\r\n";
            builder.AppendFormat(format, text3, text4, name);
            return builder.ToString();
        }

        private string CreatTableStyleLeft(int num1, string text1)
        {
            return this.CreatTable(num1.ToString(), text1);
        }

        private string CreatTableStyleRight(string text1, int num1)
        {
            return this.CreatTable(text1, num1.ToString());
        }

        private string OrgNodeHtmlImg(OrgNode node1)
        {
            node1.ImageFolder = this.ImageFolder;
            return node1.OrgNodeHtml();
        }

        protected override void RenderContents(HtmlTextWriter output)
        {
            this.CreatNodeStyle(this.OrgNodeDemo, this.OrgNodeOrientation);
            if (this.HttpContent != null)
            {
                output.Write(this.HttpContent);
            }
        }

        private void StyleSelectStyle(OrgNode node1, Orientation orientation1)
        {
            if (orientation1 == Orientation.Horizontal)
            {
                this.HttpContent = this.HttpContent + "<table cellspacing=0 cellpadding=0 border=0 height='100%'>\r\n";
                for (int i = 0; i < node1.Nodes.Count; i++)
                {
                    this.HttpContent = this.HttpContent + "    <tr>\r\n";
                    if (node1.Nodes.Count > 1)
                    {
                        if (i == 0)
                        {
                            this.HttpContent = this.HttpContent + "        <td width=1 align=Right vAlign=bottom>" + this.CreatTableStyleColor("1", "100%") + "</td>\r\n";
                        }
                        else if (i == (node1.Nodes.Count - 1))
                        {
                            this.HttpContent = this.HttpContent + "        <td width=1 align=Right vAlign=top>" + this.CreatTableStyleLeft(1, "50%") + "</td>\r\n";
                        }
                        else
                        {
                            this.HttpContent = this.HttpContent + "        <td width=1 align=Right vAlign=bottom>" + this.CreatTableStyleLeft(1, "100%") + "</td>\r\n";
                        }
                        this.HttpContent = this.HttpContent + "        <td align=Left vAlign=middle>" + this.CreatTableStyleCenter(20, 1) + "</td>\r\n";
                        this.HttpContent = this.HttpContent + "        <td align=Left vAlign=middle>" + this.OrgNodeHtmlImg(node1.Nodes[i]) + "</td>\r\n";
                    }
                    else
                    {
                        this.HttpContent = this.HttpContent + "        <td align=Left vAlign=middle>" + this.OrgNodeHtmlImg(node1.Nodes[i]) + "</td>\r\n";
                    }
                    if (node1.Nodes[i].Nodes.Count > 0)
                    {
                        this.HttpContent = this.HttpContent + "        <td align=Left vAlign=middle>\r\n";
                        this.HttpContent = this.HttpContent + "            <table cellspacing=0 cellpadding=0 align=Left border=0>\r\n";
                        this.HttpContent = this.HttpContent + "                <tr>\r\n";
                        this.HttpContent = this.HttpContent + "                    <td colspan=0 valign=middle align=Left width=20 class=OrgChartCellPadding>\r\n";
                        if (node1.Nodes[i].Nodes.Count == 1)
                        {
                            this.HttpContent = this.HttpContent + "                        " + this.CreatTableStyleCenter(20, 1);
                        }
                        else
                        {
                            this.HttpContent = this.HttpContent + "                        " + this.CreatTableStyleCenter(20, 1);
                        }
                        this.HttpContent = this.HttpContent + "                    \r\n";
                        this.HttpContent = this.HttpContent + "                    </td>\r\n";
                        this.HttpContent = this.HttpContent + "                    <td>\r\n";
                        this.StyleSelectStyle(node1.Nodes[i], orientation1);
                        this.HttpContent = this.HttpContent + "                    </td>\r\n";
                        this.HttpContent = this.HttpContent + "                </tr>\r\n";
                        this.HttpContent = this.HttpContent + "            </table>\r\n";
                        this.HttpContent = this.HttpContent + "        </td>\r\n";
                    }
                    this.HttpContent = this.HttpContent + "    </tr>\r\n";
                }
                this.HttpContent = this.HttpContent + "</table>\r\n";
            }
            else
            {
                this.HttpContent = this.HttpContent + "<table cellspacing=0 cellpadding=0 border=0 Width=100%>\r\n";
                if (node1.Nodes.Count > 1)
                {
                    this.HttpContent = this.HttpContent + "    <tr>\r\n";
                    this.HttpContent = this.HttpContent + "        <td height=1 align=Right>" + this.CreatTableStyleRight("50%", 1) + "</td>\r\n";
                    for (int k = 1; k < (node1.Nodes.Count - 1); k++)
                    {
                        this.HttpContent = this.HttpContent + "        <td height=1 align=Right>" + this.CreatTableStyleRight("100%", 1) + "</td>\r\n";
                    }
                    this.HttpContent = this.HttpContent + "        <td height=1 align=Left>" + this.CreatTableStyleRight("50%", 1) + "</td>\r\n";
                    this.HttpContent = this.HttpContent + "    </tr>\r\n";
                    this.HttpContent = this.HttpContent + "    <tr>\r\n";
                    for (int m = 0; m < node1.Nodes.Count; m++)
                    {
                        this.HttpContent = this.HttpContent + "        <td align=Center>" + this.CreatTableStyleCenter(1, 40) + "</td>\r\n";
                    }
                    this.HttpContent = this.HttpContent + "    </tr>\r\n";
                }
                this.HttpContent = this.HttpContent + "    <tr>\r\n";
                for (int j = 0; j < node1.Nodes.Count; j++)
                {
                    this.HttpContent = this.HttpContent + "        <td valign=top >\r\n";
                    this.HttpContent = this.HttpContent + this.OrgNodeHtmlImg(node1.Nodes[j]);
                    if (node1.Nodes[j].Nodes.Count > 0)
                    {
                        this.HttpContent = this.HttpContent + "            <table cellspacing=0 cellpadding=0 align=center>\r\n";
                        this.HttpContent = this.HttpContent + "                <tr>\r\n";
                        this.HttpContent = this.HttpContent + "                    <td colspan=0 valign=top align=Center width=100% class=OrgChartCellPadding>\r\n";
                        this.HttpContent = this.HttpContent + "                        " + this.CreatTableStyleCenter(1, 20);
                        this.StyleSelectStyle(node1.Nodes[j], orientation1);
                        this.HttpContent = this.HttpContent + "                    \r\n";
                        this.HttpContent = this.HttpContent + "                    </td>\r\n";
                        this.HttpContent = this.HttpContent + "                </tr>\r\n";
                        this.HttpContent = this.HttpContent + "            </table>\r\n";
                    }
                    this.HttpContent = this.HttpContent + "        </td>\r\n";
                }
                this.HttpContent = this.HttpContent + "    </tr>\r\n";
                this.HttpContent = this.HttpContent + "</table>\r\n";
            }
        }

        [Bindable(true), Category("Appearance"), Description("机构图的扩展方向是垂直的还是水平的"), DefaultValue("")]
        public Orientation ChartStyle
        {
            get
            {
                return this.OrgNodeOrientation;
            }
            set
            {
                this.OrgNodeOrientation = value;
            }
        }

        [DefaultValue(""), Category("Appearance"), Description("存放图片的目录"), Bindable(true)]
        public string ImageFolder
        {
            get
            {
                if (this.ImgFolder == null)
                {
                    this.ImgFolder = "images/";
                }
                this.ImgFolder = this.ImgFolder.Trim();
                if (!this.ImgFolder.EndsWith(@"\") && !this.ImgFolder.EndsWith("/"))
                {
                    this.ImgFolder = this.ImgFolder + "/";
                }
                return this.ImgFolder;
            }
            set
            {
                this.ImgFolder = value;
            }
        }

        [DefaultValue(""), Description("机构图连线的颜色"), Category("Appearance")]
        public Color LineColor
        {
            get
            {
                if (this.OrgNodeColor.IsEmpty)
                {
                    this.OrgNodeColor = Color.Blue;
                }
                return this.OrgNodeColor;
            }
            set
            {
                this.OrgNodeColor = value;
            }
        }

        [DefaultValue("1px"), Category("Appearance"), Description("机构图连线的宽度")]
        public Unit LineWidth
        {
            get
            {
                return this.OrgNodeUnit;
            }
            set
            {
                this.OrgNodeUnit = value;
            }
        }

        public OrgNode Node
        {
            get
            {
                return this.OrgNodeDemo;
            }
            set
            {
                this.OrgNodeDemo = value;
            }
        }

        public enum ExpandableValue
        {
            Always,
            Auto,
            CheckOnce
        }

        public enum Orientation
        {
            Vertical,
            Horizontal
        }
    }
}
