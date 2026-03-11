using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Com.ValuePlus.Utils
{
    /// <summary>
    /// 类型转换通用操作类
    /// </summary>
    public class TypeParse
    {
        #region  判断对象是否为Int32类型的数字
        /// <summary>
        /// 判断对象是否为Int32类型的数字
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public static bool IsNumeric(object Expression)
        {
            if (Expression != null)
            {
                string str = Expression.ToString();
                if (str.Length > 0 && str.Length <= 11 && Regex.IsMatch(str, @"^[-]?[0-9]*[.]?[0-9]*$"))
                {
                    if ((str.Length < 10) || (str.Length == 10 && str[0] == '1') || (str.Length == 11 && str[0] == '-' && str[1] == '1'))
                    {
                        return true;
                    }
                }
            }
            return false;

        }
        #endregion

        #region 判断对象是否为Double类型的数字
        /// <summary>
        ///  判断对象是否为Double类型的数字
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public static bool IsDouble(object Expression)
        {
            if (Expression != null)
            {
                return Regex.IsMatch(Expression.ToString(), @"^([0-9])[0-9]*(\.\w*)?$");
            }
            return false;
        }
        #endregion

        #region string型转换为bool型
        /// <summary>
        /// string型转换为bool型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的bool类型结果</returns>
        public static bool StrToBool(object Expression, bool defValue)
        {
            if (Expression != null)
            {
                if (string.Compare(Expression.ToString(), "true", true) == 0)
                {
                    return true;
                }
                else if (string.Compare(Expression.ToString(), "false", true) == 0)
                {
                    return false;
                }
            }
            return defValue;
        }
        #endregion

        #region 将对象转换为Int32类型
        /// <summary>
        /// 将对象转换为Int32类型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static int StrToInt(object Expression, int defValue)
        {

            if (Expression != null)
            {
                string str = Expression.ToString();
                if (str.Length > 0 && str.Length <= 11 && Regex.IsMatch(str, @"^[-]?[0-9]*$"))
                {
                    if ((str.Length < 10) || (str.Length == 10 && str[0] == '1') || (str.Length == 11 && str[0] == '-' && str[1] == '1'))
                    {
                        return Convert.ToInt32(str);
                    }
                }
            }
            return defValue;
        }
        #endregion

        #region string型转换为float型
        /// <summary>
        /// string型转换为float型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static float StrToFloat(object strValue, float defValue)
        {
            if ((strValue == null) || (strValue.ToString().Length > 10))
            {
                return defValue;
            }

            float intValue = defValue;
            if (strValue != null)
            {
                bool IsFloat = Regex.IsMatch(strValue.ToString(), @"^([-]|[0-9])[0-9]*(\.\w*)?$");
                if (IsFloat)
                {
                    intValue = Convert.ToSingle(strValue);
                }
            }
            return intValue;
        }
        #endregion

        #region 判断给定的字符串数组(strNumber)中的数据是不是都为数值型
        /// <summary>
        /// 判断给定的字符串数组(strNumber)中的数据是不是都为数值型
        /// </summary>
        /// <param name="strNumber">要确认的字符串数组</param>
        /// <returns>是则返加true 不是则返回 false</returns>
        public static bool IsNumericArray(string[] strNumber)
        {
            if (strNumber == null)
            {
                return false;
            }
            if (strNumber.Length < 1)
            {
                return false;
            }
            foreach (string id in strNumber)
            {
                if (!IsNumeric(id))
                {
                    return false;
                }
            }
            return true;

        }
        #endregion

        #region 将long型数值转换为Int32类型
        /// <summary>
        /// 将long型数值转换为Int32类型
        /// </summary>
        /// <param name="objNum"></param>
        /// <returns></returns>
        public static int SafeInt32(object objNum)
        {
            if (objNum == null)
            {
                return 0;
            }
            string strNum = objNum.ToString();
            if (IsNumeric(strNum))
            {

                if (strNum.ToString().Length > 9)
                {
                    if (strNum.StartsWith("-"))
                    {
                        return int.MinValue;
                    }
                    else
                    {
                        return int.MaxValue;
                    }
                }
                return Int32.Parse(strNum);
            }
            else
            {
                return 0;
            }
        }
        #endregion

        #region 验证是否为正整数
        /// <summary>
        /// 验证是否为正整数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsInt(string str)
        {

            return Regex.IsMatch(str, @"^[0-9]*$");
        }
        #endregion

        #region ConverToStringUTF8
        public static string ConvertToStringUtf8(string str)
        {
            //byte[] buffer = UTF8Encoding.GetBytes(str);
            byte[] buffer = Encoding.ASCII.GetBytes(str);
            System.Text.Encoding encoding = System.Text.Encoding.UTF8;
            return encoding.GetString(buffer);
        }
        #endregion

        #region ConvertToString
        /// <summary>
        /// 转换成字符串,如果不能转换,出现异常同则返回缺省值
        /// </summary>
        /// <param name="obj">转换对象</param>
        /// <param name="iDefault">缺省值</param>
        /// <returns>转换结果值</returns>
        public static string ConvertToString(Object obj, string strDefault)
        {
            try
            {
                if (obj == null)
                {
                    return strDefault;
                }

                return obj.ToString();
            }
            catch
            {
            }
            return strDefault;
        }
        /// <summary>
        /// 转换成整形数,如果不能转换,出现异常同则返回0
        /// </summary>
        /// <param name="obj">转换对象</param>
        /// <returns>转换结果值</returns>
        public static string ConvertToString(Object obj)
        {
            return ConvertToString(obj, "");
        }
        #endregion

        #region ConvertToLong
        /// <summary>
        /// 转换成长整形数,如果不能转换,出现异常同则返回缺省值
        /// </summary>
        /// <param name="obj">转换对象</param>
        /// <param name="iDefault">缺省值</param>
        /// <returns>转换结果值</returns>
        public static long ConvertToLong(Object obj, long lDefault)
        {
            try
            {
                if (obj == null)
                {
                    return lDefault;
                }

                long lRet = long.Parse(obj.ToString().Trim());
                return lRet;
            }
            catch
            {
            }
            return lDefault;
        }
        /// <summary>
        /// 转换成长整形数,如果不能转换,出现异常同则返回缺省值
        /// </summary>
        /// <param name="obj">转换对象</param>
        /// <param name="iDefault">缺省值</param>
        /// <returns>转换结果值</returns>
        public static long ConvertToLong(Object obj)
        {
            return ConvertToLong(obj, 0);
        }

        #endregion

        #region ConvertToInt
        /// <summary>
        /// 转换成整形数,如果不能转换,出现异常同则返回缺省值
        /// </summary>
        /// <param name="obj">转换对象</param>
        /// <param name="iDefault">缺省值</param>
        /// <returns>转换结果值</returns>
        public static int ConvertToInt(Object obj, int iDefault)
        {
            try
            {
                if (obj == null)
                {
                    return iDefault;
                }
                //int.Parse(obj.ToString().Trim())
                int iRet = Convert.ToInt32(Convert.ToDouble(obj.ToString().Trim()));
                return iRet;
            }
            catch
            {
            }
            return iDefault;
        }
        /// <summary>
        /// 转换成整形数,如果不能转换,出现异常同则返回0
        /// </summary>
        /// <param name="obj">转换对象</param>
        /// <returns>转换结果值</returns>
        public static int ConvertToInt(Object obj)
        {
            return ConvertToInt(obj, 0);
        }
        #endregion

        #region ConvertToDouble
        /// <summary>
        /// 转换成double,如果不能转换,出现异常同则返回缺省值
        /// </summary>
        /// <param name="obj">转换对象</param>
        /// <param name="iDefault">缺省值</param>
        /// <returns>转换结果值</returns>
        public static double ConvertToDouble(Object obj, double dDefault)
        {
            try
            {
                if (obj == null)
                {
                    return dDefault;
                }

                double dRet = double.Parse(obj.ToString().Trim());
                return dRet;
            }
            catch
            {
            }
            return dDefault;
        }
        /// <summary>
        /// 转换成double,如果不能转换,出现异常同则返回0
        /// </summary>
        /// <param name="obj">转换对象</param>
        /// <returns>转换结果值</returns>
        public static double ConvertToDouble(Object obj)
        {
            return ConvertToDouble(obj, 0);
        }
        #endregion

        #region ConvertToDecimal
        /// <summary>
        /// 转换成decimal, 如果不能转换,出现异常同则返回缺省值
        /// </summary>
        /// <param name="obj">转换对象</param>
        /// <param name="iDefault">缺省值</param>
        /// <returns>转换结果值</returns>
        public static decimal ConvertToDecimal(Object obj, decimal dDefault)
        {
            try
            {
                if (obj == null)
                {
                    return dDefault;
                }
                decimal dRet = decimal.Parse(obj.ToString().Trim());
                return dRet;
            }
            catch
            {
            }
            return dDefault;
        }
        /// <summary>
        /// 转换成float,如果不能转换,出现异常同则返回0
        /// </summary>
        /// <param name="obj">转换对象</param>
        /// <returns>转换结果值</returns>
        public static decimal ConvertToDecimal(Object obj)
        {
            return ConvertToDecimal(obj, 0);
        }
        #endregion

        #region ConvertToFloat
        /// <summary>
        /// 转换成float, 如果不能转换,出现异常同则返回缺省值
        /// </summary>
        /// <param name="obj">转换对象</param>
        /// <param name="iDefault">缺省值</param>
        /// <returns>转换结果值</returns>
        public static float ConvertToFloat(Object obj, float fDefault)
        {
            try
            {
                if (obj == null)
                {
                    return fDefault;
                }
                float fRet = float.Parse(obj.ToString().Trim());
                return fRet;
            }
            catch
            {
            }
            return fDefault;
        }
        /// <summary>
        /// 转换成float,如果不能转换,出现异常同则返回0
        /// </summary>
        /// <param name="obj">转换对象</param>
        /// <returns>转换结果值</returns>
        public static float ConvertToFloat(Object obj)
        {
            return ConvertToFloat(obj, 0);
        }

        #endregion

        #region ConvertToDateTime
        /// <summary>
        /// 转换成DateTime, 如果不能转换,出现异常同则返回缺省值
        /// </summary>
        /// <param name="obj">转换对象</param>
        /// <param name="iDefault">缺省值</param>
        /// <returns>转换结果值</returns>
        public static DateTime ConvertToDateTime(Object obj, DateTime dateDefault)
        {
            try
            {
                if (obj != null && obj is DateTime)
                    return (DateTime)obj;

                DateTime dateRet = DateTime.Parse(obj.ToString().Trim());
                if ((dateRet > DateTime.Parse("1753-01-01 12:00:00"))
                    && (dateRet < DateTime.Parse("9999-01-01 23:59:59")))
                {
                    return dateRet;
                }
            }
            catch
            {
            }
            return dateDefault;
        }


        /// <summary>
        /// 转换成float,如果不能转换,出现异常同则返回0
        /// </summary>
        /// <param name="obj">转换对象</param>
        /// <returns>转换结果值</returns>
        public static DateTime ConvertToDateTime(Object obj)
        {
            return ConvertToDateTime(obj, DateTime.Parse("1900-01-01 01:01:01"));
        }
        #endregion

        #region ConvertToDateTimeOfShort
        /// <summary>
        /// 转换成DateTime, 如果不能转换,出现异常同则返回缺省值
        /// </summary>
        /// <param name="obj">转换对象</param>
        /// <param name="iDefault">缺省值</param>
        /// <returns>转换结果值</returns>
        public static DateTime ConvertToDateTimeOfShort(Object obj, DateTime dateDefault)
        {
            try
            {
                DateTime dateRet = DateTime.Parse(DateTime.Parse(obj.ToString().Trim()).ToShortDateString());
                if ((dateRet > DateTime.Parse("1753-01-01 12:00:00"))
                    && (dateRet < DateTime.Parse("9999-01-01 23:59:59")))
                {
                    return dateRet;
                }
            }
            catch
            {
            }
            return dateDefault;
        }
        #endregion

        #region ConvertToBool
        /// <summary>
        /// 转换成BOOL,如果不能转换,出现异常同则返回false
        /// </summary>
        /// <param name="obj">转换对象</param>
        /// <returns>转换结果值</returns>
        public static bool ConvertToBool(Object obj)
        {
            try
            {
                return bool.Parse(obj.ToString());
            }
            catch
            {
            }
            return false;
        }
        #endregion


        #region ConvertToByteArray
        /// <summary>
        /// 转换成Byte[],如果不能转换,出现异常同则返回null
        /// </summary>
        /// <param name="obj">转换对象</param>
        /// <returns>转换结果值</returns>
        public static Byte[] ConvertToByteArray(Object obj)
        {
            try
            {
                if (obj == System.DBNull.Value)
                {
                    return null;
                }
                Byte[] byaRet = new byte[((Byte[])obj).Length];
                ((Byte[])obj).CopyTo(byaRet, 0);
                return byaRet;
            }
            catch
            {
            }
            return null;
        }
        #endregion

        #region MoneyConvertToString
        /// <summary>
        /// 金额变换字符串, 是否显示金额为0
        /// </summary>
        /// <param name="obj">转换对象</param>
        /// <param name="bShowZero">是否显示0</param>
        /// <returns>转换结果值</returns>
        public static string MoneyConvertToString(Object obj, bool bShowZero)
        {
            try
            {
                decimal dMoney = ConvertToDecimal(obj);
                if (dMoney == 0 && bShowZero == false)
                {
                    return "";
                }
                return dMoney.ToString("0.00");
            }
            catch
            {
                return "0.00";
            }
        }
        /// <summary>
        /// 金额变换字符串, 金额为0时不显示
        /// </summary>
        /// <param name="obj">转换对象</param>
        /// <returns>转换结果值</returns>
        public static string MoneyConvertToString(Object obj)
        {
            return MoneyConvertToString(obj, false);
        }
        #endregion

        #region DateConvertToString
        /// <summary>
        /// 日期变换字符串, 是否显示日期为1900-01-01
        /// </summary>
        /// <param name="obj">转换对象</param>
        /// <param name="bShowZero">日期为1900-01-01是否</param>
        /// <returns>转换结果值</returns>
        public static string DateConvertToString(Object obj, bool bShow)
        {
            try
            {
                DateTime dtDate = ConvertToDateTime(obj);
                if (dtDate == DateTime.Parse("1900-01-01 01:01:01") && bShow == false)
                {
                    return "";
                }
                return dtDate.ToString("yyyy-MM-dd");
            }
            catch
            {
                return "1900-01-01";
            }
        }
        #endregion

        #region DateConvertToString
        /// <summary>
        /// 日期时间变换字符串, 日期为1900-01-01时不显示
        /// </summary>
        /// <param name="obj">转换对象</param>
        /// <returns>转换结果值</returns>
        public static string DateConvertToString(Object obj)
        {
            return DateConvertToString(obj, false);
        }
        /// <summary>
        /// 日期时间变换字符串, 日期为1900-01-01时不显示
        /// </summary>
        /// <param name="obj">转换对象</param>
        /// <returns>转换结果值</returns>
        public static string DateTimeConvertToString(Object obj)
        {
            return DateTimeConvertToString(obj, false);
        }
        #endregion

        #region DateTimeConvertToString
        /// <summary>
        /// 日期时间变换字符串, 日期为1900-01-01时不显示 bShowZero为是否显示时间
        /// </summary>
        /// <param name="obj">转换对象</param>
        /// <param name="bShowZero">是否显示时间</param>
        /// <returns>转换结果值</returns>
        public static string DateTimeConvertToString(Object obj, bool bShowTime)
        {
            try
            {
                DateTime dtDateTime = ConvertToDateTime(obj);
                if (dtDateTime == DateTime.Parse("1900-01-01 01:01:01"))
                {
                    return "";
                }
                if (!bShowTime || (dtDateTime.Hour == 0 && dtDateTime.Minute == 0 && dtDateTime.Second == 0))
                {
                    return dtDateTime.ToString("yyyy-MM-dd");
                }
                return dtDateTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
            catch
            {
                return "2007-07-30 12:00:00";
            }
        }
        #endregion

        #region DateTimeConvertToStringShort
        /// <summary>
        /// 日期时间变换字符串, 日期为1900-01-01时不显示 bShowZero为是否显示时间
        /// </summary>
        /// <param name="obj">转换对象</param>
        /// <param name="bShowZero">是否显示时间</param>
        /// <returns>转换结果值</returns>
        public static string DateTimeConvertToStringShort(Object obj)
        {
            try
            {
                DateTime dtDateTime = ConvertToDateTime(obj);
                if (dtDateTime == DateTime.Parse("1900-01-01 01:01:01"))
                {
                    return "";
                }
                return dtDateTime.ToString("MM.dd");
            }
            catch
            {
                return "07.30";
            }
        }
        public static string DateTimeConvertToStringShort(Object obj, bool bShowTime)
        {
            return DateTimeConvertToStringShort(obj);
        }
        #endregion

        #region DateTimeConvertToStringNoSpace
        /// <summary>
        /// 没有空格，以-替换空格和:号
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string DateTimeConvertToStringNoSpace(Object obj)
        {
            try
            {
                DateTime dtDateTime = ConvertToDateTime(obj);
                if (dtDateTime.Hour == 0 && dtDateTime.Minute == 0 && dtDateTime.Second == 0)
                {
                    return dtDateTime.ToString("yyyy-MM-dd-HH-mm");
                }
                return dtDateTime.ToString("yyyy-MM-dd-HH-mm");
            }
            catch
            {
                return DateTime.Now.ToString("yyyy-MM-dd-HH-mm");
            }
        }
        #endregion

        #region DateTimeConvertToStringNoSpaceAll
        /// <summary>
        /// 没有空格
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string DateTimeConvertToStringNoSpaceAll(Object obj)
        {
            try
            {
                DateTime dtDateTime = ConvertToDateTime(obj);
                if (dtDateTime.Hour == 0 && dtDateTime.Minute == 0 && dtDateTime.Second == 0)
                {
                    return dtDateTime.ToString("yyyyMMddHHmmss");
                }
                return dtDateTime.ToString("yyyyMMddHHmmss");
            }
            catch
            {
                return DateTime.Now.ToString("yyyyMMddHHmmss");
            }
        }

        #endregion

        #region DateTimeConvertToStringCN 1900年01月01日
        /// <summary>
        /// 日期时间变换字符串, 是否显示日期为1900-01-01
        /// </summary>
        /// <param name="obj">转换对象</param>
        /// <param name="bShowZero">日期为1900-01-01是否</param>
        /// <returns>转换结果值</returns>
        public static string DateTimeConvertToStringCN(Object obj)
        {
            try
            {
                DateTime dtDateTime = ConvertToDateTime(obj);
                if (dtDateTime == null)
                {
                    dtDateTime = DateTime.Now;
                }
                return dtDateTime.ToString("yyyy年MM月dd日");
            }
            catch
            {
                return DateTime.Now.ToString("yyyy年MM月dd日");
            }
        }
        #endregion

        #region DateTimeConvertToStringCNAndWeek 1900年01月01日 星期一
        /// <summary>
        /// 日期时间变换字符串, 是否显示日期为1900-01-01
        /// </summary>
        /// <param name="obj">转换对象</param>
        /// <param name="bShowZero">日期为1900-01-01是否</param>
        /// <returns>转换结果值</returns>
        public static string DateTimeConvertToStringCNAndWeek(Object obj)
        {
            try
            {
                DateTime dtDateTime = ConvertToDateTime(obj, DateTime.Now);
                return dtDateTime.ToString("yyyy年MM月dd日 ") + dtDateTime.DayOfWeek.ToString();
            }
            catch
            {
                DateTime dt = DateTime.Now;
                return dt.ToString("yyyy年MM月dd日 ") + dt.DayOfWeek.ToString();
            }
        }
        #endregion
    }
}

