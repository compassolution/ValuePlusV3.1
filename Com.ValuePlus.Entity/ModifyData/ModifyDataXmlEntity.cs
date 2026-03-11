using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Com.ValuePlus.Entity.ModifyData
{
    /// <summary>
    /// 修改数据表功能xml文件的实体类
    /// </summary>
    [Serializable]
    public class ModifyDataXmlEntity
    {
        private string _ID;//
        private string _DESCCN;//中文描述
        private string _DESCEN;//英文描述
        private string _SQL;//要显示和修改的记录集的sql语句
        private string _KEY;//sql对应主键
        private Hashtable _FIELDS;//字段实体
        private string _LOG;//英文显示名
        private string _PAGESIZE;//服务器分页的页面大小

        /// <summary>
        /// _ID
        /// </summary>
        public string strID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        /// <summary>
        /// _DESCCN
        /// </summary>
        public string strDESCCN
        {
            get { return _DESCCN; }
            set { _DESCCN = value; }
        }
        /// <summary>
        /// _DESCEN
        /// </summary>
        public string strDESCEN
        {
            get { return _DESCEN; }
            set { _DESCEN = value; }
        }
        /// <summary>
        /// _SQL
        /// </summary>
        public string strSQL
        {
            get { return _SQL; }
            set { _SQL = value; }
        }
        /// <summary>
        /// _KEY
        /// </summary>
        public string strKEY
        {
            get { return _KEY; }
            set { _KEY = value; }
        }
        /// <summary>
        /// _FIELDS
        /// </summary>
        public Hashtable hsTableFIELDS
        {
            get { return _FIELDS; }
            set { _FIELDS = value; }
        }
        /// <summary>
        /// _LOG
        /// </summary>
        public string strLOG
        {
            get { return _LOG; }
            set { _LOG = value; }
        }
        /// <summary>
        /// _PAGESIZE
        /// </summary>
        public string strPAGESIZE
        {
            get { return _PAGESIZE; }
            set { _PAGESIZE = value; }
        }
    }
}
