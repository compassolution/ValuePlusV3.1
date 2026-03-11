using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using Com.ValuePlus.Archive.Entity;
using Com.ValuePlus.Archive.WebCtrls;

namespace Com.ValuePlus.Archive.BLL
{
    public class WebControlCreateBll
    {
        /// <summary>
        /// 创建常规文本框控件
        /// </summary>
        /// <param name="entityHRTMPSD"></param>
        /// <param name="strCtrlId"></param>
        /// <param name="strGroupType"></param>
        /// <param name="strGroupType"></param>
        /// <returns></returns>
        public static GeneralTextBox CreateGeneralTextBox(Entity_TB_HRTMPSD entityHRTMPSD, String strCtrlId, String strGroupType) 
        {
            GeneralTextBox txt = new GeneralTextBox();
            txt.ID = strCtrlId;
            txt.CssClass = "Text_edit_Archive";
            txt.IsKey = entityHRTMPSD.PISKEY.Value.ToString();
            txt.IsNull = entityHRTMPSD.PNULL.Value.ToString();
            txt.DataType = entityHRTMPSD.PTYPE.ToString();
            txt.NameEn = entityHRTMPSD.PDESC.ToString();
            txt.NameCn = entityHRTMPSD.PDESCCHS.ToString();
            txt.GroupType = strGroupType;
            txt.MaxLength = entityHRTMPSD.PLEN.Value;
            txt.IsSave = entityHRTMPSD.PSAVE.ToString();
            if (entityHRTMPSD.PWIDTH.Value <= 0)
            {
                txt.Width = Unit.Pixel(entityHRTMPSD.PWIDTH.Value);
            }
            //txt.Text = entityHRTMPSD.PDEFAULT;
            //多行文本框
            if ((entityHRTMPSD.PCTRL == 3) || (entityHRTMPSD.PCTRL == 6))
            {
                txt.TextMode = TextBoxMode.MultiLine;
                txt.Height = Unit.Pixel(entityHRTMPSD.PWIDTH.Value);
                if (entityHRTMPSD.PWIDTH.Value <= 0)
                {
                    txt.Width = Unit.Pixel(entityHRTMPSD.PWIDTH.Value);
                }

                if (entityHRTMPSD.PCTRL == 6)
                {
                    txt.Attributes.Add("Widelines", "true");
                }
                else
                {
                    txt.Attributes.Add("Widelines", "false");
                }
                txt.Style.Add("behavior", "url(../HTC/textboxzoom.htc)");
            }
            else if (entityHRTMPSD.PCTRL == 4)////金额类型可能会存在逗号分开 add by sammen 20140312
            {
                txt.Attributes.Add("textAlign","Right");
                txt.Attributes.Remove("OnChange");
                txt.Attributes.Add("OnChange", "formatCurrency(this.id," + entityHRTMPSD.PPREC.ToString() + ")");
            }
            //密码框
            if (entityHRTMPSD.PCTRL == 9)
            {
                txt.TextMode = TextBoxMode.Password;
            }

            //权限属性设置
            switch (entityHRTMPSD.PRIGHT.Value){
                case 0://可编辑
                    break;
                case 1://只读
                    txt.SetCtrlReadOnly(true);
                    break;
                case 2://隐藏
                    break;
                case 3://排除
                    break;
            }
            ////系统参数只读设置
            //if (entityHRTMPSD.PSYS.Value != 0)
            //{
            //    txt.SetCtrlReadOnly(true);
            //}
            return txt;
        }

        /// <summary>
        /// 创建常规普通下拉框控件
        /// </summary>
        /// <param name="entityHRTMPSD"></param>
        /// <param name="strCtrlId"></param>
        /// <param name="strGroupType"></param>
        /// <returns></returns>
        public static GeneralDropDownList CreateGeneralDropDownList(Entity_TB_HRTMPSD entityHRTMPSD, String strCtrlId, String strGroupType) 
        {
            GeneralDropDownList ddList = new GeneralDropDownList();
            ddList.ID = strCtrlId;
            ddList.CssClass = "Select_edit_Archive";
            ddList.IsKey = entityHRTMPSD.PISKEY.Value.ToString();
            ddList.IsNull = entityHRTMPSD.PNULL.Value.ToString();
            ddList.DataType = entityHRTMPSD.PTYPE.ToString();
            ddList.NameEn = entityHRTMPSD.PDESC.ToString();
            ddList.NameCn = entityHRTMPSD.PDESCCHS.ToString();
            ddList.IsSave = entityHRTMPSD.PSAVE.ToString();
            ddList.GroupType = strGroupType;
            ddList.EnableViewState = false;

            //只读时属性设置
            switch (entityHRTMPSD.PRIGHT.Value)
            {
                case 0://可编辑
                    break;
                case 1://只读
                    ddList.SetCtrlReadOnly(true);
                    break;
                case 2://隐藏
                    break;
                case 3://排除
                    break;
            }
            ////系统参数只读设置
            //if (entityHRTMPSD.PSYS.Value != 0)
            //{
            //    ddList.SetCtrlReadOnly(true);
            //}

            return ddList;
        }

