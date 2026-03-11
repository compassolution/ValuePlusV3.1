using System;
using System.Collections.Generic;
using System.Text;
using Com.ValuePlus.Common.Config;

namespace Com.ValuePlus.SysParams
{

    /// <summary>
    /// 系统基本参数库的参数值获取类
    /// </summary>
    public class FlowConfigParamGetter
    {
        private static String strArchiveName = BaseConfig.Instance.GetConfigValueByKey("TID_FlowConfigParam");

        /// <summary>
        /// 通过基本参数名称获取其对应参数值
        /// </summary>
        /// <param name="strParamName">参数名</param>
        /// <returns></returns>
        public static String GetFlowConfigParamValue(String strParamName)
        {
            return ParamGetter.GetParamValue(strArchiveName, strParamName); 
        }

    }
}
