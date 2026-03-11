using System;
namespace Com.ValuePlus.Utils.Cache
{
   
    public class CacheKeyIsNullOrEmptyException : Exception
    {
        private const string m_Message = "标识缓存的缓存键不能为NULL或空值！";

        public CacheKeyIsNullOrEmptyException() : base("标识缓存的缓存键不能为NULL或空值！")
        {
        }

        public CacheKeyIsNullOrEmptyException(string message) : base(message)
        {
        }

        public CacheKeyIsNullOrEmptyException(string message, Exception ex) : base(message, ex)
        {
        }
    }
}

