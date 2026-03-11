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

using Com.ValuePlus.Web;
using System.Drawing;
using Com.ValuePlus.DAL;
using System.Windows.Forms;
using Com.ValuePlus.OrgChart;

public partial class DeptManager_OrganizationChart : PageBase
{
    //protected const String strTableName = "VW_Sys_Department";
    protected const int iImage_Width = 1200;//画布宽度
    protected const int iImage_Height = 600;//画布高度

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            try
            {
                this.DrawDeptChart();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }
    }

    #region 作废
    //private DataTable GetDeptInfoL1()
    //{
    //    String strSql = "select * from " + strTableName + " where LID = 'D1' order by LID";
    //    return  SqlParamDao.GetDataTableBySql(strSql);
    //}

    //private DataTable GetDeptInfoL2(String strParent)
    //{
    //    String strSql = "select * from " + strTableName + " where LID = 'D2' and CUID ='" + strParent + "' order by CID";
    //    return  SqlParamDao.GetDataTableBySql(strSql);
    //}

    //private DataTable GetDeptInfoL3(String strParent)
    //{
    //    String strSql = "select * from " + strTableName + " where LID = 'D3' and CUID ='" + strParent + "' order by CID";
    //    return  SqlParamDao.GetDataTableBySql(strSql);
    //}

    //private void GraphicsImage()
    //{
    //    Bitmap image = new Bitmap(iImage_Width, iImage_Height); 
    //    Graphics g = Graphics.FromImage(image); //创建画布 
    //    try 
    //    {
    //        g.Clear(Color.YellowGreen); //清空背景色 
    //        Font font1 = new Font("宋体", 12); //设置字体类型和大小 
    //        Brush brush = new SolidBrush(Color.Red); //设置画刷颜色 
    //        Pen pen = new Pen(Color.Blue,1); //创建画笔对象 
    //        g.DrawString("GDI+绘制直线、矩形和多边形", font1, brush, 100, 20); 
    //        //*****首先画第一级别组织架构
    //        this.DrawDeptLevel1(g);



    //        //g.DrawLine(pen, 40, 100, 100, 80); //绘制直线 pen：确定线条的颜色、宽度和样式。 
    //        ////pt1(40,80)：表示要连接的第一个点;pt2(100,80)：表示要连接的第二个点。 
    //        //g.DrawRectangle(pen, 130, 60, 100, 40); //绘制矩形 pen：确定矩形的颜色、宽度和样式。
    //        //g.DrawString("上海佘山酒店", font1, brush, 150, 80);  
    //        //x(130)：要绘制矩形的左上角的x坐标；y(60)：要绘制矩形的左上角的y坐标； 
    //        //width(100)：要绘制矩形的宽度；height(40)：要绘制矩形的高度。 
    //        //Point[] points = new Point[6]; 
    //        //points[0].X=300; 
    //        //points[0].Y=60; 
    //        //points[1].X=250; 
    //        //points[1].Y=80; 
    //        //points[2].X=300; 
    //        //points[2].Y=100; 
    //        //points[3].X=350; 
    //        //points[3].Y=100; 
    //        //points[4].X=400; 
    //        //points[4].Y=80; 
    //        //points[5].X=350; 
    //        //points[5].Y=60; 
    //        //g.DrawPolygon(pen, points); //绘制多边形 pen：确定多边形的颜色、宽度和样式；points：表示多边形的顶点。
    //        System.IO.MemoryStream ms = new System.IO.MemoryStream( );
    //        image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
    //        g.Dispose();
    //        Response.ClearContent( ); 
    //        Response.ContentType = "image/Gif"; 
    //        Response.BinaryWrite(ms.ToArray( )); 
    //    } 
    //    catch(Exception ms) 
    //    { 
    //        Response.Write(ms.Message); 
    //    } 
    //}

    //private void DrawDeptLevel1(Graphics g)
    //{
    //    DataTable dt = this.GetDeptInfoL1();
    //    if ((dt != null) & (dt.Rows.Count > 0))
    //    {
    //        int iCount = dt.Rows.Count;

    //        Font font1 = new Font("宋体", 10); //设置字体类型和大小 
    //        Brush brush = new SolidBrush(Color.Red); //设置画刷颜色 
    //        Pen pen = new Pen(Color.Blue, 1); //创建画笔对象 

    //        for (int i = 0; i < iCount; i++)
    //        {
    //            DataRow dr = dt.Rows[i];
    //            String strDeptId = dr["CID"].ToString();
    //            String strDeptName = dr["CDESC"].ToString();
    //            if (base.Language.Equals("zh-cn"))
    //            {
    //                strDeptName = dr["CDESCCHS"].ToString();
    //            }

    //            g.DrawRectangle(pen, 130, 60, 100, 40); //绘制矩形 pen：确定矩形的颜色、宽度和样式。
    //            g.DrawString("上海佘山酒店", font1, brush, 150, 80);  
    //        }
    //    }
    //}

    //private void DrawChart()
    //{
    //    DataTable dt1 = this.GetDeptInfoL1();
    //    if ((dt1 != null) && (dt1.Rows.Count > 0))
    //    {
    //        DataRow dr1 = dt1.Rows[0];
    //        String strDeptId1 = dr1["CID"].ToString();
    //        String strDeptName1 = dr1["CDESC"].ToString();
    //        if (base.Language.Equals("zh-cn"))
    //        {
    //            strDeptName1 = dr1["CDESCCHS"].ToString();
    //        }
    //        OrgNode OrgNode1 = new OrgNode();
    //        OrgNode1.Text = strDeptName1;
    //        OrgNode1.Description = strDeptName1;
    //        OrgNode1.Type = "ROLES";
    //        OrgNode1.ImageUrl = "../common/images/down_list.gif";
    //        OrgNode1.LayoutFlow = OrgNode.LabelLayoutFlow.Horizontal;

    //        DataTable dt2 = this.GetDeptInfoL2(strDeptId1);
    //        if ((dt2 != null) && (dt2.Rows.Count > 0))
    //        {
    //            for (int j = 0; j < dt2.Rows.Count; j++)
    //            {
    //                DataRow dr2 = dt2.Rows[j];
    //                String strDeptId2 = dr2["CID"].ToString();
    //                String strDeptName2 = dr2["CDESC"].ToString();
    //                if (base.Language.Equals("zh-cn"))
    //                {
    //                    strDeptName2 = dr2["CDESCCHS"].ToString();
    //                }
    //                OrgNode OrgNode2 = new OrgNode();
    //                OrgNode2.Text = strDeptName2;
    //                OrgNode2.Description = strDeptName2;
    //                OrgNode2.Type = "ROLES";
    //                //OrgNode2.ImageUrl = "../common/images/down_list.gif";
    //                OrgNode2.LayoutFlow = OrgNode.LabelLayoutFlow.Vertical;

    //                DataTable dt3 = this.GetDeptInfoL3(strDeptId2);
    //                if ((dt3 != null) && (dt3.Rows.Count > 0))
    //                {
    //                    for (int k = 0; k < dt3.Rows.Count; k++)
    //                    {
    //                        DataRow dr3 = dt3.Rows[k];
    //                        String strDeptId3 = dr3["CID"].ToString();
    //                        String strDeptName3 = dr3["CDESC"].ToString();
    //                        if (base.Language.Equals("zh-cn"))
    //                        {
    //                            strDeptName3 = dr3["CDESCCHS"].ToString();
    //                        }
    //                        OrgNode OrgNode3 = new OrgNode();
    //                        OrgNode3.Text = strDeptName3;
    //                        OrgNode3.Description = strDeptName3;
    //                        OrgNode3.Type = "ROLES";
    //                        //OrgNode3.ImageUrl = "../common/images/down_list.gif";
    //                        OrgNode3.LayoutFlow = OrgNode.LabelLayoutFlow.Vertical;

    //                        OrgNode2.Nodes.Add(OrgNode3);
    //                    }
    //                }

    //                OrgNode1.Nodes.Add(OrgNode2);

    //            }
    //        }
    //        OrgChart1.Node = OrgNode1;
    //        OrgChart1.LineColor = System.Drawing.Color.Green;
    //        OrgChart1.LineWidth = 30;
    //        OrgChart1.ChartStyle = OrgChart.Orientation.Vertical;
    //    }

    //    //OrgNode OrgNode6 = new OrgNode();
    //    //OrgNode6.Text = "Exc";
    //    //OrgNode6.Description = "Exc";
    //    //OrgNode6.Type = "ROLES";

    //    //OrgNode OrgNode7 = new OrgNode();
    //    //OrgNode7.Text = "KIT";
    //    //OrgNode7.Description = "KIT";
    //    //OrgNode7.Type = "ROLES";

    //    //OrgNode OrgNode8 = new OrgNode();
    //    //OrgNode8.Text = "SPA";
    //    //OrgNode8.Description = "SPA";
    //    //OrgNode8.Type = "ROLES";

    //    //OrgNode OrgNode2 = new OrgNode();
    //    //OrgNode2.Text = "Fin";
    //    //OrgNode2.Description = "Fin Dept";
    //    //OrgNode2.Type = "ROLES";

    //    //OrgNode OrgNode3 = new OrgNode();
    //    //OrgNode3.Text = "HR";
    //    //OrgNode3.Description = "HR Dept";
    //    //OrgNode3.Type = "ROLES";

    //    //OrgNode OrgNode4 = new OrgNode();
    //    //OrgNode4.Text = "IT";
    //    //OrgNode4.Description = "IT Sec";
    //    //OrgNode4.Type = "ROLES";

    //    //OrgNode OrgNode5 = new OrgNode();
    //    //OrgNode5.Text = "Floor";
    //    //OrgNode5.Description = "Floor";
    //    //OrgNode5.Type = "ROLES";

    //    //OrgNode1.Nodes.Add(OrgNode3);
    //    //OrgNode1.Nodes.Add(OrgNode6);
    //    //OrgNode1.Nodes.Add(OrgNode7);
    //    //OrgNode2.Nodes.Add(OrgNode8);
    //    //OrgNode3.Nodes.Add(OrgNode5);
    //    //OrgNode3.Nodes.Add(OrgNode2);
    //    //OrgNode3.Nodes.Add(OrgNode4);

    //    //OrgNode1.ImageUrl = "s5.gif";
    //    //OrgNode2.ImageUrl = "s5.gif";
    //    //OrgNode3.ImageUrl = "s5.gif";
    //    //OrgNode4.ImageUrl = "s5.gif";
    //    //OrgNode5.ImageUrl = "s5.gif";
    //    //OrgNode6.ImageUrl = "s5.gif";
    //    //OrgNode7.ImageUrl = "s5.gif";
    //    //OrgNode8.ImageUrl = "s5.gif";

    //    //OrgChart OrgChart1 = new OrgChart();
    //}
    #endregion

    /// <summary>
    /// 根据表TB_HR_DEPT数据划出组织架构图
    /// </summary>
    private void DrawDeptChart()
    {
        String strSql = "select * from TB_HR_DEPT where SPARENTDEPTID is null or SPARENTDEPTID=''";
        DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
        if ((dt != null) && (dt.Rows.Count > 0))
        {
            DataRow dr = dt.Rows[0];
            String strDeptId = dr["SDEPTID"].ToString();
            String strDeptName = dr["SDEPTNAME"].ToString();
            if (base.Language.Equals("zh-cn"))
            {
                strDeptName = dr["SDEPTNAMECN"].ToString();
            }
            OrgNode OrgNode = new OrgNode();
            OrgNode.Text = strDeptName;
            OrgNode.Description = strDeptName;
            OrgNode.Type = "ROLES";
            OrgNode.ImageUrl = "../common/images/down_list.gif";
            OrgNode.LayoutFlow = OrgNode.LabelLayoutFlow.Horizontal;

            this.DrawSubDeptChart(strDeptId, OrgNode);

            OrgChart1.Node = OrgNode;
            OrgChart1.LineColor = System.Drawing.Color.Green;
            OrgChart1.LineWidth = 30;
            OrgChart1.ChartStyle = OrgChart.Orientation.Vertical;
        }

    }

    /// <summary>
    /// 根据表TB_HR_DEPT数据划出各子节点的组织架构图
    /// </summary>
    /// <param name="strParentDeptId"></param>
    /// <param name="OrgNode_Parent"></param>
    private void DrawSubDeptChart(String strParentDeptId, OrgNode OrgNode_Parent)
    {
        String strSql = "select * from TB_HR_DEPT where SPARENTDEPTID = '" + strParentDeptId + "' ORDER BY SDEPTID";
        DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
        if ((dt != null) && (dt.Rows.Count > 0))
        {
            for (int j = 0; j < dt.Rows.Count; j++)
            {
                DataRow dr = dt.Rows[j];
                String strDeptId = dr["SDEPTID"].ToString();
                String strDeptName = dr["SDEPTNAME"].ToString();
                if (base.Language.Equals("zh-cn"))
                {
                    strDeptName = dr["SDEPTNAMECN"].ToString();
                }
                OrgNode OrgNode = new OrgNode();
                OrgNode.Text = strDeptName;
                OrgNode.Description = strDeptName;
                OrgNode.Type = "ROLES";
                //OrgNode2.ImageUrl = "../common/images/down_list.gif";
                OrgNode.LayoutFlow = OrgNode.LabelLayoutFlow.Vertical;

                this.DrawSubDeptChart(strDeptId, OrgNode);

                OrgNode_Parent.Nodes.Add(OrgNode);

            }
        }
    }
}
