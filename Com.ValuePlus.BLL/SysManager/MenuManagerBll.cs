using System;
using System.Collections;
using System.Data;
using System.Collections.Generic;
using System.Text;
using Com.ValuePlus.DAL;
using System.Data;

namespace Com.ValuePlus.BLL.SysManager
{
    public class MenuManagerBll
    {

        #region 查询栏目表所有记录
        /// <summary>
        /// 查询栏目表所有记录
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllMenuInfo()
        {
            MenuInfoDao daoMenu = new MenuInfoDao();
            return daoMenu.findAll();
        }
        #endregion

        #region 根据主键查询栏目表记录，返回datatable记录
        /// <summary>
        /// 根据主键查询栏目表记录，返回datatable记录
        /// </summary>
        /// <param name="strMenuCode"></param>
        /// <returns>DataSet</returns>
        public DataTable GetMenuInfoByMenuCode(String strMenuCode)
        {
            MenuInfoDao daoMenu = new MenuInfoDao();
            return daoMenu.findTableByMenuCode(strMenuCode);
        }
        #endregion

        #region 判断栏目编码是否已经存在
        /// <summary>
        /// 判断栏目编码是否已经存在
        /// </summary>
        /// <param name="strMenuCode"></param>
        /// <returns></returns>
        public Boolean IsExsitMenuCode(String strMenuCode)
        {
            Boolean bIsExsit = true;
            MenuInfoDao daoMenu = new MenuInfoDao();
            DataSet ds = daoMenu.findByMenuCode(strMenuCode);
            if (ds == null)//ds为空
            {
                bIsExsit = false;
            }
            else
            {
                if (ds.Tables.Count == 0)//ds中没有表
                {
                    bIsExsit = false;
                }
                else
                {
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 0)//ds中的表没有数据
                    {
                        bIsExsit = false;
                    }
                }
            }
            return bIsExsit;
        }
        #endregion

