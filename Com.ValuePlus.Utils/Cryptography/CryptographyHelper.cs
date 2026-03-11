using System;
using System.IO;
using System.Web ;
using System.Text;
using System.Security.Cryptography;
using Com.ValuePlus.Utils.NET;
using System.Web.Security;

//加密与解密
namespace Com.ValuePlus.Utils.Cryptography
{
	/// <summary>
	/// CryptographyHelper 的摘要说明。
	/// </summary>
	public class CryptographyHelper
	{

#region //构造函数
		private  CryptographyHelper()
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//
		}
		#endregion

#region 进行DES加密       
        /// <summary>
        /// 进行DES加密。
        /// </summary>
        /// <param name="pToEncrypt">要加密的字符串。</param>
        /// <param name="sKey">密钥，且必须为8位。</param>
        /// <param name="pIV">向量</param>
        /// <param name="encoding"></param>
        /// <returns>以Base64格式返回的加密字符串。</returns>
        public static  string Encrypt(string pToEncrypt, string sKey, byte[] pIV, Encoding encoding)
        {
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                byte[] inputByteArray = encoding.GetBytes(pToEncrypt);
                des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                des.IV = pIV;// new byte[] { 227, 105, 5, 40, 162, 158, 143, 156 };
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    cs.Close();
                }
                string str = Convert.ToBase64String(ms.ToArray());
                ms.Close();
                return str;
            }
        }
        #endregion

#region 进行DES解密
        /**/
        /// <summary>
        /// 进行DES解密。
        /// </summary>
        /// <param name="pToDecrypt">要解密的以Base64</param>
        /// <param name="sKey">密钥，且必须为8位。</param>
        /// <param name="pIV">向量</param>
        /// <param name="encoding"></param>
        /// <returns>已解密的字符串。</returns>
        public static  string Decrypt(string pToDecrypt, string sKey, byte[] pIV, Encoding encoding)
        {
            byte[] inputByteArray = Convert.FromBase64String(pToDecrypt);
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                des.IV = pIV;// new byte[] { 227, 105, 5, 40, 162, 158, 143, 156 };
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    cs.Close();
                }
                string str = encoding.GetString(ms.ToArray());
                ms.Close();
                return str;
            }
        }
        #endregion

#region //3des加密方法
        /// <summary>
        /// 3des加密
        /// </summary>
        /// <param name="pKey"></param>
        /// <param name="pIV"></param>
        /// <param name="strTobeEnCrypted"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string Encrypt3des(byte[] pKey, byte[] pIV, string strTobeEnCrypted, Encoding encoding)
		{
			System.Security.Cryptography.ICryptoTransform ct;
			System.IO.MemoryStream  ms;
			System.Security.Cryptography.CryptoStream  cs;
			System.Security.Cryptography.SymmetricAlgorithm mCSP;
			byte[] bytCode;
			byte[] bytIV;
			byte[] bytValue;	
			//将key转化成字节数组	
			bytCode = pKey;
			//将加密向量转化成字节数组
			bytIV	= pIV;
			//创建3des加密对象
			mCSP = new System.Security.Cryptography.TripleDESCryptoServiceProvider();			
			mCSP.Key = bytCode;
			mCSP.IV = bytIV;
            bytValue = encoding.GetBytes(strTobeEnCrypted);
			ct = mCSP.CreateEncryptor();
			ms = new MemoryStream();			
			cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
			cs.Write(bytValue, 0, bytValue.Length);
			cs.FlushFinalBlock();	
			cs.Close();
			return Convert.ToBase64String(ms.ToArray());		
		}
		#endregion

