using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using Com.ValuePlus.Archive.WebCtrls;
using Com.ValuePlus.Archive.Entity;
using System.Data;
using Com.ValuePlus.Archive.BLL;
using Com.ValuePlus.Archive.Property;
using Com.ValuePlus.Archive.DAL;
using System.Collections;
using System.Drawing;
using Com.ValuePlus.Archive.Utils;
using Com.ValuePlus.Archive.Config;
using Com.ValuePlus.DataLog;
using Com.ValuePlus.DataLog.Entity;
using System.Resources;
using System.IO;

namespace Com.ValuePlus.Web
{
    public class ArchivePageBase : PageBase
    {
        #region  属性定义
        //private string _TID;
        //private string _RID;
        //private string _SID;
        //private string _GID;
        //private string _KEY;
        //private string _KEYTYPE;
        //private string _KEYVALUE;
        //private string _GRIDKEY;
        //private string _GRIDKEYVALUE;
        //private string _OPTYPE;
        //private int _iTabelCellNumSetting;
        //private string _strLabelWidthSetting;
        //private string _strCtrlIDSuffix;

        /// <summary>
        /// 中英文资源
        /// </summary>
        public ResourceManager rmLanguageResource
        {
            get
            {
                return ViewState["rmLanguageResource"] as ResourceManager;
            }
            set
            {
                ViewState["rmLanguageResource"] = value;
            }
        }

        /// <summary>
        /// 模板TID
        /// </summary>
        public string TID
        {
            get
            {
                return ViewState["TID_ViewState"] as string;
            }
            set
            {
                ViewState["TID_ViewState"] = value;
            }
        }
        /// <summary>
        /// 角色RID
        /// </summary>
        public string RID
        {
            get
            {
                return ViewState["RID_ViewState"] as string;
            }
            set
            {
                ViewState["RID_ViewState"] = value;
            }
        }
        /// <summary>
        /// 场景SID
        /// </summary>
        public string SID
        {
            get
            {
                return ViewState["SID_ViewState"] as string;
            }
            set
            {
                ViewState["SID_ViewState"] = value;
            }
        }
        /// <summary>
        /// 分组GID
        /// </summary>
        public string GID
        {
            get
            {
                return ViewState["GID_ViewState"] as string;
            }
            set
            {
                ViewState["GID_ViewState"] = value;
            }
        }
        /// <summary>
        /// 模板信息唯一主键字段
        /// </summary>
        public string KEY
        {
            get
            {
                return ViewState["KEY_ViewState"] as string;
            }
            set
            {
                ViewState["KEY_ViewState"] = value;
            }
        }
        /// <summary>
        /// 模板信息唯一主键字段类型
        /// </summary>
        public string KEYTYPE
        {
            get
            {
                return ViewState["KEYTYPE_ViewState"] as string;
            }
            set
            {
                ViewState["KEYTYPE_ViewState"] = value;
            }
        }
        /// <summary>
        /// 模板信息唯一主键值
        /// </summary>
        public string KEYVALUE
        {
            get
            {
                return ViewState["KEYVALUE_ViewState"] as string;
            }
            set
            {
                ViewState["KEYVALUE_ViewState"] = value;
            }
        }
        /// <summary>
        /// 模板某表格信息记录唯一主键字段
        /// </summary>
        public string GRIDKEY
        {
            get
            {
                return ViewState["GRIDKEY_ViewState"] as string;
            }
            set
            {
                ViewState["GRIDKEY_ViewState"] = value;
            }
        }
        /// <summary>
        /// 模板某表格信息记录唯一主键值
        /// </summary>
        public string GRIDKEYVALUE
        {
            get
            {
                return ViewState["GRIDKEYVALUE_ViewState"] as string;
            }
            set
            {
                ViewState["GRIDKEYVALUE_ViewState"] = value;
            }
        }
        /// <summary>
        /// 操作类型（add/edit/readonly）
        /// </summary>
        public string OPTYPE
        {
            get
            {
                return ViewState["OPTYPE_ViewState"] as string;
            }
            set
            {
                ViewState["OPTYPE_ViewState"] = value;
            }
        }
        /// <summary>
        /// 表格列数设置
        /// </summary>
        public int iTabelCellNumSetting
        {
            get
            {
                if (ViewState["iTabelCellNumSetting_ViewState"] != null)
                {
                    return (int)ViewState["iTabelCellNumSetting_ViewState"];
                }
                else
                {
                    return 6;
                }
            }
            set
            {
                ViewState["iTabelCellNumSetting_ViewState"] = value;
            }
        }
        /// <summary>
        /// 表格LABEL列的宽度
        /// </summary>
        public string strLabelWidthSetting
        {
            get
            {
                return ViewState["strLabelWidthSetting_ViewState"] as string;
            }
            set
            {
                ViewState["strLabelWidthSetting_ViewState"] = value;
            }
        }
        /// <summary>
        /// 控件ID的前缀
        /// </summary>
        public string strCtrlIDSuffix
        {
            get
            {
                return ViewState["strCtrlIDSuffix_ViewState"] as string;
            }
            set
            {
                ViewState["strCtrlIDSuffix_ViewState"] = value;
            }
        }
        /// <summary>
        /// 当前用户相关参数
        /// </summary>
        public Hashtable hsCurUserParamValue
        {
            get
            {
                if (ViewState["hsCurUserParamValue"] == null)
                {
                    return new Hashtable();
                }
                return (Hashtable)ViewState["hsCurUserParamValue"];
            }
            set
            {
                ViewState["hsCurUserParamValue"] = value;
            }
        }
        /// <summary>
        /// 当前用户的当前角色下的参数值
        /// </summary>
        public Hashtable hsCurRoleParamValue
        {
            get
            {
                if (ViewState["hsCurRoleParamValue"] == null)
                {
                    return new Hashtable();
                }
                return (Hashtable)ViewState["hsCurRoleParamValue"];
            }
            set
            {
                ViewState["hsCurRoleParamValue"] = value;
            }
        }

        #endregion

        #region 创建页面控件
        /// <summary>
        /// 创建Table
        /// </summary>
        /// <param name="entityHRTMPSG"></param>
        /// <param name="strTabelWidthSetting"></param>
        /// <returns>TableCell</returns>
        public Table CreateHtmlTable(Entity_TB_HRTMPSG entityHRTMPSG, String strTabelWidthSetting)
        {
            Table tb = new Table();
            tb.ID = ServerCtrlIDGetterBll.GetCtrlID_Table(entityHRTMPSG.TID, entityHRTMPSG.GID);
            tb.CssClass = "table_archive";
            int iWidth = 0;
            if (!String.IsNullOrEmpty(strTabelWidthSetting))
            {
                if (strTabelWidthSetting.IndexOf("%") > -1)
                {
                    iWidth = int.Parse(strTabelWidthSetting.Substring(0, strTabelWidthSetting.Length - 1));
                    tb.Width = Unit.Percentage(iWidth);
                }
                else
                {
                    iWidth = int.Parse(strTabelWidthSetting);
                    tb.Width = Unit.Pixel(iWidth);
                }
            }
            //设置Edge浏览器兼容时加上这句
            tb.Style.Add("table-layout", "fixed");

            return tb;
        }

        /// <summary>
        /// 创建TableRow
        /// </summary>
        /// <param name="tb"></param>
        /// <returns>TableCell</returns>
        public TableRow CreateTableRow(Table tb)
        {
            TableRow row = new TableRow();
            row.CssClass = "tr_Normal";
            tb.Rows.Add(row);
            return row;
        }

        /// <summary>
        /// 创建TableCell
        /// </summary>
        /// <param name="row"></param>
        /// <returns>TableCell</returns>
        public TableCell CreateTableCell(TableRow row)
        {
            TableCell cell = new TableCell();
            cell.CssClass = "td_Normal_Label";
            row.Cells.Add(cell);
            return cell;
        }

        /// <summary>
        /// 创建Label
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="PID"></param>
        /// <param name="PDESC"></param>
        /// <param name="isHeader"></param>
        /// <param name="FontL"></param>
        /// <returns></returns>
        public Label CreateLabel(TableCell cell, Entity_TB_HRTMPSD entityHRTMPSD, String strLanguage)
        {
            Label lb = new Label();
            Label lb_Star = new Label();//必填提示label

            lb.ID = ServerCtrlIDGetterBll.GetCtrlID_Lable(entityHRTMPSD.TID, entityHRTMPSD.GID, entityHRTMPSD.PID);
            if (entityHRTMPSD.PTYPE.ToUpper().Equals("CH"))//页眉label
            {
                cell.CssClass = "td_CH_Label";
                cell.ColumnSpan = this.iTabelCellNumSetting;
                lb.CssClass = "label_CH_Archive";
                lb.ToolTip = "HIDE/SHOW";
                lb.Text = ">>";
                lb.Style.Add("cursor", "hand");
            }
            else
            {
                lb.CssClass = "label_edit_Archive";
                cell.CssClass = "td_Normal_Label";
                if (entityHRTMPSD.PNULL == 0)//必填项
                {
                    //lb.Text = lb.Text + "*";
                    lb_Star.Text = "*";
                    lb_Star.ForeColor = Color.Red;
                }
                //设置CELL宽度
                int iLableCellWidth = 0;
                if (!String.IsNullOrEmpty(this.strLabelWidthSetting))
                {
                    if (this.strLabelWidthSetting.IndexOf("%") > -1)
                    {
                        iLableCellWidth = int.Parse(this.strLabelWidthSetting.Substring(0, this.strLabelWidthSetting.Length - 1));
                        cell.Width = Unit.Percentage(iLableCellWidth);
                    }
                    else
                    {
                        iLableCellWidth = int.Parse(strLabelWidthSetting);
                        cell.Width = Unit.Pixel(iLableCellWidth);
                    }
                }
            }

            if (strLanguage.Equals("zh-cn"))
            {
                lb.Text = lb.Text+entityHRTMPSD.PDESCCHS;
            }
            else
            {
                lb.Text = lb.Text+entityHRTMPSD.PDESC;
            }

            cell.Controls.Add(lb);
            cell.Controls.Add(lb_Star);
            return lb;
        }

