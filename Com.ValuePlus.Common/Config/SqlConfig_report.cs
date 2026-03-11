using System;
using System.Collections;
using System.Text;
using System.IO;
using Com.ValuePlus.Utils.Cryptography;

namespace Com.ValuePlus.Common.Config
{
    /// <summary>
    /// 配置文件读取业务类
    /// </summary>
    public sealed class SqlConfig_report
    {
        #region 对类SqlConfig_report自身的初始化
        /// <summary>
        /// 自身对象实例
        /// </summary>
        private static SqlConfig_report instance = new SqlConfig_report();


        /// <summary>
        /// 配置缓存
        /// </summary>
        private Com.ValuePlus.Config.IConfigManager ConfigCache = null;


        /// <summary>
        /// 加此私有构造函数，防止此类对象通过new对象实例化
        /// </summary>
        private SqlConfig_report()
        {
            //配置文件
            ConfigCache = Com.ValuePlus.Config.ConfigFactory.GetConfigManager();
            ConfigCache.Load(@"bin\SqlBasicMetaData_Report.config", Com.ValuePlus.Config.FileTypeEnum.SqlXmlType);
        }

        /// <summary>
        /// 获得此对象的实例
        /// </summary>
        public static SqlConfig_report Instance
        {
            get
            {
                return instance;
            }
        }
        #endregion 对类SqlConfig_view自身的初始化

        #region 根据用户标识查询视图VW_HR_UP1记录
        /// <summary>
        /// 获取sql语句【根据用户标识查询视图VW_HR_UP1记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_VW_HR_UP1_select_byUserCode()
        {
            return ConfigCache.GetSqlBasicMetaData("VW_HR_UP1.select.byUserCode");
        }
        #endregion

        #region 根据主键查询数据表【TB_HRTMPSD】的一条记录
        /// <summary>
        /// 获取sql语句【根据主键查询数据表【TB_HRTMPSD】的一条记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_TB_HRTMPSD_select_byKey()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_HRTMPSD.select.byKey");
        }
        #endregion
    }


}
