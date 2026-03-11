using System;
using System.Collections.Generic;
using System.Text;
using Com.ValuePlus.DAL.WinForm;
using System.Data;
using System.Collections;

namespace Com.ValuePlus.BLL.WinForm
{
    public class AdjustStaffPayBll
    {
        #region 查询薪资库员工列表一条记录获取列名，返回列名DataSet记录集
        /// <summary>
        /// 查询薪资库员工列表一条记录获取列名，返回列名DataSet记录集
        /// </summary>
        /// <returns></returns>
        public DataSet GetStaffListCol()
        {
            AdjustStaffPayDAO daoAdjustStaffPay = new AdjustStaffPayDAO();
            return daoAdjustStaffPay.findAllStaffList_Col();
        }
        #endregion

        #region 查询薪资库员工列表记录数，返回记录数
        /// <summary>
        /// 查询薪资库员工列表记录数，返回记录数
        /// </summary>
        /// <returns>int</returns>
        public int GetStaffListCount()
        {
            AdjustStaffPayDAO daoAdjustStaffPay = new AdjustStaffPayDAO();
            return daoAdjustStaffPay.findAllStaffListCount();
        }
        #endregion

        #region 查询薪资库员工列表所有记录，返回DataSet记录集
        /// <summary>
        /// 查询薪资库员工列表所有记录，返回DataSet记录集
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllStaffList()
        {
            AdjustStaffPayDAO daoAdjustStaffPay = new AdjustStaffPayDAO();
            return daoAdjustStaffPay.findAllStaffList();
        }
        #endregion

        #region 分页查询薪资库员工列表所有记录，返回DataSet记录集
        /// <summary>
        /// 分页查询薪资库员工列表所有记录，返回DataSet记录集
        /// </summary>
        /// <param name="iPageSize"></param>
        /// <param name="iStartIndex"></param>
        /// <returns></returns>
        public DataSet GetAllStaffListMultiPage(int iPageSize, int iStartIndex)
        {
            AdjustStaffPayDAO daoAdjustStaffPay = new AdjustStaffPayDAO();
            return daoAdjustStaffPay.findAllStaffListMultiPage(iPageSize, iStartIndex);
        }
        #endregion

        #region 分页根据条件查询薪资库员工列表相关记录，返回DataSet记录集
        /// <summary>
        /// 分页根据条件查询薪资库员工列表相关记录，返回DataSet记录集
        /// </summary>
        /// <param name="iPageSize"></param>
        /// <param name="iStartIndex"></param>
        /// <param name="strFilterSql"></param>
        /// <returns></returns>
        public DataSet GetAllStaffListMultiPageByCondition(int iPageSize, int iStartIndex, String strFilterSql)
        {
            AdjustStaffPayDAO daoAdjustStaffPay = new AdjustStaffPayDAO();
            return daoAdjustStaffPay.findAllStaffListMultiPageByCondition(iPageSize, iStartIndex,strFilterSql);
        }
        #endregion

        #region 根据条件查询薪资库员工列表相关记录，返回DataSet记录集
        /// <summary>
        /// 根据条件查询薪资库员工列表相关记录，返回DataSet记录集
        /// <param name="strFilterSql"></param>
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllStaffListByCondition(String strFilterSql)
        {
            AdjustStaffPayDAO daoAdjustStaffPay = new AdjustStaffPayDAO();
            return daoAdjustStaffPay.findStaffListByCondition(strFilterSql);
        }
        #endregion

        #region 查询员工需调整的薪资列表的一条记录，返回列名DataSet记录集
        /// <summary>
        /// 查询员工需调整的薪资列表的一条记录，返回列名DataSet记录集
        /// </summary>
        /// <returns></returns>
        public DataSet GetPayItemListCol()
        {
            AdjustStaffPayDAO daoAdjustStaffPay = new AdjustStaffPayDAO();
            return daoAdjustStaffPay.findPayItemList_Col();
        }
        #endregion

        #region 根据员工编号查询其对应的需调整的薪资列表，返回DataSet记录集
        /// <summary>
        /// 根据员工编号查询其对应的需调整的薪资列表，返回DataSet记录集
        /// <param name="strStaffNo"></param>
        /// </summary>
        /// <returns></returns>
        public DataSet GetPayItemListByStaffNo(String strStaffNo)
        {
            AdjustStaffPayDAO daoAdjustStaffPay = new AdjustStaffPayDAO();
            return daoAdjustStaffPay.findPayItemListByStaffNo(strStaffNo);
        }
        #endregion

        #region 批量更新表员工薪资变更调整信息（表prempl_2）,返回记录数
        /// <summary>
        /// 批量更新表员工薪资变更调整信息（表prempl_2）,返回记录数
        /// </summary>
        /// <param name="strModifyResults"></param>
        /// <returns>int</returns>
        public int SaveStaffPayValueBatch(String strModifyResults)
        {

            AdjustStaffPayDAO daoAdjustStaffPay = new AdjustStaffPayDAO();
            return daoAdjustStaffPay.updateStaffPayValueBatch(strModifyResults);
        }
        #endregion

    }
}
