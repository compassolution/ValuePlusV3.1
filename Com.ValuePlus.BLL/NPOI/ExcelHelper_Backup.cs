using System;
using System.Collections.Generic;
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

namespace Com.ValuePlus.BLL.NPOI
{
    /// <summary>
    /// 利用NPOI组件对Excel的操作
    /// </summary>
    public class ExcelHelper_Backup
    {
        /// <summary>
        /// 日志声明
        /// </summary>
        protected static Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 读取只有一个Sheet的Excel2003文件，并返回DataTable数据集
        /// </summary>
        /// <param name="strFileFullName"></param>
        /// <param name="isBuildKey">是否要为每天记录自动生成一条主键</param>
        /// <returns></returns>
        public static DataTable GetDataTableFromExcel2003OneSheet(String strFileFullName,bool isBuildKey)
        {
            DataTable dtData = new DataTable();
            try
            {
                using (FileStream fs = File.OpenRead(@strFileFullName))   //打开myxls.xls文件
                {
                    HSSFWorkbook hssfworkbook = new HSSFWorkbook(fs);   //把xls文件中的数据写入wk中
                    if (hssfworkbook.NumberOfSheets > 0)
                    {

                        ISheet sheet = (HSSFSheet)hssfworkbook.GetSheetAt(0);   //读取第一个Sheet表数据

                        #region 获取sheet的首行并创建表结构
                        //增加主键列
                        if (isBuildKey)
                        {
                            DataColumn column = new DataColumn("sKey");
                            column.MaxLength = 50;//默认数据库字符串字段长度为50
                            column.Unique = true;
                            dtData.Columns.Add(column);
                        }
                        HSSFRow headerRow = (HSSFRow)sheet.GetRow(0);
                        //一行最后一个方格的编号 即总的列数
                        int cellCount = headerRow.LastCellNum;
                        for (int i = headerRow.FirstCellNum; i < cellCount; i++)
                        {
                            if ((headerRow.GetCell(i) != null) && (!String.IsNullOrEmpty(headerRow.GetCell(i).StringCellValue)))
                            {
                                DataColumn column = new DataColumn(headerRow.GetCell(i).StringCellValue);
                                #region 获取Excel单元格设置数据类型(暂时没办法精确)
                                //switch (headerRow.GetCell(i).CellStyle.DataFormat)
                                //{
                                //    case 5:
                                //    case 6:
                                //    case 7:
                                //    case 8:
                                //    case 23:
                                //    case 24:
                                //    case 25:
                                //    case 26:
                                //    case 41:
                                //    case 42:
                                //    case 43:
                                //    case 44:
                                //    case 176:
                                //    case 178:
                                //    case 179:
                                //    case 180:
                                //    case 181:
                                //    case 182:
                                //    case 183:
                                //    case 184:
                                //    case 185://数值型
                                //        dtData.Columns.Add(headerRow.GetCell(i).StringCellValue, typeof(Decimal));
                                //        break;
                                //    case 177:
                                //    case 186:
                                //    case 187:
                                //    case 188:
                                //    case 189:
                                //    case 190://日期时间型
                                //        dtData.Columns.Add(headerRow.GetCell(i).StringCellValue, typeof(DateTime));
                                //        break;
                                //    default://其他都为字符串型
                                //        dtData.Columns.Add(headerRow.GetCell(i).StringCellValue, typeof(String));
                                //        break;


                                //}
                                #endregion 获取Excel单元格设置数据类型(暂时没办法精确)
                                column.MaxLength = 50;//默认数据库字符串字段长度为50
                                dtData.Columns.Add(column);

                            }
                        }
                        #endregion 获取sheet的首行并创建表结构

                        #region 从第二行开始读取Excel表数据
                        for (int j = 1; j <= sheet.LastRowNum; j++)  //LastRowNum 是当前表的总行数/
                        {
                            DataRow dr = dtData.NewRow();
                            IRow row = (HSSFRow)sheet.GetRow(j);  //读取当前行数据
                            int iStart = 0;
                            if (isBuildKey)//如果存在唯一标识列
                            {
                                iStart = 1;
                                dr[0] = j.ToString();
                            }
                            if (row != null)
                            {
                                for (int i = iStart; i < dtData.Columns.Count; i++)
                                {
                                    if (row.GetCell(i - iStart) != null)
                                    {
                                        DataColumn column = dtData.Columns[i];
                                        byte[] strlength = null;
                                        switch (row.GetCell(i - iStart).CellType)
                                        {
                                            case CellType.BLANK: //空数据类型处理
                                                dr[i] = "";
                                                break;
                                            case CellType.STRING: //字符串类型
                                                dr[i] = row.GetCell(i - iStart).StringCellValue.Trim();
                                                //动态变更字符串类型字段的数据长度
                                                strlength = System.Text.Encoding.Default.GetBytes(row.GetCell(i - iStart).StringCellValue.Trim());
                                                column.MaxLength = column.MaxLength > strlength.Length ? column.MaxLength : strlength.Length;
                                                break;
                                            case CellType.NUMERIC: //数字类型
                                                if (HSSFDateUtil.IsCellDateFormatted(row.GetCell(i - iStart)))
                                                {
                                                    dr[i] = row.GetCell(i - iStart).DateCellValue.ToString("yyyy-MM-dd HH:mm:ss");
                                                }
                                                else
                                                {
                                                    dr[i] = row.GetCell(i - iStart).NumericCellValue.ToString("0.0000").Trim();
                                                }
                                                break;
                                            case CellType.FORMULA://公式型
                                                HSSFFormulaEvaluator e = new HSSFFormulaEvaluator(hssfworkbook);
                                                if (row.GetCell(i - iStart).CachedFormulaResultType == CellType.NUMERIC)
                                                {
                                                    dr[i] = e.Evaluate(row.GetCell(i - iStart)).NumberValue.ToString("0.0000").Trim();
                                                }
                                                else
                                                {
                                                    dr[i] = e.Evaluate(row.GetCell(i - iStart)).StringValue.Trim();
                                                }
                                                break;
                                            default:
                                                dr[i] = row.GetCell(i - iStart).ToString().Trim();
                                                //动态变更字符串类型字段的数据长度
                                                strlength = System.Text.Encoding.Default.GetBytes(row.GetCell(i - iStart).ToString().Trim());
                                                column.MaxLength = column.MaxLength > strlength.Length ? column.MaxLength : strlength.Length;
                                                break;

                                        }

                                    }
                                    else
                                    {
                                        dr[i] = null;
                                    }
                                }
                                dtData.Rows.Add(dr);
                            }
                        }
                        #endregion 从第二行开始读取Excel表数据

                        sheet = null;
                    }
                    hssfworkbook = null;
                }
            }

            catch (Exception err)
            {
                log.Error("读取只有一个Sheet的Excel2003文件失败：" + err.ToString());
            }

            return dtData;
        }


