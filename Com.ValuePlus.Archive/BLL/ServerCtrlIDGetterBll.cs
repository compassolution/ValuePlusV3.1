using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Com.ValuePlus.Archive.DAL;

namespace Com.ValuePlus.Archive.BLL
{
    /// <summary>
    /// 服务器端自动生成控件的ID生成类
    /// </summary>
    public class ServerCtrlIDGetterBll
    {
        /// <summary>
        /// 获取某模板的的主表信息的数据表名
        /// </summary>
        /// <param name="strTID"></param>
        /// <param name="strGID"></param>
        /// <returns></returns>
        public static String GetArchiveTableNameByGroup(String strTID, String strGID)
        {
            return strTID + "_" + strGID;
        }

        #region 获取相应服务器端控件的ID
        /// <summary>
        /// 获取某字段关联的所有控件ID的后缀名
        /// </summary>
        /// <param name="strTID"></param>
        /// <param name="strGID"></param>
        /// <param name="strPID"></param>
        /// <returns></returns>
        public static String GetCtrlIDSuffixName(String strTID, String strGID, String strPID)
        {
            return strTID + "_" + strGID + "_" + strPID;
        }

        /// <summary>
        /// 获取某容器控件ID的后缀名
        /// </summary>
        /// <param name="strTID"></param>
        /// <param name="strGID"></param>
        /// <returns></returns>
        public static String GetCtrlIDSuffixName(String strTID, String strGID)
        {
            return strTID + "_" + strGID;
        }

        /// <summary>
        /// 获取某模板的某分组信息显示在页面中的TABID(页签页面类型用)
        /// </summary>
        /// <param name="strTID"></param>
        /// <param name="strGID"></param>
        /// <returns></returns>
        public static String GetCtrlID_Tab(String strTID, String strGID)
        {
            return "tab_" + GetCtrlIDSuffixName(strTID,strGID);
        }

        /// <summary>
        /// 获取某模板存放某分组信息的DIV控件ID(平铺页面类型用)
        /// </summary>
        /// <param name="strTID"></param>
        /// <param name="strGID"></param>
        /// <returns></returns>
        public static String GetCtrlID_GroupDiv(String strTID, String strGID)
        {
            return "gDiv_" + GetCtrlIDSuffixName(strTID, strGID);
        }

        /// <summary>
        /// 获取某模板存放某分组信息标题提示区域的的DIV控件ID(平铺页面类型用)
        /// </summary>
        /// <param name="strTID"></param>
        /// <param name="strGID"></param>
        /// <returns></returns>
        public static String GetCtrlID_GroupTitleDiv(String strTID, String strGID)
        {
            return "gTitleDiv_" + GetCtrlIDSuffixName(strTID, strGID); 
        }

        /// <summary>
        /// 获取某模板存放某分组名称且可以选择的checkBox控件ID(平铺页面类型用)
        /// </summary>
        /// <param name="strTID"></param>
        /// <param name="strGID"></param>
        /// <returns></returns>
        public static String GetCtrlID_GroupCheckBox(String strTID, String strGID)
        {
            return "gCBox_" + GetCtrlIDSuffixName(strTID, strGID); 
        }

        /// <summary>
        /// 获取某模板某分组信息所在PageView的控件ID
        /// </summary>
        /// <param name="strTID"></param>
        /// <param name="strGID"></param>
        /// <returns></returns>
        public static String GetCtrlID_PageView(String strTID,String strGID)
        {
            return "pv_" + GetCtrlIDSuffixName(strTID, strGID);
        }

        /// <summary>
        /// 获取某模板某分组信息所在PageView控件中的页面TABLE的ID
        /// </summary>
        /// <param name="strTID"></param>
        /// <param name="strGID"></param>
        /// <returns></returns>
        public static String GetCtrlID_Table(String strTID, String strGID)
        {
            return "tb_" + GetCtrlIDSuffixName(strTID, strGID);
        }

