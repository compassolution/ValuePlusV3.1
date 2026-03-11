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
using System.Resources;
using System.Threading;
using System.Globalization;
using System.Reflection;
using Com.ValuePlus.BLL.User;
using Com.ValuePlus.Entity;
using Com.ValuePlus.DataLog;
using Com.ValuePlus.DataLog.Enum;
using Com.ValuePlus.Web;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Common;
using Com.ValuePlus.BLL.SysManager;
using Com.ValuePlus.Utils.Session;
using Com.ValuePlus.BLL.Regist;
using Com.ValuePlus.Utils;
using Com.ValuePlus.Common.Security;

public partial class login : System.Web.UI.Page
{
    protected HttpCookie httpCookie;
    protected string strHttpCookieName;
    protected ResourceManager rmLocResourceManager;
    /// <summary>
    /// 日志声明
    /// </summary>
    private static Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    protected void Page_Load(object sender, EventArgs e)
    {
        //首先清空单点登录的数据库链接
        SessionHelper.RemoveSession(CacheName.DBConnectSessionName);

        //获取域及当前域账号
        this.GetDomainInfo();

        rmLocResourceManager = this.GetResourceManager("Login");
        string sExceptionTip = Com.ValuePlus.Common.Exception.ExceptionInfo.GetExceptionTipInfo("DataExcetion");
    }

