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
using Com.ValuePlus.BLL.Regist;
using Com.ValuePlus.Entity.Regist;

public partial class Regist_CopyrightInfo : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {

            ResourceManager rmLocResourceManager = base.GetResourceManager("RegistCopyright");
            this.Label1.Text = rmLocResourceManager.GetString("lbAssignInfo");
            this.Label2.Text = rmLocResourceManager.GetString("lbAssignTo");
            this.Label3.Text = rmLocResourceManager.GetString("lbAssignCode");
            this.Label4.Text = rmLocResourceManager.GetString("lbAssignDate");
            this.Button1.Text = rmLocResourceManager.GetString("btnRegist");

            this.strTipSuccessAssign = rmLocResourceManager.GetString("tipSuccessAssign");
            this.strTipHavaNoAssign = rmLocResourceManager.GetString("tipHavaNoAssign");
            //加载注册授权信息
            this.LoadRegistInfo();
        }
    }

    #region viewstate初始化区域
    private string strTipSuccessAssign
    {
        get
        {
            return ViewState["Regist_strTipSuccessAssign_ViewState"] as string;
        }
        set
        {
            ViewState["Regist_strTipSuccessAssign_ViewState"] = value;
        }
    }
    private string strTipHavaNoAssign
    {
        get
        {
            return ViewState["Regist_strTipHavaNoAssign_ViewState"] as string;
        }
        set
        {
            ViewState["Regist_strTipHavaNoAssign_ViewState"] = value;
        }
    }
    #endregion

    #region 加载注册授权信息
    /// <summary>
    /// 加载注册授权信息
    /// </summary>
    private void LoadRegistInfo()
    {
        RegistBll bllRegist = new RegistBll();
        RegistInfoEntity entityRegist = bllRegist.GetRegistEntityInfo();

        if (bllRegist.IsHaveRecord())
        {
            this.Label_ClientName.Text = entityRegist.strSCLIENTNAME.ToString();
            this.Label_AssignStr.Text = entityRegist.strSASSIGNSTR.ToString();
            this.Label_AssignDate.Text = entityRegist.dtDTREGISTDATA.ToShortDateString().ToString();

            this.Label5.Text = this.strTipSuccessAssign;
            this.divButton.Style["display"] = "none";
        }
        else
        {
            this.Label5.Text = this.strTipHavaNoAssign;
            this.divButton.Style["display"] = "";
        }
    }
    #endregion

    /// <summary>
    /// 授权转向
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Button1_Click(object sender, EventArgs e)
    {
        Response.Redirect("RegistPage.aspx");
    }

}
