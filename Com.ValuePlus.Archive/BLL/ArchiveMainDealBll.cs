using System;
using System.Collections;
using System.Text;
using System.IO;
using Com.ValuePlus.Utils.Cryptography;
using Com.ValuePlus.Archive.Config;
using System.Data;
using Com.ValuePlus.Archive.Utils;
using Com.ValuePlus.Archive.DAL;
using Com.ValuePlus.Archive.Entity;
using Com.ValuePlus.BLL.User;

namespace Com.ValuePlus.Archive.BLL
{
    public class ArchiveMainDealBll
    {
        /// <summary>
        /// 日志声明
        /// </summary>
        protected static Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 服务器分页获取模板列表记录
        /// </summary>
        /// <param name="strSqlString"></param>
        /// <param name="iPageSize"></param>
        /// <param name="iPageIndex"></param>
        /// <param name="strKEY"></param>
        /// <param name="strSort"></param>
        /// <param name="iRecordCount"></param>
        /// <returns></returns>
        public static DataSet GetArchiveListByServerPaging(String strSqlString,int iPageSize,int iPageIndex,String strKEY,String strSort,ref int iRecordCount)
        {
            String strCountSql = PagingSqlUtil.GetCountSql(strSqlString);
            //获取服务器端分页的语句
            if (String.IsNullOrEmpty(strSort))
            {
                strSqlString = PagingSqlUtil.GetPagingSql(strSqlString, iPageSize, iPageIndex, strKEY);
            }
            else
            {
                strSqlString = PagingSqlUtil.GetPagingSql(strSqlString, iPageSize, iPageIndex, strKEY, strSort);

            }
            //加载读取数据集，并返回记录数
            DataSet dsGridList = SqlParamDao.GetDataSetBySql(strSqlString);
            iRecordCount = SqlParamDao.ExecuteScalarBySql(strCountSql);
            return dsGridList;
        }

        /// <summary>
        /// 删除模板列表中的某条记录前的判断(对表TB_HRTMPSA中ALOCATION='6'的处理)
        /// </summary>
        /// <param name="strTID"></param>
        /// <param name="strRID"></param>
        /// <param name="strSID"></param>
        /// <param name="strKeyValue"></param>
        /// <param name="strUserId"></param>
        /// <param name="isAdmin"></param>
        /// <param name="isHis"></param>
        /// <returns></returns>
        public static int JudgeBeforeDelete(String strTID,String strRID, String strSID, String strKeyValue,String strUserId,bool isAdmin,String isHis)
        {
            ArchiveActionBll bllAction = new ArchiveActionBll();
            int iDeleteCount = bllAction.ExcuteActionDetailSPByLocation(strTID,strRID,strSID,6,strKeyValue,strUserId,isAdmin,isHis);
            return iDeleteCount;
        }

        /// <summary>
        /// 删除模板列表中的某条记录（同时删除该主键对应的所有分组表中记录）
        /// </summary>
        /// <param name="strTID"></param>
        /// <param name="strRID"></param>
        /// <param name="strSID"></param>
        /// <param name="strKey"></param>
        /// <param name="strKeyValue"></param>
        /// <returns></returns>
        public static int DeleteArchiveOneRecord(String strTID, String strRID, String strSID, String strKey, String strKeyValue, String strUserId, bool isAdmin, String isHis)
        {
            StringBuilder strBSql = new StringBuilder();
            int iDeleteCount = 0;
            string strSqlstring = "SELECT GID FROM TB_HRTMPG WHERE TID='" + strTID + "' AND GVIEW=0";
            DataTable dt = SqlParamDao.GetDataTableBySql(strSqlstring);
            if ((dt != null) && (dt.Rows.Count > 0))
            {
                try
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        String strGID = dr["GID"].ToString();
                        String strTableName = strTID + "_" + strGID;
                        String strSql = "delete from " + strTableName + " where " + strKey + " = '" + strKeyValue + "';";
                        strBSql.Append(strSql);
                    }
                    if ((strBSql != null) && (!String.IsNullOrEmpty(strBSql.ToString())))
                    {
                        Object obj = SqlParamDao.ExecuteNonQueryBySql(strBSql.ToString());
                        if (obj != null)
                        {
                            iDeleteCount = Convert.ToInt32(obj);
                            //删除后继续处理
                            DealAfterDelete(strTID, strRID, strSID,strKeyValue, strUserId, isAdmin, isHis);
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    iDeleteCount = -1;
                }
            }
            return iDeleteCount;
        }

