using System;
using System.Collections.Generic;
using System.Text;
using Com.ValuePlus.Common.Config;

namespace Com.ValuePlus.SysParams
{
    /// <summary>
    /// 系统基本参数库的参数值获取类
    /// </summary>
    public class BaseParamsGetter
    {
        private static String strArchiveName = BaseConfig.Instance.GetConfigValueByKey("TID_BasicParam");

        /// <summary>
        /// 通过基本参数名称获取其对应参数值
        /// </summary>
        /// <param name="strParamName">参数名</param>
        /// <returns></returns>
        public static String GetBasicParamValue(String strParamName)
        {
            return ParamGetter.GetParamValue(strArchiveName, strParamName);
        }

    }
}
