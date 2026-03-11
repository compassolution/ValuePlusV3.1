using Com.ValuePlus.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.ValuePlus.Weixin
{
    /// <summary>
    /// WX User的相关管理类
    /// </summary>
    public class WXUser
    {
        /// <summary>
        /// 日志声明
        /// </summary>
        protected static Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 微信用户访问记录写入
        /// </summary>
        /// <param name="strMobileNo"></param>
        /// <param name="strProjectId"></param>
        /// <param name="strOPDesc"></param>
        public static void RecordWXUser_3(String strMobileNo,String strProjectId,String strOPDesc)
        {
            StringBuilder sbSql = new StringBuilder();
            //新增用户访问记录信息表的写入 add by sammen 20190919
            try
            {
                String strNowTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                sbSql.Append(" insert into WXUser_3 (OpenId,LNO,MobileNo,OPTime,ProjectId,OPDesc) \r\n");
                sbSql.Append(" select top 1 OpenId,isnull((SELECT MAX(LNO)+1 FROM WXUser_3 WHERE OpenId = A.OpenId),1),'" + strMobileNo + "','" + strNowTime + "','" + strProjectId + "','"+ strOPDesc + "'");
                sbSql.Append(" from WXUser_1 A where isnull([MobileNo],'') = '" + strMobileNo + "' order by LastTime desc\r\n");
                log.Error("用户访问记录信息表的写入,脚本：" + sbSql.ToString());
                int iCount = SqlParamDao.ExecuteNonQueryBySql(sbSql.ToString());

            }
            catch (Exception ex)
            {
                log.Error("用户访问记录信息表的写入出错：" + ex);
            }
            //新增用户访问记录信息表的写入 add by sammen 20190919
        }

        #region App_CS中公众号页面暂时没有用到如下方法
        /// <summary>
        /// 插入或者更新WXUser数据主表
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static int RecordWXUser_1(EntityWXUser entity)
        {
            int iReturn = 0;
            StringBuilder sbSql = new StringBuilder();
            try
            {
                String strNowTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                sbSql.Append("if not exists (select * from [WXUser_1] where [OpenId] = '" + entity.OpenId + "') \r\n");
                sbSql.Append("begin \r\n");
                //---如果OpenId不存在则插入一条记录
                sbSql.Append("insert into [WXUser_1] ([OpenId],[NickName],[Sex],[Province],[City],[Country],[HeadImgUrl],[Unionid],[MobileNo],[Password],[SMSCount],[RegTime],[LastTime]) values (");
                sbSql.Append(" '" + entity.OpenId + "'");
                sbSql.Append(",'" + entity.NickName + "'");
                sbSql.Append(",'" + entity.Sex + "'");
                sbSql.Append(",'" + entity.Province + "'");
                sbSql.Append(",'" + entity.City + "'");
                sbSql.Append(",'" + entity.Country + "'");
                sbSql.Append(",'" + entity.HeadImgUrl + "'");
                sbSql.Append(",'" + entity.Unionid + "'");
                sbSql.Append(",'" + entity.MobileNo + "'");
                sbSql.Append(",'" + entity.Password + "'");
                sbSql.Append(",'1'");
                sbSql.Append(",'" + strNowTime + "'");
                sbSql.Append(",'" + strNowTime + "'");
                sbSql.Append(" )");
                sbSql.Append("end \r\n");
                sbSql.Append("else \r\n");
                sbSql.Append("begin \r\n");
                //---如果OpenId存在则更新一条记录
                sbSql.Append(" UPDATE [WXUser_1] SET ");
                sbSql.Append("  [NickName] = '" + entity.NickName + "'");
                sbSql.Append("  ,[Sex] = '" + entity.Sex + "'");
                sbSql.Append("  ,[Province] = '" + entity.Province + "'");
                sbSql.Append("  ,[City] = '" + entity.City + "'");
                sbSql.Append("  ,[Country] = '" + entity.Country + "'");
                sbSql.Append("  ,[HeadImgUrl] = '" + entity.HeadImgUrl + "'");
                sbSql.Append("  ,[Privilege] = '" + entity.Privilege + "'");
                sbSql.Append("  ,[Unionid] = '" + entity.Unionid + "'");
                sbSql.Append("  ,[MobileNo] = '" + entity.MobileNo + "'");
                sbSql.Append("  ,[Password] = '" + entity.Password + "'");
                sbSql.Append("  ,[SMSCount] = '" + entity.SMSCount + "'");
                sbSql.Append("  ,[LastTime] = '" + entity.LastTime + "'");
                sbSql.Append("  where [OpenId] = '" + entity.OpenId + "' \r\n");
                sbSql.Append("end; \r\n");
                iReturn = SqlParamDao.ExecuteNonQueryBySql(sbSql.ToString());
                log.Error("插入或者更新WXUser数据主表成功,脚本：" + sbSql.ToString());
            }
            catch(Exception ex)
            {
                log.Error("插入或者更新WXUser数据主表失败,脚本：" + sbSql.ToString());
            }
            return iReturn;
        }

        /// <summary>
        /// 插入WXUser手机号绑定记录表
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static int RecordWXUser_2(String strOpenId,String strMobileNo)
        {
            int iReturn = 0;
            StringBuilder sbSql = new StringBuilder();
            try
            {
                String strNowTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                sbSql.Append("insert into [WXUser_2] ([OpenId],[LNO],[MobileNo],[RegTime]) values (");
                sbSql.Append(" '" + strOpenId + "'");
                sbSql.Append(",(select ISNULL(max(LNO),0)+1 from WXUser_2 where OpenId = '"+ strOpenId + "')");         
                sbSql.Append(",'" + strMobileNo + "'");
                sbSql.Append(",'" + strNowTime + "'");
                sbSql.Append(" )");
                iReturn = SqlParamDao.ExecuteNonQueryBySql(sbSql.ToString());
                log.Error("插入WXUser手机号绑定记录表成功,脚本：" + sbSql.ToString());
            }
            catch (Exception ex)
            {
                log.Error("插入WXUser手机号绑定记录表失败,脚本：" + sbSql.ToString());
            }
            return iReturn;
        }
        #endregion

    }
}
