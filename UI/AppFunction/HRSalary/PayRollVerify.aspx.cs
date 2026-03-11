using System;
using System.Collections.Generic;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Com.ValuePlus.Web;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Archive.BLL;
using Com.ValuePlus.Archive.Entity;
using System.Data;
using System.Text;
using Com.ValuePlus.Common.Security;

public partial class AppFunction_HRSalary_PayRollVerify : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            try
            {
                this.strTID = Request.Params["TID"] == null ? "PREMPL" : Request.Params["RID"].ToString();
                this.strRID = Request.Params["RID"] == null ? "" : Request.Params["RID"].ToString();
                this.strSID = Request.Params["SID"] == null ? "" : Request.Params["SID"].ToString();

                //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
                this.strTID = SQLInjectionDefense.ReplaceSQLReservedKeyword(this.strTID);
                this.strRID = SQLInjectionDefense.ReplaceSQLReservedKeyword(this.strRID);
                this.strSID = SQLInjectionDefense.ReplaceSQLReservedKeyword(this.strSID);

                this.RefreshData();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }
    }

    #region viewstate初始化区域
    private string strTID
    {
        get
        {
            return ViewState["strTID"] as string;
        }
        set
        {
            ViewState["strTID"] = value;
        }
    }
    private string strRID
    {
        get
        {
            return ViewState["strRID"] as string;
        }
        set
        {
            ViewState["strRID"] = value;
        }
    }
    private string strSID
    {
        get
        {
            return ViewState["strSID"] as string;
        }
        set
        {
            ViewState["strSID"] = value;
        }
    }
    private string strYearMonth
    {
        get
        {
            return ViewState["strYearMonth"] as string;
        }
        set
        {
            ViewState["strYearMonth"] = value;
        }
    }
    private string strStatusCode
    {
        get
        {
            return ViewState["strStatusCode"] as string;
        }
        set
        {
            ViewState["strStatusCode"] = value;
        }
    }
    private string strStatusName
    {
        get
        {
            return ViewState["strStatusName"] as string;
        }
        set
        {
            ViewState["strStatusName"] = value;
        }
    }
    #endregion

    private void RefreshData()
    {
        this.contentFrame.Attributes["src"] = "../../Archive/Archive.aspx?DOCU=" + this.strTID + "&ROLE=" + this.strRID;

        //如果是薪资库则显示当前月份及状态
        if (this.strTID.Equals("PREMPL"))
        {
            //获取当前薪资期间及其状态
            this.GetCurYearMonth();
        }
        //获取动作并自动生成到动作区
        this.GetAndBuildActionsBySID(this.strSID);

    }

    /// <summary>
    /// 获取当前薪资期间及其状态
    /// </summary>
    private void GetCurYearMonth()
    {
        String strSql = "select TOP 1 * from KQPERD_1 A INNER JOIN TB_HRLSTD B ON A.PCALSTAT = B.CID WHERE B.LID = 'CalStatus' AND A.PISNOW = '1'";
        DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
        if ((dt != null) && (dt.Rows.Count > 0))
        {
            DataRow dr = dt.Rows[0];
            this.strYearMonth = dr["PID"].ToString();
            this.strStatusCode = dr["CID"].ToString();
            this.strStatusName = dr["CDESC"].ToString();
            if (this.Language.Equals("zh-cn"))
            {
                this.strStatusName = dr["CDESCCHS"].ToString();
            }
        }
    }

    /// <summary>
    /// 获取动作并自动生成到动作区
    /// </summary>
    /// <param name="strSID"></param>
    private void GetAndBuildActionsBySID(String strSID)
    {
        ArchiveActionBll bllAction = new ArchiveActionBll();
        ArrayList arrActionList = bllAction.GetActionDetailList(this.strTID, this.strRID, this.strSID, 0, "", base.GetUserCode(), this.IsAdminstrator(), "0");

        if ((arrActionList != null) && (arrActionList.Count > 0))
        {
            String strActionName = "";
            String strActionPage = "";

            StringBuilder strBuilderAction = new StringBuilder();
            strBuilderAction.Append("\r\n");

            this.listPage.Items.Clear();
            for (int i = 0; i < arrActionList.Count; i++)
            {
                Entity_CreateAction entityAction = (Entity_CreateAction)arrActionList[i];
                if (this.Language.Equals("en-us"))
                {
                    strActionName = entityAction.ADESC;
                }
                else
                {
                    strActionName = entityAction.ADESCCHS;
                }
                strActionPage = entityAction.ADETAIL;
                //存储过程类型的动作
                if (entityAction.ATYPE.ToString().Equals("2"))
                {

                    strBuilderAction.Append("                    <a href=\"#\" onclick=\"javascript:doExcuteAction('" + strActionPage + "');\" class=\"a_Left\">" + strActionName + "</a>\r\n");

                }
                else
                {
                    ListItem lstItem = new ListItem();
                    lstItem.Value = entityAction.ADETAIL;
                    lstItem.Text = strActionName;
                    lstItem.Attributes.Add("PageDetail", strActionPage);
                    lstItem.Attributes.CssStyle.Add("text-decoration", "line-through");
                    lstItem.Attributes.CssStyle.Add("color", "gray");

                    this.listPage.Items.Add(lstItem);
                }
            }
            //如果是薪资库则显示当前月份及状态
            if (this.strTID.Equals("PREMPL"))
            {
                if (this.Language.Equals("en-us"))
                {
                    strBuilderAction.Append("                    <span class=\"left_ts\">Current Month：" + this.strYearMonth + "</span> \r\n");
                    strBuilderAction.Append("                    <span class=\"left_ts\">---(Salary Status：" + this.strStatusName + ")</span> \r\n");
                }
                else
                {
                    strBuilderAction.Append("                    <span class=\"left_ts\">当前薪资期间：" + this.strYearMonth + "</span> \r\n");
                    strBuilderAction.Append("                    <span class=\"left_ts\">---(薪资状态：" + this.strStatusName + ")</span> \r\n");
                }
            }
            //在页面显示动作
            this.divActionArea.InnerHtml = strBuilderAction.ToString();

        }

    }

    /// <summary>
    /// 刷新数据集事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Refresh_Click(object sender, EventArgs e)
    {
        this.RefreshData();
    }


    #region ListBox框变更事件
    protected void listPage_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        //int iSelected = this.listPage.SelectedIndex;
        //ListItem lstItem = (ListItem)this.listPage.Items[iSelected];
        //this.contentFrame.Attributes["src"] = lstItem.Value;
        
        this.contentFrame.Attributes["src"] = "../"+this.listPage.SelectedValue;
        int iSelected = this.listPage.SelectedIndex;
        if (this.listPage.Items.Count > iSelected)
        {
            this.listPage.Items[iSelected].Selected = true;
        }

    }

    #endregion

}