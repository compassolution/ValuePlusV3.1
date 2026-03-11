using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Collections;
namespace Com.ValuePlus.Database
{
    internal sealed class SqlServerParameterManager
    {


        public void AddSqlServerParameter(ref List<DbParameter> paramlist, string VariableName, Object value, TypeDao typoDao, int length, ParameterDirection inoutput)
        {
            SqlParameter subSqlParam = new SqlParameter();
            if (length > 0) subSqlParam.Size = length;
            if (!string.IsNullOrEmpty(VariableName)) subSqlParam.ParameterName = VariableName;
            if (!string.IsNullOrEmpty(typoDao.ToString().ToLower()))
            {
                subSqlParam.SqlDbType = SqlTypeString2SqlType(typoDao.ToString().ToLower());
            }
            subSqlParam.Value = value;          
            subSqlParam.Direction = inoutput;
            paramlist.Add(subSqlParam);
        }

        public void AddSqlServerParameter(ref List<DbParameter> paramlist, string VariableName, Object value, TypeDao typoDao, int length)
        {
            SqlParameter subSqlParam = new SqlParameter();
            if (length > 0) subSqlParam.Size = length;
            if (!string.IsNullOrEmpty(VariableName)) subSqlParam.ParameterName = VariableName;
            if (!string.IsNullOrEmpty(typoDao.ToString().ToLower()))
            {
                subSqlParam.SqlDbType = SqlTypeString2SqlType(typoDao.ToString().ToLower());
            }
            subSqlParam.Value = value;
            //将变量参数加入到command中
            paramlist.Add(subSqlParam);
        }

        /// <summary>
        /// 转化成sql类型
        /// </summary>
        /// <param name="sqlbasicMetadata"></param>
        /// <returns></returns>
        public SqlParameter[] MakeSqlServerParameter(Com.ValuePlus.Utils.SqlBasicMetaData sqlbasicMetadata)
        {           
            if (sqlbasicMetadata.NameValueParameters != null && sqlbasicMetadata.NameValueParameters.Count > 0)
            {
                SqlParameter[] sqlparam = new SqlParameter[sqlbasicMetadata.NameValueParameters.Count];
                for (int i = 0; i < sqlbasicMetadata.NameValueParameters.Count; i++)
                {
                    Com.ValuePlus.Utils.NameValueParameters nv = sqlbasicMetadata.NameValueParameters[i];
                    SqlParameter subSqlParam = new SqlParameter();
                    if (nv.VariableLength > 0) subSqlParam.Size = nv.VariableLength;
                    subSqlParam.ParameterName = nv.VariableName;
                    subSqlParam.SqlDbType = SqlTypeString2SqlType(nv.VariableType);
                    if(sqlbasicMetadata.CommandType.ToUpper() != "TEXT"){
                        if (string.IsNullOrEmpty(nv.InOutPut))
                        {
                            subSqlParam.Direction = ParameterDirection.Input;
                        }
                        else
                        {
                            switch (nv.InOutPut.ToUpper())
                            {
                                case "InputOutput":
                                    subSqlParam.Direction = ParameterDirection.InputOutput;
                                    break;
                                case "OUTPUT":
                                    subSqlParam.Direction = ParameterDirection.Output;
                                    break;
                                default:
                                    subSqlParam.Direction = ParameterDirection.Input;
                                    break;
                            }
                        }
                    }
                    sqlparam[i] = subSqlParam;
                }
                return sqlparam;
            }
            return null;
        }

        /// <summary>
        /// 执行存储过程的时候强制匹配类型
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="paras"></param>
        public void ExecuteProcedure(ref DbCommand cmd,ArrayList paras)
        {
            SqlCommand sqlcmd = (SqlCommand)cmd ;
            sqlcmd.CommandType = CommandType.StoredProcedure;
            SqlCommandBuilder.DeriveParameters(sqlcmd);
            if (paras != null)
            {
                for (int i = 1; i < sqlcmd.Parameters.Count; i++)
                {
                    sqlcmd.Parameters[i].Value = paras[i-1];
                }
            }
            cmd = sqlcmd; 
        }

