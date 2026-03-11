<%@ WebHandler Language="C#" Class="WorkOrderHandler" %>

using System;
using System.Web;
using System.Collections;
using System.Text;
using System.IO;
using System.Data;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Com.ValuePlus.BLL.SysManager;
using Com.ValuePlus.BLL.User;
using Com.ValuePlus.DataLog;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Labor;
using Com.ValuePlus.Web;

public class WorkOrderHandler : HandlerBase,IHttpHandler, System.Web.SessionState.IRequiresSessionState
{

    public void ProcessRequest (HttpContext context) {
        //解析客户端传递过来的json data
        log.Error("传入参数的context.Request.InputStream:" + context.Request.InputStream);
        StreamReader reader = new StreamReader(context.Request.InputStream);
        String strParamJson = HttpUtility.UrlDecode(reader.ReadToEnd());
        log.Error("传入参数的Json data字符串:" + strParamJson.ToString());

        string param = WebCommon.GetJsonObjectValue(strParamJson,"param").ToString();//请求类型参数
        string strUserCode = WebCommon.GetJsonValue(strParamJson,"usercode").ToString();//用户名
        string strUserType = WebCommon.GetJsonValue(strParamJson,"usertype").ToString();//用户类型
        string strCompanyCode = WebCommon.GetJsonValue(strParamJson,"companycode").ToString();//公司编码
        string strDeptCode = WebCommon.GetJsonValue(strParamJson,"deptcode").ToString();//部门编码
        string strWONO = WebCommon.GetJsonValue(strParamJson,"wono").ToString();//工单编码
        int iPageIndex = String.IsNullOrEmpty(WebCommon.GetJsonValue(strParamJson,"pageindex"))?0:int.Parse(WebCommon.GetJsonValue(strParamJson,"pageindex").ToString());//当前页索引
        int iPageSize = String.IsNullOrEmpty(WebCommon.GetJsonValue(strParamJson,"pagesize"))?0:int.Parse(WebCommon.GetJsonValue(strParamJson,"pagesize").ToString());//每页记录数
        string strIsValid = WebCommon.GetJsonValue(strParamJson,"isvalid").ToString();//是否有效
        string strStatusFrom = WebCommon.GetJsonValue(strParamJson,"statusfrom").ToString();//状态从
        string strStatusTo = WebCommon.GetJsonValue(strParamJson,"statusto").ToString();//状态到
        string strOperateDesc = WebCommon.GetJsonValue(strParamJson,"operatedesc").ToString();//操作描述
        string strOperateIdea = WebCommon.GetJsonValue(strParamJson,"operateidea").ToString();//操作意见
        string strSetIsValid = WebCommon.GetJsonValue(strParamJson,"setisvalid").ToString();//设置是否有效标识
        string strConditionSql = WebCommon.GetJsonObjectValue(strParamJson,"conditionsql").ToString();//查询条件语句
        string strQueryDate = WebCommon.GetJsonValue(strParamJson,"querydate").ToString();//查询日期
        string strQueryTime = WebCommon.GetJsonValue(strParamJson,"querytime").ToString();//查询时间
        string strDateFrom = WebCommon.GetJsonValue(strParamJson,"datefrom").ToString();//日期从
        string strDateTo = WebCommon.GetJsonValue(strParamJson,"dateto").ToString();//日期到
        string strAttType = WebCommon.GetJsonValue(strParamJson,"atttype").ToString();//打卡类型
        string strAttTime = WebCommon.GetJsonValue(strParamJson,"atttime").ToString();//打卡时间
        string strEmployCompany = WebCommon.GetJsonValue(strParamJson,"employcompany").ToString();//用人单位编码
        string strEmployItem = WebCommon.GetJsonValue(strParamJson,"employitem").ToString();//用工类型
        string strServiceCompany = WebCommon.GetJsonValue(strParamJson,"servicecompany").ToString();//外包公司编码
        string strOrderBy = WebCommon.GetJsonValue(strParamJson,"orderby").ToString();//排序
        string strIsLoadActionList = WebCommon.GetJsonValue(strParamJson,"isloadactionlist").ToString();//排序
        string strGetType = WebCommon.GetJsonValue(strParamJson,"gettype").ToString();//获取类别

        String strBeObjectType = WebCommon.GetJsonValue(strParamJson, "beobjecttype").ToString();
        String strBeObjectCode = WebCommon.GetJsonValue(strParamJson, "beobjectcode").ToString();
        String strDoObjectType = WebCommon.GetJsonValue(strParamJson, "doobjecttype").ToString();
        String strDoObjectCode = WebCommon.GetJsonValue(strParamJson, "doobjectcode").ToString();
        String strSEQNO = WebCommon.GetJsonValue(strParamJson, "seqno").ToString();

        string strPostDataObject = WebCommon.GetJsonObjectValue(strParamJson,"postdataobj").ToString();//Json数据库对象

        string strLaborUserCode = WebCommon.GetJsonValue(strParamJson,"laborusercode").ToString();//外包工用户名
        string strIsSelect = WebCommon.GetJsonValue(strParamJson,"isselect").ToString();//选择或者是取消选择
        string strIsToConfirm = WebCommon.GetJsonValue(strParamJson,"istoconfirm").ToString();//选择或者是取消选择

        StringBuilder sbImportParam = new StringBuilder();
        sbImportParam.Append("param:" + param);
        sbImportParam.Append(",strLaborUserCode:" + strLaborUserCode);
        sbImportParam.Append(",strUserCode:" + strUserCode);
        sbImportParam.Append(",strUserType:" + strUserType);
        sbImportParam.Append(",strCompanyCode:" + strCompanyCode);
        sbImportParam.Append(",strConditionSql:" + strConditionSql);
        sbImportParam.Append(",strPostDataObject:" + strPostDataObject);
        log.Error("Labor/MPHandler/WorkOrderHandler.ashx传入参数:" + sbImportParam.ToString());

        switch (param.ToLower().ToString())
        {
            case "getoneworkorderinfo":
                this.GetOneWorkOrderInfo(context, strUserCode, strWONO, strIsValid,strIsLoadActionList);
                break;
            case "getworkorderlist":
                this.GetWorkOrderList(context, strUserCode, iPageIndex, iPageSize, strConditionSql,strOrderBy,strIsLoadActionList);
                break;
            case "gettimeline":
                this.GetWorkOrderTimeline(context, strUserCode, strWONO);
                break;
            case "getworkorderlabors":
                this.GetWorkOrderLabors(context, strUserCode, strWONO);
                break;
            case "getworkorderlaborslist":
                this.GetWorkOrderLaborsList(context, strUserCode, strWONO);
                break;
            case "getworkorderlaborsusercodearray":
                this.GetWorkOrderLaborsUserCodeArray(context, strUserCode, strWONO);
                break;
            case "getworkorderstatusconfig":
                this.GetWorkOrderStatusConfig(context, strWONO, strUserCode);
                break;
            case "getpendingqty":
                this.GetWorkOrderPendingQty(context, strUserCode);
                break;
            case "getworkorderstatustabs":
                this.GetWorkOrderStatusTabs(context, strUserCode);
                break;
            case "getlaborondutyinfo":
                this.GetLaborOnDutyInfo(context, strLaborUserCode, strWONO,strEmployCompany,strServiceCompany, strQueryTime, strUserCode,strAttType);
                break;
            case "getworkorderappraiseinfo":
                this.GetWorkOrderAppraiseInfo(context, strUserCode, strWONO, strBeObjectType, strBeObjectCode, strDoObjectType, strDoObjectCode);
                break;
            case "getmyyearmonthlist":
                this.GetMyYearMonthList(context, strUserCode,strGetType);
                break;
            case "getmysummarydata":
                this.GetMySummaryData(context, strUserCode,strDateFrom,strDateTo,strEmployCompany,strDeptCode,strEmployItem,strServiceCompany);
                break;
            case "saveworkorderappraisedata":
                this.SaveWorkOrderAppraiseData(context, strUserCode, strWONO,strSEQNO, strPostDataObject);
                break;
            case "deleteworkorder":
                this.DeleteOneWorkOrderInfo(context, strUserCode, strWONO, strSetIsValid);
                break;
            case "saveworkorder":
                this.SaveWorkOrderInfo(context, strUserCode, strCompanyCode, strWONO, strPostDataObject);
                break;
            case "setworkorderstatus":
                this.SetWorkOrderStatus(context, strUserCode, strWONO, strStatusFrom, strStatusTo, strOperateDesc, strOperateIdea);
                break;
            case "getindexrealtimedata":
                //根据用户编码获取首页实时数据项目
                this.GetIndexRealTimeData(context, strUserCode, strQueryDate);
                break;
            case "selectworkorderlabors":
                this.SelectWorkOrderLabors(context, strWONO, strLaborUserCode, strIsSelect, strUserCode);
                break;
            case "laborconfirmworkorder":
                this.LaborConfirmWorkOrder(context, strWONO, strLaborUserCode, strIsToConfirm);
                break;
            case "judgelaborisconfirmworkorder":
                this.JudgeLaborIsConfirmWorkOrder(context, strWONO, strLaborUserCode);
                break;
            case "savelaborattrecord":
                this.SaveLaborAttRecord(context, strLaborUserCode, strWONO, strAttType, strAttTime, strUserCode);
                break;
        }

    }

