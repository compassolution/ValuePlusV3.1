using System;
using System.Collections;
using System.Data;
using System.Collections.Generic;
using System.Text;
using Com.ValuePlus.DAL.Notice;
using Com.ValuePlus.Entity.Notice;
namespace Com.ValuePlus.BLL.Notice
{
    public class NoticeBll
    {
        #region 查询公告表所有记录
        /// <summary>
        /// 查询公告表所有记录
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllNoticInfo()
        {
            NoticeDao daoNotice = new NoticeDao();
            return daoNotice.findAll();
        }
        #endregion

        #region 根据主键查询公告表记录，返回datatable记录
        /// <summary>
        /// 根据主键查询公告表记录，返回datatable记录
        /// </summary>
        /// <param name="strKey"></param>
        /// <returns>DataSet</returns>
        public DataTable GetNoticeInfoByKey(String strKey)
        {
            NoticeDao daoNotice = new NoticeDao();
            return daoNotice.findTableByKey(strKey);
        }
        #endregion

        #region 根据主键查询公告表一条记录，返回NoticeEntity实体
        /// <summary>
        /// 根据主键查询公告表一条记录，返回NoticeEntity实体
        /// </summary>
        /// <param name="strKey"></param>
        /// <returns>DataSet</returns>
        public NoticeEntity GetNoticeEntityByKey(String strKey)
        {
            NoticeDao daoNotice = new NoticeDao();
            NoticeEntity entityNotice = new NoticeEntity();
            DataTable dt = daoNotice.findTableByKey(strKey);
            if (dt.Rows.Count > 0)
            {
            	entityNotice.strSKEY = dt.Rows[0]["SKEY"].ToString();
                entityNotice.strSTITLE = dt.Rows[0]["STITLE"].ToString();
                entityNotice.strSCONTENT = dt.Rows[0]["SCONTENT"].ToString();
                entityNotice.strSPUBLISHOR = dt.Rows[0]["SPUBLISHOR"].ToString();
                DateTime date1 = new DateTime();
                if (dt.Rows[0]["DTPUBLISHTIME"] != null)
                {
                    date1 = DateTime.Parse(dt.Rows[0]["DTPUBLISHTIME"].ToString());
                }
                entityNotice.dtDTPUBLISHTIME = date1;
                entityNotice.strSSOURCE = dt.Rows[0]["SSOURCE"].ToString();
                entityNotice.strSEDITOR = dt.Rows[0]["SEDITOR"].ToString();
                DateTime date2 = new DateTime();
                if (dt.Rows[0]["DTEDITTIME"] != null)
                {
                    date2 = DateTime.Parse(dt.Rows[0]["DTEDITTIME"].ToString());
                }
                entityNotice.dtDTEDITTIME = date2;
                entityNotice.strBISSTOP = dt.Rows[0]["BISSTOP"].ToString();
            }
            return entityNotice;
        }
        #endregion

        #region 根据是否停用标记查询公告表相应记录，并返回前10条记录
        /// <summary>
        /// 根据是否停用标记查询公告表相应记录，并返回前10条记录
        /// </summary>
        /// <param name="bIsStop"></param>
        /// <returns></returns>
        public DataSet GetNoticInfoByIsStop(String bIsStop)
        {
            NoticeDao daoNotice = new NoticeDao();
            return daoNotice.findByIsStop(bIsStop);
        }
        #endregion

        #region 插入公告表一条记录
        /// <summary>
        /// 插入公告表一条记录
        /// </summary>
        /// <param name="NoticeEntity"></param>
        /// <returns></returns>
        public int AddNoticeInfo(NoticeEntity entityNotice)
        {
            NoticeDao daoNotice = new NoticeDao();
            return daoNotice.insertOneRow(entityNotice);
        }
        #endregion

        #region 根据公告表主键删除一条记录
        /// <summary>
        /// 根据公告表主键删除一条记录
        /// </summary>
        /// <param name="strKey"></param>
        /// <returns></returns>
        public int deleteNoticeInfo(String strKey)
        {
            int iCount = 0;
            NoticeDao daoNotice = new NoticeDao();
            iCount = daoNotice.deleteByKey(strKey);
            return iCount;
        }
        #endregion

        #region 根据公告表主键更新一条记录
        /// <summary>
        /// 根据公告表主键更新一条记录
        /// </summary>
        /// <param name="NoticeEntity"></param>
        /// <returns></returns>
        public int updateNoticeInfo(NoticeEntity entityNotice)
        {
            int iCount = 0;
            NoticeDao daoNotice = new NoticeDao();
            iCount = daoNotice.updateByKey(entityNotice);
            return iCount;
        }
        #endregion
    }
}
