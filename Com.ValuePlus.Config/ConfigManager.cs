using System;
using System.Collections;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
namespace Com.ValuePlus.Config
{
    /// <summary>
    /// 配置文件管理类,限制只能读properties,txt,xml,config文件.针对properties和txt都采用同一种方式处理.针对xml和config文件采用同一种方式处理
    /// </summary>
    public sealed  class ConfigManager:IConfigManager
    {

        #region 最近文件加载时间
        /// <summary>
        /// 最近文件加载时间
        /// </summary>
        private DateTime Configfileoldchange;
        #endregion       

        #region 定时器
        /// <summary>
        /// 定时器,用于读取最新的文件信息
        /// </summary>
        private System.Timers.Timer ManagerTimer = new System.Timers.Timer(15000);
        #endregion

        #region 配置文件路径
        /// <summary>
        /// 配置文件路径
        /// </summary>
        private string ConfigFilePath = string.Empty;
        #endregion

        #region 配置文件对象
        /// <summary>
        /// 配置文件对象
        /// </summary>
        private Hashtable ConfigObject = null;
        #endregion

        #region 文件类型
        private FileTypeEnum FileType;
        #endregion

        #region 对象类型
        private Type objectType;
        #endregion

        #region 对象
        private object objectreturn = null;
        #endregion

        #region 获得对象,主要用于xml可以序列化的情形
        /// <summary>
        /// 获得对象,主要用于xml可以序列化的情形
        /// </summary>
        /// <returns>value</returns>
        public object GetProperty(){
            return objectreturn;
        }
        #endregion

        #region 加载配置文件并初始化参数,主要用于xml可以序列化的情形
        /// <summary>
        /// 加载配置文件并初始化参数,主要用于xml可以序列化的情形
        /// </summary>
        /// <param name="FilePath">文件路径</param>
        /// <param name="objecttype0">文件类型</param>
        public void Load(string FilePath, Type objecttype0)
        {
            log.Debug("开始加载配置文件:" + this.ConfigFilePath);
            //文件类型
            this.objectType = objecttype0;
            //文件路径
            if (File.Exists(FilePath))
            {
                this.ConfigFilePath = FilePath;
            }
            else
            {
                this.ConfigFilePath = System.IO.Path.Combine(RootPath, FilePath);
            }
            //获得修改时间
            this.Configfileoldchange = GetLastWriteTime(this.ConfigFilePath);
            //读文件信息
            bool outcheck = false;
            object HtTemp = LoadConfig(ref this.Configfileoldchange, this.ConfigFilePath, false, out outcheck);
            if (outcheck && HtTemp!=null) this.objectreturn = HtTemp;

            //设置定时器
            ManagerTimer.AutoReset = true;
            ManagerTimer.Enabled = true;
            ManagerTimer.Elapsed += new System.Timers.ElapsedEventHandler(TimerSerielize_Elapsed);
            ManagerTimer.Start();
        }
        #endregion

        #region 日志声明
        /// <summary>
        /// 日志声明
        /// </summary>
        private Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region 站点跟路径
        private string RootPath = System.AppDomain.CurrentDomain.BaseDirectory;
        #endregion

        #region 获得配置文件值
        /// <summary>
        /// 获得配置文件值 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetProperty(string key)
        {
            if (ConfigObject != null)
            {
                if (ConfigObject[key.ToLower()] != null)
                {
                    return ConfigObject[key.ToLower()].ToString();
                }
            }
            return string.Empty;

        }
        #endregion

        #region 获得配置文件值
        /// <summary>
        /// 获得配置文件值 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSqlBasicMetaData(string key)
        {
            if (ConfigObject != null)
            {
                if (ConfigObject[key.ToLower()] != null)
                {
                    return ConfigObject[key.ToLower()] as Com.ValuePlus.Utils.SqlBasicMetaData;
                }
            }
            return null;

        }
        #endregion

        #region 加载配置文件并初始化参数
        /// <summary>
        /// 加载配置文件并初始化参数
        /// </summary>
        /// <param name="FilePath"></param>
        /// <param name="filetype"></param>
        public void Load(string FilePath, FileTypeEnum filetype)
        {
            log.Debug("开始加载配置文件:" + this.ConfigFilePath);
            //文件类型
            this.FileType = filetype;
            //文件路径
            if (File.Exists(FilePath))
            {
                this.ConfigFilePath = FilePath;
            }
            else
            {
                this.ConfigFilePath = System.IO.Path.Combine(RootPath, FilePath);
            }           
            //文件类型
            this.FileType = filetype;            
            //获得修改时间
            this.Configfileoldchange = GetLastWriteTime(this.ConfigFilePath);           
            //读文件信息
            bool outcheck = false;
            Hashtable HtTemp = LoadProperties(out outcheck,this.ConfigFilePath,true,ref this.Configfileoldchange,filetype) as Hashtable;
            if (outcheck) this.ConfigObject = HtTemp;     

            //设置定时器
            ManagerTimer.AutoReset = true;
            ManagerTimer.Enabled = true;
            ManagerTimer.Elapsed += new System.Timers.ElapsedEventHandler(Timer_Elapsed);
            ManagerTimer.Start();
        }
        #endregion

