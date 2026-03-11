using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Com.ValuePlus.Web;
using System.Data;
using System.Data.OleDb;
using Com.ValuePlus.DAL;
using System.Data.SqlClient;
using Com.ValuePlus.Common.Config;
using System.Collections;
using System.IO;
using Com.ValuePlus.Common.Security;

public partial class AppFunction_BTW_ImportOrder : PageBase
{
    protected String strOrderFilePath = "../../UserFile/ArchiveAtt/AttachFile/BATCHORDER-1";//文件相对路径
    protected String strDbConnection = BaseConfig.Instance.GetConnectionString();
    protected String strTableName = "ExcelOrders";//excel临时表名

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            this.DoHtmlAction();
        }
    }

    #region viewstate初始化区域
    private string strBCode
    {
        get
        {
            return ViewState["strBCode_ViewState"] as string;
        }
        set
        {
            ViewState["strBCode_ViewState"] = value;
        }
    }
    private string strFilePathAndName
    {
        get
        {
            return ViewState["strFilePathAndName_ViewState"] as string;
        }
        set
        {
            ViewState["strFilePathAndName_ViewState"] = value;
        }
    }
    private string strDtFlag
    {
        get
        {
            return ViewState["strDtFlag_ViewState"] as string;
        }
        set
        {
            ViewState["strDtFlag_ViewState"] = value;
        }
    }
    #endregion

    /// <summary>
    /// html页面预处理
    /// </summary>
    private void DoHtmlAction()
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        sb.Append("<script language=\"javascript\">\r\n");
        sb.Append("    if(confirm('Are you sure to do it ？')){\r\n");
        sb.Append("        javascript:__doPostBack('btnDoSubmit','')\r\n");
        sb.Append("    }else{window.close();}");
        sb.Append("</script>");

        Page.ClientScript.RegisterStartupScript(typeof(Page), "addConfirm", sb.ToString());
    }

    #region 执行导入操作
    /// <summary>
    /// 执行存储过程事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void DoImport_Click(object sender, EventArgs e)
    {
        try
        {
            //出库单号
            if (Request.Params["P0"] != null)
            {
                this.strBCode = Request.Params["P0"].ToString();
                //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
                this.strBCode = SQLInjectionDefense.ReplaceSQLReservedKeyword(this.strBCode);

                //获取订单excel文件名
                this.strFilePathAndName = this.GetFilePathAndName(strBCode);
                if (!String.IsNullOrEmpty(this.strFilePathAndName))
                {
                    if (!File.Exists(this.strFilePathAndName))
                    {
                        this.AlertMessageBox(this.Page, "The Excel File may be had Deteled,please upload it again!");
                        Page.ClientScript.RegisterStartupScript(typeof(Page), "success", "<script language=javascript>window.close();</script>");
                    }
                    else
                    {
                        //记住本次操作的时间戳
                        this.strDtFlag = System.Guid.NewGuid().ToString();

                        try
                        {
                            //导入excel文档到数据库
                            this.ImportExcelOneSheetToDB(this.strFilePathAndName);
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex.Message.ToString());
                            log.Error("AppFunction_BTW_ImportOrder.ImportExcelOneSheetToDB() Error!");
                            this.AlertMessageBox(this.Page, "导入数据格式有误，请检查修正后重新导入：特别注意列Article和列HS Code需设置成数值型！"+ex.Message.ToString());
                            Page.ClientScript.RegisterStartupScript(typeof(Page), "success", "<script language=javascript>window.close();</script>");
                            return;
                        }
                        
                        try
                        {
                                        //数据库存储过程作相应后续处理
                            this.ExecImportSp();
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex.Message.ToString());
                            this.AlertMessageBox(this.Page, "导入数据失败！" + ex.Message.ToString());
                            Page.ClientScript.RegisterStartupScript(typeof(Page), "success", "<script language=javascript>window.close();</script>");
                            return;
                        }
                        //
                        Page.ClientScript.RegisterStartupScript(typeof(Page), "success", "<script language=javascript>ImportSuccessfully();</script>");
                    }
                }
                else
                {
                    this.AlertMessageBox(this.Page, "These is no Excel File,please upload it at first!");
                    Page.ClientScript.RegisterStartupScript(typeof(Page), "success", "<script language=javascript>window.close();</script>");
                }
            }
        }
        catch (Exception ex)
        {
            log.Error(ex.Message.ToString());
            this.AlertMessageBox(this.Page, "导入数据失败！" + ex.Message.ToString());
            Page.ClientScript.RegisterStartupScript(typeof(Page), "success", "<script language=javascript>window.close();</script>");
            return;
        }
    }

    #endregion

    /// <summary>
    /// 获取订单excel文件名
    /// </summary>
    /// <param name="strBCode"></param>
    /// <returns></returns>
    private String GetFilePathAndName(String strBCode)
    {
        String strFilePath = base.MapPath(strOrderFilePath);
        String strFilePathAndName = "";
        try
        {
            String strSql = "SELECT ATF FROM BATCHORDER_1 WHERE BCODE = '" + strBCode + "'";
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
            if ((dt != null) && (dt.Rows.Count > 0))
            {
                String strFileName = dt.Rows[0]["ATF"].ToString();
                if (!String.IsNullOrEmpty(strFileName))
                {
                    strFilePathAndName = strFilePath + "\\" + strBCode + "\\" + strFileName;
                }
            }
        }
        catch (Exception ex)
        {
            log.Error(ex.Message.ToString());
            log.Error("AppFunction_BTW_ImportOrder.GetFilePathAndName() Error!");
        }
        return strFilePathAndName;
    }

    /// <summary>
    /// 数据库存储过程作相应后续处理
    /// </summary>
    private void ExecImportSp()
    {
        Hashtable hsTable = new Hashtable();
        hsTable.Add("P0",this.strBCode);
        hsTable.Add("dtFlag", this.strDtFlag);
        String strReturn = SqlParamDao.ExcuteSPReturnStr("USP_IMPORDS",hsTable);
        strReturn = this.GetActionTipString(strReturn);
        this.AlertMessageBox(this.Page,strReturn);
    }

    /// <summary>
    /// 获取存储动作执行返回字符串
    /// </summary>
    /// <param name="strReturn"></param>
    /// <returns></returns>
    private String GetActionTipString(String strReturn)
    {
        try
        {
            String strSql = "SELECT * FROM TB_HRTMPAR WHERE TID = 'BATCHORDER' AND AID = 'EXPORT' AND '" + strReturn + "'>= ECFROM AND '" + strReturn + "' <=ECTO";
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
            if ((dt != null) && (dt.Rows.Count > 0))
            {
                String strMsg = dt.Rows[0]["MESSENG"].ToString();
                if (this.Language.Equals("zh-cn"))
                {
                    strMsg = dt.Rows[0]["MESSCHS"].ToString();
                }
                strReturn = strMsg;
            }
        }
        catch (Exception ex)
        {
            log.Error(ex.Message.ToString());
            log.Error("AppFunction_BTW_ImportOrder.GetActionTipString() Error!");
        }
        return strReturn;
    }

    /// <summary>
    /// 将 Excel 文件转成 DataTable
    /// </summary>
    /// <param name="serverMapPathExcel">Excel文件及其路径</param>
    /// <param name="strSheetName">工作表名,如:Sheet1</param>
    /// <param name="isTitleOrDataOfFirstRow">True 第一行是标题,False 第一行是数据</param>
    /// <returns>DataTable</returns>
    private DataTable ExcelToDataTable(string serverMapPathExcel, string strSheetName, bool isTitleOrDataOfFirstRow)
    {

        string HDR = string.Empty;//如果第一行是数据而不是标题的话, 应该写: "HDR=No;"
        if (isTitleOrDataOfFirstRow)
        {
            HDR = "YES";//第一行是标题
        }
        else
        {
            HDR = "NO";//第一行是数据
        }
        //源的定义 
        string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + serverMapPathExcel + ";" + "Extended Properties='Excel 8.0;HDR=" + HDR + ";IMEX=1';";

        //Sql语句
        //string strExcel = string.Format("select * from [{0}$]", strSheetName); 这是一种方法
        string strExcel = "select * from   [" + strSheetName + "]";
        //定义存放的数据表
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        //连接数据源
        using (OleDbConnection conn = new OleDbConnection(strConn))
        {
            try
            {
                conn.Open();
                //适配到数据源
                OleDbDataAdapter adapter = new OleDbDataAdapter(strExcel, strConn);
                
                adapter.Fill(ds, this.strTableName);

                dt = ds.Tables[this.strTableName];
                //增加时间戳列并赋值
                DataColumn totalColumn = new DataColumn();
                totalColumn.DataType = Type.GetType("System.String") ;
                totalColumn.ColumnName = "FlagDt";
                dt.Columns.Add(totalColumn);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["FlagDt"] = this.strDtFlag;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message.ToString());
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
        return dt;
    }

    /// <summary>
    /// 读取excel文件到数据库（1个sheet）
    /// </summary>
    /// <param name="strFile"></param>
    private void ImportExcelOneSheetToDB(String strFile)
    {
        DataTable excelDataTable = this.ExcelToDataTable(strFile, "sheet1$", true);
        if (excelDataTable.Columns.Count > 1)
        {
            SqlBulkCopy sqlbulkcopy = new SqlBulkCopy(strDbConnection, SqlBulkCopyOptions.UseInternalTransaction);
            sqlbulkcopy.DestinationTableName = this.strTableName;//数据库中的表名

            sqlbulkcopy.WriteToServer(excelDataTable);
            sqlbulkcopy.Close();
        }
        

    }

    /// <summary>
    /// 读取excel文件到数据库（多个sheet）
    /// </summary>
    /// <param name="strFile"></param>
    private void ImportExcelToDB(String strFile)
    {
        OleDbConnection objConn = new System.Data.OleDb.OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strFile + ";" + "Extended Properties=Excel 8.0;");
        objConn.Open();
        try
        {
            DataTable schemaTable = objConn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, null);
            string sheetName = string.Empty;
            for (int j = 0; j < schemaTable.Rows.Count; j++)
            {
                sheetName = schemaTable.Rows[j][2].ToString().Trim();//获取 Excel 的表名，默认值是sheet1 
                DataTable excelDataTable = this.ExcelToDataTable(strFile, sheetName, true);
                if (excelDataTable.Columns.Count > 1)
                {
                    SqlBulkCopy sqlbulkcopy = new SqlBulkCopy(strDbConnection, SqlBulkCopyOptions.UseInternalTransaction);
                    sqlbulkcopy.DestinationTableName = this.strTableName;//数据库中的表名

                    sqlbulkcopy.WriteToServer(excelDataTable);
                    sqlbulkcopy.Close();
                }
            }
        }
        catch (Exception ex)
        {
            log.Error(ex.Message.ToString());
            log.Error("AppFunction_BTW_ImportOrder.ImportExcelToDB() Error!");
        }
        finally
        {
            objConn.Close();
            objConn.Dispose();
        }


    }


}
