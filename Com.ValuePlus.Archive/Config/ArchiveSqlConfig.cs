using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ValuePlus.Archive.Config
{
    public class ArchiveSqlConfig
    {
        #region 对类ArchiveSqlConfig自身的初始化
        /// <summary>
        /// 自身对象实例
        /// </summary>
        private static ArchiveSqlConfig instance = new ArchiveSqlConfig();


        /// <summary>
        /// 配置缓存
        /// </summary>
        private Com.ValuePlus.Config.IConfigManager ConfigCache = null;

        /// <summary>
        /// 加此私有构造函数，防止此类对象通过new对象实例化
        /// </summary>
        private ArchiveSqlConfig()
        {
            ConfigCache = Com.ValuePlus.Config.ConfigFactory.GetConfigManager();
            ConfigCache.Load(@"Archive\Config\ArchiveSqlConfig.config", Com.ValuePlus.Config.FileTypeEnum.SqlXmlType);
        }

        /// <summary>
        /// 获得此对象的实例
        /// </summary>
        public static ArchiveSqlConfig Instance
        {
            get
            {
                return instance;
            }
        }
        #endregion

        
        #region 模板管理相关配置文件获取
        /// <summary>
        /// 模板管理－获得模板
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetTemplate_TB_HRTMPH()
        {
            return ConfigCache.GetSqlBasicMetaData("Template_TB_HRTMPH");
        }


        /// <summary>
        /// 模板管理－更新模板
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetTemplate_TB_HRTMPH_Update()
        {
            return ConfigCache.GetSqlBasicMetaData("Template_TB_HRTMPH_Update");
        }


        /// <summary>
        /// 模板管理－删除模板
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetTemplate_TB_HRTMPH_Delete()
        {
            return ConfigCache.GetSqlBasicMetaData("Template_TB_HRTMPH_Delete");
        }


        /// <summary>
        /// 模板管理－增加模板
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetTemplate_TB_HRTMPH_Add()
        {
            return ConfigCache.GetSqlBasicMetaData("Template_TB_HRTMPH_Add");
        }
        /// <summary>
        /// 模板详情－获得模板
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetTemplate_TB_HRTMPG()
        {
            return ConfigCache.GetSqlBasicMetaData("Template_TB_HRTMPG");
        }


        /// <summary>
        /// 模板详情－更新模板
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetTemplate_TB_HRTMPG_Update()
        {
            return ConfigCache.GetSqlBasicMetaData("Template_TB_HRTMPG_Update");
        }


        /// <summary>
        /// 模板详情－删除模板
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetTemplate_TB_HRTMPG_Delete()
        {
            return ConfigCache.GetSqlBasicMetaData("Template_TB_HRTMPG_Delete");
        }


        /// <summary>
        /// 模板详情－增加模板
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetTemplate_TB_HRTMPG_Add()
        {
            return ConfigCache.GetSqlBasicMetaData("Template_TB_HRTMPG_Add");
        }
        /// <summary>
        /// 模板角色－获得模板
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetTemplate_TB_HRTMPR()
        {
            return ConfigCache.GetSqlBasicMetaData("Template_TB_HRTMPR");
        }


        /// <summary>
        /// 模板角色－更新模板
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetTemplate_TB_HRTMPR_Update()
        {
            return ConfigCache.GetSqlBasicMetaData("Template_TB_HRTMPR_Update");
        }


        /// <summary>
        /// 模板角色－删除模板
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetTemplate_TB_HRTMPR_Delete()
        {
            return ConfigCache.GetSqlBasicMetaData("Template_TB_HRTMPR_Delete");
        }


        /// <summary>
        /// 模板角色－增加模板
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetTemplate_TB_HRTMPR_Add()
        {
            return ConfigCache.GetSqlBasicMetaData("Template_TB_HRTMPR_Add");
        }

        /// <summary>
        /// 模板状态－获得模板
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetTemplate_TB_HRTMPS()
        {
            return ConfigCache.GetSqlBasicMetaData("Template_TB_HRTMPS");
        }


        /// <summary>
        /// 模板状态－更新模板
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetTemplate_TB_HRTMPS_Update()
        {
            return ConfigCache.GetSqlBasicMetaData("Template_TB_HRTMPS_Update");
        }


        /// <summary>
        /// 模板状态－删除模板
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetTemplate_TB_HRTMPS_Delete()
        {
            return ConfigCache.GetSqlBasicMetaData("Template_TB_HRTMPS_Delete");
        }


        /// <summary>
        /// 模板状态－增加模板
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetTemplate_TB_HRTMPS_Add()
        {
            return ConfigCache.GetSqlBasicMetaData("Template_TB_HRTMPS_Add");
        }

        /// <summary>
        /// 模板行为－获得模板
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetTemplate_TB_HRTMPA()
        {
            return ConfigCache.GetSqlBasicMetaData("Template_TB_HRTMPA");
        }


        /// <summary>
        /// 模板行为－更新模板
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetTemplate_TB_HRTMPA_Update()
        {
            return ConfigCache.GetSqlBasicMetaData("Template_TB_HRTMPA_Update");
        }


        /// <summary>
        /// 模板行为－删除模板
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetTemplate_TB_HRTMPA_Delete()
        {
            return ConfigCache.GetSqlBasicMetaData("Template_TB_HRTMPA_Delete");
        }


        /// <summary>
        /// 模板行为－增加模板
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetTemplate_TB_HRTMPA_Add()
        {
            return ConfigCache.GetSqlBasicMetaData("Template_TB_HRTMPA_Add");
        }

        /// <summary>
        /// 模板事件－获得模板
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetTemplate_TB_HRTMPE()
        {
            return ConfigCache.GetSqlBasicMetaData("Template_TB_HRTMPE");
        }


        /// <summary>
        /// 模板事件－更新模板
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetTemplate_TB_HRTMPE_Update()
        {
            return ConfigCache.GetSqlBasicMetaData("Template_TB_HRTMPE_Update");
        }


        /// <summary>
        /// 模板事件－删除模板
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetTemplate_TB_HRTMPE_Delete()
        {
            return ConfigCache.GetSqlBasicMetaData("Template_TB_HRTMPE_Delete");
        }


        /// <summary>
        /// 模板事件－增加模板
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetTemplate_TB_HRTMPE_Add()
        {
            return ConfigCache.GetSqlBasicMetaData("Template_TB_HRTMPE_Add");
        }

        /// <summary>
        /// 模板详情-d－获得模板
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetTemplate_TB_HRTMPD_D()
        {
            return ConfigCache.GetSqlBasicMetaData("Template_TB_HRTMPD_D");
        }


        /// <summary>
        /// 模板详情-d－更新模板
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetTemplate_TB_HRTMPD_D_Update()
        {
            return ConfigCache.GetSqlBasicMetaData("Template_TB_HRTMPD_D_Update");
        }


        /// <summary>
        /// 模板详情-d－删除模板
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetTemplate_TB_HRTMPD_D_Delete()
        {
            return ConfigCache.GetSqlBasicMetaData("Template_TB_HRTMPD_D_Delete");
        }


        /// <summary>
        /// 模板详情-d－增加模板
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetTemplate_TB_HRTMPD_D_Add()
        {
            return ConfigCache.GetSqlBasicMetaData("Template_TB_HRTMPD_D_Add");
        }

        /// <summary>
        /// 模板系统参数－D
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetTemplate_TB_HRTMPDSys_D()
        {
            return ConfigCache.GetSqlBasicMetaData("Template_TB_HRTMPDSys_D");
        }
        /// <summary>
        /// 模板系统字体－D
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetTemplate_TB_HRTMPDFont_D()
        {
            return ConfigCache.GetSqlBasicMetaData("Template_TB_HRTMPDFont_D");
        }
        /// <summary>
        /// 票据获得模板详情定义
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetTemplate_TB_DocumentTemplateDefine()
        {
            return ConfigCache.GetSqlBasicMetaData("Template_TB_DocumentTemplateDefine");
        }
        /// <summary>
        /// 票据获得模板SA
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetTemplate_TB_HRTMPSA_Document()
        {
            return ConfigCache.GetSqlBasicMetaData("Template_TB_HRTMPSA_Document");
        }
        /// <summary>
        /// 票据获得模板AR
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetTemplate_TB_HRTMPAR_Document()
        {
            return ConfigCache.GetSqlBasicMetaData("Template_TB_HRTMPAR_Document");
        }
        /// <summary>
        /// 票据获得模板SG
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetTemplate_TB_HRTMPSG_Document()
        {
            return ConfigCache.GetSqlBasicMetaData("Template_TB_HRTMPSG_Document");
        }

        /// <summary>
        /// 票据获得模板SD
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetTemplate_TB_HRTMPSD_Document()
        {
            return ConfigCache.GetSqlBasicMetaData("Template_TB_HRTMPSD_Document");
        }
        #endregion

    }
}
