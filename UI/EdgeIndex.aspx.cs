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
using Com.ValuePlus.SysParams;
public partial class EdgeIndex : Com.ValuePlus.Web.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            String strUrlQueryString = Server.UrlDecode(Request.Url.Query.ToString());
            Hashtable hsTableUrlQuery = WebCommon.GetUrlAnalyse(strUrlQueryString);
            string strParam = hsTableUrlQuery["param"] == null ? string.Empty : hsTableUrlQuery["param"].ToString();//param
            string strChangeLanguage = hsTableUrlQuery["language"] == null ? string.Empty : hsTableUrlQuery["language"].ToString();//language
            string strMenuId = hsTableUrlQuery["menuid"] == null ? string.Empty : hsTableUrlQuery["menuid"].ToString();//menuid

            switch (strParam.ToLower().ToString())
            {
                case "getpagebasicdata":
                    String strReturnBasicData = this.GetPageBasicData().ToString();
                    Response.Write(strReturnBasicData);
                    Response.End();
                    break;
                case "changelanguage":
                    Response.Write(this.ChangeLanguage(strChangeLanguage).ToString());
                    Response.End();
                    break;
                case "getmenupathbymenuid":
                    Response.Write(this.GetMenuPathByMenuId(strMenuId).ToString());
                    Response.End();
                    break;
                case "exitapplication":
                    Response.Write(this.ExitApplication().ToString());
                    Response.End();
                    break;
                default:
                    //获取lic
                    this.GetSystemLisence(null);
                    if (!this.strIsLicValid.Equals("1"))
                    {
                        Response.Redirect(string.Format("{0}{1}", GetSiteWebAddress(), "Regist/LicenseWarning.aspx?msg=" + this.strLicInValidMsg), true);
                    }
                    break;
                    
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
            log.Error("topFrame.Page_Load error!");
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
    private string strIsLicValid
    {
        get
        {
            return ViewState["strIsLicValid_ViewState"] as string;
        }
        set
        {
            ViewState["strIsLicValid_ViewState"] = value;
        }
    }
    private string strLicInValidMsg
    {
        get
        {
            return ViewState["strLicInValidMsg_ViewState"] as string;
        }
        set
        {
            ViewState["strLicInValidMsg_ViewState"] = value;
        }
    }
    #endregion

    /// <summary>
    /// 获取IndexEdge页面的基础数据信息
    /// </summary>
    /// <returns></returns>
    private String GetPageBasicData()
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        StringBuilder sbReturnRowData = new StringBuilder();
        StringBuilder sbReturnColumnData = new StringBuilder();
        StringBuilder sbReturnLicData = new StringBuilder();
        StringBuilder sbReturnIndexToolsData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "获取IndexEdge页面的基础数据信息";

        ResourceManager rmLocResourceManager_SysUpdate = base.GetResourceManager("SysUpdate");
        String strLbUpdateAlert = rmLocResourceManager_SysUpdate.GetString("lbUpdateAlert");
        ResourceManager rmLocResourceManager_License = base.GetResourceManager("License");
        String strAuthorName = rmLocResourceManager_License.GetString("Label_Name");
        String strAuthorDate = rmLocResourceManager_License.GetString("Label_Date");
        ResourceManager rmLocResourceManager_TopFrame = base.GetResourceManager("TopFrame");
        String str_Chinese = rmLocResourceManager_TopFrame.GetString("txt_Chinese");
        String str_English = rmLocResourceManager_TopFrame.GetString("txt_English");
        String strExitTips = rmLocResourceManager_TopFrame.GetString("tipExit");

        //this.strTipExitFailed = rmLocResourceManager_TopFrame.GetString("tipExitFaild");
        //this.lbPendingAlert.Text = rmLocResourceManager_TopFrame.GetString("lbRealTimeAlert");
        //this.cbNeverPengdingAlert.Text = rmLocResourceManager_TopFrame.GetString("lbNotAlert");
        try
        {
            //获取基本参数设置中的“系统项目简称”
            String strProductShortName = BaseParamsGetter.GetBasicParamValue("ProductShortName");
            //获取基本参数设置中的“是否提示系统更新”
            String strIsAlertSysUpdate = BaseParamsGetter.GetBasicParamValue("IsAlertSysUpdate");

            //获取基本参数设置中的“是否开启实时提醒功能”和“实时提醒间隔时间”
            String strIsRealtimeAlert = BaseParamsGetter.GetBasicParamValue("IsRealtimeAlert") == "" ? "0" : BaseParamsGetter.GetBasicParamValue("IsRealtimeAlert");
            String strRealtimeAlertInterval = BaseParamsGetter.GetBasicParamValue("iRealtimeAlertInterval") == "" ? "6000" : BaseParamsGetter.GetBasicParamValue("iRealtimeAlertInterval");

            //获取默认显示第一级别菜单的个数
            String strShowMenuNumber = BaseParamsGetter.GetBasicParamValue("IShowMenuNumber");
            String strUserName = this.Language == "zh-cn" ? this.GetUserInfo().SUSERNAMECN : this.GetUserInfo().SUSERNAME;

            StringBuilder sbLanguageTips = new StringBuilder();
            sbLanguageTips.Append("{");
            sbLanguageTips.Append("\"ToHomeTips\":\"" + (this.Language == "zh-cn" ? "回到首页" : "To Home") + "\"");
            sbLanguageTips.Append(",\"SystemTips\":\"" + (this.Language == "zh-cn" ? "系统提示" : "System Tips") + "\"");
            sbLanguageTips.Append(",\"YouHavePendingTips\":\"" + (this.Language == "zh-cn" ? "您有待办事项未处理" : "You have some to do list ") + "\"");
            sbLanguageTips.Append(",\"ClickToInTips\":\"" + (this.Language == "zh-cn" ? "请点击进入" : "Please click " )+ "\"");
            sbLanguageTips.Append(",\"RejectShowTips\":\"" + (this.Language == "zh-cn" ? "不再提醒" : "No longer remind") + "\"");
            sbLanguageTips.Append(",\"CloseThisTimeTips\":\"" + (this.Language == "zh-cn" ? "本次关闭" : "Close ") + "\"");
            sbLanguageTips.Append(",\"MoreMenu\":\"" + (this.Language == "zh-cn" ? "更多" : "More") + "\"");
            sbLanguageTips.Append(",\"ToolAndDoc\":\"" + (this.Language == "zh-cn" ? "文档及工具" : "Documents & Tools") + "\"");
            sbLanguageTips.Append(",\"UpdateAlertTips\":\"" + strLbUpdateAlert + "\"");
            sbLanguageTips.Append(",\"ConfirmTips\":\"" + (this.Language == "zh-cn" ? "确定" : "Confirm") + "\"");
            sbLanguageTips.Append(",\"CancelTips\":\"" + (this.Language == "zh-cn" ? "取消" : "Cancel") + "\"");
            sbLanguageTips.Append(",\"ChangePasswordTips\":\"" + (this.Language == "zh-cn" ? "修改密码" : "Change password") + "\"");
            sbLanguageTips.Append(",\"InnerMsgTips\":\"" + (this.Language == "zh-cn" ? "消息通知" : "Message") + "\"");
            sbLanguageTips.Append(",\"LicAuthorNameTips\":\"" + strAuthorName + "\"");
            sbLanguageTips.Append(",\"LicAuthorDateTips\":\"" + strAuthorDate + "\"");
            sbLanguageTips.Append(",\"LogoutBtnTips\":\"" + (this.Language == "zh-cn" ? "退出登录" : "Logout") + "\"");
            sbLanguageTips.Append(",\"ExitTips\":\"" + strExitTips + "\"");
            sbLanguageTips.Append("}");

            if (String.IsNullOrEmpty(strShowMenuNumber))
            {
                strShowMenuNumber = "8";
            }

            sbResultData.Append(",\"UserId\":\"" + UserLoginBll.Language.ToString() + "\"");
            sbResultData.Append(",\"UserName\":\"" + strUserName + "\"");
            sbResultData.Append(",\"ProjectId\":\"" + this.GetProjectId() + "\"");
            sbResultData.Append(",\"ProductShortName\":\"" + strProductShortName + "\"");
            sbResultData.Append(",\"RemoteServer\":\"" + this.GetRemoteServer() + "\"");
            sbResultData.Append(",\"HomePageUrl\":\"" + GetHomePageUrl().ToString() + "\"");
            sbResultData.Append(",\"IsAlertSysUpdate\":\"" + strIsAlertSysUpdate + "\"");
            sbResultData.Append(",\"IsRealtimeAlert\":\"" + strIsRealtimeAlert + "\"");
            sbResultData.Append(",\"RealtimeAlertInterval\":\"" + strRealtimeAlertInterval + "\"");
            sbResultData.Append(",\"CurLanguage\":\"" + this.Language.ToString() + "\"");
            sbResultData.Append(",\"LanguageJson\":" + this.GetLanguageJsonString(rmLocResourceManager_TopFrame) + "");
            sbResultData.Append(",\"LanguageTips\":" + sbLanguageTips.ToString() + "");
            sbResultData.Append(",\"ShowMenuCount\":\"" + strShowMenuNumber + "\"");

            //第一级别栏目数据
            sbReturnRowData.Append(this.GetUserFirstLevelMenu());
            //获取系统授权数据
            sbReturnLicData.Append(this.GetSystemLisence(rmLocResourceManager_License).ToString());

            //Server.Transfer("Regist/LicenseWarning.aspx?msg=" + "test");

            //获取首页文档工具集
            sbReturnIndexToolsData.Append(GetIndexToolsList());

            strReturnCode = "1";
            strReturnMsg = strMethodDesc + "成功";
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = strMethodDesc + "出错";
            log.Error(ex);
        }
        finally
        {

            sbResult.Append("{");
            sbResult.Append("\"ReturnCode\":\"" + strReturnCode.ToString() + "\"");
            sbResult.Append(",\"ReturnMsg\":\"" + strReturnMsg.ToString() + "\"");
            sbResult.Append(sbResultData.ToString());
            if (!String.IsNullOrEmpty(sbReturnIndexToolsData.ToString()))
            {
                sbResult.Append(",\"IndexToolsData\"" + sbReturnIndexToolsData.ToString());
            }
            if (!String.IsNullOrEmpty(sbReturnRowData.ToString()))
            {
                sbResult.Append(",\"UserFirstLevelMenu\"" + sbReturnRowData.ToString() + "");
            }
            if (!String.IsNullOrEmpty(sbReturnLicData.ToString()))
            {
                sbResult.Append(",\"LicenseData\":" + sbReturnLicData.ToString());
            }
            
            sbResult.Append("}");
            //log.Error(strMethodDesc + "Return Json:" + sbResult.ToString());
        }
        return sbResult.ToString();
    }

    /// <summary>
    /// 获取系统授权文件
    /// </summary>
    private String GetSystemLisence(ResourceManager rmLocResourceManager_License)
    {
        StringBuilder sbResultData = new StringBuilder();
        String strProductName = "";
        String strVersion = "";
        String strClientName = "";
        String strValidDate = "";
        this.strIsLicValid = "1";
        this.strLicInValidMsg = "";
        //截止有效期距离今天的天数，大于0表示在今天之后
        int iValidDateDiffDays = -9999;

        sbResultData.Append("{");

        if(rmLocResourceManager_License==null)
        {
            rmLocResourceManager_License = base.GetResourceManager("License");
        }
        RegistBll bllLicense = new RegistBll();
        LicenseEntity entity = bllLicense.GetLicenseInfo();
        if ((entity != null) && (!String.IsNullOrEmpty(entity.strClientNameChs)))
        {
            //判断是否根据机器码绑定，如果存在机器码但是不匹配则退出
            if ((!String.IsNullOrEmpty(entity.strMachine)) && (!entity.strMachine.Equals(this.GetServerMachineCode())))
            {
                RegistBll.LicenseInfo = null;
                RegistBll.LicenseIsValid = false;
                this.strIsLicValid = "0";
                this.strLicInValidMsg = rmLocResourceManager_License.GetString("Label_NoLicense");
            }else
            {
                strProductName = entity.strProductName;
                strVersion = entity.strVersion;
                strClientName = this.Language.Equals("zh-cn")? entity.strClientNameChs: entity.strClientName;
                DateTime dtValidDate = entity.dtValid;
                strValidDate = dtValidDate.Date.ToString("yyyy-MM-dd");
                //判断有效期是否小于提醒期限
                iValidDateDiffDays = dtValidDate.Subtract(DateTime.Now).Days+1;

                RegistBll.LicenseIsValid = true;
                if (iValidDateDiffDays <= 0)
                {
                    RegistBll.LicenseIsValid = false;
                    this.strIsLicValid = "0";
                    this.strLicInValidMsg = rmLocResourceManager_License.GetString("Label_LicenseExpired");
                }
            }
            //将License写入数据库 add by sammen 20220610
            bllLicense.WriteLicenseInfoToDB(entity,this.GetUserCode(),this.GetProjectId(),this.GetServerMachineCode());
        }
        else
        {
            RegistBll.LicenseIsValid = false;
            this.strIsLicValid = "0";
            this.strLicInValidMsg = rmLocResourceManager_License.GetString("Label_NoLicense");
        }
        sbResultData.Append("\"ProductName\":\"" + strProductName + "\"");
        sbResultData.Append(",\"Version\":\"" + strVersion + "\"");
        sbResultData.Append(",\"ClientName\":\"" + strClientName + "\"");
        sbResultData.Append(",\"ValidDate\":\"" + strValidDate + "\"");
        sbResultData.Append(",\"IsValid\":\"" + this.strLicInValidMsg + "\"");
        sbResultData.Append(",\"InValidMsg\":\"" + this.strLicInValidMsg + "\"");
        sbResultData.Append(",\"ValidDateDiffDays\":\"" + iValidDateDiffDays.ToString() + "\"");
        sbResultData.Append(",\"DiffDaysNoticeMsg\":\"" + rmLocResourceManager_License.GetString("Label_LicenseTip") + "\"");
        sbResultData.Append(",\"DiffDaysNoticeDays\":\"" + BaseParamsGetter.GetBasicParamValue("iLicenseWarningDay") + "\"");

        sbResultData.Append("}");
        return sbResultData.ToString();

    }

    /// <summary>
    /// 获取该应用的主页面链接
    /// </summary>
    private String GetLanguageJsonString(ResourceManager rmLocResourceManager_TopFrame)
    {
        String str_Chinese = rmLocResourceManager_TopFrame.GetString("txt_Chinese");
        String str_English = rmLocResourceManager_TopFrame.GetString("txt_English");

        StringBuilder sbResult = new StringBuilder();
        sbResult.Append("[");
        sbResult.Append("{\"value\":\"zh-cn\",\"name\":\"" + str_Chinese + "\",\"active\":\""+ ((this.Language == "zh-cn") ?"1":"0")+ "\"}");
        sbResult.Append(",{\"value\":\"en-us\",\"name\":\"" + str_English + "\",\"active\":\"" + ((this.Language == "zh-cn") ? "0" : "1") + "\"}");
        sbResult.Append("]");

        return sbResult.ToString();
    }

    /// <summary>
    /// 获取该应用的主页面链接
    /// </summary>
    private String GetHomePageUrl()
    {
        String strHomePage = "Welcome.aspx";
        //获取该应用的主页面链接
        String strConfigHomePage = BaseParamsGetter.GetBasicParamValue("DefaultHomePageUrl");
        if (String.IsNullOrEmpty(strConfigHomePage))
        {
            strHomePage = "Welcome.aspx";
            log.Error("topFrame.GetHomePageUrl() error：参数配置中不存在DefaultHomePageUrl的配置项目!");
        }
        else
        {
            strHomePage = strConfigHomePage;
        }
        return strHomePage;
    }

    /// <summary>
    /// 动态加载登陆用户的第一级功能菜单
    /// </summary>
    private string GetUserFirstLevelMenu()
    {
        StringBuilder sbResult = new StringBuilder();
        StringBuilder sbSql = new StringBuilder();
        try
        {
            //获取第一级别菜单
            String strRootMenuCode = BaseParamsGetter.GetBasicParamValue("MenuRootId");
            sbSql.Append("SELECT rm.[SMENUCODE],rm.[SMENUNAME],rm.[SMENUNAMECN],rm.[SURLDETAIL],rm.[SMENUTYPE],rm.[SIMAGE],rm.[SPARENTCODE],rm.[SLEVEL],rm.[SORDER],rm.[BISSTOP] \r\n");
            if (Com.ValuePlus.BLL.User.UserLoginBll.IsAdminstratorUser())
            {
                sbSql.Append(" FROM [TB_HR_MENU] rm  \r\n");
                sbSql.Append(" where 1=1 \r\n");
            }
            else
            {
                sbSql.Append(" FROM [TB_HR_MENU] rm inner join [TB_HR_USERMENU] ru on rm.[SMENUCODE]=ru.[SMENUCODE]  \r\n");
                sbSql.Append(" where ru.[SRIGHTS]='1' and ru.[SUSERID]='"+this.GetUserCode()+ "'  \r\n");
            }
            sbSql.Append(" and rm.[BISSTOP]=0 and SPARENTCODE = '" + strRootMenuCode + "'  \r\n");
            sbSql.Append(" ORDER BY [SORDER] ASC  \r\n");

            String strSql = sbSql.ToString();
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);

            sbResult.Append(WebCommon.GetJsonStringByDataTable(dt, "", false));
        } catch(Exception ex){

        }
        return sbResult.ToString();

    }

    /// <summary>
    /// 获取首页文档工具集
    /// </summary>
    private string GetIndexToolsList()
    {
        StringBuilder sbResult = new StringBuilder();
        StringBuilder sbSql = new StringBuilder();
        try
        {
            sbSql.Append("select * from IndexTools_1 where IsValid = '1' order by DORDER \r\n");

            String strSql = sbSql.ToString();
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);

            sbResult.Append(WebCommon.GetJsonStringByDataTable(dt, "", false));
        }
        catch (Exception ex)
        {

        }
        return sbResult.ToString();

    }

    /// <summary>
    /// 变更当前语言
    /// </summary>
    /// <returns></returns>
    private String ChangeLanguage(String strChangeLanguage)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbReturnRowData = new StringBuilder();
        StringBuilder sbReturnColumnData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "变更当前语言";

        try
        {
            this.Language = strChangeLanguage;

            strReturnCode = "1";
            strReturnMsg = strMethodDesc + "成功";
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = strMethodDesc + "出错";
            log.Error(ex);
        }
        finally
        {
            sbResult.Append("{");
            sbResult.Append("\"ReturnCode\":\"" + strReturnCode.ToString() + "\"");
            sbResult.Append(",\"ReturnMsg\":\"" + strReturnMsg.ToString() + "\"");
            sbResult.Append("}");
            //log.Error(strMethodDesc + "Return Json:" + sbResult.ToString());
        }
        return sbResult.ToString();
    }

    /// <summary>
    /// 通过MenuId获取其完整路径
    /// </summary>
    /// <param name="strParentMenuId"></param>
    /// <returns></returns>
    private String GetMenuPathByMenuId(String strMenuId)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbReturnData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        StringBuilder sbSql = new StringBuilder();
        String strMethodDesc = "通过MenuId获取其完整路径";

        try
        {
            if (String.IsNullOrEmpty(strMenuId))
            {
                strMenuId = BaseParamsGetter.GetBasicParamValue("MenuRootId");
            }
            sbSql.Append("select * from [Fun_SYS_GetMenuPathByMenuCode]('" + strMenuId + "') ORDER BY SLEVEL \r\n");
            String strSql = sbSql.ToString();
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);

            sbReturnData.Append(WebCommon.GetJsonStringByDataTable(dt, "", false));


            strReturnCode = "1";
            strReturnMsg = strMethodDesc + "成功";
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = strMethodDesc + "出错";
            log.Error(ex);
        }
        finally
        {
            sbResult.Append("{");
            sbResult.Append("\"ReturnCode\":\"" + strReturnCode.ToString() + "\"");
            sbResult.Append(",\"ReturnMsg\":\"" + strReturnMsg.ToString() + "\"");
            sbResult.Append(",\"ReturnLanguage\":\"" + this.Language.ToString() + "\"");
            sbResult.Append(",\"ReturnMenuCode\":\"" + strMenuId + "\"");
            if (!String.IsNullOrEmpty(sbReturnData.ToString()))
            {
                sbResult.Append(",\"ReturnData\"" + sbReturnData.ToString());
            }
            sbResult.Append("}");
            //log.Error(strMethodDesc + "Return Json:" + sbResult.ToString());
        }
        return sbResult.ToString();
    }


    /// <summary>
    /// 退出当前系统
    /// </summary>
    /// <returns></returns>
    private String ExitApplication()
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "退出当前系统";
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
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "", "<script language=javascript>window.parent.location.href = 'SSO.aspx';</script>");
            }
            else
            {
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "", "<script language=javascript>window.parent.location.href = 'login.aspx';</script>");
            }
            strReturnCode = "1";
            strReturnMsg = strMethodDesc + "成功";
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = strMethodDesc + "出错";
            log.Error(ex);
        }
        finally
        {
            sbResult.Append("{");
            sbResult.Append("\"ReturnCode\":\"" + strReturnCode.ToString() + "\"");
            sbResult.Append(",\"ReturnMsg\":\"" + strReturnMsg.ToString() + "\"");
            sbResult.Append("}");
            //log.Error(strMethodDesc + "Return Json:" + sbResult.ToString());
        }
        return sbResult.ToString();
    }

}