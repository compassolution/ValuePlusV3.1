using System;
using System.Text;

namespace Com.ValuePlus.Archive.Entity
{
    /// <summary>
    /// 数据表TB_HRTMPRD实体类
    /// </summary>
    [Serializable]
    public class Entity_TB_HRTMPRD
    {
        private string  _TID;
        private string  _RID;
        private string  _SID;

        /// <summary>
        /// 对属性TID的读写
        /// </summary>
        public string  TID
        {
            get{return _TID;}
            set{_TID=value;}
        }
 
        /// <summary>
        /// 对属性RID的读写
        /// </summary>
        public string  RID
        {
            get{return _RID;}
            set{_RID=value;}
        }
 
        /// <summary>
        /// 对属性SID的读写
        /// </summary>
        public string  SID
        {
            get{return _SID;}
            set{_SID=value;}
        }
 
    }
}

