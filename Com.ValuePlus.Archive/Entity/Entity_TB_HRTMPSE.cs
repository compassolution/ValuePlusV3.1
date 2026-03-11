using System;
using System.Text;

namespace Com.ValuePlus.Archive.Entity
{
    /// <summary>
    /// 数据表TB_HRTMPSE实体类
    /// </summary>
    [Serializable]
    public class Entity_TB_HRTMPSE
    {
        private string  _TID;
        private string  _SID;
        private string  _EID;
        private string  _EDESC;
        private string  _EDESCCHS;
        private string  _GID;
        private string  _PID;
        private string  _ENAME;
        private string  _ECONT;
        private int?  _ERIGHT;

        /// <summary>
        /// 对属性TID的读写
        /// </summary>
        public string  TID
        {
            get{return _TID;}
            set{_TID=value;}
        }
 
        /// <summary>
        /// 对属性SID的读写
        /// </summary>
        public string  SID
        {
            get{return _SID;}
            set{_SID=value;}
        }
 
        /// <summary>
        /// 对属性EID的读写
        /// </summary>
        public string  EID
        {
            get{return _EID;}
            set{_EID=value;}
        }
 
        /// <summary>
        /// 对属性EDESC的读写
        /// </summary>
        public string  EDESC
        {
            get{return _EDESC;}
            set{_EDESC=value;}
        }
 
        /// <summary>
        /// 对属性EDESCCHS的读写
        /// </summary>
        public string  EDESCCHS
        {
            get{return _EDESCCHS;}
            set{_EDESCCHS=value;}
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
        /// 对属性ENAME的读写
        /// </summary>
        public string  ENAME
        {
            get{return _ENAME;}
            set{_ENAME=value;}
        }
 
        /// <summary>
        /// 对属性ECONT的读写
        /// </summary>
        public string  ECONT
        {
            get{return _ECONT;}
            set{_ECONT=value;}
        }
 
        /// <summary>
        /// 对属性ERIGHT的读写
        /// </summary>
        public int?  ERIGHT
        {
            get{return _ERIGHT;}
            set{_ERIGHT=value;}
        }
 
    }
}

