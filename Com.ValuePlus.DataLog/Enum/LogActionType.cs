using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ValuePlus.DataLog.Enum
{
    /// <summary>
    /// 操作日志的动作类型的枚举类
    /// </summary>
    public class LogActionType
    {
        /// <summary>
        /// 用户登录成功
        /// </summary>
        public const String Login_Success = "10";
        /// <summary>
        /// 用户不存在
        /// </summary>
        public const String Login_NoUser = "10-1";
        /// <summary>
        /// 用户密码错误
        /// </summary>
        public const String Login_ErrorPassword = "10-2";
        /// <summary>
        /// 修改用户密码
        /// </summary>
        public const String Login_ChangePassword = "10-3";
        /// <summary>
        /// 修改用户密码失败
        /// </summary>
        public const String Login_ChangePasswordFailed = "10-4";

        /// <summary>
        /// 用户信息查看
        /// </summary>
        public const String User_View = "100";
        /// <summary>
        /// 增加用户成功
        /// </summary>
        public const String User_AddSuccess = "100-1";
        /// <summary>
        /// 增加用户失败
        /// </summary>
        public const String User_AddFailed = "100-1-1";
        /// <summary>
        /// 删除用户成功
        /// </summary>
        public const String User_DeleteSuccess = "100-2";
        /// <summary>
        /// 修改用户信息成功
        /// </summary>
        public const String User_ModifySuccess = "100-3";

        /// <summary>
        /// 用户注销成功
        /// </summary>
        public const String LogOut_Success = "20";

        /// <summary>
        /// 新增模板操作
        /// </summary>
        public const String Archive_Add = "200-1";
        /// <summary>
        /// 删除模板操作
        /// </summary>
        public const String Archive_Delete = "200-2";
        /// <summary>
        /// 修改模板操作
        /// </summary>
        public const String Archive_Modify = "200-3";
        /// <summary>
        /// 查看模板操作
        /// </summary>
        public const String Archive_View = "200-4";

        /// <summary>
        /// 在线用户查看
        /// </summary>
        public const String OnlineUser_View = "30";

        /// <summary>
        /// 动作执行 add by sammen 20250304
        /// </summary>
        public const String ActionExecute = "40";
        public const String ActionExecute_Success = "40-1";
        public const String ActionExecute_Failed = "40-2";

        /// <summary>
        /// 报表查询 add by sammen 20250304
        /// </summary>
        public const String ReportView = "50";

    }
}
