using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ValuePlus.DataLog.Enum
{
    /// <summary>
    /// 操作日志类型的枚举类
    /// </summary>
    public class LogType 
    {
        /// <summary>
        /// 用户登录
        /// </summary>
        public const String Login = "10";
        /// <summary>
        /// 用户管理
        /// </summary>
        public const String User = "100";
        /// <summary>
        /// 用户注销
        /// </summary>
        public const String LogOut = "20";
        /// <summary>
        /// 模板操作
        /// </summary>
        public const String Archive = "200";
        /// <summary>
        /// 在线用户
        /// </summary>
        public const String OnlineUser = "30";
    }
}
