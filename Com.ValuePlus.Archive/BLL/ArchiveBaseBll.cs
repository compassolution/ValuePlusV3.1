using System;
using System.Text;
using System.Data;
using System.Collections;
using Com.ValuePlus.Archive.DAL;

namespace Com.ValuePlus.Archive.BLL
{
    public class ArchiveBaseBll
    {

        #region 获得档案模板列表
        /// <summary>
        /// 获得档案模板列表
        /// </summary>
        /// <returns></returns>
        public static DataTable GetTemplateList()
        {
            return ArchiveBaseDao.GetTemplateList();
        }
        #endregion

        #region 更新档案模板列表
        /// <summary>
        /// 更新档案模板列表
        /// </summary>
        /// <returns></returns>
        public static int UpdateTemplate(string TID, string TDESC, string TDESCCHS, string TREC, string BTNSTANTION, string ROLEFILE, string BISSTOP)
        {

            return ArchiveBaseDao.UpdateTemplate(TID, TDESC, TDESCCHS, TREC, BTNSTANTION, ROLEFILE, BISSTOP);
        }
        #endregion

        #region 删除档案模板列表
        /// <summary>
        /// 删除档案模板列表
        /// </summary>
        /// <returns></returns>
        public static int DeleteTemplate(string TID)
        {

            return ArchiveBaseDao.DeleteTemplate(TID);

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

            return ArchiveBaseDao.AddTemplate(TID, TDESC, TDESCCHS, TREC, BTNSTANTION, ROLEFILE, BISSTOP);
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
            return ArchiveBaseDao.GetTemplateDetail(TID);
        }
        #endregion

        #region 更新档案模板详情
        /// <summary>
        /// 更新档案模板详情
        /// </summary>
        /// <returns></returns>
        public static int UpdateTemplateDetail(string TID, string GID, string GDESC, string GDESCCHS, string GTYPE, string GORDER, string GLIMIT, string GCOUNT, string GWIDTH, string GHIST, string GPAGE, string GRCOUNT, string GVIEW, string GSQL, string USERPAGE, string islarge)
        {

            return ArchiveBaseDao.UpdateTemplateDetail(TID, GID, GDESC, GDESCCHS, GTYPE, GORDER, GLIMIT, GCOUNT, GWIDTH, GHIST, GPAGE, GRCOUNT, GVIEW, GSQL, USERPAGE, islarge);
        }
        #endregion

        #region 删除档案模板详情
        /// <summary>
        ///删除档案模板详情 
        /// </summary>
        /// <param name="GID"></param>
        /// <returns></returns>
        public static int DeleteTemplateDetail(string GID, string TID)
        {

            return ArchiveBaseDao.DeleteTemplateDetail(GID, TID);
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

            return ArchiveBaseDao.AddTemplateDetail(TID, GID, GDESC, GDESCCHS, GTYPE, GORDER, GLIMIT, GCOUNT, GWIDTH, GHIST, GPAGE, GRCOUNT, GVIEW, GSQL, USERPAGE, islarge);
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
            return ArchiveBaseDao.GetTemplateRole(TID);
        }
        #endregion

        #region 更新档案模板角色
        /// <summary>
        /// 更新档案模板角色
        /// </summary>
        /// <returns></returns>
        public static int UpdateTemplateRole(string TID, string RID, string RDESC, string RDESCCHS, string RPARA0, string RPARA1, string RPARA2, string RPARA3, string RPARA4, string RPARA5, string RPARA6, string RPARA7, string RPARA8, string RPARA9, string RORDER)
        {

            return ArchiveBaseDao.UpdateTemplateRole(TID, RID, RDESC, RDESCCHS, RPARA0, RPARA1, RPARA2, RPARA3, RPARA4, RPARA5, RPARA6, RPARA7, RPARA8, RPARA9, RORDER);
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

            return ArchiveBaseDao.DeleteTemplateRole(RID, TID);
        }
        #endregion

