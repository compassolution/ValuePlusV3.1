using System;
using System.Text;
using System.Collections;
using Com.ValuePlus.Archive.Config;
using System.Data;
using Com.ValuePlus.Archive.DAL;
using Com.ValuePlus.Archive.Entity;

namespace Com.ValuePlus.Archive.BLL
{
    public class GetArchiveSettingBll
    {
        /// <summary>
        /// 通过当前用户编码取其对应的参数值
        /// </summary>
        /// <param name="strUserId"></param>
        /// <param name="isAdmin"></param>
        /// <returns>Hashtable</returns>
        public Hashtable GetUserParamValueByUserId(String strUserId,bool isAdmin)
        {
            String strViewName = Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("UserParamInfoViewName");
            String strUserCodeName = Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("UserParamInfoKeyName");
            Hashtable hsUserParamValue = new Hashtable();
            string strSql = "select * from " + strViewName + " where " + strUserCodeName + "='" + strUserId + "'";
            if (isAdmin)
            {
                strSql = "select * from " + strViewName + " order by " + strUserCodeName;
            }
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
            if ((dt != null) && (dt.Rows.Count > 0))
            {
                DataRow row = dt.Rows[0];
                hsUserParamValue = ParamOperationBll.SetUserParamValue(row);
            }
            return hsUserParamValue;
        }

        /// <summary>
        /// 通过TID&RID获取当前角色下所设置的参数值
        /// </summary>
        /// <param name="strTid"></param>
        /// <param name="strRid"></param>
        /// <param name="strUserId"></param>
        /// <param name="isAdmin"></param>
        /// <returns>Hashtable</returns>
        public Hashtable GetRoleParamValueByTidARid(String strTid, String strRid, String strUserId,bool isAdmin)
        {
            Hashtable hsRoleParamValue = new Hashtable();
            String strSql = "SELECT * FROM TB_HRTMPR WHERE TID='" + strTid + "' AND RID = '" + strRid + "'";

            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
            if ((dt != null) && (dt.Rows.Count > 0))
            {
                Hashtable hsUserParamValue = GetUserParamValueByUserId(strUserId,isAdmin);
                DataRow row = dt.Rows[0];
                hsRoleParamValue = ParamOperationBll.SetRoleParamValue(row, strUserId, hsUserParamValue);
            }

            return hsRoleParamValue;
        }

        /// <summary>
        /// 通过TID&RID获取当前角色下所设置的参数值
        /// </summary>
        /// <param name="strTid"></param>
        /// <param name="strRid"></param>
        /// <param name="strUserId"></param>
        /// <param name="isAdmin"></param>
        /// <param name="hsUserParamValue"></param>
        /// <returns>Hashtable</returns>
        public Hashtable GetRoleParamValueByTidARid(String strTid, String strRid, String strUserId, bool isAdmin, Hashtable hsUserParamValue)
        {
            Hashtable hsRoleParamValue = new Hashtable();
            String strSql = "SELECT * FROM TB_HRTMPR WHERE TID='" + strTid + "' AND RID = '" + strRid + "'";

            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
            if ((dt != null) && (dt.Rows.Count > 0))
            {
                DataRow row = dt.Rows[0];
                hsRoleParamValue = ParamOperationBll.SetRoleParamValue(row, strUserId, hsUserParamValue);
            }

            return hsRoleParamValue;
        }

        /// <summary>
        /// 通过TID获取其主信息表的主键字段信息
        /// </summary>
        /// <param name="strTid"></param>
        public DataTable GetKeyInfoByTID(String strTid)
        {
            String strKeyName = "";
            String strSql = "SELECT A.TID,A.GID,A.PID,A.PTYPE FROM TB_HRTMPD A INNER JOIN TB_HRTMPG B ON A.TID=B.TID AND A.GID = B.GID WHERE  A.TID='" + strTid + "' AND B.GTYPE = '0' AND A.PISKEY = '1'";
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
            return dt;
        }


        /// <summary>
        /// 通过TID及SID获取当前场景下的数据集SQL语句
        /// </summary>
        /// <param name="strTid"></param>
        /// <param name="strSid"></param>
        /// <param name="strCurMainTableName"></param>
        /// <param name="strCurKey"></param>
        /// <returns>String</returns>
        public String GetSenceSqlByTidASid(String strTid, String strSid, String strCurMainTableName, String strCurKey)
        {
            String strSenceSql = "";
            String strSql = "SELECT * FROM TB_HRTMPS WHERE TID ='" + strTid + "' AND SID = '" + strSid + "'";
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
            if ((dt != null) && (dt.Rows.Count > 0))
            {
                if (!String.IsNullOrEmpty(dt.Rows[0]["SSLCT"].ToString()))
                {
                    strSenceSql = dt.Rows[0]["SSLCT"].ToString();
                }
                else//如果
                {
                    strSenceSql = "select * from " + strCurMainTableName + " order by " + strCurKey;
                }
            }
            return strSenceSql;
        }

        /// <summary>
        /// 通过TID&SID获取当前状态下可见的模板分组信息
        /// </summary>
        /// <param name="strTid"></param>
        /// <param name="strSid"></param>
        /// <returns>ArrayList</returns>
        public ArrayList GetGroupListByTidARid(String strTid, String strSid)
        {
            ArrayList arrListGroupList = new ArrayList();
            String strSql = "SELECT * FROM TB_HRTMPSG WHERE TID='" + strTid + "' AND SID='" + strSid + "' AND GRIGHT<8 ORDER BY GORDER";

            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
            if ((dt != null) && (dt.Rows.Count > 0))
            {
                for (int i = 0; i < dt.Rows.Count;i++ )
                {
                    DataRow row = dt.Rows[i];
                    Entity_TB_HRTMPSG entity = SetDataToEntityBll.SetDataToEntity_TB_HRTMPSG(row);
                    arrListGroupList.Add(entity);
                }
            }

            return arrListGroupList;
        }

    }
}