        /// <summary>
        /// 获取某模板某分组信息所在PageView控件中的页面TABLE中行tr的ID
        /// </summary>
        /// <param name="strTID"></param>
        /// <param name="strGID"></param>
        /// <param name="strIndex"></param>
        /// <returns></returns>
        public static String GetCtrlID_Row(String strTID, String strGID,String strIndex)
        {
            return "tr_" + GetCtrlIDSuffixName(strTID, strGID)+ "_" + strIndex;
        }

        /// <summary>
        /// 获取某字段所显示LABLE的控件ID
        /// </summary>
        /// <param name="strTID"></param>
        /// <param name="strGID"></param>
        /// <param name="strPID"></param>
        /// <returns></returns>
        public static String GetCtrlID_Lable(String strTID, String strGID,String strPID)
        {
            return "label_" + GetCtrlIDSuffixName(strTID, strGID, strPID);
        }

        /// <summary>
        /// 获取某字段显示为TEXTBOX控件的控件ID
        /// </summary>
        /// <param name="strTID"></param>
        /// <param name="strGID"></param>
        /// <param name="strPID"></param>
        /// <returns></returns>
        public static String GetCtrlID_TextBox(String strTID, String strGID, String strPID)
        {
            return "txt_" + GetCtrlIDSuffixName(strTID, strGID, strPID);
        }

        /// <summary>
        /// 获取某字段显示为DropDownList控件的控件ID
        /// </summary>
        /// <param name="strTID"></param>
        /// <param name="strGID"></param>
        /// <param name="strPID"></param>
        /// <returns></returns>
        public static String GetCtrlID_DropDownList(String strTID, String strGID, String strPID)
        {
            return "ddList_" + GetCtrlIDSuffixName(strTID, strGID, strPID);
        }

        /// <summary>
        /// 获取某字段显示为DBTEXTBOX控件的控件ID
        /// </summary>
        /// <param name="strTID"></param>
        /// <param name="strGID"></param>
        /// <param name="strPID"></param>
        /// <returns></returns>
        public static String GetCtrlID_DBTextBox(String strTID, String strGID, String strPID)
        {
            return "dbTxt_" + GetCtrlIDSuffixName(strTID, strGID, strPID);
        }

        /// <summary>
        /// 获取某字段显示为ArchiveImage控件的控件ID
        /// </summary>
        /// <param name="strTID"></param>
        /// <param name="strGID"></param>
        /// <param name="strPID"></param>
        /// <returns></returns>
        public static String GetCtrlID_ImageTextBox(String strTID, String strGID, String strPID)
        {
            return "imgTxt_" + GetCtrlIDSuffixName(strTID, strGID, strPID); 
        }

        /// <summary>
        /// 获取某字段显示为CHECKBOX控件的控件ID
        /// </summary>
        /// <param name="strTID"></param>
        /// <param name="strGID"></param>
        /// <param name="strPID"></param>
        /// <returns></returns>
        public static String GetCtrlID_CheckBox(String strTID, String strGID, String strPID)
        {
            return "ckb_" + GetCtrlIDSuffixName(strTID, strGID, strPID);
        }

        /// <summary>
        /// 获取DATAGRID控件的控件ID
        /// </summary>
        /// <param name="strTID"></param>
        /// <param name="strGID"></param>
        /// <param name="strPID"></param>
        /// <returns></returns>
        public static String GetCtrlID_DataGrid(String strTID, String strGID)
        {
            return "dataGrid_" + GetCtrlIDSuffixName(strTID, strGID);
        }

        /// <summary>
        /// 获取模板附件上传控件的控件ID
        /// </summary>
        /// <param name="strTID"></param>
        /// <param name="strGID"></param>
        /// <param name="strPID"></param>
        /// <returns></returns>
        public static String GetCtrlID_AttachmentCtrl(String strTID, String strGID, String strPID)
        {
            return "attah_" + GetCtrlIDSuffixName(strTID, strGID, strPID);
        }

