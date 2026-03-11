using System;
using System.Collections.Generic;
using System.Web;
using System.Text;
using System.Web.UI;
using Com.ValuePlus.Entity;
using System.Resources;
using Com.ValuePlus.BLL.User;
using System.Reflection;
using Com.ValuePlus.Utils;
using Com.ValuePlus.Common;
using Com.ValuePlus.DAL;

namespace Com.ValuePlus.Web
{
    public class PageBase:Page
    {


        /// <summary>
        /// 日志声明
        /// </summary>
        protected static Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 获得客户端浏览器类型 
        /// add by sammen 20220104
        /// </summary>
        /// <returns></returns>
        public string GetBrowserType()
        {
            System.Web.HttpBrowserCapabilities browser = Request.Browser;
            return browser.Type.ToString();
        }

        #region 获得站点的域名
        /// <summary>
        /// 获得站点Scheme(http/https)
        /// </summary>
        /// <returns></returns>
        public string GetSiteSchema()
        {
            //modify by sammen 20181130 增加对https网站的支持
            return HttpContext.Current.Request.Url.Scheme.ToString();
        }
        /// <summary>
        /// 获得站点的域名
        /// </summary>
        /// <returns></returns>
        public string GetSiteWebAddress()
        {
            return String.Format(GetSiteSchema() + "://{0}/",RequestUtils.GetCurrentFullHost());
        }

        #endregion

        /// <summary>
        /// 获取请求客户端IP地址
        /// </summary>
        /// <returns></returns>
        public string GetClientIPAddress()
        {
            return Com.ValuePlus.Utils.RequestUtils.GetIP();
        }

        #region 获得登录用户信息
        /// <summary>
        /// 获得登录用户信息
        /// </summary>
        /// <returns></returns>
        public  UserInfo GetUserInfo()
        {
            return Com.ValuePlus.BLL.User.UserLoginBll.LoginUserInfo;         
        }
        #endregion

        #region 弹出对话框
        /// <summary>
        /// 弹出对话框
        /// </summary>
        /// <param name="page"></param>
        /// <param name="values"></param>
        public void AlertMessageBox(System.Web.UI.Page page, string values)
        {
            page.ClientScript.RegisterStartupScript(page.GetType(), "", "<script language=javascript>alert('" + values + "')</script>"); 
        }
        #endregion

        #region 获得或设置语言
        /// <summary>
        /// 获得或设置语言
        /// </summary>
        public  string Language
        {
            get
            {
                return Com.ValuePlus.BLL.User.UserLoginBll.Language;
            }
            set
            {
                Com.ValuePlus.BLL.User.UserLoginBll.Language = value;               
            }
        }
        #endregion

        #region 获得服务器机器码
        /// <summary>
        /// 获得服务器机器码
        /// </summary>
        /// <returns></returns>
        public string GetServerMachineCode()
        {
            UserInfo userInfo = Com.ValuePlus.BLL.User.UserLoginBll.LoginUserInfo;
            if (userInfo != null)
            {
                return userInfo.MachineCode;
            }
            return string.Empty;
        }
        #endregion  

        #region 获得用户编号
        /// <summary>
        /// 获得用户编号
        /// </summary>
        /// <returns></returns>
        public string GetUserCode()
        {
            UserInfo userInfo = Com.ValuePlus.BLL.User.UserLoginBll.LoginUserInfo;
            if (userInfo != null)
            {
                return userInfo.SUSERID;
            }
            return string.Empty;
        }
        #endregion       

        #region 获得用户帐号
        /// <summary>
        /// 获得用户帐号
        /// </summary>
        /// <returns></returns>
        public string GetUserAccount()
        {
            UserInfo userInfo = Com.ValuePlus.BLL.User.UserLoginBll.LoginUserInfo;
            if (userInfo != null)
            {
                return userInfo.SACCOUNTID;
            }
            return string.Empty;
        }
        #endregion

        #region 获得用户登录密码
        /// <summary>
        /// 获得用户编号
        /// </summary>
        /// <returns></returns>
        public string GetAccountPassword()
        {
            UserInfo userInfo = Com.ValuePlus.BLL.User.UserLoginBll.LoginUserInfo;
            if (userInfo != null)
            {
                return userInfo.SPWD;
            }
            return string.Empty;
        }
        #endregion

        #region 获得用户部门
        /// <summary>
        /// 获得用户部门
        /// </summary>
        /// <returns></returns>
        public string GetUserDept()
        {
            UserInfo userInfo = Com.ValuePlus.BLL.User.UserLoginBll.LoginUserInfo;
            if (userInfo != null)
            {
                return userInfo.SDEPTCODE;
            }
            return string.Empty;
        }
        #endregion  

        #region 判断是否是超级管理员
        /// <summary>
        /// 是否是超级管理员为ture为超级管理员，否为普通用户
        /// </summary>
        /// <returns></returns>
        public bool IsAdminstrator()
        {
            return Com.ValuePlus.BLL.User.UserLoginBll.IsAdminstratorUser();
        }
        #endregion

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

        #region 获得远程服务器站点的域名
        /// <summary>
        /// 获得远程服务器站点的域名
        /// </summary>
        /// <returns></returns>
        public string GetRemoteServer()
        {
            String strServer = Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("SysUpdateServer");
            if (String.IsNullOrEmpty(strServer))
            {
                strServer = "http://115.29.110.153";
            }
            return strServer.TrimEnd('/');
        }
        #endregion

