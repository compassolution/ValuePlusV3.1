<%@ WebHandler Language="C#" Class="Payroll" %>

using System;
using System.Web;
using System.Text;
using System.Data;
using System.IO;
using System.Collections;
using Com.ValuePlus.Web;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Weixin;
using Com.ValuePlus.Utils.Serializable;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

public class Payroll : HandlerBase, IHttpHandler, System.Web.SessionState.IRequiresSessionState {

    public void ProcessRequest (HttpContext context) {
        //解析客户端传递过来的json data
        StreamReader reader = new StreamReader(context.Request.InputStream);
        String strParamJson = HttpUtility.UrlDecode(reader.ReadToEnd());
        log.Error("传入参数的Json data字符串:" + strParamJson.ToString());

        string strParam = WebCommon.GetJsonValue(strParamJson,"param").ToString();//请求类型参数
        string strLanguage = WebCommon.GetJsonValue(strParamJson,"language").ToString();
        string strMobileNo = WebCommon.GetJsonValue(strParamJson,"mobileno").ToString();
        string strProjectId = WebCommon.GetJsonValue(strParamJson,"projectid").ToString();
        string strYearMonthType = WebCommon.GetJsonValue(strParamJson,"yearmonthtype").ToString();
        string strYearMonth = WebCommon.GetJsonValue(strParamJson,"yearmonth").ToString();
        string strStaffNo = WebCommon.GetJsonValue(strParamJson,"staffno").ToString();

        log.Error("Payroll.ashx,MobileNo:"+strMobileNo+";Language:"+strLanguage);
        if (strParam.Equals("getlastoneprojectmonth"))
        {
            context.Response.Write(this.GetLastOneProjectMonth(context, strMobileNo, strLanguage));
        }
        else if (strParam.Equals("getprojectidlist"))
        {
            context.Response.Write(this.GetProjectList(context,strMobileNo,strLanguage));
        }
        else if (strParam.Equals("getyearmonth"))
        {
            context.Response.Write(this.GetYearMonthList(context,strMobileNo,strProjectId));
        }
        else if (strParam.Equals("getcurstaffdutyinfo"))
        {
            context.Response.Write(this.GetCurStaffDutyInfo(context,strMobileNo,strProjectId,strYearMonth,strStaffNo));
        }
        else if (strParam.Equals("getstaffpayrolldetail"))
        {
            context.Response.Write(this.GetStaffPayrollDetail(context,strMobileNo,strProjectId,strYearMonth,strStaffNo));
        }
        else if (strParam.Equals("setfirstviewtime"))
        {
            //设置员工查看对应薪资期间的员工薪资条的第一次查看时间
            context.Response.Write(this.SetFirstViewTime(context,strMobileNo,strProjectId,strYearMonth,strStaffNo));
        }
        else if (strParam.Equals("staffconfirmpayroll"))
        {
            //设置员工确认对应薪资期间的员工薪资条的确认时间
            context.Response.Write(this.StaffConfirmPayroll(context,strMobileNo,strProjectId,strYearMonth,strStaffNo));
        }
        else if (strParam.Equals("systemautoconfirmpayroll"))
        {
            //根据默认日期系统自动设置确认薪资的时间
            context.Response.Write(this.SystemAutoConfirmPayroll(context,strMobileNo,strProjectId,strYearMonth,strStaffNo));
        }
    }

