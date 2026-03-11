using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Collections;
namespace Com.ValuePlus.Utils.Cookie
{
    public class CookieHelper
    {       

        #region 写cookie值
        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="strValue">值</param>
        /// <param name="strValue">过期时间(分钟)</param>
        public static void WriteCookie(string strName, string strValue, int expires)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie.Value = strValue;
            if (expires > 0)
            {
                cookie.Expires = DateTime.Now.AddMinutes(expires);
            }
            HttpContext.Current.Response.AppendCookie(cookie);

        }
        #endregion

        #region 写cookie值
        /// <summary>
        /// 写cookie值
       /// </summary>
        /// <param name="strName">名称</param>
       /// <param name="doMain">域</param>
        /// <param name="strValue">值</param>
        /// <param name="expires">过期时间(分钟)</param>
        public static void WriteCookie(string strName, string doMain,string strValue, int expires)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie.Domain = doMain;
            cookie.Value = strValue;
            if (expires > 0)
            {
                cookie.Expires = DateTime.Now.AddMinutes(expires);
            }
            HttpContext.Current.Response.AppendCookie(cookie);

        }
        #endregion

        #region 读cookie值
        /// <summary>
        /// 读cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <returns>cookie值</returns>
        public static string GetCookie(string strName)
        {
            if (HttpContext.Current.Request.Cookies != null && HttpContext.Current.Request.Cookies[strName] != null)
            {
                return HttpContext.Current.Request.Cookies[strName].Value.ToString();
            }
            return "";
        }
        #endregion

        #region 写cookie值
        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="doMain">域</param>
        /// <param name="strValue">值</param>
        /// <param name="expires">过期时间(分钟)</param>
        public static void WriteCookie(string strName, string doMain, Hashtable strValue, int expires)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            if (!string.IsNullOrEmpty(doMain))
            {
                cookie.Domain = doMain;
            }
            System.Collections.IDictionaryEnumerator enumerator = strValue.GetEnumerator();
            while (enumerator.MoveNext())
            {
                cookie[enumerator.Key.ToString()] = enumerator.Value.ToString();
            }
            if (expires > 0)
            {
                cookie.Expires = DateTime.Now.AddMinutes(expires);
            }
            HttpContext.Current.Response.AppendCookie(cookie);

        }
        #endregion         

        #region 读cookie值
        /// <summary>
        /// 读cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <returns>cookie值</returns>
        public static Hashtable GetCookieHt(string strName)
        {
            Hashtable ht = null;
            if (HttpContext.Current.Request.Cookies != null && HttpContext.Current.Request.Cookies[strName] != null)
            {
                System.Web.HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies[strName];
                ht = new Hashtable();
                foreach (string nv in cookie.Values.AllKeys)
                {
                    ht.Add(nv, cookie.Values[nv]);
                }
            }
            return ht;
        }
        #endregion
    }
}
