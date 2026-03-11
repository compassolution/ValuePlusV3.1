using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ValuePlus.Archive.Entity
{

    /// <summary>
    /// 获取并创建动作的实体类
    /// </summary>
    [Serializable]
    public class Entity_CreateAction
    {
        private string _TID;
        private string _SID;
        private string _AID;
        private string _ADESC;
        private string _ADESCCHS;
        private string _ATYPE;
        private string _ADETAIL;
        private int? _MOVENEXT;
        private int? _AUTOSAVE;

        /// <summary>
        /// 对属性TID的读写
        /// </summary>
        public string TID
        {
            get { return _TID; }
            set { _TID = value; }
        }

        /// <summary>
        /// 对属性SID的读写
        /// </summary>
        public string SID
        {
            get { return _SID; }
            set { _SID = value; }
        }

        /// <summary>
        /// 对属性AID的读写
        /// </summary>
        public string AID
        {
            get { return _AID; }
            set { _AID = value; }
        }

        /// <summary>
        /// 对属性ADESC的读写
        /// </summary>
        public string ADESC
        {
            get { return _ADESC; }
            set { _ADESC = value; }
        }

        /// <summary>
        /// 对属性ADESCCHS的读写
        /// </summary>
        public string ADESCCHS
        {
            get { return _ADESCCHS; }
            set { _ADESCCHS = value; }
        }

        /// <summary>
        /// 对属性ATYPE的读写
        /// </summary>
        public string ATYPE
        {
            get { return _ATYPE; }
            set { _ATYPE = value; }
        }

        /// <summary>
        /// 对属性ADETAIL的读写
        /// </summary>
        public string ADETAIL
        {
            get { return _ADETAIL; }
            set { _ADETAIL = value; }
        }

        /// <summary>
        /// 对属性MOVENEXT的读写
        /// </summary>
        public int? MOVENEXT
        {
            get { return _MOVENEXT; }
            set { _MOVENEXT = value; }
        }

        /// <summary>
        /// 对属性ISAUTOSAVE的读写
        /// </summary>
        public int? ISAUTOSAVE
        {
            get { return _AUTOSAVE; }
            set { _AUTOSAVE = value; }
        }
    }
}
