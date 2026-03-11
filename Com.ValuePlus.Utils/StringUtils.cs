using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;
using System.Drawing;
namespace Com.ValuePlus.Utils
{
    /// <summary>
    /// 字符通用操作类
    /// </summary>
    public class StringUtils
    {
        private static Regex RegexBr = new Regex(@"(\r\n)", RegexOptions.IgnoreCase);

        #region 生成随机数
        /// <summary>
        /// 生成随机数
        /// </summary>
        /// <param name="n">随机数位数</param>
        /// <returns>随机数</returns>
        public static string RandomNum(int n) //
        {
            string strchar = "0,1,2,3,4,5,6,7,8,9";
            string[] VcArray = strchar.Split(',');
            string VNum = "";
            int temp = -1;   //记录上次随机数值，尽量避免产生几个一样的随机数
            //采用一个简单的算法以保证生成随机数的不同
            Random rand = new Random();
            for (int i = 1; i < n + 1; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(i * temp * unchecked((int)
                        DateTime.Now.Ticks));
                }
                //int t = rand.Next(35) ;
                int t = rand.Next(10);
                if (temp != -1 && temp == t)
                {
                    return RandomNum(n);
                }
                temp = t;
                VNum += VcArray[t];
            }
            return VNum;//返回生成的随机数
        }
        #endregion