        #region 增加档案模板角色
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
        public static int AddTemplateRole(string TID, string RID, string RDESC, string RDESCCHS, string RPARA0, string RPARA1, string RPARA2, string RPARA3, string RPARA4, string RPARA5, string RPARA6, string RPARA7, string RPARA8, string RPARA9, string RORDER)
        {

            return ArchiveBaseDao.AddTemplateRole(TID, RID, RDESC, RDESCCHS, RPARA0, RPARA1, RPARA2, RPARA3, RPARA4, RPARA5, RPARA6, RPARA7, RPARA8, RPARA9, RORDER);
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
            return ArchiveBaseDao.GetTemplateScene(TID);
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

            return ArchiveBaseDao.UpdateTemplateScene(TID, SID, SDESC, SDESCCHS, SSLCT, SREF, SADD, SDEL, SEDIT, SALERT, SORDER, SSIZE, SCFORM, Filter);
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

            return ArchiveBaseDao.DeleteTemplateScene(SID, TID);

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

            return ArchiveBaseDao.AddTemplateScene(TID, SID, SDESC, SDESCCHS, SSLCT, SREF, SADD, SDEL, SEDIT, SALERT, SORDER, SSIZE, SCFORM, Filter);

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
            return ArchiveBaseDao.GetTemplateAction(TID);
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

            return ArchiveBaseDao.UpdateTemplateAction(TID, AID, ADESC, ADESCCHS, ATYPE, ADETAIL, APARA0, APARA1, APARA2, APARA3, APARA4, APARA5, APARA6, APARA7, APARA8, APARA9, ALOCATION, AORDER, EXCEPTIONS, ATYPEY, ADETAILY, APARAY0, APARAY1, APARAY2, APARAY3, APARAY4, APARAY5, APARAY6, APARAY7, APARAY8, APARAY9, ATYPEN, ADETAILN, APARAN0, APARAN1, APARAN2, APARAN3, APARAN4, APARAN5, APARAN6, APARAN7, APARAN8, APARAN9, MOVENEXT);

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

            return ArchiveBaseDao.DeleteTemplateAction(AID, TID);

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

            return ArchiveBaseDao.AddTemplateAction(TID, AID, ADESC, ADESCCHS, ATYPE, ADETAIL, APARA0, APARA1, APARA2, APARA3, APARA4, APARA5, APARA6, APARA7, APARA8, APARA9, ALOCATION, AORDER, EXCEPTIONS, ATYPEY, ADETAILY, APARAY0, APARAY1, APARAY2, APARAY3, APARAY4, APARAY5, APARAY6, APARAY7, APARAY8, APARAY9, ATYPEN, ADETAILN, APARAN0, APARAN1, APARAN2, APARAN3, APARAN4, APARAN5, APARAN6, APARAN7, APARAN8, APARAN9, MOVENEXT);


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
            return ArchiveBaseDao.GetTemplateEvent(TID);
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

            return ArchiveBaseDao.UpdateTemplateEvent(TID, EID, EDESC, EDESCCHS, GID, PID, ENAME, ECONT);

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

            return ArchiveBaseDao.DeleteTemplateEvent(EID, TID);

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

            return ArchiveBaseDao.AddTemplateEvent(TID, EID, EDESC, EDESCCHS, GID, PID, ENAME, ECONT);

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
            return ArchiveBaseDao.GetTemplateDetail_D(TID, GID);
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

            return ArchiveBaseDao.UpdateTemplateDetail_D(TID, GID, PID, PDESC, PDESCCHS, PTYPE, PLEN, PPREC, PNULL, PDEFAULT, PISKEY, PCTRL, PCTRLID, PCTRLD, PORDER, PRIGHT, PSYS, PLIST, PWIDTH, PFONTL, PFONTC, PAGGR, PAGDEST, PMAST, PSAVE);

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

            return ArchiveBaseDao.DeleteTemplateDetail_d(PID, GID, TID);
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

            return ArchiveBaseDao.AddTemplateDetail_D(TID, GID, PID, PDESC, PDESCCHS, PTYPE, PLEN, PPREC, PNULL, PDEFAULT, PISKEY, PCTRL, PCTRLID, PCTRLD, PORDER, PRIGHT, PSYS, PLIST, PWIDTH, PFONTL, PFONTC, PAGGR, PAGDEST, PMAST, PSAVE);
        }
        #endregion

        #region 档案模板系统参数－D
        /// <summary>
        ///  档案模板系统参数－D
        /// </summary>
        /// <returns></returns>
        public static DataTable GetTemplate_TB_HRTMPDSys_D(string language)
        {
            return ArchiveBaseDao.GetTemplate_TB_HRTMPDSys_D(language);
        }
        #endregion

        #region 档案模板系统字体－D
        /// <summary>
        ///  档案模板系统字体－D
        /// </summary>
        /// <returns></returns>
        public static DataTable GetTemplate_TB_HRTMPDFont_D(string language)
        {
            return ArchiveBaseDao.GetTemplate_TB_HRTMPDFont_D(language);
        }
        #endregion

        #region 票据获得档案模板详情定义
        /// <summary>
        ///  票据获得档案模板详情定义
        /// </summary>
        /// <returns></returns>
        public static DataTable GetTemplate_TB_DocumentTemplateDefine(string TID, string GID)
        {
            return ArchiveBaseDao.GetTemplate_TB_DocumentTemplateDefine(TID, GID);
        }
        #endregion

        #region 票据获得档案模板SA
        /// <summary>
        ///  票据获得档案模板SA
        /// </summary>
        /// <returns></returns>
        public static DataTable GetTemplate_TB_HRTMPSA_Document(string TID, string SID)
        {
            return ArchiveBaseDao.GetTemplate_TB_HRTMPSA_Document(TID, SID);
        }
        #endregion

        #region 票据获得档案模板AR
        /// <summary>
        ///  票据获得档案模板AR
        /// </summary>
        /// <returns></returns>
        public static DataTable GetTemplate_TB_HRTMPAR_Document(string TID, string AID, string ECFROM, string language)
        {
            return ArchiveBaseDao.GetTemplate_TB_HRTMPAR_Document(TID, AID, ECFROM, language);
        }
        #endregion

        #region 票据获得档案模板SG
        /// <summary>
        ///  票据获得档案模板SG
        /// </summary>
        /// <returns></returns>
        public static DataTable GetTemplate_TB_HRTMPSG_Document(string TID, string SID, string GID)
        {
            return ArchiveBaseDao.GetTemplate_TB_HRTMPSG_Document(TID, SID, GID);
        }
        #endregion

        #region 票据获得档案模板SD
        /// <summary>
        ///  票据获得档案模板SD
        /// </summary>
        /// <returns></returns>
        public static DataTable GetTemplate_TB_HRTMPSD_Document(string TID, string SID, string GID)
        {
            return ArchiveBaseDao.GetTemplate_TB_HRTMPSD_Document(TID, SID, GID);
        }
        #endregion



    }
}
