using System;
using System.Text;
using Com.ValuePlus.DAL.AppFuction.OverTimeRestVerify;
using System.Data;
using System.Collections;

namespace Com.ValuePlus.BLL.AppFunction.OverTimeRestVerify
{
    public class OTRestVerifyBll
    {
        #region 根据当前用户ID查询其对应管理的员工信息的所有记录，返回DataSet记录集
        /// <summary>
        /// 根据当前用户ID查询其对应管理的员工信息的所有记录，返回DataSet记录集
        /// </summary>
        /// <param name="strUserId"></param>
        /// <returns></returns>
        public DataSet GetStuffInfoByUserId(String strUserId)
        {
            OTRestVerifyDao dao = new OTRestVerifyDao();
            return dao.findStuffInfoByUserId(strUserId);
        }
        #endregion

        #region 通过当前员工编号查询表【KQOVTM_1】中对应的加班记录，返回DataSet记录集
        /// <summary>
        /// 通过当前员工编号查询表【KQOVTM_1】中对应的加班记录，返回DataSet记录集
        /// <param name="strStuffId"></param>
        /// <param name="dtFrom"></param>
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet GetOverTimeInfoByStuffId(String strStuffId, DateTime dtFrom)
        {
            OTRestVerifyDao dao = new OTRestVerifyDao();
            return dao.findOverTimeInfoByStuffId(strStuffId, dtFrom);
        }
        #endregion

        #region 通过当前员工编号查询表【KQLV_1】中对应的调休记录，返回DataSet记录集
        /// <summary>
        /// 通过当前员工编号查询表【KQLV_1】中对应的加班记录，返回DataSet记录集
        /// <param name="strStuffId"></param>
        /// <param name="dtFrom"></param>
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet GetLvInfoByStuffId(String strStuffId, DateTime dtFrom)
        {
            OTRestVerifyDao dao = new OTRestVerifyDao();
            return dao.findLvInfoByStuffId(strStuffId, dtFrom);
        }
        #endregion
        
        #region 根据存储过程名称及其参数执行存储过程，返回影响记录数
        /// <summary>
        /// 根据存储过程名称及其参数执行存储过程，返回影响记录数
        /// </summary>
        /// <param name="strSpName"></param>
        /// <param name="hsTableParam"></param>
        /// <returns></returns>
        public int ExcuteSP(String strSpName, Hashtable hsTableParam)
        {
            OTRestVerifyDao dao = new OTRestVerifyDao();
            return dao.ExcuteSP(strSpName, hsTableParam);

        }
        #endregion

    }
}
