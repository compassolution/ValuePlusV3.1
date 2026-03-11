using Com.ValuePlus.DAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Com.ValuePlus.Labor
{
    public class LUser
    {
        /// <summary>
        /// 日志声明
        /// </summary>
        protected static Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 通过账号/手机号/昵称获取用户信息数据表集合
        /// </summary>
        /// <param name="strUserCode"></param>
        /// <param name="strMobileNo"></param>
        /// <param name="strNickName"></param>
        /// <param name="strIsValid"></param>
        /// <returns></returns>
        public static DataTable GetUserInfoDataTable(String strUserCode,String strMobileNo,String strNickName, String strIsValid)
        {
            DataTable dtReturn = new DataTable();
            try
            {
                StringBuilder sbSql = new StringBuilder();
                sbSql.Append("select A.* from [VW_Labor_LUserMain] A where 1=1");
                sbSql.Append(" and (");
                if (!String.IsNullOrEmpty(strUserCode))
                {
                    sbSql.Append(" UserCode = " + JObjectToDB.GetColumnEncryptDataString("LUser_1", "UserCode", strUserCode));//用户编号
                }
                if (!String.IsNullOrEmpty(strMobileNo))
                {
                    sbSql.Append(" or isnull(MobileNo,'') = " + JObjectToDB.GetColumnEncryptDataString("LUser_1", "MobileNo", strMobileNo));//手机号码
                }
                if (!String.IsNullOrEmpty(strNickName))
                {
                    sbSql.Append(" or isnull(NickName,'') = " + JObjectToDB.GetColumnEncryptDataString("LUser_1", "NickName", strNickName));//昵称
                }
                sbSql.Append(" )");
                if (!String.IsNullOrEmpty(strIsValid))
                {
                    sbSql.Append(" and A.IsValid = " + JObjectToDB.GetColumnEncryptDataString("LUser_1", "IsValid", strIsValid));
                }
                String strSql = sbSql.ToString();

                log.Error("Labor 通过账号/手机号/昵称获取用户信息数据表集合，Sql:" + strSql);
                dtReturn = SqlParamDao.GetDataTableBySql(strSql);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return dtReturn;
        }

        /// <summary>
        /// 通过OpenId获取用户信息数据表集合
        /// </summary>
        /// <param name="strOpenId"></param>
        /// <param name="strIsValid"></param>
        /// <returns></returns>
        public static DataTable GetUserInfoDataTableByOpenId(String strOpenId, String strIsValid)
        {
            DataTable dtReturn = new DataTable();
            try
            {
                StringBuilder sbSql = new StringBuilder();
                sbSql.Append("select A.* from [VW_Labor_LUserMain] A where 1=1");
                sbSql.Append(" and isnull(OpenId,'') = " + JObjectToDB.GetColumnEncryptDataString("LUser_1", "OpenId", strOpenId));
                if (!String.IsNullOrEmpty(strIsValid))
                {
                    sbSql.Append(" and A.IsValid = " + JObjectToDB.GetColumnEncryptDataString("LUser_1", "IsValid", strIsValid));
                }
                String strSql = sbSql.ToString();

                log.Error("Labor 通过OpenId获取用户信息数据表集合，Sql:" + strSql);
                dtReturn = SqlParamDao.GetDataTableBySql(strSql);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return dtReturn;
        }
        
        /// <summary>
        /// 通过用户和条件获取用户信息数据表集合【获取全部，不分页】
        /// </summary>
        /// <param name="strUserCode"></param>
        /// <param name="strCondition"></param>
        /// <param name="iTopRows"></param>
        /// <param name="strOrderBy"></param>
        /// <returns></returns>
        public static DataTable GetUserInfoDataTable(String strUserCode, String strCondition, int iTopRows, String strOrderBy)
        {
            DataTable dtReturn = new DataTable();
            try
            {
                StringBuilder sbSql = new StringBuilder();
                if (iTopRows == 0)
                {
                    sbSql.Append("select * from [VW_Labor_LUserMain] A where 1=1");
                }
                else
                {
                    sbSql.Append("select top " + iTopRows.ToString() + " * from [VW_Labor_LUserMain] A where 1=1");
                }
                if (!String.IsNullOrEmpty(strCondition))
                {
                    sbSql.Append(strCondition);
                }
                if (!String.IsNullOrEmpty(strOrderBy))
                {
                    sbSql.Append(" order by " + strOrderBy);
                }

                String strSql = sbSql.ToString();

                log.Error("Labor 通过用户和条件获取用户信息数据表集合【获取全部，不分页】，Sql:" + strSql);
                dtReturn = SqlParamDao.GetDataTableBySql(strSql);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return dtReturn;
        }
        
        /// <summary>
        /// 通过公司及部门获取用户信息数据表集合
        /// </summary>
        /// <param name="strCompanyCode"></param>
        /// <param name="strDeptCode"></param>
        /// <param name="strIsValid"></param>
        /// <returns></returns>
        public static DataTable GetUserInfoDataTableByDept(String strCompanyCode, String strDeptCode, String strIsValid)
        {
            DataTable dtReturn = new DataTable();
            try
            {
                StringBuilder sbSql = new StringBuilder();
                sbSql.Append("select A.* from [VW_Labor_LUserMain] A where 1=1");
                sbSql.Append(" and isnull(CompanyCode,'') = " + JObjectToDB.GetColumnEncryptDataString("LUser_1", "CompanyCode", strCompanyCode));
                sbSql.Append(" and isnull(DeptCode,'') = " + JObjectToDB.GetColumnEncryptDataString("LUser_1", "DeptCode", strDeptCode));//部门编码
                //if (!String.IsNullOrEmpty(strDeptCode))
                //{
                //    sbSql.Append(" and isnull(DeptCode,'') = '" + strDeptCode + "'");//部门编码
                //}
                if (!String.IsNullOrEmpty(strIsValid))
                {
                    sbSql.Append(" and A.IsValid = " + JObjectToDB.GetColumnEncryptDataString("LUser_1", "IsValid", strIsValid));
                }
                sbSql.Append(" order by A.IsValid,A.CreateTime DESC");
                String strSql = sbSql.ToString();

                log.Error("通过公司及部门获取用户信息数据表集合，Sql:" + strSql);
                dtReturn = SqlParamDao.GetDataTableBySql(strSql);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return dtReturn;
        }

        /// <summary>
        /// 根据用户编码获取我的伙伴用户列表集合
        /// </summary>
        /// <param name="strUserCode"></param>
        /// <param name="strAimUserType"></param>
        /// <param name="strUserCode"></param>
        /// <param name="strFocusType">关注类型仅我是用人单位是有效</param>
        /// <param name="strIsValid"></param>
        /// <param name="strIsValid"></param>
        /// <param name="iTopRows"></param>
        /// <param name="strOrderBy"></param>
        /// <returns></returns>
        public static DataTable GetMyPartnerUserDataTable(String strUserCode, String strAimUserType, String strIsValid,String strCondition, int iTopRows, String strOrderBy)
        {
            DataTable dtReturn = new DataTable();
            StringBuilder sbSql = new StringBuilder();
            try
            {
                String strTop = "";
                if (iTopRows != 0)
                {
                    strTop = " top " + iTopRows.ToString();
                }
                sbSql.Append("select " + strTop + " * from [VW_Labor_LUserMain] A INNER JOIN ");
                sbSql.Append("  [dbo].[Fun_Labor_GetPartnerCode_ByUserCode]('" + strUserCode + "','" + strAimUserType + "','','" + strIsValid + "') B ON A.UserCode = B.Partner_Code");
                sbSql.Append(" where 1=1 ");
                if (!String.IsNullOrEmpty(strCondition))
                {
                    sbSql.Append(strCondition);
                }
                if (!String.IsNullOrEmpty(strOrderBy))
                {
                    sbSql.Append(" order by " + strOrderBy);
                }

                String strSql = sbSql.ToString();

                log.Error("Labor 根据用户编码获取我的伙伴用户列表集合，Sql:" + strSql);
                dtReturn = SqlParamDao.GetDataTableBySql(strSql);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.Error("Labor 根据用户编码获取我的伙伴用户列表集合出错，Sql:" + sbSql.ToString());
            }
            return dtReturn;
        }

        /// <summary>
        /// 通过用户编码及时间范围等条件获取打卡记录列表集合
        /// </summary>
        /// <param name="strLaborCode"></param>
        /// <param name="strWONO"></param>
        /// <param name="strQueryTimeFrom"></param>
        /// <param name="strQueryTimeTo"></param>
        /// <param name="strAttType"></param>
        /// <param name="strAttLocation"></param>
        /// <param name="strEmployCompanyCode"></param>
        /// <param name="strServiceCompanyCode"></param>
        /// <param name="iTopRows"></param>
        /// <param name="strOrderBy"></param>
        /// <param name="strQueryUserCode"></param>
        /// <returns></returns>
        public static DataTable GetLaborAttDataTable(String strLaborCode, String strWONO, String strQueryTimeFrom, String strQueryTimeTo
            , String strAttType, String strAttLocation,String strEmployCompanyCode,String strServiceCompanyCode, int iTopRows, String strOrderBy,String strQueryUserCode)
        {
            DataTable dtReturn = new DataTable();
            StringBuilder sbSql = new StringBuilder();
            try
            {
                String strTop = "";
                if (iTopRows != 0)
                {
                    strTop = " top " + iTopRows.ToString();
                }
                sbSql.Append("select " + strTop + " * from [VW_Labor_LUserAtt] A where 1=1");
                if (!String.IsNullOrEmpty(strLaborCode))
                {
                    sbSql.Append(" and UserCode = " + JObjectToDB.GetColumnEncryptDataString("LUser_1", "UserCode", strLaborCode));
                }
                if (!String.IsNullOrEmpty(strWONO))
                {
                    sbSql.Append(" and WONO = " + JObjectToDB.GetColumnEncryptDataString("LUser_3", "WONO", strWONO));
                }
                if (!String.IsNullOrEmpty(strEmployCompanyCode))
                {
                    sbSql.Append(" and EmployCompany = " + JObjectToDB.GetColumnEncryptDataString("LUser_3", "EmployCompany", strEmployCompanyCode));
                }
                if (!String.IsNullOrEmpty(strServiceCompanyCode))
                {
                    sbSql.Append(" and isnull(ServiceCompany,'') = " + JObjectToDB.GetColumnEncryptDataString("LUser_3", "ServiceCompany", strServiceCompanyCode));
                }
                if (!String.IsNullOrEmpty(strQueryTimeFrom))
                {
                    sbSql.Append(" and convert(varchar(20),convert(datetime,AttTime),23) >= convert(varchar(20),convert(datetime," + JObjectToDB.GetColumnEncryptDataString("LUser_3", "AttTime", strQueryTimeFrom)+",23)");
                }
                if (!String.IsNullOrEmpty(strQueryTimeTo))
                {
                    sbSql.Append(" and convert(varchar(20),convert(datetime,AttTime),23) <= convert(varchar(20),convert(datetime," + JObjectToDB.GetColumnEncryptDataString("LUser_3", "AttTime", strQueryTimeTo)+",23)");
                }
                if (!String.IsNullOrEmpty(strAttType))
                {
                    sbSql.Append(" and AttType = " + JObjectToDB.GetColumnEncryptDataString("LUser_3", "AttType", strAttType));
                }
                if (!String.IsNullOrEmpty(strLaborCode))
                {
                    sbSql.Append(" and isnull(AttLocation,'') = " + JObjectToDB.GetColumnEncryptDataString("LUser_3", "AttLocation", strLaborCode));
                }
                if (!String.IsNullOrEmpty(strOrderBy))
                {
                    sbSql.Append(" order by " + strOrderBy);
                }
                else
                {
                    sbSql.Append(" order by A.SEQNO DESC ");
                }
                String strSql = sbSql.ToString();

                log.Error("Labor 通过用户编码及时间范围获取打卡记录列表集合集合，Sql:" + strSql);
                dtReturn = SqlParamDao.GetDataTableBySql(strSql);
            }
            catch (Exception ex)
            {
                log.Error("Labor 通过用户编码及时间范围获取打卡记录列表集合集合出错，Sql:" + sbSql.ToString());
                log.Error(ex);
            }
            return dtReturn;
        }

        /// <summary>
        /// 根据手机号码获取用户编码
        /// </summary>
        /// <param name="strMobileNo"></param>
        /// <returns></returns>
        public static String GetUserCodeByMobileNo(string strMobileNo)
        {
            String strUserCode = "";
            String strSql = "select * from LUser_1 where MobileNo = " + JObjectToDB.GetColumnEncryptDataString("LUser_1", "MobileNo", strMobileNo);
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
            if (dt !=null&& dt.Rows.Count>0)
            {
                strUserCode = dt.Rows[0]["UserCode"].ToString();
            }
            return strUserCode;
        }

        /// <summary>
        /// 根据用户类型自动生成用户编号
        /// </summary>
        /// <param name="strUserType"></param>
        /// <returns></returns>
        public static string GenerateUserCode(string strUserType)
        {
            string strUserCode = "";
            string strPreFix = "";
            switch (strUserType)
            {
                case "1"://用人单位
                    strPreFix = "EP";
                    break;
                case "2"://外包公司
                    strPreFix = "OS";
                    break;
                case "3"://外包工
                    strPreFix = "LB";
                    break;
            }
            Random rad = new Random();//实例化随机数产生器rad；
            bool IsExists = true;
            while (IsExists)
            {
                //8位随机数
                String strRandomCode = rad.Next(10000000, 100000000).ToString();
                strUserCode = strPreFix + strRandomCode;

                String strSql = "select count(1) from LUser_1 where UserCode = " + JObjectToDB.GetColumnEncryptDataString("LUser_1", "UserCode", strUserCode);
                int iCount = SqlParamDao.ExecuteScalarBySql(strSql);
                if (iCount <= 0)
                {
                    IsExists = false;
                    break;
                }
            }

            return strUserCode;
        }

        /// <summary>
        /// 插入一条默认的用户信息记录to LUser_1
        /// </summary>
        /// <param name="strNeedInsertUserCode"></param>
        /// <param name="strMobileNo"></param>
        /// <param name="strPassword"></param>
        /// <param name="strUserType"></param>
        /// <param name="IsAdministrator"></param>
        /// <param name="strUserName"></param>
        /// <param name="strPosiName"></param>
        /// <param name="strCompanyCode"></param>
        /// <param name="strDeptCode"></param>
        /// <param name="strDoUserCode"></param>
        /// <returns></returns>
        public static int InsertOneRegUser(string strNeedInsertUserCode, String strMobileNo,string strPassword,string strUserType, string strUserRole
            , String IsAdministrator, String strUserName,String strPosiName,String strCompanyCode,String strDeptCode,string strDoUserCode)
        {
            int iReturn = 0;
            StringBuilder sbSql = new StringBuilder();
            try
            {
                String strCurTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                //String IsAdministrator = "2";
                string strDefaultUserName = strUserName;
                if (String.IsNullOrEmpty(strDefaultUserName))
                {
                    switch (strUserType)
                    {
                        case "1"://用人单位
                            strDefaultUserName = "用人单位用户";
                            break;
                        case "2"://外包公司
                            strDefaultUserName = "外包公司用户";
                            break;
                        case "3"://外包工
                            strDefaultUserName = "外包工用户";
                            break;
                    }
                }
                String strTableName = "LUser_1";
                //新增一个用户，包括必填项目默认值
                sbSql.Append("INSERT INTO LUser_1(UserCode,Password,UserType,UserRole,UserName,PosiName,IsAdministrator,WorkOrderTime,AppraiseGoodTime,AppraiseCommonTime,AppraiseBadTime,AppraiseDefaultTime,AppraiseGoodRate");
                sbSql.Append(",MobileNo,AppraiseStarItem1,AppraiseStarItem2,AppraiseStarItem3,AppraiseStarItem4,AppraiseStarItem5,AppraiseStarAll,Liveness,CreateTime,CreateUser,IsValid");
                sbSql.Append(",CompanyCode,DeptCode)");
                //sbSql.Append(" values ('" + strNeedInsertUserCode + "','" + strPassword + "','" + strUserType + "','" + strUserRole + "','" + strDefaultUserName + "','"  + strPosiName + "','" + IsAdministrator + "',0,0,0,0,0,100.00");
                //sbSql.Append(" ,'" + strMobileNo + "',5,5,5,5,5,5,0,'" + strCurTime + "','" + strDoUserCode + "','1'");
                //sbSql.Append(" ,'" + strCompanyCode + "','" + strDeptCode + "')");
                sbSql.Append(" values (");
                sbSql.Append(JObjectToDB.GetColumnEncryptDataString(strTableName, "UserCode", strNeedInsertUserCode));
                sbSql.Append(","+JObjectToDB.GetColumnEncryptDataString(strTableName, "Password", strPassword));
                sbSql.Append(","+JObjectToDB.GetColumnEncryptDataString(strTableName, "UserType", strUserType));
                sbSql.Append(","+JObjectToDB.GetColumnEncryptDataString(strTableName, "UserRole", strUserRole));
                sbSql.Append(","+JObjectToDB.GetColumnEncryptDataString(strTableName, "UserName", strDefaultUserName));
                sbSql.Append(","+JObjectToDB.GetColumnEncryptDataString(strTableName, "PosiName", strPosiName));
                sbSql.Append(","+JObjectToDB.GetColumnEncryptDataString(strTableName, "IsAdministrator", IsAdministrator));
                sbSql.Append(","+JObjectToDB.GetColumnEncryptDataString(strTableName, "WorkOrderTime","0" ));
                sbSql.Append(","+JObjectToDB.GetColumnEncryptDataString(strTableName, "AppraiseGoodTime", "0"));
                sbSql.Append(","+JObjectToDB.GetColumnEncryptDataString(strTableName, "AppraiseCommonTime", "0"));
                sbSql.Append(","+JObjectToDB.GetColumnEncryptDataString(strTableName, "AppraiseBadTime", "0"));
                sbSql.Append(","+JObjectToDB.GetColumnEncryptDataString(strTableName, "AppraiseDefaultTime", "0"));
                sbSql.Append(","+JObjectToDB.GetColumnEncryptDataString(strTableName, "AppraiseGoodRate", "100.00"));

                sbSql.Append(","+JObjectToDB.GetColumnEncryptDataString(strTableName, "MobileNo", strMobileNo));
                sbSql.Append(","+JObjectToDB.GetColumnEncryptDataString(strTableName, "AppraiseStarItem1", "5"));
                sbSql.Append(","+JObjectToDB.GetColumnEncryptDataString(strTableName, "AppraiseStarItem2", "5"));
                sbSql.Append(","+JObjectToDB.GetColumnEncryptDataString(strTableName, "AppraiseStarItem3", "5"));
                sbSql.Append(","+JObjectToDB.GetColumnEncryptDataString(strTableName, "AppraiseStarItem4", "5"));
                sbSql.Append(","+JObjectToDB.GetColumnEncryptDataString(strTableName, "AppraiseStarItem5", "5"));
                sbSql.Append(","+JObjectToDB.GetColumnEncryptDataString(strTableName, "AppraiseStarAll", "5"));
                sbSql.Append(","+JObjectToDB.GetColumnEncryptDataString(strTableName, "Liveness", "0"));
                sbSql.Append(","+JObjectToDB.GetColumnEncryptDataString(strTableName, "CreateTime", strCurTime));
                sbSql.Append(","+JObjectToDB.GetColumnEncryptDataString(strTableName, "CreateUser", strDoUserCode));
                sbSql.Append(","+JObjectToDB.GetColumnEncryptDataString(strTableName, "IsValid", "1"));

                sbSql.Append("," + JObjectToDB.GetColumnEncryptDataString(strTableName, "CompanyCode", strCompanyCode));
                sbSql.Append("," + JObjectToDB.GetColumnEncryptDataString(strTableName, "DeptCode", strDeptCode));


                sbSql.Append(" )");
                log.Error("插入一条默认的用户信息记录to LUser_1,SQL:" + sbSql.ToString());
                iReturn = SqlParamDao.ExecuteNonQueryBySql(sbSql.ToString());

            }
            catch (Exception ex)
            {
                iReturn = -1;
                log.Error("插入一条默认的用户信息记录to LUser_1失败:sql:" + sbSql.ToString());
                log.Error(ex);
                
            }
            return iReturn;
        }

        /// <summary>
        /// 删除账号信息
        /// </summary>
        /// <param name="strUserCode"></param>
        /// <param name="strNeedOpAccount"></param>
        /// <param name="strSetIsValid"></param>
        /// <returns></returns>
        public static int DeleteAccountInfo(String strUserCode, String strNeedOpAccount, String strSetIsValid)
        {
            String strCurDataTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            StringBuilder sbSql = new StringBuilder();
            if (!String.IsNullOrEmpty(strSetIsValid))
            {
                sbSql.Append("update LUser_1 set IsValid = " + JObjectToDB.GetColumnEncryptDataString("LUser_1", "IsValid", strSetIsValid));
                sbSql.Append("  ,LastModifyUser = " + JObjectToDB.GetColumnEncryptDataString("LUser_1", "LastModifyUser", strUserCode));
                sbSql.Append("  ,LastModifyTime = " + JObjectToDB.GetColumnEncryptDataString("LUser_1", "LastModifyTime", strCurDataTime));
                sbSql.Append(" where UserCode = " + JObjectToDB.GetColumnEncryptDataString("LUser_1", "UserCode", strNeedOpAccount));
            }
            else
            {
                sbSql.Append("delete from LUser_6 where UserCode = " + JObjectToDB.GetColumnEncryptDataString("LUser_1", "UserCode", strNeedOpAccount));
                sbSql.Append("delete from LUser_5 where UserCode = " + JObjectToDB.GetColumnEncryptDataString("LUser_1", "UserCode", strNeedOpAccount));
                sbSql.Append("delete from LUser_4 where UserCode = " + JObjectToDB.GetColumnEncryptDataString("LUser_1", "UserCode", strNeedOpAccount));
                sbSql.Append("delete from LUser_3 where UserCode = " + JObjectToDB.GetColumnEncryptDataString("LUser_1", "UserCode", strNeedOpAccount));
                sbSql.Append("delete from LUser_2 where UserCode = " + JObjectToDB.GetColumnEncryptDataString("LUser_1", "UserCode", strNeedOpAccount));
                sbSql.Append("delete from LUser_1 where UserCode = " + JObjectToDB.GetColumnEncryptDataString("LUser_1", "UserCode", strNeedOpAccount));
            }
            log.Error("删除账号信息:sql:" + sbSql.ToString());
            int iCount = SqlParamDao.ExecuteNonQueryBySql(sbSql.ToString());
            return iCount;

        }

        /// <summary>
        /// 根据账号修改账号密码
        /// </summary>
        /// <param name="strAimUserCode"></param>
        /// <param name="strOldPassword"></param>
        /// <param name="strNewPassword"></param>
        /// <param name="strOpUserCode"></param>
        /// <returns></returns>
        public static int ChangePasswordByUserCode(String strAimUserCode, String strOldPassword, String strNewPassword, String strOpUserCode)
        {
            String strCurDataTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("update LUser_1 set Password = " + JObjectToDB.GetColumnEncryptDataString("LUser_1", "Password", strNewPassword));
            sbSql.Append(",LastModifyUser = " + JObjectToDB.GetColumnEncryptDataString("LUser_1", "LastModifyUser", strOpUserCode));
            sbSql.Append(",LastModifyTime = " + JObjectToDB.GetColumnEncryptDataString("LUser_1", "LastModifyTime", strCurDataTime));
            sbSql.Append(" where UserCode = " + JObjectToDB.GetColumnEncryptDataString("LUser_1", "UserCode", strAimUserCode));

            log.Error("修改账号密码:sql:" + sbSql.ToString());
            int iCount = SqlParamDao.ExecuteNonQueryBySql(sbSql.ToString());
            return iCount;

        }

        /// <summary>
        /// 根据账号修改手机号
        /// </summary>
        /// <param name="strAimUserCode"></param>
        /// <param name="strOldMobileNo"></param>
        /// <param name="strNewMobileNo"></param>
        /// <param name="strOpUserCode"></param>
        /// <returns></returns>
        public static int ChangeChangeMobileNo(String strAimUserCode, String strOldMobileNo, String strNewMobileNo, String strOpUserCode)
        {
            String strCurDataTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("update LUser_1 set MobileNo = " + JObjectToDB.GetColumnEncryptDataString("LUser_1", "MobileNo", strNewMobileNo));
            sbSql.Append(",LastModifyUser = " + JObjectToDB.GetColumnEncryptDataString("LUser_1", "LastModifyUser", strOpUserCode));
            sbSql.Append(",LastModifyTime = " + JObjectToDB.GetColumnEncryptDataString("LUser_1", "LastModifyTime", strCurDataTime));
            sbSql.Append(" where UserCode = " + JObjectToDB.GetColumnEncryptDataString("LUser_1", "UserCode", strAimUserCode));

            log.Error("根据账号修改手机号:sql:" + sbSql.ToString());
            int iCount = SqlParamDao.ExecuteNonQueryBySql(sbSql.ToString());
            return iCount;

        }

        /// <summary>
        /// 挂载某用户到某公司
        /// </summary>
        /// <param name="strUserCode"></param>
        /// <param name="strCompanyCode"></param>
        /// <returns></returns>
        public static int ConnectUserToCompany(String strUserCode, String strCompanyCode)
        {
            int iReturn = 0;
            String strSql = "";
            try
            {
                StringBuilder sbSql = new StringBuilder();
                sbSql.Append("UPDATE LUser_1 SET CompanyCode = " + JObjectToDB.GetColumnEncryptDataString("LUser_1", "CompanyCode", strCompanyCode));
                sbSql.Append(" where UserCode = " + JObjectToDB.GetColumnEncryptDataString("LUser_1", "UserCode", strUserCode));
                strSql = sbSql.ToString();
                iReturn = SqlParamDao.ExecuteNonQueryBySql(strSql);
            }
            catch (Exception ex)
            {
                iReturn = -1;
                log.Error("挂载某用户到某公司失败:sql:" + strSql);
                log.Error(ex);

            }
            return iReturn;

        }

        /// <summary>
        /// 更新用户头像连接
        /// </summary>
        /// <param name="strUserCode"></param>
        /// <param name="strSitePathAndFileName"></param>
        /// <returns></returns>
        public static int UpdateUserLogoImage(String strUserCode, String strSitePathAndFileName)
        {
            int iReturn = 0;
            String strSql = "";
            try
            {
                StringBuilder sbSql = new StringBuilder();
                sbSql.Append("UPDATE LUser_1 SET ProfilePhoto = " + JObjectToDB.GetColumnEncryptDataString("LUser_1", "ProfilePhoto", strSitePathAndFileName));
                sbSql.Append(" where UserCode = " + JObjectToDB.GetColumnEncryptDataString("LUser_1", "UserCode", strUserCode));
                strSql = sbSql.ToString();
                iReturn = SqlParamDao.ExecuteNonQueryBySql(strSql);
            }
            catch (Exception ex)
            {
                iReturn = -1;
                log.Error("更新用户头像连接失败:sql:" + strSql);
                log.Error(ex);

            }
            return iReturn;

        }

        /// <summary>
        /// 更新用户的微信信息
        /// </summary>
        /// <param name="strUserCode"></param>
        /// <param name="strOpenId"></param>
        /// <param name="strUnionId"></param>
        /// <param name="strAvatarUrl"></param>
        /// <returns></returns>
        public static int UpdateUserWenxinInfo(String strUserCode, String strOpenId,String strUnionId,String strAvatarUrl)
        {
            int iReturn = 0;
            String strSql = "";
            try
            {
                StringBuilder sbSql = new StringBuilder();
                sbSql.Append("UPDATE LUser_1 SET OpenId = " + JObjectToDB.GetColumnEncryptDataString("LUser_1", "OpenId", strOpenId));
                sbSql.Append(" ,UnionId = " + JObjectToDB.GetColumnEncryptDataString("LUser_1", "UnionId", strUnionId));
                sbSql.Append(" ,AvatarUrl = " + JObjectToDB.GetColumnEncryptDataString("LUser_1", "AvatarUrl", strAvatarUrl));
                sbSql.Append(" where UserCode = " + JObjectToDB.GetColumnEncryptDataString("LUser_1", "UserCode", strUserCode));
                strSql = sbSql.ToString();
                iReturn = SqlParamDao.ExecuteNonQueryBySql(strSql);
            }
            catch (Exception ex)
            {
                iReturn = -1;
                log.Error("更新用户的微信信息失败:sql:" + strSql);
                log.Error(ex);

            }
            return iReturn;

        }

        /// <summary>
        /// 更新用户OpenId到指定的手机号码
        /// </summary>
        /// <param name="strOpenId"></param>
        /// <param name="strUnionId"></param>
        /// <returns></returns>
        public static int UpdateOpenIdByMobileNo(String strOpenId, String strMobileNo)
        {
            int iReturn = 0;
            String strSql = "";
            try
            {
                StringBuilder sbSql = new StringBuilder();
                sbSql.Append("UPDATE LUser_1 SET OpenId = " + JObjectToDB.GetColumnEncryptDataString("LUser_1", "OpenId", strOpenId));
                sbSql.Append(" where MobileNo = " + JObjectToDB.GetColumnEncryptDataString("LUser_1", "MobileNo", strMobileNo));
                strSql = sbSql.ToString();
                log.Error("更新用户OpenId到指定的手机号码的脚本sql:" + strSql);
                iReturn = SqlParamDao.ExecuteNonQueryBySql(strSql);
            }
            catch (Exception ex)
            {
                iReturn = -1;
                log.Error("更新用户OpenId到指定的手机号码失败:sql:" + strSql);
                log.Error(ex);

            }
            return iReturn;

        }

        /// <summary>
        /// 用人单位添加关注/取消关注某一外包公司获取外包工
        /// </summary>
        /// <param name="hsTableParams"></param>
        /// <returns></returns>
        public static int DoFocusObject(Hashtable hsTableParams)
        {
            int iReturn = 0;
            String strSPName = "USP_Labor_FocusObject";
            try
            {
                iReturn = SqlParamDao.ExcuteSP(strSPName, hsTableParams);
            }
            catch (Exception ex)
            {
                iReturn = -1;
                log.Error("Labor 用人单位添加关注/取消关注某一外包公司" + ex);
                log.Error("用人单位添加关注/取消关注某一外包公司出错:SPName:" + strSPName + ",Params:" + hsTableParams.ToString());
            }
            return iReturn;

        }


        /// <summary>
        /// 注册用户信息后的后续逻辑操作
        /// </summary>
        /// <param name="hsTableParams"></param>
        /// <returns></returns>
        public static int DoAfterRegUser(Hashtable hsTableParams)
        {
            int iReturn = 0;
            String strSPName = "USP_Labor_AfterRegUser";
            try
            {
                iReturn = SqlParamDao.ExcuteSP(strSPName, hsTableParams);
            }
            catch (Exception ex)
            {
                iReturn = -1;
                log.Error("Labor 注册用户信息后的后续逻辑操作出错" + ex);
                log.Error("注册用户信息后的后续逻辑操作出错:SPName:" + strSPName + ",Params:" + hsTableParams.ToString());
            }
            return iReturn;
        }

        /// <summary>
        /// 用户登录成功过的后续逻辑操作
        /// </summary>
        /// <param name="hsTableParams"></param>
        /// <returns></returns>
        public static int DoAfterUserLogin(Hashtable hsTableParams)
        {
            int iReturn = 0;
            String strSPName = "USP_Labor_AfterLogin";
            try
            {
                iReturn = SqlParamDao.ExcuteSP(strSPName, hsTableParams);
            }
            catch (Exception ex)
            {
                iReturn = -1;
                log.Error("Labor 用户登录成功过的后续逻辑操作出错" + ex);
                log.Error("用户登录成功过的后续逻辑操作出错:SPName:" + strSPName + ",Params:" + hsTableParams.ToString());
            }
            return iReturn;
        }

    }
}
