using System;
namespace Com.ValuePlus.Utils.Cache
{
   
    public class CacheObjectIsNullOrDBNullException : Exception
    {
        private const string m_Message = "需要缓存的对象不能为NULL或DBNULL！";

        public CacheObjectIsNullOrDBNullException() : base("需要缓存的对象不能为NULL或DBNULL！")
        {
        }

        public CacheObjectIsNullOrDBNullException(string message) : base(message)
        {
        }

        public CacheObjectIsNullOrDBNullException(string message, Exception ex) : base(message, ex)
        {
        }
    }
}

