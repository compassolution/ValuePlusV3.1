using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ValuePlus.Flow.Config
{
    public sealed class FlowConfig
    {

        #region 对类FlowConfig自身的初始化
        /// <summary>
        /// 自身对象实例
        /// </summary>
        private static FlowConfig instance = new FlowConfig();


        /// <summary>
        /// 配置缓存
        /// </summary>
        private Com.ValuePlus.Config.IConfigManager ConfigCache = null;

        /// <summary>
        /// 加此私有构造函数，防止此类对象通过new对象实例化
        /// </summary>
        private FlowConfig()
        {
            ConfigCache = Com.ValuePlus.Config.ConfigFactory.GetConfigManager();
            ConfigCache.Load(@"Flow\Config\FlowConfig.config", Com.ValuePlus.Config.FileTypeEnum.XmlType);
        }

        /// <summary>
        /// 获得此对象的实例
        /// </summary>
        public static FlowConfig Instance
        {
            get
            {
                return instance;
            }
        }
        #endregion

        #region 对单个配置项的值获取
        /// <summary>
        /// 根据配置文件key获取对应值
        /// </summary>
        /// <param name="strKey"></param>
        /// <returns></returns>
        public string GetConfigValueByKey(string strKey)
        {
            return (string)ConfigCache.GetProperty(strKey);
        }
        #endregion

    }
}
