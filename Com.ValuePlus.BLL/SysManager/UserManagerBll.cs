using System;
using System.Collections.Generic;
using System.Text;
using Com.ValuePlus.DAL;
using System.Data;

namespace Com.ValuePlus.BLL.SysManager
{
    public class UserManagerBll
    {

        #region 查询系统用户信息表所有记录,返回dataset
        /// <summary>
        /// 查询系统用户信息表所有记录
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllUserInfo()
        {
            UserManagerDao daoUser = new UserManagerDao();
            return daoUser.findAll();
        }
        #endregion

        #region 查询系统用户信息表所有记录,返回datatable
        /// <summary>
        /// 查询系统用户信息表所有记录
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllUserInfoTable()
        {
            UserManagerDao daoUser = new UserManagerDao();
            return daoUser.findAllTable();
        }
        #endregion

        #region 根据主键查询系统用户表记录，返回datatable记录
        /// <summary>
        /// 根据主键查询系统用户表记录，返回datatable记录
        /// </summary>
        /// <param name="strUserId"></param>
        /// <returns>DataSet</returns>
        public DataTable GetUserInfoByUserId(String strUserId)
        {
            UserManagerDao daoUser = new UserManagerDao();
            return daoUser.findTableByUserId(strUserId);
        }
        #endregion

