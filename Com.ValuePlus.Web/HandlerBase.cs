using System;
using System.Collections.Generic;
using System.Text;
using Com.ValuePlus.Utils;
using System.Collections;
using System.Web;
using Com.ValuePlus.Entity;
using System.Resources;
using System.Reflection;
using Com.ValuePlus.BLL.User;

namespace Com.ValuePlus.Web
{
    public class HandlerBase
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
            System.Web.HttpBrowserCapabilities browser = HttpContext.Current.Request.Browser;
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
            return String.Format(GetSiteSchema() + "://{0}/", RequestUtils.GetCurrentFullHost());
        }

        #endregion

        /// <summary>
        /// 获取服务器端日期
        /// </summary>
        /// <returns></returns>
        public string GetServerDate()
        {
            return DateTime.Now.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// 获取服务器端日期时间
        /// </summary>
        /// <returns></returns>
        public string GetServerDateTime()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

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
        public UserInfo GetUserInfo()
        {
            return Com.ValuePlus.BLL.User.UserLoginBll.LoginUserInfo;
        }
        #endregion

        #region 获得或设置语言
        /// <summary>
        /// 获得或设置语言
        /// </summary>
        public string Language
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

        /// <summary>
        /// 根据特殊算法分析url连接，返回参数及其值的hashtable
        /// </summary>
        /// <param name="strUrlQueryString"></param>
        /// <returns>Hashtable</returns>
        public Hashtable GetUrlAnalyse(HttpContext context)
        {
            String strUrlQueryString = context.Server.UrlDecode(context.Request.Url.Query.ToString());
            return WebCommon.GetUrlAnalyse(strUrlQueryString);
        }

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
}
