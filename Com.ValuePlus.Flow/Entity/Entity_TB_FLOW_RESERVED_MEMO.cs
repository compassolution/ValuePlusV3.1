using System;
using System.Text;

namespace Com.ValuePlus.Flow.Entity
{
    /// <summary>
    /// 数据表TB_FLOW_RESERVED_MEMO实体类
    /// </summary>
    [Serializable]
    public class Entity_TB_FLOW_RESERVED_MEMO
    {
        private string  _SRESERVEDCODE;
        private string  _SFLOWPATHCODE;
        private string  _SRESERVEDMEMO;
        private string  _SRESERVEDMEMOCN;
        private decimal  _NINDEX;

        /// <summary>
        /// 对属性SRESERVEDCODE的读写
        /// </summary>
        public string  SRESERVEDCODE
        {
            get{return _SRESERVEDCODE;}
            set{_SRESERVEDCODE=value;}
        }
 
        /// <summary>
        /// 对属性SFLOWPATHCODE的读写
        /// </summary>
        public string  SFLOWPATHCODE
        {
            get{return _SFLOWPATHCODE;}
            set{_SFLOWPATHCODE=value;}
        }
 
        /// <summary>
        /// 对属性SRESERVEDMEMO的读写
        /// </summary>
        public string  SRESERVEDMEMO
        {
            get{return _SRESERVEDMEMO;}
            set{_SRESERVEDMEMO=value;}
        }
 
        /// <summary>
        /// 对属性SRESERVEDMEMOCN的读写
        /// </summary>
        public string  SRESERVEDMEMOCN
        {
            get{return _SRESERVEDMEMOCN;}
            set{_SRESERVEDMEMOCN=value;}
        }
 
        /// <summary>
        /// 对属性NINDEX的读写
        /// </summary>
        public decimal  NINDEX
        {
            get{return _NINDEX;}
            set{_NINDEX=value;}
        }
 
    }
}

