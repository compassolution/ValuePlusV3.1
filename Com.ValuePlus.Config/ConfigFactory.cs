using System;

namespace Com.ValuePlus.Config
{
    /// <summary>
    /// 配置文件工厂
    /// </summary>
    public sealed class ConfigFactory
    {
        /// <summary>
        /// 获得配置管理实例,配置文件管理类,限制只能读properties,txt,xml,config文件.针对properties和txt都采用同一种方式处理.针对xml和config文件采用同一种方式处理
        /// </summary>
        /// <returns></returns>
        public static IConfigManager GetConfigManager()
        {
            return new ConfigManager();
        }       

    }
}
