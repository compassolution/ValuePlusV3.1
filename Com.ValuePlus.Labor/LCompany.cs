using Com.ValuePlus.DAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Com.ValuePlus.Labor
{
    public class LCompany
    {
        /// <summary>
        /// 日志声明
        /// </summary>
        protected static Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 根据用户类型自动生成公司编号
        /// </summary>
        /// <param name="strUserType"></param>
        /// <returns></returns>
        public static string GenerateCompanyCode(string strUserType)
        {
            string strCompanyCode = "";
            string strPreFix = "";
            switch (strUserType)
            {
                case "1"://用人单位
                    strPreFix = "E";
                    break;
                case "2"://外包公司
                    strPreFix = "O";
                    break;
            }
            Random rad = new Random();//实例化随机数产生器rad；
            bool IsExists = true;
            while (IsExists)
            {
                //6位随机数
                String strRandomCode = rad.Next(100000, 1000000).ToString();
                strCompanyCode = strPreFix + strRandomCode;

                //String strSql = "select count(1) from LCompany_1 where CompanyCode = '" + strCompanyCode + "'";
                String strSql = "select count(1) from LCompany_1 where CompanyCode = " + JObjectToDB.GetColumnEncryptDataString("LCompany_1", "CompanyCode", strCompanyCode);
                int iCount = SqlParamDao.ExecuteScalarBySql(strSql);
                if (iCount <= 0)
                {
                    IsExists = false;
                    break;
                }
            }

            return strCompanyCode;
        }

        /// <summary>
        /// 通过特定条件获取公司信息数据表集合【获取全部，不分页】
        /// </summary>
        /// <param name="strUserCode"></param>
        /// <param name="strCondition"></param>
        /// <param name="iTopRows">为0时则获取全部</param>
        /// <param name="strOrderBy"></param>
        /// <returns></returns>
        public static DataTable GetCompanyInfoDataTableFromCondition(String strUserCode, String strCondition, int iTopRows, String strOrderBy)
        {
            DataTable dtReturn = new DataTable();
            StringBuilder sbSql = new StringBuilder();
            try
            {
                if (iTopRows == 0)
                {
                    sbSql.Append("select * from [VW_Labor_LCompanyMain] A where 1=1");
                }
                else
                {
                    sbSql.Append("select top "+iTopRows.ToString()+" * from [VW_Labor_LCompanyMain] A where 1=1");
                }
                if (!String.IsNullOrEmpty(strCondition))
                {
                    sbSql.Append(strCondition);
                }
                if (!String.IsNullOrEmpty(strOrderBy))
                {
                    sbSql.Append(" order by "+ strOrderBy);
                }
                String strSql = sbSql.ToString();
                
                log.Error("Labor 通过特定条件获取公司信息数据表集合【获取全部，不分页】，Sql:" + strSql);
                dtReturn = SqlParamDao.GetDataTableBySql(strSql);
            }
            catch (Exception ex)
            {
                log.Error("Labor 通过特定条件获取公司信息数据表集合【获取全部，不分页】出错，Sql:" + sbSql.ToString());
                log.Error(ex);
            }
            return dtReturn;
        }
        
        /// <summary>
        /// 通过公司编码获取公司信息数据表集合
        /// </summary>
        /// <param name="strCompanyCode"></param>
        /// <param name="strIsValid"></param>
        /// <returns></returns>
        public static DataTable GetCompanyInfoDataTable(String strCompanyCode, String strIsValid)
        {
            DataTable dtReturn = new DataTable();
            try
            {
                StringBuilder sbSql = new StringBuilder();

                sbSql.Append("select * from [VW_Labor_LCompanyMain] A where 1=1");
                sbSql.Append(" and CompanyCode = " + JObjectToDB.GetColumnEncryptDataString("LCompany_1", "CompanyCode", strCompanyCode));
                if (!String.IsNullOrEmpty(strIsValid))
                {
                    sbSql.Append(" and A.IsValid = " + JObjectToDB.GetColumnEncryptDataString("LCompany_1", "IsValid", strIsValid));
                }
                String strSql = sbSql.ToString();

                log.Error("Labor 通过公司编码获取公司信息数据表集合，Sql:" + strSql);
                dtReturn = SqlParamDao.GetDataTableBySql(strSql);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return dtReturn;
        }

        /// <summary>
        /// 通过公司编码获取公司所有部门信息数据表集合
        /// </summary>
        /// <param name="strCompanyCode"></param>
        /// <param name="strIsValid"></param>
        /// <returns></returns>
        public static DataTable GetDeptInfoDataTable(String strCompanyCode,String strIsValid)
        {
            DataTable dtReturn = new DataTable();
            try
            {
                StringBuilder sbSql = new StringBuilder();
                
                sbSql.Append("select * from [VW_Labor_LCompanyDept] A where 1=1 ");
                sbSql.Append(" and A.CompanyCode = " + JObjectToDB.GetColumnEncryptDataString("LCompany_1", "CompanyCode", strCompanyCode));
                if (!String.IsNullOrEmpty(strIsValid))
                {
                    sbSql.Append(" and A.IsValid = " + JObjectToDB.GetColumnEncryptDataString("LCompany_1", "IsValid", strIsValid));
                }
                sbSql.Append(" order by A.IsValid,A.CreateTime DESC");
                String strSql = sbSql.ToString();

                log.Error("Labor 通过公司编码获取公司所有部门信息数据表集合，Sql:" + strSql);
                dtReturn = SqlParamDao.GetDataTableBySql(strSql);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return dtReturn;
        }

        /// <summary>
        /// 通过公司编码获取公司就餐时间信息数据表集合
        /// </summary>
        /// <param name="strCompanyCode"></param>
        /// <returns></returns>
        public static DataTable GetDiningTimeInfoDataTable(String strCompanyCode)
        {
            DataTable dtReturn = new DataTable();
            try
            {
                StringBuilder sbSql = new StringBuilder();

                sbSql.Append("select * from [VW_Labor_LCompanyDiningTime] A where 1=1 ");
                sbSql.Append(" and A.CompanyCode = " + JObjectToDB.GetColumnEncryptDataString("LCompany_1", "CompanyCode", strCompanyCode));
                sbSql.Append(" order by A.CompanyCode");
                String strSql = sbSql.ToString();

                log.Error("Labor 通过公司编码获取公司就餐时间信息数据表集合，Sql:" + strSql);
                dtReturn = SqlParamDao.GetDataTableBySql(strSql);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return dtReturn;
        }


        /// <summary>
        /// 通过公司编码获取公司所有部门及账号信息数据表集合,返回json数据对象
        /// </summary>
        /// <param name="strCompanyCode"></param>
        /// <param name="strIsValid"></param>
        /// <param name="isEscape"></param>
        /// <returns></returns>
        public static String GetDeptAndAccountJsonData(String strCompanyCode, String strIsValid,bool isEscape)
        {
            StringBuilder sBuilder = new StringBuilder();
            string json = "";
            try
            {
                //公司下面部门信息
                DataTable dtDept = GetDeptInfoDataTable(strCompanyCode, strIsValid);

                int iColCount = dtDept.Columns.Count;
                //sBuilder.Append("\"" + strNodeName + "\":[ ");
                sBuilder.Append("\"ResultData\"" + ":[ ");
                if ((dtDept != null) && (dtDept.Rows.Count > 0))
                {
                    for (int i = 0; i < dtDept.Rows.Count; i++)
                    {
                        if (i > 0)
                        {
                            sBuilder.Append(",{");
                        }
                        else
                        {
                            sBuilder.Append("{");
                        }
                        for (int j = 0; j < dtDept.Columns.Count; j++)
                        {
                            String strColName = dtDept.Columns[j].ColumnName;
                            String strColType = dtDept.Columns[j].DataType.ToString();
                            //对值进行编码处理特殊字符，如引号等
                            String strColValue = dtDept.Rows[i][dtDept.Columns[j].ColumnName].ToString();
                            if (!String.IsNullOrEmpty(strColValue))
                            {
                                switch (strColType)
                                {
                                    case "System.DateTime":
                                        strColValue = DateTime.Parse(strColValue).ToString("yyyy-MM-dd HH:mm:ss");
                                        //如果是短日期
                                        if (strColValue.EndsWith("00:00:00"))
                                        {
                                            strColValue = strColValue.Substring(0, strColValue.Length - 9);
                                        }
                                        break;
                                }
                            }
                            if (isEscape)
                            {
                                strColValue = Microsoft.JScript.GlobalObject.escape(strColValue);
                            }

                            if (j == 0)
                            {
                                sBuilder.Append("\"" + strColName + "\":\"" + strColValue + "\"");
                            }
                            else
                            {
                                sBuilder.Append(",\"" + strColName + "\":\"" + strColValue + "\"");
                            }

                            //部门编码则获取对象编码下的账号信息
                            if (strColName.ToLower().Equals("deptcode"))
                            {
                                DataTable dtUser = LUser.GetUserInfoDataTableByDept(strCompanyCode, strColValue, strIsValid);
                                sBuilder.Append("," + CommonJson.GetJsonStringByDataTable(dtUser, "\"AccountList_Dept\"", isEscape));
                            }
                        }

                        sBuilder.Append("}");
                    }

                }

                sBuilder.Append("]");

                json = sBuilder.ToString();
                log.Error("通过公司编码获取公司所有部门及账号信息数据表集合,返回json数据对象:" + json);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return json;
        }

        /// <summary>
        /// 根据用户编码获取我的伙伴公司列表集合
        /// </summary>
        /// <param name="strUserCode"></param>
        /// <param name="strAimUserType"></param>
        /// <param name="strUserCode"></param>
        /// <param name="strFocusType">关注类型仅我是用人单位是有效</param>
        /// <param name="strIsValid"></param>
        /// <param name="strCondition"></param>
        /// <param name="iTopRows"></param>
        /// <param name="strOrderBy"></param>
        /// <returns></returns>
        public static DataTable GetMyPartnerCompanyDataTable(String strUserCode, String strAimUserType, String strFocusType,String strIsValid, String strCondition, int iTopRows, String strOrderBy)
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
                sbSql.Append("select " + strTop + " * from [VW_Labor_LCompanyMain] A INNER JOIN ");
                sbSql.Append("  [dbo].[Fun_Labor_GetPartnerCode_ByUserCode]('" + strUserCode + "','" + strAimUserType + "','"+ strFocusType + "','" + strIsValid + "') B ON A.CompanyCode = B.Partner_Code");
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

                log.Error("Labor 根据用户编码获取我的伙伴公司列表集合，Sql:" + strSql);
                dtReturn = SqlParamDao.GetDataTableBySql(strSql);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.Error("Labor 根据用户编码获取我的伙伴公司列表集合出错，Sql:" + sbSql.ToString());
            }
            return dtReturn;
        }

        /// <summary>
        /// 更新公司logo头像连接
        /// </summary>
        /// <param name="strCompanyCode"></param>
        /// <param name="strSitePathAndFileName"></param>
        /// <returns></returns>
        public static int UpdateCompanyLogoImage(String strCompanyCode, String strSitePathAndFileName)
        {
            int iReturn = 0;
            String strSql = "";
            try
            {
                //strSql = "UPDATE LCompany_1 set CompanyLogo = '" + strSitePathAndFileName + "' where CompanyCode = '" + strCompanyCode + "'";
                StringBuilder sbSql = new StringBuilder();
                sbSql.Append("UPDATE LCompany_1 SET CompanyLogo = " + JObjectToDB.GetColumnEncryptDataString("LCompany_1", "CompanyLogo", strSitePathAndFileName));
                sbSql.Append(" where CompanyCode = " + JObjectToDB.GetColumnEncryptDataString("LCompany_1", "CompanyCode", strCompanyCode));
                strSql = sbSql.ToString();

                iReturn = SqlParamDao.ExecuteNonQueryBySql(strSql);
            }
            catch (Exception ex)
            {
                iReturn = -1;
                log.Error("更新公司logo头像连接失败:sql:" + strSql);
                log.Error(ex);

            }
            return iReturn;

        }
        
        /// <summary>
        /// 删除部门信息
        /// </summary>
        /// <param name="strCompanyCode"></param>
        /// <param name="strDeptCode"></param>
        /// <param name="strSetIsValid"></param>
        /// <param name="strUserCode"></param>
        /// <returns></returns>
        public static int DeleteDeptInfo(String strCompanyCode, String strDeptCode, String strSetIsValid, String strUserCode)
        {
            String strCurDataTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            StringBuilder sbSql = new StringBuilder();
            if (!String.IsNullOrEmpty(strSetIsValid))
            {
                sbSql.Append("update LCompany_2 set IsValid = " + JObjectToDB.GetColumnEncryptDataString("LCompany_2", "IsValid", strSetIsValid));
                sbSql.Append("  ,LastModifyUser = " + JObjectToDB.GetColumnEncryptDataString("LCompany_2", "LastModifyUser", strUserCode));
                sbSql.Append("  ,LastModifyTime = " + JObjectToDB.GetColumnEncryptDataString("LCompany_2", "LastModifyTime", strCurDataTime));
                sbSql.Append(" where CompanyCode = " + JObjectToDB.GetColumnEncryptDataString("LCompany_2", "CompanyCode", strCompanyCode));
                sbSql.Append(" and DeptCode = " + JObjectToDB.GetColumnEncryptDataString("LCompany_2", "DeptCode", strDeptCode));

            }
            else
            {
                sbSql.Append("delete from LCompany_2 ");
                sbSql.Append(" where CompanyCode = " + JObjectToDB.GetColumnEncryptDataString("LCompany_2", "CompanyCode", strCompanyCode));
                sbSql.Append(" and DeptCode = " + JObjectToDB.GetColumnEncryptDataString("LCompany_2", "DeptCode", strDeptCode));
                //同时将该部门下的账号的部门置空
                sbSql.Append("update LUser_1 set DeptCode = ''");
                sbSql.Append(" where CompanyCode = " + JObjectToDB.GetColumnEncryptDataString("LCompany_2", "CompanyCode", strCompanyCode));
                sbSql.Append(" and DeptCode = " + JObjectToDB.GetColumnEncryptDataString("LCompany_2", "DeptCode", strDeptCode));
            }
            log.Error("删除部门信息:sql:" + sbSql.ToString());
            int iCount = SqlParamDao.ExecuteNonQueryBySql(sbSql.ToString());
            return iCount;

        }

        /// <summary>
        /// 注册公司信息后的后续逻辑操作
        /// </summary>
        /// <param name="hsTableParams"></param>
        /// <returns></returns>
        public static int DoAfterRegCompany(Hashtable hsTableParams)
        {
            int iReturn = 0;
            String strSPName = "USP_Labor_AfterRegCompany";
            try
            {
                iReturn = SqlParamDao.ExcuteSP(strSPName, hsTableParams);
            }
            catch (Exception ex)
            {
                iReturn = -1;
                log.Error("Labor 注册公司信息后的后续逻辑操作出错" + ex);
                log.Error("注册公司信息后的后续逻辑操作出错:SPName:" + strSPName + ",Params:" + hsTableParams.ToString());
            }
            return iReturn;
        }

        /// <summary>
        /// 外包公司添加外包工或者自行退出挂靠公司的操作
        /// </summary>
        /// <param name="hsTableParams"></param>
        /// <returns></returns>
        public static int DoLaborDockOutsourced(Hashtable hsTableParams)
        {
            int iReturn = 0;
            String strSPName = "USP_Labor_DockLabor";
            try
            {
                iReturn = SqlParamDao.ExcuteSP(strSPName, hsTableParams);
            }
            catch (Exception ex)
            {
                iReturn = -1;
                log.Error("Labor 外包公司添加外包工或者自行退出挂靠公司的操作出错" + ex);
                log.Error("外包公司添加外包工或者自行退出挂靠公司的操作出错:SPName:" + strSPName + ",Params:" + hsTableParams.ToString());
            }
            return iReturn;
        }

    }
}
