using System;
using System.Collections.Generic;
using System.Text;
using Com.ValuePlus.DAL.WinForm;
using System.Data;
using System.Collections;

namespace Com.ValuePlus.BLL.WinForm
{
    public class KQPaiBanBll
    {
        #region 查询考勤日期设定表所有记录，返回DataSet记录集
        /// <summary>
        /// 查询考勤日期设定表所有记录，返回DataSet记录集
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllKQPred()
        {
            KQPerdDao daoKQPerd = new KQPerdDao();
            return daoKQPerd.findAll();
        }
        #endregion

        #region 查询考勤日期设定表所有记录，返回DataSet记录集
        /// <summary>
        /// 查询考勤日期设定表所有记录，返回DataSet记录集
        /// </summary>
        /// <returns></returns>
        public DataSet GetKQPredInfoById(String strPid)
        {
            KQPerdDao daoKQPerd = new KQPerdDao();
            return daoKQPerd.findById(strPid);
        }
        #endregion

        #region 通过YEARMONTH查询表【KQTOE_1】和【KQTOE_2】一条记录，获取列名，返回DataSet记录集
        /// <summary>
        /// 通过YEARMONTH查询表【KQTOE_1】和【KQTOE_2】一条记录，获取列名，返回DataSet记录集
        /// </summary>
        /// <returns></returns>
        public DataSet GetKQTOE1A2InfoGridCol()
        {
            KQPaiBanDAO daoKQPaiBan = new KQPaiBanDAO();
            return daoKQPaiBan.findKQTOE1A2GridCol();
        }
        #endregion

        #region 通过YEARMONTH查询表【KQTOE_1】和【KQTOE_2】所有记录，返回DataSet记录集
        /// <summary>
        /// 通过YEARMONTH查询表【KQTOE_1】和【KQTOE_2】所有记录，返回DataSet记录集
        /// <param name="strYearMonth"></param>
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllKQTOE1A2InfoByYearMonth(String strYearMonth)
        {
            KQPaiBanDAO daoKQPaiBan = new KQPaiBanDAO();
            return daoKQPaiBan.findKQTOE1A2ByYearMonth(strYearMonth);
        }
        #endregion

        #region 通过YEARMONTH获取指定用户可以查询到表【KQTOE_1】和【KQTOE_2】的记录，返回DataSet记录集
        /// <summary>
        /// 通过YEARMONTH获取指定用户可以查询到表【KQTOE_1】和【KQTOE_2】的记录，返回DataSet记录集
        /// <param name="strYearMonth"></param>
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllKQTOE1A2InfoByYearMonthAUserId(String strYearMonth,String strUserId)
        {
            KQPaiBanDAO daoKQPaiBan = new KQPaiBanDAO();
            return daoKQPaiBan.findKQTOE1A2ByYearMonthAUserId(strYearMonth, strUserId);
        }
        #endregion
        
        #region 根据主键批量更新表KQTOE_2的记录,返回记录数
        /// <summary>
        /// 根据主键批量更新表KQTOE_2的记录,返回记录数
        /// </summary>
        /// <param name="strModifyResults"></param>
        /// <returns></returns>
        public int ModifyBatchKQTOE2DataByKey(String strModifyResults)
        {
            KQPaiBanDAO daoKQPaiBan = new KQPaiBanDAO();

            return daoKQPaiBan.updateBatchKQTOE2DataByKey(strModifyResults);
        }
        #endregion

        #region 查询考勤班次表【KQSHIF_1】一条记录，获取列名，返回DataSet记录集
        /// <summary>
        /// 查询考勤班次表【KQSHIF_1】一条记录，获取列名，返回DataSet记录集
        /// </summary>
        /// <returns></returns>
        public DataSet GetKQShifGridCol()
        {
            KQPaiBanDAO daoKQPaiBan = new KQPaiBanDAO();
            return daoKQPaiBan.findKQShifGridCol();
        }
        #endregion

        #region 查询考勤班次表【KQSHIF_1】所有记录，返回DataSet记录集
        /// <summary>
        /// 查询考勤班次表【KQSHIF_1】所有记录，返回DataSet记录集
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllKQShif()
        {
            KQPaiBanDAO daoKQPaiBan = new KQPaiBanDAO();
            return daoKQPaiBan.findAllKQShif();
        }
        #endregion

