using System;
using System.Collections.Generic;
using System.Text;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Entity;
using System.Data;

namespace Com.ValuePlus.BLL.User
{
    public class UserBll
    {
        
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="usercode"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public UserInfo login(string usercode, string pwd,string guid,string ip)
        {           
            return UserInfoDao.userIsRight(usercode, pwd, guid, ip);
        }

        #region 用户正常退出信息
        /// <summary>
        /// 用户正常退出信息
        /// </summary>
        /// <param name="strLoginKey"></param>
        /// <returns></returns>
        public int ExitLogin(String strUserId)
        {
            return UserInfoDao.exitLogin(strUserId);
        }
        #endregion

        #region 获得登陆用户功能菜单集合
        /// <summary>
        /// 获得登陆用户功能菜单集合
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public DataTable GetUserMenuDataTable(UserInfo userInfo)
        {
            DataTable dt = new DataTable();
            if (Com.ValuePlus.BLL.User.UserLoginBll.IsAdminstratorUser())
            {
                dt = UserInfoDao.GetUserTreeAdmin();
            }
            else
            {
                dt = UserInfoDao.GetUserTreeNotAdmin(userInfo.SUSERID);
            }
            return dt;
        }
        #endregion

        #region 获得管理员用户树菜单
        /// <summary>
        /// 获得管理员用户树菜单
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public string GetUserTreeAdmin(string language, String strMenuCode)
        {
            string sResult = string.Empty;
            DataTable dt = UserInfoDao.GetUserTreeAdmin();
            if (dt != null && dt.Rows.Count > 0)
            {
                CreateTreeXmlStructure(dt, strMenuCode, language, ref sResult);
            }
            return sResult;
        }
        #endregion

        #region 获得非管理员用户树菜单
       /// <summary>
        /// 获得非管理员用户树菜单
       /// </summary>
       /// <param name="usercode"></param>
       /// <param name="language"></param>
       /// <returns></returns>
        public string GetUserTreeNotAdmin(string usercode, String strMenuCode, string language)
        {
            string sResult = string.Empty;
            DataTable dt = UserInfoDao.GetUserTreeNotAdmin(usercode);
            if (dt != null && dt.Rows.Count > 0)
            {
                CreateTreeXmlStructure(dt, strMenuCode, language, ref sResult);
            }
            return sResult;
        }
        #endregion


        #region 获得未登录用户树菜单
        /// <summary>
        /// 获得未登录用户树菜单
        /// </summary>
        /// <param name="usercode"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public string GetUserTreeNotLogin()
        {
            string sResult = string.Empty;
            DataTable dt = UserInfoDao.GetUserTreeAdmin();
            if (dt != null && dt.Rows.Count > 0)
            {
                CreateTreeXmlStructureNotLogin(dt, Com.ValuePlus.Utils.Culture.CultureInfo.GetCustomCulture(), ref sResult);
            }
            return sResult;
        }
        #endregion

        #region 创建树结构未登录
        /// <summary>
        /// 创建树结构未登录
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="language"></param>
        /// <param name="sResult"></param>
        private void CreateTreeXmlStructureNotLogin(DataTable dt, string language, ref string sResult)
        {
            sResult = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><tree id=\"0\">";
            //首先创建根节点
            DataRow[] drs = dt.Select("SPARENTCODE is null or SPARENTCODE = ''");
            if (drs != null && drs.Length > 0)
            {
                string sTitle = drs[0]["SMENUNAME"].ToString();
                if (language == "zh-cn")
                {
                    sTitle = drs[0]["SMENUNAMECN"].ToString();
                }
                sTitle = Com.ValuePlus.Utils.HtmlUtils.HtmlEncode(sTitle);
                if (drs[0]["SIMAGE"] == null || drs[0]["SIMAGE"] == DBNull.Value)
                {
                    sResult = sResult + "<item id=\"" + drs[0]["SMENUCODE"].ToString() + "\" open=\"true\" text=\"" + sTitle + "\" tooltip=\"" + sTitle + "\" im0=\"dhtmlxtree_icon.gif\" im1=\"dhtmlxtree_icon.gif\" im2=\"dhtmlxtree_icon.gif\">";
                }
                else
                {
                    sResult = sResult + "<item id=\"" + drs[0]["SMENUCODE"].ToString() + "\" open=\"true\" text=\"" + sTitle + "\" tooltip=\"" + sTitle + "\" im0=\"" + drs[0]["SIMAGE"].ToString() + "\" im1=\"" + drs[0]["SIMAGE"].ToString() + "\" im2=\"" + drs[0]["SIMAGE"].ToString() + "\">";
                }
                if (!(drs[0]["SURLDETAIL"] == null || drs[0]["SURLDETAIL"] == DBNull.Value))
                {
                    sResult = sResult + "<userdata name=\"url\">";
                    sResult = sResult + Com.ValuePlus.Utils.StringUtils.ReplaceKeyStringToXml(drs[0]["SURLDETAIL"].ToString());
                    sResult = sResult + "</userdata>";
                }               
               
                sResult += "</item>";
            }
            sResult += "</tree>";
        }
        #endregion