#region //3des加密方法
        /// <summary>
        /// 3des加密
        /// </summary>
        /// <param name="pKey"></param>
        /// <param name="pIV"></param>
        /// <param name="strTobeEnCrypted"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string Encrypt3des(string pKey, string pIV, string strTobeEnCrypted, Encoding encoding)
		{
			System.Security.Cryptography.ICryptoTransform ct;
			System.IO.MemoryStream  ms;
			System.Security.Cryptography.CryptoStream  cs;
			System.Security.Cryptography.SymmetricAlgorithm mCSP;
			byte[] bytCode;
			byte[] bytIV =null;
			byte[] bytValue;	
			//将key转化成字节数组	
			bytCode = Hex.decode(pKey);
            if (!string.IsNullOrEmpty(pIV))
            {
                //将加密向量转化成字节数组
                bytIV = Hex.decode(pIV);
            }
			//创建3des加密对象
			mCSP = new System.Security.Cryptography.TripleDESCryptoServiceProvider();			
			mCSP.Key = bytCode;
            if (!string.IsNullOrEmpty(pIV))
            {
                mCSP.IV = bytIV;
            }
            bytValue = encoding.GetBytes(strTobeEnCrypted);
			ct = mCSP.CreateEncryptor();
			ms = new MemoryStream();			
			cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
			cs.Write(bytValue, 0, bytValue.Length);
			cs.FlushFinalBlock();	
			cs.Close();
			return Convert.ToBase64String(ms.ToArray());		
		}
		#endregion

#region //3des解密方法
        /// <summary>
        /// 3des解密
        /// </summary>
        /// <param name="pKey"></param>
        /// <param name="pIV"></param>
        /// <param name="strTobeDeCrypted"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string Decrypt3des(byte[] pKey, byte[] pIV, string strTobeDeCrypted, Encoding encoding)
		{
			System.Security.Cryptography.ICryptoTransform ct;
			System.IO.MemoryStream  ms;
			System.Security.Cryptography.CryptoStream  cs;
			System.Security.Cryptography.SymmetricAlgorithm mCSP;
			byte[] bytCode;
			byte[] bytIV;
			byte[] bytValue;	
			//将key转化成字节数组	
			bytCode = pKey;
			//将加密向量转化成字节数组
			bytIV	= pIV;
			//创建3des加密对象
			mCSP = new System.Security.Cryptography.TripleDESCryptoServiceProvider();		
			mCSP.Key = bytCode;
			mCSP.IV = bytIV;
			bytValue = Convert.FromBase64String(strTobeDeCrypted );
			ct = mCSP.CreateDecryptor();
			ms = new MemoryStream();			
			cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
			cs.Write(bytValue, 0, bytValue.Length);
			cs.FlushFinalBlock();	
			cs.Close();
            return encoding.GetString(ms.ToArray());
		}
		#endregion

#region //3des解密方法
        /// <summary>
        /// 3des解密
        /// </summary>
        /// <param name="pKey"></param>
        /// <param name="pIV"></param>
        /// <param name="strTobeDeCrypted"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
		public static string Decrypt3des( string pKey, string pIV, string strTobeDeCrypted,Encoding encoding )
		{
			System.Security.Cryptography.ICryptoTransform ct;
			System.IO.MemoryStream  ms;
			System.Security.Cryptography.CryptoStream  cs;
			System.Security.Cryptography.SymmetricAlgorithm mCSP;
			byte[] bytCode;
			byte[] bytIV=null;
			byte[] bytValue;	
			//将key转化成字节数组	
			bytCode = Hex.decode(pKey);
            if(!string.IsNullOrEmpty(pIV))
            {
			//将加密向量转化成字节数组
			bytIV	= Hex.decode(pIV);
            }
			//创建3des加密对象
			mCSP = new System.Security.Cryptography.TripleDESCryptoServiceProvider();		
			mCSP.Key = bytCode;
            if (!string.IsNullOrEmpty(pIV))
            {
                mCSP.IV = bytIV;
            }
			bytValue = Convert.FromBase64String(strTobeDeCrypted);
			ct = mCSP.CreateDecryptor();
			ms = new MemoryStream();			
			cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
			cs.Write(bytValue, 0, bytValue.Length);
			cs.FlushFinalBlock();	
			cs.Close();
            return encoding.GetString(ms.ToArray());
		}
		#endregion

#region //SHA1加密方法
        /// <summary>
        /// sha1加密
        /// </summary>
        /// <param name="strTobeDigest"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string SHA1(string strTobeDigest, Encoding encoding)
		{
            byte[] input = encoding.GetBytes(strTobeDigest);
			System.Security.Cryptography.SHA1CryptoServiceProvider sha1obj = new SHA1CryptoServiceProvider();
			byte[] sResult;
			sResult = sha1obj.ComputeHash(input);
			return Convert.ToBase64String(sResult);		
		}