        /// <summary>
        /// 根据配置创建不同控件到CELL
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="entityHRTMPSD"></param>
        /// <param name="dtRecord"></param>
        /// <param name="strGroupType"></param>
        public void CreateWebControlToCell(TableCell cell, Entity_TB_HRTMPSD entityHRTMPSD, DataTable dtRecord, String strGroupType)
        {

            //设置CELL宽度
            if ((!String.IsNullOrEmpty(this.strLabelWidthSetting)) && (this.iTabelCellNumSetting > 0))
            {
                if (this.strLabelWidthSetting.IndexOf("%") > -1)
                {
                    int iLableCellWidth = int.Parse(this.strLabelWidthSetting.Substring(0, this.strLabelWidthSetting.Length - 1));
                    int iControlCellWidth = 100 / (this.iTabelCellNumSetting / 2) - iLableCellWidth;
                    cell.Width = Unit.Percentage(iControlCellWidth);
                }
            }

            int iCtrlType = entityHRTMPSD.PCTRL.Value;
            //设置控件ID的后缀
            this.strCtrlIDSuffix = ServerCtrlIDGetterBll.GetCtrlIDSuffixName(entityHRTMPSD.TID, entityHRTMPSD.GID, entityHRTMPSD.PID);

            String strOpType = this.OPTYPE;
            //获取当前字段分组的权限---------add by sammen 20121026
            String strSG = "SELECT * FROM TB_HRTMPSG WHERE TID = '" + entityHRTMPSD.TID + "' AND SID = '" + entityHRTMPSD.SID + "' AND GID = '" + entityHRTMPSD.GID + "'";
            DataTable dt = SqlParamDao.GetDataTableBySql(strSG);
            if ((dt != null)&&(dt.Rows.Count==1))
            {
                String strSGRight = dt.Rows[0]["GRIGHT"].ToString();
                switch (strSGRight)
                {
                    case "0"://只读
                        strOpType = "readonly";
                        break;
                }
            }

            switch (iCtrlType)
            {
                case 0://Edit文本框
                    this.CreateGeneralTextBox(cell, entityHRTMPSD, strOpType, dtRecord, strGroupType);
                    break;
                case 1://List列表
                    this.CreateDropDownList(cell, entityHRTMPSD, strOpType, dtRecord, strGroupType);
                    break;
                case 2://DB List数据列表
                    this.CreateDBListTextBox(cell, entityHRTMPSD, strOpType, dtRecord, strGroupType);
                    break;
                case 3://Edit isMultilines文本框(多行)
                    this.CreateGeneralTextBox(cell, entityHRTMPSD, strOpType, dtRecord, strGroupType);
                    break;
                case 4://Edit 逗号分开的金额类型文本框 modify by sammen 20140312
                    this.CreateGeneralTextBox(cell, entityHRTMPSD, strOpType, dtRecord, strGroupType);
                    break;
                case 5://Edit Picture文本框（图片）
                    this.CreateImageTextBox(cell, entityHRTMPSD, strOpType, dtRecord, strGroupType);
                    break;
                case 6://Edit Widelines文本框（宽行）
                    this.CreateGeneralTextBox(cell, entityHRTMPSD, strOpType, dtRecord, strGroupType);
                    break;
                case 7://List(Except Master)列表(非主显示)
                    this.CreateDropDownList(cell, entityHRTMPSD, strOpType, dtRecord, strGroupType);
                    break;
                case 8://DB List(Except Master)数据列表(非主显示)
                    this.CreateDBListTextBox(cell, entityHRTMPSD, strOpType, dtRecord, strGroupType);
                    break;
                case 9://Edit Password文本框(密码)
                    this.CreateGeneralTextBox(cell, entityHRTMPSD, strOpType, dtRecord, strGroupType);
                    break;
                case 10://Attachment附加文件
                    this.CreateAttachmentCtrl(cell, entityHRTMPSD, strOpType, dtRecord, strGroupType);
                    break;
                case 11://CheckBox复选框
                    this.CreateGeneralCheckBox(cell, entityHRTMPSD, strOpType, dtRecord, strGroupType);
                    break;
                case 112://DatePicker日期面板
                    this.CreateGeneralTextBox(cell, entityHRTMPSD, strOpType, dtRecord, strGroupType);
                    break;
                case 12://DateTimePicker日期时间面板
                    this.CreateGeneralTextBox(cell, entityHRTMPSD, strOpType, dtRecord, strGroupType);
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
                    this.CreateFreeTextBox(cell, entityHRTMPSD, strOpType, dtRecord, strGroupType);
                    break;
                case 20://PrintPage打印页
                    break;
                case 21://EditMultiTextbox多选列表
                    break;
                case 22://明细链接页面控件
                    this.CreateDetailLinkButtion(cell, entityHRTMPSD, strOpType, dtRecord, strGroupType);
                    break;

            }

        }