        /// <summary>
        /// 获取模板明细链接控件的控件ID
        /// </summary>
        /// <param name="strTID"></param>
        /// <param name="strGID"></param>
        /// <param name="strPID"></param>
        /// <returns></returns>
        public static String GetCtrlID_DetailLinkButton(String strTID, String strGID, String strPID)
        {
            return "detailLink_" + GetCtrlIDSuffixName(strTID, strGID, strPID);
        }

        /// <summary>
        /// 获取某字段显示为FreeTextBox控件的控件ID
        /// </summary>
        /// <param name="strTID"></param>
        /// <param name="strGID"></param>
        /// <param name="strPID"></param>
        /// <returns></returns>
        public static String GetCtrlID_FreeTextBox(String strTID, String strGID, String strPID)
        {
            return "freeTxt_" + GetCtrlIDSuffixName(strTID, strGID, strPID);
        }

        #endregion

        /// <summary>
        /// 根据字段ID获取类型并输出控件ID
        /// </summary>
        /// <param name="strTID"></param>
        /// <param name="strGID"></param>
        /// <param name="strPID"></param>
        /// <returns></returns>
        public static String GetCtrlIDByPID(String strTID, String strGID, String strPID)
        {
            String strCtrlID = ""; 
            if ((!String.IsNullOrEmpty(strTID)) && (!String.IsNullOrEmpty(strGID)) && (!String.IsNullOrEmpty(strPID)))
            {
                String strSql = "select * from TB_HRTMPD WHERE TID ='" + strTID + "' AND GID ='" + strGID + "' AND PID ='" + strPID + "'";
                DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
                if ((dt != null) && (dt.Rows.Count > 0))
                {
                    DataRow dr = dt.Rows[0];
                    int iCtrlType = (int)dr["PCTRL"];
                    switch (iCtrlType)
                    {
                        case 0://Edit文本框
                            strCtrlID = GetCtrlID_TextBox(strTID, strGID, strPID);
                            break;
                        case 1://List列表
                            strCtrlID = GetCtrlID_DropDownList(strTID,strGID,strPID);
                            break;
                        case 2://DB List数据列表
                            strCtrlID = GetCtrlID_DBTextBox(strTID, strGID, strPID);
                            break;
                        case 3://Edit isMultilines文本框(多行)
                            strCtrlID = GetCtrlID_TextBox(strTID,strGID,strPID);
                            break;
                        case 4://Edit 逗号分开的金额类型文本框 modify by sammen 20140312
                            strCtrlID = GetCtrlID_TextBox(strTID, strGID, strPID);
                            break;
                        case 5://Edit Picture文本框（图片）
                            strCtrlID = GetCtrlID_ImageTextBox(strTID,strGID,strPID);
                            break;
                        case 6://Edit Widelines文本框（宽行）
                            strCtrlID = GetCtrlID_TextBox(strTID,strGID,strPID);
                            break;
                        case 7://List(Except Master)列表(非主显示)
                            strCtrlID = GetCtrlID_DropDownList(strTID,strGID,strPID);
                            break;
                        case 8://DB List(Except Master)数据列表(非主显示)
                            strCtrlID = GetCtrlID_DBTextBox(strTID, strGID, strPID);
                            break;
                        case 9://Edit Password文本框(密码)
                            strCtrlID = GetCtrlID_TextBox(strTID,strGID,strPID);
                            break;
                        case 10://Attachment附加文件
                            strCtrlID = GetCtrlID_AttachmentCtrl(strTID,strGID,strPID);
                            break;
                        case 11://CheckBox复选框
                            strCtrlID = GetCtrlID_CheckBox(strTID,strGID,strPID);
                            break;
                        case 112://DatePicker日期面板
                            strCtrlID = GetCtrlID_TextBox(strTID,strGID,strPID);
                            break;
                        case 12://DateTimePicker日期时间面板
                            strCtrlID = GetCtrlID_TextBox(strTID,strGID,strPID);
                            break;
                        case 13://Mask Edit格式化编辑
                            break;
                        case 14://List on Server服务器端列表
                            break;
                        case 15://Tree List树形列表
                            break;
                        case 115://Multi TreeList多选树控件
                            break;
                        case 16://Edit Customized客户化文本框
                            break;
                        case 17://Word微软字处理
                            break;
                        case 18://Multi Selection多选
                            break;
                        case 19://RichTextBox格式化文本框
                            break;
                        case 20://PrintPage打印页
                            break;
                        case 21://EditMultiTextbox多选列表
                            break;
                        case 22://明细链接页面控件
                            strCtrlID = GetCtrlID_DetailLinkButton(strTID, strGID, strPID);
                            break;
                    }

                }
            }
            return strCtrlID;
        }

