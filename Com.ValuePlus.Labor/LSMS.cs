using Com.ValuePlus.DAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Com.ValuePlus.Labor
{
    public class LSMS
    {
        /// <summary>
        /// 日志声明
        /// </summary>
        protected static Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 记录发送验证码后的记录
        /// </summary>
        /// <param name="strMobileNo"></param>
        /// <param name="strVerifyCode"></param>
        /// <returns></returns>
        public static string WriteSMSSendRecord(String strMobileNo, String strVerifyCode)
        {
            string strKeyReturn = "";
            try
            {
                StringBuilder sbSql = new StringBuilder();
                String strCurTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                String strKey = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                //String strKey = Guid.NewGuid().ToString();
                String strTableName = "LSMS_1";
                sbSql.Append("INSERT INTO LSMS_1(SKEY,MobileNo,GetTime,VerifyCode)");
                //sbSql.Append(" values ('" + strKey + "','" + strMobileNo + "','" + strCurTime + "','" + strVerifyCode + "')");
                sbSql.Append(" values (");
                sbSql.Append(JObjectToDB.GetColumnEncryptDataString(strTableName, "SKEY", strKey));
                sbSql.Append(","+JObjectToDB.GetColumnEncryptDataString(strTableName, "MobileNo", strMobileNo));
                sbSql.Append(","+JObjectToDB.GetColumnEncryptDataString(strTableName, "GetTime", strCurTime));
                sbSql.Append(","+JObjectToDB.GetColumnEncryptDataString(strTableName, "VerifyCode", strVerifyCode));
                sbSql.Append(" )");
                log.Error("LSMS.WriteSMSSendRecord 记录发送验证码后的记录,SQL:"+ sbSql.ToString());
                int iReturn = SqlParamDao.ExecuteNonQueryBySql(sbSql.ToString());
                if (iReturn > 0)
                {
                    strKeyReturn = strKey;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return strKeyReturn;
        }

        /// <summary>
        /// 短信接口发送
        /// </summary>
        /// <param name="strParam"></param>
        /// <returns></returns>
        public static string PostData(string strParam)
        {
            try
            {
                string purl = "http://service2.winic.org/service.asmx/SendMessages?"+ strParam;
                byte[] data = System.Text.Encoding.GetEncoding("UTF-8").GetBytes(strParam);
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
                log.Error("短信接口调用失败:" + ex);
                return "posterror";
            }
        }

    }
}
