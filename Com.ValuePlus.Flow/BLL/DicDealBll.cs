using System;
using System.Text;
using Com.ValuePlus.Flow.DAL;
using System.Data;

namespace Com.ValuePlus.Flow.BLL
{
    public class DicDealBll
    {
        #region 查询表某字典表所有记录，返回DataTable记录集
        /// <summary>
        /// 查询表某字典表所有记录，返回DataTable记录集
        /// </summary>
        /// <param name="strTableName">字典表名</param>
        /// <param name="strIsStop">是否停用标志，NULL标示所有</param>
        /// <returns></returns>
        public static DataTable GetAllDicList(String strTableName,String strIsStop)
        {
            String strSql = "select * from " + strTableName + " where 1=1";
            if (!String.IsNullOrEmpty(strIsStop))
            {
                strSql = strSql + " and BSTOP = " + strIsStop;
            }
            strSql = strSql + " ORDER BY NINDEX";
            return SqlParamDao.GetDataTableBySql(strSql);
        }
        #endregion

        #region 根据字段类型编码获取字段类型名称
        /// <summary>
        /// 根据字段类型编码获取字段类型名称
        /// </summary>
        /// <param name="strFieldTypeCode"></param>
        /// <param name="strLanguage"></param>
        /// <returns></returns>
        public static String GetFieldTypeNameByCode(String strFieldTypeCode, String strLanguage)
        {
            String strReturn = "";
            String strSql = "select * from TB_DIC_FORM_FIELD_TYPE where SFIELDTYPECODE = " + strFieldTypeCode;
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
            if (dt != null && dt.Rows.Count > 0)
            {
                if ((!String.IsNullOrEmpty(strLanguage)) && (strLanguage.Equals("zh-cn")))
                {
                    strReturn = dt.Rows[0]["SFIELDTYPENAMECN"].ToString();
                }
                else
                {
                    strReturn = dt.Rows[0]["SFIELDTYPENAME"].ToString();
                }
            }
            return strReturn;
        }
        #endregion

    }
}
