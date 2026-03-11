using System;
using System.Data;
using System.Data.Common;
using Com.ValuePlus.Database;
using Com.ValuePlus.Common.Config;
namespace Com.ValuePlus.DAL
{
    public class UerErrDao
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
        public static int InertError(string USERCODE, string TID, string SID,string RID,string DETAIL)
        {
            int count = 0;
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(SqlBasicMetaDataConfig.Instance.GetUserErrInsert());
                param[0].Value = USERCODE;
                param[1].Value = TID;
                param[2].Value = SID;
                param[3].Value = RID;
                param[4].Value = DETAIL;
                count = dao.ExecuteNonQuery(SqlBasicMetaDataConfig.Instance.GetUserErrInsert(), param);
            }
            return count;
        }
        #endregion
    }
}
