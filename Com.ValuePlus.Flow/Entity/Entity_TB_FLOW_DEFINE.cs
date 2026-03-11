using System;
using System.Text;

namespace Com.ValuePlus.Flow.Entity
{
    /// <summary>
    /// 数据表TB_FLOW_DEFINE实体类
    /// </summary>
    [Serializable]
    public class Entity_TB_FLOW_DEFINE
    {
        private string _SFLOWCODE;
        private string _SFLOWNAME;
        private string _SFLOWNAMECN;
        private string _SFLOWDESC;
        private string _SFLOWDESCCN;
        private Decimal? _NWORKCOUNTDAY;
        private string _BISNEEDACCEPT;
        private string _BISACTIVEWITHSUB;
        private string _BSTOP;

        /// <summary>
        /// 对属性SFLOWCODE的读写
        /// </summary>
        public string SFLOWCODE
        {
            get { return _SFLOWCODE; }
            set { _SFLOWCODE = value; }
        }

        /// <summary>
        /// 对属性SFLOWNAME的读写
        /// </summary>
        public string SFLOWNAME
        {
            get { return _SFLOWNAME; }
            set { _SFLOWNAME = value; }
        }

        /// <summary>
        /// 对属性SFLOWNAMECN的读写
        /// </summary>
        public string SFLOWNAMECN
        {
            get { return _SFLOWNAMECN; }
            set { _SFLOWNAMECN = value; }
        }

        /// <summary>
        /// 对属性SFLOWDESC的读写
        /// </summary>
        public string SFLOWDESC
        {
            get { return _SFLOWDESC; }
            set { _SFLOWDESC = value; }
        }

        /// <summary>
        /// 对属性SFLOWDESCCN的读写
        /// </summary>
        public string SFLOWDESCCN
        {
            get { return _SFLOWDESCCN; }
            set { _SFLOWDESCCN = value; }
        }

        /// <summary>
        /// 对属性NWORKCOUNTDAY的读写
        /// </summary>
        public Decimal? NWORKCOUNTDAY
        {
            get { return _NWORKCOUNTDAY; }
            set { _NWORKCOUNTDAY = value; }
        }

        /// <summary>
        /// 对属性BISNEEDACCEPT的读写
        /// </summary>
        public string BISNEEDACCEPT
        {
            get { return _BISNEEDACCEPT; }
            set { _BISNEEDACCEPT = value; }
        }

        /// <summary>
        /// 对属性BISACTIVEWITHSUB的读写
        /// </summary>
        public string BISACTIVEWITHSUB
        {
            get { return _BISACTIVEWITHSUB; }
            set { _BISACTIVEWITHSUB = value; }
        }

        /// <summary>
        /// 对属性BSTOP的读写
        /// </summary>
        public string BSTOP
        {
            get { return _BSTOP; }
            set { _BSTOP = value; }
        }

    }
}
