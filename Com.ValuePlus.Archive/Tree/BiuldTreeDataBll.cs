using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Com.ValuePlus.Archive.DAL;
using Com.ValuePlus.Common;

namespace Com.ValuePlus.Archive.Tree
{
    public class BiuldTreeDataBll
    {
        /// <summary>
        /// 日志声明
        /// </summary>
        private Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    
        #region 获得特定树形模板业务表中的所有数据
        /// <summary>
        /// 获得特定树形模板业务表中的所有数据
        /// </summary>
        /// <param name="strTid"></param>
        /// <param name="strOpType"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public string GetTreeDataAll(String strTid,String strOpType,string language)
        {
            string sResult = string.Empty;
            String strTableName = "TREE_" + strTid;
            DataTable dt = new DataTable();
            try
            {
                String strSql = "SELECT * FROM " + strTableName + " order by TREEORDER ";
                dt = SqlParamDao.GetDataTableBySql(strSql);
            }
            catch (Exception ex)
            {
                return "The tree code you config is wrong!";
            }
            //if (dt != null && dt.Rows.Count > 0)
            //{
                CreateTreeXmlStructure(strTid,strOpType,dt, language, ref sResult);
            //}
            return sResult;
        }
        #endregion

        #region 创建树结构
        /// <summary>
        /// 创建树结构
        /// </summary>
        /// <param name="strTid"></param>
        /// <param name="strOpType"></param>
        /// <param name="dt"></param>
        /// <param name="language"></param>
        /// <param name="sResult"></param>
        private void CreateTreeXmlStructure(String strTid, String strOpType, DataTable dt, string language, ref string sResult)
        {
            sResult = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n";
            sResult = sResult + "<tree id=\"0\">\r\n    ";
            //首先创建根节点
            DataRow[] drs = dt.Select("PARENTCODE is null or PARENTCODE = ''");
            if (drs != null && drs.Length > 0)
            {
                string sTitle = "("+drs[0]["TREECODE"].ToString()+")"+drs[0]["TREENAME"].ToString() ;
                if (language == "zh-cn")
                {
                    sTitle = "(" + drs[0]["TREECODE"].ToString() + ")" + drs[0]["TREENAMECHS"].ToString();
                }
                sTitle = Com.ValuePlus.Utils.HtmlUtils.HtmlEncode(sTitle);
                if (drs[0]["TREEIMAGE"] == null || drs[0]["TREEIMAGE"] == DBNull.Value)
                {
                    sResult = sResult + "<item id=\"" + drs[0]["TREECODE"].ToString() + "\" open=\"true\" text=\"" + sTitle + "\" tooltip=\"" + sTitle + "\" >\r\n";
                }
                else
                {
                    sResult = sResult + "<item id=\"dhtmlxtree\" open=\"true\" text=\"" + sTitle + "\" tooltip=\"" + sTitle + "\" im0=\"" + drs[0]["TREEIMAGE"].ToString() + "\" >\r\n";
                }
                sResult = sResult + "<userdata name=\"url\">";
                
                //如果是对树形结构的操作
                if ((strOpType.ToLower().Equals("add")) || (strOpType.ToLower().Equals("add")) || (strOpType.ToLower().Equals("add")))
                {
                    String strParamString = "TREE=" + strTid + "&CODE=" + drs[0]["TREECODE"].ToString() + "&OPTYPE=" + strOpType;
                    strParamString = UrlParamEncryption.EncryptionUrlParam(strParamString);
                    sResult = sResult + "TreeList.aspx?" + strParamString;
                }
                else
                {
                    //其他页面操作链接
                    String strSql1 = "select * from TREECONFIG_3 WHERE TID = '" + strTid + "' AND OPTYPE = '" + strOpType + "'";
                    DataTable dt1 = SqlParamDao.GetDataTableBySql(strSql1);
                    if ((dt1 != null) && (dt1.Rows.Count > 0))
                    {
                        String strPageUrl = dt1.Rows[0]["OPPAGE"].ToString().Replace("%TREECODE%", drs[0]["TREECODE"].ToString());
                        strPageUrl = Com.ValuePlus.Utils.StringUtils.ReplaceKeyStringToXml(strPageUrl);
                        sResult = sResult + strPageUrl;
                    }
                }
                sResult = sResult + "</userdata>\r\n";

                //创建子节点
                CreateSubTreeXmlStructure(strTid, strOpType,dt, drs[0]["TREECODE"].ToString(), language, ref sResult);
                sResult += "</item>\r\n";
            }
            sResult += "</tree>\r\n";
        }
        #endregion