        #region 根据当前用户查询其可查看的考勤班次表【KQSHIF_1】记录，返回DataSet记录集
        /// <summary>
        /// 根据当前用户查询其可查看的考勤班次表【KQSHIF_1】记录，返回DataSet记录集
        /// </summary>
        /// <returns></returns>
        public DataSet GetKQShifInfoByUserId(String strUserId)
        {
            KQPaiBanDAO daoKQPaiBan = new KQPaiBanDAO();
            return daoKQPaiBan.findKQShifInfoByUserId(strUserId);
        }
        #endregion


        #region 根据考勤月份获取表TB_HR_KQ_UNNORMAL记录，返回DataSet记录集
        /// <summary>
        /// 根据考勤月份获取表TB_HR_KQ_UNNORMAL记录，返回DataSet记录集
        /// </summary>
        /// <param name="strYearMonth"></param>
        /// <returns></returns>
        public DataSet GetUnNormalInfoByYearMonth(String strYearMonth)
        {
            KQUnNormalResultDao daoKQUnNormal = new KQUnNormalResultDao();
            return daoKQUnNormal.findUnNormalByYearMonth(strYearMonth);
        }
        #endregion

        #region 根据员工编号及考勤日期获取表TB_HR_KQ_UNNORMAL记录，返回DataSet记录集
        /// <summary>
        /// 根据员工编号及考勤日期获取表TB_HR_KQ_UNNORMAL记录，返回DataSet记录集
        /// </summary>
        /// <param name="strEmNo"></param>
        /// <param name="strYearMonth"></param>
        /// <param name="strDay"></param>
        /// <returns></returns>
        public DataSet GetUnNormalInfoByEmnoADay(String strEmNo, String strYearMonth, String strDay)
        {
            KQUnNormalResultDao daoKQUnNormal = new KQUnNormalResultDao();
            return daoKQUnNormal.findUnNormalByEmnoADay(strEmNo, strYearMonth, strDay);
        }
        #endregion

        #region 根据员工编号及考勤日期更新表TB_HR_KQ_UNNORMAL记录，返回记录数
        /// <summary>
        /// 根据员工编号及考勤日期更新表TB_HR_KQ_UNNORMAL记录，返回记录数
        /// </summary>
        /// <param name="strKey"></param>
        /// <param name="strEmNo"></param>
        /// <param name="strYearMonth"></param>
        /// <param name="strDay"></param>
        /// <param name="strState"></param>
        /// <returns></returns>
        public int ModifyUnNormalByEmnoADay(String strEmNo, String strYearMonth, String strDay, String strState)
        {
            KQUnNormalResultDao daoKQUnNormal = new KQUnNormalResultDao();

            return daoKQUnNormal.updateUnNormalByEmnoADay(strEmNo, strYearMonth, strDay, strState);
        }
        #endregion

        #region 查询【KQRSSZ_1】和【KQRSSZ_2】一条记录，获取列名，返回DataSet记录集
        /// <summary>
        /// 查询【KQRSSZ_1】和【KQRSSZ_2】一条记录，获取列名，返回DataSet记录集
        /// </summary>
        /// <returns></returns>
        public DataSet GetKQResultGridCol()
        {
            KQPaiBanResultDao daoKQResult = new KQPaiBanResultDao();
            return daoKQResult.findResultGridCol();
        }
        #endregion

        #region 对数据表【KQRSSZ_1】和【KQRSSZ_2】的通过员工编号，月份及日期select操作，返回DataSet记录集
        /// <summary>
        /// 对数据表【KQRSSZ_1】和【KQRSSZ_2】的通过员工编号，月份及日期select操作，返回DataSet记录集
        /// </summary>
        /// <param name="strEmNo"></param>
        /// <param name="strYearMonth"></param>
        /// <param name="dtDay"></param>
        /// <returns></returns>
        public DataSet GetKQResultByEmnoADay(String strEmNo, String strYearMonth, DateTime dtDay)
        {
            KQPaiBanResultDao daoKQResult = new KQPaiBanResultDao();
            return daoKQResult.findResultByEmnoADay(strEmNo, strYearMonth, dtDay);
        }
        #endregion

        #region 根据用户ID对数据表【KQRSSZ_1】和【KQRSSZ_2】的通过员工编号，月份及日期select操作，返回DataSet记录集
        /// <summary>
        /// 根据用户ID对数据表【KQRSSZ_1】和【KQRSSZ_2】的通过员工编号，月份及日期select操作，返回DataSet记录集
        /// </summary>
        /// <param name="strYearMonth"></param>
        /// <param name="strUserId"></param>
        /// <param name="strFlagToNormal"></param>
        /// <returns></returns>
        public DataSet GetKQResultByEmnoADayAUserId(String strYearMonth, String strUserId, String strFlagToNormal)
        {
            KQPaiBanResultDao daoKQResult = new KQPaiBanResultDao();
            return daoKQResult.findResultByEmnoADayAUserId(strYearMonth, strUserId, strFlagToNormal);
        }
        #endregion

