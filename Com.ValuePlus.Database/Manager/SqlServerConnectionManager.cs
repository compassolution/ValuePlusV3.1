using System;
using System.Data.OracleClient;
using System.Data.SqlClient;
using System.Data.Common;


namespace Com.ValuePlus.Database
{
    /// <summary>
    /// 单例模式创建SqlServer数据库对象工厂
    /// </summary>
    internal sealed class SqlServerConnectionManager
    {
        /// <summary>
        /// 日志声明
        /// </summary>
        private static Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 自身对象实例
        /// </summary>
        private static SqlServerConnectionManager instance = new SqlServerConnectionManager();

        /// <summary>
        /// SqlServer数据库对象工厂
        /// </summary>
        public  System.Data.SqlClient.SqlClientFactory SqlFactoryInstance = null;


        /// <summary>
        /// 私有构造函数
        /// </summary>
        static SqlServerConnectionManager()
        {

        }

        /// <summary>
        /// 私有构造函数，防止通过new方法实例化此对象
        /// </summary>
        SqlServerConnectionManager()
        {
            log.Debug("构造函数启动：创建SqlServer数据库连接工厂相当于创建对应的连接数据库连接池");
            this.SqlFactoryInstance = SqlClientFactory.Instance;
            log.Debug("构造函数结束：创建SqlServer数据库连接工厂结束");
        }

        /// <summary>
        /// 获得自身对象的实例
        /// </summary>
        /// <returns></returns>
        public static SqlServerConnectionManager Instance
        {
            get
            {
                return instance;
            }
        }

    }
}
