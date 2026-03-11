using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Web;

namespace Com.ValuePlus.Common.TipData
{
    public class TipData
    {
        #region 读取提示信息
        /// <summary>
        /// 读取提示信息
        /// </summary>
        /// <param name="tipName">提示的名称</param>
        /// <param name="values">参数</param>
        /// <returns>提示语</returns>
        public static string GetTipText(string tipName, params string[] values)
        {
            DataTable dt = new DataTable();

            if (HttpContext.Current.Cache[CacheName.TipsCacheName] == null)
            {
                dt.Clear();//读取之前先清除。
                dt.ReadXmlSchema(HttpContext.Current.Server.MapPath("~/TipData/TipsSchema.xsd"));
                dt.ReadXml(HttpContext.Current.Server.MapPath("~/TipData/Tips.xml"));
                HttpContext.Current.Cache[CacheName.TipsCacheName] = dt;//永久缓存
                ////将缓存操作写入日志
                //DevDept.Lib.APPUtils.APPUtils_Log.APPUtils_Log_Manager.WriteLog(
                //      "缓存名：" + CacheName.GetTipsCacheName() + "；"
                //    + "操作：缓存写入；"
                //    + "时间：" + DateTime.Now);
            }
            else
            {
                dt = HttpContext.Current.Cache[CacheName.TipsCacheName] as DataTable;
            }
            DataRow[] drArray = dt.Select("Name='" + tipName + "'");
            if (drArray != null && drArray.Length > 0)
            {
                string retValue = "";

                if (values.Length > 0)
                {
                    retValue = string.Format(drArray[0]["Values"].ToString(), values);
                }
                else
                {
                    retValue = drArray[0]["Values"].ToString();
                }

                return retValue;
            }
            return "";
        }
        #endregion
    }
}
