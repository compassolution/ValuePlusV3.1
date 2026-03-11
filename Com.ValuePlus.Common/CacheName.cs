using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ValuePlus.Common
{
    public class CacheName
    {

        #region 缓存提示语句名字
        /// <summary>
        /// 缓存提示语句名字
        /// </summary>
        /// <returns></returns>
        public static string TipsCacheName
        {
            get{ 
                return "TipsSchemaCacheName";
            }
        }
        #endregion
        
        #region 用户登录信息session名字
        /// <summary>
        /// 用户登录信息session名字
        /// </summary>
        /// <returns></returns>
        public static string LoginSessionName
        {
            get
            {
                return "UserLoginSessionName";
            }
        }
        #endregion

        #region 用户登录信息Cookie名字
        /// <summary>
        /// 用户登录信息Cookie名字
        /// </summary>
        /// <returns></returns>
        public static string LoginCookieName
        {
            get
            {
                return "UserLoginCookieName";
            }
        }
        #endregion

        #region 软件授权License信息Cookie名字
        /// <summary>
        /// 软件授权License信息Cookie名字
        /// </summary>
        /// <returns></returns>
        public static string LicenseCookieName
        {
            get
            {
                return "LicenseCookieName";
            }
        }
        /// <summary>
        /// 软件授权License信息Session名字
        /// </summary>
        /// <returns></returns>
        public static string LicenseSessionName
        {
            get
            {
                return "LicenseSessionName";
            }
        }
        #endregion

        #region 软件授权License是否有效的Cookie名字
        /// <summary>
        /// 软件授权License是否有效的Cookie名字
        /// </summary>
        /// <returns></returns>
        public static string LicenseIsValidCookieName
        {
            get
            {
                return "LicenseIsValidCookieName";
            }
        }
        /// <summary>
        /// 软件授权License是否有效的Session名字
        /// </summary>
        /// <returns></returns>
        public static string LicenseIsValidSessionName
        {
            get
            {
                return "LicenseIsValidSessionName";
            }
        }
        #endregion

        #region 访问数据源链接节点标识session名字
        /// <summary>
        /// 访问数据源链接节点标识session名字
        /// </summary>
        /// <returns></returns>
        public static string DBConnectSessionName
        {
            get
            {
                return "DBConnectSessionName";
            }
        }
        #endregion

    }
}
