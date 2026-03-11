using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Com.ValuePlus.BLL.SysManager;
using Com.ValuePlus.Web;
using System.Resources;
using Com.ValuePlus.Common.Config;
using System.Text;
using Com.ValuePlus.Common.Security;

public partial class actionPath : PageBase
{
    private String strMenuId;
    private int iCount;
    private String strCaptionColName;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!base.IsPostBack)
        {
            strMenuId = "";
            Hashtable hsTable = new Hashtable();
            iCount = 0;
            if (base.Language.Equals("zh-cn"))
            {
                strCaptionColName = "SMENUNAMECN";
            }
            else if (base.Language.Equals("en-us"))
            {
                strCaptionColName = "SMENUNAME";
            }

            if (Request.Params["menuId"] != null)
            {
                strMenuId = Request.Params["menuId"].ToString();
            }
            //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
            strMenuId = SQLInjectionDefense.ReplaceSQLReservedKeyword(strMenuId);
            this.setActionPathCaption(strMenuId, hsTable);

        }
    }

    /// <summary>
    /// 设置路径显示字符串
    /// </summary>
    /// <param name="strParentMenuId"></param>
    /// <param name="hsTable"></param>
    private void setActionPathCaption(String strParentMenuId,Hashtable hsTable)
    {
        try
        {
            String strPathCaption = "";
            StringBuilder strBuilder = new StringBuilder();
            if (strParentMenuId.Equals(""))
            {
                String strRootId = Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("MenuRootId");
                MenuManagerBll bllMenu = new MenuManagerBll();
                DataTable dtRoot = bllMenu.GetMenuInfoByMenuCode(strRootId);
                strPathCaption = "[" + dtRoot.Rows[0][strCaptionColName].ToString() + "]";
                strBuilder.Append("    <span class=\"left_ts\">" + strPathCaption + "</span>");
            }
            else
            {
                String strCurMenuCode = "";
                String strCurMenuCaption = "";
                hsTable = this.getHTMenuCaption(strParentMenuId, hsTable);
                int iHsTableCount = hsTable.Count;
                for (int i = iHsTableCount - 1; i >= 0; i--)
                {
                    strCurMenuCode = (hsTable[i.ToString()].ToString().Split('*'))[0].ToString();
                    strCurMenuCaption = (hsTable[i.ToString()].ToString().Split('*'))[1].ToString();

                    strPathCaption = strPathCaption + "--->>" + hsTable[i.ToString()];
                    if (i > 0)
                    {
                        //strBuilder.Append("    <a href=\"javascript:forwardSubFunction('" + strCurMenuCode + "');\" onmouseover=\"javascript:window.status='" + strCurMenuCode + "';return true;\"><br>" + strCurMenuCaption + "</a>--->>\r\n");
                        strBuilder.Append("    <span onclick=\"javascript:forwardSubFunction('" + strCurMenuCode + "');\" onmouseover=\"javascript:window.status='" + strCurMenuCode + "';return true;\"  title=\"Click to View the following of " + strCurMenuCaption + "\" class=\"left_ts\">" + strCurMenuCaption + "</span>--->>");
                    }
                    else//最后一级不提供链接
                    {
                        strBuilder.Append("    <span class=\"left_ts\">" + strCurMenuCaption + "</span>");
                    }
                }
                //strPathCaption = strPathCaption.TrimStart('-').TrimStart('-').TrimStart('-').TrimStart('>').TrimStart('>');
            }
            //this.Label1.Text = strPathCaption;
            strBuilder.Append("  \r\n");

            //在页面显示
            this.divPath.InnerHtml = strBuilder.ToString();
        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
    }

    /// <summary>
    /// 获取多级菜单的hashTable
    /// </summary>
    /// <param name="strParentMenuId"></param>
    /// <param name="hsTable"></param>
    /// <returns></returns>
    private Hashtable getHTMenuCaption(String strParentMenuId, Hashtable hsTable)
    {
        try
        {
            MenuManagerBll bllMenu = new MenuManagerBll();
            DataTable dtRoot = bllMenu.GetMenuInfoByMenuCode(strParentMenuId);
            String strCount = iCount.ToString();
            String strMenuCode = dtRoot.Rows[0]["SMENUCODE"].ToString();
            String strCaption = "";
            String strTempMenId = "";

            strCaption = "[" + dtRoot.Rows[0][strCaptionColName].ToString() + "]";
            hsTable.Add(strCount, strMenuCode + "*" + strCaption);
            iCount++;

            if (!String.IsNullOrEmpty(dtRoot.Rows[0]["SPARENTCODE"].ToString()))
            {
                strTempMenId = dtRoot.Rows[0]["SPARENTCODE"].ToString();
                getHTMenuCaption(strTempMenId, hsTable);
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
        return hsTable;
    }

}
