using System;
using System.Collections;
using System.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Com.ValuePlus.Web;
using System.Resources;
using System.Drawing;
using System.Reflection;
using Com.ValuePlus.BLL.Export;
using System.IO;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Database;
using Com.ValuePlus.Common.Config;

public partial class AppFunction_PrintStaffCard_PrintStaffCard : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            ResourceManager rm = base.GetResourceManager("PrintStaffCard");
            this.lbldcno.Text = rm.GetString("lbDCNO");
            this.Label_DocuInfo.Text = rm.GetString("lbDocuInfo");
            this.Label_DCNO.Text = rm.GetString("lbDCNO");
            this.Label_Name.Text = rm.GetString("lbName");
            this.Label_NameCn.Text = rm.GetString("lbNameCn");
            this.lbDCNO.Text = rm.GetString("lbDCNO");
            this.lbName.Text = rm.GetString("lbName");
            this.lbNameCn.Text = rm.GetString("lbNameCn");
            this.aFilter.Text = rm.GetString("aFilter");
            this.aMake.Text = rm.GetString("aMake");
            this.aReMake.Text = rm.GetString("aReMake");
            this.aMultiMake.Text = rm.GetString("aMultiMake");
            this.lbTitle.Text = rm.GetString("lbTitle");
            this.lbHaveTip.Text = rm.GetString("lbHaveTip");
            this.lbNotHaveTip.Text = rm.GetString("lbNotHaveTip");
            this.lbMultiTip.Text = rm.GetString("lbMultiTip");
            this.aReMake.Attributes.Add("onclick", "javascript:if(confirm('Are you Sure?')){return true;}else{return false;}");
            this.aMultiMake.Attributes.Add("onclick", "javascript:if(confirm('" + rm.GetString("confirmprintinfo") + "')){return true;}else{return false;}");

            this.strsuccess = rm.GetString("successprintinfo");
            this.strprintfailinfo1 = rm.GetString("printfailinfo1");
            this.strprintfailinfo2 = rm.GetString("printfailinfo2");
            this.strprintfailinfo3 = rm.GetString("printfailinfo3");

            this.trHaveOp.Visible = false;
            this.trNotHavedOp.Visible = false;

            //员工照片路径
            this.strPhotoPath = "../../" + Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("StaffPhotoPath");
            this.strPhotoPath = Server.MapPath(this.strPhotoPath);

        }
    }

    #region 当前页面局部变量
    private string strPhotoPath
    {
        get
        {
            return this.ViewState["strPhotoPath"] as string;
        }
        set
        {
            this.ViewState["strPhotoPath"] = value;
        }

    }
    private string strprintfailinfo1
    {
        get
        {
            return this.ViewState["strprintfailinfo1"] as string;
        }
        set
        {
            this.ViewState["strprintfailinfo1"] = value;
        }

    }
    private string strprintfailinfo2
    {
        get
        {
            return this.ViewState["strprintfailinfo2"] as string;
        }
        set
        {
            this.ViewState["strprintfailinfo2"] = value;
        }

    }
    private string strprintfailinfo3
    {
        get
        {
            return this.ViewState["strprintfailinfo3"] as string;
        }
        set
        {
            this.ViewState["strprintfailinfo3"] = value;
        }

    }
    private string strsuccess
    {
        get
        {
            return this.ViewState["strsuccess"] as string;
        }
        set
        {
            this.ViewState["strsuccess"] = value;
        }

    }
    #endregion

    /// <summary>
    /// 判断某员工是否存在员工卡数据
    /// </summary>
    /// <param name="strDcno"></param>
    /// <returns></returns>
    private bool JudgeIsHaveData(String strDcno)
    {
        bool isHave = false;
        if (!String.IsNullOrEmpty(strDcno))
        {
            String strSql = "select * from [TB_HRCARD] where dcno = '"+strDcno+"'";
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
            if ((dt != null) && (dt.Rows.Count > 0))
            {
                isHave = true;
            }
        }
        return isHave;
    }

    private void FilterDCNO()
    {
        if (!String.IsNullOrEmpty(this.txtdcno.Text.ToString()))
        {
            String strDcno = this.txtdcno.Text.ToString().Trim();
            string strSql = "select * from [View_PrintStaffCard] where dcno='" + strDcno + "'";
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
            if ((dt != null) && (dt.Rows.Count > 0))
            {
                DataRow row = dt.Rows[0];
                this.lbDCNO.Text = strDcno;
                this.lbName.Text = row["DCNAME"].ToString();
                this.lbNameCn.Text = row["DCNAMECHS"].ToString();

                if (this.JudgeIsHaveData(strDcno))
                {
                    this.trHaveOp.Visible = true;
                    this.trNotHavedOp.Visible = false;
                }
                else
                {
                    this.trHaveOp.Visible = false;
                    this.trNotHavedOp.Visible = true;
                }
            }
            else
            {
                this.lbDCNO.Text = "";
                this.lbName.Text = "";
                this.lbNameCn.Text = "";

                this.trHaveOp.Visible = false;
                this.trNotHavedOp.Visible = false;
            }
        }
    }

    protected void aFilter_Click(object sender, EventArgs e)
    {
        this.FilterDCNO();
        this.txtErrLog.Text = "";
    }

    protected void aMake_Click(object sender, EventArgs e)
    {
        this.txtErrLog.Text = "";
        String strDcno = this.txtdcno.Text;
        if (String.IsNullOrEmpty(strDcno))
        {
            return;
        }
        string strSql = "select dcphoto from [View_PrintStaffCard] where dcno='" + strDcno + "'";
        try
        {
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
            if ((dt != null) && (dt.Rows.Count > 0))
            {
                DataRow dr = dt.Rows[0];
                if (dr[0] == DBNull.Value)
                {
                    String strErrLog = this.strprintfailinfo2 + strDcno;
                    this.WriteErrLog(strErrLog);
                    this.txtdcno.Focus();
                    return;
                }
                else
                {
                    this.InsertStaffPhotoImageBytes(strDcno,false);
                }
            }
            else
            {
                String strErrLog = this.strprintfailinfo1 + strDcno;
                this.WriteErrLog(strErrLog);
                this.txtdcno.Focus();
            }
            this.FilterDCNO();
        }
        catch (Exception ex)
        {
            log.Error(ex);
            AlertMessageBox(this.Page, ex.ToString().Replace("'", "\"").Replace("\n", "").Replace("\r", ""));
        }
    }

    protected void aReMake_Click(object sender, EventArgs e)
    {
        this.txtErrLog.Text = "";
        String strDcno = this.txtdcno.Text;
        if (String.IsNullOrEmpty(strDcno))
        {
            return;
        }
        string strSql = "select dcphoto from [View_PrintStaffCard] where dcno='" + strDcno + "'";
        try
        {
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
            if ((dt != null) && (dt.Rows.Count > 0))
            {
                DataRow dr = dt.Rows[0];
                if (dr[0] == DBNull.Value)
                {
                    String strErrLog = this.strprintfailinfo2 + strDcno;
                    this.WriteErrLog(strErrLog);
                    this.txtdcno.Focus();
                    return;
                }
                else
                {
                    this.InsertStaffPhotoImageBytes(strDcno,true);
                }
            }
            else
            {
                String strErrLog = this.strprintfailinfo1 + strDcno;
                this.WriteErrLog(strErrLog);
                this.txtdcno.Focus();
            }
            this.FilterDCNO();
        }
        catch (Exception ex)
        {
            log.Error(ex);
            AlertMessageBox(this.Page, ex.ToString().Replace("'", "\"").Replace("\n", "").Replace("\r", ""));
        }
    }

    protected void aMultiMake_Click(object sender, EventArgs e)
    {
        this.txtErrLog.Text = "";
        this.InsertStaffPhotoImageBytes(null,true);
    } 

    /// <summary>
    /// 插入员工照片的二进制数据
    /// </summary>
    /// <param name="strDcno"></param>
    private void InsertStaffPhotoImageBytes(String strDCNO,bool IsCanUpdate)
    {
        string strSql = "select dcno,dcphoto from [View_PrintStaffCard] ";
        if (!String.IsNullOrEmpty(strDCNO))
        {
            strSql = strSql + " where DCNO ='" + strDCNO + "'";
        }
        try
        {
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                using (IDataReader dr = dao.ExecuteReader(CommandType.Text, strSql, null))
                {
                    while (dr.Read())
                    {
                        string strDcno = dr["dcno"].ToString();
                        string strPhotoName = "";
                        if (dr["dcphoto"] == System.DBNull.Value)
                        {
                            strPhotoName = string.Empty;
                        }
                        else
                        {
                            strPhotoName = dr["dcphoto"].ToString();
                        }
                        if (!string.IsNullOrEmpty(strPhotoName))
                        {
                            bool bIsSuccess = false;
                            byte[] imageBytes = ReadPhotoFile(strPhotoName, ref bIsSuccess);
                            if (bIsSuccess)
                            {
                                strSql = "if not exists ( select * from [TB_HRCARD] where dcno = @DCNO) insert into [TB_HRCARD]([DCNO],[PHOTO]) values (@DCNO,@PHOTO)";
                                if (IsCanUpdate)
                                {
                                    strSql =strSql+"\r\n"+ "if exists ( select * from [TB_HRCARD] where dcno = @DCNO) update [TB_HRCARD] SET [PHOTO] = @PHOTO WHERE [DCNO] =@DCNO";
                                }
                                dao.AddParameter("@DCNO", strDcno, TypeDao.VarChar, 15);
                                dao.AddParameter("@PHOTO", imageBytes, TypeDao.Image, 0);
                                dao.ExecuteNonQuery(CommandType.Text, strSql, dao.GetParameters());

                                String strErrLog = this.strsuccess + strPhotoName;
                                this.WriteErrLog(strErrLog);
                            }
                            else
                            {
                                String strErrLog = this.strprintfailinfo3 + strPhotoName;
                                this.WriteErrLog(strErrLog);
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
            AlertMessageBox(this.Page, ex.ToString().Replace("'", "\"").Replace("\n", "").Replace("\r", ""));
        }
    }

    /// <summary>
    /// 读取服务器中照片文件返回二进制
    /// </summary>
    /// <param name="strPhotoName"></param>
    /// <param name="success"></param>
    /// <returns></returns>
    private byte[] ReadPhotoFile(string strPhotoName, ref bool success)
    {
        if (Directory.Exists(this.strPhotoPath))
        {
            string strFilePathAndName = Path.Combine(this.strPhotoPath, strPhotoName);
            if (File.Exists(strFilePathAndName))
            {
                using (FileStream fs = new FileStream(strFilePathAndName, FileMode.Open))
                {
                    byte[] imagebytes = new byte[fs.Length];
                    BinaryReader br = new BinaryReader(fs);
                    imagebytes = br.ReadBytes(Convert.ToInt32(fs.Length));
                    success = true;
                    return imagebytes;
                }
            }
            else
            {
                return null;
            }
        }
        else
        {
            success = false;
            return null;
        }
    }

    /// <summary>
    /// 写操作日志
    /// </summary>
    /// <param name="strErrLog"></param>
    private void WriteErrLog(String strErrLog)
    {
        this.txtErrLog.Visible = true;

        if (!String.IsNullOrEmpty(strErrLog))
        {
            this.txtErrLog.Text = this.txtErrLog.Text + "\r\n" + strErrLog;
        }
    }

}
