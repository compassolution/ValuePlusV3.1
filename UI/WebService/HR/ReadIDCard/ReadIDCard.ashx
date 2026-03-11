<%@ WebHandler Language="C#" Class="ReadIDCard" %>

using System;
using System.Web;
using System.Text;
using System.Data;
using System.Collections;
using Com.ValuePlus.Web;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Utils.Serializable;
using System.IO;

public class ReadIDCard : HandlerBase, IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    public void ProcessRequest (HttpContext context) {
        Hashtable hsTableUrlQuery = this.GetUrlAnalyse(context);
        string strParam = hsTableUrlQuery["param"] == null ? string.Empty : hsTableUrlQuery["param"].ToString();//param
                
        string strCardNo = hsTableUrlQuery["cardno"] == null ? string.Empty : hsTableUrlQuery["cardno"].ToString();
        string strName = hsTableUrlQuery["name"] == null ? string.Empty : hsTableUrlQuery["name"].ToString();
        string strSex = hsTableUrlQuery["sex"] == null ? string.Empty : hsTableUrlQuery["sex"].ToString();
        string strNation = hsTableUrlQuery["nation"] == null ? string.Empty : hsTableUrlQuery["nation"].ToString();
        string strBirthDate = hsTableUrlQuery["birthdate"] == null ? string.Empty : hsTableUrlQuery["birthdate"].ToString();
        string strAddress = hsTableUrlQuery["address"] == null ? string.Empty : hsTableUrlQuery["address"].ToString();
        string strOrg = hsTableUrlQuery["org"] == null ? string.Empty : hsTableUrlQuery["org"].ToString();
        string strValidata = hsTableUrlQuery["validata"] == null ? string.Empty : hsTableUrlQuery["validata"].ToString();
        string strPhoto = hsTableUrlQuery["photo"] == null ? string.Empty : hsTableUrlQuery["photo"].ToString();

