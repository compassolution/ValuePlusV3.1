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
    public class ExcelHelper
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

            #region 设置表格样式
            HSSFCellStyle cellStyle_TitlenHead = (HSSFCellStyle)workbook.CreateCellStyle();//表头栏样式
            HSSFCellStyle cellStyle_ColumnHead = (HSSFCellStyle)workbook.CreateCellStyle();//列头栏样式
            HSSFCellStyle cellStyle_Value1 = (HSSFCellStyle)workbook.CreateCellStyle();//奇数行样式
            HSSFCellStyle cellStyle_Value2 = (HSSFCellStyle)workbook.CreateCellStyle();//偶数行样式
            cellStyle_ColumnHead.DataFormat = format.GetFormat("@");
            cellStyle_Value1.DataFormat = format.GetFormat("@");
            cellStyle_Value2.DataFormat = format.GetFormat("@");

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

            //日期格式CellStyle
            HSSFCellStyle cellStyle_Value1_Date = (HSSFCellStyle)workbook.CreateCellStyle();
            HSSFCellStyle cellStyle_Value2_Date = (HSSFCellStyle)workbook.CreateCellStyle();
            HSSFCellStyle cellStyle_Value1_DateTime = (HSSFCellStyle)workbook.CreateCellStyle();
            HSSFCellStyle cellStyle_Value2_DateTime = (HSSFCellStyle)workbook.CreateCellStyle();
            cellStyle_Value1_Date.CloneStyleFrom(cellStyle_Value1);
            cellStyle_Value2_Date.CloneStyleFrom(cellStyle_Value2);
            cellStyle_Value1_DateTime.CloneStyleFrom(cellStyle_Value1);
            cellStyle_Value2_DateTime.CloneStyleFrom(cellStyle_Value2);
            cellStyle_Value1_Date.DataFormat = format.GetFormat("yyyy-MM-dd");
            cellStyle_Value2_Date.DataFormat = format.GetFormat("yyyy-MM-dd");
            cellStyle_Value1_DateTime.DataFormat = format.GetFormat("yyyy-MM-dd HH:mm:ss");
            cellStyle_Value2_DateTime.DataFormat = format.GetFormat("yyyy-MM-dd HH:mm:ss");

            //整型格式CellStyle
            HSSFCellStyle cellStyle_Value1_Int = (HSSFCellStyle)workbook.CreateCellStyle();
            HSSFCellStyle cellStyle_Value2_Int = (HSSFCellStyle)workbook.CreateCellStyle();
            cellStyle_Value1_Int.CloneStyleFrom(cellStyle_Value1);
            cellStyle_Value2_Int.CloneStyleFrom(cellStyle_Value2);
            cellStyle_Value1_Int.DataFormat = format.GetFormat("0");
            cellStyle_Value2_Int.DataFormat = format.GetFormat("0");
            //数值格式CellStyle
            HSSFCellStyle cellStyle_Value1_Decimal0 = (HSSFCellStyle)workbook.CreateCellStyle();
            HSSFCellStyle cellStyle_Value2_Decimal0 = (HSSFCellStyle)workbook.CreateCellStyle();
            HSSFCellStyle cellStyle_Value1_Decimal1 = (HSSFCellStyle)workbook.CreateCellStyle();
            HSSFCellStyle cellStyle_Value2_Decimal1 = (HSSFCellStyle)workbook.CreateCellStyle();
            HSSFCellStyle cellStyle_Value1_Decimal2 = (HSSFCellStyle)workbook.CreateCellStyle();
            HSSFCellStyle cellStyle_Value2_Decimal2 = (HSSFCellStyle)workbook.CreateCellStyle();
            HSSFCellStyle cellStyle_Value1_Decimal3 = (HSSFCellStyle)workbook.CreateCellStyle();
            HSSFCellStyle cellStyle_Value2_Decimal3 = (HSSFCellStyle)workbook.CreateCellStyle();
            HSSFCellStyle cellStyle_Value1_Decimal4 = (HSSFCellStyle)workbook.CreateCellStyle();
            HSSFCellStyle cellStyle_Value2_Decimal4 = (HSSFCellStyle)workbook.CreateCellStyle();
            cellStyle_Value1_Decimal0.CloneStyleFrom(cellStyle_Value1);
            cellStyle_Value2_Decimal0.CloneStyleFrom(cellStyle_Value2);
            cellStyle_Value1_Decimal1.CloneStyleFrom(cellStyle_Value1);
            cellStyle_Value2_Decimal1.CloneStyleFrom(cellStyle_Value2);
            cellStyle_Value1_Decimal2.CloneStyleFrom(cellStyle_Value1);
            cellStyle_Value2_Decimal2.CloneStyleFrom(cellStyle_Value2);
            cellStyle_Value1_Decimal3.CloneStyleFrom(cellStyle_Value1);
            cellStyle_Value2_Decimal3.CloneStyleFrom(cellStyle_Value2);
            cellStyle_Value1_Decimal4.CloneStyleFrom(cellStyle_Value1);
            cellStyle_Value2_Decimal4.CloneStyleFrom(cellStyle_Value2);
            cellStyle_Value1_Decimal0.DataFormat = format.GetFormat("0");
            cellStyle_Value2_Decimal0.DataFormat = format.GetFormat("0");
            cellStyle_Value1_Decimal1.DataFormat = format.GetFormat("0.0");
            cellStyle_Value2_Decimal1.DataFormat = format.GetFormat("0.0");
            cellStyle_Value1_Decimal2.DataFormat = format.GetFormat("0.00");
            cellStyle_Value2_Decimal2.DataFormat = format.GetFormat("0.00");
            cellStyle_Value1_Decimal3.DataFormat = format.GetFormat("0.000");
            cellStyle_Value2_Decimal3.DataFormat = format.GetFormat("0.000");
            cellStyle_Value1_Decimal4.DataFormat = format.GetFormat("0.0000");
            cellStyle_Value2_Decimal4.DataFormat = format.GetFormat("0.0000");
            Hashtable hsTableDecimalFormat1 = new Hashtable();
            Hashtable hsTableDecimalFormat2 = new Hashtable();
            hsTableDecimalFormat1.Add("0", cellStyle_Value1_Decimal0);
            hsTableDecimalFormat1.Add("1", cellStyle_Value1_Decimal1);
            hsTableDecimalFormat1.Add("2", cellStyle_Value1_Decimal2);
            hsTableDecimalFormat1.Add("3", cellStyle_Value1_Decimal3);
            hsTableDecimalFormat1.Add("4", cellStyle_Value1_Decimal4);
            hsTableDecimalFormat2.Add("0", cellStyle_Value2_Decimal0);
            hsTableDecimalFormat2.Add("1", cellStyle_Value2_Decimal1);
            hsTableDecimalFormat2.Add("2", cellStyle_Value2_Decimal2);
            hsTableDecimalFormat2.Add("3", cellStyle_Value2_Decimal3);
            hsTableDecimalFormat2.Add("4", cellStyle_Value2_Decimal4);

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
            HSSFRow headerRow = (HSSFRow)sheet.CreateRow(0);
            headerRow.HeightInPoints = 35;
            headerRow.CreateCell(0).SetCellValue(strFileName);

            headerRow.GetCell(0).CellStyle = cellStyle_TitlenHead;
            //合并表头单元格
            sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, myTable.Columns.Count - 1)); 
            #endregion

            #region 创建列头
            HSSFRow columnRow = (HSSFRow)sheet.CreateRow(1);
            // handling header.    
            foreach (DataColumn column in myTable.Columns)
            {
                HSSFCell cell = (HSSFCell)columnRow.CreateCell(column.Ordinal);
                cell.SetCellValue(column.Caption);

                switch (column.DataType.ToString())
                {
                    case "System.DateTime"://日期类型
                        cellStyle_ColumnHead.DataFormat = format.GetFormat("yyyy-MM-dd HH:mm:ss");
                        break;
                    case "System.Decimal"://高精度数值型 
                        cellStyle_ColumnHead.DataFormat = format.GetFormat("0.00");
                        break;
                    default:
                        cellStyle_ColumnHead.DataFormat = format.GetFormat("@");
                        break;
                }
                cell.CellStyle = cellStyle_ColumnHead;

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

            try
            {
                #region 插入表数据
                // handling value.  
                int rowIndex = 2;//由于第一行是表头标题
                foreach (DataRow row in myTable.Rows)
                {
                    HSSFRow dataRow = (HSSFRow)sheet.CreateRow(rowIndex);
                    foreach (DataColumn column in myTable.Columns)
                    {
                        HSSFCell cell = (HSSFCell)dataRow.CreateCell(column.Ordinal);

                        String strColumnValue = row[column].ToString();
                        object columnValue = row[column];
                        String strDataType = column.DataType.ToString();
                        //根据ColumnValue的值判断数据类型【将可能输出自定义的类型】
                        JudgeDataTypeByValue(strColumnValue, ref strDataType);
                        switch (strDataType)
                        {
                            case "System.DateTime"://日期类型
                            case "Custom.Date"://自定义日期类型Custom.Date
                            case "Custom.DateTime"://自定义日期时间类型Custom.DateTime
                                // 直接设置日期值而不是转换为字符串
                                if (columnValue != DBNull.Value)
                                {
                                    cell.SetCellValue(Convert.ToDateTime(columnValue));
                                }
                                else
                                {
                                    cell.SetCellValue("");
                                }
                                if(strDataType == "Custom.Date")
                                {
                                    //奇偶数行不同style
                                    cell.CellStyle = (rowIndex % 2 != 0) ? cellStyle_Value1_Date : cellStyle_Value2_Date;
                                }
                                else
                                {
                                    //奇偶数行不同style
                                    cell.CellStyle = (rowIndex % 2 != 0) ? cellStyle_Value1_DateTime : cellStyle_Value2_DateTime;
                                }

                                break;
                            case "System.Boolean"://布尔型      
                                bool boolV = false;
                                bool.TryParse(strColumnValue, out boolV);
                                cell.SetCellValue(boolV);
                                //strColumnValue = boolV.ToString();
                                cell.SetCellType(CellType.BOOLEAN);
                                //奇偶数行不同style
                                cell.CellStyle = (rowIndex % 2 != 0) ? cellStyle_Value1 : cellStyle_Value2;
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
                                //奇偶数行不同style
                                cell.CellStyle = (rowIndex % 2 != 0) ? cellStyle_Value1_Int : cellStyle_Value2_Int;
                                break;
                            case "System.Double"://浮点型 
                            case "System.Decimal"://高精度数值型 
                                decimal decimalV = new decimal(0.0000);
                                decimal.TryParse(strColumnValue, out decimalV);
                                //获取精度(小数位数)
                                int iPrecision = (decimalV.ToString()).IndexOf('.')>0?(decimalV.ToString()).Length - (decimalV.ToString()).IndexOf('.') - 1:0;
                                iPrecision = (iPrecision>4)?4:iPrecision;
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
                                //奇偶数行不同style
                                cell.CellStyle = (rowIndex % 2 != 0) ? (HSSFCellStyle)hsTableDecimalFormat1[iPrecision.ToString()] : (HSSFCellStyle)hsTableDecimalFormat2[iPrecision.ToString()];
                                break;
                            case "System.String"://字符串类型  
                            case "System.DBNull"://空值处理   
                            default:
                                cell.SetCellValue(strColumnValue);
                                cell.SetCellType(CellType.STRING);
                                //奇偶数行不同style
                                cell.CellStyle = (rowIndex % 2 != 0) ? cellStyle_Value1 : cellStyle_Value2;
                                break;
                        }

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
                //如果出错，删除sheet的所有行,保留抬头和列名
                int lastRowNum = sheet.LastRowNum;
                for (int i = 2; i <= lastRowNum; i++){
                    sheet.RemoveRow(sheet.GetRow(i));
                }
                headerRow.GetCell(0).SetCellValue(strFileName + "（导出数据失败：" + err.Message + "）");
            }

            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;
            sheet = null;
            workbook = null;

            return ms;
        }

        /// <summary>
        /// 根据ColumnValue的值判断数据类型【将可能输出自定义的类型】
        /// </summary>
        /// <param name="strColumnValue"></param>
        /// <returns></returns>
        private static void JudgeDataTypeByValue(string strColumnValue, ref string strDataType)
        {
            if (string.IsNullOrEmpty(strColumnValue))
                return;

            // 只判断原有类型是字符串或者本身是日期类型的
            if (
                (strDataType.Equals("System.String", StringComparison.Ordinal) || strDataType.Equals("System.DateTime", StringComparison.Ordinal))
                && (strColumnValue.IndexOf("-")>-1 || strColumnValue.IndexOf("/") > -1 || strColumnValue.IndexOf("AM") > -1 || strColumnValue.IndexOf("PM") > -1)
                && DateTime.TryParse(strColumnValue, out DateTime date))
            {
                // 判断是否为纯日期（时间部分为0）
                if (date.TimeOfDay.TotalSeconds == 0)
                {
                    strDataType = "Custom.Date";
                }
                else
                {
                    strDataType = "Custom.DateTime";
                }
            }
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
