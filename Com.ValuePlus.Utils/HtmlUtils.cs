using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.IO;
using Microsoft.VisualBasic;
using System.Web.UI;
using System.Configuration;
using System.Web.Configuration;
using System.Reflection;
namespace Com.ValuePlus.Utils
{
    /// <summary>
    /// html通用操作类
    /// </summary>
    public class HtmlUtils
    {
        public static Regex RegexFont = new Regex(@"<font color=" + "\".*?\"" + @">([\s\S]+?)</font>", RegexOptions.None);

        #region 从HTML中获取文本,保留br,p,img
        /// <summary>
        /// 从HTML中获取文本,保留br,p,img
        /// </summary>
        /// <param name="HTML"></param>
        /// <returns></returns>
        public static string GetTextFromHTML(string HTML)
        {
            System.Text.RegularExpressions.Regex regEx = new System.Text.RegularExpressions.Regex(@"</?(?!br|/?p|img)[^>]*>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            return regEx.Replace(HTML, "");
        }
        #endregion

        #region 替换回车换行符为html换行符
        /// <summary>
        /// 替换回车换行符为html换行符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string StrFormat(string str)
        {
            string str2;

            if (str == null)
            {
                str2 = "";
            }
            else
            {
                str = str.Replace("\r\n", "<br />");
                str = str.Replace("\n", "<br />");
                str2 = str;
            }
            return str2;
        }
        #endregion

        #region 生成指定数量的html空格符号
        /// <summary>
        /// 生成指定数量的html空格符号
        /// </summary>
        /// <param name="nSpaces"></param>
        /// <returns></returns>
        public static string Spaces(int nSpaces)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < nSpaces; i++)
            {
                sb.Append(" &nbsp;&nbsp;");
            }
            return sb.ToString();
        }
        #endregion

