using System;
using System.Collections.Generic;
using System.Text;
using Com.ValuePlus.DAL.Report;
using System.Data;
using Com.ValuePlus.Entity.Report;

namespace Com.ValuePlus.BLL.Report
{
    public class ReportMainBll
    {
        #region 根据参数字符串和用户标识进行相应处理，返回字符串
        /// <summary>
        /// 根据参数字符串和用户标识进行相应处理，返回字符串
        /// </summary>
        /// <param name="strParam"></param>
        /// <param name="strUserCode"></param>
        /// <returns></returns>
        public String GetUserParamReplaced(String strParam, String strUserCode)
        {
            ReportMainDao daoReportMain = new ReportMainDao();
            DataSet ds = daoReportMain.selectViewHrUp1All(strUserCode);
            if (ds.Tables.Count != 0)
            {
                if (ds.Tables[0].Rows.Count == 0)
                {
                    return strParam;
                }
                for (int i = 0; i < 10; i++)
                {
                    if (strParam == ("%P" + i.ToString() + "%"))
                    {
                        return ds.Tables[0].Rows[0]["P" + i.ToString()].ToString();
                    }
                }
            }
            return strParam;
        }
        #endregion

        #region 根据主键查询数据表【TB_HRTMPSD】的一条记录,返回dataset记录
        /// <summary>
        /// 根据主键查询数据表【TB_HRTMPSD】的一条记录,返回dataset记录
        /// </summary>
        /// <param name="strTid"></param>
        /// <param name="strGid"></param>
        /// <param name="strSid"></param>
        /// <param name="strPid"></param>
        /// <returns></returns>
        public DataSet GetAll_TB_HRTMPSD_InfoByKey(String strTid, String strGid, String strSid, String strPid)
        {
            ReportMainDao daoReportMain = new ReportMainDao();
            DataSet ds = daoReportMain.selectTB_HRTMPSD_ByKey(strTid, strGid, strSid, strPid);
            return ds;
        }
        #endregion

        #region 根据实体ReportXmlEntity从TB_HRTMPSD中获取对应信息并返回实体类ReportXmlEntity
        /// <summary>
        /// 根据实体ReportXmlEntity从TB_HRTMPSD中获取对应信息并返回实体类ReportXmlEntity
        /// </summary>
        /// <param name="strLanguage"></param>
        /// <param name="entityXml"></param>
        /// <returns></returns>
        public ReportXmlEntity GetEntityInfoByParam(String strLanguage, ReportXmlEntity entityXml)
        {
            if (entityXml != null)
            {
                DataSet ds = this.GetAll_TB_HRTMPSD_InfoByKey(entityXml.strTid, entityXml.strGid, entityXml.strSid, entityXml.strPid);
                if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
                {
                    entityXml.strPID = ds.Tables[0].Rows[0]["PID"].ToString();
                    entityXml.strPCTRLTYPE = ds.Tables[0].Rows[0]["PCTRL"].ToString();
                    entityXml.strPDESCCHS = ds.Tables[0].Rows[0]["PDESCCHS"].ToString();
                    entityXml.strPDESC = ds.Tables[0].Rows[0]["PDESC"].ToString();
                    entityXml.strPCTRLID = ds.Tables[0].Rows[0]["PCTRLID"].ToString();
                    entityXml.strPCTRLSQL = ds.Tables[0].Rows[0]["PCTRLD"].ToString();
                    entityXml.strPDATATYPE = ds.Tables[0].Rows[0]["PTYPE"].ToString();
                    entityXml.strPDEFAULT = ds.Tables[0].Rows[0]["PDEFAULT"].ToString();
                }
            }
            return entityXml;
        }
        #endregion
    }

}
