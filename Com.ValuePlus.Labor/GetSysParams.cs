using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ValuePlus.Labor
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
            return String.IsNullOrEmpty(StrAppId) ? Const.Weixin_appid : StrAppId;
        }

        /// <summary>
        /// 从系统参数配置中获取开发者密码(AppSecret),如参数中未配置，则获取常量
        /// </summary>
        /// <returns></returns>
        public static string GetWeixin_Appsecret()
        {
            String StrAppsecret = Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("Weixin_appsecret");
            return String.IsNullOrEmpty(StrAppsecret) ? Const.Weixin_appsecret : StrAppsecret;
        }

        /// <summary>
        /// 从系统参数配置中发送服务通知时待办工单的模板消息的id,如参数中未配置，则返回空
        /// </summary>
        /// <returns></returns>
        public static string GetWeixin_TemplateId_WorkOrderPending()
        {
            String StrTemplateId_WorkOrderPending = Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("Weixin_TemplateId_WorkOrderPending");
            return String.IsNullOrEmpty(StrTemplateId_WorkOrderPending) ? "" : StrTemplateId_WorkOrderPending;
        }
    }

}
