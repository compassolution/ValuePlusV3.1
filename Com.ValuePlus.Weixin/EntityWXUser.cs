using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ValuePlus.Weixin
{
    /// <summary>
    /// 微信用户信息实体类
    /// 
    /// </summary>
    [Serializable]
    public class EntityWXUser
    {
        private string _OpenId;
        private string _NickName;
        private string _Sex;
        private string _Province;
        private string _City;
        private string _Country;
        private string _HeadImgUrl;
        private string _Privilege;
        private string _Unionid;
        private string _MobileNo;
        private string _Password;
        private string _SMSCount;
        private string _RegTime;
        private string _LastTime;

        public string OpenId { get { return _OpenId; } set { _OpenId = value; } }
        public string NickName { get { return _NickName; } set { _NickName = value; } }
        public string Sex { get { return _Sex; } set { _Sex = value; } }
        public string Province { get { return _Province; } set { _Province = value; } }
        public string City { get { return _City; } set { _City = value; } }
        public string Country { get { return _Country; } set { _Country = value; } }
        public string HeadImgUrl { get { return _HeadImgUrl; } set { _HeadImgUrl = value; } }
        public string Privilege { get { return _Privilege; } set { _Privilege = value; } }
        public string Unionid { get { return _Unionid; } set { _Unionid = value; } }
        public string MobileNo { get { return _MobileNo; } set { _MobileNo = value; } }
        public string Password { get { return _Password; } set { _Password = value; } }
        public string SMSCount { get { return _SMSCount; } set { _SMSCount = value; } }
        public string RegTime { get { return _RegTime; } set { _RegTime = value; } }
        public string LastTime { get { return _LastTime; } set { _LastTime = value; } }

    }
}
