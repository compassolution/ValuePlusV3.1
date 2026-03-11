using System;
using System.Collections;
using System.Text;
using System.IO;
using Com.ValuePlus.Utils.Cryptography;
using Com.ValuePlus.Archive.Config;
using Com.ValuePlus.Archive.Entity;
using System.Data;
using Com.ValuePlus.Archive.DAL;

namespace Com.ValuePlus.Archive.BLL
{
    public class TMPSQSGetterBll
    {
        /// <summary>
        /// 日志声明
        /// </summary>
        protected static Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 获取模板场景组合查询条件配置信息
        /// </summary>
        /// <param name="strTID"></param>
        /// <returns></returns>
        public static DataTable GetTemplateStylesEntity(String strTID,String strSID)
        {
            DataTable dt = new DataTable();
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select A.COLQTY,B.SEQNO,B.SHOWDESC,B.SHOWDESCCHS,B.QueryRule");
            sbSql.Append(" ,C.TID,C.GID,C.SID,C.PID,C.PDESC,C.PDESCCHS,C.PORDER ");
            sbSql.Append(" ,ISNULL(B.PTYPE,C.PTYPE) AS PTYPE ");
            sbSql.Append(" ,ISNULL(B.PCTRL,C.PCTRL) AS PCTRL ");
            sbSql.Append(" ,ISNULL(B.PCTRLID,C.PCTRLID) AS PCTRLID ");
            sbSql.Append(" ,ISNULL(B.PCTRLD,C.PCTRLD) AS PCTRLD ");
            sbSql.Append(" ,ISNULL(B.PMAST,C.PMAST) AS PMAST ");
            sbSql.Append(" from TMPSQS_1 A INNER JOIN TMPSQS_2 B ON A.TSID = B.TSID ");
            sbSql.Append(" INNER JOIN TB_HRTMPSD C ON A.TID = C.TID AND A.SID = C.SID AND B.GID = C.GID AND B.PID = C.PID  ");
            sbSql.Append(" WHERE A.BISVALID = '1' AND A.TID = '"+ strTID + "' AND A.SID = '"+ strSID + "' ");
            sbSql.Append(" ORDER BY convert(int,B.PORDER) ");

            try
            {
                dt = SqlParamDao.GetDataTableBySql(sbSql.ToString());
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.Error("获取模板场景组合查询条件配置信息出错: Sql:" + sbSql.ToString());
            }
            return dt;
        }

    }
}