        /// <summary>
        /// 删除模板列表中的某条记录后要做的处理(对表TB_HRTMPSA中ALOCATION='4'的处理)
        /// </summary>
        /// <param name="strTID"></param>
        /// <param name="strRID"></param>
        /// <param name="strSID"></param>
        /// <param name="strKeyValue"></param>
        /// <param name="strUserId"></param>
        /// <param name="isAdmin"></param>
        /// <param name="isHis"></param>
        /// <returns></returns>
        private static int DealAfterDelete(String strTID, String strRID, String strSID, String strKeyValue, String strUserId, bool isAdmin, String isHis)
        {
            ArchiveActionBll bllAction = new ArchiveActionBll();
            int iDeleteCount = bllAction.ExcuteActionDetailSPByLocation(strTID, strRID, strSID, 4, strKeyValue, strUserId, isAdmin, isHis);
            return iDeleteCount;
        }

        /// <summary>
        /// 判断当前页签是进行新增还是修改（存在一条记录则修改，不存在记录则新增）
        /// </summary>
        /// <param name="strGroupTableName"></param>
        /// <param name="strOpType"></param>
        /// <param name="strKeyField"></param>
        /// <param name="strKeyValue"></param>
        /// <returns></returns>
        public static bool JudgeIsCanInsert(String strGroupTableName, String strOpType, String strKeyField, String strKeyValue)
        {
            //判断当前页签是进行新增还是修改（存在一条记录则修改，不存在记录则新增）
            bool bIsInsert = false;
            if (strOpType.Equals("add"))
            {
                bIsInsert = true;
            }
            else
            {
                DataTable dtTemp = SqlParamDao.GetDataTableBySql("select * from " + strGroupTableName + " where " + strKeyField + "='" + strKeyValue + "'");
                if ((dtTemp == null) || (dtTemp.Rows.Count <= 0))
                {
                    bIsInsert = true;
                }
            }
            return bIsInsert;
        }

        /// <summary>
        /// 判断主表中是否存在某记录值
        /// </summary>
        /// <param name="strCurMainTableName"></param>
        /// <param name="strKey"></param>
        /// <param name="strKeyValue"></param>
        /// <returns></returns>
        public static bool IsHadKeyValue(String strCurMainTableName, String strKey, String strKeyValue)
        {
            bool isHave = false;

            string strSqlstring = "SELECT * FROM " + strCurMainTableName + " WHERE "+strKey+"='" + strKeyValue + "'";
            DataTable dt = SqlParamDao.GetDataTableBySql(strSqlstring);
            if ((dt != null) && (dt.Rows.Count > 0))
            {
                isHave = true;
            }
            return isHave;
        }

        /// <summary>
        /// 新增档案一条记录
        /// </summary>
        /// <param name="strBuilderSql"></param>
        /// <param name="strTID"></param>
        /// <param name="strRID"></param>
        /// <param name="strSID"></param>
        /// <param name="strKeyValue"></param>
        /// <param name="strUserId"></param>
        /// <param name="isAdmin"></param>
        /// <param name="isHis"></param>
        /// <returns></returns>
        public static int AddArchiveData(StringBuilder strBuilderSql, String strTID, String strRID, String strSID, String strKeyValue, String strUserId, bool isAdmin, String strIsHis)
        {
            ArchiveActionBll bllAction = new ArchiveActionBll();

            //增加前的action操作（ALOCATION=5）
            bllAction.ExcuteActionDetailSPByLocation(strTID, strRID, strSID, 5, strKeyValue, strUserId, isAdmin, strIsHis);
            int iExcuteDBCount = SqlParamDao.ExecuteNonQueryBySql(strBuilderSql.ToString());
            //增加后的action操作（ALOCATION=2）
            bllAction.ExcuteActionDetailSPByLocation(strTID, strRID, strSID, 2, strKeyValue, strUserId, isAdmin, strIsHis);

            return iExcuteDBCount;
        }

        /// <summary>
        /// 修改档案一条记录
        /// </summary>
        /// <param name="strBuilderSql"></param>
        /// <param name="strTID"></param>
        /// <param name="strRID"></param>
        /// <param name="strSID"></param>
        /// <param name="strKeyValue"></param>
        /// <param name="strUserId"></param>
        /// <param name="isAdmin"></param>
        /// <param name="strIsHis"></param>
        /// <returns></returns>
        public static int ModifyArchiveData(StringBuilder strBuilderSql,String strTID, String strRID, String strSID, String strKeyValue, String strUserId, bool isAdmin, String strIsHis)
        {
            ArchiveActionBll bllAction = new ArchiveActionBll();
            int iExcuteDBCount = SqlParamDao.ExecuteNonQueryBySql(strBuilderSql.ToString());
            //编辑时的action操作（ALOCATION=3）
            bllAction.ExcuteActionDetailSPByLocation(strTID, strRID, strSID, 3, strKeyValue, strUserId, isAdmin, strIsHis);

            return iExcuteDBCount;
        }


