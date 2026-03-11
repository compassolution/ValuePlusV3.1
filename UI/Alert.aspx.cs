using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Com.ValuePlus.Archive.BLL;
using System.Resources;

public partial class Alert : Com.ValuePlus.Web.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            this.aToDealAlert.Attributes.Add("onclick", "return false;");
            ResourceManager rmLocResourceManager = base.GetResourceManager("Alert");
            this.aToDealAlert.Text = rmLocResourceManager.GetString("aDealAlert");
            this.lbPendingList.Text = rmLocResourceManager.GetString("lbPendingList");
            try
            {
                int iCount = ArchiveAlertBll.GetAlertDetailCount(this.GetUserCode(),this.Language, false);
                this.lbPendingCount.Text = iCount.ToString();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.Error("Alert.ASPX.Page_Load error!");
            }
        }

    }
}