        /// <summary>
        /// 创建数据列表文本框控件
        /// </summary>
        /// <param name="entityHRTMPSD"></param>
        /// <param name="strMastValue"></param>
        /// <param name="strCtrlId"></param>
        /// <param name="strGroupType"></param>
        /// <returns></returns>
        public static DBTextBox CreateDBTextBox(Entity_TB_HRTMPSD entityHRTMPSD, String strMastValue, String strCtrlId, String strGroupType) 
        {
            DBTextBox txt = new DBTextBox();
            txt.ID = strCtrlId;
            txt.CssClass = "Text_edit_Archive";
            txt.IsKey = entityHRTMPSD.PISKEY.Value.ToString();
            txt.IsNull = entityHRTMPSD.PNULL.Value.ToString();
            txt.DataType = entityHRTMPSD.PTYPE.ToString();
            txt.NameEn = entityHRTMPSD.PDESC.ToString();
            txt.NameCn = entityHRTMPSD.PDESCCHS.ToString();
            txt.GroupType = strGroupType;
            txt.MaxLength = entityHRTMPSD.PLEN.Value;
            txt.IsSave = entityHRTMPSD.PSAVE.ToString();
            if (entityHRTMPSD.PWIDTH.Value <= 0)
            {
                txt.Width = Unit.Pixel(entityHRTMPSD.PWIDTH.Value);
            }
            //txt.Text = entityHRTMPSD.PDEFAULT;

            //控件中图标按钮控件设置
            txt.image.ID = "img_" + txt.ID;
            txt.image.ToolTip = strMastValue;
            txt.image.ImageUrl = "../../common/images/search1.png";
            txt.TID = entityHRTMPSD.TID;
            txt.SID = entityHRTMPSD.SID;
            txt.GID = entityHRTMPSD.GID;
            txt.PID = entityHRTMPSD.PID; ;
            txt.MASTVALUE = strMastValue;
            txt.SetImageClickFunction();
            txt.SetCtrlReadOnly(false);

            //权限属性设置
            switch (entityHRTMPSD.PRIGHT.Value)
            {
                case 0://可编辑
                    break;
                case 1://只读
                    txt.SetCtrlReadOnly(true);
                    break;
                case 2://隐藏
                    break;
                case 3://排除
                    break;
            }
            ////系统参数只读设置
            //if (entityHRTMPSD.PSYS.Value != 0)
            //{
            //    txt.SetCtrlReadOnly(true);
            //}

            return txt;
        }

