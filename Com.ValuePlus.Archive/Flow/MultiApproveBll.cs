using Com.ValuePlus.Archive.BLL;
using Com.ValuePlus.Archive.DAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Com.ValuePlus.Archive.Flow
{
    /// <summary>
    /// 流程批量审批时综合处理类
    /// </summary>
    public class MultiApproveBll
    {
        /// <summary>
        /// 日志声明
        /// </summary>
        protected static Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 获取待办处理的审批列表中行数据集
        /// </summary>
        /// <param name="strTID"></param>
        /// <param name="strRID"></param>
        /// <param name="strSID"></param>
        /// <param name="strFilterSql"></param>
        /// <param name="strUserCode"></param>
        /// <param name="strLanguage"></param>
        /// <returns></returns>
        public static DataTable GetArchiveSSLCTByFilter(String strTID, String strRID, String strSID,String strFilterSql,String strUserCode, String strLanguage)
        {
            DataTable dt_Rows = new DataTable();
            try
            {
                //获取列表行数据
                String strSql = ArchiveMainDealBll.GetArchiveSceneSSLCT(strTID, strRID, strSID, strUserCode, strLanguage,true).ToUpper();
                if (!String.IsNullOrEmpty(strFilterSql))
                {
                    DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
                    DataView dv_ListShow = new DataView();
                    dv_ListShow.Table = dt;
                    dv_ListShow.RowFilter = strFilterSql;
                    dt_Rows = dv_ListShow.ToTable();
                }else
                {
                    dt_Rows = SqlParamDao.GetDataTableBySql(strSql);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return dt_Rows;
        }

        /// <summary>
        /// 获取待办处理的审批列表中主显示列数据集
        /// </summary>
        /// <param name="strTID"></param>
        /// <param name="strRID"></param>
        /// <param name="strSID"></param>
        /// <param name="strLanguage"></param>
        /// <returns></returns>
        public static DataTable GetListShowColumns(String strTID, String strRID, String strSID,String strLanguage)
        {
            DataTable dt_Column = new DataTable();
            try
            {
                //获取列表列数据
                StringBuilder sbSql_Column = new StringBuilder();
                sbSql_Column.Append("select UPPER(PID) as ColumnValue," + (strLanguage.Equals("en-us") ? "PDESC" : "PDESCCHS") + " as ColumnName ");
                sbSql_Column.Append(" from TB_HRTMPSD WHERE TID = '"+ strTID + "' AND GID = '1' AND SID = '"+ strSID + "' AND PTYPE NOT IN ('CH','CS') ");
                sbSql_Column.Append(" AND PRIGHT NOT IN ('2') AND PCTRL NOT IN ('11') AND PLIST  = '1' ORDER BY PORDER");
                String strSql_Column = sbSql_Column.ToString();
                dt_Column = SqlParamDao.GetDataTableBySql(strSql_Column);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return dt_Column;
        }

        /// <summary>
        /// 获取待办处理的审批列表中主显示列数据集
        /// </summary>
        /// <param name="strTID"></param>
        /// <param name="strRID"></param>
        /// <param name="strSID"></param>
        /// <param name="strKeyValue"></param>
        /// <param name="iFolderLevel">距根目录下的目录级别，0为根目录</param>
        /// <returns></returns>
        public static String GetArchiveDetailUrl(String strTID, String strRID, String strSID, String strKeyValue,int iFolderLevel)
        {
            String strURL = "";
            try
            {
                StringBuilder sbParamString = new StringBuilder();
                sbParamString.Append("TID=" + strTID + "&RID=" + strRID + "&SID=" + strSID + "&KEY=FLOWCODE&KEYVALUE=" + strKeyValue);
                sbParamString.Append("&OPTYPE=readonly&sqlAddtion=&searchValue=&sortExp=");
                sbParamString.Append("&hideTopToolbar=1");
                String strPoint = "";
                int i = 0;
                while(i < iFolderLevel){
                    strPoint = strPoint + "../";
                    i++;
                }
                strURL = strPoint + "Archive/Detail/EditArchiveDetail.aspx?" + Com.ValuePlus.Common.UrlParamEncryption.EncryptionUrlParam(sbParamString.ToString());
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return strURL;
        }

        /// <summary>
        /// 针对某条流程进行同意/退回操作
        /// </summary>
        /// <param name="strTID"></param>
        /// <param name="strRID"></param>
        /// <param name="strSID"></param>
        /// <param name="strKeyValue"></param>
        /// <param name="strOpFlag"></param>
        /// <param name="strUserCode"></param>
        /// <param name="strLanguage"></param>
        /// <returns></returns>
        public static String ApproveOneFlowInstance(String strTID, String strRID, String strSID, String strKeyValue, String strOpFlag, String strUserCode,String strLanguage,ref String strReturnCode)
        {
            String strReturnMsg = "";

            //获取同意下一步的动作清单
            StringBuilder sbSql_Action = new StringBuilder();
            sbSql_Action.Append("select ISNULL(ACondition,'') as ACTIONCONDITION, A.* from FLFlowConfig_3 A INNER JOIN FLFlowConfig_4 B ON A.ConfigCode = B.ConfigCode AND A.SeqNo = B.SeqNo ");
            //相应模版响应角色下在响应场景下可视的动作
            sbSql_Action.Append(" WHERE A.ConfigCode = '" + strTID + "'+'" + strRID + "' AND B.SceneCode = '" + strSID + "' ");
            //存储过程类型的动作
            sbSql_Action.Append(" AND ATYPE = '2' ");
            //明细页面的动作
            sbSql_Action.Append(" AND ALOCATION = '1' ");
            //动作名称必须是USP_FL_Flow_ApproveAction
            sbSql_Action.Append(" AND ADETAIL = 'USP_FL_Flow_ApproveAction' ");
            if (strOpFlag.ToLower().Equals("agree"))
            {
                //同意下一步的：动作流程状态到，包括：030审核中/090审核通过/110二次录入
                sbSql_Action.Append(" AND ISNULL(StatusTo,'') IN ('030','090','130') ");
            }
            else
            {
                //拒绝退回的：动作流程状态到，包括：050已退回/150二次已退回
                sbSql_Action.Append(" AND ISNULL(StatusTo,'') IN ('050','150') ");
            }
            sbSql_Action.Append(" ORDER BY AORDER ");
            String strSql_Action = sbSql_Action.ToString();
            DataTable dt_Action = SqlParamDao.GetDataTableBySql(strSql_Action);
            if (dt_Action != null && dt_Action.Rows.Count > 0)
            {
                Hashtable hsTableCur = new Hashtable();
                //只会执行一个动作
                for (int i = 0; i < dt_Action.Rows.Count; i++)
                {
                    hsTableCur = new Hashtable();
                    DataRow drAction = dt_Action.Rows[i];
                    String strSeqNo = drAction["SeqNo"].ToString();
                    String strActionID = "000" + strSeqNo;
                    strActionID = strActionID.Substring(strActionID.Length - 3, 3);
                    strActionID = strTID + strRID + "A" + strActionID;
                    //存储当前有用信息
                    hsTableCur.Add("ADETAIL", drAction["ADETAIL"].ToString());
                    hsTableCur.Add("ADESC", (strLanguage.Equals("en-us") ? drAction["ADESC"].ToString() : drAction["ADESCCHS"].ToString()));
                    hsTableCur.Add("ActionID", strActionID);
                    String strAconditon = drAction["ACTIONCONDITION"].ToString();
                    //根据ACondition判断该流程是否适用这个动作
                    if (!String.IsNullOrEmpty(strAconditon))
                    {
                        strAconditon = strAconditon.Replace("%ActionID%", strActionID);
                        String strExists = "select count(1) from " + strTID + "_1 where FlowCode= '" + strKeyValue + "' AND (" + strAconditon + ") ";
                        int iExists = SqlParamDao.ExecuteScalarBySql(strExists);
                        if (iExists > 0)
                        {
                            //如果符合条件则就取当前动作跳出循环
                            break;
                        }
                    }
                    else
                    {
                        //如果没有条件则就取当前动作跳出循环
                        break;
                    }
                }

                //获取到的当前动作后进行动作执行
                String strADETAIL = hsTableCur["ADETAIL"].ToString();
                String strADESC = hsTableCur["ADESC"].ToString();
                String strACTIONID = hsTableCur["ActionID"].ToString();
                String strIdea = "批量审批：" + strADESC;
                //存储过程名称必须是USP_FL_Flow_ApproveAction，且参数名必须是USP_FL_Flow_ApproveAction的对应参数名【不可修改】
                Hashtable hsTableSPParamAValue = new Hashtable();
                hsTableSPParamAValue.Add("sTID", strTID);
                hsTableSPParamAValue.Add("sRID", strRID);
                hsTableSPParamAValue.Add("sSID", strSID);
                hsTableSPParamAValue.Add("sAID", strACTIONID);
                hsTableSPParamAValue.Add("KeyValue", strKeyValue);
                hsTableSPParamAValue.Add("UserId", strUserCode);
                hsTableSPParamAValue.Add("InputIdea", strIdea);
                //执行存储过程并返回整形值iReturn
                int iReturn = ArchiveActionBll.DoExcuteSP(strADETAIL, hsTableSPParamAValue);
                //根据iReturn判断执行提示
                String strMsgName = (strLanguage == "zh-cn") ? "MESSCHS" : "MESSENG";
                String strMsg = "";
                String strSql_Tips = "SELECT " + strMsgName + " FROM TB_HRTMPAR WHERE TID='" + strTID + "' AND AID='" + strACTIONID + "' AND ECFROM<=" + iReturn + " AND ECTO>=" + iReturn;
                DataTable dtTips = SqlParamDao.GetDataTableBySql(strSql_Tips);
                if (dtTips != null && dtTips.Rows.Count > 0)
                {
                    strMsg = dtTips.Rows[0][0].ToString().Replace("@S@", iReturn.ToString());

                    strReturnCode = iReturn.ToString();
                    strReturnMsg = strMsg;
                }
                else
                {
                    if (iReturn < 0)
                    {
                        strReturnCode = iReturn.ToString();
                        strReturnMsg = strADESC + "失败";
                    }
                    else
                    {
                        strReturnCode = "1";
                        strReturnMsg = strADESC + "成功";
                    }
                }
            }
            else
            {
                strReturnCode = "-1";
                strReturnMsg = "审批失败，流程动作配置缺失！";
            }

            return strReturnMsg;
        }

    }
}
