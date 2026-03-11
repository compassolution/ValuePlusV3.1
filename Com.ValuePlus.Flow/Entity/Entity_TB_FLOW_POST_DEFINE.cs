using System;
using System.Text;

namespace Com.ValuePlus.Flow.Entity
{
    /// <summary>
    /// 数据表TB_FLOW_POST_DEFINE实体类
    /// </summary>
    [Serializable]
    public class Entity_TB_FLOW_POST_DEFINE
    {
        private string  _SPOSTCODE;
        private string  _SFLOWCODE;
        private string  _SPOSTNAME;
        private string  _SPOSTNAMECN;
        private decimal  _NWORKDAY;
        private string  _SPLUGIN_PRE;
        private string  _SPLUGIN_AFTER;
        private string  _BISSTARTPOST;

        /// <summary>
        /// 对属性SPOSTCODE的读写
        /// </summary>
        public string  SPOSTCODE
        {
            get{return _SPOSTCODE;}
            set{_SPOSTCODE=value;}
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
        /// 对属性SPOSTNAME的读写
        /// </summary>
        public string  SPOSTNAME
        {
            get{return _SPOSTNAME;}
            set{_SPOSTNAME=value;}
        }
 
        /// <summary>
        /// 对属性SPOSTNAMECN的读写
        /// </summary>
        public string  SPOSTNAMECN
        {
            get{return _SPOSTNAMECN;}
            set{_SPOSTNAMECN=value;}
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

        /// <summary>
        /// 对属性BISSTARTPOST的读写
        /// </summary>
        public string BISSTARTPOST
        {
            get { return _BISSTARTPOST; }
            set { _BISSTARTPOST = value; }
        }
 
    }
}

