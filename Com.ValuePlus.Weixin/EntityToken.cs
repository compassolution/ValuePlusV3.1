using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ValuePlus.Weixin
{
    /// <summary>
    /// Token獲取記錄存儲類
    /// 
    /// </summary>
    [Serializable]
    public class EntityToken
    {
        private string _token;
        private string _getTime;
        private string _expireTime;
        private bool _isExired;

        /// <summary>
        /// Token
        /// </summary>
        public string Access_Token
        {
            get { return _token; }
            set { _token = value; }
        }
        /// <summary>
        /// 獲取時間
        /// </summary>
        public string GetTime
        {
            get { return _getTime; }
            set { _getTime = value; }
        }
        /// <summary>
        /// 有效期至（從獲取時間后7200s內有效）
        /// </summary>
        public string ExpireTime
        {
            get { return _expireTime; }
            set { _expireTime = value; }
        }
        /// <summary>
        /// 是否过期
        /// </summary>
        public bool IsExpired
        {
            get { return _isExired; }
            set { _isExired = value; }
        }
    }
}
