using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;
using Com.ValuePlus.DAL.Query;
using System.Web;
namespace Com.ValuePlus.BLL.Export
{
    public class ExportOptionBll
    { 
        #region 将dataset数据执行导出Excel操作
        /// <summary>
        /// 将dataset数据执行导出Excel操作
        /// </summary>
        /// <param name="ds">要导出的DataSet</param>
        /// <param name="strExcelFileName">要导出的文件名</param>
        public static void doExportToExcel(DataSet ds, string strExcelFileName)
        {
            
                if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[0] != null))
                {
                    DataTable dt = ds.Tables[0];
                    StringBuilder sw = new StringBuilder();
                    string strColumn = "";
                    for (int i = 0; i < dt.Columns.Count; i++)//取字段名
                    {
                        if (!string.IsNullOrEmpty(strColumn))
                        {
                            strColumn += "," + dt.Columns[i].ColumnName;
                        }
                        else
                        {
                            strColumn = dt.Columns[i].ColumnName;
                        }
                    }
                    sw.Append(strColumn);
                    sw.Append("\n");

                    for (int i = 0; i < dt.Rows.Count; i++)//取记录值
                    {
                        string strRow = "";
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            string sTemp = dt.Rows[i][j].ToString().Replace(",", "，");
                            if (!string.IsNullOrEmpty(sTemp))
                            {
                                if (sTemp.Substring(0, 1) == "0")
                                {
                                    sTemp = sTemp + "\x09";
                                }
                                else
                                {
                                    if (sTemp.Length > 11)
                                    {
                                        sTemp = sTemp + "\x09";
                                    }
                                }
                            }

                            if (!string.IsNullOrEmpty(strRow))
                            {

                                strRow += "," + sTemp;
                            }
                            else
                            {
                                if (dt.Rows[i][j] == null || dt.Rows[i][j] == DBNull.Value || string.IsNullOrEmpty(dt.Rows[i][j].ToString()))
                                {
                                    strRow = " ";
                                }
                                else
                                {
                                    strRow += sTemp;
                                }
                            }
                        }
                        sw.Append(strRow);
                        sw.Append("\n");
                    }
                    
                    HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
                    if (string.IsNullOrEmpty(strExcelFileName))
                    {
                        if (string.IsNullOrEmpty(ds.Tables[0].TableName))
                        {
                            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode("sheet1", Encoding.GetEncoding("GB2312")) + ".csv");
                        }
                        else
                        {
                            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode(ds.Tables[0].TableName, Encoding.GetEncoding("GB2312")) + ".csv");
                        }
                    }
                    else
                    {
                        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode(strExcelFileName, Encoding.GetEncoding("GB2312")) + ".csv");
                    }
                    HttpContext.Current.Response.ContentType = "text/csv";
                    HttpContext.Current.Response.HeaderEncoding = System.Text.Encoding.GetEncoding("gb2312");
                    HttpContext.Current.Response.Write(sw.ToString()); 
                }
           
        }
        #endregion

       
    }
}
