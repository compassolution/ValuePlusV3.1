using System;
using System.Collections;
using System.Text;
using System.IO;
using Com.ValuePlus.Utils.Cryptography;
using Com.ValuePlus.Utils;

namespace Com.ValuePlus.Common.Config
{
    /// <summary>
    /// 配置文件读取业务类
    /// </summary>
    public sealed class SqlConfig_wsm
    {
        #region 对类SqlConfig_wsm自身的初始化
        /// <summary>
        /// 自身对象实例
        /// </summary>
        private static SqlConfig_wsm instance = new SqlConfig_wsm();


        /// <summary>
        /// 配置缓存
        /// </summary>
        private Com.ValuePlus.Config.IConfigManager ConfigCache = null;


        /// <summary>
        /// 加此私有构造函数，防止此类对象通过new对象实例化
        /// </summary>
        private SqlConfig_wsm()
        {
            //配置文件
            ConfigCache = Com.ValuePlus.Config.ConfigFactory.GetConfigManager();
            ConfigCache.Load(@"bin\SqlBasicMetaData_WSM.config", Com.ValuePlus.Config.FileTypeEnum.SqlXmlType);
        }

        /// <summary>
        /// 获得此对象的实例
        /// </summary>
        public static SqlConfig_wsm Instance
        {
            get
            {
                return instance;
            }
        }
        #endregion 对类SqlConfig_wsm自身的初始化

        #region 对表TB_HRLSTH的数据操作
        /// <summary>
        /// 获取sql语句【获取表TB_HRLSTH所有记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_TB_HRLSTH_selectAll()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_HRLSTH.selectAll");
        }

