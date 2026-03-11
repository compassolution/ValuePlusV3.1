using System;
namespace Com.ValuePlus.Utils.Cache
{
   

    public class CachFilePathIsNullOrEmptyException : Exception
    {
        private const string m_Message = "缓存所依赖的文件的路径不能为NULL或空值！";

        public CachFilePathIsNullOrEmptyException() : base("缓存所依赖的文件的路径不能为NULL或空值！")
        {
        }

        public CachFilePathIsNullOrEmptyException(string message) : base(message)
        {
        }

        public CachFilePathIsNullOrEmptyException(string message, Exception ex) : base(message, ex)
        {
        }
    }
}