        /// <summary>
        /// 读取只有一个Sheet的Excel2003文件，并将数据集写入到指定数据表中，返回记录数
        /// </summary>
        /// <param name="strFileFullName"></param>
        /// <param name="strDBTableName"></param>
        /// <param name="isBuildKey">是否要为每天记录自动生成一条主键</param>
        /// <returns></returns>
        public static int InsertDBFromExcel2003OneSheet(String strFileFullName, String strDBTableName, bool isBuildKey)
        {
            int iRowCount = -1;
            DataTable dt = GetDataTableFromExcel2003OneSheet(strFileFullName, isBuildKey);
            iRowCount = SqlParamDao.InsertDBFromDataTable(dt, strDBTableName);

            return iRowCount;
        }

        /// <summary>
        /// 利用NOPI控件导出数据集到excel文件，返回数据流
        /// </summary>
        /// <param name="myTable"></param>
        public MemoryStream ExportToExcel_ReturnMS(DataTable myTable, String strFileName, String strSheetName, String strSystemName, String strUserCode)
        {
            Hashtable hsColumnMaxLength = new Hashtable();
            MemoryStream ms = new MemoryStream();

            HSSFWorkbook workbook = new HSSFWorkbook();
            HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet(strSheetName);
            IDataFormat format = workbook.CreateDataFormat();
            SummaryInformation si = PropertySetFactory.CreateSummaryInformation();

            si.Author = strUserCode; //填加xls文件作者信息
            //si.ApplicationName = "ValuePlus";//填加xls文件创建程序信息     
            //si.LastAuthor = "ValuePlus系统";//填加xls文件最后保存者信息     
            si.Comments = strSystemName;//填加xls文件作者信息     
            si.Title = strFileName; //填加xls文件标题信息     
            si.Subject = strFileName;//填加文件主题信息     
            si.CreateDateTime = DateTime.Now;
            workbook.SummaryInformation = si;

            try
            {
                #region 设置表格样式
                HSSFCellStyle cellStyle_TitlenHead = (HSSFCellStyle)workbook.CreateCellStyle();//表头栏样式
                HSSFCellStyle cellStyle_ColumnHead = (HSSFCellStyle)workbook.CreateCellStyle();//列头栏样式
                HSSFCellStyle cellStyle_Value1 = (HSSFCellStyle)workbook.CreateCellStyle();//奇数行样式
                HSSFCellStyle cellStyle_Value2 = (HSSFCellStyle)workbook.CreateCellStyle();//偶数行样式
                cellStyle_ColumnHead.DataFormat = format.GetFormat("General");
                cellStyle_Value1.DataFormat = format.GetFormat("General");
                cellStyle_Value2.DataFormat = format.GetFormat("General");

                //设置样式：水平对齐居左
                cellStyle_TitlenHead.Alignment = HorizontalAlignment.CENTER;
                cellStyle_ColumnHead.Alignment = HorizontalAlignment.LEFT;
                cellStyle_Value1.Alignment = HorizontalAlignment.LEFT;
                cellStyle_Value2.Alignment = HorizontalAlignment.LEFT;
                //设置边框颜色
                cellStyle_TitlenHead.TopBorderColor = HSSFColor.OLIVE_GREEN.BLUE.index;
                cellStyle_ColumnHead.BottomBorderColor = HSSFColor.OLIVE_GREEN.BLUE.index;
                cellStyle_Value1.BottomBorderColor = HSSFColor.ROSE.BLUE.index;
                cellStyle_Value2.BottomBorderColor = HSSFColor.SEA_GREEN.BLUE.index;

                ////设置背景颜色
                //cellStyle_TitlenHead.FillPattern = FillPatternType.SOLID_FOREGROUND;
                //cellStyle_ColumnHead.FillPattern = FillPatternType.SOLID_FOREGROUND;
                //cellStyle_Value1.FillPattern = FillPatternType.SOLID_FOREGROUND;
                //cellStyle_Value2.FillPattern = FillPatternType.SOLID_FOREGROUND;
                //cellStyle_TitlenHead.FillForegroundColor = HSSFColor.WHITE.index;
                //cellStyle_ColumnHead.FillForegroundColor = HSSFColor.PALE_BLUE.index;
                //cellStyle_Value1.FillForegroundColor = HSSFColor.LIGHT_CORNFLOWER_BLUE.index;
                //cellStyle_Value2.FillForegroundColor = HSSFColor.LIGHT_GREEN.index;

                //字体样式对象
                IFont font_TitleHead = workbook.CreateFont();//表头字体
                IFont font_ColumnHead = workbook.CreateFont();//列头字体
                IFont font_Data = workbook.CreateFont();//数据字体

                font_TitleHead.FontHeightInPoints = 26;
                font_TitleHead.Boldweight = 700;

                font_ColumnHead.Boldweight = short.MaxValue;
                font_ColumnHead.FontName = "微软雅黑";

                font_Data.Boldweight = short.MinValue;
                font_Data.FontName = "微软雅黑";

                //使用SetFont方法将字体样式添加到单元格样式中 
                cellStyle_TitlenHead.SetFont(font_TitleHead);
                cellStyle_ColumnHead.SetFont(font_ColumnHead);
                cellStyle_Value1.SetFont(font_Data);
                cellStyle_Value2.SetFont(font_Data);
                #endregion 设置表格样式

                #region 表头及样式
                {
                    HSSFRow headerRow = (HSSFRow)sheet.CreateRow(0);
                    headerRow.HeightInPoints = 35;
                    headerRow.CreateCell(0).SetCellValue(strFileName);

                    headerRow.GetCell(0).CellStyle = cellStyle_TitlenHead;
                    //合并表头单元格
                    sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, myTable.Columns.Count - 1));
                }
                #endregion

