using System;
using System.Text;
using Com.ValuePlus.Database;
using Com.ValuePlus.Flow.Config;
using System.Data;
using System.Data.Common;

namespace Com.ValuePlus.Flow.DAL.Flow
{
    public class FlowPostDao
    {
        /// <summary>
        /// 日志声明
        /// </summary>
        protected static Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region 根据流程定义获取TB_FLOW_POST_DEFINE所有岗位，返回dateset记录集
        /// <summary>
        /// 根据流程定义获取TB_FLOW_POST_DEFINE所有岗位，返回dateset记录集
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet findAllPostInfoByFlowCode(String strFlowCode)
        {
            DataSet ds = new DataSet();
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(FlowSqlConfig.Instance.GetSqlForTB_FLOW_POST_DEFINE_selectByFlowCode());
                param[0].Value = strFlowCode;
                ds = dao.ExecuteDataSet(FlowSqlConfig.Instance.GetSqlForTB_FLOW_POST_DEFINE_selectByFlowCode(), param);
            }
            return ds;
        }
        #endregion

        #region 根据主键查询TB_FLOW_POST_DEFINE的相应记录，返回dateset记录集
        /// <summary>
        /// 根据主键查询TB_FLOW_POST_DEFINE的相应记录，返回dateset记录集
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet findPostInfoByPostCode(String strPostCode)
        {
            DataSet ds = new DataSet();
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(FlowSqlConfig.Instance.GetSqlForTB_FLOW_POST_DEFINE_selectByKey());
                param[0].Value = strPostCode;
                ds = dao.ExecuteDataSet(FlowSqlConfig.Instance.GetSqlForTB_FLOW_POST_DEFINE_selectByKey(), param);
            }
            return ds;
        }
        #endregion

        #region 根据流程定义获取该流程定义的起始岗位，返回dateset记录集
        /// <summary>
        /// 根据流程定义获取该流程定义的起始岗位，返回dateset记录集
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet findStartPostByFlowCode(String strFlowCode)
        {
            DataSet ds = new DataSet();
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(FlowSqlConfig.Instance.GetSqlForTB_FLOW_POST_DEFINE_selectStartPostByFlowCode());
                param[0].Value = strFlowCode;
                ds = dao.ExecuteDataSet(FlowSqlConfig.Instance.GetSqlForTB_FLOW_POST_DEFINE_selectStartPostByFlowCode(), param);
            }
            return ds;
        }
        #endregion

        #region 根据岗位编码查询TB_FLOW_POST_ACTION的相应记录，返回dateset记录集
        /// <summary>
        /// 根据岗位编码查询TB_FLOW_POST_ACTION的相应记录，返回dateset记录集
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet findActionByPostCode(String strPostCode)
        {
            DataSet ds = new DataSet();
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(FlowSqlConfig.Instance.GetSqlForTB_FLOW_POST_ACTION_selectByPostCode());
                param[0].Value = strPostCode;
                ds = dao.ExecuteDataSet(FlowSqlConfig.Instance.GetSqlForTB_FLOW_POST_ACTION_selectByPostCode(), param);
            }
            return ds;
        }
        #endregion

        #region 根据前岗位编码查询TB_FLOW_PATH中对应的下岗位记录，返回dateset记录集
        /// <summary>
        /// 根据前岗位编码查询TB_FLOW_PATH中对应的下岗位记录，返回dateset记录集
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet findNextPostInfoByPrePostCode(String strPostCode)
        {
            DataSet ds = new DataSet();
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(FlowSqlConfig.Instance.GetSqlForTB_FLOW_PATH_selectByPrePostCode());
                param[0].Value = strPostCode;
                ds = dao.ExecuteDataSet(FlowSqlConfig.Instance.GetSqlForTB_FLOW_PATH_selectByPrePostCode(), param);
            }
            return ds;
        }
        #endregion

        #region 根据岗位编码查询TB_FLOW_POST_ACTOR的相应记录，返回dateset记录集
        /// <summary>
        /// 根据岗位编码查询TB_FLOW_POST_ACTOR的相应记录，返回dateset记录集
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet findActorByPostCode(String strPostCode)
        {
            DataSet ds = new DataSet();
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(FlowSqlConfig.Instance.GetSqlForTB_FLOW_POST_ACTOR_selectByPostCode());
                param[0].Value = strPostCode;
                ds = dao.ExecuteDataSet(FlowSqlConfig.Instance.GetSqlForTB_FLOW_POST_ACTOR_selectByPostCode(), param);
            }
            return ds;
        }
        #endregion

        #region 根据路径编码获取表TB_FLOW_RESERVED_MEMO相应记录，返回dateset记录集
        /// <summary>
        /// 根据路径编码获取表TB_FLOW_RESERVED_MEMO相应记录，返回dateset记录集
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet findReservedMemoByPathCode(String strPathCode)
        {
            DataSet ds = new DataSet();
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(FlowSqlConfig.Instance.GetSqlForTB_FLOW_RESERVED_MEMO_selectByPathCode());
                param[0].Value = strPathCode;
                ds = dao.ExecuteDataSet(FlowSqlConfig.Instance.GetSqlForTB_FLOW_RESERVED_MEMO_selectByPathCode(), param);
            }
            return ds;
        }
        #endregion

    }
}
