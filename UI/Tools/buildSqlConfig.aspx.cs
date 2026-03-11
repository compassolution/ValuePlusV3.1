using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data.SqlClient;
using System.Text;
using Com.ValuePlus.Common.Config;
using System.IO;
using Com.ValuePlus.Web;

public partial class Tools_buildSqlConfig : PageBase
{
    private String strConfigFileName = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        this.strConfigFileName = this.txtConfigFile.Text.ToString().Trim();
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        String connstring = BaseConfig.Instance.GetConnectionString();
        //string connstring = "User ID=sa;PWD=1234567890;Initial Catalog=DB_VALUEPLUS;Data Source=localhost;";
        String strTableName = this.txtTableName.Text;
        String strOpType = this.dList.Text;
        if(strTableName!=null){
            strTableName = strTableName.Trim().ToUpper();
            String tableNameStr = "select ID from sysobjects where name = '" + strTableName + "'";
            using (SqlConnection conn = new SqlConnection(connstring))
            {
                StringBuilder sbResult = new StringBuilder();
                StringBuilder sbResult1 = new StringBuilder();
                StringBuilder sbResult2 = new StringBuilder();
                StringBuilder sbResult3 = new StringBuilder();
                StringBuilder sbResult4 = new StringBuilder();

                String strColNameStr1 = "";
                String strColNameStr2 = "";
                String strColNameStr3 = "";
                String strSqlStr = "";

                String strColName = "";
                String strTypeName = "";
                String strLength = "";

                conn.Open();
                 //使用信息架构视图
                SqlCommand sqlcmd = new SqlCommand("SELECT A.NAME,B.NAME,C.NAME,B.LENGTH FROM sysobjects A,syscolumns B,systypes C WHERE A.xtype = 'U' AND A.name = '" + strTableName + "' AND A.ID = B.ID	AND B.XTYPE = C.XTYPE  order by B.colorder", conn);
                SqlDataReader dr=sqlcmd.ExecuteReader();
                int iCount = 0;
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        strColName = dr.GetString(1);
                        strTypeName = dr.GetValue(2).ToString();
                        strLength = dr.GetValue(3).ToString();

                        if (strOpType.Equals("selectAll"))
                        {
                            strColNameStr1 += "," + strColName;
                            if (iCount == 0)
                            {
                                strColNameStr2 = strColName;
                            }
                        }
                        else if (strOpType.Equals("selectByKey"))
                        {
                            strColNameStr1 += "," + strColName;
                            if (iCount == 0)
                            {
                                strColNameStr2 = strColName;
                                strColNameStr3 = "@" + strColName;
                                sbResult2.Append(buildColConfig(strColName, strTypeName, strLength));
                            }
                        }
                        else if (strOpType.Equals("insert"))
                        {
                            strColNameStr1 += "," + strColName;
                            strColNameStr2 += ",@" + strColName;
                            sbResult2.Append(buildColConfig(strColName, strTypeName, strLength));
                        }
                        else if (strOpType.Equals("updateByKey"))
                        {
                            strColNameStr1 += "," + strColName + "=@" + strColName;
                            if (iCount == 0)
                            {
                                strColNameStr2 = strColName;
                                strColNameStr3 = "@" + strColName;
                            }
                            sbResult2.Append(buildColConfig(strColName, strTypeName, strLength));
                        }
                        else if (strOpType.Equals("deleteByKey"))
                        {
                            if (iCount == 0)
                            {
                                strColNameStr2 = strColName;
                                strColNameStr3 = "@" + strColName;
                                sbResult2.Append(buildColConfig(strColName, strTypeName, strLength));
                            }
                        }
                        else
                        {
                        }

                        iCount++;
                    }
                    if (strOpType.Equals("selectAll"))
                    {
                        strColNameStr1 = strColNameStr1.Substring(1, strColNameStr1.Length - 1);
                        strSqlStr = "SELECT " + strColNameStr1 + " FROM " + strTableName + " ORDER BY " + strColNameStr2;
                    }
                    else if (strOpType.Equals("selectByKey"))
                    {
                        strColNameStr1 = strColNameStr1.Substring(1, strColNameStr1.Length - 1);
                        strSqlStr = "SELECT " + strColNameStr1 + " FROM " + strTableName + " WHERE " + strColNameStr2 + " =" + strColNameStr3;
                    }
                    else if (strOpType.Equals("insert"))
                    {
                        strColNameStr1 = strColNameStr1.Substring(1, strColNameStr1.Length - 1);
                        strColNameStr2 = strColNameStr2.Substring(1, strColNameStr2.Length - 1);
                        strSqlStr = "INSERT INTO " + strTableName + "( " + strColNameStr1 + ") VALUES (" + strColNameStr2 + ")";
                    }
                    else if (strOpType.Equals("updateByKey"))
                    {
                        strColNameStr1 = strColNameStr1.Substring(1, strColNameStr1.Length - 1);
                        strSqlStr = "UPDATE " + strTableName + " SET " + strColNameStr1 + " WHERE " + strColNameStr2 + " =" + strColNameStr3;
                    }
                    else if (strOpType.Equals("deleteByKey"))
                    {
                        strSqlStr = "DELETE FROM " + strTableName + " WHERE " + strColNameStr2 + " =" + strColNameStr3;
                    }
                    else
                    {
                    }

                    sbResult.Append(buildHeadCofing(strTableName, strSqlStr, strOpType));
                    sbResult.Append(sbResult2.ToString());
                    sbResult.Append(buildBottomCofing(strTableName, strOpType));
                }
                conn.Close();

