using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using Com.ValuePlus.BLL.NPOI;
using System.Data;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.IO;
using System.Data.SqlClient;
using System.Text;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Common.Config;
using NPOI.HSSF.Extractor;

/// <summary>
///ImportExcelToDB 的摘要说明
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
//若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。 
// [System.Web.Script.Services.ScriptService]
public class ImportFileToDB : System.Web.Services.WebService {

    /// <summary>
    /// 日志声明
    /// </summary>
    protected static Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    protected static String strDbConnection = BaseConfig.Instance.GetConnectionString();

    public ImportFileToDB () {

        //如果使用设计的组件，请取消注释以下行 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string HelloWorld() {
        return "Hello World";
    }

    /// <summary>
    /// 导入Excel2003文档的第一个Sheet数据到指定数据表中
    /// </summary>
    /// <returns></returns>
    [WebMethod]
    public String ImportExcel2003OneSheetToDB(String strFileFullName,String strDBTableName)
    {
        //DataTable dtData = ExcelHelper.GetDataTableFromExcel2003OneSheet(strFileFullName,true);

        //int iCount = SqlParamDao.InsertDBFromDataTable(dtData, strDBTableName);

        int iCount = ExcelHelper.InsertDBFromExcel2003OneSheet(strFileFullName,strDBTableName,true);
        return iCount.ToString();
    }


    /// <summary>
    /// 导入Excel文档到指定数据表中
    /// </summary>
    /// <returns></returns>
    [WebMethod]
    public String ImportExcel2003MultiSheetToDB(String strFileFullName, String strDBTableName)
    {
        String bIsSuccess = "true";
        //String strSql = "select * from [VW_Location_Moblie] order by SLCODE";
        //StringBuilder sbr = new StringBuilder();

        //using (FileStream fs = File.OpenRead(@strFileFullName))   //打开myxls.xls文件
        //{
        //    HSSFWorkbook wk = new HSSFWorkbook(fs);   //把xls文件中的数据写入wk中
        //    for (int i = 0; i < wk.NumberOfSheets; i++)  //NumberOfSheets是myxls.xls中总共的表数
        //    {
        //        ISheet sheet = wk.GetSheetAt(i);   //读取当前表数据
        //        for (int j = 0; j <= sheet.LastRowNum; j++)  //LastRowNum 是当前表的总行数
        //        {
        //            IRow row = sheet.GetRow(j);  //读取当前行数据
        //            if (row != null)
        //            {
        //                sbr.Append("-------------------------------------\r\n"); //读取行与行之间的提示界限
        //                for (int k = 0; k <= row.LastCellNum; k++)  //LastCellNum 是当前行的总列数
        //                {
        //                    ICell cell = row.GetCell(k);  //当前表格
        //                    if (cell != null)
        //                    {
        //                        sbr.Append(cell.ToString());   //获取表格中的数据并转换为字符串类型
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}
        //sbr.ToString();
        //using (StreamWriter wr = new StreamWriter(new FileStream(@"c:/myText.txt", FileMode.Append)))  //把读取xls文件的数据写入myText.txt文件中
        //{
        //    wr.Write(sbr.ToString());
        //    wr.Flush();
        //}


        return bIsSuccess;
    }



}
