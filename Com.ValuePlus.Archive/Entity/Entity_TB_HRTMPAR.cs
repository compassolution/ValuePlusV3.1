using System;
using System.Text;

namespace Com.ValuePlus.Archive.Entity
{
    /// <summary>
    /// 数据表TB_HRTMPAR实体类
    /// </summary>
    [Serializable]
    public class Entity_TB_HRTMPAR
    {
        private string  _TID;
        private string  _AID;
        private int?  _ECFROM;
        private int?  _ECTO;
        private string  _MESSENG;
        private string  _MESSCHS;

        /// <summary>
        /// 对属性TID的读写
        /// </summary>
        public string  TID
        {
            get{return _TID;}
            set{_TID=value;}
        }
 
        /// <summary>
        /// 对属性AID的读写
        /// </summary>
        public string  AID
        {
            get{return _AID;}
            set{_AID=value;}
        }
 
        /// <summary>
        /// 对属性ECFROM的读写
        /// </summary>
        public int?  ECFROM
        {
            get{return _ECFROM;}
            set{_ECFROM=value;}
        }
 
        /// <summary>
        /// 对属性ECTO的读写
        /// </summary>
        public int?  ECTO
        {
            get{return _ECTO;}
            set{_ECTO=value;}
        }
 
        /// <summary>
        /// 对属性MESSENG的读写
        /// </summary>
        public string  MESSENG
        {
            get{return _MESSENG;}
            set{_MESSENG=value;}
        }
 
        /// <summary>
        /// 对属性MESSCHS的读写
        /// </summary>
        public string  MESSCHS
        {
            get{return _MESSCHS;}
            set{_MESSCHS=value;}
        }
 
    }
}

