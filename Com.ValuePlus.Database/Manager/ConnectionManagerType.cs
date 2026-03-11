using System;

namespace Com.ValuePlus.Database
{
    /// <summary>
    /// 用于区分数据库连接的工厂
    /// </summary>
    internal enum ConnectionManagerType
    {
        /// <summary>
        /// 表示为oracle连接工厂
        /// </summary>
        OracleType = 0, 
        /// <summary>
        /// 表示为sqlserver连接工厂
        /// </summary>
        SqlServerType = 1,
        /// <summary>
        /// 表示为access连接工厂
        /// </summary>
        OleDbType = 2,
        /// <summary>
        /// 表示为MySql连接工厂
        /// </summary>
        MySqlType = 3

    }
}