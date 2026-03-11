using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Xml;
using Com.ValuePlus.Entity.UpDownLoad;
using System.IO;

namespace Com.ValuePlus.BLL.UpDownLoad
{
    /// <summary>
    /// 上传下载相应xml配置文件读取类(作废，改用从数据库中配置读取)
    /// </summary>
    public class ReadConfigXmlBll
    {
        /// <summary>
        /// 日志声明
        /// </summary>
        protected static Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region 读取上传下载相应xml配置文件,存储在hashTable中
        /// <summary>
        /// 读取上传下载相应xml配置文件,存储在hashTable中
        /// </summary>
        /// <param name="strXmlFilePathAndName"></param>
        /// <returns>Hashtable</returns>
        public static Hashtable ReadXmlFile(String strXmlFilePathAndName)
        {
            Hashtable hsTable = new Hashtable();
            try
            {
                // 打开一个 XML 文件 
                if (File.Exists(strXmlFilePathAndName))
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(strXmlFilePathAndName);
                    XmlNode xmlNode = xmlDoc.SelectSingleNode("//UpDownLoadConfig");

                    XmlNodeList nodeList = xmlNode.ChildNodes;
                    int iCount = nodeList.Count;

                    for (int i = 0; i < iCount; i++)
                    {
                        UpDownLoadXmlEntity entityXml = new UpDownLoadXmlEntity();
                        XmlNode xmlNodeI = nodeList[i];
                        entityXml.ID = xmlNodeI.Name;
                        entityXml.NAME = xmlNodeI.ChildNodes[0].InnerText.ToString();
                        entityXml.NAME_CN = xmlNodeI.ChildNodes[1].InnerText.ToString();
                        entityXml.DESC = xmlNodeI.ChildNodes[2].InnerText.ToString();
                        entityXml.DESC_CN = xmlNodeI.ChildNodes[3].InnerText.ToString();
                        entityXml.PATH = xmlNodeI.ChildNodes[4].InnerText.ToString();
                        hsTable.Add(entityXml.ID, entityXml);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return hsTable;
        }
        #endregion

    }
}