        #region 创建树子节点结构
        /// <summary>
        /// 创建树子节点结构
        /// </summary>
        /// <param name="strTid"></param>
        /// <param name="strOpType"></param>
        /// <param name="dt"></param>
        /// <param name="id"></param>
        /// <param name="language"></param>
        /// <param name="sResult"></param>
        private void CreateSubTreeXmlStructure(String strTid, String strOpType, DataTable dt, string id, string language, ref string sResult)
        {
            DataRow[] drs = dt.Select("PARENTCODE = '" + id + "'", "TREEORDER ASC");
            if (drs != null && drs.Length > 0)
            {
                foreach (DataRow dr in drs)
                {
                    string sTitle = "(" + dr["TREECODE"].ToString() + ")" + dr["TREENAME"].ToString();
                    if (language == "zh-cn")
                    {
                        sTitle = "(" + dr["TREECODE"].ToString() + ")" + dr["TREENAMECHS"].ToString();
                    }
                    sTitle = Com.ValuePlus.Utils.HtmlUtils.HtmlEncode(sTitle);
                    if (dr["TREEIMAGE"] == null || dr["TREEIMAGE"] == DBNull.Value)
                    {
                        sResult = sResult + "<item id=\"sub" + dr["TREECODE"].ToString() + "\" text=\"" + sTitle + "\"  tooltip=\"" + sTitle + "\" >\r\n";
                    }
                    else
                    {
                        sResult = sResult + "<item id=\"sub" + dr["TREECODE"].ToString() + "\" text=\"" + sTitle + "\"  tooltip=\"" + sTitle + "\"  im0=\"" + dr["TREEIMAGE"].ToString() + "\" >\r\n";
                    }
                    sResult = sResult + "<userdata name=\"url\">";
                    //如果是对树形结构的操作
                    if ((strOpType.ToLower().Equals("add")) || (strOpType.ToLower().Equals("add")) || (strOpType.ToLower().Equals("add")))
                    {
                        String strParamString = "TREE=" + strTid + "&CODE=" + dr["TREECODE"].ToString() + "&OPTYPE=" + strOpType;
                        strParamString = UrlParamEncryption.EncryptionUrlParam(strParamString);
                        sResult = sResult + "TreeList.aspx?" + strParamString;
                    }
                    else
                    {
                        //其他页面操作链接
                        String strSql1 = "select * from TREECONFIG_3 WHERE TID = '" + strTid + "' AND OPTYPE = '" + strOpType + "'";
                        DataTable dt1 = SqlParamDao.GetDataTableBySql(strSql1);
                        if ((dt1 != null) && (dt1.Rows.Count > 0))
                        {
                            String strPageUrl = dt1.Rows[0]["OPPAGE"].ToString().Replace("%TREECODE%", dr["TREECODE"].ToString());
                            strPageUrl = Com.ValuePlus.Utils.StringUtils.ReplaceKeyStringToXml(strPageUrl);
                            sResult = sResult + strPageUrl;
                        }
                    }
                    sResult = sResult + "</userdata>\r\n";

                    //创建子节点
                    CreateSubTreeXmlStructure(strTid, strOpType,dt, dr["TREECODE"].ToString(), language, ref sResult);
                    sResult += "</item>\r\n";
                }
            }
        }
        #endregion

    }
}
