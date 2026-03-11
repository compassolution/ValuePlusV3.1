using System;
using System.Collections.Generic;
using System.Text;
using log4net;
using System.Reflection;

namespace Com.ValuePlus.Log
{
    /// <summary>
    /// 日志操作类	/// 
    /// LogHelper 的摘要说明。
    /// </summary>
    internal class LogHelper : Com.ValuePlus.Log.ILog
    {
        private log4net.ILog log;
        private string logname = "LogInfo";
        private bool isLogEnable = true;

        /// <summary>
        /// 默认日志
        /// </summary>
        public LogHelper()
        {
            log = log4net.LogManager.GetLogger(logname);           
        }
        /// <summary>
        /// 自定义日志
        /// </summary>
        /// <param name="logger"></param>
        public LogHelper(string  logger)
        {
            logname = logger;
            log = log4net.LogManager.GetLogger(logger);
        }
        /// <summary>
        /// 类定义日志
        /// </summary>
        /// <param name="type"></param>
        public LogHelper(Type type)
        {
            log = log4net.LogManager.GetLogger(type);
        }
        /// <summary>
        /// 记录错误级别的信息，包括自定义描述信息和程序异常
        /// </summary>
        /// <param name="message">自定义描述信息</param>
        /// <param name="e">程序异常</param>
        public void Error(object message, Exception e)
        {
            if (isLogEnable)
            {
                log.Error(message, e);
            }

        }

        /// <summary>
        /// 记录错误级别的信息，包括自定义描述信息
        /// </summary>
        /// <param name="message">自定义描述信息</param>
        public void Error(object message)
        {
            if (isLogEnable)
            {
                log.Error(message);
            }
        }       

        /// <summary>
        /// 记录警告级别的信息，包括自定义描述信息和程序异常
        /// </summary>
        /// <param name="message">自定义描述信息</param>
        /// <param name="e">程序异常</param>
        public void Warn(object message, Exception e)
        {
            if (isLogEnable)
            {
                log.Warn(message, e);
            }
        }

        /// <summary>
        /// 记录警告级别的信息，包括自定义描述信息
        /// </summary>
        /// <param name="message">自定义描述信息</param>
        public void Warn(object message)
        {

            if (isLogEnable)
            {
                log.Warn(message);
            }
        }

        /// <summary>
        /// 记录严重级别的信息，包括自定义描述信息和程序异常
        /// </summary>
        /// <param name="message">自定义描述信息</param>
        /// <param name="e">程序异常</param>
        public void Fatal(object message, Exception e)
        {

            if (isLogEnable)
            {
                log.Fatal(message, e);
            }
        }

        /// <summary>
        /// 记录严重级别的信息，包括自定义描述信息
        /// </summary>
        /// <param name="message">自定义描述信息</param>
        public void Fatal(object message)
        {

            if (isLogEnable)
            {
                log.Fatal(message);
            }
        }
        /// <summary>
        /// 记录信息级别的信息，包括自定义描述信息和程序异常
        /// </summary>
        /// <param name="message"></param>
        /// <param name="e"></param>
        public void Info(object message, Exception e)
        {

            if (isLogEnable)
            {
                log.Info(message, e);
            }
        }
        /// <summary>
        /// 记录信息级别的信息，包括自定义描述信息
        /// </summary>
        /// <param name="message"></param>
        public void Info(object message)
        {

            if (isLogEnable)
            {
                log.Info(message);
            }
        }

        /// <summary>
        /// 记录调试级别的信息，包括自定义描述信息和程序异常
        /// </summary>
        /// <param name="message">自定义描述信息</param>
        /// <param name="e">程序异常</param>
        public void Debug(object message, Exception e)
        {
            if (isLogEnable)
            {
                log.Debug(message, e);
            }
        }

        /// <summary>
        /// 记录调试级别的信息，包括自定义描述信息
        /// </summary>
        /// <param name="message">自定义描述信息</param>
        public void Debug(object message)
        {

            if (isLogEnable)
            {
                log.Debug(message);
            }
        }

    }
}
