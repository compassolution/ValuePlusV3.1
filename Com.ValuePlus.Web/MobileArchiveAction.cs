using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Archive.BLL;
using System.Web;
using Com.ValuePlus.Archive.Entity;
using Com.ValuePlus.SysParams;
using Com.ValuePlus.Archive.Property;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Com.ValuePlus.DataLog.Entity;
using Com.ValuePlus.DataLog;
using Com.ValuePlus.DataLog.Enum;
using Com.ValuePlus.Common.Config;
using System.IO;
using System.Xml;

namespace Com.ValuePlus.Web
{
    public class MobileArchiveAction : PageBase
    {
        protected String strXmlFileRelaTivePath = BaseConfig.Instance.GetConfigValueByKey("PATH_ActionXmlFile");//存储过程参数对应xml文件相对路径

        /// <summary>
        /// 获取模板状态下的动作列表数据
        /// </summary>
        /// <param name="strUserID"></param>
        /// <param name="strTID"></param>
        /// <param name="strRID"></param>
        /// <param name="strSID"></param>
        /// <param name="strAID"></param>
        /// <param name="strLocation"></param>
        /// <param name="strRequestLanguage"></param>
        public String GetArchiveActionData( String strUserID, String strTID, String strRID, String strSID, String strAID,String strLocation, String strRequestLanguage)
        {
            try
            {
                //当前SID下可视的Action信息
                StringBuilder sbSql_Action = new StringBuilder();
                sbSql_Action.Append("select * from TB_HRTMPSA WHERE TID = '" + strTID + "' AND [SID] = '" + strSID + "' ");
                sbSql_Action.Append(" AND ARIGHT = '1' AND ALOCATION in ("+ strLocation + ")");
                if(!String.IsNullOrEmpty(strAID))
                {
                    sbSql_Action.Append(" AND AID = '" + strAID + "'");
                }
                sbSql_Action.Append(" AND ATYPE IN ('2')");//移动端暂时只加载存储过程类型
                sbSql_Action.Append(" ORDER BY AORDER");
                log.Error("获取模板状态下的动作列表相关业务数据sql:"+ sbSql_Action.ToString());
                DataTable dt_SceneActionList = SqlParamDao.GetDataTableBySql(sbSql_Action.ToString());

                StringBuilder sbJson = new StringBuilder();
                sbJson.Append(WebCommon.GetJsonStringByDataTable(dt_SceneActionList, "\"ActionList\"", true));//当前SID下可视的Action信息

                string json = sbJson.ToString();

                //json = "2";
                return json ;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return "";
            }
        }
        
        /// <summary>
        /// 获取动作列表
        /// </summary>
        /// <param name="strTID"></param>
        /// <param name="strRID"></param>
        /// <param name="strSID"></param>
        /// <param name="strActionLocation">动作所在位置</param>
        /// <param name="strUserId"></param>
        /// <returns></returns>
        public ArrayList GetActionList(string strTID, String strRID, String strSID, String strActionLocation, String strUserId)
        {
            ArchiveActionBll bllAction = new ArchiveActionBll();
            ArrayList arrActionList = bllAction.GetActionDetailList(strTID, strRID, strSID, int.Parse(strActionLocation), "", strUserId, this.IsAdminstrator(), "0");
            return arrActionList;
        }

