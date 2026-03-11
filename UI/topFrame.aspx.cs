using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Resources;
using System.Text;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Com.ValuePlus.Web;
using Com.ValuePlus.BLL.User;
using Com.ValuePlus.Entity;
using Com.ValuePlus.Common;
using Com.ValuePlus.Common.Config;
using Com.ValuePlus.DataLog;
using Com.ValuePlus.DataLog.Enum;
using Com.ValuePlus.BLL.Regist;
using Com.ValuePlus.Archive.BLL;
using Com.ValuePlus.Entity.Regist;
using Com.ValuePlus.DAL;
using Newtonsoft.Json;
using System.Web.Services;
using Com.ValuePlus.Utils.Session;

public partial class topFrame : Com.ValuePlus.Web.PageBase
{
    protected String strHomePage = "Welcome.aspx";
    protected String IsRealtimeAlert = "0";//是否开启实时提醒功能
    protected int iRealtimeAlertInterval = 2000;//实时提醒时间间隔

    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["language"] = UserLoginBll.Language;
        if (!Page.IsPostBack)
        {
            try
            {
                this.aClosePendingAlert.Attributes.Add("onclick", "return false;");
                //判断授权文件
                this.JudgeLisence();

                //获取并设置该应用的主页面链接
                this.GetHomePageUrl();

                ResourceManager rmLocResourceManager = base.GetResourceManager("TopFrame");
                this.strTipExitFailed = rmLocResourceManager.GetString("tipExitFaild");
                this.lbPendingAlert.Text = rmLocResourceManager.GetString("lbRealTimeAlert");
                this.cbNeverPengdingAlert.Text = rmLocResourceManager.GetString("lbNotAlert");

                //初始化图片按钮
                this.imgBtn_exit.Attributes.Add("onclick", "ExitSystem('" + rmLocResourceManager.GetString("tipExit") + "');return false;");

                //加载语言选择区域
                this.BuildLanguageSelect(rmLocResourceManager);

                // 动态加载登陆用户的第一级功能菜单
                this.BuildUserFirstFunction();

                //获取当前登陆用户名称并显示
                this.GetLoginUserAndView();

                //获取基本参数设置中的“是否开启实时提醒功能”和“实时提醒间隔时间”
                this.hfIsRealtimeAlert.Value = Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("IsRealtimeAlert") == "" ? IsRealtimeAlert : Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("IsRealtimeAlert");
                this.hfRealtimeAlertInterval.Value = Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("iRealtimeAlertInterval") == "" ? iRealtimeAlertInterval.ToString() :Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("iRealtimeAlertInterval");

            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.Error("topFrame.Page_Load error!");
            }

        }
    }

    #region viewstate初始化区域
    private string strTipExitFailed
    {
        get
        {
            return ViewState["topFrame_strTipExitFailed_ViewState"] as string;
        }
        set
        {
            ViewState["topFrame_strTipExitFailed_ViewState"] = value;
        }
    }
    #endregion

    /// <summary>
    /// 判断授权文件
    /// </summary>
    private void JudgeLisence()
    {
        //判断lisence ---add by sammen 20121122
        //新增对license的全局变量的写入 ---add by sammen 20140616
        RegistBll bllLicense = new RegistBll();
        LicenseEntity entity = bllLicense.GetLicenseInfo();
        ResourceManager rmLocResourceManager = this.GetResourceManager("License");
        if ((entity != null) && (!String.IsNullOrEmpty(entity.strClientNameChs)))
        {
            //判断是否根据机器码绑定，如果存在机器码但是不匹配则退出
            if ((!String.IsNullOrEmpty(entity.strMachine)) && (!entity.strMachine.Equals(this.GetServerMachineCode())))
            {
                RegistBll.LicenseInfo = null;
                RegistBll.LicenseIsValid = false;
                Response.Redirect(string.Format("{0}{1}", GetSiteWebAddress(), "Regist/LicenseWarning.aspx?msg=" + rmLocResourceManager.GetString("Label_NoLicense")), true);
            }

            DateTime dtValidDate = entity.dtValid;
            //判断有效期是否小于提醒期限
            int iDiff = dtValidDate.Subtract(DateTime.Now).Days;
            RegistBll.LicenseInfo = entity;
            RegistBll.LicenseIsValid = true;
            if (iDiff < 0)
            {
                RegistBll.LicenseIsValid = false;
                Response.Redirect(string.Format("{0}{1}", GetSiteWebAddress(), "Regist/LicenseWarning.aspx?msg=" + rmLocResourceManager.GetString("Label_LicenseExpired")), true);
            }

            //将License写入数据库 add by sammen 20220610
            bllLicense.WriteLicenseInfoToDB(entity, this.GetUserCode(), this.GetProjectId(), this.GetServerMachineCode());
        }
        else
        {
            RegistBll.LicenseInfo = null;
            RegistBll.LicenseIsValid = false;
            Response.Redirect(string.Format("{0}{1}", GetSiteWebAddress(), "Regist/LicenseWarning.aspx?msg=" + rmLocResourceManager.GetString("Label_NoLicense")), true);
        }

    }
    /// <summary>
    /// 获取该应用的主页面链接
    /// </summary>
    private void GetHomePageUrl()
    {
        //获取该应用的主页面链接
        String strConfigHomePage = Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("DefaultHomePageUrl");
        if (String.IsNullOrEmpty(strConfigHomePage))
        {
            log.Error("topFrame.GetHomePageUrl() error：参数配置中不存在DefaultHomePageUrl的配置项目!");
        }
        else
        {
            strHomePage = strConfigHomePage;
        }
        this.hfHomePageUrl.Value = strHomePage;
        //this.mainFrame.Attributes["src"] = strHomePage;
        //this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "", "<script language=javascript>parent.document.getElementById('mainFrame').src=" + strHomePage + ";</script>"); 
    }

    #region 加载语言选择区域
    /// <summary>
    /// 加载语言选择区域
    /// </summary>
    private void BuildLanguageSelect(ResourceManager rmLocResourceManager)
    {
        String str_Default = rmLocResourceManager.GetString("txt_Default");
        String str_Chinese = rmLocResourceManager.GetString("txt_Chinese");
        String str_English = rmLocResourceManager.GetString("txt_English");

        ListItem lItem_Default = new ListItem(str_Default, "default");
        ListItem lItem_Chinese = new ListItem(str_Chinese, "zh-cn");
        ListItem lItem_English = new ListItem(str_English, "en-us");

        this.DropDownList1.Items.Clear();
        this.DropDownList1.Items.Insert(0, lItem_Chinese);
        this.DropDownList1.Items.Insert(1, lItem_English);

        this.ListBox1.Items.Clear();
        this.ListBox1.Items.Insert(0, lItem_Chinese);
        this.ListBox1.Items.Insert(1, lItem_English);

        if (UserLoginBll.Language == "zh-cn")
        {
            this.ListBox1.Items[0].Selected = true;
            this.ListBox1.Items[1].Selected = false;
        }
        else if (UserLoginBll.Language == "en-us")
        {
            this.ListBox1.Items[0].Selected = false;
            this.ListBox1.Items[1].Selected = true;
        }
        //this.ListBox1.Attributes.Add("onchange", "BeforeChangeLanguge();");
    }
    #endregion

    #region 语言类型选择变更事件【DropDownList1】
    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (this.DropDownList1.SelectedValue == "default")
        {
            ViewState["language"] = UserLoginBll.Language;//默认语言
        }
        else if (this.DropDownList1.SelectedValue == "zh-cn")
        {
            ViewState["language"] = "zh-cn";//选择中文
            base.Language = "zh-cn";
        }
        else if (this.DropDownList1.SelectedValue == "en-us")
        {
            ViewState["language"] = "en-us";//选择英文
            base.Language = "en-us";
        }
        //base.Response.Write("<script language=javascript> parent.document.getElementById('mainFrame').src=parent.document.getElementById('mainFrame').src </script>");
        //base.Response.Write("<script language=javascript> parent.document.getElementById('bottomFrame').src=parent.document.getElementById('bottomFrame').src </script>");
        //base.Response.Write("<script language=javascript> window.location.href=window.location.href </script>");
        //base.Response.Write("<script language=javascript> AfterChangeLanguge();</script>");
        this.Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "<script language=javascript>AfterChangeLanguge();</script>");
        //加载语言选择区域
        ResourceManager rmLocResourceManager = base.GetResourceManager("TopFrame");
        this.BuildLanguageSelect(rmLocResourceManager);
        // 动态加载登陆用户的第一级功能菜单
        this.BuildUserFirstFunction();
        //获取当前登陆用户名称并显示
        this.GetLoginUserAndView();

    }
    #endregion

    #region 语言类型选择变更事件【ListBox1】
    protected void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (this.ListBox1.SelectedValue == "default")
        {
            ViewState["language"] = UserLoginBll.Language;//默认语言
        }
        else if (this.ListBox1.SelectedValue == "zh-cn")
        {
            ViewState["language"] = "zh-cn";//选择中文
            base.Language = "zh-cn";
        }
        else if (this.ListBox1.SelectedValue == "en-us")
        {
            ViewState["language"] = "en-us";//选择英文
            base.Language = "en-us";
        }
        //base.Response.Write("<script language=javascript> parent.document.getElementById('mainFrame').src=parent.document.getElementById('mainFrame').src </script>");
        //base.Response.Write("<script language=javascript> parent.document.getElementById('bottomFrame').src=parent.document.getElementById('bottomFrame').src </script>");
        //base.Response.Write("<script language=javascript> window.location.href=window.location.href </script>");
        //base.Response.Write("<script language=javascript> AfterChangeLanguge();</script>");
        this.Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "<script language=javascript>AfterChangeLanguge();</script>");
        //加载语言选择区域
        ResourceManager rmLocResourceManager = base.GetResourceManager("TopFrame");
        this.BuildLanguageSelect(rmLocResourceManager);
        // 动态加载登陆用户的第一级功能菜单
        this.BuildUserFirstFunction();
        //获取当前登陆用户名称并显示
        this.GetLoginUserAndView();

    }
    #endregion

    #region 动态加载登陆用户的第一级功能菜单，同时读取用户菜单数据信息存入缓存
    /// <summary>
    /// 动态加载登陆用户的第一级功能菜单，同时读取用户菜单数据信息存入缓存
    /// </summary>
    private void BuildUserFirstFunction()
    {
        UserBll userBll = new UserBll();
        DataTable dt = new DataTable();
        UserInfo userInfo ;
        bool isOnline = UserLoginBll.AuthLoginUser(out userInfo);
        if (isOnline)
        {
            dt = userBll.GetUserMenuDataTable(userInfo);
            ////读取用户菜单数据信息存入缓存//暂时不用缓存
            //if (Com.ValuePlus.Utils.Session.SessionHelper.GetSession("dtLoginUserMenu") == null)
            //{
            //    Com.ValuePlus.Utils.Session.SessionHelper.SetSession("dtLoginUserMenu",dt);
            //}
        }
        else
        {
            Page.Response.Write(userBll.GetUserTreeNotLogin());
        }

        //获取第一级别菜单
        String strRootMenuCode = Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("MenuRootId");
        //获取默认显示第一级别菜单的个数
        String strShowMenuNumber = Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("IShowMenuNumber");
        int iShowMenuNumber = 6;
        if (!String.IsNullOrEmpty(strShowMenuNumber))
        {
            iShowMenuNumber = int.Parse(strShowMenuNumber);
        }

        DataRow[] drs = dt.Select("SPARENTCODE = '" + strRootMenuCode + "'");
        StringBuilder strBuilderMenu = new StringBuilder();
        if (drs != null && drs.Length > 0)
        {
            int iRowCount = drs.Length;//记录数
            int iLimit = iRowCount;
            int iGapCount = iRowCount - iShowMenuNumber;
            if (iGapCount > 0)
            {
                iLimit = iShowMenuNumber;
            }

            //先显示最大限度显示个数的部分栏目
            for (int i = 0; i < iLimit; i++)
            {
                DataRow dr = drs[i];

                String strMenuCode = dr["SMENUCODE"].ToString();
                string sTitle = dr["SMENUNAME"].ToString();
                if (this.Language == "zh-cn")
                {
                    sTitle = dr["SMENUNAMECN"].ToString();
                }
                strBuilderMenu.Append("<a href=\"javascript:redirectMenuFunction('" + strMenuCode + "');\"  style=\"display:inline;\" onmouseover=\"javascript:window.status='" + sTitle + "';return true;\">" + sTitle + "</a>\r\n");

            }

            //如果记录数大于默认显示的个数，则其余的在“更多”里显示
            if (iGapCount > 0)
            {
                strBuilderMenu.Append("                 <div id=\"divRoleScene\" class=\"navMenu\">\r\n");
                strBuilderMenu.Append("                     <ul>\r\n");
                strBuilderMenu.Append("                         <li >\r\n");
                if (this.Language.Equals("en-us"))
                {
                    strBuilderMenu.Append("                             <a  href=\"#\" style=\"cursor:hand;display:inline;font-weight:bold\">More>>>></a>\r\n");
                }
                else
                {
                    strBuilderMenu.Append("                             <a  href=\"#\" style=\"cursor:hand;display:inline;font-weight:bold\">更多>>>></a>\r\n");
                }
                strBuilderMenu.Append("                             <ul>\r\n");
                strBuilderMenu.Append("                                 <div>\r\n");
                for (int j = iLimit; j < iRowCount; j++)
                {
                    DataRow dr = drs[j];

                    String strMenuCode = dr["SMENUCODE"].ToString();
                    string sTitle = dr["SMENUNAME"].ToString();
                    if (this.Language == "zh-cn")
                    {
                        sTitle = dr["SMENUNAMECN"].ToString();
                    }
                    //strBuilderMenu.Append("                 <li><a href=\"javascript:selectOtherRole('" + strParamString + "');\" class=\"a_Left\">" + ds.Tables[0].Rows[i][strShowName].ToString() + "</a></li>\r\n");
                    strBuilderMenu.Append("<li><a href=\"javascript:redirectMenuFunction('" + strMenuCode + "');\"  style=\"display:inline;\" onmouseover=\"javascript:window.status='" + sTitle + "';return true;\">" + sTitle + "</a></li>\r\n");

                }
                strBuilderMenu.Append("                                 </div>\r\n");
                strBuilderMenu.Append("                             </ul>\r\n");
                strBuilderMenu.Append("                         </li>\r\n");
                strBuilderMenu.Append("                     </ul>\r\n");
                strBuilderMenu.Append("                 </div>\r\n");
            }

            this.dirMenuArea.InnerHtml = strBuilderMenu.ToString();
        }
    }
    #endregion

    #region 获取当前登陆用户名称并显示
    /// <summary>
    /// 获取当前登陆用户名称并显示
    /// </summary>
    private void GetLoginUserAndView()
    {
        if (this.Language == "zh-cn")
        {
            Label1.Text = "您好：" + this.GetUserInfo().SUSERNAMECN;
        }
        else
        {
            Label1.Text = "welcome," + this.GetUserInfo().SUSERNAME;
        }
    }
    #endregion

    #region 安全退出系统
    /// <summary>
    /// 安全退出系统
    /// </summary>
    protected void imgBtn_exit_Click(object sender, ImageClickEventArgs e)
    {
        UserBll bllUser = new UserBll();
        try
        {
            this.divWaitting.Visible = true;
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
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "", "<script language=javascript>window.parent.location.href = 'SSO.aspx';</script>");
            }
            else
            {
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "", "<script language=javascript>window.parent.location.href = 'login.aspx';</script>");
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "", "<script language=\"javascript\">alert('" + this.strTipExitFailed + "');</script>"); 
        }
    }
    #endregion

    /// <summary>
    /// 即时获取当前用户的待办事项条数
    /// </summary>
    /// <returns></returns>
    [WebMethod]
    public static String GetCurUserPendingCount()
    {
        UserInfo userInfo = Com.ValuePlus.BLL.User.UserLoginBll.LoginUserInfo;
        String strCurUserId = "";
        if (userInfo != null)
        {
            strCurUserId = userInfo.SUSERID;
        }
        //Response.Write(json);
        //StringBuilder sbSql = new StringBuilder() ;
        ////sbSql.Append("SELECT rd.TID,t.TDESCCHS AS [TDESCRIPTION],rd.RID,r.RDESCCHS AS [RDESCRIPTION],rd.SID,s.SDESCCHS AS [SDESCRIPTION],0 AS [TCOUNT]");
        //sbSql.Append("SELECT count(*)");
        //sbSql.Append("FROM TB_HRTMPRD rd,TB_HR_USERROLE ur,TB_HRTMPS s,TB_HRTMPH t,TB_HRTMPR r ");
        //sbSql.Append("WHERE rd.TID=ur.TID AND rd.RID=ur.RID AND s.TID=ur.TID AND s.SID=rd.SID ");
        //sbSql.Append("AND s.SALERT=1 AND t.TID=ur.TID AND r.TID=ur.TID AND r.RID=ur.RID ");
        //sbSql.Append("and ur.SUSERID='" + strCurUserId + "'");
        //String strSql = sbSql.ToString();

        int iCount = ArchiveAlertBll.GetAlertDetailCount(strCurUserId, "zh-cn", false);
        string json = "[{\"UserId\":\"" + strCurUserId + "\",\"iPendingCount\":\"" + iCount .ToString()+ "\"}]";

        return json;
    }

    //输出对象
    void ResponseObject(object obj)
    {
        Response.Write(JsonConvert.SerializeObject(obj));
    }

    //输出消息
    void ResponseMsg(string msg)
    {
        Response.Write(JsonConvert.SerializeObject(msg));
    }

}
