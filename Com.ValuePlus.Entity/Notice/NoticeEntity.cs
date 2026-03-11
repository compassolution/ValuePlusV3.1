using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ValuePlus.Entity.Notice
{
    
    /// <summary>
    /// 公告信息实体类
    /// </summary>
    [Serializable]
    public class NoticeEntity
    {
        private string _SKEY;
        private string _STITLE;
        private string _SCONTENT;
        private string _SPUBLISHOR;
        private DateTime _DTPUBLISHTIME;
        private string _SSOURCE;
        private string _SEDITOR;
        private DateTime _DTEDITTIME;
        private string _BISSTOP;

        /// <summary>
        /// _SKEY
        /// </summary>
        public string strSKEY
        {
            get { return _SKEY; }
            set { _SKEY = value; }
        }

        /// <summary>
        /// _STITLE
        /// </summary>
        public string strSTITLE
        {
            get { return _STITLE; }
            set { _STITLE = value; }
        }

        /// <summary>
        /// _SCONTENT
        /// </summary>
        public string strSCONTENT
        {
            get { return _SCONTENT; }
            set { _SCONTENT = value; }
        }

        /// <summary>
        /// _SPUBLISHOR
        /// </summary>
        public string strSPUBLISHOR
        {
            get { return _SPUBLISHOR; }
            set { _SPUBLISHOR = value; }
        }

        /// <summary>
        /// _DTPUBLISHTIME
        /// </summary>
        public DateTime dtDTPUBLISHTIME
        {
            get { return _DTPUBLISHTIME; }
            set { _DTPUBLISHTIME = value; }
        }

        /// <summary>
        /// _SSOURCE
        /// </summary>
        public string strSSOURCE
        {
            get { return _SSOURCE; }
            set { _SSOURCE = value; }
        }


        /// <summary>
        /// _SEDITOR
        /// </summary>
        public string strSEDITOR
        {
            get { return _SEDITOR; }
            set { _SEDITOR = value; }
        }

        /// <summary>
        /// _DTEDITTIME
        /// </summary>
        public DateTime dtDTEDITTIME
        {
            get { return _DTEDITTIME; }
            set { _DTEDITTIME = value; }
        }

        /// <summary>
        /// _BISSTOP
        /// </summary>
        public string strBISSTOP
        {
            get { return _BISSTOP; }
            set { _BISSTOP = value; }
        }
    }
}
