using System;
using System.Data;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;
namespace Com.ValuePlus.Database
{
    /// <summary>
    /// 数据库地层操作接口，对业务层开放，隐藏具体的实现
    /// </summary>
    public interface IDatabaseDAO:IDisposable
    {        

        #region 事务处理
        /// <summary>
        /// 开始事务
        /// </summary>
         void BeginTransaction();

        /// <summary>
        /// 确认事务
        /// </summary>
         void Commit();

        /// <summary>
        /// 回滚事务
        /// </summary>
         void RoolBack();
        #endregion

        #region 公共处理方法
        /// <summary>
        /// 执行更新操作
        /// </summary>
        /// <param name="commandType">命令类型(存储过程,命令文本或其它.)</param>
        /// <param name="Sql">sql语句或存储过程名称</param>
        /// <param name="commandParameters">绑定变量</param>
        /// <returns>操作的记录数</returns>
        int ExecuteNonQuery(CommandType commandType, String Sql, params DbParameter[] commandParameters);


        /// <summary>
        /// 返回结果集第一行的第一列
        /// </summary>
        /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param>
        /// <param name="Sql">sql语句或存储过程名称</param>
        /// <param name="commandParameters">绑定变量</param>
        /// <returns></returns>
        object ExecuteScalar(CommandType commandType, String Sql, params DbParameter[] commandParameters);

        /// <summary>
        /// 返回顺序查询记录集
        /// </summary>
        /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param>
        /// <param name="Sql">sql语句或存储过程名称</param>
        /// <param name="commandParameters">绑定变量</param>
        /// <returns></returns>
        IDataReader ExecuteReader(CommandType commandType, String Sql, params DbParameter[] commandParameters);

        /// <summary>
        /// 返回一个包含结果集的DataSet的操作
        /// </summary>
        /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param>
        /// <param name="Sql">sql语句或存储过程名称</param>
        /// <param name="commandParameters">绑定变量</param>
        /// <returns></returns>
        DataSet ExecuteDataSet(CommandType commandType, String Sql, params DbParameter[] commandParameters);

        /// <summary>
        /// 分页处理
        /// </summary>
        /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param>
        /// <param name="Sql">sql语句或存储过程名称</param>
        /// <param name="startRecord">开始的记录索引，为-1返回所有记录</param>
        /// <param name="maxRecords">检索的最大记录数</param>
        /// <param name="commandParameters">绑定变量</param>
        /// <returns></returns>
        DataSet ExecuteDataSet(CommandType commandType, String Sql, int startRecord, int maxRecords, params DbParameter[] commandParameters);      


        #endregion   


        /// <summary>
        /// 创建参数
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        DbParameter[] MakeParameter(Com.ValuePlus.Utils.SqlBasicMetaData objSqlBasicMetaData);



     
        /// <summary>
        /// 执行更新操作
        /// </summary>
        /// <param name="objSqlBasicMetaData">包含了sql语句及语句类型</param>
        /// <param name="commandParameters">绑定变量</param>
        /// <returns>操作的记录数</returns>
         int ExecuteNonQuery(Com.ValuePlus.Utils.SqlBasicMetaData objSqlBasicMetaData, params DbParameter[] commandParameters);


        /// <summary>
        /// 返回结果集第一行的第一列
        /// </summary>
        /// <param name="objSqlBasicMetaData">包含了sql语句及语句类型</param>
        /// <param name="commandParameters">绑定变量</param>
        /// <returns></returns>
         object ExecuteScalar(Com.ValuePlus.Utils.SqlBasicMetaData objSqlBasicMetaData, params DbParameter[] commandParameters);


        /// <summary>
        /// 返回顺序查询记录集
        /// </summary>
        /// <param name="objSqlBasicMetaData">包含了sql语句及语句类型</param>
        /// <param name="commandParameters">绑定变量</param>
        /// <returns></returns>
         IDataReader ExecuteReader(Com.ValuePlus.Utils.SqlBasicMetaData objSqlBasicMetaData, params DbParameter[] commandParameters);

        /// <summary>
        /// 返回一个包含结果集的DataSet的操作
        /// </summary>
        /// <param name="objSqlBasicMetaData">包含了sql语句及语句类型</param>
        /// <param name="commandParameters">绑定变量</param>
        /// <returns></returns>
         DataSet ExecuteDataSet(Com.ValuePlus.Utils.SqlBasicMetaData objSqlBasicMetaData, params DbParameter[] commandParameters);

        /// <summary>
        /// 分页处理
        /// </summary>
        /// <param name="objSqlBasicMetaData">包含了sql语句及语句类型</param>
        /// <param name="startRecord">开始的记录索引若startRecord为-1表示返回所有记录</param>
        /// <param name="maxRecords">检索的最大记录数</param>
        /// <param name="commandParameters">绑定变量</param>
        /// <returns></returns>
         DataSet ExecuteDataSet(Com.ValuePlus.Utils.SqlBasicMetaData objSqlBasicMetaData, int startRecord, int maxRecords, params DbParameter[] commandParameters);

         /// <summary>
         /// 执行存储过程操作
        /// </summary>
         /// <param name="spName">包含了sql语句及语句类型</param>
         /// <param name="paras">绑定变量</param>
        /// <returns>操作的记录数</returns>
         string ExecuteProcedure(string spName, ArrayList paras);

         /// <summary>
         /// 执行存储过程操作
         /// </summary>
         /// <param name="spName">包含了sql语句及语句类型</param>
         /// <param name="paras">绑定变量</param>
         /// <returns>操作的记录数</returns>
         string ExecuteProcedure(string spName, Hashtable paras);

         /// <summary>
        /// 执行更新操作
        /// </summary>
        /// <param name="objSqlBasicMetaData">包含了sql语句及语句类型</param>
        /// <param name="commandParameters">绑定变量</param>
        /// <returns>操作的记录数</returns>
         ArrayList ExecuteProcedureArrayList(string sql, ArrayList paras);

         #region 创建参数和GetParameters配对
        /// <summary>
         ///  创建参数和GetParameters配对
        /// </summary>
        /// <param name="VariableName"></param>
        /// <param name="value"></param>
        /// <param name="typoDao"></param>
        /// <param name="length"></param>
        /// <param name="inoutput"></param>
         void AddParameter(string VariableName, Object value, TypeDao typoDao, int length, ParameterDirection inoutput);
         #endregion

         #region 创建参数和GetParameters配对
         /// <summary>
         /// 创建参数和GetParameters配对
         /// </summary>
         /// <param name="VariableName"></param>
         /// <param name="value"></param>
         /// <param name="typoDao"></param>
         /// <param name="length"></param>
          void AddParameter(string VariableName, Object value, TypeDao typoDao, int length);
         #endregion

         #region 获得所有参数,此只针对采用AddParameter增加的参数
         /// <summary>
         /// 获得所有参数,此只针对采用AddParameter增加的参数
         /// </summary>
         /// <returns></returns>
         DbParameter[] GetParameters();
         #endregion



    }
}
