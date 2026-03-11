<%@ WebHandler Language="C#" Class="UserHandler" %>

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
using Com.ValuePlus.Weixin;
using Com.ValuePlus.Labor;
using Com.ValuePlus.Web;

public class UserHandler : HandlerBase,IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    public void ProcessRequest (HttpContext context) {
        //解析客户端传递过来的json data
        StreamReader reader = new StreamReader(context.Request.InputStream);
        String strParamJson = HttpUtility.UrlDecode(reader.ReadToEnd());
        log.Error("传入参数的Json data字符串:" + strParamJson.ToString());

        string param = WebCommon.GetJsonObjectValue(strParamJson,"param").ToString();//请求类型参数
        string strMobileNo = WebCommon.GetJsonValue(strParamJson,"mobileno").ToString();//登录名
        string strPassword = WebCommon.GetJsonValue(strParamJson,"password").ToString();//密码
        string strUserCode = WebCommon.GetJsonValue(strParamJson,"usercode").ToString();//用户名
        string strNickName = WebCommon.GetJsonValue(strParamJson,"nickname").ToString();//昵称
        string strIsAdministrator = WebCommon.GetJsonValue(strParamJson,"isadministrator").ToString();//是否管理员
        string strUserType = WebCommon.GetJsonValue(strParamJson,"usertype").ToString();//用户类型
        string strUserRole = WebCommon.GetJsonValue(strParamJson,"userrole").ToString();//用户角色
        string strCompanyCode = WebCommon.GetJsonValue(strParamJson,"companycode").ToString();//公司编码
        string strPostDataObject = WebCommon.GetJsonObjectValue(strParamJson,"postdataobj").ToString();//界面传递过来的json数据
        string strPostWeixinUserInfo = WebCommon.GetJsonObjectValue(strParamJson,"weixinuserobj").ToString();//界面传递过来获取到微信用户信息的json数据

        string strConditionSql = WebCommon.GetJsonObjectValue(strParamJson,"conditionsql").ToString();//查询条件语句
        string strIsValid = WebCommon.GetJsonValue(strParamJson,"isvalid").ToString();//是否有效
        string strAimUserType = WebCommon.GetJsonValue(strParamJson,"aimusertype").ToString();//目标用户类型
        int iTopRows = String.IsNullOrEmpty(WebCommon.GetJsonValue(strParamJson,"toprows").ToString())?0:int.Parse(WebCommon.GetJsonValue(strParamJson,"toprows").ToString());//获取前多少条记录
        string strOrderBy = WebCommon.GetJsonObjectValue(strParamJson,"orderby").ToString();//排序

        string strLaborUserCode = WebCommon.GetJsonValue(strParamJson,"laborusercode").ToString();//外包工用户名
        string strOutsourcedCode = WebCommon.GetJsonValue(strParamJson,"outsourcedcode").ToString();//外包公司编码
        string strIsDock = WebCommon.GetJsonValue(strParamJson,"isdock").ToString();//挂靠或者是取消挂靠

        string strQueryUserCode = WebCommon.GetJsonObjectValue(strParamJson,"queryusercode").ToString();//查询用户编码
        string strEmployCompanyCode = WebCommon.GetJsonObjectValue(strParamJson,"employcompanycode").ToString();//用人单位编码
        string strServiceCompanyCode = WebCommon.GetJsonObjectValue(strParamJson,"servicecompanycode").ToString();//外包公司编码
        string strQueryTimeFrom = WebCommon.GetJsonObjectValue(strParamJson,"timefrom").ToString();//查询时间从
        string strQueryTimeTo = WebCommon.GetJsonObjectValue(strParamJson,"timeto").ToString();//查询时间到
        string strWONO = WebCommon.GetJsonObjectValue(strParamJson,"wono").ToString();//WONO
        string strAttType = WebCommon.GetJsonObjectValue(strParamJson,"atttype").ToString();//attType
        string strAttLocation = WebCommon.GetJsonObjectValue(strParamJson,"attlocation").ToString();//AttLocation
        string strIsFocus = WebCommon.GetJsonValue(strParamJson,"isfocus").ToString();//是否是关注

