using System;
using System.Collections.Generic;
using System.Text;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Entity;
using System.Data;


namespace Com.ValuePlus.BLL.User
{
    public class UserErr
    {
        #region 写入错误日志信息
        /// <summary>
        /// 写入错误日志信息
        /// </summary>
        /// <param name="USERCODE"></param>
        /// <param name="TID"></param>
        /// <param name="SID"></param>
        /// <param name="RID"></param>
        /// <param name="DETAIL"></param>
        /// <returns></returns>
        public static int InertError(string USERCODE, string TID, string SID, string RID, string DETAIL)
        {
            try
            {
                return UerErrDao.InertError(USERCODE, TID, SID, RID, DETAIL);
            }
            catch (Exception ex)
            {
                Log.LogFactory.CreateInstance().Error(ex);
                return 0;
            }
        }
        #endregion
    }
}
