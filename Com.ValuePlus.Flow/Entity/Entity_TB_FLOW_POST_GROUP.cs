using System;
using System.Text;

namespace Com.ValuePlus.Flow.Entity
{
    /// <summary>
    /// 数据表TB_FLOW_POST_GROUP实体类
    /// </summary>
    [Serializable]
    public class Entity_TB_FLOW_POST_GROUP
    {
        private string  _SPOSTGROUPCODE;
        private string  _SPOSTGROUPNAME;
        private string  _SPOSTGROUPNAMECN;

        /// <summary>
        /// 对属性SPOSTGROUPCODE的读写
        /// </summary>
        public string  SPOSTGROUPCODE
        {
            get{return _SPOSTGROUPCODE;}
            set{_SPOSTGROUPCODE=value;}
        }
 
        /// <summary>
        /// 对属性SPOSTGROUPNAME的读写
        /// </summary>
        public string  SPOSTGROUPNAME
        {
            get{return _SPOSTGROUPNAME;}
            set{_SPOSTGROUPNAME=value;}
        }
 
        /// <summary>
        /// 对属性SPOSTGROUPNAMECN的读写
        /// </summary>
        public string  SPOSTGROUPNAMECN
        {
            get{return _SPOSTGROUPNAMECN;}
            set{_SPOSTGROUPNAMECN=value;}
        }
 
    }
}

