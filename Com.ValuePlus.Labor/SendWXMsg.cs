using System;
using System.Collections.Generic;
using System.Text;
using Com.ValuePlus.Weixin;
using System.Data;
using System.Collections;
using Com.ValuePlus.DAL;
using System.Web.Script.Serialization;

namespace Com.ValuePlus.Labor
{
    /// <summary>
    /// 微信小程序订阅消息发送工单状态变更
    /// 目前小程序只支持一次性订阅，无法长期订阅，故此功能暂时搁置。
    /// </summary>
    public class SendWXMsg
    {
        /// <summary>
        /// 日志声明
        /// </summary>
        protected static Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static String strAppId = GetSysParams.GetWeixin_AppId();
        private static String strAppsecret = GetSysParams.GetWeixin_Appsecret();
        //发送服务通知时待办工单的模板消息的id
        public String strTemplateId_WorkOrderPending = GetSysParams.GetWeixin_TemplateId_WorkOrderPending();

        /// <summary>
        /// 工单状态变化时发送订阅消息
        /// </summary>
        /// <param name="strWONO"></param>
        /// <param name="strStatusTo"></param>
        /// <param name="IsReload">是否重新获取需推送记录【字符串1或者true时需重新获取】</param>
        /// <returns></returns>
        public static String SendWOPendingMsg(String strWONO,String strStatusTo,String IsReload)
        {
            StringBuilder sbReturn = new StringBuilder();
            try
            {
                String strTemplateId_WorkOrderPending = Com.ValuePlus.Labor.GetSysParams.GetWeixin_TemplateId_WorkOrderPending();
                //获取AccessToken的接口(外包工的入口编码为030)
                String strNow = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                String strrAccessToken = AccessTokenGetter.GetValidAccessToken(strAppId, strAppsecret, strNow, "030");
                //请求地址
                string apiUrl = Com.ValuePlus.Weixin.Const.WeixinMP_URL_sendSubscribeMessage;
                //string apiUrl = "https://api.weixin.qq.com/cgi-bin/message/subscribe/send?access_token={0}";
                string requestUrl = string.Format(apiUrl, strrAccessToken);

                log.Error("【Labor SendWXMsg.SendWOPendingMsg过程测试】TemplateId_WorkOrderPending：" + strTemplateId_WorkOrderPending);
                log.Error("【Labor SendWXMsg.SendWOPendingMsg过程测试】requestUrl：" + requestUrl);
                log.Error("【Labor SendWXMsg.SendWOPendingMsg过程测试】WONO：" + strWONO);
                log.Error("【Labor SendWXMsg.SendWOPendingMsg过程测试】StatusTo：" + strStatusTo);
                log.Error("【Labor SendWXMsg.SendWOPendingMsg过程测试】IsReload：" + IsReload);

                DataTable dt = GetPushMsgOrderUserList(strWONO,strStatusTo, IsReload);
                int iAllQty = dt.Rows.Count;
                int iSuccessQty = 0;

                log.Error("【Labor SendWXMsg.SendWOPendingMsg过程测试】需推送消息条数：" + iAllQty);

                if (dt != null && dt.Rows.Count > 0)
                {
                    for(int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        String strOpenId = dr["OpenId"].ToString();
                        log.Error("【Labor SendWXMsg.SendWOPendingMsg过程测试】循环记录:"+i.ToString()+"，OpenId：" + strOpenId);
                        if (String.IsNullOrEmpty(strOpenId))
                        {
                            continue;
                        }
                        String strKEY = dr["SKEY"].ToString();
                        String strHandleCompanyName = dr["HandleCompanyName"].ToString();
                        String strHandleUserName = dr["HandleUserName"].ToString();
                        String strStatusName = dr["HandleStatusName"].ToString();
                        String strHandleTime = DateTime.Parse(dr["HandleTime"].ToString()).ToString("yyyy年M月dd日 HH:mm");
                        StringBuilder sbJsonData = new StringBuilder();
                        sbJsonData.Append("{");
                        sbJsonData.Append("     \"touser\": \"" + strOpenId + "\",");
                        sbJsonData.Append("     \"template_id\": \"" + strTemplateId_WorkOrderPending + "\",");
                        sbJsonData.Append("     \"page\": \"/pages/WorkOrder/WorkOrderList\",");
                        sbJsonData.Append("     \"miniprogram_state\": \"formal\",");
                        sbJsonData.Append("     \"lang\": \"zh_CN\",");//string 	否 	模板需要放大的关键词，不填则默认无放大
                        sbJsonData.Append("     \"data\": {");
                        sbJsonData.Append("         \"character_string1\": {");
                        sbJsonData.Append("             \"value\": \"" + strWONO + "\"");
                        sbJsonData.Append("         },");
                        sbJsonData.Append("         \"thing2\": {");
                        sbJsonData.Append("             \"value\": \"" + strHandleCompanyName + "\"");
                        sbJsonData.Append("         },");
                        sbJsonData.Append("         \"name3\": {");
                        sbJsonData.Append("             \"value\": \"" + strHandleUserName + "\"");
                        sbJsonData.Append("         },");
                        sbJsonData.Append("         \"thing4\": {");
                        sbJsonData.Append("             \"value\": \"" + strStatusName + "\"");
                        sbJsonData.Append("         },");
                        sbJsonData.Append("         \"date5\": {");
                        sbJsonData.Append("             \"value\": \"" + strHandleTime + "\"");
                        sbJsonData.Append("         }");
                        sbJsonData.Append("     }");
                        sbJsonData.Append("}"); ;
                        string ReText = PublicHelper.WebRequestPostOrGet(requestUrl, sbJsonData.ToString());//post/get方法获取信息 
                        JavaScriptSerializer myJson = new JavaScriptSerializer();
                        Dictionary<string, object> DicText = (Dictionary<string, object>)myJson.DeserializeObject(ReText);
                        String strErrCode = DicText["errcode"].ToString();
                        String strErrMsg = DicText["errmsg"].ToString();

                        log.Error("【Labor SendWXMsg.SendWOPendingMsg过程测试】执行发送接口后ErrCode：" + strErrCode);
                        log.Error("【Labor SendWXMsg.SendWOPendingMsg过程测试】执行发送接口后ErrMsg：" + strErrMsg);
                        //发送订阅消息后的处理
                        DoAfterSuccessSendMsg(strKEY, strWONO, strStatusTo, strErrCode, strErrMsg);

                        iSuccessQty++;
                    }
                }


                sbReturn.Append("{");
                sbReturn.Append("   \"WONO\":\"" + strWONO + "\"");
                sbReturn.Append("   ,\"StatusTo\":\"" + strStatusTo + "\"");
                sbReturn.Append("   ,\"AllQty\":\"" + iAllQty.ToString() + "\"");
                sbReturn.Append("   ,\"SuccessQty\":\"" + iSuccessQty.ToString() + "\"");
                sbReturn.Append("}");

            }
            catch(Exception ex)
            {
                log.Error("Labor 工单状态变化时发送订阅消息出错" + ex);
            }
            return sbReturn.ToString();
        }

