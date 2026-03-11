using System;
using MySql.Data.MySqlClient;
using System.Data.Common;
using System.Data;
using System.Collections;
using System.Collections.Generic;
namespace Com.ValuePlus.Database
{
    internal sealed class MySqlParameterManager
    {

        /// <summary>
        /// 转化成MySq类型
        /// </summary>
        /// <param name="sqlbasicMetadata"></param>
        /// <returns></returns>
        public MySqlParameter[] MakeSqlServerParameter(Com.ValuePlus.Utils.SqlBasicMetaData sqlbasicMetadata)
        {

            return null;
        }


        public void AddSqlServerParameter(ref List<DbParameter> cmd, string VariableName, Object value, TypeDao typoDao, int length, ParameterDirection inoutput)
        {
          
        }
        public void AddSqlServerParameter(ref List<DbParameter> cmd, string VariableName, Object value, TypeDao typoDao, int length)
        {

        }

        /// <summary>
        /// 执行存储过程的时候强制匹配类型
        /// </summary>
        /// <param name="cmd"></param>
        public void ExecuteProcedure(ref DbCommand cmd, ArrayList paras)
        {
            
        }

        /// <summary>
        /// 执行存储过程的时候强制匹配类型
        /// </summary>
        /// <param name="cmd"></param>
        public void ExecuteProcedure(ref DbCommand cmd, Hashtable paras)
        {

        }
        /// <summary>
        /// 执行存储过程的时候强制匹配类型
        /// </summary>
        /// <param name="cmd"></param>
        public void ExecuteProcedureInOut(ref DbCommand cmd, ArrayList paras)
        {
            
        }
    }
}