    /// <summary>
    /// 获取最后一个就职单位的最后一个月份信息
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strMobileNo"></param>
    /// <param name="strLanguage"></param>
    private String GetLastOneProjectMonth(HttpContext context,String strMobileNo,String strLanguage)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "获取最后一个就职单位的最后一个月份信息";
        try
        {
            log.Error("获取最后一个就职单位的最后一个月份信息,MobileNo:"+strMobileNo+";Language:"+strLanguage);
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select TOP 1 ProjectId ");
            if (strLanguage.Equals("0"))
            {
                sbSql.Append(",ProjectNameChs as ProjectName");
            }else
            {
                sbSql.Append(",ProjectName");
            }
            sbSql.Append(",YearMonth ");
            sbSql.Append(",PSTART+'/'+PEND as YearMonthScope ");
            sbSql.Append(",EMPNO AS STAFFNO ");//add by sammen 20260204 增加工号STAFFNO的传递
            sbSql.Append(" from [TB_Remote_StaffSalaryData] A where 1=1 ");
            //modify by sammen 20260105 由于 有些员工中途更换手机号，造成更换手机号后的月份就无法获取到了
            //这里修改方案为，现根据手机号获取到工号，然后通过工号获取月份
            sbSql.Append(" AND StaffMobile = '"+strMobileNo+"'");//modify by sammen 20260205 后续各个方法都新增的工号的传递，可恢复，并且必须恢复，因为只是根据工号定位可能还会串到其他酒店
            sbSql.Append(" AND EMPNO = (select top 1 EMPNO from TB_Remote_StaffSalaryData where StaffMobile = '"+strMobileNo+"' order by JDATE DESC)");
            //ADD BY SAMMEN 20251020 如果在某酒店离职，则无法查看到该酒店的所有月度薪资
            //sbSql.Append(" AND NOT EXISTS (SELECT TOP 1 DCSTATUS FROM [TB_Remote_StaffSalaryData] WHERE StaffMobile = A.StaffMobile AND DCSTATUS IN ('3'))");
            //MODIFY BY SAMMEN 20251030 同一个项目，可能存在多次入职的情况，取最后一次入职的记录是否已离职》
            sbSql.Append(" AND (SELECT TOP 1 DCSTATUS FROM [TB_Remote_StaffSalaryData] WHERE ProjectId = A.ProjectId AND StaffMobile = A.StaffMobile order by JDATE DESC ) NOT IN ('3')");
            sbSql.Append(" order by YEARMONTH DESC");
            String strSql = sbSql.ToString();
            log.Error(strMethodDesc+"SQL:"+strSql);

            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
            sbResultData.Append(WebCommon.GetJsonStringByDataTable(dt,"\"ResultData\"",false));

            strReturnCode = "1";
            strReturnMsg = strMethodDesc+"成功";
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = strMethodDesc+"出错";
            log.Error(ex);
        }
        finally{
            StringBuilder sbResultStatus = new StringBuilder();
            sbResultStatus.Append("\"ReturnStatus\":{");
            sbResultStatus.Append("\"ReturnCode\":\""+strReturnCode.ToString()+"\"");
            sbResultStatus.Append(",\"ReturnMsg\":\""+strReturnMsg.ToString()+"\"");
            sbResultStatus.Append("}");

            sbResult.Append("{");
            sbResult.Append(sbResultStatus.ToString());
            if (!String.IsNullOrEmpty(sbResultData.ToString()))
            {
                sbResult.Append(","+sbResultData.ToString());
            }
            sbResult.Append("}");
            log.Error(strMethodDesc+"Return Json:"+sbResult.ToString());
        }
        return sbResult.ToString();
    }

    /// <summary>
    /// 获取就职单位列表
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strMobileNo"></param>
    /// <param name="strLanguage"></param>
    private String GetProjectList(HttpContext context,String strMobileNo,String strLanguage)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "获取就职单位列表";
        try
        {
            log.Error("获取就职单位列表,MobileNo:"+strMobileNo+";Language:"+strLanguage);
            StringBuilder sbSql = new StringBuilder();
            if (strLanguage.Equals("0"))
            {
                sbSql.Append("select distinct ProjectId,ProjectNameChs as ProjectName ");
            }else
            {
                sbSql.Append("select distinct ProjectId,ProjectName ");
            }
            sbSql.Append(" from [TB_Remote_StaffSalaryData] A where StaffMobile = '"+strMobileNo+"'");
            //ADD BY SAMMEN 20251020 如果在某酒店离职，则无法查看到该酒店的所有月度薪资
            //sbSql.Append(" AND NOT EXISTS (SELECT TOP 1 DCSTATUS FROM [TB_Remote_StaffSalaryData] WHERE StaffMobile = A.StaffMobile AND DCSTATUS IN ('3'))");
            //MODIFY BY SAMMEN 20251030 同一个项目，可能存在多次入职的情况，取最后一次入职的记录是否已离职》
            sbSql.Append(" AND (SELECT TOP 1 DCSTATUS FROM [TB_Remote_StaffSalaryData] WHERE ProjectId = A.ProjectId AND StaffMobile = A.StaffMobile order by JDATE DESC ) NOT IN ('3')");
            String strSql = sbSql.ToString();
            log.Error(strMethodDesc+"SQL:"+strSql);

            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
            sbResultData.Append(WebCommon.GetJsonStringByDataTable(dt,"\"ResultData\"",false));

            strReturnCode = "1";
            strReturnMsg = strMethodDesc+"成功";
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = strMethodDesc+"出错";
            log.Error(ex);
        }
        finally{
            StringBuilder sbResultStatus = new StringBuilder();
            sbResultStatus.Append("\"ReturnStatus\":{");
            sbResultStatus.Append("\"ReturnCode\":\""+strReturnCode.ToString()+"\"");
            sbResultStatus.Append(",\"ReturnMsg\":\""+strReturnMsg.ToString()+"\"");
            sbResultStatus.Append("}");

            sbResult.Append("{");
            sbResult.Append(sbResultStatus.ToString());
            if (!String.IsNullOrEmpty(sbResultData.ToString()))
            {
                sbResult.Append(","+sbResultData.ToString());
            }
            sbResult.Append("}");
        }
        return sbResult.ToString();
    }

    /// <summary>
    /// 获取薪资期间列表
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strMobileNo"></param>
    /// <param name="strProjectId"></param>
    private String GetYearMonthList(HttpContext context,String strMobileNo,String strProjectId)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "获取薪资期间列表";
        try
        {
            String strCurYearMonth = "";
            StringBuilder sbSql = new StringBuilder();
            //add by sammen 20260204 增加工号STAFFNO的传递
            sbSql.Append("select distinct YearMonth,PSTART,PEND,EMPNO AS STAFFNO from [TB_Remote_StaffSalaryData] where 1=1  ");
            sbSql.Append(" AND ProjectId = '"+strProjectId+"'");
            //modify by sammen 20260105 由于 有些员工中途更换手机号，造成更换手机号后的月份就无法获取到了
            //这里修改方案为，现根据手机号获取到工号，然后通过工号获取月份
            //sbSql.Append(" AND StaffMobile = '"+strMobileNo+"'");
            //delete by sammen 20260204 有些酒店(雄安索菲特)人事关系需求，很多员工离职后重新入职，手机号不变，工号有变更，又想能看到之前离职工号的工资条
            //sbSql.Append(" AND EMPNO = (select top 1 EMPNO from TB_Remote_StaffSalaryData where ProjectId = '"+strProjectId+"' and StaffMobile = '"+strMobileNo+"' order by JDATE DESC)");
            //add    by sammen 20260204 有些酒店(雄安索菲特)人事关系需求，很多员工离职后重新入职，手机号不变，工号有变更，又想能看到之前离职工号的工资条
            sbSql.Append(" AND EMPNO in (select DISTINCT EMPNO from TB_Remote_StaffSalaryData where ProjectId = '"+strProjectId+"' and StaffMobile = '"+strMobileNo+"')");
            sbSql.Append(" order by YEARMONTH desc");
            String strSql = sbSql.ToString();

            log.Error(strMethodDesc+"SQL:"+strSql);
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
            sbResultData.Append(WebCommon.GetJsonStringByDataTable(dt,"\"ResultData\"",false));

            strReturnCode = "1";
            strReturnMsg = strMethodDesc+"成功";
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = strMethodDesc+"出错";
            log.Error(ex);
        }
        finally{
            StringBuilder sbResultStatus = new StringBuilder();
            sbResultStatus.Append("\"ReturnStatus\":{");
            sbResultStatus.Append("\"ReturnCode\":\""+strReturnCode.ToString()+"\"");
            sbResultStatus.Append(",\"ReturnMsg\":\""+strReturnMsg.ToString()+"\"");
            sbResultStatus.Append("}");

            sbResult.Append("{");
            sbResult.Append(sbResultStatus.ToString());
            if (!String.IsNullOrEmpty(sbResultData.ToString()))
            {
                sbResult.Append(","+sbResultData.ToString());
            }
            sbResult.Append("}");
        }
        return sbResult.ToString();
    }

    /// <summary>
    /// 获取当前登录用户当月在职信息
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strMobileNo"></param>
    /// <param name="strProjectId"></param>
    /// <param name="strYearMonth"></param>
    /// <param name="strStaffNo"></param>
    /// <returns></returns>
    public String GetCurStaffDutyInfo(HttpContext context,String strMobileNo,String strProjectId,String strYearMonth,String strStaffNo)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "获取当前登录用户当月在职信息";
        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select top 1 * from [TB_Remote_StaffSalaryData] where 1=1  ");
            sbSql.Append(" AND ProjectId = '"+strProjectId+"'");
            sbSql.Append(" AND YEARMONTH = '"+strYearMonth+"'");
            //modify by sammen 20260105 由于 有些员工中途更换手机号，造成更换手机号后的月份就无法获取到了
            //这里修改方案为，现根据手机号获取到工号，然后通过工号获取月份
            //sbSql.Append(" AND StaffMobile = '"+strMobileNo+"'");
            //MODIFY by sammen 20260204 有些酒店(雄安索菲特)人事关系需求，很多员工离职后重新入职，手机号不变，工号有变更，又想能看到之前离职工号的工资条,此次通过传递工号进来确定
            //sbSql.Append(" AND EMPNO = (select top 1 EMPNO from TB_Remote_StaffSalaryData where ProjectId = '"+strProjectId+"' and StaffMobile = '"+strMobileNo+"' order by JDATE DESC)");
            sbSql.Append(" AND EMPNO = '"+strStaffNo+"'");
            String strSql = sbSql.ToString();
            log.Error(strMethodDesc+"SQL:"+strSql);

            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
            sbResultData.Append(WebCommon.GetJsonStringByDataTable(dt,"\"ResultData\"",false));

            strReturnCode = "1";
            strReturnMsg = strMethodDesc+"成功";
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = strMethodDesc+"出错";
            log.Error(ex);
        }
        finally{
            StringBuilder sbResultStatus = new StringBuilder();
            sbResultStatus.Append("\"ReturnStatus\":{");
            sbResultStatus.Append("\"ReturnCode\":\""+strReturnCode.ToString()+"\"");
            sbResultStatus.Append(",\"ReturnMsg\":\""+strReturnMsg.ToString()+"\"");
            sbResultStatus.Append("}");

            sbResult.Append("{");
            sbResult.Append(sbResultStatus.ToString());
            if (!String.IsNullOrEmpty(sbResultData.ToString()))
            {
                sbResult.Append(","+sbResultData.ToString());
            }
            sbResult.Append("}");
        }
        return sbResult.ToString();
    }

    /// <summary>
    /// 获取员工对应薪资期间的员工薪资条明细
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strMobileNo"></param>
    /// <param name="strProjectId"></param>
    /// <param name="strYearMonth"></param>
    /// <param name="strStaffNo"></param>
    private String GetStaffPayrollDetail(HttpContext context,String strMobileNo,String strProjectId,String strYearMonth,String strStaffNo)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "获取员工对应薪资期间的员工薪资条明细";
        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select * from [TB_Remote_StaffSalaryData] where 1=1  ");
            //modify by sammen 20260105 由于 有些员工中途更换手机号，造成更换手机号后的月份就无法获取到了
            //这里修改方案为，现根据手机号获取到工号，然后通过工号获取月份
            //sbSql.Append(" AND StaffMobile = '"+strMobileNo+"'");
            //MODIFY by sammen 20260204 有些酒店(雄安索菲特)人事关系需求，很多员工离职后重新入职，手机号不变，工号有变更，又想能看到之前离职工号的工资条,此次通过传递工号进来确定
            //sbSql.Append(" AND EMPNO = (select top 1 EMPNO from TB_Remote_StaffSalaryData where ProjectId = '"+strProjectId+"' and StaffMobile = '"+strMobileNo+"' order by JDATE DESC)");
            sbSql.Append(" AND EMPNO = '"+strStaffNo+"'");
            sbSql.Append(" AND ProjectId = '"+strProjectId+"'");
            sbSql.Append(" AND YEARMONTH = '"+strYearMonth+"'");
            sbSql.Append(" and ISSHOW = 'true' order by ITEMPORDER");
            String strSql = sbSql.ToString();
            log.Error(strMethodDesc+"SQL:"+strSql);

            DataTable dt = SqlParamDao.GetDataTableBySql(sbSql.ToString());
            sbResultData.Append(WebCommon.GetJsonStringByDataTable(dt,"\"ResultData\"",false));

            strReturnCode = "1";
            strReturnMsg = strMethodDesc+"成功";

            //新增用户访问记录信息表的写入 add by sammen 20190919
            WXUser.RecordWXUser_3(strMobileNo,strProjectId,"查看员工薪资条");
            //新增用户访问记录信息表的写入 add by sammen 20190919

        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = strMethodDesc+"出错";
            log.Error(ex);
        }
        finally{
            StringBuilder sbResultStatus = new StringBuilder();
            sbResultStatus.Append("\"ReturnStatus\":{");
            sbResultStatus.Append("\"ReturnCode\":\""+strReturnCode.ToString()+"\"");
            sbResultStatus.Append(",\"ReturnMsg\":\""+strReturnMsg.ToString()+"\"");
            sbResultStatus.Append("}");

            sbResult.Append("{");
            sbResult.Append(sbResultStatus.ToString());
            if (!String.IsNullOrEmpty(sbResultData.ToString()))
            {
                sbResult.Append(","+sbResultData.ToString());
            }
            sbResult.Append("}");
        }
        return sbResult.ToString();
    }

    /// <summary>
    /// 设置员工查看对应薪资期间的员工薪资条的第一次查看时间
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strMobileNo"></param>
    /// <param name="strProjectId"></param>
    /// <param name="strYearMonth"></param>
    /// <param name="strStaffNo"></param>
    /// <returns></returns>
    private String SetFirstViewTime(HttpContext context,String strMobileNo,String strProjectId,String strYearMonth,String strStaffNo)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "设置员工查看对应薪资期间的员工薪资条的第一次查看时间";
        try
        {
            String strNow = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            StringBuilder sbSql = new StringBuilder();
            //add by sammen 20260205新增FirstViewMobileNo字段的写入
            sbSql.Append("UPDATE [TB_Remote_StaffSalaryData] SET FirstViewTime = '"+strNow+"',FirstViewMobileNo = '"+strMobileNo+"' ");
            sbSql.Append(" where 1=1 ");
            //modify by sammen 20260105 由于 有些员工中途更换手机号，造成更换手机号后的月份就无法获取到了
            //这里修改方案为，现根据手机号获取到工号，然后通过工号获取月份
            //sbSql.Append(" AND StaffMobile = '"+strMobileNo+"'");
            //MODIFY by sammen 20260204 有些酒店(雄安索菲特)人事关系需求，很多员工离职后重新入职，手机号不变，工号有变更，又想能看到之前离职工号的工资条,此次通过传递工号进来确定
            //sbSql.Append(" AND EMPNO = (select top 1 EMPNO from TB_Remote_StaffSalaryData where ProjectId = '"+strProjectId+"' and StaffMobile = '"+strMobileNo+"' order by JDATE DESC)");
            sbSql.Append(" AND EMPNO = '"+strStaffNo+"'");
            sbSql.Append(" AND ProjectId = '"+strProjectId+"'");
            sbSql.Append(" AND YEARMONTH = '"+strYearMonth+"'");
            sbSql.Append(" AND isnull(FirstViewTime,'') = ''");
            String strSql = sbSql.ToString();
            log.Error("设置员工查看对应薪资期间的员工薪资条的第一次查看时间:"+strSql);
            int iCount = SqlParamDao.ExecuteNonQueryBySql(strSql);

            strReturnCode = "1";
            strReturnMsg = strMethodDesc+"成功";
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = strMethodDesc+"出错";
            log.Error(ex);
        }
        finally{
            StringBuilder sbResultStatus = new StringBuilder();
            sbResultStatus.Append("\"ReturnStatus\":{");
            sbResultStatus.Append("\"ReturnCode\":\""+strReturnCode.ToString()+"\"");
            sbResultStatus.Append(",\"ReturnMsg\":\""+strReturnMsg.ToString()+"\"");
            sbResultStatus.Append("}");

            sbResult.Append("{");
            sbResult.Append(sbResultStatus.ToString());
            if (!String.IsNullOrEmpty(sbResultData.ToString()))
            {
                sbResult.Append(","+sbResultData.ToString());
            }
            sbResult.Append("}");
        }
        return sbResult.ToString();
    }

    /// <summary>
    /// 设置员工确认对应薪资期间的员工薪资条的确认时间
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strMobileNo"></param>
    /// <param name="strProjectId"></param>
    /// <param name="strYearMonth"></param>
    /// <param name="strStaffNo"></param>
    /// <returns></returns>
    private String StaffConfirmPayroll(HttpContext context,String strMobileNo,String strProjectId,String strYearMonth,String strStaffNo)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "设置员工确认对应薪资期间的员工薪资条的确认时间";
        try
        {
            String strNow = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            StringBuilder sbSql = new StringBuilder();
            //add by sammen 20260205新增StaffConfirmMobileNo字段的写入
            sbSql.Append("UPDATE [TB_Remote_StaffSalaryData] SET StaffConfirmTime = '"+strNow+"', StaffConfirmMobileNo = '"+strMobileNo+"' where 1=1  ");
            //modify by sammen 20260105 由于 有些员工中途更换手机号，造成更换手机号后的月份就无法获取到了
            //这里修改方案为，现根据手机号获取到工号，然后通过工号获取月份
            //sbSql.Append(" AND StaffMobile = '"+strMobileNo+"'");
            //MODIFY by sammen 20260204 有些酒店(雄安索菲特)人事关系需求，很多员工离职后重新入职，手机号不变，工号有变更，又想能看到之前离职工号的工资条,此次通过传递工号进来确定
            //sbSql.Append(" AND EMPNO = (select top 1 EMPNO from TB_Remote_StaffSalaryData where ProjectId = '"+strProjectId+"' and StaffMobile = '"+strMobileNo+"' order by JDATE DESC)");
            sbSql.Append(" AND EMPNO = '"+strStaffNo+"'");
            sbSql.Append(" AND ProjectId = '"+strProjectId+"'");
            sbSql.Append(" AND YEARMONTH = '"+strYearMonth+"'");
            String strSql = sbSql.ToString();
            log.Error("设置员工确认对应薪资期间的员工薪资条的确认时间:"+strSql);

            int iCount = SqlParamDao.ExecuteNonQueryBySql(strSql);

            strReturnCode = "1";
            strReturnMsg = strMethodDesc+"成功";
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = strMethodDesc+"出错";
            log.Error(ex);
        }
        finally{
            StringBuilder sbResultStatus = new StringBuilder();
            sbResultStatus.Append("\"ReturnStatus\":{");
            sbResultStatus.Append("\"ReturnCode\":\""+strReturnCode.ToString()+"\"");
            sbResultStatus.Append(",\"ReturnMsg\":\""+strReturnMsg.ToString()+"\"");
            sbResultStatus.Append("}");

            sbResult.Append("{");
            sbResult.Append(sbResultStatus.ToString());
            if (!String.IsNullOrEmpty(sbResultData.ToString()))
            {
                sbResult.Append(","+sbResultData.ToString());
            }
            sbResult.Append("}");
        }
        return sbResult.ToString();
    }


    /// <summary>
    /// 根据默认日期系统自动设置确认薪资的时间
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strMobileNo"></param>
    /// <param name="strProjectId"></param>
    /// <param name="strYearMonth"></param>
    /// <param name="strStaffNo"></param>
    /// <returns></returns>
    private String SystemAutoConfirmPayroll(HttpContext context,String strMobileNo,String strProjectId,String strYearMonth ,String strStaffNo)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "根据默认日期系统自动设置确认薪资的时间";
        try
        {
            //int iConfirmDays = 7;
            ////获取默认多久系统默认已确认
            //StringBuilder sbSql_Get = new StringBuilder();
            //sbSql_Get = new StringBuilder();
            //sbSql_Get.Append("select top 1 * from [TB_Remote_StaffSalaryData] where 1=1  ");
            //sbSql_Get.Append(" AND StaffMobile = '"+strMobileNo+"'");
            //sbSql_Get.Append(" AND ProjectId = '"+strProjectId+"'");
            //sbSql_Get.Append(" AND YEARMONTH = '"+strYearMonth+"'");
            //sbSql_Get.Append(" order by ITEMPORDER");
            //String strSql_Get = sbSql_Get.ToString();
            //log.Error(strMethodDesc+"SQL:"+strSql_Get);
            //DataTable dt_Get = SqlParamDao.GetDataTableBySql(strSql_Get.ToString());
            //if(dt_Get!=null && dt_Get.Rows.Count>0){
            //    String strConfirmDays = dt_Get.Rows[0]["SystemConfirmTime"].ToString();
            //    if(!String.IsNullOrEmpty(strConfirmDays)){
            //        iConfirmDays = int.Parse(strConfirmDays);
            //    }
            //}

            String strNow = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("UPDATE [TB_Remote_StaffSalaryData] SET SystemConfirmTime = '"+strNow+"' ");
            sbSql.Append(" where 1=1 ");
            //modify by sammen 20260105 由于 有些员工中途更换手机号，造成更换手机号后的月份就无法获取到了
            //这里修改方案为，现根据手机号获取到工号，然后通过工号获取月份
            //sbSql.Append(" AND StaffMobile = '"+strMobileNo+"'");
            //MODIFY by sammen 20260204 有些酒店(雄安索菲特)人事关系需求，很多员工离职后重新入职，手机号不变，工号有变更，又想能看到之前离职工号的工资条,此次通过传递工号进来确定
            //sbSql.Append(" AND EMPNO = (select top 1 EMPNO from TB_Remote_StaffSalaryData where ProjectId = '"+strProjectId+"' and StaffMobile = '"+strMobileNo+"' order by JDATE DESC)");
            sbSql.Append(" AND EMPNO = '"+strStaffNo+"'");
            sbSql.Append(" AND ProjectId = '"+strProjectId+"'");
            sbSql.Append(" AND YEARMONTH = '"+strYearMonth+"'");
            //---员工已经查看过的
            sbSql.Append(" AND isnull(FirstViewTime,'') <> ''");
            //---员工未自己人工确认过的
            sbSql.Append(" AND isnull(StaffConfirmTime,'') = ''");
            //---未设置系统确认时间的
            sbSql.Append(" AND isnull(SystemConfirmTime,'') = ''");
            sbSql.Append(" AND DATEDIFF(HOUR,isnull(FirstViewTime,'2099-12-31 00:00:00'),GETDATE()) > 24* isnull(SystemConfirmDays,7)");// +iConfirmDays.ToString()+"");
            String strSql = sbSql.ToString();
            log.Error("根据默认日期系统自动设置确认薪资的时间:"+strSql);
            int iCount = SqlParamDao.ExecuteNonQueryBySql(strSql);

            strReturnCode = "1";
            strReturnMsg = strMethodDesc+"成功";
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = strMethodDesc+"出错";
            log.Error(ex);
        }
        finally{
            StringBuilder sbResultStatus = new StringBuilder();
            sbResultStatus.Append("\"ReturnStatus\":{");
            sbResultStatus.Append("\"ReturnCode\":\""+strReturnCode.ToString()+"\"");
            sbResultStatus.Append(",\"ReturnMsg\":\""+strReturnMsg.ToString()+"\"");
            sbResultStatus.Append("}");

            sbResult.Append("{");
            sbResult.Append(sbResultStatus.ToString());
            if (!String.IsNullOrEmpty(sbResultData.ToString()))
            {
                sbResult.Append(","+sbResultData.ToString());
            }
            sbResult.Append("}");
        }
        return sbResult.ToString();
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}