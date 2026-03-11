using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace Com.ValuePlus.Weixin
{
    /// <summary>    /// 
    /// HttpRequestUtil：http请求与响应辅助类
    /// </summary>
    public class HttpRequestHelper
    {
        /// <summary>
        /// 向微信服务器发送请求时的编码
        /// </summary>
        public static readonly Encoding RequestEncoding = Encoding.UTF8;
        /// <summary>
        /// 微信服务器响应的编码
        /// </summary>
        public static readonly Encoding ResponseEncoding = Encoding.UTF8;

        /// <summary>
        /// Post 提交调用抓取
        /// </summary>
        /// <param name="url">提交地址</param>
        /// <param name="jsonData">json数据</param>
        /// <returns>string</returns>
        public static string RequestPostData(string url, string jsonData)
        {
            byte[] bs = RequestEncoding.GetBytes(jsonData);
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
            req.Method = "Post";
            req.Timeout = 120 * 1000;
            req.ContentType = "application/x-www-form-urlencoded;";
            req.ContentLength = bs.Length;

            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(bs, 0, bs.Length);
                reqStream.Flush();
            }

            using (WebResponse wr = req.GetResponse())
            {
                //在这里对接收到的页面内容进行处理
                Stream strm = wr.GetResponseStream();
                StreamReader sr = new StreamReader(strm, RequestEncoding);

                string line;
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                while ((line = sr.ReadLine()) != null)
                {
                    sb.Append(line + System.Environment.NewLine);
                }
                sr.Close();
                strm.Close();
                return sb.ToString();
            }
        }

        #region 请求Url，不发送数据
        /// <summary>
        /// 请求Url，不发送数据
        /// </summary>
        public static string RequestUrl(string url)
        {
            return RequestUrl(url, "POST");
        }

        /// <summary>
        /// 请求Url，不发送数据
        /// </summary>
        public static string RequestUrl(string url, string method)
        {
            // 设置参数
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            CookieContainer cookieContainer = new CookieContainer();
            request.CookieContainer = cookieContainer;
            request.AllowAutoRedirect = true;
            request.Method = method;
            request.ContentType = "text/html";
            request.Headers.Add("charset", "utf-8");

            //发送请求并获取相应回应数据
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            //直到request.GetResponse()程序才开始向目标网页发送Post请求
            Stream responseStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(responseStream, RequestEncoding);
            //返回结果网页（html）代码
            string content = sr.ReadToEnd();
            return content;
        }
        #endregion

    }
}
