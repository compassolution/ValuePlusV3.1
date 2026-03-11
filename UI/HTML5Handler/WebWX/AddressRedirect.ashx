<%@ WebHandler Language="C#" Class="AddressRedirect" %>

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

public class AddressRedirect : HandlerBase, IHttpHandler, System.Web.SessionState.IRequiresSessionState {

    public void ProcessRequest (HttpContext context) {
        //解析客户端传递过来的json data
        StreamReader reader = new StreamReader(context.Request.InputStream);
        String strParamJson = HttpUtility.UrlDecode(reader.ReadToEnd());
        log.Error("传入参数的Json data字符串:" + strParamJson.ToString());

        string strParam = WebCommon.GetJsonValue(strParamJson,"param").ToString();//请求类型参数
        string strLanguage = WebCommon.GetJsonValue(strParamJson,"language").ToString();
        string strMobileNo = WebCommon.GetJsonValue(strParamJson,"mobileno").ToString();
        string strProduct = WebCommon.GetJsonValue(strParamJson,"product").ToString();

        log.Error("AddressRedirect.ashx,MobileNo:"+strMobileNo+";Language:"+strLanguage);
        if (strParam.Equals("getmobileaddressbymobileno"))
        {
            //根据手机号码获取其当前所属单位以及其对应手机端应用地址
            context.Response.Write(this.GetMobileAddressByMobileNo(strMobileNo,strProduct,strLanguage));
        }

    }

    /// <summary>
    /// 根据手机号码获取其当前所属单位以及其对应手机端应用地址
    /// </summary>
    /// <param name="strMobileNo"></param>
    /// <param name="strProduct"></param>
    /// <param name="strProduct"></param>
    private String GetMobileAddressByMobileNo(String strMobileNo,String strProduct,String strLanguage)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "根据手机号码获取其当前所属单位以及其对应手机端应用地址";
        try
        {
            //首先获取手机号对应的ProjectId
            String strProjectId = "";
            String strMobileAddress = "";
            String strSql_ProjectId = "select TOP 1 ProjectId from TB_Remote_StaffBasicData where DCMOBILE = '"+strMobileNo+"' AND DCSTATUS <> '3' ORDER BY DCJOIN DESC";
            DataTable dt_ProjectId = SqlParamDao.GetDataTableBySql(strSql_ProjectId);
            if( dt_ProjectId!=null && dt_ProjectId.Rows.Count==1){
                strProjectId = dt_ProjectId.Rows[0]["ProjectId"].ToString();

                //再通过ProjectId获取手机端地址
                String strSql_Address = "SELECT top 1 ISNULL(MOBILEADD,'') AS MobileAdd FROM WEBADD_1 where ProjectId = '"+strProjectId+"'";
                DataTable dt_Address = SqlParamDao.GetDataTableBySql(strSql_Address);
                if( dt_Address != null && dt_Address.Rows.Count == 1 && !String.IsNullOrEmpty(dt_Address.Rows[0]["MobileAdd"].ToString())){
                    strMobileAddress = dt_Address.Rows[0]["MobileAdd"].ToString();

                    strReturnCode = "1";
                    strReturnMsg = strMethodDesc+"成功";
                }else{
                    strReturnCode = "-29";
                    strReturnMsg = "当前手机号"+strMobileNo+"无法跳转，请确保服务授权是否有效!";
                }
            }else{
                strReturnCode = "-19";
                strReturnMsg = "当前手机号"+strMobileNo+"无效或与人事档案库不匹配!";
            }
            sbResultData.Append("\"ResultData\":{");
            sbResultData.Append("\"projectId\":\""+strProjectId+"\"");
            sbResultData.Append(",\"mobileAddress\":\""+strMobileAddress+"\"");
            sbResultData.Append("}");
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


    public bool IsReusable {
        get {
            return false;
        }
    }

}