using System;
using System.Collections.Generic;
using System.Text;
using Com.ValuePlus.Archive.DAL;
using Com.ValuePlus.Archive.Entity;
using System.Collections;
using System.Data;


namespace Com.ValuePlus.Archive.Tree
{
    //树形模板中的相关动作类
    public class TreeActionBll
    {
        /// <summary>
        /// 日志声明
        /// 传递存储过程的参数如下：
        /// @TID varchar(50), --树形模板ID
        /// @Key varchar(50), --树形模板主表主键字段ID
        /// @KeyValue varchar(50), --树形模板主表主键值
        /// @UserId varchar(50) --当前用户ID
        /// </summary>
        /// 
        protected static Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static string strLID_TreeActions = "TREEACTIONTYPE";//动作类型列表
        //private static string strSpName_BeforeAdd = "USP_SYS_Tree_BeforeAdd";//新增前(默认的，如果数据库中获取不到)
        //private static string strSpName_AfterAdd = "USP_SYS_Tree_AfterAdd";//新增后(默认的，如果数据库中获取不到)
        //private static string strSpName_BeforeDelete = "USP_SYS_Tree_BeforeDelete";//删除前(默认的，如果数据库中获取不到)
        //private static string strSpName_AfterDelete = "USP_SYS_Tree_AfterDelete";//删除后(默认的，如果数据库中获取不到)
        //private static string strSpName_AfterEdit = "USP_SYS_Tree_AfterEdit";//编辑保存后(默认的，如果数据库中获取不到)

        /// <summary>
        /// 新增前动作ID
        /// </summary>
        public const string ActionID_BeforeAdd = "USP_SYS_Tree_BeforeAdd";//新增前(默认的，如果数据库中获取不到)
        /// <summary>
        /// 新增后动作ID
        /// </summary>
        public const string ActionID_AfterAdd = "USP_SYS_Tree_AfterAdd";//新增后(默认的，如果数据库中获取不到)
        /// <summary>
        /// 删除前动作ID
        /// </summary>
        public const string ActionID_BeforeDelete = "USP_SYS_Tree_BeforeDelete";//删除前(默认的，如果数据库中获取不到)
        /// <summary>
        /// 删除后动作ID
        /// </summary>
        public const string ActionID_AfterDelete = "USP_SYS_Tree_AfterDelete";//删除后(默认的，如果数据库中获取不到)
        /// <summary>
        /// 编辑后动作ID
        /// </summary>
        public const string ActionID_AfterEdit = "USP_SYS_Tree_AfterEdit";//编辑保存后(默认的，如果数据库中获取不到)

        /// <summary>
        /// 获取模板表格类型的动作列表
        /// </summary>
        public static void GetTreeActions()
        {
            //String strSql = "select * from TB_HRLSTD WHERE LID = '" + strLID_TreeActions + "'";
            //DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
            //if ((dt != null) && (dt.Rows.Count > 0))
            //{
            //    for (int i = 0; i < dt.Rows.Count; i++)
            //    {
            //        DataRow dr = dt.Rows[i];
            //        switch (dr["CID"].ToString())
            //        {
            //            case ActionID_BeforeAdd:
            //                strSpName_BeforeAdd = dr["P0"] == null ? strSpName_BeforeAdd : dr["P0"].ToString();
            //                break;
            //            case ActionID_AfterAdd:
            //                strSpName_AfterAdd = dr["P0"] == null ? strSpName_BeforeAdd : dr["P0"].ToString();
            //                break;
            //            case ActionID_BeforeDelete:
            //                strSpName_BeforeDelete = dr["P0"] == null ? strSpName_BeforeAdd : dr["P0"].ToString();
            //                break;
            //            case ActionID_AfterDelete:
            //                strSpName_AfterDelete = dr["P0"] == null ? strSpName_BeforeAdd : dr["P0"].ToString();
            //                break;
            //            case ActionID_AfterEdit:
            //                strSpName_AfterEdit = dr["P0"] == null ? strSpName_BeforeAdd : dr["P0"].ToString();
            //                break;
            //        }
            //    }
            //}
        }

        /// <summary>
        /// 获取执行动作后的提示
        /// </summary>
        /// <param name="strTid"></param>
        /// <param name="strActionType"></param>
        /// <param name="iReturnValue"></param>
        /// <param name="strLanguage"></param>
        /// <returns></returns>
        public static String GetTreeActionTips(String strTid, String strActionType, int iReturnValue, String strLanguage)
        {
            String strMsg = "";
            try
            {
                String strSql = "select * from TREECONFIG_2 WHERE TID = '" + strTid + "' AND ACTIONDESC = '" + strActionType + "' AND IVALUE =" + iReturnValue;
                DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
                if ((dt != null) && (dt.Rows.Count > 0))
                {
                    if (strLanguage.Equals("zh-cn"))
                    {
                        strMsg = dt.Rows[0]["TIPSCHS"].ToString();
                    }
                    else
                    {
                        strMsg = dt.Rows[0]["TIPS"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.Error("TreeActionBll---GetTreeActionTips error: TID = '" + strTid + "' AND ACTIONTYPE = '" + strActionType + "' AND IVALUE =" + iReturnValue);
            }

            return strMsg;
        }

        #region 增加前动作执行
        /// <summary>
        /// 增加前动作执行
        /// <param name="hsTableParam"></param>
        /// 返回值=1为正常
        /// </summary>
        public static int DoExcuteSP_BeforeAdd(Hashtable hsTableParam)
        {
            return DoExcuteTreeAction(ActionID_BeforeAdd, hsTableParam);
        }
        #endregion

        #region 增加后动作执行
        /// <summary>
        /// 增加后动作执行
        /// <param name="hsTableParam"></param>
        /// 返回值=1为正常
        /// </summary>
        public static int DoExcuteSP_AfterAdd(Hashtable hsTableParam)
        {
            return DoExcuteTreeAction(ActionID_AfterAdd, hsTableParam);
        }
        #endregion

        #region 删除前动作执行
        /// <summary>
        /// 删除前动作执行
        /// <param name="hsTableParam"></param>
        /// 返回值=1为正常
        /// </summary>
        public static int DoExcuteSP_BeforeDelete(Hashtable hsTableParam)
        {
            return DoExcuteTreeAction(ActionID_BeforeDelete, hsTableParam);
        }
        #endregion

        #region 删除后动作执行
        /// <summary>
        /// 删除后动作执行
        /// <param name="hsTableParam"></param>
        /// 返回值=1为正常
        /// </summary>
        public static int DoExcuteSP_AfterDelete(Hashtable hsTableParam)
        {
            return DoExcuteTreeAction(ActionID_AfterDelete, hsTableParam);
        }
        #endregion

        #region 编辑保存后动作执行
        /// <summary>
        /// 编辑保存后动作执行
        /// <param name="hsTableParam"></param>
        /// 返回值=1为正常
        /// </summary>
        public static int DoExcuteSP_AfterEdit(Hashtable hsTableParam)
        {
            return DoExcuteTreeAction(ActionID_AfterEdit, hsTableParam);
        }
        #endregion

        #region 存储过程执行
        /// <summary>
        /// <param name="strSpName"></param>
        /// <param name="hsTableParam"></param>
        /// </summary>
        private static int DoExcuteTreeAction(String strSpName, Hashtable hsTableParam)
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
                log.Error("TreeActionBll---DoExcuteTreeAction error: SPNAME:" + strSpName);
            }
            return iCount;
        }
        #endregion
    }
}