        #region (获取总数)根据用户ID对数据表【KQRSSZ_1】和【KQRSSZ_2】的通过员工编号，月份及日期select操作，返回记录数
        /// <summary>
        /// (获取总数)根据用户ID对数据表【KQRSSZ_1】和【KQRSSZ_2】的通过员工编号，月份及日期select操作，返回记录数
        /// </summary>
        /// <param name="strYearMonth"></param>
        /// <param name="strUserId"></param>
        /// <param name="strFlagToNormal"></param>
        /// <returns></returns>
        public int GetKQResultCountByEmnoADayAUserId(String strYearMonth, String strUserId, String strFlagToNormal)
        {
            KQPaiBanResultDao daoKQResult = new KQPaiBanResultDao();
            return daoKQResult.findCountByEmnoADayAUserId(strYearMonth, strUserId, strFlagToNormal);
        }
        #endregion

        #region (分页获取)根据用户ID对数据表【KQRSSZ_1】和【KQRSSZ_2】的通过员工编号，月份及日期select操作，返回DataSet记录集
        /// <summary>
        /// (分页获取)根据用户ID对数据表【KQRSSZ_1】和【KQRSSZ_2】的通过员工编号，月份及日期select操作，返回DataSet记录集
        /// </summary>
        /// <param name="strYearMonth"></param>
        /// <param name="strUserId"></param>
        /// <param name="iPageSize"></param>
        /// <param name="iStartIndex"></param>
        /// <param name="strFlagToNormal"></param>
        /// <returns></returns>
        public DataSet GetKQResultByEmnoADayAUserId(String strYearMonth, String strUserId, int iPageSize, int iStartIndex,String strFlagToNormal)
        {
            KQPaiBanResultDao daoKQResult = new KQPaiBanResultDao();
            return daoKQResult.findResultByEmnoADayAUserId(strYearMonth, strUserId, iPageSize, iStartIndex, strFlagToNormal);
        }
        #endregion

        #region 通过员工编号，月份及日期更新对数据表【KQRSSZ_2】，返回DataSet记录数
        /// <summary>
        /// 通过员工编号，月份及日期更新对数据表【KQRSSZ_2】，返回DataSet记录数
        /// </summary>
        /// <param name="strJbhour"></param>
        /// <param name="strTxhour"></param>
        /// <param name="strRemark"></param>
        /// <param name="strIsToNormal"></param>
        /// <param name="strIsAllow"></param>
        /// <param name="strEmNo"></param>
        /// <param name="dtDay"></param>
        /// <returns></returns>
        public int ModifyKQResultByEmnoADay(String strJbhour, String strTxhour, String strRemark, String strIsToNormal, String strIsAllow, String strEmNo, DateTime dtDay)
        {
            KQPaiBanResultDao daoKQResult = new KQPaiBanResultDao();

            return daoKQResult.updateResultByEmnoADay(strJbhour, strTxhour, strRemark, strIsToNormal, strIsAllow, strEmNo, dtDay);
        }
        #endregion

        #region 根据主键批量更新表KQTOE_2的记录,返回记录数
        /// <summary>
        /// 根据主键批量更新表KQTOE_2的记录,返回记录数
        /// </summary>
        /// <param name="strModifyResults"></param>
        /// <returns></returns>
        public int ModifyBatchKQRSSZ2DataByKey(String strModifyResults)
        {
            KQPaiBanResultDao daoKQResult = new KQPaiBanResultDao();

            return daoKQResult.updateBatchKQRSSZ2DataByKey(strModifyResults);
        }
        #endregion

        #region 根据存储过程名称及其参数执行存储过程，返回影响记录数
        /// <summary>
        /// 根据存储过程名称及其参数执行存储过程，返回影响记录数
        /// </summary>
        /// <param name="strSpName"></param>
        /// <param name="hsTableParam"></param>
        /// <returns></returns>
        public int ExcuteSP(String strSpName,Hashtable hsTableParam)
        {
            KQPaiBanDAO daoKQPaiBan = new KQPaiBanDAO();
            return daoKQPaiBan.ExcuteSP(strSpName, hsTableParam);
        }
        #endregion


    }
}