        /// <summary>
        /// 创建文本框控件到CELL控件中（包括多行文本，宽文本,密码，日期）
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="entityHRTMPSD"></param>
        /// <param name="strOpType">操作类型（新增/编辑/只读）</param>
        /// <param name="dtRecord"></param>
        /// <param name="strGroupType"></param>
        /// <returns></returns>
        public GeneralTextBox CreateGeneralTextBox(TableCell cell, Entity_TB_HRTMPSD entityHRTMPSD, String strOpType, DataTable dtRecord, String strGroupType)
        {
            String strCtrlId = ServerCtrlIDGetterBll.GetCtrlID_TextBox(entityHRTMPSD.TID, entityHRTMPSD.GID, entityHRTMPSD.PID);
            GeneralTextBox txt = WebControlCreateBll.CreateGeneralTextBox(entityHRTMPSD, strCtrlId, strGroupType);

            //add by sammen 20210604 优化新增
            txt.Attributes.Remove("ptype");
            txt.Attributes.Add("ptype", entityHRTMPSD.PTYPE.ToString());
            txt.Attributes.Remove("ctrltype");
            txt.Attributes.Add("ctrltype", entityHRTMPSD.PCTRL.ToString());
            txt.Attributes.Remove("optype");
            txt.Attributes.Add("optype", strOpType.ToString());
            try
            {
                //时间或者日期类型
                if ((entityHRTMPSD.PTYPE.ToLower().Equals("date")) || (entityHRTMPSD.PTYPE.ToLower().Equals("datetime")) || (entityHRTMPSD.PCTRL == 112) || (entityHRTMPSD.PCTRL == 12))
                {
                    if (entityHRTMPSD.PRIGHT == 0)
                    {
                        if (entityHRTMPSD.PCTRL == 12)//日期
                        {
                            txt.Attributes.Remove("datetype");
                            txt.Attributes.Add("datetype", "date");
                        }
                        else if (entityHRTMPSD.PCTRL == 112)//日期时间
                        {
                            txt.Attributes.Remove("datetype");
                            txt.Attributes.Add("datetype", "datetime");
                        }
                    }
                }
                //如果是编辑和只读状态，则回填记录值
                if ((dtRecord != null) && (dtRecord.Rows.Count > 0))
                {
                    String strTextValue = "";
                    //时间或者日期类型
                    if ((entityHRTMPSD.PTYPE.ToLower().Equals("date")) || (entityHRTMPSD.PTYPE.ToLower().Equals("datetime")))
                    {
                        if (dtRecord.Rows[0][entityHRTMPSD.PID] != DBNull.Value)
                        {
                            //时间格式化成相应文本显示（yyyy-MM-dd HH:mm:ss）
                            DateTime dt = (DateTime)dtRecord.Rows[0][entityHRTMPSD.PID];
                            if (entityHRTMPSD.PTYPE.ToLower().Equals("date"))
                            {
                                strTextValue = dt.ToString("yyyy-MM-dd");
                            }
                            else if (entityHRTMPSD.PTYPE.ToLower().Equals("datetime"))
                            {
                                strTextValue = dt.ToString("yyyy-MM-dd HH:mm:ss");
                            }
                        }
                    }
                    else if (entityHRTMPSD.PTYPE.ToLower().Equals("time"))
                    {
                        if (dtRecord.Rows[0][entityHRTMPSD.PID] != DBNull.Value)
                        {
                            strTextValue = dtRecord.Rows[0][entityHRTMPSD.PID].ToString();
                        }
                    }
                    else if (entityHRTMPSD.PTYPE.ToLower().Equals("numeric") && entityHRTMPSD.PCTRL.ToString().Equals("4"))//金额类型存在逗号分开 add by sammen 20140312
                    {
                        strTextValue = string.Format("{0:N}",dtRecord.Rows[0][entityHRTMPSD.PID]);
                        if((entityHRTMPSD.PPREC > 2)&& (entityHRTMPSD.PPREC != 2))
                        {
                            strTextValue = string.Format("{0:N" + entityHRTMPSD.PPREC.ToString()+ "}", dtRecord.Rows[0][entityHRTMPSD.PID]);
                        }
                    }
                    else
                    {
                        //strTextValue = dtRecord.Rows[0][entityHRTMPSD.PID].ToString();
                        strTextValue = dtRecord.Rows[0][entityHRTMPSD.PID].ToString().Replace("<br>", "\n");//换行符的处理
                    }
                    txt.Text = strTextValue;
                    txt.OldValue = strTextValue;

                    //主键字段不能修改
                    if ((entityHRTMPSD.PID.Equals(this.KEY)) || (entityHRTMPSD.PISKEY.Value==1))
                    {
                        txt.SetCtrlReadOnly(true);
                    }
                }
                else
                {
                    txt.Text = ParamOperationBll.ReplaceParamToUserValue(entityHRTMPSD.PDEFAULT, this.hsCurUserParamValue);

                    //如果是新增时，则判断自增类型的填入
                    if ((entityHRTMPSD.PTYPE.ToLower().Equals("intc")) || (entityHRTMPSD.PTYPE.ToLower().Equals("ints")))
                    {
                        txt.Text = this.InitIncreaceNoShow(entityHRTMPSD, txt.Text);
                        txt.SetCtrlReadOnly(true);
                    }
                    //获取字段定义中系统参数字段的值
                    //增加判断，只有在可编辑状态下，可以自动填入新的系统参数值 add by sammen 20130917
                    if (!strOpType.ToLower().Equals("readonly"))
                    {
                        String strSysParamValue = this.GetConfigSystemDefaultValue(entityHRTMPSD);
                        if (!String.IsNullOrEmpty(strSysParamValue))
                        {
                            txt.Text = strSysParamValue;
                            //txt.SetCtrlReadOnly(true);
                        }
                    }

                }
                //如果是非主表的主键字段则回填
                if ((entityHRTMPSD.PID.Equals(this.KEY)) && (!strGroupType.Equals("0")))
                {
                    txt.Text = this.KEYVALUE;
                }

                //只读状态时不能修改
                if (strOpType.ToLower().Equals("readonly"))
                {
                    txt.SetCtrlReadOnly(true);
                }
                cell.Controls.Add(txt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.Error("\r\n");
                log.Error("CreateGeneralTextBox创建文本框控件到CELL控件中（包括多行文本，宽文本）失败：TextBoxID:" + txt.ID);
            }
            return txt;
        }

        /// <summary>
        /// 创建下拉框控件到CELL控件中
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="entityHRTMPSD"></param>
        /// <param name="strOpType">操作类型（新增/编辑/只读）</param>
        /// <param name="dtRecord"></param>
        /// <param name="strGroupType"></param>
        /// <returns></returns>
        public GeneralDropDownList CreateDropDownList(TableCell cell, Entity_TB_HRTMPSD entityHRTMPSD, String strOpType, DataTable dtRecord, String strGroupType)
        {
            String strCtrlId = ServerCtrlIDGetterBll.GetCtrlID_DropDownList(entityHRTMPSD.TID, entityHRTMPSD.GID, entityHRTMPSD.PID);
            GeneralDropDownList ddList = WebControlCreateBll.CreateGeneralDropDownList(entityHRTMPSD, strCtrlId, strGroupType);

            //add by sammen 20210604 优化新增
            ddList.Attributes.Remove("ptype");
            ddList.Attributes.Add("ptype", entityHRTMPSD.PTYPE.ToString());
            ddList.Attributes.Remove("ctrltype");
            ddList.Attributes.Add("ctrltype", entityHRTMPSD.PCTRL.ToString());
            ddList.Attributes.Remove("optype");
            ddList.Attributes.Add("optype", strOpType.ToString());
            try
            {
                TB_HRLSTDProperty property_LSTD = new TB_HRLSTDProperty(entityHRTMPSD.PCTRLID);
                String strWhereBIsstop= "";
                if (property_LSTD.TABLENAME.Equals("TB_HRLSTD"))
                {
                    strWhereBIsstop = " AND isnull(BISSTOP,'0') <> '1' ";//只显示正常使用的项目
                }
                String strSql = "SELECT LID,CID,CDESC,CDESCCHS,CUID FROM " + property_LSTD.TABLENAME + " WHERE LID= '" + property_LSTD.LID + "'"+strWhereBIsstop+" ORDER BY " + property_LSTD.ORDER;
                
                //如果存在主控字段
                String strMasterFiled = entityHRTMPSD.PMAST;

                if (!String.IsNullOrEmpty(strMasterFiled))
                {
                    PMASTProperty property_PMAST = new PMASTProperty(strMasterFiled, entityHRTMPSD.TID, entityHRTMPSD.GID, this.KEY, this.KEYVALUE);
                    String strMasterValue = property_PMAST.MASTFIELDVALUE;

                    //获取服务器端控件客户端界面选择的值时设置其他控件的MASTVALUE
                    String strGetClientSettingMastValue = this.GetClientSettingMastValue(strCtrlId);
                    if (!String.IsNullOrEmpty(strGetClientSettingMastValue))
                        // modify by sammen 20241106 之前是只要session里面有值就设置为MasterValue，现在调整为session里有值，且strMasterValue本身为空时设置
                        // modify by sammen 20250317 恢复到之前只要是session里有值就设置为MasterValue。
                        //                           因为出现了在修改模板数据时，联动下拉框保存时无法获取被值的问题，如档案库中部门与职位的联动，在修改信息时，职位下拉框的值信息无法获取到
                        //if (!String.IsNullOrEmpty(strGetClientSettingMastValue) && String.IsNullOrEmpty(strMasterValue))
                    {
                        strMasterValue = strGetClientSettingMastValue;
                    }

                    strSql = "SELECT LID,CID,CDESC,CDESCCHS,CUID FROM " + property_LSTD.TABLENAME + " WHERE LID= '" + property_LSTD.LID + "' and CUID ='" + strMasterValue + "' ORDER BY " + property_LSTD.ORDER;
                }

                DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
                if ((dt != null) && (dt.Rows.Count > 0))
                {
                    ListItem item = new ListItem("", "");
                    ddList.Items.Add(item);
                    string strShowName = "";
                    if (base.Language.Equals("zh-cn"))
                    {
                        strShowName = "CDESCCHS";
                    }
                    else
                    {
                        strShowName = "CDESC";
                    }
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        item = new ListItem(dt.Rows[i][strShowName].ToString(), dt.Rows[i]["CID"].ToString());

                        //如果是编辑和只读状态，则回填记录值
                        if ((dtRecord != null) && (dtRecord.Rows.Count > 0))
                        {
                            if (dt.Rows[i]["CID"].ToString().ToLower().Equals(dtRecord.Rows[0][entityHRTMPSD.PID].ToString().ToLower()))
                            {
                                item.Selected = true;
                            }
                            if ((entityHRTMPSD.PID.Equals(this.KEY)) || (entityHRTMPSD.PISKEY.Value == 1))//主键字段不能修改
                            {
                                ddList.SetCtrlReadOnly(true);
                            }
                            ddList.OldValue = dtRecord.Rows[0][entityHRTMPSD.PID].ToString();
                        }
                        else
                        {
                            //新增时默认值
                            if (dt.Rows[i]["CID"].ToString().ToLower().Equals(ParamOperationBll.ReplaceParamToUserValue(entityHRTMPSD.PDEFAULT, this.hsCurUserParamValue).ToLower()))
                            {
                                item.Selected = true;
                            }
                            //获取字段定义中系统参数字段的值
                            //增加判断，只有在可编辑状态下，可以自动填入新的系统参数值 add by sammen 20130917
                            if (!strOpType.ToLower().Equals("readonly"))
                            {
                                String strSysParamValue = this.GetConfigSystemDefaultValue(entityHRTMPSD);
                                if (!String.IsNullOrEmpty(strSysParamValue))
                                {
                                    if (dt.Rows[i]["CID"].ToString().ToLower().Equals(strSysParamValue.ToLower()))
                                    {
                                        item.Selected = true;
                                        //ddList.SetCtrlReadOnly(true);
                                    }
                                }
                            }
                        }
                        ddList.Items.Add(item);
                    }
                }

                ddList.TabIndex = (short)entityHRTMPSD.PORDER.Value;
                ddList.Attributes.Add("onchange", "onChangeCtrlValue('" + ddList.ID + "','" + this.KEYVALUE + "');");
                //只读状态时不能修改
                if (strOpType.ToLower().Equals("readonly"))
                {
                    ddList.SetCtrlReadOnly(true);
                }
                cell.Controls.Add(ddList);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.Error("\r\n");
                log.Error("CreateDropDownList创建下拉框控件到CELL控件中失败：DropDownListID:" + ddList.ID);
            }
            return ddList;
        }

