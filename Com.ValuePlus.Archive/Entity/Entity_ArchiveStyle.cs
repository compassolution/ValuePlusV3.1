using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ValuePlus.Archive.Entity
{
    /// <summary>
    /// 模板明细中的显示样式的定义的实体类
    /// 读取配置文件ArchiveStyleConfig.Config
    /// </summary>
    [Serializable]
    public class Entity_ArchiveStyle
    {
        private string _CellNum;
        private string _TableWidth;
        private string _LabelWidth;
        private string _PageStyle;
        private string _IsCloseAfterSaved;
        private string _IsRealTimeAlert;
        private string _DefaultFilterPID;
        private string _IsSearchByPY;

        /// <summary>
        /// 对属性CellNum(显示列数)的读写
        /// 必须为偶数
        /// </summary>
        public string CellNum
        {
            get { return _CellNum; }
            set { _CellNum = value; }
        }
        /// <summary>
        /// 对属性TableWidth（Table表格的宽度）的读写
        /// </summary>
        public string TableWidth
        {
            get { return _TableWidth; }
            set { _TableWidth = value; }
        }
        /// <summary>
        /// 对属性LabelWidth（Label显示宽度）的读写
        /// </summary>
        public string LabelWidth
        {
            get { return _LabelWidth; }
            set { _LabelWidth = value; }
        }
        /// <summary>
        /// 对属性PageStyle（页面类型）的读写
        /// 0:平铺；1:页签
        /// </summary>
        public string PageStyle
        {
            get { return _PageStyle; }
            set { _PageStyle = value; }
        }
        /// <summary>
        /// 对属性_IsCloseAfterSaved（保存后是否关闭明细页面）的读写
        /// 0:平铺；1:页签
        /// </summary>
        public string IsCloseAfterSaved
        {
            get { return _IsCloseAfterSaved; }
            set { _IsCloseAfterSaved = value; }
        }

        /// <summary>
        /// 是否需要弹出实时提醒
        /// 0否1是
        /// </summary>
        public string IsRealTimeAlert
        {
            get { return _IsRealTimeAlert; }
            set { _IsRealTimeAlert = value; }
        }

        /// <summary>
        /// 默认搜索字段PID
        /// </summary>
        public string DefaultFilterPID
        {
            get { return _DefaultFilterPID; }
            set { _DefaultFilterPID = value; }
        }

        /// <summary>
        /// 列表搜索是否启用首字母拼音
        /// </summary>
        public string IsSearchByPY
        {
            get { return _IsSearchByPY; }
            set { _IsSearchByPY = value; }
        }
        

    }
}
