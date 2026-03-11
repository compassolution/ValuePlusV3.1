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
using ThoughtWorks.QRCode.Codec;
using Com.ValuePlus.Common.Security;

public partial class AppFunction_PrintBarCode_PrintBarCode : PageBase
{
    #region 当前页面局部变量
    private string strBarCodePicPath
    {
        get
        {
            return this.ViewState["strBarCodePicPath"] as string;
        }
        set
        {
            this.ViewState["strBarCodePicPath"] = value;
        }

    }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            //ResourceManager rm = base.GetResourceManager("PrintBarCode");

            //BarCode路径
            this.strBarCodePicPath = "../../" + Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("BarCodePicPath");
            this.strBarCodePicPath = Server.MapPath(this.strBarCodePicPath);
            if (Request.QueryString["tblname"] != null)
            {
                string tblname;
                string colname;
                tblname = Request.QueryString["tblname"].ToString();
                colname = Request.QueryString["colname"].ToString();

                //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
                tblname = SQLInjectionDefense.ReplaceSQLReservedKeyword(tblname);
                colname = SQLInjectionDefense.ReplaceSQLReservedKeyword(colname);

                this.GenerateQRBarCode(tblname, colname);
                string key = "AlertMessage";
                string script = string.Format("alert('{0}');window.close();", "成功创建条码");
                this.ClientScript.RegisterClientScriptBlock(typeof(Page), key, script, true);
                this.ClientScript.RegisterStartupScript(typeof(Page), "Close", "window.opener=null;window.close()", true);
            }
            else
            {
                string key = "AlertMessage";
                string script = string.Format("alert('{0}');window.close();", "配置错误导致创建条码失败，请与系统管理员联系");
                this.ClientScript.RegisterClientScriptBlock(typeof(Page), key, script, true);
                this.ClientScript.RegisterStartupScript(typeof(Page), "Close", "window.opener=null;window.close()", true);
            }
        }

    }

    private void GenerateQRBarCode(string tblname,string colname)
    {
        string strSql;
        try
        {
            strSql = "select  " + colname + " from " + tblname + "";
            DataTable dtbc = SqlParamDao.GetDataTableBySql(strSql);
            if ((dtbc != null) && (dtbc.Rows.Count > 0))
            {
                for (int m = 0; m < dtbc.Rows.Count; m++)
                {
                    DataRow drbc = dtbc.Rows[m];
                    if (drbc[0] == DBNull.Value)
                    {
                        return;
                    }
                    else
                    {
                        this.InsertBarCodePicImageBytes(drbc[0].ToString().Trim(), tblname+"-"+colname, tblname, colname);
                    }
                }
            }
            dtbc.Dispose();
        }
        catch (Exception ex)
        {
            log.Error(ex);
            AlertMessageBox(this.Page, ex.ToString().Replace("'", "\"").Replace("\n", "").Replace("\r", ""));
        }
    }
    /// <summary>
    /// 插入员工照片的二进制数据
    /// </summary>
    /// <param name="strDcno"></param>
    private void InsertBarCodePicImageBytes(String strvalue, String subpath,String tblname,String colname)
    {
        string str = strvalue;
        string strSql;
        QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
        String encoding = "AlphaNumeric";
        if (encoding == "Byte")
        {
            qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
        }
        else if (encoding == "AlphaNumeric")
        {
            qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.ALPHA_NUMERIC;
        }
        else if (encoding == "Numeric")
        {
            qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.NUMERIC;
        }
        qrCodeEncoder.QRCodeScale = 2;
        qrCodeEncoder.QRCodeVersion = 1;

        string errorCorrect = "M";
        if (errorCorrect == "L")
            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.L;
        else if (errorCorrect == "M")
            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
        else if (errorCorrect == "Q")
            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.Q;
        else if (errorCorrect == "H")
            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.H;

        String data = strvalue;
        System.IO.MemoryStream ms = new System.IO.MemoryStream();
        System.Drawing.Image myimg = qrCodeEncoder.Encode(data);
        myimg.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
        string destname = this.strBarCodePicPath + subpath +"\\" + data.ToString() + ".gif";
        if (!Directory.Exists(this.strBarCodePicPath + subpath))
        {
            DirectoryInfo folder = Directory.CreateDirectory(this.strBarCodePicPath + subpath);
        }
        myimg.Save(destname);

        using (FileStream fs = new FileStream(destname, FileMode.Open))
        {
            byte[] buffByte = new byte[fs.Length];
            BinaryReader br = new BinaryReader(fs);
            buffByte = br.ReadBytes(Convert.ToInt32(fs.Length));
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                strSql = "if not exists ( select * from [TB_BARCODEIMG] where tblname = @tblname and colname=@colname and strvalue=@strvalue and bctype=@bctype) insert into [TB_BARCODEIMG](tblname,colname,strvalue,bctype,barcode) values (@tblname,@colname,@strvalue,@bctype,@barcode)";
                dao.AddParameter("@tblname", tblname, TypeDao.VarChar, 50);
                dao.AddParameter("@colname", colname, TypeDao.VarChar, 50);
                dao.AddParameter("@strvalue", strvalue, TypeDao.VarChar, 100);
                dao.AddParameter("@bctype", "QR", TypeDao.VarChar, 10);
                dao.AddParameter("@barcode", buffByte, TypeDao.Image, 0);
                dao.ExecuteNonQuery(CommandType.Text, strSql, dao.GetParameters());

            }
        }

        ms.Close();
        ms = null;
    }

    /// <summary>
    /// 写操作日志
    /// </summary>
    /// <param name="strErrLog"></param>
    private void WriteErrLog(String strErrLog)
    {

    }
}