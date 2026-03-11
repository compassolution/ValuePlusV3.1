using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.ValuePlus.SysTask
{

    /// <summary>
    /// 系统计划任务的实体类
    /// 读取数据表SYSTASK_1
    /// </summary>
    [Serializable]
    public class Entity_SysTask
    {
        private string _TaskCode;
        private string _TaskName;
        private string _TaskDesc;
        private string _ExecType;
        private string _Month;
        private string _Day;
        private string _Time;
        private string _TaskDetail;
        private string _IsStop;


        /// <summary>
        /// 对属性_TaskCode(任务编码)的读写
        /// 
        /// </summary>
        public string TaskCode
        {
            get { return _TaskCode; }
            set { _TaskCode = value; }
        }
        /// <summary>
        /// 对属性_TaskName（任务名称）的读写
        /// </summary>
        public string TaskName
        {
            get { return _TaskName; }
            set { _TaskName = value; }
        }
        /// <summary>
        /// 对属性_TaskDesc（任务描述）的读写
        /// </summary>
        public string TaskDesc
        {
            get { return _TaskDesc; }
            set { _TaskDesc = value; }
        }
        /// <summary>
        /// 对属性_ExecType（执行期别）的读写
        /// 010	Every Year	每年
        /// 020	Every Month	每月
        /// 030	Every Day	每天
        /// 040	Every Hour	每小时
        /// </summary>
        public string ExecType
        {
            get { return _ExecType; }
            set { _ExecType = value; }
        }
        /// <summary>
        /// 对属性_Month（月份）的读写
        /// 
        /// </summary>
        public string Month
        {
            get { return _Month; }
            set { _Month = value; }
        }

        /// <summary>
        /// 对属性_Day（日期）的读写
        /// 
        /// </summary>
        public string Day
        {
            get { return _Day; }
            set { _Day = value; }
        }

        /// <summary>
        /// 对属性_Time（时间）的读写
        /// 时间点，可多个时间点，中间用;分开
        /// </summary>
        public string Time
        {
            get { return _Time; }
            set { _Time = value; }
        }

        /// <summary>
        /// 对属性_TaskDetail（执行语句）的读写
        /// SQL语句
        /// </summary>
        public string TaskDetail
        {
            get { return _TaskDetail; }
            set { _TaskDetail = value; }
        }

        /// <summary>
        /// 对属性_IsStop（是否停用）的读写
        /// 1/停用2/正常使用
        /// </summary>
        public string IsStop
        {
            get { return _IsStop; }
            set { _IsStop = value; }
        }


    }

}
