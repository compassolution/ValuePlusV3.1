using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Com.ValuePlus.Archive.Entity
{
    /// <summary>
    /// 模板中主控字段PMAST影响的控件及其数据集的实体类
    /// </summary>
    [Serializable]
    public class Entity_AjaxPMastObject
    {
        private string _TID;
        private string _GID;
        private string _CtrlType;
        private string _CtrlId;
        private string _MastValue;
        private DataTable _DtResult;

        /// <summary>
        /// 对属性TID的读写
        /// </summary>
        public string TID
        {
            get { return _TID; }
            set { _TID = value; }
        }

        /// <summary>
        /// 对属性GID的读写
        /// </summary>
        public string GID
        {
            get { return _GID; }
            set { _GID = value; }
        }
        /// <summary>
        /// 对属性CtrlType的读写
        /// </summary>
        public string CtrlType
        {
            get { return _CtrlType; }
            set { _CtrlType = value; }
        }

        /// <summary>
        /// 对属性CtrlId的读写
        /// </summary>
        public string CtrlId
        {
            get { return _CtrlId; }
            set { _CtrlId = value; }
        }

        /// <summary>
        /// 对属性MastValue的读写
        /// </summary>
        public string MastValue
        {
            get { return _MastValue; }
            set { _MastValue = value; }
        }

        /// <summary>
        /// 对属性DtResult的读写
        /// </summary>
        public DataTable DtResult
        {
            get { return _DtResult; }
            set { _DtResult = value; }
        }
    }
}
