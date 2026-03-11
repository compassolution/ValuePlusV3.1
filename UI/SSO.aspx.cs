using Com.ValuePlus.BLL.SysManager;
using Com.ValuePlus.BLL.User;
using Com.ValuePlus.Common;
using Com.ValuePlus.DAL;
using Com.ValuePlus.DataLog;
using Com.ValuePlus.DataLog.Enum;
using Com.ValuePlus.Entity;
using Com.ValuePlus.Utils.Session;
using Com.ValuePlus.Web;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SSO : System.Web.UI.Page
{
    public ResourceManager rmLocResourceManager;
    /// <summary>
    /// 日志声明
    /// </summary>
    private static Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!base.IsPostBack)
        {
            try
            {
                Hashtable hsTableUrlQuery = WebCommon.GetUrlAnalyse(Server.UrlDecode(Request.Url.Query.ToString()));

                string strAjaxParam = hsTableUrlQuery["ajaxparam"] == null ? string.Empty : hsTableUrlQuery["ajaxparam"].ToString();//请求类型参数
                
                //如果param有值则是Ajax请求，如果没有传指，则指页面正常加载
                switch (strAjaxParam)
                {
                    case "":
                        //页面访问的加载
                        LoadPage(sender, e);
                        break;
                    case "getdbconnect":
                        //获取需单点登录的多账套列表
                        GetMultiDBConnetct();
                        break;
                    default:
                        break;
                }
            }
            catch (System.Threading.ThreadAbortException)
            {
                //从Aspx后台代码返回json到页面时，使用了Response.End()后出现的异常直接忽视
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return;
            }
        }

    }

    /// <summary>
    /// 页面访问的加载
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void LoadPage(object sender, EventArgs e)
    {
        //首先清空单点登录的数据库链接
        SessionHelper.RemoveSession(CacheName.DBConnectSessionName);

        rmLocResourceManager = this.GetResourceManager("Login");
        string sExceptionTip = Com.ValuePlus.Common.Exception.ExceptionInfo.GetExceptionTipInfo("DataExcetion");
        this.strTip_ErrorAccount = this.GetResourceManager("Login").GetString("Tip_ErrorAccount");
        this.strTipError5Times = this.GetResourceManager("Login").GetString("tipError5Times");
        this.strTipErrorCount = this.GetResourceManager("Login").GetString("tipErrorCount");
        this.strTip_ErrorUserStop = this.GetResourceManager("Login").GetString("Tip_ErrorUserStop");
        this.strTip_ErrorSystem = this.GetResourceManager("Login").GetString("Tip_ErrorSystem");
    }


    #region viewstate初始化区域
    private string strTip_ErrorAccount
    {
        get
        {
            return ViewState["strTip_ErrorAccount"] as string;
        }
        set
        {
            ViewState["strTip_ErrorAccount"] = value;
        }
    }
    private string strTipError5Times
    {
        get
        {
            return ViewState["strTipError5Times"] as string;
        }
        set
        {
            ViewState["strTipError5Times"] = value;
        }
    }
    private string strTipErrorCount
    {
        get
        {
            return ViewState["strTipErrorCount"] as string;
        }
        set
        {
            ViewState["strTipErrorCount"] = value;
        }
    }
    private string strTip_ErrorUserStop
    {
        get
        {
            return ViewState["strTip_ErrorUserStop"] as string;
        }
        set
        {
            ViewState["strTip_ErrorUserStop"] = value;
        }
    }
    private string strTip_ErrorSystem
    {
        get
        {
            return ViewState["strTip_ErrorSystem"] as string;
        }
        set
        {
            ViewState["strTip_ErrorSystem"] = value;
        }
    }    
    #endregion

    /// <summary>
    /// 获取需单点登录的多账套列表
    /// </summary>
    private void GetMultiDBConnetct()
    {
        String strReturn = "";
        try
        {
            StringBuilder sBuilder = new StringBuilder();

            StringBuilder sbSql_ResultData = new StringBuilder();
            sbSql_ResultData.Append("select  * from VW_Sys_ProjectId where 1=1");
            sbSql_ResultData.Append(" ORDER BY P9");
            DataTable dt_ResultData = SqlParamDao.GetDataTableBySql(sbSql_ResultData.ToString());

            sBuilder.Append("{");
            sBuilder.Append(WebCommon.GetJsonStringByDataTable(dt_ResultData, "ResultData"));
            sBuilder.Append("}");

            strReturn = sBuilder.ToString();
        }
        catch (Exception ex)
        {
            log.Error(ex);
            strReturn = "-1";
        }
        finally
        {
            Response.Clear();
            Response.Write(strReturn);
            Response.End();
        }
    }

    protected void LoginButton_Click(object sender, EventArgs e)
    {
        UserBll bllUser = new UserBll();
        String strAccount = Request.Form["txt_UserName"].Trim().ToUpper();
        String strPwd = Request.Form["txt_Password"].Trim().ToUpper();
        String strDBConnectFlag = Request.Form["txt_DBConnect"].ToString();
        SessionHelper.SetSession(CacheName.DBConnectSessionName, strDBConnectFlag);

        //首先判断该用户是否首次登陆修改过密码，是否需要强制修改密码
        /// <returns>0:不需要修改密码</returns>
        /// <returns>1:首次登陆需要修改密码</returns>
        /// <returns>2:密码已过期,请及时修改密码</returns>
        String isNeedChangePassword = JudgeIsNeedChangePassword(strAccount);
        if (!isNeedChangePassword.Equals("0"))
        {
            String strParamString = "strUserId=" + strAccount + "&tipType=" + isNeedChangePassword;
            Response.Redirect("UserManager/ChangePwd.aspx?" + UrlParamEncryption.EncryptionUrlParam(strParamString));

        }
        else
        {
            try
            {
                UserInfo userInfo = bllUser.login(strAccount, strPwd, System.Guid.NewGuid().ToString("N"), Com.ValuePlus.Utils.RequestUtils.GetIP());
                String strTipInfo = "";
                if (userInfo == null)
                {
                    strTipInfo = this.strTip_ErrorAccount;
                    //判断是应为密码输入错误，则最多只能错误5次
                    //add by sammen 20150604
                    UserManagerBll bllUserManager = new UserManagerBll();
                    if (bllUserManager.IsExsitAccountId(strAccount))
                    {
                        int iLoginErrorCount = UserLoginError.GetLoginErrorCount(strAccount, false);
                        iLoginErrorCount = iLoginErrorCount + 1;
                        if (iLoginErrorCount > 5)
                        {
                            //停用用户
                            UserLoginError.StopUser(strAccount);
                            strTipInfo = this.strTipError5Times;
                        }
                        else
                        {
                            strTipInfo = this.strTipErrorCount.Replace("*", iLoginErrorCount.ToString());
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
                    if (userInfo.BISSTOP == "1")
                    {
                        //Com.ValuePlus.BLL.User.LogHandle.register(strAccount, "10", Com.ValuePlus.Utils.RequestUtils.GetIP(), "10-2");
                        DataLogWriter.Log_Login(strAccount, Com.ValuePlus.Utils.RequestUtils.GetIP(), LogActionType.Login_NoUser);
                        this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "", "<script language=javascript>alert('" + this.strTip_ErrorUserStop + "');</script>");
                    }
                    else
                    {
                        //登录成功

                        //清空登录错误次数信息 add by sammen 20150604
                        UserLoginError.ClearLoginErrorLog(strAccount);

                        userInfo.Loginip = Com.ValuePlus.Utils.RequestUtils.GetIP();
                        userInfo.Logintiem = System.DateTime.Now;
                        //Com.ValuePlus.BLL.User.LogHandle.register(userInfo.SACCOUNTID, "10", userInfo.Loginip, "10");
                        DataLogWriter.Log_Login(strAccount, Com.ValuePlus.Utils.RequestUtils.GetIP(), LogActionType.Login_Success);
                        UserLoginBll.LoginUserInfo = userInfo;
                        this.ExecuteAfterLogin();//add by sammen 20131021
                        Response.Redirect("index.html");
                    }
                }

            }
            catch (Exception ex)
            {
                log.Error(ex);
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "", "<script language=javascript>alert('" + this.strTip_ErrorSystem + "');</script>");
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
                if (iDaysChangePassword > 0)//强制修改天数>0才需要提示修改密码
                {
                    TimeSpan TimeSpants1 = new TimeSpan(DateTime.Now.Ticks);
                    TimeSpan TimeSpants2 = new TimeSpan(dtLastChange.Ticks);
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
        String strWebSiteHomeUrl = strURLSchame + "://" + Com.ValuePlus.Utils.RequestUtils.GetCurrentFullHost();

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
        ResourceManager sR = new ResourceManager(string.Format("Resources.{0}", resoucename), Assembly.Load("App_GlobalResources"));
        return sR;
    }
    #endregion

}