    /// <summary>
    /// 根据工单编码获取一条工单数据明细
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strUserCode"></param>
    /// <param name="strWONO"></param>
    /// <param name="strIsValid"></param>
    /// <param name="strIsLoadActionList"></param>
    public void GetOneWorkOrderInfo(HttpContext context,String strUserCode,String strWONO,String strIsValid,String strIsLoadActionList)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();

        StringBuilder sbResult = new StringBuilder();
        try
        {
            bool isLoadActionList = strIsLoadActionList.Equals("1") ? true : false;
            StringBuilder sbCondition = new StringBuilder();
            if (!String.IsNullOrEmpty(strWONO))
            {
                sbCondition.Append(" AND WONO = " + JObjectToDB.GetColumnEncryptDataString("LWorkOrder_1", "WONO", strWONO));
            }
            strIsValid = String.IsNullOrEmpty(strIsValid) ? "1" : strIsValid;
            sbCondition.Append(" AND IsValid = " + JObjectToDB.GetColumnEncryptDataString("LWorkOrder_1", "IsValid", strIsValid));

            //只传入工单号，则只获取一个工单信息
            DataTable dt = LWorkOrder.GetWorkOrderInfoDataTable(strUserCode,sbCondition.ToString(),0,"WONO DESC");

            strReturnCode = "1";
            strReturnMsg = "根据工单编码获取一条工单数据明细成功";
            //sbResultData.Append(WebCommon.GetJsonStringByDataTable(dt,"\"ResultData\"",false));
            //获取工单信息数据表包括对应可执行动作集合,返回json数据对象
            String strWorkOrderAndActionsList = LWorkOrder.GetWorkOrdersAndActionsJsonData(strUserCode, dt,isLoadActionList, false);
            sbResultData.Append(strWorkOrderAndActionsList);
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = "根据工单编码获取一条工单数据明细出错,请稍候重试";
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
        log.Error("根据工单编码获取一条工单数据明细时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }

    /// <summary>
    /// 根据用户编码获取其对应的所有工单数据集合
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strUserCode"></param>
    /// <param name="iPageIndex"></param>
    /// <param name="iPageSize"></param>
    /// <param name="strCondition"></param>
    /// <param name="strOrderBy"></param>
    /// <param name="strIsLoadActionList"></param>
    public void GetWorkOrderList(HttpContext context,String strUserCode,int iPageIndex,int iPageSize,String strCondition,String strOrderBy,String strIsLoadActionList)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();

        StringBuilder sbResult = new StringBuilder();
        try
        {
            strOrderBy = String.IsNullOrEmpty(strOrderBy) ? " WONO DESC" : strOrderBy;
            bool isLoadActionList = strIsLoadActionList.Equals("1") ? true : false;
            Hashtable hsTableRefValues = new Hashtable();
            hsTableRefValues.Add("iTotalRowCount",0);//记录行数
            hsTableRefValues.Add("SummaryEmployLaborCount", 0);//计划用工人数汇总
            hsTableRefValues.Add("SummaryActualLaborCount", 0);//实际到岗人数汇总
            hsTableRefValues.Add("SummaryTotalHours", 0.0);//用工总小时数汇总
            hsTableRefValues.Add("SummaryTotalRoomsCount", 0);//总房间数汇总
            hsTableRefValues.Add("SummaryTotalSubsidy", 0.00);//额外补贴合计汇总
            hsTableRefValues.Add("SummaryTotalCost", 0.00);//总费用汇总

            StringBuilder sbCondition = new StringBuilder();
            sbCondition.Append(strCondition);
            DataTable dt = LWorkOrder.GetWorkOrderInfoDataTable(strUserCode, sbCondition.ToString(), iPageIndex, iPageSize, strOrderBy, ref hsTableRefValues);
            //获取工单信息数据表包括对应可执行动作集合,返回json数据对象
            String strWorkOrderAndActionsList = LWorkOrder.GetWorkOrdersAndActionsJsonData(strUserCode, dt, isLoadActionList,false);
            sbResultData.Append(strWorkOrderAndActionsList);

            int iTotalRowCount = int.Parse(hsTableRefValues["iTotalRowCount"].ToString());
            int SummaryEmployLaborCount = int.Parse(hsTableRefValues["SummaryEmployLaborCount"].ToString());
            int SummaryActualLaborCount = int.Parse(hsTableRefValues["SummaryActualLaborCount"].ToString());
            int SummaryTotalHours = int.Parse(hsTableRefValues["SummaryTotalHours"].ToString());
            int SummaryTotalRoomsCount = int.Parse(hsTableRefValues["SummaryTotalRoomsCount"].ToString());
            int SummaryTotalSubsidy = int.Parse(hsTableRefValues["SummaryTotalSubsidy"].ToString());
            int SummaryTotalCost = int.Parse(hsTableRefValues["SummaryTotalCost"].ToString());

            strReturnCode = "1";
            strReturnMsg = "根据用户编码获取其对应的所有工单数据集合成功";

            //返回分页数据
            int iRemain = iPageSize>0?iTotalRowCount % iPageSize:iPageSize;
            int iTotalPages = iPageSize>0?iTotalRowCount / iPageSize:iPageSize;
            iTotalPages = iRemain == 0 ? iTotalPages : iTotalPages + 1;
            sbResultData.Append(",\"PagesData\":{");
            sbResultData.Append("\"iPageIndex\":\""+iPageIndex.ToString()+"\"");
            sbResultData.Append(",\"iPageSize\":\""+iPageSize.ToString()+"\"");
            sbResultData.Append(",\"iTotalRowCount\":\""+iTotalRowCount.ToString()+"\"");
            sbResultData.Append(",\"iTotalPages\":\""+iTotalPages.ToString()+"\"");
            sbResultData.Append("}");
            //返回汇总数据
            sbResultData.Append(",\"SummaryData\":{");
            sbResultData.Append("\"SummaryEmployLaborCount\":\""+SummaryEmployLaborCount.ToString()+"\"");
            sbResultData.Append(",\"SummaryActualLaborCount\":\""+SummaryActualLaborCount.ToString()+"\"");
            sbResultData.Append(",\"SummaryTotalHours\":\""+SummaryTotalHours.ToString()+"\"");
            sbResultData.Append(",\"SummaryTotalRoomsCount\":\""+SummaryTotalRoomsCount.ToString()+"\"");
            sbResultData.Append(",\"SummaryTotalSubsidy\":\""+SummaryTotalSubsidy.ToString()+"\"");
            sbResultData.Append(",\"SummaryTotalCost\":\""+SummaryTotalCost.ToString()+"\"");
            sbResultData.Append("}");
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = "根据用户编码获取其对应的所有工单数据集合出错,请稍候重试";
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
        log.Error("根据用户编码获取其对应的所有工单数据集合时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }


    /// <summary>
    /// 根据工单编码获取时间轴信息
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strUserCode"></param>
    /// <param name="strWONO"></param>
    /// <param name="strIsValid"></param>
    public void GetWorkOrderTimeline(HttpContext context,String strUserCode,String strWONO)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();

        StringBuilder sbResult = new StringBuilder();
        try
        {
            DataTable dt = LWorkOrder.GetWorkOrderTimelineDataTable(strWONO," SEQNO DESC");

            strReturnCode = "1";
            strReturnMsg = "根据工单编码获取时间轴信息成功";
            sbResultData.Append(WebCommon.GetJsonStringByDataTable(dt, "\"ResultData\"", false));
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = "根据工单编码获取时间轴信息出错,请稍候重试";
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
        log.Error("根据工单编码获取时间轴信息时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }

    /// <summary>
    /// 根据工单编码获取工单外包工信息
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strUserCode"></param>
    /// <param name="strWONO"></param>
    /// <param name="strIsValid"></param>
    public void GetWorkOrderLabors(HttpContext context,String strUserCode,String strWONO)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();

        StringBuilder sbResult = new StringBuilder();
        try
        {
            DataTable dt = LWorkOrder.GetWorkOrderLaborsDataTable(strWONO,"","");

            strReturnCode = "1";
            strReturnMsg = "根据工单编码获取工单外包工信息成功";
            sbResultData.Append(WebCommon.GetJsonStringByDataTable(dt, "\"ResultData\"", false));
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = "根据工单编码获取工单外包工信息出错,请稍候重试";
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
        log.Error("根据工单编码获取工单外包工信息时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }

    /// <summary>
    /// 根据工单编码获取工单外包工信息
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strUserCode"></param>
    /// <param name="strWONO"></param>
    public void GetWorkOrderLaborsList(HttpContext context,String strUserCode,String strWONO)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();

        StringBuilder sbResult = new StringBuilder();
        try
        {
            DataTable dt = LWorkOrder.GetWorkOrderLaborsDataTable(strWONO, "", "");
            sbResultData.Append(WebCommon.GetJsonStringByDataTable(dt, "\"ResultData\"", false));
            strReturnCode = "1";
            strReturnMsg = "根据工单编码获取工单外包工信息成功";
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = "根据工单编码获取工单外包工信息出错,请稍候重试";
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
        log.Error("根据工单编码获取工单外包工信息时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }

    /// <summary>
    /// 根据工单编码获取工单外包工信息,仅返回用户编码数组
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strUserCode"></param>
    /// <param name="strWONO"></param>
    public void GetWorkOrderLaborsUserCodeArray(HttpContext context,String strUserCode,String strWONO)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();

        StringBuilder sbResult = new StringBuilder();
        try
        {
            sbResultData.Append("\"ResultData\":[");
            DataTable dt = LWorkOrder.GetWorkOrderLaborsDataTable(strWONO, "", "");
            if (dt != null & dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    String strLaborCode = dr["UserCode"].ToString();
                    String strConfimTime = dr["ConfirmTime"].ToString();
                    if (i == 0)
                    {
                        sbResultData.Append("{\"LaborCode\":\"" + strLaborCode + "\",\"ConfirmTime\":\"" + strConfimTime + "\"}");
                    }
                    else
                    {
                        sbResultData.Append(",{\"LaborCode\":\"" + strLaborCode + "\",\"ConfirmTime\":\"" + strConfimTime + "\"}");
                    }
                }
            }
            sbResultData.Append("]");

            strReturnCode = "1";
            strReturnMsg = "根据工单编码获取工单外包工,仅返回用户编码数组成功";
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = "根据工单编码获取工单外包工,仅返回用户编码数组出错,请稍候重试";
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
        log.Error("根据工单编码获取工单外包工,仅返回用户编码数组时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }

    /// <summary>
    /// 保存工单信息
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strUserCode"></param>
    /// <param name="strCompanyCode"></param>
    /// <param name="strWONO"></param>
    /// <param name="strPostDataObject"></param>
    public void SaveWorkOrderInfo(HttpContext context,String strUserCode ,String strCompanyCode,String strWONO,String strPostDataObject)
    {
        log.Error("保存工单信息的数据："+strPostDataObject.ToString());
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();

        StringBuilder sbResult = new StringBuilder();
        try
        {
            String strOpType = "edit";
            if (String.IsNullOrEmpty(strWONO))
            {
                //如果编码不存在，则自动生成一个新编码
                strWONO = LWorkOrder.GenerateWONO(strCompanyCode);
                strOpType = "add";
            }
            string strTableName = "LWorkOrder_1";

            //json字符串转json的JObject对象
            JObject jo = (JObject)JsonConvert.DeserializeObject(strPostDataObject);

            Hashtable hsTableKey = new Hashtable();
            hsTableKey.Add("WONO", strWONO);

            int iCount = 0;
            //判断这个主键是否存在，如果
            if (JObjectToDB.JudgeRecordIsExists(strTableName, hsTableKey))
            {
                iCount = JObjectToDB.UpdateJObjectDataToTable(jo, strTableName, hsTableKey);
            }else
            {
                iCount = JObjectToDB.InsertJObjectDataToTable(jo, strTableName, hsTableKey);
            }

            if (iCount > 0)
            {
                //保存成功后，继续后续操作
                Hashtable hsTableParams = new Hashtable();
                hsTableParams.Add("WONO",strWONO);
                hsTableParams.Add("OpType",strOpType);
                hsTableParams.Add("UserCode",strUserCode);
                iCount = LWorkOrder.DoAfterSaveWorkOrder(hsTableParams);

                strReturnCode = "1";
                strReturnMsg = "保存工单信息成功";
                sbResultData.Append("\"ResultData\":{\"WONO\":\"" + strWONO + "\"}");
            }
            else
            {
                strReturnCode = "-2";
                strReturnMsg = "保存工单信息失败";
            }
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = "保存工单信息出错,请稍候重试";
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
        log.Error("保存工单信息时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }

    /// <summary>
    /// 删除工单信息
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strUserCode"></param>
    /// <param name="strNeedOpAccount"></param>
    /// <param name="strSetIsValid"></param>//为空时直接删除
    public void DeleteOneWorkOrderInfo(HttpContext context,String strUserCode,String strWONO,String strSetIsValid)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();

        StringBuilder sbResult = new StringBuilder();
        try
        {
            int iCount = LWorkOrder.DeleteWorkOrderInfo(strUserCode,strWONO,strSetIsValid);
            if (iCount > 0)
            {
                strReturnCode = "1";
                strReturnMsg = "删除工单信息成功";
                sbResultData.Append("\"ResultData\":{\"WONO\":\"" + strWONO + "\",\"UserCode\":\"" + strUserCode + "\"}");
            }
            else
            {
                strReturnCode = "-2";
                strReturnMsg = "删除工单信息失败";
            }
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = "删除工单信息出错,请稍候重试";
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
        log.Error("删除工单信息时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }

    /// <summary>
    /// 设置工单状态
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strUserCode"></param>
    /// <param name="strWONO"></param>
    /// <param name="strStatusFrom"></param>
    /// <param name="strStatusTo"></param>
    /// <param name="strOperateDesc"></param>
    /// <param name="strOperateIdea"></param>
    public void SetWorkOrderStatus(HttpContext context,String strUserCode,String strWONO,String strStatusFrom,String strStatusTo,String strOperateDesc,String strOperateIdea)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();

        StringBuilder sbResult = new StringBuilder();
        try
        {
            Hashtable hsTableParams = new Hashtable();
            hsTableParams.Add("WONO", strWONO);
            hsTableParams.Add("StatusFrom", strStatusFrom);
            hsTableParams.Add("StatusTo", strStatusTo);
            hsTableParams.Add("OperateDesc", strOperateDesc);
            hsTableParams.Add("OperateIdea", strOperateIdea);
            hsTableParams.Add("UserCode", strUserCode);
            int iCount = LWorkOrder.SetWorkOrderStatus(hsTableParams);
            if (iCount > 0)
            {
                strReturnCode = "1";
                strReturnMsg = "设置工单状态成功";
                sbResultData.Append("\"ResultData\":{\"WONO\":\"" + strWONO + "\"");
                sbResultData.Append("   ,\"UserCode\":\"" + strUserCode + "\"");
                sbResultData.Append("   ,\"StatusFrom\":\"" + strStatusFrom + "\"");
                sbResultData.Append("   ,\"StatusTo\":\"" + strStatusTo + "\"");
                sbResultData.Append("   ,\"OperateDesc\":\"" + strOperateDesc + "\"");
                sbResultData.Append("}");
            }
            else
            {
                strReturnCode = "-2";
                strReturnMsg = "设置工单状态失败";
            }
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = "设置工单状态出错,请稍候重试";
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
        log.Error("设置工单状态时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }

    /// <summary>
    /// 根据用户编码获取首页实时数据项目
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strUserCode"></param>
    public void GetIndexRealTimeData(HttpContext context,String strUserCode,String strQueryData)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();

        StringBuilder sbResult = new StringBuilder();
        StringBuilder sbSql = new StringBuilder();
        try
        {
            sbSql.Append("SELECT * FROM [dbo].[Fun_Labor_GetRealTimeData_ByUserCode]('"+strUserCode+"','"+strQueryData+"')");
            DataTable dt = SqlParamDao.GetDataTableBySql(sbSql.ToString());

            strReturnCode = "1";
            strReturnMsg = "根据用户编码获取首页实时数据项目成功";
            sbResultData.Append(WebCommon.GetJsonStringByDataTable(dt, "\"ResultData\"", false));
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = "根据用户编码获取首页实时数据项目出错,请稍候重试";
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
        log.Error("根据用户编码获取首页实时数据项目时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }

    /// <summary>
    /// 选择工单外包工人员名单
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strUserCode"></param>
    /// <param name="strWONO"></param>
    /// <param name="strStatusFrom"></param>
    /// <param name="strStatusTo"></param>
    /// <param name="strOperateDesc"></param>
    /// <param name="strOperateIdea"></param>
    public void SelectWorkOrderLabors(HttpContext context,String strWONO,String LaborUserCode,String strIsSelect,String OpUserCode)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();

        StringBuilder sbResult = new StringBuilder();
        try
        {
            Hashtable hsTableParams = new Hashtable();
            hsTableParams.Add("WONO", strWONO);
            hsTableParams.Add("LaborUserCode", LaborUserCode);
            hsTableParams.Add("IsSelect", strIsSelect);
            hsTableParams.Add("OpUserCode", OpUserCode);
            int iCount = LWorkOrder.SelectWorkOrderLabors(hsTableParams);
            if (iCount > 0)
            {
                strReturnCode = "1";
                strReturnMsg = "选择工单外包工人员名单成功";
                sbResultData.Append("\"ResultData\":{\"WONO\":\"" + strWONO + "\"");
                sbResultData.Append("   ,\"LaborUserCode\":\"" + LaborUserCode + "\"");
                sbResultData.Append("   ,\"IsSelect\":\"" + strIsSelect + "\"");
                sbResultData.Append("   ,\"OpUserCode\":\"" + OpUserCode + "\"");
                sbResultData.Append("}");
            }
            else
            {
                strReturnCode = "-2";
                strReturnMsg = "选择工单外包工人员名单失败";
            }
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = "选择工单外包工人员名单出错,请稍候重试";
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
        log.Error("选择工单外包工人员名单时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }

    /// <summary>
    /// 外包工确认接单或者取消确认
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strWONO"></param>
    /// <param name="LaborUserCode"></param>
    /// <param name="IsToConfirm"></param>
    public void LaborConfirmWorkOrder(HttpContext context,String strWONO,String LaborUserCode,String IsToConfirm)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();

        StringBuilder sbResult = new StringBuilder();
        try
        {
            int iCount = LWorkOrder.LaborConfirmAcceptWorkOrder(strWONO,LaborUserCode,IsToConfirm);
            if (iCount > 0)
            {
                strReturnCode = "1";
                strReturnMsg = IsToConfirm.Equals("1")?"外包工确认接单成功":"外包工取消接单成功";
                sbResultData.Append("\"ResultData\":{\"WONO\":\"" + strWONO + "\"");
                sbResultData.Append("   ,\"LaborUserCode\":\"" + LaborUserCode + "\"");
                sbResultData.Append("}");
            }
            else
            {
                strReturnCode = "-2";
                strReturnMsg = IsToConfirm.Equals("1")?"外包工确认接单失败":"外包工取消接单失败";
                strReturnMsg = "外包工确认接单或者取消确认失败";
            }
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = "外包工确认接单或者取消确认出错,请稍候重试";
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
        log.Error("外包工确认接单或者取消确认时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }

    /// <summary>
    /// 判断当前外包工是否确认接单
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strWONO"></param>
    /// <param name="LaborUserCode"></param>
    public void JudgeLaborIsConfirmWorkOrder(HttpContext context,String strWONO,String LaborUserCode)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();

        StringBuilder sbResult = new StringBuilder();
        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("SELECT count(1) from LWorkOrder_2 where isnull(ConfirmTime,'')<>''");
            sbSql.Append(" and WONO = " + JObjectToDB.GetColumnEncryptDataString("LWorkOrder_1", "WONO", strWONO));
            sbSql.Append(" and UserCode = " + JObjectToDB.GetColumnEncryptDataString("LUser_1", "UserCode", LaborUserCode));
            int iCount = SqlParamDao.ExecuteScalarBySql(sbSql.ToString());
            strReturnCode = "1";
            strReturnMsg = "判断当前外包工是否确认接单成功";
            sbResultData.Append("\"ResultData\":{\"WONO\":\"" + strWONO + "\"");
            sbResultData.Append("   ,\"LaborUserCode\":\"" + LaborUserCode + "\"");
            sbResultData.Append("   ,\"IsConfrim\":\"" + iCount.ToString() + "\"");
            sbResultData.Append("}");
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = "判断当前外包工是否确认接单出错,请稍候重试";
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
        log.Error("判断当前外包工是否确认接单时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }

    /// <summary>
    /// 根据用户编码获取待办工单数量
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strUserCode"></param>
    public void GetWorkOrderPendingQty(HttpContext context,String strUserCode)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();

        StringBuilder sbResult = new StringBuilder();
        try
        {
            int iCount = LWorkOrder.GetWorkOrderPendingQty(strUserCode);
            strReturnCode = "1";
            strReturnMsg = "根据用户编码获取待办工单数量成功";
            sbResultData.Append("\"ResultData\":{\"UserCode\":\"" + strUserCode + "\"");
            sbResultData.Append("   ,\"PendingQty\":\"" + iCount.ToString() + "\"");
            sbResultData.Append("}");
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = "根据用户编码获取待办工单数量出错,请稍候重试";
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
        log.Error("根据用户编码获取待办工单数量时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }

    /// <summary>
    /// 通过用户编号获取针对工单列表中Tab状态分类的分组
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strUserCode"></param>
    public void GetWorkOrderStatusTabs(HttpContext context,String strUserCode)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();

        StringBuilder sbResult = new StringBuilder();
        try
        {
            String strWorkOrderStatusTabs = LWorkOrder.GetWorkOrderStatusTabsJsonData(strUserCode,false);
            sbResultData.Append(strWorkOrderStatusTabs);
            strReturnCode = "1";
            strReturnMsg = "通过用户编号获取针对工单列表中Tab状态分类的分组成功";
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = "通过用户编号获取针对工单列表中Tab状态分类的分组出错,请稍候重试";
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
        log.Error("通过用户编号获取针对工单列表中Tab状态分类的分组时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }

    /// <summary>
    /// 通过外包工用户编码获取当前时间其工单上岗信息列表
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strLaborUserCode"></param>
    /// <param name="strWONO"></param>
    /// <param name="strEmployCompany"></param>
    /// <param name="strWONO"></param>
    /// <param name="strQueryTime"></param>
    /// <param name="strUserCode"></param>
    public void GetLaborOnDutyInfo(HttpContext context,String strLaborUserCode,String strWONO ,String strEmployCompany ,String strServiceCompany ,String strQueryTime ,String strUserCode,String strAttType)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();

        StringBuilder sbResult = new StringBuilder();
        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("SELECT * FROM [dbo].[Fun_Labor_GetLaborDutyInfo]('"+strLaborUserCode+"','"+strWONO+"','"+strEmployCompany+"','"+strServiceCompany+"','"+strAttType+"','"+strQueryTime+"','"+strUserCode+"')");
            log.Error("通过外包工用户编码获取当前时间其工单上岗信息列表:"+sbSql.ToString());
            DataTable dt = SqlParamDao.GetDataTableBySql(sbSql.ToString());

            sbResultData.Append(WebCommon.GetJsonStringByDataTable(dt, "\"ResultData\"", false));
            strReturnCode = "1";
            strReturnMsg = "通过外包工用户编码获取当前时间其工单上岗信息列表成功";
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = "通过外包工用户编码获取当前时间其工单上岗信息列表出错,请稍候重试";
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
        log.Error("通过外包工用户编码获取当前时间其工单上岗信息列表时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }

    /// <summary>
    /// 保存外包工打卡记录
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strLaborUserCode"></param>
    /// <param name="strWONO"></param>
    /// <param name="strAttType"></param>
    /// <param name="strAttTime"></param>
    /// <param name="strUserCode"></param>
    public void SaveLaborAttRecord(HttpContext context,String strLaborUserCode,String strWONO,String strAttType,String strAttTime,String strUserCode)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();

        StringBuilder sbResult = new StringBuilder();
        try
        {
            Hashtable hsTableParams = new Hashtable();
            hsTableParams.Add("LaborCode", strLaborUserCode);
            hsTableParams.Add("WONO", strWONO);
            hsTableParams.Add("AttType", strAttType);
            hsTableParams.Add("AttTime", strAttTime);
            hsTableParams.Add("OpUserCode", strUserCode);
            int iCount = LWorkOrder.SaveLaborAttRecord(hsTableParams);
            if (iCount > 0)
            {
                strReturnCode = "1";
                strReturnMsg = "保存外包工打卡记录成功";
                sbResultData.Append("\"ResultData\":{\"WONO\":\"" + strWONO + "\"");
                sbResultData.Append("   ,\"LaborCode\":\"" + strLaborUserCode + "\"");
                sbResultData.Append("   ,\"AttType\":\"" + strAttType + "\"");
                sbResultData.Append("   ,\"AttTime\":\"" + strAttTime + "\"");
                sbResultData.Append("   ,\"OpUserCode\":\"" + strUserCode + "\"");
                sbResultData.Append("}");
            }
            else
            {
                strReturnCode = "-2";
                strReturnMsg = "保存外包工打卡记录失败";
            }
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = "保存外包工打卡记录出错,请稍候重试";
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
        log.Error("保存外包工打卡记录时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }

    /// <summary>
    /// 通过工单号获取该工单的状态配置列表
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strWONO"></param>
    /// <param name="strUserCode"></param>
    public void GetWorkOrderStatusConfig(HttpContext context,String strWONO,String strUserCode)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();

        StringBuilder sbResult = new StringBuilder();
        StringBuilder sbSql = new StringBuilder();
        try
        {
            DataTable dt = LWorkOrder.GetWorkOrderStatusConfig(strWONO, strUserCode);

            strReturnCode = "1";
            strReturnMsg = "通过工单号获取该工单的状态配置列表成功";
            sbResultData.Append(WebCommon.GetJsonStringByDataTable(dt, "\"ResultData\"", false));
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = "通过工单号获取该工单的状态配置列表出错,请稍候重试";
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
        log.Error("通过工单号获取该工单的状态配置列表时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }

    /// <summary>
    /// 通过工单号获取评价信息
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strUserCode"></param>
    /// <param name="strWONO"></param>
    /// <param name="strBeObjectType"></param>
    /// <param name="strBeObjectCode"></param>
    /// <param name="strDoObjectType"></param>
    /// <param name="strDoObjectCode"></param>
    public void GetWorkOrderAppraiseInfo(HttpContext context,String strUserCode,String strWONO,String strBeObjectType, String strBeObjectCode, String strDoObjectType, String strDoObjectCode)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();

        StringBuilder sbResult = new StringBuilder();
        try
        {
            //改工单评价信息表数据
            DataTable dt = LWorkOrder.GetWorkOrderAppraiseDataTable(strWONO,strBeObjectType, strBeObjectCode, strDoObjectType, strDoObjectCode);

            //该工单评价信息表中的最大SEQNO
            String strSql_MaxSeqNo = "select ISNULL(max(SEQNO),0) FROM LWorkOrder_4 WHERE WONO = " + JObjectToDB.GetColumnEncryptDataString("LWorkOrder_1", "WONO", strWONO);
            int iMaxSEQNO = SqlParamDao.ExecuteScalarBySql(strSql_MaxSeqNo);

            sbResultData.Append(WebCommon.GetJsonStringByDataTable(dt, "\"ResultData\"", false));
            sbResultData.Append(",\"iMaxSEQNO\":\""+iMaxSEQNO.ToString()+"\"");

            strReturnCode = "1";
            strReturnMsg = "通过工单号获取评价信息成功";
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = "通过工单号获取评价信息出错,请稍候重试";
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
        log.Error("通过工单号获取评价信息时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }

    /// <summary>
    /// 通过用户获取其涉及工单的年度月度列表
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strUserCode"></param>
    /// <param name="strGetType"></param>
    public void GetMyYearMonthList(HttpContext context,String strUserCode,String strGetType)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();

        StringBuilder sbResult = new StringBuilder();
        try
        {
            //改工单评价信息表数据
            DataTable dt = LWorkOrder.GetYearMonthDataTable(strUserCode,strGetType);

            sbResultData.Append(WebCommon.GetJsonStringByDataTable(dt, "\"ResultData\"", false));

            strReturnCode = "1";
            strReturnMsg = "通过用户获取其涉及工单的年度月度列表成功";
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = "通过用户获取其涉及工单的年度月度列表出错,请稍候重试";
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
        log.Error("通过用户获取其涉及工单的年度月度列表时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }

    /// <summary>
    /// 根据用户编码获取统计数据
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strUserCode"></param>
    /// <param name="strDateFrom"></param>
    /// <param name="strDateTo"></param>
    /// <param name="strEmployCompany"></param>
    /// <param name="strDeptCode"></param>
    /// <param name="strEmployItem"></param>
    /// <param name="strServiceCompany"></param>
    public void GetMySummaryData(HttpContext context,String strUserCode,String strDateFrom,String strDateTo,String strEmployCompany,String strDeptCode,String strEmployItem,String strServiceCompany)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();

        StringBuilder sbResult = new StringBuilder();
        try
        {
            Hashtable hsTableParams = new Hashtable();
            hsTableParams.Add("UserCode",strUserCode);
            hsTableParams.Add("WorkOrderDateFrom",strDateFrom);
            hsTableParams.Add("WorkOrderDateTo",strDateTo);
            hsTableParams.Add("EmployCompany",strEmployCompany);
            hsTableParams.Add("DeptCode",strDeptCode);
            hsTableParams.Add("EmployItem",strEmployItem);
            hsTableParams.Add("ServiceCompany",strServiceCompany);
            DataTable dt = LWorkOrder.GetWorkOrderSummaryData(strUserCode,hsTableParams);

            sbResultData.Append(WebCommon.GetJsonStringByDataTable(dt, "\"ResultData\"", false));

            strReturnCode = "1";
            strReturnMsg = "根据用户编码获取统计数据成功";
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = "根据用户编码获取统计数据出错,请稍候重试";
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
        log.Error("根据用户编码获取统计数据时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }

    /// <summary>
    /// 保存工单评价信息
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strUserCode"></param>
    /// <param name="strWONO"></param>
    /// <param name="strPostDataObject"></param>
    public void SaveWorkOrderAppraiseData(HttpContext context,String strUserCode ,String strWONO,String strSEQNO,String strPostDataObject)
    {
        log.Error("保存工单评价信息的数据："+strPostDataObject.ToString());
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();

        StringBuilder sbResult = new StringBuilder();
        try
        {
            string strTableName = "LWorkOrder_4";

            //json字符串转json的JObject对象
            JObject jo = (JObject)JsonConvert.DeserializeObject(strPostDataObject);

            Hashtable hsTableKey = new Hashtable();
            hsTableKey.Add("WONO", strWONO);
            hsTableKey.Add("SEQNO", strSEQNO);

            int iCount = 0;
            String strOpType = "";
            //判断这个主键是否存在，如果
            if (JObjectToDB.JudgeRecordIsExists(strTableName, hsTableKey))
            {
                strOpType = "edit";
                iCount = JObjectToDB.UpdateJObjectDataToTable(jo, strTableName, hsTableKey);
            }else
            {
                strOpType = "add";
                iCount = JObjectToDB.InsertJObjectDataToTable(jo, strTableName, hsTableKey);
            }

            if (iCount > 0)
            {
                //保存成功后，继续后续操作
                Hashtable hsTableParams = new Hashtable();
                hsTableParams.Add("WONO",strWONO);
                hsTableParams.Add("SEQNO",strSEQNO);
                hsTableParams.Add("OpType",strOpType);
                hsTableParams.Add("UserCode",strUserCode);
                iCount = LWorkOrder.DoAfterSaveWorkOrderAppraise(hsTableParams);

                strReturnCode = "1";
                strReturnMsg = "保存工单评价信息成功";
                sbResultData.Append("\"ResultData\":{\"WONO\":\"" + strWONO + "\"}");
            }
            else
            {
                strReturnCode = "-2";
                strReturnMsg = "保存工单评价信息失败";
            }
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = "保存工单评价信息出错,请稍候重试";
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
        log.Error("保存工单评价信息时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}