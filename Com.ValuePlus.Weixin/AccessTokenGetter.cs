using Com.ValuePlus.DAL;
using Com.ValuePlus.Log;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Com.ValuePlus.Weixin
{
    /// <summary>
    /// Access_Token获取类
    /// </summary>
    public class AccessTokenGetter
    {
        /// <summary>
        /// 日志声明
        /// </summary>
        protected static Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 获取最新的有效的AccessToken
        /// </summary>
        /// <param name="strAppId"></param>
        /// <param name="strAppSecret"></param>
        /// <param name="strOpTime"></param>
        /// <param name="strSourceType">业务识别号</param>
        /// <returns></returns>
        public static String GetValidAccessToken(String strAppId, String strAppSecret, String strOpTime, String strSourceType)
        {
            String strReturnValue = "";
            try
            {
                //为保证有效性,先更新数据库中的Token状态
                int iUpdateCount = UpdateExpiredAccessToken(strAppId, strAppSecret, strOpTime);

                //再从数据库中获取有效的Token
                String strAccessTokenFromDataBase = GetAccessTokenFromDataBase(strAppId, strAppSecret, strOpTime, strSourceType);
                if (String.IsNullOrEmpty(strAccessTokenFromDataBase))
                {
                    //如果数据库中无效，则从微信接口中获取
                    String strAccessTokenFromWXAPI = GetAccessTokenFromWXAPI(strAppId, strAppSecret, strOpTime);
                    strReturnValue = strAccessTokenFromWXAPI;
                    //同时写入到数据库中
                    int iSaveCount = SaveAccessTokenToDataBase(strAppId, strAppSecret, strOpTime, strSourceType, strAccessTokenFromWXAPI);
                }
                else
                {
                    //如果数据库存在有效Token,则直接返回
                    strReturnValue = strAccessTokenFromDataBase;

                }
            }
            catch (Exception ex)
            {
                log.Error("获取最新的有效的AccessToken【GetValidAccessToken】失败:" + ex.ToString());
            }
            return strReturnValue;
        }

        /// <summary>
        /// 公众号开发者ID(AppID)从平台数据库中获取已经生成的AccessToken
        /// </summary>
        /// <param name="strAppId"></param>
        /// <param name="strAppSecret"></param>
        /// <param name="strOpTime"></param>
        /// <param name="strSourceType">业务识别号</param>
        /// <returns></returns>
        private static string GetAccessTokenFromDataBase(String strAppId,String strAppSecret,String strOpTime, String strSourceType)
        {
            String strReturnValue = "";
            try
            {
                StringBuilder sbSql = new StringBuilder();
                sbSql.Append("select top 1 * from WXAccessToken_1 where AppId = '" + strAppId + "' and AppSecret = '"+ strAppSecret + "' ");
                sbSql.Append(" and '"+ strOpTime + "' < convert(varchar(30),convert(datetime,ExpiredTime),120) ");
                sbSql.Append(" and [IsExpired] <> '1' order by ExpiredTime desc");
                string strSql = sbSql.ToString();
                DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    strReturnValue = dr["AccessToken"].ToString();
                    String strGUID = dr["GUID"].ToString();

                    //同时写入数据库中现有AccessToken的调用记录
                    SaveGetRecordToDataBase(strGUID, strOpTime, strSourceType);

                }
            }
            catch (Exception ex)
            {
                log.Error("公众号开发者ID(AppID)从平台数据库中获取已经生成的AccessToken【GetAccessTokenFromDataBase】失败:" + ex.ToString());
            }
            return strReturnValue;
        }

        /// <summary>
        /// 写入数据库中现有AccessToken的调用记录
        /// </summary>
        /// <param name="strAppId"></param>
        /// <param name="strAppSecret"></param>
        /// <param name="strOpTime"></param>
        /// <param name="strSourceType">业务识别号</param>
        /// <returns></returns>
        private static int SaveGetRecordToDataBase(String strGUID, String strOpTime, String strSourceType)
        {
            int iCount = 0;
            try
            {
                StringBuilder sbSql = new StringBuilder();
                sbSql.Append("INSERT INTO [WXAccessToken_2]([GUID],[SEQNO],[GetSourceType],[GetTime]) values");
                sbSql.Append(" ('" + strGUID + "'");
                sbSql.Append(" ,(ISNULL((select MAX(convert(int,SEQNO)) from [WXAccessToken_2] where [GUID] = '" + strGUID + "'),0)+1)");
                sbSql.Append(" ,'" + strSourceType + "','" + strOpTime + "')");

                string strSql = sbSql.ToString();
                iCount = SqlParamDao.ExecuteNonQueryBySql(strSql);

            }
            catch (Exception ex)
            {
                iCount = -1;
                log.Error("写入数据库中现有AccessToken的调用记录【SaveGetRecordToDataBase】失败:" + ex.ToString());
            }
            return iCount;
        }
        
        /// <summary>
        /// 保存新生成的AccessToken到数据库中
        /// </summary>
        /// <param name="strAppId"></param>
        /// <param name="strAppSecret"></param>
        /// <param name="strOpTime"></param>
        /// <param name="strSourceType">业务识别号</param>
        /// <param name="strAccessToken"></param>
        /// <returns></returns>
        private static int SaveAccessTokenToDataBase(String strAppId, String strAppSecret, String strOpTime,String strSourceType,String strAccessToken)
        {
            int iCount = 0;
            try
            {
                //失效时间，微信默认为2个小时/120分钟/72000秒，此处设置失效时间为100分钟/6000秒
                String strExpireTime = DateTime.Parse(strOpTime).AddMinutes(100).ToString("yyyy-MM-dd HH:mm:ss");
                String strNewGUID = System.Guid.NewGuid().ToString();

                StringBuilder sbSql = new StringBuilder();
                sbSql.Append("INSERT INTO [WXAccessToken_1]([GUID],[AppId],[AppSecret],[GetSourceType],[GetTime],[ExpiredTime],[IsExpired],[AccessToken]) values");
                sbSql.Append(" ('" + strNewGUID + "','"+ strAppId + "','"+ strAppSecret + "','"+ strSourceType + "','"+ strOpTime + "','"+ strExpireTime + "','2','"+ strAccessToken + "')");
                //顺便更新其他的为无效状态
                sbSql.Append(";Update [WXAccessToken_1] set [IsExpired] = '1' where [GUID] <> '"+ strNewGUID + "'");

                string strSql = sbSql.ToString();
                iCount = SqlParamDao.ExecuteNonQueryBySql(strSql);

            }
            catch (Exception ex)
            {
                iCount = -1;
                log.Error("保存新生成的AccessToken到数据库中【SaveAccessTokenToDataBase】失败:" + ex.ToString());
            }
            return iCount;
        }

        /// <summary>
        /// 根据失效时间更新是否失效状态
        /// 目前操作时间大于失效时间点的所有已保存的token都设置为失效
        /// </summary>
        /// <param name="strAppId"></param>
        /// <param name="strAppSecret"></param>
        /// <param name="strOpTime"></param>
        private static int UpdateExpiredAccessToken(String strAppId, String strAppSecret, String strOpTime)
        {
            int iCount = 0;
            try
            {
                StringBuilder sbSql = new StringBuilder();
                sbSql.Append("update WXAccessToken_1 set IsExpired = '1' where AppId = '" + strAppId + "' and AppSecret = '" + strAppSecret + "' ");
                sbSql.Append(" and '"+ strOpTime + "' >= convert(varchar(30),convert(datetime,ExpiredTime),120) ");
                string strSql = sbSql.ToString();
                iCount = SqlParamDao.ExecuteNonQueryBySql(strSql);

            }
            catch (Exception ex)
            {
                iCount = -1;
                log.Error("根据失效时间更新是否失效状态【UpdateExpiredAccessToken】失败:" + ex.ToString());
            }
            return iCount;
        }

        /// <summary>
        /// 公众号开发者ID(AppID)从通过微信API获取新生成的AccessToken
        /// </summary>
        /// <param name="strAppId"></param>
        /// <param name="strAppSecret"></param>
        /// <param name="strOpTime"></param>
        /// <returns></returns>
        private static string GetAccessTokenFromWXAPI(String strAppId, String strAppSecret, String strOpTime)
        {
            String strReturnValue = "";
            try
            {
                EntityToken entityToken = GetAccessTokenEntity(strAppId, strAppSecret, strOpTime);
                if (!entityToken.IsExpired)
                {
                    strReturnValue = entityToken.Access_Token;
                }
                //strReturnValue = PublicApi.GetAccessTokenFromUrl(strAppId, strAppSecret);
            }
            catch (Exception ex)
            {
                log.Error("公众号开发者ID(AppID)从通过微信API获取新生成的AccessToken【GetAccessTokenFromWXAPI】失败:" + ex.ToString());
            }
            return strReturnValue;
        }

        /// <summary>
        /// 公众号开发者ID(AppID)从通过微信API获取新生成的AccessToken实体类Entity
        /// </summary>
        /// <param name="strAppId"></param>
        /// <param name="strAppSecret"></param>
        /// <param name="strOpTime"></param>
        /// <returns></returns>
        private static EntityToken GetAccessTokenEntity(String strAppId, String strAppSecret, String strOpTime)
        {
            EntityToken entityToken = new Weixin.EntityToken();
            try
            {
                String strUrl = string.Format(Const.Weixin_URL_GetBasicAccessToken.ToString(), strAppId, strAppSecret);
                string strJson = HttpRequestHelper.RequestUrl(strUrl);
                string returnToken = JsonHelper.GetJsonValue(strJson, "access_token");
                bool IsExpired = false;
                if (JsonHelper.GetJsonValue(strJson, "errcode") == "42001")
                {
                    IsExpired = true;
                }
                entityToken.Access_Token = JsonHelper.GetJsonValue(strJson, "access_token");
                entityToken.GetTime = strOpTime;
                entityToken.ExpireTime = DateTime.Parse(strOpTime).AddMinutes(120).ToString("yyyy-MM-dd HH:mm:ss");
                entityToken.IsExpired = IsExpired;
            }
            catch (Exception ex)
            {
                log.Error("公众号开发者ID(AppID)从通过微信API获取新生成的AccessToken实体类Entity【GetAccessTokenEntity】失败:" + ex.ToString());
            }
            return entityToken;
        }

    }
}
