using System;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Web;
using System.Xml.Serialization;
using System.Xml;
using System.Text;

namespace Com.ValuePlus.Utils.Serializable
{
    public class SerializableHelper
    {


        #region 将数据流反序列化为对象
        /// <summary>
        /// 将数据流反序列化为对象
        /// </summary>
        /// <param name="type"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static object StreamDeserializeObject(Type type, Stream stream, System.Text.Encoding encoding)
        {
            try
            {
                using (StreamReader reader = new StreamReader(stream, encoding))
                {
                    XmlSerializer serializer = new XmlSerializer(type);
                    return serializer.Deserialize(reader);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }
        #endregion


        #region XMLDeserialize
        /// <summary>
        /// xml反序列化
        /// </summary>
        /// <param name="str"></param>
        /// <param name="type"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static object XMLDeserialize(string str, Type type, System.Text.Encoding encoding)
        {
            object data = new object();
            try
            {
                XmlSerializer serializer = new XmlSerializer(type);
                byte[] binaryData = encoding.GetBytes(str);
                using (MemoryStream streamMemory = new MemoryStream(binaryData))
                {
                    data = serializer.Deserialize(streamMemory);
                }
            }
            catch (Exception ex)
            {
                data = null;
                throw ex;
            }
            finally
            {
            }
            return data;
        }
        #endregion

        #region XMLSerialize
        /// <summary>
        /// xml序列化
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string XMLSerialize(object obj, System.Text.Encoding encoding)
        {
            string str = string.Empty;
            try
            {
                using (MemoryStream memory = new MemoryStream())
                {
                    XmlSerializer serializer = new XmlSerializer(obj.GetType());
                    serializer.Serialize(memory, obj);
                    string binaryData = encoding.GetString(memory.GetBuffer());
                    str = binaryData;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return str;


        }
        #endregion
    }
}
