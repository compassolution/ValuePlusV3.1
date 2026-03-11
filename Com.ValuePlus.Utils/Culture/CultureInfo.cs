using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Globalization;
namespace Com.ValuePlus.Utils.Culture
{
    public class CultureInfo
    {

        /// <summary>
        /// 获得客户端浏览器语言
        /// </summary>
        /// <returns></returns>
        public static string GetCustomCulture()
        {
            return System.Web.HttpContext.Current.Request.UserLanguages[0];
        }

        /// <summary>
        /// 设置用户线程文化
        /// </summary>
        /// <param name="language"></param>
        public static void SetCurrentRequestCulture(string language)
        {
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture(language);
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(language);
        }
    }
}
