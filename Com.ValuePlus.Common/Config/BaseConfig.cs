using System;
using System.Collections;
using System.Text;
using System.IO;
using Com.ValuePlus.Utils.Cryptography;
using Com.ValuePlus.Utils.Session;

namespace Com.ValuePlus.Common.Config
{
    /// <summary>
    /// 配置文件读取业务类
    /// </summary>
    public sealed class BaseConfig
    {
        /// <summary>
        /// 日志声明
        /// </summary>
        private static Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 应用程序根目录
        /// </summary>
        private string RootPath = System.AppDomain.CurrentDomain.BaseDirectory;

        /// <summary>
        /// 自身对象实例
        /// </summary>
        private static BaseConfig instance = new BaseConfig();

        /// <summary>
        /// 配置缓存
        /// </summary>
        private Com.ValuePlus.Config.IConfigManager ConfigCache = null;

        /// <summary>
        /// 加此私有构造函数，防止此类对象通过new对象实例化
        /// </summary>
        private BaseConfig()
        {
            //邮箱配置文件
            ConfigCache = Com.ValuePlus.Config.ConfigFactory.GetConfigManager();
            ConfigCache.Load(@"bin\BasicDataConfig.config", Com.ValuePlus.Config.FileTypeEnum.XmlType);
        }

        /// <summary>
        /// 获得此对象的实例
        /// </summary>
        public static BaseConfig Instance
        {
            get
            {
                return instance;
            }
        }

        #region 获得数据库连接串
        /// <summary>
        /// 获得默认的数据库连接串
        /// </summary>
        /// <returns></returns>
        public string GetConnectionString()
        {
            String strReturnConnectString = EnDesc3Connstring(ConfigCache.GetProperty("DbConnectString"));

            ////如果是单点登录的，从Session获取数据链接字符串的节点标识，如果存在则获取对应的数据库链接（add by sammen 20170901）
            //String strDBConnectSessionName = SessionHelper.GetSession(CacheName.DBConnectSessionName) as String;
            //if (!String.IsNullOrEmpty(strDBConnectSessionName))
            //{
            //    strDBConnectSessionName = "DbConnectString_" + strDBConnectSessionName;
            //    strReturnConnectString = EnDesc3Connstring(ConfigCache.GetProperty(strDBConnectSessionName));
            //}

            return strReturnConnectString;

        }
        #endregion

        #region 解密登陆数据库信息
        /// <summary>
        /// 解密登陆数据库信息
        /// </summary>
        /// <param name="connstring"></param>
        /// <returns></returns>
        private  string EnDesc3Connstring(string connstring)
        {
            string sConn = Com.ValuePlus.Utils.Cryptography.CryptographyHelper.Decrypt3des(new byte[] {0x11, 0x22, 0x5F, 0x68, (byte)0x88, 0x70, 0x40, 0x38
                , 0x28, 0x25, 0x79, 0x51, (byte)0xCB, (byte)0xDD, 0x55, 0x66
                , 0x77, 0x29, 0x74, (byte)0x98, 0x30, 0x40, 0x36, (byte)0xE2}, new byte[] { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF }, connstring,System.Text.Encoding.UTF8); ;
            return sConn;
        }
        #endregion

        #region 根据配置文件key获取对应值
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

        #region 根据配置文件CommonSqlConifig中的key获取对应值
        /// <summary>
        /// 根据配置文件CommonSqlConifig中的key获取对应值
        /// </summary>
        /// <param name="strKey"></param>
        /// <returns></returns>
        public string GetCommonConfigSqlByKey(string strKey)
        {
            Com.ValuePlus.Config.IConfigManager ConfigCache_CommonSql = Com.ValuePlus.Config.ConfigFactory.GetConfigManager();
            ConfigCache_CommonSql.Load(@"bin\CommonSqlConfig.config", Com.ValuePlus.Config.FileTypeEnum.XmlType);
            return (string)ConfigCache_CommonSql.GetProperty(strKey);
        }
        #endregion

    }
}
