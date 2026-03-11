using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ValuePlus.Flow.Config
{
    public sealed class FlowSqlConfig
    {
        
        #region 对类FlowSqlConfig自身的初始化
        /// <summary>
        /// 自身对象实例
        /// </summary>
        private static FlowSqlConfig instance = new FlowSqlConfig();


        /// <summary>
        /// 配置缓存
        /// </summary>
        private Com.ValuePlus.Config.IConfigManager ConfigCache = null;

        /// <summary>
        /// 加此私有构造函数，防止此类对象通过new对象实例化
        /// </summary>
        private FlowSqlConfig()
        {
            ConfigCache = Com.ValuePlus.Config.ConfigFactory.GetConfigManager();
            ConfigCache.Load(@"Flow\Config\FlowSqlConfig.config", Com.ValuePlus.Config.FileTypeEnum.SqlXmlType);
        }

        /// <summary>
        /// 获得此对象的实例
        /// </summary>
        public static FlowSqlConfig Instance
        {
            get
            {
                return instance;
            }
        }
        #endregion

        #region 获取表【TB_FORM_DEFINE】相关sql语句的配置项

        /// <summary>
        /// 获取sql语句【获取表TB_FORM_DEFINE所有记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSqlForTB_FORM_DEFINE_selectAll()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_FORM_DEFINE.selectAll");
        }
        
        /// <summary>
        /// 获取sql语句【根据ID获取表TB_FORM_DEFINE的一条记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSqlForTB_FORM_DEFINE_selectByKey()
        {

            return ConfigCache.GetSqlBasicMetaData("TB_FORM_DEFINE.selectByKey");
        }
        /// <summary>
        /// 获取sql语句【根据ID获取表TB_FORM_DEFINE的记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSqlForTB_FORM_DEFINE_selectByCode()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_FORM_DEFINE.selectByCode");
        }
        /// <summary>
        /// 获取sql语句【新增表TB_FORM_DEFINE表的一条记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSqlForTB_FORM_DEFINE_insert()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_FORM_DEFINE.insert");
        }
        /// <summary>
        /// 获取sql语句【根据ID更新表TB_FORM_DEFINE表的一条记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSqlForTB_FORM_DEFINE_updateByKey()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_FORM_DEFINE.updateByKey");
        }
        /// <summary>
        /// 获取sql语句【根据ID删除表TB_FORM_DEFINE表的一条记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSqlForTB_FORM_DEFINE_deleteByKey()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_FORM_DEFINE.deleteByKey");
        }

        #endregion

        #region 获取表【TB_FORM_FIELD】相关sql语句的配置项
        /// <summary>
        /// 获取sql语句【获取表TB_FORM_FIELD所有记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSqlForTB_FORM_FIELD_selectAll()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_FORM_FIELD.selectAll");
        }
        /// <summary>
        /// 获取sql语句【根据主键获取表TB_FORM_FIELD一条记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSqlForTB_FORM_FIELD_selectByKey()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_FORM_FIELD.selectByKey");
        }
        /// <summary>
        /// 获取sql语句【根据外键获取表TB_FORM_FIELD记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSqlForTB_FORM_FIELD_selectByFormId()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_FORM_FIELD.selectByFormId");
        }
        /// <summary>
        /// 获取sql语句【根据主键和外键一起获取表TB_FORM_FIELD记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSqlForTB_FORM_FIELD_selectByKeyAFormId()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_FORM_FIELD.selectByKeyAFormId"); 
        }
        /// <summary>
        /// 获取sql语句【添加一条记录到表TB_FORM_FIELD中】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSqlForTB_FORM_FIELD_insert()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_FORM_FIELD.insert");
        }
        /// <summary>
        /// 获取sql语句【根据主键更新表TB_FORM_FIELD一条记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSqlForTB_FORM_FIELD_updateByKey()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_FORM_FIELD.updateByKey");
        }
        /// <summary>
        /// 获取sql语句【根据主键删除表TB_FORM_FIELD一条记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSqlForTB_FORM_FIELD_deleteByKey()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_FORM_FIELD.deleteByKey");
        }

        #endregion


        #region 获取表【TB_FLOW_WORK_INSTANCE】相关sql语句的配置项
        /// <summary>
        /// 获取sql语句【获取表TB_FLOW_WORK_INSTANCE所有记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSqlForTB_FLOW_WORK_INSTANCE_selectAll()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_FLOW_WORK_INSTANCE.selectAll");
        }
        /// <summary>
        /// 获取sql语句【根据主键获取表TB_FLOW_WORK_INSTANCE一条记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSqlForTB_FLOW_WORK_INSTANCE_selectByKey()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_FLOW_WORK_INSTANCE.selectByKey");
        }
        /// <summary>
        /// 获取sql语句【添加一条记录到表TB_FLOW_WORK_INSTANCE中】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSqlForTB_FLOW_WORK_INSTANCE_insert()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_FLOW_WORK_INSTANCE.insert");
        }
        /// <summary>
        /// 获取sql语句【根据主键更新表TB_FLOW_WORK_INSTANCE一条记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSqlForTB_FLOW_WORK_INSTANCE_updateByKey()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_FLOW_WORK_INSTANCE.updateByKey");
        }
        /// <summary>
        /// 获取sql语句【根据主键删除表TB_FLOW_WORK_INSTANCE一条记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSqlForTB_FLOW_WORK_INSTANCE_deleteByKey()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_FLOW_WORK_INSTANCE.deleteByKey");
        }

        /// <summary>
        /// 获取sql语句【对数据表【TB_FLOW_WORK_INSTANCE】和【TB_FLOW_WORKFLOW_DETAIL】的selectByUserid操作相关配置文件（待办事项查询）】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSqlForTB_FLOW_WORK_INSTANCE_selectPendingByUserId()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_FLOW_WORK_INSTANCE.selectPendingByUserId");
        }
        /// <summary>
        /// 获取sql语句【同时对数据表【TB_FLOW_WORK_INSTANCE】和【TB_FLOW_WORKFLOW_DETAIL】的流程转交状态编码操作相关配置文件】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSqlForTB_FLOW_WORK_INSTANCE_TB_FLOW_WORKFLOW_DETAIL_updateMoveStateByFlowId()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_FLOW_WORK_INSTANCE.TB_FLOW_WORKFLOW_DETAIL.updateMoveStateByFlowId");
        }
        /// <summary>
        /// 获取sql语句【同时对数据表【TB_FLOW_WORK_INSTANCE】和【TB_FLOW_WORKFLOW_DETAIL】的流程状态编码操作相关配置文件】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSqlForTB_FLOW_WORK_INSTANCE_TB_FLOW_WORKFLOW_DETAIL_updateFlowStateByFlowId()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_FLOW_WORK_INSTANCE.TB_FLOW_WORKFLOW_DETAIL.updateFlowStateByFlowId");
        }
        /// <summary>
        /// 获取sql语句【同时对数据表【TB_FLOW_WORK_INSTANCE】和【TB_FLOW_WORKFLOW_DETAIL】的流程业务状态操作相关配置文件】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSqlForTB_FLOW_WORK_INSTANCE_TB_FLOW_WORKFLOW_DETAIL_updateBizStateByFlowId()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_FLOW_WORK_INSTANCE.TB_FLOW_WORKFLOW_DETAIL.updateBizStateByFlowId");
        }
        #endregion

        #region 获取表【TB_FLOW_DEFINE】相关sql语句的配置项
        /// <summary>
        /// 获取sql语句【获取表TB_FLOW_DEFINE所有记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSqlForTB_FLOW_DEFINE_selectAll()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_FLOW_DEFINE.selectAll");
        }
        /// <summary>
        /// 获取sql语句【根据主键获取表TB_FLOW_DEFINE一条记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSqlForTB_FLOW_DEFINE_selectByKey()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_FLOW_DEFINE.selectByKey");
        }
        /// <summary>
        /// 获取sql语句【添加一条记录到表TB_FLOW_DEFINE中】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSqlForTB_FLOW_DEFINE_insert()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_FLOW_DEFINE.insert");
        }
        /// <summary>
        /// 获取sql语句【根据主键更新表TB_FLOW_DEFINE一条记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSqlForTB_FLOW_DEFINE_updateByKey()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_FLOW_DEFINE.updateByKey");
        }
        /// <summary>
        /// 获取sql语句【根据主键删除表TB_FLOW_DEFINE一条记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSqlForTB_FLOW_DEFINE_deleteByKey()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_FLOW_DEFINE.deleteByKey");
        }
        /// <summary>
        /// 获取sql语句【根据流程实例ID获取表TB_FLOW_DEFINE的一条记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSqlForTB_FLOW_DEFINE_selectByWorkFlowCode()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_FLOW_DEFINE.selectByWorkFlowCode");
        }
        #endregion

        #region 获取表【TB_FLOW_WORKFLOW_DETAIL】相关sql语句的配置项
        /// <summary>
        /// 获取sql语句【获取表TB_FLOW_WORKFLOW_DETAIL所有记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSqlForTB_FLOW_WORKFLOW_DETAIL_selectAll()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_FLOW_WORKFLOW_DETAIL.selectAll");
        }
        /// <summary>
        /// 获取sql语句【根据主键获取表TB_FLOW_WORKFLOW_DETAIL一条记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSqlForTB_FLOW_WORKFLOW_DETAIL_selectByKey()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_FLOW_WORKFLOW_DETAIL.selectByKey");
        }
        /// <summary>
        /// 获取sql语句【添加一条记录到表TB_FLOW_WORKFLOW_DETAIL中】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSqlForTB_FLOW_WORKFLOW_DETAIL_insert()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_FLOW_WORKFLOW_DETAIL.insert");
        }
        /// <summary>
        /// 获取sql语句【根据主键更新表TB_FLOW_WORKFLOW_DETAIL一条记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSqlForTB_FLOW_WORKFLOW_DETAIL_updateByKey()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_FLOW_WORKFLOW_DETAIL.updateByKey");
        }
        /// <summary>
        /// 获取sql语句【根据主键删除表TB_FLOW_WORKFLOW_DETAIL一条记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSqlForTB_FLOW_WORKFLOW_DETAIL_deleteByKey()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_FLOW_WORKFLOW_DETAIL.deleteByKey");
        }
        /// <summary>
        /// 获取sql语句【根据流程实例ID获取表TB_FLOW_WORKFLOW_DETAIL最后一条流转信息】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSqlForTB_FLOW_WORKFLOW_DETAIL_selectLastTrasferByFlowId()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_FLOW_WORKFLOW_DETAIL.selectLastTrasferByFlowId");
        }

        /// <summary>
        /// 获取sql语句【根据流程实例编码对数据表【TB_FLOW_WORKFLOW_DETAIL】的selectPendingByUserId获取上一条流转信息】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSqlForTB_FLOW_WORKFLOW_DETAIL_selectPreviousTrasferByFlowId()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_FLOW_WORKFLOW_DETAIL.selectPreviousTrasferByFlowId");
        }

        /// <summary>
        /// 获取sql语句【根据流程实例ID获取该流程实例所有流转信息】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSqlForTB_FLOW_WORKFLOW_DETAIL_selectAllByFlowId()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_FLOW_WORKFLOW_DETAIL.selectAllByFlowId");
        }

        #endregion

        #region 获取表【TB_FLOW_POST_DEFINE】相关sql语句的配置项
        /// <summary>
        /// 获取sql语句【根据主键获取表TB_FLOW_POST_DEFINE一条记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSqlForTB_FLOW_POST_DEFINE_selectByKey()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_FLOW_POST_DEFINE.selectByKey");
        }

        /// <summary>
        /// 获取sql语句【根据流程定义获取TB_FLOW_POST_DEFINE所有岗位】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSqlForTB_FLOW_POST_DEFINE_selectByFlowCode()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_FLOW_POST_DEFINE.selectByFlowCode");
        }
        /// <summary>
        /// 获取sql语句【根据流程定义获取起始岗位】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSqlForTB_FLOW_POST_DEFINE_selectStartPostByFlowCode()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_FLOW_POST_DEFINE.selectStartPostByFlowCode");
        }
        #endregion

        #region 获取表【TB_FLOW_POST_ACTION】相关sql语句的配置项
        /// <summary>
        /// 获取sql语句【根据岗位编码获取表TB_FLOW_POST_ACTION记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSqlForTB_FLOW_POST_ACTION_selectByPostCode()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_FLOW_POST_ACTION.selectByPostCode");
        }

        #endregion

        #region 获取表【TB_FLOW_PATH】相关sql语句的配置项
        /// <summary>
        /// 获取sql语句【根据前岗位编码获取表TB_FLOW_PATH中对应的下岗位记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSqlForTB_FLOW_PATH_selectByPrePostCode()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_FLOW_PATH.selectByPrePostCode");
        }
        #endregion

        #region 获取表【TB_FLOW_POST_ACTOR】相关sql语句的配置项
        /// <summary>
        /// 获取sql语句【根据岗位编码获取表TB_FLOW_POST_ACTOR的记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSqlForTB_FLOW_POST_ACTOR_selectByPostCode()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_FLOW_POST_ACTOR.selectByPostCode");
        }
        #endregion

        #region 获取表【TB_FLOW_RESERVED_MEMO】相关sql语句的配置项
        /// <summary>
        /// 获取sql语句【根据路径编码获取表TB_FLOW_RESERVED_MEMO相应记录】
        /// </summary>
        /// <returns></returns>
        public Com.ValuePlus.Utils.SqlBasicMetaData GetSqlForTB_FLOW_RESERVED_MEMO_selectByPathCode()
        {
            return ConfigCache.GetSqlBasicMetaData("TB_FLOW_RESERVED_MEMO.selectByPathCode");
        }
        #endregion


    }
}
