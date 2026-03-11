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
using System.Threading;
using System.Resources;
using Com.ValuePlus.Web;
using Com.ValuePlus.BLL.SysManager;
using Com.ValuePlus.Common.Security;

public partial class UserManager_SetUserMenu : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        String strUserId = Request.Params["userId"].ToString();
        //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
        strUserId = SQLInjectionDefense.ReplaceSQLReservedKeyword(strUserId);

        this.hfUserId.Value = strUserId;
        this.lbUserId.Text = strUserId;

        if (!Page.IsPostBack)
        {
            ResourceManager rmLocResourceManager = base.GetResourceManager("UserManager");
            this.Button1.Text = rmLocResourceManager.GetString("btnAssign");
            this.Button1.Attributes.Add("onclick", "getMenuArr()");
            this.Button2.Text = rmLocResourceManager.GetString("btnSelAll");
            this.Button3.Text = rmLocResourceManager.GetString("btnUnSelAll");
            //全选全不选按钮点击不刷新
            this.Button2.Attributes.Add("onclick", "return false;");
            this.Button3.Attributes.Add("onclick", "return false;");
            this.Button4.Attributes.Add("onclick", "return false;");

            this.strTip1 = rmLocResourceManager.GetString("Tip6");
            this.strErr3 = rmLocResourceManager.GetString("Err3");
            this.strSuccessTip = rmLocResourceManager.GetString("Success4");
            
        }
    }

    #region viewstate初始化区域
    private string strTip1
    {
        get
        {
            return ViewState["userMenu_strTip1_ViewState"] as string;
        }
        set
        {
            ViewState["userMenu_strTip1_ViewState"] = value;
        }
    }
    private string strErr3
    {
        get
        {
            return ViewState["userMenu_strErr3_ViewState"] as string;
        }
        set
        {
            ViewState["userMenu_strErr3_ViewState"] = value;
        }
    }
    private string strSuccessTip
    {
        get
        {
            return ViewState["userMenu_strSuccessTip_ViewState"] as string;
        }
        set
        {
            ViewState["userMenu_strSuccessTip_ViewState"] = value;
        }
    }
    #endregion

    #region 用户栏目授权操作
    /// <summary>
    /// 用户栏目授权操作
    /// </summary>
    protected void Button1_Click(object sender, EventArgs e)
    {
        String strUserId = this.hfUserId.Value;
        String str = this.hfMenuCodeArr.Value;

        String[] strMenuCode = null;
        if (!String.IsNullOrEmpty(str))
        { 
            strMenuCode = str.Split(',');
        }
        try
        {
            if (!String.IsNullOrEmpty(strUserId))
            {
                UserManagerBll bllUserManager = new UserManagerBll();
                bllUserManager.AssignUserMenu(strUserId, strMenuCode);
                this.AlertMessageBox(this.Page, this.strSuccessTip);
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
            this.AlertMessageBox(this.Page, this.strErr3);
        }
        
        
    }
    #endregion

}
