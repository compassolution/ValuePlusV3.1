   using System;
   namespace Com.ValuePlus.Utils.Cache
{
 

    public class AbsoluteExpirationIsErrorException : Exception
    {
        private const string m_Message = "缓存的对象的过期时间必须要大于当前时间！";

        public AbsoluteExpirationIsErrorException() : base("缓存的对象的过期时间必须要大于当前时间！")
        {
        }

        public AbsoluteExpirationIsErrorException(string message) : base(message)
        {
        }

        public AbsoluteExpirationIsErrorException(string message, Exception ex) : base(message, ex)
        {
        }
    }
}

