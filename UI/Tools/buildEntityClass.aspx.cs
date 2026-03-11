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

public partial class Tools_buildEntityClass : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            String strFilePath = base.MapPath("");
            String strTableName = this.txtTableName.Text;

            this.txtFilePath.Text = strFilePath + "\\AutoFile";

            this.txtTableName.Attributes.Add("onkeyup", "setFileName();");
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        this.buildEntityClass(); 
    }

    protected void Button3_Click(object sender, EventArgs e)
    {
        this.setDataRowToEntity();
    }

    /// <summary>
    /// 根据表结构生成实体类
    /// </summary>
    private void buildEntityClass()
    {
        String strTableName = this.txtTableName.Text;
        String strNameSpace = this.txtNameSpace.Text;
        String strClassName = "Entity_" + strTableName;

        String connstring = BaseConfig.Instance.GetConnectionString();
        if (strTableName != null)
        {
            strTableName = strTableName.Trim().ToUpper();
            using (SqlConnection conn = new SqlConnection(connstring))
            {
                conn.Open();
                //使用信息架构视图
                SqlCommand sqlcmd = new SqlCommand("SELECT A.NAME,B.NAME,C.NAME,B.LENGTH FROM sysobjects A,syscolumns B,systypes C WHERE A.xtype = 'U' AND A.name = '" + strTableName + "' AND A.ID = B.ID	AND B.XTYPE = C.XTYPE  order by B.colorder", conn);
                SqlDataReader dr = sqlcmd.ExecuteReader();
                int iCount = 0;
                if (dr.HasRows)
                {
                    String strColName = "";
                    String strTypeName = "";
                    String strLength = "";

                    StringBuilder strBAll = new StringBuilder();
                    StringBuilder strBParam = new StringBuilder();
                    StringBuilder strBMethod = new StringBuilder();

                    strBAll.Append("using System;\r\n");
                    strBAll.Append("using System.Text;\r\n");
                    strBAll.Append("\r\n");
                    strBAll.Append("namespace " + strNameSpace + "\r\n");
                    strBAll.Append("{\r\n");

                    strBAll.Append("    /// <summary>\r\n");
                    strBAll.Append("    /// 数据表" + strTableName + "实体类\r\n");
                    strBAll.Append("    /// </summary>\r\n");
                    strBAll.Append("    [Serializable]\r\n");
                    strBAll.Append("    public class " + strClassName + "\r\n");
                    strBAll.Append("    {\r\n");

                    while (dr.Read())
                    {
                        strColName = dr.GetValue(1).ToString().ToUpper();
                        strTypeName = dr.GetValue(2).ToString();
                        strTypeName = this.DataTypeToCType(strTypeName);
                        strLength = dr.GetValue(3).ToString().ToUpper();

                        strBParam.Append("        private " + strTypeName + "  " + "_" + strColName + ";" + "\r\n");

                        strBMethod.Append("        /// <summary>\r\n");
                        strBMethod.Append("        /// 对属性" + strColName + "的读写\r\n");
                        strBMethod.Append("        /// </summary>\r\n");
                        strBMethod.Append("        public " + strTypeName + "  " + strColName + "\r\n");
                        strBMethod.Append("        {\r\n");
                        strBMethod.Append("            get{return _" + strColName + ";}\r\n");
                        strBMethod.Append("            set{_" + strColName + "=value;}\r\n");
                        strBMethod.Append("        }\r\n");
                        strBMethod.Append(" \r\n");

                        iCount++;
                    }
                    conn.Close();

                    strBAll.Append(strBParam);
                    strBAll.Append("\r\n");
                    strBAll.Append(strBMethod);
                    strBAll.Append("    }\r\n");
                    strBAll.Append("}\r\n");

                    this.txtResult.Text = strBAll.ToString();
                    this.txtFileName.Text = strClassName;
                    this.WriteFile(strBAll.ToString(), strClassName);
                }
            }
        }


    }

    /// <summary>
    /// 将DataRow填充到实体变量中
    /// </summary>
    private void setDataRowToEntity()
    {
        String strTableName = this.txtTableName.Text;
        String strNameSpace = this.txtNameSpace.Text;
        String strEntityName = "Entity_" + strTableName;
        String strMethodName = "SetDataTo" + strEntityName;

        String connstring = BaseConfig.Instance.GetConnectionString();
        if (strTableName != null)
        {
            strTableName = strTableName.Trim().ToUpper();
            using (SqlConnection conn = new SqlConnection(connstring))
            {
                conn.Open();
                //使用信息架构视图
                SqlCommand sqlcmd = new SqlCommand("SELECT A.NAME,B.NAME,C.NAME,B.LENGTH FROM sysobjects A,syscolumns B,systypes C WHERE A.xtype = 'U' AND A.name = '" + strTableName + "' AND A.ID = B.ID	AND B.XTYPE = C.XTYPE  order by B.colorder", conn);
                SqlDataReader dr = sqlcmd.ExecuteReader();
                int iCount = 0;
                if (dr.HasRows)
                {
                    String strColName = "";
                    String strTypeName = "";
                    String strLength = "";

                    StringBuilder strBAll = new StringBuilder();

                    strBAll.Append("    /// <summary>\r\n");
                    strBAll.Append("    /// 数据行DataRow填充到" + strTableName + "实体变量中返回实体对象\r\n");
                    strBAll.Append("    /// </summary>\r\n");
                    strBAll.Append("    /// <param name=dr></param>\r\n");
                    strBAll.Append("    /// <returns>" + strEntityName + "</returns>\r\n");
                    strBAll.Append("    public static " + strEntityName + " " + strMethodName + "(DataRow dr)\r\n");
                    strBAll.Append("    {\r\n");
                    strBAll.Append("        " + strEntityName + " entity = new " + strEntityName + "();\r\n");
                    strBAll.Append("        if (dr != null)\r\n");
                    strBAll.Append("        {\r\n");


                    while (dr.Read())
                    {
                        strColName = dr.GetValue(1).ToString().ToUpper();
                        strTypeName = dr.GetValue(2).ToString();
                        strLength = dr.GetValue(3).ToString().ToUpper();
                        if (strTypeName.Equals("int"))
                        {
                            strBAll.Append("            entity." + strColName + " = dr[\"" + strColName + "\"] == DBNull.Value ? 0 : int.Parse(dr[\"" + strColName + "\"].ToString());\r\n");
                        }
                        else if ((strTypeName.Equals("datetime"))||(strTypeName.Equals("date")))
                        {
                            strBAll.Append("            entity." + strColName + " = dr[\"" + strColName + "\"] == DBNull.Value ? NULL : DateTime.Parse(dr[\"" + strColName + "\"].ToString());\r\n");
                        }
                        else if (strTypeName.Equals("money") || strTypeName.Equals("smallmoney") || strTypeName.Equals("numeric") || strTypeName.Equals("decimal"))
                        {
                            strBAll.Append("            entity." + strColName + " = dr[\"" + strColName + "\"] == DBNull.Value ? 0 : Decimal.Parse(dr[\"" + strColName + "\"].ToString());\r\n");
                        }
                        else
                        {
                            strBAll.Append("            entity." + strColName + " = dr[\"" + strColName + "\"].ToString();\r\n");
                        }

                        iCount++;
                    }
                    conn.Close();

                    strBAll.Append("        }\r\n");
                    strBAll.Append("        return entity;\r\n");
                    strBAll.Append("    }\r\n");

                    this.txtResult.Text = strBAll.ToString();
                }
            }
        }

    }

    /// <summary>
    /// 将sql数据类型转换成C#变量类型
    /// </summary>
    /// <param name="dataType"></param>
    /// <returns></returns>
    private string DataTypeToCType(string dataType)
    {
        string retType = "";
        if (dataType.Equals("text") || dataType.Equals("varchar") || dataType.Equals("char") || dataType.Equals("nvarchar") || dataType.Equals("nchar"))
            return "string";
        if (dataType.Equals("int"))
            return "int?";
        if (dataType.Equals("smallint"))
            return "Int16?";
        if (dataType.Equals("tinyint"))
            return "byte";
        if (dataType.Equals("bigint"))
            return "long?";
        if (dataType.Equals("bit"))
            return "bool?";
        if (dataType.Equals("money") || dataType.Equals("smallmoney") || dataType.Equals("numeric") || dataType.Equals("decimal"))
            return "Decimal?";
        if (dataType.Equals("datetime") || dataType.Equals("smalldatetime") || dataType.Equals("timestamp"))
            return "DateTime?";
        if (dataType.Equals("real"))
            return "Single?";
        if (dataType.Equals("float"))
            return "double?";
        if (dataType.Equals("image") || dataType.Equals("binary") || dataType.Equals("varbinary"))
            return "byte[]?";
        if (dataType.Equals("uniqueidentifier"))
            return "Guid";

        return retType;
    }

    /// <summary>
    /// 根据sql数据类型输出字符串转成其他类型的字符串
    /// </summary>
    /// <param name="dataType"></param>
    /// <param name="strValue"></param>
    /// <returns></returns>
    private object GetConvertString(string dataType,String strValue)
    {
        object obj = new object();
        //if (dataType.Equals("text") || dataType.Equals("varchar") || dataType.Equals("char") || dataType.Equals("nvarchar") || dataType.Equals("nchar"))
        //    return strValue;
        //if (dataType.Equals("int"))
        //    return int.Parse(strValue);
        //if (dataType.Equals("smallint"))
        //    return int.Parse(strValue); 
        //if (dataType.Equals("tinyint"))
        //    return byte.Parse(strValue);
        //if (dataType.Equals("bigint"))
        //    return long.Parse(strValue);
        //if (dataType.Equals("bit"))
        //    return bool.Parse(strValue);
        //if (dataType.Equals("money") || dataType.Equals("smallmoney") || dataType.Equals("numeric") || dataType.Equals("decimal"))
        //    return Decimal.Parse(strValue);
        //if (dataType.Equals("datetime") || dataType.Equals("smalldatetime") || dataType.Equals("timestamp"))
        //    return DateTime.Parse(strValue);
        //if (dataType.Equals("real"))
        //    return Single.Parse(strValue);
        //if (dataType.Equals("float"))
        //    return double.Parse(strValue);
        //if (dataType.Equals("image") || dataType.Equals("binary") || dataType.Equals("varbinary"))
        //    return byte.Parse(strValue);
        //if (dataType.Equals("uniqueidentifier"))
        //    return strValue;

        return obj;
    }

    /// <summary>
    /// 输出到文件
    /// </summary>
    /// <param name="str"></param>
    private void WriteFile(string str,String strClassName)
    {
        StreamWriter sr;
        if (!Directory.Exists(this.txtFilePath.Text))
        {
            Directory.CreateDirectory(this.txtFilePath.Text);
        }

        String strFileName = this.txtFilePath.Text + "\\" + "" + strClassName + ".cs";
        if (File.Exists(strFileName)) //如果文件存在,则创建File.AppendText对象
        {
            File.Delete(strFileName);
            sr = File.CreateText(strFileName);
        }
        else   //如果文件不存在,则创建File.CreateText对象
        {
            sr = File.CreateText(strFileName);
        }
        sr.WriteLine(str);
        sr.Flush();
        sr.Close();
    }


    protected void Button2_Click(object sender, EventArgs e)
    {
        Page.Response.Redirect("buildSqlConfig.aspx");
    }

}
