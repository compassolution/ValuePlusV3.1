using Com.ValuePlus.Archive.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Com.ValuePlus.BLL.SMS;

namespace Com.ValuePlus.Archive.Flow
{
    public class FLUserSMS
    {
        /// <summary>
        /// 日志声明
        /// </summary>
        protected static Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 在动作执行之后根据FLUser中的配置进行短信发送
        /// </summary>
        /// <param name="strTID"></param>
        /// <param name="strRID"></param>
        /// <param name="strSID"></param>
        /// <param name="strAID"></param>
        /// <param name="strAID">strKeyValue</param>
        /// <param name="strPushType">1:SMS短信/2:PUSH公众号</param>
        /// <returns></returns>
        public static string SendSMSAfterActionExecute (String strTID,String strRID,String strSID,String strAID,String strKeyValue, String strPushType)
        {
            String strReturn = "";
            StringBuilder sbSql = new StringBuilder();
            try
            {
                //根据TID / RID / AID获取FLUser中配置所需推送提醒的场景
                DataTable dt = GetNeedNoticeScene(strTID,strRID,strAID, strKeyValue, strPushType);
                if (dt != null && dt.Rows.Count > 0)
                {
                    for(int i=0;i<dt.Rows.Count;i++)
                    {
                        DataRow dr = dt.Rows[i];
                        String strToUserId = dr["SUSERID"].ToString();
                        try
                        {
                            String strMobileNo = dr["DCMOBILE"].ToString();
                            if(!String.IsNullOrEmpty(strMobileNo))
                            {
                                String strSendContent = dr["SMSContent"].ToString();
                                if (String.IsNullOrEmpty(strSendContent))
                                {
                                    strSendContent = "【VP人力资源系统】您有新的待办事项，请登录PC端或者手机端程序进行处理！";
                                }else
                                {

                                }

                                //发送短信
                                SendSMS.DoSendMessage(strMobileNo, strSendContent);
                                log.Error("流程提交后给" + strMobileNo + "发送短信成功："+ strSendContent + "\r\n");
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Error("流程提交后给"+ strToUserId + "发送短信失败\r\n");
                            log.Error(ex.Message.ToString());
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                log.Error("在动作执行之后根据FLUser中的配置进行短信发送失败\r\n");
                log.Error(ex.Message.ToString());
            }

            return strReturn;
        }

        /// <summary>
        /// 根据TID/RID/AID获取流程FLFlowConfig_3中对应的下一个岗位的FLUser中配置所需推送提醒的场景
        /// </summary>
        /// <param name="strTID"></param>
        /// <param name="strRID"></param>
        /// <param name="strAID"></param>
        /// <param name="strKeyValue"></param>
        /// <param name="strPushType">1:SMS短信/2:PUSH公众号</param>
        /// <returns></returns>
        public static DataTable GetNeedNoticeScene(String strTID, String strRID, String strAID,String strKeyValue, String strPushType)
        {
            StringBuilder sbSql = new StringBuilder();
            DataTable dtNeedNotice = new DataTable();
            try
            {
                //先盘点strAID是流程业务中的AID，才进行后续操作 add by sammen 20250305
                String strJudgeFlowArchive = "SELECT 1 FROM FLFLOW_1 WHERE FCODE = '" + strTID + "'";
                DataTable dtJudegeFlowArchive = SqlParamDao.GetDataTableBySql(strJudgeFlowArchive);
                if (dtJudegeFlowArchive != null && dtJudegeFlowArchive.Rows.Count > 0)
                {

                    //首先根据TID/RID/AID获取流程FLFlowConfig_3中对应的下一个岗位
                    String strSql = "SELECT * FROM FLFlowConfig_3 WHERE ConfigCode = '" + strTID + "'+'" + strRID + "' AND SeqNo = CONVERT(INT,RIGHT('" + strAID + "',3))";
                    DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
                    if (dt != null && dt.Rows.Count == 1)
                    {
                        String strNextFlowPost = dt.Rows[0]["ToPostCode"].ToString();
                        String strStatusTo = dt.Rows[0]["StatusTo"].ToString();
                        if (!String.IsNullOrEmpty(strNextFlowPost))
                        {
                            String strSqlMatchDept = " and 1=1 ";
                            //############首先盘点下一个岗位是否需要匹配流程的所在部门 add by sammen 20241206
                            String strSqlIsMatchDept = "select ISNULL(IsViewDept,'false') as IsViewDept from FLPost_1 where PostCode = '" + strNextFlowPost + "'";
                            DataTable dtIsMatchDept = SqlParamDao.GetDataTableBySql(strSqlIsMatchDept);
                            if (dtIsMatchDept != null && dtIsMatchDept.Rows.Count == 1)
                            {
                                String strIsViewDept = dtIsMatchDept.Rows[0]["IsViewDept"].ToString().ToLower();
                                //如果下一岗位需要匹配部门
                                if (strIsViewDept.Equals("true"))
                                {
                                    //获取本流程实例的申请部门编码
                                    String strTableName = strTID + "_1";
                                    String strSqlGetDeptCode = "select RequestDept from " + strTableName + " where FlowCode = '" + strKeyValue + "'";
                                    DataTable dtGetDeptCode = SqlParamDao.GetDataTableBySql(strSqlGetDeptCode);
                                    if (dtGetDeptCode != null && dtGetDeptCode.Rows.Count == 1)
                                    {
                                        String strDeptCode = dtGetDeptCode.Rows[0]["RequestDept"].ToString().ToLower();
                                        strSqlMatchDept = " AND '" + strDeptCode + "' in ((SELECT DEPTCODE FROM FLUser_2 WHERE SUSERID = A.SUSERID))";
                                    }
                                }
                            }
                            //############首先盘点下一个岗位是否需要匹配流程的所在部门 add by sammen 20241206

                            //获取下个岗位下一个状态所对应的用户是否需要发送短信
                            sbSql.Append("select A.*,E.DCMOBILE,E.DCNO as STAFFNO from FLUser_6 A INNER JOIN FLUser_3 B on A.SUSERID = B.SUSERID and A.FCODE = B.FCODE AND A.PostCode = B.PostCode \r\n");
                            sbSql.Append(" INNER JOIN FLScene_2 C ON A.SceneCode = C.SceneCode  \r\n");
                            sbSql.Append(" INNER JOIN FLUser_1 D ON A.SUSERID = D.SUSERID  \r\n");
                            sbSql.Append(" INNER JOIN HRDOCU_1 E ON (D.SUSERID = E.DCNO OR D.STAFFNO = E.DCNO) \r\n");
                            sbSql.Append(" where B.FCODE = '" + strTID + "' and B.PostCode = '" + strNextFlowPost + "' AND C.StatusCode = '" + strStatusTo + "' \r\n");
                            //add by sammen 20250818 离职后的不接受通知
                            sbSql.Append(" AND D.IsStop <> '1' and E.DCSTATUS <> '3' \r\n");
                            if (strPushType.Equals("1"))
                            {
                                //短信推送
                                sbSql.Append(" AND ISNULL(ISSMS,'2') = '1' \r\n");
                            }
                            else if (strPushType.Equals("2"))
                            {
                                //公众号推送
                                sbSql.Append(" AND ISNULL(ISPUSH,'2') = '1'  \r\n");
                            }
                            sbSql.Append(strSqlMatchDept);
                            strSql = sbSql.ToString();
                            dtNeedNotice = SqlParamDao.GetDataTableBySql(strSql);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("根据TID/RID/AID获取FLUser中配置所需推送提醒的场景失败\r\n");
                log.Error(ex.Message.ToString());
            }

            return dtNeedNotice;
        }

    }
}
