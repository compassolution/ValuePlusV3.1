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
using System.Reflection;
using Com.ValuePlus.Web;
using Com.ValuePlus.BLL.SysManager;
using Com.ValuePlus.Entity;
using Com.ValuePlus.DataLog;
using Com.ValuePlus.DataLog.Enum;
using Com.ValuePlus.Common;
using Com.ValuePlus.BLL.User;
using Com.ValuePlus.DAL;
using System.Text;
using Com.ValuePlus.Utils.Session;
using Com.ValuePlus.SysParams;

public partial class UserManager_ChangerPwd : System.Web.UI.Page
{
    protected ResourceManager rmLocResourceManager;
    /// <summary>
    /// 日志声明
    /// </summary>
    private static Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            ////解密传递字符串并获取对应参数值
            ////此种情况是在登录时要求修改密码的情况下进入（从login.aspx跳转而来）
            Hashtable htUrlQuery = UrlParamEncryption.DecryptionUrlParam(base.Request.Url.Query.ToString());
            this.strUserId = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "strUserId");
            this.IsForceChange = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "tipType");

            if (String.IsNullOrEmpty(this.strUserId))
            {
                UserInfo infoUser = UserLoginBll.LoginUserInfo;
                this.strUserId = infoUser.SUSERID;
            }
            //获取旧密码
            String strSql = "select dbo.Fun_Decode_Password(SPWD) AS SPWD from TB_HR_USER WHERE SUSERID = '" + strUserId + "'";
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
            if ((dt != null) && (dt.Rows.Count > 0))
            {
                this.strOldPassword = dt.Rows[0]["SPWD"].ToString();
            }

            //获取密码长度最大最小的配置
            String strPasswordMinLength = BaseParamsGetter.GetBasicParamValue("iPasswordMinLength");
            strPasswordMinLength = String.IsNullOrEmpty(strPasswordMinLength) ? "8" : strPasswordMinLength;
            String strPasswordMaxLength = BaseParamsGetter.GetBasicParamValue("iPasswordMaxLength");
            strPasswordMaxLength = String.IsNullOrEmpty(strPasswordMaxLength) ? "16" : strPasswordMaxLength;
            this.hfMinPasswordLength.Value = strPasswordMinLength;
            this.hfMaxPasswordLength.Value = strPasswordMaxLength;

            this.txtLoginName.Text = this.strUserId;

            ResourceManager rmLocResourceManager = GetResourceManager("ChangePwd");
            this.lbLoginName.Text = rmLocResourceManager.GetString("lbLoginName");
            this.strTipFailed = rmLocResourceManager.GetString("tipFailed");
            this.strTipSuccess = rmLocResourceManager.GetString("tipSuccess");
            this.strTipPwdRule = rmLocResourceManager.GetString("tipPwdRule");
            this.strTipPwdRule = this.strTipPwdRule.Replace("8", strPasswordMinLength).Replace("16", strPasswordMaxLength);
            this.strTipNewNotMatch = rmLocResourceManager.GetString("tipNewNotMatch");
            this.strTipNewCantMatchOld = rmLocResourceManager.GetString("tipNewCantMatchOld");
            this.strTipOldNotRight = rmLocResourceManager.GetString("tipOldNotRight");
            this.strTipNotRepeat5His = rmLocResourceManager.GetString("tipNotRepeat5His");

            ////自定义设置页面文字显示的中英文字符串
            this.lbLoginName.Text = rmLocResourceManager.GetString("lbLoginName");
            this.lbOldPwd.Text = rmLocResourceManager.GetString("lbOldPwd");
            this.lbNewPwd.Text = rmLocResourceManager.GetString("lbNewPwd");
            this.lbNewPwdConfirm.Text = rmLocResourceManager.GetString("lbConfirmPwd");
            this.lbPwdTips1.Text = this.strTipPwdRule;
            this.lbPwdTips2.Text = this.strTipPwdRule;
            this.btnChangePwd.Text = rmLocResourceManager.GetString("btnChangePwd");
            this.btnReset.Text = rmLocResourceManager.GetString("btnReset");
            this.btnReset.Attributes.Add("onclick", "document.form1.reset();return false;");
            this.btnChangePwd.Attributes.Add("onclick", "return checkInput();");
            this.btnToLoginPage.Text = rmLocResourceManager.GetString("btnReLogin");

            this.hfOldPwd.Value = this.strOldPassword;
            this.hfWarnTip1.Value = rmLocResourceManager.GetString("warnTip1");
            this.hfWarnTip2.Value = rmLocResourceManager.GetString("warnTip2");
            this.hfWarnTip2.Value = this.hfWarnTip2.Value.Replace("8", strPasswordMinLength).Replace("16", strPasswordMaxLength);
            this.hfWarnTip3.Value = rmLocResourceManager.GetString("warnTip3");
            this.hfWarnTip4.Value = rmLocResourceManager.GetString("tipOldNotRight");
            this.hfWarnTip5.Value = rmLocResourceManager.GetString("tipNewCantMatchOld");

            if (!String.IsNullOrEmpty(this.IsForceChange))
            {
                this.btnToLoginPage.Visible = true;

                if (this.IsForceChange.Equals("1"))
                {
                    this.lbTips.Text = rmLocResourceManager.GetString("Tip_NeedChangePassword_FirstLogin");
                }
                else if (this.IsForceChange.Equals("2"))
                {
                    this.lbTips.Text = rmLocResourceManager.GetString("Tip_NeedChangePassword_Expire");
                }
            }
            else
            {
                this.btnToLoginPage.Visible = false;
                this.lbTips.Text = rmLocResourceManager.GetString("lbChangePwd");
            }

        }

    }

    #region viewstate初始化区域
    private string IsForceChange
    {
        get
        {
            return ViewState["changePwd_IsForceChange_ViewState"] as string;
        }
        set
        {
            ViewState["changePwd_IsForceChange_ViewState"] = value;
        }
    }
    private string strUserId
    {
        get
        {
            return ViewState["changePwd_strUserId_ViewState"] as string;
        }
        set
        {
            ViewState["changePwd_strUserId_ViewState"] = value;
        }
    }
    private string strOldPassword
    {
        get
        {
            return ViewState["changePwd_strOldPassword_ViewState"] as string;
        }
        set
        {
            ViewState["changePwd_strOldPassword_ViewState"] = value;
        }
    }
    private string strTipFailed
    {
        get
        {
            return ViewState["changePwd_strTipFailed_ViewState"] as string;
        }
        set
        {
            ViewState["changePwd_strTipFailed_ViewState"] = value;
        }
    }
    private string strTipSuccess
    {
        get
        {
            return ViewState["changePwd_strTipSuccess_ViewState"] as string;
        }
        set
        {
            ViewState["changePwd_strTipSuccess_ViewState"] = value;
        }
    }
    private string strTipPwdRule
    {
        get
        {
            return ViewState["changePwd_strTipPwdRule_ViewState"] as string;
        }
        set
        {
            ViewState["changePwd_strTipPwdRule_ViewState"] = value;
        }
    }
    private string strTipNewNotMatch
    {
        get
        {
            return ViewState["changePwd_strTipNewNotMatch_ViewState"] as string;
        }
        set
        {
            ViewState["changePwd_strTipNewNotMatch_ViewState"] = value;
        }
    }
    private string strTipNewCantMatchOld
    {
        get
        {
            return ViewState["changePwd_strTipNewCantMatchOld_ViewState"] as string;
        }
        set
        {
            ViewState["changePwd_strTipNewCantMatchOld_ViewState"] = value;
        }
    }
    private string strTipOldNotRight
    {
        get
        {
            return ViewState["changePwd_strTipOldNotRight_ViewState"] as string;
        }
        set
        {
            ViewState["changePwd_strTipOldNotRight_ViewState"] = value;
        }
    }
    private string strTipNotRepeat5His
    {
        get
        {
            return ViewState["changePwd_strTipNotRepeat5His_ViewState"] as string;
        }
        set
        {
            ViewState["changePwd_strTipNotRepeat5His_ViewState"] = value;
        }
    }
    #endregion

    /// <summary>
    /// 执行修改密码
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnChangePwd_Click(object sender, EventArgs e)
    {
        String strNewPwd = this.txtNewPwd.Text.ToString();

        UserManagerBll bllUser = new UserManagerBll();
        int iCount = 0;

        try
        {
            //判断新密码不能与最近5次修改过的密码中任意一条重复
            //add by sammen 20150604
            string strTimer = Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("Times_SameAsLastPwd");
            int iTimer = 5;
            if (!String.IsNullOrEmpty(strTimer))
            {
                iTimer = int.Parse(strTimer);
            }

            StringBuilder sbSql_Repeat = new StringBuilder();
            sbSql_Repeat.Append("SELECT COUNT(*) FROM (");
            sbSql_Repeat.Append(" SELECT top "+ iTimer .ToString()+ " * FROM [TB_HR_CHANGEPWD_HIS] where SUSERID = '" + strUserId + "' order by DTCHANGEDATE");
            sbSql_Repeat.Append(") A WHERE dbo.fun_decode_password(A.[SNEWPWD]) = '" + strNewPwd + "'");
            String strSql_Repeat = sbSql_Repeat.ToString();
            int iCount_Repeat = SqlParamDao.ExecuteScalarBySql(strSql_Repeat);
            if (iCount_Repeat > 0)
            {
                this.lbTips.Text = this.strTipNotRepeat5His.Replace("5", iTimer.ToString());
                return;
            }


            iCount = bllUser.ChangeUserPwd(strNewPwd, strUserId);
            if (iCount > 0)
            {
                //Com.ValuePlus.BLL.User.LogHandle.register(base.GetUserAccount(), "10", Com.ValuePlus.Utils.RequestUtils.GetIP(), "10-3");
                DataLogWriter.Log_Login(this.strUserId, Com.ValuePlus.Utils.RequestUtils.GetIP(), LogActionType.Login_ChangePassword);
                this.lbTips.Text = this.strTipSuccess;
                //this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "", "<script language=javascript>alert('" + this.strTipSuccess + "');</script>");

                //写修改密码记录
                String strSpName = "USP_Sys_RecordPasswordHis";
                Hashtable hsTableParam = new Hashtable();
                hsTableParam.Add("UserId", this.strUserId);
                hsTableParam.Add("OldPwd", this.strOldPassword);
                hsTableParam.Add("NewPwd", strNewPwd);
                hsTableParam.Add("OPUserIP", Com.ValuePlus.Utils.RequestUtils.GetIP());//操作用户IP

                try
                {
                    iCount = SqlParamDao.ExcuteSP(strSpName, hsTableParam);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }

                this.strOldPassword = strNewPwd;
                this.hfOldPwd.Value = this.strOldPassword;

            }
            else
            {
                //Com.ValuePlus.BLL.User.LogHandle.register(base.GetUserAccount(), "10", Com.ValuePlus.Utils.RequestUtils.GetIP(), "10-4");
                DataLogWriter.Log_Login(this.strUserId, Com.ValuePlus.Utils.RequestUtils.GetIP(), LogActionType.Login_ChangePasswordFailed);
                //this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "", "<script language=javascript>alert('" + this.strTipFailed + "');</script>");
                this.lbTips.Text = this.strTipSuccess;
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
            //this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "", "<script language=javascript>alert('" + this.strTipFailed + "');</script>");
            this.lbTips.Text = this.strTipSuccess;
        }
    }
    
    /// <summary>
    /// 返回登录界面
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnToLoginPage_Click(object sender, EventArgs e)
    {
        //清除Cookie同时清除所有session
        UserLoginBll.AbandomSession();

        //如果是单点登录的 add by sammen 20170901
        if ((SessionHelper.GetSession(CacheName.DBConnectSessionName) != null) && (!String.IsNullOrEmpty(SessionHelper.GetSession(CacheName.DBConnectSessionName).ToString())))
        {
            Com.ValuePlus.Utils.Session.SessionHelper.RemoveSession(CacheName.DBConnectSessionName);
            Response.Redirect("../SSO.aspx");
        }
        else
        {
            Response.Redirect("../login.aspx");
        }
    }

    #region 读取资源文件
    /// <summary>
    /// 读取资源文件
    /// </summary>
    /// <param name="resoucename">直接文件名，不包含zh-cn例如：资源文件名字为:login.zh-cn.resx,只需要传递login就可以了</param>
    /// <returns></returns>
    public ResourceManager GetResourceManager(string resoucename)
    {
        //先设置文化
        UserLoginBll.SetCulture();
        //获取异常语句提示
        ResourceManager sR = new ResourceManager(string.Format("Resources.{0}", resoucename), Assembly.Load("App_GlobalResources"));
        return sR;
    }
    #endregion

}
