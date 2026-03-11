using System;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using System.Collections;
using Com.ValuePlus.Common.Config;

namespace Com.ValuePlus.Database
{
    /// <summary>
    /// 数据库底层操作
    /// 此对象只能通过DAOFactory工厂创建实例，不允许采用其它方式创建实例
    /// </summary>
    internal sealed class DatabaseDAO : IDatabaseDAO
    {
        #region 私有变量
        /// <summary>
        /// 数据库连接串
        /// </summary>
        private string sConnectionString = null;

        /// <summary>
        /// 数据库连接对象
        /// </summary>
        private DbConnection _connection = null;

        /// <summary>
        /// 事务对象
        /// </summary>
        private DbTransaction _transaction = null;        

        /// <summary>
        /// 数据库连接工厂类型
        /// </summary>
        private ConnectionManagerType _type;

        /// <summary>
        /// 存储过程执行报错前等待时间
        /// </summary>
        private int iCommandTimeoutForSP = 30;

        #endregion      

        #region 私有构造函数，防止通过此来实例化
        /// <summary>
        /// 私有构造函数，防止通过此来实例化
        /// </summary>
        private DatabaseDAO()
        {

        }
        #endregion

        #region 公共构造函数
        /// <summary>
        /// 构造函数，获得数据库连接对象
        /// </summary>
        /// <param name="conn"></param>
        public DatabaseDAO(String strConn,ConnectionManagerType type)
        {
            sConnectionString = strConn;
            _type = type;
            if (!String.IsNullOrEmpty(BaseConfig.Instance.GetConfigValueByKey("iCommandTimeoutForSP")))
            {
                this.iCommandTimeoutForSP = int.Parse(BaseConfig.Instance.GetConfigValueByKey("iCommandTimeoutForSP"));
            }
        }
        #endregion

        #region 私有属性，获得连接工厂
        /// <summary>
        /// 获得连接工厂
        /// </summary>
        /// <returns></returns>
        private DbProviderFactory Factory
        {
            get
            {
                return ConnectionManager.Instance.FactoryInstance(_type);
            }
        }
        #endregion

        #region 实现Idispose实现资源释放
        /// <summary>
        /// 实现Dispose
        /// </summary>
        public void Dispose(){
            if (_connection != null)
            {                
                if (_connection.State == ConnectionState.Open)
                {
                    _connection.Close();
                }
                _connection.Dispose();
            }
            if (_transaction != null)
            {
                _transaction.Dispose();
            }           
           
       } 
        #endregion

        #region 事务处理
        /// <summary>
        /// 开始事务
        /// </summary>
        public void BeginTransaction()
        {
            //连接对象为空创建对象
            if (_connection == null)
            {
                _connection = Factory.CreateConnection();                
            }
            //是否为开放状态
            if (_connection.State != ConnectionState.Open)
            {
                _connection.ConnectionString = sConnectionString;
                _connection.Open();
            }  
            //开始事务
            if (_transaction == null)
            {
                _transaction = _connection.BeginTransaction();
            }
        }

