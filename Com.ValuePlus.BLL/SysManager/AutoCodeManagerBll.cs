using System;
using System.Collections.Generic;
using System.Text;
using Com.ValuePlus.DAL;
using System.Data;

namespace Com.ValuePlus.BLL.SysManager
{
    public class AutoCodeManagerBll
    {

        #region 查询自动编号表所有记录,返回dataset
        /// <summary>
        /// 查询自动编号表所有记录
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllAutoInfo()
        {
            AutoCodeDao daoAuto = new AutoCodeDao();
            return daoAuto.findAll();
        }
        #endregion

        #region 根据主键查询自动编号表记录，返回datatable记录
        /// <summary>
        /// 根据主键查询自动编号表记录，返回datatable记录
        /// </summary>
        /// <param name="strAid"></param>
        /// <returns>DataSet</returns>
        public DataSet GetAutoInfoByAid(String strAid)
        {
            AutoCodeDao daoAuto = new AutoCodeDao();
            return daoAuto.findByAid(strAid);
        }
        #endregion

        #region 判断自动编号编码是否已经存在
        /// <summary>
        /// 判断自动编号编码是否已经存在
        /// </summary>
        /// <param name="strAid"></param>
        /// <returns></returns>
        public Boolean IsExsitAId(String strAid)
        {
            Boolean bIsExsit = true;
            AutoCodeDao daoAuto = new AutoCodeDao();
            DataSet ds = daoAuto.findByAid(strAid);
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

        #region 插入自动编号表一条记录
        /// <summary>
        /// 插入自动编号表一条记录
        /// </summary>
        /// <param name="strAID"></param>
        /// <param name="strADESC"></param>
        /// <param name="strADESCCHS"></param>
        /// <param name="strAPREFIX"></param>
        /// <param name="strADATE"></param>
        /// <param name="iALENGTH"></param>
        /// <param name="iANEXTNO"></param>
        /// <param name="strALASTDATE"></param>
        /// <returns></returns>
        public int AddAutoInfo(String strAID, String strADESC, String strADESCCHS, String strAPREFIX, String strADATE, int iALENGTH, int iANEXTNO, String strALASTDATE)
        {
            AutoCodeDao daoAuto = new AutoCodeDao();
            return daoAuto.insertOneRow(strAID,strADESC,strADESCCHS,strAPREFIX,strADATE,iALENGTH,iANEXTNO,strALASTDATE);
        }
        #endregion

        #region 根据主键更新自动编号表一条记录
        /// <summary>
        /// 根据主键更新自动编号表一条记录
        /// </summary>
        /// <param name="strAID"></param>
        /// <param name="strADESC"></param>
        /// <param name="strADESCCHS"></param>
        /// <param name="strAPREFIX"></param>
        /// <param name="strADATE"></param>
        /// <param name="iALENGTH"></param>
        /// <param name="iANEXTNO"></param>
        /// <param name="strALASTDATE"></param>
        /// <returns></returns>
        public int UpdateAutoInfoByAid(String strAID, String strADESC, String strADESCCHS, String strAPREFIX, String strADATE, int iALENGTH, int iANEXTNO, String strALASTDATE)
        {
            AutoCodeDao daoAuto = new AutoCodeDao();
            return daoAuto.updateByAid(strAID, strADESC, strADESCCHS, strAPREFIX, strADATE, iALENGTH, iANEXTNO, strALASTDATE);
        }
        #endregion

        #region 根据主键删除自动编号表记录，返回操作数
        /// <summary>
        /// 根据主键删除自动编号表记录，返回操作数
        /// </summary>
        /// <param name="strAid"></param>
        /// <returns>DataSet</returns>
        public int DeleteAutoInfoByAid(String strAid)
        {
            AutoCodeDao daoAuto = new AutoCodeDao();
            return daoAuto.deleteById(strAid);
        }
        #endregion


    }
}