        #region 定时处理函数
        /// <summary>
        /// 定时处理函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            log.Debug("定时器开始扫描:" +this.ConfigFilePath);
            //读文件信息
            bool outcheck = false;
            Hashtable HtTemp = LoadProperties(out outcheck, this.ConfigFilePath, false, ref this.Configfileoldchange, this.FileType) as Hashtable;
            if (outcheck) this.ConfigObject = HtTemp;
        }
        #endregion

        #region 定时处理函数
        /// <summary>
        /// 定时处理函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerSerielize_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            log.Debug("定时器开始扫描:" + this.ConfigFilePath);
            //读文件信息
            bool outcheck = false;
            object HtTemp = LoadConfig(ref this.Configfileoldchange, this.ConfigFilePath, true, out outcheck);
            if (outcheck && HtTemp != null) this.objectreturn = HtTemp;           
        }
        #endregion

        #region 获得配置文件的修改时间
        /// <summary>
        /// 获得配置文件的修改时间
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private  DateTime GetLastWriteTime(string path)
        {
            DateTime dt;
            try
            {
                dt = System.IO.File.GetLastWriteTime(path);
            }catch (Exception ex){
                log.Error(ex.ToString());
                dt = DateTime.MinValue;
            }
            return dt;
        }
        #endregion

        #region 将文件转换成键值对
        /// <summary>
        /// 将文件转换成键值对
        /// </summary>
        /// <param name="checksucc">检查是否需要符值</param>
        /// <param name="configpath">配置文件路径</param>
        /// <param name="readcheck">是否强制读文件</param>
        /// <param name="fileoldchange">文件上次读的时间</param>
        /// <param name="filetype">文件类型</param>
        private  object LoadProperties(out bool checksucc,string configpath,bool readcheck,ref DateTime fileoldchange,FileTypeEnum filetype){
            checksucc = false;
            bool checkfile = false;
            Hashtable ht = null;
            if(readcheck){
                checkfile = true;
            }else{
                //判断修改时间
                 DateTime m_filenewchange = GetLastWriteTime(configpath);
                 if (fileoldchange != m_filenewchange)
                 {
                     fileoldchange = m_filenewchange;
                     checkfile = true;
                 }
            }
            if (checkfile)
            {            
                ht = new Hashtable();
                log.Debug("扫描文件有修改:" + this.ConfigFilePath);
                if (filetype == FileTypeEnum.TxtType)
                {
                    if (ReadTxtConfig(ref ht, configpath))
                    {
                        checksucc = true;
                        return ht;
                    }
                }
                else
                {
                    if (filetype == FileTypeEnum.XmlType)
                    {
                        if (ReadXmlConfig(ref ht, configpath))
                        {
                            checksucc = true;
                            return ht;
                        }
                    }
                    else
                    {
                        if (ReadSqlXmlConfig(ref ht, configpath))
                        {
                            checksucc = true;
                            return ht;
                        }
                    }
                }
            }
            return null;
        }
