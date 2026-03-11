using System;
using System.Text;

namespace Com.ValuePlus.Flow.Entity
{
    /// <summary>
    /// 数据表TB_FLOW_ENTITY实体类
    /// </summary>
    [Serializable]
    public class Entity_TB_FLOW_ENTITY
    {
        private string  _SENTITYID;
        private string  _SFORMID;
        private string  _SFORMKEYVALUE;
        private string  _SENTITYNAME;
        private string  _SENTITYNAMECN;
        private string  _SENTITYDESC;
        private string  _SENTITYDESCCN;

        /// <summary>
        /// 对属性SENTITYID的读写
        /// </summary>
        public string  SENTITYID
        {
            get{return _SENTITYID;}
            set{_SENTITYID=value;}
        }
 
        /// <summary>
        /// 对属性SFORMID的读写
        /// </summary>
        public string  SFORMID
        {
            get{return _SFORMID;}
            set{_SFORMID=value;}
        }
 
        /// <summary>
        /// 对属性SFORMKEYVALUE的读写
        /// </summary>
        public string  SFORMKEYVALUE
        {
            get{return _SFORMKEYVALUE;}
            set{_SFORMKEYVALUE=value;}
        }
 
        /// <summary>
        /// 对属性SENTITYNAME的读写
        /// </summary>
        public string  SENTITYNAME
        {
            get{return _SENTITYNAME;}
            set{_SENTITYNAME=value;}
        }
 
        /// <summary>
        /// 对属性SENTITYNAMECN的读写
        /// </summary>
        public string  SENTITYNAMECN
        {
            get{return _SENTITYNAMECN;}
            set{_SENTITYNAMECN=value;}
        }
 
        /// <summary>
        /// 对属性SENTITYDESC的读写
        /// </summary>
        public string  SENTITYDESC
        {
            get{return _SENTITYDESC;}
            set{_SENTITYDESC=value;}
        }
 
        /// <summary>
        /// 对属性SENTITYDESCCN的读写
        /// </summary>
        public string  SENTITYDESCCN
        {
            get{return _SENTITYDESCCN;}
            set{_SENTITYDESCCN=value;}
        }
 
    }
}