        /// <summary>
        /// 确认事务
        /// </summary>
        public void Commit()
        {
            if (_connection != null)
            {
                if (_transaction != null)
                {
                    //确认事务
                    _transaction.Commit();
                }
                if (_connection.State == ConnectionState.Open)
                {
                    _connection.Close();
                }
                if (_transaction != null)
                {
                    //确认事务
                    _transaction.Dispose();
                }
                _connection.Dispose();
            }
        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        public void RoolBack()
        {
            if (_connection != null)
            {
                if (_transaction != null)
                {
                    //回滚事务
                    _transaction.Rollback();
                }
                if (_connection.State == ConnectionState.Open)
                {
                    _connection.Close();
                }
                _connection.Dispose();
            }
            if (_transaction != null)
            {
                _transaction.Dispose();
            }
        }
        #endregion

        #region 公共处理方法
        /// <summary>
        /// 执行更新操作
        /// </summary>
        /// <param name="commandType">命令类型(存储过程,命令文本或其它.)</param>
        /// <param name="Sql">sql语句或存储过程名称</param>
        /// <param name="commandParameters">绑定变量</param>
        /// <returns>操作的记录数</returns>
        public int ExecuteNonQuery(CommandType commandType,String Sql, params DbParameter[] commandParameters)
        {
            //判断_connection是否为空
            if (_transaction != null)
            {
                //事务处理
                using (DbCommand cmd =  Factory.CreateCommand())
                {
                    //设置超时
                    cmd.CommandTimeout = this.iCommandTimeoutForSP;
                    PrepareCommand(cmd, _transaction.Connection, _transaction, commandType, Sql, commandParameters);
                    // Finally, execute the command
                    int retval = cmd.ExecuteNonQuery();
                    // 清除参数,以便再次使用.
                    cmd.Parameters.Clear();
                    return retval;
                } 
            }
            else
            {   
                //非事务处理
                //开始创建Connection
                using (DbConnection connection =  Factory.CreateConnection())
                {
                    connection.ConnectionString = sConnectionString;
                    using (DbCommand cmd =   Factory.CreateCommand())
                    {
                        //设置超时
                        cmd.CommandTimeout = this.iCommandTimeoutForSP;
                        PrepareCommand(cmd, connection, null, commandType, Sql, commandParameters);
                        // Finally, execute the command
                        int retval = cmd.ExecuteNonQuery();
                        // 清除参数,以便再次使用.
                        cmd.Parameters.Clear();
                        return retval;                        
                    }
                }
            }
            
        }        
       

        /// <summary>
        /// 返回结果集第一行的第一列
        /// </summary>
        /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param>
        /// <param name="Sql">sql语句或存储过程名称</param>
        /// <param name="commandParameters">绑定变量</param>
        /// <returns></returns>
        public object ExecuteScalar(CommandType commandType, String Sql, params DbParameter[] commandParameters)
        {
             //判断_connection是否为空
            if (_transaction != null)
            {
                //事务处理
                using (DbCommand cmd = Factory.CreateCommand())
                {
                    //设置超时
                    cmd.CommandTimeout = this.iCommandTimeoutForSP;
                    PrepareCommand(cmd, _transaction.Connection, _transaction, commandType, Sql, commandParameters);
                    // 执行DbCommand命令,并返回结果.
                    object retval = cmd.ExecuteScalar();
                    // 清除参数,以便再次使用.
                    cmd.Parameters.Clear();
                    return retval;
                }
            }
            else
            {
                //开始创建eConnection
                using (DbConnection connection = Factory.CreateConnection())
                {
                    connection.ConnectionString = sConnectionString;
                    //创建Command
                    using (DbCommand cmd = Factory.CreateCommand())
                    {
                        //设置超时
                        cmd.CommandTimeout = this.iCommandTimeoutForSP;
                        PrepareCommand(cmd, connection, null, commandType, Sql, commandParameters);
                        // 执行DbCommand命令,并返回结果.
                        object retval = cmd.ExecuteScalar();
                        // 清除参数,以便再次使用.
                        cmd.Parameters.Clear();
                        return retval;
                    }
                }
            }
        }
      

        /// <summary>
        /// 返回顺序查询记录集
        /// </summary>
        /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param>
        /// <param name="Sql">sql语句或存储过程名称</param>
        /// <param name="commandParameters">绑定变量</param>
        /// <returns></returns>
        public IDataReader ExecuteReader(CommandType commandType, String Sql, params DbParameter[] commandParameters)
        {
             //判断_connection是否为空
            if (_transaction != null)
            {
                //事务处理
                using (DbCommand cmd = Factory.CreateCommand())
                {
                    //设置超时
                    cmd.CommandTimeout = this.iCommandTimeoutForSP;
                    PrepareCommand(cmd, _transaction.Connection, _transaction, commandType, Sql, commandParameters);
                    // 创建数据阅读器
                    IDataReader dataReader;
                    dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    cmd.Parameters.Clear();
                    return dataReader;
                }
            }
            else
            {
                //开始创建Connection
                DbConnection connection = Factory.CreateConnection();
                connection.ConnectionString = sConnectionString;
                //创建Command
                DbCommand cmd = Factory.CreateCommand();
                try
                {
                    //设置超时
                    cmd.CommandTimeout = this.iCommandTimeoutForSP;
                    PrepareCommand(cmd, connection, null, commandType, Sql, commandParameters);
                    // 创建数据阅读器
                    IDataReader dataReader;
                    dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    cmd.Parameters.Clear();
                    return dataReader;
                }
                catch (Exception ex)
                {
                    //异常就关闭连接对象并关闭对象
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                    connection.Dispose();
                    throw new Exception(ex.ToString(), ex);
                }
            }
           
        }

        /// <summary>
        /// 返回一个包含结果集的DataSet的操作
        /// </summary>
        /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param>
        /// <param name="Sql">sql语句或存储过程名称</param>
        /// <param name="commandParameters">绑定变量</param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(CommandType commandType, String Sql, params DbParameter[] commandParameters)
        {
             //判断_connection是否为空
            if (_transaction != null)
            {
                //事务处理
                using (DbCommand cmd = Factory.CreateCommand())
                {
                    //设置超时
                    cmd.CommandTimeout = this.iCommandTimeoutForSP;
                    PrepareCommand(cmd, _transaction.Connection, _transaction, commandType, Sql, commandParameters);
                    // 创建DbDataAdapter和DataSet.
                    using (DbDataAdapter da = Factory.CreateDataAdapter())
                    {
                        da.SelectCommand = cmd;
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        cmd.Parameters.Clear();
                        return ds;
                    }
                }
            }
            else
            {
                //开始创建Connection
                using (DbConnection connection = Factory.CreateConnection())
                {
                    connection.ConnectionString = sConnectionString;
                    //创建Command
                    using (DbCommand cmd = Factory.CreateCommand())
                    {
                        //设置超时
                        cmd.CommandTimeout = this.iCommandTimeoutForSP;
                        PrepareCommand(cmd, connection, null, commandType, Sql, commandParameters);
                        // 创建DbDataAdapter和DataSet.
                        using (DbDataAdapter da = Factory.CreateDataAdapter())
                        {
                            da.SelectCommand = cmd;
                            DataSet ds = new DataSet();
                            da.Fill(ds);
                            cmd.Parameters.Clear();
                            return ds;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 分页处理
        /// </summary>
        /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param>
        /// <param name="Sql">sql语句或存储过程名称</param>
        /// <param name="startRecord">开始的记录索引若startRecord为-1表示返回所有记录</param>
        /// <param name="maxRecords">检索的最大记录数</param>
        /// <param name="commandParameters">绑定变量</param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(CommandType commandType, String Sql, int startRecord, int maxRecords, params DbParameter[] commandParameters)            
        {
            //开始创建Connection
            using (DbConnection connection =  Factory.CreateConnection())
            {
                connection.ConnectionString = sConnectionString;
                //创建Command
                using (DbCommand cmd =  Factory.CreateCommand())
                {
                    //设置超时
                    cmd.CommandTimeout = this.iCommandTimeoutForSP;
                    PrepareCommand(cmd, connection, null, commandType, Sql, commandParameters);
                    DataSet ds = new DataSet();
                    using (DbDataAdapter oradp =   Factory.CreateDataAdapter())
                    {
                        oradp.SelectCommand = cmd;
                        if (startRecord >= 0)
                        {
                            oradp.Fill(ds, startRecord, maxRecords, "ds");
                        }
                        else
                        {
                            oradp.Fill(ds);
                        }
                    }
                    return ds;
                }
            }
        }
        #endregion   

        #region 私有方法
        /// <summary>
        /// 预处理用户提供的命令,数据库连接/事务/命令类型/参数
        /// </summary>
        /// <param name="command">要处理的DbCommand</param>
        /// <param name="connection">数据库连接</param>
        /// <param name="transaction">一个有效的事务或者是null值</param>
        /// <param name="commandType">命令类型 (存储过程,命令文本, 其它.)</param>
        /// <param name="commandText">存储过程名或都SQL命令文本</param>
        /// <param name="commandParameters">和命令相关联的DbParameter参数数组,如果没有参数为'null'</param>
        /// <param name="mustCloseConnection"><c>true</c> 如果连接是打开的,则为true,其它情况下为false.</param>
        private  void PrepareCommand(DbCommand command, DbConnection  connection, DbTransaction transaction, CommandType commandType, string commandText, DbParameter[] commandParameters)
        {
            if (connection.State != ConnectionState.Open)
            {                
                connection.Open();
            }            
            // 给命令分配一个数据库连接.
            command.Connection = connection;
            // 设置命令文本(存储过程名或SQL语句)
            command.CommandText = commandText;
            // 分配事务
            if (transaction != null)
            {
                command.Transaction = transaction;
            }
            // 设置命令类型.
            command.CommandType = commandType;

            // 分配命令参数
            if (commandParameters != null)
            {
                AttachParameters(command, commandParameters);
            }
            return;
        }

        /// <summary>
        /// 将DbParameter参数数组(参数值)分配给DbCommand命令.
        /// 这个方法将给任何一个参数分配DBNull.Value;
        /// 该操作将阻止默认值的使用.
        /// </summary>
        /// <param name="command">命令名</param>
        /// <param name="commandParameters">OracleParameter数组</param>
        private  void AttachParameters(DbCommand command, params DbParameter[] commandParameters)
        {
            if (commandParameters != null)
            {
                foreach (DbParameter  p in commandParameters)
                {
                    if (p != null)
                    {
                        // 检查未分配值的输出参数,将其分配以DBNull.Value.
                        if ((p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Input) &&  (p.Value == null))
                        {
                            p.Value = DBNull.Value;
                        }
                        command.Parameters.Add(p);
                    }
                }
            }
        }
       
        #endregion

        #region 公共处理方法
        /// <summary>
        /// 执行更新操作
        /// </summary>
        /// <param name="objSqlBasicMetaData">包含了sql语句及语句类型</param>
        /// <param name="commandParameters">绑定变量</param>
        /// <returns>操作的记录数</returns>
        public int ExecuteNonQuery(Com.ValuePlus.Utils.SqlBasicMetaData objSqlBasicMetaData, params DbParameter[] commandParameters)
        {
            //判断_connection是否为空
            if (_transaction != null)
            {
                //事务处理
                using (DbCommand cmd = Factory.CreateCommand())
                {
                    //设置超时
                    cmd.CommandTimeout = this.iCommandTimeoutForSP;
                    PrepareCommand(cmd, _transaction.Connection, _transaction, GetSqlCommandType(objSqlBasicMetaData.CommandType), objSqlBasicMetaData.CommandSql, commandParameters);
                    // Finally, execute the command
                    int retval = cmd.ExecuteNonQuery();
                    // 清除参数,以便再次使用.
                    cmd.Parameters.Clear();
                    return retval;
                }
            }
            else
            {
                //非事务处理
                //开始创建Connection
                using (DbConnection connection = Factory.CreateConnection())
                {
                    connection.ConnectionString = sConnectionString;
                    using (DbCommand cmd = Factory.CreateCommand())
                    {
                        //设置超时
                        cmd.CommandTimeout = this.iCommandTimeoutForSP;
                        PrepareCommand(cmd, connection, null, GetSqlCommandType(objSqlBasicMetaData.CommandType), objSqlBasicMetaData.CommandSql, commandParameters);
                        // Finally, execute the command
                        int retval = cmd.ExecuteNonQuery();
                        // 清除参数,以便再次使用.
                        cmd.Parameters.Clear();
                        return retval;
                    }
                }
            }

        }


        /// <summary>
        /// 返回结果集第一行的第一列
        /// </summary>
        /// <param name="objSqlBasicMetaData">包含了sql语句及语句类型</param>
        /// <param name="commandParameters">绑定变量</param>
        /// <returns></returns>
        public object ExecuteScalar(Com.ValuePlus.Utils.SqlBasicMetaData objSqlBasicMetaData, params DbParameter[] commandParameters)
        {
            //判断_connection是否为空
            if (_transaction != null)
            {
                //事务处理
                using (DbCommand cmd = Factory.CreateCommand())
                {
                    //设置超时
                    cmd.CommandTimeout = this.iCommandTimeoutForSP;
                    PrepareCommand(cmd, _transaction.Connection, _transaction, GetSqlCommandType(objSqlBasicMetaData.CommandType), objSqlBasicMetaData.CommandSql, commandParameters);
                    // 执行DbCommand命令,并返回结果.
                    object retval = cmd.ExecuteScalar();
                    // 清除参数,以便再次使用.
                    cmd.Parameters.Clear();
                    return retval;
                }
            }
            else
            {
                //开始创建eConnection
                using (DbConnection connection = Factory.CreateConnection())
                {
                    connection.ConnectionString = sConnectionString;
                    //创建Command
                    using (DbCommand cmd = Factory.CreateCommand())
                    {
                        //设置超时
                        cmd.CommandTimeout = this.iCommandTimeoutForSP;
                        PrepareCommand(cmd, connection, null, GetSqlCommandType(objSqlBasicMetaData.CommandType), objSqlBasicMetaData.CommandSql, commandParameters);
                        // 执行DbCommand命令,并返回结果.
                        object retval = cmd.ExecuteScalar();
                        // 清除参数,以便再次使用.
                        cmd.Parameters.Clear();
                        return retval;
                    }
                }
            }
        }


        /// <summary>
        /// 返回顺序查询记录集
        /// </summary>
        /// <param name="objSqlBasicMetaData">包含了sql语句及语句类型</param>
        /// <param name="commandParameters">绑定变量</param>
        /// <returns></returns>
        public IDataReader ExecuteReader(Com.ValuePlus.Utils.SqlBasicMetaData objSqlBasicMetaData, params DbParameter[] commandParameters)
        {
            //判断_connection是否为空
            if (_transaction != null)
            {
                //事务处理
                using (DbCommand cmd = Factory.CreateCommand())
                {
                    //设置超时
                    cmd.CommandTimeout = this.iCommandTimeoutForSP;
                    PrepareCommand(cmd, _transaction.Connection, _transaction, GetSqlCommandType(objSqlBasicMetaData.CommandType), objSqlBasicMetaData.CommandSql, commandParameters);
                    // 创建数据阅读器
                    IDataReader dataReader;
                    dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    cmd.Parameters.Clear();
                    return dataReader;
                }
            }
            else
            {
                //开始创建Connection
                DbConnection connection = Factory.CreateConnection();
                connection.ConnectionString = sConnectionString;
                //创建Command
                DbCommand cmd = Factory.CreateCommand();
                try
                {
                    //设置超时
                    cmd.CommandTimeout = this.iCommandTimeoutForSP;
                    PrepareCommand(cmd, connection, null, GetSqlCommandType(objSqlBasicMetaData.CommandType), objSqlBasicMetaData.CommandSql, commandParameters);
                    // 创建数据阅读器
                    IDataReader dataReader;
                    dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    cmd.Parameters.Clear();
                    return dataReader;
                }
                catch (Exception ex)
                {
                    //异常就关闭连接对象并关闭对象
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                    connection.Dispose();
                    throw new Exception(ex.ToString(), ex);
                }
            }

        }

        /// <summary>
        /// 返回一个包含结果集的DataSet的操作
        /// </summary>
        /// <param name="objSqlBasicMetaData">包含了sql语句及语句类型</param>
        /// <param name="commandParameters">绑定变量</param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(Com.ValuePlus.Utils.SqlBasicMetaData objSqlBasicMetaData, params DbParameter[] commandParameters)
        {
            //判断_connection是否为空
            if (_transaction != null)
            {
                //事务处理
                using (DbCommand cmd = Factory.CreateCommand())
                {
                    //设置超时
                    cmd.CommandTimeout = this.iCommandTimeoutForSP;
                    PrepareCommand(cmd, _transaction.Connection, _transaction, GetSqlCommandType(objSqlBasicMetaData.CommandType), objSqlBasicMetaData.CommandSql, commandParameters);
                    // 创建DbDataAdapter和DataSet.
                    using (DbDataAdapter da = Factory.CreateDataAdapter())
                    {
                        da.SelectCommand = cmd;
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        cmd.Parameters.Clear();
                        return ds;
                    }
                }
            }
            else
            {
                //开始创建Connection
                using (DbConnection connection = Factory.CreateConnection())
                {
                    connection.ConnectionString = sConnectionString;
                    //创建Command
                    using (DbCommand cmd = Factory.CreateCommand())
                    {
                        //设置超时
                        cmd.CommandTimeout = this.iCommandTimeoutForSP;
                        PrepareCommand(cmd, connection, null, GetSqlCommandType(objSqlBasicMetaData.CommandType), objSqlBasicMetaData.CommandSql, commandParameters);
                        // 创建DbDataAdapter和DataSet.
                        using (DbDataAdapter da = Factory.CreateDataAdapter())
                        {
                            da.SelectCommand = cmd;
                            DataSet ds = new DataSet();
                            da.Fill(ds);
                            cmd.Parameters.Clear();
                            return ds;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 分页处理
        /// </summary>
        /// <param name="objSqlBasicMetaData">包含了sql语句及语句类型</param>
        /// <param name="startRecord">开始的记录索引若startRecord为-1表示返回所有记录</param>
        /// <param name="maxRecords">检索的最大记录数</param>
        /// <param name="commandParameters">绑定变量</param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(Com.ValuePlus.Utils.SqlBasicMetaData objSqlBasicMetaData, int startRecord, int maxRecords, params DbParameter[] commandParameters)
        {
            //开始创建Connection
            using (DbConnection connection = Factory.CreateConnection())
            {
                connection.ConnectionString = sConnectionString;
                //创建Command
                using (DbCommand cmd = Factory.CreateCommand())
                {
                    //设置超时
                    cmd.CommandTimeout = this.iCommandTimeoutForSP;
                    PrepareCommand(cmd, connection, null, GetSqlCommandType(objSqlBasicMetaData.CommandType), objSqlBasicMetaData.CommandSql, commandParameters);
                    DataSet ds = new DataSet();
                    using (DbDataAdapter oradp = Factory.CreateDataAdapter())
                    {
                        oradp.SelectCommand = cmd;
                        if (startRecord >= 0)
                        {
                            oradp.Fill(ds, startRecord, maxRecords, "ds");
                        }
                        else
                        {
                            oradp.Fill(ds);
                        }
                    }
                    return ds;
                }
            }
        }
        
        #endregion

        #region 存储过程相关操作
        /// <summary>
        /// 执行存储过程操作
        /// </summary>
        /// <param name="spName">包含了sql语句及语句类型</param>
        /// <param name="paras">绑定变量</param>
        /// <returns>操作的记录数</returns>
        public string ExecuteProcedure(string spName,  ArrayList paras)
        {
            //判断_connection是否为空
            if (_transaction != null)
            {
                //事务处理
                DbCommand cmd = Factory.CreateCommand();
                if (_connection.State != ConnectionState.Open)
                {
                    _connection.Open();
                }
                // 给命令分配一个数据库连接.
                cmd.Connection = _connection;
                // 设置命令文本(存储过程名或SQL语句)
                cmd.CommandText = spName;
                // 分配事务
                if (_transaction != null)
                {
                    cmd.Transaction = _transaction;
                }
                switch (_type)
                {
                    case ConnectionManagerType.OracleType:
                        new OracleParameterManager().ExecuteProcedure(ref cmd, paras);
                        break;
                    case ConnectionManagerType.SqlServerType:
                        new SqlServerParameterManager().ExecuteProcedure(ref cmd, paras);
                        break;
                    case ConnectionManagerType.MySqlType:
                        new MySqlParameterManager().ExecuteProcedure(ref cmd, paras);
                        break;
                    default:
                        new OracleParameterManager().ExecuteProcedure(ref cmd, paras);
                        break;
                }         

                cmd.ExecuteNonQuery();
                return cmd.Parameters[0].Value.ToString();
                
            }
            else
            {
                //非事务处理
                //开始创建Connection
                using (DbConnection connection = Factory.CreateConnection())
                {
                    connection.ConnectionString = sConnectionString;
                    DbCommand cmd = Factory.CreateCommand();
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    // 给命令分配一个数据库连接.
                    cmd.Connection = connection;
                    // 设置命令文本(存储过程名或SQL语句)
                    cmd.CommandText = spName;
                    switch (_type)
                    {
                        case ConnectionManagerType.OracleType:
                            new OracleParameterManager().ExecuteProcedure(ref cmd, paras);
                            break;
                        case ConnectionManagerType.SqlServerType:
                            new SqlServerParameterManager().ExecuteProcedure(ref cmd, paras);
                            break;
                        case ConnectionManagerType.MySqlType:
                            new MySqlParameterManager().ExecuteProcedure(ref cmd, paras);
                            break;
                        default:
                            new OracleParameterManager().ExecuteProcedure(ref cmd, paras);
                            break;
                    }                   
                    cmd.ExecuteNonQuery();
                    return cmd.Parameters[0].Value.ToString();
                }
            }

        }

        /// <summary>
        /// 执行存储过程操作
        /// </summary>
        /// <param name="spName">包含了sql语句及语句类型</param>
        /// <param name="paras">绑定变量</param>
        /// <returns>操作的记录数</returns>
        public string ExecuteProcedure(string spName, Hashtable hsParas)
        {
            //判断_connection是否为空
            if (_transaction != null)
            {
                //事务处理
                DbCommand cmd = Factory.CreateCommand();
                if (_connection.State != ConnectionState.Open)
                {
                    _connection.Open();
                }
                // 给命令分配一个数据库连接.
                cmd.Connection = _connection;
                // 设置命令文本(存储过程名或SQL语句)
                cmd.CommandText = spName;
                //设置超时
                cmd.CommandTimeout = this.iCommandTimeoutForSP;
                // 分配事务
                if (_transaction != null)
                {
                    cmd.Transaction = _transaction;
                }
                switch (_type)
                {
                    case ConnectionManagerType.OracleType:
                        new OracleParameterManager().ExecuteProcedure(ref cmd, hsParas);
                        break;
                    case ConnectionManagerType.SqlServerType:
                        new SqlServerParameterManager().ExecuteProcedure(ref cmd, hsParas);
                        break;
                    case ConnectionManagerType.MySqlType:
                        new MySqlParameterManager().ExecuteProcedure(ref cmd, hsParas);
                        break;
                    default:
                        new OracleParameterManager().ExecuteProcedure(ref cmd, hsParas);
                        break;
                }

                cmd.ExecuteNonQuery();
                return cmd.Parameters[0].Value.ToString();

            }
            else
            {
                //非事务处理
                //开始创建Connection
                using (DbConnection connection = Factory.CreateConnection())
                {
                    connection.ConnectionString = sConnectionString;
                    DbCommand cmd = Factory.CreateCommand();
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    // 给命令分配一个数据库连接.
                    cmd.Connection = connection;
                    // 设置命令文本(存储过程名或SQL语句)
                    cmd.CommandText = spName;
                    //设置超时
                    cmd.CommandTimeout = this.iCommandTimeoutForSP;
                    switch (_type)
                    {
                        case ConnectionManagerType.OracleType:
                            new OracleParameterManager().ExecuteProcedure(ref cmd, hsParas);
                            break;
                        case ConnectionManagerType.SqlServerType:
                            new SqlServerParameterManager().ExecuteProcedure(ref cmd, hsParas);
                            break;
                        case ConnectionManagerType.MySqlType:
                            new MySqlParameterManager().ExecuteProcedure(ref cmd, hsParas);
                            break;
                        default:
                            new OracleParameterManager().ExecuteProcedure(ref cmd, hsParas);
                            break;
                    }
                    cmd.ExecuteNonQuery();
                    return cmd.Parameters[0].Value.ToString();
                }
            }

        }


        /// <summary>
        /// 执行更新操作
        /// </summary>
        /// <param name="objSqlBasicMetaData">包含了sql语句及语句类型</param>
        /// <param name="commandParameters">绑定变量</param>
        /// <returns>操作的记录数</returns>
        public ArrayList ExecuteProcedureArrayList(string sql, ArrayList paras)
        {
            //判断_connection是否为空
            if (_transaction != null)
            {
                //事务处理
                DbCommand cmd = Factory.CreateCommand();
                if (_connection.State != ConnectionState.Open)
                {
                    _connection.Open();
                }
                // 给命令分配一个数据库连接.
                cmd.Connection = _connection;
                // 设置命令文本(存储过程名或SQL语句)
                cmd.CommandText = sql;
                // 分配事务
                if (_transaction != null)
                {
                    cmd.Transaction = _transaction;
                }
                switch (_type)
                {
                    case ConnectionManagerType.OracleType:
                        new OracleParameterManager().ExecuteProcedureInOut(ref cmd, paras);
                        break;
                    case ConnectionManagerType.SqlServerType:
                        new SqlServerParameterManager().ExecuteProcedureInOut(ref cmd, paras);
                        break;
                    case ConnectionManagerType.MySqlType:
                        new MySqlParameterManager().ExecuteProcedureInOut(ref cmd, paras);
                        break;
                    default:
                        new OracleParameterManager().ExecuteProcedureInOut(ref cmd, paras);
                        break;
                }

                cmd.ExecuteNonQuery();

                for (int num = 1; num < cmd.Parameters.Count; num++)
                {
                    if ((cmd.Parameters[num].Direction == ParameterDirection.Output) | (cmd.Parameters[num].Direction == ParameterDirection.InputOutput))
                    {
                        paras[num - 1] = cmd.Parameters[num].Value;
                    }
                }
                return paras;


            }
            else
            {
                //非事务处理
                //开始创建Connection
                using (DbConnection connection = Factory.CreateConnection())
                {
                    connection.ConnectionString = sConnectionString;
                    DbCommand cmd = Factory.CreateCommand();
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    // 给命令分配一个数据库连接.
                    cmd.Connection = connection;
                    // 设置命令文本(存储过程名或SQL语句)
                    cmd.CommandText = sql;
                    switch (_type)
                    {
                        case ConnectionManagerType.OracleType:
                            new OracleParameterManager().ExecuteProcedureInOut(ref cmd, paras);
                            break;
                        case ConnectionManagerType.SqlServerType:
                            new SqlServerParameterManager().ExecuteProcedureInOut(ref cmd, paras);
                            break;
                        case ConnectionManagerType.MySqlType:
                            new MySqlParameterManager().ExecuteProcedureInOut(ref cmd, paras);
                            break;
                        default:
                            new OracleParameterManager().ExecuteProcedureInOut(ref cmd, paras);
                            break;
                    }
                    cmd.ExecuteNonQuery();

                    for (int num = 1; num < cmd.Parameters.Count; num++)
                    {
                        if ((cmd.Parameters[num].Direction == ParameterDirection.Output) | (cmd.Parameters[num].Direction == ParameterDirection.InputOutput))
                        {
                            paras[num - 1] = cmd.Parameters[num].Value;
                        }
                    }
                    return paras;
                }
            }

        }

        #endregion

        #region 语句类型
        /// <summary>
        /// 语句类型
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private CommandType GetSqlCommandType(string str){
            if (str.ToUpper() != "TEXT")
            {
                return CommandType.StoredProcedure;
            }
            return CommandType.Text;
        }
        #endregion


        #region 创建参数
        /// <summary>
        /// 创建参数
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public DbParameter[] MakeParameter(Com.ValuePlus.Utils.SqlBasicMetaData objSqlBasicMetaData)
        {
            switch (_type)
            {
                case ConnectionManagerType.OracleType:
                    return new OracleParameterManager().MakeSqlServerParameter(objSqlBasicMetaData);
                    break;
                case ConnectionManagerType.SqlServerType:
                    return new SqlServerParameterManager().MakeSqlServerParameter(objSqlBasicMetaData);
                    break;
                case ConnectionManagerType.MySqlType:
                    return new MySqlParameterManager().MakeSqlServerParameter(objSqlBasicMetaData);
                    break;
                default:
                    return new OracleParameterManager().MakeSqlServerParameter(objSqlBasicMetaData);
                    break;
            }
        }
        #endregion


        #region 创建参数和GetParameters配对
       /// <summary>
        /// 创建参数和GetParameters配对
       /// </summary>
       /// <param name="VariableName"></param>
       /// <param name="value"></param>
       /// <param name="typoDao"></param>
       /// <param name="length"></param>
       /// <param name="inoutput"></param>
        public void AddParameter(string VariableName, Object value, TypeDao typoDao, int length, ParameterDirection inoutput)
        {
            if (paramArrayList == null) paramArrayList = new List<DbParameter>();
            switch (_type)
            {
                case ConnectionManagerType.OracleType:
                    new OracleParameterManager().AddSqlServerParameter(ref paramArrayList, VariableName, value, typoDao, length, inoutput);
                    break;
                case ConnectionManagerType.SqlServerType:
                    new SqlServerParameterManager().AddSqlServerParameter(ref paramArrayList, VariableName, value, typoDao, length, inoutput);
                    break;
                case ConnectionManagerType.MySqlType:
                    new MySqlParameterManager().AddSqlServerParameter(ref paramArrayList, VariableName, value, typoDao, length, inoutput);
                    break;
                default:
                    new OracleParameterManager().AddSqlServerParameter(ref paramArrayList, VariableName, value, typoDao, length, inoutput);
                    break;
            }
        }
        #endregion

        #region 创建参数和GetParameters配对
        /// <summary>
        /// 创建参数和GetParameters配对
        /// </summary>
        /// <param name="VariableName"></param>
        /// <param name="value"></param>
        /// <param name="typoDao"></param>
        /// <param name="length"></param>
        public void AddParameter(string VariableName, Object value, TypeDao typoDao, int length)
        {
            if (paramArrayList == null) paramArrayList = new List<DbParameter>();
            switch (_type)
            {
                case ConnectionManagerType.OracleType:
                    new OracleParameterManager().AddSqlServerParameter(ref paramArrayList, VariableName, value, typoDao, length);
                    break;
                case ConnectionManagerType.SqlServerType:
                    new SqlServerParameterManager().AddSqlServerParameter(ref paramArrayList, VariableName, value, typoDao, length);
                    break;
                case ConnectionManagerType.MySqlType:
                    new MySqlParameterManager().AddSqlServerParameter(ref paramArrayList, VariableName, value, typoDao, length);
                    break;
                default:
                    new OracleParameterManager().AddSqlServerParameter(ref paramArrayList, VariableName, value, typoDao, length);
                    break;
            }
        }
        #endregion


        /// <summary>
        /// 记录参数列表
        /// </summary>
        private List<DbParameter> paramArrayList = null;

        #region 获得所有参数,此只针对采用AddParameter增加的参数,只能获取一次
        /// <summary>
        /// 获得所有参数,此只针对采用AddParameter增加的参数,只能获取一次
        /// </summary>
        /// <returns></returns>
        public DbParameter[] GetParameters()
        {
            if (paramArrayList == null || paramArrayList.Count == 0 ) return null;
           
            DbParameter[] dbParams = new DbParameter[paramArrayList.Count];
            for (int i = 0; i < paramArrayList.Count; i++)
            {
                dbParams[i] = paramArrayList[i];
            }
            paramArrayList.Clear();
            return dbParams;

        }
        #endregion


    }
}