        /// <summary>
        /// 获取新增档案的keyvalue
        /// </summary>
        /// <param name="arrListObject"></param>
        /// <param name="strKey"></param>
        /// <returns></returns>
        public static String GetKeyValueAdded(ArrayList arrListObject,String strKey)
        {
            String strKeyValueAdded = "";
            if ((arrListObject != null) && (arrListObject.Count > 0))
            {
                for (int i = 0; i < arrListObject.Count; i++)
                {
                    Entity_ToDBObject entityDBObject = (Entity_ToDBObject)arrListObject[i];
                    if (entityDBObject != null)
                    {
                        if (strKey.Equals(entityDBObject.FIELDNAME))
                        {
                            strKeyValueAdded = entityDBObject.FIELDVALUE_NEW;
                            break;
                        }
                    }
                }
            }
            return strKeyValueAdded;
        }

        /// <summary>
        /// 将其他分组中的主键字段设置为新键值
        /// </summary>
        /// <param name="arrListObject"></param>
        /// <param name="strKey"></param>
        /// <param name="strNewKeyValue"></param>
        /// <returns></returns>
        public static ArrayList SetOtherGroupNewKeyValue(ArrayList arrListObject,String strKey, String strNewKeyValue)
        {
            if ((arrListObject != null) && (arrListObject.Count > 0))
            {
                for (int i = 0; i < arrListObject.Count; i++)
                {
                    Entity_ToDBObject entityDBObject = (Entity_ToDBObject)arrListObject[i];
                    if (entityDBObject != null)
                    {
                        if (strKey.Equals(entityDBObject.FIELDNAME))
                        {
                            entityDBObject.FIELDVALUE_NEW = strNewKeyValue;
                            arrListObject.RemoveAt(i);
                            arrListObject.Insert(i, entityDBObject);
                        }
                    }
                }
            }
            return arrListObject;
        }

        /// <summary>
        /// 获取相应场景下的SQL语句SSLCT
        /// </summary>
        /// <param name="strTID"></param>
        /// <param name="strRID"></param>
        /// <param name="strSID"></param>
        /// <param name="strUserCode"></param>
        /// <param name="strLanguage"></param>
        /// <param name="isReplaceTBLSTD">是否替换字典数据</param>
        /// <returns></returns>
        public static String GetArchiveSceneSSLCT(String strTID, String strRID, String strSID,String strUserCode,String strLanguage,bool isReplaceTBLSTD)
        {
            String strTableName = strTID + "_1";
            String strSqlString = "SELECT * FROM " + strTableName;
            //获取相应场景下的SQL语句SSLCT
            String strSqlSSLCT = "SELECT SSLCT,SDEL,SEDIT,SADD,SSIZE,SCFORM,SREF FROM TB_HRTMPS WHERE TID='" + strTID + "' AND SID='" + strSID + "'";
            DataTable dtSSLCT = SqlParamDao.GetDataTableBySql(strSqlSSLCT);
            if (dtSSLCT != null && dtSSLCT.Rows.Count > 0)
            {
                //通过TID获取当前角色下所设置的参数值
                GetArchiveSettingBll bllGetArchiveSetting = new GetArchiveSettingBll();
                Hashtable hsCurRoleParamValue = bllGetArchiveSetting.GetRoleParamValueByTidARid(strTID, strRID, strUserCode, Com.ValuePlus.BLL.User.UserLoginBll.IsAdminstratorUser());

                DataRow dr = dtSSLCT.Rows[0];
                if ((dr[0] != DBNull.Value) && (dr[0].ToString() != ""))//SSLCT过滤查询语句
                {
                    strSqlString = dr[0].ToString();
                    strSqlString = ParamOperationBll.ReplaceSceneSqlParam(strSqlString, hsCurRoleParamValue);
                }

                if (isReplaceTBLSTD)
                {
                    //将sql语句中涉及字典表的替换成字典表相应字段
                    ReplaceSqlIncludeTBLSTD lstdReplace = new ReplaceSqlIncludeTBLSTD();
                    strSqlString = lstdReplace.GetSqlIncludeLSTHDetail(strSqlString, strTID, strSID, "1", strLanguage);
                }
            }
            return strSqlString;
        }

    }
}