        #region 判断顺序号是否已经存在
        /// <summary>
        /// 判断顺序号是否已经存在
        /// </summary>
        /// <param name="strOrderCode"></param>
        /// <returns></returns>
        public Boolean IsExsitOrderCode(String strOrderCode)
        {
            Boolean bIsExsit = true;
            MenuInfoDao daoMenu = new MenuInfoDao();
            DataSet ds = daoMenu.findByOrder(strOrderCode);
            if (ds == null)//ds为空
            {
                bIsExsit = false;
            }
            else
            {
                if (ds.Tables.Count == 0)//ds中没有表
                {
                    bIsExsit = false;
                }
                else
                {
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 0)//ds中的表没有数据
                    {
                        bIsExsit = false;
                    }
                }
            }
            return bIsExsit;
        }
        #endregion

        #region 根据栏目编码和顺序号判断是否可以修改
        /// <summary>
        /// 根据栏目编码和顺序号判断是否可以修改
        /// </summary>
        /// <param name="strMenuCode"></param>
        /// <param name="strOrderCode"></param>
        /// <returns></returns>
        public Boolean IsCanModifyByOrder(String strMenuCode,String strOrderCode)
        {
            Boolean bIsCanModify = true;

            MenuInfoDao daoMenu = new MenuInfoDao();
            DataTable dt = daoMenu.findTableByMenuCode(strMenuCode);

            if (dt.Rows.Count> 0)//ds中没有表
            {
                String strTempOrder = dt.Rows[0]["SORDER"].ToString();
                if ((!strTempOrder.Equals(strOrderCode)) && (IsExsitOrderCode(strOrderCode)))
                {
                    bIsCanModify = false;
                }
            }
            return bIsCanModify;
        }
        #endregion

        #region 判断栏目编码是否已经存在下级子目录
        /// <summary>
        /// 判断栏目编码是否已经存在下级子目录
        /// </summary>
        /// <param name="strMenuCode"></param>
        /// <returns></returns>
        public Boolean IsExsitSubLevel(String strMenuCode)
        {
            Boolean bIsExsit = true;
            MenuInfoDao daoMenu = new MenuInfoDao();
            DataSet ds = daoMenu.findSubLevelByMenuCode(strMenuCode);
            if (ds == null)//ds为空
            {
                bIsExsit = false;
            }
            else
            {
                if (ds.Tables.Count == 0)//ds中没有表
                {
                    bIsExsit = false;
                }
                else
                {
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 0)//ds中的表没有数据
                    {
                        bIsExsit = false;
                    }
                }
            }
            return bIsExsit;
        }
        #endregion

        #region 根据栏目编码获取所有直属下级子目录信息
        /// <summary>
        /// 根据栏目编码获取所有直属下级子目录信息
        /// </summary>
        /// <param name="strMenuCode"></param>
        /// <returns>DataTable</returns>
        public DataTable GetSubLevelMenu(String strMenuCode)
        {
            MenuInfoDao daoMenu = new MenuInfoDao();
            DataSet ds = daoMenu.findSubLevelByMenuCode(strMenuCode);
            DataTable dt = new DataTable();
            if (ds != null && ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
            }
            return dt;
        }
        #endregion

        #region 根据栏目编码以及用户编码查询该用户具有的有直属下级子目录信息
        /// <summary>
        /// 根据栏目编码以及用户编码查询该用户具有的有直属下级子目录信息
        /// </summary>
        /// <param name="strMenuCode"></param>
        /// <param name="strUserId"></param>
        /// <returns>DataTable</returns>
        public DataTable GetSubLevelMenuByMenuCodeAUserId(String strMenuCode, String strUserId)
        {
            MenuInfoDao daoMenu = new MenuInfoDao();
            DataSet ds = daoMenu.findSubLevelByMenuCodeAUserId(strMenuCode, strUserId);
            DataTable dt = new DataTable();
            if (ds != null && ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
            }
            return dt;
        }
        #endregion

        #region 插入栏目字典定义表一条记录
        /// <summary>
        /// 插入栏目字典定义表一条记录
        /// </summary>
        /// <param name="strSMENUCODE"></param>
        /// <param name="strSMENUNAME"></param>
        /// <param name="strSMENUNAMECN"></param>
        /// <param name="strSURLDETAIL"></param>
        /// <param name="strSMENUTYPE"></param>
        /// <param name="strSIMAGE"></param>
        /// <param name="strSPARENTCODE"></param>
        /// <param name="strSLEVEL"></param>
        /// <param name="strSORDER"></param>
        /// <param name="strBISSTOP"></param>
        /// <param name="strSSHOWLOCATION"></param>
        /// <returns></returns>
        public int AddMenuInfo(String strSMENUCODE, String strSMENUNAME, String strSMENUNAMECN, String strSURLDETAIL, String strSMENUTYPE, String strSIMAGE, String strSPARENTCODE, String strSLEVEL, String strSORDER,String strSSHOWLOCATION)
        {
            MenuInfoDao daoMenu = new MenuInfoDao();
            return daoMenu.insertOneRow(strSMENUCODE, strSMENUNAME, strSMENUNAMECN, strSURLDETAIL, strSMENUTYPE, strSIMAGE, strSPARENTCODE, strSLEVEL, strSORDER, "0", strSSHOWLOCATION);
        }
        #endregion

        #region 根据栏目定义表主键删除一条记录
        /// <summary>
        /// 根据栏目定义表主键删除一条记录
        /// </summary>
        /// <param name="strMenuCode"></param>
        /// <returns></returns>
        public int deleteMenuInfo(String strMenuCode)
        {
            int iCount = 0;
            MenuInfoDao daoMenu = new MenuInfoDao();
            UserMenuInfoDao daoUserMenu = new UserMenuInfoDao();
            if (IsExsitMenuCode(strMenuCode))
            {
                daoUserMenu.deleteByMenuCode(strMenuCode);
                iCount = daoMenu.deleteByMenuCode(strMenuCode);
            }
            return iCount;
        }
        #endregion

        #region 根据栏目定义表主键更新一条记录
        /// <summary>
        /// 根据栏目定义表主键更新一条记录
        /// </summary>
        /// <param name="strSMENUCODE"></param>
        /// <param name="strSMENUNAME"></param>
        /// <param name="strSMENUNAMECN"></param>
        /// <param name="strSURLDETAIL"></param>
        /// <param name="strSMENUTYPE"></param>
        /// <param name="strSIMAGE"></param>
        /// <param name="strSPARENTCODE"></param>
        /// <param name="strSLEVEL"></param>
        /// <param name="strSORDER"></param>
        /// <param name="strBISSTOP"></param>
        /// <param name="strSSHOWLOCATION"></param>
        /// <returns></returns>
        public int updateMenuInfo(String strSMENUCODE, String strSMENUNAME, String strSMENUNAMECN, String strSURLDETAIL, String strSMENUTYPE, String strSIMAGE, String strSPARENTCODE, String strSLEVEL, String strSORDER,String strBISSTOP,String strSSHOWLOCATION)
        {
            int iCount = 0;
            MenuInfoDao daoMenu = new MenuInfoDao();
            if (IsExsitMenuCode(strSMENUCODE))
            {
                iCount = daoMenu.updateByMenuCode(strSMENUCODE, strSMENUNAME, strSMENUNAMECN, strSURLDETAIL, strSMENUTYPE, strSIMAGE, strSPARENTCODE, strSLEVEL, strSORDER, strBISSTOP, strSSHOWLOCATION);
            }
            return iCount;
        }
        #endregion

        #region 获得所有栏目树
        /// <summary>
        /// 获得所有栏目树
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public string GetMenuTreeAll(string language)
        {
            string sResult = string.Empty;
            MenuInfoDao daoMenu = new MenuInfoDao();
            DataTable dt = daoMenu.findAllTable();
            if (dt != null && dt.Rows.Count > 0)
            {
                CreateTreeXmlStructure(dt, language, ref sResult);
            }
            return sResult;
        }
        #endregion

        #region 创建树结构
        /// <summary>
        /// 创建树结构
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="language"></param>
        /// <param name="sResult"></param>
        private void CreateTreeXmlStructure(DataTable dt, string language, ref string sResult)
        {
            sResult = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><tree id=\"0\">";
            //首先创建根节点
            DataRow[] drs = dt.Select("SPARENTCODE is null or SPARENTCODE = ''");
            if (drs != null && drs.Length > 0)
            {
                String strBISSTOP = drs[0]["BISSTOP"] == null ? "1" : drs[0]["BISSTOP"].ToString();
                String strBISSTOP_Desc = drs[0]["BISSTOP"].ToString().Equals("1") ? "《Stopped》" : "";
                string sTitle = strBISSTOP_Desc + drs[0]["SMENUNAME"].ToString() + "【" + drs[0]["SMENUCODE"].ToString() + "】";
                if (language == "zh-cn")
                {
                    strBISSTOP_Desc = strBISSTOP.Equals("1") ? "《已停用》" : "";
                    sTitle = strBISSTOP_Desc + drs[0]["SMENUNAMECN"].ToString() + "【" + drs[0]["SMENUCODE"].ToString() + "】";
                }
                sTitle = Com.ValuePlus.Utils.HtmlUtils.HtmlEncode(sTitle);
                if (drs[0]["SIMAGE"] == null || drs[0]["SIMAGE"] == DBNull.Value)
                {
                    sResult = sResult + "<item id=\"" + drs[0]["SMENUCODE"].ToString() + "\" open=\"true\" text=\"" + sTitle + "\" tooltip=\"" + sTitle + "\" im0=\"dhtmlxtree_icon.gif\" im1=\"dhtmlxtree_icon.gif\" im2=\"dhtmlxtree_icon.gif\">";
                }
                else
                {
                    sResult = sResult + "<item id=\"dhtmlxtree\" open=\"true\" text=\"" + sTitle + "\" tooltip=\"" + sTitle + "\" im0=\"" + drs[0]["SIMAGE"].ToString() + "\" im1=\"" + drs[0]["SIMAGE"].ToString() + "\" im2=\"" + drs[0]["SIMAGE"].ToString() + "\">";
                }
                sResult = sResult + "<userdata name=\"url\">";
                sResult = sResult + "MenuDetail.aspx?menuCode=" + drs[0]["SMENUCODE"].ToString();
                sResult = sResult + "</userdata>";
                
                //创建子节点
                CreateSubTreeXmlStructure(dt, drs[0]["SMENUCODE"].ToString(), language, ref sResult);
                sResult += "</item>";
            }
            sResult += "</tree>";
        }
        #endregion

        #region 创建树子节点结构
        /// <summary>
        /// 创建树子节点结构
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="id"></param>
        /// <param name="language"></param>
        /// <param name="sResult"></param>
        private void CreateSubTreeXmlStructure(DataTable dt, string id, string language, ref string sResult)
        {
            DataRow[] drs = dt.Select("SPARENTCODE = '" + id + "'", "SORDER ASC");
            if (drs != null && drs.Length > 0)
            {
                foreach (DataRow dr in drs)
                {
                    String strBISSTOP = dr["BISSTOP"] == null ? "1" : dr["BISSTOP"].ToString();
                    String strBISSTOP_Desc = strBISSTOP.Equals("1") ? "《Stopped》" : "";
                    string sTitle = strBISSTOP_Desc + dr["SMENUNAME"].ToString() + "【" + dr["SMENUCODE"].ToString() + "】";
                    if (language == "zh-cn")
                    {
                        strBISSTOP_Desc = strBISSTOP.Equals("1") ? "《已停用》" : "";
                        sTitle = strBISSTOP_Desc + dr["SMENUNAMECN"].ToString() + "【" + dr["SMENUCODE"].ToString() + "】";
                    }
                    sTitle = Com.ValuePlus.Utils.HtmlUtils.HtmlEncode(sTitle);
                    if (dr["SIMAGE"] == null || dr["SIMAGE"] == DBNull.Value)
                    {
                        sResult = sResult + "<item id=\"sub" + dr["SMENUCODE"].ToString() + "\" text=\"" + sTitle + "\"  tooltip=\"" + sTitle + "\" >";
                    }
                    else
                    {
                        sResult = sResult + "<item id=\"sub" + dr["SMENUCODE"].ToString() + "\" text=\"" + sTitle + "\"  tooltip=\"" + sTitle + "\"  im0=\"" + dr["SIMAGE"].ToString() + "\" >";
                    }
                    sResult = sResult + "<userdata name=\"url\">";
                    sResult = sResult + "MenuDetail.aspx?menuCode=" + dr["SMENUCODE"].ToString();
                    sResult = sResult + "</userdata>";
                    
                    //创建子节点
                    CreateSubTreeXmlStructure(dt, dr["SMENUCODE"].ToString(), language, ref sResult);
                    sResult += "</item>";
                }
            }

        }
        #endregion
    }
}
