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
using Com.ValuePlus.Common.Security;

public class ReadIDCard : HandlerBase, IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    public void ProcessRequest (HttpContext context) {
        Hashtable hsTableUrlQuery = this.GetUrlAnalyse(context);
        string strParam = hsTableUrlQuery["param"] == null ? string.Empty : hsTableUrlQuery["param"].ToString();//param

        if (strParam.Equals("submit"))
        {
            this.SaveIDCardDetail(context);
        }
        else if (strParam.Equals("getlist"))
        {
            this.GetIDCardList(context);
        }
    }

    /// <summary>
    /// 保存身份证件储存信息
    /// </summary>
    /// <param name="context"></param>
    private void SaveIDCardDetail(HttpContext context)
    {
        String strSql = "";
        try
        {
            String strCardNo = context.Request["txt_CardNo"].ToString();
            String strName = context.Request["txt_Name"].ToString();
            String strSex = context.Request["txt_Sex"].ToString();
            String strNation = context.Request["txt_Nation"].ToString();
            String strBirth = DateTime.Parse(context.Request["txt_BirthDay"].ToString()).ToString("yyyy-MM-dd");
            String strAddress = context.Request["txt_Address"].ToString();
            String strOrg = context.Request["txt_Org"].ToString();
            String strValidata = context.Request["txt_ValidDate"].ToString();
            
            //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
            strCardNo = SQLInjectionDefense.ReplaceSQLReservedKeyword(strCardNo);
            strName = SQLInjectionDefense.ReplaceSQLReservedKeyword(strName);
            strSex = SQLInjectionDefense.ReplaceSQLReservedKeyword(strSex);
            strNation = SQLInjectionDefense.ReplaceSQLReservedKeyword(strNation);
            strBirth = SQLInjectionDefense.ReplaceSQLReservedKeyword(strBirth);
            strAddress = SQLInjectionDefense.ReplaceSQLReservedKeyword(strAddress);
            strOrg = SQLInjectionDefense.ReplaceSQLReservedKeyword(strOrg);
            strValidata = SQLInjectionDefense.ReplaceSQLReservedKeyword(strValidata);

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

            String strPhoto = context.Request["txt_Photo"].ToString();

            if (!String.IsNullOrEmpty(strCardNo))
            {
                StringBuilder sbSql = new StringBuilder();
                sbSql.Append("delete from [TB_IDCARD] where [CARDNO] = '"+strCardNo+"';");
                sbSql.Append("insert into [TB_IDCARD]([CARDNO],[SNAME],[SSEX],[SNATION],[SBIRTHDAY],[SADDRESS],[SORG],[SVALIDDATEF],[SVALIDDATET],[SPHOTO],[INPUTTIME]) values (");
                sbSql.Append(" '"+strCardNo+"','"+strName+"','"+strSex+"','"+strNation+"','"+strBirth+"',");
                sbSql.Append(" '"+strAddress+"','"+strOrg+"','"+strValidDateF+"','"+strValidDateT+"','"+strPhoto+"','"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"')");

                strSql = sbSql.ToString();
                int iResult = SqlParamDao.ExecuteNonQueryBySql(strSql);
                if (iResult >= 1)
                {
                    context.Response.Write("success");
                    return;
                }else
                {
                    context.Response.Write("error");
                    return;
                }
            }

        }
        catch (Exception ex)
        {
            context.Response.Write("error");
            log.Error(ex);
            log.Error("error Sql:" + strSql);
        }
    }

    /// <summary>
    /// 获取可更新的更新列表
    /// </summary>
    /// <param name="context"></param>
    private void GetIDCardList(HttpContext context)
    {
        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select * from [TB_IDCARD] order by [INPUTTIME] desc");

            DataTable dt = SqlParamDao.GetDataTableBySql(sbSql.ToString());
            int iColCount = dt.Columns.Count;
            StringBuilder sBuilder = new StringBuilder();
            sBuilder.Append("{ResultData:[ ");
            if ((dt != null) && (dt.Rows.Count > 0))
            {
                //sBuilder.Append("   " + this.ulHeaderMenu.ClientID + ".innerHTML =\"\r\n");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i > 0)
                    {
                        sBuilder.Append(",{");
                    }
                    else
                    {
                        sBuilder.Append("{");
                    }
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        String strColName = dt.Columns[j].ColumnName;
                        //对值进行编码处理特殊字符，如引号等
                        String strColValue = Microsoft.JScript.GlobalObject.escape(dt.Rows[i][dt.Columns[j].ColumnName].ToString());
                        //String strColValue = dt.Rows[i][dt.Columns[j].ColumnName].ToString();
                        if (j == 0)
                        {
                            sBuilder.Append(strColName + ":'" + strColValue + "'");
                        }
                        else
                        {
                            sBuilder.Append("," + strColName + ":'" + strColValue + "'");
                        }
                    }

                    sBuilder.Append("}");
                }

            }
            sBuilder.Append("]");
            sBuilder.Append("}");

            string json = sBuilder.ToString();

            context.Response.Write(json);

        }
        catch (Exception ex)
        {
            log.Error(ex);
            context.Response.Write("error");
        }
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}

