using System;
using System.Text;

namespace Com.ValuePlus.DataLog.Entity
{
    /// <summary>
    /// 数据表HRLOG_2实体类
    /// </summary>
    [Serializable]
    public class Entity_HRLOG_2
    {
        private string  _CID;
        private int?  _LNO;
        private string  _TID;
        private string  _GID;
        private string  _PID;
        private string  _POVAL;
        private string  _PNVAL;

        /// <summary>
        /// 对属性CID的读写
        /// </summary>
        public string  CID
        {
            get{return _CID;}
            set{_CID=value;}
        }
 
        /// <summary>
        /// 对属性LNO的读写
        /// </summary>
        public int?  LNO
        {
            get{return _LNO;}
            set{_LNO=value;}
        }
 
        /// <summary>
        /// 对属性TID的读写
        /// </summary>
        public string  TID
        {
            get{return _TID;}
            set{_TID=value;}
        }
 
        /// <summary>
        /// 对属性GID的读写
        /// </summary>
        public string  GID
        {
            get{return _GID;}
            set{_GID=value;}
        }
 
        /// <summary>
        /// 对属性PID的读写
        /// </summary>
        public string  PID
        {
            get{return _PID;}
            set{_PID=value;}
        }
 
        /// <summary>
        /// 对属性POVAL的读写
        /// </summary>
        public string  POVAL
        {
            get{return _POVAL;}
            set{_POVAL=value;}
        }
 
        /// <summary>
        /// 对属性PNVAL的读写
        /// </summary>
        public string  PNVAL
        {
            get{return _PNVAL;}
            set{_PNVAL=value;}
        }
 
    }
}