        switch (strParam.ToLower().ToString())
        {
            case "testconnect":
                context.Response.Write(this.TestConnetct().ToString());
                break;
            case "submit":
                context.Response.Write(this.SaveIDCardDetail(strCardNo ,strName ,strSex, strNation, strBirthDate,strAddress,strOrg,strValidata,strPhoto).ToString());
                break;
            case "getlist":
                context.Response.Write(this.GetIDCardList().ToString());
                break;
        }
    }
    
    /// <summary>
    ///测试远程服务链接
    /// </summary>
    private String TestConnetct()
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbReturnData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "测试远程服务链接";
        try
        {
            
            strReturnCode = "1";
            strReturnMsg = strMethodDesc + "成功";
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = strMethodDesc + "出错";
            log.Error(ex);
        }
        finally
        {
            StringBuilder sbResultStatus = new StringBuilder();
            sbResultStatus.Append("{");
            sbResultStatus.Append("\"ReturnCode\":\"" + strReturnCode.ToString() + "\"");
            sbResultStatus.Append(",\"ReturnMsg\":\"" + strReturnMsg.ToString() + "\"");
            
            sbResult.Append(sbResultStatus.ToString());
            if (!String.IsNullOrEmpty(sbReturnData.ToString()))
            {
                sbResult.Append(",\"ReturnData\":\"" + sbReturnData.ToString()+"\"");
            }
            sbResult.Append("}");
            log.Error(strMethodDesc + "Return Json:" + sbResult.ToString());
        }
        return sbResult.ToString();
    }

    /// <summary>
    /// 保存身份证件储存信息
    /// </summary>
    /// <param name="strCardNo">身份证号</param>
    /// <param name="strName">姓名</param>
    /// <param name="strSex">性别</param>
    /// <param name="strNation">民族</param>
    /// <param name="strBirthDate">出生日期</param>
    /// <param name="strAddress">户口地址</param>
    /// <param name="strOrg">发证机关</param>
    /// <param name="strValidata">有效期</param>
    /// <param name="strPhoto">照片</param>
    private String SaveIDCardDetail(String strCardNo,String strName,String strSex,String strNation,String strBirthDate
        ,String strAddress,String strOrg,String strValidata,String strPhoto)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbReturnData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "保存身份证件储存信息";
        String strSql = "";
        try
        {
            String[] strValidataArrary = strValidata.Split('-');
            //String strValidDateF = DateTime.Parse(strValidataArrary[0]).ToString("yyyy-MM-dd");
            //String strValidDateT = DateTime.Parse(strValidataArrary[1]).ToString("yyyy-MM-dd");
            String strValidDateF = "";
            String strValidDateT = "";
            //身份证有效期未必都是日期格式，比如还有汉字永久
            try
            {
                strValidDateF = DateTime.Parse(strValidataArrary[0]).ToString("yyyy-MM-dd");
            }
            catch (Exception ex)
            {
                strValidDateF = strValidataArrary[0].Replace(" ","").Replace("年","").Replace("月","").Replace("日","");
            }
            try
            {
                strValidDateT = DateTime.Parse(strValidataArrary[1]).ToString("yyyy-MM-dd");
            }
            catch (Exception ex)
            {
                strValidDateT = strValidataArrary[1].Replace(" ","").Replace("年","").Replace("月","").Replace("日","");
            }
            if (!String.IsNullOrEmpty(strCardNo))
            {
                StringBuilder sbSql = new StringBuilder();
                sbSql.Append("delete from [TB_IDCARD] where [CARDNO] = '"+strCardNo+"';");
                sbSql.Append("insert into [TB_IDCARD]([CARDNO],[SNAME],[SSEX],[SNATION],[SBIRTHDAY],[SADDRESS],[SORG],[SVALIDDATEF],[SVALIDDATET],[SPHOTO],[INPUTTIME]) values (");
                sbSql.Append(" '"+strCardNo+"','"+strName+"','"+strSex+"','"+strNation+"','"+strBirthDate+"',");
                sbSql.Append(" '"+strAddress+"','"+strOrg+"','"+strValidDateF+"','"+strValidDateT+"','"+strPhoto+"','"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"')");

                strSql = sbSql.ToString();
                int iCount = SqlParamDao.ExecuteNonQueryBySql(strSql);
                if (iCount > 0)
                {
                    strReturnCode = "1";
                    strReturnMsg = strMethodDesc + "成功";
                }
                else
                {
                    strReturnCode = "-1";
                    strReturnMsg = strMethodDesc + "失败";
                }
            }

        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = strMethodDesc + "出错";
            log.Error(ex);
        }
        finally
        {
            StringBuilder sbResultStatus = new StringBuilder();
            sbResultStatus.Append("{");
            sbResultStatus.Append("\"ReturnCode\":\"" + strReturnCode.ToString() + "\"");
            sbResultStatus.Append(",\"ReturnMsg\":\"" + strReturnMsg.ToString() + "\"");
            
            sbResult.Append(sbResultStatus.ToString());
            if (!String.IsNullOrEmpty(sbReturnData.ToString()))
            {
                sbResult.Append(",\"ReturnData\":\"" + sbReturnData.ToString()+"\"");
            }
            sbResult.Append("}");
            log.Error(strMethodDesc + "Return Json:" + sbResult.ToString());
        }
        return sbResult.ToString();
    }

    /// <summary>
    /// 获取已登记的身份证信息列表
    /// </summary>
    /// <param name="context"></param>
    private String GetIDCardList()
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbReturnData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "获取已登记的身份证信息列表";
        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select * from [TB_IDCARD] order by [INPUTTIME] desc");

            DataTable dt = SqlParamDao.GetDataTableBySql(sbSql.ToString());
            //将DataTable转为Byte[]
            DataSet ds = dt.DataSet;
            byte[] db = WinCeDataSetHelper.GetZipBytesByDataSet(ds);
            sbReturnData.Append(Convert.ToBase64String(db));
            
            strReturnCode = "1";
            strReturnMsg = strMethodDesc + "成功";
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = strMethodDesc + "出错";
            log.Error(ex);
        }
        finally
        {
            StringBuilder sbResultStatus = new StringBuilder();
            sbResultStatus.Append("{");
            sbResultStatus.Append("\"ReturnCode\":\"" + strReturnCode.ToString() + "\"");
            sbResultStatus.Append(",\"ReturnMsg\":\"" + strReturnMsg.ToString() + "\"");
            
            sbResult.Append(sbResultStatus.ToString());
            if (!String.IsNullOrEmpty(sbReturnData.ToString()))
            {
                sbResult.Append(",\"ReturnData\":\"" + sbReturnData.ToString()+"\"");
            }
            sbResult.Append("}");
            log.Error(strMethodDesc + "Return Json:" + sbResult.ToString());
        }
        return sbResult.ToString();
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}

