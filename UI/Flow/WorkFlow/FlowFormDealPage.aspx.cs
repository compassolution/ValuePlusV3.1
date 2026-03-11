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
using Com.ValuePlus.Flow.BLL.Flow;
using Com.ValuePlus.Common;
using Com.ValuePlus.Flow.Entity;
using Com.ValuePlus.Flow.Config;

public partial class Flow_WorkFlow_FlowFormDealPage : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            try
            {
                ////解密传递字符串并获取对应参数值
                Hashtable htUrlQuery = UrlParamEncryption.DecryptionUrlParam(base.Request.Url.Query.ToString());
                this.strCurFlowId = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "flowId");
                this.strCurPostCode = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "postId");
                this.strPageUrl = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "pageUrl");

                this.frmOpPage.Attributes["src"] = strPageUrl;
                //this.frmOpPage.Attributes.Add("onload", "javascript:SetFrameHeight(this);");
            }
            catch (Exception ex)
            {
                log.Error(ex);
                Response.Write("<script language=\"javascript\">alert('" + "加载当前流程表单处理信息失败" + "');</script>");
            }
        }

    }

    #region viewstate初始化区域
    private string strCurFlowId
    {
        get
        {
            return ViewState["strCurFlowId_ViewState"] as string;
        }
        set
        {
            ViewState["strCurFlowId_ViewState"] = value;
        }
    }
    private string strCurPostCode
    {
        get
        {
            return ViewState["strCurPostCode_ViewState"] as string;
        }
        set
        {
            ViewState["strCurPostCode_ViewState"] = value;
        }
    }
    private string strPageUrl
    {
        get
        {
            return ViewState["strPageUrl_ViewState"] as string;
        }
        set
        {
            ViewState["strPageUrl_ViewState"] = value;
        }
    }
    #endregion

    #region 页面按钮点击事件
    /// <summary>
    /// 返回按钮事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Back_Click(object sender, EventArgs e)
    {
        String strParam = "flowId=" + this.strCurFlowId;
        Response.Redirect("FlowDealPage.aspx?" + UrlParamEncryption.EncryptionUrlParam(strParam),false);
    }
    #endregion

}