                #region 创建列头
                HSSFRow columnRow = (HSSFRow)sheet.CreateRow(1);
                // handling header.    
                foreach (DataColumn column in myTable.Columns)
                {
                    HSSFCell cell = (HSSFCell)columnRow.CreateCell(column.Ordinal);
                    cell.CellStyle = cellStyle_ColumnHead;
                    cell.SetCellValue(column.Caption);

                    //设置该列的最大字符数
                    if (!hsColumnMaxLength.ContainsKey(column.Ordinal.ToString()))
                    {
                        hsColumnMaxLength.Add(column.Ordinal.ToString(), GetStringLength(column.Caption));
                    }
                    else
                    {
                        if ((int)hsColumnMaxLength[column.Ordinal.ToString()] < GetStringLength(column.Caption))
                        {
                            hsColumnMaxLength.Remove(column.Ordinal.ToString());
                            hsColumnMaxLength.Add(column.Ordinal.ToString(), GetStringLength(column.Caption));
                        }
                    }

                }
                #endregion

                #region 插入表数据
                // handling value.  
                int rowIndex = 2;//由于第一行是表头标题
                foreach (DataRow row in myTable.Rows)
                {
                    HSSFRow dataRow = (HSSFRow)sheet.CreateRow(rowIndex);
                    foreach (DataColumn column in myTable.Columns)
                    {
                        HSSFCell cell = (HSSFCell)dataRow.CreateCell(column.Ordinal);
                        HSSFCellStyle cellStyle_Value1_Use = cellStyle_Value1;
                        HSSFCellStyle cellStyle_Value2_Use = cellStyle_Value2;
                        cellStyle_Value1_Use.DataFormat = format.GetFormat("General");
                        cellStyle_Value2_Use.DataFormat = format.GetFormat("General");

                        String strColumnValue = row[column].ToString();
                        switch (column.DataType.ToString())
                        {
                            case "System.DateTime"://日期类型
                                if (!String.IsNullOrEmpty(strColumnValue))
                                {
                                    DateTime dtTemp = DateTime.Parse(strColumnValue);
                                    String strTempValue = dtTemp.ToString("yyyy-MM-dd HH:mm:ss");//modify by sammen 20190626
                                    cell.SetCellValue(strTempValue);
                                    //strColumnValue = dtTemp.ToString("yyyy-MM-dd HH:mm:ss");
                                }
                                else
                                {
                                    strColumnValue = "";
                                    cell.SetCellValue(strColumnValue);
                                }
                                cellStyle_Value1_Use.DataFormat = format.GetFormat("yyyy-MM-dd HH:mm:ss");
                                cellStyle_Value2_Use.DataFormat = format.GetFormat("yyyy-MM-dd HH:mm:ss"); 
                                break;
                            case "System.Boolean"://布尔型      
                                bool boolV = false;
                                bool.TryParse(strColumnValue, out boolV);
                                cell.SetCellValue(boolV);
                                //strColumnValue = boolV.ToString();
                                cell.SetCellType(CellType.BOOLEAN);
                                break;
                            case "System.Int16"://整型      
                            case "System.Int32":
                            case "System.Int64":
                            case "System.Byte":
                                int intV = 0;
                                int.TryParse(strColumnValue, out intV);
                                cell.SetCellValue(intV);
                                //strColumnValue = intV.ToString();
                                cell.SetCellType(CellType.NUMERIC);
                                break;
                            case "System.Double"://浮点型 
                                double doubV = 0.0000;
                                double.TryParse(strColumnValue, out doubV);
                                cell.SetCellValue(doubV);
                                //strColumnValue = doubV.ToString();
                                cell.SetCellType(CellType.NUMERIC);
                                break;
                            case "System.Decimal"://高精度数值型 
                                decimal decimalV = new decimal(0.0000);
                                decimal.TryParse(strColumnValue, out decimalV);
                                //获取精度(小数位数)
                                int iPrecision = (decimalV.ToString()).IndexOf('.')>0?(decimalV.ToString()).Length - (decimalV.ToString()).IndexOf('.') - 1:0;
                                string strDecimalFormat = iPrecision>0?"0.":"0";
                                for(int i = 0; i < iPrecision; i++)
                                {
                                    strDecimalFormat = strDecimalFormat + "0";
                                }
                                //double dbValue = Math.Round(double.Parse(decimalV.ToString()), iPrecision);
                                double dbValue = double.Parse(decimalV.ToString());
                                cell.SetCellValue(dbValue);
                                //strColumnValue = decimalV.ToString();
                                cell.SetCellType(CellType.NUMERIC);
                                cellStyle_Value1_Use.DataFormat = format.GetFormat(strDecimalFormat);
                                cellStyle_Value2_Use.DataFormat = format.GetFormat(strDecimalFormat);
                                break;
                            case "System.String"://字符串类型  
                            case "System.DBNull"://空值处理  
                                cell.SetCellValue(strColumnValue);
                                cell.SetCellType(CellType.STRING);
                                break;
                            default:
                                cell.SetCellValue(strColumnValue);
                                cell.SetCellType(CellType.STRING);
                                break;
                        }

                        //根据值来判断类型(无效，需再研究)
                        //Decimal dParseValue;
                        //if (Decimal.TryParse(strColumnValue.Replace(",",""), out dParseValue) == true) //判断是否可以转换为数值型
                        //{
                        //    strColumnValue = dParseValue.ToString();
                        //    cellStyle_Value1_Use.DataFormat = format.GetFormat("0.00");
                        //    cellStyle_Value2_Use.DataFormat = format.GetFormat("0.00"); 
                        //}

                        //cell.SetCellValue(strColumnValue);
                        if (rowIndex % 2 != 0)//奇数行
                        {
                            cell.CellStyle = cellStyle_Value1_Use;
                        }
                        else
                        {
                            cell.CellStyle = cellStyle_Value2_Use;
                        }
                        //设置数据格式


                        //设置该列的最大字符数
                        if (!hsColumnMaxLength.ContainsKey(column.Ordinal.ToString()))
                        {
                            hsColumnMaxLength.Add(column.Ordinal.ToString(), GetStringLength(strColumnValue));
                        }
                        else
                        {
                            if ((int)hsColumnMaxLength[column.Ordinal.ToString()] < GetStringLength(strColumnValue))
                            {
                                hsColumnMaxLength.Remove(column.Ordinal.ToString());
                                hsColumnMaxLength.Add(column.Ordinal.ToString(), GetStringLength(strColumnValue));
                            }
                        }
                    }
                    rowIndex++;
                }
                #endregion

