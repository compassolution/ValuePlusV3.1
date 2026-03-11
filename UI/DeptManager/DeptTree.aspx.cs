using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Com.ValuePlus.Web;
using Com.ValuePlus.DAL;
using System.Resources;

public partial class DeptManager_DeptTree : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            try
            {
                this.aReload.Attributes.Add("onclick", "ShowWaitingDiv();");
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }
    }

    protected void Page_LoadComplete(object sender, EventArgs e)
    {
        Page.ClientScript.RegisterStartupScript(typeof(Page), "hideWaitingDiv", "<script language=\"javascript\">HideWaitingDiv();</script>");
    }

    /// <summary>
    /// 重新加载操作
    /// </summary>
    protected void aReload_Click(object sender, EventArgs e)
    {
        try
        {
            int iCount = SqlParamDao.ExcuteSP("[SP_InitDataToTB_HR_DEPT]",null);

            base.AlertMessageBox(this.Page, "Succussfully!");
            Response.Redirect("DeptTree.aspx");
        }
        catch (Exception ex)
        {
            log.Error(ex);
            log.Error("重新加载操作失败,方法aReload_Click");
        }
    }

}
