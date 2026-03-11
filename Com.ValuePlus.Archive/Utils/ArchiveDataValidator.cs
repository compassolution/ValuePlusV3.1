using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Com.ValuePlus.Archive.Utils
{
    public class ArchiveDataValidator
    {
        //#region 判断日期
        ///// <summary>
        ///// 判断日期的日期部分格式
        ///// </summary>
        ///// <param name="dateStr">输入的日期的日期部分字符串</param>
        ///// <returns>bool</returns>
        //public static bool isDate(string dateStr)
        //{
        //    bool _isDate = false;
        //    string matchStr = "";
        //    matchStr += @"^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-))$ ";
        //    RegexOptions option = (RegexOptions.IgnoreCase | (RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace));
        //    if (Regex.IsMatch(dateStr, matchStr, option))
        //        _isDate = true;
        //    else
        //        _isDate = false;

        //    return _isDate;

        //}

        ///// <summary>
        ///// 判断日期的时间部分格式
        ///// </summary>
        ///// <param name="time_str">输入日期的时间部分字符串</param>
        ///// <returns>bool</returns>
        //public static bool isTime(string time_str)
        //{
        //    bool _isDate = false;
        //    string matchStr = "";
        //    //matchStr += @"^(\s(((0?[0-9])|([1-2][0-3]))\:([0-5]?[0-9])((\s)|(\:([0-5]?[0-9])))))?$ ";

        //    //matchStr += @"(20|21|22|23|[0-1]?\d):[0-5]?\d:[0-5]?\d$";

        //    //matchStr += @"(0*[0-9]|[1-2][0-3]):(0*[0-9]|[1-5][0-9]):(0[0-9]|[1-5][0-9])";
        //    matchStr += @"^([01]\d|2[0-3]):([0-5]\d):([0-5]\d)$";
        //    RegexOptions option = (RegexOptions.IgnoreCase | (RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace));
        //    if (Regex.IsMatch(time_str, matchStr, option))
        //        _isDate = true;
        //    else
        //        _isDate = false;

        //    return _isDate;
        //}

        ///// <summary>
        ///// 判断日期的全部格式
        ///// </summary>
        ///// <param name="dateStr">输入日期的字符串</param>
        ///// <returns></returns>
        //public static bool isDateTime(string dateStr)
        //{
        //    bool _isDate = false;
        //    string matchStr = "";
        //    matchStr += @"^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-)) ";
        //    matchStr += @"(\s(((0?[0-9])|([1-2][0-3]))\:([0-5]?[0-9])((\s)|(\:([0-5]?[0-9])))))?$ ";
        //    RegexOptions option = (RegexOptions.IgnoreCase | (RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace));
        //    if (Regex.IsMatch(dateStr, matchStr, option))
        //        _isDate = true;
        //    else
        //        _isDate = false;
        //    return _isDate;
        //}

        //#endregion 判断日期

        /// <summary>
        /// 判断是否可转化为DateTime类型
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static bool isDate(String strValue)
        {
            try
            {
                if (strValue.Length < 8)//如果只是输入年月也判断非日期类型
                {
                    return false;
                }
                Convert.ToDateTime(strValue);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 判断是否是时间格式
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static bool isTime(String strValue)
        {
            try
            {
                Convert.ToDateTime("1900-01-01 "+strValue);
                return true;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// 判断是否可转化为INT类型
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static bool isInt32(String strValue)
        {
            try
            {
                Convert.ToInt32(strValue);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 判断是否可转化为numeric类型
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static bool isNumeric(String strValue)
        {
            try
            {
                Convert.ToDecimal(strValue);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 判断是否可转化为float类型
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static bool isFloat(String strValue)
        {
            try
            {
                Convert.ToSingle(strValue);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 根据数据类型以及页面输入，判断输入值是否有效
        /// </summary>
        /// <param name="strType"></param>
        /// <param name="strInputValue"></param>
        /// <returns></returns>
        public static bool IsValidDataInput(String strType,String strInputValue)
        {
            bool bIsValid = true;
            if ((!String.IsNullOrEmpty(strType)) && (!String.IsNullOrEmpty(strInputValue)))
            {
                switch (strType.ToLower())
                {
                    case "date":
                    case "time":
                    case "datetime":
                        bIsValid = isDate(strInputValue);
                        break;
                    case "int":
                        bIsValid = isInt32(strInputValue);
                        break;
                    case "numeric":
                        bIsValid = isNumeric(strInputValue);
                        break;
                    case "float":
                        bIsValid = isFloat(strInputValue);
                        break;
                }
            }
            return bIsValid;
        }

    }
}
