using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;

namespace Com.ValuePlus.Utils.HTTPRequest
{
    public class HttpXml
    {
        /// <summary>
        /// 发送xml的http请求
        /// </summary>
        /// <param name="request"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string SendHttpXml(string request, string url)
        {
            HttpWebRequest oWebReq = (HttpWebRequest)HttpWebRequest.Create(url) as HttpWebRequest;
            byte[] reqBytes = UTF8Encoding.UTF8.GetBytes(request);
            oWebReq.ContentLength = reqBytes.Length;
            oWebReq.Method = "POST";
            oWebReq.ContentType = "text/xml";
            using (Stream oReqStream = oWebReq.GetRequestStream())
            {
                oReqStream.Write(reqBytes, 0, reqBytes.Length);
            }
            string resultCode = string.Empty;
            try
            {
                using (WebResponse response = oWebReq.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        resultCode = reader.ReadToEnd().ToString();
                    }
                }

            }
            catch (Exception ex)
            {
                Com.ValuePlus.Log.LogFactory.CreateInstance("SendHttpXml").Error("发送web请求错误Data:" + request + "\nURL=" + url, ex);
            }
            return resultCode;
        }
    }
}