        /// <summary>
        /// 创建图片选择浏览文本控件
        /// </summary>
        /// <param name="entityHRTMPSD"></param>
        /// <param name="strCtrlId"></param>
        /// <param name="strGroupType"></param>
        /// <param name="strKeyValue"></param>
        /// <returns></returns>
        public static ImageTextBox CreateImageTextBox(Entity_TB_HRTMPSD entityHRTMPSD, String strCtrlId, String strGroupType,String strKeyValue) 
        {
            ImageTextBox txt = new ImageTextBox();
            txt.ID = strCtrlId;
            txt.CssClass = "Text_edit_Archive";
            txt.IsKey = entityHRTMPSD.PISKEY.Value.ToString();
            txt.IsNull = entityHRTMPSD.PNULL.Value.ToString();
            txt.DataType = entityHRTMPSD.PTYPE.ToString();
            txt.NameEn = entityHRTMPSD.PDESC.ToString();
            txt.NameCn = entityHRTMPSD.PDESCCHS.ToString();
            txt.GroupType = strGroupType;
            txt.MaxLength = entityHRTMPSD.PLEN.Value;
            txt.IsSave = entityHRTMPSD.PSAVE.ToString();
            if (entityHRTMPSD.PWIDTH.Value <= 0)
            {
                txt.Width = Unit.Pixel(entityHRTMPSD.PWIDTH.Value);
            }
            //txt.Text = entityHRTMPSD.PDEFAULT;

            //控件中图标按钮控件及图片设置
            txt.image.ID = "img_" + txt.ID;
            txt.ImagePath = entityHRTMPSD.PCTRLD;
            if ((!String.IsNullOrEmpty(entityHRTMPSD.PCTRLID)) && (entityHRTMPSD.PCTRLID.Equals("1")))
            {
                txt.ImagePath = entityHRTMPSD.PCTRLD + "/" + strKeyValue;
            }
            txt.SetImageStyle();
            txt.image.Style.Add("behavior", "url(../HTC/ImageZoom.htc)");
            txt.image.CssClass = "Image_IMAGETEXT_Archive";
            txt.SetImgBtnAttributes();
            txt.SetCtrlReadOnly(false);

            //权限属性设置
            switch (entityHRTMPSD.PRIGHT.Value)
            {
                case 0://可编辑
                    break;
                case 1://只读
                    txt.SetCtrlReadOnly(true);
                    break;
                case 2://隐藏
                    break;
                case 3://排除
                    break;
            }
            ////系统参数只读设置
            //if (entityHRTMPSD.PSYS.Value != 0)
            //{
            //    txt.SetCtrlReadOnly(true);
            //}

            return txt;
        }

        /// <summary>
        /// 创建常规多选框控件
        /// </summary>
        /// <param name="entityHRTMPSD"></param>
        /// <param name="strCtrlId"></param>
        /// <param name="strGroupType"></param>
        /// <returns></returns>
        public static GeneralCheckBox CreateGeneralCheckBox(Entity_TB_HRTMPSD entityHRTMPSD, String strCtrlId, String strGroupType)
        {
            GeneralCheckBox ckBox = new GeneralCheckBox();
            ckBox.ID = strCtrlId;
            ckBox.CssClass = "CheckBox_edit_Archive";
            ckBox.IsKey = entityHRTMPSD.PISKEY.Value.ToString();
            ckBox.IsNull = entityHRTMPSD.PNULL.Value.ToString();
            ckBox.DataType = entityHRTMPSD.PTYPE.ToString();
            ckBox.NameEn = entityHRTMPSD.PDESC.ToString();
            ckBox.NameCn = entityHRTMPSD.PDESCCHS.ToString();
            ckBox.IsSave = entityHRTMPSD.PSAVE.ToString();
            ckBox.GroupType = strGroupType;

            //只读时属性设置
            switch (entityHRTMPSD.PRIGHT.Value)
            {
                case 0://可编辑
                    break;
                case 1://只读
                    ckBox.SetCtrlReadOnly(true);
                    break;
                case 2://隐藏
                    break;
                case 3://排除
                    break;
            }
            ////系统参数只读设置
            //if (entityHRTMPSD.PSYS.Value != 0)
            //{
            //    ckBox.SetCtrlReadOnly(true);
            //}

            return ckBox;
        }

        /// <summary>
        /// 创建DataGrid控件
        /// </summary>
        /// <param name="entityHRTMPSG"></param>
        /// <param name="strCtrlId"></param>
        /// <returns></returns>
        public static DataGrid CreateDataGrid(Entity_TB_HRTMPSG entityHRTMPSG, String strCtrlId) 
        {
            DataGrid dataGrid = new DataGrid(); 
            dataGrid.ID = strCtrlId;
            dataGrid.ShowHeader = true;
            dataGrid.Visible = true;
            dataGrid.AutoGenerateColumns = false;
            dataGrid.CssClass = "DataGrid_Style";
            dataGrid.HeaderStyle.CssClass = "DataGrid_HeaderStyle";
            dataGrid.ItemStyle.CssClass = "DataGrid_ItemStyle";
            dataGrid.Attributes.Add("GRIGHT", entityHRTMPSG.GRIGHT.Value.ToString());

            return dataGrid;
        }