        /// <summary>
        /// 创建数据列表文本框控件到CELL控件中
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="entityHRTMPSD"></param>
        /// <param name="strOpType">操作类型（新增/编辑/只读）</param>
        /// <param name="dtRecord"></param>
        /// <param name="strGroupType"></param>
        /// <returns></returns>
        public DBTextBox CreateDBListTextBox(TableCell cell, Entity_TB_HRTMPSD entityHRTMPSD, String strOpType, DataTable dtRecord, String strGroupType)
        {
            String strCtrlId = ServerCtrlIDGetterBll.GetCtrlID_DBTextBox(entityHRTMPSD.TID, entityHRTMPSD.GID, entityHRTMPSD.PID);
            //获取该字段对应的PMAST主控值
            String strMastValue = "";
            if (!String.IsNullOrEmpty(entityHRTMPSD.PMAST))
            {
                TmpdPMastBll bllMast = new TmpdPMastBll();
                //Modify By Sammen 20161109(如果Mast字段来源于列表分组，则需要定位到列表分组的某条记录值)
                strMastValue = bllMast.GetMastValueByCtrlId(strCtrlId, entityHRTMPSD.PMAST, this.KEY, this.KEYVALUE,this.GRIDKEY,this.GRIDKEYVALUE);
            }
            DBTextBox txt = WebControlCreateBll.CreateDBTextBox(entityHRTMPSD, strMastValue, strCtrlId, strGroupType);
            txt.RID = this.RID;

            //add by sammen 20210604 优化新增
            txt.Attributes.Remove("ptype");
            txt.Attributes.Add("ptype", entityHRTMPSD.PTYPE.ToString());
            txt.Attributes.Remove("ctrltype");
            txt.Attributes.Add("ctrltype", entityHRTMPSD.PCTRL.ToString());
            txt.Attributes.Remove("optype");
            txt.Attributes.Add("optype", strOpType.ToString());
            try
            {
                //如果是编辑和只读状态，则回填记录值
                if ((dtRecord != null) && (dtRecord.Rows.Count > 0))
                {
                    txt.Text = dtRecord.Rows[0][entityHRTMPSD.PID].ToString();
                    txt.OldValue = dtRecord.Rows[0][entityHRTMPSD.PID].ToString();

                    //主键字段或者只读状态时不能修改
                    if ((entityHRTMPSD.PID.Equals(this.KEY)) || (entityHRTMPSD.PISKEY.Value == 1))
                    {
                        txt.SetCtrlReadOnly(true);
                    }

                    //如果是主表的主键字段则回填
                    if (entityHRTMPSD.PID.Equals(this.KEY))
                    {
                        txt.Text = this.KEYVALUE;
                    }
                }
                else
                {
                    txt.Text = ParamOperationBll.ReplaceParamToUserValue(entityHRTMPSD.PDEFAULT, this.hsCurUserParamValue);

                    //如果是新增时，则判断自增类型的填入
                    if ((entityHRTMPSD.PTYPE.ToLower().Equals("intc")) || (entityHRTMPSD.PTYPE.ToLower().Equals("ints")))
                    {
                        txt.Text = this.InitIncreaceNoShow(entityHRTMPSD, txt.Text);
                        txt.SetCtrlReadOnly(true);
                    }
                    //获取字段定义中系统参数字段的值
                    //增加判断，只有在可编辑状态下，可以自动填入新的系统参数值 add by sammen 20130917
                    if (!strOpType.ToLower().Equals("readonly"))
                    {
                        String strSysParamValue = this.GetConfigSystemDefaultValue(entityHRTMPSD);
                        if (!String.IsNullOrEmpty(strSysParamValue))
                        {
                            txt.Text = strSysParamValue;
                            //txt.SetCtrlReadOnly(true);
                        }
                    }
                }

                txt.Attributes.Add("onpropertychange", "onChangeCtrlValue('" + txt.ID + "','" + this.KEYVALUE + "');");
                //txt.Attributes.Add("onblur", "onChangeCtrlValue('" + txt.ID + "','" + this.KEYVALUE + "');");

                //如果是非主表的主键字段则回填
                if ((entityHRTMPSD.PID.Equals(this.KEY)) && (!strGroupType.Equals("0")))
                {
                    txt.Text = this.KEYVALUE;
                }
                //只读状态时不能修改
                if (strOpType.ToLower().Equals("readonly"))
                {
                    txt.SetCtrlReadOnly(true);
                }

                txt.SetImageClickFunction();

                cell.Controls.Add(txt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.Error("\r\n");
                log.Error("CreateDBListTextBox创建数据列表文本框控件到CELL控件中失败：DBTextBoxID:" + txt.ID);
            }
            return txt;
        }

        /// <summary>
        /// 创建图片控件到CELL控件中
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="entityHRTMPSD"></param>
        /// <param name="strOpType">操作类型（新增/编辑/只读）</param>
        /// <param name="dtRecord"></param>
        /// <param name="strGroupType"></param>
        /// <returns></returns>
        public ImageTextBox CreateImageTextBox(TableCell cell, Entity_TB_HRTMPSD entityHRTMPSD, String strOpType, DataTable dtRecord, String strGroupType)
        {
            String strCtrlId = ServerCtrlIDGetterBll.GetCtrlID_TextBox(entityHRTMPSD.TID, entityHRTMPSD.GID, entityHRTMPSD.PID);
            ImageTextBox imgTextBox = WebControlCreateBll.CreateImageTextBox(entityHRTMPSD, strCtrlId, strGroupType,this.KEYVALUE);

            //add by sammen 20210604 优化新增
            imgTextBox.Attributes.Remove("ptype");
            imgTextBox.Attributes.Add("ptype", entityHRTMPSD.PTYPE.ToString());
            imgTextBox.Attributes.Remove("ctrltype");
            imgTextBox.Attributes.Add("ctrltype", entityHRTMPSD.PCTRL.ToString());
            imgTextBox.Attributes.Remove("optype");
            imgTextBox.Attributes.Add("optype", strOpType.ToString());
            try
            {
                //如果是编辑和只读状态，则回填记录值
                if ((dtRecord != null) && (dtRecord.Rows.Count > 0))
                {
                    imgTextBox.Text = dtRecord.Rows[0][entityHRTMPSD.PID].ToString();
                    imgTextBox.OldValue = dtRecord.Rows[0][entityHRTMPSD.PID].ToString();

                    //主键字段或者只读状态时不能修改
                    if ((entityHRTMPSD.PID.Equals(this.KEY)) || (entityHRTMPSD.PISKEY.Value == 1))
                    {
                        imgTextBox.SetCtrlReadOnly(true);
                    }
                }
                else
                {
                    imgTextBox.Text = ParamOperationBll.ReplaceParamToUserValue(entityHRTMPSD.PDEFAULT, this.hsCurUserParamValue);

                    //如果是新增时，则判断自增类型的填入
                    if ((entityHRTMPSD.PTYPE.ToLower().Equals("intc")) || (entityHRTMPSD.PTYPE.ToLower().Equals("ints")))
                    {
                        imgTextBox.Text = this.InitIncreaceNoShow(entityHRTMPSD, imgTextBox.Text);
                        imgTextBox.SetCtrlReadOnly(true);
                    }
                    //获取字段定义中系统参数字段的值
                    //增加判断，只有在可编辑状态下，可以自动填入新的系统参数值 add by sammen 20130917
                    if (!strOpType.ToLower().Equals("readonly"))
                    {
                        String strSysParamValue = this.GetConfigSystemDefaultValue(entityHRTMPSD);
                        if (!String.IsNullOrEmpty(strSysParamValue))
                        {
                            imgTextBox.Text = strSysParamValue;
                            //imgTextBox.SetCtrlReadOnly(true);
                        }
                    }
                }

                //如果是非主表的主键字段则回填
                if ((entityHRTMPSD.PID.Equals(this.KEY)) && (!strGroupType.Equals("0")))
                {
                    imgTextBox.Text = this.KEYVALUE;
                }
                //只读状态时不能修改
                if (strOpType.ToLower().Equals("readonly"))
                {
                    imgTextBox.SetCtrlReadOnly(true);
                }

                imgTextBox.SetImageStyle();
                cell.Controls.Add(imgTextBox);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.Error("\r\n");
                log.Error("CreateImageTextBox创建图片控件到CELL控件中失败：ArchiveImageID:" + imgTextBox.ID);
            }
            return imgTextBox;
        }

        /// <summary>
        /// 创建复选框控件到CELL控件中
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="entityHRTMPSD"></param>
        /// <param name="strOpType">操作类型（新增/编辑/只读）</param>
        /// <param name="dtRecord"></param>
        /// <param name="strGroupType"></param>
        /// <returns></returns>
        public GeneralCheckBox CreateGeneralCheckBox(TableCell cell, Entity_TB_HRTMPSD entityHRTMPSD, String strOpType, DataTable dtRecord, String strGroupType)
        {
            String strCtrlId = ServerCtrlIDGetterBll.GetCtrlID_CheckBox(entityHRTMPSD.TID, entityHRTMPSD.GID, entityHRTMPSD.PID);
            GeneralCheckBox chkBox = WebControlCreateBll.CreateGeneralCheckBox(entityHRTMPSD, strCtrlId, strGroupType);

            //add by sammen 20210604 优化新增
            chkBox.Attributes.Remove("ptype");
            chkBox.Attributes.Add("ptype", entityHRTMPSD.PTYPE.ToString());
            chkBox.Attributes.Remove("ctrltype");
            chkBox.Attributes.Add("ctrltype", entityHRTMPSD.PCTRL.ToString());
            chkBox.Attributes.Remove("optype");
            chkBox.Attributes.Add("optype", strOpType.ToString());
            try
            {
                if (!String.IsNullOrEmpty(entityHRTMPSD.PDEFAULT))
                {
                    chkBox.Checked = bool.Parse(entityHRTMPSD.PDEFAULT);
                }
                //chkBox.SetCtrlReadOnly(false);
                //如果是编辑和只读状态，则回填记录值
                if ((dtRecord != null) && (dtRecord.Rows.Count > 0))
                {
                    if (dtRecord.Rows[0][entityHRTMPSD.PID] != DBNull.Value)
                    {
                        chkBox.Checked = bool.Parse(dtRecord.Rows[0][entityHRTMPSD.PID].ToString());
                    }
                    chkBox.OldValue = dtRecord.Rows[0][entityHRTMPSD.PID].ToString();

                    if ((entityHRTMPSD.PID.Equals(this.KEY)) || (entityHRTMPSD.PISKEY.Value == 1))//主键字段不能修改
                    {
                        chkBox.SetCtrlReadOnly(true);
                    }
                }
                else
                {
                    chkBox.Checked = bool.Parse(ParamOperationBll.ReplaceParamToUserValue(entityHRTMPSD.PDEFAULT, this.hsCurUserParamValue));

                    //获取字段定义中系统参数字段的值
                    //增加判断，只有在可编辑状态下，可以自动填入新的系统参数值 add by sammen 20130917
                    if (!strOpType.ToLower().Equals("readonly"))
                    {
                        String strSysParamValue = this.GetConfigSystemDefaultValue(entityHRTMPSD);
                        if ((!String.IsNullOrEmpty(strSysParamValue)) && (strSysParamValue.ToLower().Equals("true")))
                        {
                            chkBox.Checked = bool.Parse(strSysParamValue);
                            //chkBox.SetCtrlReadOnly(true);
                        }
                    }
                }

                if (strOpType.ToLower().Equals("readonly"))//只读状态时不能修改
                {
                    chkBox.SetCtrlReadOnly(true);
                }
                cell.Controls.Add(chkBox);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.Error("\r\n");
                log.Error("CreateGeneralCheckBox创建复选框控件到CELL控件中失败：CheckBoxID:" + chkBox.ID);
            }
            return chkBox;
        }

        /// <summary>
        /// 创建附件上传框控件到CELL控件中
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="entityHRTMPSD"></param>
        /// <param name="strOpType">操作类型（新增/编辑/只读）</param>
        /// <param name="dtRecord"></param>
        /// <param name="strGroupType"></param>
        /// <returns></returns>
        public AttachmentCtrl CreateAttachmentCtrl(TableCell cell, Entity_TB_HRTMPSD entityHRTMPSD, String strOpType, DataTable dtRecord, String strGroupType)
        {
            String strCtrlId = ServerCtrlIDGetterBll.GetCtrlID_AttachmentCtrl(entityHRTMPSD.TID, entityHRTMPSD.GID, entityHRTMPSD.PID);
            AttachmentCtrl attachCtrl = WebControlCreateBll.CreateAttachmentCtrl(entityHRTMPSD, strCtrlId, strGroupType);
            try
            {
                attachCtrl.SetCtrlClick(entityHRTMPSD.TID, entityHRTMPSD.SID, entityHRTMPSD.GID, this.KEY, this.KEYVALUE,this.GRIDKEY,this.GRIDKEYVALUE, entityHRTMPSD.PID, strOpType);

                cell.Controls.Add(attachCtrl);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.Error("\r\n");
                log.Error("CreateAttachmentCtrl创建附件上传框控件到CELL控件中失败：AttachmentCtrlID:" + attachCtrl.ID);
            }
            return attachCtrl;
        }

        /// <summary>
        /// 创建明细链接页面控件到CELL控件中
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="entityHRTMPSD"></param>
        /// <param name="strOpType">操作类型（新增/编辑/只读）</param>
        /// <param name="dtRecord"></param>
        /// <param name="strGroupType"></param>
        /// <returns></returns>
        public DetailLinkButtion CreateDetailLinkButtion(TableCell cell, Entity_TB_HRTMPSD entityHRTMPSD, String strOpType, DataTable dtRecord, String strGroupType)
        {
            String strCtrlId = ServerCtrlIDGetterBll.GetCtrlID_DetailLinkButton(entityHRTMPSD.TID, entityHRTMPSD.GID, entityHRTMPSD.PID);
            DetailLinkButtion detailLink = WebControlCreateBll.CreateDetailLinkButtion(entityHRTMPSD, strCtrlId, strGroupType);

            try
            {
                String strPctrlId = entityHRTMPSD.PCTRLID;
                detailLink.DKEYVALUE = "NONE";
                if (!String.IsNullOrEmpty(strPctrlId))
                {
                    //表示对应某个字段
                    if (strPctrlId.StartsWith("@"))
                    {
                        strPctrlId = strPctrlId.Substring(1, strPctrlId.Length-1);
                        if ((dtRecord != null) && (dtRecord.Rows.Count > 0))
                        {
                            if (dtRecord.Rows[0][strPctrlId] != DBNull.Value)
                            {
                                detailLink.DKEYVALUE = dtRecord.Rows[0][strPctrlId].ToString();
                            }
                        }
                    }
                    else
                    {
                        detailLink.DKEYVALUE = strPctrlId;
                    }
                }

                detailLink.SetClickFunction();
                cell.Controls.Add(detailLink);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.Error("\r\n");
                log.Error("CreateDetailLinkButtion创建明细链接页面控件到CELL控件中失败：" + strCtrlId);
            }
            return detailLink;
        }
        
        /// <summary>
        /// 创建格式化文本框控件到CELL控件中
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="entityHRTMPSD"></param>
        /// <param name="strOpType">操作类型（新增/编辑/只读）</param>
        /// <param name="dtRecord"></param>
        /// <param name="strGroupType"></param>
        /// <returns></returns>
        public MyFreeTextBox CreateFreeTextBox(TableCell cell, Entity_TB_HRTMPSD entityHRTMPSD, String strOpType, DataTable dtRecord, String strGroupType)
        {
            String strCtrlId = ServerCtrlIDGetterBll.GetCtrlID_FreeTextBox(entityHRTMPSD.TID, entityHRTMPSD.GID, entityHRTMPSD.PID);
            MyFreeTextBox txt = WebControlCreateBll.CreateFreeTextBox(entityHRTMPSD, strCtrlId, strGroupType);

            //控件语言
            txt.Language = this.Language.ToString();
            //图片浏览路径
            String strImageGalleryRelaTivePath = "~/UserFile/FreeTextBox/" + this.GetUserCode() + "/UploadImg/";
            String strImageGalleryPath = base.MapPath(strImageGalleryRelaTivePath);
            if (!Directory.Exists(strImageGalleryPath))
            {
                Directory.CreateDirectory(strImageGalleryPath);
            }
            txt.ImageGalleryPath = strImageGalleryRelaTivePath;

            try
            {
                //如果是编辑和只读状态，则回填记录值
                if ((dtRecord != null) && (dtRecord.Rows.Count > 0))
                {
                    String strTextValue = dtRecord.Rows[0][entityHRTMPSD.PID].ToString().Replace("<br>", "\n");//换行符的处理

                    txt.Text = strTextValue;
                    txt.OldValue = strTextValue;

                    //主键字段不能修改
                    if ((entityHRTMPSD.PID.Equals(this.KEY)) || (entityHRTMPSD.PISKEY.Value == 1))
                    {
                        txt.SetCtrlReadOnly(true);
                    }
                }
                else
                {
                    txt.Text = ParamOperationBll.ReplaceParamToUserValue(entityHRTMPSD.PDEFAULT, this.hsCurUserParamValue);

                    //如果是新增时，则判断自增类型的填入
                    if ((entityHRTMPSD.PTYPE.ToLower().Equals("intc")) || (entityHRTMPSD.PTYPE.ToLower().Equals("ints")))
                    {
                        txt.Text = this.InitIncreaceNoShow(entityHRTMPSD, txt.Text);
                        txt.SetCtrlReadOnly(true);
                    }
                    //获取字段定义中系统参数字段的值
                    //增加判断，只有在可编辑状态下，可以自动填入新的系统参数值 add by sammen 20130917
                    if (!strOpType.ToLower().Equals("readonly"))
                    {
                        String strSysParamValue = this.GetConfigSystemDefaultValue(entityHRTMPSD);
                        if (!String.IsNullOrEmpty(strSysParamValue))
                        {
                            txt.Text = strSysParamValue;
                            //txt.SetCtrlReadOnly(true);
                        }
                    }

                }
                //如果是非主表的主键字段则回填
                if ((entityHRTMPSD.PID.Equals(this.KEY)) && (!strGroupType.Equals("0")))
                {
                    txt.Text = this.KEYVALUE;
                }
                //只读状态时不能修改
                if (strOpType.ToLower().Equals("readonly"))
                {
                    txt.SetCtrlReadOnly(true);
                }
                cell.Controls.Add(txt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.Error("\r\n");
                log.Error("FreeTextBox创建格式化文本框控件到CELL控件中（包括多行文本，宽文本）失败：TextBoxID:" + txt.ID);
            }
            return txt;
        }


        /// <summary>
        /// 初始化自增字段类型的显示
        /// </summary>
        /// <param name="entityHRTMPSD"></param>
        /// <param name="strValue"></param>
        /// <returns></returns>
        private String InitIncreaceNoShow(Entity_TB_HRTMPSD entityHRTMPSD,String strValue)
        {
            if (!String.IsNullOrEmpty(entityHRTMPSD.PTYPE))
            {
                if (entityHRTMPSD.PTYPE.ToLower().Equals("intc"))//客户端自增
                {
                    strValue = Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("DefaultShowStr_CleintIncreaceNo");
                }
                else if (entityHRTMPSD.PTYPE.ToLower().Equals("ints"))//服务器端自增
                {
                    strValue = Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("DefaultShowStr_ServerIncreaceNo");
                }
            }
            return strValue;
        }

        /// <summary>
        /// 获取字段定义中系统参数字段的值
        /// </summary>
        /// <param name="entityHRTMPSD"></param>
        /// <returns></returns>
        private String GetConfigSystemDefaultValue(Entity_TB_HRTMPSD entityHRTMPSD)
        {
            //系统参数设置
            int strSysParam = entityHRTMPSD.PSYS.Value;
            String strSysParamValue = "";
            switch (strSysParam)
            {
                case 0://非系统参数
                    break;
                case 1://默认为当前用户ID
                    strSysParamValue = this.GetUserCode();
                    break;
                case 2://默认为当前时间
                    DateTime dtNow = DateTime.Now;
                    strSysParamValue = dtNow.ToString("yyyy-MM-dd HH:mm:ss");
                    //资产系统中，判断如果目前日期月份是否是当前月份，如果是则返回系统当前时间，否则取Monthly_1表中当前月份的最后一天的23:59:59
                    //add by sammen 20160725
                    strSysParamValue = GetCurMonthlyLastDateTime();
                    break;
                case 3://默认为当前状态
                    strSysParamValue = this.SID;
                    break;
                case 4://默认为选中
                    strSysParamValue = "true";
                    break;
                case 5://默认为当前角色
                    strSysParamValue = this.RID;
                    break;
                case 6://默认为当前用户公司【暂停配置】
                    break;
                case 7://默认为当前用户英文名【暂停配置】
                    break;
                case 8://默认为当前用户中文名【暂停配置】
                    break;
                case 9://默认为当前用户所在组【暂停配置】
                    break;
                case 10://默认为当前用户所在部门【暂停配置】
                    break;
            }
            return strSysParamValue;
        }

        #endregion

        #region 保存CELL中各控件的值，返回ArrayList对象
        /// <summary>
        /// 保存CELL中各控件的值，返回ArrayList对象
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="strGID"></param>
        /// <param name="arrListObject"></param>
        /// <param name="entityLog2">日志对象，有变化时才返回</param>
        /// <returns></returns>
        public ArrayList GetCtrlValueInCell(TableCell cell, String strGID, ArrayList arrListObject, ref Entity_HRLOG_2 entityLog2)
        {
            if (cell.Controls.Count > 0)
            {
                Entity_ToDBObject entityDBObject = new Entity_ToDBObject();
                #region 保存GeneralTextBox
                if (cell.Controls[0] is GeneralTextBox)
                {
                    GeneralTextBox ctrlTemp = (GeneralTextBox)cell.Controls[0];
                    String strFieldID = ServerCtrlIDGetterBll.GetPIDByCtrlId(ctrlTemp.ID);
                    //字段名称（用于校验提示）
                    String strFieldName = ctrlTemp.NameCn;
                    if (this.Language.Equals("en-us"))
                    {
                        strFieldName = ctrlTemp.NameEn;
                    }
                    //判断是否是主表主键且是否填入了值
                    if ((ctrlTemp.IsKey.Equals("1") && (ctrlTemp.GroupType.Equals("0")) && (String.IsNullOrEmpty(ctrlTemp.Text.Trim()))))
                    {
                        ctrlTemp.Focus();
                        ctrlTemp.BackColor = Color.Red;
                        base.AlertMessageBox(this, this.rmLanguageResource.GetString("tipKeyFiledIsNull") + strFieldName);
                        return null;
                    }
                    else
                    {
                        ctrlTemp.BackColor = Color.White;
                    }
                    //判断必填
                    if ((ctrlTemp.IsNull.Equals("0")) && (String.IsNullOrEmpty(ctrlTemp.Text.Trim())))
                    {
                        //排除一种情况：常规页面中的主键字段不需要判断，在新增保存时会自动赋值，而在修改的时候不可能为空
                        if (!((!ctrlTemp.GroupType.Equals("0")) && (strFieldID.Equals(this.KEY))))
                        {
                            ctrlTemp.Focus();
                            ctrlTemp.BackColor = Color.Red;
                            base.AlertMessageBox(this, this.rmLanguageResource.GetString("tipRequiredFiledIsNull") + strFieldName);
                            return null;
                        }
                    }
                    else
                    {
                        ctrlTemp.BackColor = Color.White;
                    }
                    //数据输入校验
                    String strValidatMsg = this.DataValidator(ctrlTemp.DataType, ctrlTemp.Text);
                    if (!String.IsNullOrEmpty(strValidatMsg))
                    {
                        ctrlTemp.Focus();
                        ctrlTemp.BackColor = Color.Red;
                        base.AlertMessageBox(this, strValidatMsg);
                        return null;
                    }
                    String strFieldValue = ctrlTemp.Text;

                    ////自增型字段处理///delete by sammen 20140327
                    //strFieldValue = this.GetIncreaceNo(strGID, strFieldID, ctrlTemp.DataType, ctrlTemp.GroupType, strFieldValue);

                    entityDBObject.TID = this.TID;//ADD BY sammen 20140327
                    entityDBObject.GID = strGID;//ADD BY sammen 20140327
                    entityDBObject.CTRLID = ctrlTemp.ID;
                    entityDBObject.FIELDNAME = strFieldID;
                    entityDBObject.FIELDVALUE_OLD = ctrlTemp.OldValue;
                    entityDBObject.FIELDVALUE_NEW = strFieldValue;
                    //entityDBObject.FIELDTYPE = "varchar";
                    entityDBObject.FIELDTYPE = ctrlTemp.DataType;
                    entityDBObject.GROUPTYPE = ctrlTemp.GroupType;
                    entityDBObject.PSAVE = ctrlTemp.IsSave;
                    if (arrListObject != null)
                    {
                        arrListObject.Add(entityDBObject);
                    }
                    
                    //如果前后值发生变化，则返回日志对象
                    if ((entityLog2!=null)&&(!this.ComparedStringIsSame(entityDBObject.FIELDVALUE_OLD,entityDBObject.FIELDVALUE_NEW)))
                    {
                        entityLog2 = DataLogWriter.SetDataToEntity_HRLOG_2(this.TID, strGID, entityDBObject.FIELDNAME, entityDBObject.FIELDVALUE_OLD, entityDBObject.FIELDVALUE_NEW);
                    }
                }
                #endregion
                #region 保存DBTextBox
                else if (cell.Controls[0] is DBTextBox)
                {
                    DBTextBox ctrlTemp = (DBTextBox)cell.Controls[0];
                    String strFieldID = ServerCtrlIDGetterBll.GetPIDByCtrlId(ctrlTemp.ID);
                    //字段名称（用于校验提示）
                    String strFieldName = ctrlTemp.NameCn;
                    if (this.Language.Equals("en-us"))
                    {
                        strFieldName = ctrlTemp.NameEn;
                    }
                    //判断是否是主表主键且是否填入了值
                    if ((ctrlTemp.IsKey.Equals("1") && (ctrlTemp.GroupType.Equals("0")) && (String.IsNullOrEmpty(ctrlTemp.Text.Trim()))))
                    {
                        ctrlTemp.Focus();
                        ctrlTemp.BackColor = Color.Red;
                        base.AlertMessageBox(this, this.rmLanguageResource.GetString("tipKeyFiledIsNull") + strFieldName);
                        return null;

                    }
                    else
                    {
                        ctrlTemp.BackColor = Color.White;
                    }
                    //判断必填
                    if ((ctrlTemp.IsNull.Equals("0")) && (String.IsNullOrEmpty(ctrlTemp.Text.Trim())))
                    {
                        //排除一种情况：常规页面中的主键字段不需要判断，在新增保存时会自动赋值，而在修改的时候不可能为空
                        if (!((!ctrlTemp.GroupType.Equals("0")) && (strFieldID.Equals(this.KEY))))
                        {
                            ctrlTemp.Focus();
                            ctrlTemp.BackColor = Color.Red;
                            base.AlertMessageBox(this, this.rmLanguageResource.GetString("tipRequiredFiledIsNull") + strFieldName);
                            return null;
                        }
                    }
                    else
                    {
                        ctrlTemp.BackColor = Color.White;
                    }
                    //数据输入校验
                    String strValidatMsg = this.DataValidator(ctrlTemp.DataType, ctrlTemp.Text);
                    if (!String.IsNullOrEmpty(strValidatMsg))
                    {
                        ctrlTemp.Focus();
                        ctrlTemp.BackColor = Color.Red;
                        base.AlertMessageBox(this, strValidatMsg);
                        return null;
                    }
                    String strFieldValue = ctrlTemp.Text;
                    //自增型字段处理//delete by sammen 20140327
                    //strFieldValue = this.GetIncreaceNo(strGID, strFieldID, ctrlTemp.DataType, ctrlTemp.GroupType, strFieldValue);

                    entityDBObject.TID = this.TID;//ADD BY sammen 20140327
                    entityDBObject.GID = strGID;//ADD BY sammen 20140327
                    entityDBObject.CTRLID = ctrlTemp.ID;
                    entityDBObject.FIELDNAME = strFieldID;
                    entityDBObject.FIELDVALUE_OLD = ctrlTemp.OldValue;
                    entityDBObject.FIELDVALUE_NEW = strFieldValue;
                    //entityDBObject.FIELDTYPE = "varchar";
                    entityDBObject.FIELDTYPE = ctrlTemp.DataType;
                    entityDBObject.GROUPTYPE = ctrlTemp.GroupType;
                    entityDBObject.PSAVE = ctrlTemp.IsSave;
                    if (arrListObject != null)
                    {
                        arrListObject.Add(entityDBObject);
                    }
                    //如果前后值发生变化，则返回日志对象
                    if ((entityLog2 != null) && (!this.ComparedStringIsSame(entityDBObject.FIELDVALUE_OLD,entityDBObject.FIELDVALUE_NEW)))
                    {
                        entityLog2 = DataLogWriter.SetDataToEntity_HRLOG_2(this.TID, strGID, entityDBObject.FIELDNAME, entityDBObject.FIELDVALUE_OLD, entityDBObject.FIELDVALUE_NEW);
                    }
                }
                #endregion
                #region 保存ImageTextBox
                else if (cell.Controls[0] is ImageTextBox)
                {
                    ImageTextBox ctrlTemp = (ImageTextBox)cell.Controls[0];
                    String strFieldID = ServerCtrlIDGetterBll.GetPIDByCtrlId(ctrlTemp.ID);
                    //字段名称（用于校验提示）
                    String strFieldName = ctrlTemp.NameCn;
                    if (this.Language.Equals("en-us"))
                    {
                        strFieldName = ctrlTemp.NameEn;
                    }
                    //判断是否是主表主键且是否填入了值
                    if ((ctrlTemp.IsKey.Equals("1") && (ctrlTemp.GroupType.Equals("0")) && (String.IsNullOrEmpty(ctrlTemp.Text.Trim()))))
                    {
                        ctrlTemp.Focus();
                        ctrlTemp.BackColor = Color.Red;
                        base.AlertMessageBox(this, this.rmLanguageResource.GetString("tipKeyFiledIsNull") + strFieldName);
                        return null;

                    }
                    else
                    {
                        ctrlTemp.BackColor = Color.White;
                    }
                    //判断必填
                    if ((ctrlTemp.IsNull.Equals("0")) && (String.IsNullOrEmpty(ctrlTemp.Text.Trim())))
                    {
                        //排除一种情况：常规页面中的主键字段不需要判断，在新增保存时会自动赋值，而在修改的时候不可能为空
                        if (!(!(ctrlTemp.GroupType.Equals("0"))) && (strFieldID.Equals(this.KEY)))
                        {
                            ctrlTemp.Focus();
                            ctrlTemp.BackColor = Color.Red;
                            base.AlertMessageBox(this, this.rmLanguageResource.GetString("tipRequiredFiledIsNull") + strFieldName);
                            return null;
                        }
                    }
                    else
                    {
                        ctrlTemp.BackColor = Color.White;
                    }
                    //数据输入校验
                    String strValidatMsg = this.DataValidator(ctrlTemp.DataType, ctrlTemp.Text);
                    if (!String.IsNullOrEmpty(strValidatMsg))
                    {
                        ctrlTemp.Focus();
                        ctrlTemp.BackColor = Color.Red;
                        base.AlertMessageBox(this, strValidatMsg);
                        return null;
                    }
                    String strFieldValue = ctrlTemp.Text;
                    //自增型字段处理//delete by sammen 20140327
                    //strFieldValue = this.GetIncreaceNo(strGID, strFieldID, ctrlTemp.DataType, ctrlTemp.GroupType, strFieldValue);

                    entityDBObject.TID = this.TID;//ADD BY sammen 20140327
                    entityDBObject.GID = strGID;//ADD BY sammen 20140327
                    entityDBObject.CTRLID = ctrlTemp.ID;
                    entityDBObject.FIELDNAME = strFieldID;
                    entityDBObject.FIELDVALUE_OLD = ctrlTemp.OldValue;
                    entityDBObject.FIELDVALUE_NEW = strFieldValue;
                    //entityDBObject.FIELDTYPE = "varchar";
                    entityDBObject.FIELDTYPE = ctrlTemp.DataType;
                    entityDBObject.GROUPTYPE = ctrlTemp.GroupType;
                    entityDBObject.PSAVE = ctrlTemp.IsSave;
                    if (arrListObject != null)
                    {
                        arrListObject.Add(entityDBObject);
                    }
                    //如果前后值发生变化，则返回日志对象
                    if ((entityLog2 != null) && (!this.ComparedStringIsSame(entityDBObject.FIELDVALUE_OLD,entityDBObject.FIELDVALUE_NEW)))
                    {
                        entityLog2 = DataLogWriter.SetDataToEntity_HRLOG_2(this.TID, strGID, entityDBObject.FIELDNAME, entityDBObject.FIELDVALUE_OLD, entityDBObject.FIELDVALUE_NEW);
                    }
                }
                #endregion
                #region 保存GeneralDropDownList
                else if (cell.Controls[0] is GeneralDropDownList)
                {
                    GeneralDropDownList ctrlTemp = (GeneralDropDownList)cell.Controls[0];
                    String strFieldID = ServerCtrlIDGetterBll.GetPIDByCtrlId(ctrlTemp.ID);
                    //字段名称（用于校验提示）
                    String strFieldName = ctrlTemp.NameCn;
                    if (this.Language.Equals("en-us"))
                    {
                        strFieldName = ctrlTemp.NameEn;
                    }
                    //判断是否是主表主键且是否填入了值
                    if ((ctrlTemp.IsKey.Equals("1") && (ctrlTemp.GroupType.Equals("0")) && (String.IsNullOrEmpty(ctrlTemp.Text))))
                    {
                        ctrlTemp.Focus();
                        ctrlTemp.BackColor = Color.Red;
                        base.AlertMessageBox(this, this.rmLanguageResource.GetString("tipKeyFiledIsNull") + strFieldName);
                        return null;

                    }
                    else
                    {
                        ctrlTemp.BackColor = Color.White;
                    }
                    //判断必填
                    if ((ctrlTemp.IsNull.Equals("0")) && (String.IsNullOrEmpty(ctrlTemp.Text)))
                    {
                        //排除一种情况：常规页面中的主键字段不需要判断，在新增保存时会自动赋值，而在修改的时候不可能为空
                        if (!((!ctrlTemp.GroupType.Equals("0")) && (strFieldID.Equals(this.KEY))))
                        {
                            ctrlTemp.Focus();
                            ctrlTemp.BackColor = Color.Red;
                            base.AlertMessageBox(this, this.rmLanguageResource.GetString("tipRequiredFiledIsNull") + strFieldName);
                            return null;
                        }

                    }
                    else
                    {
                        ctrlTemp.BackColor = Color.White;
                    }
                    entityDBObject.TID = this.TID;//ADD BY sammen 20140327
                    entityDBObject.GID = strGID;//ADD BY sammen 20140327
                    entityDBObject.CTRLID = ctrlTemp.ID;
                    entityDBObject.FIELDNAME = ServerCtrlIDGetterBll.GetPIDByCtrlId(ctrlTemp.ID);
                    entityDBObject.FIELDVALUE_OLD = ctrlTemp.OldValue;
                    entityDBObject.FIELDVALUE_NEW = ctrlTemp.SelectedValue;
                    //entityDBObject.FIELDTYPE = "varchar";
                    entityDBObject.FIELDTYPE = ctrlTemp.DataType;
                    entityDBObject.GROUPTYPE = ctrlTemp.GroupType;
                    entityDBObject.PSAVE = ctrlTemp.IsSave;
                    if (arrListObject != null)
                    {
                        arrListObject.Add(entityDBObject);
                    }
                    //如果前后值发生变化，则返回日志对象
                    if ((entityLog2 != null) && (!this.ComparedStringIsSame(entityDBObject.FIELDVALUE_OLD,entityDBObject.FIELDVALUE_NEW)))
                    {
                        entityLog2 = DataLogWriter.SetDataToEntity_HRLOG_2(this.TID, strGID, entityDBObject.FIELDNAME, entityDBObject.FIELDVALUE_OLD, entityDBObject.FIELDVALUE_NEW);
                    }
                }
                #endregion
                #region 保存GeneralCheckBox
                else if (cell.Controls[0] is GeneralCheckBox)
                {
                    GeneralCheckBox ctrlTemp = (GeneralCheckBox)cell.Controls[0];
                    entityDBObject.TID = this.TID;//ADD BY sammen 20140327
                    entityDBObject.GID = strGID;//ADD BY sammen 20140327

                    entityDBObject.CTRLID = ctrlTemp.ID;
                    entityDBObject.FIELDNAME = ServerCtrlIDGetterBll.GetPIDByCtrlId(ctrlTemp.ID);
                    entityDBObject.FIELDVALUE_OLD = ctrlTemp.OldValue;
                    if (ctrlTemp.Checked)
                    {
                        entityDBObject.FIELDVALUE_NEW = "true";
                    }
                    else
                    {
                        entityDBObject.FIELDVALUE_NEW = "false";
                    }
                    //entityDBObject.FIELDTYPE = "varchar";
                    entityDBObject.FIELDTYPE = ctrlTemp.DataType;
                    entityDBObject.GROUPTYPE = ctrlTemp.GroupType;
                    entityDBObject.PSAVE = ctrlTemp.IsSave;
                    if (arrListObject != null)
                    {
                        arrListObject.Add(entityDBObject);
                    }
                    //如果前后值发生变化，则返回日志对象
                    if ((entityLog2 != null) && (!this.ComparedStringIsSame(entityDBObject.FIELDVALUE_OLD,entityDBObject.FIELDVALUE_NEW)))
                    {
                        entityLog2 = DataLogWriter.SetDataToEntity_HRLOG_2(this.TID, strGID, entityDBObject.FIELDNAME, entityDBObject.FIELDVALUE_OLD, entityDBObject.FIELDVALUE_NEW);
                    }
                }
                #endregion

                #region 保存MyFreeTextBox
                if (cell.Controls[0] is MyFreeTextBox)
                {
                    MyFreeTextBox ctrlTemp = (MyFreeTextBox)cell.Controls[0];
                    String strFieldID = ServerCtrlIDGetterBll.GetPIDByCtrlId(ctrlTemp.ID);
                    //字段名称（用于校验提示）
                    String strFieldName = ctrlTemp.NameCn;
                    if (this.Language.Equals("en-us"))
                    {
                        strFieldName = ctrlTemp.NameEn;
                    }
                    //判断必填
                    if ((ctrlTemp.IsNull.Equals("0")) && (String.IsNullOrEmpty(ctrlTemp.Text.Trim())))
                    {
                        //排除一种情况：常规页面中的主键字段不需要判断，在新增保存时会自动赋值，而在修改的时候不可能为空
                        if (!((!ctrlTemp.GroupType.Equals("0")) && (strFieldID.Equals(this.KEY))))
                        {
                            ctrlTemp.Focus();
                            ctrlTemp.BackColor = Color.Red;
                            base.AlertMessageBox(this, this.rmLanguageResource.GetString("tipRequiredFiledIsNull") + strFieldName);
                            return null;
                        }
                    }
                    else
                    {
                        ctrlTemp.BackColor = Color.White;
                    }
                    //数据输入校验
                    String strValidatMsg = this.DataValidator(ctrlTemp.DataType, ctrlTemp.Text);
                    if (!String.IsNullOrEmpty(strValidatMsg))
                    {
                        ctrlTemp.Focus();
                        ctrlTemp.BackColor = Color.Red;
                        base.AlertMessageBox(this, strValidatMsg);
                        return null;
                    }

                    String strFieldValue = ctrlTemp.Text;
                    entityDBObject.TID = this.TID;//ADD BY sammen 20140327
                    entityDBObject.GID = strGID;//ADD BY sammen 20140327
                    entityDBObject.CTRLID = ctrlTemp.ID;
                    entityDBObject.FIELDNAME = strFieldID;
                    entityDBObject.FIELDVALUE_OLD = ctrlTemp.OldValue;
                    entityDBObject.FIELDVALUE_NEW = strFieldValue;
                    //entityDBObject.FIELDTYPE = "varchar";
                    entityDBObject.FIELDTYPE = ctrlTemp.DataType;
                    entityDBObject.GROUPTYPE = ctrlTemp.GroupType;
                    entityDBObject.PSAVE = ctrlTemp.IsSave;
                    if (arrListObject != null)
                    {
                        arrListObject.Add(entityDBObject);
                    }

                    //如果前后值发生变化，则返回日志对象
                    if ((entityLog2 != null) && (!this.ComparedStringIsSame(entityDBObject.FIELDVALUE_OLD, entityDBObject.FIELDVALUE_NEW)))
                    {
                        entityLog2 = DataLogWriter.SetDataToEntity_HRLOG_2(this.TID, strGID, entityDBObject.FIELDNAME, entityDBObject.FIELDVALUE_OLD, entityDBObject.FIELDVALUE_NEW);
                    }
                }
                #endregion
                else
                {
                }

            }
            return arrListObject;
        }

        /// <summary>
        /// 判断数据类型相关输入校验
        /// </summary>
        /// <param name="strDataType"></param>
        private String DataValidator(String strDataType,String strInputValue)
        {
            String strMsg = "";
            if ((!String.IsNullOrEmpty(strDataType))&&(!String.IsNullOrEmpty(strInputValue)))
            {
                switch (strDataType.ToLower())
                {
                    case "datetime":
                    case "date":
                        if (!ArchiveDataValidator.isDate(strInputValue))
                        {
                            strMsg = this.rmLanguageResource.GetString("tipErrorDateFormat");
                        }
                        break;
                    case "time":
                        if (!ArchiveDataValidator.isTime(strInputValue))
                        {
                            strMsg = this.rmLanguageResource.GetString("tipErrorDateFormat")+" HH:MM:SS";
                        }
                        break;
                    case "int":
                        if (!ArchiveDataValidator.isInt32(strInputValue))
                        {
                            strMsg = this.rmLanguageResource.GetString("tipErrorIntegerFormat");
                        }
                        break;
                    case "numeric":
                        //if (!ArchiveDataValidator.isNumeric(strInputValue))
                        if (!ArchiveDataValidator.isNumeric(strInputValue.Replace(",", "")))//金额类型可能会存在逗号分开 add by sammen 20140312
                        {
                            strMsg = this.rmLanguageResource.GetString("tipErrorNumericFormat");
                        }
                        break;
                    case "float":
                        if (!ArchiveDataValidator.isFloat(strInputValue))
                        {
                            strMsg = this.rmLanguageResource.GetString("tipErrorNumericFormat");
                        }
                        break;
                }
            }
            return strMsg;
        }


        /// <summary>
        /// 新增时，自增型字段值的获取
        /// </summary>
        /// <param name="strPID"></param>
        /// <param name="strDataType"></param>
        /// <param name="strGroupType"></param>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public String GetIncreaceNo(String strGID,String strPID,String strDataType,String strGroupType, String strValue)
        {
            if ((!String.IsNullOrEmpty(strPID)) && (!String.IsNullOrEmpty(strDataType)))
            {
                if (strDataType.ToLower().Equals("intc"))//客户端自增
                {
                    //新增时才自增
                    if ((!String.IsNullOrEmpty(strValue)) && (strValue.Equals(Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("DefaultShowStr_CleintIncreaceNo"))))
                    {
                        int iNo = AutoIncreaseFiledBll.GetClientAutoFiledNo(this.TID, strGID, strPID, strDataType, this.KEY, this.KEYVALUE);
                        if (iNo > 0)
                        {
                            strValue = iNo.ToString();
                        }
                    }
                }
                else if (strDataType.ToLower().Equals("ints"))//服务器端自增
                {
                    if (strGroupType.Equals("0"))//主信息表
                    {
                        if (strPID.Equals(this.KEY))//主表的模板主键
                        {
                            //如果是新增，且页面中未提前赋值，则后台自动生成编号
                            if ((this.OPTYPE.ToLower().Equals("add")) && (!String.IsNullOrEmpty(strValue)) && (strValue.Equals(Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("DefaultShowStr_ServerIncreaceNo"))))
                            {
                                strValue = AutoIncreaseFiledBll.GetServerAutoFiledNo(this.TID, strGID, strPID, "");
                            }
                        }
                        else//主表中的非模板主键
                        {
                            //新增时才自增
                            if ((!String.IsNullOrEmpty(strValue)) && (strValue.Equals(Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("DefaultShowStr_ServerIncreaceNo"))))
                            {
                                strValue = AutoIncreaseFiledBll.GetServerAutoFiledNo(this.TID, strGID, strPID, "");
                            }
                        }
                    }
                    else
                    {
                        //非主信息表，则仅仅不能自增模板主键字段
                        if (!strPID.Equals(this.KEY))
                        {
                            //新增时才自增
                            if ((!String.IsNullOrEmpty(strValue)) && (strValue.Equals(Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("DefaultShowStr_ServerIncreaceNo"))))
                            {
                                strValue = AutoIncreaseFiledBll.GetServerAutoFiledNo(this.TID, strGID, strPID, "");
                            }
                        }
                    }
                }
            }
            return strValue;
        }

        /// <summary>
        /// 比较两字符串是否相同，其中null与空字符串为相同无差异
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <returns></returns>
        private bool ComparedStringIsSame(String str1, String str2)
        {
            bool IsSame = true;
            if (String.IsNullOrEmpty(str1))
            {
                if (String.IsNullOrEmpty(str2))
                {
                    IsSame = true;
                }
                else
                {
                    IsSame = false;
                }
            }
            else
            {
                if (String.IsNullOrEmpty(str2))
                {
                    IsSame = false;
                }
                else
                {
                    if (str1.Equals(str2))
                    {
                        IsSame = true;
                    }
                    else
                    {
                        IsSame = false;
                    }
                }
            }
            return IsSame;
        }

        #endregion


        #region 保存并获取服务器端控件客户端界面选择的值(动态生成的服务器端控件在回传后无法获取客户端的赋值)
        /// <summary>
        /// 保存服务器端控件客户端界面选择的值
        /// </summary>
        /// <param name="strCtrlId"></param>
        /// <param name="strCtrlValue"></param>
        /// <param name="strKeyValue"></param>
        public void SaveServerCtrlClientSettingValue(String strCtrlId, String strCtrlValue, String strKeyValue)
        {
            String strSessionName = strCtrlId + "*" + strKeyValue;
            if (Session["hsTableSaveServerCtrlValue"] != null)
            {
                Hashtable hs = (Hashtable)Session["hsTableSaveServerCtrlValue"];
                if (hs.ContainsKey(strSessionName))
                {
                    hs.Remove(strSessionName);
                    hs.Add(strSessionName, strCtrlValue);
                }
                else
                {
                    hs.Add(strSessionName, strCtrlValue);
                }
                Session["hsTableSaveServerCtrlValue"] = hs;
            }
            else
            {
                Hashtable hs = new Hashtable();
                hs.Add(strSessionName, strCtrlValue);
                Session["hsTableSaveServerCtrlValue"] = hs;
            }
        }

        /// <summary>
        /// 获取服务器端控件回传前客户端的选择值
        /// </summary>
        /// <param name="strCtrlId"></param>
        /// <param name="strKeyValue"></param>
        public String GetServerCtrlClientSettingValue(String strCtrlId, String strKeyValue)
        {
            String strSessionName = strCtrlId + "*" + strKeyValue;
            String strSessionValue = "";
            if (Session["hsTableSaveServerCtrlValue"] != null)
            {
                Hashtable hs = (Hashtable)Session["hsTableSaveServerCtrlValue"];
                if (hs.ContainsKey(strSessionName))
                {
                    strSessionValue = hs[strSessionName].ToString();
                    //hs.Remove(strSessionName);
                }

            }
            return strSessionValue;
        }

        /// <summary>
        /// 保存服务器端控件客户端界面选择的值时设置其他控件的MASTVALUE
        /// </summary>
        /// <param name="strCtrlId"></param>
        /// <param name="strMastValue"></param>
        public void SaveClientSettingMastValue(String strCtrlId, String strMastValue)
        {
            String strSessionName = strCtrlId + "*MastValue";
            if (Session["hsTableSaveServerCtrlMastValue"]!= null)
            {
                Hashtable hs = (Hashtable)Session["hsTableSaveServerCtrlMastValue"];
                if (hs.ContainsKey(strSessionName))
                {
                    hs.Remove(strSessionName);
                    hs.Add(strSessionName, strMastValue);
                }
                else
                {
                    hs.Add(strSessionName, strMastValue);
                }
                Session["hsTableSaveServerCtrlMastValue"] = hs;
            }
            else
            {
                Hashtable hs = new Hashtable();
                hs.Add(strSessionName, strMastValue);
                Session["hsTableSaveServerCtrlMastValue"] = hs;
            }
        }

        /// <summary>
        /// 获取服务器端控件客户端界面选择的值时设置其他控件的MASTVALUE
        /// </summary>
        /// <param name="strCtrlId"></param>
        public String GetClientSettingMastValue(String strCtrlId)
        {
            String strSessionName = strCtrlId + "*MastValue";
            String strSessionValue = "";
            if (Session["hsTableSaveServerCtrlMastValue"] != null)
            {
                Hashtable hs = (Hashtable)Session["hsTableSaveServerCtrlMastValue"];
                if (hs.ContainsKey(strSessionName))
                {
                    strSessionValue = hs[strSessionName].ToString();
                    //hs.Remove(strSessionName);
                }

            }
            return strSessionValue;
        }

        /// <summary>
        /// 清除服务器端控件客户端界面选择的值
        /// </summary>
        public void ClearServerCtrlClientSettingSession()
        {
            Session["hsTableSaveServerCtrlValue"] = null;
            Session["hsTableSaveServerCtrlMastValue"] = null;
        }
        #endregion

        #region 页面事件重载
        /// <summary>
        /// 重写页面初始化
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            this.rmLanguageResource = base.GetResourceManager("ArchiveBase");

            base.OnInit(e);
        }

        ///// <summary>
        ///// 重写页面卸载
        ///// </summary>
        ///// <param name="e"></param>
        //protected override void OnUnload(EventArgs e)
        //{
        //    this.ClearServerCtrlClientSettingSession();

        //    base.OnUnload(e);
        //}
        #endregion
        
        /// <summary>
        /// 针对FreeTextBoxCtrl控件修改字体及其大小
        /// </summary>
        public void AddFontToFreeTextBoxCtrl(ArrayList arrListMyFreeTextBox)
        {
            if (arrListMyFreeTextBox != null)
            {
                int iCount = arrListMyFreeTextBox.Count;
                for (int i = 0; i < iCount; i++)
                {
                    MyFreeTextBox freeText = (MyFreeTextBox)arrListMyFreeTextBox[i];
                    freeText.AddAllFont();
                    //freeText.AddSpecificFont();
                    freeText.AddFontSize();
                }
            }
        }

        /// <summary>
        /// 默认系统时间
        //  资产系统中，判断如果目前日期月份是否是当前月份，如果是则返回系统当前时间，否则取Monthly_1表中当前月份的最后一天的23:59:59
        /// </summary>
        /// <returns></returns>
        public String GetCurMonthlyLastDateTime()
        {
            String strCurMonthlyLastDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            try
            {
                String strSql = "select dbo.[Fun_AM_GetCurMonthlyLastDatetime]('" + strCurMonthlyLastDateTime + "') as returnTime";
                DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
                if ((dt != null) && (dt.Rows.Count > 0))
                {
                    strCurMonthlyLastDateTime = dt.Rows[0]["returnTime"].ToString();
                }
            }
            catch (Exception ex)
            {
                log.Error("调用数据库函数[Fun_AM_GetCurMonthlyLastDatetime]出错！");
            }
            return strCurMonthlyLastDateTime;

        }
    }
}
