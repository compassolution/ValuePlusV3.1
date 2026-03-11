using System;
using System.Collections;
using System.Data;
using System.Collections.Generic;
using System.Text;
using Com.ValuePlus.DAL;

namespace Com.ValuePlus.BLL.SysManager
{
    public class DeptManagerBll
    {
        #region 获得所有栏目树
        /// <summary>
        /// 获得所有栏目树
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public string GetDeptTreeAll(string language)
        {
            string sResult = string.Empty;
            String strSql = "select * from TB_HR_DEPT ORDER BY SLEVEL,SPARENTDEPTID,SDEPTID";
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
            if (dt != null && dt.Rows.Count > 0)
            {
                CreateTreeXmlStructure(dt, language, ref sResult);
            }
            return sResult;
        }
        #endregion

        #region 创建树结构
        /// <summary>
        /// 创建树结构
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="language"></param>
        /// <param name="sResult"></param>
        private void CreateTreeXmlStructure(DataTable dt, string language, ref string sResult)
        {
            sResult = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><tree id=\"0\">";
            //首先创建根节点
            DataRow[] drs = dt.Select("SPARENTDEPTID is null or SPARENTDEPTID = ''");
            if (drs != null && drs.Length > 0)
            {
                string sTitle = drs[0]["SDEPTNAME"].ToString() ;
                //string sTitle = drs[0]["SDEPTNAME"].ToString() + "【" + drs[0]["SDEPTID"].ToString() + "】";
                if (language == "zh-cn")
                {
                    sTitle = drs[0]["SDEPTNAMECN"].ToString();
                    //sTitle = drs[0]["SDEPTNAMECN"].ToString() + "【" + drs[0]["SDEPTID"].ToString() + "】";
                }
                sTitle = Com.ValuePlus.Utils.HtmlUtils.HtmlEncode(sTitle);
                
                sResult = sResult + "<item id=\"" + drs[0]["SDEPTID"].ToString() + "\" open=\"true\" text=\"" + sTitle + "\" tooltip=\"" + sTitle + "\" >";
                
                //sResult = sResult + "<userdata name=\"url\">";
                //sResult = sResult + "DeptDetail.aspx?DeptCode=" + drs[0]["SDEPTID"].ToString();
                //sResult = sResult + "</userdata>";

                //创建子节点
                CreateSubTreeXmlStructure(dt, drs[0]["SDEPTID"].ToString(), language, ref sResult);
                sResult += "</item>";
            }
            sResult += "</tree>";
        }
        #endregion

        #region 创建树子节点结构
        /// <summary>
        /// 创建树子节点结构
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="id"></param>
        /// <param name="language"></param>
        /// <param name="sResult"></param>
        private void CreateSubTreeXmlStructure(DataTable dt, string id, string language, ref string sResult)
        {
            DataRow[] drs = dt.Select("SPARENTDEPTID = '" + id + "'", "SDEPTID ASC");
            if (drs != null && drs.Length > 0)
            {
                foreach (DataRow dr in drs)
                {
                    string sTitle = dr["SDEPTNAME"].ToString() ;
                    //string sTitle = dr["SDEPTNAME"].ToString() + "【" + dr["SDEPTID"].ToString() + "】";
                    if (language == "zh-cn")
                    {
                        sTitle = dr["SDEPTNAMECN"].ToString();
                        //sTitle = dr["SDEPTNAMECN"].ToString() + "【" + dr["SDEPTID"].ToString() + "】";
                    }
                    sTitle = Com.ValuePlus.Utils.HtmlUtils.HtmlEncode(sTitle);
                    
                    sResult = sResult + "<item id=\"sub" + dr["SDEPTID"].ToString() + "\" text=\"" + sTitle + "\"  tooltip=\"" + sTitle + "\" >";
                    
                    //sResult = sResult + "<userdata name=\"url\">";
                    //sResult = sResult + "DeptDetail.aspx?DeptCode=" + dr["SDEPTID"].ToString();
                    //sResult = sResult + "</userdata>";

                    //创建子节点
                    CreateSubTreeXmlStructure(dt, dr["SDEPTID"].ToString(), language, ref sResult);
                    sResult += "</item>";
                }
            }

        }
        #endregion

    }
}
