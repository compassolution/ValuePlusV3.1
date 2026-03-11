using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ValuePlus.Entity.UpDownLoad
{
    /// <summary>
    /// 上传下载配置xml文件实体类
    /// </summary>
    [Serializable]
    public class UpDownLoadXmlEntity
    {
        private string _id;
        private string _name;
        private string _namecn;
        private string _desc;
        private string _desccn;
        private string _path;//
        private string _allowType;//

        /// <summary>
        /// 配置项ID
        /// </summary>
        public string ID
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// 英文名称
        /// </summary>
        public string NAME
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// 中文名称
        /// </summary>
        public string NAME_CN
        {
            get { return _namecn; }
            set { _namecn = value; }
        }

        /// <summary>
        /// 英文描述
        /// </summary>
        public string DESC
        {
            get { return _desc; }
            set { _desc = value; }
        }

        /// <summary>
        /// 中文描述
        /// </summary>
        public string DESC_CN
        {
            get { return _desccn; }
            set { _desccn = value; }
        }

        /// <summary>
        /// 上传下载路径
        /// </summary>
        public string PATH
        {
            get { return _path; }
            set { _path = value; }
        }
        /// <summary>
        /// 可上传文件类型
        /// </summary>
        public string ALLOWTYPE
        {
            get { return _allowType; }
            set { _allowType = value; }
        }
        
    }
}
