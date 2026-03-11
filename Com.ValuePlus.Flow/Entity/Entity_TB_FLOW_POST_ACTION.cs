using System;
using System.Text;

namespace Com.ValuePlus.Flow.Entity
{
    /// <summary>
    /// 数据表TB_FLOW_POST_ACTION实体类
    /// </summary>
    [Serializable]
    public class Entity_TB_FLOW_POST_ACTION
    {
        private string  _SFLOWACTIONCODE;
        private string  _SPOSTCODE;
        private string  _SFLOWACTIONNAME;
        private string  _SFLOWACTIONNAMECN;
        private string  _SACTIONDETAIL;
        private decimal  _NINDEX;
        private string  _SBIZSTATUS;
        private string  _SPLUGIN_PRE;
        private string  _SPLUGIN_AFTER;

        /// <summary>
        /// 对属性SFLOWACTIONCODE的读写
        /// </summary>
        public string  SFLOWACTIONCODE
        {
            get{return _SFLOWACTIONCODE;}
            set{_SFLOWACTIONCODE=value;}
        }
 
        /// <summary>
        /// 对属性SPOSTCODE的读写
        /// </summary>
        public string  SPOSTCODE
        {
            get{return _SPOSTCODE;}
            set{_SPOSTCODE=value;}
        }
 
        /// <summary>
        /// 对属性SFLOWACTIONNAME的读写
        /// </summary>
        public string  SFLOWACTIONNAME
        {
            get{return _SFLOWACTIONNAME;}
            set{_SFLOWACTIONNAME=value;}
        }
 
        /// <summary>
        /// 对属性SFLOWACTIONNAMECN的读写
        /// </summary>
        public string  SFLOWACTIONNAMECN
        {
            get{return _SFLOWACTIONNAMECN;}
            set{_SFLOWACTIONNAMECN=value;}
        }
 
        /// <summary>
        /// 对属性SACTIONDETAIL的读写
        /// </summary>
        public string  SACTIONDETAIL
        {
            get{return _SACTIONDETAIL;}
            set{_SACTIONDETAIL=value;}
        }
 
        /// <summary>
        /// 对属性NINDEX的读写
        /// </summary>
        public decimal  NINDEX
        {
            get{return _NINDEX;}
            set{_NINDEX=value;}
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
        /// 对属性SPLUGIN_PRE的读写
        /// </summary>
        public string  SPLUGIN_PRE
        {
            get{return _SPLUGIN_PRE;}
            set{_SPLUGIN_PRE=value;}
        }
 
        /// <summary>
        /// 对属性SPLUGIN_AFTER的读写
        /// </summary>
        public string  SPLUGIN_AFTER
        {
            get{return _SPLUGIN_AFTER;}
            set{_SPLUGIN_AFTER=value;}
        }
 
    }
}

