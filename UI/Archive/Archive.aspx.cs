using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Resources;
using System.Text;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Com.ValuePlus.Web;

using Com.ValuePlus.Archive.DAL;
using Com.ValuePlus.Archive.Config;
using Com.ValuePlus.Archive.BLL;
using Com.ValuePlus.Common;
using Com.ValuePlus.Common.Security;

public partial class Archive_Archive : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!base.IsPostBack)
        {
            //模板编码
            if (base.Request.Params["DOCU"] != null)
            {
                this.strDocuName = base.Request.Params["DOCU"].ToString();

                //是否默认显示列表的标志（status=select表示不显示列表而显示查询框）
                if (base.Request.Params["status"] == null)
                {
                    this.strOpenStatus = "";
                }
                else
                {
                    this.strOpenStatus = base.Request.Params["status"].ToString();
                }

                //可配置其中某个角色进行快速定位
                if (base.Request.Params["ROLE"] == null)
                {
                    this.strCurRole = "";
                    this.strSingleRole = "";
                }
                else
                {
                    this.strCurRole = base.Request.Params["ROLE"].ToString();
                    this.strSingleRole = base.Request.Params["ROLE"].ToString();
                }

                //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
                this.strDocuName = SQLInjectionDefense.ReplaceSQLReservedKeyword(this.strDocuName);
                this.strCurRole = SQLInjectionDefense.ReplaceSQLReservedKeyword(this.strCurRole);
                this.strSingleRole = SQLInjectionDefense.ReplaceSQLReservedKeyword(this.strSingleRole);
                this.strOpenStatus = SQLInjectionDefense.ReplaceSQLReservedKeyword(this.strOpenStatus);

                try
                {
                    //当前登录用户
                    this.strCurUserCode = this.GetUserCode();
                    this.DoLanguageSetting();
                    this.GetInfo_TB_HRTMPH(this.strDocuName);
                    //大图标显示时每行显示的数量
                    String strSceneListHtmlColCount = Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("iSenceListHtmlColCount");
                    this.iSceneListHtmlColCount = int.Parse(strSceneListHtmlColCount);
                    //大图标显示用户功能列表时是否显示相同图标
                    this.strSceneListIsSameBigIcon = Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("SenceListIsSameBigIcon");
                    //大图标显示用户功能列表时是否显示相同图标
                    this.strSceneListBigIconSize = Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("SenceListBigIconSize");

                    //创建角色列表
                    this.CreateRole(this.strCurUserCode, this.strDocuName);
                }catch(Exception ex)
                {
                    Page.ClientScript.RegisterStartupScript(typeof(Page), "alertError", "<script language=\"javascript\">alert('获取模板角色场景出错');</script>");
                    log.Error("获取模板角色场景出错：");
                    log.Error(ex.ToString());
                }
            }
        }
    }

    #region viewstate初始化区域
    private string strDocuName
    {
        get
        {
            return ViewState["strDocuName"] as string;
        }
        set
        {
            ViewState["strDocuName"] = value;
        }
    }
    private string strOpenStatus
    {
        get
        {
            return ViewState["strOpenStatus"] as string;
        }
        set
        {
            ViewState["strOpenStatus"] = value;
        }
    }
    private string strSingleRole
    {
        get
        {
            return ViewState["strSingleRole"] as string;
        }
        set
        {
            ViewState["strSingleRole"] = value;
        }
    }
    private string strCurUserCode
    {
        get
        {
            return ViewState["strCurUserCode"] as string;
        }
        set
        {
            ViewState["strCurUserCode"] = value;
        }
    }
    private string strCurRole
    {
        get
        {
            return ViewState["strCurRole"] as string;
        }
        set
        {
            ViewState["strCurRole"] = value;
        }
    }
    private int iRoleCount
    {
        get
        {
            if (this.ViewState["iRoleCount"] != null)
            {
                return (int)this.ViewState["iRoleCount"];
            }
            return 0;
        }
        set
        {
            this.ViewState["iRoleCount"] = value;
        }
    }
    private int iSceneCount
    {
        get
        {
            if (this.ViewState["iSceneCount"] != null)
            {
                return (int)this.ViewState["iSceneCount"];
            }
            return 0;
        }
        set
        {
            this.ViewState["iCurSceneCount"] = value;
        }
    }
    private int iSceneListHtmlColCount
    {
        get
        {
            if (this.ViewState["iSceneListHtmlColCount"] != null)
            {
                return (int)this.ViewState["iSceneListHtmlColCount"];
            }
            return 0;
        }
        set
        {
            this.ViewState["iSceneListHtmlColCount"] = value;
        }
    }
    private string strSceneListIsSameBigIcon
    {
        get
        {
            return ViewState["strSceneListIsSameBigIcon"] as string;
        }
        set
        {
            ViewState["strSceneListIsSameBigIcon"] = value;
        }
    }
    private string strSceneListBigIconSize
    {
        get
        {
            return ViewState["strSceneListBigIconSize"] as string;
        }
        set
        {
            ViewState["strSceneListBigIconSize"] = value;
        }
    }
    #endregion

    /// <summary>
    /// 页面语言设置以及基础设置
    /// </summary>
    private void DoLanguageSetting()
    {
        ResourceManager rmLocResourceManager = base.GetResourceManager("Archive");
        this.Page.Title = rmLocResourceManager.GetString("lbIndexTitle");
        this.Label1.Text = rmLocResourceManager.GetString("tipWarmTips");
    }

    #region 根据模板获取定义信息
    /// <summary>
    /// 根据模板获取定义信息
    /// </summary>
    /// <param name="strRole"></param>
    /// <param name="strTemplate"></param>
    ///<param name="iRoleIndex"></param>
    private void GetInfo_TB_HRTMPH(String strTemplate)
    {
        string strSql = "SELECT * FROM TB_HRTMPH WHERE TID='" + strTemplate + "'";
        DataSet ds = SqlParamDao.GetDataSetBySql(strSql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["ArchiveIsLog"] = ds.Tables[0].Rows[0]["TREC"].ToString();
            if (this.Language.Equals("zh-cn"))
            {
                Session["ArchiveDesc"] = ds.Tables[0].Rows[0]["TDESCCHS"].ToString();
                this.Page.Title = Session["ArchiveDesc"] + "首页";
            }
            else
            {
                Session["ArchiveDesc"] = ds.Tables[0].Rows[0]["TDESC"].ToString();
                this.Page.Title = Session["ArchiveDesc"]+" Index";
            }
        }

    }
    #endregion

    #region 创建登陆用户所具有的角色列表
    /// <summary>
    /// 创建登陆用户所具有的角色列表
    /// </summary>
    /// <param name="strUserCode"></param>
    /// <param name="strTemplate"></param>
    private void CreateRole(String strUserCode, String strTemplate)
    {
        String strSql;
        string str2;
        if (this.IsAdminstrator())
        {
            //如果是管理员账号
            strSql = "SELECT r.RID,r.RDESC,r.RDESCCHS FROM TB_HRTMPR r WHERE r.TID='" + strTemplate + "' ORDER BY RORDER";
            if (!String.IsNullOrEmpty(this.strSingleRole))
            {
                strSql = "SELECT r.RID,r.RDESC,r.RDESCCHS FROM TB_HRTMPR r WHERE r.TID='" + strTemplate + "' AND r.RID = '"+this.strSingleRole+"' ORDER BY RORDER";
            }
        }
        else
        {
            strSql = "SELECT r.RID,r.RDESC,r.RDESCCHS FROM TB_HRTMPR r,TB_HR_USERROLE ur WHERE r.TID=ur.TID AND r.RID=ur.RID AND ur.TID='" + strTemplate + "' AND ur.SUSERID='" + strUserCode + "' ORDER BY r.RORDER";
            if (!String.IsNullOrEmpty(this.strSingleRole))
            {
                strSql = "SELECT r.RID,r.RDESC,r.RDESCCHS FROM TB_HRTMPR r,TB_HR_USERROLE ur WHERE r.TID=ur.TID AND r.RID=ur.RID AND ur.TID='" + strTemplate + "' AND ur.SUSERID='" + strUserCode + "' AND r.RID = '" + this.strSingleRole + "' ORDER BY r.RORDER";
            }
        }
        DataSet ds = SqlParamDao.GetDataSetBySql(strSql);
        if (this.Language == "zh-cn")
        {
            str2 = "RDESCCHS";
        }
        else
        {
            str2 = "RDESC";
        }
        this.iRoleCount = ds.Tables[0].Rows.Count;
        StringBuilder strBuilderRole = new StringBuilder();
        if (ds.Tables[0].Rows.Count > 0)
        {
            //循环在页面写出
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                String strLiRole = "liRole" + (i + 1).ToString();
                strBuilderRole.Append("                     \r\n");
                strBuilderRole.Append("                         <li id=\"" + strLiRole + "\"><a href=\"#\"><img src=\"../common/images/down_list.gif\" height=12 width = 12 />" + ds.Tables[0].Rows[i][str2].ToString() + "</a></li>\r\n");

                //根据角色读取并填充状态列表信息
                this.CreateScene(ds.Tables[0].Rows[i]["RID"].ToString(), this.strDocuName, i);
            }
            //在页面显示
            this.divRoleArea.InnerHtml = strBuilderRole.ToString();

        }
        else
        {
            this.Label1.Text = "温馨提示：您尚不具备操作该功能的任何角色，敬请联系系统管理员！";
            this.Label1.ForeColor = System.Drawing.Color.Red;
        }
    }
    #endregion

    #region 选择某个角色
    /// <summary>
    /// 选择某个角色
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void SelectOneRoleClick(object sender, EventArgs e)
    {
        if (this.strCurRole != ((System.Web.UI.WebControls.DropDownList)sender).SelectedValue)
        {
            this.strCurRole = ((System.Web.UI.WebControls.DropDownList)sender).SelectedValue;
            //this.CreateScene(this.strCurRole, this.strDocuName);
        }

    }
    #endregion

    #region 根据模板及某一角色创建所具有的状态区域
    /// <summary>
    /// 根据模板及某一角色创建所具有的状态区域
    /// </summary>
    /// <param name="strRole"></param>
    /// <param name="strTemplate"></param>
    ///<param name="iRoleIndex"></param>
    private void CreateScene(String strRole, String strTemplate, int iRoleIndex)
    {
        string strSql = "SELECT s.SID,s.SDESC,s.SDESCCHS FROM TB_HRTMPS s,TB_HRTMPRD rs WHERE s.TID=rs.TID AND s.SID=rs.SID AND rs.TID='" + strTemplate + "' AND rs.RID='" + strRole + "' ORDER BY s.SORDER";
        DataSet dsScene = SqlParamDao.GetDataSetBySql(strSql);
        if (dsScene.Tables[0].Rows.Count > 0)
        {
            this.BuildSceneArea(dsScene, strRole, iRoleIndex);
        }

    }
    #endregion

    #region 加载用户功能列表（大图标）
    /// <summary>
    /// 加载用户功能列表（大图标）
    /// </summary>
    /// <param name="dsScene"></param>
    /// <param name="strRole"></param>
    /// <param name="iRoleIndex"></param>
    private void BuildSceneArea(DataSet dsScene, String strRole, int iRoleIndex)
    {
        int iShowCount = this.iSceneListHtmlColCount;//每行显s示图标个数
        String strWidth = "width =" + (100 / iShowCount).ToString() + "%";

        DataTable dtSceneList = dsScene.Tables[0];
        if ((dtSceneList != null) && (dtSceneList.Rows.Count > 0))
        {
            int iDtRowsCount = dtSceneList.Rows.Count;//场景记录数
            //如果只有一个角色，则直接跳转到列表页面
            if (this.iRoleCount <= 1)
            {
                String strSID = "";//场景编码
                DataRow dr = dtSceneList.Rows[0];
                strSID = dr["SID"].ToString();
                //String strParamString = ArchiveCommon.GetBeforeTransmitParam(this.strDocuName, strRole, strSID, this.strOpenStatus);
                String strParamString = "TID=" + this.strDocuName + "&RID=" + strRole + "&SID=" + strSID + "&status=" + this.strOpenStatus + "&ROLE=" + this.strSingleRole;
                this.RedirectToListPage(strParamString);
            }
            else
            {
                StringBuilder strBuilderScene = new StringBuilder();
                HtmlGenericControl LDiv = new HtmlGenericControl("div");
                LDiv.ID = "idSceneDiv" + (iRoleIndex + 1).ToString();
                LDiv.Attributes.Add("class", "content");
                strBuilderScene.Append("                \r\n");
                strBuilderScene.Append("                    <div class=\"post\">\r\n");
                strBuilderScene.Append("                        <table class=\"table\" width=\"98%\">\r\n");
                int iHtmlRowsCount = 1;//在页面显示列表图标的行数
                if (iDtRowsCount > iShowCount)
                {
                    if (iDtRowsCount % iShowCount > 0)
                    {
                        iHtmlRowsCount = iDtRowsCount / iShowCount + 1;
                    }
                    else
                    {
                        iHtmlRowsCount = iDtRowsCount / iShowCount;
                    }
                }

                String strSID = "";//场景编码
                String strSDESC = "";//场景名称
                String strMenuBigImage = "";//栏目显示大图标
                String strBigImageSize = " width=\"" + this.strSceneListBigIconSize + "\" height = \"" + this.strSceneListBigIconSize + "\"";
                //先循环在页面写出从第一行到倒手第二行的
                for (int i = 1; i <= iHtmlRowsCount - 1; i++)
                {
                    strBuilderScene.Append("\r\n");
                    strBuilderScene.Append("                            <tr>\r\n");
                    for (int j = (i - 1) * iShowCount; j <= i * iShowCount - 1; j++)
                    {
                        DataRow dr = dtSceneList.Rows[j];
                        strSID = dr["SID"].ToString();
                        if (this.Language == "zh-cn")
                        {
                            strSDESC = dr["SDESCCHS"].ToString();
                        }
                        else
                        {
                            strSDESC = dr["SDESC"].ToString();
                        }

                        strMenuBigImage = this.GetBigImageName(j);
                        //String strParamString = ArchiveCommon.GetBeforeTransmitParam(this.strDocuName, strRole, strSID, this.strOpenStatus);
                        String strParamString = "TID=" + this.strDocuName + "&RID=" + strRole + "&SID=" + strSID + "&status=" + this.strOpenStatus + "&ROLE=" + this.strSingleRole;
                        strParamString = UrlParamEncryption.EncryptionUrlParam(strParamString);
                        strBuilderScene.Append("                                <td " + strWidth + " style=\"cursor:hand\" align=\"center\"><a href=\"javascript:toArchiveListPage('" + strParamString + "');\" onmouseover=\"javascript:window.status='" + strSDESC + "';return true;\"><img alt=\"" + strSDESC + "\" src=\"../common/images/fuctionIcon/" + strMenuBigImage + "\" " + strBigImageSize + " /><br>" + strSDESC + "</a></td>\r\n");

                    }
                    strBuilderScene.Append("                            </tr>\r\n");
                }
                //然后在页面写出最后一行
                strBuilderScene.Append("                            <tr>\r\n");
                int iDtRemainCount = iDtRowsCount - (iHtmlRowsCount - 1) * iShowCount;//最后一行还剩几条记录
                int iHtmlRemainCount = iShowCount - iDtRemainCount;//页面中最后一行还剩几个空位
                for (int j = (iHtmlRowsCount - 1) * iShowCount; j < iDtRowsCount; j++)
                {
                    DataRow dr = dtSceneList.Rows[j];
                    strSID = dr["SID"].ToString();
                    if (this.Language == "zh-cn")
                    {
                        strSDESC = dr["SDESCCHS"].ToString();
                    }
                    else
                    {
                        strSDESC = dr["SDESC"].ToString();
                    }
                    strMenuBigImage = this.GetBigImageName(j);

                    //String strParamString = ArchiveCommon.GetBeforeTransmitParam(this.strDocuName, strRole, strSID, strOpenStatus);
                    String strParamString = "TID=" + this.strDocuName + "&RID=" + strRole + "&SID=" + strSID + "&status=" + this.strOpenStatus + "&ROLE=" + this.strSingleRole;
                    strParamString = UrlParamEncryption.EncryptionUrlParam(strParamString);
                    strBuilderScene.Append("                                <td " + strWidth + " style=\"cursor:hand\" align=\"center\"><a href=\"javascript:toArchiveListPage('" + strParamString + "');\" onmouseover=\"javascript:window.status='" + strSDESC + "';return true;\"><img alt=\"" + strSDESC + "\" src=\"../common/images/fuctionIcon/" + strMenuBigImage + "\" " + strBigImageSize + " /><br>" + strSDESC + "</a></td>\r\n");

                }
                //最后填充未占满的区域
                for (int n = 1; n <= iHtmlRemainCount; n++)
                {
                    strBuilderScene.Append("                                <td></td>\r\n");
                }
                strBuilderScene.Append("                              </tr>\r\n");

                strBuilderScene.Append("                        </table>\r\n");

                strBuilderScene.Append("            </div>\r\n");
                strBuilderScene.Append("            \r\n");
                //在页面显示
                LDiv.InnerHtml = strBuilderScene.ToString();
                this.divSceneArea.Controls.Add(LDiv);
            }
        }

    }

    /// <summary>
    /// 动态获取大图标文件名
    /// </summary>
    /// <param name="j"></param>
    /// <returns></returns>
    private String GetBigImageName(int j)
    {
        String strMenuBigImage = "Function.png";
        if (!(String.IsNullOrEmpty(this.strSceneListIsSameBigIcon)) && (this.strSceneListIsSameBigIcon.Equals("0")))
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

    /// <summary>
    /// 直接跳转到列表页面
    /// </summary>
    private void RedirectToListPage(String strParamString)
    {
        String strUrl = "ArchiveMain.aspx?" + UrlParamEncryption.EncryptionUrlParam(strParamString);
        Response.Redirect(strUrl,false);
    }
}
