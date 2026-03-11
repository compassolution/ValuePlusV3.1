using System;
using System.Text;
using Com.ValuePlus.Archive.DAL;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;

namespace Com.ValuePlus.Archive.BLL
{
    public class ArchiveFontSetterBll
    {
        /// <summary>
        /// 设置LABLE的色彩方案
        /// </summary>
        /// <param name="lb"></param>
        /// <param name="strFontL"></param>
        public static void SetLabelFont(Label lb, string strFontL)
        {
            if (!String.IsNullOrEmpty(strFontL))
            {
                string strSql = "SELECT * FROM TB_HRFONT WHERE FID='" + strFontL + "'";
                DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    lb.Font.Name = row["FNAME"].ToString();
                    lb.Font.Size = FontUnit.Parse(row["FSIZE"].ToString());
                    lb.Font.Bold = Convert.ToBoolean(row["FBOLD"].ToString());
                    lb.Font.Italic = Convert.ToBoolean(row["FITALIC"].ToString());
                    lb.Font.Overline = Convert.ToBoolean(row["FOVERLINE"].ToString());
                    lb.Font.Strikeout = Convert.ToBoolean(row["FSTRIKEOUT"].ToString());
                    lb.Font.Underline = Convert.ToBoolean(row["FUNDERLINE"].ToString());
                    lb.ForeColor = ColorTranslator.FromHtml(row["FCOLOR"].ToString());
                }
            }
        }

        /// <summary>
        /// 设置TEXTBOX的色彩方案
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="strFontC"></param>
        public static void SetTextBoxFont(TextBox txt, string strFontC)
        {
            if (!String.IsNullOrEmpty(strFontC))
            {
                string strSql = "SELECT * FROM TB_HRFONT WHERE FID='" + strFontC + "'";
                DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    txt.Font.Name = row["FNAME"].ToString();
                    txt.Font.Size = FontUnit.Parse(row["FSIZE"].ToString());
                    txt.Font.Bold = Convert.ToBoolean(row["FBOLD"].ToString());
                    txt.Font.Italic = Convert.ToBoolean(row["FITALIC"].ToString());
                    txt.Font.Overline = Convert.ToBoolean(row["FOVERLINE"].ToString());
                    txt.Font.Strikeout = Convert.ToBoolean(row["FSTRIKEOUT"].ToString());
                    txt.Font.Underline = Convert.ToBoolean(row["FUNDERLINE"].ToString());
                    txt.ForeColor = ColorTranslator.FromHtml(row["FCOLOR"].ToString());
                }
            }
        }
    }
}
