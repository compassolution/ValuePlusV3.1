using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ValuePlus.Archive.Entity
{
    /// <summary>
    /// 更新数据库所需元素的实体类
    /// </summary>
    [Serializable]
    public class Entity_ToDBObject
    {
        private string _TID;
        private string _GID;
        private string _CTRLID;
        private string _FIELDNAME;
        private string _FIELDOLDVALUE;
        private string _FIELDNEWVALUE;
        private string _FIELDTYPE;
        private string _GROUPTYPE;
        private string _IsSave;

        /// <summary>
        /// 对属性_TID的读写[TID]
        /// ADD BY sammen 20140327
        /// </summary>
        public string TID
        {
            get { return _TID; }
            set { _TID = value; }
        }

        /// <summary>
        /// 对属性_GID的读写[GID]
        /// ADD BY sammen 20140327
        /// </summary>
        public string GID
        {
            get { return _GID; }
            set { _GID = value; }
        }

        /// <summary>
        /// 对属性_CTRLID的读写[字段控件ID]
        /// </summary>
        public string CTRLID 
        {
            get { return _CTRLID; }
            set { _CTRLID = value; }
        }
        /// <summary>
        /// 对属性FIELDNAME的读写[字段名]
        /// </summary>
        public string FIELDNAME
        {
            get { return _FIELDNAME; }
            set { _FIELDNAME = value; }
        }

        /// <summary>
        /// 对属性FIELDVALUE_OLD的读写[字段原始值]
        /// </summary>
        public string FIELDVALUE_OLD
        {
            get { return _FIELDOLDVALUE; }
            set { _FIELDOLDVALUE = value; }
        }

        /// <summary>
        /// 对属性FIELDVALUE的读写[字段变更后值]
        /// </summary>
        public string FIELDVALUE_NEW
        {
            get { return _FIELDNEWVALUE; }
            set { _FIELDNEWVALUE = value; }
        }

        /// <summary>
        /// 对属性FIELDTYPE的读写[字段类型]
        /// </summary>
        public string FIELDTYPE
        {
            get { return _FIELDTYPE; }
            set { _FIELDTYPE = value; }
        }

        /// <summary>
        /// 对属性GROUPTYPE的读写[字段类型]
        /// </summary>
        public string GROUPTYPE
        {
            get { return _GROUPTYPE; }
            set { _GROUPTYPE = value; }
        }

        /// <summary>
        /// 对属性PSAVE的读写[是否加密]
        /// </summary>
        public string PSAVE
        {
            get { return _IsSave; }
            set { _IsSave = value; }
        }

    }
}
