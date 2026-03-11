using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Com.ValuePlus.Web;
using Com.ValuePlus.Common.Config;
using Com.ValuePlus.Common;
using System.Resources;
using Com.ValuePlus.BLL.User;
using Com.ValuePlus.DataLog;
using Com.ValuePlus.DataLog.Enum;
using Com.ValuePlus.Entity.Regist;
using Com.ValuePlus.BLL.Regist;
using Com.ValuePlus.Utils.Session;

public partial class Regist_License : PageBase
{
    public string IsAlertSysUpdate;
    public string strLbUpdateAlert;

    protected void Page_Load(object sender, EventArgs e)
    {
        IsAlertSysUpdate = Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("IsAlertSysUpdate");
        ResourceManager rmLocResourceManager = base.GetResourceManager("SysUpdate");
        strLbUpdateAlert = rmLocResourceManager.GetString("lbUpdateAlert");

        this.GetLicenseInfo();
    }

    private void GetLicenseInfo()
    {
        ResourceManager rmLocResourceManager = base.GetResourceManager("License");
        this.Label_Name.Text = rmLocResourceManager.GetString("Label_Name");
        this.Label_Date.Text = rmLocResourceManager.GetString("Label_Date");
        try
        {
            RegistBll bllLicense = new RegistBll();
            //LicenseEntity entity = (LicenseEntity)RegistBll.LicenseInfo;
            //不从session中读取 modify by sammen 20210605
            LicenseEntity entity = bllLicense.GetLicenseInfo();
            if (entity != null)
            {
                String strName = "";
                DateTime dtValidDate = DateTime.Now;
                String strProductName = "";
                String strVersion = "";

                if (this.Language.Equals("en-us"))
                {
                    strName = entity.strClientName;
                }
                else
                {
                    strName = entity.strClientNameChs;
                }
                strProductName = entity.strProductName;
                strVersion = entity.strVersion;
                dtValidDate = entity.dtValid;

                this.lb_Product.Text = strProductName + "  " + strVersion;
                this.lb_Name.Text = strName;
                this.lb_Date.Text = dtValidDate.Date.ToString("yyyy-MM-dd");

                this.tdValid.Visible = true;
                //判断有效期是否小于提醒期限
                int iDiff = dtValidDate.Subtract(DateTime.Now).Days;
                int iWarningDay = int.Parse(Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("iLicenseWarningDay"));

                if (iDiff < iWarningDay)//小于
                {
                    this.tdInvalid.Visible = true;
                    this.Label_Invalid.Text = rmLocResourceManager.GetString("Label_LicenseTip") + iDiff.ToString();
                    if (iDiff < 0)
                    {
                        this.Label_Invalid.Text = rmLocResourceManager.GetString("Label_LicenseExpired");
                        //退出系统
                        this.LogoutSystem();
                    }
                    this.Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "<script language=javascript>showWarningPage('" + this.Label_Invalid.Text + "');</script>");
                }
                else
                {
                    this.tdInvalid.Visible = false;
                }

            }
            else
            {
                this.Label_Invalid.Text = rmLocResourceManager.GetString("Label_NoLicense");
                this.tdValid.Visible = false;
                this.tdInvalid.Visible = true;
                //this.Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "<script language=javascript>showWarningPage('"+ this.Label_Invalid.Text + "');</script>"); 
                //退出系统
                this.LogoutSystem();
            }

        }
        catch (Exception ex)
        {
            this.Label_Invalid.Text = rmLocResourceManager.GetString("Label_NoLicense");
            log.Error("本软件系统获取使用许可时出错！\r\n");
            log.Error(ex.Message.ToString());
        }
    }

    /// <summary>
    /// 退出系统
    /// </summary>
    private void LogoutSystem()
    {
        UserBll bllUser = new UserBll();
        try
        {
            String strUserId = base.GetUserCode();
            int iCount = bllUser.ExitLogin(strUserId);

            //注销日志
            DataLogWriter.Log_LogOut(this.GetUserCode(), Com.ValuePlus.Utils.RequestUtils.GetIP(), LogActionType.LogOut_Success);
            //清除Cookie同时清除所有session
            UserLoginBll.AbandomSession();

            //如果是单点登录的 add by sammen 20170901
            if ((SessionHelper.GetSession(CacheName.DBConnectSessionName) != null) && (!String.IsNullOrEmpty(SessionHelper.GetSession(CacheName.DBConnectSessionName).ToString())))
            {
                Com.ValuePlus.Utils.Session.SessionHelper.RemoveSession(CacheName.DBConnectSessionName);
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "", "<script language=javascript>alert('" + this.Label_Invalid.Text + "');window.parent.location.href = '../SSO.aspx';</script>");
            }
            else
            {
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "", "<script language=javascript>alert('" + this.Label_Invalid.Text + "');window.parent.location.href = '../login.aspx';</script>");
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
    }

}