#endregion

        #region 读xml文件
        /// <summary>
        /// 读xml文件
        /// </summary>
        /// <param name="ht">加载存储对象</param>
        /// <param name="configpath">配置文件路径</param>        
        private bool ReadXmlConfig(ref Hashtable ht, string configpath)
        {                  
            try
            {
                XmlDocument dom = new XmlDocument();     
                using (FileStream fs = new FileStream(configpath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    dom.Load(fs);
                }
                XmlNodeList nodelist = dom.DocumentElement.ChildNodes;
                if (nodelist != null && nodelist.Count > 0)
                {
                    foreach (XmlNode node in nodelist)
                    {
                        if (node.NodeType == XmlNodeType.Element)
                        {
                            if (!ht.Contains(node.Name.Trim().ToLower()))
                            {                               
                                ht.Add(node.Name.Trim().ToLower(), node.InnerText);
                            }
                        }
                    }
                    if (ht.Count > 0)
                    {
                        return true;
                    }
                }                
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
            }
            return false;
        }
#endregion

        #region 读txt文件
        /// <summary>
        /// 读txt文件
        /// </summary>
        /// <param name="ht">加载存储对象</param>
        /// <param name="configpath">配置文件路径</param>        
        private bool ReadTxtConfig(ref Hashtable ht, string configpath)
        {
            try
            {
                using (StreamReader sr = new StreamReader(configpath))
                {                  
                    string str = string.Empty;
                    sr.BaseStream.Seek(0, SeekOrigin.End);
                    sr.BaseStream.Seek(0, SeekOrigin.Begin);
                    while ((str = sr.ReadLine()) != null)
                    {
                        str = str.Trim();
                        if (!str.StartsWith("#") && (str.IndexOf('=')>0))
                        {
                            string key = str.Substring(0, str.IndexOf('=')).Trim().ToLower();                            
                            string value ;
                            if(str.Length == (str.IndexOf('=') + 1)) {
                                value = string.Empty;
                            }else{  
                                value = str.Substring(str.IndexOf('=') + 1).Trim();
                            }
                            if (!ht.Contains(key.ToLower()))
                            {
                                ht.Add(key, value);
                            }                            
                        }
                    }
                }
                if (ht.Count > 0)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
            }
            return false;
        }
        #endregion

        #region 读sqlxml文件
        /// <summary>
        /// 读sqlxml文件
        /// </summary>
        /// <param name="ht">加载存储对象</param>
        /// <param name="configpath">配置文件路径</param>        
        private bool ReadSqlXmlConfig(ref Hashtable ht, string configpath)
        {
            try
            {
                XmlDocument dom = new XmlDocument();
                using (FileStream fs = new FileStream(configpath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    dom.Load(fs);
                }
                XmlNodeList nodelist = dom.DocumentElement.ChildNodes;
                if (nodelist != null && nodelist.Count > 0)
                {
                    foreach (XmlNode node in nodelist)
                    {
                        if (node.NodeType == XmlNodeType.Element)
                        {
                            if (!ht.Contains(node.Name.Trim().ToLower()))
                            {
                                ht.Add(node.Name.Trim().ToLower(), XMLDeserialize(string.Format("<?xml version=\"1.0\"?>{0}", node.InnerXml), typeof(Com.ValuePlus.Utils.SqlBasicMetaData), Encoding.UTF8));
                            }
                        }
                    }
                    if (ht.Count > 0)
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
            }
            return false;
        }
        #endregion

        #region 序列化处理
        /// <summary>
        /// 加载(反序列化)指定对象类型的配置对象
        /// </summary>
        /// <param name="fileoldchange">文件加载时间</param>
        /// <param name="configFilePath">配置文件所在路径(包括文件名)</param>
        /// <param name="checkTime">是否检查并更新传递进来的"文件加载时间"变量</param>
        /// <param name="outcheck">输出是否更新对象</param>
        /// <returns></returns>
        private  object LoadConfig(ref DateTime fileoldchange, string configFilePath, bool checkTime, out bool outcheck)
        {
            outcheck = false;
            string m_configfilepath = configFilePath;
            object m_configinfo =null;

            if (checkTime)
            {
                DateTime m_filenewchange = GetLastWriteTime(configFilePath);
                log.Debug("检查文件时间是否变化:old:" + m_filenewchange.ToString() + ";new:" + fileoldchange.ToString());

                //当程序运行中config文件发生变化时则对config重新赋值
                if (fileoldchange != m_filenewchange)
                {
                    fileoldchange = m_filenewchange;
                    log.Debug("时间变化,重新加载文件");
                    m_configinfo = DeserializeInfo(configFilePath, this.objectType);
                    outcheck = true;
                }
            }
            else
            {
                log.Debug("不比较时间就直接加载文件");
                m_configinfo = DeserializeInfo(configFilePath, this.objectType);
                outcheck = true;

            }
            return m_configinfo;
        }


        /// <summary>
        /// 反序列化指定的类
        /// </summary>
        /// <param name="configfilepath">config 文件的路径</param>
        /// <param name="configtype">相应的类型</param>
        /// <returns></returns>
        private  object DeserializeInfo(string configfilepath, Type configtype)
        {
            return XMLDeserialize(configtype, configfilepath);
        }

        #region XMLDeserialize
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <param name="filename">文件路径</param>
        /// <returns></returns>
        public  object XMLDeserialize(Type type, string filename)
        {
            FileStream fs = null;
            try
            {
                // open the stream...
                fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                XmlSerializer serializer = new XmlSerializer(type);
                return serializer.Deserialize(fs);
            }
            catch (Exception ex)
            {
                log.Error("读公共配置文件序列化错误", ex);
            }
            finally
            {
                if (fs != null)
                    fs.Close();
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
        private  object XMLDeserialize(string str, Type type, System.Text.Encoding encoding)
        {
            object data = null;
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
                log.Error(string.Format("XMLDeserialize错误，序列化字符串为:{0},{1}",str,ex));
            }            
            return data;
        }
        #endregion
#endregion

    }
}
