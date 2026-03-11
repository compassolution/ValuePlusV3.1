using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Com.ValuePlus.BLL.SysManager;
using System.Text;
using Com.ValuePlus.Common.Config;
using System.Resources;
using Com.ValuePlus.Common.Security;

public partial class FunctionList : Com.ValuePlus.Web.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            String strFunCode = Request.Params["sCode"] == null ? "" : Request.Params["sCode"].ToString();
            String strType = Request.Params["type"] == null ? "icon" : Request.Params["type"].ToString();

            //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
            strFunCode = SQLInjectionDefense.ReplaceSQLReservedKeyword(strFunCode);
            strType = SQLInjectionDefense.ReplaceSQLReservedKeyword(strType);

            this.strFunCode = strFunCode;
            this.strType = strType;

            //初始化区域页面相关
            this.InitPageShow();

            if (this.strType.Equals("list"))
            {
                //加载用户功能列表（列表）
                this.BuildFunctionList(strFunCode);
            }
            else
            {
                //加载用户功能列表（大图标）
                this.BuildFunctionBigIcon(strFunCode);
            }

        }
    }

    #region viewstate初始化区域
    private string strFunCode
    {
        get
        {
            return ViewState["strFunCode"] as string;
        }
        set
        {
            ViewState["strFunCode"] = value;
        }
    }
    private string strType
    {
        get
        {
            return ViewState["strType"] as string;
        }
        set
        {
            ViewState["strType"] = value;
        }
    }
    private string strFunParentCode
    {
        get
        {
            return ViewState["strFunParentCode"] as string;
        }
        set
        {
            ViewState["strFunParentCode"] = value;
        }
    }
    private string strLbClick
    {
        get
        {
            return ViewState["strLbClick"] as string;
        }
        set
        {
            ViewState["strLbClick"] = value;
        }
    }
    private int iFunListHtmlColCount
    {
        get
        {
            if (this.ViewState["iFunListHtmlColCount"] != null)
            {
                return (int)this.ViewState["iFunListHtmlColCount"];
            }
            return 0;
        }
        set
        {
            this.ViewState["iFunListHtmlColCount"] = value;
        }
    }
    private string strFunListIsSameBigIcon
    {
        get
        {
            return ViewState["strFunListIsSameBigIcon"] as string;
        }
        set
        {
            ViewState["strFunListIsSameBigIcon"] = value;
        }
    }
    private string strFunListBigIconSize
    {
        get
        {
            return ViewState["strFunListBigIconSize"] as string;
        }
        set
        {
            ViewState["strFunListBigIconSize"] = value;
        }
    }
    #endregion

    #region 初始化区域页面相关
    /// <summary>
    /// 初始化区域页面相关
    /// </summary>
    private void InitPageShow()
    {
        //中英文设置
        this.SetLanguageLabel();
        //大图标显示时每行显示的数量
        String strFunListHtmlColCount = Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("iFunListHtmlColCount");
        //大图标显示用户功能列表时是否显示相同图标
        this.strFunListIsSameBigIcon = Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("FunListIsSameBigIcon");
        //大图标显示用户功能列表时是否显示相同图标
        this.strFunListBigIconSize = Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("FunListBigIconSize");

        iFunListHtmlColCount = int.Parse(strFunListHtmlColCount);
        if (this.strType.Equals("icon"))
        {
            this.tdTitle.ColSpan = iFunListHtmlColCount;
        }
        else if (strType.Equals("list"))
        {
            this.tdTitle.ColSpan = 3;
        }
        else
        {
            this.tdTitle.ColSpan = iFunListHtmlColCount;
        }
        //获取当前栏目的上级栏目编码并设置到缓存
        this.GetFunPrarentCode(this.strFunCode);
        //切换显示及返回区域
        if (!String.IsNullOrEmpty(this.strFunParentCode))
        {
            this.aBack.HRef = "javascript:forwardSubFunction('" + this.strFunParentCode + "','001');";
        }
        this.aToIcon.HRef = "javascript:window.location.href = 'FunctionList.aspx?sCode=" + this.strFunCode + "&type=icon';";
        this.aToList.HRef = "javascript:window.location.href = 'FunctionList.aspx?sCode=" + this.strFunCode + "&type=list';";

        this.aBack.Attributes.Add("onmouseover", "javascript:window.status='';return true;");
        this.aToIcon.Attributes.Add("onmouseover", "javascript:window.status='';return true;");
        this.aToList.Attributes.Add("onmouseover", "javascript:window.status='';return true;");

    }
    #endregion

    #region 设置中英文
    /// <summary>
    /// 设置中英文
    /// </summary>
    private void SetLanguageLabel()
    {
        ResourceManager rmLocResourceManager = base.GetResourceManager("FunctionList");
        this.Label1.Text = rmLocResourceManager.GetString("lbBack");
        this.Label2.Text = rmLocResourceManager.GetString("lbList");
        this.Label3.Text = rmLocResourceManager.GetString("lbIcon");
        this.strLbClick = rmLocResourceManager.GetString("lbClick");
    }
    #endregion

    #region 获取当前栏目的上级栏目编码并设置到缓存
    /// <summary>
    /// 获取当前栏目的上级栏目编码并设置到缓存
    /// </summary>
    /// <param name="strCode"></param>
    /// <returns></returns>
    private void GetFunPrarentCode(String strCode)
    {
        this.strFunParentCode = null;
        MenuManagerBll bllMenu = new MenuManagerBll();
        DataTable dtMenu = bllMenu.GetMenuInfoByMenuCode(strFunCode);
        if ((dtMenu != null) && (dtMenu.Rows.Count > 0))
        {
            DataRow dr = dtMenu.Rows[0];
            this.strFunParentCode = dr["SPARENTCODE"].ToString();
        }
    }
    #endregion

    #region 获取当前页面的功能菜单数据集
    /// <summary>
    /// 获取当前页面的功能菜单数据集
    /// </summary>
    /// <param name="?"></param>
    /// <returns></returns>
    private DataTable GetDTMenuList(String strFunCode){
        String strCurUserId = base.GetUserCode();
        DataTable dtMenuList = new DataTable();
        //暂时不用缓存
        //String strDtSessionName = "dtSession"+strFunCode;
        //if (Session[strDtSessionName] != null)
        //{
        //    dtMenuList = (DataTable)Session[strDtSessionName];
        //}
        //else
        //{
            MenuManagerBll bllMenu = new MenuManagerBll();
            if (base.IsAdminstrator())
            {
                dtMenuList = this.GetNewDataTable(bllMenu.GetSubLevelMenu(strFunCode), "BISSTOP = '0'");
            }
            else
            {
                dtMenuList = this.GetNewDataTable(bllMenu.GetSubLevelMenuByMenuCodeAUserId(strFunCode, strCurUserId), "BISSTOP = '0'");
            }
        //    Session[strDtSessionName] = dtMenuList;
        //}
        return dtMenuList;
    }
    #endregion

    #region 执行DataTable中的查询返回新的DataTable
    /// <summary>
    /// 执行DataTable中的查询返回新的DataTable
    /// </summary>
    /// <param name="dt">源数据DataTable</param>
    /// <param name="condition">查询条件</param>
    /// <returns></returns>
    private DataTable GetNewDataTable(DataTable dt, string condition)
    {
        DataTable newdt = new DataTable();
        newdt = dt.Clone();
        DataRow[] dr = dt.Select(condition);
        for (int i = 0; i < dr.Length; i++)
        {
            newdt.ImportRow((DataRow)dr[i]);
        }
        return newdt;//返回的查询结果
    }
    #endregion 

    #region 加载用户功能列表（大图标）
    /// <summary>
    /// 加载用户功能列表（大图标）
    /// </summary>
    /// <param name="strFunCode"></param>
    private void BuildFunctionBigIcon(String strFunCode)
    {
        int iShowCount = this.iFunListHtmlColCount;//每行显示图标个数

        DataTable dtMenuList = this.GetDTMenuList(strFunCode);
        if ((dtMenuList!=null) && (dtMenuList.Rows.Count > 0))
        {
            int iDtRowsCount = dtMenuList.Rows.Count;//功能列表记录数
            int iHtmlRowsCount = 1;//在页面显示列表图标的行数
            if (iDtRowsCount > iShowCount)
            {
                if (iDtRowsCount % iShowCount > 0)
                {
                    iHtmlRowsCount = iDtRowsCount / iShowCount+1;
                }
                else
                {
                    iHtmlRowsCount = iDtRowsCount / iShowCount;
                }
            }

            StringBuilder strBuilderMenu = new StringBuilder();
            // modify by sammen 20230211 
            // 增加可以自定义图标及换行的功能
            bool bIsNewRow = false;
            int iCurRowHadCount = 0;
            for (int i = 0; i < iDtRowsCount; i++)
            {
                DataRow dr = dtMenuList.Rows[i];
                String strMenuCode = dr["SMENUCODE"].ToString();//栏目编码
                String strMenuType = dr["SMENUTYPE"].ToString();//栏目类型
                if(strMenuCode.Equals("HR0400"))
                {
                    String strMenuType1 = dr["SMENUTYPE"].ToString();//栏目类型

                }

                #region 创建行头<tr>
                if (i == 0)
                {
                    //第一个栏目前先创建<tr>
                    strBuilderMenu.Append("<tr>\r\n");
                }else if(bIsNewRow)
                {
                    //非第一个栏目需要换行时时创建<tr>
                    strBuilderMenu.Append("<tr>\r\n");
                }
                #endregion 创建行头<tr>

                #region 创建<td>
                if (strMenuType.ToUpper().Equals("L"))
                {
                    //换行型栏目
                    int iTempColSpan = 1;
                    if(iCurRowHadCount< iShowCount){
                        iTempColSpan = iShowCount - iCurRowHadCount;
                    }
                    strBuilderMenu.Append(" <td colspan = \""+ iTempColSpan.ToString()+ "\"></td>\r\n");
                    bIsNewRow = true;//设置需要换行
                    iCurRowHadCount = 0;//设置此行数量为0，需要重新开始
                }
                else
                {
                    strBuilderMenu.Append(this.BuildOneFunction(dr, i));
                    if (i >= (iDtRowsCount - 1))
                    {   
                        //如果是最后一个栏目
                        int iTempColSpan = 1;
                        if (iCurRowHadCount < iShowCount)
                        {
                            iTempColSpan = iShowCount - iCurRowHadCount;
                        }
                        strBuilderMenu.Append(" <td colspan = \"" + iTempColSpan.ToString() + "\"></td>\r\n");
                    }
                    bIsNewRow = false;
                    iCurRowHadCount++;
                }
                #endregion 创建</td>

                #region 创建行尾</tr>
                if (i >= (iDtRowsCount - 1) || iCurRowHadCount >= iShowCount)
                {
                    //最后一个栏目或者当前行栏目数已经等于每行规定个数了创建</tr>
                    strBuilderMenu.Append("</tr>\r\n");
                }
                else if (bIsNewRow)
                {
                    //非最后一个栏目需要换行时时创建<tr>
                    strBuilderMenu.Append("</tr>\r\n");
                }
                #endregion 创建行尾</tr>

                //当前行栏目数等于每行规定个数
                if (iCurRowHadCount >= iShowCount) {
                    iCurRowHadCount = 0;
                    bIsNewRow = true;
                }
            }


            //在页面显示
            this.divFunctionListArea.InnerHtml = strBuilderMenu.ToString();
        }

    }

    /// <summary>
    /// 创建单个菜单图标入口
    /// </summary>
    /// <param name="dr"></param>
    /// <param name="iRowIndex">行索引</param>
    /// <returns></returns>
    private String BuildOneFunction(DataRow dr,int iRowIndex)
    {
        StringBuilder strBuilderMenu = new StringBuilder();
        String strBigImageSize = " width=\"" + this.strFunListBigIconSize + "\" height = \"" + this.strFunListBigIconSize + "\"";
        int iShowCount = this.iFunListHtmlColCount;//每行显示图标个数
        String strWidth = "width =\"" + (100 / iShowCount).ToString() + "%\"";
        //String strWidth = "";//如果涉及有换行功能，则无法根据记录总数进行计算，何况不设置width时，也能平均分配宽度

        String strMenuCode = dr["SMENUCODE"].ToString();//栏目编码
        String strMenuName = dr["SMENUNAME"].ToString();//栏目名称
        if (this.Language == "zh-cn")
        {
            strMenuName = dr["SMENUNAMECN"].ToString();
        }
        String strMenuUrl = dr["SURLDETAIL"].ToString();// 栏目对应链接
        String strMenuShowLocation = dr["SSHOWLOCATION"].ToString();//栏目打开位置
        String strMenuType = dr["SMENUTYPE"].ToString();//栏目类型
        String strMenuBigImage = dr["SIMAGE"].ToString();//栏目显示大图标
        if (String.IsNullOrEmpty(strMenuBigImage)) strMenuBigImage = this.GetBigImageName(iRowIndex);

        //如果还有下级栏目则进入其下级目录列表，如果没有下级则直接进入对应页面
        if (strMenuType.ToUpper().Equals("F"))
        {
            strBuilderMenu.Append("    \r\n");
            strBuilderMenu.Append("  <td " + strWidth + " align=\"center\" colspan = \"1\"> \r\n");
            strBuilderMenu.Append("     <a href=\"javascript:forwardSubFunction('" + strMenuCode + "','" + strMenuShowLocation + "');\"  onmouseover=\"javascript:window.status='" + strMenuName + "';return true;\"> \r\n");
            strBuilderMenu.Append("     <img alt=\"" + strMenuName + "\" src=\"common/images/fuctionIcon/Function_folder.png\" " + strBigImageSize + " /><br> " + strMenuName + " \r\n");
            strBuilderMenu.Append("     </a> \r\n");
            strBuilderMenu.Append("  </td> \r\n");

        }
        else
        {
            strBuilderMenu.Append("  <td " + strWidth + " align=\"center\" colspan = \"1\"> \r\n");
            strBuilderMenu.Append("     <a href=\"javascript:forwardPage('" + strMenuUrl + "','" + strMenuCode + "','" + strMenuShowLocation + "');\" onmouseover=\"javascript:window.status='" + strMenuName + "';return true;\"> \r\n");
            strBuilderMenu.Append("     <img alt=\"" + strMenuName + "\" src=\"common/images/fuctionIcon/" + strMenuBigImage + "\" " + strBigImageSize + " /><br> " + strMenuName + " \r\n");
            strBuilderMenu.Append("     </a> \r\n");
            strBuilderMenu.Append("  </td> \r\n");
        }

        return strBuilderMenu.ToString();
    }

    /// <summary>
    /// 动态获取大图标文件名
    /// </summary>
    /// <param name="j"></param>
    /// <returns></returns>
    private String GetBigImageName(int j)
    {
        String strMenuBigImage = "Function.png";
        if (!(String.IsNullOrEmpty(this.strFunListIsSameBigIcon)) && (this.strFunListIsSameBigIcon.Equals("0")))
        {
            int iImageCount = 20;
            if (j <= iImageCount)
            {
                strMenuBigImage = "Function" + j.ToString() + ".png";
            }
            else
            {
                int iTemp = j % iImageCount;
                strMenuBigImage = "Function" + (iTemp).ToString() + ".png";
            }
        }
        return strMenuBigImage;
    }
    #endregion

    #region 加载用户功能列表（列表）
    /// <summary>
    /// 加载用户功能列表（列表）
    /// </summary>
    /// <param name="strFunCode"></param>
    private void BuildFunctionList(String strFunCode)
    {
        DataTable dtMenuList = this.GetDTMenuList(strFunCode);
        if ((dtMenuList != null) && (dtMenuList.Rows.Count > 0))
        {
            StringBuilder strBuilderMenu = new StringBuilder();
            String strMenuCode = "";//栏目编码
            String strMenuName = "";//栏目名称
            String strMenuUrl = "";//栏目对应链接
            String strImg = "";//图片名称
            String strMenuShowLocation = "";//栏目打开位置
            strBuilderMenu.Append("\r\n");

            for (int j = 0; j <= dtMenuList.Rows.Count - 1; j++)
            {
                strBuilderMenu.Append("  <tr>\r\n");
                strBuilderMenu.Append("    <td style=\"width:5%\" align=\"center\"><span style=\"margin-left:5px;\">"+(j+1).ToString()+"</span></td>\r\n");
                DataRow dr = dtMenuList.Rows[j];
                strMenuCode = dr["SMENUCODE"].ToString();
                if (this.Language == "zh-cn")
                {
                    strMenuName = dr["SMENUNAMECN"].ToString();
                }
                else
                {
                    strMenuName = dr["SMENUNAME"].ToString();
                }
                strMenuUrl = dr["SURLDETAIL"].ToString();
                strMenuShowLocation = dr["SSHOWLOCATION"].ToString();
                strImg = dr["SIMAGE"].ToString();
                if(String.IsNullOrEmpty(strImg)) strImg = "leaf.gif";
                //如果还有下级栏目则进入其下级目录列表，如果没有下级则直接进入对应页面
                if (String.IsNullOrEmpty(strMenuUrl))
                {
                    strBuilderMenu.Append("    <td style=\"width:70%;cursor:hand;font-weight:bold; \" align=\"left\"><a href=\"javascript:forwardSubFunction('" + strMenuCode + "','" + strMenuShowLocation + "');\" onmouseover=\"javascript:window.status='" + strMenuName + "';return true;\"><img src=\"common/images/treeIcon/" + strImg + "\" />" + strMenuName + "</a></td>\r\n");
                    strBuilderMenu.Append("    <td style=\"width:25%;cursor:hand;font-weight:bold; \" align=\"right\"><a href=\"javascript:forwardSubFunction('" + strMenuCode + "','" + strMenuShowLocation + "');\" onmouseover=\"javascript:window.status='" + strMenuName + "';return true;\">" + this.strLbClick + "</a>\r\n");
                }
                else
                {
                    strBuilderMenu.Append("    <td style=\"width:70%;cursor:hand;font-weight:bold; \" align=\"left\"><a href=\"javascript:forwardPage('" + strMenuUrl + "','" + strMenuCode + "','" + strMenuShowLocation + "');\" onmouseover=\"javascript:window.status='" + strMenuName + "';return true;\"><img src=\"common/images/treeIcon/" + strImg + "\" />" + strMenuName + "</a></td>\r\n");
                    strBuilderMenu.Append("    <td style=\"width:25%;cursor:hand;font-weight:bold; \" align=\"right\"><a href=\"javascript:forwardPage('" + strMenuUrl + "','" + strMenuCode + "','" + strMenuShowLocation + "');\" onmouseover=\"javascript:window.status='" + strMenuName + "';return true;\">" + this.strLbClick + "</a>\r\n");
                }
                strBuilderMenu.Append("  </tr>\r\n");
            }
        

            //在页面显示
            this.divFunctionListArea.InnerHtml = strBuilderMenu.ToString();
        }

    }
    #endregion

}
