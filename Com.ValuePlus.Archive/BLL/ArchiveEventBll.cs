using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Com.ValuePlus.Archive.DAL;

namespace Com.ValuePlus.Archive.BLL
{
    /// <summary>
    /// 模板设置中的事件配置处理类
    /// </summary>
    public class ArchiveEventBll
    {
        /// <summary>
        /// 动态加载页面中的客户端事件
        /// </summary>
        /// <param name="strTid"></param>
        /// <param name="strSid"></param>
        /// <param name="strBrowserType"></param>
        public static String BuildClientEventScript(String strTid, String strSid, String strBrowserType)
        {
            StringBuilder strBuilderContent = new StringBuilder();
            if ((String.IsNullOrEmpty(strTid)) || (String.IsNullOrEmpty(strSid)))
            {
                return "";
            }

            try
            {
                String strSql = "select * from TB_HRTMPSE where TID = '" + strTid + "' AND SID ='" + strSid + "' AND ERIGHT = 1 order by EID ";
                DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
                if ((dt != null) && (dt.Rows.Count > 0))
                {
                    strBuilderContent.Append("<script language=\"javascript\">\r\n");
                    strBuilderContent.Append("$(document).ready(function () {\r\n");

                    int iCount = dt.Rows.Count;
                    for (int i = 0; i < iCount; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        String strGid = dr["GID"].ToString();
                        String strPid = dr["PID"].ToString();
                        String strEvent = dr["ENAME"].ToString();
                        String strClientName = ServerCtrlIDGetterBll.GetCtrlIDByPID(strTid, strGid, strPid);
                        String strEventName = "Fun_" + strClientName + "_" + strEvent;
                        String strEventContent = dr["ECONT"].ToString();
                        strEventContent = ReplacePidToClientObject(strEventContent, strTid, strGid);

                        ////首先对各个控件添加监听事件
                        //strBuilderContent.Append("	if(document.getElementById(\"" + strClientName + "\")!=null)\r\n");
                        //strBuilderContent.Append("	{\r\n");
                        //strBuilderContent.Append("	    var obj = document.getElementById(\"" + strClientName + "\");\r\n");
                        //strBuilderContent.Append("	    if(window.addEventListener)\r\n");
                        //strBuilderContent.Append("	    { \r\n");
                        //strBuilderContent.Append("	        //其它浏览器的事件代码: Mozilla, Netscape, Firefox\r\n");
                        //strBuilderContent.Append("	        obj.addEventListener('" + strEvent + "', " + strEventName + ", false);\r\n");
                        //strBuilderContent.Append("	    } \r\n");
                        //strBuilderContent.Append("	    else \r\n");
                        //strBuilderContent.Append("	    {\r\n");
                        //strBuilderContent.Append("	        //IE 的事件代码 在原先事件上添加 add 方法\r\n");
                        //strBuilderContent.Append("	        obj.attachEvent('" + strEvent + "'," + strEventName + ");       \r\n");
                        //strBuilderContent.Append("	    }\r\n");
                        //strBuilderContent.Append("	}\r\n");

                        //利用jquery注册事件(Modify by sammen 20170724)
                        // edge浏览器不支持onpropertychange事件，替换成oninput add by sammen 20220104
                        if (strBrowserType.ToLower().Contains("chrome"))
                        {
                            strEvent = strEvent.Replace("onpropertychange", "oninput");
                        }
                        strEvent = strEvent.Substring(2, strEvent.Length - 2);//去掉前两位on字符
                        strBuilderContent.Append("	$('#"+ strClientName + "').unbind('"+ strEvent  + "').bind('"+ strEvent + "', function () {\r\n");
                        strBuilderContent.Append("	    "+strEventName+"();\r\n");
                        strBuilderContent.Append("});\r\n");

                        //然后生成事件体
                        strBuilderContent.Append("	function " + strEventName + "(){\r\n");
                        strBuilderContent.Append("      "+strEventContent + "\r\n");
                        strBuilderContent.Append("	}\r\n");
                    }
                    strBuilderContent.Append("});\r\n");

                    strBuilderContent.Append("</script>\r\n");
                    return strBuilderContent.ToString();
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                return "";
            }
        }


        /// <summary>
        /// 将字符串中两头用%标记出的字符串转化成相应javascript对象
        /// </summary>
        /// <param name="strContent"></param>
        /// <param name="strTid"></param>
        /// <param name="strGid"></param>
        /// <returns></returns>
        private static String ReplacePidToClientObject(String strContent, String strTid, String strGid)
        {
            if (!String.IsNullOrEmpty(strContent))
            {
                String strFiledGid = strGid;
                String[] strArr = strContent.Split('%');
                int iLength = strArr.Length;
                for (int i = 1; i < iLength; i = i + 2)
                {
                    String strFiled = strArr[i];
                    String strPID = strFiled;
                    //支持不同分组中的字段值获取 add by sammen 20180116
                    //支持格式可为"TID_GID.PID"或者"GID_PID"
                    if (strFiled.IndexOf(".") > 0)
                    {
                        ///格式为"TID_GID.PID"
                        if (strFiled.IndexOf("_") > 0)
                        {
                            String tempTIDGID = strFiled.Split('.')[0];
                            strFiledGid = tempTIDGID.Split('_')[1];
                        }
                        else//格式可为"GID.PID"
                        {
                            strFiledGid = strFiled.Split('.')[0];
                        }
                        strPID = strFiled.Split('.')[1];
                    }
                    String strCtrlId = ServerCtrlIDGetterBll.GetCtrlIDByPID(strTid, strFiledGid, strPID);
                    strContent = strContent.Replace("%" + strFiled + "%", "document.getElementById(\"" + strCtrlId + "\")");
                }
                return strContent;
            }
            else
            {
                return strContent;
            }
        }
    }
}
