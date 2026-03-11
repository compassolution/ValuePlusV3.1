using System;
using System.Text;

namespace Com.ValuePlus.Flow.Entity
{
    /// <summary>
    /// 数据表TB_FLOW_POST_GROUP_DETAIL实体类
    /// </summary>
    [Serializable]
    public class Entity_TB_FLOW_POST_GROUP_DETAIL
    {
        private string  _SPOSTGROUPCODE;
        private string  _SPOSTCODE;

        /// <summary>
        /// 对属性SPOSTGROUPCODE的读写
        /// </summary>
        public string  SPOSTGROUPCODE
        {
            get{return _SPOSTGROUPCODE;}
            set{_SPOSTGROUPCODE=value;}
        }
 
        /// <summary>
        /// 对属性SPOSTCODE的读写
        /// </summary>
        public string  SPOSTCODE
        {
            get{return _SPOSTCODE;}
            set{_SPOSTCODE=value;}
        }
 
    }
}