        /// <summary>
        /// 获取sql语句【根据主键获取表TB_HRLSTH一条记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_TB_HRLSTH_select_byLid()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_HRLSTH.select.byLid");
        }

        /// <summary>
        /// 获取sql语句【插入表TB_HRLSTH一条记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_TB_HRLSTH_insert()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_HRLSTH.insert");
        }

        /// <summary>
        /// 获取sql语句【根据表TB_HRLSTH删除记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_TB_HRLSTH_delete_byLid()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_HRLSTH.delete.byLid");
        }

        /// <summary>
        /// 获取sql语句【根据表TB_HRLSTH主键更新记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_TB_HRLSTH_update_byLid()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_HRLSTH.update.byLid");
        }
        #endregion 对表TB_HRLSTH的数据操作

        #region 对表TB_HRLSTD的数据操作
        /// <summary>
        /// 获取sql语句【根据清单定义表ID查询】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_TB_HRLSTD_select_byLid()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_HRLSTD.select.byLid");
        }

        /// <summary>
        /// 获取sql语句【根据清单定义表ID和清单内容明细表ID查询】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_TB_HRLSTD_select_byLidCid()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_HRLSTD.select.byLidCid");
        }

        /// <summary>
        /// 获取sql语句【插入表TB_HRLSTD一条记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_TB_HRLSTD_insert()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_HRLSTD.insert");
        }

        /// <summary>
        /// 获取sql语句【根据清单定义表ID删除表TB_HRLSTD记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_TB_HRLSTD_delete_byLid()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_HRLSTD.delete.byLid");
        }


        /// <summary>
        /// 获取sql语句【根据清单定义表ID和清单内容明细表ID删除表TB_HRLSTD记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_TB_HRLSTD_delete_byLidCid()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_HRLSTD.delete.byLidCid");
        }

        /// <summary>
        /// 获取sql语句【根据根据清单定义表ID和清单内容明细表ID更新表TB_HRLSTD记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_TB_HRLSTD_update_byLidCid()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_HRLSTD.update.byLidCid");
        }
        #endregion 对表TB_HRLSTD的数据操作

        #region 对表TB_HR_MENU的数据操作
        /// <summary>
        /// 获取sql语句【获取表TB_HR_MENU所有记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_TB_HR_MENU_selectAll()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_HR_MENU.selectAll");
        }

        /// <summary>
        /// 获取sql语句【根据主键获取表TB_HR_MENU一条记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_TB_HR_MENU_select_byMenuCode()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_HR_MENU.select.byMenuCode");
        }

        /// <summary>
        /// 获取sql语句【根据栏目编码获取表TB_HR_MENU的子栏目记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_TB_HR_MENU_selectSubLevel_byMenuCode()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_HR_MENU.selectSubLevel.byMenuCode");
        }

        /// <summary>
        /// 获取sql语句【根据栏目编码以及用户编码查询该用户具有的子目录记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_TB_HR_MENU_selectSubLevel_byMenuCodeAUserId()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_HR_MENU.selectSubLevel.byMenuCodeAUserId");
        }

        /// <summary>
        /// 获取sql语句【插入表TB_HR_MENU一条记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_TB_HR_MENU_insert()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_HR_MENU.insert");
        }

        /// <summary>
        /// 获取sql语句【根据表TB_HR_MENU删除记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_TB_HR_MENU_delete_byMenuCode()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_HR_MENU.delete.byMenuCode");
        }

        /// <summary>
        /// 获取sql语句【根据表TB_HR_MENU主键更新记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_TB_HR_MENU_update_byMenuCode()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_HR_MENU.update.byMenuCode");
        }

        /// <summary>
        /// 获取sql语句【根据顺序号获取表TB_HR_MENU记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_TB_HR_MENU_select_byOrder()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_HR_MENU.select.byOrder");
        }
        /// <summary>
        /// 获取sql语句【根据父栏目顺序号查找表TB_HR_MENU中所有子栏目】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_TB_HR_MENU_selectSubOrder_byParentOrder()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_HR_MENU.selectSubOrder.byParentOrder");
        }
        /// <summary>
        /// 获取sql语句【根据旧顺序号将TB_HR_MENU中相关记录的顺序号更新成新顺序号】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_TB_HR_MENU_updateOrder_byOrder()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_HR_MENU.updateOrder.byOrder");
        }


        #endregion 对表TB_HR_MENU的数据操作

        #region 对表TB_HR_USERMENU的数据操作
        /// <summary>
        /// 获取sql语句【根据主键获取表TB_HR_USERMENU记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_TB_HR_USERMENU_selectById()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_HR_USERMENU.select");
        }

        /// <summary>
        /// 获取sql语句【根据用户ID获取表TB_HR_USERMENU记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_TB_HR_USERMENU_select_byUserId()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_HR_USERMENU.select.byUserId");
        }

        /// <summary>
        /// 获取sql语句【根据栏目编码获取表TB_HR_USERMENU】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_TB_HR_USERMENU_select_byMenuCode()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_HR_USERMENU.select.byMenuCode");
        }

        /// <summary>
        /// 获取sql语句【插入表TB_HR_USERMENU一条记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_TB_HR_USERMENU_insert()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_HR_USERMENU.insert");
        }

        /// <summary>
        /// 获取sql语句【根据用户ID删除表TB_HR_USERMENU记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_TB_HR_USERMENU_delete_byUserId()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_HR_USERMENU.delete.byUserId");
        }

        /// <summary>
        /// 获取sql语句【根据栏目编码删除表TB_HR_USERMENU记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_TB_HR_USERMENU_delete_byMenuCode()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_HR_USERMENU.delete.byMenuCode");
        }

        /// <summary>
        /// 获取sql语句【根据用户ID更新表TB_HR_USERMENU记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_TB_HR_USERMENU_update_byUserId()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_HR_USERMENU.update.byUserId");
        }

        /// <summary>
        /// 获取sql语句【根据栏目编码更新表TB_HR_USERMENU记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_TB_HR_USERMENU_update_byMenuCode()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_HR_USERMENU.update.byMenuCode");
        }
        #endregion 对表TB_HR_USERMENU的数据操作

        #region 对表TB_HR_USER的数据操作
        /// <summary>
        /// 获取sql语句【获取表TB_HR_USER所有记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_TB_HR_USER_selectAll()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_HR_USER.selectAll");
        }

        /// <summary>
        /// 获取sql语句【根据主键获取表TB_HR_USER一条记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_TB_HR_USER_select_byUserId()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_HR_USER.select.byUserId");
        }

        /// <summary>
        /// 获取sql语句【根据用户登录帐号获取表TB_HR_USER一条记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_TB_HR_USER_select_byAccountId()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_HR_USER.select.byAccountId");
        }

        /// <summary>
        /// 获取sql语句【插入表TB_HR_USER一条记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_TB_HR_USER_insert()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_HR_USER.insert");
        }

        /// <summary>
        /// 获取sql语句【根据表TB_HR_USER删除记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_TB_HR_USER_delete_byUserId()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_HR_USER.delete.byUserId");
        }

        /// <summary>
        /// 获取sql语句【根据表TB_HR_USER主键更新记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_TB_HR_USER_update_byUserId()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_HR_USER.update.byUserId");
        }

        /// <summary>
        /// 获取sql语句【修改表TB_HR_USER中密码字段】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_TB_HR_USER_changPwd_byUserId()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_HR_USER.changePwd.byUserId");
        }
        #endregion 对表TB_HR_USER的数据操作

        #region 对表TB_HR_USERROLE的数据操作
        /// <summary>
        /// 获取sql语句【根据用户ID获取表TB_HR_USERROLE记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_TB_HR_USERROLE_select_byUserId()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_HR_USERROLE.select.byUserId");
        }

        /// <summary>
        /// 获取sql语句【插入表TB_HR_USERROLE一条记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_TB_HR_USERROLE_insert()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_HR_USERROLE.insert");
        }

        /// <summary>
        /// 获取sql语句【根据用户ID删除表TB_HR_USERROLE记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_TB_HR_USERROLE_delete_byUserId()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_HR_USERROLE.delete.byUserId");
        }

        /// <summary>
        /// 获取sql语句【根据用户ID更新表TB_HR_USERROLE记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_TB_HR_USERROLE_update_byUserId()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_HR_USERROLE.update.byUserId");
        }
        #endregion

        #region 对表TB_HRTMPR的数据操作
        /// <summary>
        /// 获取sql语句【查询表TB_HRTMPR的所有记录的部分字段】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_TB_HRTMPR_select_part()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_HRTMPR.select.part");
        }

        /// <summary>
        /// 获取sql语句【根据用户ID获取表TB_HRTMPR的相关记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_TB_HRTMPR_select_byUserId()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_HRTMPR.select.byUserId");
        }

        /// <summary>
        /// 获取sql语句【根据获取表TB_HRTMPR中所有记录部分字段，同时过滤掉已经存在于特定用户的角色】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_TB_HRTMPR_select_partNotIn_byUserId()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_HRTMPR.select.partNotIn.byUserId");
        }

        /// <summary>
        /// 获取sql语句【根据当前用户ID查询该用户角色且要设置的用户不具备的角色的详细信息】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_TB_HRTMPR_select_byCurUserIdAndSelUserId()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_HRTMPR.select.byCurUserIdAndSelUserId");
        }
        #endregion

        #region 对表TB_HRAUTO的数据操作
        /// <summary>
        /// 获取sql语句【获取表TB_HRAUTO所有记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_TB_HRAUTO_selectAll()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_HRAUTO.selectAll");
        }

        /// <summary>
        /// 获取sql语句【根据主键获取表TB_HRAUTO一条记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_TB_HRAUTO_select_byAid()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_HRAUTO.select.byAid");
        }

        /// <summary>
        /// 获取sql语句【插入表TB_HRAUTO一条记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_TB_HRAUTO_insert()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_HRAUTO.insert");
        }

        /// <summary>
        /// 获取sql语句【根据表TB_HRAUTO删除记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_TB_HRAUTO_delete_byAid()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_HRAUTO.delete.byAid");
        }

        /// <summary>
        /// 获取sql语句【根据表TB_HRAUTO主键更新记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_TB_HRAUTO_update_byAid()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_HRAUTO.update.byAid");
        }
        #endregion 对表TB_HRAUTO的数据操作

        #region 对表KQPERD_1(期间设定)的数据操作
        /// <summary>
        /// 获取sql语句【获取表KQPERD_1所有记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_KQPERD_1_selectAll()
        {
            return ConfigCache.GetSqlBasicMetaData("KQPERD_1.selectAll");
        }
        /// <summary>
        /// 根据PID获取数据表【KQPERD_1】相应记录集
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_KQPERD_1_select_byId()
        {
            return ConfigCache.GetSqlBasicMetaData("KQPERD_1.select.byId");
        }
        #endregion

        #region 对表KQSHIF_1(班次类型)的数据操作
        /// <summary>
        /// 获取sql语句【获取表KQSHIF_1相应查询列】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_KQSHIF_1_select_getCol()
        {
            return ConfigCache.GetSqlBasicMetaData("KQSHIF_1.select.getCol");
        }

        /// <summary>
        /// 获取sql语句【获取表KQSHIF_1所有记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_KQSHIF_1_selectAll()
        {
            return ConfigCache.GetSqlBasicMetaData("KQSHIF_1.selectAll");
        }

        /// <summary>
        /// 获取sql语句【根据用户ID和班次编码以及过滤视图获取数据表【KQSHIF_1】的部分记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_KQSHIF_1_select_ByUserId()
        {
            String strVwName = BaseConfig.Instance.GetConfigValueByKey("VwName_PaibanShift_Filter");
            String strVwParam1 = BaseConfig.Instance.GetConfigValueByKey("VwCol1_PaibanShift_Filter");
            String strVwParam2 = BaseConfig.Instance.GetConfigValueByKey("VwCol2_PaibanShift_Filter");
            SqlBasicMetaData metaData = ConfigCache.GetSqlBasicMetaData("KQSHIF_1.select.ByUserId");
            string sCommandSql = metaData.CommandSql.Replace("[VIEWNAME]", strVwName);
            sCommandSql = sCommandSql.Replace("[SUSERID]", strVwParam1);
            sCommandSql = sCommandSql.Replace("[SHIFTCODE]", strVwParam2);
            metaData.CommandSql = sCommandSql;
            return metaData;
        }        
        #endregion

        #region 对表KQTOE_1(考勤排班表1)的数据操作
        /// <summary>
        /// 获取sql语句【获取表KQTOE_1所有记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_KQTOE_1_selectAll()
        {
            return ConfigCache.GetSqlBasicMetaData("KQTOE_1.selectAll");
        }
        #endregion

        #region 对表KQTOE_2(考勤排班表2)的数据操作
        /// <summary>
        /// 获取sql语句【获取表KQTOE_2所有记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_KQTOE_2_selectAll()
        {
            return ConfigCache.GetSqlBasicMetaData("KQTOE_2.selectAll");
        }

        /// <summary>
        /// 获取sql语句【根据主键获取表KQTOE_2的记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_KQTOE_2_select_byKey()
        {
            return ConfigCache.GetSqlBasicMetaData("KQTOE_2.select.byKey");
        }

        /// <summary>
        /// 获取sql语句【对数据表【KQTOE_1】和【KQTOE_2】的select操作,主要获取查询列表显示列】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_KQTOE_1A2_select_getCol()
        {
            return ConfigCache.GetSqlBasicMetaData("KQTOE_1A2.select.getCol");
        }

        /// <summary>
        /// 获取sql语句【对数据表【KQTOE_1】和【KQTOE_2】的通过YEARMONTH 进行select操作】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_KQTOE_1A2_select_byYearMonth()
        {
            return ConfigCache.GetSqlBasicMetaData("KQTOE_1A2.select.byYearMonth");
        }

        /// <summary>
        /// 获取sql语句【对数据表【KQTOE_1】和【KQTOE_2】的通过YEARMONTH和用户ID 进行select操作】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_KQTOE_1A2_select_byYearMonthAndUserId()
        {
            String strVwName = BaseConfig.Instance.GetConfigValueByKey("VwName_PaibanStaff_Filter");
            String strVwParam1 = BaseConfig.Instance.GetConfigValueByKey("VwCol1_PaibanStaff_Filter");
            String strVwParam2 = BaseConfig.Instance.GetConfigValueByKey("VwCol2_PaibanStaff_Filter");
            SqlBasicMetaData metaData = ConfigCache.GetSqlBasicMetaData("KQTOE_1A2.select.byYearMonthAndUserId");
            string sCommandSql = metaData.CommandSql.Replace("[VIEWNAME]", strVwName);
            sCommandSql = sCommandSql.Replace("[SUSERID]", strVwParam1);
            sCommandSql = sCommandSql.Replace("[STAFFID]", strVwParam2);
            metaData.CommandSql = sCommandSql;
            return metaData;
        }
        
        /// <summary>
        /// 获取sql语句【根据主键更新表KQTOE_2的记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_KQTOE_2_update_byKey()
        {
            return ConfigCache.GetSqlBasicMetaData("KQTOE_2.update.byKey");
        }
        #endregion

        #region 对表TB_HR_KQ_UNNORMAL(非正常考勤记录表)的数据操作【暂时不用】
        /// <summary>
        /// 获取sql语句【根据考勤月份获取表TB_HR_KQ_UNNORMAL记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_TB_HR_KQ_UNNORMAL_select_byYearMonth()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_HR_KQ_UNNORMAL.select.byYearMonth");
        }

        /// <summary>
        /// 获取sql语句【根据员工编号及考勤日期获取表TB_HR_KQ_UNNORMAL记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_TB_HR_KQ_UNNORMAL_select_byNODay()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_HR_KQ_UNNORMAL.select.byNODay");
        }

        /// <summary>
        /// 获取sql语句【根据员工编号及考勤日期更新表TB_HR_KQ_UNNORMAL记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_TB_HR_KQ_UNNORMAL_update_byNODay()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_HR_KQ_UNNORMAL.update.byNODay");
        }
        #endregion

        #region 对表VW_HR_KQ_UNNORMAL(非正常考勤记录表)的数据操作【暂时不用】
        /// <summary>
        /// 获取sql语句【根据考勤月份获取表TB_HR_KQ_UNNORMAL记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_VW_HR_KQ_UNNORMAL_select_byYearMonth()
        {

            String strVwName = BaseConfig.Instance.GetConfigValueByKey("VwName_UnNormalKQ_Filter");
            String strVwParam1 = BaseConfig.Instance.GetConfigValueByKey("VwCol1_UnNormalKQ_Filter");
            String strVwParam2 = BaseConfig.Instance.GetConfigValueByKey("VwCol2_UnNormalKQ_Filter");
            String strVwParam3 = BaseConfig.Instance.GetConfigValueByKey("VwCol3_UnNormalKQ_Filter");
            String strVwParam4 = BaseConfig.Instance.GetConfigValueByKey("VwCol4_UnNormalKQ_Filter");

            SqlBasicMetaData metaData = ConfigCache.GetSqlBasicMetaData("VW_HR_KQ_UNNORMAL.select.byYearMonth");

            string sCommandSql = metaData.CommandSql.Replace("[VIEWNAME]", strVwName);
            sCommandSql = sCommandSql.Replace("[SSTAFFNO]", strVwParam1);
            sCommandSql = sCommandSql.Replace("[SYEARMONTH]", strVwParam2);
            sCommandSql = sCommandSql.Replace("[SDAY]", strVwParam3);
            sCommandSql = sCommandSql.Replace("[SSTATE]", strVwParam4);
            metaData.CommandSql = sCommandSql;
            return metaData;

        }
        #endregion

        #region 对表对数据视图【VW_PAIBAN_STAFF_FILTER】的数据操作
        /// <summary>
        /// 获取sql语句【对数据视图【VW_PAIBAN_STAFF_FILTER】的通过用户ID 进行select操作】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_VW_PAIBAN_STAFF_FILTER_select_byUserId()
        {
            String strVwName = BaseConfig.Instance.GetConfigValueByKey("VwName_PaibanStaff_Filter");
            String strVwParam1 = BaseConfig.Instance.GetConfigValueByKey("VwCol1_PaibanStaff_Filter");
            SqlBasicMetaData metaData = ConfigCache.GetSqlBasicMetaData("VW_PAIBAN_STAFF_FILTER.select.byUserId");
            string sCommandSql = metaData.CommandSql.Replace("[VIEWNAME]", strVwName);
            sCommandSql = sCommandSql.Replace("[SUSERID]", strVwParam1);
            metaData.CommandSql = sCommandSql;
            return metaData;
        }
        #endregion

        #region 对表KQRSSZ_2(考勤结果记录表)的数据操作
        /// <summary>
        /// 获取sql语句【对数据表【KQRSSZ_1】和【KQRSSZ_2】的select操作,,主要获取查询列表显示列】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_KQRSSZ_1A2_select_getCol()
        {
            return ConfigCache.GetSqlBasicMetaData("KQRSSZ_1A2.select.getCol");
        }

        /// <summary>
        /// 获取sql语句【对数据表【KQRSSZ_1】和【KQRSSZ_2】的通过员工编号，月份及日期select操作】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_KQRSSZ_1A2_select_byNoDate()
        {
            return ConfigCache.GetSqlBasicMetaData("KQRSSZ_1A2.select.byNoDate");
        }

        /// <summary>
        /// 获取sql语句【根据用户ID对数据表【KQRSSZ_1】和【KQRSSZ_2】的通过员工编号，月份及日期select操作】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_KQRSSZ_1A2_select_byNoDateAUserId()
        {
            String strVwName = BaseConfig.Instance.GetConfigValueByKey("VwName_PaibanStaff_Filter");
            String strVwParam1 = BaseConfig.Instance.GetConfigValueByKey("VwCol1_PaibanStaff_Filter");
            String strVwParam2 = BaseConfig.Instance.GetConfigValueByKey("VwCol2_PaibanStaff_Filter");
            SqlBasicMetaData metaData = ConfigCache.GetSqlBasicMetaData("KQRSSZ_1A2.select.byNoDateAUserId");
            string sCommandSql = metaData.CommandSql.Replace("[VIEWNAME]", strVwName);
            sCommandSql = sCommandSql.Replace("[SUSERID]", strVwParam1);
            sCommandSql = sCommandSql.Replace("[STAFFID]", strVwParam2);
            metaData.CommandSql = sCommandSql;
            return metaData;
        }

        /// <summary>
        /// 获取sql语句【根据用户ID对数据表【KQRSSZ_1】和【KQRSSZ_2】的通过员工编号，月份及日期select总数操作】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_KQRSSZ_1A2_selectCount_byNoDateAUserId()
        {
            String strVwName = BaseConfig.Instance.GetConfigValueByKey("VwName_PaibanStaff_Filter");
            String strVwParam1 = BaseConfig.Instance.GetConfigValueByKey("VwCol1_PaibanStaff_Filter");
            String strVwParam2 = BaseConfig.Instance.GetConfigValueByKey("VwCol2_PaibanStaff_Filter");
            SqlBasicMetaData metaData = ConfigCache.GetSqlBasicMetaData("KQRSSZ_1A2.selectCount.byNoDateAUserId");
            string sCommandSql = metaData.CommandSql.Replace("[VIEWNAME]", strVwName);
            sCommandSql = sCommandSql.Replace("[SUSERID]", strVwParam1);
            sCommandSql = sCommandSql.Replace("[STAFFID]", strVwParam2);
            metaData.CommandSql = sCommandSql;
            return metaData;
        }

        /// <summary>
        /// 分页获取sql语句【根据用户ID对数据表【KQRSSZ_1】和【KQRSSZ_2】的通过员工编号，月份及日期select操作】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_KQRSSZ_1A2_select_byNoDateAUserId_MultiPage()
        {
            String strVwName = BaseConfig.Instance.GetConfigValueByKey("VwName_PaibanStaff_Filter");
            String strVwParam1 = BaseConfig.Instance.GetConfigValueByKey("VwCol1_PaibanStaff_Filter");
            String strVwParam2 = BaseConfig.Instance.GetConfigValueByKey("VwCol2_PaibanStaff_Filter");
            SqlBasicMetaData metaData = ConfigCache.GetSqlBasicMetaData("KQRSSZ_1A2.select.byNoDateAUserId.MultiPage");
            string sCommandSql = metaData.CommandSql.Replace("[VIEWNAME]", strVwName);
            sCommandSql = sCommandSql.Replace("[SUSERID]", strVwParam1);
            sCommandSql = sCommandSql.Replace("[STAFFID]", strVwParam2);
            metaData.CommandSql = sCommandSql;
            return metaData;
        }

        /// <summary>
        /// 获取sql语句【通过员工编号，月份及日期对数据表和【KQRSSZ_2】的update操作】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_KQRSSZ_2_update_byNoDate()
        {
            return ConfigCache.GetSqlBasicMetaData("KQRSSZ_2.update.byNoDate");
        }
        #endregion

        #region 对表TB_VP_REGIST的数据操作
        /// <summary>
        /// 获取sql语句【获取表TB_VP_REGIST所有记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_TB_VP_REGIST_select()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_VP_REGIST.select");
        }

        /// <summary>
        /// 获取sql语句【插入表TB_VP_REGIST一条记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_TB_VP_REGIST_insert()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_VP_REGIST.insert");
        }

        /// <summary>
        /// 获取sql语句【更新表TB_VP_REGIST的记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_TB_VP_REGIST_update()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_VP_REGIST.update");
        }
        #endregion

        #region 对表TB_HR_PUBLIC_NOTICE的数据操作
        /// <summary>
        /// 获取sql语句【获取表TB_HR_PUBLIC_NOTICE所有记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_TB_HR_PUBLIC_NOTICE_selectAll()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_HR_PUBLIC_NOTICE.selectAll");
        }

        /// <summary>
        /// 获取sql语句【根据主键获取表TB_HR_PUBLIC_NOTICE一条记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_TB_HR_PUBLIC_NOTICE_select_byKey()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_HR_PUBLIC_NOTICE.select.ByKey");
        }

        /// <summary>
        /// 获取sql语句【根据顺序号获取表TB_HR_PUBLIC_NOTICE记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_TB_HR_PUBLIC_NOTICE_select_byIsStop()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_HR_PUBLIC_NOTICE.select.ByIsStop");
        }

        /// <summary>
        /// 获取sql语句【插入表TB_HR_PUBLIC_NOTICE一条记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_TB_HR_PUBLIC_NOTICE_insert()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_HR_PUBLIC_NOTICE.insert");
        }

        /// <summary>
        /// 获取sql语句【根据表TB_HR_PUBLIC_NOTICE删除记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_TB_HR_PUBLIC_NOTICE_delete_byKey()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_HR_PUBLIC_NOTICE.delete.ByKey");
        }

        /// <summary>
        /// 获取sql语句【根据表TB_HR_PUBLIC_NOTICE主键更新记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_TB_HR_PUBLIC_NOTICE_update_byKey()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_HR_PUBLIC_NOTICE.update.ByKey");
        }

        #endregion 对表TB_HR_PUBLIC_NOTICE的数据操作

        #region 对表KQOVTM_1和表KQLV_1的数据操作
        /// <summary>
        /// 获取sql语句【获取表KQOVTM_1中与某员工相关的所有记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_KQOVTM_1_select_byStuffId()
        {
            String strOVType = BaseConfig.Instance.GetConfigValueByKey("ot_type");
            SqlBasicMetaData metaData = ConfigCache.GetSqlBasicMetaData("KQOVTM_1.select.byStuffId");
            string sCommandSql = metaData.CommandSql.Replace("[OTTYPE]", strOVType);
            metaData.CommandSql = sCommandSql;
            return metaData;

            //return ConfigCache.GetSqlBasicMetaData("KQOVTM_1.select.byStuffId");
        }

        /// <summary>
        /// 获取sql语句【获取表KQLV_1中与某员工相关的所有记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_KQLV_1_select_byStuffId()
        {
            String strLVType = BaseConfig.Instance.GetConfigValueByKey("lv_type");
            SqlBasicMetaData metaData = ConfigCache.GetSqlBasicMetaData("KQLV_1.select.byStuffId");
            string sCommandSql = metaData.CommandSql.Replace("[LVTYPE]", strLVType);
            metaData.CommandSql = sCommandSql;
            return metaData;

            //return ConfigCache.GetSqlBasicMetaData("KQLV_1.select.byStuffId");
        }

        #endregion

        #region 对数据表【GRROTO_3】系统考勤员用户列表的select操作相关配置文件
        /// <summary>
        /// 获取sql语句【对数据表【GRROTO_3】系统考勤员用户列表的select操作相关配置文件】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSql_GRROTO_3_selectAtt_byId()
        {
            return ConfigCache.GetSqlBasicMetaData("GRROTO_3.selectAtt.byId");
        }
        #endregion


    }
}
