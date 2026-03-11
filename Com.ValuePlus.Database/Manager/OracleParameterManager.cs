using System;
using System.Data.OracleClient;
using System.Data.Common;
using System.Data;
using System.Collections;
using System.Collections.Generic;
namespace Com.ValuePlus.Database
{
    internal sealed class OracleParameterManager
    {

        /// <summary>
        /// 转化成Oracle类型
        /// </summary>
        /// <param name="sqlbasicMetadata"></param>
        /// <returns></returns>
        public OracleParameter[] MakeSqlServerParameter(Com.ValuePlus.Utils.SqlBasicMetaData sqlbasicMetadata)
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
