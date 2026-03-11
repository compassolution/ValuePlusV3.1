using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.SessionState;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Resources;
using Com.ValuePlus.Common.Config;

public partial class WinForm_StaffPayFrm : Com.ValuePlus.Web.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            ResourceManager rmLocResourceManager = base.GetResourceManager("FrmStaffPay");
            this.hfLanguage.Value = this.Language;
            this.hfTitle.Value = rmLocResourceManager.GetString("tipTitle");
            this.hfBtnSave.Value = rmLocResourceManager.GetString("btnSave");
            this.hfTipGridColumnsText.Value = rmLocResourceManager.GetString("tipGridColumnsText");
            this.hfTipGridSortAscText.Value = rmLocResourceManager.GetString("tipGridSortAscText");
            this.hfTipGridSortDescText.Value = rmLocResourceManager.GetString("tipGridSortDescText");
            this.hfTipMsg.Value = rmLocResourceManager.GetString("tipMsg");
            this.hfTipNoSave.Value = rmLocResourceManager.GetString("tipNoSave");
            this.hfTipSaveMsg.Value = rmLocResourceManager.GetString("tipSaveSuccess");
        }
    }
}
