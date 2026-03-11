using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Com.ValuePlus.Web;
using Com.ValuePlus.Utils;
using Com.ValuePlus.Flow.DAL;
using System.Web.UI.WebControls;
using Com.ValuePlus.Common;

public partial class AppFunction_SalaryCalculate_CalculateFlow : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            try
            {
                //创建薪资计算流程页面展现
                this.BuildCalculateFlowArea();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                this.AlertMessageBox(this.Page,"加载薪资计算流程错误");
            }
        }

    }

    #region viewstate初始化区域
    private ArrayList ArrDeptName
    {
        get
        {
            return ViewState["ArrDeptName_ViewState"] as ArrayList;
        }
        set
        {
            ViewState["ArrDeptName_ViewState"] = value;
        }
    }
    #endregion

    #region 获取薪资计算流程中涉及到的部门
    /// <summary>
    /// 获取薪资计算流程中涉及到的部门
    /// </summary>
    /// <returns></returns>
    private ArrayList GetFlowDeptInfo()
    {
        ArrayList arrList = new ArrayList(); 
        String strSql = "";
        if (base.Language.Equals("en-us"))
        {
            strSql = "SELECT distinct(SDEPTNAME) FROM TB_SALARY_CALCULATE_FLOW GROUP BY SDEPTNAME";
        }
        else
        {
            strSql = "SELECT distinct(SDEPTNAMECN) FROM TB_SALARY_CALCULATE_FLOW GROUP BY SDEPTNAMECN";
        }
        DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
        if ((dt != null) && (dt.Rows.Count > 0))
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                String strName = dt.Rows[i][0].ToString();
                arrList.Add(strName);
            }
        }
        return arrList;
    }
    #endregion

    #region 获取薪资计算流程明细信息
    /// <summary>
    /// 获取薪资计算流程明细信息
    /// </summary>
    /// <returns></returns>
    private DataTable GetCalculateFlowInfo()
    {
        String strSql = "SELECT * FROM TB_SALARY_CALCULATE_FLOW ORDER BY NORDER";
        DataTable dt = new DataTable();
        try
        {
            dt = SqlParamDao.GetDataTableBySql(strSql);
        }
        catch (Exception ex)
        {
            log.Error(ex);
            this.AlertMessageBox(this.Page, "读取数据库出错：TB_SALARY_CALCULATE_FLOW表不存在！");
        }

        return dt;
    }
    #endregion

    #region 创建薪资计算流程页面展现
    /// <summary>
    /// 创建薪资计算流程页面展现
    /// </summary>
    private void BuildCalculateFlowArea()
    {
        DataTable dt = this.GetCalculateFlowInfo();
        
        if ((dt != null) && (dt.Rows.Count > 0))
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                String strOpCode = "";
                String strOrder = "";
                String strOpName = "";
                String strImage = "";
                String strDeptName = "";
                String strUrlDetail = "";
                String strParams = "";
                String strShowTip = "";
                String strIsCurStep = "";
                String strOPDate = "";
                strOpCode = dt.Rows[i]["SOPERATIONCODE"].ToString();
                strOrder = dt.Rows[i]["NORDER"].ToString();
                if (base.Language.Equals("en-us"))
                {
                    strOpName = dt.Rows[i]["SOPNAME"].ToString();
                    strDeptName = dt.Rows[i]["SDEPTNAME"].ToString();
                }
                else
                {
                    strOpName = dt.Rows[i]["SOPNAMECN"].ToString();
                    strDeptName = dt.Rows[i]["SDEPTNAMECN"].ToString();
                }
                strUrlDetail = dt.Rows[i]["SOPDETAIL"].ToString();
                strParams = "pageUrl=" + strUrlDetail;
                strImage = dt.Rows[i]["SIMAGE"].ToString();
                strIsCurStep = dt.Rows[i]["BISCUROP"].ToString();
                if (dt.Rows[i]["DTOPDATE"] != DBNull.Value)
                {
                    DateTime dtOpDate = DateTime.Parse(dt.Rows[i]["DTOPDATE"].ToString());
                    strOPDate = "【" + dtOpDate.ToString("yyyy-MM-dd HH:mm:ss") + "】";
                }
                strShowTip = "(" + strDeptName + ")" + strOpName + strOPDate;

                //动态创建流程步骤
                TableRow row1 = new TableRow();
                TableCell cellOp = new TableCell();
                Image imageOp = new Image();
                Image imageCur = new Image();
                Label labelOp = new Label();
                imageOp.ID = "image" + strOpCode;
                imageOp.ImageAlign = ImageAlign.Middle;
                imageOp.ImageUrl = strImage;
                imageOp.Height = Unit.Pixel(30);
                imageCur.ImageUrl = "../../common/images/SalaryFlow/CurrentStep.png";
                labelOp.ID = "lb" + strOpCode;
                labelOp.Text = "Step"+strOrder+"、"+strShowTip;
                if (strIsCurStep.Equals("1"))
                {
                    //cellOp.BackColor = System.Drawing.Color.GreenYellow;
                    cellOp.Style.Add("cursor", "hand");
                    cellOp.Attributes.Add("onclick", "redirectToDeal('" + UrlParamEncryption.EncryptionUrlParam(strParams) + "')");
                    labelOp.ForeColor = System.Drawing.Color.Red;
                    cellOp.Controls.Add(imageCur);
                }
                cellOp.HorizontalAlign = HorizontalAlign.Center;
                cellOp.VerticalAlign = VerticalAlign.Middle;
                cellOp.Height = Unit.Pixel(10);
                cellOp.Controls.Add(imageOp);
                cellOp.Controls.Add(labelOp);
                cellOp.HorizontalAlign = HorizontalAlign.Center;
                cellOp.ToolTip = strShowTip;
                row1.Controls.Add(cellOp);
                row1.HorizontalAlign = HorizontalAlign.Center;
                this.divFlowImageArea.Controls.Add(row1);

                if (i < dt.Rows.Count-1)//最后一步无需再增加箭头
                {

                    TableRow row2 = new TableRow();
                    TableCell cellRrrow = new TableCell();
                    Image imageArrow = new Image();
                    imageArrow.ID = "arrow" + strOpCode;
                    imageArrow.ImageAlign = ImageAlign.Middle;
                    imageArrow.ImageUrl = "../../common/images/SalaryFlow/downArrow.png";
                    imageArrow.Height = Unit.Pixel(16);
                    cellRrrow.Controls.Add(imageArrow);
                    cellRrrow.HorizontalAlign = HorizontalAlign.Center;
                    row2.Controls.Add(cellRrrow);
                    row2.HorizontalAlign = HorizontalAlign.Center;

                    this.divFlowImageArea.Controls.Add(row2);
                }
            }
        }
    }
    #endregion

}
