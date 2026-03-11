using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.IO;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Com.ValuePlus.Utils.HTTPRequest
{
    /// <summary>
    /// 对Url处理的请求
    /// </summary>
    public class HttpUrl
    {
        private string cookieHeader = null;
        // Cookie容器
        private CookieContainer cookieContainer = new CookieContainer();
        // Proxy实体
        private WebProxy webProxy = null;
        // 请求代理字符串
        private const string CST_USER_AGENT = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; Maxthon; .NET CLR 1.1.4322)";
        // 请求超时时间（毫秒）
        private const int CST_REQUEST_TIME_OUT = 90000;

        /// <summary>
        /// 构造函数
        /// </summary>
        public HttpUrl()
        {
        }

        /// <summary>
        /// 设置代理服务器
        /// </summary>
        /// <param name="proxyIp">代理IP</param>
        /// <param name="proxyPoint">代理端口</param>
        /// <returns>是否成功</returns>
        public bool setProxy(string proxyIp, int proxyPoint)
        {
            if (proxyIp != null && proxyIp.Length > 0 && proxyPoint > 0)
            {
                webProxy = new WebProxy(proxyIp, proxyPoint);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 请求网页内容
        /// </summary>
        /// <param name="sUrl">请求地址</param>
        /// <returns>HttpResponse信息对象</returns>
        public HttpResponse request(string sUrl)
        {
            return request(sUrl, "", "get", "", "");
        }

        /// <summary>
        /// 请求网页内容
        /// </summary>
        /// <param name="sUrl">请求地址</param>
        /// <param name="referenceUrl">发起请求引用页面</param>
        /// <returns>HttpResponse信息对象</returns>
        public HttpResponse request(string sUrl, string referenceUrl)
        {
            return request(sUrl, referenceUrl, "get", "", "");
        }

        /// <summary>
        /// 请求网页内容
        /// </summary>
        /// <param name="sUrl">请求地址</param>
        /// <param name="referenceUrl">发起请求引用页面</param>
        /// <param name="methodString">请求方法：get/post</param>
        /// <param name="dataString">请求数据，格式：name1=xxxx&name2=xxxx&name2=xxxx</param>
        /// <returns>HttpResponse信息对象</returns>
        public HttpResponse request(string sUrl, string referenceUrl, string methodString, string dataString)
        {
            return request(sUrl, referenceUrl, methodString, dataString, "");
        }

        /// <summary>
        /// 请求网页内容
        /// </summary>
        /// <param name="sUrl">请求地址</param>
        /// <param name="referenceUrl">发起请求引用页面</param>
        /// <param name="methodString">请求方法：get/post</param>
        /// <param name="dataString">请求数据，格式：name1=xxxx&name2=xxxx&name2=xxxx</param>
        /// <param name="charset">页面编码类型</param>
        /// <returns>HttpResponse信息对象</returns>
        public HttpResponse request(string sUrl, string referenceUrl, string methodString, string dataString, string characterSet)
        {
            // 定义返回请求对象
            HttpResponse httpResponse = null;
            httpResponse = new HttpResponse();

            // 请求地址无效则返回错误
            if (sUrl == null || sUrl.Length < 1)
            {
                httpResponse.setResponseErrorMessage("请求地址不能为空。");
                return httpResponse;
            }

            // 修正输入数据
            string url = sUrl;
            string method = (methodString == null) ? "GET" : methodString.ToUpper();
            string data = (dataString == null) ? "" : dataString;
            string refurl = (referenceUrl == null) ? "" : referenceUrl;
            string charset = (characterSet == null) ? "" : characterSet;
            if (data.Length > 0 && method == "GET")
            {
                url += ((url.IndexOf("?") > 0) ? "&" : "?") + data;
            }

            // 预定义对象
            HttpWebResponse response = null;
            Stream responseStream = null;
            StreamReader readStream = null;
            MemoryStream ms = null;
            MemoryStream ms2 = null;
            try
            {
                // 创建请求对象
                HttpWebRequest http = (HttpWebRequest)System.Net.WebRequest.Create(url);

                // 请求前设置请求参数
                // 设置Proxy代理
                if (webProxy != null) http.Proxy = webProxy;
                // 伪造IE请求
                http.UserAgent = CST_USER_AGENT;
                // 设置超时时间
                http.Timeout = CST_REQUEST_TIME_OUT;
                // 设置请求引用页面
                http.Referer = refurl;
                // 容许接受任何文件
                http.Accept = "*/*";
                // 允许接受GZip数据流
                //http.Headers.Add("Accept-Encoding: gzip, deflate");
                // 保持连接持续性
                http.KeepAlive = true;
                // 允许重定向
                http.AllowAutoRedirect = true;
                // 设置Cookie容器
                http.CookieContainer = cookieContainer;
                // 不是第一次访问页面则设置同一进程Cookie内容
                if (cookieHeader != null) http.CookieContainer.SetCookies(http.RequestUri, cookieHeader);
                // 设置数据提交方式
                http.Method = method;
                // 获取编码方式
                Encoding encode = null;
                if (charset.Length > 0) encode = Encoding.GetEncoding(charset);

                // 写入利用post方式进行提交的数据
                if (method == "POST")
                {
                    // 设置提交类型
                    http.ContentType = "application/x-www-form-urlencoded";
                    // 开始分析提交数据
                    if (data.Length > 0)
                    {
                        // 生成提交编码类型
                        Encoding encPost = (encode == null) ? Encoding.Default : encode;
                        StringBuilder UrlEncoded = new StringBuilder();
                        Char[] reserved = { '?', '=', '&' };
                        byte[] SomeBytes = null;
                        int i = 0, j;
                        // 编码提交数据
                        while (i < data.Length)
                        {
                            j = data.IndexOfAny(reserved, i);
                            if (j == -1)
                            {
                                UrlEncoded.Append(System.Web.HttpUtility.UrlEncode(data.Substring(i, data.Length - i), encPost));
                                break;
                            }
                            UrlEncoded.Append(System.Web.HttpUtility.UrlEncode(data.Substring(i, j - i), encPost));
                            UrlEncoded.Append(data.Substring(j, 1));
                            i = j + 1;
                        }
                        // 取得编码后的提交字节流
                        ASCIIEncoding ascEncode = new ASCIIEncoding();
                        SomeBytes = ascEncode.GetBytes(UrlEncoded.ToString());
                        // 设置数据长度
                        http.ContentLength = SomeBytes.Length;
                        // 写入提交数据
                        Stream postStream = http.GetRequestStream();
                        postStream.Write(SomeBytes, 0, SomeBytes.Length);
                        postStream.Close();
                    }
                    else
                    {
                        http.ContentLength = 0;
                    }
                }

                // 开始请求数据并取得响应内容
                response = (HttpWebResponse)http.GetResponse();

                // 处理Cookie信息
                // 若第一次访问则获取相应Cookie头信息
                string header = http.CookieContainer.GetCookieHeader(http.RequestUri);
                if (cookieHeader == null && header != null && header.Length > 0) cookieHeader = header.Replace(";", ",");
                // 附加Cookie信息到Cookie容器
                cookieContainer.Add(response.Cookies);

                // 取得响应字节流
                responseStream = response.GetResponseStream();

                int read = 0;
                byte[] buffer = new byte[8192];

                // 普通方式（无压缩传输），把字节流保存到ms2
                ms2 = new MemoryStream();
                try
                {
                    // 读取字节流
                    while (true)
                    {
                        read = responseStream.Read(buffer, 0, 8192);
                        if (read == 0)
                            break;
                        else
                            ms2.Write(buffer, 0, read);
                    }
                }
                catch (Exception)
                {
                }
                ms2.Seek(0, SeekOrigin.Begin);

                // 获取响应编码方式
                string responseCharSet = null;
                if (encode == null)
                {
                    responseCharSet = response.CharacterSet;
                    if (responseCharSet == null || responseCharSet.Length < 1)
                    {
                        encode = Encoding.Default;
                        responseCharSet = encode.BodyName;
                    }
                    else
                    {
                        try
                        {
                            encode = Encoding.GetEncoding(responseCharSet);
                        }
                        catch
                        {
                            encode = Encoding.Default;
                            responseCharSet = encode.BodyName;
                        }
                    }
                }
                else
                {
                    responseCharSet = charset;
                }

                // 获取文本
                string responseText = "";
                if (response.ContentType.IndexOf("text") >= 0)
                {
                    // 非压缩文件流，则直接读取
                    readStream = new StreamReader(ms2, encode);
                    // 是文本内容则获取文本
                    responseText = readStream.ReadToEnd();
                    // 临时设置响应内容
                    httpResponse.setResponseText(responseText);
                    // 查找编码方式
                    string meta = httpResponse.getElementString("<meta ", ">", "Content-Type");
                    if (meta != null && meta.Length > 0)
                    {
                        // 获取编码
                        string findCharSet = httpResponse.getElementProperty(meta, "content");
                        if (findCharSet != null && findCharSet.Length > 0)
                        {
                            int pos = findCharSet.IndexOf("charset", StringComparison.OrdinalIgnoreCase);
                            if (pos > 0)
                            {
                                pos = findCharSet.IndexOf("=", pos + 7);
                                if (pos > 0)
                                {
                                    findCharSet = findCharSet.Substring(pos + 1).Trim();
                                }
                                else
                                {
                                    findCharSet = responseCharSet;
                                }
                            }
                            else
                            {
                                findCharSet = responseCharSet;
                            }
                            // 若找到编码方式，判断跟之前的解码方式是否一致
                            if (!responseCharSet.Equals(findCharSet, StringComparison.OrdinalIgnoreCase))
                            {
                                // 假如不一致则按页面编码方式重新解码
                                try
                                {
                                    encode = Encoding.GetEncoding(findCharSet);
                                    responseCharSet = findCharSet;
                                    // 临时流返回开始
                                    ms2.Seek(0, SeekOrigin.Begin);
                                    StreamReader readStream2 = new StreamReader(ms2, encode);
                                    // 重新获取页面内容
                                    responseText = readStream2.ReadToEnd();
                                    readStream2.Close();
                                    readStream2 = null;
                                    // 重新设置返回参数
                                    httpResponse.setResponseText(responseText);
                                }
                                catch
                                {
                                }
                            }
                        }
                    }
                }
                // 设置返回参数
                httpResponse.setResponseCharacterSet(responseCharSet);
                httpResponse.setResponseContentEncoding(response.ContentEncoding);
                httpResponse.setResponseContentType(response.ContentType);
                httpResponse.setResponseStream(ms2);
                httpResponse.setResponseSucceed(true);
                httpResponse.setResponseText(responseText);
                httpResponse.setResponseUrl(response.ResponseUri.ToString());
            }
            catch (Exception e)
            {
                httpResponse.setResponseSucceed(false);
                httpResponse.setResponseErrorMessage(e.ToString());
                Com.ValuePlus.Log.LogFactory.CreateInstance("UrlRequest").Error("发送web请求错误URL=" + sUrl, e);
            }
            finally
            {
                try
                {
                    if (ms != null)
                    {
                        ms.Close();
                        ms = null;
                    }
                    if (readStream != null)
                    {
                        readStream.Close();
                        readStream = null;
                    }
                    if (responseStream != null)
                    {
                        responseStream.Close();
                        responseStream = null;
                    }
                    if (response != null)
                    {
                        response.Close();
                        response = null;
                    }
                }
                catch (Exception) { }
            }
            return httpResponse;
        }

    }

    /// <summary>
    /// 网站数据请求响应对象
    /// </summary>
    public class HttpResponse
    {
        // 响应是否成功标志
        private bool responseSucceed = false;
        // 响应URL
        private string responseUrl = "";
        // 响应文本
        private string responseText = "";
        // 响应文本小写字符串
        private string responseTextLower = "";
        // 响应字节流
        private Stream responseStream = null;
        // 响应的错误信息
        private string responseErrorMessage = "";
        // 响应字符集
        private string responseCharacterSet = "";
        // 响应字符集编码方法
        private string responseContentEncoding = "";
        // 响应内容类型
        private string responseContentType = "";

        /// <summary>
        /// 构造函数
        /// </summary>
        public HttpResponse()
        {
        }

        /// <summary>
        /// 设置响应状态标志
        /// </summary>
        /// <param name="bSucceed">是否成功</param>
        internal void setResponseSucceed(bool bSucceed)
        {
            this.responseSucceed = bSucceed;
        }

        /// <summary>
        /// 设置响应页URL
        /// </summary>
        /// <param name="sUrl">响应页URL</param>
        internal void setResponseUrl(string sUrl)
        {
            this.responseUrl = sUrl;
        }

        /// <summary>
        /// 设置响应文本内容
        /// </summary>
        /// <param name="sText">响应文本</param>
        internal void setResponseText(string sText)
        {
            this.responseText = sText;
        }

        /// <summary>
        /// 设置响应字节流
        /// </summary>
        /// <param name="mStream">字节流</param>
        internal void setResponseStream(Stream mStream)
        {
            this.responseStream = mStream;
        }

        /// <summary>
        /// 设置响应错误信息
        /// </summary>
        /// <param name="sMessage">错误信息</param>
        internal void setResponseErrorMessage(string sMessage)
        {
            this.responseErrorMessage = sMessage;
        }

        /// <summary>
        /// 设置响应字符集
        /// </summary>
        /// <param name="sCharacterSet">响应字符集</param>
        internal void setResponseCharacterSet(string sCharacterSet)
        {
            this.responseCharacterSet = sCharacterSet;
        }

        /// <summary>
        /// 设置响应字符集编码方法
        /// </summary>
        /// <param name="sCharacterSet">响应字符集编码方法</param>
        internal void setResponseContentEncoding(string sContentEncoding)
        {
            this.responseContentEncoding = sContentEncoding;
        }

        /// <summary>
        /// 设置响应内容类型
        /// </summary>
        /// <param name="sCharacterSet">响应内容类型</param>
        internal void setResponseContentType(string sContentType)
        {
            this.responseContentType = sContentType;
        }

        /// <summary>
        /// 获取响应状态标志
        /// </summary>
        public bool getResponseSucceed()
        {
            return this.responseSucceed;
        }

        /// <summary>
        /// 获取响应页URL
        /// </summary>
        public string getResponseUrl()
        {
            return this.responseUrl;
        }

        /// <summary>
        /// 获取响应文本内容
        /// </summary>
        public string getResponseText()
        {
            return this.responseText;
        }

        /// <summary>
        /// 获取响应字节流
        /// </summary>
        public Stream getResponseStream()
        {
            return this.responseStream;
        }

        /// <summary>
        /// 获取响应错误信息
        /// </summary>
        public string getResponseErrorMessage()
        {
            return this.responseErrorMessage;
        }

        /// <summary>
        /// 获取响应字符集
        /// </summary>
        public string getResponseCharacterSet()
        {
            return this.responseCharacterSet;
        }

        /// <summary>
        /// 获取响应字符集编码方法
        /// </summary>
        public string getResponseContentEncoding()
        {
            return this.responseContentEncoding;
        }

        /// <summary>
        /// 获取响应内容类型
        /// </summary>
        public string getResponseContentType()
        {
            return this.responseContentType;
        }

        /// <summary>
        /// 关闭响应对象及字节流
        /// </summary>
        public void close()
        {
            if (this.responseStream != null)
            {
                try
                {
                    this.responseStream.Close();
                    this.responseStream = null;
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// 根据元素关键字获取完整的元素字符串
        /// </summary>
        /// <param name="startElementString">元素开始位字符串</param>
        /// <param name="endElementString">元素结束位字符串</param>
        /// <param name="elementKey">元素内部关键字</param>
        /// <returns>元素结构字符串</returns>
        public string getElementString(string startElementString, string endElementString, string elementKey)
        {
            string sHtml = this.responseText;
            string ret = "";
            try
            {
                //new Regex(@"<\s*a[^>]*>([^<]|<(?!/a))*<\s*/a\s*>", RegexOptions.Multiline); 
                string regs = startElementString + ".*" + elementKey + ".*" + endElementString;
                Regex reg = new Regex(regs, RegexOptions.Multiline | RegexOptions.IgnoreCase);
                ret = reg.Match(sHtml).Value;
                int pos = ret.IndexOf(endElementString);
                if (pos > 0)
                {
                    ret = ret.Substring(0, pos + endElementString.Length);
                }
            }
            catch (Exception)
            {
                ret = "";
            }
            return ret;
        }

        /// <summary>
        /// 取得元素结构字符串
        /// </summary>
        /// <param name="startHtml">开始位置关键字字符串</param>
        /// <param name="finalHtml">结束位置关键字字符串</param>
        /// <param name="startElementString">元素开始位字符串</param>
        /// <param name="endElementString">元素结束位字符串</param>
        /// <param name="matchIndex">第几个元素</param>
        /// <returns>元素结构字符串</returns>
        public string getElementString(string startHtml, string finalHtml, string startElementString, string endElementString, int matchIndex)
        {
            // 假如未转换为小写，则转换小写
            if (this.responseTextLower.Length < 1)
            {
                this.responseTextLower = this.responseText.ToLower();
            }
            // 初始化数据
            int i = 0;
            int l = 0;
            int pos1 = 0;
            int pos2 = 0;
            if (matchIndex < 1) matchIndex = 1;
            string ret = "";
            string sHtml = this.responseText;
            string fHtml = this.responseTextLower;
            string findKey = startHtml.ToLower();
            string finalKey = finalHtml.ToLower();
            string beginString = startElementString.ToLower();
            string endString = endElementString.ToLower();
            // 取得开始标记位置
            if (findKey.Length > 0)
            {
                pos1 = fHtml.IndexOf(findKey);
                if (pos1 >= 0)
                {
                    fHtml = fHtml.Substring(pos1);
                    sHtml = sHtml.Substring(pos1);
                }
            }
            // 取得结束标记位置
            if (finalKey.Length > 0)
            {
                pos1 = fHtml.IndexOf(finalKey);
                if (pos1 >= 0)
                {
                    fHtml = fHtml.Substring(0, pos1);
                    sHtml = sHtml.Substring(0, pos1);
                }
            }
            // 查找开始字符串
            l = beginString.Length;
            for (i = 0; i < matchIndex; i++)
            {
                pos1 = fHtml.IndexOf(beginString, pos2);
                if (pos1 < 0) break;
                pos2 = pos1 + l;
            }
            if (pos1 >= 0)
            {
                // 查找标记结束位置
                pos2 = fHtml.IndexOf(endString, pos1);
                if (pos2 > 0)
                {
                    ret = sHtml.Substring(pos1, pos2 - pos1 + 1);
                }
            }
            return ret;
        }

        /// <summary>
        /// 取得元素属性值
        /// </summary>
        /// <param name="elementString">元素字符串（如：<input name="abc" value="123" />，不区分大小写）</param>
        /// <param name="propName">需要取得的属性标签名（如：value）</param>
        /// <returns>属性值</returns>
        public string getElementProperty(string elementString, string propName)
        {
            string ret = "";
            try
            {
                string ends = "";
                string regs = "(\\s|<|\\.)" + propName + "(\\s+)?=((\\s+)?(\"|\')?)(.*)";
                Regex reg = new Regex(regs, RegexOptions.Multiline | RegexOptions.IgnoreCase);
                // 搜索属性值
                Match m;
                MatchCollection mc = reg.Matches(elementString);
                if (mc.Count > 0)
                {
                    m = mc[0];
                    if (m.Groups.Count >= 6)
                    {
                        ends = m.Groups[5].Value;
                        ret = m.Groups[6].Value;
                    }
                }
                if (ret.Length > 0)
                {
                    if (ends.Length < 1) ends = ">|\\s";
                    reg = new Regex(ends, RegexOptions.Multiline | RegexOptions.IgnoreCase);
                    if (reg.IsMatch(ret))
                    {
                        m = reg.Match(ret);
                        ret = ret.Substring(0, m.Index);
                    }
                }
            }
            catch (Exception)
            {
                ret = "";
            }
            return ret;
        }

        /// <summary>
        /// 取得表单隐藏域内容
        /// </summary>
        /// <param name="formIndex">第几个FORM标单</param>
        /// <returns>a=xxx&b=xxxx&c=xxxx</returns>
        public string getFormHiddens(int formIndex)
        {
            // 假如未转换为小写，则转换小写
            if (this.responseTextLower.Length < 1)
            {
                this.responseTextLower = this.responseText.ToLower();
            }
            // 初始化数据
            if (formIndex < 1) formIndex = 1;
            string srcHtml = this.responseText;
            string findHtml = this.responseTextLower;
            string ret = "";
            string formString = this.getElementString("", "", "<form ", "</form>", formIndex);
            if (formString.Length > 0)
            {
                // 取得所有input元素
                int l = 0;
                int pos1 = 0;
                int pos2 = 0;
                string temp = "";
                // 查找开始字符串
                string beginString = "<input ";
                string endString = ">";
                l = beginString.Length;
                while (pos1 >= 0)
                {
                    // 查找input开始位置pos1
                    pos1 = findHtml.IndexOf(beginString, pos2);
                    if (pos1 >= 0)
                    {
                        // 查找标记结束位置pos2
                        pos2 = findHtml.IndexOf(endString, pos1);
                        if (pos2 > 0)
                        {
                            // 获取input字符串
                            temp = srcHtml.Substring(pos1, pos2 - pos1 + 1);
                            if (this.getElementProperty(temp, "type").Trim() == "hidden")
                            {
                                string n = this.getElementProperty(temp, "name");
                                if (n.Length < 1)
                                {
                                    n = this.getElementProperty(temp, "id");
                                }
                                if (n.Length > 0)
                                {
                                    // 隐藏域则获取隐藏域数据
                                    string v = this.getElementProperty(temp, "value");
                                    if (ret.Length > 0)
                                    {
                                        ret += "&";
                                    }
                                    ret += (n + "=" + v);
                                }
                            }
                        }
                        pos2 = pos1 + l;
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// 取得相对链接全路径URL
        /// </summary>
        /// <param name="referenceUrl">当前引用页URL</param>
        /// <param name="relativeUrl">相对位置URL</param>
        /// <returns>全路径URL</returns>
        public string getFullUrl(string referenceUrl, string relativeUrl)
        {
            string refUrl = referenceUrl;
            string newUrl = relativeUrl;
            string ret = newUrl;
            // 检测相对地址是否已经是完整地址
            string checkUrl = newUrl.ToLower();
            // 绝对链接
            if (checkUrl.StartsWith("http://") || checkUrl.StartsWith("ftp://") || checkUrl.StartsWith("https://") || checkUrl.StartsWith("rtsp://") || checkUrl.StartsWith("mms://"))
            {
                return newUrl;
            }
            // 相对页为空则返回引用页URL
            if (newUrl == "" || newUrl == "?")
            {
                return refUrl;
            }
            // 去除相对页后的问号
            if (newUrl.EndsWith("?"))
            {
                newUrl = newUrl.Substring(0, newUrl.Length - 1);
            }
            // 初始化上级目录计数器
            int parentCount = 1;
            // 获取相对页的相对链接
            if (newUrl.StartsWith("/"))
            {
                // 当前目录
                ret = newUrl.Substring(1);
            }
            else if (newUrl.StartsWith("./"))
            {
                // 当前目录
                ret = newUrl.Substring(2);
            }
            else
            {
                while (ret.StartsWith("../"))
                {
                    // 取得上级目录总和
                    parentCount++;
                    ret = ret.Substring(3);
                }
            }
            int pos1 = 0;
            int pos2 = 0;
            string root = "";
            // 获取引用页连接类型长度
            int startPos = refUrl.IndexOf("://") + 3;
            if (!refUrl.EndsWith("/"))
            {
                pos1 = refUrl.LastIndexOf("/");
                pos2 = refUrl.LastIndexOf(".");
                root = refUrl + ((pos1 > pos2 || pos1 < startPos) ? "/" : "");
            }
            else
            {
                root = refUrl;
            }
            // 修正引用页地址级数
            for (int i = 0; i < parentCount; i++)
            {
                pos1 = root.LastIndexOf("/");
                if (pos1 > 7)
                {
                    root = root.Substring(0, pos1);
                }
            }
            if (!root.EndsWith("/")) root += "/";
            return root + ret;
        }
    }
}
