using Com.ValuePlus.SysParams;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Com.ValuePlus.BLL.SMS
{
    public class SendSMS
    {
        private static string strSMS_UID = BaseParamsGetter.GetBasicParamValue("smsAccountId");
        private static string strSMS_PWD = BaseParamsGetter.GetBasicParamValue("smsPassword");
        private static string strSMS_URL = "http://service2.winic.org/service.asmx/SendMessages?";

        /// <summary>
        /// 日志声明
        /// </summary>
        protected static Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// 发送验证码短信
        /// </summary>
        /// <param name="context"></param>
        /// <param name="strRequestLanguage"></param>
        /// <param name="strRequestLanguage"></param>
        public static String DoSendMessage(String strMobileNo,String strSendMsg)
        {
            String strReturnResult = "";
            try
            {
                String strSMSParam = "uid=" + strSMS_UID + "&pwd=" + strSMS_PWD + "&tos=" + strMobileNo + "&msg=" + strSendMsg + "&otime=";

                strReturnResult = PostData(strSMS_URL,strSMSParam);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return strReturnResult;
        }

        /// <summary>
        /// 短信接口发送
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string PostData(string purl,string str)
        {
            try
            {
                byte[] data = System.Text.Encoding.GetEncoding("UTF-8").GetBytes(str);
                // 准备请求    
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(purl);
                //设置超时     
                req.Timeout = 30000;
                req.Method = "Post";
                req.ContentType = "application/x-www-form-urlencoded";
                req.ContentLength = data.Length;
                Stream stream = req.GetRequestStream();
                // 发送数据   
                stream.Write(data, 0, data.Length);
                stream.Close();

                HttpWebResponse rep = (HttpWebResponse)req.GetResponse();
                Stream receiveStream = rep.GetResponseStream();
                Encoding encode = System.Text.Encoding.GetEncoding("UTF-8");
                // Pipes the stream to a higher level stream reader with the required encoding format.   
                StreamReader readStream = new StreamReader(receiveStream, encode);

                Char[] read = new Char[256];
                int count = readStream.Read(read, 0, 256);
                StringBuilder sb = new StringBuilder("");
                while (count > 0)
                {
                    String readstr = new String(read, 0, count);
                    sb.Append(readstr);
                    count = readStream.Read(read, 0, 256);
                }

                rep.Close();
                readStream.Close();

                return sb.ToString();
            }
            catch (Exception ex)
            {
                log.Error("Com.ValuePlus.BLL.SMS短信接口调用失败:" + ex);
                return "";
            }
        }

    }
}