#endregion

#region //MD5加密方法
        /// <summary>
        /// md5加密,返回小写
        /// </summary>
        /// <param name="req"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
		public static string MD5(string req,Encoding encoding)
		{
            byte[] input = encoding.GetBytes(req);
			System.Security.Cryptography.MD5CryptoServiceProvider  MD5obj = new MD5CryptoServiceProvider();
			byte[] sResult;
			sResult = MD5obj.ComputeHash(input);
			StringBuilder md5Result = new StringBuilder();
			for(int i=0;i<sResult.Length ;i++){
				md5Result.Append( sResult[i].ToString("x2"));
			}      
			return md5Result.ToString().ToLower();
		}
		#endregion

#region //MD5加密方法
        /// <summary>
        /// md5加密,返回小写
        /// </summary>
        /// <param name="req"></param>
        /// <param name="key"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string MD5(string req, string key, Encoding encoding)
		{
            byte[] input = encoding.GetBytes(req + key);
			System.Security.Cryptography.MD5CryptoServiceProvider  MD5obj = new MD5CryptoServiceProvider();
			byte[] sResult;
			sResult = MD5obj.ComputeHash(input);
			StringBuilder md5Result = new StringBuilder();
			for(int i=0;i<sResult.Length ;i++)
			{
				md5Result.Append( sResult[i].ToString("x2"));
			}      
			return md5Result.ToString().ToLower();
		}
		#endregion
       
#region base64 解码字符串 
       /// <summary>
       /// base64解码
       /// </summary>
       /// <param name="sInputString"></param>
        /// <param name="encoding"></param>
       /// <returns></returns>
        public static string DecryptString(string sInputString, Encoding encoding)
        {
          char[] sInput = sInputString.ToCharArray();
          try{
            byte[] bOutput = System.Convert.FromBase64String(sInputString);
            return encoding.GetString(bOutput);
          }
          catch ( System.ArgumentNullException ){
            //base 64 字符数组为null
            return "";
          }
          catch ( System.FormatException ) {
            //长度错误，无法整除4
            return "";
          }      
    }
#endregion

#region base64编码字符串
   /// <summary>
   /// base64加密
   /// </summary>
        /// <param name="sInputString"></param>
        /// <param name="encoding"></param>
   /// <returns></returns>
        public static string EncryptString(string sInputString, Encoding encoding)
        {
            byte[] bInput = encoding.GetBytes(sInputString);
      try {
        return System.Convert.ToBase64String(bInput,0,bInput.Length);
      }
      catch (System.ArgumentNullException){
        //二进制数组为NULL.
        return "";
      }
      catch (System.ArgumentOutOfRangeException){
        //长度不够
        return "";
      }
  }
    #endregion

#region SHA256函数
  /// <summary>
  /// SHA256函数
  /// </summary>
        /// <param name="str">原始字符串</param>
        /// <param name="encoding"></param>
  /// <returns>SHA256结果</returns>
        public static string SHA256(string str, Encoding encoding)
  {
      byte[] SHA256Data = encoding.GetBytes(str);
      SHA256Managed Sha256 = new SHA256Managed();
      byte[] Result = Sha256.ComputeHash(SHA256Data);
      return Convert.ToBase64String(Result);  //返回长度为44字节的字符串
  }
  #endregion

#region 通用加密
  /// <summary>
        /// 通用加密
        /// </summary>
        /// <param name="str"></param>
        /// <param name="cryptype"></param>
        /// <returns></returns>
        public static string SecurityConvert(String str, String cryptype)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(str, cryptype.ToUpper());
        }
        /// <summary>
        /// 求字符串的SHA1哈希值
        /// </summary>
        /// <param name="str">待加密得字符串</param>
        /// <returns></returns>
        public static string EncryptSHA1(string str)
        {
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "SHA1");
        }
        /// <summary>
        /// 求字符串的MD5哈希值
        /// </summary>
        /// <param name="str">待加密得字符串</param>
        /// <returns></returns>
        public static string EncryptMD5(string str)
        {
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5");
        }
  #endregion

    }
}
