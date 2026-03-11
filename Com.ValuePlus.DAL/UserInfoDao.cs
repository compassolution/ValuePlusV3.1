using System;
using System.Data;
using System.Data.Common;
using Com.ValuePlus.Database;
using Com.ValuePlus.Common.Config;
using Com.ValuePlus.Entity;
namespace Com.ValuePlus.DAL
{
    public class UserInfoDao
    {
        #region 判断用户名密码是否正确
        /// <summary>
        /// 判断用户名密码是否正确
        /// </summary>
        /// <param name="strAccount"></param>
        /// <param name="strPwd"></param>
        /// <returns></returns>
        public static UserInfo userIsRight(String strAccount, String strPwd, String guid, string ip)
        {
            UserInfo userInfo = null;
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(SqlBasicMetaDataConfig.Instance.GetUserLoginAuth());
                param[0].Value = strAccount;
                param[1].Value = strPwd;
                param[2].Value = guid;
                param[3].Value = ip;               
                using (IDataReader dr = dao.ExecuteReader(SqlBasicMetaDataConfig.Instance.GetUserLoginAuth(), param))
                {
                    while (dr.Read() && dr.FieldCount > 1)
                    {
                        userInfo = new UserInfo();
                        if (dr["BISALERT"] != DBNull.Value)
                        {
                            userInfo.BISALERT = dr["BISALERT"].ToString();
                        }
                        if (dr["BISGROUPUSER"] != DBNull.Value)
                        {
                            userInfo.BISGROUPUSER = dr["BISGROUPUSER"].ToString();
                        }
                        if (dr["BISSTOP"] != DBNull.Value)
                        {
                            userInfo.BISSTOP = dr["BISSTOP"].ToString();
                        }
                        if (dr["SACCOUNTID"] != DBNull.Value)
                        {
                            userInfo.SACCOUNTID = dr["SACCOUNTID"].ToString();
                        }
                        if (dr["SDEPT"] != DBNull.Value)
                        {
                            userInfo.SDEPT = dr["SDEPT"].ToString();
                        }
                        if (dr["SDEPTCN"] != DBNull.Value)
                        {
                            userInfo.SDEPTCN = dr["SDEPTCN"].ToString();
                        }
                        if (dr["SPOSI"] != DBNull.Value)
                        {
                            userInfo.SPOSI = dr["SPOSI"].ToString();
                        }
                        if (dr["SPOSICN"] != DBNull.Value)
                        {
                            userInfo.SPOSICN = dr["SPOSICN"].ToString();
                        }
                        if (dr["STAFFNO"] != DBNull.Value)
                        {
                            userInfo.STAFFNO = dr["STAFFNO"].ToString();
                        }
                        if (dr["STREECLR"] != DBNull.Value)
                        {
                            userInfo.STREECLR = dr["STREECLR"].ToString();
                        }
                        if (dr["SUSERID"] != DBNull.Value)
                        {
                            userInfo.SUSERID = dr["SUSERID"].ToString();
                        }
                        if (dr["SUSERNAME"] != DBNull.Value)
                        {
                            userInfo.SUSERNAME = dr["SUSERNAME"].ToString();
                        }
                        if (dr["SUSERNAMECN"] != DBNull.Value)
                        {
                            userInfo.SUSERNAMECN = dr["SUSERNAMECN"].ToString();
                        }
                        if (dr["SWORKCLR"] != DBNull.Value)
                        {
                            userInfo.SWORKCLR = dr["SWORKCLR"].ToString();
                        }
                        if (dr["SPWD"] != DBNull.Value)
                        {
                            userInfo.SPWD = dr["SPWD"].ToString();
                        }                     
                    }
                }                
            }
            return userInfo;
        }
        #endregion