        string strOpenId = WebCommon.GetJsonValue(strParamJson,"openid").ToString();//openid

        StringBuilder sbImportParam = new StringBuilder();
        sbImportParam.Append("param:" + param);
        sbImportParam.Append(",strMobileNo:" + strMobileNo);
        sbImportParam.Append(",strPassword:" + strPassword);
        sbImportParam.Append(",strUserCode:" + strUserCode);
        sbImportParam.Append(",strUserType:" + strUserType);
        sbImportParam.Append(",strCompanyCode:" + strCompanyCode);
        sbImportParam.Append(",strPostDataObject:" + strPostDataObject);
        log.Error("Labor/MPHandler/UserHandler.ashx传入参数:" + sbImportParam.ToString());

        switch (param.ToLower().ToString())
        {
            case "reguser":
                this.RegistUser(context, strMobileNo, strPassword,strUserType,strUserRole,strIsAdministrator,strPostWeixinUserInfo);
                break;
            case "updateuser":
                this.UpdateUserInfo(context,strUserCode,strUserType, strPostDataObject);
                break;
            case "bindopenidtomobileno":
                this.BindOpenIdToMobileNo(context, strOpenId,strMobileNo,strPostWeixinUserInfo);
                break;
            case "bindopenidtouserinfo":
                this.BindOpenIdToUserCode(context, strOpenId,strUserCode,strPostWeixinUserInfo);
                break;
            case "unbindopenid":
                this.UnBindOpenId(context, strUserCode);
                break;
            case "getuserinfo":
                this.GetUserInfo(context,strUserCode, strMobileNo, strNickName);
                break;
            case "getuserinfobycondition":
                this.GetUserInfoByCondition(context,strUserCode, strConditionSql, iTopRows,strOrderBy);
                break;
            case "getmypartneruserlist":
                this.GetMyPartnerUserList(context,strUserCode,strAimUserType,strIsValid,strConditionSql,iTopRows,strOrderBy);
                break;
            case "getusercodebymobileno":
                this.GetUserCodeByMobileNo(context,strMobileNo);
                break;
            case "getmypartnerusercodearray":
                //仅返回用户编码数组
                this.GetMyPartnerUserCodeArray(context,strUserCode,strAimUserType,strIsValid,strConditionSql,iTopRows,strOrderBy);
                break;
            case "getlaborattdatalist":
                this.GetLaborAttDataList(context,strLaborUserCode,strWONO,strQueryTimeFrom,strQueryTimeTo,strAttType,strAttLocation
                    ,strEmployCompanyCode,strServiceCompanyCode,iTopRows,strOrderBy,strQueryUserCode);
                break;
            case "labordockoutsourced":
                this.LaborDockOutsourced(context, strLaborUserCode, strCompanyCode,strIsDock,strUserCode);
                break;
            case "focuslabor":
                this.FocusLabor(context, strUserCode, strLaborUserCode,strIsFocus);
                break;
        }
    }

    /// <summary>
    /// 用人单位添加关注/取消关注某一外包工
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strUserCode"></param>
    /// <param name="strLaborCode"></param>
    /// <param name="strIsValid"></param>
    public void FocusLabor(HttpContext context,String strUserCode,String strLaborCode, String strIsFocus)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();

        StringBuilder sbResult = new StringBuilder();
        try
        {
            Hashtable hsTableParams = new Hashtable();
            hsTableParams.Add("UserCode", strUserCode);//当前用户
            hsTableParams.Add("BeObjectType", "3");//被关注对象类型
            hsTableParams.Add("BeObjectCode", strLaborCode);//被关注对象编码
            hsTableParams.Add("FocusType", "2");//关注类型（1、合作/2主动关注）
            hsTableParams.Add("IsFocus", strIsFocus);//是否是关注（1、关注，0,、取消关注）

            int iCount = LUser.DoFocusObject(hsTableParams);
            if (iCount > 0)
            {
                strReturnCode = "1";
                strReturnMsg = "用人单位添加关注/取消关注某一外包工成功";
                sbResultData.Append("\"ResultData\":{\"UserCode\":\"" + strUserCode + "\"");
                sbResultData.Append("   ,\"CompanyCode\":\"" + strLaborCode + "\"");
                sbResultData.Append("   ,\"IsFocus\":\"" + strIsFocus + "\"");
                sbResultData.Append("}");
            }
            else
            {
                strReturnCode = "-2";
                strReturnMsg = "用人单位添加关注/取消关注某一外包工失败";
            }
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = "用人单位添加关注/取消关注某一外包工出错,请稍候重试";
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
        log.Error("用人单位添加关注/取消关注某一外包工时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }


    /// <summary>
    /// 外包工挂靠或者取消挂靠到某一外包公司
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strLaborUserCode"></param>
    /// <param name="strCompanyCode"></param>
    /// <param name="strIsDock"></param>
    /// <param name="strUserCode"></param>
    public void LaborDockOutsourced(HttpContext context,String strLaborUserCode,String strCompanyCode, String strIsDock,String strUserCode)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();

        StringBuilder sbResult = new StringBuilder();
        try
        {
            /// <param name="strUserCode">用人单位编码</param>
            /// <param name="strCompanyCode">外包公司编码</param>
            /// <param name="IsDock">是否是挂靠（1、挂靠，0,、取消挂靠）</param>
            Hashtable hsTableParams = new Hashtable();
            hsTableParams.Add("LaborUserCode", strLaborUserCode);
            hsTableParams.Add("OutsourcedCompanyCode", strCompanyCode);
            hsTableParams.Add("IsDock", strIsDock);
            hsTableParams.Add("OpUserCode", strUserCode);

            int iCount = LCompany.DoLaborDockOutsourced(hsTableParams);
            if (iCount > 0)
            {
                strReturnCode = "1";
                strReturnMsg = "外包工挂靠或者取消挂靠到某一外包公司成功";
                sbResultData.Append("\"ResultData\":{\"LaborUserCode\":\"" + strLaborUserCode + "\"");
                sbResultData.Append("   ,\"OutsourcedCompanyCode\":\"" + strCompanyCode + "\"");
                sbResultData.Append("   ,\"IsDock\":\"" + strIsDock + "\"");
                sbResultData.Append("   ,\"OpUserCode\":\"" + strUserCode + "\"");
                sbResultData.Append("}");
            }
            else
            {
                strReturnCode = "-2";
                strReturnMsg = "外包工挂靠或者取消挂靠到某一外包公司失败";
            }
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = "外包工挂靠或者取消挂靠到某一外包公司出错,请稍候重试";
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
        log.Error("外包工挂靠或者取消挂靠到某一外包公司时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }

    /// <summary>
    /// 加载伙伴外包工用户数组,仅返回用户编码数组
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strUserCode"></param>
    /// <param name="strAimUserType"></param>
    /// <param name="strIsValid"></param>
    /// <param name="strCondition"></param>
    /// <param name="iTopRows"></param>
    /// <param name="strOrderBy"></param>
    public void GetMyPartnerUserCodeArray(HttpContext context,String strUserCode,String strAimUserType,String strIsValid,String strCondition,int iTopRows,String strOrderBy)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();

        StringBuilder sbResult = new StringBuilder();
        try
        {
            sbResultData.Append("\"ResultData\":[");
            DataTable dt = LUser.GetMyPartnerUserDataTable(strUserCode,strAimUserType,strIsValid,strCondition,iTopRows,strOrderBy);
            if (dt != null & dt.Rows.Count > 0)
            {
                for(int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    String strLaborCode = dr["UserCode"].ToString();
                    if (i == 0)
                    {
                        sbResultData.Append("{\"LaborCode\":\"" + strLaborCode + "\"}");
                    }else
                    {
                        sbResultData.Append(",{\"LaborCode\":\"" + strLaborCode + "\"}");
                    }
                }
            }
            sbResultData.Append("]");

            strReturnCode = "1";
            strReturnMsg = "加载伙伴外包工用户数组,仅返回用户编码数组成功";
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = "加载伙伴外包工用户数组,仅返回用户编码数组出错,请稍候重试";
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
        log.Error("加载伙伴外包工用户数组,仅返回用户编码数组时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }

    /// <summary>
    /// 根据手机号码获取用户编码 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strUserCode"></param>
    /// <param name="strMobileNo"></param>
    /// <param name="strNickName"></param>
    public void GetUserCodeByMobileNo(HttpContext context,String strMobileNo)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();

        StringBuilder sbResult = new StringBuilder();
        try
        {
            String strUserCode = LUser.GetUserCodeByMobileNo(strMobileNo);

            if (String.IsNullOrEmpty(strUserCode))
            {
                strReturnCode = "-1";
                strReturnMsg = "该手机号码尚未注册!";
            }else
            {
                strReturnCode = "1";
                strReturnMsg = "根据手机号码获取用户编码成功";
            }
            sbResultData.Append("\"ResultData\":\""+strUserCode+"\"");
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = "根据手机号码获取用户编码出错,请稍候重试";
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
        log.Error("根据手机号码获取用户编码时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }

    /// <summary>
    /// 保存用户信息
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strPostDataObject"></param>
    /// <param name="strUserCode"></param>
    /// <param name="strUserType"></param>
    public void UpdateUserInfo(HttpContext context,String strUserCode,String strUserType,String strPostDataObject)
    {
        log.Error("保存用户信息的数据："+strPostDataObject.ToString());
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();

        StringBuilder sbResult = new StringBuilder();
        try
        {
            string strTableName = "LUser_1";
            //JObject jo = JObject.Parse(strPostDataObject);
            //json字符串转json的JObject对象
            JObject jo = (JObject)JsonConvert.DeserializeObject(strPostDataObject);

            Hashtable hsTableKey = new Hashtable();
            hsTableKey.Add("UserCode", strUserCode);

            int iCount = JObjectToDB.UpdateJObjectDataToTable(jo, strTableName, hsTableKey);

            if (iCount > 0)
            {
                strReturnCode = "1";
                strReturnMsg = "保存用户信息成功";
                //sbResultData.Append("\"ResultData\":{\"UserCode\":\""+strUserCode+"\"}");
            }else
            {
                strReturnCode = "-2";
                strReturnMsg = "保存用户信息失败";
            }
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = "保存用户信息出错,请稍候重试";
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
        log.Error("保存用户信息时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }

    /// <summary>
    /// 注册用户
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strMobileNo"></param>
    /// <param name="strPassword"></param>
    /// <param name="strUserType"></param>
    /// <param name="strIsAdministrator"></param>
    /// <param name="strPostWeixinUserInfo"></param>
    public void RegistUser(HttpContext context, String strMobileNo, String strPassword,String strUserType,String strUserRole,String strIsAdministrator,String strPostWeixinUserInfo)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        String strNow = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        StringBuilder sbResult = new StringBuilder();
        try
        {
            if (!String.IsNullOrEmpty(LUser.GetUserCodeByMobileNo(strMobileNo)))
            {
                strReturnCode = "-1";
                strReturnMsg = "该手机号已注册";
            }else
            {
                String strUserCode = LUser.GenerateUserCode(strUserType);
                String strOpenId = "";
                String strUnionId = "";
                String strAvatarUrl = "";

                //如果传入了微信用户信息，保存微信用户信息到WXUser并返回实体类
                EntityWXUser entityWXUser = this.SaveWXUserAndReturnEntity(strPostWeixinUserInfo);
                if (entityWXUser != null&&!String.IsNullOrEmpty(entityWXUser.OpenId))
                {
                    strOpenId = entityWXUser.OpenId;
                    strUnionId = entityWXUser.Unionid;
                    strAvatarUrl = entityWXUser.HeadImgUrl;
                }

                //插入LUser_1用户表一条记录
                int iCount = LUser.InsertOneRegUser(strUserCode,strMobileNo,strPassword,strUserType,strUserRole,strIsAdministrator,"","","","",strUserCode);
                if (iCount > 0)
                {
                    if (!String.IsNullOrEmpty(strOpenId))
                    {
                        //更新用户的微信信息
                        int iUpdateCount = LUser.UpdateUserWenxinInfo(strUserCode,strOpenId,strUnionId,strAvatarUrl);
                    }

                    //保存用户信息后的后续操作
                    Hashtable hsTableParams = new Hashtable();
                    hsTableParams.Add("RegUserCode",strUserCode);
                    hsTableParams.Add("OpUserCode",strUserCode);
                    LUser.DoAfterRegUser(hsTableParams);

                    strReturnCode = "1";
                    strReturnMsg = "注册成功，登录账号为:"+strUserCode;
                    sbResultData.Append("\"ResultData\":{\"UserCode\":\""+strUserCode+"\"}");
                }else
                {
                    strReturnCode = "-2";
                    strReturnMsg = "注册失败,请稍候重试";
                }
            }
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = "注册时服务器端出错,请稍候重试";
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
        log.Error("注册时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }


    /// <summary>
    /// 根据用户编码获取用户信息
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strUserCode"></param>
    /// <param name="strMobileNo"></param>
    /// <param name="strNickName"></param>
    public void GetUserInfo(HttpContext context,String strUserCode,String strMobileNo,String strNickName)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();

        StringBuilder sbResult = new StringBuilder();
        try
        {
            DataTable dt = LUser.GetUserInfoDataTable(strUserCode,strMobileNo,strNickName,"");

            strReturnCode = "1";
            strReturnMsg = "获取用户信息成功";
            sbResultData.Append(WebCommon.GetJsonStringByDataTable(dt,"\"ResultData\"",false));
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = "获取用户信息出错,请稍候重试";
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
        log.Error("根据用户编码获取用户信息时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }

    /// <summary>
    /// 根据用户编码获取我的伙伴用户列表
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strUserCode"></param>
    /// <param name="strAimUserType">目标公司对象类型</param>
    /// <param name="strIsValid"></param>
    /// <param name="strCondition"></param>
    /// <param name="iTopRows"></param>
    /// <param name="strOrderBy"></param>
    public void GetMyPartnerUserList(HttpContext context,String strUserCode,String strAimUserType,String strIsValid,String strCondition,int iTopRows,String strOrderBy)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();

        StringBuilder sbResult = new StringBuilder();
        try
        {
            DataTable dt = LUser.GetMyPartnerUserDataTable(strUserCode,strAimUserType,strIsValid,strCondition,iTopRows,strOrderBy);

            strReturnCode = "1";
            strReturnMsg = "根据用户编码获取我的伙伴用户列表成功";
            sbResultData.Append(WebCommon.GetJsonStringByDataTable(dt,"\"ResultData\"",false));
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = "根据用户编码获取我的伙伴用户列表出错,请稍候重试";
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
        log.Error("根据用户编码获取我的伙伴用户列表时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }

    /// <summary>
    /// 通过用户编码及时间范围等条件获取打卡记录
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strLaborUserCode"></param>
    /// <param name="strWONO"></param>
    /// <param name="strQueryTimeFrom"></param>
    /// <param name="strQueryTimeTo"></param>
    /// <param name="strAttType"></param>
    /// <param name="strAttLocation"></param>
    /// <param name="strEmployCompanyCode"></param>
    /// <param name="strServiceCompanyCode"></param>
    /// <param name="iTopRows"></param>
    /// <param name="strOrderBy"></param>
    /// <param name="strQueryUserCode"></param>
    public void GetLaborAttDataList(HttpContext context,String strLaborUserCode,String strWONO,String strQueryTimeFrom,String strQueryTimeTo
        ,String strAttType,String strAttLocation,String strEmployCompanyCode,String strServiceCompanyCode,int iTopRows,String strOrderBy,String strQueryUserCode)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();

        StringBuilder sbResult = new StringBuilder();
        try
        {
            DataTable dt = LUser.GetLaborAttDataTable(strLaborUserCode,strWONO,strQueryTimeFrom,strQueryTimeTo,strAttType,strAttLocation
                ,strEmployCompanyCode,strServiceCompanyCode,iTopRows,strOrderBy,strQueryUserCode);

            strReturnCode = "1";
            strReturnMsg = "通过用户编码及时间范围等条件获取打卡记录成功";
            sbResultData.Append(WebCommon.GetJsonStringByDataTable(dt,"\"ResultData\"",false));
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = "通过用户编码及时间范围等条件获取打卡记录出错,请稍候重试";
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
        log.Error("通过用户编码及时间范围等条件获取打卡记录时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }

    /// <summary>
    /// 根据条件获取用户列表
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strUserCode"></param>
    /// <param name="strCondition"></param>
    /// <param name="iTopRows"></param>
    /// <param name="strOrderBy"></param>
    public void GetUserInfoByCondition(HttpContext context,String strUserCode,String strCondition,int iTopRows,String strOrderBy)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();

        StringBuilder sbResult = new StringBuilder();
        try
        {
            DataTable dt = LUser.GetUserInfoDataTable(strUserCode,strCondition,iTopRows,strOrderBy);

            strReturnCode = "1";
            strReturnMsg = "根据条件获取用户列表成功";
            sbResultData.Append(WebCommon.GetJsonStringByDataTable(dt,"\"ResultData\"",false));
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = "根据条件获取用户列表出错,请稍候重试";
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
        log.Error("根据条件获取用户列表时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }

    /// <summary>
    /// 保存微信用户信息到WXUser并返回实体类
    /// </summary>
    /// <param name="strPostWeixinUserInfo"></param>
    /// <returns></returns>
    private EntityWXUser SaveWXUserAndReturnEntity(String strPostWeixinUserInfo)
    {
        EntityWXUser entityWXUser = new EntityWXUser();
        String strNow = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        if (!String.IsNullOrEmpty(strPostWeixinUserInfo)&&!strPostWeixinUserInfo.Equals("[]"))
        {
            //json字符串转json的JObject对象
            JObject jo = (JObject)JsonConvert.DeserializeObject(strPostWeixinUserInfo);
            try
            {
                foreach (var item in jo)
                {
                    string strColumnName = item.Key;
                    string strColumnValue = item.Value.ToString();
                    switch (strColumnName)
                    {
                        case "openId" : entityWXUser.OpenId = strColumnValue; break;
                        case "nickName" : entityWXUser.NickName = strColumnValue; break;
                        case "gender" : entityWXUser.Sex = strColumnValue; break;
                        case "province" : entityWXUser.Province = strColumnValue; break;
                        case "city" : entityWXUser.City = strColumnValue; break;
                        case "country" : entityWXUser.Country = strColumnValue; break;
                        case "avatarUrl" : entityWXUser.HeadImgUrl = strColumnValue;  break;
                        case "privilege" : entityWXUser.Privilege = strColumnValue; break;
                        case "unionid" : entityWXUser.Unionid = strColumnValue; break;
                        case "MobileNo" : entityWXUser.MobileNo = strColumnValue; break;
                    }
                }
                entityWXUser.Password = "123";
                entityWXUser.SMSCount = "0";
                entityWXUser.RegTime = strNow;
                entityWXUser.LastTime = strNow;
                //写入管理平台微信用户表
                WXUser.RecordWXUser_1(entityWXUser);
            }catch(Exception ex)
            {
                log.Error("Labor 保存微信用户信息到WXUser并返回实体类出错："+ex.ToString());
            }
        }
        return entityWXUser;
    }


    /// <summary>
    /// 绑定微信到手机号码
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strOpenId"></param>
    /// <param name="strMobileNo"></param>
    /// <param name="strPostWeixinUserInfo"></param>
    public void BindOpenIdToMobileNo(HttpContext context,String strOpenId,String strMobileNo,String strPostWeixinUserInfo)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();

        StringBuilder sbResult = new StringBuilder();
        try
        {
            //如果传入了微信用户信息，保存微信用户信息到WXUser并返回实体类
            EntityWXUser entityWXUser = this.SaveWXUserAndReturnEntity(strPostWeixinUserInfo);

            int iCount = LUser.UpdateOpenIdByMobileNo(strOpenId,strMobileNo);
            log.Error("绑定微信"+strOpenId+"到手机号码"+strMobileNo+"的SQL语句更新记录数:"+iCount.ToString());
            if (iCount > 0)
            {
                sbResultData.Append("\"ResultData\":{\"OpenId\":\"" + strOpenId + "\"");
                sbResultData.Append("   ,\"MobileNo\":\"" + strMobileNo + "\"");
                sbResultData.Append("}");
                strReturnCode = "1";
                strReturnMsg = "绑定微信到手机号码"+strMobileNo+"成功";
            }
            else
            {
                strReturnCode = "-2";
                strReturnMsg = "绑定微信到手机号码"+strMobileNo+"失败";
            }
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = "绑定微信"+strOpenId+"到手机号码"+strMobileNo+"出错,请稍候重试";
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
        log.Error("绑定微信"+strOpenId+"到手机号码"+strMobileNo+"时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }

    /// <summary>
    /// 绑定微信号到当前账号
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strOpenId"></param>
    /// <param name="strUserCode"></param>
    /// <param name="strPostWeixinUserInfo"></param>
    public void BindOpenIdToUserCode (HttpContext context,String strOpenId,String strUserCode,String strPostWeixinUserInfo)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();

        StringBuilder sbResult = new StringBuilder();
        try
        {
            //如果传入了微信用户信息，保存微信用户信息到WXUser并返回实体类
            EntityWXUser entityWXUser = this.SaveWXUserAndReturnEntity(strPostWeixinUserInfo);

            String strUnionId = "";
            String strAvatarUrl = "";
            if (entityWXUser != null&&!String.IsNullOrEmpty(entityWXUser.OpenId))
            {
                strOpenId = entityWXUser.OpenId;
                strUnionId = entityWXUser.Unionid;
                strAvatarUrl = entityWXUser.HeadImgUrl;
            }
            //更新用户的微信信息
            int iUpdateCount = LUser.UpdateUserWenxinInfo(strUserCode,strOpenId,strUnionId,strAvatarUrl);
            log.Error("绑定微信"+strOpenId+"到绑定微信号到当前账号"+strUserCode);
            if (iUpdateCount > 0)
            {
                sbResultData.Append("\"ResultData\":{\"OpenId\":\"" + strOpenId + "\"");
                sbResultData.Append("   ,\"MobileNo\":\"" + strUserCode + "\"");
                sbResultData.Append("}");
                strReturnCode = "1";
                strReturnMsg = "绑定微信到当前账号"+strUserCode+"成功";
            }
            else
            {
                strReturnCode = "-2";
                strReturnMsg = "绑定微信到当前账号"+strUserCode+"失败";
            }
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = "绑定微信"+strOpenId+"到当前账号"+strUserCode+"出错,请稍候重试";
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
        log.Error("绑定微信"+strOpenId+"到当前账号"+strUserCode+"时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }

    /// <summary>
    /// 解绑微信号
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strUserCode"></param>
    public void UnBindOpenId (HttpContext context,String strUserCode)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();

        StringBuilder sbResult = new StringBuilder();
        try
        {
            //更新用户的微信信息
            int iUpdateCount = LUser.UpdateUserWenxinInfo(strUserCode,null,null,null);

            strReturnCode = "1";
            strReturnMsg = "解绑微信号成功";
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = "解绑微信号出错,请稍候重试";
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
        log.Error("解绑微信号时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}