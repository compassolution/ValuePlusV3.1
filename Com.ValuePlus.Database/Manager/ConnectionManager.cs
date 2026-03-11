using System.Data.OracleClient;
using System.Data.Common;

namespace Com.ValuePlus.Database
{
    /// <summary>
    /// 单例模式创建数据库对象工厂
    /// </summary>
    internal  sealed class ConnectionManager
    {
        /// <summary>
        /// 日志声明
        /// </summary>
        private static Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 自身对象实例
        /// </summary>
        private static ConnectionManager instance = new ConnectionManager();      
       

        /// <summary>
        /// 私有构造函数
        /// </summary>
        static  ConnectionManager()
        {
                      
        }

        /// <summary>
        /// 私有构造函数，防止通过new方法实例化此对象
        /// </summary>
        ConnectionManager()
        {
           
        }

        /// <summary>
        /// 获得自身对象的实例
        /// </summary>
        /// <returns></returns>
        public static ConnectionManager Instance
        {
            get
            {
                return instance;
            }
        }

        /// <summary>
        /// 获得对应的数据库连接工厂实例
        /// </summary>
        /// <returns></returns>
        public DbProviderFactory FactoryInstance(ConnectionManagerType type)
        {
            switch (type)
            {
                case ConnectionManagerType.OracleType :
                    return OracleConnectionManager.Instance.OracleFactoryInstance;
                    break;
                case ConnectionManagerType.SqlServerType:
                    return SqlServerConnectionManager.Instance.SqlFactoryInstance;
                    break;
                case ConnectionManagerType.MySqlType:
                    return MySqlConnectionManager.Instance.MySqlFactoryInstance;
                    break;
                default:
                    return OleDbConnectionManager.Instance.OleDbFactoryInstance;
                    break;                
            }
        }
    }
}
