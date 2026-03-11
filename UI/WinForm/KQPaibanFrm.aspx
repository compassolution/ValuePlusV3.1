<%@ Page Language="C#" AutoEventWireup="true" CodeFile="KQPaibanFrm.aspx.cs" Inherits="WinForm_KQPaibanFrm" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<title>员工排班</title>
<meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7">

<link href="../extjs/resources/css/ext-all.css" rel="stylesheet" type="text/css" />

<script src="../extjs/adapter/ext/ext-base.js" type="text/javascript"></script>
<script src="../extjs/ext-all.js" type="text/javascript"></script>
<script src="../common/JQuery/jquery-1.10.2.js"></script>

<script src="../common/js/stringUtil.js" type="text/javascript"></script>
<script src="../common/js/customObject.js" type="text/javascript"></script>
<style >
    body { font:12px/18px Arial, "宋体", Verdana, sans-serif; margin:5px;  background:#fff;	}
    
    .focusCellCss{background:#F9F;}
    .unNormalCellCss{background:#ff0000;}
    .unNormalxCellCss{background:#009900;}
    .unNormalnsCellCss{background:#0099ff;}
    .unNormalawCellCss{background:#00ee99;}
    .unNormalleCellCss{background:#ff00ff;}
    .unNormalxleCellCss{background:#ffaa22;}
    .unToNormalCellCss{background:#ffff00;}
    .x-grid3-row-over {cursor:hand; }
    
    
    .btnImage1{
        background: url(../common/images/Attachment.png) left top no-repeat !important;
        width:50px;
    }
	
	.msg .x-box-mc {
		font-size:12px;
		}
	#msg-div {
		position:absolute;
		left:650px;
		top:410px;
		width:600px;
		z-index:20000;
	}
	.msg-close{
		width:10px; height:10px; position:absolute; top:1px; right:10px;cursor:hand;
	}
	.msg-h3 {
		font-size:12px;
		color:#2870b2;
		font-weight:bold;
		margin:10px 0;
	}
</style>

</head>
<body>
<!--#include   file= "../common/WaitProccess.htm"--> 
<form id="form1" runat="server">
<asp:HiddenField ID="hfUserId" runat ="server" />
<asp:HiddenField ID="hfUserType" runat ="server" />
<asp:HiddenField ID="hfYearMonth" runat ="server" />

<asp:HiddenField ID="hfLanguage" runat ="server" />
<asp:HiddenField ID="hfBtnSave" runat ="server" />
<asp:HiddenField ID="hfBtnClose" runat ="server" />
<asp:HiddenField ID="hfBtnLoadInfo" runat ="server" />
<asp:HiddenField ID="hfTipTitle" runat ="server" />
<asp:HiddenField ID="hfTipTip" runat ="server" />
<asp:HiddenField ID="hfTipSelectMonth" runat ="server" />
<asp:HiddenField ID="hfTipMsg" runat ="server" />
<asp:HiddenField ID="hfTipSaveMsg" runat ="server" />
<asp:HiddenField ID="hfTipNoSave" runat ="server" />
<asp:HiddenField ID="hfTipCanCopy" runat ="server" />
<asp:HiddenField ID="hfTipGridColumnsText" runat ="server" />
<asp:HiddenField ID="hfTipGridSortAscText" runat ="server" />
<asp:HiddenField ID="hfTipGridSortDescText" runat ="server" />
<asp:HiddenField ID="hfBtnExcuteSp_Analyse" runat ="server" />
<asp:HiddenField ID="hfTipNoAnalyseData" runat ="server" />
<asp:HiddenField ID="hfTipAnalyseSuccess" runat ="server" />
<asp:HiddenField ID="hfTipNoAnalyseFailed" runat ="server" />
<asp:HiddenField ID="hfTipTobeAnalyse" runat ="server" />
<asp:HiddenField ID="hfTipPeriodLocked" runat ="server" />

<asp:HiddenField ID="hfTipAnalysStatus1" runat ="server" />
<asp:HiddenField ID="hfTipAnalysStatus2" runat ="server" />
<asp:HiddenField ID="hfTipAnalysStatus3" runat ="server" />

<asp:HiddenField ID="hfListNormal" runat ="server" />
<asp:HiddenField ID="hfListUnNormal" runat ="server" />
<asp:HiddenField ID="hfMenuCopyShifCode" runat ="server" />
<asp:HiddenField ID="hfMenuShowUnNormalGird" runat ="server" />
<asp:HiddenField ID="hfBatchAdjustResult" runat ="server" />
<asp:HiddenField ID="hfMenuCopyThisRow" runat ="server" />
<asp:HiddenField ID="hfMenuPasteToRow" runat ="server" />
<asp:HiddenField ID="hfMenuViewResultGrid" runat ="server" />
<asp:HiddenField ID="hfBtnAdjustResult" runat ="server" />
<asp:HiddenField ID="hfBtnAdjustOTREST" runat ="server" />
<asp:HiddenField ID="hfLVOVMustInt" runat ="server" />
<asp:HiddenField ID="hfMenuFillCheckInOut" runat ="server" />
<asp:HiddenField ID="hfMenuAnalyzeTheStaff" runat ="server" />
<asp:HiddenField ID="hfMenuLVOTRecord" runat ="server" />
<asp:HiddenField ID="hfMenuCheckInOutRecord" runat ="server" />
<asp:HiddenField ID="hfMenuAttendanceSummary" runat ="server" />
<asp:HiddenField ID="hfMenuAttendanceAnalysisResult" runat ="server" />
<asp:HiddenField ID="hfMenuAttendanceReport" runat ="server" />
<asp:HiddenField ID="hfMenuAttendanceExceptionReport" runat ="server" />
<asp:HiddenField ID="hfMenuOvertimeBalanceReport" runat ="server" />
<asp:HiddenField ID="hfMenuLeaveDocument" runat ="server" />
<asp:HiddenField ID="hfMenuOvertimeDocument" runat ="server" />

<asp:HiddenField ID="hfIsCanEditTx" runat ="server" />
<asp:HiddenField ID="hfIsCanEditJb" runat ="server" />
<asp:HiddenField ID="hfIsCanEditIsToNormal" runat ="server" />

<asp:HiddenField ID="hfIsRealTimeSave" runat ="server" />
<asp:HiddenField ID="hfIsRealTimeAnalyse" runat ="server" />
<asp:HiddenField ID="hfCurServerDate" runat ="server" />
<asp:HiddenField ID="hfCurServerTime" runat ="server" />

<asp:HiddenField ID="hfIsKQAnalyseByStaff" runat ="server" />
<asp:HiddenField ID="hfIsShowPageAfterKQAnalyse" runat ="server" />
<asp:HiddenField ID="hfIsUseOA_LV" runat ="server" />
<asp:HiddenField ID="hfIsUseOA_OT" runat ="server" />
<asp:HiddenField ID="hfTxtShiftCodeFilter" runat ="server" />

<script>
    //  window.moveTo(0,0);  
    //  window.resizeTo(screen.availWidth,screen.availHeight);
    /**
    *信息提示框，显示后迅速消失
    */
    ShowMsg = function () {
        var msgCt;

        function createBox(t, s, isClose) {
            var html = new Array();

            html.push('<div class="msg">');
            html.push('<div class="x-box-tl"><div class="x-box-tr"><div class="x-box-tc">');
            html.push('</div></div></div>');
            html.push('<div class="x-box-ml"><div class="x-box-mr"><div class="x-box-mc">');
            if (t) {
                html.push('<h3 class="msg-h3">');
                html.push(t);
                html.push('</h3>');
            }
            if (isClose) {
                html.push('<span class="msg-close" onclick="ShowMsg.close()"><img src="' + closeImageUrl + '" mce_src="' + closeImageUrl + '"/></span>');
            }
            html.push(s);
            html.push('</div></div></div>');
            html.push('<div class="x-box-bl"><div class="x-box-br"><div class="x-box-bc"></div></div></div>');
            html.push('</div>');
            return html.join('');
        }
        return {
            /**
            * 显示信息
            * title:标题
            * msg:提示信息
            * time：显示时间，超时后自动消失
            * alignEl：对齐到该对象的左下角
            * offsets[Array]：横向偏移像素，比如：[200,0]标识右移200个像素
            * positon：动画
            * animate[boolean]：是否开启动画 
            * isClose[boolean]：是否可关闭
            */
            showtip: function (title, msg, width, time, alignEl, offsets, position, animate, isClose) {
                width = width ? width : '250px';
                time = time ? time : 2;
                alignEl = alignEl ? alignEl : document;
                position = position ? position : 't-t';
                animate = animate ? animate : false;
                this.close();
                if (!msgCt) {
                    msgCt = Ext.DomHelper.insertFirst(document.body, { id: 'msg-div' }, true);
                    msgCt.setWidth(width);
                }
                //采用默认动画将div的最中央对齐到alignEl的左下角,并右移200个象素,且不能超出窗口
                msgCt.alignTo(alignEl, position, offsets, animate);
                var m = Ext.DomHelper.append(msgCt, { html: createBox(title, msg, isClose) }, true);
                m.slideIn('t').pause(time).ghost("b", { remove: true }); //元素从上滑入效果,可不带参数调用下同  
            },

            //提示信息
            alerttip: function (msg, field, alignEl, width) {
                width = width ? width : '150px';
                this.showtip('', html, '150px', 2, field, [120, 0], 't-t', true, false);
                var html = '<span style="font-size:9px;" mce_style="font-size:9px;">' + msg + '</span>';
            },

            close: function () {
                var div = document.getElementById('msg-div');
                if (div) {
                    div.style.display = 'none';
                }
                msgCt = '';
            }
        };
    } ();

</script>   
    <div id="div1" style="width:100%">
    
    </div>
    
    <div id="div2" style="width:100%;height:100%; text-align:center; vertical-align:middle;display:none" class="ext-el-mask" align="center">
        <img src="../extjs/resources/images/default/shared/large-loading.gif" style="width:15; height:15" />
        <font color="gray" >Loading.......</font>
        <br\>
        <div align="center" style="width:36%">
        <div id="divWaitingTitle" align="center" style="text-align:left;"></div>
        <div id="divWaitingTip" align="center" style="text-align:left;"></div>
            </div>
    </div>
    
    <div id="div3">
	
    </div>
    
    <%--定义显示/隐藏 操作处理中层--Grid--%>
    <script type="text/javascript">
        function showOpWaittingDiv() {//显示操作处理中层
            Ext.fly("div2").fadeIn();
        }
        function hideOpWaittingDiv() {//操作完成，隐藏处理中层
            Ext.fly("div2").fadeOut();
            $("#divWaitingTitle").empty();
            $("#divWaitingTip").empty();
        }
        showOpWaittingDiv(); 

      
    </script>
    <%--公共函数 --%>
    <script type="text/javascript">
        //请求超时时间ms(默认等候服务器响应时长为30000,即30秒)
        Ext.Ajax.timeout = 600000; //


        function CopyClip(text) {
            clipboardData.setData('text', (text));
        }

        //是否允许当天日期前的排班可修改
        var IsCanModify_BeforeToday = false;
//        alert(!IsCanModify_BeforeToday);
    </script>

    <%--声明全局的变量--%>
    <script type="text/javascript">
        var curServerDate = document.getElementById("hfCurServerDate").value;

        //为获取动态生成的列而设置的变量
        var datacol = null;
        var datacol_Shif = null;
        var datacol_Result = null;
        //为获取当前表格数据的store
        var StoreGrid = null;
        var StoreGrid_Shif = null;
        var StoreGrid_Result = null;
        //排班表格的列数
        var colCount_Paiban = 0;
        //员工考勤结果表格的列数
        var colCount_Result = 0;
        //被选择的班次值
        var varShiftValue = "";
        //进入该页面将剪贴板清空
        //        clipboardData.setData('text',"");
        var varSetPaiBanValue = "";
        //为了读取特定员工的考勤结果信息，设置的全局变量
        var curGridStuffNo = ""; //员工编号
        var curGridYearMonth = ""; //考勤月份
        var curGridDay = ""; //所选择的日期

        //为了复制某行排班数据到另一行的排班信息hashtable
        var hashPaibanRow = null;
        //为了实现异常原始状态和调整后的状态来回切换Michael 160316
        var showunoramlornot = "Y";
        //为了记录某个cell是否存在css的hashtable
        var hashCellPoint = null;
        //当前月份是否锁定的标志位(1、锁定，2未锁定)
        var curYearMonthIsLock = "";
        //当前月份当前小部门考勤分心状态标志位(1、部门分析，2人事部审核)
        var curSectionAnalysStatus = "";
        //获取当前排班月份的起始时间和结束时间
        var curMonthStartDay = "";
        var curMonthEndDay = "";


        //记录日期DAY是否执行隐藏的标志
        var bIsHiddenDay1 = false;
        var bIsHiddenDay2 = false;
        var bIsHiddenDay3 = false;


        var IsCanEditTx = document.getElementById("hfIsCanEditTx").value;
        var IsCanEditJb = document.getElementById("hfIsCanEditJb").value;
        var IsCanEditIsToNormal = document.getElementById("hfIsCanEditIsToNormal").value;
        var varUserId = document.getElementById("hfUserId").value
        var varUserType = document.getElementById("hfUserType").value
    </script>  
      
    <%--读取存在日期的xml暂时不用 --%>
    <script type="text/javascript">
        var storeDayItem = new Ext.data.Store({
            proxy: new Ext.data.HttpProxy({ url: 'DayItem.xml', method: 'GET' }),
            reader: new Ext.data.XmlReader(
            // records will have an "Item" tag
             {record: 'item' },
             [
            // set up the fields mapping into the xml doc to extract *attributes*
                {name: 'value', mapping: '@value' },
                { name: 'id', mapping: '@id' }
          ])
        });
        storeDayItem.on('load', AJAX_Loaded, this, true); //这里需要注意
        storeDayItem.load();


        function AJAX_Loaded() {
            //         alert(storeDayItem.getCount());
            for (var i = 0; i < storeDayItem.getCount(); i++) {
                var rec = storeDayItem.getAt(i);
                alert("value = '" + rec.get("value"));
                alert("id = '" + rec.get("id"));
            }
        }


    </script>
    
    <%--排班月份数据加载--ComboBox--%>
    <script type="text/javascript">
        var paramYearMonth = document.getElementById("hfYearMonth").value
        var clientYearMonth = paramYearMonth;
        if (clientYearMonth == "" || clientYearMonth == null) {
            var varClientCurDate = new Date();
            var clientYear = varClientCurDate.getFullYear().toString();       //年
            var clientMonth = varClientCurDate.getMonth() + 1;     //月
            if (clientMonth < 10) clientMonth = "0" + clientMonth.toString();
            clientYearMonth = clientYear.substr(2, 2) + clientMonth;
        }


        function MakeComboBox() {
            var storeCombo = new Ext.data.Store
            ({
                proxy: new Ext.data.HttpProxy({ url: "Ajax/KQPaibanAjax.aspx?param=KQPerd" + "&userId=" + varUserId }),
                reader: new Ext.data.JsonReader({}, [{ name: 'PID'}])
            });
            storeCombo.load();
            var cboYearMonth = new Ext.form.ComboBox
            ({
                id: "cboYearMonth",
                editable: false,
                store: storeCombo,
                emptyText: document.getElementById("hfTipSelectMonth").value,
                typeAhead: true,
                triggerAction: 'all',
                valueField: 'PID',
                displayField: 'PID',
                selectOnFocus: true,
                width: 120,
                resizable: true,
                listeners: {
                    Render: function (combo) {
                        combo.setValue(clientYearMonth); //默认值
                        combo.fireEvent('select', '');


                        //初始化是否实时保存排班信息的界面控制 add by sammen 20120618
                        if (document.getElementById("hfIsRealTimeSave").value == "1") {
                            Ext.getCmp("btnSave").hide();
                            Ext.getCmp("btnTemp2").hide();
                        } else {
                            Ext.getCmp("btnSave").show();
                            Ext.getCmp("btnTemp2").show();
                        }
                    }
                }

            });
            cboYearMonth.on("select", function () {
                //删除所有GRID的渲染
                if (Ext.getCmp("editGridPanel")) { Ext.getCmp("formOne").remove(Ext.getCmp("editGridPanel")); }
                if (Ext.getCmp("StuffResultGridPanel")) { Ext.getCmp("formOne").remove(Ext.getCmp("StuffResultGridPanel")); }
                if (Ext.getCmp("btnSaveResult")) { Ext.getCmp("formOne").remove(Ext.getCmp("btnSaveResult")); }

                //获取当前月份是否被锁定的信息
                var varYear = cboYearMonth.getValue();
                Ext.Ajax.request({
                    url: "Ajax/KQPaibanAjax.aspx?param=GetKQPredLock&yearMonth=" + varYear + "&userId=" + varUserId,
                    method: "POST",
                    success: function (p1, p2) {
                        var de1 = Ext.decode(p1.responseText);
                        if (de1 != null) {
                            var strTemp = de1[varYear];
                            var arrTmp = strTemp.split("*");
                            curMonthStartDay = arrTmp[0]; //当前排班月起始日期
                            curMonthEndDay = arrTmp[1]; //当前排班月结束日期
                            curYearMonthIsLock = arrTmp[2]; //当前排班月是否锁定标志
                            curSectionAnalysStatus = arrTmp[3]; //当前排班月当前部门考勤分析状态标志
                            var varShowTips = document.getElementById("hfTipTitle").value + "（From：" + curMonthStartDay + "---->To：" + curMonthEndDay + "）";
                            if (curYearMonthIsLock == 1) {
                                varShowTips = varShowTips + document.getElementById("hfTipPeriodLocked").value
                            }
                            if (curSectionAnalysStatus == 1) {
                                varShowTips = varShowTips + "----" + document.getElementById("hfTipAnalysStatus1").value
                            }
                            if (curSectionAnalysStatus == 2) {
                                varShowTips = varShowTips + "----" + document.getElementById("hfTipAnalysStatus2").value
                            }
                            if (curSectionAnalysStatus == 3) {
                                varShowTips = varShowTips + "----" + document.getElementById("hfTipAnalysStatus3").value
                            }
                            Ext.getCmp("formOne").setTitle(varShowTips);
                        }
                        //                         Ext.Msg.alert('cur',curMonthStartDay+"_"+curMonthEndDay+"_"+curSectionAnalysStatus);
                    }
                })
                Ext.getCmp("formOne").doLayout();
                //同时加载员工排班列表 --add by sammen 20160129
                LoadStaffPaibanList();
            });


            this.ComboBox = cboYearMonth;


            if (paramYearMonth != "" && paramYearMonth != null) {
                this.ComboBox.disable();
            }
        }
     </script>
     
    <%--排班班次数据--Grid--%>
    <script type="text/javascript">
        function MakeDataColumn_Shif() {//加载班次数据列
            this.fields = '';
            this.columns = '';
            this.addColumns = function (name, caption) {
                //alert(name+'jia'+caption);
                if (this.fields.length > 0) {
                    this.fields += ',';
                }
                if (this.columns.length > 0) {
                    this.columns += ',';
                }
                this.fields += '{name:"' + name + '"}';
                if ((name == 'SHCODE') || (name == 'SHNAME')) {
                    //                    this.columns += '{header:"' + caption + '",dataIndex:"' + name + '",width:200,sortable:true,hidden:false,css:"background: #FFCCFF;"}';
                    this.columns += '{header:"' + caption + '",dataIndex:"' + name + '",width:200,sortable:true,hidden:false}';
                } else {

                    //                    this.columns += '{header:"' + caption + '",dataIndex:"' + name + '",width:200,sortable:true,hidden:false,css:"background: #FFCCFF;"}';
                    this.columns += '{header:"' + caption + '",dataIndex:"' + name + '",width:200,sortable:true,hidden:false}';
                }


            }
        }


        function MakeGridView_Shif() {//加载排班数据
            var cm = new Ext.grid.ColumnModel(eval('([' + datacol_Shif.columns + '])'));
            cm.defaultSortable = true;
            var fields = eval('([' + datacol_Shif.fields + '])');
            //alert(fields);
            StoreGrid_Shif = new Ext.data.Store
            ({
                proxy: new Ext.data.HttpProxy({ url: "Ajax/KQPaibanAjax.aspx?param=KQShifDataInfo" + "&userId=" + varUserId }),
                reader: new Ext.data.JsonReader({ totalProperty: "KQShifTotalPorperty", root: "KQShifResult", fields: fields }),
                storeId: "SHCODE"
            });
            StoreGrid_Shif.removeAll();
            StoreGrid_Shif.load({});
            var pagingBar = new Ext.PagingToolbar
            ({
                emptyMsg: "没有数据显示",
                displayMsg: "显示从{0}条数据到{1}条数据，共{2}条数据",
                store: StoreGrid_Shif
                //pageSize: 9
            });


            var gridPanel_Shif = new Ext.grid.GridPanel
            ({
                cm: cm,
                id: "ShifGridPanel",
                tabTip: document.getElementById("hfTipCanCopy").value,
                columnWidth: .1,
                store: StoreGrid_Shif,
                autoScroll: true,
                autoWidth: false,
                region: 'north',
                height: 150,
                stripeRows: true,
                viewConfig: { forceFit: true },
                columnLines: true,
                trackMouseOver: true,
                sm: new Ext.grid.RowSelectionModel({ singleSelect: true }),
                viewConfig: {

                    columnsText: document.getElementById("hfTipGridColumnsText").value,
                    sortAscText: document.getElementById("hfTipGridSortAscText").value,
                    sortDescText: document.getElementById("hfTipGridSortDescText").value,
                    forceFit: true
                }

            });
            gridPanel_Shif.on("cellcontextmenu", function (grid, rowIndex, columnIndex, e) {
                e.preventDefault();
                if (rowIndex < 0) { return; }
                var treeMenu = new Ext.menu.Menu
                ([
                    { text: document.getElementById("hfMenuCopyShifCode").value, handler: function () { DbClickShifRow(grid, rowIndex, e); treeMenu.hide(); } },
                    { text: 'ShowDetail', handler: function () { ShowShifDetail(grid, rowIndex, e); treeMenu.hide(); } }
                ]);
                treeMenu.showAt(e.getPoint());
            });
            this.GridView = gridPanel_Shif;

            gridPanel_Shif.addListener('rowdblclick', DbClickShifRow);
            //            gridPanel_Shif.addListener('rowmousedown', ShowShifDetail);        
        }
        
        //刷新班次列表数据 add by sammen 20191010
        function RefreshGridView_Shif(strFilterSql) {
            strFilterSql = encodeURI(strFilterSql);
            var cm = new Ext.grid.ColumnModel(eval('([' + datacol_Shif.columns + '])'));
            var fields = eval('([' + datacol_Shif.fields + '])');
            StoreGrid_Shif = new Ext.data.Store
            ({
                proxy: new Ext.data.HttpProxy({ url: "Ajax/KQPaibanAjax.aspx?param=KQShifDataInfo" + "&userId=" + varUserId + "&filterSql=" + strFilterSql }),
                reader: new Ext.data.JsonReader({ totalProperty: "KQShifTotalPorperty", root: "KQShifResult", fields: fields }),
                storeId: "SHCODE"
            });
            StoreGrid_Shif.removeAll();
            StoreGrid_Shif.load({});
            //重新绑定grid列表
            Ext.getCmp("ShifGridPanel").reconfigure(StoreGrid_Shif, cm);
        }


        //双击班次信息某一行事件,同时将选中行的字段SHCODE值填充到排版表格中处于编辑状态的网格中
        var DbClickShifRow = function (grid, rowIndex, e) {
            var selectionModel = grid.getSelectionModel();
            selectionModel.selectRow(rowIndex);
            var record = selectionModel.getSelected();
            var varSHCode = record.data['SHCODE'];
            varShiftValue = varSHCode;
            varSetPaiBanValue = varSHCode;
            //            CopyClip(varSHCode);


        }
        //查看班次信息某详细信息
        var ShowShifDetail = function (grid, rowIndex, e) {
            var selectionModel = grid.getSelectionModel();
            selectionModel.selectRow(rowIndex);
            var record = selectionModel.getSelected();
            var varSHCODE = record.data['SHCODE'];
            var varSHNAME = record.data['SHNAME'];
            var varSHIN1 = record.data['IN1'];
            var varSHOUT1 = record.data['OUT1'];
            var varSHIN2 = record.data['IN2'];
            var varSHOUT2 = record.data['OUT2'];
            var strShow = 'CODE：' + varSHCODE + '<br/>NAME：' + varSHNAME
                        + '<br/>IN1：&nbsp;&nbsp;&nbsp;' + varSHIN1 + '<br/>OUT1：' + varSHOUT1
                        + '<br/>IN2：&nbsp;&nbsp;&nbsp;' + varSHIN2 + '<br/>OUT2：' + varSHOUT2;
            Ext.Msg.alert('Shift Detail', strShow);


        }


        //加载排班班次信息
        function loadKQShifInfo() {
            Ext.Ajax.request({
                url: "Ajax/KQPaibanAjax.aspx?param=KQShifColumnInfo" + "&userId=" + varUserId,
                method: "POST",
                success: function (p1, p2) {
                    var de1 = Ext.decode(p1.responseText);
                    datacol_Shif = new MakeDataColumn_Shif();
                    for (var j = 0; j < de1.length; j++) {
                        for (var q in de1[j]) {
                            datacol_Shif.addColumns(q, de1[j][q]);
                        }
                    }


                    if (Ext.getCmp("ShifGridPanel")) { Ext.getCmp("formOne").remove(Ext.getCmp("ShifGridPanel")); }
                    var grid_Shif = new MakeGridView_Shif().GridView;
                    Ext.getCmp("formOne").add(grid_Shif);

                    Ext.getCmp("formOne").doLayout();
                }
            })


        }
    </script>
   
    <%--选取排班月份后后显示其排班数据--Grid--%>
    <script type="text/javascript">
        function MakeDataColumn() {//加载排班数据列
            var curSelYearMonth = Ext.getCmp("cboYearMonth").getValue();
            var indexDayCol = 0;
            this.fields = '';
            this.columns = '';
            this.addColumns = function (name, caption) {
                if (this.fields.length > 0) {
                    this.fields += ',';
                }
                if (this.columns.length > 0) {
                    this.columns += ',';
                }
                this.fields += '{name:"' + name + '"}';
                ////modify by sammen 20191010新增部门名称列的显示
                if ((name == '1EMPDEPT') || (name == '1EMPLOYEE') || (name == 'YEARMONTH') || (name == 'DeptNameChs') || (name == 'DeptName')) {
                    this.columns += '{header:"' + caption + '",dataIndex:"' + name + '",width:180,sortable:true,css:"bgcolor:black;background: #9F6;",hidden:true}';
                } else

                    if ((name == 'CNAME') || (name == 'DCPOSICHS') || (name == 'EMPLOYEE')) {
                        if (document.getElementById("hfLanguage").value == "zh-cn") {//如果是中文环境则显示中文名
                            this.columns += '{header:"' + caption + '",dataIndex:"' + name + '",width:180,sortable:true,css:"bgcolor:black;background: #9F6;",hidden:false}';
                        } else {
                            this.columns += '{header:"' + caption + '",dataIndex:"' + name + '",width:180,sortable:true,css:"bgcolor:black;background: #9F6;",hidden:true}';
                        }
                    } else if ((name == 'ENAME') || (name == 'DCPOSI') || (name == 'EMPLOYEE')) {
                        if (document.getElementById("hfLanguage").value == "zh-cn") {//如果是英文环境则显示英文名
                            this.columns += '{header:"' + caption + '",dataIndex:"' + name + '",width:180,sortable:true,css:"bgcolor:black;background: #9F6;",hidden:true}';
                        } else {
                            this.columns += '{header:"' + caption + '",dataIndex:"' + name + '",width:180,sortable:true,css:"bgcolor:black;background: #9F6;",hidden:false}';
                        }
                    } else {
                        if (isFixDayCol(name)) {
                            var colBackNum = name.substring(1, name.length); //这些列必须是D1,D2,D3,D4.....D31
                            var iColBackNum = parseInt(colBackNum); //取字段名后面的数字

                            var colDayShowName = caption.split("*")[0];
                            var colDayIsLock = caption.split("*")[1];
                            var colDayIsLockShowName = colDayIsLock == "1" ? "已锁定" : "可排班";
                            if (document.getElementById("hfLanguage").value != "zh-cn") {//如果不是中文环境{
                                colDayIsLockShowName = colDayIsLock == "1" ? "Locked" : "Active";
                            }

                            //如果当月或者当天锁定或者审核状态或者已完成分析，则列头显示红色
                            if ((curYearMonthIsLock == 1) || (curSectionAnalysStatus == 3) || ((varUserType != 1) && (curSectionAnalysStatus != 1))||(colDayIsLock=='1')) {
                                colDayShowName = "<span style='color:red;'>" + colDayShowName + "</span>";
                            }
                            
                            //周中和周末不同列颜色显示
                            var cssSpeCol = "";
                            var varCurDayTemp = new Date(curMonthStartDay.replace("-", "/").replace("-", "/")).add(Date.DAY, +iColBackNum - 1);
                            if (varCurDayTemp.getDay() == 5) {//周五
                                cssSpeCol = 'css:"border-right-color: blue;",';
                            } else if (varCurDayTemp.getDay() == 0) {//周日
                                cssSpeCol = 'css:"border-right-color: blue;",';
                            }

                            //                          if(isWeekEnd(curSelYearMonth,colBackNum,curMonthStartDay)==5){//周五
                            //                            cssSpeCol = 'css:"border-right-color: blue;",';
                            //                          }else if(isWeekEnd(curSelYearMonth,colBackNum,curMonthStartDay)==0){//周日
                            //                            cssSpeCol = 'css:"border-right-color: blue;",';
                            //                          }

                            //如果当前月份被锁定，则所有都不可编辑
                            //增加逻辑：分析状态为完成或者是小部门操作且非小部门分析状态
                            if ((curYearMonthIsLock == 1) || (curSectionAnalysStatus == 3) || ((varUserType != 1) && (curSectionAnalysStatus != 1))) {
                                //                        if(curYearMonthIsLock==1){
                                this.columns += '{header:"' + colDayShowName + '",dataIndex:"' + name + '",width:100,sortable:true,' + cssSpeCol + 'hidden:false,tooltip:"' + colDayIsLockShowName + '"}';
                            } else {
                                //判断如果是今天之前的日期，则不可编辑/ add by sammen 20160803
                                var buildDate = curMonthStartDay.toDate();
                                var strBuildDate = AddDays(buildDate, indexDayCol);

                                //if ((strBuildDate < curServerDate) && (!IsCanModify_BeforeToday) && (strBuildDate<'2016-09-20')) {
                                if (colDayIsLock=='1') {
                                    this.columns += '{header:"' + colDayShowName + '",dataIndex:"' + name + '",width:100,sortable:true,' + cssSpeCol + 'hidden:false,tooltip:"' + colDayIsLockShowName + '"}';
                                } else {
                                    this.columns += '{header:"' + colDayShowName + '",dataIndex:"' + name + '",width:100,sortable:true,' + cssSpeCol + 'hidden:false,tooltip:"' + colDayIsLockShowName + '",'
                                    //                                + 'editor:new Ext.form.TextField({maxLength:3})}';
                                    //                                + 'editor:new Ext.form.TextField({maxLength:3,enableKeyEvents: true,validator:function(){if(checkInputShif(this.getValue())){return true;}else{return false;} }})}';//ADD BY WSM 20110323
                                + 'editor:new Ext.form.TextField({maxLength:3,disabled:true})}'; //ADD BY WSM 20110701 or 'readonly'
                                    //+ 'editor:new Ext.form.TextField({maxLength:8,disabled:false,selectOnFocus:true})}'; //modify BY WSM 20190614班次可以手工输入
                                }
                            }
                            var checkInputShif = function (val) {
                                //                            var temp = 'AR1';
                                //                            if(val = temp){
                                //                                return true;
                                //                            }else{
                                //                                return false;
                                //                            }
                            };
                            indexDayCol++;
                        }
                    }
            }
        }

        //加载排班数据【用在过滤行记录时使用】add by sammen 20190628
        function LoadStaffGridData() {
            var cm = new Ext.grid.ColumnModel(eval('([' + datacol.columns + '])'));
            var fields = eval('([' + datacol.fields + '])');
            var curYearMonth = Ext.getCmp("cboYearMonth").getValue();

            var FilterValue_StaffNo = "700299";
            var curCondition = " 1=1 ";
            if (FilterValue_StaffNo != '') {
                curCondition = curCondition + " and B.EMPLOYEE like '%" + FilterValue_StaffNo + "%'";
            }
            curCondition = encodeURI(curCondition);
            var tempStoreGrid = new Ext.data.Store
            ({
                proxy: new Ext.data.HttpProxy({ url: "Ajax/KQPaibanAjax.aspx?param=KQPaiBanDataInfo&yearMonth=" + curYearMonth + "&userId=" + varUserId + "&condition=" + curCondition }),
                reader: new Ext.data.JsonReader({ totalProperty: "KQPaiBanTotalPorperty", root: "KQPaiBanResult", fields: fields }),
                storeId: "EMPLOYEE"
            });
            tempStoreGrid.removeAll();
            tempStoreGrid.load({ params: { start: 0, limit: 9 } });
            //重新绑定grid列表
            Ext.getCmp("editGridPanel").reconfigure(tempStoreGrid, cm);
        }

        function MakeGridView() {//加载排班数据
            var cm = new Ext.grid.ColumnModel(eval('([' + datacol.columns + '])'));
            cm.defaultSortable = true;
            var fields = eval('([' + datacol.fields + '])');
            var curYearMonth = Ext.getCmp("cboYearMonth").getValue();
            //var curCondition = " B.EMPLOYEE = '700299' ";
            StoreGrid = new Ext.data.Store
            ({
                proxy: new Ext.data.HttpProxy({ url: "Ajax/KQPaibanAjax.aspx?param=KQPaiBanDataInfo&yearMonth=" + curYearMonth + "&userId=" + varUserId }),
                //proxy: new Ext.data.HttpProxy({ url: "Ajax/KQPaibanAjax.aspx?param=KQPaiBanDataInfo&yearMonth=" + curYearMonth + "&userId=" + varUserId + "&condition=" + curCondition }),
                reader: new Ext.data.JsonReader({ totalProperty: "KQPaiBanTotalPorperty", root: "KQPaiBanResult", fields: fields }),
                storeId: "EMPLOYEE"
            });
            StoreGrid.removeAll();
            StoreGrid.load({ params: { start: 0, limit: 9} });
            var pagingBar = new Ext.PagingToolbar
            ({
                displayInfo: true,
                emptyMsg: "没有数据显示",
                displayMsg: "显示从{0}条数据到{1}条数据，共{2}条数据",
                store: StoreGrid
                //pageSize: 9
            });
            var gridPanel_Paiban = new Ext.grid.EditorGridPanel
            ({
                cm: cm,
                id: "editGridPanel",
                columnWidth: .9,
                store: StoreGrid,
                frame: false,
                border: true,
                region: 'center',
                pageSize: 16,
                height: 370,
                stripeRows: true,
                viewConfig: { forceFit: true },
                clicksToEdit: 2,
                columnLines: true,
                trackMouseOver: true,
                sm: new Ext.grid.RowSelectionModel({ singleSelect: false }),
                viewConfig: {

                    columnsText: document.getElementById("hfTipGridColumnsText").value,
                    sortAscText: document.getElementById("hfTipGridSortAscText").value,
                    sortDescText: document.getElementById("hfTipGridSortDescText").value,
                    forceFit: true
                }
            });
            gridPanel_Paiban.on("contextmenu", function (grid, rowIndex, columnIndex, e) {
                var treeMenu = new Ext.menu.Menu
                ([
                    { text: document.getElementById("hfMenuShowUnNormalGird").value, handler: function () { cellContext_showUnNormal(grid); treeMenu.hide(); } }
                ]);
                //                treeMenu.showAt(e.getPoint());
            });
            gridPanel_Paiban.on("cellcontextmenu", function (grid, rowIndex, columnIndex, e) {
                e.preventDefault();
                if (rowIndex < 0) { return; }
                clickPaiBanCell(grid, rowIndex, columnIndex, e, "2"); //右键点击
                var treeMenu = new Ext.menu.Menu
                ([
                //                    {text:"还原至编辑前",handler:function(){cellContext_restore(grid,rowIndex,columnIndex,e);treeMenu.hide();}},
                    {text: document.getElementById("hfMenuCopyThisRow").value,
                    handler: function () {
                        showOpWaittingDiv();
                        cellContext_copyPaibanRow(grid, rowIndex);
                        treeMenu.hide();
                        hideOpWaittingDiv();
                    }
                },

                    { text: document.getElementById("hfMenuPasteToRow").value,
                        handler: function () {
                            showOpWaittingDiv();
                            //如果当月被锁定则不可编辑
                            //增加逻辑：分析状态为完成或者是小部门操作且非小部门分析状态
                            if (curYearMonthIsLock == 1) {
                                treeMenu.hide();
                                hideOpWaittingDiv();
                                Ext.Msg.alert(document.getElementById("hfTipTip").value, Ext.getCmp("cboYearMonth").getValue() + "：" + document.getElementById("hfTipPeriodLocked").value);

                            } else if (curSectionAnalysStatus == 3) {
                                treeMenu.hide();
                                hideOpWaittingDiv();
                                Ext.Msg.alert(document.getElementById("hfTipTip").value, Ext.getCmp("cboYearMonth").getValue() + "：" + document.getElementById("hfTipAnalysStatus3").value);

                            } else if ((varUserType != 1) && (curSectionAnalysStatus != 1)) {
                                treeMenu.hide();
                                hideOpWaittingDiv();
                                Ext.Msg.alert(document.getElementById("hfTipTip").value, Ext.getCmp("cboYearMonth").getValue() + "：" + document.getElementById("hfTipAnalysStatus2").value);

                            } else {

                                cellContext_pastePaibanRow(grid, rowIndex);
                                treeMenu.hide();
                                hideOpWaittingDiv();
                            }
                        }
                    },
                    { text: document.getElementById("hfMenuViewResultGrid").value,
                        handler: function () {
                            //                            showOpWaittingDiv();
                            viewStuffResultGrid(grid, rowIndex, columnIndex, e);
                            treeMenu.hide();
                            //                            hideOpWaittingDiv();
                        }
                    },
                    { text: document.getElementById("hfMenuShowUnNormalGird").value,
                        handler: function () {
                            cellContext_showUnNormal(grid);
                            treeMenu.hide();
                        }
                    },
                    {
                        text: document.getElementById("hfMenuFillCheckInOut").value,
                        handler: function () {

                            var selectionModel = grid.getSelectionModel();
                            selectionModel.selectRow(rowIndex);
                            var record = selectionModel.getSelected();

                            curGridStuffNo = record.get("EMPLOYEE");
                            curGridDay = varColName.substring(1, varColName.length);
                            var varYear = Ext.getCmp("cboYearMonth").getValue();
                            treeMenu.hide();
							var shiftday = 'D'+curGridDay; 
							if (varUserType == 0) {
							    if (document.getElementById("hfLanguage").value == "zh-cn") {//如果是中文环境
							        alert("当前账号无补卡权限，如需补卡请与人力资源部联系");
							    } else {
							        alert("This Staff have no rights to Fill,please contact administrtor!");
							    }
							}
							else {
							    var varFillConfirmString = "确定将第" + curGridDay + "日班次" + record.data[shiftday] + "补卡？";
							    if (document.getElementById("hfLanguage").value != "zh-cn") {//如果不是中文环境
							        varFillConfirmString = "Are you sure to Fill the record on Day " + curGridDay + " and Shift " + record.data[shiftday] + "?";
							    }
							    if (confirm(varFillConfirmString)) {
								    var varComment = prompt("请输入补卡原因：","");
								    if (document.getElementById("hfLanguage").value != "zh-cn") {//如果不是中文环境
								        varComment = prompt("Please input the reason：", "");
								    }
									if (varComment == "") {
									    if (document.getElementById("hfLanguage").value == "zh-cn") {//如果是中文环境
									        alert("补卡原因不能为空！");
									    } else {
									        alert("The Reason can not be empty!");
									    }
									}else{
										var url = "../Query/QueryMain.aspx?SP=USP_HR_KQBK&DCNO=" + curGridStuffNo + "&YearMonth=" + varYear + "&SDay=" + curGridDay+ "&SHIFT=" + record.data[shiftday]+ "&userId=" + varUserId+"&Comment="+varComment;
										//alert(url);
										window.open(url, 'newwindow', 'width=1200,height=600,top=100,left=100, center:1, toolbar=no, menubar=no, scrollbars=yes,resizable=yes,location=no, status=no');
										//alert("确定补卡:"+varUserType+" "+varYear+" "+curGridDay+" "+record.get("EMPLOYEE")+" "+record.data[shiftday]);
									}
								}
							}
                        }
                    },
                    {
                        text: document.getElementById("hfMenuAnalyzeTheStaff").value,
                        handler: function () {

                            var selectionModel = grid.getSelectionModel();
                            selectionModel.selectRow(rowIndex);
                            var record = selectionModel.getSelected();

                            curGridStuffNo = record.get("EMPLOYEE");
                            curGridDay = varColName.substring(1, varColName.length);
                            var varYear = Ext.getCmp("cboYearMonth").getValue();
                            treeMenu.hide();

                            var url = "../Query/QueryMain.aspx?SP=SP_AnlysePaiBanResultByEmpPBPAGE&SUSERID=" + curGridStuffNo + "&KEY=" + varYear + curGridStuffNo + "&SDay=" + curGridDay;
                            window.open(url, 'spwindow', 'width=1200,height=600,top=100,left=100, center:1, toolbar=no, menubar=no, scrollbars=yes,resizable=yes,location=no, status=no');
                        }
                    },
                    {
                        text: document.getElementById("hfMenuLVOTRecord").value,
                        handler: function () {

                            var selectionModel = grid.getSelectionModel();
                            selectionModel.selectRow(rowIndex);
                            var record = selectionModel.getSelected();

                            curGridStuffNo = record.get("EMPLOYEE");
                            curGridDay = varColName.substring(1, varColName.length);
                            var varYear = Ext.getCmp("cboYearMonth").getValue();
                            treeMenu.hide();

                            var url = "../Query/QueryMain.aspx?SP=USP_HR_QRY_OneStaff_LVOT&DCNO=" + curGridStuffNo + "&YearMonth=" + varYear + "&SDay=" + curGridDay;
                            window.open(url, 'newwindow', 'width=1200,height=600,top=100,left=100, center:1, toolbar=no, menubar=no, scrollbars=yes,resizable=yes,location=no, status=no');
                        }
                    }, {
                        text: document.getElementById("hfMenuCheckInOutRecord").value,
                        handler: function () {

                            var selectionModel = grid.getSelectionModel();
                            selectionModel.selectRow(rowIndex);
                            var record = selectionModel.getSelected();

                            curGridStuffNo = record.get("EMPLOYEE");
                            curGridDay = varColName.substring(1, varColName.length);
                            var varYear = Ext.getCmp("cboYearMonth").getValue();
                            treeMenu.hide();

                            var url = "../Query/QueryMain.aspx?SP=USP_HR_QRY_OneStaff_ATT&DCNO=" + curGridStuffNo + "&YearMonth=" + varYear + "&SDay=" + curGridDay;
                            window.open(url, 'newwindow', 'width=1200,height=600,top=100,left=100, center:1, toolbar=no, menubar=no, scrollbars=yes,resizable=yes,location=no, status=no');
                        }
                    },
                    {
                        text: document.getElementById("hfMenuAttendanceSummary").value,
                        handler: function () {

                            var selectionModel = grid.getSelectionModel();
                            selectionModel.selectRow(rowIndex);
                            var record = selectionModel.getSelected();

                            curGridStuffNo = record.get("EMPLOYEE");
                            curGridDay = varColName.substring(1, varColName.length);
                            var varYear = Ext.getCmp("cboYearMonth").getValue();
                            treeMenu.hide();

                            var url = "../Query/QueryMain.aspx?SP=USP_HR_QRY_KQRSSZ&USERTYPE=" + varUserType + "&YearMonth=" + varYear + "&USERCODE=" + varUserId;
                            window.open(url, 'newwindow', 'width=1200,height=600,top=100,left=100, center:1, toolbar=no, menubar=no, scrollbars=yes,resizable=yes,location=no, status=no');
                        }
                    },
                    {
                        text: document.getElementById("hfMenuAttendanceAnalysisResult").value,
                        handler: function () {

                            var selectionModel = grid.getSelectionModel();
                            selectionModel.selectRow(rowIndex);
                            var record = selectionModel.getSelected();

                            curGridStuffNo = record.get("EMPLOYEE");
                            curGridDay = varColName.substring(1, varColName.length);
                            var varYear = Ext.getCmp("cboYearMonth").getValue();
                            treeMenu.hide();

                            var url = "../Query/QueryMain.aspx?SP=USP_HR_QRY_ATTANALY&USERID=" + varUserId + "&YearMonth=" + varYear;
                            window.open(url, 'newwindow', 'width=1200,height=600,top=100,left=100, center:1, toolbar=no, menubar=no, scrollbars=yes,resizable=yes,location=no, status=no');
                        }
                    },
                    {
                        text: document.getElementById("hfMenuAttendanceReport").value,
                        handler: function () {

                            var selectionModel = grid.getSelectionModel();
                            selectionModel.selectRow(rowIndex);
                            var record = selectionModel.getSelected();

                            curGridStuffNo = record.get("EMPLOYEE");
                            curGridDay = varColName.substring(1, varColName.length);
                            var varYear = Ext.getCmp("cboYearMonth").getValue();
                            treeMenu.hide();

                            var url = "../Report/ReportMain.aspx?RPT=File/attendance_XX.rpt&P0=" + varUserId + "&P1=" + varYear+"&P21=";
                            window.open(url, 'newwindow', 'width=1200,height=600,top=100,left=100, center:1, toolbar=no, menubar=no, scrollbars=yes,resizable=yes,location=no, status=no');
                        }
                    },
                    {
                        text: document.getElementById("hfMenuAttendanceExceptionReport").value,
                        handler: function () {

                            var selectionModel = grid.getSelectionModel();
                            selectionModel.selectRow(rowIndex);
                            var record = selectionModel.getSelected();

                            curGridStuffNo = record.get("EMPLOYEE");
                            curGridDay = varColName.substring(1, varColName.length);
                            var varYear = Ext.getCmp("cboYearMonth").getValue();
                            treeMenu.hide();

                            var url = "../Report/ReportMain.aspx?RPT=File/kqecp_RM1.rpt&P0=" + varUserId + "&P1=" + varYear + "&P21=";
                            window.open(url, 'newwindow', 'width=1200,height=600,top=100,left=100, center:1, toolbar=no, menubar=no, scrollbars=yes,resizable=yes,location=no, status=no');
                        }
                    },
                    {
                        text: document.getElementById("hfMenuOvertimeBalanceReport").value,
                        handler: function () {

                            var selectionModel = grid.getSelectionModel();
                            selectionModel.selectRow(rowIndex);
                            var record = selectionModel.getSelected();

                            curGridStuffNo = record.get("EMPLOYEE");
                            curGridDay = varColName.substring(1, varColName.length);
                            var varYear = Ext.getCmp("cboYearMonth").getValue();
                            treeMenu.hide();

                            var url = "../Report/ReportMain.aspx?RPT=File/Attendance_LO_XX.rpt&P0=" + varUserId + "&P1=" + varYear + "&P21=";
                            window.open(url, 'newwindow', 'width=1200,height=600,top=100,left=100, center:1, toolbar=no, menubar=no, scrollbars=yes,resizable=yes,location=no, status=no');
                        }
                    },					
                    {
                        text: document.getElementById("hfMenuLeaveDocument").value,
                        handler: function () {

                            var selectionModel = grid.getSelectionModel();
                            selectionModel.selectRow(rowIndex);
                            var record = selectionModel.getSelected();

                            curGridStuffNo = record.get("EMPLOYEE");
                            curGridDay = varColName.substring(1, varColName.length);
                            var varYear = Ext.getCmp("cboYearMonth").getValue();
                            treeMenu.hide();

                            if (varUserType == 1) {//人事部
                                var url = "../Archive/ArchiveMain.aspx?cuBBQl+jUdG/c18gdcpUd09s9uM90egoDWSiye32q6XdW2R7FZrIfAxPeFfR/O+4";
                                if (document.getElementById("hfIsUseOA_LV").value == '1') {
                                    url = "../Archive/ArchiveMain.aspx?WMjK0mGuAtaHF1IeSvDP2EWQf8Vw651UNLEUa/0dJtqe4jAL7Un4ttGZpfGe3hPEJueXE//IpZ4=";//链接OA流程模式
                                }
                                window.open(url, 'newwindow', 'width=1200,height=600,top=100,left=100,location = yes, center:1, toolbar=no, menubar=no, scrollbars=yes,resizable=yes,location=no, status=no');
                                window.opener = null;
                            }
                            else {//小部门
                                var url = "../Archive/ArchiveMain.aspx?cuBBQl+jUdEX4/J/B3NAG7+CZKLBbjJb4mhUnvti0oJimfuaeqLLdKBskx2ZbEub";
                                if (document.getElementById("hfIsUseOA_LV").value == '1') {
                                    url = "../Archive/ArchiveMain.aspx?WMjK0mGuAtaHF1IeSvDP2N629YHvqv4yWrm+MO9rNt10V3LvqnHRlB+aMOEp14b4Yv++pkxSAs8=";//链接OA流程模式
                                }
                                window.open(url, 'newwindow', 'width=1200,height=600,top=100,left=100, location = yes,center:1, toolbar=no, menubar=no, scrollbars=yes,resizable=yes,location=no, status=no');
                                //alert("部门考勤员暂时无此权限");
                                window.opener = null;
                            }
                        }
                    },
                    {
                        text: document.getElementById("hfMenuOvertimeDocument").value,
                        handler: function () {

                            var selectionModel = grid.getSelectionModel();
                            selectionModel.selectRow(rowIndex);
                            var record = selectionModel.getSelected();

                            curGridStuffNo = record.get("EMPLOYEE");
                            curGridDay = varColName.substring(1, varColName.length);
                            var varYear = Ext.getCmp("cboYearMonth").getValue();
                            treeMenu.hide();

                            if (varUserType == 1) {
                                var url = "../Archive/ArchiveMain.aspx?wZrD9CCxd/s0STBIrQhoavOp/7niPEvxqUSby+0O58x7J47EYpDa6j+0Re3mRxRs";
                                if (document.getElementById("hfIsUseOA_OT").value == '1') {
                                    url = "../Archive/ArchiveMain.aspx?JAQbuZd0n9zaICRCF7rlb04xbRBNwBk+N8BAdd9HC9EIxNyYDu874CqhupLMCj6g2DcNY1JpLzE=";//链接OA流程模式
                                }
                                window.open(url, 'newwindow', 'width=1200,height=600,top=100,left=100,location = yes, center:1, toolbar=no, menubar=no, scrollbars=yes,resizable=yes,location=no, status=no');
                                window.opener = null;
                            }
                            else {
                                var url = "../Archive/ArchiveMain.aspx?wZrD9CCxd/vxa9yoc8GXYIeMEIzg7QFBQF5r4uT9xibmVSGziBIT/1y7AaQw4d56";
                                if (document.getElementById("hfIsUseOA_OT").value == '1') {
                                    url = "../Archive/ArchiveMain.aspx?JAQbuZd0n9zaICRCF7rlb/odeOgOhhWWyRdUnbV7G1yuBCEcUUncxf785qITa7TBfemZVw/rRYU=";//链接OA流程模式
                                }
                                window.open(url, 'newwindow', 'width=1200,height=600,top=100,left=100, location = yes,center:1, toolbar=no, menubar=no, scrollbars=yes,resizable=yes,location=no, status=no');
                                //alert("部门考勤员暂时无此权限");
                                window.opener = null;
                            }
                        }
                    }

                ]);
                treeMenu.showAt(e.getPoint());
            });


            this.GridView = gridPanel_Paiban;

            //默认显示异常结果
            setTimeout(function () {
                var grid = Ext.getCmp("editGridPanel");
                showUnNormalGrid(grid);
                //关闭后执行  
                hideOpWaittingDiv();
            }, 1000);

            //增加表格双击事件处理
            gridPanel_Paiban.addListener('celldblclick', dbClickPaiBanCell);
            gridPanel_Paiban.addListener('cellclick', clickPaiBanCell);
        }




        //双击排班信息某一个表格事件,同时将全局班次变量赋值到该网格中
        var dbClickPaiBanCell = function (grid, rowIndex, columnIndex, e) {
            //增加逻辑：分析状态为完成或者是小部门操作且非小部门分析状态
            if (curYearMonthIsLock == 1) {
                Ext.Msg.alert(document.getElementById("hfTipTip").value, Ext.getCmp("cboYearMonth").getValue() + "：" + document.getElementById("hfTipPeriodLocked").value);
                return;
            }
            if (curSectionAnalysStatus == 3) {
                Ext.Msg.alert(document.getElementById("hfTipTip").value, Ext.getCmp("cboYearMonth").getValue() + "：" + document.getElementById("hfTipAnalysStatus3").value);
                return;
            }
            if ((varUserType != 1) && (curSectionAnalysStatus != 1)) {
                Ext.Msg.alert(document.getElementById("hfTipTip").value, Ext.getCmp("cboYearMonth").getValue() + "：" + document.getElementById("hfTipAnalysStatus2").value);
                return;
            }
            if (varSetPaiBanValue == "") {
                //delete BY WSM 20190614班次可以手工输入，则删除下面一行
                Ext.Msg.alert(document.getElementById("hfTipTip").value, document.getElementById("hfTipCanCopy").value);
                return;
            }
            var varColName;
            if (grid.colModel.getCellEditor(columnIndex, rowIndex) != null) {//编辑网格
                varColName = grid.getColumnModel().getDataIndex(columnIndex);
                var selectionModel = grid.getSelectionModel();
                var record = selectionModel.getSelected();
                //设置为剪贴板中数据
                record.set(varColName, varSetPaiBanValue);

                //add  by sammen 20120618 实时保存
                if (document.getElementById("hfIsRealTimeSave").value == "1") {
                    SavePaibanRealTime();
                }
            }
        }


        //单机排班信息某一个表格事件
        var clickPaiBanCell = function (grid, rowIndex, columnIndex, e, clickType) {
            curGridYearMonth = Ext.getCmp("cboYearMonth").getValue();
            varColName = grid.getColumnModel().getDataIndex(columnIndex);
            //单击时就显示考勤结果Michael 160316
//            showOpWaittingDiv();
            viewStuffResultGrid(grid, rowIndex, columnIndex, e);
//            hideOpWaittingDiv();

            //
            if (isFixDayCol(varColName)) {
                //修改该cell背景，没有点击过时着色，点击过再点击则去掉css,有无css交叉显示
                var varCell = grid.getView().getCell(rowIndex, columnIndex);
                var ExtCell = Ext.get(varCell);


                var strKey = rowIndex.toString() + "*" + columnIndex.toString();
                if (hashCellPoint != null) {//点击过至少一个cell
                    if (hashCellPoint.contains(strKey)) {//点击过此cell
                        if (hashCellPoint.items(strKey) == "true") {//当存在css时，则去掉css
                            if (clickType != "2") {//如果不是右键点击则去掉css

                                //ExtCell.removeClass("focusCellCss");
                                hashCellPoint.remove(strKey);
                                hashCellPoint.add(strKey, "false");
                            }
                        } else if (hashCellPoint.items(strKey) == "false") {//当不存在css时，则增加css
                            //ExtCell.addClass("focusCellCss");
                            hashCellPoint.remove(strKey);
                            hashCellPoint.add(strKey, "true");
                        }
                    } else {//尚未点击过此cell
                        //ExtCell.addClass("focusCellCss");
                        hashCellPoint.remove(strKey);
                        hashCellPoint.add(strKey, "true"); //点击后设为true
                    }
                } else {//尚未点击过任何cell
                    //ExtCell.addClass("focusCellCss");
                    var tempHashCellPoint = new Hashtable();
                    tempHashCellPoint.add(strKey, "true"); //点击后设为true
                    hashCellPoint = tempHashCellPoint;
                }
            }

        }


        //查看该员工该日期的考勤记录
        var viewStuffResultGrid = function (grid, rowIndex, columnIndex, e) {
            curGridYearMonth = Ext.getCmp("cboYearMonth").getValue();
            varColName = grid.getColumnModel().getDataIndex(columnIndex);
            if (isFixDayCol(varColName)) {
                var colDayIsLockShowName = grid.getColumnModel().getColumnTooltip(columnIndex);
                var colDayIsLock = ((colDayIsLockShowName.indexOf("已锁定")>-1)||(colDayIsLockShowName.indexOf("Locked")>-1)) ? "1" : "2";
                var selectionModel = grid.getSelectionModel();
                selectionModel.selectRow(rowIndex);
                var record = selectionModel.getSelected();


                curGridStuffNo = record.get("EMPLOYEE");
                curGridDay = varColName.substring(1, varColName.length);

                //加载员工考勤记录
                loadKQStruffResultInfo(colDayIsLock);
            }
        }


        //右键还原编辑前数据的操作
        var cellContext_restore = function (grid, rowIndex, columnIndex, e) {
            var varColName = grid.getColumnModel().getDataIndex(columnIndex);


            //获取编辑前的数据
            var record_old = grid.getStore().getAt(rowIndex);
            var value_old = record_old.get(varColName);
            alert(value_old);


            var selectionModel = grid.getSelectionModel();
            selectionModel.selectRow(rowIndex);
            var record = selectionModel.getSelected();
            //            alert(record.get(varColName));
            //设置为剪贴板中数据
            record.set(varColName, value_old);
            //            selectionModel.clearSelections();
        }


        //右键“显示是否正常网格”
        var cellContext_showUnNormal = function (grid) {
            showUnNormalGrid(grid);
        }


        //“显示是否正常网格”
        function showUnNormalGrid(grid) {
            if (grid != null) {
                var rowCount_Paiban = grid.getStore().getCount(); //行数
                curGridYearMonth = Ext.getCmp("cboYearMonth").getValue();
                if (showunoramlornot == "Y") {
                    this.showunoramlornot = "N";
                } else {
                    this.showunoramlornot = "Y";
                }
                Ext.Ajax.request({
                    url: "Ajax/KQPaibanAjax.aspx?param=KQUnNormalResultInfo&yearMonth=" + curGridYearMonth + "&userId=" + varUserId,
                    method: "POST",
                    success: function (p1, p2) {
                        var de1 = Ext.decode(p1.responseText);
                        var stuffNo;
                        var stuffNo1;
                        var stuffDay;
                        var stuffDay1;
                        var day1;
                        var colIndex;
                        var unNormalState = 0;


                        //遍历当前网格的列数               
                        for (var j = 0; j < rowCount_Paiban; j++) {
                            var record = grid.getStore().getAt(j);
                            stuffNo = record.data['EMPLOYEE'];
                            for (var c = 0; c < de1.length; c++) {
                                for (var q in de1[c]) {
                                    stuffDay1 = q;
                                    var iIndex = stuffDay1.indexOf('*');
                                    stuffNo1 = stuffDay1.substring(0, iIndex);
                                    day1 = stuffDay1.substring(iIndex + 1, stuffDay1.length);
                                    ////modify by sammen 20191010新增部门名称列的显示【下面的数字如果列发生变化，则数字也需要随之变化】
                                    colIndex = parseInt(day1) + 7; //通过字段名获取列序号（字段名必须是D1/D2/.....D11....）5表示出了日期字段外的其他字段的个数
                                    //用不同的颜色显示考勤异常Michael 160316
                                    var unNormalStateStr;
                                    var unNormalStatetype;
                                    unNormalStateStr = de1[c][q];
                                    var uIndex = unNormalStateStr.indexOf('@');
                                    unNormalState = unNormalStateStr.substring(0, 1);
                                    unNormalStatetype = unNormalStateStr.substring(uIndex + 1, unNormalStateStr.length);
                                    //unNormalState = de1[c][q];
                                    //
                                    if (stuffNo1 == stuffNo) {
                                        if (unNormalState == '2') {//非正常数据

                                            var varCell = grid.getView().getCell(j, colIndex);
                                            var ExtCell = Ext.get(varCell);
                                            //ExtCell.attributes.add("onmousein",MessageBox.alert("OK"));
                                            ExtCell.removeClass("unToNormalCellCss");
                                            //用不同的颜色显示考勤异常Michael 160316											
                                            if (unNormalStatetype == '*') {
                                                ExtCell.addClass("unNormalxCellCss")
                                            } else if (unNormalStatetype == 'LE') {
                                                ExtCell.addClass("unNormalleCellCss")
                                            } else if (unNormalStatetype == 'LELE') {
                                                ExtCell.addClass("unNormalleCellCss")
                                            } else if (unNormalStatetype == '!AW') {
                                                ExtCell.addClass("unNormalawCellCss")
                                            } else if (unNormalStatetype == '!?') {
                                                ExtCell.addClass("unNormalnsCellCss");
                                            } else if (unNormalStatetype == '*LE') {
                                                ExtCell.addClass("unNormalxleCellCss")
                                            } else {
                                                ExtCell.addClass("unNormalCellCss")
                                            }
                                            //
                                        } else if (unNormalState == 1) {//调整为正常数据

                                            var varCell = grid.getView().getCell(j, colIndex);
                                            var ExtCell = Ext.get(varCell);
                                            ExtCell.removeClass("unNormalCellCss");
                                            //显示考勤异常单击时显示异常和显示调整后交替Michael 160316
                                            if (showunoramlornot == "Y") {
                                                ExtCell.addClass("unToNormalCellCss");
                                            } else {
                                                ExtCell.removeClass("unToNormalCellCss");

                                                if (unNormalStatetype == '*') {
                                                    ExtCell.addClass("unNormalxCellCss")
                                                } else if (unNormalStatetype == 'LE') {
                                                    ExtCell.addClass("unNormalleCellCss")
                                                } else if (unNormalStatetype == 'LELE') {
                                                    ExtCell.addClass("unNormalleCellCss")
                                                } else if (unNormalStatetype == '!AW') {
                                                    ExtCell.addClass("unNormalawCellCss")
                                                } else if (unNormalStatetype == '!?') {
                                                    ExtCell.addClass("unNormalnsCellCss")
                                                } else if (unNormalStatetype == '*LE') {
                                                    ExtCell.addClass("unNormalxleCellCss")
                                                } else {
                                                    ExtCell.addClass("unNormalCellCss")
                                                }
                                            }
                                            //
                                        }
                                    }
                                }
                            }
                        }
                        Ext.getCmp("formOne").render("div1");
                    }

                })
            }


        }


        //复制某行排班信息
        var cellContext_copyPaibanRow = function (grid, rowIndex) {
            var tempHashPaibanRow = new Hashtable();
            var record = grid.getStore().getAt(rowIndex);
            var varColName;
            var colValue;
            for (var i = 0; i < colCount_Paiban; i++) {
                varColName = grid.getColumnModel().getDataIndex(i);
                colValue = record.get(varColName);
                if (isFixDayCol(varColName)) {
                    tempHashPaibanRow.add(varColName, colValue);
                }
            }
            hashPaibanRow = tempHashPaibanRow;
        }


        //粘贴某行排班信息
        var cellContext_pastePaibanRow = function (grid, rowIndex) {
            if ((hashPaibanRow != null) && (hashPaibanRow.count() > 0)) {
                var varColName;
                var colValue;
                for (var i = 0; i < colCount_Paiban; i++) {
                    var colModel = grid.getColumnModel();
                    varColName = colModel.getDataIndex(i);
                    colValue = hashPaibanRow.items(varColName);
                    if (isFixDayCol(varColName)) {
                        var selectionModel = grid.getSelectionModel();
                        selectionModel.selectRow(rowIndex);
                        var record = selectionModel.getSelected();
                        record.set(varColName, colValue);
                    }
                }
                //                    Ext.getCmp("btnSave").resumeEvents();
                //add  by sammen 20120618 实时保存
                if (document.getElementById("hfIsRealTimeSave").value == "1") {
                    SavePaibanRealTime();
                }
            }


        }


        //批量调整异常考勤信息
        var cellContext_batchAdjust = function (grid) {
            openResultWindow();
        }
        //判断非正常考勤数据的状态位
        function getNormalState(stuffDay, de1) {
            var unNormalState = 0;
            for (var c = 0; c < de1.length; c++) {
                for (var q in de1[c]) {
                    if (stuffDay == q) {

                        unNormalState = de1[c][q];
                    }
                }
            }
            return unNormalState;
        }
        //
    </script>
     
    <%--为员工考勤结果中的combox数据的准备--Grid--%>
    <script type="text/javascript">
        Ext.data.status = [
        //            ['0','正常'],//正常
            ['1', document.getElementById("hfListNormal").value], //调整为正常
            ['2', document.getElementById("hfListUnNormal").value] //异常
        ];
        var statusDS = new Ext.data.SimpleStore({ //通过字典表获得用户使用状态数据源
            fields: ['code', 'value'],
            data: Ext.data.status //这里对应我在字典表里定义的类型名称
        });
                
    </script>
    
    <%--员工考勤结果信息数据--Grid--%>
    <script type="text/javascript">
        function MakeDataColumn_Result(colDayIsLock) {//加载员工考勤信息数据列
            this.fields = '';
            this.columns = '';
            this.addColumns = function (name, caption) {
                //alert(name+'jia'+caption);
                if (this.fields.length > 0) {
                    this.fields += ',';
                }
                if (this.columns.length > 0) {
                    this.columns += ',';
                }
                this.fields += '{name:"' + name + '"}';
                if ((name == 'EM_NO') || (name == 'Ymonth') || (name == 'OverTime') || (name == 'LVhrs') || (name == 'r_Date')) {
                    this.columns += '{header:"' + caption + '",dataIndex:"' + name + '",width:8,sortable:true,hidden:false}';
                } else if ((name == 'FactIn') || (name == 'FactOut') || (name == 'FactIn1') || (name == 'FactOut1')) {
                    this.columns += '{header:"' + caption + '",dataIndex:"' + name + '",width:15,sortable:true,hidden:false}';
                } else if ((name == 'DCNAMEC')) {
                    if (document.getElementById("hfLanguage").value == "zh-cn") {//如果是中文环境则显示中文名
                        this.columns += '{header:"' + caption + '",dataIndex:"' + name + '",width:10,sortable:true,css:"background: #9F6;"}';
                    } else {

                        this.columns += '{header:"' + caption + '",dataIndex:"' + name + '",width:10,sortable:true,css:"background: #9F6;",hidden:true}';
                    }
                } else if ((name == 'DCNAMEE')) {
                    if (document.getElementById("hfLanguage").value == "zh-cn") {//如果是英文环境则显示英文名
                        this.columns += '{header:"' + caption + '",dataIndex:"' + name + '",width:10,sortable:true,css:"background: #9F6;",hidden:true}';
                    } else {

                        this.columns += '{header:"' + caption + '",dataIndex:"' + name + '",width:10,sortable:true,css:"background: #9F6;"}';
                    }
                } else if (name == 'STXHOUR') {
                    if ((IsCanEditTx == 0) || (curYearMonthIsLock == 1) || (curSectionAnalysStatus == 3) || ((varUserType != 1) && (curSectionAnalysStatus != 1)) || (colDayIsLock=='1')) {
                        this.columns += '{header:"' + caption + '",dataIndex:"' + name + '",width:8,sortable:true,hidden:true,css:"background: #FFCCFF;"}';
                    } else {

                        this.columns += '{header:"' + caption + '",dataIndex:"' + name + '",width:8,sortable:true,hidden:true,css:"background: #FFCCFF;",editor:new Ext.form.TextField({maxLength:4})}';
                    }
                } else if (name == 'SJBHOUR') {
                    if ((IsCanEditJb == 0) || (curYearMonthIsLock == 1) || (curSectionAnalysStatus == 3) || ((varUserType != 1) && (curSectionAnalysStatus != 1)) || (colDayIsLock == '1')) {
                        this.columns += '{header:"' + caption + '",dataIndex:"' + name + '",width:8,sortable:true,hidden:true,css:"background: #FFCCFF;"}';
                    } else {

                        this.columns += '{header:"' + caption + '",dataIndex:"' + name + '",width:8,sortable:true,hidden:true,css:"background: #FFCCFF;",editor:new Ext.form.TextField({maxLength:4})}';
                    }
                } else if (name == 'SREMARK') {
                    if ((curYearMonthIsLock == 1) || (curSectionAnalysStatus == 3) || ((varUserType != 1) && (curSectionAnalysStatus != 1)) || (colDayIsLock == '1')) {
                        this.columns += '{header:"' + caption + '",dataIndex:"' + name + '",width:30,sortable:true,hidden:false,css:"background: #FFCCFF;"}';
                    } else {

                        this.columns += '{header:"' + caption + '",dataIndex:"' + name + '",width:10,sortable:true,hidden:false,css:"background: #FFCCFF;",editor:new Ext.form.TextArea({maxLength:300})}';
                    }
                } else if (name == 'BISTONORMAL') {
                    ///增加开关IsCanEditIsToNormal的逻辑add by sammen 20180105
                    ///未锁定或者非审核状态下，人事部审核能编辑，部门人员只有在开关IsCanEditIsToNormal=1时才能编辑 
                    if (((varUserType != 1) && (IsCanEditIsToNormal != 1)) || (curYearMonthIsLock == 1) || (curSectionAnalysStatus == 3) || ((varUserType != 1) && (curSectionAnalysStatus != 1)) || (colDayIsLock == '1')) {
                        this.columns += '{header:"' + caption + '",dataIndex:"' + name + '",width:10,sortable:true,hidden:false,css:"background: #FFCCFF;"}';
                    } else {

                        this.columns += '{header:"' + caption + '",dataIndex:"' + name + '",width:8,sortable:true,hidden:false,css:"background: #FFCCFF;",'
                                + 'editor:new Ext.form.ComboBox({store: statusDS,valueField :"code",displayField: "value",hiddenName:"value",mode: "local",editable:false,triggerAction:"all"}) }';
                    }
                } else if ((name == 'ExceptionChs')) {
                    this.columns += '{header:"' + caption + '",dataIndex:"' + name + '",width:12,sortable:true,hidden:false}';
                }
                else {

                    this.columns += '{header:"' + caption + '",dataIndex:"' + name + '",width:8,sortable:true,hidden:true,css:"background: #FFCCFF;"}';
                }


            }
        }


        function MakeGridView_Result() {//加载员工考勤结果信息数据
            var cm = new Ext.grid.ColumnModel(eval('([' + datacol_Result.columns + '])'));
            cm.defaultSortable = true;
            var fields = eval('([' + datacol_Result.fields + '])');
            StoreGrid_Result = new Ext.data.Store
            ({
                proxy: new Ext.data.HttpProxy({ url: "Ajax/KQPaibanAjax.aspx?param=KQStuffResultDataInfo&stuffNo=" + curGridStuffNo + "&yearMonth=" + curGridYearMonth + "&day=" + curGridDay + "&yearMonthStartDay=" + curMonthStartDay + "&userId=" + varUserId }),
                reader: new Ext.data.JsonReader({ totalProperty: "KQStuffResultTotalPorperty", root: "KQStuffResult", fields: fields }),
                storeId: "SHCODE"
            });
            StoreGrid_Result.removeAll();
            StoreGrid_Result.load({});

            var pagingBar = new Ext.PagingToolbar
            ({
                emptyMsg: "没有数据显示",
                displayMsg: "显示从{0}条数据到{1}条数据，共{2}条数据",
                store: StoreGrid_Result
                //pageSize: 9
            });


            var gridPanel_Result = new Ext.grid.EditorGridPanel
            ({
                cm: cm,
                id: "StuffResultGridPanel",
                columnWidth: .9,
                store: StoreGrid_Result,
                frame: false,
                border: true,
                region: 'top',
                pageSize: 16,
                height: 60,
                stripeRows: true,
                clicksToEdit: 1,
                viewConfig: { forceFit: true },
                columnLines: true,
                sm: new Ext.grid.RowSelectionModel({ singleSelect: false }),
                viewConfig: {

                    columnsText: document.getElementById("hfTipGridColumnsText").value,
                    sortAscText: document.getElementById("hfTipGridSortAscText").value,
                    sortDescText: document.getElementById("hfTipGridSortDescText").value,
                    forceFit: true
                }

            });
            this.GridView = gridPanel_Result;
            gridPanel_Result.on('afteredit', afterEditResult, this); //add  by sammen 20120618 即时保存
        }
        function afterEditResult(e) {//add  by sammen 20120618 即时保存
            SaveReusltRealTime(e);
        };


        //加载员工考勤结果信息
        function loadKQStruffResultInfo(colDayIsLock) {
            Ext.Ajax.request({
                url: "Ajax/KQPaibanAjax.aspx?param=KQStuffResultColumnInfo" + "&userId=" + varUserId,
                method: "POST",
                success: function (p1, p2) {
                    var de1 = Ext.decode(p1.responseText);
                    datacol_Result = new MakeDataColumn_Result(colDayIsLock);
                    colCount_Result = de1.length;
                    for (var j = 0; j < de1.length; j++) {
                        for (var q in de1[j]) {
                            datacol_Result.addColumns(q, de1[j][q]);
                        }
                    }
                    if (Ext.getCmp("StuffResultGridPanel")) { Ext.getCmp("formOne").remove(Ext.getCmp("StuffResultGridPanel")); }
                    if (Ext.getCmp("btnSaveResult")) { Ext.getCmp("formOne").remove(Ext.getCmp("btnSaveResult")); }
                    var grid_Result = new MakeGridView_Result().GridView;


                    //delete  by sammen 20120618 即时保存时，批量保存暂时屏蔽
                    //                    var btn_SaveResult = new MakeBtn_saveResult(Ext.getCmp("StuffResultGridPanel")).btnSaveResult;
                    //                    Ext.getCmp("formOne").add(btn_SaveResult);

                    Ext.getCmp("formOne").add(grid_Result);
                    Ext.getCmp("formOne").doLayout();

                }
            })



        }


        //保存员工考勤结果调整信息
        function MakeBtn_saveResult(grid) {
            var btnSaveResult = new Ext.Button({
                id: "btnSaveResult",
                height: 25,
                columnWidth: .9,
                style: "background:#9F6",
                text: document.getElementById("hfBtnAdjustResult").value,
                handler: function () {
                    showOpWaittingDiv();
                    if (StoreGrid_Result == null) {
                        hideOpWaittingDiv();
                        return;
                    }
                    if (Ext.getCmp("StuffResultGridPanel").activeEditor != null) {
                        Ext.getCmp("StuffResultGridPanel").activeEditor.completeEdit();
                    }
                    var recordArray = StoreGrid_Result.getModifiedRecords();
                    if (recordArray.length > 0) {
                        //                        var selectionModel = grid.getSelectionModel();
                        //                        selectionModel.selectRow(0); 
                        //                        var record = selectionModel.getSelected();
                        //                        



                        var items = recordArray[0].getChanges();
                        //                        var strJbHour = record.get("SJBHOUR");
                        //                        var strTxHour = record.get("STXHOUR");
                        //                        var strRemark = record.get("SREMARK");
                        //                        var strIsNormal = record.get("BISTONORMAL");
                        var resultStr1 = '';
                        for (var key in items) {
                            if (items[key] == "") {

                                resultStr1 += "^" + key + '*' + 'space';
                            } else {

                                if ((key == "SJBHOUR") && (!isPositiveNumber(items[key]))) {
                                    Ext.Msg.alert(document.getElementById("hfTipMsg").value, document.getElementById("hfLVOVMustInt").value);
                                    hideOpWaittingDiv();
                                    return;
                                }
                                if ((key == "STXHOUR") && (!isPositiveNumber(items[key]))) {
                                    Ext.Msg.alert(document.getElementById("hfTipMsg").value, document.getElementById("hfLVOVMustInt").value);
                                    hideOpWaittingDiv();
                                    return;
                                }
                                resultStr1 += "^" + key + '*' + items[key];
                            }
                        }
                        resultStr1 = resultStr1.substring(1, resultStr1.length);
                        resultStr1 = encodeURI(resultStr1);
                        //　　                    alert(resultStr1);
                        curGridYearMonth = Ext.getCmp("cboYearMonth").getValue();
                        if ((curGridStuffNo != "") && (curGridYearMonth != "") && (curGridDay != "")) {
                            Ext.Ajax.request({
                                url: "Ajax/KQPaibanAjax.aspx?param=saveKQStuffResultData&yearMonth=" + curGridYearMonth + "&stuffNo=" + curGridStuffNo + "&day=" + curGridDay + "&resultModifyStr=" + resultStr1 + "&yearMonthStartDay=" + curMonthStartDay + "&userId=" + varUserId,
                                method: "POST",
                                success: function (p1, p2) {
                                    Ext.Msg.alert(document.getElementById("hfTipMsg").value, document.getElementById("hfTipSaveMsg").value);
                                    //重新加载
                                    if (Ext.getCmp("StuffResultGridPanel")) { Ext.getCmp("formOne").remove(Ext.getCmp("StuffResultGridPanel")); }
                                    var grid = new MakeGridView_Result().GridView;
                                    Ext.getCmp("formOne").add(btnSaveResult);
                                    Ext.getCmp("formOne").add(grid);
                                    Ext.getCmp("formOne").doLayout();
                                },
                                failure: function (p1, p2) { }
                            })
                        } else {

                            Ext.Msg.alert(document.getElementById("hfTipMsg").value, document.getElementById("hfTipNoSave").value);
                        }
                    } else {

                        Ext.Msg.alert(document.getElementById("hfTipMsg").value, document.getElementById("hfTipNoSave").value);
                    }
                    hideOpWaittingDiv();
                }
            });

            this.btnSaveResult = btnSaveResult;
        }

        //让员工结果表格结束编辑状态
        var stopResultGirdEditing = function (grid) {
            alert(grid);
            grid.stopEditing();
            //            var tempHashPaibanRow = new Hashtable();
            //            var record = StoreGrid_Result.getAt(0);
            //            var varColName;
            //            var colValue;
            //            for(var i=0;i<colCount_Result;i++){
            //                
            //            }


        }
        
    </script>
    
    <%--ExtJS页面加载及渲染--Grid--%>
    <script type="text/javascript">

        function ready() {
            //var tabs = new Ext.TabPanel({ id: "tabs", activeTab: 0 });
            var formOne = new Ext.Panel({
                id: "formOne",
                renderTo: "div1",
                layout: "form",
                region: 'center',
                title: document.getElementById("hfTipTitle").value,
                autoScroll: true,
		        bodyStyle: 'padding:0px 0px 255px 0px',//增加底端的空白区域，主要是因为右键菜单显示时可能会超过可视区域。
                loader: loadKQShifInfo(),
                tbar: [new MakeComboBox().ComboBox,
                       new Ext.Button
                            ({ id: "btnTemp1",
                                width: "10",
                                text: "      "
                            }),
                            // add by sammen 20191010 增加班次的过滤
                       new Ext.form.TextField({
                           id: "txtShiftCodeFilter",
                           emptyText: document.getElementById("hfTxtShiftCodeFilter").value,
                           width: "100",
                           enableKeyEvents: true,
                           listeners: {
                               keyup: function (e) {
                                   var filterSql = " (";
                                   filterSql = filterSql + " (SHCODE like '¥" + Ext.getCmp("txtShiftCodeFilter").getValue() + "¥' )";
                                   filterSql = filterSql + " or (SHNAME like '¥" + Ext.getCmp("txtShiftCodeFilter").getValue() + "¥' )";
                                   filterSql = filterSql + " or (ISNULL(IN1,'') like '¥" + Ext.getCmp("txtShiftCodeFilter").getValue() + "¥' )";
                                   filterSql = filterSql + " or (ISNULL(OUT1,'') like '¥" + Ext.getCmp("txtShiftCodeFilter").getValue() + "¥' )";
                                   filterSql = filterSql + " or (ISNULL(IN2,'') like '¥" + Ext.getCmp("txtShiftCodeFilter").getValue() + "¥' )";
                                   filterSql = filterSql + " or (ISNULL(OUT2,'') like '¥" + Ext.getCmp("txtShiftCodeFilter").getValue() + "¥' )";
                                   filterSql = filterSql + " )";
                                   //刷新班次表数据
                                   RefreshGridView_Shif(filterSql);

                               }
                           }
                       }),
                       new Ext.Button
                            ({ id: "btnLoad",
                                text: document.getElementById("hfBtnLoadInfo").value,
                                //                            width:"100",
                                //                            iconCls: "btnImage1",
                                //                            style:"background:#8632DC",
                                handler: function () {
                                    LoadStaffPaibanList();
                                }
                            }),
                        new Ext.Button
                            ({ id: "btnTemp2",
                                width: "10",
                                text: ">>>"
                            }),
                        new Ext.Button
                            ({ id: "btnSave",
                                //                            width:"100",
                                //                            style:"background:#9B5FD9",
                                text: document.getElementById("hfBtnSave").value,
                                disabled: false,
                                handler: function () {
                                    //                                Ext.getCmp("formOne").setDisabled(true);
                                    if (Ext.getCmp("editGridPanel") == null) {
                                        return;
                                    }
                                    showOpWaittingDiv();
                                    if ((Ext.getCmp("editGridPanel") != null) && (Ext.getCmp("editGridPanel").activeEditor != null)) {
                                        Ext.getCmp("editGridPanel").activeEditor.completeEdit();
                                    }
                                    var curYearMonth = Ext.getCmp("cboYearMonth").getValue();
                                    if (curYearMonth != "") {
                                        if (StoreGrid == null) {
                                            return;
                                        }
                                        var results = {};
                                        var recordArray = StoreGrid.getModifiedRecords();
                                        var recordOld;
                                        var resultStr = '';
                                        for (var i = 0; i < recordArray.length; i++) {
                                            var strEmNo = recordArray[i].get("EMPLOYEE");
                                            var strYearMonth = recordArray[i].get("YEARMONTH");
                                            var resultStr1 = '';
                                            items = recordArray[i].getChanges();
                                            for (var key in items) {
                                                if (items[key] == "") {
                                                    resultStr1 += "^" + key + '*' + 'space';
                                                } else {
                                                    resultStr1 += "^" + key + '*' + items[key];
                                                }
                                            }
                                            resultStr1 = resultStr1.substring(1, resultStr1.length);
                                            resultStr += "!" + strEmNo + '#' + strYearMonth + "$" + resultStr1;
                                            //****拼写字符串的含义*****
                                            //!   ：记录与记录之间间隔符
                                            //#   ：员工编号与排班月份间隔符
                                            //$   ：一条记录key与其修改所有对象的间隔符
                                            //^   ：同一条记录的多个修改对象之间的间隔符
                                            //*   ：同一条记录的同一个修改对象中，修改字段及修改后值之间的间隔符
                                            //****拼写字符串的含义*****
                                        };
                                        resultStr = resultStr.substring(1, resultStr.length);
                                        //alert(resultStr);
                                        if (resultStr != "") {
                                            var curYearMonth = Ext.getCmp("cboYearMonth").getValue();
                                            Ext.Ajax.request({
                                                url: "Ajax/KQPaibanAjax.aspx?param=saveKQPaiBanData&resultStr=" + resultStr + "&yearMonth=" + curYearMonth + "&userId=" + varUserId,
                                                async: false,   //ASYNC 是否异步( TRUE 异步 , FALSE 同步)
                                                params: { results: results },
                                                method: "POST",
                                                success: function (p1, p2) {

                                                    //                                            var de = Ext.decode(p1.responseText);
                                                    Ext.Msg.alert(document.getElementById("hfTipMsg").value, document.getElementById("hfTipSaveMsg").value);
                                                    //重新加载
                                                    if (Ext.getCmp("editGridPanel")) { Ext.getCmp("formOne").remove(Ext.getCmp("editGridPanel")); }
                                                    var grid = new MakeGridView().GridView;
                                                    Ext.getCmp("formOne").add(grid);
                                                    //删除员工考勤结果GRID
                                                    if (Ext.getCmp("StuffResultGridPanel")) { Ext.getCmp("formOne").remove(Ext.getCmp("StuffResultGridPanel")); }
                                                    if (Ext.getCmp("btnSaveResult")) { Ext.getCmp("formOne").remove(Ext.getCmp("btnSaveResult")); }

                                                    Ext.getCmp("formOne").doLayout();
                                                },
                                                failure: function (p1, p2) { }

                                            })
                                        } else {
                                            Ext.Msg.alert(document.getElementById("hfTipMsg").value, document.getElementById("hfTipNoSave").value);
                                        }
                                    } else {
                                        Ext.Msg.alert(document.getElementById("hfTipTip").value, document.getElementById("hfTipSelectMonth").value);
                                    }
                                    //                               Ext.getCmp("formOne").setDisabled(false);
                                    hideOpWaittingDiv();
                                }
                            }),
                        new Ext.Button
                            ({ id: "btnTemp3",
                                width: "10",
                                text: ">>>"
                            }),

                        new Ext.Button
                            ({ id: "btnExcuteSp_Analyse",
                                text: document.getElementById("hfBtnExcuteSp_Analyse").value,
                                //                            width:"100",
                                //                            style:"background:#AC82D8",
                                handler: function () {
                                    //增加逻辑：分析状态为完成或者是小部门操作且非小部门分析状态
                                    if (curYearMonthIsLock == 1) {
                                        Ext.Msg.alert(document.getElementById("hfTipTip").value, Ext.getCmp("cboYearMonth").getValue() + "：" + document.getElementById("hfTipPeriodLocked").value);
                                        return;
                                    }
                                    if (curSectionAnalysStatus == 3) {
                                        Ext.Msg.alert(document.getElementById("hfTipTip").value, Ext.getCmp("cboYearMonth").getValue() + "：" + document.getElementById("hfTipAnalysStatus3").value);
                                        return;
                                    }
                                    if ((varUserType != 1) && (curSectionAnalysStatus != 1)) {
                                        Ext.Msg.alert(document.getElementById("hfTipTip").value, Ext.getCmp("cboYearMonth").getValue() + "：" + document.getElementById("hfTipAnalysStatus2").value);
                                        return;
                                    }
                                    Ext.MessageBox.confirm(document.getElementById("hfBtnExcuteSp_Analyse").value, document.getElementById("hfTipTobeAnalyse").value, function (btn) {
                                        if (btn == 'yes') {
                                            if (document.getElementById("hfIsKQAnalyseByStaff").value != '1') {
                                                //批量执行考勤分析
                                                ExecAnalyseBatch();
                                            } else {
                                                //按员工依次执行考勤分析
                                                ExecAnalyseByStaff();
                                            }
                                        }
                                    });
                                }
                            }),

                        new Ext.Button
                            ({ id: "btnTemp4",
                                width: "10",
                                text: ">>>"
                            }),

                        new Ext.Button
                            ({ id: "btnShowUnnormal",
                                //                            width:"10",
                                text: document.getElementById("hfMenuShowUnNormalGird").value,
                                //                            style:"background:#BEA2DA",
                                modal: true,
                                handler: function () {
                                    showOpWaittingDiv();
                                    var curYearMonth = Ext.getCmp("cboYearMonth").getValue();
                                    if (curYearMonth != "") {
                                        var grid = Ext.getCmp("editGridPanel");
                                        showUnNormalGrid(grid);
                                        hideOpWaittingDiv();
                                    } else {
                                        Ext.Msg.alert(document.getElementById("hfTipTip").value, document.getElementById("hfTipSelectMonth").value);
                                        hideOpWaittingDiv();
                                    }
                                    var s = "";
                                    s += '<table class="" style="width:240px;" cellspacing="0" cellpadding="0">';
                                    s += '<tr>';
                                    if (document.getElementById("hfLanguage").value == "zh-cn") {//如果是中文环境{
                                        s += '	<td class=""    width="100%">可再次点击查看调整前/后的考勤状态</td>';
                                    } else {
                                        s += '	<td class=""    width="100%">Swicth before or after adjustment</td>';
                                    }
                                    s += '</tr>';
                                    s += '</table>';

                                    ShowMsg.showtip('', s, '250px', 1, Ext.get('tog'), [-90, 60], 't-t', false, false);

                                }
                            }),

                        new Ext.Button
                            ({ id: "btnTemp5",
                                width: "10",
                                text: ">>>",
                                labelStyle: "color:red",
                                modal: true,
                                handler: function () {

                                }
                            }),

                        new Ext.Button
                            ({ id: "btnBatchAdjust",
                                //                            width:"10",
                                text: document.getElementById("hfBatchAdjustResult").value,
                                //                            style:"background:#D1C1E1",
                                modal: true,
                                handler: function () {
                                    showOpWaittingDiv();
                                    openResultWindow();
                                    hideOpWaittingDiv();
                                }
                            }),
                        new Ext.Button
                            ({ id: "btnTemp6",
                                width: "10",
                                text: ">>>",
                                labelStyle: "color:red"
                            }),

                        new Ext.Button
                            ({ id: "btnAdjustOTREST",
                                //                            width:"10",
                                text: document.getElementById("hfBtnAdjustOTREST").value,
                                //                            style:"background:#D1C1E1",
                                modal: true,
                                handler: function () {
                                    showOpWaittingDiv();
                                    openAdjustOTRESTWindow();
                                    hideOpWaittingDiv();
                                }
                            }),
                        new Ext.form.Checkbox
                            ({ id: "btnDoDay1",
                                boxLabel: 'D1--D10',
                                checked: true,
                                width: "70",
                                style: "background:yellowgreen",
                                disabled: true,
                                hidden: true,
                                handler: function () {
                                    setColHidden('1');
                                }
                            }),
                        new Ext.form.Checkbox
                            ({ id: "btnDoDay2",
                                boxLabel: 'D11--D20',
                                checked: true,
                                width: "70",
                                style: "background:yellowgreen",
                                disabled: true,
                                hidden: true,
                                handler: function () {
                                    setColHidden('2');
                                }
                            }),
                        new Ext.form.Checkbox
                            ({ id: "btnDoDay3",
                                boxLabel: 'D21--D31',
                                checked: true,
                                width: "70",
                                style: "background:yellowgreen",
                                disabled: true,
                                hidden: true,
                                handler: function () {
                                    setColHidden('3');
                                }
                            })
                        ]
            });
        }
        Ext.onReady(ready);
    </script>
    
    <%--定义加载员工排班列表的函数体--%>
    <script type="text/javascript">
        function LoadStaffPaibanList() {
            //            showOpWaittingDiv();
            var curYearMonth = Ext.getCmp("cboYearMonth").getValue();
            if (curYearMonth != "") {
                Ext.Ajax.request({
                    url: "Ajax/KQPaibanAjax.aspx?param=KQPaiBanColumnInfo&yearMonth=" + curYearMonth + "&userId=" + varUserId,
                    method: "POST",
                    success: function (p1, p2) {
                        var de = Ext.decode(p1.responseText);
                        colCount_Paiban = de.length; //设置全局变量
                        if (de.Msg == "") {
                        }
                        else {
                            datacol = new MakeDataColumn();
                            for (var i = 0; i < de.length; i++) {
                                for (var p in de[i]) {
                                    datacol.addColumns(p, de[i][p]);
                                }
                            }
                            if (Ext.getCmp("editGridPanel")) { Ext.getCmp("formOne").remove(Ext.getCmp("editGridPanel")); }
                            var grid = new MakeGridView().GridView;

                            Ext.getCmp("formOne").add(grid);
                            //删除员工考勤结果GRID
                            if (Ext.getCmp("StuffResultGridPanel")) { Ext.getCmp("formOne").remove(Ext.getCmp("StuffResultGridPanel")); }
                            if (Ext.getCmp("btnSaveResult")) { Ext.getCmp("formOne").remove(Ext.getCmp("btnSaveResult")); }

                            Ext.getCmp("formOne").doLayout();
                        }
                        Ext.getCmp("btnDoDay1").setDisabled(false);
                        Ext.getCmp("btnDoDay2").setDisabled(false);
                        Ext.getCmp("btnDoDay3").setDisabled(false);
                    },
                    failure: function (p1, p2) { }
                })
            } else {
                Ext.Msg.alert(document.getElementById("hfTipTip").value, document.getElementById("hfTipSelectMonth").value);
            }
            hideOpWaittingDiv();

        }
    </script>

    <%--动态设置列隐藏--%>
    <script type="text/javascript">
        function setColHidden(days) {
            var grid = Ext.getCmp("editGridPanel");
            showOpWaittingDiv();

            if (Ext.getCmp("editGridPanel")) {
                var cm = grid.getColumnModel();
                var iCount = cm.getColumnCount();
                if (days == 1) {//D1--D10
                    if (bIsHiddenDay1 == false) {
                        bIsHiddenDay1 = true;
                        for (var i = 5; i <= 14; i++) {
                            cm.setHidden(i, true);
                        }
                    } else {
                        bIsHiddenDay1 = false;
                        for (var i = 5; i <= 14; i++) {
                            cm.setHidden(i, false);
                        }
                    }
                } else if (days == 2) {//D11--D20
                    if (bIsHiddenDay2 == false) {
                        bIsHiddenDay2 = true;
                        for (var i = 15; i <= 24; i++) {
                            cm.setHidden(i, true);
                        }
                    } else {
                        bIsHiddenDay2 = false;
                        for (var i = 15; i <= 24; i++) {
                            cm.setHidden(i, false);
                        }
                    }
                } else if (days == 3) {//D21--D31
                    if (bIsHiddenDay3 == false) {
                        bIsHiddenDay3 = true;
                        for (var i = 25; i <= iCount - 1; i++) {
                            cm.setHidden(i, true);
                        }
                    } else {
                        bIsHiddenDay3 = false;
                        for (var i = 25; i <= iCount - 1; i++) {
                            cm.setHidden(i, false);

                        }
                    }

                }
                hideOpWaittingDiv();
            }
        }
    </script>
    
    <%--新窗口调整异常考勤记录--%>
    <script type="text/javascript">
        function openResultWindow(param) {
            var curYearMonth = Ext.getCmp("cboYearMonth").getValue();
            if (curYearMonth == "") {
                Ext.Msg.alert(document.getElementById("hfTipTip").value, document.getElementById("hfTipSelectMonth").value);
                return;
            }

            var myPanel = new Ext.Panel({
                layout: 'fit',
                html: '<iframe src="FrmAdjustResult.aspx?yearMonth=' + curYearMonth
                    + '&userId=' + varUserId
                    + '&userType=' + varUserType
                    + '&isLock=' + curYearMonthIsLock
                    + '&analysStatus=' + curSectionAnalysStatus
                    + '" width=100% height=100%></iframe>',
                frame: true
            })

            var win = new Ext.Window({
                title: document.getElementById("hfBatchAdjustResult").value,
                maximized: true,
                //            width : 1000,
                //            height : 600,
                resizable: false,
                closable: true,
                draggable: true,
                resizable: false,
                layout: 'fit',
                modal: false,
                plain: true, // 表示为渲染window body的背景为透明的背景
                bodyStyle: 'padding:5px;',
                items: [myPanel],
                buttonAlign: 'center',
                buttons: [{
                    text: document.getElementById("hfBtnClose").value,
                    type: 'button',
                    handler: function () {
                        win.close();
                    }
                }]
            });

            win.show();
        }

    </script>
    <%----%>
    <%--新窗口调整加班调休小时--%>
    <script type="text/javascript">
        function openAdjustOTRESTWindow() {
            var curYearMonth = Ext.getCmp("cboYearMonth").getValue();
            if (curYearMonth == "") {
                Ext.Msg.alert(document.getElementById("hfTipTip").value, document.getElementById("hfTipSelectMonth").value);
                return;
            }

            var myPanel = new Ext.Panel({
                layout: 'fit',
                html: '<iframe src="FrmAdjustOTREST.aspx?yearMonth=' + curYearMonth
                    + '&userId=' + varUserId
                    + '&userType=' + varUserType
                    + '&isLock=' + curYearMonthIsLock
                    + '&analysStatus=' + curSectionAnalysStatus
                    + '" width=100% height=100%></iframe>',
                frame: true
            })

            var win = new Ext.Window({
                title: document.getElementById("hfBtnAdjustOTREST").value,
                maximized: true,
                //            width : 1000,
                //            height : 600,
                resizable: false,
                closable: true,
                draggable: true,
                resizable: false,
                layout: 'fit',
                modal: false,
                plain: true, // 表示为渲染window body的背景为透明的背景
                bodyStyle: 'padding:5px;',
                items: [myPanel],
                buttonAlign: 'center',
                buttons: [{
                    text: document.getElementById("hfBtnClose").value,
                    type: 'button',
                    handler: function () {
                        win.close();
                    }
                }]
            });

            win.show();
        }

    </script>
    
    <%--即时保存排班结果信息--Grid--%>
    <script type="text/javascript">
        function SavePaibanRealTime() {
            if (StoreGrid == null) {
                return;
            }
            if (Ext.getCmp("editGridPanel").activeEditor != null) {
                Ext.getCmp("editGridPanel").activeEditor.completeEdit();
            }

            var recordArray = StoreGrid.getModifiedRecords();
            curGridYearMonth = Ext.getCmp("cboYearMonth").getValue();
            var varAdjustResult = "";
            for (var i = 0; i < recordArray.length; i++) {
                var strStaffNo = recordArray[i].get("EMPLOYEE");
                items = recordArray[i].getChanges();
                for (var key in items) {
                    if (i = 0) {
                        varAdjustResult += key + "＠" + items[key].trim();
                    } else {
                        varAdjustResult += "＄" + key + "＠" + items[key].trim();
                    }
                }

            }
            if (varAdjustResult != "") {
                varAdjustResult = encodeURI(varAdjustResult);
                Ext.Ajax.request({
                    url: "Ajax/KQPaibanAjax.aspx?param=savePaibanRealTime&stuffNo=" + strStaffNo + "&yearMonth=" + curGridYearMonth + "&resultModifyStr=" + varAdjustResult + "&userId=" + varUserId,
                    method: "POST",
                    success: function (p1, p2) {
                        //                        e.record.commit();
                        //重新加载
                        if (Ext.getCmp("editGridPanel")) { Ext.getCmp("formOne").remove(Ext.getCmp("editGridPanel")); }
                        var grid = new MakeGridView().GridView;
                        Ext.getCmp("formOne").add(grid);

                        Ext.getCmp("formOne").doLayout();
                        hideOpWaittingDiv();
                    },
                    failure: function (p1, p2) { }
                })
            }
        }
    </script>
    
    <%--即时保存考勤结果调整数据（单条记录保存）--Grid--%>
    <script type="text/javascript">
        function SaveReusltRealTime(e) {
            if (StoreGrid_Result == null) {
                return;
            }
            if (Ext.getCmp("StuffResultGridPanel").activeEditor != null) {
                Ext.getCmp("StuffResultGridPanel").activeEditor.completeEdit();
            }

            var recordArray = StoreGrid_Result.getModifiedRecords();
            var varAdjustResult = "";
            for (var i = 0; i < recordArray.length; i++) {
                var strStaffNo = recordArray[i].get("EM_NO");
                var strSeqNo = recordArray[i].get("SEQNO");
                var strDate = recordArray[i].get("r_Date");
                items = recordArray[i].getChanges();
                for (var key in items) {
                    if (key == "STXHOUR") {
                        if (!isPositiveNumber(items[key])) {
                            Ext.Msg.alert(document.getElementById("hfTipMsg").value, document.getElementById("hfLVOVMustInt").value);
                        } else {
                            varAdjustResult += "STXHOUR＠" + items[key].trim();
                        }
                    } else if (key == "SJBHOUR") {
                        if (!isPositiveNumber(items[key])) {
                            Ext.Msg.alert(document.getElementById("hfTipMsg").value, document.getElementById("hfLVOVMustInt").value);
                        } else {
                            varAdjustResult += "SJBHOUR＠" + items[key].trim();
                        }
                    } else if (key == "BISTONORMAL") {
                        varAdjustResult += "BISTONORMAL＠" + items[key].trim();
                    } else if (key == "SREMARK") {
                        varAdjustResult += "SREMARK＠" + items[key].trim();
                    }
                }

            }
            if (varAdjustResult != "") {
                varAdjustResult = encodeURI(varAdjustResult);
                Ext.Ajax.request({
                    url: "Ajax/KQPaibanAjax.aspx?param=saveAdjustResultRealTime&seqNo=" + strSeqNo + "&date=" + strDate + "&resultModifyStr=" + varAdjustResult + "&userId=" + varUserId,
                    method: "POST",
                    success: function (p1, p2) {
                        e.record.commit();
                        //重新加载
                        //                        if (Ext.getCmp("StuffResultGridPanel")) { Ext.getCmp("formOne").remove(Ext.getCmp("StuffResultGridPanel")); }
                        //                        var grid = new MakeGridView_Result().GridView;
                        //                        Ext.getCmp("formOne").add(grid);

                        //                        Ext.getCmp("formOne").doLayout();
                        //                        hideOpWaittingDiv();
                    },
                    failure: function (p1, p2) { }
                })
            }
        }
    </script>
    

    <%--批量执行考勤分析的脚本--%>
    <script type="text/javascript">
        //批量执行考勤分析
        function ExecAnalyseBatch() {
            showOpWaittingDiv();
            var curYearMonth = Ext.getCmp("cboYearMonth").getValue();
            if (curYearMonth != "") {
                Ext.Ajax.request({
                    url: "Ajax/KQPaibanAjax.aspx?param=ExcuteSP_Analyse&yearMonth=" + curYearMonth + "&userId=" + varUserId,
                    method: "POST",
                    success: function (p1, p2) {

                        var de = Ext.decode(p1.responseText);
                        if (de == "-1") {
                            Ext.Msg.alert(document.getElementById("hfTipTip").value, document.getElementById("hfTipNoAnalyseFailed").value);
                        }
                        else {
                            //重新加载
                            ReloadFrmData();
                            Ext.Msg.alert(document.getElementById("hfTipTip").value, document.getElementById("hfTipAnalyseSuccess").value);
                        }
                        hideOpWaittingDiv();
                    },
                    failure: function (p1, p2) {

                        Ext.Msg.alert(document.getElementById("hfTipTip").value, 'TimeOut');
                        hideOpWaittingDiv();
                    }


                })

            } else {
                Ext.Msg.alert(document.getElementById("hfTipTip").value, document.getElementById("hfTipSelectMonth").value);
                hideOpWaittingDiv();
            }
        }

        function ReloadFrmData() {
            //重新加载
            if (Ext.getCmp("editGridPanel")) {
                Ext.getCmp("formOne").remove(Ext.getCmp("editGridPanel"));
                var grid = new MakeGridView().GridView;
                Ext.getCmp("formOne").add(grid);
            }
            //删除所有GRID的渲染
            //                                                    if (Ext.getCmp("editGridPanel")) { Ext.getCmp("formOne").remove(Ext.getCmp("editGridPanel")); }
            if (Ext.getCmp("StuffResultGridPanel")) { Ext.getCmp("formOne").remove(Ext.getCmp("StuffResultGridPanel")); }
            if (Ext.getCmp("btnSaveResult")) { Ext.getCmp("formOne").remove(Ext.getCmp("btnSaveResult")); }

            Ext.getCmp("formOne").doLayout();

            //默认显示异常结果
            setTimeout(function () {
                var grid = Ext.getCmp("editGridPanel");
                showUnNormalGrid(grid);
                //关闭后执行  
                hideOpWaittingDiv();
            }, 1000);
        }
    </script>
    

    <%--按员工依次执行考勤分析的脚本 add by sammen 20180122--%>
    <script type="text/javascript">
        //按员工依次执行考勤分析
        function ExecAnalyseByStaff() {
            showOpWaittingDiv();
            $("#divWaitingTitle").empty();
            $("#divWaitingTip").empty();
            var curYearMonth = Ext.getCmp("cboYearMonth").getValue();
            if (curYearMonth != "") {
                Ext.Ajax.request({
                    url: "Ajax/KQPaibanAjax.aspx?param=GetStaffList&yearMonth=" + curYearMonth + "&userId=" + varUserId,
                    method: "POST",
                    async: false,
                    success: function (p1, p2) {
                        //var de1 = Ext.decode(p1.responseText);
                        var data = p1.responseText;
                        if (data == "") return false;
                        var dataobj = eval("(" + data + ")");

                        if (dataobj.ResultData == "") return false;
                        var item = dataobj.ResultData[0];
                        var rows = dataobj.ResultData.length;
                        //alert(rows);

                        var hsTableStaffList = new Hashtable();
                        if (document.getElementById("hfLanguage").value == "zh-cn") {//如果是中文环境
                            $("#divWaitingTitle").append('<br\><span style = "color:#0066FF">(' + new Date().format("yyyy-MM-dd hh:mm:ss") + ')&nbsp;&nbsp;即将对' + rows.toString() + '个员工进行考勤分析......</span>&nbsp;&nbsp;');
                        } else {
                            $("#divWaitingTitle").append('<br\><span style = "color:#0066FF">(' + new Date().format("yyyy-MM-dd hh:mm:ss") + ')&nbsp;&nbsp;Beginning to Analyze ' + rows.toString() + ' Staffs......</span>&nbsp;&nbsp;');
                        }
                        for (var i = 0; i < rows; i++) {
                            var StaffNo = unescape(dataobj.ResultData[i]["EMPLOYEE"]);
                            var StaffName = unescape(dataobj.ResultData[i]["CNAME"]);
                            var StartDay = unescape(dataobj.ResultData[i]["StartDay"]);
                            var EndDay = unescape(dataobj.ResultData[i]["EndDay"]);
                            var TempString = StaffNo + '#' + StaffName;
                            //alert(TempString);

                            hsTableStaffList.remove(i.toString());
                            hsTableStaffList.add(i.toString(), TempString);
                            
                        }
                        var iSuccess = 0;
                        if (rows > 0) {
                            var FirstStartDay = unescape(dataobj.ResultData[0]["StartDay"]);
                            var FirstEndDay = unescape(dataobj.ResultData[0]["EndDay"]);
                            //考勤分析前的数据初始化
                            ExecAnalyseByStaffInit(rows, iSuccess, curYearMonth, varUserId, hsTableStaffList, FirstStartDay, FirstEndDay)
                        } else {
                            if (document.getElementById("hfLanguage").value == "zh-cn") {//如果是中文环境
                                Ext.Msg.alert(document.getElementById("hfTipTip").value, '当前账户没有可分析的员工!', function () {
                                    //关闭后执行  
                                    hideOpWaittingDiv();
                                });
                            } else {
                                Ext.Msg.alert(document.getElementById("hfTipTip").value, 'No Staff to be analyzed!', function () {
                                    //关闭后执行  
                                    hideOpWaittingDiv();
                                });

                            }

                        }

                    },
                    failure: function (p1, p2) {
                        Ext.Msg.alert(document.getElementById("hfTipTip").value, 'Failed', function () {
                            //关闭后执行  
                            hideOpWaittingDiv();
                        });
                    }


                })
            } else {
                Ext.Msg.alert(document.getElementById("hfTipTip").value, document.getElementById("hfTipSelectMonth").value, function () {
                    //关闭后执行  
                    hideOpWaittingDiv();
                });
            }
        }

        //按员工依次执行考勤分析前的数据初始化
        function ExecAnalyseByStaffInit(rows,iSuccess, curYearMonth, varUserId, hsTableStaffList, FirstStartDay, FirstEndDay) {
            ////分析完成后执行汇总
            if (rows > 0) {
                var varSuccess = "";
                var varFailed = "";
                var varAnalyzeOneByOne = "";
                if (document.getElementById("hfLanguage").value == "zh-cn") {//如果是中文环境
                    $("#divWaitingTitle").append('<br\><span style = "color:#0066FF">(' + new Date().format("yyyy-MM-dd hh:mm:ss") + ')&nbsp;&nbsp;正在进行考勤分析数据初始化......请稍后......</span>&nbsp;&nbsp;');
                    varSuccess = "考勤分析数据初始化成功";
                    varFailed = "考勤分析数据初始化失败";
                    varAnalyzeOneByOne = "将根据员工编号依次分析";
                } else {
                    $("#divWaitingTitle").append('<br\><span style = "color:#0066FF">(' + new Date().format("yyyy-MM-dd hh:mm:ss") + ')&nbsp;&nbsp;Initializing attendance data......Waitting......</span>&nbsp;&nbsp;');
                    varSuccess = "Initializing data successfully";
                    varFailed = "Initializing data failed";
                    varAnalyzeOneByOne = "Being Analyzed by Staff No.";
                }
                var varUrl = "Ajax/KQPaibanAjax.aspx?param=ExcuteAnalyseByStaffInit&yearMonth=" + curYearMonth + "&userId=" + varUserId + "&startday=" + FirstStartDay + "&endday=" + FirstEndDay;
                //alert(varUrl);
                //setTimout(function () {
                Ext.Ajax.request({
                    url: varUrl,
                    method: "POST",
                    async: false,
                    success: function (p1, p2) {
                        //var de1 = Ext.decode(p1.responseText);
                        var data = p1.responseText;
                        if (data == "") return false;

                        //alert(data);
                        var dataobj = eval("(" + data + ")");
                        if (data == '1') {
                            $("#divWaitingTitle").append('<br\><span style = "color:green">(' + new Date().format("yyyy-MM-dd hh:mm:ss") + ')&nbsp;&nbsp;' + varSuccess + ','+varAnalyzeOneByOne+'......<span id="spanShowProgressPercent" style = "color:red"></span>......</span>&nbsp;&nbsp;');
                            //考勤分析数据初始化成功后，首先从第一个开始执行员工分析
                            //var FirstStaffNo = unescape(dataobj.ResultData[0]["EMPLOYEE"]);
                            //var FirstStaffName = unescape(dataobj.ResultData[0]["CNAME"]);
                            //alert("---StaffNo:" + FirstStaffNo + ";StartDay:" + FirstStartDay + ";EndDay:" + FirstEndDay);
                            DoExecAnalyseByStaff(0, rows, iSuccess, curYearMonth, varUserId, hsTableStaffList, FirstStartDay, FirstEndDay);

                        } else {
                            $("#divWaitingTitle").append('<br\><span style = "color:red">(' + new Date().format("yyyy-MM-dd hh:mm:ss") + ')&nbsp;&nbsp;' + varFailed + '</span>&nbsp;&nbsp;');
                            Ext.Msg.alert(document.getElementById("hfTipTip").value, '' + varFailed + '!', function () {
                                //关闭后执行  
                                hideOpWaittingDiv();
                            });
                        }
                    },
                    failure: function (p1, p2) {
                        Ext.Msg.alert(document.getElementById("hfTipTip").value, '' + varFailed + '!', function () {
                            //关闭后执行  
                            hideOpWaittingDiv();
                        });
                    }
                })
                //}, 500);
            } else {
                Ext.Msg.alert(document.getElementById("hfTipTip").value, '' + varFailed + '!', function () {
                    //关闭后执行  
                    hideOpWaittingDiv();
                });
            }
        }

        //依次执行每个员工的考勤分析
        function DoExecAnalyseByStaff(index, rows, iSuccess, curYearMonth, varUserId, hsTableStaffList, StartDay, EndDay) {
            var varAnalyzeStaff = "";
            var varSuccess = "";
            var varFailed = "";
            var varAnalyzeOneByOne = "";
            var varComplete = "";
            if (document.getElementById("hfLanguage").value == "zh-cn") {//如果是中文环境
                varAnalyzeStaff = "分析员工";
                varSuccess = "考勤分析成功";
                varFailed = "考勤分析失败";
                varAnalyzeOneByOne = "将根据员工编号依次分析";
                varComplete = "已完成";
            } else {
                varAnalyzeStaff = "Analyze Staff";
                varSuccess = "Analyze Result Successfully";
                varFailed = "Analyze Result failed";
                varAnalyzeOneByOne = "Being Analyzed by Staff No.";
                varComplete = "Completed ";
            }

            var StaffString = hsTableStaffList.items(index.toString());
            var StaffStringArray = StaffString.split('#');
            if(StaffStringArray!=null&&StaffStringArray.length==2){                
                var StaffNo = StaffStringArray[0];
                var StaffName = StaffStringArray[1];
                var analyseUrl = "Ajax/KQPaibanAjax.aspx?param=ExcuteAnalyseByStaff&yearMonth=" + curYearMonth + "&userId=" + varUserId + "&stuffNo=" + StaffNo + "&startday=" + StartDay + "&endday=" + EndDay;
                Ext.Ajax.request({
                    url: analyseUrl,
                    method: "POST",
                    async: false,
                    success: function (p1, p2) {
                        //var de1 = Ext.decode(p1.responseText);
                        var data = p1.responseText;
                        if (data == "") return false;

                        var dataobj = eval("(" + data + ")");

                        if (data.toString() == "1") {
                            //$("#divWaitingTip").append('<br\><span id = "span_"' + StaffNo + ' color="red">' + (index + 1).toString() +analyseUrl+'</span>');
                            var tipsHtml = '<br\><span style = "color:green">' + (index + 1).toString() + '/' + (rows).toString() + '、(' + new Date().format("yyyy-MM-dd hh:mm:ss") + ')&nbsp;&nbsp;' + varAnalyzeStaff + ':' + StaffName + '(' + StaffNo + ')' + varSuccess + '&nbsp;&nbsp;√</span>';
                            var varPercent = Math.round((index+1) / rows * 100);
                            $("#spanShowProgressPercent").html(varComplete + varPercent.toString() + "%");

                            $("#divWaitingTip").prepend(tipsHtml);
                            iSuccess++;
                        } else {
                            //Ext.Msg.alert(document.getElementById("hfTipTip").value, '针对员工' + StaffName + '(' + StaffNo + ')的考勤分析失败!');
                            var tipsHtml = '<br\><span style = "color:red">' + (index + 1).toString() + '/' + (rows).toString() + '、(' + new Date().format("yyyy-MM-dd hh:mm:ss") + ')&nbsp;&nbsp;' + varAnalyzeStaff + '' + StaffName + '(' + StaffNo + ')' + varFailed + '&nbsp;&nbsp;×</span>&nbsp;&nbsp;';
                            $("#divWaitingTip").prepend(tipsHtml);
                        
                        }

                        if (index < rows - 1) {
                            //接着执行下一个员工
                            DoExecAnalyseByStaff(index + 1, rows,iSuccess, curYearMonth, varUserId, hsTableStaffList, StartDay, EndDay)
                        }else if (index == rows - 1) {
                            //按员工依次执行考勤分析后进行汇总计算
                            ExecAnalyseByStaffSummary(index, rows,iSuccess, curYearMonth, varUserId,hsTableStaffList, StartDay, EndDay);
                        }
                    },
                    failure: function (p1, p2) {
                        Ext.Msg.alert(document.getElementById("hfTipTip").value, varAnalyzeStaff + StaffName + '(' + StaffNo + ')' + varFailed + '!', function () {
                            //关闭后执行  
                            hideOpWaittingDiv();
                        });
                    }
                })
            } else {
                Ext.Msg.alert(document.getElementById("hfTipTip").value, varAnalyzeStaff + StaffName + '(' + StaffNo + ')' + varFailed + '!', function () {
                    //关闭后执行  
                    hideOpWaittingDiv();
                });
            }
        }

        //按员工依次执行考勤分析后进行汇总计算
        function ExecAnalyseByStaffSummary(index, rows, iSuccess, curYearMonth, varUserId, hsTableStaffList, StartDay, EndDay) {
            var varAnalyzeStaff = "";
            var varSuccess = "";
            var varFailed = "";
            var varAnalyzeOneByOne = "";
            if (document.getElementById("hfLanguage").value == "zh-cn") {//如果是中文环境
                varAnalyzeStaff = "分析员工";
                varSuccess = "考勤分析成功";
                varFailed = "考勤分析失败";
                varAnalyzeOneByOne = "将根据员工编号依次分析";
            } else {
                varAnalyzeStaff = "Analyze Staff";
                varSuccess = "Analyze Result Successfully";
                varFailed = "Analyze Result failed";
                varAnalyzeOneByOne = "Being Analyzed by Staff No.";
            }

            ////分析完成后执行汇总
            if (index == rows - 1) {
                if (document.getElementById("hfLanguage").value == "zh-cn") {//如果是中文环境
                    $("#divWaitingTitle").append('<br\><span style = "color:#0066FF">(' + new Date().format("yyyy-MM-dd hh:mm:ss") + ')&nbsp;&nbsp;开始对' + rows.toString() + '个员工进行考勤分析汇总......</span>&nbsp;&nbsp;');
                } else {
                    $("#divWaitingTitle").append('<br\><span style = "color:#0066FF">(' + new Date().format("yyyy-MM-dd hh:mm:ss") + ')&nbsp;&nbsp;Being Analyze Summary ' + rows.toString() + 'Staffs......</span>&nbsp;&nbsp;');
                }
                var varUrl = "Ajax/KQPaibanAjax.aspx?param=ExcuteAnalyseByStaffSummary&yearMonth=" + curYearMonth + "&userId=" + varUserId + "&startday=" + StartDay + "&endday=" + EndDay;
                //alert(varUrl);
                //setTimout(function () {
                Ext.Ajax.request({
                    url: varUrl,
                    method: "POST",
                    async: false,
                    success: function (p1, p2) {
                        //var de1 = Ext.decode(p1.responseText);
                        var data = p1.responseText;
                        if (data == "") return false;

                        //alert(data);
                        var dataobj = eval("(" + data + ")");
                        if (data == '1') {
                            if (document.getElementById("hfLanguage").value == "zh-cn") {//如果是中文环境
                                $("#divWaitingTitle").append('<br\><span style = "color:green">(' + new Date().format("yyyy-MM-dd hh:mm:ss") + ')&nbsp;&nbsp;成功完成' + iSuccess.toString() + '个员工的考勤分析</span>&nbsp;&nbsp;');
                                Ext.Msg.alert(document.getElementById("hfTipTip").value, '成功完成' + rows.toString() + '个员工的考勤分析!', function () {
                                    //重新加载
                                    ReloadFrmData();
                                });
                            } else {
                                $("#divWaitingTitle").append('<br\><span style = "color:green">(' + new Date().format("yyyy-MM-dd hh:mm:ss") + ')&nbsp;&nbsp;Completed ' + iSuccess.toString() + ' Staffs Analyzed </span>&nbsp;&nbsp;');
                                Ext.Msg.alert(document.getElementById("hfTipTip").value, 'Completed ' + rows.toString() + ' Staffs Analyzed!', function () {
                                    //重新加载
                                    ReloadFrmData();
                                });
                            }
                        } else {
                            if (document.getElementById("hfLanguage").value == "zh-cn") {//如果是中文环境
                                $("#divWaitingTitle").append('<br\><span style = "color:red">(' + new Date().format("yyyy-MM-dd hh:mm:ss") + ')&nbsp;&nbsp;针对' + rows.toString() + '个员工的考勤分析汇总失败</span>&nbsp;&nbsp;');
                                Ext.Msg.alert(document.getElementById("hfTipTip").value, '针对' + rows.toString() + '个员工的考勤分析汇总失败!', function () {
                                    //重新加载
                                    ReloadFrmData();
                                });
                            } else {
                                $("#divWaitingTitle").append('<br\><span style = "color:red">(' + new Date().format("yyyy-MM-dd hh:mm:ss") + ')&nbsp;&nbsp;Failed to Analyzed' + rows.toString() + ' Staffs</span>&nbsp;&nbsp;');
                                Ext.Msg.alert(document.getElementById("hfTipTip").value, 'Failed to Analyzed ' + rows.toString() + ' Staffs!', function () {
                                    //重新加载
                                    ReloadFrmData();
                                });
                            }
                        }
                    },
                    failure: function (p1, p2) {
                        if (document.getElementById("hfLanguage").value == "zh-cn") {//如果是中文环境
                            Ext.Msg.alert(document.getElementById("hfTipTip").value, '针对' + rows.toString() + '个员工的考勤分析汇总失败!', function () {
                                //重新加载
                                ReloadFrmData();
                            });
                        } else {
                            Ext.Msg.alert(document.getElementById("hfTipTip").value, 'Failed to Analyzed ' + rows.toString() + ' Staffs!', function () {
                                //重新加载
                                ReloadFrmData();
                            });
                        }
                    }
                })
                //}, 500);
            } else {
                if (document.getElementById("hfLanguage").value == "zh-cn") {//如果是中文环境
                    Ext.Msg.alert(document.getElementById("hfTipTip").value, '考勤分析失败,共' + rows.toString() + '个员工，成功分析' + iSuccess.toString() + '个!', function () {
                        //重新加载
                        ReloadFrmData();
                    });
                } else {
                    Ext.Msg.alert(document.getElementById("hfTipTip").value, 'Failed,Total ' + rows.toString() + ' Staffs，' + iSuccess.toString() + ' Successfully', function () {
                        //重新加载
                        ReloadFrmData();
                    });
                }
            }
        }

    </script>

    <%----%>
    <script type="text/javascript">

        hideOpWaittingDiv();
    </script>

</form>
</body>
</html>