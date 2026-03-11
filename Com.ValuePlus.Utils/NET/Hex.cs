using System;

//16进制的转换
namespace Com.ValuePlus.Utils.NET
{
	/// <summary>
	/// Hex 的摘要说明。
	/// </summary>
	public class Hex
	{
#region //构造函数
		private  Hex()
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//
		}
		#endregion

#region //转成16进制
       /// <summary>
        /// 转成16进制
       /// </summary>
       /// <param name="str"></param>
       /// <returns></returns>
		public static byte[] decode(string  str)
		{
			byte[]          bytes = new byte[str.Length / 2];
			string          buf = str.ToLower();		
			for (int i = 0; i < buf.Length ; i += 2)
			{
				char    left  = Convert.ToChar(buf.Substring(i,1));
				char    right =  Convert.ToChar(buf.Substring(i+1,1));
				int     index = i / 2;			
				if (left < 'a')
				{
					bytes[index] = (byte)((left - '0') << 4);
				}
				else
				{
					bytes[index] = (byte)((left - 'a' + 10) << 4);
				}
				if (right < 'a')
				{
					bytes[index] += (byte)(right - '0');
				}
				else
				{
					bytes[index] += (byte)(right - 'a' + 10);
				}
			}
			return bytes;
		}   
		#endregion

#region //将16进制转成对应的字符型数据
        /// <summary>
        /// 将16进制转成对应的字符型数据
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
		public static string encode(byte[] str)
		{
			string result = "";
			for(int i = 0 ;i<str.Length ;i ++)
			{
				byte left = (byte)( str[i] >> 4);
				byte right = (byte)( str[i] & 0x0f);		
				result = result + intTochar(left) + intTochar(right);
			}
			return result.ToUpper();
		}
		#endregion

#region //将其设置成相应的16进制的字符
        /// <summary>
        /// 将其设置成相应的16进制的字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
		public static char intTochar(byte str)
		{
			int i;
			if(str>=10)
			{
				i = str - 10;
				switch(i)
				{
					case 0:
						return 'a';
					case 1:
						return 'b';
					case 2:
						return 'c';
					case 3:
						return 'd';
					case 4:
						return 'e';
					case 5:
						return 'f';
					default:
						return 'a';
				}
			}
			else
			{
				switch(str)
				{
					case 0:
						return '0';
					case 1:
						return '1';
					case 2:
						return '2';
					case 3:
						return '3';
					case 4:
						return '4';
					case 5:
						return '5';
					case 6:
						return '6';
					case 7:
						return '7';
					case 8:
						return '8';
					case 9:
						return '9';
					default:
						return '0';
				}		
			}
		}
		#endregion

	}
}
