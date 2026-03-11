using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ValuePlus.Archive.Config
{
    public sealed class ArchiveStyleConfig
    {
        /// <summary>
        /// 自身对象实例
        /// </summary>
        private static ArchiveStyleConfig instance = new ArchiveStyleConfig();


        /// <summary>
        /// 配置缓存
        /// </summary>
        private Com.ValuePlus.Config.IConfigManager ConfigCache = null;

        /// <summary>
        /// 加此私有构造函数，防止此类对象通过new对象实例化
        /// </summary>
        private ArchiveStyleConfig()
        {
            //邮箱配置文件
            ConfigCache = Com.ValuePlus.Config.ConfigFactory.GetConfigManager();
            ConfigCache.Load(@"Archive\Config\ArchiveStyleConfig.config", Com.ValuePlus.Config.FileTypeEnum.XmlType);
        }

        /// <summary>
        /// 获得此对象的实例
        /// </summary>
        public static ArchiveStyleConfig Instance
        {
            get
            {
                return instance;
            }
        }
        /// <summary>
        /// 根据配置文件key获取对应值
        /// </summary>
        /// <param name="strKey"></param>
        /// <returns></returns>
        public string GetConfigValueByKey(string strKey)
        {
            return (string)ConfigCache.GetProperty(strKey);
        }
    }
}
