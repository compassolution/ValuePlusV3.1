using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ValuePlus.Entity.Regist
{
    /// <summary>
    /// 软件授权文件信息实体类
    /// </summary>
    [Serializable]
    public class LicenseEntity
    {
        private string _SCLIENTNAME;
        private string _SCLIENTNAMECHS;
        private string _SPRODUCTORNAME;
        private string _SVERSION;
        private string _MACHINE;
        private DateTime _DTVALID;


        /// <summary>
        /// _SCLIENTNAME
        /// </summary>
        public string strClientName
        {
            get { return _SCLIENTNAME; }
            set { _SCLIENTNAME = value; }
        }

        /// <summary>
        /// _SCLIENTNAMECHS
        /// </summary>
        public string strClientNameChs
        {
            get { return _SCLIENTNAMECHS; }
            set { _SCLIENTNAMECHS = value; }
        }

        /// <summary>
        /// _SPRODUCTORNAME
        /// </summary>
        public string strProductName
        {
            get { return _SPRODUCTORNAME; }
            set { _SPRODUCTORNAME = value; }
        }

        /// <summary>
        /// _SVERSION
        /// </summary>
        public string strVersion
        {
            get { return _SVERSION; }
            set { _SVERSION = value; }
        }

        /// <summary>
        /// _MACHINE
        /// </summary>
        public string strMachine
        {
            get { return _MACHINE; }
            set { _MACHINE = value; }
        }

        /// <summary>
        /// _DTREGISTDATA
        /// </summary>
        public DateTime dtValid
        {
            get { return _DTVALID; }
            set { _DTVALID = value; }
        }

    }

}
