<%@ WebHandler Language="C#" Class="ReadIDCard" %>

using System;
using System.Web;
using System.Text;
using System.Data;
using System.Collections;
using Com.ValuePlus.Web;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Labor;
using Com.ValuePlus.Utils.Serializable;
using System.IO;
using Com.ValuePlus.Common.Security;

public class ReadIDCard : HandlerBase, IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    public void ProcessRequest (HttpContext context) {
        Hashtable hsTableUrlQuery = this.GetUrlAnalyse(context);
        string strParam = hsTableUrlQuery["param"] == null ? string.Empty : hsTableUrlQuery["param"].ToString();//param
        string strAttType = hsTableUrlQuery["atttype"] == null ? string.Empty : hsTableUrlQuery["atttype"].ToString();//checktype

        switch (strParam.ToLower().ToString())
        {
            case "getloginusercode":
                context.Response.Write(this.GetUserCode());
                break;
            case "checkinout":
                this.DoCheckInOut(context,strAttType, this.GetUserCode());
                break;
            case "queryrecord":
                this.QueryRecord(context,this.GetUserCode());
                break;
        }
    }

    /// <summary>
    /// 门禁签到签出操作
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strAttType"></param>
    /// <param name="strUserCode"></param>
    public void DoCheckInOut(HttpContext context,String strAttType ,String strUserCode)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();

        StringBuilder sbResult = new StringBuilder();

        int iValidWorkOrder = 0;
        bool IsCanCheckInOut = false;
        String strCurCheckedWONO = "";
        try
        {
            String strCardNo = context.Request["txt_CardNo"].ToString();
            String strAttTime = context.Request["txt_CurTime"].ToString();
            //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
            strCardNo = SQLInjectionDefense.ReplaceSQLReservedKeyword(strCardNo);
            strAttTime = SQLInjectionDefense.ReplaceSQLReservedKeyword(strAttTime);

            String strLaborUserCode = strCardNo;
            String strEmployCompany = "";
            String strServiceCompany = "";
            String strWONO = "";
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("SELECT * FROM [dbo].[Fun_Labor_GetLaborDutyInfo]('"+strLaborUserCode+"','"+strWONO+"','"+strEmployCompany+"','"+strServiceCompany+"','"+strAttType+"','"+strAttTime+"','"+strUserCode+"')");
            sbSql.Append(" order by isnull(EntryStartTime,'') desc");
            log.Error("门禁签到签出操作时，通过外包工用户编码获取当前时间其工单上岗信息列表:"+sbSql.ToString());
            DataTable dt = SqlParamDao.GetDataTableBySql(sbSql.ToString());
            int iCount = dt.Rows.Count;
            if (iCount > 0)
            {
                for(int i = 0; i < iCount; i++)
                {
                    String strTempWONO = dt.Rows[i]["WONO"].ToString();
                    String strTempEntryStartTime = dt.Rows[i]["EntryStartTime"].ToString();
                    String strTempEmployItem_Name = dt.Rows[i]["EmployItem_Name"].ToString();
                    String strTempEmployDept_Name = dt.Rows[i]["EmployDept_Name"].ToString();
                    //存在有效工单
                    if (!String.IsNullOrEmpty(strTempWONO))
                    {
                        iValidWorkOrder++;

                        if (strAttType.Equals("010"))///入场签到
                        {
                            //如果入场时间到了设置的开始入场，则签到
                            if (DateTime.Parse(strAttTime) >= DateTime.Parse(strTempEntryStartTime))
                            {
                                IsCanCheckInOut = true;
                                strCurCheckedWONO = strCurCheckedWONO +strTempWONO+ ";";
                            }else
                            {
                                IsCanCheckInOut = false;
                            }
                        }
                        else if (strAttType.Equals("020"))//离场签出
                        {
                            IsCanCheckInOut = true;
                            strCurCheckedWONO = strCurCheckedWONO +strTempWONO+ ";";

                        }
                    }
                }
            }

            if(iValidWorkOrder<=0)
            {
                strReturnCode = "-1";
                strReturnMsg = "该身份证件未登记注册或今天无服务工单！";
            }else
            {
                if (IsCanCheckInOut)
                {
                    string[] strCurCheckedWONO_Arrary = strCurCheckedWONO.Split(';');
                    for(int j = 0; j < strCurCheckedWONO_Arrary.Length; j++)
                    {
                        String strToCheckWONO = strCurCheckedWONO_Arrary[j];
                        if (!String.IsNullOrEmpty(strToCheckWONO))
                        {
                            Hashtable hsTableParams = new Hashtable();
                            hsTableParams.Add("LaborCode", strLaborUserCode);
                            hsTableParams.Add("WONO", strToCheckWONO);
                            hsTableParams.Add("AttType", strAttType);
                            hsTableParams.Add("AttTime", strAttTime);
                            hsTableParams.Add("OpUserCode", strUserCode);
                            int iCount010 = LWorkOrder.SaveLaborAttRecord(hsTableParams);
                        }
                    }
                    sbResultData.Append(WebCommon.GetJsonStringByDataTable(dt, "\"ResultData\"", false));
                    strReturnCode = "1";
                    strReturnMsg = "该身份证件今天存在"+iValidWorkOrder.ToString()+"份服务工单！";
                }else
                {
                    strReturnCode = "-2";
                    strReturnMsg = "该身份证件今天有"+iValidWorkOrder.ToString()+"份服务,但尚未到达入场时间！";
                }
            }
            //保存身份证读取信息到数据表中
            this.SaveIDCardInfo(context,strUserCode);
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = "门禁签到签出操作出错,请稍候重试";
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
        log.Error("门禁签到签出操作时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }

    /// <summary>
    /// 保存身份证读取信息到数据表中
    /// </summary>
    /// <param name="context"></param>
    private void SaveIDCardInfo(HttpContext context,String strUserCode)
    {
        StringBuilder sbSql = new StringBuilder();
        try
        {
            String txt_CardNo = context.Request.Form["txt_CardNo"].ToString();
            //$("#txt_CardNo").val("");
            //$("#txt_CardNo_Show").val("");
            //$("#txt_Name").val("");
            //$("#txt_Sex").val("");
            //$("#txt_Nation").val("");
            //$("#txt_BirthDay").val("");
            //$("#txt_Address").val("");
            //$("#txt_Org").val("");
            //$("#txt_ValidDate").val("");
            //$("#txt_Photo").val("");
            sbSql.Append("if not exists (select 1 from [LIDCard_1] where [CardNo] = '"+txt_CardNo+"' )\r\n");
            sbSql.Append("begin\r\n");
            sbSql.Append("INSERT INTO [LIDCard_1]([CardNo],[Name],[Sex],[Nation],[BirthDay],[Address],[Org],[ValidDate],[Photo],[CreateTime],[CreateUser])values(\r\n");
            sbSql.Append("'"+context.Request.Form["txt_CardNo"].ToString()+"'\r\n");
            sbSql.Append(",'"+context.Request.Form["txt_Name"].ToString()+"'\r\n");
            sbSql.Append(",'"+context.Request.Form["txt_Sex"].ToString()+"'\r\n");
            sbSql.Append(",'"+context.Request.Form["txt_Nation"].ToString()+"'\r\n");
            sbSql.Append(",'"+context.Request.Form["txt_BirthDay"].ToString()+"'\r\n");
            sbSql.Append(",'"+context.Request.Form["txt_Address"].ToString()+"'\r\n");
            sbSql.Append(",'"+context.Request.Form["txt_Org"].ToString()+"'\r\n");
            sbSql.Append(",'"+context.Request.Form["txt_ValidDate"].ToString()+"'\r\n");
            sbSql.Append(",'"+context.Request.Form["txt_Photo"].ToString()+"'\r\n");
            sbSql.Append(",'"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"'\r\n");
            sbSql.Append(",'"+strUserCode+"')\r\n");
            sbSql.Append("end\r\n");
            sbSql.Append("else\r\n");
            sbSql.Append("begin\r\n");
            sbSql.Append("  UPDATE [LIDCard_1] SET ");
            sbSql.Append("  [Name] = '"+context.Request.Form["txt_Name"].ToString()+"'\r\n");
            sbSql.Append("  ,[Sex] = '"+context.Request.Form["txt_Sex"].ToString()+"'\r\n");
            sbSql.Append("  ,[Nation] = '"+context.Request.Form["txt_Nation"].ToString()+"'\r\n");
            sbSql.Append("  ,[BirthDay] = '"+context.Request.Form["txt_BirthDay"].ToString()+"'\r\n");
            sbSql.Append("  ,[Address] = '"+context.Request.Form["txt_Address"].ToString()+"'\r\n");
            sbSql.Append("  ,[Org] = '"+context.Request.Form["txt_Org"].ToString()+"'\r\n");
            sbSql.Append("  ,[ValidDate] = '"+context.Request.Form["txt_ValidDate"].ToString()+"'\r\n");
            sbSql.Append("  ,[Photo] = '"+context.Request.Form["txt_Photo"].ToString()+"'\r\n");
            sbSql.Append("  ,[CreateTime] = '"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"'\r\n");
            sbSql.Append("  ,[CreateUser] = '"+strUserCode+"'\r\n");
            sbSql.Append("  where [CardNo] = '"+context.Request.Form["txt_CardNo"].ToString()+"'\r\n");
            sbSql.Append("end\r\n");
            sbSql.Append("--更新LUser_1表中的证件照信息\r\n");
            sbSql.Append("exec [USP_Labor_LUser_UpdateIdentityPhoto] '"+txt_CardNo+"','"+strUserCode+"'");
            log.Error("保存身份证读取信息到数据表中,SQL："+sbSql.ToString());

            int iCount = SqlParamDao.ExecuteNonQueryBySql(sbSql.ToString());
            if (iCount > 0)
            {
                
            }
        }
        catch (Exception ex)
        {
            log.Error("保存身份证读取信息到数据表中,SQL："+sbSql.ToString());
            log.Error("保存身份证读取信息到数据表中出错"+ex);
        }
    }

    /// <summary>
    /// 根据条件获取签到签出记录列表
    /// </summary>
    /// <param name="context"></param>
    private void QueryRecord(HttpContext context,String strUserCode)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();

        StringBuilder sbResult = new StringBuilder();
        try
        {
            String strCardNo = context.Request["txt_CardNo"].ToString();
            String strLaborName = context.Request["txt_LaborName"].ToString();
            String strAttDate = context.Request["txt_AttDate"].ToString();
            String strAttType = context.Request["sel_AttType"].ToString();
            //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
            strCardNo = SQLInjectionDefense.ReplaceSQLReservedKeyword(strCardNo);
            strLaborName = SQLInjectionDefense.ReplaceSQLReservedKeyword(strLaborName);
            strAttDate = SQLInjectionDefense.ReplaceSQLReservedKeyword(strAttDate);
            strAttType = SQLInjectionDefense.ReplaceSQLReservedKeyword(strAttType);

            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select top 1000 A.LaborUser_IdentityNo,A.LaborUser_Name,A.LaborUser_GenderName,A.AttType_Name,A.AttTime,A.AttDesc");
            sbSql.Append(" ,C.EmployCompany_Name,C.EmployDept_Name,C.EmployItem_Name,C.ServiceCompany_Name");
            sbSql.Append(" from [VW_Labor_LUserAtt] A INNER JOIN LUser_1 B ON A.EmployCompany = ISNULL(B.CompanyCode,'')");
            sbSql.Append(" INNER JOIN VW_Labor_LWorkOrderMain C ON A.WONO = C.WONO  where 1=1");
            //sbSql.Append(" and B.UserCode = '" + strUserCode + "'");
            if (!String.IsNullOrEmpty(strCardNo))
            {
                sbSql.Append(" and A.LaborUser_IdentityNo = '" + strCardNo + "'");
            }
            if (!String.IsNullOrEmpty(strLaborName))
            {
                sbSql.Append(" and A.LaborUser_Name like '%" + strLaborName + "%'");
            }
            if (!String.IsNullOrEmpty(strAttDate))
            {
                sbSql.Append(" and left(A.AttTime,10) = '" + strAttDate + "'");
            }
            if (!String.IsNullOrEmpty(strAttType))
            {
                sbSql.Append(" and A.AttType = '" + strAttType + "'");
            }
            sbSql.Append(" order by A.AttTime DESC ");
            String strSql = sbSql.ToString();

            DataTable dtReturn = SqlParamDao.GetDataTableBySql(strSql);
            sbResultData.Append(WebCommon.GetJsonStringByDataTable(dtReturn, "\"ResultData\"", false));

            strReturnCode = "1";
            strReturnMsg = "根据条件获取签到签出记录列表成功";
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = "根据条件获取签到签出记录列表出错,请稍候重试";
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
        log.Error("根据条件获取签到签出记录列表时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}