        #region 手机号码
        /// <summary>
        /// 移动手机号码
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public static bool IsMobileNumber(string mobile)
        {
            if (Regex.IsMatch(mobile, @"^8613[4-9]\d{8}$|^8613[4-9]\d{8}$|^86159\d{8}$|^86159\d{8}$"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region 返回字符串真实长度, 1个汉字长度为2
        /// <summary>
        /// 返回字符串真实长度, 1个汉字长度为2
        /// </summary>
        /// <returns></returns>
        public static int GetStringLength(string str)
        {
            return Encoding.Default.GetBytes(str).Length;
        }
        #endregion

        #region 删除字符串尾部的回车/换行/空格
        /// <summary>
        /// 删除字符串尾部的回车/换行/空格
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RTrim(string str)
        {
            for (int i = str.Length; i >= 0; i--)
            {
                if (str[i].Equals(" ") || str[i].Equals("\r") || str[i].Equals("\n"))
                {
                    str.Remove(i, 1);
                }
            }
            return str;
        }
        #endregion

        #region 将全角数字转换为数字
        /// <summary>
        /// 将全角数字转换为数字
        /// </summary>
        /// <param name="SBCCase"></param>
        /// <returns></returns>
        public static string SBCCaseToNumberic(string SBCCase)
        {
            char[] c = SBCCase.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                byte[] b = System.Text.Encoding.Unicode.GetBytes(c, i, 1);
                if (b.Length == 2)
                {
                    if (b[1] == 255)
                    {
                        b[0] = (byte)(b[0] + 32);
                        b[1] = 0;
                        c[i] = System.Text.Encoding.Unicode.GetChars(b)[0];
                    }
                }
            }
            return new string(c);
        }
        #endregion

        #region 将字符串转换为Color
        /// <summary>
        /// 将字符串转换为Color
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Color ToColor(string color)
        {
            int red, green, blue = 0;
            char[] rgb;
            color = color.TrimStart('#');
            color = Regex.Replace(color.ToLower(), "[g-zG-Z]", "");
            switch (color.Length)
            {
                case 3:
                    rgb = color.ToCharArray();
                    red = Convert.ToInt32(rgb[0].ToString() + rgb[0].ToString(), 16);
                    green = Convert.ToInt32(rgb[1].ToString() + rgb[1].ToString(), 16);
                    blue = Convert.ToInt32(rgb[2].ToString() + rgb[2].ToString(), 16);
                    return Color.FromArgb(red, green, blue);
                case 6:
                    rgb = color.ToCharArray();
                    red = Convert.ToInt32(rgb[0].ToString() + rgb[1].ToString(), 16);
                    green = Convert.ToInt32(rgb[2].ToString() + rgb[3].ToString(), 16);
                    blue = Convert.ToInt32(rgb[4].ToString() + rgb[5].ToString(), 16);
                    return Color.FromArgb(red, green, blue);
                default:
                    return Color.FromName(color);

            }
        }
        #endregion

        #region 清除给定字符串中的回车及换行符
        /// <summary>
        /// 清除给定字符串中的回车及换行符
        /// </summary>
        /// <param name="str">要清除的字符串</param>
        /// <returns>清除后返回的字符串</returns>
        public static string ClearBR(string str)
        {
            //Regex r = null;
            Match m = null;

            //r = new Regex(@"(\r\n)",RegexOptions.IgnoreCase);
            for (m = RegexBr.Match(str); m.Success; m = m.NextMatch())
            {
                str = str.Replace(m.Groups[0].ToString(), "");
            }


            return str;
        }
        #endregion

        #region 从字符串的指定位置截取指定长度的子字符串
        /// <summary>
        /// 从字符串的指定位置截取指定长度的子字符串
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <param name="startIndex">子字符串的起始位置</param>
        /// <param name="length">子字符串的长度</param>
        /// <returns>子字符串</returns>
        public static string CutString(string str, int startIndex, int length)
        {
            if (String.IsNullOrEmpty(str)) return str;
            if (startIndex >= 0)
            {
                if (length < 0)
                {
                    length = length * -1;
                    if (startIndex - length < 0)
                    {
                        length = startIndex;
                        startIndex = 0;
                    }
                    else
                    {
                        startIndex = startIndex - length;
                    }
                }


                if (startIndex > str.Length)
                {
                    return "";
                }


            }
            else
            {
                if (length < 0)
                {
                    return "";
                }
                else
                {
                    if (length + startIndex > 0)
                    {
                        length = length + startIndex;
                        startIndex = 0;
                    }
                    else
                    {
                        return "";
                    }
                }
            }

            if (str.Length - startIndex < length)
            {
                length = str.Length - startIndex;
            }

            return str.Substring(startIndex, length);
        }
        #endregion

        #region 从字符串的指定位置开始截取到字符串结尾的了符串
        /// <summary>
        /// 从字符串的指定位置开始截取到字符串结尾的了符串
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <param name="startIndex">子字符串的起始位置</param>
        /// <returns>子字符串</returns>
        public static string CutString(string str, int startIndex)
        {
            return CutString(str, startIndex, str.Length);
        }
        #endregion

        #region 分割字符串
        /// <summary>
        /// 分割字符串
        /// </summary>
        public static string[] SplitString(string strContent, string strSplit)
        {
            if (strContent.IndexOf(strSplit) < 0)
            {
                string[] tmp = { strContent };
                return tmp;
            }
            return Regex.Split(strContent, Regex.Escape(strSplit), RegexOptions.IgnoreCase);
        }
        #endregion

        #region  进行指定的替换(脏字过滤)
        /// <summary>
        /// 进行指定的替换(脏字过滤)
        /// </summary>
        public static string StrFilter(string str, string bantext)
        {
            string text1 = "";
            string text2 = "";
            string[] textArray1 = SplitString(bantext, "\r\n");
            for (int num1 = 0; num1 < textArray1.Length; num1++)
            {
                text1 = textArray1[num1].Substring(0, textArray1[num1].IndexOf("="));
                text2 = textArray1[num1].Substring(textArray1[num1].IndexOf("=") + 1);
                str = str.Replace(text1, text2);
            }
            return str;
        }
        #endregion

        #region 字符串如果操过指定长度则将超出的部分用指定字符串代替
        /// <summary>
        /// 字符串如果操过指定长度则将超出的部分用指定字符串代替
        /// </summary>
        /// <param name="p_SrcString">要检查的字符串</param>
        /// <param name="p_Length">指定长度</param>
        /// <param name="p_TailString">用于替换的字符串</param>
        /// <returns>截取后的字符串</returns>
        public static string GetSubString(string p_SrcString, int p_Length, string p_TailString)
        {
            return GetSubString(p_SrcString, 0, p_Length, p_TailString);
        }
        #endregion

        #region 取指定长度的字符串
        /// <summary>
        /// 取指定长度的字符串
        /// </summary>
        /// <param name="p_SrcString">要检查的字符串</param>
        /// <param name="p_StartIndex">起始位置</param>
        /// <param name="p_Length">指定长度</param>
        /// <param name="p_TailString">用于替换的字符串</param>
        /// <returns>截取后的字符串</returns>
        public static string GetSubString(string p_SrcString, int p_StartIndex, int p_Length, string p_TailString)
        {


            string myResult = p_SrcString;

            //当是日文或韩文时(注:中文的范围:\u4e00 - \u9fa5, 日文在\u0800 - \u4e00, 韩文为\xAC00-\xD7A3)
            if (System.Text.RegularExpressions.Regex.IsMatch(p_SrcString, "[\u0800-\u4e00]+") ||
                System.Text.RegularExpressions.Regex.IsMatch(p_SrcString, "[\xAC00-\xD7A3]+"))
            {
                //当截取的起始位置超出字段串长度时
                if (p_StartIndex >= p_SrcString.Length)
                {
                    return "";
                }
                else
                {
                    return p_SrcString.Substring(p_StartIndex,
                                                   ((p_Length + p_StartIndex) > p_SrcString.Length) ? (p_SrcString.Length - p_StartIndex) : p_Length);
                }
            }


            if (p_Length >= 0)
            {
                byte[] bsSrcString = Encoding.Default.GetBytes(p_SrcString);

                //当字符串长度大于起始位置
                if (bsSrcString.Length > p_StartIndex)
                {
                    int p_EndIndex = bsSrcString.Length;

                    //当要截取的长度在字符串的有效长度范围内
                    if (bsSrcString.Length > (p_StartIndex + p_Length))
                    {
                        p_EndIndex = p_Length + p_StartIndex;
                    }
                    else
                    {   //当不在有效范围内时,只取到字符串的结尾

                        p_Length = bsSrcString.Length - p_StartIndex;
                        p_TailString = "";
                    }



                    int nRealLength = p_Length;
                    int[] anResultFlag = new int[p_Length];
                    byte[] bsResult = null;

                    int nFlag = 0;
                    for (int i = p_StartIndex; i < p_EndIndex; i++)
                    {

                        if (bsSrcString[i] > 127)
                        {
                            nFlag++;
                            if (nFlag == 3)
                            {
                                nFlag = 1;
                            }
                        }
                        else
                        {
                            nFlag = 0;
                        }

                        anResultFlag[i] = nFlag;
                    }

                    if ((bsSrcString[p_EndIndex - 1] > 127) && (anResultFlag[p_Length - 1] == 1))
                    {
                        nRealLength = p_Length + 1;
                    }

                    bsResult = new byte[nRealLength];

                    Array.Copy(bsSrcString, p_StartIndex, bsResult, 0, nRealLength);

                    myResult = Encoding.Default.GetString(bsResult);

                    myResult = myResult + p_TailString;
                }
            }

            return myResult;
        }
        #endregion

        #region 自定义的替换字符串函数
        /// <summary>
        /// 自定义的替换字符串函数
        /// </summary>
        /// <param name="SourceString"></param>
        /// <param name="SearchString"></param>
        /// <param name="ReplaceString"></param>
        /// <param name="IsCaseInsensetive"></param>
        /// <returns></returns>
        public static string ReplaceString(string SourceString, string SearchString, string ReplaceString, bool IsCaseInsensetive)
        {
            return Regex.Replace(SourceString, Regex.Escape(SearchString), ReplaceString, IsCaseInsensetive ? RegexOptions.IgnoreCase : RegexOptions.None);
        }
        #endregion

        #region 判断是否为base64字符串
        /// <summary>
        /// 判断是否为base64字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsBase64String(string str)
        {
            //A-Z, a-z, 0-9, +, /, =
            return Regex.IsMatch(str, @"[A-Za-z0-9\+\/\=]");
        }
        #endregion

        #region 检测是否有Sql危险字符
        /// <summary>
        /// 检测是否有Sql危险字符
        /// </summary>
        /// <param name="str">要判断字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsSafeSqlString(string str)
        {

            return !Regex.IsMatch(str, @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']");
        }
        #endregion

        #region 是否输入的信息中包含了关键字符,存在为真，不存在为假
        public static bool CheckSqlKey(string req)
        {
            string sKeySql;
            string sGetReq;
            string[] aKeySql;
            bool bBoolean;
            bBoolean = false;
            sGetReq = req;
            sKeySql = "'|;|and|exec|insert|select|delete|update|count|*|%|chr|mid|master|truncate|char|declare| or|or ";
            aKeySql = sKeySql.Split('|');
            if (sGetReq != "")
            {
                for (int i = 0; i < aKeySql.Length; i++)
                {
                    if (sGetReq.IndexOf(aKeySql[i]) >= 0)
                    {
                        bBoolean = true;
                        break;
                    }
                }
            }
            return bBoolean;
        }
        #endregion

        #region 检测是否有危险的可能用于链接的字符串
        /// <summary>
        /// 检测是否有危险的可能用于链接的字符串
        /// </summary>
        /// <param name="str">要判断字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsSafeUserInfoString(string str)
        {
            return !Regex.IsMatch(str, @"^\s*$|^c:\\con\\con$|[%,\*" + "\"" + @"\s\t\<\>\&]|游客|^Guest");
        }
        #endregion

        #region 清理字符串
        /// <summary>
        /// 清理字符串
        /// </summary>
        /// <param name="strIn"></param>
        /// <returns></returns>
        public static string CleanInput(string strIn)
        {
            return Regex.Replace(strIn.Trim(), @"[^\w\.@-]", "");
        }
        #endregion

        #region 删除最后一个字符
        /// <summary>
        /// 删除最后一个字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ClearLastChar(string str)
        {
            if (str == "")
                return "";
            else
                return str.Substring(0, str.Length - 1);
        }
        #endregion

        #region FsRandom 随机数
        /// <summary>
        /// 
        /// </summary>
        /// <param name="MaxInt">最大值，包含</param>
        /// <returns></returns>
        public static int FsRandom(int MaxInt)
        {
            Random rnd = new Random(unchecked((int)DateTime.Now.Ticks) + FsRandomS(100000));
            return rnd.Next(0, MaxInt);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="MinInt">最小值，包含</param>
        /// <param name="MaxInt">最大值，包含</param>
        /// <returns></returns>
        public static int FsRandom(int MinInt, int MaxInt)
        {
            Random rnd = new Random(unchecked((int)DateTime.Now.Ticks) + FsRandomS(100000));
            return rnd.Next(MinInt, MaxInt);
        }

        private static int FsRandomS(int MaxInt)
        {
            Random rnd = new Random(new System.Globalization.GregorianCalendar().GetHashCode());
            return rnd.Next(0, MaxInt);
        }

        #endregion

        #region RandomString
        private const string FS_RANDOM_STRING = "123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ123456789";
        /// <summary>
        /// 得到长度为length的随机字符串
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string FsRandomString(int length)
        {
            try
            {
                string res = "";
                for (int i = 0; i < length; i++)
                {
                    int s1 = FsRandom(0, 44);
                    res += FS_RANDOM_STRING.Substring(s1, 1);
                }
                return res;
            }
            catch
            {
                return "";
            }
        }
        #endregion

        #region 得到文件的扩展名
        /// <summary>
        /// 得到文件的扩展名
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetExpendName(string fileName)
        {
            string strExp = string.Empty;

            int idx = fileName.LastIndexOf(".");
            if (idx != -1)
            {
                strExp = fileName.Substring(idx + 1);
            }
            return strExp;
        }
        #endregion

        #region 生成文件名，不包括扩展名
        /// <summary>
        /// 生成文件名，不包括扩展名
        /// </summary>
        /// <returns></returns>
        public static string GenerateName()
        {
            //文件名生成逻辑是按照时间来生成
            DateTime dtTime = DateTime.Now;
            string strName = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}",
                                            StringUtils.RandomNum(4).ToString(), dtTime.Year.ToString(),
                                            dtTime.Month.ToString().PadLeft(2, '0'),
                                            dtTime.Day.ToString().PadLeft(2, '0'),
                                            dtTime.Hour.ToString().PadLeft(2, '0'),
                                            dtTime.Minute.ToString().PadLeft(2, '0'),
                                            dtTime.Second.ToString().PadLeft(2, '0'),
                                            dtTime.Millisecond.ToString().PadLeft(3, '0'), StringUtils.RandomNum(3).ToString());

            return strName;
        }
        #endregion

        #region 取指定长度的字符串
        /// <summary>
        /// 取指定长度的字符串
        /// </summary>
        /// <param name="p_SrcString">要检查的字符串</param>
        /// <param name="p_Length">指定长度</param>
        /// <param name="p_TailString">用于替换的字符串</param>
        /// <returns>截取后的字符串</returns>
        public static string CutString(string p_SrcString, int p_Length, string p_TailString)
        {


            string myResult = string.Empty;
            int mylength = 0;
            if (string.IsNullOrEmpty(p_SrcString)) return p_SrcString;
            //当是日文或韩文时(注:中文的范围:\u4e00 - \u9fa5, 日文在\u0800 - \u4e00, 韩文为\xAC00-\xD7A3)
            for (int i = 0; i < p_SrcString.Length; i++)
            {
                if (mylength >= p_Length) break;
                string mytemp = p_SrcString.Substring(i, 1);
                if (System.Text.RegularExpressions.Regex.IsMatch(mytemp, "[\u4e00-\u9fa5]+") || System.Text.RegularExpressions.Regex.IsMatch(mytemp, "[\u0800-\u4e00]+") ||
                System.Text.RegularExpressions.Regex.IsMatch(mytemp, "[\xAC00-\xD7A3]+"))
                {
                    myResult += mytemp;
                    mylength += 2;
                }
                else
                {
                    myResult += mytemp;
                    mylength += 1;
                }
            }

            myResult += p_TailString;

            return myResult;
        }
        #endregion


        #region 替换字符串中包含的xml的关键字，防止解析出错
        /// <summary>
        /// 替换字符串中包含的xml的关键字，防止解析出错
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ReplaceKeyStringToXml(string str)
        {
            if(!string.IsNullOrEmpty(str))
            {
                str = str.Replace("&", "&amp;");
                str = str.Replace("<", "&lt;");
                str = str.Replace(">", "&gt;");
                str = str.Replace("'", "&apos;");
                str = str.Replace("\"", "&quot;");
            }
            return str;
        }
        #endregion

        #region 判断是否为正整数
        /// <summary>
        /// 判断是否为正整数
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns>是正整数返回true,不是返回false</returns>
        public static bool isPositiveInt(string strValue)
        {

            Regex regex = new Regex("^[0-9]*[1-9][0-9]*$");
            return regex.IsMatch(strValue.Trim()); 

        }
        #endregion


    }
}
