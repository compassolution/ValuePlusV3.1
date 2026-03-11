using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Com.ValuePlus.Web;
using Com.ValuePlus.Common;

public partial class AppFunction_SalaryCalculate_CalculateDealPage : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            try
            {
                ////解密传递字符串并获取对应参数值
                Hashtable htUrlQuery = UrlParamEncryption.DecryptionUrlParam(base.Request.Url.Query.ToString());
                String strPageUrl = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "pageUrl");

                this.frmOpPage.Attributes["src"] = strPageUrl;
                //this.frmOpPage.Attributes.Add("onload", "javascript:SetFrameHeight(this);");
            }
            catch (Exception ex)
            {
                log.Error(ex);
                Response.Write("<script language=\"javascript\">alert('" + "加载薪资计算事项处理信息失败" + "');</script>");
            }
        }
    }

    #region 页面按钮点击事件
    /// <summary>
    /// 返回流程处理按钮事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BackFlow_Click(object sender, EventArgs e) 
    {
        Response.Redirect("CalculateFlow.aspx", false);
    }

    #endregion

}
