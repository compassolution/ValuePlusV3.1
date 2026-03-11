using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ValuePlus.Database
{
   
    /// <summary>
    /// 工厂方法类，负责产出具体的连接池对象
    /// 此对业务层的开放，并且所有数据库底层操作只能从此进入
    /// </summary>
    public class DAOFactory
    {      

       /// <summary>
        /// 创建操作数据库Oracle对象
       /// </summary>
       /// <param name="connectionstring">数据库连接</param>
       /// <returns></returns>
        public static IDatabaseDAO CreateOracleDAO(string connectionstring)
        {
            return new DatabaseDAO(connectionstring, ConnectionManagerType.OracleType);
        }       

        /// <summary>
        /// 扩展可以连接SqlServer数据库，能够支持多数据库操作，同时也支持不同类型的数据库
        /// </summary>
        /// <param name="connectionstring">数据库连接</param>
        /// <returns></returns>
        public static IDatabaseDAO CreateSqlServerDAO(string connectionstring)
        {
            return new DatabaseDAO(connectionstring, ConnectionManagerType.SqlServerType);
        }        

        /// <summary>
        /// 扩展可以连接MySql数据库，能够支持多数据库操作，同时也支持不同类型的数据库
        /// </summary>
        /// <param name="connectionstring">数据库连接</param>
        /// <returns></returns>
        public static IDatabaseDAO CreateMySqlDAO(string connectionstring)
        {
            return new DatabaseDAO(connectionstring, ConnectionManagerType.MySqlType);
        }       

        /// <summary>
        /// 扩展可以连接OleDb数据库，能够支持多数据库操作，同时也支持不同类型的数据库
        /// </summary>
        /// <param name="connectionstring">数据库连接</param>
        /// <returns></returns>
        public static IDatabaseDAO CreateOleDbDAO(string connectionstring)
        {
            return new DatabaseDAO(connectionstring, ConnectionManagerType.OleDbType);
        }

        public static IDatabaseDAO CreateOleDbDAO()
        {
            throw new NotImplementedException();
        }
    }
}
