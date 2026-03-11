using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ValuePlus.Entity.Report
{
    /// <summary>
    /// 报表参数xml文件实体类（包含通过这些参数获取到的相关信息）
    /// </summary>
    [Serializable]
    public class ReportXmlEntity
    {
        //xml文件中涉及的字段
        private string _alias;
        private string _tid;
        private string _gid;
        private string _sid;
        private string _pid;

        //通过这些参数获取到的相关信息
        private string _PID;//显示控件ID
        private string _PCTRLTYPE;//显示控件类型
        
        private string _PDESCCHS;//中文名描述
        private string _PDESC;//英文名描述
        private string _PCTRLID;//控件关键字段
        private string _PCTRLD;//控件内容语句

        private string _PDATATYPE;//字段类型
        private string _PDEFAULT;//字段默认值

        /// <summary>
        /// _aliasid
        /// </summary>
        public string strAlias
        {
            get { return _alias; }
            set { _alias = value; }
        }

        /// <summary>
        /// _tid
        /// </summary>
        public string strTid
        {
            get { return _tid; }
            set { _tid = value; }
        }

        /// <summary>
        /// _gid
        /// </summary>
        public string strGid
        {
            get { return _gid; }
            set { _gid = value; }
        }

        /// <summary>
        /// _sid
        /// </summary>
        public string strSid
        {
            get { return _sid; }
            set { _sid = value; }
        }

        /// <summary>
        /// _pid
        /// </summary>
        public string strPid
        {
            get { return _pid; }
            set { _pid = value; }
        }

        /// <summary>
        /// _PID
        /// </summary>
        public string strPID
        {
            get { return _PID; }
            set { _PID = value; }
        }

        /// <summary>
        /// _PCTRLTYPE
        /// </summary>
        public string strPCTRLTYPE
        {
            get { return _PCTRLTYPE; }
            set { _PCTRLTYPE = value; }
        }

        /// <summary>
        /// _PDESCCHS
        /// </summary>
        public string strPDESCCHS
        {
            get { return _PDESCCHS; }
            set { _PDESCCHS = value; }
        }

        /// <summary>
        /// _PDESC
        /// </summary>
        public string strPDESC
        {
            get { return _PDESC; }
            set { _PDESC = value; }
        }

        /// <summary>
        /// _PCTRLID
        /// </summary>
        public string strPCTRLID
        {
            get { return _PCTRLID; }
            set { _PCTRLID = value; }
        }

        /// <summary>
        /// _PCTRLD
        /// </summary>
        public string strPCTRLSQL
        {
            get { return _PCTRLD; }
            set { _PCTRLD = value; }
        }

        /// <summary>
        /// _PDATATYPE
        /// </summary>
        public string strPDATATYPE
        {
            get { return _PDATATYPE; }
            set { _PDATATYPE = value; }
        }

        /// <summary>
        /// _PDEFAULT
        /// </summary>
        public string strPDEFAULT
        {
            get { return _PDEFAULT; }
            set { _PDEFAULT = value; }
        }

    }
}
