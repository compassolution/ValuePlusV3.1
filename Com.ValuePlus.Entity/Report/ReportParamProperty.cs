using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ValuePlus.Entity.Report
{
    /// <summary>
    /// 报表参数属性实体类
    /// </summary>
    [Serializable]
    public class ReportParamProperty
    {
        private string _paramName;
        private string _paramTextTip;
        private string _paramDataType;

        /// <summary>
        /// _paramName
        /// </summary>
        public string strParamName
        {
            get { return _paramName; }
            set { _paramName = value; }
        }

        /// <summary>
        /// _paramTextTip
        /// </summary>
        public string strParamTextTip
        {
            get { return _paramTextTip; }
            set { _paramTextTip = value; }
        }

        /// <summary>
        /// _paramDataType
        /// </summary>
        public string strParamDataType
        {
            get { return _paramDataType; }
            set { _paramDataType = value; }
        }
    }
}
