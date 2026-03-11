using System;
using System.Data.Common;
using Com.ValuePlus.Database;
using Com.ValuePlus.Common.Config;
using Com.ValuePlus.Common;
using Com.ValuePlus.Archive.Config;
//using Com.ValuePlus.DAL;
using System.Text;
using System.Data;

namespace Com.ValuePlus.Archive.DAL
{
    public class ArchiveBaseDao
    {
        #region 获得档案模板列表
        /// <summary>
        /// 获得档案模板列表
        /// </summary>
        /// <returns></returns>
        public static DataTable GetTemplateList()
        {
            DataTable dt = null;
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DataSet ds = dao.ExecuteDataSet(ArchiveSqlConfig.Instance.GetTemplate_TB_HRTMPH(), null);
                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                }
            }
            return dt;
        }
        #endregion

        #region 更新档案模板列表
        /// <summary>
        /// 更新档案模板列表
        /// </summary>
        /// <param name="TID"></param>
        /// <param name="TDESC"></param>
        /// <param name="TDESCCHS"></param>
        /// <param name="TREC"></param>
        /// <param name="BTNSTANTION"></param>
        /// <param name="ROLEFILE"></param>
        /// <param name="BISSTOP"></param>
        /// <returns></returns>
        public static int UpdateTemplate(string TID, string TDESC, string TDESCCHS, string TREC, string BTNSTANTION, string ROLEFILE, string BISSTOP)
        {

            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(ArchiveSqlConfig.Instance.GetTemplate_TB_HRTMPH_Update());
                param[0].Value = TDESC;
                param[1].Value = TDESCCHS;
                param[2].Value = TREC;
                param[3].Value = BTNSTANTION;
                param[4].Value = ROLEFILE;
                param[5].Value = BISSTOP;
                param[6].Value = TID;
                return dao.ExecuteNonQuery(ArchiveSqlConfig.Instance.GetTemplate_TB_HRTMPH_Update(), param);
            }

        }
        #endregion

        #region 删除档案模板列表
        /// <summary>
        /// 删除档案模板列表
        /// </summary>
        /// <returns></returns>
        public static int DeleteTemplate(string TID)
        {

            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(ArchiveSqlConfig.Instance.GetTemplate_TB_HRTMPH_Delete());
                param[0].Value = TID;
                return dao.ExecuteNonQuery(ArchiveSqlConfig.Instance.GetTemplate_TB_HRTMPH_Delete(), param);
            }

        }
        #endregion

        #region 增加档案模板
        /// <summary>
        /// 增加档案模板
        /// </summary>
        /// <param name="TID"></param>
        /// <param name="TDESC"></param>
        /// <param name="TDESCCHS"></param>
        /// <param name="TREC"></param>
        /// <param name="BTNSTANTION"></param>
        /// <param name="ROLEFILE"></param>
        /// <param name="BISSTOP"></param>
        /// <returns></returns>
        public static int AddTemplate(string TID, string TDESC, string TDESCCHS, string TREC, string BTNSTANTION, string ROLEFILE, string BISSTOP)
        {

            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(ArchiveSqlConfig.Instance.GetTemplate_TB_HRTMPH_Add());
                param[0].Value = TID;
                param[1].Value = TDESC;
                param[2].Value = TDESCCHS;
                param[3].Value = TREC;
                param[4].Value = BTNSTANTION;
                param[5].Value = ROLEFILE;
                param[6].Value = BISSTOP;
                return dao.ExecuteNonQuery(ArchiveSqlConfig.Instance.GetTemplate_TB_HRTMPH_Add(), param);
            }

        }
        #endregion

        #region 获得档案模板详情
        /// <summary>
        /// 获得档案模板详情
        /// </summary>
        /// <param name="TID"></param>
        /// <returns></returns>
        public static DataTable GetTemplateDetail(string TID)
        {
            DataTable dt = null;
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(ArchiveSqlConfig.Instance.GetTemplate_TB_HRTMPG());
                param[0].Value = TID;
                DataSet ds = dao.ExecuteDataSet(ArchiveSqlConfig.Instance.GetTemplate_TB_HRTMPG(), param);
                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                }
            }
            return dt;
        }
        #endregion

        #region 更新档案模板详情
        /// <summary>
        /// 更新档案模板详情
        /// </summary>
        /// <param name="TID"></param>
        /// <param name="TDESC"></param>
        /// <param name="TDESCCHS"></param>
        /// <param name="TREC"></param>
        /// <param name="BTNSTANTION"></param>
        /// <param name="ROLEFILE"></param>
        /// <param name="BISSTOP"></param>
        /// <returns></returns>
        public static int UpdateTemplateDetail(string TID, string GID, string GDESC, string GDESCCHS, string GTYPE, string GORDER, string GLIMIT, string GCOUNT, string GWIDTH, string GHIST, string GPAGE, string GRCOUNT, string GVIEW, string GSQL, string USERPAGE, string islarge)
        {

            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(ArchiveSqlConfig.Instance.GetTemplate_TB_HRTMPG_Update());
                param[0].Value = GDESC;
                param[1].Value = GDESCCHS;
                param[2].Value = GTYPE;
                param[3].Value = GORDER;
                param[4].Value = GLIMIT;
                param[5].Value = GCOUNT;
                param[6].Value = GWIDTH;
                param[7].Value = GHIST;
                param[8].Value = GPAGE;
                param[9].Value = GRCOUNT;
                param[10].Value = GVIEW;
                param[11].Value = GSQL;
                param[12].Value = USERPAGE;
                param[13].Value = islarge;
                param[14].Value = GID;
                param[15].Value = TID;
                return dao.ExecuteNonQuery(ArchiveSqlConfig.Instance.GetTemplate_TB_HRTMPG_Update(), param);
            }

        }
        #endregion

        #region 删除档案模板详情
        /// <summary>
        /// 删除档案模板详情
        /// </summary>
        /// <param name="GID"></param>
        /// <returns></returns>
        public static int DeleteTemplateDetail(string GID, string TID)
        {

            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(ArchiveSqlConfig.Instance.GetTemplate_TB_HRTMPG_Delete());
                param[0].Value = GID;
                param[1].Value = TID;
                return dao.ExecuteNonQuery(ArchiveSqlConfig.Instance.GetTemplate_TB_HRTMPG_Delete(), param);
            }

        }
        #endregion

        #region 增加档案模板详情
        /// <summary>
        /// 增加档案模板详情
        /// </summary>
        /// <param name="TID"></param>
        /// <param name="GID"></param>
        /// <param name="GDESC"></param>
        /// <param name="GDESCCHS"></param>
        /// <param name="GTYPE"></param>
        /// <param name="GORDER"></param>
        /// <param name="GLIMIT"></param>
        /// <param name="GCOUNT"></param>
        /// <param name="GWIDTH"></param>
        /// <param name="GHIST"></param>
        /// <param name="GPAGE"></param>
        /// <param name="GRCOUNT"></param>
        /// <param name="GVIEW"></param>
        /// <param name="GSQL"></param>
        /// <param name="USERPAGE"></param>
        /// <param name="islarge"></param>
        /// <returns></returns>
        public static int AddTemplateDetail(string TID, string GID, string GDESC, string GDESCCHS, string GTYPE, string GORDER, string GLIMIT, string GCOUNT, string GWIDTH, string GHIST, string GPAGE, string GRCOUNT, string GVIEW, string GSQL, string USERPAGE, string islarge)
        {

            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(ArchiveSqlConfig.Instance.GetTemplate_TB_HRTMPG_Add());
                param[0].Value = TID;
                param[1].Value = GID;
                param[2].Value = GDESC;
                param[3].Value = GDESCCHS;
                param[4].Value = GTYPE;
                param[5].Value = GORDER;
                param[6].Value = GLIMIT;
                param[7].Value = GCOUNT;
                param[8].Value = GWIDTH;
                param[9].Value = GHIST;
                param[10].Value = GPAGE;
                param[11].Value = GRCOUNT;
                param[12].Value = GVIEW;
                param[13].Value = GSQL;
                param[14].Value = USERPAGE;
                param[15].Value = islarge;
                return dao.ExecuteNonQuery(ArchiveSqlConfig.Instance.GetTemplate_TB_HRTMPG_Add(), param);
            }

        }
        #endregion

        #region 获得档案模板角色
        /// <summary>
        /// 获得档案模板角色
        /// </summary>
        /// <param name="TID"></param>
        /// <returns></returns>
        public static DataTable GetTemplateRole(string TID)
        {
            DataTable dt = null;
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(ArchiveSqlConfig.Instance.GetTemplate_TB_HRTMPR());
                param[0].Value = TID;
                DataSet ds = dao.ExecuteDataSet(ArchiveSqlConfig.Instance.GetTemplate_TB_HRTMPR(), param);
                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                }
            }
            return dt;
        }
        #endregion

        #region 更新档案模板角色
        /// <summary>
        /// 更新档案模板角色
        /// </summary>
        /// <param name="TID"></param>
        /// <param name="RID"></param>
        /// <param name="RDESC"></param>
        /// <param name="RDESCCHS"></param>
        /// <param name="RPARA0"></param>
        /// <param name="RPARA1"></param>
        /// <param name="RPARA2"></param>
        /// <param name="RPARA3"></param>
        /// <param name="RPARA4"></param>
        /// <param name="RPARA5"></param>
        /// <param name="RPARA6"></param>
        /// <param name="RPARA7"></param>
        /// <param name="RPARA8"></param>
        /// <param name="RPARA9"></param>
        /// <param name="RORDER"></param>
        /// <returns></returns>
        public static int UpdateTemplateRole(string TID, string RID, string RDESC, string RDESCCHS, string RPARA0, string RPARA1, string RPARA2, string RPARA3, string RPARA4, string RPARA5, string RPARA6, string RPARA7, string RPARA8, string RPARA9, string RORDER)
        {

            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(ArchiveSqlConfig.Instance.GetTemplate_TB_HRTMPR_Update());
                param[0].Value = RDESC;
                param[1].Value = RDESCCHS;
                param[2].Value = RPARA0;
                param[3].Value = RPARA1;
                param[4].Value = RPARA2;
                param[5].Value = RPARA3;
                param[6].Value = RPARA4;
                param[7].Value = RPARA5;
                param[8].Value = RPARA6;
                param[9].Value = RPARA7;
                param[10].Value = RPARA8;
                param[11].Value = RPARA9;
                param[12].Value = RORDER;
                param[13].Value = RID;
                param[14].Value = TID;
                return dao.ExecuteNonQuery(ArchiveSqlConfig.Instance.GetTemplate_TB_HRTMPR_Update(), param);
            }

        }
        #endregion

        #region 删除档案模板角色
        /// <summary>
        /// 删除档案模板角色
        /// </summary>
        /// <param name="RID"></param>
        /// <param name="TID"></param>
        /// <returns></returns>
        public static int DeleteTemplateRole(string RID, string TID)
        {

            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(ArchiveSqlConfig.Instance.GetTemplate_TB_HRTMPR_Delete());
                param[0].Value = RID;
                param[1].Value = TID;
                return dao.ExecuteNonQuery(ArchiveSqlConfig.Instance.GetTemplate_TB_HRTMPR_Delete(), param);
            }

        }
        #endregion

        #region 增加档案模板角色
        /// <summary>
        /// 增加档案模板角色
        /// </summary>
        /// <param name="TID"></param>
        /// <param name="RID"></param>
        /// <param name="RDESC"></param>
        /// <param name="RDESCCHS"></param>
        /// <param name="RPARA0"></param>
        /// <param name="RPARA1"></param>
        /// <param name="RPARA2"></param>
        /// <param name="RPARA3"></param>
        /// <param name="RPARA4"></param>
        /// <param name="RPARA5"></param>
        /// <param name="RPARA6"></param>
        /// <param name="RPARA7"></param>
        /// <param name="RPARA8"></param>
        /// <param name="RPARA9"></param>
        /// <param name="RORDER"></param>
        /// <returns></returns>
        public static int AddTemplateRole(string TID, string RID, string RDESC, string RDESCCHS, string RPARA0, string RPARA1, string RPARA2, string RPARA3, string RPARA4, string RPARA5, string RPARA6, string RPARA7, string RPARA8, string RPARA9, string RORDER)
        {

            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(ArchiveSqlConfig.Instance.GetTemplate_TB_HRTMPR_Add());
                param[0].Value = TID;
                param[1].Value = RID;
                param[2].Value = RDESC;
                param[3].Value = RDESCCHS;
                param[4].Value = RPARA0;
                param[5].Value = RPARA1;
                param[6].Value = RPARA2;
                param[7].Value = RPARA3;
                param[8].Value = RPARA4;
                param[9].Value = RPARA5;
                param[10].Value = RPARA6;
                param[11].Value = RPARA7;
                param[12].Value = RPARA8;
                param[13].Value = RPARA9;
                param[14].Value = RORDER;
                return dao.ExecuteNonQuery(ArchiveSqlConfig.Instance.GetTemplate_TB_HRTMPR_Add(), param);
            }

        }
        #endregion

        #region 获得档案模板状态
        /// <summary>
        /// 获得档案模板状态
        /// </summary>
        /// <param name="TID"></param>
        /// <returns></returns>
        public static DataTable GetTemplateScene(string TID)
        {
            DataTable dt = null;
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(ArchiveSqlConfig.Instance.GetTemplate_TB_HRTMPS());
                param[0].Value = TID;
                DataSet ds = dao.ExecuteDataSet(ArchiveSqlConfig.Instance.GetTemplate_TB_HRTMPS(), param);
                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                }
            }
            return dt;
        }
        #endregion

        #region 更新档案模板状态
        /// <summary>
        /// 更新档案模板状态
        /// </summary>
        /// <param name="TID"></param>
        /// <param name="SID"></param>
        /// <param name="SDESC"></param>
        /// <param name="SDESCCHS"></param>
        /// <param name="SSLCT"></param>
        /// <param name="SREF"></param>
        /// <param name="SADD"></param>
        /// <param name="SDEL"></param>
        /// <param name="SEDIT"></param>
        /// <param name="SALERT"></param>
        /// <param name="SORDER"></param>
        /// <param name="SSIZE"></param>
        /// <param name="SCFORM"></param>
        /// <param name="Filter"></param>
        /// <returns></returns>
        public static int UpdateTemplateScene(string TID, string SID, string SDESC, string SDESCCHS, string SSLCT, string SREF, string SADD, string SDEL, string SEDIT, string SALERT, string SORDER, string SSIZE, string SCFORM, string Filter)
        {

            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(ArchiveSqlConfig.Instance.GetTemplate_TB_HRTMPS_Update());
                param[0].Value = SDESC;
                param[1].Value = SDESCCHS;
                param[2].Value = SSLCT;
                param[3].Value = SREF;
                param[4].Value = SADD;
                param[5].Value = SDEL;
                param[6].Value = SEDIT;
                param[7].Value = SALERT;
                param[8].Value = SORDER;
                param[9].Value = SSIZE;
                param[10].Value = SCFORM;
                param[11].Value = Filter;
                param[12].Value = SID;
                param[13].Value = TID;
                return dao.ExecuteNonQuery(ArchiveSqlConfig.Instance.GetTemplate_TB_HRTMPS_Update(), param);
            }

        }
        #endregion

        #region 删除档案模板状态
        /// <summary>
        /// 删除档案模板状态
        /// </summary>
        /// <param name="SID"></param>
        /// <param name="TID"></param>
        /// <returns></returns>
        public static int DeleteTemplateScene(string SID, string TID)
        {

            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(ArchiveSqlConfig.Instance.GetTemplate_TB_HRTMPS_Delete());
                param[0].Value = SID;
                param[1].Value = TID;
                return dao.ExecuteNonQuery(ArchiveSqlConfig.Instance.GetTemplate_TB_HRTMPS_Delete(), param);
            }

        }
        #endregion

        #region 增加档案模板状态
        /// <summary>
        /// 增加档案模板状态
        /// </summary>
        /// <param name="TID"></param>
        /// <param name="SID"></param>
        /// <param name="SDESC"></param>
        /// <param name="SDESCCHS"></param>
        /// <param name="SSLCT"></param>
        /// <param name="SREF"></param>
        /// <param name="SADD"></param>
        /// <param name="SDEL"></param>
        /// <param name="SEDIT"></param>
        /// <param name="SALERT"></param>
        /// <param name="SORDER"></param>
        /// <param name="SSIZE"></param>
        /// <param name="SCFORM"></param>
        /// <param name="Filter"></param>
        /// <returns></returns>
        public static int AddTemplateScene(string TID, string SID, string SDESC, string SDESCCHS, string SSLCT, string SREF, string SADD, string SDEL, string SEDIT, string SALERT, string SORDER, string SSIZE, string SCFORM, string Filter)
        {

            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(ArchiveSqlConfig.Instance.GetTemplate_TB_HRTMPS_Add());
                param[0].Value = TID;
                param[1].Value = SID;
                param[2].Value = SDESC;
                param[3].Value = SDESCCHS;
                param[4].Value = SSLCT;
                param[5].Value = SREF;
                param[6].Value = SADD;
                param[7].Value = SDEL;
                param[8].Value = SEDIT;
                param[9].Value = SALERT;
                param[10].Value = SORDER;
                param[11].Value = SSIZE;
                param[12].Value = SCFORM;
                param[13].Value = Filter;
                return dao.ExecuteNonQuery(ArchiveSqlConfig.Instance.GetTemplate_TB_HRTMPS_Add(), param);
            }

        }
        #endregion

        #region 获得档案模板行为
        /// <summary>
        /// 获得档案模板行为
        /// </summary>
        /// <param name="TID"></param>
        /// <returns></returns>
        public static DataTable GetTemplateAction(string TID)
        {
            DataTable dt = null;
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(ArchiveSqlConfig.Instance.GetTemplate_TB_HRTMPA());
                param[0].Value = TID;
                DataSet ds = dao.ExecuteDataSet(ArchiveSqlConfig.Instance.GetTemplate_TB_HRTMPA(), param);
                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                }
            }
            return dt;
        }
        #endregion

        #region 更新档案模板行为
        /// <summary>
        /// 更新档案模板行为
        /// </summary>
        /// <param name="TID"></param>
        /// <param name="AID"></param>
        /// <param name="ADESC"></param>
        /// <param name="ADESCCHS"></param>
        /// <param name="ATYPE"></param>
        /// <param name="ADETAIL"></param>
        /// <param name="APARA0"></param>
        /// <param name="APARA1"></param>
        /// <param name="APARA2"></param>
        /// <param name="APARA3"></param>
        /// <param name="APARA4"></param>
        /// <param name="APARA5"></param>
        /// <param name="APARA6"></param>
        /// <param name="APARA7"></param>
        /// <param name="APARA8"></param>
        /// <param name="APARA9"></param>
        /// <param name="ALOCATION"></param>
        /// <param name="AORDER"></param>
        /// <param name="EXCEPTIONS"></param>
        /// <param name="ATYPEY"></param>
        /// <param name="ADETAILY"></param>
        /// <param name="APARAY0"></param>
        /// <param name="APARAY1"></param>
        /// <param name="APARAY2"></param>
        /// <param name="APARAY3"></param>
        /// <param name="APARAY4"></param>
        /// <param name="APARAY5"></param>
        /// <param name="APARAY6"></param>
        /// <param name="APARAY7"></param>
        /// <param name="APARAY8"></param>
        /// <param name="APARAY9"></param>
        /// <param name="ATYPEN"></param>
        /// <param name="ADETAILN"></param>
        /// <param name="APARAN0"></param>
        /// <param name="APARAN1"></param>
        /// <param name="APARAN2"></param>
        /// <param name="APARAN3"></param>
        /// <param name="APARAN4"></param>
        /// <param name="APARAN5"></param>
        /// <param name="APARAN6"></param>
        /// <param name="APARAN7"></param>
        /// <param name="APARAN8"></param>
        /// <param name="APARAN9"></param>
        /// <param name="MOVENEXT"></param>
        /// <returns></returns>
        public static int UpdateTemplateAction(string TID, string AID, string ADESC, string ADESCCHS, string ATYPE, string ADETAIL, string APARA0, string APARA1, string APARA2, string APARA3, string APARA4, string APARA5, string APARA6, string APARA7, string APARA8, string APARA9, string ALOCATION, string AORDER, string EXCEPTIONS, string ATYPEY, string ADETAILY, string APARAY0, string APARAY1, string APARAY2, string APARAY3, string APARAY4, string APARAY5, string APARAY6, string APARAY7, string APARAY8, string APARAY9, string ATYPEN, string ADETAILN, string APARAN0, string APARAN1, string APARAN2, string APARAN3, string APARAN4, string APARAN5, string APARAN6, string APARAN7, string APARAN8, string APARAN9, string MOVENEXT)
        {

            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(ArchiveSqlConfig.Instance.GetTemplate_TB_HRTMPA_Update());
                param[0].Value = ADESC;
                param[1].Value = ADESCCHS;
                param[2].Value = ATYPE;
                param[3].Value = ADETAIL;
                param[4].Value = APARA0;
                param[5].Value = APARA1;
                param[6].Value = APARA2;
                param[7].Value = APARA3;
                param[8].Value = APARA4;
                param[9].Value = APARA5;
                param[10].Value = APARA6;
                param[11].Value = APARA7;
                param[12].Value = APARA8;
                param[13].Value = APARA9;
                param[14].Value = ALOCATION;
                param[15].Value = AORDER;
                param[16].Value = EXCEPTIONS;
                param[17].Value = ATYPEY;
                param[18].Value = ADETAILY;
                param[19].Value = APARAY0;
                param[20].Value = APARAY1;
                param[21].Value = APARAY2;
                param[22].Value = APARAY3;
                param[23].Value = APARAY4;
                param[24].Value = APARAY5;
                param[25].Value = APARAY6;
                param[26].Value = APARAY7;
                param[27].Value = APARAY8;
                param[28].Value = APARAY9;
                param[29].Value = ATYPEN;
                param[30].Value = ADETAILN;
                param[31].Value = APARAN0;
                param[32].Value = APARAN1;
                param[33].Value = APARAN2;
                param[34].Value = APARAN3;
                param[35].Value = APARAN4;
                param[36].Value = APARAN5;
                param[37].Value = APARAN6;
                param[38].Value = APARAN7;
                param[39].Value = APARAN8;
                param[40].Value = APARAN9;
                param[41].Value = MOVENEXT;
                param[42].Value = AID;
                param[43].Value = TID;
                return dao.ExecuteNonQuery(ArchiveSqlConfig.Instance.GetTemplate_TB_HRTMPA_Update(), param);
            }

        }
        #endregion

        #region 删除档案模板行为
        /// <summary>
        /// 删除档案模板行为
        /// </summary>
        /// <param name="AID"></param>
        /// <param name="TID"></param>
        /// <returns></returns>
        public static int DeleteTemplateAction(string AID, string TID)
        {

            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(ArchiveSqlConfig.Instance.GetTemplate_TB_HRTMPA_Delete());
                param[0].Value = AID;
                param[1].Value = TID;
                return dao.ExecuteNonQuery(ArchiveSqlConfig.Instance.GetTemplate_TB_HRTMPA_Delete(), param);
            }

        }
        #endregion

        #region 增加档案模板行为
        /// <summary>
        /// 增加档案模板行为
        /// </summary>
        /// <param name="TID"></param>
        /// <param name="AID"></param>
        /// <param name="ADESC"></param>
        /// <param name="ADESCCHS"></param>
        /// <param name="ATYPE"></param>
        /// <param name="ADETAIL"></param>
        /// <param name="APARA0"></param>
        /// <param name="APARA1"></param>
        /// <param name="APARA2"></param>
        /// <param name="APARA3"></param>
        /// <param name="APARA4"></param>
        /// <param name="APARA5"></param>
        /// <param name="APARA6"></param>
        /// <param name="APARA7"></param>
        /// <param name="APARA8"></param>
        /// <param name="APARA9"></param>
        /// <param name="ALOCATION"></param>
        /// <param name="AORDER"></param>
        /// <param name="EXCEPTIONS"></param>
        /// <param name="ATYPEY"></param>
        /// <param name="ADETAILY"></param>
        /// <param name="APARAY0"></param>
        /// <param name="APARAY1"></param>
        /// <param name="APARAY2"></param>
        /// <param name="APARAY3"></param>
        /// <param name="APARAY4"></param>
        /// <param name="APARAY5"></param>
        /// <param name="APARAY6"></param>
        /// <param name="APARAY7"></param>
        /// <param name="APARAY8"></param>
        /// <param name="APARAY9"></param>
        /// <param name="ATYPEN"></param>
        /// <param name="ADETAILN"></param>
        /// <param name="APARAN0"></param>
        /// <param name="APARAN1"></param>
        /// <param name="APARAN2"></param>
        /// <param name="APARAN3"></param>
        /// <param name="APARAN4"></param>
        /// <param name="APARAN5"></param>
        /// <param name="APARAN6"></param>
        /// <param name="APARAN7"></param>
        /// <param name="APARAN8"></param>
        /// <param name="APARAN9"></param>
        /// <param name="MOVENEXT"></param>
        /// <returns></returns>
        public static int AddTemplateAction(string TID, string AID, string ADESC, string ADESCCHS, string ATYPE, string ADETAIL, string APARA0, string APARA1, string APARA2, string APARA3, string APARA4, string APARA5, string APARA6, string APARA7, string APARA8, string APARA9, string ALOCATION, string AORDER, string EXCEPTIONS, string ATYPEY, string ADETAILY, string APARAY0, string APARAY1, string APARAY2, string APARAY3, string APARAY4, string APARAY5, string APARAY6, string APARAY7, string APARAY8, string APARAY9, string ATYPEN, string ADETAILN, string APARAN0, string APARAN1, string APARAN2, string APARAN3, string APARAN4, string APARAN5, string APARAN6, string APARAN7, string APARAN8, string APARAN9, string MOVENEXT)
        {

            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(ArchiveSqlConfig.Instance.GetTemplate_TB_HRTMPA_Add());
                param[0].Value = TID;
                param[1].Value = AID;
                param[2].Value = ADESC;
                param[3].Value = ADESCCHS;
                param[4].Value = ATYPE;
                param[5].Value = ADETAIL;
                param[6].Value = APARA0;
                param[7].Value = APARA1;
                param[8].Value = APARA2;
                param[9].Value = APARA3;
                param[10].Value = APARA4;
                param[11].Value = APARA5;
                param[12].Value = APARA6;
                param[13].Value = APARA7;
                param[14].Value = APARA8;
                param[15].Value = APARA9;
                param[16].Value = ALOCATION;
                param[17].Value = AORDER;
                param[18].Value = EXCEPTIONS;
                param[19].Value = ATYPEY;
                param[20].Value = ADETAILY;
                param[21].Value = APARAY0;
                param[22].Value = APARAY1;
                param[23].Value = APARAY2;
                param[24].Value = APARAY3;
                param[25].Value = APARAY4;
                param[26].Value = APARAY5;
                param[27].Value = APARAY6;
                param[28].Value = APARAY7;
                param[29].Value = APARAY8;
                param[30].Value = APARAY9;
                param[31].Value = ATYPEN;
                param[32].Value = ADETAILN;
                param[33].Value = APARAN0;
                param[34].Value = APARAN1;
                param[35].Value = APARAN2;
                param[36].Value = APARAN3;
                param[37].Value = APARAN4;
                param[38].Value = APARAN5;
                param[39].Value = APARAN6;
                param[40].Value = APARAN7;
                param[41].Value = APARAN8;
                param[42].Value = APARAN9;
                param[43].Value = MOVENEXT;
                return dao.ExecuteNonQuery(ArchiveSqlConfig.Instance.GetTemplate_TB_HRTMPA_Add(), param);
            }

        }
        #endregion

        #region 获得档案模板事件
        /// <summary>
        /// 获得档案模板事件
        /// </summary>
        /// <param name="TID"></param>
        /// <returns></returns>
        public static DataTable GetTemplateEvent(string TID)
        {
            DataTable dt = null;
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(ArchiveSqlConfig.Instance.GetTemplate_TB_HRTMPE());
                param[0].Value = TID;
                DataSet ds = dao.ExecuteDataSet(ArchiveSqlConfig.Instance.GetTemplate_TB_HRTMPE(), param);
                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                }
            }
            return dt;
        }
        #endregion

        #region 更新档案模板事件
        /// <summary>
        /// 更新档案模板事件
        /// </summary>
        /// <param name="TID"></param>
        /// <param name="EID"></param>
        /// <param name="EDESC"></param>
        /// <param name="EDESCCHS"></param>
        /// <param name="GID"></param>
        /// <param name="PID"></param>
        /// <param name="ENAME"></param>
        /// <param name="ECONT"></param>
        /// <returns></returns>
        public static int UpdateTemplateEvent(string TID, string EID, string EDESC, string EDESCCHS, string GID, string PID, string ENAME, string ECONT)
        {

            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(ArchiveSqlConfig.Instance.GetTemplate_TB_HRTMPE_Update());
                param[0].Value = EDESC;
                param[1].Value = EDESCCHS;
                param[2].Value = GID;
                param[3].Value = PID;
                param[4].Value = ENAME;
                param[5].Value = ECONT;
                param[6].Value = EID;
                param[7].Value = TID;
                return dao.ExecuteNonQuery(ArchiveSqlConfig.Instance.GetTemplate_TB_HRTMPE_Update(), param);
            }

        }
        #endregion

        #region 删除档案模板事件
        /// <summary>
        /// 删除档案模板事件
        /// </summary>
        /// <param name="EID"></param>
        /// <param name="TID"></param>
        /// <returns></returns>
        public static int DeleteTemplateEvent(string EID, string TID)
        {

            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(ArchiveSqlConfig.Instance.GetTemplate_TB_HRTMPE_Delete());
                param[0].Value = EID;
                param[1].Value = TID;
                return dao.ExecuteNonQuery(ArchiveSqlConfig.Instance.GetTemplate_TB_HRTMPE_Delete(), param);
            }

        }
        #endregion

        #region 增加档案模板事件
        /// <summary>
        /// 增加档案模板事件
        /// </summary>
        /// <param name="TID"></param>
        /// <param name="EID"></param>
        /// <param name="EDESC"></param>
        /// <param name="EDESCCHS"></param>
        /// <param name="GID"></param>
        /// <param name="PID"></param>
        /// <param name="ENAME"></param>
        /// <param name="ECONT"></param>
        /// <returns></returns>
        public static int AddTemplateEvent(string TID, string EID, string EDESC, string EDESCCHS, string GID, string PID, string ENAME, string ECONT)
        {

            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(ArchiveSqlConfig.Instance.GetTemplate_TB_HRTMPE_Add());
                param[0].Value = TID;
                param[1].Value = EID;
                param[2].Value = EDESC;
                param[3].Value = EDESCCHS;
                param[4].Value = GID;
                param[5].Value = PID;
                param[6].Value = ENAME;
                param[7].Value = ECONT;
                return dao.ExecuteNonQuery(ArchiveSqlConfig.Instance.GetTemplate_TB_HRTMPE_Add(), param);
            }

        }
        #endregion

        #region 获得档案模板详情_D
        /// <summary>
        ///  获得档案模板详情_D
        /// </summary>
        /// <param name="TID"></param>
        /// <returns></returns>
        public static DataTable GetTemplateDetail_D(string TID, string GID)
        {
            DataTable dt = null;
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(ArchiveSqlConfig.Instance.GetTemplate_TB_HRTMPD_D());
                param[0].Value = TID;
                param[1].Value = GID;
                DataSet ds = dao.ExecuteDataSet(ArchiveSqlConfig.Instance.GetTemplate_TB_HRTMPD_D(), param);
                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                }
            }
            return dt;
        }
        #endregion

        #region 更新档案模板详情_D
        /// <summary>
        /// 更新档案模板详情_D
        /// </summary>
        /// <param name="TID"></param>
        /// <param name="GID"></param>
        /// <param name="PID"></param>
        /// <param name="PDESC"></param>
        /// <param name="PDESCCHS"></param>
        /// <param name="PTYPE"></param>
        /// <param name="PLEN"></param>
        /// <param name="PPREC"></param>
        /// <param name="PNULL"></param>
        /// <param name="PDEFAULT"></param>
        /// <param name="PISKEY"></param>
        /// <param name="PCTRL"></param>
        /// <param name="PCTRLID"></param>
        /// <param name="PCTRLD"></param>
        /// <param name="PORDER"></param>
        /// <param name="PRIGHT"></param>
        /// <param name="PSYS"></param>
        /// <param name="PLIST"></param>
        /// <param name="PWIDTH"></param>
        /// <param name="PFONTL"></param>
        /// <param name="PFONTC"></param>
        /// <param name="PAGGR"></param>
        /// <param name="PAGDEST"></param>
        /// <param name="PMAST"></param>
        /// <param name="PSAVE"></param>
        /// <returns></returns>
        public static int UpdateTemplateDetail_D(string TID, string GID, string PID, string PDESC, string PDESCCHS, string PTYPE, string PLEN, string PPREC, string PNULL, string PDEFAULT, string PISKEY, string PCTRL, string PCTRLID, string PCTRLD, string PORDER, string PRIGHT, string PSYS, string PLIST, string PWIDTH, string PFONTL, string PFONTC, string PAGGR, string PAGDEST, string PMAST, string PSAVE)
        {

            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(ArchiveSqlConfig.Instance.GetTemplate_TB_HRTMPD_D_Update());
                param[0].Value = PDESC;
                param[1].Value = PDESCCHS;
                param[2].Value = PTYPE;
                param[3].Value = PLEN;
                param[4].Value = PPREC;
                param[5].Value = PNULL;
                param[6].Value = PDEFAULT;
                param[7].Value = PISKEY;
                param[8].Value = PCTRL;
                param[9].Value = PCTRLID;
                param[10].Value = PCTRLD;
                param[11].Value = PORDER;
                param[12].Value = PRIGHT;
                param[13].Value = PSYS;
                param[14].Value = PLIST;
                param[15].Value = PWIDTH;
                param[16].Value = PFONTL;
                param[17].Value = PFONTC;
                param[18].Value = PAGGR;
                param[19].Value = PAGDEST;
                param[20].Value = PMAST;
                param[21].Value = PSAVE;
                param[22].Value = PID;
                param[23].Value = GID;
                param[24].Value = TID;
                return dao.ExecuteNonQuery(ArchiveSqlConfig.Instance.GetTemplate_TB_HRTMPD_D_Update(), param);
            }

        }
        #endregion

        #region 删除档案模板详情_D
        /// <summary>
        /// 删除档案模板详情_D
        /// </summary>
        /// <param name="PID"></param>
        /// <param name="GID"></param>
        /// <param name="TID"></param>
        /// <returns></returns>
        public static int DeleteTemplateDetail_d(string PID, string GID, string TID)
        {

            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(ArchiveSqlConfig.Instance.GetTemplate_TB_HRTMPD_D_Delete());
                param[0].Value = PID;
                param[1].Value = GID;
                param[2].Value = TID;
                return dao.ExecuteNonQuery(ArchiveSqlConfig.Instance.GetTemplate_TB_HRTMPD_D_Delete(), param);
            }

        }
        #endregion

        #region 增加档案模板详情_D
        /// <summary>
        /// 增加档案模板详情_D
        /// </summary>
        /// <param name="TID"></param>
        /// <param name="GID"></param>
        /// <param name="PID"></param>
        /// <param name="PDESC"></param>
        /// <param name="PDESCCHS"></param>
        /// <param name="PTYPE"></param>
        /// <param name="PLEN"></param>
        /// <param name="PPREC"></param>
        /// <param name="PNULL"></param>
        /// <param name="PDEFAULT"></param>
        /// <param name="PISKEY"></param>
        /// <param name="PCTRL"></param>
        /// <param name="PCTRLID"></param>
        /// <param name="PCTRLD"></param>
        /// <param name="PORDER"></param>
        /// <param name="PRIGHT"></param>
        /// <param name="PSYS"></param>
        /// <param name="PLIST"></param>
        /// <param name="PWIDTH"></param>
        /// <param name="PFONTL"></param>
        /// <param name="PFONTC"></param>
        /// <param name="PAGGR"></param>
        /// <param name="PAGDEST"></param>
        /// <param name="PMAST"></param>
        /// <param name="PSAVE"></param>
        /// <returns></returns>
        public static int AddTemplateDetail_D(string TID, string GID, string PID, string PDESC, string PDESCCHS, string PTYPE, string PLEN, string PPREC, string PNULL, string PDEFAULT, string PISKEY, string PCTRL, string PCTRLID, string PCTRLD, string PORDER, string PRIGHT, string PSYS, string PLIST, string PWIDTH, string PFONTL, string PFONTC, string PAGGR, string PAGDEST, string PMAST, string PSAVE)
        {

            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(ArchiveSqlConfig.Instance.GetTemplate_TB_HRTMPD_D_Add());
                param[0].Value = TID;
                param[1].Value = GID;
                param[2].Value = PID;
                param[3].Value = PDESC;
                param[4].Value = PDESCCHS;
                param[5].Value = PTYPE;
                param[6].Value = PLEN;
                param[7].Value = PPREC;
                param[8].Value = PNULL;
                param[9].Value = PDEFAULT;
                param[10].Value = PISKEY;
                param[11].Value = PCTRL;
                param[12].Value = PCTRLID;
                param[13].Value = PCTRLD;
                param[14].Value = PORDER;
                param[15].Value = PRIGHT;
                param[16].Value = PSYS;
                param[17].Value = PLIST;
                param[18].Value = PWIDTH;
                param[19].Value = PFONTL;
                param[20].Value = PFONTC;
                param[21].Value = PAGGR;
                param[22].Value = PAGDEST;
                param[23].Value = PMAST;
                param[24].Value = PSAVE;
                return dao.ExecuteNonQuery(ArchiveSqlConfig.Instance.GetTemplate_TB_HRTMPD_D_Add(), param);
            }

        }
        #endregion

        #region 档案模板系统参数－D
        /// <summary>
        ///  档案模板系统参数－D
        /// </summary>
        /// <returns></returns>
        public static DataTable GetTemplate_TB_HRTMPDSys_D(string language)
        {
            DataTable dt = null;
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(ArchiveSqlConfig.Instance.GetTemplate_TB_HRTMPDSys_D());
                param[0].Value = language;
                DataSet ds = dao.ExecuteDataSet(ArchiveSqlConfig.Instance.GetTemplate_TB_HRTMPDSys_D(), param);
                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                }
            }
            return dt;
        }
        #endregion

        #region 档案模板系统字体－D
        /// <summary>
        ///  档案模板系统字体－D
        /// </summary>
        /// <returns></returns>
        public static DataTable GetTemplate_TB_HRTMPDFont_D(string language)
        {
            DataTable dt = null;
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(ArchiveSqlConfig.Instance.GetTemplate_TB_HRTMPDFont_D());
                param[0].Value = language;
                DataSet ds = dao.ExecuteDataSet(ArchiveSqlConfig.Instance.GetTemplate_TB_HRTMPDFont_D(), param);
                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                }
            }
            return dt;
        }
        #endregion

        #region 票据获得档案模板详情定义
        /// <summary>
        ///  票据获得档案模板详情定义
        /// </summary>
        /// <returns></returns>
        public static DataTable GetTemplate_TB_DocumentTemplateDefine(string TID, string GID)
        {
            DataTable dt = null;
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(ArchiveSqlConfig.Instance.GetTemplate_TB_DocumentTemplateDefine());
                param[0].Value = TID;
                param[1].Value = GID;
                DataSet ds = dao.ExecuteDataSet(ArchiveSqlConfig.Instance.GetTemplate_TB_DocumentTemplateDefine(), param);
                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                }
            }
            return dt;
        }
        #endregion

        #region 票据获得档案模板SA
        /// <summary>
        ///  票据获得档案模板SA
        /// </summary>
        /// <returns></returns>
        public static DataTable GetTemplate_TB_HRTMPSA_Document(string TID, string SID)
        {
            DataTable dt = null;
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(ArchiveSqlConfig.Instance.GetTemplate_TB_HRTMPSA_Document());
                param[0].Value = TID;
                param[1].Value = SID;
                DataSet ds = dao.ExecuteDataSet(ArchiveSqlConfig.Instance.GetTemplate_TB_HRTMPSA_Document(), param);
                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                }
            }
            return dt;
        }
        #endregion

        #region 票据获得档案模板AR
        /// <summary>
        ///  票据获得档案模板AR
        /// </summary>
        /// <returns></returns>
        public static DataTable GetTemplate_TB_HRTMPAR_Document(string TID, string AID, string ECFROM, string language)
        {
            DataTable dt = null;
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(ArchiveSqlConfig.Instance.GetTemplate_TB_HRTMPAR_Document());
                param[0].Value = language;
                param[1].Value = TID;
                param[2].Value = AID;
                param[3].Value = ECFROM;
                DataSet ds = dao.ExecuteDataSet(ArchiveSqlConfig.Instance.GetTemplate_TB_HRTMPAR_Document(), param);
                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                }
            }
            return dt;
        }
        #endregion

        #region 票据获得档案模板SG
        /// <summary>
        ///  票据获得档案模板SG
        /// </summary>
        /// <returns></returns>
        public static DataTable GetTemplate_TB_HRTMPSG_Document(string TID, string SID, string GID)
        {
            DataTable dt = null;
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(ArchiveSqlConfig.Instance.GetTemplate_TB_HRTMPSG_Document());
                param[0].Value = TID;
                param[1].Value = SID;
                param[2].Value = GID;
                DataSet ds = dao.ExecuteDataSet(ArchiveSqlConfig.Instance.GetTemplate_TB_HRTMPSG_Document(), param);
                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                }
            }
            return dt;
        }
        #endregion

        #region 票据获得档案模板SD
        /// <summary>
        ///  票据获得档案模板SD
        /// </summary>
        /// <returns></returns>
        public static DataTable GetTemplate_TB_HRTMPSD_Document(string TID, string SID, string GID)
        {
            DataTable dt = null;
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(ArchiveSqlConfig.Instance.GetTemplate_TB_HRTMPSD_Document());
                param[0].Value = TID;
                param[1].Value = SID;
                param[2].Value = GID;
                DataSet ds = dao.ExecuteDataSet(ArchiveSqlConfig.Instance.GetTemplate_TB_HRTMPSD_Document(), param);
                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                }
            }
            return dt;
        }
        #endregion

    }
}
