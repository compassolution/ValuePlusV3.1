using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ValuePlus.Entity.ModifyData
{
    /// <summary>
    /// 修改数据表功能xml文件中的字段描述实体类
    /// </summary>
    [Serializable]
    public class ModifyDataFieldEntity
    {
        private string _ID;//字段名称
        private string _CNNAME;//字段中文显示名
        private string _ENNAME;//英文显示名
        private string _CTYPE;//界面显示控件类型
        private string _WIDTH;//界面显示控件长度
        private string _LENGTH;//数据长度
        private string _ISQUERY;//是否显示在查询区域
        private string _FROMTYPE;//数据来源类型
        private string _FROMKEY;//数据来源如果是sql，则应对应的key字段
        private string _FROMCNSQL;//数据来源中文sql
        private string _FROMENSQL;//数据来源英文sql

        /// <summary>
        /// _ID
        /// </summary>
        public string strID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        /// <summary>
        /// _CNNAME
        /// </summary>
        public string strCNNAME
        {
            get { return _CNNAME; }
            set { _CNNAME = value; }
        }
        /// <summary>
        /// _ENNAME
        /// </summary>
        public string strENNAME
        {
            get { return _ENNAME; }
            set { _ENNAME = value; }
        }
        /// <summary>
        /// _CTYPE
        /// </summary>
        public string strCTYPE
        {
            get { return _CTYPE; }
            set { _CTYPE = value; }
        }
        /// <summary>
        /// _WIDTH
        /// </summary>
        public string strWIDTH
        {
            get { return _WIDTH; }
            set { _WIDTH = value; }
        }
        /// <summary>
        /// _LENGTH
        /// </summary>
        public string strLENGTH
        {
            get { return _LENGTH; }
            set { _LENGTH = value; }
        }
        /// <summary>
        /// _ISQUERY
        /// </summary>
        public string strISQUERY
        {
            get { return _ISQUERY; }
            set { _ISQUERY = value; }
        }
        /// <summary>
        /// _FROMTYPE
        /// </summary>
        public string strFROMTYPE
        {
            get { return _FROMTYPE; }
            set { _FROMTYPE = value; }
        }
        /// <summary>
        /// _FROMKEY
        /// </summary>
        public string strFROMKEY
        {
            get { return _FROMKEY; }
            set { _FROMKEY = value; }
        }
        /// <summary>
        /// _FROMCNSQL
        /// </summary>
        public string strFROMCNSQL
        {
            get { return _FROMCNSQL; }
            set { _FROMCNSQL = value; }
        }
        /// <summary>
        /// _FROMENSQL
        /// </summary>
        public string strFROMENSQL
        {
            get { return _FROMENSQL; }
            set { _FROMENSQL = value; }
        }
    }
}
