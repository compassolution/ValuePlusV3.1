using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ValuePlus.Weixin
{
    public class Const
    {
        /// <summary>
        /// 公众号开发者ID(AppID)--默认值，可在系统参数表中进行配置
        /// </summary>
        public const string Weixin_appid = "wx6f2c4b315e434172";
        /// <summary>
        /// 开发者密码(AppSecret)--默认值，可在系统参数表中进行配置
        /// </summary>
        //public const string Weixin_appsecret = "4105b7a2ef03816d2c1275545440dabe";
        public const string Weixin_appsecret = "bafad182cc4500b38416020baeb2739e";

        /// <summary>
        /// 【用户授权】用户同意授权，获取code
        /// </summary>
        public const string Weixin_URL_GetCodeUrl = "https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&response_type=code&scope=snsapi_userinfo&state=STATE&redirect_uri={1}#wechat_redirect";

        /// <summary>
        /// 【用户授权】通过code换取网页授权access_token
        /// </summary>
        public const string Weixin_URL_GetAccessTokenByCode = "https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code";

        /// <summary>
        /// 【用户授权】刷新access_token
        /// </summary>
        public const string Weixin_URL_RefreshToken = "https://api.weixin.qq.com/sns/oauth2/refresh_token?appid={0}&grant_type=refresh_token&refresh_token={1}";

        /// <summary>
        /// 【基础操作】用access_token和openid获取到用户基本信息
        /// </summary>
        public const string Weixin_URL_GetUserInfo = "https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}&lang=zh_CN";

        /// <summary>
        /// 【基础操作】获取基础access token的接口地址,每天有次数限制2000
        /// </summary>
        public const string Weixin_URL_GetBasicAccessToken = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}";

        /// <summary>
        /// 【群发消息消息】获得模板ID
        /// </summary>
        public const string Weixin_URL_SendMassMessage = "https://api.weixin.qq.com/cgi-bin/message/mass/send?access_token={0}";

        /// <summary>
        /// 【模板消息】获取模板列表
        /// 参数及返回值参照页面https://developers.weixin.qq.com/doc/offiaccount/Message_Management/Template_Message_Interface.html#2
        /// </summary>
        public const string Weixin_URL_GetTemplateList = "https://api.weixin.qq.com/cgi-bin/template/get_all_private_template?access_token={0}";

        /// <summary>
        /// 【模板消息】根据模板简称获得模板ID
        /// 参数及返回值参照页面https://developers.weixin.qq.com/doc/offiaccount/Message_Management/Template_Message_Interface.html#2
        /// </summary>
        public const string Weixin_URL_GetTemplateId = "https://api.weixin.qq.com/cgi-bin/template/api_add_template?access_token={0}";

        /// <summary>
        /// 【模板消息】根据模板简称删除模板
        /// 参数及返回值参照页面https://developers.weixin.qq.com/doc/offiaccount/Message_Management/Template_Message_Interface.html#2
        /// </summary>
        public const string Weixin_URL_DeleteTemplate = "https://api.weixin.qq.com/cgi-bin/template/del_private_template?access_token={0}";

        /// <summary>
        /// 【模板消息】发送模板消息
        /// 参数及返回值参照页面https://developers.weixin.qq.com/doc/offiaccount/Message_Management/Template_Message_Interface.html#2
        /// </summary>
        public const string Weixin_URL_SendTemplateMessage = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token={0}";

        /// <summary>
        /// 上传多媒体的接口地址
        /// 上传的多媒体文件有格式和大小限制，如下：
        ////图片（image）: 1M，支持JPG格式
        ////语音（voice）：2M，播放长度不超过60s，支持AMR\MP3格式
        ////视频（video）：10MB，支持MP4格式
        ////缩略图（thumb）：64KB，支持JPG格式
        /// </summary>
        public const string Weixin_URL_UploadMedia = "http://file.api.weixin.qq.com/cgi-bin/media/upload?access_token={0}&type={0}";

        #region 微信小程序接口
        //【小程序】调用 auth.code2Session 接口，换取 用户唯一标识 OpenID 和 会话密钥 session_key。
        public const String WeixinMP_URL_GetAccessTokenByCode = "https://api.weixin.qq.com/sns/jscode2session?appid={0}&secret={1}&js_code={2}&grant_type=authorization_code";

        //【小程序】调用 sendTemplateMessage发送模板消息【此接口已于2020-1-11日暂停使用】
        //public const String WeixinMP_URL_sendTemplateMessage = "https://api.weixin.qq.com/cgi-bin/message/wxopen/template/send?access_token={0}";

        //【小程序】调用 sendTemplateMessage发送订阅消息消息
        public const String WeixinMP_URL_sendSubscribeMessage =  "https://api.weixin.qq.com/cgi-bin/message/subscribe/send?access_token={0}";
        #endregion

    }
}