        #region 检测是否符合email格式
        /// <summary>
        /// 检测是否符合email格式
        /// </summary>
        /// <param name="strEmail">要判断的email字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsValidEmail(string strEmail)
        {
            return Regex.IsMatch(strEmail, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }
        #endregion

        #region 检测是否符合email后缀格式
        /// <summary>
        /// 检测是否符合email后缀格式
        /// </summary>
        /// <param name="strEmail"></param>
        /// <returns></returns>
        public static bool IsValidDoEmail(string strEmail)
        {
            return Regex.IsMatch(strEmail, @"^@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }
        #endregion

        #region 检测是否是正确的Url
        /// <summary>
        /// 检测是否是正确的Url
        /// </summary>
        /// <param name="strUrl">要验证的Url</param>
        /// <returns>判断结果</returns>
        public static bool IsURL(string strUrl)
        {
            return Regex.IsMatch(strUrl, @"^(http|https)\://([a-zA-Z0-9\.\-]+(\:[a-zA-Z0-9\.&%\$\-]+)*@)*((25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])|localhost|([a-zA-Z0-9\-]+\.)*[a-zA-Z0-9\-]+\.(com|edu|gov|int|mil|net|org|biz|arpa|info|name|pro|aero|coop|museum|[a-zA-Z]{1,10}))(\:[0-9]+)*(/($|[a-zA-Z0-9\.\,\?\'\\\+&%\$#\=~_\-]+))*$");
        }
        #endregion

        #region 获得email的后缀
        /// <summary>
        /// 获得email的后缀
        /// </summary>
        /// <param name="strEmail"></param>
        /// <returns></returns>
        public static string GetEmailHostName(string strEmail)
        {
            if (strEmail.IndexOf("@") < 0)
            {
                return "";
            }
            return strEmail.Substring(strEmail.LastIndexOf("@")).ToLower();
        }
        #endregion

        #region 返回URL中结尾的文件名
        /// <summary>
        /// 返回URL中结尾的文件名
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>		
        public static string GetFilename(string url)
        {
            if (url == null)
            {
                return "";
            }
            string[] strs1 = url.Split(new char[] { '/' });
            return strs1[strs1.Length - 1].Split(new char[] { '?' })[0];
        }
        #endregion

        #region 改正sql语句中的转义字符
        /// <summary>
        /// 改正sql语句中的转义字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string mashSQL(string str)
        {
            string str2;

            if (str == null)
            {
                str2 = "";
            }
            else
            {
                str = str.Replace("\'", "'");
                str2 = str;
            }
            return str2;
        }
        #endregion

        #region 替换sql语句中的有问题符号
        /// <summary>
        /// 替换sql语句中的有问题符号
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ChkSQL(string str)
        {
            string str2;

            if (str == null)
            {
                str2 = "";
            }
            else
            {
                str = str.Replace("'", "''");
                str2 = str;
            }
            return str2;
        }
        #endregion

        #region 转换为静态html
        /// <summary>
        /// 转换为静态html
        /// </summary>
        /// <param name="path"></param>
        /// <param name="outpath"></param>
        public void transHtml(string path, string outpath)
        {
            Page page = new Page();
            StringWriter writer = new StringWriter();
            page.Server.Execute(path, writer);
            FileStream fs;
            if (File.Exists(page.Server.MapPath("") + "\\" + outpath))
            {
                File.Delete(page.Server.MapPath("") + "\\" + outpath);
                fs = File.Create(page.Server.MapPath("") + "\\" + outpath);
            }
            else
            {
                fs = File.Create(page.Server.MapPath("") + "\\" + outpath);
            }
            byte[] bt = Encoding.Default.GetBytes(writer.ToString());
            fs.Write(bt, 0, bt.Length);
            fs.Close();
        }
        #endregion

        #region 转换为简体中文
        /// <summary>
        /// 转换为简体中文
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToSChinese(string str)
        {
            return Strings.StrConv(str, VbStrConv.SimplifiedChinese, 0);
        }
        #endregion

        #region 转换为繁体中文
        /// <summary>
        /// 转换为繁体中文
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToTChinese(string str)
        {
            return Strings.StrConv(str, VbStrConv.TraditionalChinese, 0);
        }
        #endregion

        #region 替换html字符
        /// <summary>
        /// 替换html字符
        /// </summary>
        /// <param name="strHtml"></param>
        /// <returns></returns>
        public static string EncodeHtml(string strHtml)
        {
            if (strHtml != "")
            {
                strHtml = strHtml.Replace(",", "&def");
                strHtml = strHtml.Replace("'", "&dot");
                strHtml = strHtml.Replace(";", "&dec");
                return strHtml;
            }
            return "";
        }
        #endregion

        #region 获得伪静态页码显示链接
        /// <summary>
        /// 获得伪静态页码显示链接
        /// </summary>
        /// <param name="curPage">当前页数</param>
        /// <param name="countPage">总页数</param>
        /// <param name="url">超级链接地址</param>
        /// <param name="extendPage">周边页码显示个数上限</param>
        /// <returns>页码html</returns>
        public static string GetStaticPageNumbers(int curPage, int countPage, string url, string expname, int extendPage)
        {
            int startPage = 1;
            int endPage = 1;

            string t1 = "<a href=\"" + url + "-1" + expname + "\">&laquo;</a>";
            string t2 = "<a href=\"" + url + "-" + countPage + expname + "\">&raquo;</a>";

            if (countPage < 1) countPage = 1;
            if (extendPage < 3) extendPage = 2;

            if (countPage > extendPage)
            {
                if (curPage - (extendPage / 2) > 0)
                {
                    if (curPage + (extendPage / 2) < countPage)
                    {
                        startPage = curPage - (extendPage / 2);
                        endPage = startPage + extendPage - 1;
                    }
                    else
                    {
                        endPage = countPage;
                        startPage = endPage - extendPage + 1;
                        t2 = "";
                    }
                }
                else
                {
                    endPage = extendPage;
                    t1 = "";
                }
            }
            else
            {
                startPage = 1;
                endPage = countPage;
                t1 = "";
                t2 = "";
            }

            StringBuilder s = new StringBuilder("");

            s.Append(t1);
            for (int i = startPage; i <= endPage; i++)
            {
                if (i == curPage)
                {
                    s.Append("<span>");
                    s.Append(i);
                    s.Append("</span>");
                }
                else
                {
                    s.Append("<a href=\"");
                    s.Append(url);
                    s.Append("-");
                    s.Append(i);
                    s.Append(expname);
                    s.Append("\">");
                    s.Append(i);
                    s.Append("</a>");
                }
            }
            s.Append(t2);

            return s.ToString();
        }
        #endregion

        #region 获得帖子的伪静态页码显示链接
        /// <summary>
        /// 获得帖子的伪静态页码显示链接
        /// </summary>
        /// <param name="expname"></param>
        /// <param name="countPage">总页数</param>
        /// <param name="url">超级链接地址</param>
        /// <param name="extendPage">周边页码显示个数上限</param>
        /// <returns>页码html</returns>
        public static string GetPostPageNumbers(int countPage, string url, string expname, int extendPage)
        {
            int startPage = 1;
            int endPage = 1;
            int curPage = 1;

            string t1 = "<a href=\"" + url + "-1" + expname + "\">&laquo;</a>";
            string t2 = "<a href=\"" + url + "-" + countPage + expname + "\">&raquo;</a>";

            if (countPage < 1) countPage = 1;
            if (extendPage < 3) extendPage = 2;

            if (countPage > extendPage)
            {
                if (curPage - (extendPage / 2) > 0)
                {
                    if (curPage + (extendPage / 2) < countPage)
                    {
                        startPage = curPage - (extendPage / 2);
                        endPage = startPage + extendPage - 1;
                    }
                    else
                    {
                        endPage = countPage;
                        startPage = endPage - extendPage + 1;
                        t2 = "";
                    }
                }
                else
                {
                    endPage = extendPage;
                    t1 = "";
                }
            }
            else
            {
                startPage = 1;
                endPage = countPage;
                t1 = "";
                t2 = "";
            }

            StringBuilder s = new StringBuilder("");

            s.Append(t1);
            for (int i = startPage; i <= endPage; i++)
            {
                s.Append("<a href=\"");
                s.Append(url);
                s.Append("-");
                s.Append(i);
                s.Append(expname);
                s.Append("\">");
                s.Append(i);
                s.Append("</a>");
            }
            s.Append(t2);

            return s.ToString();
        }
        #endregion

        #region 获得页码显示链接
        /// <summary>
        /// 获得页码显示链接
        /// </summary>
        /// <param name="curPage">当前页数</param>
        /// <param name="countPage">总页数</param>
        /// <param name="url">超级链接地址</param>
        /// <param name="extendPage">周边页码显示个数上限</param>
        /// <returns>页码html</returns>
        public static string GetPageNumbers(int curPage, int countPage, string url, int extendPage)
        {
            return GetPageNumbers(curPage, countPage, url, extendPage, "page");
        }
        #endregion

        #region 获得页码显示链接
        /// <summary>
        /// 获得页码显示链接
        /// </summary>
        /// <param name="curPage">当前页数</param>
        /// <param name="countPage">总页数</param>
        /// <param name="url">超级链接地址</param>
        /// <param name="extendPage">周边页码显示个数上限</param>
        /// <param name="pagetag">页码标记</param>
        /// <returns>页码html</returns>
        public static string GetPageNumbers(int curPage, int countPage, string url, int extendPage, string pagetag)
        {
            return GetPageNumbers(curPage, countPage, url, extendPage, pagetag, null);
        }
        #endregion

        #region 获得页码显示链接
        /// <summary>
        /// 获得页码显示链接
        /// </summary>
        /// <param name="curPage">当前页数</param>
        /// <param name="countPage">总页数</param>
        /// <param name="url">超级链接地址</param>
        /// <param name="extendPage">周边页码显示个数上限</param>
        /// <param name="pagetag">页码标记</param>
        /// <param name="anchor">锚点</param>
        /// <returns>页码html</returns>
        public static string GetPageNumbers(int curPage, int countPage, string url, int extendPage, string pagetag, string anchor)
        {
            if (pagetag == "")
                pagetag = "page";
            int startPage = 1;
            int endPage = 1;

            if (url.IndexOf("?") > 0)
            {
                url = url + "&";
            }
            else
            {
                url = url + "?";
            }

            string t1 = "<a href=\"" + url + "&" + pagetag + "=1";
            string t2 = "<a href=\"" + url + "&" + pagetag + "=" + countPage;
            if (anchor != null)
            {
                t1 += anchor;
                t2 += anchor;
            }
            t1 += "\">&laquo;</a>";
            t2 += "\">&raquo;</a>";

            if (countPage < 1)
                countPage = 1;
            if (extendPage < 3)
                extendPage = 2;

            if (countPage > extendPage)
            {
                if (curPage - (extendPage / 2) > 0)
                {
                    if (curPage + (extendPage / 2) < countPage)
                    {
                        startPage = curPage - (extendPage / 2);
                        endPage = startPage + extendPage - 1;
                    }
                    else
                    {
                        endPage = countPage;
                        startPage = endPage - extendPage + 1;
                        t2 = "";
                    }
                }
                else
                {
                    endPage = extendPage;
                    t1 = "";
                }
            }
            else
            {
                startPage = 1;
                endPage = countPage;
                t1 = "";
                t2 = "";
            }

            StringBuilder s = new StringBuilder("");

            s.Append(t1);
            for (int i = startPage; i <= endPage; i++)
            {
                if (i == curPage)
                {
                    s.Append("<span>");
                    s.Append(i);
                    s.Append("</span>");
                }
                else
                {
                    s.Append("<a href=\"");
                    s.Append(url);
                    s.Append(pagetag);
                    s.Append("=");
                    s.Append(i);
                    if (anchor != null)
                    {
                        s.Append(anchor);
                    }
                    s.Append("\">");
                    s.Append(i);
                    s.Append("</a>");
                }
            }
            s.Append(t2);

            return s.ToString();
        }
        #endregion

        #region 返回 HTML 字符串的编码结果
        /// <summary>
        /// 返回 HTML 字符串的编码结果
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>编码结果</returns>
        public static string HtmlEncode(string str)
        {
            return HttpUtility.HtmlEncode(str);
        }
        #endregion

        #region 返回 HTML 字符串的解码结果
        /// <summary>
        /// 返回 HTML 字符串的解码结果
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>解码结果</returns>
        public static string HtmlDecode(string str)
        {
            return HttpUtility.HtmlDecode(str);
        }
        #endregion

        #region 返回 URL 字符串的编码结果
        /// <summary>
        /// 返回 URL 字符串的编码结果
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>编码结果</returns>
        public static string UrlEncode(string str)
        {
            return HttpUtility.UrlEncode(str);
        }
        #endregion

        #region 返回 URL 字符串的编码结果
        /// <summary>
        /// 返回 URL 字符串的编码结果
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>解码结果</returns>
        public static string UrlDecode(string str)
        {
            return HttpUtility.UrlDecode(str);
        }
        #endregion

        #region 为脚本替换特殊字符串
        /// <summary>
        /// 为脚本替换特殊字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ReplaceStrToScript(string str)
        {
            str = str.Replace("\\", "\\\\");
            str = str.Replace("'", "\\'");
            str = str.Replace("\"", "\\\"");
            return str;
        }
        #endregion

        #region 得到真实路径
        /// <summary>
        /// 得到真实路径
        /// </summary>
        /// <returns></returns>
        public static string GetTruePath()
        {
            string forumPath = HttpContext.Current.Request.Path;
            if (forumPath.LastIndexOf("/") != forumPath.IndexOf("/"))
            {
                forumPath = forumPath.Substring(forumPath.IndexOf("/"), forumPath.LastIndexOf("/") + 1);
            }
            else
            {
                forumPath = "/";
            }
            return forumPath;

        }
        #endregion

        #region 移除Html标记
        /// <summary>
        /// 移除Html标记
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string RemoveHtml(string content)
        {
            string regexstr = @"<[^>]*>";
            return Regex.Replace(content, regexstr, string.Empty, RegexOptions.IgnoreCase);
        }
        #endregion

        #region 过滤HTML中的不安全标签
        /// <summary>
        /// 过滤HTML中的不安全标签
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string RemoveUnsafeHtml(string content)
        {
            content = Regex.Replace(content, @"(\<|\s+)o([a-z]+\s?=)", "$1$2", RegexOptions.IgnoreCase);
            content = Regex.Replace(content, @"(script|frame|form|meta|behavior|style)([\s|:|>])+", "$1.$2", RegexOptions.IgnoreCase);
            return content;
        }
        #endregion

        #region 将用户组Title中的font标签去掉
        /// <summary>
        /// 将用户组Title中的font标签去掉
        /// </summary>
        /// <param name="title">用户组Title</param>
        /// <returns></returns>
        public static string RemoveFontTag(string title)
        {
            Match m = RegexFont.Match(title);
            if (m.Success)
            {
                return m.Groups[1].Value;
            }
            return title;
        }
        #endregion

        #region Replace(string str, int start, int end, string replaceto)
        /// <summary>
        /// aaaabbbbbcccc
        /// 换成 aaaazzzzzcccc
        /// Replace("aaaabbbbbcccc",4,9,"zzzzz")
        /// </summary>
        /// <param name="str">要替换的字符</param>
        /// <param name="start">开始处</param>
        /// <param name="end">结束处</param>
        /// <param name="replaceto">替换的字符</param>
        /// <returns></returns>
        public static string Replace(string str, int start, int end, string replaceto)
        {
            if (str == null)
            {
                return "";
            }
            try
            {
                string a1 = str.Substring(0, start);
                string a2 = str.Substring(end);
                return a1 + replaceto + a2;
            }
            catch
            {
                return str;
            }
        }
        #endregion

        #region Substring(string str,int start,int end)
        /// <summary>
        /// 取得子字符串
        /// </summary>
        /// <param name="str">字符</param>
        /// <param name="start">开始</param>
        /// <param name="end">结束</param>
        /// <returns></returns>
        public static string Substring(string str, int start)
        {
            return Substring(str, start, 0);
        }
        public static string Substring(string str, int start, int end)
        {
            try
            {
                if (end >= start)
                {
                    return str.Substring(start, end - start);
                }
                else
                {
                    return str.Substring(start);
                }
            }
            catch
            {
                return str;
            }
        }
        #endregion

        #region string ContextEditHtml(string str)
        /// <summary>
        /// 转换成编辑器用的代码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ContextEditHtml(string str)
        {
            string s = str.Replace("\"", "\\\"");
            //s = StrTools.replace(s, "<", "&lt");
            //s = StrTools.replace(s, ">", "&gt");
            s = s.Replace("'", "&acute;");
            s = s.Replace("\n", "\\n");
            s = s.Replace("\r", "\\r");
            return s;
        }
        #endregion

        #region string ContextEditHtmlUbb(string str)
        /// <summary>
        /// 将含UBB的代码转换成编辑器用的代码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ContextEditHtmlUbb(string str)
        {
            try
            {
                string s = str.Replace("\"", "\\\"");
                //s = s.Replace("<", "&lt");
                //s = s.Replace(">", "&gt");
                s = s.Replace("'", "&acute;");
                s = s.Replace("\n", "\\n");
                s = s.Replace("\r", "\\r");
                return s;
            }
            catch
            {
                return str;
            }
        }
        #endregion

        #region string ContextNoHtml(string str)
        /// <summary>
        /// 去掉HTML标识
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ContextNoHtml(string str)
        {
            try
            {
                string s = str.Replace("<", "&lt;");
                s = s.Replace(">", "&gt;");
                s = s.Replace("  ", " &nbsp;");
                //s = s.Replace("\"", "\\\"");
                //s = s.Replace("\r\n", "<br>");
                s = s.Replace("\r", "<br>");
                return s;
            }
            catch
            {
                return str;
            }
        }
        #endregion

        #region string ContextUbb2Html(string str)
        /// <summary>
        /// UBB转换成HTML显示
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ContextUbb2Html(string str)
        {
            return Ubb2html(str, true);
        }

        #endregion

        #region string ContextUbb2Html(string str, bool isHidden)
        /// <summary>
        /// UBB转换成HTML显示
        /// </summary>
        /// <param name="str"></param>
        /// <param name="isHidden"></param>
        /// <returns></returns>
        public static string ContextUbb2Html(string str, bool isHidden)
        {
            return Ubb2html(str, isHidden);
        }
        #endregion

        #region string Ubb2html(string ubbCode, bool isHidden)
        public static string Ubb2html(string ubbCode, bool isHidden)
        {
            string htmlCode = ubbCode;
            ///try
            //{
            string[] UBBhead = {
                    "[b", "[img", "[u", "[center", "[url", "[email", "[i", "[flash",
                    "[fly", "[move", "[color", "[size", "[face", "[rm", "[mp", "[qt",
                    "[dir", "[glow", "[shadow", "[quote", "[sound", "[hide"};
            string[] UBBfoot = {
                    "[/b]", "[/img]", "[/u]", "[/center]", "[/url]", "[/email]", "[/i]",
                    "[/flash]", "[/fly]", "[/move]", "[/color]", "[/size]", "[/face]",
                    "[/rm]", "[/mp]", "[/qt]", "[/dir]", "[/glow]", "[/shadow]",
                    "[/quote]", "[/sound]", "[/hide]"};
            for (int i = 0; i < UBBfoot.Length; i++)
            {
                htmlCode = UbbReplace(htmlCode, UBBhead[i], UBBfoot[i], isHidden);
            }

            //}
            //catch (Exception e)
            //{
            // }

            return htmlCode;
        }
        #endregion

        #region string UbbReplace(string code, string head, string foot,bool isHidden)
        private static string UbbReplace(string code, string head, string foot,
                                     bool isHidden)
        {
            try
            {
                int index1 = 0, index2 = 0, index3 = 0, index4 = 0;
                string other = "";
                while (index3 >= 0)
                {
                    index3 = code.IndexOf(foot, index3);
                    if (index3 < 0)
                    {
                        break;
                    }
                    //code = " " + code; //待检查问题。加空格可以全替换，不然第一个不替换
                    //[i]12345[/i]
                    index4 = index3 + foot.Length - 1;

                    string leftstr = Substring(code, 0, index3);
                    //FastSpring.Utils.MainConfig.PrintErrLine("leftstr:" + leftstr);
                    index1 = leftstr.IndexOf(head);
                    if (index1 < 0)
                        return code;
                    string midstr = Substring(code, index1, index3);
                    //FastSpring.Utils.MainConfig.PrintErrLine("midstr:" + midstr);
                    index2 = midstr.IndexOf("]");
                    string mid1 = Substring(midstr, index2 + 1);
                    //FastSpring.Utils.MainConfig.PrintErrLine("mid1:" + mid1);

                    //左边  正确的
                    leftstr = Substring(leftstr, 0, index1);
                    //FastSpring.Utils.MainConfig.PrintErrLine("leftstr:" + leftstr);

                    //右边　正确的
                    string rightstr = Substring(code, index4 + 1);
                    //FastSpring.Utils.MainConfig.PrintErrLine("rightstr:" + rightstr);

                    if (foot.Equals("[/img]"))
                    {
                        //图片
                        //<img src="" border="0" alt="点击查看全图" onload="javascript:if(this.width>screen.width-200) this.width=screen.width-200">
                        other = "<a href='" + mid1 + "' target=_blank><img src=\"" + mid1 +
                                "\" border=0 onmousewheel='return bbimg(this)' onLoad=\"javascript:if(this.width>580)this.width=580;\"></a>";
                        code = leftstr + other + rightstr;
                    }
                    else if (foot.Equals("[/move]"))
                    {
                        //移动
                        other = "<MARQUEE scrollamount=3>" + mid1 + "</marquee>";
                        code = leftstr + other + rightstr;
                    }
                    else if (foot.Equals("[/fly]"))
                    {
                        //飞行
                        other = "<MARQUEE width=90% behavior=alternate scrollamount=3>" + mid1 +
                                "</marquee>";
                        code = leftstr + other + rightstr;
                    }
                    else if (foot.Equals("[/center]"))
                    {
                        //对齐
                        other = "<div align=center>" + mid1 + "</div>";
                        code = leftstr + other + rightstr;
                    }
                    else if (foot.Equals("[/hide]"))
                    {
                        if (isHidden)
                        {
                            //隐藏
                            other = "<i><font color=red>以下内容回复才可以查看</font></i><br>";
                            code = leftstr + other + rightstr;

                        }
                        else
                        {
                            //不隐藏
                            other = mid1;
                            code = leftstr + other + rightstr;
                        }
                    }
                    else if (foot.Equals("[/url]"))
                    {
                        //连接
                        string mid0 = Substring(midstr, head.Length, index2);
                        other = "<a href" + mid0 + " target=_blank>" + mid1 + "</a>";
                        code = leftstr + other + rightstr;
                    }
                    else if (foot.Equals("[/color]"))
                    {
                        //颜色
                        string mid0 = Substring(midstr, head.Length, index2);
                        other = "<font color" + mid0 + ">" + mid1 + "</font>";
                        code = leftstr + other + rightstr;
                    }
                    else if (foot.Equals("[/size]"))
                    {
                        //大小
                        string mid0 = Substring(midstr, head.Length, index2);
                        other = "<font size" + mid0 + ">" + mid1 + "</font>";
                        code = leftstr + other + rightstr;
                    }
                    else if (foot.Equals("[/face]"))
                    {
                        //字体
                        string mid0 = Substring(midstr, head.Length, index2);
                        other = "<font face" + mid0 + ">" + mid1 + "</font>";
                        code = leftstr + other + rightstr;
                    }
                    else if (foot.Equals("[/email]"))
                    {
                        //email
                        other = "<a href=mailto:" + mid1 + " >" + mid1 + "</a>";
                        code = leftstr + other + rightstr;
                    }
                    else if (foot.Equals("[/flash]"))
                    {
                        //flash
                        string mid0 = Substring(midstr, head.Length, index2);
                        string mid01 = "200";
                        string mid02 = "200";
                        int index0 = mid0.IndexOf(",");
                        if (index0 > 0)
                        {
                            mid01 = Substring(mid0, 1, index0);
                            mid02 = Substring(mid0, index0 + 1);
                        }
                        other = "<a href=" + mid1 + " TARGET=_blank><IMG SRC=../Html/bbsimages/files/swf.gif border=0 alt=点击开新窗口欣赏该FLASH动画! height=16 width=16>[全屏欣赏]</a><br><OBJECT codeBase=http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=4,0,2,0 classid=clsid:D27CDB6E-AE6D-11cf-96B8-444553540000 width=500 height=400><PARAM NAME=movie VALUE=" +
                                mid1 + "><PARAM NAME=quality VALUE=high><embed src=" + mid1 + " quality=high pluginspage='http://www.macromedia.com/shockwave/download/index.cgi?P1_Prod_Version=ShockwaveFlash' type='application/x-shockwave-flash' width=" +
                                mid01 + " height=" + mid02 + "></embed></OBJECT>";
                        code = leftstr + other + rightstr;

                    }
                    else if (foot.Equals("[/dir]"))
                    {
                        //dir
                        string mid0 = Substring(midstr, head.Length, index2);
                        string mid01 = "200";
                        string mid02 = "200";
                        int index0 = mid0.IndexOf(",");
                        if (index0 > 0)
                        {
                            mid01 = Substring(mid0, 1, index0);
                            mid02 = Substring(mid0, index0 + 1);
                        }
                        other = "<object classid=clsid:166B1BCA-3F9C-11CF-8075-444553540000 codebase=http://download.macromedia.com/pub/shockwave/cabs/director/sw.cab#version=7,0,2,0 width=" +
                                mid01 + " height=" + mid02 + "><param name=src value=" + mid1 +
                                "><embed src=" + mid1 +
                                " pluginspage=http://www.macromedia.com/shockwave/download/ width=" +
                                mid01 + " height=" + mid02 + "></embed></object>";
                        code = leftstr + other + rightstr;
                    }
                    else if (foot.Equals("[/rm]"))
                    {
                        //realplayer
                        string mid0 = Substring(midstr, head.Length, index2);
                        string mid01 = "200";
                        string mid02 = "200";
                        int index0 = mid0.IndexOf(",");
                        if (index0 > 0)
                        {
                            mid01 = Substring(mid0, 1, index0);
                            mid02 = Substring(mid0, index0 + 1);
                        }

                        other = "<OBJECT classid=clsid:CFCDAA03-8BE4-11cf-B84B-0020AFBBCCFA class=OBJECT id=RAOCX width=" +
                                mid01 + " height=" + mid02 + "><PARAM NAME=SRC VALUE=" + mid1 + "><PARAM NAME=CONSOLE VALUE=Clip1><PARAM NAME=CONTROLS VALUE=imagewindow><PARAM NAME=AUTOSTART VALUE=true></OBJECT><br><OBJECT classid=CLSID:CFCDAA03-8BE4-11CF-B84B-0020AFBBCCFA height=32 id=video2 width=" +
                                mid01 + "><PARAM NAME=SRC VALUE=" + mid1 + "><PARAM NAME=AUTOSTART VALUE=-1><PARAM NAME=CONTROLS VALUE=controlpanel><PARAM NAME=CONSOLE VALUE=Clip1></OBJECT>";
                        code = leftstr + other + rightstr;
                    }
                    else if (foot.Equals("[/mp]"))
                    {
                        //mideaplayer
                        string mid0 = Substring(midstr, head.Length, index2);
                        string mid01 = "200";
                        string mid02 = "200";
                        int index0 = mid0.IndexOf(",");
                        if (index0 > 0)
                        {
                            mid01 = Substring(mid0, 1, index0);
                            mid02 = Substring(mid0, index0 + 1);
                        }

                        other = "<object align=middle classid=CLSID:22d6f312-b0f6-11d0-94ab-0080c74c7e95 class=OBJECT id=MediaPlayer width=" +
                                mid01 + " height=" + mid02 +
                                " ><param name=ShowStatusBar value=-1><param name=Filename value=" +
                                mid1 + "><embed type=application/x-oleobject codebase=http://activex.microsoft.com/activex/controls/mplayer/en/nsmp2inf.cab#Version=5,1,52,701 flename=mp src=" +
                                mid1 + " width=" + mid01 + " height=" + mid02 +
                                "></embed></object>";
                        code = leftstr + other + rightstr;
                    }
                    else if (foot.Equals("[/qt]"))
                    {
                        //quicktime
                        string mid0 = Substring(midstr, head.Length, index2);
                        string mid01 = "200";
                        string mid02 = "200";
                        int index0 = mid0.IndexOf(",");
                        if (index0 > 0)
                        {
                            mid01 = Substring(mid0, 1, index0);
                            mid02 = Substring(mid0, index0 + 1);
                        }

                        other = "<embed src=" + mid1 + " width=" + mid01 + " height=" + mid02 + " autoplay=true loop=false controller=true playeveryframe=false cache=false scale=TOFIT bgcolor=#000000 kioskmode=false targetcache=false pluginspage=http://www.apple.com/quicktime/>";
                        code = leftstr + other + rightstr;
                    }
                    else if (foot.Equals("[/quote]"))
                    {
                        //引用
                        other = "<table style=\"width:100%\" cellpadding=5 cellspacing=1><TR><TD width=\"100%\"><I>" +
                                mid1 + "</I></td></tr></table><br>";
                        code = leftstr + other + rightstr;
                    }
                    else if (foot.Equals("[/sound]"))
                    {
                        //声音
                        other = "<a href=" + mid1 + " target=_blank><IMG SRC=../Html/bbsimages/files/mid.gif border=0 alt='背景音乐'></a><bgsound src=" +
                                mid1 + " loop=\"-1\">";
                        code = leftstr + other + rightstr;
                    }
                    else if (foot.Equals("[/glow]"))
                    {
                        //发光
                        string mid0 = Substring(midstr, head.Length, index2);
                        string mid01 = "100";
                        string mid02 = "100";
                        string mid03 = "100";
                        int index0 = mid0.IndexOf(",");
                        if (index0 > 0)
                        {
                            mid01 = Substring(mid0, 1, index0);
                            string mid00 = Substring(mid0, index0 + 1);
                            int index01 = mid00.IndexOf(",");
                            if (index01 > 0)
                            {
                                mid02 = Substring(mid00, 0, index01);
                                mid03 = Substring(mid00, index01 + 1);
                            }
                        }
                        other = "<table width=" + mid01 +
                                " ><tr><td style=\"filter:glow(color=" + mid02 + ", strength=" +
                                mid03 + ")\">" + mid1 + "</td></tr></table>";
                        code = leftstr + other + rightstr;
                    }
                    else if (foot.Equals("[/shadow]"))
                    {
                        //发光
                        string mid0 = Substring(midstr, head.Length, index2);
                        string mid01 = "100";
                        string mid02 = "100";
                        string mid03 = "100";
                        int index0 = mid0.IndexOf(",");
                        if (index0 > 0)
                        {
                            mid01 = Substring(mid0, 1, index0);
                            string mid00 = Substring(mid0, index0 + 1);
                            int index01 = mid00.IndexOf(",");
                            if (index01 > 0)
                            {
                                mid02 = Substring(mid00, 0, index01);
                                mid03 = Substring(mid00, index01 + 1);
                            }
                        }
                        other = "<table width=" + mid01 +
                                " ><tr><td style=\"filter:shadow(color=" + mid02 +
                                ", strength=" + mid03 + ")\">" + mid1 + "</td></tr></table>";
                        code = leftstr + other + rightstr;
                    }
                    else
                    {
                        midstr = "<" + Substring(midstr, 1);
                        //FastSpring.Utils.MainConfig.PrintErrLine("midstr0:" + midstr);
                        midstr = Substring(midstr, 0, index2) + ">" + Substring(midstr, index2 + 1);
                        //FastSpring.Utils.MainConfig.PrintErrLine("midstr0:" + midstr);
                        midstr += "<" + foot.Substring(1, foot.Length - 2) + ">";
                        //midstr = Substring(midstr, 0, index3 - index1) + "<" +  Substring(midstr,index3 - index1 + 1);
                        //FastSpring.Utils.MainConfig.PrintErrLine("midstr0:" + midstr);
                        //midstr = Substring(midstr, 0, index4 - index1) + ">";
                        //FastSpring.Utils.MainConfig.PrintErrLine("midstr0:" + midstr);
                        code = leftstr + midstr + rightstr;
                    }
                }
            }
            catch
            {
            }
            return code;
        }
        #endregion

        #region 危险标签清理
        /// <summary>
        /// 危险标签清理
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string DangerTagsFilter(string html)
        {
            //清理<iframe>
            html = Regex.Replace(html, @"<[/]?\s*i\s*f\s*r\s*a\s*m\s*e[^>]*>", "", RegexOptions.IgnoreCase);
            //清理<frameset>
            html = Regex.Replace(html, @"<[/]?\s*f\s*r\s*a\s*m\s*e\s*s\s*e\s*t[^>]*>", "", RegexOptions.IgnoreCase);

            //处理非<script>危险属性
            html = Regex.Replace(html, @"<(?![/]?script)([^>]*)>",
                delegate(Match m)
                {
                    string tag = m.Value;
                    //处理属性中的script内容
                    tag = Regex.Replace(tag, @"[\s-]*s\s*c\s*r\s*i\s*p\s*t", "-script", RegexOptions.IgnoreCase);
                    //处理样式中的expression
                    tag = Regex.Replace(tag, @"[\s-]*e\s*x\s*p\s*r\s*e\s*s\s*s\s*i\s*o\s*n", "-expression", RegexOptions.IgnoreCase);
                    //处理事件
                    tag = Regex.Replace(tag, @"([/\s]+)(on[^=]+=)", " _$2", RegexOptions.IgnoreCase);
                    return tag.Replace("&", "&amp;");
                }, RegexOptions.IgnoreCase);

            //处理<script>标签的defer属性
            html = Regex.Replace(html, @"(<script[^>]*[\s\/]+)(defer)(.*>)", "$1_defer$3", RegexOptions.IgnoreCase);
            //处理<style>标签
            return Regex.Replace(html, @"(<[/]?)(style[^>]*>)", "$1T:$2", RegexOptions.IgnoreCase); ;
        }
        #endregion


        /// <summary>
        /// 对字符串进行兼容javascript脚本语言的ecsape编码
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string Escape(string s)
        {
            StringBuilder sb = new StringBuilder();
            byte[] ba = System.Text.Encoding.Unicode.GetBytes(s);
            for (int i = 0; i < ba.Length; i += 2)
            {
                sb.Append("%u");
                sb.Append(ba[i + 1].ToString("X2"));
                sb.Append(ba[i].ToString("X2"));
            }
            return sb.ToString();
        }

        public static void ClientMethod(string message, string active)
        {
            HttpResponse response = HttpContext.Current.Response;
            response.Expires = -1;
            response.ContentType = "text/html; charset=" + HttpContext.Current.Request.ContentEncoding.WebName;
            response.AppendHeader("Active", active);
            response.Clear();
            response.Write(message);
        }

        /// <summary>
        /// 获得当前系统的Response编码
        /// </summary>
        /// <returns></returns>
        public static Encoding GetEncoding()
        {
            Encoding encoding = Encoding.UTF8;

            object config = WebConfigurationManager.GetWebApplicationSection("system.web/globalization");
            Type type1 = config.GetType();

            if (type1 != null)
            {
                PropertyInfo pinfo = type1.GetProperty("responseEncoding", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (pinfo != null)
                {
                    encoding = (Encoding)pinfo.GetValue(config, null);
                }
                else
                {
                    FieldInfo finfo = type1.GetField("responseEncoding", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                    if (finfo != null)
                    {
                        encoding = (Encoding)finfo.GetValue(config);
                    }
                }
            }

            return encoding;
        }

        #region 输出到html
        /// <summary>
        /// 输出到html
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string sRepOutHTML(string str)
        {
            string s;
            s = str;
            if (!String.IsNullOrEmpty(s))
            {
                s = s.Replace("<", "&#60;");
                s = s.Replace(">", "&#62;");
                s = s.Replace("'", "&#39;");
                s = s.Replace("\"", "&#34;");
                s = s.Replace("\n", "<br>");
            }
            return s;
        }
        #endregion

        #region 输出到编辑器
        public static string sRepOutEditor(string s)
        {
            if (!String.IsNullOrEmpty(s))
            {
                s = s.Replace("\"", "\\\"");
                s = s.Replace("'", "\\'");
                //'s = replace(s,chr(13),"\r")
                //'s = replace(s,chr(10),"\n")			
            }
            return s;
        }
        #endregion

        #region 输出到表单时时的字符串过滤
        public static string sRepOutForm(string str)
        {
            string s;
            s = str;
            if (!String.IsNullOrEmpty(s))
            {
                s = s.Replace("'", "\\'");
                s = s.Replace("\"", "\\\"");
            }
            return s;
        }
        #endregion


        #region 编辑器过滤
        public static string RepOutHTMLEditor(string str)
        {
            string s;
            s = str;
            if (!String.IsNullOrEmpty(s))
            {
                s = s.Replace("<", "&#60;");
                s = s.Replace(">", "&#62;");
                s = s.Replace("&nbsp;", "&#38;&#110;&#98;&#115;&#112;&#59;");
                s = s.Replace("\"", "&#34;");
                s = s.Replace("\n", "<BR>");
            }
            return s;
        }

        public static string RepOutEditorEditor(string s)
        {
            if (!String.IsNullOrEmpty(s))
            {
                s = s.Replace("\"", "\\\"");
                s = s.Replace("'", "\\'");
                s = s.Replace("\n", "\\n");
                s = s.Replace("\r", "\\r");
            }
            return s;
        }

        #endregion

        #region 初始进编辑器
        public static string sRepOutHTMLFirstedit(string str)
        {
            string s;
            s = str;
            if (!String.IsNullOrEmpty(s))
            {
                s = s.Replace("<", "&#60;");
                s = s.Replace(">", "&#62;");
                s = s.Replace("\"", "&#34;");
            }
            return s;
        }

        #endregion


        /// <summary>
        /// 弹出消息，若url不为空，页面进行转向
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="url"></param>
        /// <param name="position">String.empty,self,parent</param>
        public static void AlertOrLocation(string msg, string url, string position)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            //弹出消息
            sb.Append("<script language=\"javascript\">");
            sb.Append("alert(\"");
            sb.Append(msg);
            sb.Append("\");");
            //弹出之后操作，
            //如果有url，则转向
            if (url != string.Empty)
            {
                if (!String.IsNullOrEmpty(position))
                {
                    sb.Append("window.");
                    sb.Append(position);
                    sb.Append(".location.href=\"");
                    sb.Append(url);
                    sb.Append("\";");
                }
                else
                {
                    sb.Append("window");
                    sb.Append(".location.href=\"");
                    sb.Append(url);
                    sb.Append("\";");
                }
            }
            sb.Append("</script>");
            System.Web.HttpContext.Current.Response.Write(sb.ToString());
        }

        /// <summary>
        /// 弹出消息并关闭本窗口
        /// </summary>
        /// <param name="msg"></param>
        public static void AlertAndCloseWindow(string msg)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            //弹出消息
            sb.Append("<script language=\"javascript\">");
            sb.Append("alert(\"");
            sb.Append(msg);
            sb.Append("\");");
            //弹出之后操作，
            sb.Append("window.close();");
            sb.Append("</script>");
            System.Web.UI.Page page = (System.Web.UI.Page)System.Web.HttpContext.Current.Handler;

            page.RegisterStartupScript("addconfirm", sb.ToString());
        }


        /// <summary>
        /// 弹出窗口并返回上个页面
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="count"></param>
        public static void AlertAndGoBack(string msg, int count)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            //弹出消息
            sb.Append("<script language=\"javascript\">");
            sb.Append("alert(\"");
            sb.Append(msg);
            sb.Append("\");");
            //弹出之后操作，根据页面刷新次数得到要返回的次数
            sb.Append("window.history.go(");
            sb.Append(count.ToString());
            sb.Append(");");
            sb.Append("</script>");
            System.Web.HttpContext.Current.Response.Write(sb.ToString());
        }

        /// <summary>
        /// 在page页面弹出对话框
        /// </summary>
        /// <param name="page">页面</param>
        /// <param name="msg">消息</param>
        public static void Alert(System.Web.UI.Page page, string msg)
        {


            page.RegisterStartupScript("addconfirm", "<script language='javascript' defer>alert('" + msg.ToString() + "');</script>");
        }

        /// <summary>
        /// 在page页面弹出对话框,并关闭本窗口
        /// </summary>
        /// <param name="page">页面</param>
        /// <param name="msg">消息</param>
        public static void AlertAndCloseWindow(System.Web.UI.Page page, string msg)
        {

            page.RegisterStartupScript("addconfirm", "<script language='javascript' defer>alert('" + msg.ToString() + "');window.close();</script>");
        }

        /// <summary>
        /// 弹出对话框,返回值,关闭窗口
        /// </summary>
        /// <param name="page">页面</param>
        /// <param name="msg">消息</param>
        /// <param name="back">返回值</param>
        public static void AlterAndReturnAndCloseWindow(System.Web.UI.Page page, string msg, string back)
        {


            page.RegisterStartupScript("addconfirm", "<script language='javascript' defer>alert('" + msg.ToString() + "');window.returnValue ='" + back + "';window.close();</script>");
        }

        /// <summary>
        /// 返回值,关闭窗口
        /// </summary>
        /// <param name="page">页面</param>
        /// <param name="back">返回值</param>
        public static void ReturnAndCloseWindow(System.Web.UI.Page page, string back)
        {


            page.RegisterStartupScript("addconfirm", "<script language='javascript' defer>window.returnValue ='" + back + "';window.close();</script>");
        }

        /// <summary>
        /// 弹出对话框,转到其他页
        /// </summary>
        /// <param name="page">页面</param>
        /// <param name="msg">消息</param>
        /// <param name="url">其他页</param>
        public static void AlertAndLocation(System.Web.UI.Page page, string msg, string url)
        {


            page.RegisterStartupScript("addconfirm", "<script language='javascript' defer>alert('" + msg.ToString() + "');window.location='" + url + "';</script>");
        }


        /// <summary>
        /// button添加确认操作脚本
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="url">转向地址</param>
        /// <param name="position">String.Empty,self,parent</param>
        public static void ButtonAddConfirmScript(System.Web.UI.WebControls.Button btn, string msg, string url, string position)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            //弹出消息
            sb.Append("<script language=\"javascript\">");
            sb.Append("\n");
            sb.Append("function AddConfirm(msg,url,position)");
            sb.Append("\n{");
            sb.Append("if (!window.confrim(\"msg\"))");
            sb.Append("\n{");
            sb.Append("		widow.event.returnValue=false;");
            sb.Append("\n	if (url != \"\")");
            sb.Append("\n	{");
            sb.Append("\n	window.");
            sb.Append(position);
            sb.Append(".location.href=\"url\";");
            sb.Append("\n	}");

            sb.Append("\n}");
            sb.Append("\n}");

            sb.Append("</script>");

            System.Web.UI.Page page = (System.Web.UI.Page)System.Web.HttpContext.Current.Handler;

            page.RegisterStartupScript("addconfirm", sb.ToString());
            //添加按钮
            btn.Attributes.Add("onclick", "javascript:AddConfrim(\"" + msg + "\",\"" + url + "\",\"" + position + "\");");
        }

    }
}
