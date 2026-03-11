using System;
using System.Collections.Generic;
using System.Text;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Entity;
using System.Data;

namespace Com.ValuePlus.BLL.User
{
    public class UserLoginError
    {
        #region 写入错误登录信息
        /// <summary>
        /// 写入错误登录信息
        /// </summary>
        /// <param name="strUserId"></param>
        /// <param name="strPwd"></param>
        /// <returns></returns>
        public static void InertLoginError(string strUserId, string strPwd)
        {
            try
            {
                StringBuilder sbSql = new StringBuilder();
                String strKey = System.Guid.NewGuid().ToString();
                sbSql.Append("INSERT INTO [TB_HR_LOGIN_ERROR] ([SKEY] ,[SUSERID] ,[SPWD] ,[DTLOGINTIME])");
                sbSql.Append(" values('"+strKey+"','"+strUserId+"','"+strPwd+"','"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"')");
                String strSql = sbSql.ToString();
                int iCount = SqlParamDao.ExecuteNonQueryBySql(strSql);
            }
            catch (Exception ex)
            {
                Log.LogFactory.CreateInstance().Error(ex);
            }
        }
        #endregion

        #region 判断同一账号错误登录的次数
        /// <summary>
        /// 判断同一账号错误登录的次数
        /// </summary>
        /// <param name="strUserId"></param>
        /// <param name="Is24Hours">是否判断是同一天</param>
        /// <returns>次数</returns>
        public static int GetLoginErrorCount(string strUserId,bool Is24Hours)
        {
            int iCount = 0;
            try
            {
                StringBuilder sbSql = new StringBuilder();
                sbSql.Append("select COUNT(*) from [TB_HR_LOGIN_ERROR] where SUSERID = '" + strUserId + "'");
                //如果需要判断是同一天的话，则加上下面这句
                if (Is24Hours)
                {
                    sbSql.Append(" AND CONVERT(VARCHAR(20),GETDATE(),23) = CONVERT(VARCHAR(20),DTLOGINTIM,23)");
                }
                String strSql = sbSql.ToString();
                iCount = SqlParamDao.ExecuteScalarBySql(strSql);
            }
            catch (Exception ex)
            {
                Log.LogFactory.CreateInstance().Error(ex);
            }
            return iCount;
        }
        #endregion
        
        #region 清空账号错误登录日志
        /// <summary>
        /// 清空账号错误登录日志
        /// </summary>
        /// <param name="strUserId"></param>
        /// <returns>次数</returns>
        public static void ClearLoginErrorLog(string strUserId)
        {
            try
            {
                StringBuilder sbSql = new StringBuilder();
                sbSql.Append("delete from [TB_HR_LOGIN_ERROR] where SUSERID = '" + strUserId + "'");
                String strSql = sbSql.ToString();
                int iCount = SqlParamDao.ExecuteNonQueryBySql(strSql);
            }
            catch (Exception ex)
            {
                Log.LogFactory.CreateInstance().Error(ex);
            }
        }
        #endregion

        #region 停用用户
        /// <summary>
        /// 停用用户
        /// </summary>
        /// <param name="strUserId"></param>
        /// <returns></returns>
        public static void StopUser(string strUserId)
        {
            try
            {
                StringBuilder sbSql = new StringBuilder();
                sbSql.Append("UPDATE TB_HR_USER SET BISSTOP = '1' WHERE SUSERID = '" + strUserId + "'");
                String strSql = sbSql.ToString();
                int iCount = SqlParamDao.ExecuteNonQueryBySql(strSql);
            }
            catch (Exception ex)
            {
                Log.LogFactory.CreateInstance().Error(ex);
            }
        }
        #endregion

        #region 激活用户
        /// <summary>
        /// 激活用户
        /// </summary>
        /// <param name="strUserId"></param>
        /// <returns></returns>
        public static void ActiveUser(string strUserId)
        {
            try
            {
                StringBuilder sbSql = new StringBuilder();
                sbSql.Append("UPDATE TB_HR_USER SET BISSTOP = '2' WHERE SUSERID = '" + strUserId + "'");
                String strSql = sbSql.ToString();
                int iCount = SqlParamDao.ExecuteNonQueryBySql(strSql);
            }
            catch (Exception ex)
            {
                Log.LogFactory.CreateInstance().Error(ex);
            }
        }
        #endregion

    }
}