        /// <summary>
        /// 执行存储过程的时候强制匹配类型
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="hsTableParam"></param>
        public void ExecuteProcedure(ref DbCommand cmd, Hashtable hsTableParam)
        {
            SqlCommand sqlcmd = (SqlCommand)cmd;
            sqlcmd.CommandType = CommandType.StoredProcedure;
            SqlCommandBuilder.DeriveParameters(sqlcmd);
            if (hsTableParam != null)
            {
                if ((hsTableParam != null) && (hsTableParam.Count > 0))
                {
                    foreach (System.Collections.DictionaryEntry entity in hsTableParam)
                    {
                        String strParamName = entity.Key.ToString();
                        String strParamValue = hsTableParam[strParamName].ToString();
                        sqlcmd.Parameters["@" + strParamName].Value = strParamValue;
                    }
                }
            }
            cmd = sqlcmd;
        }

        /// <summary>
        /// 执行存储过程的时候强制匹配类型
        /// </summary>
        /// <param name="cmd"></param>
        public void ExecuteProcedureInOut(ref DbCommand cmd, ArrayList paras)
        {
            SqlCommand sqlcmd = (SqlCommand)cmd;
            sqlcmd.CommandType = CommandType.StoredProcedure;
            SqlCommandBuilder.DeriveParameters(sqlcmd);
            if (paras != null)
            {
                for (int i = 1; i < sqlcmd.Parameters.Count; i++)
                {
                    if ((sqlcmd.Parameters[i].Direction == ParameterDirection.Input) | (sqlcmd.Parameters[i].Direction == ParameterDirection.InputOutput))
                    {
                        sqlcmd.Parameters[i].Value = paras[i-1];
                    }
                }
            }
            cmd = sqlcmd; 
        }

        /// <summary>
        /// 将字符串类型转换成sqlserver数据类型
        /// </summary>
        /// <param name="sqlTypeString"></param>
        /// <returns></returns>
        private static SqlDbType SqlTypeString2SqlType(string sqlTypeString)
        {
            SqlDbType dbType = SqlDbType.Variant;//默认为Object

            switch (sqlTypeString.ToLower())
            {
                case "int":
                    dbType = SqlDbType.Int;
                    break;
                case "varchar":
                    dbType = SqlDbType.VarChar;
                    break;
                case "bit":
                    dbType = SqlDbType.Bit;
                    break;
                case "datetime":
                    dbType = SqlDbType.DateTime;
                    break;
                case "decimal":
                    dbType = SqlDbType.Decimal;
                    break;
                case "float":
                    dbType = SqlDbType.Float;
                    break;
                case "image":
                    dbType = SqlDbType.Image;
                    break;
                case "money":
                    dbType = SqlDbType.Money;
                    break;
                case "ntext":
                    dbType = SqlDbType.NText;
                    break;
                case "nvarchar":
                    dbType = SqlDbType.NVarChar;
                    break;
                case "smalldatetime":
                    dbType = SqlDbType.SmallDateTime;
                    break;
                case "smallint":
                    dbType = SqlDbType.SmallInt;
                    break;
                case "text":
                    dbType = SqlDbType.Text;
                    break;
                case "bigint":
                    dbType = SqlDbType.BigInt;
                    break;
                case "binary":
                    dbType = SqlDbType.Binary;
                    break;
                case "char":
                    dbType = SqlDbType.Char;
                    break;
                case "nchar":
                    dbType = SqlDbType.NChar;
                    break;
                case "numeric":
                    dbType = SqlDbType.Decimal;
                    break;
                case "real":
                    dbType = SqlDbType.Real;
                    break;
                case "smallmoney":
                    dbType = SqlDbType.SmallMoney;
                    break;
                case "sql_variant":
                    dbType = SqlDbType.Variant;
                    break;
                case "timestamp":
                    dbType = SqlDbType.Timestamp;
                    break;
                case "tinyint":
                    dbType = SqlDbType.TinyInt;
                    break;
                case "uniqueidentifier":
                    dbType = SqlDbType.UniqueIdentifier;
                    break;
                case "varbinary":
                    dbType = SqlDbType.VarBinary;
                    break;
                case "xml":
                    dbType = SqlDbType.Xml;
                    break;
            }
            return dbType;
        }

    }
}
