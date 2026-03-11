using Com.ValuePlus.Archive.DAL;
using Com.ValuePlus.Log;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Com.ValuePlus.Archive.Admin
{
    public class ExportArchiveConfig
    {
        /// <summary>
        /// 日志声明
        /// </summary>
        public static ILog log = LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 根据模板ID清除其所有分组的所有数据
        /// </summary>
        /// <param name="strTid"></param>
        public String GetClearArchiveConfigSql(String strTid)
        {
            StringBuilder sbDelteSql = new StringBuilder();
            sbDelteSql.Append("----首先删除相关配置\r\n");
            sbDelteSql.Append("DELETE FROM TB_HRTMPSE where TID = '" + strTid + "'\r\n");
            sbDelteSql.Append("DELETE FROM TB_HRTMPE where TID = '" + strTid + "'\r\n");
            sbDelteSql.Append("DELETE FROM TB_HRTMPSA where TID = '" + strTid + "'\r\n");
            sbDelteSql.Append("DELETE FROM TB_HRTMPAR where TID = '" + strTid + "'\r\n");
            sbDelteSql.Append("DELETE FROM TB_HRTMPA where TID = '" + strTid + "'\r\n");
            sbDelteSql.Append("DELETE FROM TB_HRTMPSD where TID = '" + strTid + "'\r\n");
            sbDelteSql.Append("DELETE FROM TB_HRTMPSG where TID = '" + strTid + "'\r\n");

            sbDelteSql.Append("DELETE FROM TB_HRTMPRD where TID = '" + strTid + "'\r\n");
            sbDelteSql.Append("DELETE FROM TB_HR_USERROLE where TID = '" + strTid + "'\r\n");
            sbDelteSql.Append("DELETE FROM TB_HRTMPR where TID = '" + strTid + "'\r\n");

            sbDelteSql.Append("DELETE FROM TB_HRTMPD where TID = '" + strTid + "'\r\n");
            sbDelteSql.Append("DELETE FROM TB_HRTMPG where TID = '" + strTid + "'\r\n");
            sbDelteSql.Append("DELETE FROM TB_HRTMPS where TID = '" + strTid + "'\r\n");
            sbDelteSql.Append("DELETE FROM TB_HRTMPH where TID = '" + strTid + "'\r\n");

            return sbDelteSql.ToString();

        }

        /// <summary>
        /// 根据模板编码导其出所有配置数据
        /// </summary>
        /// <param name="strTid"></param>
        /// <param name="bIsExportBizData"></param>
        /// <returns></returns>
        public String GetArchiveConfigExportSql(string strTid, bool bIsExportBizData)
        {
            String strSql = "SELECT * FROM TB_HRTMPH where TID = '" + strTid + "'";
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);

            StringBuilder sbExportSql = new StringBuilder();
            if ((dt != null) && (dt.Rows.Count > 0))
            {
                DataRow row = dt.Rows[0];
                sbExportSql.Append("--模板定义表的配置\r\n");
                sbExportSql.Append("INSERT INTO TB_HRTMPH( TID,TDESC,TDESCCHS,TCRTDATE,TREC,BTNSTANTION,ROLEFILE,BISSTOP)");
                sbExportSql.Append(" VALUES ('" + RepalceSingleQuote(row, "TID") + "','" + RepalceSingleQuote(row, "TDESC") + "','" + RepalceSingleQuote(row, "TDESCCHS") + "','" + RepalceSingleQuote(row, "TCRTDATE") + "','" + RepalceSingleQuote(row, "TREC") + "','" + RepalceSingleQuote(row, "BTNSTANTION") + "','" + RepalceSingleQuote(row, "ROLEFILE") + "','" + RepalceSingleQuote(row, "BISSTOP") + "')\r\n");

                //获取模板分组配置的导出脚本
                sbExportSql.Append(this.ExportArchiveGroups(strTid));
                //获取模板动作配置的导出脚本
                sbExportSql.Append(this.ExportArchiveActions(strTid));
                //获取模板事件配置的导出脚本
                sbExportSql.Append(this.ExportArchiveEvents(strTid));
                //获取模板状态配置的导出脚本
                sbExportSql.Append(this.ExportArchiveSences(strTid));
                //获取模板角色配置的导出脚本
                sbExportSql.Append(this.ExportArchiveRoles(strTid));

                //如果需要导出业务数据表数据
                if (bIsExportBizData)
                {
                    sbExportSql.Append(this.ExportTablesData(strTid));
                }

            }
            return sbExportSql.ToString();
        }

        /// <summary>
        /// 获取模板分组配置的导出脚本
        /// </summary>
        /// <param name="strTid"></param>
        /// <returns></returns>
        private String ExportArchiveGroups(string strTid)
        {
            String strSql = "SELECT * FROM TB_HRTMPG where TID = '" + strTid + "'";
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);

            StringBuilder sbExportSql = new StringBuilder();
            if ((dt != null) && (dt.Rows.Count > 0))
            {
                sbExportSql.Append("--模板分组表的配置\r\n");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow row = dt.Rows[i];
                    String strGid = row["GID"].ToString();
                    sbExportSql.Append("----模板分组" + strGid + "的配置\r\n");
                    sbExportSql.Append("INSERT INTO TB_HRTMPG( TID,GID,GDESC,GDESCCHS,GTYPE,GORDER,GLIMIT,GCOUNT,GWIDTH,GHIST,GPAGE,GRCOUNT,GVIEW,GSQL,USERPAGE,islarge)");
                    sbExportSql.Append(" VALUES ('" + RepalceSingleQuote(row, "TID") + "','" + RepalceSingleQuote(row, "GID") + "','" + RepalceSingleQuote(row, "GDESC") + "','" + RepalceSingleQuote(row, "GDESCCHS") + "','" + RepalceSingleQuote(row, "GTYPE") + "','" + RepalceSingleQuote(row, "GORDER") + "','" + RepalceSingleQuote(row, "GLIMIT") + "','" + RepalceSingleQuote(row, "GCOUNT") + "','" + RepalceSingleQuote(row, "GWIDTH") + "','" + RepalceSingleQuote(row, "GHIST") + "','" + RepalceSingleQuote(row, "GPAGE") + "','" + RepalceSingleQuote(row, "GRCOUNT") + "','" + RepalceSingleQuote(row, "GVIEW") + "','" + RepalceSingleQuote(row, "GSQL") + "','" + RepalceSingleQuote(row, "USERPAGE") + "','" + RepalceSingleQuote(row, "islarge") + "')\r\n");

                    //获取各分组下的字段属性的配置
                    sbExportSql.Append(this.ExportArchivePropertys(strTid, strGid));
                }
            }
            return sbExportSql.ToString();
        }

        /// <summary>
        /// 获取各分组下的字段属性的配置
        /// </summary>
        /// <param name="strTid"></param>
        /// <param name="strGid"></param>
        /// <returns></returns>
        private String ExportArchivePropertys(string strTid, String strGid)
        {
            String strSql = "SELECT * FROM TB_HRTMPD where TID = '" + strTid + "' AND GID ='" + strGid + "'";
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);

            StringBuilder sbExportSql = new StringBuilder();
            if ((dt != null) && (dt.Rows.Count > 0))
            {
                sbExportSql.Append("----模板分组" + strGid + "下各表字段属性的配置\r\n");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow row = dt.Rows[i];
                    sbExportSql.Append("INSERT INTO TB_HRTMPD( TID,GID,PID,PDESC,PDESCCHS,PTYPE,PLEN,PPREC,PNULL,PDEFAULT,PISKEY,PCTRL,PCTRLID,PCTRLD,PORDER,PRIGHT,PSYS,PLIST,PWIDTH,PFONTL,PFONTC,PAGGR,PAGDEST,PMAST,PSAVE)");
                    sbExportSql.Append(" VALUES ('" + RepalceSingleQuote(row, "TID") + "','" + RepalceSingleQuote(row, "GID") + "','" + RepalceSingleQuote(row, "PID") + "','" + RepalceSingleQuote(row, "PDESC") + "','" + RepalceSingleQuote(row, "PDESCCHS") + "','" + RepalceSingleQuote(row, "PTYPE") + "','" + RepalceSingleQuote(row, "PLEN") + "','" + RepalceSingleQuote(row, "PPREC") + "','" + RepalceSingleQuote(row, "PNULL") + "','" + RepalceSingleQuote(row, "PDEFAULT") + "','" + RepalceSingleQuote(row, "PISKEY") + "','" + RepalceSingleQuote(row, "PCTRL") + "','" + RepalceSingleQuote(row, "PCTRLID") + "','" + RepalceSingleQuote(row, "PCTRLD") + "','" + RepalceSingleQuote(row, "PORDER") + "','" + RepalceSingleQuote(row, "PRIGHT") + "','" + RepalceSingleQuote(row, "PSYS") + "','" + RepalceSingleQuote(row, "PLIST") + "','" + RepalceSingleQuote(row, "PWIDTH") + "','" + RepalceSingleQuote(row, "PFONTL") + "','" + RepalceSingleQuote(row, "PFONTC") + "','" + RepalceSingleQuote(row, "PAGGR") + "','" + RepalceSingleQuote(row, "PAGDEST") + "','" + RepalceSingleQuote(row, "PMAST") + "','" + RepalceSingleQuote(row, "PSAVE") + "')\r\n");
                }
            }
            return sbExportSql.ToString();
        }

        /// <summary>
        /// 获取模板动作配置的导出脚本
        /// </summary>
        /// <param name="strTid"></param>
        /// <returns></returns>
        private String ExportArchiveActions(string strTid)
        {
            String strSql = "SELECT * FROM TB_HRTMPA where TID = '" + strTid + "'";
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);

            StringBuilder sbExportSql = new StringBuilder();
            if ((dt != null) && (dt.Rows.Count > 0))
            {
                sbExportSql.Append("--模板动作表的配置\r\n");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow row = dt.Rows[i];
                    String strAid = row["AID"].ToString();
                    sbExportSql.Append("----模板动作" + strAid + "的配置\r\n");
                    sbExportSql.Append("INSERT INTO TB_HRTMPA (TID,AID,ADESC,ADESCCHS,ATYPE,ADETAIL,APARA0,APARA1,APARA2,APARA3,APARA4,APARA5,APARA6,APARA7,APARA8,APARA9,ALOCATION,AORDER,MOVENEXT,ISAUTOSAVE)");
                    sbExportSql.Append(" VALUES ('" + RepalceSingleQuote(row, "TID") + "','" + RepalceSingleQuote(row, "AID") + "','" + RepalceSingleQuote(row, "ADESC") + "','" + RepalceSingleQuote(row, "ADESCCHS") + "','" + RepalceSingleQuote(row, "ATYPE") + "','" + RepalceSingleQuote(row, "ADETAIL") + "','" + RepalceSingleQuote(row, "APARA0") + "','" + RepalceSingleQuote(row, "APARA1") + "','" + RepalceSingleQuote(row, "APARA2") + "','" + RepalceSingleQuote(row, "APARA3") + "','" + RepalceSingleQuote(row, "APARA4") + "','" + RepalceSingleQuote(row, "APARA5") + "','" + RepalceSingleQuote(row, "APARA6") + "','" + RepalceSingleQuote(row, "APARA7") + "','" + RepalceSingleQuote(row, "APARA8") + "','" + RepalceSingleQuote(row, "APARA9") + "','" + RepalceSingleQuote(row, "ALOCATION") + "','" + RepalceSingleQuote(row, "AORDER") + "','" + RepalceSingleQuote(row, "MOVENEXT") + "','" + RepalceSingleQuote(row, "ISAUTOSAVE") + "')\r\n");

                    //获取各动作下的消息提示配置
                    sbExportSql.Append(this.ExportArchiveActionTips(strTid, strAid));
                }
            }
            return sbExportSql.ToString();
        }

        /// <summary>
        /// 获取模板动作消息提示配置的导出脚本
        /// </summary>
        /// <param name="strTid"></param>
        /// <param name="strAid"></param>
        /// <returns></returns>
        private String ExportArchiveActionTips(string strTid, String strAid)
        {
            String strSql = "SELECT * FROM TB_HRTMPAR where TID = '" + strTid + "' AND AID = '" + strAid + "'";
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);

            StringBuilder sbExportSql = new StringBuilder();
            if ((dt != null) && (dt.Rows.Count > 0))
            {
                sbExportSql.Append("----模板动作" + strAid + "的消息提示表的配置\r\n");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow row = dt.Rows[i];
                    sbExportSql.Append("INSERT INTO TB_HRTMPAR (TID,AID,ECFROM,ECTO,MESSENG,MESSCHS)");
                    sbExportSql.Append(" VALUES ('" + RepalceSingleQuote(row, "TID") + "','" + RepalceSingleQuote(row, "AID") + "','" + RepalceSingleQuote(row, "ECFROM") + "','" + RepalceSingleQuote(row, "ECTO") + "','" + RepalceSingleQuote(row, "MESSENG") + "','" + RepalceSingleQuote(row, "MESSCHS") + "')\r\n");
                }
            }
            return sbExportSql.ToString();
        }

        /// <summary>
        /// 获取模板事件配置的导出脚本
        /// </summary>
        /// <param name="strTid"></param>
        /// <returns></returns>
        private String ExportArchiveEvents(string strTid)
        {
            String strSql = "SELECT * FROM TB_HRTMPE where TID = '" + strTid + "'";
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);

            StringBuilder sbExportSql = new StringBuilder();
            if ((dt != null) && (dt.Rows.Count > 0))
            {
                sbExportSql.Append("--模板事件表的配置\r\n");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow row = dt.Rows[i];
                    sbExportSql.Append("INSERT INTO TB_HRTMPE( TID,EID,EDESC,EDESCCHS,GID,PID,ENAME,ECONT)");
                    sbExportSql.Append(" VALUES ('" + RepalceSingleQuote(row, "TID") + "','" + RepalceSingleQuote(row, "EID") + "','" + RepalceSingleQuote(row, "EDESC") + "','" + RepalceSingleQuote(row, "EDESCCHS") + "','" + RepalceSingleQuote(row, "GID") + "','" + RepalceSingleQuote(row, "PID") + "','" + RepalceSingleQuote(row, "ENAME") + "','" + RepalceSingleQuote(row, "ECONT") + "')\r\n");
                }
            }
            return sbExportSql.ToString();
        }

        /// <summary>
        /// 获取模板状态场景配置的导出脚本
        /// </summary>
        /// <param name="strTid"></param>
        /// <returns></returns>
        private String ExportArchiveSences(string strTid)
        {
            String strSql = "SELECT * FROM TB_HRTMPS where TID = '" + strTid + "'";
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);

            StringBuilder sbExportSql = new StringBuilder();
            if ((dt != null) && (dt.Rows.Count > 0))
            {
                sbExportSql.Append("--模板状态场景表的配置\r\n");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow row = dt.Rows[i];
                    String strSid = row["SID"].ToString();
                    sbExportSql.Append("----模板状态场景" + strSid + "的配置\r\n");
                    sbExportSql.Append("INSERT INTO TB_HRTMPS (TID,SID,SDESC,SDESCCHS,SSLCT,SREF,SADD,SDEL,SEDIT,SALERT,SORDER,SSIZE,SCFORM,Filter)");
                    sbExportSql.Append(" VALUES ('" + RepalceSingleQuote(row, "TID") + "','" + RepalceSingleQuote(row, "SID") + "','" + RepalceSingleQuote(row, "SDESC") + "','" + RepalceSingleQuote(row, "SDESCCHS") + "','" + RepalceSingleQuote(row, "SSLCT") + "','" + RepalceSingleQuote(row, "SREF") + "','" + RepalceSingleQuote(row, "SADD") + "','" + RepalceSingleQuote(row, "SDEL") + "','" + RepalceSingleQuote(row, "SEDIT") + "','" + RepalceSingleQuote(row, "SALERT") + "','" + RepalceSingleQuote(row, "SORDER") + "','" + RepalceSingleQuote(row, "SSIZE") + "','" + RepalceSingleQuote(row, "SCFORM") + "','" + RepalceSingleQuote(row, "Filter") + "')\r\n");

                    //获取各状态场景下的分组配置
                    sbExportSql.Append(this.ExportArchiveSGroups(strTid, strSid));
                    //获取各状态场景下的动作配置
                    sbExportSql.Append(this.ExportArchiveSActions(strTid, strSid));
                    //获取各状态场景下的事件配置
                    sbExportSql.Append(this.ExportArchiveSEvents(strTid, strSid));
                }
            }
            return sbExportSql.ToString();
        }

        /// <summary>
        /// 获取模板场景状态下分组配置的导出脚本
        /// </summary>
        /// <param name="strTid"></param>
        /// <param name="strSid"></param>
        /// <returns></returns>
        private String ExportArchiveSGroups(string strTid, string strSid)
        {
            String strSql = "SELECT * FROM TB_HRTMPSG where TID = '" + strTid + "' AND SID ='" + strSid + "'";
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);

            StringBuilder sbExportSql = new StringBuilder();
            if ((dt != null) && (dt.Rows.Count > 0))
            {
                sbExportSql.Append("----模板场景状态" + strSid + "下分组表的配置\r\n");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow row = dt.Rows[i];
                    sbExportSql.Append("INSERT INTO TB_HRTMPSG( TID,SID,GID,GDESC,GDESCCHS,GTYPE,GORDER,GLIMIT,GCOUNT,GWIDTH,GRIGHT,GSLCT,GPAGE,GRCOUNT,DEFAULTCOLUMN,USERPAGE,islarge)");
                    sbExportSql.Append(" VALUES ('" + RepalceSingleQuote(row, "TID") + "','" + RepalceSingleQuote(row, "SID") + "','" + RepalceSingleQuote(row, "GID") + "','" + RepalceSingleQuote(row, "GDESC") + "','" + RepalceSingleQuote(row, "GDESCCHS") + "','" + RepalceSingleQuote(row, "GTYPE") + "','" + RepalceSingleQuote(row, "GORDER") + "','" + RepalceSingleQuote(row, "GLIMIT") + "','" + RepalceSingleQuote(row, "GCOUNT") + "','" + RepalceSingleQuote(row, "GWIDTH") + "','" + RepalceSingleQuote(row, "GRIGHT") + "','" + RepalceSingleQuote(row, "GSLCT") + "','" + RepalceSingleQuote(row, "GPAGE") + "','" + RepalceSingleQuote(row, "GRCOUNT") + "','" + RepalceSingleQuote(row, "DEFAULTCOLUMN") + "','" + RepalceSingleQuote(row, "USERPAGE") + "','" + RepalceSingleQuote(row, "islarge") + "')\r\n");

                    //获取各场景状态下分组下的字段属性的配置
                    sbExportSql.Append(this.ExportArchiveSPropertys(strTid, strSid, row["GID"].ToString()));
                }
            }
            return sbExportSql.ToString();
        }

        /// <summary>
        /// 获取状态分组下的字段属性的配置
        /// </summary>
        /// <param name="strTid"></param>
        /// <param name="strSid"></param>
        /// <param name="strGid"></param>
        /// <returns></returns>
        private String ExportArchiveSPropertys(string strTid, String strSid, String strGid)
        {
            String strSql = "SELECT * FROM TB_HRTMPSD where TID = '" + strTid + "' AND SID = '" + strSid + "' AND GID ='" + strGid + "'";
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);

            StringBuilder sbExportSql = new StringBuilder();
            if ((dt != null) && (dt.Rows.Count > 0))
            {
                sbExportSql.Append("------模板场景状态" + strSid + "下分组" + strGid + "表的配置\r\n");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow row = dt.Rows[i];
                    sbExportSql.Append("INSERT INTO TB_HRTMPSD( TID,SID,GID,PID,PDESC,PDESCCHS,PTYPE,PLEN,PPREC,PNULL,PDEFAULT,PISKEY,PCTRL,PCTRLID,PCTRLD,PORDER,PRIGHT,PSYS,PLIST,PWIDTH,PFONTL,PFONTC,PAGGR,PAGDEST,PMAST,PSAVE)");
                    sbExportSql.Append(" VALUES ('" + RepalceSingleQuote(row, "TID") + "','" + RepalceSingleQuote(row, "SID") + "','" + RepalceSingleQuote(row, "GID") + "','" + RepalceSingleQuote(row, "PID") + "','" + RepalceSingleQuote(row, "PDESC") + "','" + RepalceSingleQuote(row, "PDESCCHS") + "','" + RepalceSingleQuote(row, "PTYPE") + "','" + RepalceSingleQuote(row, "PLEN") + "','" + RepalceSingleQuote(row, "PPREC") + "','" + RepalceSingleQuote(row, "PNULL") + "','" + RepalceSingleQuote(row, "PDEFAULT") + "','" + RepalceSingleQuote(row, "PISKEY") + "','" + RepalceSingleQuote(row, "PCTRL") + "','" + RepalceSingleQuote(row, "PCTRLID") + "','" + RepalceSingleQuote(row, "PCTRLD") + "','" + RepalceSingleQuote(row, "PORDER") + "','" + RepalceSingleQuote(row, "PRIGHT") + "','" + RepalceSingleQuote(row, "PSYS") + "','" + RepalceSingleQuote(row, "PLIST") + "','" + RepalceSingleQuote(row, "PWIDTH") + "','" + RepalceSingleQuote(row, "PFONTL") + "','" + RepalceSingleQuote(row, "PFONTC") + "','" + RepalceSingleQuote(row, "PAGGR") + "','" + RepalceSingleQuote(row, "PAGDEST") + "','" + RepalceSingleQuote(row, "PMAST") + "','" + RepalceSingleQuote(row, "PSAVE") + "')\r\n");
                }
            }
            return sbExportSql.ToString();
        }

        /// <summary>
        /// 获取模板状态分组下动作配置的导出脚本
        /// </summary>
        /// <param name="strTid"></param>
        /// <param name="strSid"></param>
        /// <returns></returns>
        private String ExportArchiveSActions(string strTid, String strSid)
        {
            String strSql = "SELECT * FROM TB_HRTMPSA where TID = '" + strTid + "' AND SID = '" + strSid + "'";
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);

            StringBuilder sbExportSql = new StringBuilder();
            if ((dt != null) && (dt.Rows.Count > 0))
            {
                sbExportSql.Append("----模板场景状态" + strSid + "下动作的配置\r\n");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow row = dt.Rows[i];
                    sbExportSql.Append("INSERT INTO TB_HRTMPSA (TID,SID,AID,ADESC,ADESCCHS,ATYPE,ADETAIL,APARA0,APARA1,APARA2,APARA3,APARA4,APARA5,APARA6,APARA7,APARA8,APARA9,ALOCATION,AORDER,MOVENEXT,ISAUTOSAVE,ARIGHT)");
                    sbExportSql.Append(" VALUES ('" + RepalceSingleQuote(row, "TID") + "','" + RepalceSingleQuote(row, "SID") + "','" + RepalceSingleQuote(row, "AID") + "','" + RepalceSingleQuote(row, "ADESC") + "','" + RepalceSingleQuote(row, "ADESCCHS") + "','" + RepalceSingleQuote(row, "ATYPE") + "','" + RepalceSingleQuote(row, "ADETAIL") + "','" + RepalceSingleQuote(row, "APARA0") + "','" + RepalceSingleQuote(row, "APARA1") + "','" + RepalceSingleQuote(row, "APARA2") + "','" + RepalceSingleQuote(row, "APARA3") + "','" + RepalceSingleQuote(row, "APARA4") + "','" + RepalceSingleQuote(row, "APARA5") + "','" + RepalceSingleQuote(row, "APARA6") + "','" + RepalceSingleQuote(row, "APARA7") + "','" + RepalceSingleQuote(row, "APARA8") + "','" + RepalceSingleQuote(row, "APARA9") + "','" + RepalceSingleQuote(row, "ALOCATION") + "','" + RepalceSingleQuote(row, "AORDER") + "','" + RepalceSingleQuote(row, "MOVENEXT") + "','" + RepalceSingleQuote(row, "ISAUTOSAVE") + "','" + RepalceSingleQuote(row, "ARIGHT") + "')\r\n");

                }
            }
            return sbExportSql.ToString();
        }

        /// <summary>
        /// 获取模板状态下事件配置的导出脚本
        /// </summary>
        /// <param name="strTid"></param>
        /// <param name="strSid"></param>
        /// <returns></returns>
        private String ExportArchiveSEvents(string strTid, string strSid)
        {
            String strSql = "SELECT * FROM TB_HRTMPSE where TID = '" + strTid + "' AND SID = '" + strSid + "'";
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);

            StringBuilder sbExportSql = new StringBuilder();
            if ((dt != null) && (dt.Rows.Count > 0))
            {
                sbExportSql.Append("----模板场景状态" + strSid + "下事件的配置\r\n");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow row = dt.Rows[i];
                    sbExportSql.Append("INSERT INTO TB_HRTMPSE( TID,SID,EID,EDESC,EDESCCHS,GID,PID,ENAME,ECONT,ERIGHT)");
                    sbExportSql.Append(" VALUES ('" + RepalceSingleQuote(row, "TID") + "','" + RepalceSingleQuote(row, "SID") + "','" + RepalceSingleQuote(row, "EID") + "','" + RepalceSingleQuote(row, "EDESC") + "','" + RepalceSingleQuote(row, "EDESCCHS") + "','" + RepalceSingleQuote(row, "GID") + "','" + RepalceSingleQuote(row, "PID") + "','" + RepalceSingleQuote(row, "ENAME") + "','" + RepalceSingleQuote(row, "ECONT") + "','" + RepalceSingleQuote(row, "ERIGHT") + "')\r\n");
                }
            }
            return sbExportSql.ToString();
        }


        /// <summary>
        /// 获取模板角色配置的导出脚本
        /// </summary>
        /// <param name="strTid"></param>
        /// <returns></returns>
        private String ExportArchiveRoles(string strTid)
        {
            String strSql = "SELECT * FROM TB_HRTMPR where TID = '" + strTid + "'";
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);

            StringBuilder sbExportSql = new StringBuilder();
            if ((dt != null) && (dt.Rows.Count > 0))
            {
                sbExportSql.Append("--模板角色表的配置\r\n");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow row = dt.Rows[i];
                    String strRid = row["RID"].ToString();
                    sbExportSql.Append("----模板角色" + strRid + "的配置\r\n");
                    sbExportSql.Append("INSERT INTO TB_HRTMPR ( TID,RID,RDESC,RDESCCHS,RPARA0,RPARA1,RPARA2,RPARA3,RPARA4,RPARA5,RPARA6,RPARA7,RPARA8,RPARA9,RORDER)");
                    sbExportSql.Append(" VALUES ('" + RepalceSingleQuote(row, "TID") + "','" + RepalceSingleQuote(row, "RID") + "','" + RepalceSingleQuote(row, "RDESC") + "','" + RepalceSingleQuote(row, "RDESCCHS") + "','" + RepalceSingleQuote(row, "RPARA0") + "','" + RepalceSingleQuote(row, "RPARA1") + "','" + RepalceSingleQuote(row, "RPARA2") + "','" + RepalceSingleQuote(row, "RPARA3") + "','" + RepalceSingleQuote(row, "RPARA4") + "','" + RepalceSingleQuote(row, "RPARA5") + "','" + RepalceSingleQuote(row, "RPARA6") + "','" + RepalceSingleQuote(row, "RPARA7") + "','" + RepalceSingleQuote(row, "RPARA8") + "','" + RepalceSingleQuote(row, "RPARA9") + "','" + RepalceSingleQuote(row, "RORDER") + "')\r\n");

                    //获取角色状态表配置
                    sbExportSql.Append(this.ExportArchiveRoleSences(strTid, strRid));
                }
            }
            return sbExportSql.ToString();
        }

        /// <summary>
        /// 获取模板角色配置的导出脚本
        /// </summary>
        /// <param name="strTid"></param>
        /// <returns></returns>
        public static String ExportUserRoles(string strTid)
        {
            String strSql = "SELECT * FROM TB_HRTMPR where TID = '" + strTid + "'";
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);

            StringBuilder sbExportSql = new StringBuilder();

            if ((dt != null) && (dt.Rows.Count > 0))
            {
                sbExportSql.Append("--用户角色表的配置\r\n");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow row = dt.Rows[i];
                    String strRid = row["RID"].ToString();

                    String strSql_UserRole = "SELECT * FROM TB_HR_USERROLE where TID = '" + strTid + "' AND RID = '" + strRid + "'";
                    DataTable dt_ur = SqlParamDao.GetDataTableBySql(strSql_UserRole);
                    if ((dt_ur != null) && (dt_ur.Rows.Count > 0))
                    {

                        for (int j = 0; j < dt_ur.Rows.Count; j++)
                        {
                            DataRow row_UR = dt_ur.Rows[j];

                            sbExportSql.Append("----用户角色列表（TID：" + strTid + "----RID：" + strRid + "）\r\n");
                            sbExportSql.Append("INSERT INTO TB_HR_USERROLE (SUSERID,TID,RID) values ('" + row_UR["SUSERID"].ToString() + "','" + strTid + "','" + strRid + "')\r\n");
                        }
                    }
                }
            }
            return sbExportSql.ToString();

        }

        /// <summary>
        /// 获取模板角色状态配置的导出脚本
        /// </summary>
        /// <param name="strTid"></param>
        /// <param name="strRid"></param>
        /// <returns></returns>
        private String ExportArchiveRoleSences(string strTid, String strRid)
        {
            String strSql = "SELECT * FROM TB_HRTMPRD where TID = '" + strTid + "' AND RID = '" + strRid + "'";
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);

            StringBuilder sbExportSql = new StringBuilder();
            if ((dt != null) && (dt.Rows.Count > 0))
            {
                sbExportSql.Append("----模板角色" + strRid + "下角色状态的配置\r\n");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow row = dt.Rows[i];
                    sbExportSql.Append("INSERT INTO TB_HRTMPRD ( TID,RID,SID)");
                    sbExportSql.Append(" VALUES ('" + RepalceSingleQuote(row, "TID") + "','" + RepalceSingleQuote(row, "RID") + "','" + RepalceSingleQuote(row, "SID") + "')\r\n");
                }
            }
            return sbExportSql.ToString();
        }

        /// <summary>
        /// 获取各分组对应业务表数据的导出脚本
        /// </summary>
        /// <param name="strTid"></param>
        /// <returns></returns>
        public String ExportTablesData(string strTid)
        {
            String strSql = "SELECT * FROM TB_HRTMPG where TID = '" + strTid + "'";
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);

            StringBuilder sbExportSql = new StringBuilder();
            if ((dt != null) && (dt.Rows.Count > 0))
            {
                sbExportSql.Append("\r\n");
                sbExportSql.Append("\r\n");
                sbExportSql.Append("\r\n");
                sbExportSql.Append("\r\n");
                sbExportSql.Append("\r\n");
                sbExportSql.Append("--【请先对该模板创建后，再进行业务数据的导入！】\r\n");
                sbExportSql.Append("--业务表数据表的数据\r\n");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow row = dt.Rows[i];
                    String strGid = row["GID"].ToString();
                    String strTableName = strTid + "_" + strGid;

                    //获取某业务表数据的导出脚本
                    sbExportSql.Append(this.ExportTableData(strTableName));
                }
            }
            return sbExportSql.ToString();
        }

        /// <summary>
        /// 获取某业务表数据的导出脚本
        /// </summary>
        /// <param name="strTableName"></param>
        /// <returns></returns>
        public String ExportTableData(string strTableName)
        {
            String strSql = "select a.[name] from [syscolumns] a inner join [sysobjects] b on a.[id] = b.[id] and b.[name] = '" + strTableName + "' order by [colorder]";
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);

            StringBuilder sbExportSql = new StringBuilder();
            if ((dt != null) && (dt.Rows.Count > 0))
            {
                sbExportSql.Append("----业务表数据" + strTableName + "的数据\r\n");
                String strColName = "";
                int iColCount = dt.Rows.Count;
                Hashtable hsTable = new Hashtable();
                for (int i = 0; i < iColCount; i++)
                {
                    DataRow row = dt.Rows[i];
                    String strColTemp = "[" + row["name"].ToString() + "]";
                    if (i == 0)
                    {
                        strColName = strColName + strColTemp;
                    }
                    else
                    {
                        strColName = strColName + "," + strColTemp;
                    }
                    hsTable.Add(i.ToString(), strColTemp);
                }

                //获取业务表数据
                String strData = "select " + strColName + " from " + strTableName + "";
                DataTable dtData = SqlParamDao.GetDataTableBySql(strData);
                if ((dtData != null) && (dtData.Rows.Count > 0))
                {
                    for (int j = 0; j < dtData.Rows.Count; j++)
                    {
                        DataRow rowData = dtData.Rows[j];
                        String strColValue = "";
                        for (int k = 0; k < iColCount; k++)
                        {
                            String strCol = hsTable[k.ToString()].ToString();
                            if (k == 0)
                            {
                                strColValue = strColValue + "'" + RepalceSingleQuote(rowData, strCol) + "'";
                            }
                            else
                            {
                                strColValue = strColValue + ",'" + RepalceSingleQuote(rowData, strCol) + "'";
                            }
                        }
                        if (!String.IsNullOrEmpty(strColValue))
                        {
                            sbExportSql.Append("INSERT INTO " + strTableName + " ( " + strColName + ")");
                            sbExportSql.Append(" VALUES (" + strColValue + ")\r\n");
                        }
                    }
                }
            }
            return sbExportSql.ToString();
        }


        /// <summary>
        /// 获取字段内容对应字符串（同时替换单引号）
        /// </summary>
        /// <param name="row"></param>
        /// <param name="strFieldName"></param>
        /// <returns></returns>
        private String RepalceSingleQuote(DataRow row, String strFieldName)
        {
            String str = "";
            if (!string.IsNullOrEmpty(strFieldName))
            {
                strFieldName = strFieldName.Replace("[", "").Replace("]", "");
                if (row[strFieldName] != DBNull.Value)
                {
                    str = row[strFieldName].ToString().Replace("'", "''");
                }
            }
            return str;
        }

        /// <summary>
        /// 根据清单编码导出其所有数据
        /// </summary>
        /// <param name="strLid"></param>
        /// <returns></returns>
        public String GetListDataExportSql(string strLid)
        {
            String strSql = "SELECT * FROM TB_HRLSTH where LID = '" + strLid + "'";
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);

            StringBuilder sbExportSql = new StringBuilder();
            if ((dt != null) && (dt.Rows.Count > 0))
            {
                DataRow row = dt.Rows[0];
                sbExportSql.Append("--字典清单定义表的数据配置\r\n");
                sbExportSql.Append("INSERT INTO TB_HRLSTH( LID,LDESC,LDESCCHS,BISSTOP)");
                sbExportSql.Append(" VALUES ('" + RepalceSingleQuote(row, "LID") + "','" + RepalceSingleQuote(row, "LDESC") + "','" + RepalceSingleQuote(row, "LDESCCHS") + "','" + RepalceSingleQuote(row, "BISSTOP") + "')\r\n");

                strSql = "SELECT * FROM TB_HRLSTD where LID = '" + strLid + "'";
                DataTable dt_Detail = SqlParamDao.GetDataTableBySql(strSql);

                if ((dt_Detail != null) && (dt_Detail.Rows.Count > 0))
                {
                    for (int i = 0; i < dt_Detail.Rows.Count; i++)
                    {
                        DataRow row_Detail = dt_Detail.Rows[i];

                        sbExportSql.Append("----字典清单定义'" + strLid + "'的明细数据配置\r\n");
                        sbExportSql.Append("INSERT INTO TB_HRLSTD( LID,CID,CDESC,CDESCCHS,CUID,P0,P1,P2,P3,P4,P5,P6,P7,P8,P9,BISSTOP) ");
                        sbExportSql.Append(" VALUES ('" + RepalceSingleQuote(row_Detail, "LID") + "','" + RepalceSingleQuote(row_Detail, "CID") + "','" + RepalceSingleQuote(row_Detail, "CDESC") + "','" + RepalceSingleQuote(row_Detail, "CDESCCHS") + "','" + RepalceSingleQuote(row_Detail, "CUID") + "','" + RepalceSingleQuote(row_Detail, "P0") + "','" + RepalceSingleQuote(row_Detail, "P1") + "','" + RepalceSingleQuote(row_Detail, "P2") + "','" + RepalceSingleQuote(row_Detail, "P3") + "','" + RepalceSingleQuote(row_Detail, "P4") + "','" + RepalceSingleQuote(row_Detail, "P5") + "','" + RepalceSingleQuote(row_Detail, "P6") + "','" + RepalceSingleQuote(row_Detail, "P7") + "','" + RepalceSingleQuote(row_Detail, "P8") + "','" + RepalceSingleQuote(row_Detail, "P9") + "','" + RepalceSingleQuote(row_Detail, "BISSTOP") + "')\r\n");

                    }
                }

            }
            return sbExportSql.ToString();
        }

        /// <summary>
        /// 根据表名获取现在数据库中的Drop和Create语句脚本输出
        /// </summary>
        /// <param name="strTableName"></param>
        /// <returns></returns>
        public String GetTableDropAndCreateSql(string strTableName)
        {
            StringBuilder sbExportSql = new StringBuilder();
            try{
                String strSpace = "    ";
                String strEnter = "\r\n";
                //首先获取DROP TABLE的脚本
                sbExportSql.Append("IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[" + strTableName + "]') AND type in (N'U'))" + strEnter);
                sbExportSql.Append("DROP TABLE [dbo].[" + strTableName + "] " + strEnter + strEnter);
                //再获取CREATE TABLE的脚本
                sbExportSql.Append("CREATE TABLE [dbo].[" + strTableName + "] ( " + strEnter);
                ////////处理列名、类型、长度、是否可空
                StringBuilder strSql_Column = new StringBuilder();
                strSql_Column.Append("SELECT COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH,NUMERIC_PRECISION,NUMERIC_SCALE, IS_NULLABLE ");
                strSql_Column.Append(" FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + strTableName + "'");
                DataTable dt_Column = SqlParamDao.GetDataTableBySql(strSql_Column.ToString());
                if (dt_Column != null && dt_Column.Rows.Count > 0)
                {
                    for (int i = 0; i < dt_Column.Rows.Count; i++)
                    {
                        string strColumnName = dt_Column.Rows[i]["COLUMN_NAME"].ToString();
                        string strDataType = dt_Column.Rows[i]["DATA_TYPE"].ToString();
                        string strMaxLength = dt_Column.Rows[i]["CHARACTER_MAXIMUM_LENGTH"] == null ? "" : dt_Column.Rows[i]["CHARACTER_MAXIMUM_LENGTH"].ToString();
                        //NUMERIC类型时的长度
                        string strNumericPresicion = dt_Column.Rows[i]["NUMERIC_PRECISION"] == null ? "" : dt_Column.Rows[i]["NUMERIC_PRECISION"].ToString();
                        //NUMERIC类型时的精度
                        string strNumericScale = dt_Column.Rows[i]["NUMERIC_SCALE"] == null ? "" : dt_Column.Rows[i]["NUMERIC_SCALE"].ToString();
                        string strIsNullable = dt_Column.Rows[i]["IS_NULLABLE"].ToString();

                        switch (strDataType.ToLower())
                        {
                            case "varchar":
                                sbExportSql.Append(strSpace + "[" + strColumnName + "] [" + strDataType + "] (" + strMaxLength + ")");
                                break;
                            case "int":
                            case "datetime":
                            case "date":
                            case "time":
                                sbExportSql.Append(strSpace + "[" + strColumnName + "] [" + strDataType + "] ");
                                break;
                            case "numeric":
                            case "decimal":
                                sbExportSql.Append(strSpace + "[" + strColumnName + "] [" + strDataType + "] (" + strNumericPresicion + "," + strNumericScale + ")");
                                break;
                            default:
                                sbExportSql.Append(strSpace + "[" + strColumnName + "] [" + strDataType + "] (" + strMaxLength + ")");
                                break;
                        }
                        sbExportSql.Append((strIsNullable == "YES" ? " NULL " : " NOT NULL ") + ',');
                        sbExportSql.Append(strEnter);
                    }
                }
                ////////处理主键
                StringBuilder strSql_Key = new StringBuilder();
                strSql_Key.Append("SELECT COLUMN_NAME,CONSTRAINT_NAME FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE WHERE TABLE_NAME = '" + strTableName + "' ");
                strSql_Key.Append(" AND CONSTRAINT_NAME LIKE 'PK_%' ORDER BY ORDINAL_POSITION");
                DataTable dt_Key = SqlParamDao.GetDataTableBySql(strSql_Key.ToString());
                if (dt_Key != null && dt_Key.Rows.Count > 0)
                {
                    int iCount = dt_Key.Rows.Count;
                    string strKeyName = dt_Key.Rows[0]["CONSTRAINT_NAME"].ToString();
                    sbExportSql.Append("CONSTRAINT [" + strKeyName + "] PRIMARY KEY CLUSTERED " + strEnter);
                    sbExportSql.Append("(" + strEnter);
                    for (int i = 0; i < dt_Key.Rows.Count; i++)
                    {
                        string strKeyColumnName = dt_Key.Rows[i]["COLUMN_NAME"].ToString();
                        sbExportSql.Append(strSpace + "[" + strKeyColumnName + "] ASC ");
                        sbExportSql.Append((iCount > 1 && i < iCount - 1) ? "," : "");
                        sbExportSql.Append(strEnter);
                    }
                    sbExportSql.Append(")WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY] " + strEnter);

                }
                sbExportSql.Append(") ON [PRIMARY] " + strEnter);
            }
            catch (Exception ex)
            {
                log.Error("根据表名获取现在数据库中的Drop和Create语句脚本输出GetTableDropAndCreateSql("+strTableName+")时出错：");
                log.Error(ex);
            }
            return sbExportSql.ToString();
        }
    }
}
