using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.IO;
namespace Com.ValuePlus.Utils
{
    /// <summary>
    /// 请求操作通用类
    /// </summary>
    public class RequestUtils
    {
        #region 获得当前绝对路径
        /// <summary>
        /// 获得当前绝对路径
        /// </summary>
        /// <param name="strPath">指定的路径</param>
        /// <returns>绝对路径</returns>
        public static string GetMapPath(string strPath)
        {
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Server.MapPath(strPath);
            }
            else //非web程序引用
            {
                return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strPath);
            }
        }
        #endregion

        #region 以指定的ContentType输出指定文件文件
        /// <summary>
        /// 以指定的ContentType输出指定文件文件
        /// </summary>
        /// <param name="filepath">文件路径</param>
        /// <param name="filename">输出的文件名</param>
        /// <param name="filetype">将文件输出时设置的ContentType</param>
        public static void ResponseFile(string filepath, string filename, string filetype)
        {
            Stream iStream = null;

            // 缓冲区为10k
            byte[] buffer = new Byte[10000];

            // 文件长度
            int length;

            // 需要读的数据长度
            long dataToRead;

            try
            {
                // 打开文件
                iStream = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                // 需要读的数据长度
                dataToRead = iStream.Length;

                HttpContext.Current.Response.ContentType = filetype;
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + HtmlUtils.UrlEncode(filename.Trim()).Replace("+", " "));

                while (dataToRead > 0)
                {
                    // 检查客户端是否还处于连接状态
                    if (HttpContext.Current.Response.IsClientConnected)
                    {
                        length = iStream.Read(buffer, 0, 10000);
                        HttpContext.Current.Response.OutputStream.Write(buffer, 0, length);
                        HttpContext.Current.Response.Flush();
                        buffer = new Byte[10000];
                        dataToRead = dataToRead - length;
                    }
                    else
                    {
                        // 如果不再连接则跳出死循环
                        dataToRead = -1;
                    }
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.Write("Error : " + ex.Message);
            }
            finally
            {
                if (iStream != null)
                {
                    // 关闭文件
                    iStream.Close();
                }
            }
            HttpContext.Current.Response.End();
        }
        #endregion

        #region 下载
        /// <summary>
        /// 下载
        /// </summary>
        /// <param name="downloadURL"></param>
        /// <returns></returns>
        public MemoryStream request(string downloadURL)
        {
            MemoryStream memory = new MemoryStream();
              Com.ValuePlus.Utils.HTTPRequest.HttpUrl request = new   Com.ValuePlus.Utils.HTTPRequest.HttpUrl();
            String sUrl = downloadURL;
            String methodString = "GET";
              Com.ValuePlus.Utils.HTTPRequest.HttpResponse response = request.request(sUrl, string.Empty, methodString, string.Empty, string.Empty);
            if (response != null)
            {
                try
                {
                    using (BinaryReader r = new BinaryReader(response.getResponseStream()))
                    {
                        byte[] b = new byte[1024];
                        int iSuccess = 1;
                        while (iSuccess > 0)
                        {
                            iSuccess = r.Read(b, 0, b.Length);
                            if (iSuccess > 0)
                            {
                                memory.Write(b, 0, iSuccess);
                            }

                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }
            return memory;
        }
        #endregion

        #region 得到当前访问用户的ＩＰ
        /// <summary>
        /// 得到当前访问用户的ＩＰ
        /// </summary>
        /// <returns></returns>
        public static string GetIP()
        {
            string UserIp = string.Empty;
            try
            {
                //得到用户ip
                if (HttpContext.Current != null)
                {
                    string strIp = string.Empty;
                    if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
                    {
                        strIp = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
                    }
                    else
                    {
                        if (HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] != null)
                        {
                            strIp = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
                        }
                    }
                    if (strIp.Equals("127.0.0.1")|| strIp.Equals("::1"))
                    {
                        strIp = GetLocalhostIPAddress(true);
                    }

                    UserIp = strIp;
                }
            }
            catch
            {
                return String.Empty;
            }
            return UserIp;
        }

        /// <summary>
        /// 得到当前访问用户的ＩＰ
        /// </summary>
        /// <returns></returns>
        public static string GetIP(HttpContext context)
        {
            string UserIp = string.Empty;
            try
            {
                //得到用户ip
                if (context != null)
                {
                    string strIp = string.Empty;
                    if (context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
                    {
                        strIp = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
                    }
                    else
                    {
                        if (context.Request.ServerVariables["REMOTE_ADDR"] != null)
                        {
                            strIp = context.Request.ServerVariables["REMOTE_ADDR"].ToString();
                        }
                    }
                    if (strIp.Equals("127.0.0.1") || strIp.Equals("::1"))
                    {
                        strIp = GetLocalhostIPAddress(true);
                    }

                    UserIp = strIp;
                }
            }
            catch
            {
                return String.Empty;
            }
            return UserIp;
        }
        /// <summary> 
        /// 获取本机IP 
        /// </summary> 
        /// <returns></returns> 
        public static string GetLocalhostIPAddress(bool one)
        {
            string hostName = System.Net.Dns.GetHostName();
            System.Net.IPHostEntry hostInfo = System.Net.Dns.GetHostByName(hostName);
            System.Net.IPAddress[] IpAddr = hostInfo.AddressList;
            string localIP = string.Empty;
            for (int i = 0; i < IpAddr.Length; i++)
            {
                if (one)
                {
                    localIP = IpAddr[i].ToString();
                    break;
                }
                localIP += IpAddr[i].ToString();
            }
            return localIP;
        }

        #endregion

        #region 判断当前页面是否接收到了Post请求
        /// <summary>
        /// 判断当前页面是否接收到了Post请求
        /// </summary>
        /// <returns>是否接收到了Post请求</returns>
        public static bool IsPost()
        {
            return HttpContext.Current.Request.HttpMethod.Equals("POST");
        }
        #endregion

        #region 判断当前页面是否接收到了Get请求
        /// <summary>
        /// 判断当前页面是否接收到了Get请求
        /// </summary>
        /// <returns>是否接收到了Get请求</returns>
        public static bool IsGet()
        {
            return HttpContext.Current.Request.HttpMethod.Equals("GET");
        }
        #endregion

        #region 返回指定的服务器变量信息
        /// <summary>
        /// 返回指定的服务器变量信息
        /// </summary>
        /// <param name="strName">服务器变量名</param>
        /// <returns>服务器变量信息</returns>
        public static string GetServerString(string strName)
        {
            //
            if (HttpContext.Current.Request.ServerVariables[strName] == null)
            {
                return "";
            }
            return HttpContext.Current.Request.ServerVariables[strName].ToString();
        }
        #endregion

        #region  返回上一个页面的地址
        /// <summary>
        /// 返回上一个页面的地址
        /// </summary>
        /// <returns>上一个页面的地址</returns>
        public static string GetUrlReferrer()
        {
            string retVal = null;

            try
            {
                retVal = HttpContext.Current.Request.UrlReferrer.ToString();
            }
            catch { }

            if (retVal == null)
                return "";

            return retVal;

        }
        #endregion

        #region 得到当前完整主机头,包含虚拟目录
        /// <summary>
        /// 得到当前完整主机头,包含虚拟目录
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentFullHost()
        {
            HttpRequest request = System.Web.HttpContext.Current.Request;
            string sReturn = request.Url.Host;            
            if (!request.Url.IsDefaultPort)
            {
                sReturn = string.Format("{0}:{1}", request.Url.Host, request.Url.Port.ToString());
            }
            string sVirtualPath = request.ApplicationPath;
            if(!(string.IsNullOrEmpty(sVirtualPath) || sVirtualPath=="/")){
                if (sVirtualPath.IndexOf("/") == 0)
                {
                    sReturn = sReturn + sVirtualPath;
                }
                else
                {
                    sReturn = sReturn + "/" + sVirtualPath;
                }
            }
            return sReturn.TrimEnd(new char[]{'/'});
        }
        #endregion

        #region 得到主机头
        /// <summary>
        /// 得到主机头
        /// </summary>
        /// <returns></returns>
        public static string GetHost()
        {
            return HttpContext.Current.Request.Url.Host;
        }
        #endregion

        #region 获取当前请求的原始 URL(URL 中域信息之后的部分,包括查询字符串(如果存在))
        /// <summary>
        /// 获取当前请求的原始 URL(URL 中域信息之后的部分,包括查询字符串(如果存在))
        /// </summary>
        /// <returns>原始 URL</returns>
        public static string GetRawUrl()
        {
            return HttpContext.Current.Request.RawUrl;
        }
        #endregion

        #region 判断当前访问是否来自浏览器软件
        /// <summary>
        /// 判断当前访问是否来自浏览器软件
        /// </summary>
        /// <returns>当前访问是否来自浏览器软件</returns>
        public static bool IsBrowserGet()
        {
            string[] BrowserName = { "ie", "opera", "netscape", "mozilla", "konqueror", "firefox" };
            string curBrowser = HttpContext.Current.Request.Browser.Type.ToLower();
            for (int i = 0; i < BrowserName.Length; i++)
            {
                if (curBrowser.IndexOf(BrowserName[i]) >= 0)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region 判断是否来自搜索引擎链接
        /// <summary>
        /// 判断是否来自搜索引擎链接
        /// </summary>
        /// <returns>是否来自搜索引擎链接</returns>
        public static bool IsSearchEnginesGet()
        {
            if (HttpContext.Current.Request.UrlReferrer == null)
            {
                return false;
            }
            string[] SearchEngine = { "google", "yahoo", "msn", "baidu", "sogou", "sohu", "sina", "163", "lycos", "tom", "yisou", "iask", "soso", "gougou", "zhongsou" };
            string tmpReferrer = HttpContext.Current.Request.UrlReferrer.ToString().ToLower();
            for (int i = 0; i < SearchEngine.Length; i++)
            {
                if (tmpReferrer.IndexOf(SearchEngine[i]) >= 0)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region 获得当前完整Url地址
        /// <summary>
        /// 获得当前完整Url地址
        /// </summary>
        /// <returns>当前完整Url地址</returns>
        public static string GetUrl()
        {
            return HttpContext.Current.Request.Url.ToString();
        }
        #endregion

        #region 获得指定Url参数的值
        /// <summary>
        /// 获得指定Url参数的值
        /// </summary>
        /// <param name="strName">Url参数</param>
        /// <returns>Url参数的值</returns>
        public static string GetQueryString(string strName)
        {
            if (HttpContext.Current.Request.QueryString[strName] == null)
            {
                return "";
            }
            return HttpContext.Current.Request.QueryString[strName];
        }
        #endregion

        #region 获得当前页面的名称
        /// <summary>
        /// 获得当前页面的名称
        /// </summary>
        /// <returns>当前页面的名称</returns>
        public static string GetPageName()
        {
            string[] urlArr = HttpContext.Current.Request.Url.AbsolutePath.Split('/');
            return urlArr[urlArr.Length - 1].ToLower();
        }
        #endregion

        #region 返回表单或Url参数的总个数
        /// <summary>
        /// 返回表单或Url参数的总个数
        /// </summary>
        /// <returns></returns>
        public static int GetParamCount()
        {
            return HttpContext.Current.Request.Form.Count + HttpContext.Current.Request.QueryString.Count;
        }
        #endregion

        #region 获得指定表单参数的值
        /// <summary>
        /// 获得指定表单参数的值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <returns>表单参数的值</returns>
        public static string GetFormString(string strName)
        {
            if (HttpContext.Current.Request.Form[strName] == null)
            {
                return "";
            }
            return HttpContext.Current.Request.Form[strName];
        }
        #endregion

        #region 获得Url或表单参数的值, 先判断Url参数是否为空字符串, 如为True则返回表单参数的值
        /// <summary>
        /// 获得Url或表单参数的值, 先判断Url参数是否为空字符串, 如为True则返回表单参数的值
        /// </summary>
        /// <param name="strName">参数</param>
        /// <returns>Url或表单参数的值</returns>
        public static string GetString(string strName)
        {
            if ("".Equals(GetQueryString(strName)))
            {
                return GetFormString(strName);
            }
            else
            {
                return GetQueryString(strName);
            }
        }
        #endregion

        #region 获得指定Url参数的int类型值
        /// <summary>
        /// 获得指定Url参数的int类型值
        /// </summary>
        /// <param name="strName">Url参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>Url参数的int类型值</returns>
        public static int GetQueryInt(string strName, int defValue)
        {
            return TypeParse.StrToInt(HttpContext.Current.Request.QueryString[strName], defValue);
        }
        #endregion

        #region 获得指定表单参数的int类型值
        /// <summary>
        /// 获得指定表单参数的int类型值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>表单参数的int类型值</returns>
        public static int GetFormInt(string strName, int defValue)
        {
            return TypeParse.StrToInt(HttpContext.Current.Request.Form[strName], defValue);
        }
        #endregion

        #region 获得指定Url或表单参数的int类型值, 先判断Url参数是否为缺省值, 如为True则返回表单参数的值
        /// <summary>
        /// 获得指定Url或表单参数的int类型值, 先判断Url参数是否为缺省值, 如为True则返回表单参数的值
        /// </summary>
        /// <param name="strName">Url或表单参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>Url或表单参数的int类型值</returns>
        public static int GetInt(string strName, int defValue)
        {
            if (GetQueryInt(strName, defValue) == defValue)
            {
                return GetFormInt(strName, defValue);
            }
            else
            {
                return GetQueryInt(strName, defValue);
            }
        }
        #endregion

        #region 获得指定Url参数的float类型值
        /// <summary>
        /// 获得指定Url参数的float类型值
        /// </summary>
        /// <param name="strName">Url参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>Url参数的int类型值</returns>
        public static float GetQueryFloat(string strName, float defValue)
        {
            return TypeParse.StrToFloat(HttpContext.Current.Request.QueryString[strName], defValue);
        }
        #endregion

        #region 获得指定表单参数的float类型值
        /// <summary>
        /// 获得指定表单参数的float类型值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>表单参数的float类型值</returns>
        public static float GetFormFloat(string strName, float defValue)
        {
            return TypeParse.StrToFloat(HttpContext.Current.Request.Form[strName], defValue);
        }
        #endregion

        #region 获得指定Url或表单参数的float类型值, 先判断Url参数是否为缺省值, 如为True则返回表单参数的值
        /// <summary>
        /// 获得指定Url或表单参数的float类型值, 先判断Url参数是否为缺省值, 如为True则返回表单参数的值
        /// </summary>
        /// <param name="strName">Url或表单参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>Url或表单参数的int类型值</returns>
        public static float GetFloat(string strName, float defValue)
        {
            if (GetQueryFloat(strName, defValue) == defValue)
            {
                return GetFormFloat(strName, defValue);
            }
            else
            {
                return GetQueryFloat(strName, defValue);
            }
        }
        #endregion

        #region 保存用户上传的文件
        /// <summary>
        /// 保存用户上传的文件
        /// </summary>
        /// <param name="path">保存路径</param>
        public static void SaveRequestFile(string path)
        {
            if (HttpContext.Current.Request.Files.Count > 0)
            {
                HttpContext.Current.Request.Files[0].SaveAs(path);
            }
        }
        #endregion

        #region 下载
        /// <summary>
        /// 下载
        /// </summary>
        /// <param name="fileName"></param>
        public static void DownLoadFile(string fileName,string sPath)
        {
            System.IO.FileInfo fileInfo = new System.IO.FileInfo(sPath);
            try
            {
                //一次性下载文件
                System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode(fileName, Encoding.UTF8));
                System.Web.HttpContext.Current.Response.AddHeader("Content-Length", fileInfo.Length.ToString());
                System.Web.HttpContext.Current.Response.Charset = "UTF-8";
                System.Web.HttpContext.Current.Response.ContentType = "application/octet-stream";
                byte[] buffer = new byte[fileInfo.Length];
                using (System.IO.FileStream fileStream = fileInfo.OpenRead())
                {
                    fileStream.Read(buffer, 0, buffer.Length);
                }
                System.Web.HttpContext.Current.Response.BinaryWrite(buffer);
                System.Web.HttpContext.Current.Response.Flush();
                System.Web.HttpContext.Current.Response.End();

            }catch(Exception ex){
                ////如果报错，说明文件太大，则每次读取文件，只读取100K，这样可以缓解服务器的压力
                const long ChunkSize = 102400;//100K 
                byte[] buffer = new byte[ChunkSize];

                System.Web.HttpContext.Current.Response.Clear();
                System.IO.FileStream iStream = System.IO.File.OpenRead(sPath);
                long dataLengthToRead = iStream.Length;//获取下载的文件总大小
                System.Web.HttpContext.Current.Response.ContentType = "application/octet-stream";
                System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(fileName));
                while (dataLengthToRead > 0 && System.Web.HttpContext.Current.Response.IsClientConnected)
                {
                    int lengthRead = iStream.Read(buffer, 0, Convert.ToInt32(ChunkSize));//读取的大小
                    System.Web.HttpContext.Current.Response.OutputStream.Write(buffer, 0, lengthRead);
                    System.Web.HttpContext.Current.Response.Flush();
                    dataLengthToRead = dataLengthToRead - lengthRead;
                }
                System.Web.HttpContext.Current.Response.Close();
                //return true;
            }
        }
        #endregion

        #region 获得请求的内容体的数据
        /// <summary>
        /// 获得请求的内容体的数据
        /// </summary>
        /// <param name="stream">request.inputstream</param>
        /// <returns>内容</returns>
        public static string getRequestContent(Stream stream)
        {
            using (BinaryReader br = new BinaryReader(stream))
            {
                byte[] b = new byte[stream.Length];
                br.Read(b, 0, b.Length);
                char[] c = Encoding.UTF8.GetChars(b);
                string str = new string(c);
                return str;
            }


            //BinaryReader br = new BinaryReader(Request.InputStream);
            //byte[] b = new byte[Request.InputStream.Length];
            //br.Read(b, 0, b.Length);
            //char[] c = Encoding.UTF8.GetChars(b);
            //string str = new string(c);

            //Response.Write(str);



            ////Stream stream  = Request.InputStream;
            ////StreamReader sr = new StreamReader(stream, Encoding.UTF8);           

            ////string s = sr.ReadToEnd();
            ////sr.Close();
            ////sr.Dispose();
            ////stream.Close();
            ////stream.Dispose();
            ////Response.Write(s);

        }
        #endregion

        #region 获得虚拟路径的根目录
        public static string ApplicationRootPath()
        {
            string path;
            if (string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.Url.Port.ToString()))
            {
                path = System.Web.HttpContext.Current.Request.Url.Host + System.Web.HttpContext.Current.Request.ApplicationPath.TrimEnd('/');
            }
            else
            {
                path = (System.Web.HttpContext.Current.Request.Url.Host + ":" + System.Web.HttpContext.Current.Request.Url.Port + "/" + System.Web.HttpContext.Current.Request.ApplicationPath.TrimEnd('/')).TrimEnd('/');
            }
            return path;
        }
        #endregion

        #region 获得根目录的实际的物理地址
        public static string ApplicationPhysicPath()
        {
            return System.Web.HttpContext.Current.Server.MapPath(System.Web.HttpContext.Current.Request.ApplicationPath);
        }
        #endregion


    }
}
