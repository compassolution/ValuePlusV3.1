using System;
using System.Text;

namespace Com.ValuePlus.Flow.Entity
{
    /// <summary>
    /// 数据表TB_FLOW_PATH实体类
    /// </summary>
    [Serializable]
    public class Entity_TB_FLOW_PATH
    {
        private string  _SFLOWPATHCODE;
        private string  _SFLOWCODE;
        private string  _SPOSTCODE_PRE;
        private string  _SPOSTCODE_NEXT;
        private decimal  _NWORKDAY;
        private string  _SWAYDESC;
        private string  _SWAYDESCCN;
        private string  _SBIZSTATUS;
        private decimal  _NINDEX;

        /// <summary>
        /// 对属性SFLOWPATHCODE的读写
        /// </summary>
        public string  SFLOWPATHCODE
        {
            get{return _SFLOWPATHCODE;}
            set{_SFLOWPATHCODE=value;}
        }
 
        /// <summary>
        /// 对属性SFLOWCODE的读写
        /// </summary>
        public string  SFLOWCODE
        {
            get{return _SFLOWCODE;}
            set{_SFLOWCODE=value;}
        }
 
        /// <summary>
        /// 对属性SPOSTCODE_PRE的读写
        /// </summary>
        public string  SPOSTCODE_PRE
        {
            get{return _SPOSTCODE_PRE;}
            set{_SPOSTCODE_PRE=value;}
        }
 
        /// <summary>
        /// 对属性SPOSTCODE_NEXT的读写
        /// </summary>
        public string  SPOSTCODE_NEXT
        {
            get{return _SPOSTCODE_NEXT;}
            set{_SPOSTCODE_NEXT=value;}
        }
 
        /// <summary>
        /// 对属性NWORKDAY的读写
        /// </summary>
        public decimal  NWORKDAY
        {
            get{return _NWORKDAY;}
            set{_NWORKDAY=value;}
        }
 
        /// <summary>
        /// 对属性SWAYDESC的读写
        /// </summary>
        public string  SWAYDESC
        {
            get{return _SWAYDESC;}
            set{_SWAYDESC=value;}
        }
 
        /// <summary>
        /// 对属性SWAYDESCCN的读写
        /// </summary>
        public string  SWAYDESCCN
        {
            get{return _SWAYDESCCN;}
            set{_SWAYDESCCN=value;}
        }
 
        /// <summary>
        /// 对属性SBIZSTATUS的读写
        /// </summary>
        public string  SBIZSTATUS
        {
            get{return _SBIZSTATUS;}
            set{_SBIZSTATUS=value;}
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

