 using System;
 namespace Com.ValuePlus.Utils.Cache
{
   

    public class CacheFileNotExistException : Exception
    {
        private const string m_Message = "缓存所依赖的文件不存在！";

        public CacheFileNotExistException() : base("缓存所依赖的文件不存在！")
        {
        }

        public CacheFileNotExistException(string message) : base(message)
        {
        }

        public CacheFileNotExistException(string message, Exception ex) : base(message, ex)
        {
        }
    }
}

