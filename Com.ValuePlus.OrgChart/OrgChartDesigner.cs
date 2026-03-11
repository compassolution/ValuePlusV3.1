using System;
using System.IO;
using System.Web.UI;
using System.Web.UI.Design;

namespace Com.ValuePlus.OrgChart
{
    public class OrgChartDesigner : ControlDesigner
    {
        private OrgChart OrgChartDemo;

        public OrgChartDesigner()
        {
            base.ReadOnly = true;
        }

        public override string GetDesignTimeHtml()
        {
            StringWriter writer = new StringWriter();
            HtmlTextWriter writer2 = new HtmlTextWriter(writer);
            this.OrgChartDemo = (OrgChart)base.Component;
            this.OrgChartDemo.RenderControl(writer2);
            return writer.ToString();
        }

        protected override string GetEmptyDesignTimeHtml()
        {
            return this.GetDesignTimeHtml();
        }

        protected override string GetErrorDesignTimeHtml(Exception e)
        {
            string instruction = "创建控件时出错：" + e.Message;
            return base.CreatePlaceHolderDesignTimeHtml(instruction);
        }
    }
}
