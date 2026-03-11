using System;
using System.Collections.Generic;
using System.Text;
using Com.ValuePlus.DAL;
using System.Data;

namespace Com.ValuePlus.BLL.SysManager
{
    public class DicManagerBll
    {
        #region 查询菜单定义表所有记录
        /// <summary>
        /// 查询菜单定义表所有记录
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllDicListInfo()
        {
            DicListDao daoDic = new DicListDao();
            return daoDic.findAll();
        }
        #endregion

        #region 判断清单编码是否已经存在
        /// <summary>
        /// 判断清单编码是否已经存在
        /// </summary>
        /// <returns></returns>
        public Boolean IsExsitLID(String strLid)
        {
            Boolean bIsExsit = true;
            DicListDao daoDic = new DicListDao();
            DataSet ds = daoDic.findById(strLid);
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

        #region 插入菜单字典定义表一条记录
        /// <summary>
        /// 插入菜单字典定义表一条记录
        /// </summary>
        /// <returns></returns>
        public int AddDicListInfo(String strLid, String strDesc, String strDescChs, String strIsStop)
        {
            DicListDao daoDic = new DicListDao();
            return daoDic.insertOneRow(strLid, strDesc, strDescChs, strIsStop);
        }
        #endregion

        #region 根据清单定义表主键删除一条记录
        /// <summary>
        /// 根据清单定义表主键删除一条记录
        /// </summary>
        /// <returns></returns>
        public int deleteDicListDefine(String strLid)
        {
            int iCount = 0;
            DicListDao daoDic = new DicListDao();
            if (IsExsitLID(strLid))
            {
                iCount = daoDic.deleteById(strLid);
            }
            return iCount;
        }
        #endregion

        #region 根据清单定义表主键更新一条记录
        /// <summary>
        /// 根据清单定义表主键更新一条记录
        /// </summary>
        /// <returns></returns>
        public int updateDicListDefine(String strLid, String strDesc, String strDescchs, String strIsstop)
        {
            int iCount = 0;
            DicListDao daoDic = new DicListDao();
            if (IsExsitLID(strLid))
            {
                iCount = daoDic.updateById(strLid,strDesc,strDescchs,strIsstop);
            }
            return iCount;
        }
        #endregion


        #region 根据菜单定义ID查询菜单内容明细表所有记录
        /// <summary>
        /// 根据菜单定义ID查询菜单内容明细表所有记录
        /// </summary>
        /// <returns></returns>
        public DataSet GetDicDetailInfoByLId(String strLid)
        {
            DicDetailDao daoDicDetail = new DicDetailDao();
            return daoDicDetail.findByLid(strLid);
        }
        #endregion

        #region 判断清单明细表中是否已经存在同一明细编码
        /// <summary>
        /// 判断清单明细表中是否已经存在同一明细编码
        /// </summary>
        /// <returns></returns>
        public Boolean IsExsitLIDAndCID(String strLid, String strCid)
        {
            Boolean bIsExsit = true;
            DicDetailDao daoDic = new DicDetailDao();
            DataSet ds = daoDic.findByLidACid(strLid, strCid);
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

        #region 插入菜单明细内容表一条记录
        /// <summary>
        /// 插入菜单明细内容表一条记录
        /// </summary>
        /// <returns></returns>
        public int AddDicDetailInfo(String strLid, String strCid, String strDesc, String strDescChs, String strCUID,String strP9, String strIsStop)
        {
            int iCount = 0;
            DicDetailDao daoDic = new DicDetailDao();
            iCount = daoDic.insertOneRow(strLid, strCid, strDesc, strDescChs, strCUID, null, null, null, null, null, null, null, null, null, strP9, strIsStop);
            
            return iCount;
        }
        #endregion

        #region 根据清单定义表ID及明细表ID删除明细内容表一条记录
        /// <summary>
        /// 根据清单定义表ID及明细表ID删除明细内容表一条记录
        /// </summary>
        /// <returns></returns>
        public int deleteDicDetailInfo(String strLid, String strCid)
        {
            int iCount = 0;
            DicDetailDao daoDic = new DicDetailDao();
            if (IsExsitLIDAndCID(strLid, strCid))
            {
                iCount = daoDic.deleteByLidACid(strLid, strCid);
            }
            return iCount;
        }
        #endregion

        #region 根据菜单定义表ID和明细表ID更新菜单明细内容表一条记录
        /// <summary>
        /// 根据菜单定义表ID和明细表ID更新菜单明细内容表一条记录
        /// </summary>
        /// <returns></returns>
        public int updateDicDetailInfo(String strLid, String strCid, String strDesc, String strDescChs, String strCUID, String strP0, String strP1,  String strP2,  String strP3,  String strP4,  String strP5,  String strP6,  String strP7,  String strP8,  String strP9, String strIsStop)
        {
            int iCount = 0;
            DicDetailDao daoDic = new DicDetailDao();
            if (IsExsitLIDAndCID(strLid, strCid))
            {
                iCount = daoDic.updateByLidACid(strLid, strCid, strDesc, strDescChs, strCUID, strP0, strP1, strP2, strP3, strP4, strP5, strP6, strP7, strP8, strP9, strIsStop);
            }
            return iCount;
        }
        #endregion
    }
}
