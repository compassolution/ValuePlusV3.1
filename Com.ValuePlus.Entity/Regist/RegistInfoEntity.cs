using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ValuePlus.Entity.Regist
{
    /// <summary>
    /// 软件注册授权信息实体类（包含通过这些参数获取到的相关信息）
    /// </summary>
    [Serializable]
    public class RegistInfoEntity
    {
        private string _SKEY; 
        private string _SCLIENTNAME; 
        private string _SCONTACTOR; 
        private string _SCONTACTWAY; 
        private string _SGROUPNAME; 
        private string _SREGISTSTR;
        private string _SASSIGNSTR;
        private DateTime _DTREGISTDATA;
        private string _SREQUESTIP;

        /// <summary>
        /// _SKEY
        /// </summary>
        public string strSKEY
        {
            get { return _SKEY; }
            set { _SKEY = value; }
        }

        /// <summary>
        /// _SCLIENTNAME
        /// </summary>
        public string strSCLIENTNAME
        {
            get { return _SCLIENTNAME; }
            set { _SCLIENTNAME = value; }
        }

        /// <summary>
        /// _SCONTACTOR
        /// </summary>
        public string strSCONTACTOR
        {
            get { return _SCONTACTOR; }
            set { _SCONTACTOR = value; }
        }

        /// <summary>
        /// _SCONTACTWAY
        /// </summary>
        public string strSCONTACTWAY
        {
            get { return _SCONTACTWAY; }
            set { _SCONTACTWAY = value; }
        }

        /// <summary>
        /// _SGROUPNAME
        /// </summary>
        public string strSGROUPNAME
        {
            get { return _SGROUPNAME; }
            set { _SGROUPNAME = value; }
        }

        /// <summary>
        /// _SREGISTSTR
        /// </summary>
        public string strSREGISTSTR
        {
            get { return _SREGISTSTR; }
            set { _SREGISTSTR = value; }
        }

        /// <summary>
        /// _SASSIGNSTR
        /// </summary>
        public string strSASSIGNSTR
        {
            get { return _SASSIGNSTR; }
            set { _SASSIGNSTR = value; }
        }

        /// <summary>
        /// _DTREGISTDATA
        /// </summary>
        public DateTime dtDTREGISTDATA
        {
            get { return _DTREGISTDATA; }
            set { _DTREGISTDATA = value; }
        }

        /// <summary>
        /// _SREQUESTIP
        /// </summary>
        public string strSREQUESTIP
        {
            get { return _SREQUESTIP; }
            set { _SREQUESTIP = value; }
        }

    }
}
