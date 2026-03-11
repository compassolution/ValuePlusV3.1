using System;
using System.Collections;
using System.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Com.ValuePlus.Web;
using System.Resources;
using System.Drawing;
using System.Reflection;
using Com.ValuePlus.BLL.Export;
using System.Web;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using NPOI.HSSF.UserModel;
using Com.ValuePlus.BLL.NPOI;

using System.Text;
using System.IO;
using NPOI.HSSF.UserModel;
using System.Data;
using NPOI.SS.UserModel;
using System.Data.SqlClient;
using Com.ValuePlus.DAL;
using System.Web;
using NPOI.HSSF.Util;
using System.Collections;
using NPOI.HPSF;
using NPOI.SS.Util;
using Com.ValuePlus.Common.Security;

public partial class Export_exportindex : PageBase
{

    System.Web.UI.WebControls.DataGrid DataGrid1 = new DataGrid();

    private ArrayList arr = new ArrayList();
    private ArrayList arrlistptype = new ArrayList();
    private String strWelcomeSpeech;
    private String strExcelTitleName;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            String strType = Request.Params["type"]==null?"":Request.Params["type"].ToString();
            //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
            strType = SQLInjectionDefense.ReplaceSQLReservedKeyword(strType);
            DataSet ds = new DataSet();

            if (Session["ExportDsViewDataViewState"] != null)
            {
                ds = Session["ExportDsViewDataViewState"] as DataSet;
            }
            else if (Session["ExportDsViewDataViewState_Sql"] != null)
            {
                //点击导出以后才获取数据，优化了列表页面的加载性能 modify by sammen 20151112
                String strSqlString = Session["ExportDsViewDataViewState_Sql"] as String;
                ds = SqlParamDao.GetDataSetBySql(strSqlString);
            }

            strExcelTitleName = Session["ExportDsViewDataViewState_Title"] as String;
            strWelcomeSpeech = Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("WelcomeSpeech_Chs");


