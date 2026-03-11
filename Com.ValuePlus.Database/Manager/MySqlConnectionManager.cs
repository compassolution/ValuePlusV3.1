using System;
using MySql.Data.MySqlClient;
using System.Data.Common;

namespace Com.ValuePlus.Database
{
    /// <summary>
    /// 单例模式创建MySql数据库对象工厂
    /// </summary>
    internal sealed class MySqlConnectionManager
    {
        /// <summary>
        /// 日志声明
        /// </summary>
        private static Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 自身对象实例
        /// </summary>
        private static MySqlConnectionManager instance = new MySqlConnectionManager();

        /// <summary>
        /// MySql数据库对象工厂
        /// </summary>
        public  MySql.Data.MySqlClient.MySqlClientFactory MySqlFactoryInstance = null;


        /// <summary>
        /// 私有构造函数
        /// </summary>
        static MySqlConnectionManager()
        {

        }

        /// <summary>
        /// 私有构造函数，防止通过new方法实例化此对象
        /// </summary>
        MySqlConnectionManager()
        {
            log.Debug("构造函数启动：创建MySql数据库连接工厂相当于创建对应的连接数据库连接池");
            this.MySqlFactoryInstance = MySqlClientFactory.Instance;
            log.Debug("构造函数结束：创建MySql数据库连接工厂结束");
        }

        /// <summary>
        /// 获得自身对象的实例
        /// </summary>
        /// <returns></returns>
        public static MySqlConnectionManager Instance
        {
            get
            {
                return instance;
            }
        }

    }
}