                this.txtResult.Text = sbResult.ToString();
            }
        }
        

    }

    protected void Button2_Click(object sender, EventArgs e)
    {
        this.buildGetCofingSqlMethod();
    }

    protected void Button3_Click(object sender, EventArgs e)
    {
        this.buildDALMethod();
    }

    protected void Button4_Click(object sender, EventArgs e)
    {
        Page.Response.Redirect("buildEntityClass.aspx");
    }

    #region 生成SqlConfig配置文件
    private String buildHeadCofing(String strTableName, String strSqlStr,String strOpType)
    {
        StringBuilder sbResult = new StringBuilder();
        sbResult.Append("<!--对数据表【" + strTableName + "】的" + strOpType + "操作相关配置文件-->\r\n");
        sbResult.Append("<" + strTableName + "."+strOpType+">\r\n");
        sbResult.Append("  <SqlBasicMetaData>\r\n");
        sbResult.Append("    <CommandType>text</CommandType>\r\n");
        sbResult.Append("    <CommandSql>" + strSqlStr + "</CommandSql>\r\n");
        sbResult.Append("    <NameValueParameters>\r\n");
        return sbResult.ToString();
    }

    private String buildColConfig(String strColName, String strTypeName, String strLength)
    {
        StringBuilder sbResult = new StringBuilder();
        sbResult.Append("      <NameValueParameters>\r\n");
        sbResult.Append("        <VariableName>@" + strColName + "</VariableName>\r\n");
        sbResult.Append("        <VariableType>" + strTypeName + "</VariableType>\r\n");
        sbResult.Append("        <VariableLength>" + strLength + "</VariableLength>\r\n");
        sbResult.Append("      </NameValueParameters>\r\n");
        return sbResult.ToString();
    }

    private String buildBottomCofing(String strTableName, String strOpType)
    {
        StringBuilder sbResult = new StringBuilder();
        sbResult.Append("    </NameValueParameters>\r\n");
        sbResult.Append("  </SqlBasicMetaData>\r\n");
        sbResult.Append("</" + strTableName + "." + strOpType + ">\r\n");
        return sbResult.ToString();
    }
    #endregion

    #region 生成读取SqlConfig配置文件的方法
    /// <summary>
    /// 生成读取SqlConfig配置文件的方法
    /// </summary>
    private void buildGetCofingSqlMethod()
    {
        String strTableName = this.txtTableName.Text;
        String strOpType = this.dList.Text;
        String strSummary = "";
        switch (strOpType)
        {
            case "selectAll":
                strSummary = "【获取表" + strTableName + "所有记录】";
                break;
            case "selectByKey":
                strSummary = "【根据主键获取表" + strTableName + "一条记录】";
                break;
            case "insert":
                strSummary = "【添加一条记录到表" + strTableName + "中】";
                break;
            case "updateByKey":
                strSummary = "【根据主键更新表" + strTableName + "一条记录】";
                break;
            case "deleteByKey":
                strSummary = "【根据主键删除表" + strTableName + "一条记录】";
                break;
        }

        StringBuilder sbResult = new StringBuilder();
        sbResult.Append("        /// <summary>\r\n");
        sbResult.Append("        /// 获取sql语句" + strSummary + "\r\n");
        sbResult.Append("        /// </summary>\r\n");
        sbResult.Append("        /// <returns></returns>\r\n");
        sbResult.Append("        public Com.ValuePlus.Utils.SqlBasicMetaData GetSqlFor" + strTableName + "_" + strOpType + "()\r\n");
        sbResult.Append("        {\r\n");
        sbResult.Append("           return ConfigCache.GetSqlBasicMetaData(\"" + strTableName + "." + strOpType + "\");\r\n");
        sbResult.Append("        }\r\n");

        this.txtResult.Text = sbResult.ToString();

    }
    #endregion

    #region 生成DAL数据访问方法
    /// <summary>
    /// 生成DAL数据访问方法
    /// </summary>
    private void buildDALMethod()
    {
        String strTableName = this.txtTableName.Text;
        String strOpType = this.dList.Text;
        StringBuilder sbResult = new StringBuilder();
        switch (strOpType)
        {
            case "selectAll":
                sbResult.Append(this.buildMethod_SelectAll(strTableName));
                break;
            case "selectByKey":
                sbResult.Append(this.buildMethod_SelectByKey(strTableName));
                break;
            case "insert":
                sbResult.Append(this.buildMethod_Insert(strTableName));
                break;
            case "updateByKey":
                sbResult.Append(this.buildMethod_UpdateByKey(strTableName));
                break;
            case "deleteByKey":
                sbResult.Append(this.buildMethod_DeleteByKey(strTableName));
                break;
        }

        this.txtResult.Text = sbResult.ToString();

    }
    /// <summary>
    /// 生成selectAll的DAL方法
    /// </summary>
    /// <param name="strTableName"></param>
    /// <returns></returns>
    private String buildMethod_SelectAll(String strTableName)
    {
        String strSummary = "查询表" + strTableName + "所有记录，返回dateset记录集";
        StringBuilder sbResult = new StringBuilder();

        sbResult.Append("        #region " + strSummary + "\r\n");
        sbResult.Append("        /// <summary>\r\n");
        sbResult.Append("        /// " + strSummary + "\r\n");
        sbResult.Append("        /// </summary>\r\n");
        sbResult.Append("        /// <returns>DataSet</returns>\r\n");
        sbResult.Append("        public DataSet findAll()\r\n");
        sbResult.Append("        {\r\n");
        sbResult.Append("           DataSet ds = new DataSet();\r\n");
        sbResult.Append("           using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())\r\n");
        sbResult.Append("           {\r\n");
        sbResult.Append("               ds = dao.ExecuteDataSet("+this.strConfigFileName+".Instance.GetSqlFor" + strTableName + "_selectAll(), null);\r\n");
        sbResult.Append("           }\r\n");
        sbResult.Append("           return ds;\r\n");
        sbResult.Append("        }\r\n");
        sbResult.Append("        #endregion\r\n");

        return sbResult.ToString();
    }
    /// <summary>
    /// 生成selectByKey的DAL方法
    /// </summary>
    /// <param name="strTableName"></param>
    /// <returns></returns>
    private String buildMethod_SelectByKey(String strTableName)
    {
        String strSummary = "根据主键查询" + strTableName + "的相应记录，返回dateset记录集";
        StringBuilder sbResult = new StringBuilder();

        sbResult.Append("        #region " + strSummary + "\r\n");
        sbResult.Append("        /// <summary>\r\n");
        sbResult.Append("        /// " + strSummary + "\r\n");
        sbResult.Append("        /// </summary>\r\n");
        sbResult.Append("        /// <returns>DataSet</returns>\r\n");
        sbResult.Append("        public DataSet findByKey(String strKeyValue)\r\n");
        sbResult.Append("        {\r\n");
        sbResult.Append("           DataSet ds = new DataSet();\r\n");
        sbResult.Append("           using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())\r\n");
        sbResult.Append("           {\r\n");
        sbResult.Append("               DbParameter[] param = dao.MakeParameter("+this.strConfigFileName+".Instance.GetSqlFor" + strTableName + "_selectByKey());\r\n");
        sbResult.Append("               param[0].Value = strKeyValue;\r\n");
        sbResult.Append("               ds = dao.ExecuteDataSet("+this.strConfigFileName+".Instance.GetSqlFor" + strTableName + "_selectByKey(), param);\r\n");
        sbResult.Append("           }\r\n");
        sbResult.Append("           return ds;\r\n");
        sbResult.Append("        }\r\n");
        sbResult.Append("        #endregion\r\n");

        return sbResult.ToString();
    }
    /// <summary>
    /// 生成insert的DAL方法
    /// </summary>
    /// <param name="strTableName"></param>
    /// <returns></returns>
    private String buildMethod_Insert(String strTableName)
    {
        String strSummary = "新增一条记录到表" + strTableName + "中，返回成功新增记录数";
        String strColParam = "String ";
        StringBuilder sbParamResult = this.GetTableColumnInfor(strTableName, ref strColParam);

        StringBuilder sbResult = new StringBuilder();
        sbResult.Append("        #region " + strSummary + "\r\n");
        sbResult.Append("        /// <summary>\r\n");
        sbResult.Append("        /// " + strSummary + "\r\n");
        sbResult.Append("        /// </summary>\r\n");
        sbResult.Append("        /// <returns>DataSet</returns>\r\n");
        sbResult.Append("        public int insertOneRow("+strColParam+")\r\n");
        sbResult.Append("        {\r\n");
        sbResult.Append("           int count = 0;\r\n");
        sbResult.Append("           using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())\r\n");
        sbResult.Append("           {\r\n");
        sbResult.Append("               DbParameter[] param = dao.MakeParameter("+this.strConfigFileName+".Instance.GetSqlFor" + strTableName + "_insert());\r\n");
        sbResult.Append(sbParamResult);
        sbResult.Append("               object obj = dao.ExecuteScalar("+this.strConfigFileName+".Instance.GetSqlFor" + strTableName + "_insert(), param);\r\n");
        sbResult.Append("               if (obj != null)\r\n");
        sbResult.Append("               {\r\n");
        sbResult.Append("                   count = Convert.ToInt32(obj);;\r\n");
        sbResult.Append("               }\r\n");
        sbResult.Append("           }\r\n");
        sbResult.Append("           return count;\r\n");
        sbResult.Append("        }\r\n");
        sbResult.Append("        #endregion\r\n");

        return sbResult.ToString();
    }
    /// <summary>
    /// 生成updateByKey的DAL方法
    /// </summary>
    /// <param name="strTableName"></param>
    /// <returns></returns>
    private String buildMethod_UpdateByKey(String strTableName)
    {
        String strSummary = "根据主键更新表" + strTableName + "一条记录，返回成功更新记录数";
        String strColParam = "String ";
        StringBuilder sbParamResult = this.GetTableColumnInfor(strTableName, ref strColParam);

        StringBuilder sbResult = new StringBuilder();
        sbResult.Append("        #region " + strSummary + "\r\n");
        sbResult.Append("        /// <summary>\r\n");
        sbResult.Append("        /// " + strSummary + "\r\n");
        sbResult.Append("        /// </summary>\r\n");
        sbResult.Append("        /// <returns>DataSet</returns>\r\n");
        sbResult.Append("        public int updateByKey(" + strColParam + ")\r\n");
        sbResult.Append("        {\r\n");
        sbResult.Append("           int count = 0;\r\n");
        sbResult.Append("           using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())\r\n");
        sbResult.Append("           {\r\n");
        sbResult.Append("               DbParameter[] param = dao.MakeParameter("+this.strConfigFileName+".Instance.GetSqlFor" + strTableName + "_updateByKey());\r\n");
        sbResult.Append(sbParamResult);
        sbResult.Append("               object obj = dao.ExecuteNonQuery("+this.strConfigFileName+".Instance.GetSqlFor" + strTableName + "_updateByKey(), param);\r\n");
        sbResult.Append("               if (obj != null)\r\n");
        sbResult.Append("               {\r\n");
        sbResult.Append("                   count = Convert.ToInt32(obj);;\r\n");
        sbResult.Append("               }\r\n");
        sbResult.Append("           }\r\n");
        sbResult.Append("           return count;\r\n");
        sbResult.Append("        }\r\n");
        sbResult.Append("        #endregion\r\n");

        return sbResult.ToString();
    }
    /// <summary>
    /// 生成deleteByKey的DAL方法
    /// </summary>
    /// <param name="strTableName"></param>
    /// <returns></returns>
    private String buildMethod_DeleteByKey(String strTableName)
    {
        String strSummary = "根据主键删除表" + strTableName + "的相应记录，返回成功删除记录数";
        StringBuilder sbResult = new StringBuilder();

        sbResult.Append("        #region " + strSummary + "\r\n");
        sbResult.Append("        /// <summary>\r\n");
        sbResult.Append("        /// " + strSummary + "\r\n");
        sbResult.Append("        /// </summary>\r\n");
        sbResult.Append("        /// <returns>DataSet</returns>\r\n");
        sbResult.Append("        public int deleteByKey(String strKeyValue)\r\n");
        sbResult.Append("        {\r\n");
        sbResult.Append("           int count = 0;\r\n");
        sbResult.Append("           using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())\r\n");
        sbResult.Append("           {\r\n");
        sbResult.Append("               DbParameter[] param = dao.MakeParameter("+this.strConfigFileName+".Instance.GetSqlFor" + strTableName + "_deleteByKey());\r\n");
        sbResult.Append("               param[0].Value = strKeyValue;\r\n");
        sbResult.Append("               object obj = dao.ExecuteNonQuery("+this.strConfigFileName+".Instance.GetSqlFor" + strTableName + "_deleteByKey(), param);\r\n");
        sbResult.Append("               if (obj != null)\r\n");
        sbResult.Append("               {\r\n");
        sbResult.Append("                   count = Convert.ToInt32(obj);;\r\n");
        sbResult.Append("               }\r\n");
        sbResult.Append("           }\r\n");
        sbResult.Append("           return count;\r\n");
        sbResult.Append("        }\r\n");
        sbResult.Append("        #endregion\r\n");

        return sbResult.ToString();
    }

    /// <summary>
    /// 根据表名生成参数匹配字符串，同时返回参数名字符串
    /// </summary>
    /// <param name="strTableName"></param>
    /// <param name="strColParam"></param>
    /// <returns></returns>
    private StringBuilder GetTableColumnInfor(String strTableName,ref String strColParam)
    {
        StringBuilder sbResult = new StringBuilder();
        strColParam = "";
        String connstring = BaseConfig.Instance.GetConnectionString();
        if (strTableName != null)
        {
            strTableName = strTableName.Trim().ToUpper();
            String tableNameStr = "select ID from sysobjects where name = '" + strTableName + "'";
            using (SqlConnection conn = new SqlConnection(connstring))
            {
                String strColName = "";
                String strTypeName = "";
                String strColNameParam = "";

                conn.Open();
                //使用信息架构视图
                SqlCommand sqlcmd = new SqlCommand("SELECT A.NAME,B.NAME,C.NAME,B.LENGTH FROM sysobjects A,syscolumns B,systypes C WHERE A.xtype = 'U' AND A.name = '" + strTableName + "' AND A.ID = B.ID	AND B.XTYPE = C.XTYPE order by B.colorder", conn);
                SqlDataReader dr = sqlcmd.ExecuteReader();
                int iCount = 0;
                while (dr.Read())
                {
                    String strParamPrefix = "";
                    strColName = dr.GetString(1).ToUpper();
                    strTypeName = dr.GetValue(2).ToString();
                    strTypeName = this.DataTypeToCType(strTypeName, ref strParamPrefix);
                    if (iCount == 0)
                    {
                        strColNameParam = strColNameParam + strTypeName + " " + strParamPrefix+ strColName;
                    }
                    else
                    {
                        strColNameParam = strColNameParam + "," + strTypeName + " " + strParamPrefix +  strColName;
                    }

                    sbResult.Append("               param[" + iCount.ToString() + "].Value = " + strParamPrefix  + strColName + ";\r\n");
                    iCount++;
                }
                conn.Close();

                strColParam = strColNameParam;
            }
        }
        return sbResult;
    }

    /// <summary>
    /// 将sql数据类型转换成C#变量类型
    /// </summary>
    /// <param name="dataType"></param>
    /// <returns></returns>
    private string DataTypeToCType(string dataType, ref String strParamPrefix)
    {
        string retType = "";
        if (dataType.Equals("text") || dataType.Equals("varchar") || dataType.Equals("char") || dataType.Equals("nvarchar") || dataType.Equals("nchar"))
        {
            strParamPrefix = "str";
            retType = "string";
        }
        else if (dataType.Equals("int"))
        {
            strParamPrefix = "i";
            retType = "int";
        }
        else if (dataType.Equals("smallint"))
        {
            strParamPrefix = "i";
            retType = "Int16";
        }
        else if (dataType.Equals("tinyint"))
        {
            strParamPrefix = "str";
            retType = "byte";
        }
        else if  (dataType.Equals("bigint"))
        {
            strParamPrefix = "n";
            retType = "long";
        }
        else if (dataType.Equals("bit"))
        {
            strParamPrefix = "b";
            retType = "bool";
        }
        else if (dataType.Equals("money") || dataType.Equals("smallmoney") || dataType.Equals("numeric") || dataType.Equals("decimal"))
        {
            strParamPrefix = "n";
            retType = "Decimal";
        }
        else if (dataType.Equals("datetime") || dataType.Equals("smalldatetime") || dataType.Equals("timestamp"))
        {
            strParamPrefix = "dt";
            retType = "DateTime";
        }
        else if (dataType.Equals("real"))
        {
            strParamPrefix = "n";
            retType = "Single";
        }
        else if (dataType.Equals("float"))
        {
            strParamPrefix = "n";
            retType = "double";
        }
        else if (dataType.Equals("image") || dataType.Equals("binary") || dataType.Equals("varbinary"))
        {
            strParamPrefix = "byte";
            retType = "byte[]";
        }
        else if (dataType.Equals("uniqueidentifier"))
        {
            strParamPrefix = "str";
            retType = "Guid";
        }

        return retType;
    }
    #endregion

}
