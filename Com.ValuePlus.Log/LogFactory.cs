using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ValuePlus.Log
{
    public class LogFactory
    {
        /// <summary>
        /// 默认监听器
        /// </summary>
        /// <returns></returns>
        public static Com.ValuePlus.Log.ILog CreateInstance()
        {
            return new Com.ValuePlus.Log.LogHelper();
        }
        /// <summary>
        /// 自定义监听器
        /// </summary>
        /// <param name="appender"></param>
        /// <returns></returns>
        public static Com.ValuePlus.Log.ILog CreateInstance(string logger)
        {
            return new Com.ValuePlus.Log.LogHelper(logger);
        }
        /// <summary>
        /// 类定义日志
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Com.ValuePlus.Log.ILog CreateInstance(Type type)
        {
            return new Com.ValuePlus.Log.LogHelper(type);
        }
    }
}