        #region 获取本项目标识
        /// <summary>
        /// 获取本项目标识
        /// </summary>
        /// <returns></returns>
        public string GetProjectId()
        {
            return Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("ProjectId");
        }
        #endregion

        #region 页面事件重载
        /// <summary>
        /// 重写页面初始化
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            //获取cookie值
            string sUserInfo = Com.ValuePlus.Utils.Cookie.CookieHelper.GetCookie(CacheName.LoginCookieName);
            if (String.IsNullOrEmpty(sUserInfo))
            {
                //清除Cookie同时清除所有session
                UserLoginBll.AbandomSession();
                Response.Redirect(string.Format("{0}{1}", GetSiteWebAddress(), "Error.aspx?ErrCode=001"), true);
            }else{
                // add by sammen 20240130 每次页面初始化的时候，再写一次cookie,有效期时间再推迟
                int iTimeOut = 0;
                String strTimeOut = Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("SessionTimeOut");
                if (!String.IsNullOrEmpty(strTimeOut))
                {
                    iTimeOut = int.Parse(strTimeOut);
                }
                Com.ValuePlus.Utils.Cookie.CookieHelper.WriteCookie(CacheName.LoginCookieName, sUserInfo, iTimeOut);
            }

            UserInfo userlogin = GetUserInfo();
            if (userlogin == null)
            {
                //清除Cookie同时清除所有session
                UserLoginBll.AbandomSession();
                ////Com.ValuePlus.BLL.User.LogHandle.register(this.GetUserAccount(), "20", Com.ValuePlus.Utils.RequestUtils.GetIP(), "20");
                //DataLogWriter.Log_LogOut(this.GetUserCode(), Com.ValuePlus.Utils.RequestUtils.GetIP(), LogActionType.LogOut_Success);
                Response.Redirect(string.Format("{0}{1}",GetSiteWebAddress(),"Error.aspx?ErrCode=001"),true);
            }
            else
            {
                //判断相同主机名的应用是否共享登录session
                String IsShareSession = Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("IsShareSessionSameHost");
                if(String.IsNullOrEmpty(IsShareSession)){
                    StringBuilder sbTempInsertSql = new StringBuilder();
                    sbTempInsertSql.Append("if not exists (select 1 from BASICPARAM_1 where paramName = 'IsShareSessionSameHost') \r\n");
                    sbTempInsertSql.Append("begin \r\n");
                    sbTempInsertSql.Append("insert into BASICPARAM_1(paramName,paramValue,paramDesc,label,isStop)");
                    sbTempInsertSql.Append("values('IsShareSessionSameHost','0','相同主机名的应用是否共享登录session','共享登录session','2')");
                    sbTempInsertSql.Append("end \r\n");
                    SqlParamDao.ExecuteNonQueryBySql(sbTempInsertSql.ToString());
                    IsShareSession = "0";
                }

                //判断是否需要登录
                bool isNeedLogin = false;
                if (IsShareSession.Equals("1"))
                {
                    //如果同主机名共享session,且获取不到用户名，则需要登录
                    if (string.IsNullOrEmpty(userlogin.SUSERID))
                    {
                        isNeedLogin = true;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(userlogin.SUSERID)
                        || string.IsNullOrEmpty(userlogin.WebSiteHostUrl)
                        || !userlogin.WebSiteHostUrl.ToString().ToLower().Equals(GetSiteWebAddress().ToLower()))
                    {
                        //如果同主机名不共享session,且获取不到用户名或者当前主机名和session中主机名不一致，则需要登录
                        isNeedLogin = true;
                    }
                }
                
                //如果需要登录
                if (isNeedLogin)
                {
                    //清除Cookie同时清除所有session
                    UserLoginBll.AbandomSession();
                    //Com.ValuePlus.BLL.User.LogHandle.register(this.GetUserAccount(), "20", Com.ValuePlus.Utils.RequestUtils.GetIP(), "20");
                    //DataLogWriter.Log_LogOut(this.GetUserCode(), Com.ValuePlus.Utils.RequestUtils.GetIP(), LogActionType.LogOut_Success);
                    Response.Redirect(string.Format("{0}{1}", GetSiteWebAddress(), "Error.aspx?ErrCode=001"), true);
                }
            }
            base.OnInit(e);
        }

        /// <summary>
        /// 出错处理
        /// </summary>
        /// <param name="e"></param>
        protected override void OnError(EventArgs e)
        {
            //页面发生错误，记录页面错误
            Exception exception = Server.GetLastError();
            if (exception != null)
            {

                Com.ValuePlus.BLL.User.UserErr.InertError(this.GetUserCode(), "", "", "", exception.ToString());

                Com.ValuePlus.Log.LogFactory.CreateInstance().Error(string.Format("页面出错:Date:{0},message:{1},stacktrace:{2},url:{3}", DateTime.Now.ToString(), exception.Message, exception.StackTrace, Request.Url.ToString()));
                //Response.Redirect(string.Format("{0}{1}", GetSiteWebAddress(), "Error.aspx?ErrCode=999"));
            }
            base.OnError(e);

        }
        #endregion

    }
}