        #region 通过服务器控件的ID获取其对应的TID,GID,PID,判断是否是主键值
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strCtrlId"></param>
        /// <param name="iIndex"></param>
        /// <returns></returns>
        private static String GetIDByIndex(String strCtrlId, int iIndex)
        {
            String strReturn = "";
            if (!String.IsNullOrEmpty(strCtrlId))
            {
                String[] strArr = strCtrlId.Split('_');
                if ((strArr != null) && (strArr.Length >= iIndex))
                {
                    strReturn = strArr[iIndex - 1];
                }
            }
            return strReturn;
        }

        /// <summary>
        /// 根据控件ID获取TID
        /// </summary>
        /// <param name="strCtrlID"></param>
        /// <returns></returns>
        public static String GetTIDByCtrlId(String strCtrlID)
        {
            String strTID = GetIDByIndex(strCtrlID, 2);
            return strTID;
        }

        /// <summary>
        /// 根据控件ID获取GID
        /// </summary>
        /// <param name="strCtrlID"></param>
        /// <returns></returns>
        public static String GetGIDByCtrlId(String strCtrlID)
        {
            String strGID = GetIDByIndex(strCtrlID, 3);
            return strGID;
        }

        /// <summary>
        /// 根据控件ID获取PID
        /// </summary>
        /// <param name="strCtrlID"></param>
        /// <returns></returns>
        public static String GetPIDByCtrlId(String strCtrlID)
        {
            String strPID = GetIDByIndex(strCtrlID, 4);
            //add by sammen 20130326 考虑到PID字段名里带一个下横杆的情况
            if (!String.IsNullOrEmpty(GetIDByIndex(strCtrlID, 5)))
            {
                strPID = strPID + "_" + GetIDByIndex(strCtrlID, 5);
            }
            return strPID;
        }

        /// <summary>
        /// 根据控件ID判断是否是主键值
        /// </summary>
        /// <param name="strCtrlID"></param>
        /// <returns></returns>
        public static bool IsKeyFieldCtrl(String strCtrlID) 
        {
            bool isKey = false;

            if (!String.IsNullOrEmpty(strCtrlID))
            {
                String[] strArr = strCtrlID.Split('_');
                if ((strArr != null) && (strArr.Length == 4))
                {
                    String strTID = strArr[1];
                    String strGID = strArr[2];
                    String strPID = strArr[3];
                    String strSql = "select PISKEY FROM TB_HRTMPD WHERE TID='" + strTID + "' AND GID = '" + strGID + "' AND PID='" + strPID + "'";
                    DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
                    if ((dt != null) && (dt.Rows.Count > 0))
                    {
                        String strIsKey = dt.Rows[0]["PISKEY"].ToString();
                        if (!String.IsNullOrEmpty(strIsKey))
                        {
                            if (strIsKey.Equals("1"))
                            {
                                isKey = true;
                            }
                        }
                    }
                }
            }

            return isKey;
        } 
        #endregion

    }
}
