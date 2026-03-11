
using System;
using System.IO;
using System.Web;
using System.Web.Caching;

namespace Com.ValuePlus.Utils.Cache
{ 
   

    public static class CacheManager
    {
        public static object GetCache(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new CacheKeyIsNullOrEmptyException();
            }
            return HttpRuntime.Cache[key];
        }

        public static void Insert(string key, object value)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new CacheKeyIsNullOrEmptyException();
            }
            if (value == null)
            {
                throw new CacheObjectIsNullOrDBNullException();
            }
            HttpRuntime.Cache.Insert(key, value);
        }

        public static void Insert(string key, object value, DateTime absoluteExpiration)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new CacheKeyIsNullOrEmptyException();
            }
            if (value == null)
            {
                throw new CacheObjectIsNullOrDBNullException();
            }
            if (DateTime.Now >= absoluteExpiration)
            {
                throw new AbsoluteExpirationIsErrorException();
            }
            HttpRuntime.Cache.Insert(key, value, null, absoluteExpiration, TimeSpan.Zero);
        }

        public static void Insert(string key, object value, string path)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new CacheKeyIsNullOrEmptyException();
            }
            if (value == null)
            {
                throw new CacheObjectIsNullOrDBNullException();
            }
            if (string.IsNullOrEmpty(path))
            {
                throw new CachFilePathIsNullOrEmptyException();
            }
            if (!File.Exists(path))
            {
                throw new CacheFileNotExistException();
            }
            HttpRuntime.Cache.Insert(key, value, new CacheDependency(path));
        }
    }
}

