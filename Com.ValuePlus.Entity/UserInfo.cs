using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ValuePlus.Entity
{

    /// <summary>
    /// 用户基本信息实体类
    /// </summary>
    [Serializable]
    public class UserInfo
    {

        private string _SUSERID;
        /// <summary>
        /// 用户id
        /// </summary>
        public string SUSERID
        {
            get { return _SUSERID; }
            set { _SUSERID = value; }
        }
        private string _SACCOUNTID;
        /// <summary>
        /// 用户帐号
        /// </summary>
        public string SACCOUNTID
        {
            get { return _SACCOUNTID; }
            set { _SACCOUNTID = value; }
        }
        private string _SPWD;
        /// <summary>
        /// 密码
        /// </summary>
        public string SPWD
        {
            get { return _SPWD; }
            set { _SPWD = value; }
        }
        private string _STAFFNO;
        /// <summary>
        /// 对应员工编号
        /// </summary>
        public string STAFFNO
        {
            get { return _STAFFNO; }
            set { _STAFFNO = value; }
        }
        private string _SUSERNAME;
        /// <summary>
        /// 登录用户英文名
        /// </summary>
        public string SUSERNAME
        {
            get { return _SUSERNAME; }
            set { _SUSERNAME = value; }
        }
        private string _SUSERNAMECN;
        /// <summary>
        /// 登录用户中文名
        /// </summary>
        public string SUSERNAMECN
        {
            get { return _SUSERNAMECN; }
            set { _SUSERNAMECN = value; }
        }
        private string _SDEPTCODE;
        /// <summary>
        /// 用户部门编码
        /// </summary>
        public string SDEPTCODE
        {
            get { return _SDEPTCODE; }
            set { _SDEPTCODE = value; }
        }
        private string _SDEPT;
        /// <summary>
        /// 用户部门英文名
        /// </summary>
        public string SDEPT
        {
            get { return _SDEPT; }
            set { _SDEPT = value; }
        }
        private string _SDEPTCN;
        /// <summary>
        /// 用户部门中文名
        /// </summary>
        public string SDEPTCN
        {
            get { return _SDEPTCN; }
            set { _SDEPTCN = value; }
        }
        private string _SPOSI;
        /// <summary>
        /// 用户职位英文名
        /// </summary>
        public string SPOSI
        {
            get { return _SPOSI; }
            set { _SPOSI = value; }
        }
        private string _SPOSICN;
        /// <summary>
        /// 用户职位中文名
        /// </summary>
        public string SPOSICN
        {
            get { return _SPOSICN; }
            set { _SPOSICN = value; }
        }
        private string _STREECLR;
        /// <summary>
        /// 功能树颜色
        /// </summary>
        public string STREECLR
        {
            get { return _STREECLR; }
            set { _STREECLR = value; }
        }
        private string _SWORKCLR;
        /// <summary>
        /// 工作区颜色
        /// </summary>
        public string SWORKCLR
        {
            get { return _SWORKCLR; }
            set { _SWORKCLR = value; }
        }
        private string _BISALERT;
        /// <summary>
        /// 是否更换过
        /// </summary>
        public string BISALERT
        {
            get { return _BISALERT; }
            set { _BISALERT = value; }
        }
        private string _BISGROUPUSER;
        /// <summary>
        /// 是否集团用户
        /// </summary>
        public string BISGROUPUSER
        {
            get { return _BISGROUPUSER; }
            set { _BISGROUPUSER = value; }
        }
        private string _BISSTOP;
        /// <summary>
        /// 是否停用
        /// </summary>
        public string BISSTOP
        {
            get { return _BISSTOP; }
            set { _BISSTOP = value; }
        }
        private DateTime _logintiem;

        /// <summary>
        /// 登录时间
        /// </summary>
        public DateTime Logintiem
        {
            get { return _logintiem; }
            set { _logintiem = value; }
        }
        private string _loginip;

        /// <summary>
        /// 登录ip
        /// </summary>
        public string Loginip
        {
            get { return _loginip; }
            set { _loginip = value; }
        }

        private string _language;

        /// <summary>
        /// 语言
        /// </summary>
        public string Language
        {
            get { return _language; }
            set { _language = value; }
        }

        private string _MachineCode;

        /// <summary>
        /// 服务器机器码
        /// </summary>
        public string MachineCode
        {
            get { return _MachineCode; }
            set { _MachineCode = value; }
        }

        private string _webSiteHostUrl;
        /// <summary>
        /// 站点的域名
        /// </summary>
        public string WebSiteHostUrl
        {
            get { return _webSiteHostUrl; }
            set { _webSiteHostUrl = value; }
        }
    }
}
