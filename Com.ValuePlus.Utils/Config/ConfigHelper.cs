using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace Com.ValuePlus.Utils.Config
{
    public class ConfigHelper
    {
        /// <summary>
        /// 读配置节
        /// </summary>
        /// <param name="ConfigName"></param>
        /// <returns></returns>
        public static string GetConfigValue(string ConfigName)
        {

            return System.Configuration.ConfigurationManager.AppSettings[ConfigName];
        }
    }
}