        /// <summary>
        /// 获取动作列表的数据集
        /// </summary>
        /// <param name="strTID"></param>
        /// <param name="strRID"></param>
        /// <param name="strSID"></param>
        /// <param name="strAID"></param>
        /// <param name="strLocation"></param>
        /// <param name="strUserId"></param>
        /// <returns></returns>
        public DataTable GetDataTable_ActionList(string strTID, String strRID, String strSID, String strAID,String strLocation, String strUserId)
        {
            DataTable dt = new DataTable();
            try
            {
                //当前SID下可视的Action信息
                StringBuilder sbSql = new StringBuilder();
                sbSql.Append("select * from TB_HRTMPSA WHERE TID = '" + strTID + "' AND [SID] = '" + strSID + "' ");
                sbSql.Append(" AND ARIGHT = '1' ");
                if (!String.IsNullOrEmpty(strAID))
                {
                    sbSql.Append(" AND AID = '" + strAID + "'");
                }
                if (!String.IsNullOrEmpty(strLocation))
                {
                    sbSql.Append(" AND ALOCATION in (" + strLocation + ")");
                }
                sbSql.Append(" AND ATYPE IN ('2')");//移动端暂时只加载存储过程类型
                sbSql.Append(" ORDER BY AORDER");
                dt = SqlParamDao.GetDataTableBySql(sbSql.ToString());
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return dt;
        }

        /// <summary>
        /// 读取存储过程参数相应xml文件,存储在hashTable中
        /// </summary>
        public Hashtable ReadXmlParamToHashTable(String strSPName)
        {
            String strXmlFilePathAndName = "";
            Hashtable hsTable = new Hashtable();
            MobileArchive mobileArchive = new MobileArchive();
            try
            {
                String strXmlFilePath = Server.MapPath(strXmlFileRelaTivePath);
                if (!Directory.Exists(strXmlFilePath))
                {
                    Directory.CreateDirectory(strXmlFilePath);
                }

                //this.strFilePathAndName = base.MapPath(strSP);
                strXmlFilePathAndName = strXmlFilePath + "\\" + strSPName + ".xml";

                // 打开一个 XML 文件 
                if (File.Exists(strXmlFilePathAndName))
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(strXmlFilePathAndName);
                    XmlNode xmlNode = xmlDoc.SelectSingleNode("//Fields");
                    XmlNodeList nodeList = xmlDoc.SelectNodes("//Fields");
                    int iCount = nodeList.Count;

                    for (int i = 0; i < iCount; i++)
                    {
                        SpXmlEntity entityXml = new SpXmlEntity();
                        xmlNode = nodeList[i];
                        entityXml.strAlias = xmlNode.ChildNodes[0].InnerText.ToString();
                        entityXml.strTid = xmlNode.ChildNodes[1].InnerText.ToString();
                        entityXml.strGid = xmlNode.ChildNodes[2].InnerText.ToString();
                        entityXml.strSid = xmlNode.ChildNodes[3].InnerText.ToString();
                        entityXml.strPid = xmlNode.ChildNodes[4].InnerText.ToString();
                        //根据TID/SID/GID/PID获取对应字段的相关信息
                        DataTable dt_PID = mobileArchive.GetDataTable_PropertyList(entityXml.strTid, entityXml.strSid, entityXml.strGid, entityXml.strPid, "");
                        if(dt_PID!=null&&dt_PID.Rows.Count==1)
                        {
                            entityXml.strPDESC = dt_PID.Rows[0]["PDESC"].ToString();
                            entityXml.strPDESCCHS = dt_PID.Rows[0]["PDESCCHS"].ToString();
                            entityXml.strPCTRLTYPE = dt_PID.Rows[0]["PCTRL"].ToString();
                            entityXml.strPCTRLID = dt_PID.Rows[0]["PCTRLID"].ToString();
                            entityXml.strPCTRLSQL = dt_PID.Rows[0]["PCTRLD"].ToString();
                        }
                        hsTable.Add(entityXml.strAlias, entityXml);
                    }
                }
            }
            catch(Exception ex){

            }
            return hsTable;

        }

        /// <summary>
        /// 执行存储过程类型的动作
        /// </summary>
        /// <param name="strTID"></param>
        /// <param name="strRID"></param>
        /// <param name="strSID"></param>
        /// <param name="strAID"></param>
        /// <param name="strSPName"></param>
        /// <param name="strPostJsonData"></param>
        /// <param name="strLanguage"></param>
        /// <param name="strUserCode"></param>
        /// <param name="iReturnValue"></param>
        /// <returns></returns>
        public String DoExcuteSPAction(String strTID, String strRID, String strSID, String strAID, String strSPName, String strPostJsonData
            , String strLanguage, String strUserCode, ref int iReturnValue)
        {
            String strReturnMsg = "";
            Hashtable hsTableParam = new Hashtable();
            try
            {
                JArray jsonArray = (JArray)JsonConvert.DeserializeObject(strPostJsonData);
                foreach (JObject itemJArray in jsonArray)
                {
                    String strParamName = itemJArray["paramName"].ToString();
                    String strParamValue = itemJArray["paramValue"].ToString();
                    hsTableParam.Add(strParamName, strParamValue);
                }
                iReturnValue = SqlParamDao.ExcuteSP(strSPName, hsTableParam);

                //获取执行存储过程的返回消息
                String strMsgName = "MESSENG";
                if (strLanguage == "zh-cn")
                {
                    strMsgName = "MESSCHS";
                }
                String strSql = "SELECT " + strMsgName + " FROM TB_HRTMPAR WHERE TID='" + strTID + "' AND AID='" + strAID + "' AND ECFROM<=" + iReturnValue + " AND ECTO>=" + iReturnValue;
                DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
                if ((dt!=null) && (dt.Rows.Count > 0))
                {
                    strReturnMsg = dt.Rows[0][0].ToString().Replace("@S@", iReturnValue.ToString());
                }
                else
                {
                    strReturnMsg = strLanguage == "zh-cn" ? "动作执行成功！":"Excute Successfully！";
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                iReturnValue = -1;
                strReturnMsg = strLanguage == "zh-cn" ? "动作执行失败！" : "Excute failed！";
            }
            return strReturnMsg;
        }

    }
}