        /// <summary>
        /// 创建上传附件类型控件
        /// </summary>
        /// <param name="entityHRTMPSD"></param>
        /// <param name="strCtrlId"></param>
        /// <param name="strGroupType"></param>
        /// <returns></returns>
        public static AttachmentCtrl CreateAttachmentCtrl(Entity_TB_HRTMPSD entityHRTMPSD, String strCtrlId, String strGroupType)
        {
            AttachmentCtrl attachCtrl = new AttachmentCtrl();
            attachCtrl.ID = strCtrlId;
            attachCtrl.IsKey = entityHRTMPSD.PISKEY.Value.ToString();
            attachCtrl.IsNull = entityHRTMPSD.PNULL.Value.ToString();
            attachCtrl.DataType = entityHRTMPSD.PTYPE.ToString();
            attachCtrl.NameEn = entityHRTMPSD.PDESC.ToString();
            attachCtrl.NameCn = entityHRTMPSD.PDESCCHS.ToString();
            attachCtrl.GroupType = strGroupType;
            attachCtrl.image.ImageUrl = "../../common/images/Attachment.png";
            attachCtrl.image.Width = Unit.Pixel(20);
            attachCtrl.image.Height = Unit.Pixel(20);
            attachCtrl.IsSave = entityHRTMPSD.PSAVE.ToString();

            return attachCtrl;
        }

        /// <summary>
        /// 创建明细链接类型控件
        /// </summary>
        /// <param name="entityHRTMPSD"></param>
        /// <param name="strCtrlId"></param>
        /// <param name="strGroupType"></param>
        /// <returns></returns>
        public static DetailLinkButtion CreateDetailLinkButtion(Entity_TB_HRTMPSD entityHRTMPSD, String strCtrlId, String strGroupType)
        {
            DetailLinkButtion detailLink = new DetailLinkButtion();
            detailLink.IsSave = entityHRTMPSD.PSAVE.ToString();
            detailLink.ID = strCtrlId;
            String strPctrlD = entityHRTMPSD.PCTRLD;
            if (!String.IsNullOrEmpty(strPctrlD))
            {
                String[] strArray = strPctrlD.Split(';');
                if (strArray.Length > 0)
                {
                    for (int i = 0; i < strArray.Length; i++)
                    {
                        String strTemp = strArray[i];
                        String[] strArray2 = strTemp.Split('=');
                        if (strArray2.Length > 0)
                        {
                            String strTemp2 = strArray2[0].ToUpper();
                            switch(strTemp2)
                            {
                                case "TID":
                                    detailLink.DTID = strArray2[1].ToString();
                                    break;
                                case "RID":
                                    detailLink.DRID = strArray2[1].ToString();
                                    break;
                                case "SID":
                                    detailLink.DSID = strArray2[1].ToString();
                                    break;
                                case "KEY":
                                    detailLink.DKEY = strArray2[1].ToString();
                                    break;
                            }
                        }

                    }
                }
            }

            return detailLink;
        }
        
        /// <summary>
        /// 创建格式化文本框控件
        /// </summary>
        /// <param name="entityHRTMPSD"></param>
        /// <param name="strCtrlId"></param>
        /// <param name="strGroupType"></param>
        /// <param name="strGroupType"></param>
        /// <returns></returns>
        public static MyFreeTextBox CreateFreeTextBox(Entity_TB_HRTMPSD entityHRTMPSD, String strCtrlId, String strGroupType)
        {
            MyFreeTextBox txt = new MyFreeTextBox();

            txt.ID = strCtrlId;
            txt.IsKey = entityHRTMPSD.PISKEY.Value.ToString();
            txt.IsNull = entityHRTMPSD.PNULL.Value.ToString();
            txt.DataType = entityHRTMPSD.PTYPE.ToString();
            txt.NameEn = entityHRTMPSD.PDESC.ToString();
            txt.NameCn = entityHRTMPSD.PDESCCHS.ToString();
            txt.GroupType = strGroupType;
            txt.Height = Unit.Pixel(entityHRTMPSD.PWIDTH.Value);
            txt.IsSave = entityHRTMPSD.PSAVE.ToString();
            if (entityHRTMPSD.PWIDTH.Value <= 0)
            {
                txt.Width = Unit.Pixel(entityHRTMPSD.PWIDTH.Value);
            }
            txt.Text = entityHRTMPSD.PDEFAULT;


            //权限属性设置
            switch (entityHRTMPSD.PRIGHT.Value)
            {
                case 0://可编辑
                    break;
                case 1://只读
                    txt.SetCtrlReadOnly(true);
                    break;
                case 2://隐藏
                    break;
                case 3://排除
                    break;
            }
            ////系统参数只读设置
            //if (entityHRTMPSD.PSYS.Value != 0)
            //{
            //    txt.SetCtrlReadOnly(true);
            //}
            return txt;
        }

    }
}
