using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ValuePlus.Utils
{
    /// <summary>
    /// 针对sql配置文件的基础元数据
    /// </summary>
    [Serializable]
    public class SqlBasicMetaData
    {
        private string _CommandType;
        /// <summary>
        /// 命令类型
        /// </summary>
        public string CommandType
        {
            get { return _CommandType; }
            set { _CommandType = value; }
        }
       
        private string _CommandSql;

        /// <summary>
        /// 命令sql语句
        /// </summary>
        public string CommandSql
        {
            get { return _CommandSql; }
            set { _CommandSql = value; }
        }
       
        private List<NameValueParameters> _NameValueParameters;

        /// <summary>
        /// 参数
        /// </summary>
        public List<NameValueParameters> NameValueParameters
        {
            get { return _NameValueParameters; }
            set { _NameValueParameters = value; }
        }
    }

    /// <summary>
    /// 参数结构类型
    /// </summary>
    [Serializable]
    public struct NameValueParameters
    {
        /// <summary>
        /// 变量名字
        /// </summary>
        public string VariableName;
        /// <summary>
        /// 变量类型
        /// </summary>
        public string VariableType;
        /// <summary>
        /// 变量长度
        /// </summary>
        public int VariableLength;
        /// <summary>
        /// 输入还是输出用户存储过程
        /// </summary>
        public string InOutPut;
    }
}
