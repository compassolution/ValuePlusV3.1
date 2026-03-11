using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Com.ValuePlus.Web;
using System.Data;
using Com.ValuePlus.DAL;
using System.Drawing;
using Com.ValuePlus.BLL.User;
using Com.ValuePlus.Entity;
using Com.ValuePlus.DataLog;
using Com.ValuePlus.DataLog.Enum;

public partial class News_View_PadView : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (Request.Params["newsId"] != null)
            {
                this.strNewsId = Request.Params["newsId"].ToString();

                this.strLanguage = "zh";
                if (Request.Params["lang"] != null)
                {
                    this.strLanguage = Request.Params["lang"].ToString();
                }
                try
                {
                    //默认以管理员是身份登录
                    //this.LoginSystem();
                    this.GetNewsDetailInfo(this.strNewsId);
                }
                catch (Exception ex)
                {
                    //log.Error(ex);
                }
            }
        }
    }

    #region viewstate初始化区域
    private string strNewsId
    {
        get
        {
            return ViewState["strNewsId_ViewState"] as string;
        }
        set
        {
            ViewState["strNewsId_ViewState"] = value;
        }
    }
    private string strLanguage
    {
        get
        {
            return ViewState["strLanguage_ViewState"] as string;
        }
        set
        {
            ViewState["strLanguage_ViewState"] = value;
        }
    }
    #endregion

    ///// <summary>
    ///// 默认以管理员是身份登录
    ///// </summary>
    //private void LoginSystem()
    //{
    //    UserInfo userInfo = GetUserInfo();
    //    if (userInfo == null)
    //    {
    //        UserBll bllUser = new UserBll();
    //        String strAccount = "admin";
    //        String strSql = "SELECT * FROM TB_HR_USER WHERE SUSERID = 'ADMIN'";
    //        DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
    //        if ((dt != null) && (dt.Rows.Count > 0))
    //        {
    //            String strPwd = dt.Rows[0]["PWD"].ToString();
    //            try
    //            {
    //                userInfo = bllUser.login(strAccount, strPwd, System.Guid.NewGuid().ToString("N"), Com.ValuePlus.Utils.RequestUtils.GetIP());
    //                userInfo.Loginip = Com.ValuePlus.Utils.RequestUtils.GetIP();
    //                userInfo.Logintiem = System.DateTime.Now;

    //                DataLogWriter.Log_Login(strAccount, Com.ValuePlus.Utils.RequestUtils.GetIP(), LogActionType.Login_Success);
    //                UserLoginBll.LoginUserInfo = userInfo;
    //            }
    //            catch (Exception ex)
    //            {
    //                log.Error(ex);

    //            }
    //        }
    //    }
    //}

    #region 获取相应新闻的明细信息
    /// <summary>
    /// 获取相应新闻的明细信息
    /// </summary>
    /// <returns></returns>
    private void GetNewsDetailInfo(String strNewsId)
    {
        String strSql = "SELECT * FROM TB_NEWS_CONTENT WHERE SNEWSID = '" + strNewsId + "'";
        DataTable dtNewsInfo = SqlParamDao.GetDataTableBySql(strSql);
        if ((dtNewsInfo != null) && (dtNewsInfo.Rows.Count > 0))
        {
            if (this.strLanguage.Equals("en"))
            {
                this.divContent.InnerHtml = dtNewsInfo.Rows[0]["SCONTENT"].ToString();
            }
            else
            {
                this.divContent.InnerHtml = dtNewsInfo.Rows[0]["SCONTENTCHS"].ToString();
            }
        }
    }
    #endregion

}