        #region 通过用户ID获取相应用户信息,返回UserInfo实体
        /// <summary>
        ///  通过用户ID获取相应用户信息,返回UserInfo实体
        /// </summary>
        /// <param name="strUserId"></param>
        /// <returns></returns>
        public static UserInfo getUserInfoByUserId(String strUserId)
        {
            UserInfo userInfo = null; 
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(SqlBasicMetaDataConfig.Instance.GetUserInfoByUserId());
                param[0].Value = strUserId;
                using (IDataReader dr = dao.ExecuteReader(SqlBasicMetaDataConfig.Instance.GetUserInfoByUserId(), param))
                {
                    while (dr.Read() && dr.FieldCount > 1)
                    {
                        userInfo = new UserInfo();
                        if (dr["BISALERT"] != DBNull.Value)
                        {
                            userInfo.BISALERT = dr["BISALERT"].ToString();
                        }
                        if (dr["BISGROUPUSER"] != DBNull.Value)
                        {
                            userInfo.BISGROUPUSER = dr["BISGROUPUSER"].ToString();
                        }
                        if (dr["BISSTOP"] != DBNull.Value)
                        {
                            userInfo.BISSTOP = dr["BISSTOP"].ToString();
                        }
                        if (dr["SACCOUNTID"] != DBNull.Value)
                        {
                            userInfo.SACCOUNTID = dr["SACCOUNTID"].ToString();
                        }
                        if (dr["SDEPTCODE"] != DBNull.Value)
                        {
                            userInfo.SDEPTCODE = dr["SDEPTCODE"].ToString();
                        }
                        if (dr["SDEPT"] != DBNull.Value)
                        {
                            userInfo.SDEPT = dr["SDEPT"].ToString();
                        }
                        if (dr["SDEPTCN"] != DBNull.Value)
                        {
                            userInfo.SDEPTCN = dr["SDEPTCN"].ToString();
                        }
                        if (dr["SPOSI"] != DBNull.Value)
                        {
                            userInfo.SPOSI = dr["SPOSI"].ToString();
                        }
                        if (dr["SPOSICN"] != DBNull.Value)
                        {
                            userInfo.SPOSICN = dr["SPOSICN"].ToString();
                        }
                        if (dr["STAFFNO"] != DBNull.Value)
                        {
                            userInfo.STAFFNO = dr["STAFFNO"].ToString();
                        }
                        if (dr["STREECLR"] != DBNull.Value)
                        {
                            userInfo.STREECLR = dr["STREECLR"].ToString();
                        }
                        if (dr["SUSERID"] != DBNull.Value)
                        {
                            userInfo.SUSERID = dr["SUSERID"].ToString();
                        }
                        if (dr["SUSERNAME"] != DBNull.Value)
                        {
                            userInfo.SUSERNAME = dr["SUSERNAME"].ToString();
                        }
                        if (dr["SUSERNAMECN"] != DBNull.Value)
                        {
                            userInfo.SUSERNAMECN = dr["SUSERNAMECN"].ToString();
                        }
                        if (dr["SWORKCLR"] != DBNull.Value)
                        {
                            userInfo.SWORKCLR = dr["SWORKCLR"].ToString();
                        }
                        if (dr["SPWD"] != DBNull.Value)
                        {
                            userInfo.SPWD = dr["SPWD"].ToString();
                        }
                    }
                }
            }
            return userInfo;
        }
        #endregion

        #region 用户正常退出系统
        /// <summary>
        /// 用户正常退出系统
        /// </summary>
        /// <param name="strAccount"></param>
        /// <param name="strPwd"></param>
        /// <returns></returns>
        public static int exitLogin(String strUserId)
        {
            int iCount = 0;
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(SqlBasicMetaDataConfig.Instance.GetExitLogin());
                param[0].Value = strUserId;
                object obj = dao.ExecuteNonQuery(SqlBasicMetaDataConfig.Instance.GetExitLogin(), param);
                if (obj != null)
                {
                    iCount = Convert.ToInt32(obj);
                }
                
            }
            return iCount;
        }
        #endregion

        #region 获得管理员用户树菜单
        /// <summary>
        /// 获得管理员用户树菜单
        /// </summary>
        /// <returns></returns>
        public static  DataTable GetUserTreeAdmin()
        {
            DataTable dt = null;
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DataSet ds = dao.ExecuteDataSet(SqlBasicMetaDataConfig.Instance.GetUserTreeAdmin(), null);            
                if(ds!=null && ds.Tables.Count>0){
                    dt = ds.Tables[0];
                }
            }
            return dt;
        }
        #endregion

        #region 获得非管理员用户树菜单
        /// <summary>
        /// 获得非管理员用户树菜单
        /// </summary>
       /// <param name="usercode"></param>
       /// <returns></returns>
        public static DataTable GetUserTreeNotAdmin(string usercode)
        {
            DataTable dt = null;
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(SqlBasicMetaDataConfig.Instance.GetUserTreeNotAdmin());
                param[0].Value = usercode;
                DataSet ds = dao.ExecuteDataSet(SqlBasicMetaDataConfig.Instance.GetUserTreeNotAdmin(), param);
                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                }
            }
            return dt;
        }
        #endregion

        ////查询用户ID
        //public int getIdByUserid(string userid)
        //{
        //    int count = sqlHelper.ReturnSQL("select id from userinfo where userid='" + userid + "'");
        //    return count;
        //}
        ////查询用户角色
        //public int getRoleTypeInfo(string userID)
        //{
        //    int count = 0;
        //    count = sqlHelper.ReturnSQL("select roleid from userinfo where userid='" + userID + "'");
        //    return count;
        //}
        ////判断用户状态
        //public int userStates(string userID)
        //{
        //    int count = 0;
        //    count = sqlHelper.ReturnSQL("select userstate from userinfo where userid='" + userID + "'");
        //    return count;
        //}

        ////得到总记录数
        //public int GetUserInfoCount()
        //{
        //    try
        //    {
        //        int count = sqlHelper.ReturnSQL("select count(*) from userinfo");
        //        return count;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}
        ////查询用户信息
        //public DataSet GetUserInfo(int start, int limit)
        //{
        //    try
        //    {
        //        ds = sqlHelper.GetDataSet("select top " + limit + " * from userinfo where id not in(select top " + start + " id from userinfo order by id desc) order by id desc");
        //        return ds;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        ////添加用户信息
        //public int AddUserInfo(UserInfoBean user)
        //{
        //    try
        //    {
        //        int count = sqlHelper.RunSQL("insert into userinfo values('" + user.userid + "','" + user.userName + "','" + user.userPwd + "'," + user.userState + "," + user.roleid + ")");
        //        return count;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}
        ////保存用户信息
        //public int EditUserInfo(UserInfoBean user)
        //{
        //    try
        //    {
        //        int count = sqlHelper.RunSQL("update userinfo set username='" + user.userName + "',userpwd='" + user.userPwd + "',userstate=" + user.userState + ",roleid=" + user.roleid + " where userid='" + user.userid + "'");
        //        return count;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}
        ////删除用户信息
        //public int DelUserInfo(int id)
        //{
        //    try
        //    {
        //        int count = sqlHelper.RunSQL("delete userinfo where id=" + id);
        //        return count;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        ////根据ID查询用户信息
        //public DataSet GetUserInfoById(string id)
        //{
        //    try
        //    {
        //        ds = sqlHelper.GetDataSet("select * from userinfo where id=" + id);
        //        return ds;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        ////保存用户信息
        //public int SaveUserPwdInfo(string userid, string userpwd)
        //{
        //    try
        //    {
        //        int count = sqlHelper.RunSQL("update userinfo set userpwd='" + userpwd + "' where userid='" + userid + "'");
        //        return count;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}
    }
}
