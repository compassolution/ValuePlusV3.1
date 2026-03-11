using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.ValuePlus.Weixin
{
    /// <summary>
    /// 从系统参数配置中获取对应的参数
    /// </summary>
    public class GetSysParams
    {
        /// <summary>
        /// 从系统参数配置中获取公众号开发者ID(AppID),如参数中未配置，则获取常量
        /// </summary>
        /// <returns></returns>
        public static string GetWeixin_AppId()
        {
            String StrAppId = Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("Weixin_appid");
            return String.IsNullOrEmpty(StrAppId) ? Const.Weixin_appid: StrAppId;
        }

        /// <summary>
        /// 从系统参数配置中获取公众号开发者密码(AppSecret),如参数中未配置，则获取常量
        /// </summary>
        /// <returns></returns>
        public static string GetWeixin_Appsecret()
        {
            String StrAppsecret = Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("Weixin_appsecret");
            return String.IsNullOrEmpty(StrAppsecret) ? Const.Weixin_appsecret : StrAppsecret;
        }

        /// <summary>
        /// 从系统参数配置中获取小程序-VP人事小微 开发者ID(AppID),如参数中未配置，则获取常量
        /// </summary>
        /// <returns></returns>
        public static string GetWeixin_AppId_EasyHR()
        {
            String StrAppId = Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("Weixin_appid_EasyHR");
            return String.IsNullOrEmpty(StrAppId) ? Const.Weixin_appid : StrAppId;
        }

        /// <summary>
        /// 从系统参数配置中获取小程序-VP人事小微 开发者密码(AppSecret),如参数中未配置，则获取常量
        /// </summary>
        /// <returns></returns>
        public static string GetWeixin_Appsecret_EasyHR()
        {
            String StrAppsecret = Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("Weixin_appsecret_EasyHR");
            return String.IsNullOrEmpty(StrAppsecret) ? Const.Weixin_appsecret : StrAppsecret;
        }

    }
}
