using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using Com.ValuePlus.Entity;
using Com.ValuePlus.Common;
using Com.ValuePlus.Utils;
using Com.ValuePlus.Common.Config;
using Com.ValuePlus.SysParams;
using System.Data;
using Com.ValuePlus.DAL;

namespace Com.ValuePlus.BLL.User
{
    /// <summary>
    /// 用户登录基本操作
    /// </summary>
    public class UserLoginBll
    {

        /// <summary>
        /// 判断是否需要修改密码
        /// 判断该用户是否首次登陆修改过密码，是否需要强制修改密码
        /// </summary>
        /// <param name="strUserId"></param>
        /// <returns>0:不需要修改密码</returns>
        /// <returns>1:首次登陆需要修改密码</returns>
        /// <returns>2:密码已过期,请及时修改密码</returns>
        public String JudgeIsNeedChangePassword(String strUserId)
        {
            String IsNeed = "0";

            //首次登陆是否需要修改密码
            String strIsChangePasswordFirst = BaseParamsGetter.GetBasicParamValue("IsChangePasswordFirst");
            if ((!String.IsNullOrEmpty(strIsChangePasswordFirst)) && (strIsChangePasswordFirst.Equals("1")))
            {
                //获取最后一次修改密码的日期
                String strSql = "select MAX(DTCHANGEDATE) as DTCHANGEDATE from TB_HR_CHANGEPWD_HIS where SUSERID = '" + strUserId + "'";
                DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
                if ((dt != null) && (dt.Rows.Count > 0) && (dt.Rows[0]["DTCHANGEDATE"] != null) && (!String.IsNullOrEmpty(dt.Rows[0]["DTCHANGEDATE"].ToString())))//存在修改密码的记录
                {
                    DateTime dtLastChange = DateTime.Parse(dt.Rows[0]["DTCHANGEDATE"].ToString());
                    ///强制要求修改密码的周期(单位为天，0表示不需要强制修改密码)
                    String StrDaysChangePassword = BaseParamsGetter.GetBasicParamValue("DaysChangePassword");
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
        ///  从数据库获取用户详细信息
        /// </summary>
        /// <param name="strUserID"></param>
        /// <returns></returns>
        public UserInfo GetUserInfoFromDBByUserID(String strUserID)
        {
            UserInfo userInfo = new UserInfo();
            userInfo = UserInfoDao.getUserInfoByUserId(strUserID);
            return userInfo;
        }

        #region 获得或设置登录用户信息
        /// <summary>
        /// 获得或设置登录用户信息
        /// </summary>
        /// <returns></returns>
        public static UserInfo LoginUserInfo
        {
            get{
                return GetUserInfoFromSessionOrCookie;
            }
            set{
                GetUserInfoFromSessionOrCookie =value;
            }
        }
        #endregion

        #region 获取或设置session和cookie信息
        /// <summary>
        /// 获取或设置
        /// </summary>
        private static UserInfo GetUserInfoFromSessionOrCookie
        {          
            
            get
            {
                //获取session值
                UserInfo userinfo = Com.ValuePlus.Utils.Session.SessionHelper.GetSession(CacheName.LoginSessionName) as UserInfo;
                if (userinfo != null && !string.IsNullOrEmpty(userinfo.SUSERID))
                {
                    return userinfo;
                }
                //获取cookie值
                string sUserInfo = Com.ValuePlus.Utils.Cookie.CookieHelper.GetCookie(CacheName.LoginCookieName);
                if (!string.IsNullOrEmpty(sUserInfo))
                {
                    try
                    {
                        //反序列化
                        sUserInfo = System.Web.HttpContext.Current.Server.UrlDecode(sUserInfo);
                        if(!string.IsNullOrEmpty(sUserInfo))
                        {
                            sUserInfo = Com.ValuePlus.Utils.Cryptography.CryptographyHelper.Decrypt3des(new byte[] { 0x11, 0x22, 0x5F, 0x68, (byte)0x88, 0x70, 0x40, 0x38, 0x28, 0x25, 0x79, 0x51, (byte)0xCB, (byte)0xDD, 0x55, 0x66, 0x77, 0x29, 0x74, (byte)0x98, 0x30, 0x40, 0x36, (byte)0xE2 }, new byte[] { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF }, sUserInfo, System.Text.Encoding.UTF8);
                            userinfo = Com.ValuePlus.Utils.Serializable.SerializableHelper.XMLDeserialize(sUserInfo, typeof(UserInfo), System.Text.Encoding.UTF8) as UserInfo;
                            if (userinfo != null && !string.IsNullOrEmpty(userinfo.SUSERID))
                            {
                                Com.ValuePlus.Utils.Session.SessionHelper.SetSession(CacheName.LoginSessionName ,userinfo);
                                return userinfo;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.LogFactory.CreateInstance().Error(ex);
                    }
                }
                return null;

            }
            set
            {
                //设置session
                Com.ValuePlus.Utils.Session.SessionHelper.SetSession(CacheName.LoginSessionName, value);
                if (value != null)
                {
                    //设置cookie
                    string sUserInfo0 = Com.ValuePlus.Utils.Serializable.SerializableHelper.XMLSerialize(value, System.Text.Encoding.UTF8);
                    sUserInfo0 = Com.ValuePlus.Utils.Cryptography.CryptographyHelper.Encrypt3des(new byte[] { 0x11, 0x22, 0x5F, 0x68, (byte)0x88, 0x70, 0x40, 0x38, 0x28, 0x25, 0x79, 0x51, (byte)0xCB, (byte)0xDD, 0x55, 0x66, 0x77, 0x29, 0x74, (byte)0x98, 0x30, 0x40, 0x36, (byte)0xE2 }, new byte[] { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF }, sUserInfo0, System.Text.Encoding.UTF8);
                    sUserInfo0 = System.Web.HttpContext.Current.Server.UrlEncode(sUserInfo0);

                    int iTimeOut = 0;
                    String strTimeOut = Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("SessionTimeOut");
                    if (!String.IsNullOrEmpty(strTimeOut))
                    {
                        iTimeOut = int.Parse(strTimeOut);
                    }
                    Com.ValuePlus.Utils.Cookie.CookieHelper.WriteCookie(CacheName.LoginCookieName, sUserInfo0, iTimeOut);
                }

            }
        }
        #endregion

        #region 登录验证
        /// <summary>
        /// 登录验证，返回是否登录的状态，true为已经登录
        /// </summary>
        /// <param name="userInfo">返回登录的用户信息</param>
        /// <returns></returns>
        public static bool AuthLoginUser(out UserInfo userInfo)
        {
            userInfo = LoginUserInfo;
            if (userInfo != null && !string.IsNullOrEmpty(userInfo.SUSERID))
            {                
                return true;
            }
            return false;
        }
        #endregion

        #region 判断是否是超级管理员
        /// <summary>
        /// 是否是超级管理员为ture为超级管理员，否为普通用户
        /// </summary>
        /// <returns></returns>
        public static bool IsAdminstratorUser()
        {
            UserInfo userInfo = LoginUserInfo;
            if (userInfo != null && !string.IsNullOrEmpty(userInfo.SUSERID))
            {
                if (userInfo.SACCOUNTID.ToLower() == "admin")
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region 获得或设置语言
        /// <summary>
        /// 获得或设置语言
        /// </summary>
        public static string Language
        {
            get
            {
                UserInfo userInfo = LoginUserInfo;
                string sLanguage = string.Empty;
                if (userInfo != null)
                {
                    sLanguage = userInfo.Language;
                }
                if (string.IsNullOrEmpty(sLanguage))
                {
                    sLanguage = BaseParamsGetter.GetBasicParamValue("DefaultLanguage");
                    //如果是下面四种设置值，则默认取服务器的文化语言
                    if (sLanguage.ToLower().Equals("none")|| sLanguage.ToLower().Equals("default")|| sLanguage.ToLower().Equals("na")|| sLanguage.ToLower().Equals("server"))
                    {
                        sLanguage = "";
                    }
                }
                if (string.IsNullOrEmpty(sLanguage))
                {
                    sLanguage = Com.ValuePlus.Utils.Culture.CultureInfo.GetCustomCulture();
                }
                if (string.IsNullOrEmpty(sLanguage))
                {
                    sLanguage = "zh-cn";
                }
                return sLanguage.ToLower(); 
            }
            set
            {
                UserInfo userInfo = LoginUserInfo;
                if (userInfo != null)
                {
                     userInfo.Language = value;
                     LoginUserInfo = userInfo;
                }
            }
        }
        #endregion

        #region 释放session回话
        /// <summary>
        /// 释放session回话
        /// </summary>
        public static void AbandomSession()
        {
            LoginUserInfo = null;
            Com.ValuePlus.Utils.Session.SessionHelper.RemoveSession(CacheName.LoginSessionName);
            Com.ValuePlus.Utils.Cookie.CookieHelper.WriteCookie(CacheName.LoginCookieName, "", 0);
        }
        #endregion

        #region 获取配置文件前必须调用设置当前的文化
        /// <summary>
        /// 获取配置文件前必须调用设置当前的文化
        /// </summary>
        public static void SetCulture()
        {
            //设置语言
            Com.ValuePlus.Utils.Culture.CultureInfo.SetCurrentRequestCulture(Language);
        }
        #endregion

    }
}
