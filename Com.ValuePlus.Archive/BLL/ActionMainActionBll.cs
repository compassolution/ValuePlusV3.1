using System;
using System.Collections.Generic;
using System.Text;
using Com.ValuePlus.Archive.DAL;
using Com.ValuePlus.Archive.Entity;
using System.Collections;
using System.Data;

namespace Com.ValuePlus.Archive.BLL
{
    //模板中加载（角色/场景/明细）前后的相关动作类
    // add by sammen 20140619
    public class ActionMainActionBll
    {
        protected static Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static string strSpName_BeforeLoadRole = "USP_Archive_BeforeLoadRole";//加载角色之前(默认的，如果数据库中获取不到)
        private static string strSpName_BeforeLoadSence = "USP_Archive_BeforeLoadSence";//加载场景之前(默认的，如果数据库中获取不到)
        private static string strSpName_BeforeLoadDetail = "USP_Archive_BeforeLoadDetail";//加载明细页面之前(默认的，如果数据库中获取不到)
        private static string strSpName_AfterLoadRole = "USP_Archive_AfterLoadRole";//加载角色之后(默认的，如果数据库中获取不到)
        private static string strSpName_AfterLoadSence = "USP_Archive_AfterLoadSence";//加载场景之后(默认的，如果数据库中获取不到)
        private static string strSpName_AfterLoadDetail = "USP_Archive_AfterLoadDetail";//加载明细页面之后(默认的，如果数据库中获取不到)
        //add by sammen 20161014
        private static string strSpName_BeforeLoadGridDetail = "USP_Archive_BeforeLoadGridDetail";//加载列表型明细页面之前(默认的，如果数据库中获取不到)
        private static string strSpName_AfterLoadGridDetail = "USP_Archive_AfterLoadGridDetail";//加载列表型明细页面之后(默认的，如果数据库中获取不到)


        #region 加载角色之前执行
        /// <summary>
        /// 加载角色之前执行
        /// <param name="hsTableParam"></param>
        /// 返回值=1为正常
        /// </summary>
        public static int DoExcuteSP_BeforeLoadRole(Hashtable hsTableParam)
        {
            return DoExcuteArchiveMainAction(strSpName_BeforeLoadRole, hsTableParam);
        }
        #endregion

        #region 加载场景之前执行
        /// <summary>
        /// 加载场景之前执行
        /// <param name="hsTableParam"></param>
        /// 返回值=1为正常
        /// </summary>
        public static int DoExcuteSP_BeforeLoadSence(Hashtable hsTableParam)
        {
            return DoExcuteArchiveMainAction(strSpName_BeforeLoadSence, hsTableParam);
        }
        #endregion

        #region 加载明细页面之前执行
        /// <summary>
        /// 加载明细页面之前执行
        /// <param name="hsTableParam"></param>
        /// 返回值=1为正常
        /// </summary>
        public static int DoExcuteSP_BeforeLoadDetail(Hashtable hsTableParam)
        {
            return DoExcuteArchiveMainAction(strSpName_BeforeLoadDetail, hsTableParam);
        }
        #endregion

        #region 加载角色之后执行
        /// <summary>
        /// 加载角色之后执行
        /// <param name="hsTableParam"></param>
        /// 返回值=1为正常
        /// </summary>
        public static int DoExcuteSP_AfterLoadRole(Hashtable hsTableParam)
        {
            return DoExcuteArchiveMainAction(strSpName_AfterLoadRole, hsTableParam);
        }
        #endregion

        #region 加载场景之后执行
        /// <summary>
        /// 加载场景之后执行
        /// <param name="hsTableParam"></param>
        /// 返回值=1为正常
        /// </summary>
        public static int DoExcuteSP_AfterLoadSence(Hashtable hsTableParam)
        {
            return DoExcuteArchiveMainAction(strSpName_AfterLoadSence, hsTableParam);
        }
        #endregion

        #region 加载明细页面之后执行
        /// <summary>
        /// 加载明细页面之后执行
        /// <param name="hsTableParam"></param>
        /// 返回值=1为正常
        /// </summary>
        public static int DoExcuteSP_AfterLoadDetail(Hashtable hsTableParam)
        {
            return DoExcuteArchiveMainAction(strSpName_AfterLoadDetail, hsTableParam);
        }
        #endregion
        
        #region 加载列表型明细页面之前执行
        /// <summary>
        /// 加载列表型明细页面之前执行
        /// <param name="hsTableParam"></param>
        /// 返回值=1为正常
        /// </summary>
        public static int DoExcuteSP_BeforeLoadGridDetail(Hashtable hsTableParam)
        {
            return DoExcuteArchiveMainAction(strSpName_BeforeLoadGridDetail, hsTableParam);
        }
        #endregion

        #region 加载列表型明细页面之后执行
        /// <summary>
        /// 加载列表型明细页面之后执行
        /// <param name="hsTableParam"></param>
        /// 返回值=1为正常
        /// </summary>
        public static int DoExcuteSP_AfterLoadGridDetail(Hashtable hsTableParam)
        {
            return DoExcuteArchiveMainAction(strSpName_AfterLoadGridDetail, hsTableParam);
        }
        #endregion

        #region 存储过程执行
        /// <summary>
        /// <param name="strSpName"></param>
        /// <param name="hsTableParam"></param>
        /// </summary>
        private static int DoExcuteArchiveMainAction(String strSpName, Hashtable hsTableParam)
        {
            int iCount = 0;
            try
            {
                if (!String.IsNullOrEmpty(strSpName))
                {
                    iCount = SqlParamDao.ExcuteSP(strSpName, hsTableParam);
                }
                else
                {
                    iCount = -1;
                }
            }
            catch (Exception ex)
            {
                iCount = -1;
                log.Error(ex);
                log.Error("ActionMainActionBll---DoExcuteArchiveMainAction error: SPNAME:" + strSpName);
            }
            return iCount;
        }
        #endregion


    }
}