        #region 创建树结构
        /// <summary>
        /// 创建树结构
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="language"></param>
        /// <param name="sResult"></param>
        public void  CreateTreeXmlStructure(DataTable dt,String strMenuCode, string language,ref string sResult){
            sResult = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><tree id=\"0\">";
            //首先创建根节点
            String strSqlFilter = "SPARENTCODE is null or SPARENTCODE = ''";
            if (!String.IsNullOrEmpty(strMenuCode))
            {
                strSqlFilter = "SMENUCODE = '" + strMenuCode + "'";
            }
            DataRow[] drs = dt.Select(strSqlFilter);
            if (drs != null && drs.Length > 0)
            {
                string sTitle = drs[0]["SMENUNAME"].ToString();
                if(language == "zh-cn"){
                    sTitle = drs[0]["SMENUNAMECN"].ToString();
                }
                sTitle = Com.ValuePlus.Utils.HtmlUtils.HtmlEncode(sTitle);
                if (drs[0]["SIMAGE"] == null || drs[0]["SIMAGE"] == DBNull.Value)
                {
                    sResult = sResult + "<item id=\"" + drs[0]["SMENUCODE"].ToString() + "\" open=\"true\" text=\"" + sTitle + "\" tooltip=\"" + sTitle + "\" im0=\"dhtmlxtree_icon.gif\" im1=\"dhtmlxtree_icon.gif\" im2=\"dhtmlxtree_icon.gif\">";
                }
                else
                {
                    sResult = sResult + "<item id=\"" + drs[0]["SMENUCODE"].ToString() + "\" open=\"true\" text=\"" + sTitle + "\" tooltip=\"" + sTitle + "\" im0=\"" + drs[0]["SIMAGE"].ToString() + "\" im1=\"" + drs[0]["SIMAGE"].ToString() + "\" im2=\"" + drs[0]["SIMAGE"].ToString() + "\">";
                }
                if (!(drs[0]["SURLDETAIL"] == null || drs[0]["SURLDETAIL"] == DBNull.Value))
                {
                    sResult = sResult + "<userdata name=\"url\">";
                    sResult = sResult + Com.ValuePlus.Utils.StringUtils.ReplaceKeyStringToXml(drs[0]["SURLDETAIL"].ToString());
                    sResult = sResult + "</userdata>";
                }
                //创建子节点
                CreateSubTreeXmlStructure(dt,drs[0]["SMENUCODE"].ToString(), language, ref sResult);
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
        private void CreateSubTreeXmlStructure(DataTable dt,string id, string language, ref string sResult)
        {
            DataRow[] drs = dt.Select("SPARENTCODE = '" + id + "'", "SORDER ASC");
            if (drs != null && drs.Length > 0)
            {
                foreach (DataRow dr in drs)
                {
                    string sTitle = dr["SMENUNAME"].ToString();
                    if (language == "zh-cn")
                    {
                        sTitle = dr["SMENUNAMECN"].ToString();
                    }
                    sTitle = Com.ValuePlus.Utils.HtmlUtils.HtmlEncode(sTitle);
                    if (dr["SIMAGE"] == null || dr["SIMAGE"] == DBNull.Value)
                    {
                        sResult = sResult + "<item id=\"" + dr["SMENUCODE"].ToString() + "\" text=\"" + sTitle + "\"  tooltip=\"" + sTitle + "\" >";
                    }
                    else
                    {
                        sResult = sResult + "<item id=\"" + dr["SMENUCODE"].ToString() + "\" text=\"" + sTitle + "\"  tooltip=\"" + sTitle + "\"  im0=\"" + dr["SIMAGE"].ToString() + "\" >";
                    }
                    if (!(dr["SURLDETAIL"] == null || dr["SURLDETAIL"] == DBNull.Value))
                    {
                        sResult = sResult + "<userdata name=\"url\">";
                        //if (dr["SMENUTYPE"].ToString() == "P")
                        //{
                        //    sResult = sResult + dr["SURLDETAIL"].ToString();
                        //    //this.myXmlTextWriter.WriteAttributeString("NavigateUrl", this.replaceUserParameter((string)row["ODETAIL"], usercode));
                        //}
                        //else if ( dr["SMENUTYPE"].ToString() == "D")
                        //{
                        //    sResult = sResult + "WFvouc.aspx?DOCU=" + dr["SURLDETAIL"].ToString();
                        //}
                        //else if (dr["SMENUTYPE"].ToString() == "E")
                        //{
                        //    sResult = sResult + "WFfoldv.aspx?DIR=" + dr["SURLDETAIL"].ToString();
                        //}
                        //else if (dr["SMENUTYPE"].ToString() == "R")
                        //{
                        //    sResult = sResult + "WFrepo.aspx?RPT=" + dr["SURLDETAIL"].ToString();
                        //}
                        //else if (dr["SMENUTYPE"].ToString() == "Q")
                        //{
                        //    sResult = sResult + "WFquery.aspx?QRY=" + dr["SURLDETAIL"].ToString();
                        //}
                        //else if (dr["SMENUTYPE"].ToString() == "C")
                        //{
                        //    sResult = sResult + "WFrepoc.aspx?RPT=" + dr["SURLDETAIL"].ToString();
                        //}
                        //else if (dr["SMENUTYPE"].ToString() == "U")
                        //{
                        //    sResult = sResult + "WFfoldu.aspx?DIR=" + dr["SURLDETAIL"].ToString();
                        //}
                        //else if (dr["SMENUTYPE"].ToString() == "S")
                        //{
                        //    sResult = sResult + "WFspq.aspx?SP=" + dr["SURLDETAIL"].ToString();                            
                        //}
                        //else if (dr["SMENUTYPE"].ToString() == "A")
                        //{
                        //    sResult = sResult + dr["SURLDETAIL"].ToString();
                        //    //this.myXmlTextWriter.WriteAttributeString("NavigateUrl", this.replaceUserParameter((string)row["ODETAIL"], usercode));
                        //}
                        //else
                        //{
                        //    sResult = sResult + dr["SURLDETAIL"].ToString();
                        //}                      
                        sResult = sResult + Com.ValuePlus.Utils.StringUtils.ReplaceKeyStringToXml(dr["SURLDETAIL"].ToString());
                        sResult = sResult + "</userdata>";
                    }
                    //创建子节点
                    CreateSubTreeXmlStructure(dt, dr["SMENUCODE"].ToString(), language, ref sResult);
                    sResult += "</item>";
                }                
            }	

        }
        #endregion

        #region 通过用户ID获取相应用户信息,返回UserInfo实体
        /// <summary>
        /// 通过用户ID获取相应用户信息,返回UserInfo实体
        /// </summary>
        /// <param name="strUserId"></param>
        /// <returns></returns>
        public UserInfo GetUserInfoByUserId(String strUserId)
        {
            return UserInfoDao.getUserInfoByUserId(strUserId);
        }
        #endregion

        public int login()
        {
            throw new NotImplementedException();
        }
    }
}