            //ExportOptionBll.doExportToExcel(ds, "");
            //Session["ExportDsViewDataViewState"] = null;
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null)
            {
                ds.Tables[0].TableName = Session.SessionID;
                if (strType.Equals("excel"))
                {
                    exportToExcel(ds.Tables[0]);
                }
                else if (strType.Equals("nopiExcel"))
                {
                    DataTable dtNeedExport = ds.Tables[0];
                    /// 特殊化转化DataTable add by sammen 20191226
                    //【主要为了解决：1模板列表导出时列名导出名称；2隐藏的字段不显示】
                    String strNeedExport_TID = Session["ExportDsViewDataViewState_TID"] == null ? "" : Session["ExportDsViewDataViewState_TID"].ToString();
                    String strNeedExport_SID = Session["ExportDsViewDataViewState_SID"] == null ? "" : Session["ExportDsViewDataViewState_SID"].ToString();
                    String strNeedExport_GID = Session["ExportDsViewDataViewState_GID"] == null ? "" : Session["ExportDsViewDataViewState_GID"].ToString();
                    if (!String.IsNullOrEmpty(strNeedExport_GID)
                        && !String.IsNullOrEmpty(strNeedExport_TID)
                        && !String.IsNullOrEmpty(strNeedExport_SID))
                    {
                        DataTable dt = SpecialConvertDataTable(dtNeedExport, strNeedExport_TID, strNeedExport_SID, strNeedExport_GID);
                    }

                    this.exportToExcelByNOPI(dtNeedExport);
                }
                else if (strType.Equals("pdf"))
                {
                    this.exportToPdf(ds.Tables[0]);
                }
                else if (strType.Equals("txt"))
                {
                    this.exportToTxt(ds.Tables[0]);
                }
            }
        }
    }

    /// <summary>
    /// 特殊化转化DataTable add by sammen 20191226
    //【主要为了解决：1模板列表导出时列名导出名称；2隐藏的字段不显示】
    /// </summary>
    /// <param name="dtOld"></param>
    /// <param name="strTID"></param>
    /// <param name="strSID"></param>
    /// <param name="strGID"></param>
    /// <returns></returns>
    private DataTable SpecialConvertDataTable(DataTable dtOld,String strTID,String strSID,String strGID)
    {
        DataTable dtNew = dtOld;
        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select * from TB_HRTMPSD WHERE TID = '" + strTID + "' and SID = '" + strSID + "' and GID = '" + strGID + "'");
            sbSql.Append("  AND PTYPE not in ('CH','CS','detail') AND PRIGHT IN ('0','1')");
            sbSql.Append(" order BY PORDER");
            String strSql = sbSql.ToString();
            DataTable dtTemp = SqlParamDao.GetDataTableBySql(strSql);
            if (dtTemp != null & dtTemp.Rows.Count > 0)
            {
                for (int i = 0; i < dtTemp.Rows.Count; i++)
                {
                    DataRow dr = dtTemp.Rows[i];
                    string strPID = dr["PID"].ToString();
                    string strPDESC = dr["PDESCCHS"].ToString();
                    string strPLIST = dr["PLIST"].ToString();
                    string strPTYPE = dr["PTYPE"].ToString();

                    if (dtNew.Columns.Contains(strPID))
                    {
                        if (strPLIST.Equals("0"))
                        {
                            //如果列隐藏，则去除列
                            dtNew.Columns.Remove(strPID);
                        }
                        else
                        {
                            //如果不隐藏，则替换列名
                            dtNew.Columns[strPID].ColumnName = strPDESC;
                        }
                    }

                }
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
        }

        return dtNew;
    }


    #region 导出到excel文件
    
    /// <summary>
    /// 利用NOPI控件导出到excel文件
    /// </summary>
    /// <param name="myTable"></param>
    private void exportToExcelByNOPI(DataTable myTable)
    {
        String strFileName = strExcelTitleName;
        String strSheetName = strExcelTitleName;
        ExcelHelper excelHelper = new ExcelHelper();
        MemoryStream ms = excelHelper.ExportToExcel_ReturnMS(myTable, strFileName, strSheetName, strWelcomeSpeech, this.GetUserCode());
        //MemoryStream ms = this.ExportToExcel_ReturnMS(myTable, strFileName, strSheetName, strWelcomeSpeech, this.GetUserCode());
        ExcelHelper.RenderMSToBrowser(ms, System.Web.HttpContext.Current, strFileName);

        ms.Close();
        ms = null;
    }

    /// <summary>
    /// 导出到excel文件
    /// </summary>
    /// <param name="myTable"></param>
    private void exportToExcel(DataTable myTable)
    {
        this.DataGrid1.Columns.Clear();
        this.DataGrid1.AutoGenerateColumns = true;
        this.DataGrid1.DataSource = myTable.DefaultView;
        DataGrid1.ItemDataBound += new DataGridItemEventHandler(DataGrid1_ItemDataBound);
        this.DataGrid1.AllowSorting = true;
        try
        {
            this.DataGrid1.DataBind();
        }
        catch (Exception exception2)
        {
            log.Error(exception2);
        }
        base.Response.Clear();
        HttpContext.Current.Response.Write("<meta   http-equiv=Content-Type   content=text/html;charset=GB2312>");
        base.Response.ContentType = "application/vnd.ms-excel";
        Response.AddHeader("Content-Disposition", "attachment;filename=" + Session.SessionID.ToString() + ".xls");
        Encoding encoding = Encoding.GetEncoding("GB2312");
        HttpContext.Current.Response.ContentEncoding = encoding;
        HttpContext.Current.Response.HeaderEncoding = encoding;
        //base.Response.Charset = "GB2312";
        this.EnableViewState = false;
        StringWriter writer = new StringWriter();
        HtmlTextWriter writer2 = new HtmlTextWriter(writer);
        this.DataGrid1.RenderControl(writer2);
        string s = writer.ToString();
        base.Response.Write(s);
        base.Response.Flush();
        base.Response.Close();

    }
    #endregion

    protected void DataGrid1_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        Regex Regex0Number = new Regex(@"^0\d{1,}$", RegexOptions.IgnoreCase);
        Regex RegexdNumber = new Regex(@"^\d{11,}$", RegexOptions.IgnoreCase);
        Regex Regex_Number = new Regex(@"^-\d{11,}.\d{1,}$", RegexOptions.IgnoreCase);

        if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
        {
            DataRowView dataItem = (DataRowView)e.Item.DataItem;
            for (int i = 0; i < e.Item.Cells.Count; i++)
            {   
                string numberformat = e.Item.Cells[i].Text.Trim();
                if ( !string.IsNullOrEmpty(numberformat) && this.isint(numberformat) )
                {
                    if (Regex0Number.IsMatch(numberformat) || RegexdNumber.IsMatch(numberformat) || Regex_Number.IsMatch(numberformat))
                    {
                        e.Item.Cells[i].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
                    }                                 
                }
                if (dataItem.Row.Table.Columns[i].DataType == typeof(DateTime))
                {
                    e.Item.Cells[i].Text = this.FormatData(e.Item.Cells[i].Text, @"yyyy\-MM\-dd HH:mm:ss");
                }
                
            }
        }
    }
    
    #region 导出到txt文件
    /// <summary>
    /// 导出到txt文件
    /// </summary>
    /// <param name="myTable"></param>
    protected void exportToTxt(DataTable myTable)
    {
        try
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            if (myTable.Rows.Count > 0)
            {
                int i = 0;
                for (int n = 0; n < myTable.Rows.Count; n++)
                {
                    for (int m = 0; m < myTable.Columns.Count; m++)
                    {
                        i++;
                        sb.Append(myTable.Rows[n][m].ToString());
                    }
                    sb.Append("\r\n");
                }
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Buffer = true;
                HttpContext.Current.Response.Charset = "GB2312";
                HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + Session.SessionID.ToString() + ".txt");
                HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");//设置输出流为简体中文
                Response.ContentType = "text/plain";//设置输出文件类型为txt文件。 
                this.EnableViewState = false;
                System.Globalization.CultureInfo myCItrad = new System.Globalization.CultureInfo("ZH-CN", true);
                System.IO.StringWriter oStringWriter = new System.IO.StringWriter(myCItrad);
                HttpContext.Current.Response.Write(sb.ToString());
                HttpContext.Current.Response.End();
                HttpContext.Current.Response.Close();

            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
    }
    #endregion

    #region 页面导出到PDF文件
    /// <summary>
    /// 页面导出到PDF文件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void exportToPdf(DataTable dt)
    {
        try
        {
            //在服务器端保存PDF时的文件名
            string strFileName = "导出PDF测试.pdf";
            //初始化一个目标文档类 
            Document document = new Document(PageSize.A4.Rotate(), 0, 0, 10, 10);
            //调用PDF的写入方法流
            //注意FileMode-Create表示如果目标文件不存在，则创建，如果已存在，则覆盖。
            PdfWriter writer = PdfWriter.GetInstance(document,
                new FileStream(HttpContext.Current.Server.MapPath(strFileName), FileMode.Create));


            try
            {
                ////标题字体
                //BaseFont basefont_Title = BaseFont.CreateFont(
                //  fontpath_Title,
                //  BaseFont.IDENTITY_H,
                //  BaseFont.NOT_EMBEDDED);
                //Font font_Title = new Font(basefont_Title, fontsize_Title, fontStyle_Title, fontColor_Title);
                ////表格列字体
                //BaseFont basefont_Col = BaseFont.CreateFont(
                //  fontpath_Col,
                //  BaseFont.IDENTITY_H,
                //  BaseFont.NOT_EMBEDDED);
                //Font font_Col = new Font(basefont_Col, fontsize_Col, fontStyle_Col, fontColor_Col);
                ////正文字体
                //BaseFont basefont_Context = BaseFont.CreateFont(
                //  FontPath,
                //  BaseFont.IDENTITY_H,
                //  BaseFont.NOT_EMBEDDED);
                //Font font_Context = new Font(basefont_Context, FontSize, fontStyle_Context, fontColor_Context);

                //打开目标文档对象
                document.Open();

                //添加标题

                Paragraph p_Title = new Paragraph("标题", new iTextSharp.text.Font());
                p_Title.Alignment = Element.ALIGN_CENTER;
                document.Add(p_Title);

                //根据数据表内容创建一个PDF格式的表
                PdfPTable table = new PdfPTable(dt.Columns.Count);

                //table.TotalWidth = 800f;//表格总宽度
                //table.LockedWidth = true;//锁定宽度
                //table.SetWidths(arr_Width);//设置每列宽度

                BaseFont bfChinese = BaseFont.CreateFont("C:\\WINDOWS\\Fonts\\simsun.ttc,1", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

                // 告诉程序这行是表头，这样页数大于1时程序会自动为你加上表头。
                //table.HeaderRows = 1;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        table.AddCell(new Phrase(dt.Rows[i][j].ToString(), new iTextSharp.text.Font()));
                    }

                    // 添加数据
                    //设置标题靠左居中
                    table.DefaultCell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    // 设置表体背景色
                    table.DefaultCell.BackgroundColor = iTextSharp.text.Color.WHITE;


                }

                //如果最后一个单元格数据过多，不要移动到下一页显示
                table.SplitLate = false;
                //
                table.SplitRows = false;
                //在目标文档中添加转化后的表数据
                document.Add(table);


            }

            catch (Exception)
            {
                throw;
            }
            finally
            {
                //关闭目标文件
                document.Close();
                //关闭写入流
                writer.Close();
            }


            // 弹出提示框，提示用户是否下载保存到本地
            try
            {
                //这里是你文件在项目中的位置,根目录下就这么写 
                String FullFileName = System.Web.HttpContext.Current.Server.MapPath(strFileName);
                FileInfo DownloadFile = new FileInfo(FullFileName);
                System.Web.HttpContext.Current.Response.Clear();
                System.Web.HttpContext.Current.Response.ClearHeaders();
                System.Web.HttpContext.Current.Response.Buffer = true;
                System.Web.HttpContext.Current.Response.ContentType = "application/octet-stream";
                System.Web.HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename="
                    + System.Web.HttpUtility.UrlEncode(DownloadFile.FullName, System.Text.Encoding.UTF8));
                System.Web.HttpContext.Current.Response.AppendHeader("Content-Length", DownloadFile.Length.ToString());
                System.Web.HttpContext.Current.Response.WriteFile(DownloadFile.FullName);



                //HttpContext.Current.Response.Clear();
                //HttpContext.Current.Response.Buffer = true;
                //HttpContext.Current.Response.Charset = "GB2312";
                //HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + Session.SessionID.ToString() + ".txt");
                //HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");//设置输出流为简体中文
                //Response.ContentType = "text/plain";//设置输出文件类型为txt文件。 
                //this.EnableViewState = false;
                //System.Globalization.CultureInfo myCItrad = new System.Globalization.CultureInfo("ZH-CN", true);
                //System.IO.StringWriter oStringWriter = new System.IO.StringWriter(myCItrad);
                //HttpContext.Current.Response.Write(sb.ToString());
                //HttpContext.Current.Response.End();
                //HttpContext.Current.Response.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                System.Web.HttpContext.Current.Response.Flush();
                System.Web.HttpContext.Current.Response.End();
            }


        }
        catch (DocumentException de)
        {
            Response.Write(de.ToString());
        }
    }
    #endregion


    private string inarray(string tomatch, string[] strings)
    {
        foreach (string str in strings)
        {
            string[] strArray = str.Split(new char[] { ',' });
            if (strArray[0] == tomatch)
            {
                return strArray[1];
            }
        }
        return "N/A";
    }
    private bool isint(string numberformat)
    {
        Regex regex = new Regex(@"^[+-]?\d*[.]?\d*$");
        return regex.IsMatch(numberformat);
    }
    private string FormatData(string formatdata, string formatstring)
    {
        DateTime time;
        string str = "";
        if (!(string.IsNullOrEmpty(formatdata) || !DateTime.TryParse(formatdata, out time)))
        {
            str = Convert.ToDateTime(formatdata).ToString(formatstring);
        }
        return str;
    }

    private string formatdatetime(DataRow r)
    {
        string str = "";
        if (r["PTYPE"].ToString() == "datetime")
        {
            return @"yyyy\-MM\-dd HH\:mm";
        }
        if (r["PTYPE"].ToString() == "date")
        {
            return @"yyyy\-MM\-dd";
        }
        if (r["PTYPE"].ToString() == "time")
        {
            str = @"HH\:mm";
        }
        return str;
    }
}