    /// <summary>
    /// 获取域及当前域账号
    /// </summary>
    private void GetDomainInfo()
    {
        //log.Error("准备获取域及当前域账号....");
        try
        {
            this.tr_DomainArea.Visible = false;
            string domainAndName = Page.User.Identity.Name;
            //string domainAndName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;//iis应用服务器连接池信息


            string[] infoes = domainAndName.Split(new char[1] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
            string userDomainName = "";
            string userName = "";
            if (infoes.Length > 1)
            {
                this.tr_DomainArea.Visible = true;
                userDomainName = infoes[0];
                userName = infoes[1];
                this.lb_DomainName.Text = userDomainName + "\\" + userName.ToString();
                //+ "@" + User.Identity.AuthenticationType.ToString();
            }
            else
            {
                this.tr_DomainArea.Visible = false;
            }
            log.Error("获取域及当前域账号：" + userDomainName + "\\" + userName.ToString());
        }
        catch(Exception ex)
        {
            log.Error("获取域及当前域账号出错" + ex);
        }
    }
   
    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {
        UserBll bllUser = new UserBll();
        String strAccount = this.account.Text.Trim().ToUpper();
        String strPwd = this.pwd.Text;
        //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
        strAccount = SQLInjectionDefense.ReplaceSQLReservedKeyword(strAccount);
        strPwd = SQLInjectionDefense.ReplaceSQLReservedKeyword(strPwd);

        strAccount = strAccount.Trim();
        strPwd = strPwd.Trim();

        //log.Error("VP平台验证登录,登录账号:" + strAccount);
        //log.Error("VP平台验证登录,IP地址为:" + Com.ValuePlus.Utils.RequestUtils.GetIP());
        //首先判断该用户是否首次登陆修改过密码，是否需要强制修改密码
        /// <returns>0:不需要修改密码</returns>
        /// <returns>1:首次登陆需要修改密码</returns>
        /// <returns>2:密码已过期,请及时修改密码</returns>
        String isNeedChangePassword = JudgeIsNeedChangePassword(strAccount);
        if (!isNeedChangePassword.Equals("0"))
        {
            String strParamString = "strUserId=" + strAccount + "&tipType=" + isNeedChangePassword;
            Response.Redirect("UserManager/ChangePwd.aspx?" + UrlParamEncryption.EncryptionUrlParam(strParamString));

        }else
        {
            UserInfo userInfo = new UserInfo();
            try
            {
                userInfo = bllUser.login(strAccount, strPwd, System.Guid.NewGuid().ToString("N"), Com.ValuePlus.Utils.RequestUtils.GetIP());
                String strTipInfo = "";
                if (userInfo == null)
                {
                    strTipInfo = rmLocResourceManager.GetString("Tip_ErrorAccount");
                    //判断是应为密码输入错误，则最多只能错误5次
                    //add by sammen 20150604
                    UserManagerBll bllUserManager = new UserManagerBll();
                    if (bllUserManager.IsExsitAccountId(strAccount))
                    {
                        //如果输错密码超过一定次数，则强制停用该用户
                        int iWrongTimeForceStop = int.Parse(Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("Times_InputWrongPwdToForceStopUser"));

                        int iLoginErrorCount = UserLoginError.GetLoginErrorCount(strAccount,false);
                        iLoginErrorCount = iLoginErrorCount + 1;
                        if (iLoginErrorCount > iWrongTimeForceStop)
                        {
                            //停用用户
                            UserLoginError.StopUser(strAccount);
                            strTipInfo = rmLocResourceManager.GetString("tipError5Times");
                            strTipInfo = strTipInfo.Replace("5", iWrongTimeForceStop.ToString());

                            //停用后记录用户信息变更记录（主要是停用标志） add by sammen 20180114
                            String strSpName = "USP_SYS_RecordUserInfoHis";
                            Hashtable hsTableParam = new Hashtable();
                            hsTableParam.Add("UserId", strAccount); //被修改用户ID
                            hsTableParam.Add("ModifyType", "040"); //040 密码错误强制停用
                            hsTableParam.Add("OPUserId", strAccount);//操作用户ID
                            hsTableParam.Add("OPUserIP", Com.ValuePlus.Utils.RequestUtils.GetIP());//操作用户IP
                            try
                            {
                                SqlParamDao.ExcuteSP(strSpName, hsTableParam);
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex);
                                log.Error("用户"+ strAccount + "多次输入密码错误，强制停用后记录用户信息变更记录,执行存储过程失败: SPNAME:" + strSpName);
                            }
                            //停用后记录用户信息变更记录（主要是停用标志） add by sammen 20180114

                        }
                        else
                        {
                            strTipInfo = rmLocResourceManager.GetString("tipErrorCount").Replace("*", iLoginErrorCount.ToString());
                            strTipInfo = strTipInfo.Replace("5", iWrongTimeForceStop.ToString());
                        }
                        //插入一条登录错误日志
                        UserLoginError.InertLoginError(strAccount, strPwd);
                    }

                    //Com.ValuePlus.BLL.User.LogHandle.register(strAccount, "10", Com.ValuePlus.Utils.RequestUtils.GetIP(), "10-1");
                    DataLogWriter.Log_Login(strAccount, Com.ValuePlus.Utils.RequestUtils.GetIP(), LogActionType.Login_NoUser);
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "", "<script language=javascript>alert('" + strTipInfo + "');</script>");
                }
                else
                {
                    //判断IP地址是否允许登陆 add by sammen 20200529
                    String strLoginIP = Com.ValuePlus.Utils.RequestUtils.GetIP();
                    bool bIsCanVisit = true;
                    try
                    {
                        bIsCanVisit = IPWhiteList.IsCanVisitByIPWhiteList(strLoginIP, strAccount);
                    }
                    catch(Exception ex)
                    {

                    }

                    if (bIsCanVisit)
                    {
                        if (userInfo.BISSTOP == "1")
                        {
                            //Com.ValuePlus.BLL.User.LogHandle.register(strAccount, "10", Com.ValuePlus.Utils.RequestUtils.GetIP(), "10-2");
                            DataLogWriter.Log_Login(strAccount, Com.ValuePlus.Utils.RequestUtils.GetIP(), LogActionType.Login_NoUser);
                            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "", "<script language=javascript>alert('" + rmLocResourceManager.GetString("Tip_ErrorUserStop") + "');</script>");
                        }
                        else
                        {
                            //登录成功

                            //清空登录错误次数信息 add by sammen 20150604
                            UserLoginError.ClearLoginErrorLog(strAccount);

                            userInfo.Loginip = Com.ValuePlus.Utils.RequestUtils.GetIP();
                            userInfo.Logintiem = System.DateTime.Now;
                            //服务器机器码
                            RegistBll bllLicense = new RegistBll();
                            userInfo.MachineCode = bllLicense.GetCurSeverMachineCode();
                            //站点域名
                            userInfo.WebSiteHostUrl =  String.Format(HttpContext.Current.Request.Url.Scheme.ToString() + "://{0}/", RequestUtils.GetCurrentFullHost());
                            
                            //Com.ValuePlus.BLL.User.LogHandle.register(userInfo.SACCOUNTID, "10", userInfo.Loginip, "10");
                            DataLogWriter.Log_Login(strAccount, Com.ValuePlus.Utils.RequestUtils.GetIP(), LogActionType.Login_Success);
                            UserLoginBll.LoginUserInfo = userInfo;
                            this.ExecuteAfterLogin();//add by sammen 20131021
                            Response.Redirect("index.html");
                        }
                    }
                    else
                    {
                        log.Error("IP禁止登陆：" + strLoginIP);
                        this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "", "<script language=javascript>alert('Sorry,your IP Address is not allowed to login');</script>");
                    }


                }

            }
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                log.Error("登录时出错："+ex);
                log.Error("登录时出错,登录服务器机器码：" + userInfo.MachineCode);
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "", "<script language=javascript>alert('" + rmLocResourceManager.GetString("Tip_ErrorSystem") + "');</script>");
            }
        }

    }

    /// <summary>
    /// 判断是否需要修改密码
    /// 判断该用户是否首次登陆修改过密码，是否需要强制修改密码
    /// </summary>
    /// <param name="strUserId"></param>
    /// <returns>0:不需要修改密码</returns>
    /// <returns>1:首次登陆需要修改密码</returns>
    /// <returns>2:密码已过期,请及时修改密码</returns>
    private String JudgeIsNeedChangePassword(String strUserId)
    {
        String IsNeed = "0";

        //首次登陆是否需要修改密码
        String strIsChangePasswordFirst = Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("IsChangePasswordFirst");
        if ((!String.IsNullOrEmpty(strIsChangePasswordFirst)) && (strIsChangePasswordFirst.Equals("1")))
        {
            //获取最后一次修改密码的日期
            String strSql = "select MAX(DTCHANGEDATE) as DTCHANGEDATE from TB_HR_CHANGEPWD_HIS where SUSERID = '" + strUserId + "'";
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
            if ((dt != null) && (dt.Rows.Count > 0) && (dt.Rows[0]["DTCHANGEDATE"] != null) && (!String.IsNullOrEmpty(dt.Rows[0]["DTCHANGEDATE"].ToString())))//存在修改密码的记录
            {
                DateTime dtLastChange = DateTime.Parse(dt.Rows[0]["DTCHANGEDATE"].ToString());
                ///强制要求修改密码的周期(单位为天，0表示不需要强制修改密码)
                String StrDaysChangePassword = Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("DaysChangePassword");
                int iDaysChangePassword = 0;
                if (!String.IsNullOrEmpty(strIsChangePasswordFirst))
                {
                    iDaysChangePassword = int.Parse(StrDaysChangePassword);
                }
                if(iDaysChangePassword>0)//强制修改天数>0才需要提示修改密码
                {
                    TimeSpan TimeSpants1=new TimeSpan(DateTime.Now.Ticks);  
                    TimeSpan TimeSpants2=new TimeSpan(dtLastChange.Ticks);
                    TimeSpan ts = TimeSpants1.Subtract(TimeSpants2).Duration();
                    int Days = ts.Days;
                    if (Days > iDaysChangePassword)//时间间隔超过要求的条数，则提示修改
                    {
                        IsNeed = "2";
                    }
                }
            }
            else//如果不存在修改密码记录，则需要提示“首次登陆需修改密码”
            {
                IsNeed = "1";
            }

        }
        return IsNeed;
    }

    /// <summary>
    /// //登录成功后,执行存储过程,目前主要是写入应用主机地址
    /// </summary>
    private void ExecuteAfterLogin()
    {
        //modify by sammen 20181130 增加对https网站的支持
        String strURLSchame = HttpContext.Current.Request.Url.Scheme.ToString();
        String strWebSiteHomeUrl = strURLSchame+"://" + Com.ValuePlus.Utils.RequestUtils.GetCurrentFullHost();
        String strSpName = "USP_SYS_AfterLogin";
        Hashtable hsTableParam = new Hashtable();
        hsTableParam.Add("webSiteHostUrl", strWebSiteHomeUrl);

        try
        {
            if (!String.IsNullOrEmpty(strSpName))
            {
                SqlParamDao.ExcuteSP(strSpName, hsTableParam);
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
            log.Error("登录成功后,执行存储过程失败: SPNAME:" + strSpName);
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
        ResourceManager sR = new ResourceManager(string.Format("Resources.{0}",resoucename), Assembly.Load("App_GlobalResources"));
        return sR;
    }
    #endregion

}
