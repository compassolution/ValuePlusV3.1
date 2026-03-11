using System;
using System.Text;

namespace Com.ValuePlus.DataLog.Entity
{
    /// <summary>
    /// 数据表HRLOG_1实体类
    /// </summary>
    [Serializable]
    public class Entity_HRLOG_1
    {
        private string  _CID;
        private string  _USERCODE;
        private DateTime?  _LOGTIME;
        private string  _TYPE;
        private string  _TYPED;
        private string  _REFER;
        private string  _ACTION;
        private string  _AKEY;
        private string  _DETAIL;
        private string  _RDETAIL;
        private string  _STATUS;

        /// <summary>
        /// 对属性CID的读写
        /// </summary>
        public string  CID
        {
            get{return _CID;}
            set{_CID=value;}
        }
 
        /// <summary>
        /// 对属性USERCODE的读写
        /// </summary>
        public string  USERCODE
        {
            get{return _USERCODE;}
            set{_USERCODE=value;}
        }
 
        /// <summary>
        /// 对属性LOGTIME的读写
        /// </summary>
        public DateTime?  LOGTIME
        {
            get{return _LOGTIME;}
            set{_LOGTIME=value;}
        }
 
        /// <summary>
        /// 对属性TYPE的读写
        /// </summary>
        public string  TYPE
        {
            get{return _TYPE;}
            set{_TYPE=value;}
        }
 
        /// <summary>
        /// 对属性TYPED的读写
        /// </summary>
        public string  TYPED
        {
            get{return _TYPED;}
            set{_TYPED=value;}
        }
 
        /// <summary>
        /// 对属性REFER的读写
        /// </summary>
        public string  REFER
        {
            get{return _REFER;}
            set{_REFER=value;}
        }
 
        /// <summary>
        /// 对属性ACTION的读写
        /// </summary>
        public string  ACTION
        {
            get{return _ACTION;}
            set{_ACTION=value;}
        }
 
        /// <summary>
        /// 对属性AKEY的读写
        /// </summary>
        public string  AKEY
        {
            get{return _AKEY;}
            set{_AKEY=value;}
        }
 
        /// <summary>
        /// 对属性DETAIL的读写
        /// </summary>
        public string  DETAIL
        {
            get{return _DETAIL;}
            set{_DETAIL=value;}
        }
 
        /// <summary>
        /// 对属性RDETAIL的读写
        /// </summary>
        public string  RDETAIL
        {
            get{return _RDETAIL;}
            set{_RDETAIL=value;}
        }
 
        /// <summary>
        /// 对属性STATUS的读写
        /// </summary>
        public string  STATUS
        {
            get{return _STATUS;}
            set{_STATUS=value;}
        }
 
    }
}

