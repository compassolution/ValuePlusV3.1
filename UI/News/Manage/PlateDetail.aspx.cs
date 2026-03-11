using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Web;
using System.Drawing;

public partial class News_Manage_PlateDetail : PageBase
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            this.strPlateId = Request.Params["plateId"].ToString();
            this.strParentId = Request.Params["parentId"].ToString();
            //this.strImagePath = this.hfImagePath.Value;


            //加载父栏目的下拉选择框
            this.CreateParentPlateList();
            this.SetHaveParentIdPage();

            if ((strPlateId != null) && (!strPlateId.Equals("")))//修改当前栏目信息
            {
                ViewState["opKey"] = "modify";//页面修改
                this.txtCode.ReadOnly = true;
                this.txtLevel.ReadOnly = true;

                //加载栏目表所有数据到页面中的列表中显示
                try
                {
                    //获取栏目表所有数据dataset
                    this.BindPlateInfo(true, strPlateId);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }
            else//新增栏目信息
            {
                ViewState["opKey"] = "add";//页面新增
            }
        }
    }

    #region viewstate初始化区域
    private string strPlateId
    {
        get
        {
            return ViewState["plateDetail_strPlateId_ViewState"] as string;
        }
        set
        {
            ViewState["plateDetail_strPlateId_ViewState"] = value;
        }
    }
    private string strParentId
    {
        get
        {
            return ViewState["plateDetail_strParentId_ViewState"] as string;
        }
        set
        {
            ViewState["plateDetail_strParentId_ViewState"] = value;
        }
    }
    private string strImagePath
    {
        get
        {
            return ViewState["plateDetail_strImagePath_ViewState"] as string;
        }
        set
        {
            ViewState["plateDetail_strImagePath_ViewState"] = value;
        }
    }
    #endregion

    #region 绑定数据
    /// <summary>
    /// 绑定数据
    /// </summary>
    /// <param name="bFresh">是否重新获取数据</param>
    private void BindPlateInfo(bool bFresh, String strPlateId)
    {
        DataTable dt = new DataTable();
        if (bFresh)
        {
            ViewState["PlateDetailViewState"] = GetPlateDetailInfo(strPlateId);

        }
        else
        {
            if (ViewState["PlateDetailViewState"] == null)
            {
                ViewState["PlateDetailViewState"] = GetPlateDetailInfo(strPlateId);
            }
        }
        dt = (DataTable)ViewState["PlateDetailViewState"];
        if ((dt != null) && (dt.Rows.Count > 0))
        {
            this.txtCode.Text = dt.Rows[0]["SPLATEID"].ToString();
            this.txtName.Text = dt.Rows[0]["SPLATENAME"].ToString();
            this.txtNameChs.Text = dt.Rows[0]["SPLATENAMECHS"].ToString();
            this.txtDesc.Text = dt.Rows[0]["SDESC"].ToString();
            this.txtDescChs.Text = dt.Rows[0]["SDESCCHS"].ToString();
            this.txtImage.Text = dt.Rows[0]["SIMAGENAME"].ToString();
            String strParentCode = dt.Rows[0]["SPARENTID"].ToString();
            String strLevel = dt.Rows[0]["SLEVEL"].ToString();
            String strOrder = dt.Rows[0]["SORDER"].ToString();
            String strParentLevel = (int.Parse(strLevel) - 1).ToString();
            String strSelectedValue = strParentCode + "*" + strParentLevel;
            this.ddListParent.SelectedIndex = this.ddListParent.Items.IndexOf(this.ddListParent.Items.FindByValue(strSelectedValue));
            this.txtLevel.Text = dt.Rows[0]["SLEVEL"].ToString();
            this.txtOrder.Text = strOrder;
            this.ddListIsStop.SelectedIndex = this.ddListIsStop.Items.IndexOf(this.ddListIsStop.Items.FindByValue(dt.Rows[0]["BISSTOP"].ToString()));
            if (strLevel.Equals("0"))//如果是根目录，则禁止选择上级目录
            {
                this.ddListParent.Enabled = false;
                this.txtOrder.Enabled = false;
            }
        }
    }
    #endregion

    #region 获取相应版块的明细信息
    /// <summary>
    /// 获取相应版块的明细信息
    /// </summary>
    /// <returns></returns>
    public DataTable GetPlateDetailInfo(String strPlateId)
    {
        String strSql = "SELECT * FROM TB_NEWS_PLATE WHERE SPLATEID = '" + strPlateId + "'";
        DataTable dtPlateInfo = SqlParamDao.GetDataTableBySql(strSql);
        return dtPlateInfo;
    }
    #endregion

    #region 获取所有版块信息
    /// <summary>
    /// 获取所有版块信息
    /// </summary>
    /// <returns></returns>
    private DataTable GetAllPlateInfo()
    {
        if (ViewState["AllPlateInfoViewState"] == null)
        {
            String strSql = "SELECT * FROM TB_NEWS_PLATE ORDER BY SLEVEL,SORDER";
            DataSet ds = SqlParamDao.GetDataSetBySql(strSql);
            if (ds != null)
            {
                ViewState["AllPlateInfoViewState"] = ds.Tables[0];
            }
        }
        return (DataTable)ViewState["AllPlateInfoViewState"];
    }
    #endregion

    #region 保存操作
    /// <summary>
    /// 保存操作
    /// </summary>
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        if (String.IsNullOrEmpty(this.txtCode.Text.Trim()))
        {
            this.AlertMessageBox(this.Page, "请输入后重试");
            this.txtCode.Focus();
            this.txtCode.BackColor = Color.Red;
            //返回页面
            return;
        }
        else if (String.IsNullOrEmpty(this.txtName.Text.Trim()))
        {
            this.AlertMessageBox(this.Page, "请输入后重试");
            this.txtName.Focus();
            this.txtName.BackColor = Color.Red;
            //返回页面
            return;
        }
        else if (String.IsNullOrEmpty(this.txtNameChs.Text.Trim()))
        {
            this.AlertMessageBox(this.Page, "请输入后重试");
            this.txtNameChs.Focus();
            this.txtNameChs.BackColor = Color.Red;
            //返回页面
            return;
        }
        else if (String.IsNullOrEmpty(this.txtOrder.Text))
        {
            this.AlertMessageBox(this.Page, "请输入后重试");
            this.txtOrder.Focus();
            this.txtOrder.BackColor = Color.Red;
            //返回页面
            return;
        }
        else
        {
            try
            {
                String strPlateId = this.txtCode.Text.Trim();
                String strPlateName = this.txtName.Text.Trim();
                String strPlateNameChs = this.txtNameChs.Text.Trim();
                String strPlateDesc = this.txtDesc.Text.Trim();
                String strPlateDescChs = this.txtDescChs.Text.Trim();
                String strImageName = this.txtImage.Text.Trim();
                byte[] imgByte = null;
                if (!String.IsNullOrEmpty(strImageName))
                {
                    String strImage = this.hfImagePath.Value + strImageName;

                    imgByte = this.GetPictureData(strImage);
                }

                String[] strArr = this.ddListParent.SelectedValue.Split('*');
                String strParentCode = strArr[0].ToString();
                String strLevel = this.txtLevel.Text.Trim();
                String strOrder = this.txtOrder.Text;

                String strIsstop = this.ddListIsStop.SelectedValue.ToUpper();

                //设置root根的上级为空
                if (strLevel.Equals("0"))
                {
                    strParentCode = "";
                }
                String strSql = "";
                if (ViewState["opKey"].Equals("modify"))
                {
                    strSql = "UPDATE TB_NEWS_PLATE SET [SPLATENAME] ='" + strPlateName + "' , [SPLATENAMECHS]='" + strPlateNameChs + "' ,[SDESC] ='" + strPlateDesc + "' ,[SDESCCHS]='" + strPlateDescChs + "' ,[SPARENTID]='" + strParentCode + "' ,[SLEVEL]='" + strLevel + "' ,[SIMAGENAME]='" + strImageName + "' ,[SIMAGE]='" + imgByte + "' ,[SORDER]='" + strOrder + "' ,[BISSTOP]='" + strIsstop + "' where SPLATEID = '" + strPlateId+"'";
                    int iUpdateCount = SqlParamDao.ExecuteNonQueryBySql(strSql);
                    if (iUpdateCount > 0)
                    {
                        this.AlertMessageBox(this.Page, "Successfully！");
                        this.BindPlateInfo(true, strPlateId);
                    }
                    else
                    {
                        this.AlertMessageBox(this.Page, "Failed!");
                    }
                }
                else if (ViewState["opKey"].Equals("add"))
                {
                    if (IsExsitPlateCode(strPlateId))
                    {
                        this.AlertMessageBox(this.Page, "编码已经存在，请重新输入");
                    }
                    else
                    {
                        strSql = "insert into TB_NEWS_PLATE([SPLATEID],[SPLATENAME],[SPLATENAMECHS],[SDESC],[SDESCCHS],[SPARENTID],[SLEVEL],[SIMAGENAME],[SIMAGE],[SORDER],[BISSTOP]) values(";
                        strSql = strSql + "'" + strPlateId + "','" + strPlateName + "','" + strPlateNameChs + "','" + strPlateDesc + "','" + strPlateDescChs + "','" + strParentCode + "','" + strLevel + "','" + strImageName + "','" + imgByte + "','" + strOrder + "','" + strIsstop + "')";
                        int iAddCount = SqlParamDao.ExecuteNonQueryBySql(strSql);
                    
                        if (iAddCount > 0)
                        {
                            this.AlertMessageBox(this.Page, "Successfully！");
                            Response.Write("<script language=\"javascript\">window.parent.location.reload();;</script>");
                        }
                        else
                        {
                            this.AlertMessageBox(this.Page, "Failed!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                this.AlertMessageBox(this.Page, "Failed!");
            }
        }
    }
    #endregion

    #region 触发新增操作
    /// <summary>
    /// 触发新增操作
    /// </summary>
    protected void Button2_Click(object sender, EventArgs e)
    {
        ViewState["opKey"] = "add";
        Response.Redirect("PlateDetail.aspx?plateId=");
    }
    #endregion

    #region 返回列表操作
    /// <summary>
    /// 返回列表操作
    /// </summary>
    protected void btnReturn_Click(object sender, EventArgs e)
    {
        String strId = this.strPlateId;
        if (!String.IsNullOrEmpty(this.strParentId))
        {
            strId = this.strParentId;
        }
        Response.Redirect("NewsList.aspx?plateId=" + strId);
    }
    #endregion

    #region 加载父栏目的下拉选择框
    /// <summary>
    ///加载父栏目的下拉选择框
    /// </summary>
    private void CreateParentPlateList()
    {
        String strLanguage = this.Language;
        String strSql = "select * from TB_NEWS_PLATE ORDER BY SORDER";
        DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
        ListItem lItem = new ListItem("", "");// 构造一项
        this.ddListParent.Items.Add(lItem);

        if ((dt != null) && (dt.Rows.Count > 0))
        {
            foreach (DataRow row in dt.Rows)
            {
                String strPlateId = row["SPLATEID"].ToString();
                String strPlateName = "";
                if (strLanguage.Equals("zh-cn"))
                {
                    strPlateName = row["SPLATENAMECHS"].ToString();
                }
                else
                {
                    strPlateName = row["SPLATENAME"].ToString();
                }
                String strPlateLevel = row["SLEVEL"].ToString();
                String strOrder = row["SORDER"].ToString();
                int iPlateLevel = int.Parse(strPlateLevel);

                String strLine = "";
                for (int i = 0; i < iPlateLevel; i++)
                {
                    strLine = strLine + ">>>>>";
                }

                String strListCaption = strLine + strOrder + "、" + strPlateId + "【" + strPlateName + "】";
                String strListValue = strPlateId + "*" + strPlateLevel;

                lItem = new ListItem(strListCaption, strListValue);// 构造一项
                this.ddListParent.Items.Add(lItem);
            }

        }

    }

    /// <summary>
    /// 如果是新增子版块则自动填充
    /// </summary>
    private void SetHaveParentIdPage()
    {

        //如果是新增子版块则自动填充
        if (!String.IsNullOrEmpty(this.strParentId))
        {
            DataTable dtParent = this.GetPlateDetailInfo(this.strParentId);
            if ((dtParent != null) && (dtParent.Rows.Count > 0))
            {
                String strLevel = dtParent.Rows[0]["SLEVEL"].ToString();
                String strOrder = dtParent.Rows[0]["SORDER"].ToString();
                String strSelectedValue = this.strParentId + "*" + strLevel;
                this.ddListParent.SelectedIndex = this.ddListParent.Items.IndexOf(this.ddListParent.Items.FindByValue(strSelectedValue));
                int iParentLevel = int.Parse(dtParent.Rows[0]["SLEVEL"].ToString());
                this.txtLevel.Text = (iParentLevel + 1).ToString();

                this.ddListParent.Enabled = false;
                this.txtLevel.ReadOnly = true;
            }
        }
    }
    #endregion

    #region 判断栏目编码是否已经存在
    /// <summary>
    /// 判断栏目编码是否已经存在
    /// </summary>
    /// <param name="strMenuCode"></param>
    /// <returns></returns>
    public Boolean IsExsitPlateCode(String strPlateId)
    {
        Boolean bIsExsit = true;
        String strSql = "select * from TB_NEWS_PLATE WHERE SPLATEID = '" + strPlateId + "'";
        DataSet ds = SqlParamDao.GetDataSetBySql(strSql);
        if (ds == null)//ds为空
        {
            bIsExsit = false;
        }
        else
        {
            if (ds.Tables.Count == 0)//ds中没有表
            {
                bIsExsit = false;
            }
            else
            {
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 0)//ds中的表没有数据
                {
                    bIsExsit = false;
                }
            }
        }
        return bIsExsit;
    }
    #endregion

    private byte[] GetPictureData(string imagepath)          
    {
        imagepath = Server.MapPath(imagepath);
        //根据图片文件的路径使用文件流打开，并保存为byte[]                 
        FileStream fs = new FileStream(imagepath, FileMode.Open);             
        byte[] byData = new byte[fs.Length];              
        fs.Read(byData, 0, byData.Length);             
        fs.Close();              
        return byData;          
    } 

}

