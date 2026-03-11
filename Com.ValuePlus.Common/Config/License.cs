using System;
using System.Collections;
using System.Text;
using System.IO;
using Com.ValuePlus.Utils.Cryptography;

namespace Com.ValuePlus.Common.Config
{
    /// <summary>
    /// License文件读取业务类
    /// </summary>
    public sealed class License
    {
        /// <summary>
        /// 应用程序根目录
        /// </summary>
        private string RootPath = System.AppDomain.CurrentDomain.BaseDirectory;

        /// <summary>
        /// 自身对象实例
        /// </summary>
        private static License instance = new License();

        /// <summary>
        /// 配置缓存
        /// </summary>
        private Com.ValuePlus.Config.IConfigManager ConfigCache = null;

        /// <summary>
        /// 加此私有构造函数，防止此类对象通过new对象实例化
        /// </summary>
        public License()
        {

        }

        /// <summary>
        /// 获得此对象的实例
        /// </summary>
        public static License Instance
        {
            get
            {
                return instance;
            }
        }

        #region 获得License字符串
        /// <summary>
        /// 获得License字符串
        /// </summary>
        /// <returns></returns>
        public string GetLicenseString()
        {
            String strConfigValue = "";

            String FilePath = @"SysFile\License.lic";
            String strConfigFilePath = System.IO.Path.Combine(RootPath, FilePath);

            //邮箱配置文件
            ConfigCache = Com.ValuePlus.Config.ConfigFactory.GetConfigManager();
            //文件路径
            if (!File.Exists(FilePath) && !File.Exists(strConfigFilePath))
            {
                ConfigCache = null;
            }
            else
            {
                ConfigCache.Load(FilePath, Com.ValuePlus.Config.FileTypeEnum.TxtType);
            }

            if (ConfigCache!=null)
            {
                strConfigValue = ConfigCache.GetProperty("License");
            }
            return strConfigValue;

        }
        #endregion
    }
}