        /// <summary>
        /// 根据工单编号获取其当前状态需要发送订阅消息的用户清单及内容
        /// </summary>
        /// <param name="strWONO"></param>
        /// <param name="strStatusTo"></param>
        /// <param name="IsReload">是否重新获取需推送记录【字符串1或者true时需重新获取】</param>
        /// <returns></returns>
        private static DataTable GetPushMsgOrderUserList(String strWONO,String strStatusTo,String IsReload)
        {
            DataTable dtReturn = new DataTable();
            StringBuilder sbSql = new StringBuilder();
            String strSPName = "USP_Labor_SendMsg_GetOrderUserList";
            String strSql = "select * from [_USP_Labor_SendMsg_GetOrderUserList] where WONO = '" + strWONO + "' AND ISNULL(SendIsSuccess,'0')='0'";
            log.Error("根据工单编号获取其当前状态需要发送订阅消息的用户清单及内容,strWONO:" + strWONO + ";strStatusTo:" + strStatusTo + ";IsReload:" + IsReload);
            log.Error("根据工单编号获取其当前状态需要发送订阅消息的用户清单及内容,strSql:" + strSql);
            try
            {
                if (IsReload.Equals("1")||IsReload.ToLower().Equals("true"))
                {
                    Hashtable hsTableParams = new Hashtable();
                    hsTableParams.Add("WONO", strWONO);
                    hsTableParams.Add("StatusTo", strStatusTo);
                    int iReturn = SqlParamDao.ExcuteSP(strSPName, hsTableParams);
                }
                dtReturn = SqlParamDao.GetDataTableBySql(strSql);
            }
            catch (Exception ex)
            {
                log.Error("Labor 根据工单编号获取其当前状态需要发送订阅消息的用户清单及内容出错" + ex);
                log.Error("根据工单编号获取其当前状态需要发送订阅消息的用户清单及内容失败:strSql:" + strSql);
            }
            return dtReturn;
        }

        /// <summary>
        /// 发送订阅消息后的处理
        /// </summary>
        /// <param name="strKey"></param>
        /// <param name="strWONO"></param>
        /// <param name="strStatusTo"></param>
        /// <param name="strErrCode"></param>
        /// <param name="strErrMsg"></param>
        private static void DoAfterSuccessSendMsg(String strKey,String strWONO,String strStatusTo,String strErrCode,String strErrMsg)
        {
            log.Error("【Labor SendWXMsg.SendWOPendingMsg过程测试】发送订阅消息后的处理");
            String strIsSuccess = strErrCode.Equals("0") ? "1" : "0";
            String strNowTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("UPDATE [_USP_Labor_SendMsg_GetOrderUserList] SET MsgSendTime = '" + strNowTime + "'");
            sbSql.Append(", SendErrCode = '" + strErrCode + "'");
            sbSql.Append(", SendErrMsg = '" + strErrMsg + "'");
            sbSql.Append(", SendIsSuccess = '" + strIsSuccess  + "'");
            sbSql.Append(" where SKEY = '" + strKey + "'");
            String strSql = sbSql.ToString();
            try
            {
                int iReturn = SqlParamDao.ExecuteNonQueryBySql(strSql);
            }
            catch (Exception ex)
            {
                log.Error("Labor 根据工单编号获取其当前状态需要发送订阅消息成功后的处理出错" + ex);
                log.Error("Labor 根据工单编号获取其当前状态需要发送订阅消息成功后的处理出错:strSql:" + strSql);
            }

        }


    }
}
