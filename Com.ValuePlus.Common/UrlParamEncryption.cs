using System;
using System.Text;
using Com.ValuePlus.Utils.Cryptography;
using System.Collections;
using Com.ValuePlus.Common.Security;

namespace Com.ValuePlus.Common
{
    public class UrlParamEncryption
    {
        /// <summary>
        /// 加密将要传递到下个页面的参数字符串
        /// </summary>
        /// <param name="strParamString">要传递到下个页面的参数字符串</param>
        /// <returns></returns>
        public static String EncryptionUrlParam(String strParamString)
        {
            return Encrypt3des(strParamString, System.Text.Encoding.UTF8);

        }

        /// <summary>
        /// 解密传递到本页面的参数字符串
        /// </summary>
        /// <param name="strUrlQuery"></param>
        /// <returns></returns>
        public static Hashtable DecryptionUrlParam(String strUrlQuery)
        {
            Hashtable hash = new Hashtable();
            if (!String.IsNullOrEmpty(strUrlQuery))
            {
                //String strQueryUrl = strUrlQuery.Replace("?", "");
                String strQueryUrl = strUrlQuery.Remove(0,1);//去掉第一个字符（问号?）
                strQueryUrl = Decrypt3des(strQueryUrl, System.Text.Encoding.UTF8);//解密
                int num = 20;
                String[] strArray = new String[num];
                //if (strQueryUrl.IndexOf("&") > 0)
                //{
                String[] strArray2 = strQueryUrl.Split('&');
                for (int i = 0; i < strArray2.Length; i++)
                {
                    String[] strArray3 = strArray2[i].Split('=');
                    if (strArray3.Length == 2)
                    {
                        hash.Add(strArray3[0], strArray3[1]);
                    }
                    //else if (strArray3.Length == 3)//此种情况为：参数值中本身又带有参数的链接，如*.aspx?id=4&url=test.aspx?id=4
                    //{
                    //    hash.Add(strArray3[0], strArray3[1] +"="+ strArray3[2]);
                    //}
                    //modify by sammen 20140307
                    else if (strArray3.Length > 2)
                    {
                        //如果存在多个=号，则取第一个等号前的为参数，后面的全部为参数值
                        String ParamValue = strArray2[i].Substring(strArray3[0].Length+1, strArray2[i].Length - strArray3[0].Length-1);
                        //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329 由于链接是已经加密后传递进来的，可忽视SQL注入风险
                        //ParamValue = SQLInjectionDefense.ReplaceSQLReservedKeyword(ParamValue);
                        hash.Add(strArray3[0], ParamValue);
                    }
                }
            }
            return hash;            
        }

        /// <summary>
        /// 根据解密后的url参数字符串获取对应参数值
        /// </summary>
        /// <param name="htUrlQuery">解密后的url参数字符串</param>
        /// <param name="strParamName">参数名称</param>
        /// <returns></returns>
        public static String GetUrlParamValue(Hashtable htUrlQuery, String strParamName)
        {
            String strValue = "";
            if (htUrlQuery != null)
            {
                if (htUrlQuery.ContainsKey(strParamName))
                {
                    strValue = htUrlQuery[strParamName].ToString();
                }
            }
            return strValue;
        }


        #region //3des加密方法
        /// <summary>
        /// 3des加密
        /// </summary>
        /// <param name="strTobeEnCrypted"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string Encrypt3des(string strTobeEnCrypted, Encoding encoding)
        {
            return CryptographyHelper.Encrypt3des(new byte[] {0x11, 0x22, 0x5F, 0x68, (byte)0x88, 0x70, 0x40, 0x38
                , 0x28, 0x25, 0x79, 0x51, (byte)0xCB, (byte)0xDD, 0x55, 0x66
                , 0x77, 0x29, 0x74, (byte)0x98, 0x30, 0x40, 0x36, (byte)0xE2}, new byte[] { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF }, strTobeEnCrypted, encoding);

        }
        #endregion

        #region //3des解密方法
        /// <summary>
        /// 3des解密
        /// </summary>
        /// <param name="strTobeDeCrypted"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string Decrypt3des(string strTobeDeCrypted, Encoding encoding)
        {
            return CryptographyHelper.Decrypt3des(new byte[] {0x11, 0x22, 0x5F, 0x68, (byte)0x88, 0x70, 0x40, 0x38
                , 0x28, 0x25, 0x79, 0x51, (byte)0xCB, (byte)0xDD, 0x55, 0x66
                , 0x77, 0x29, 0x74, (byte)0x98, 0x30, 0x40, 0x36, (byte)0xE2}, new byte[] { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF }, strTobeDeCrypted, encoding);
        }
        #endregion
    }
}