        #region 判断用户编码是否已经存在
        /// <summary>
        /// 判断用户编码是否已经存在
        /// </summary>
        /// <param name="strUserId"></param>
        /// <returns></returns>
        public Boolean IsExsitUserId(String strUserId)
        {
            Boolean bIsExsit = true;
            UserManagerDao daoUser = new UserManagerDao();
            DataSet ds = daoUser.findByUserId(strUserId);
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

        #region 判断用户登录名是否已经存在
        /// <summary>
        /// 判断用户登录名是否已经存在
        /// </summary>
        /// <param name="strAccountId"></param>
        /// <returns></returns>
        public Boolean IsExsitAccountId(String strAccountId)
        {
            Boolean bIsExsit = true;
            UserManagerDao daoUser = new UserManagerDao();
            DataSet ds = daoUser.findByAccountId(strAccountId);
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

        #region 插入系统用户表一条记录
        /// <summary>
        /// 插入系统用户表一条记录
        /// </summary>
        /// <param name="strSUSERID"></param>
        /// <param name="strSACCOUNTID"></param>
        /// <param name="strSPWD"></param>
        /// <param name="strSTAFFNO"></param>
        /// <param name="strSUSERNAME"></param>
        /// <param name="strSUSERNAMECN"></param>
        /// <param name="strSDEPT"></param>
        /// <param name="strSDEPTCN"></param>
        /// <param name="strSPOSI"></param>
        /// <param name="strSPOSICN"></param>
        /// <param name="strSTREECLR"></param>
        /// <param name="strSWORKCLR"></param>
        /// <param name="strBISALERT"></param>
        /// <param name="strBISGROUPUSER"></param>
        /// <param name="strBISSTOP"></param>
        /// <returns></returns>
        public int AddUserInfo(String strSUSERID, String strSACCOUNTID, String strSPWD, String strSTAFFNO, String strSUSERNAME, String strSUSERNAMECN, String strSDEPT, String strSDEPTCN, String strSPOSI, String strSPOSICN, String strSTREECLR, String strSWORKCLR, String strBISALERT, String strBISGROUPUSER, String strBISSTOP)
        {
            UserManagerDao daoUser = new UserManagerDao();
            return daoUser.insertOneRow(strSUSERID, strSACCOUNTID, strSPWD, strSTAFFNO, strSUSERNAME, strSUSERNAMECN, strSDEPT, strSDEPTCN, strSPOSI, strSPOSICN, strSTREECLR, strSWORKCLR, strBISALERT, strBISGROUPUSER, strBISSTOP) ;
        }
        #endregion

        #region 根据用户ID更新系统用户表一条记录
        /// <summary>
        /// 根据用户ID更新系统用户表一条记录
        /// </summary>
        /// <param name="strSUSERID"></param>
        /// <param name="strSACCOUNTID"></param>
        /// <param name="strSPWD"></param>
        /// <param name="strSTAFFNO"></param>
        /// <param name="strSUSERNAME"></param>
        /// <param name="strSUSERNAMECN"></param>
        /// <param name="strSDEPT"></param>
        /// <param name="strSDEPTCN"></param>
        /// <param name="strSPOSI"></param>
        /// <param name="strSPOSICN"></param>
        /// <param name="strSTREECLR"></param>
        /// <param name="strSWORKCLR"></param>
        /// <param name="strBISALERT"></param>
        /// <param name="strBISGROUPUSER"></param>
        /// <param name="strBISSTOP"></param>
        /// <returns></returns>
        public int UpdateUserInfo(String strSUSERID, String strSACCOUNTID, String strSPWD, String strSTAFFNO, String strSUSERNAME, String strSUSERNAMECN, String strSDEPT, String strSDEPTCN, String strSPOSI, String strSPOSICN, String strSTREECLR, String strSWORKCLR, String strBISALERT, String strBISGROUPUSER, String strBISSTOP)
        {
            int iCount = 0;
            UserManagerDao daoUser = new UserManagerDao();
            if (IsExsitUserId(strSUSERID))
            {
                iCount = daoUser.updateByUserId(strSUSERID, strSACCOUNTID, strSPWD, strSTAFFNO, strSUSERNAME, strSUSERNAMECN, strSDEPT, strSDEPTCN, strSPOSI, strSPOSICN, strSTREECLR, strSWORKCLR, strBISALERT, strBISGROUPUSER, strBISSTOP);
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
        public string GetMenuTreeAll(string language,String strUserId)
        {   
            string sResult = string.Empty;
            MenuInfoDao daoMenu = new MenuInfoDao();
            UserMenuInfoDao daoUserMenu = new UserMenuInfoDao();
            DataTable dt = new DataTable();

            String strCurAccountUserId = "";//当前登录用户
            Com.ValuePlus.Entity.UserInfo userInfo = Com.ValuePlus.BLL.User.UserLoginBll.LoginUserInfo;
            if (userInfo != null)
            {
                strCurAccountUserId = userInfo.SUSERID;
            }

            if (strCurAccountUserId.ToLower().Equals("admin"))
            {
                //如果是管理员，则获取全部Menu
                dt = daoMenu.findAllTable();
            }
            else//如果是其他用户，则获取他本身具备的Menu才能分配给别人
            {
                dt = daoUserMenu.findTableByUserId(strCurAccountUserId);
            }

            //获取某一用户已经具备的栏目权限列表
            DataTable dtUserMenu = daoUserMenu.findTableByUserId(strUserId);

            if (dt != null && dt.Rows.Count > 0)
            {
                CreateTreeXmlStructure(dt,dtUserMenu, language, ref sResult, strUserId);
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
        private void CreateTreeXmlStructure(DataTable dt,DataTable dtUserMenu, string language, ref string sResult,String strUserId)
        {
            sResult = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><tree id=\"0\">";
            //首先创建根节点
            DataRow[] drs = dt.Select("SPARENTCODE is null or SPARENTCODE = ''");
            if (drs != null && drs.Length > 0)
            {
                String strBISSTOP = drs[0]["BISSTOP"] == null ? "1" : drs[0]["BISSTOP"].ToString();
                String strBISSTOP_Desc = strBISSTOP.Equals("1") ? "《Stopped》" : "";
                string sTitle = strBISSTOP_Desc+drs[0]["SMENUNAME"].ToString();
                if (language == "zh-cn")
                {
                    strBISSTOP_Desc = strBISSTOP.Equals("1") ? "《已停用》" : "";
                    sTitle = strBISSTOP_Desc + drs[0]["SMENUNAMECN"].ToString();
                }
                sTitle = Com.ValuePlus.Utils.HtmlUtils.HtmlEncode(sTitle);
                sResult = sResult + "<item id=\"" + drs[0]["SMENUCODE"].ToString() + "\" open=\"1\" text=\"" + sTitle + "\" tooltip=\"" + sTitle + "\" im0=\"tombs.gif\" im1=\"tombs.gif\" im2=\"iconSafe.gif\" call=\"1\" select=\"1\">";

                //创建子节点
                CreateSubTreeXmlStructure(dt,dtUserMenu, drs[0]["SMENUCODE"].ToString(), language, ref sResult,strUserId);
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
        private void CreateSubTreeXmlStructure(DataTable dt,DataTable dtUserMenu, string id, string language, ref string sResult,String strUserId)
        {
            DataRow[] drs = dt.Select("SPARENTCODE = '" + id + "'", "SORDER ASC");

            if (drs != null && drs.Length > 0)
            {
                //MenuManagerBll bllMenuManager = new MenuManagerBll();
                foreach (DataRow dr in drs)
                {
                    String strBISSTOP = dr["BISSTOP"] == null ? "1" : dr["BISSTOP"].ToString();
                    String strBISSTOP_Desc = strBISSTOP.Equals("1") ? "《Stopped》" : "";
                    String sTitle = strBISSTOP_Desc + dr["SMENUNAME"].ToString();
                    String strMenuCode = dr["SMENUCODE"].ToString();
                    String strMenuType = dr["SMENUTYPE"].ToString();
                    //判断该用户是否具有该栏目权限，如果有，则默认选中
                    String strIsChecked = ">";
                    DataRow[] drsUserMenu = dtUserMenu.Select("SMENUCODE = '" + strMenuCode + "'");
                    if (drsUserMenu != null && drsUserMenu.Length > 0)
                    {
                        if (!strMenuType.Equals("F"))//如果不是文件夹
                        {
                            strIsChecked = " checked=\"1\">";
                        }
                    }
                    
                    if (language == "zh-cn")
                    {
                        strBISSTOP_Desc = strBISSTOP.Equals("1") ? "《已停用》" : "";
                        sTitle = strBISSTOP_Desc + dr["SMENUNAMECN"].ToString();
                    }
                    sTitle = Com.ValuePlus.Utils.HtmlUtils.HtmlEncode(sTitle);
                    sResult = sResult + "<item id=\"" + dr["SMENUCODE"].ToString() + "\" text=\"" + sTitle + "\"  tooltip=\"" + sTitle + "\" im0=\"tombs.gif\" im1=\"tombs.gif\" im2=\"iconSafe.gif\" " + strIsChecked;

                    //创建子节点
                    CreateSubTreeXmlStructure(dt, dtUserMenu, dr["SMENUCODE"].ToString(), language, ref sResult,strUserId);
                    sResult += "</item>";
                }
            }

        }
        #endregion

        #region 判断栏目编码是否在用户栏目记录集中存在
        /// <summary>
        /// 判断栏目编码是否在用户栏目记录集中存在
        /// </summary>
        /// <param name="strUserId"></param>
        /// <param name="strMenuCode"></param>
        /// <returns></returns>
        public Boolean IsExsitMenuOfUser(String strUserId, String strMenuCode)
        {
            Boolean bIsExsit = false;
            UserMenuInfoDao daoUser = new UserMenuInfoDao();
            DataTable dt = daoUser.findTableByUserId(strUserId);
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow[] drs = dt.Select("SMENUCODE = '" + strMenuCode + "'");
                if (drs != null && drs.Length > 0)
                {
                    bIsExsit = true;
                }
            }

            return bIsExsit;
        }
        #endregion

        #region 为某用户分配一组栏目
        /// <summary>
        /// 为某用户分配一组栏目
        /// </summary>
        /// <param name="strUserId"></param>
        /// <param name="strMenuCode[]"></param>
        /// <returns></returns>
        public int AssignUserMenu(String strUserId, String[] strMenuCode)
        {
            UserMenuInfoDao daoUser = new UserMenuInfoDao();

            return daoUser.insertUserMenuRecord(strUserId, strMenuCode);
        }
        #endregion

        #region 获取当前用户所具有但是需分配角色的用户不具有的角色列表，返回DataTable，其中如果是超级用户则返回角色表中所有角色
        /// <summary>
        /// 获取当前用户所具有但是需分配角色的用户不具有的角色列表，返回DataTable，其中如果是超级用户则返回角色表中所有角色
        /// </summary>
        /// <param name="strCurUserId"></param>
        /// <param name="strSelUserId"></param>
        /// <param name="bIsAdmin"></param>
        /// <returns></returns>
        public DataTable GetRoleInfoByCurUserIdAndSelUser(String strCurUserId, String strSelUserId,bool bIsAdmin)
        {
            UserRoleDao daoRole = new UserRoleDao();
            DataTable dt = null;
            if (bIsAdmin)//如果是超级用户，则读取所有角色信息
            {
                dt = daoRole.findPartColOfNotInUserRoleByUserId(strSelUserId);
            }
            else
            {
                dt = daoRole.findRoleInfoTableByCurUserIdAndSelUserId(strCurUserId, strSelUserId);
            }

            return dt;
        }
        #endregion

        #region 根据用户ID获得该用户具有的角色列表，返回DataTable
        /// <summary>
        /// 根据用户ID获得该用户具有的角色列表，返回DataTable
        /// </summary>
        /// <param name="strUserId"></param>
        /// <returns></returns>
        public DataTable GetRoleInfoByUserId(String strUserId)
        {
            UserRoleDao daoRole = new UserRoleDao();
            DataTable dt = null;
            dt = daoRole.findRoleInfoTableByUserId(strUserId);
            
            return dt;
        }
        #endregion

        #region 为某用户分配一组角色
        /// <summary>
        /// 为某用户分配一组角色
        /// </summary>
        /// <param name="strUserId"></param>
        /// <param name="strRidArr[]"></param>
        /// <returns></returns>
        public int AssignUserRole(String strUserId, String[] strRidArr)
        {
            UserRoleDao daoUserRole = new UserRoleDao();

            return daoUserRole.insertUserRoleRecord(strUserId, strRidArr);
        }
        #endregion

        #region 根据用户ID修改用户登录密码
        /// <summary>
        /// 根据用户ID修改用户登录密码
        /// </summary>
        /// <param name="strPwd"></param>
        /// <param name="strUserId"></param>
        /// <returns></returns>
        public int ChangeUserPwd(String strPwd, String strUserId)
        {
            UserManagerDao daoUser = new UserManagerDao();
            return daoUser.updatePwdByUserId(strPwd, strUserId);
        }
        #endregion


    }

}