                #region 冻结表头
                ///第一个参数表示要冻结的列数；
                //第二个参数表示要冻结的行数，这里只冻结列所以为2；
                //第三个参数表示右边区域可见的首列序号，从1开始计算；
                //第四个参数表示下边区域可见的首行序号，也是从1开始计算；
                //CreateFreezePane(int colSplit, int rowSplit, int leftmostColumn, int topRow)
                sheet.CreateFreezePane(2, 2, 2, 2);
                #endregion

                #region 设置每列的宽度为自适应
                //设置每列的宽度为自适应
                //设置列宽度
                foreach (DictionaryEntry de in hsColumnMaxLength) //ht为一个Hashtable实例
                {
                    int rowNo = int.Parse(de.Key.ToString());
                    int columnLength = int.Parse(de.Value.ToString()) + 3;//+3是为了适量增宽更好看
                    columnLength = columnLength > 20 ? 20 : columnLength;//最多不超过20个字符的显示
                    sheet.SetColumnWidth(rowNo, columnLength * 256);
                }
                #endregion
            }
            catch (Exception err)
            {
                log.Error("利用NOPI控件导出数据集到excel文件，返回数据流失败：" + err.ToString());
            }

            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;
            sheet = null;
            workbook = null;

            return ms;
        }

        /// <summary>
        /// 将MemoryStream保存到文件
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="fileName"></param>
        public static void SaveMSToFile(MemoryStream ms, string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                byte[] data = ms.ToArray();
                fs.Write(data, 0, data.Length);
                fs.Flush();
                data = null;
            }
        }

        /// <summary>
        /// 输出MemoryStream到客户端浏览器
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="context"></param>
        /// <param name="fileName"></param>
        public static void RenderMSToBrowser(MemoryStream ms, HttpContext context, string fileName)
        {
            if (context.Request.Browser.Browser == "IE")
                fileName = HttpUtility.UrlEncode(fileName);
            context.Response.AddHeader("Content-Disposition", "attachment;fileName=" + fileName + ".xls");
            context.Response.BinaryWrite(ms.ToArray());
            Encoding encoding = Encoding.GetEncoding("GB2312");
            context.Response.ContentEncoding = encoding;
            context.Response.HeaderEncoding = encoding;
            context.ApplicationInstance.CompleteRequest();
            context.Response.End();

        }


        /// <summary>
        /// 获取带中英文的字符串的实际字符串个数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        static int GetStringLength(String str)
        {
            byte[] strlength = System.Text.Encoding.Default.GetBytes(str);
            return strlength.Length;
        }
    }
}
