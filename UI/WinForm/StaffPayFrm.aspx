<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StaffPayFrm.aspx.cs" Inherits="WinForm_StaffPayFrm" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<title>员工薪资变更调整</title>
<link href="../extjs/resources/css/ext-all.css" rel="stylesheet" type="text/css" />

<script src="../extjs/adapter/ext/ext-base.js" type="text/javascript"></script>

<script src="../extjs/ext-all.js" type="text/javascript"></script>

<script src="../common/js/stringUtil.js" type="text/javascript"></script>
<script src="../common/js/customObject.js" type="text/javascript"></script>
<style >
    .focusCellCss{background:#F9F;}
    .unNormalCellCss{background:#ff0000;}
    .unToNormalCellCss{background:#ffff00;}
    .x-grid3-row-over {cursor:hand; }
</style>
</head>
<body>
<form id="form1" runat="server">
<asp:HiddenField ID="hfLanguage" runat ="server" />
<asp:HiddenField ID="hfCurYearMonth" runat ="server" />

<asp:HiddenField ID="hfTitle" runat ="server" />
<asp:HiddenField ID="hfBtnSave" runat ="server" />
<asp:HiddenField ID="hfTipGridColumnsText" runat ="server" />
<asp:HiddenField ID="hfTipGridSortAscText" runat ="server" />
<asp:HiddenField ID="hfTipMsg" runat ="server" />
<asp:HiddenField ID="hfTipSaveMsg" runat ="server" />
<asp:HiddenField ID="hfTipNoSave" runat ="server" />
<asp:HiddenField ID="hfTipGridSortDescText" runat ="server" />

    <div id="div1" style="width:100%;">
    
    </div>
    
    <div id="div2" style="width:100%;height:100%; text-align:center; vertical-align:middle;display:none" class="ext-el-mask">
        <img src="../extjs/resources/images/default/shared/large-loading.gif" style="width:15; height:15" />
        <font color="gray" >Loading.......</font>
    </div>
    
    <%--定义显示/隐藏 操作处理中层--Grid--%>
    <script type="text/javascript">
        function showOpWaittingDiv(){//显示操作处理中层
            Ext.fly("div2").fadeIn(); 
        }
        function hideOpWaittingDiv(){//操作完成，隐藏处理中层
            Ext.fly("div2").fadeOut(); 
        }
      showOpWaittingDiv(); 
    </script>
    
    <%--声明全局的变量--%>
    <script type="text/javascript">
        //为获取动态生成员工列表的列而设置的变量
        var datacol_staffList=null;
        //为获取员工列表表格数据的store（当前页）
        var StoreGrid_staffList=null;
        //员工列表表格的ColumnModel
        var cmStaffList = null;
        //为获取员工列表表格所有数据的store
        var StoreGrid_staffList_All=null;
        
        //为获取动态生成薪资项目的列而设置的变量
        var datacol_payItemList=null;
        //为获取薪资项目表格数据的store
        var StoreGrid_payItemList=null;
        
        //员工列表每页显示记录数
        var iPageSizeNum = 25;
        
        //当前选择的员工编号
        var CurYearMonthDcno = "";
    </script>  
    
    <%--加载所有薪资调整员工列表信息数据--Grid--%>
    <script type="text/javascript">
        function LoadAllStaffList(strFilterSql){
            strFilterSql = encodeURI(strFilterSql);
            var fields = eval('([' + datacol_staffList.fields + '])');
            StoreGrid_staffList_All = new Ext.data.Store
            ({
                proxy: new Ext.data.HttpProxy({ url: "Ajax/AdjustStaffPayAjax.aspx?param=FilterStaffListDataInfo&filterSql="+strFilterSql }),
                reader: new Ext.data.JsonReader({ totalProperty: "AllStaffListTotalPorperty", root: "AllStaffList", fields: fields }),
                storeId:"YEARMONTHDCNO"
            });
            StoreGrid_staffList_All.removeAll();
            StoreGrid_staffList_All.load({params:{start:0,limit:iPageSizeNum}});
            //重新绑定grid列表
            Ext.getCmp("StaffListGridPanel").reconfigure(StoreGrid_staffList_All,cmStaffList);
            //重新绑定分页工具栏
            Ext.getCmp("pagingStaffList").bind(StoreGrid_staffList_All);
        }
    </script>
    
    <%--ExtJS页面加载及渲染--Grid--%>
    <script type="text/javascript">
        function ready() {
            var formMain = new Ext.Panel({
                id: "formMain",
                renderTo: "div1",
                layout: "column",
                title: '提示：您可以在此页面中对单击员工信息在右边列表中对薪资变更进行批量调整',
                autoWidth: false,
                autoScroll: true,
                buttonAlign: "right",
                loader: loadAdjustStaffList(),
                tbar: [
                       new Ext.form.TextField({
                           id: "txtStaffNoFilter",
                           emptyText: "请输入员工编号进行过滤",
                           width: "150",
                           enableKeyEvents: true,
                           listeners: {
                               keyup: function (e) {
                                   LoadAllStaffList("EMPNO like '%" + Ext.getCmp("txtStaffNoFilter").getValue() + "%'");
                                   //                                  StoreGrid_staffList_All.filter("EMPNO",Ext.getCmp("txtStaffNoFilter").getValue());
                               }
                           }
                       }),
                       new Ext.Button
                            ({ id: "btnTemp1",
                                width: "10",
                                text: "   "
                            }),
                       new Ext.form.TextField({
                           id: "txtStaffEnNameFilter",
                           emptyText: "请输入员工英文名进行过滤",
                           width: "150",
                           enableKeyEvents: true,
                           listeners: {
                               keyup: function (e) {
                                   LoadAllStaffList("EMPNAME like '%" + Ext.getCmp("txtStaffEnNameFilter").getValue() + "%'");
                               }
                           }
                       }),
                       new Ext.form.TextField({
                           id: "txtStaffCnNameFilter",
                           emptyText: "请输入员工中文名进行过滤",
                           width: "150",
                           enableKeyEvents: true,
                           listeners: {
                               keyup: function (e) {
                                   //                                    if(Ext.getCmp("txtStaffCnNameFilter").getValue().length>0){
                                   LoadAllStaffList("EMPNAMECHS like '%" + Ext.getCmp("txtStaffCnNameFilter").getValue() + "%'");
                                   //                                    }
                               }
                           }
                       }),
                       new Ext.Button
                            ({ id: "btnTemp2",
                                width: "10",
                                text: "   "
                            }),
                       new Ext.form.TextField({
                           id: "txtDepartmentFilter",
                           emptyText: "请输入部门编号进行过滤",
                           width: "200",
                           enableKeyEvents: true,
                           listeners: {
                               keyup: function (e) {
                                   LoadAllStaffList("DCDDESCCHS like '%" + Ext.getCmp("txtDepartmentFilter").getValue() + "%'");
                               }
                           }
                       }),
                       new Ext.Button
                            ({ id: "btnTemp3",
                                width: "10",
                                text: "   "
                            }),
                       new Ext.form.TextField({
                           id: "txtDepartmentEnNameFilter",
                           emptyText: "请输入部门名称进行过滤",
                           width: "200",
                           enableKeyEvents: true,
                           listeners: {
                               keyup: function (e) {
                                   LoadAllStaffList("ODESC like '%" + Ext.getCmp("txtDepartmentEnNameFilter").getValue() + "%'");
                               }
                           }
                       }),
                       new Ext.Button
                            ({ id: "btnTemp4",
                                width: "10",
                                text: "   "
                            }),
                       new Ext.form.TextField({
                           id: "txtDepartmentCnNameFilter",
                           emptyText: "请输入部门中文名称进行过滤",
                           width: "200",
                           enableKeyEvents: true,
                           listeners: {
                               keyup: function (e) {
                                   LoadAllStaffList("ODESCCHS like '%" + Ext.getCmp("txtDepartmentCnNameFilter").getValue() + "%'");
                               }
                           }
                       }),
                       btnSaveStaffList,
                       new Ext.Button
                            ({ id: "btnTemp5",
                                width: "10",
                                text: "   "
                            }),
                       new Ext.Button
                            ({ id: "btnQueryReuslt",
                                text: "查看薪资条",
                                width: "100",
                                style: "background:#ABC7EC",
                                modal: true,
                                handler: function () {
                                    window.open('../Query/SPQuery.aspx?SP=USP_HR_QRY_PREMPL_CalResult&P0=NONE', 'btnQueryReuslt', 'left=0,top=0,width=' + (screen.availWidth - 10) + ',height=' + (screen.availHeight - 50) + ',scrollbars,resizable=yes,toolbar=no');
                                }
                            }),
                       new Ext.Button
                            ({ id: "btnTemp6",
                                width: "10",
                                text: "   "
                            }),
                       new Ext.Button
                            ({ id: "btnClose",
                                text: "关闭",
                                width: "20",
                                style: "background:#ABC7EC",
                                modal: true,
                                handler: function () {
                                    window.close();
                                }
                            })

                    ]
            });
            
            if(document.getElementById("hfLanguage").value=="zh-cn"){//如果是中文环境
                Ext.getCmp("txtStaffCnNameFilter").show();
                Ext.getCmp("txtStaffEnNameFilter").hide();
                Ext.getCmp("txtDepartmentCnNameFilter").show();
                Ext.getCmp("txtDepartmentEnNameFilter").hide();
            }else{//如果是英文文环境
                Ext.getCmp("txtStaffCnNameFilter").hide();
                Ext.getCmp("txtStaffEnNameFilter").show();
                Ext.getCmp("txtDepartmentCnNameFilter").hide();
                Ext.getCmp("txtDepartmentEnNameFilter").show();
            }
        }  
        Ext.onReady(ready);
    </script>
    
    <%--保存薪资调整员工数据按钮--Grid--%>
    <script type="text/javascript">
        var btnSaveStaffList = new Ext.Button
            ({id: "btnSaveStaffList",
            text: "保存薪资调整结果",
            width:"100",
            style: "background:#ABC7EC",
            handler:function(){
                showOpWaittingDiv(); 
                if(StoreGrid_payItemList==null){
                    hideOpWaittingDiv(); 
                    return;
                }
                if(Ext.getCmp("payItemListGridPanel").activeEditor!=null){
                    Ext.getCmp("payItemListGridPanel").activeEditor.completeEdit(); 
                }

                var staffLists={};
                var recordArray = StoreGrid_payItemList.getModifiedRecords();
                var recordOld;
                var staffListStr = '';
                var strYearMonthDcno = '';
                for(var i=0;i< recordArray.length;i++){
                    strYearMonthDcno = recordArray[i].get("YEARMONTHDCNO");
                    var strItemCode = recordArray[i].get("ITEMCODE");
                    var staffListStr1 = '';
                    
                    items = recordArray[i].getChanges();
                    for(var key in items){
                        if(items[key]==""){
                            staffListStr1 += "^" + key+'*'+'space';
                        }else{
                            if((key=="ITEMEVALUE")&&(!isAValue(items[key]))){
                                Ext.Msg.alert(document.getElementById("hfTipMsg").value, '项目值请输入数值');
                                hideOpWaittingDiv(); 
                                return;
                            }
                            staffListStr1 += "^" + key+'*'+items[key].trim().replace('!','').replace('#','').replace('$','').replace('^','').replace('*','');
                        }
                    }
                    staffListStr1 = staffListStr1.substring(1,staffListStr1.length);
                    staffListStr += "!" + strYearMonthDcno+'#'+strItemCode+"$" +staffListStr1;
    //　　                                staffListStr = encodeURI(staffListStr);
                    
                    //****拼写字符串的含义*****
                    //!   ：记录与记录之间间隔符
                    //#   ：员工编号与薪资项目编码间隔符
                    //$   ：一条记录key与其修改所有对象的间隔符
                    //^   ：同一条记录的多个修改对象之间的间隔符
                    //*   ：同一条记录的同一个修改对象中，修改字段及修改后值之间的间隔符
                    //****拼写字符串的含义*****
                };
                staffListStr = staffListStr.substring(1,staffListStr.length);
    //                                alert(staffListStr);
                if (staffListStr!=""){
                    Ext.Ajax.request({
                        url: "Ajax/AdjustStaffPayAjax.aspx?param=saveAdjustPayValueInfo&YEARMONTHDCNO=" + strYearMonthDcno,
                        params: { staffListModifyStr: staffListStr },
                        method: "POST",
                        success: function(p1, p2) {
                            Ext.Msg.alert(document.getElementById("hfTipMsg").value, document.getElementById("hfTipSaveMsg").value);
                            //重新加载
                            if (Ext.getCmp("payItemListGridPanel")) { Ext.getCmp("formMain").remove(Ext.getCmp("payItemListGridPanel")); }
                            var grid_payItemList = new MakeGridView_payItemList(CurYearMonthDcno).GridView; 
                            Ext.getCmp("formMain").add(grid_payItemList);
                            Ext.getCmp("formMain").doLayout();
                            
                            hideOpWaittingDiv(); 
                        },
                        failure: function(p1, p2) { }
                    })
                }else{
                    Ext.Msg.alert(document.getElementById("hfTipMsg").value, document.getElementById("hfTipNoSave").value);
                    hideOpWaittingDiv(); 
                }
    //                                hideOpWaittingDiv(); 
            }
            });
    </script>
    
    <%--薪资调整员工列表信息数据--Grid--%>
    <script type="text/javascript">
        function MakeDataColumn_staffList() {//加载薪资调整员工列表信息数据列
            this.fields = '';
            this.columns = '';
            this.addColumns = function(name, caption) {
                if (this.fields.length > 0) {
                    this.fields += ',';
                }
                if (this.columns.length > 0) {
                    this.columns += ',';
                }
                this.fields += '{name:"' + name + '"}';
                if (name == 'YEARMONTHDCNO') {//年月员工
                    this.columns += '{header:"' + caption + '",dataIndex:"' + name + '",width:6,sortable:true,hidden:true}';
                }
                else if (name == 'YEARMONTH') {//月份
                    this.columns += '{header:"' + caption + '",dataIndex:"' + name + '",width:4,sortable:true,hidden:false}';
                } 
                else if (name == 'EMPNO') {//员工编号
                    this.columns += '{header:"' + caption + '",dataIndex:"' + name + '",width:6,sortable:true,hidden:false}';
                } else if ((name == 'EMPNAMECHS')) {
                    if(document.getElementById("hfLanguage").value=="zh-cn"){//如果是中文环境则显示中文名
                        this.columns += '{header:"' + caption + '",dataIndex:"' + name + '",width:5,sortable:true,hidden:false}';
                    }else{
                        this.columns += '{header:"' + caption + '",dataIndex:"' + name + '",width:5,sortable:true,hidden:true}';
                    }
                }else if((name=='EMPNAME')){
                    if(document.getElementById("hfLanguage").value=="zh-cn"){//如果是英文环境则显示英文名
                        this.columns += '{header:"' + caption + '",dataIndex:"' + name + '",width:10,sortable:true,hidden:true}';
                    }else{
                        this.columns += '{header:"' + caption + '",dataIndex:"' + name + '",width:10,sortable:true,hidden:false}';
                    }
                } else if (name == 'DCDDESCCHS') {//部门编码
                    this.columns += '{header:"' + caption + '",dataIndex:"' + name + '",width:5,sortable:true,hidden:false}';
                } else if (name == 'ODESCCHS') {//部门名称
                    if (document.getElementById("hfLanguage").value == "zh-cn") {//如果是英文环境则显示英文名
                        this.columns += '{header:"' + caption + '",dataIndex:"' + name + '",width:20,sortable:true,hidden:false}';
                    } else {
                        this.columns += '{header:"' + caption + '",dataIndex:"' + name + '",width:20,sortable:true,hidden:true}';
                    }
                } else if (name == 'ODESC') {//部门名称
                    if (document.getElementById("hfLanguage").value == "zh-cn") {//如果是英文环境则显示英文名
                        this.columns += '{header:"' + caption + '",dataIndex:"' + name + '",width:20,sortable:true,hidden:true}';
                    } else {
                        this.columns += '{header:"' + caption + '",dataIndex:"' + name + '",width:20,sortable:true,hidden:false}';
                    }
                } 
                 
            }
        }
        
        function MakeGridView_staffList() {//加载薪资调整员工列表信息数据
            cmStaffList = new Ext.grid.ColumnModel(eval('([' + datacol_staffList.columns + '])'));
            cmStaffList.defaultSortable = true;
            var fields = eval('([' + datacol_staffList.fields + '])');
            StoreGrid_staffList = new Ext.data.Store
            ({
                proxy: new Ext.data.HttpProxy({ url: "Ajax/AdjustStaffPayAjax.aspx?param=MultiPageStaffListDataInfo" }),
                reader: new Ext.data.JsonReader({ totalProperty: "StaffListTotalPorperty", root: "StaffList", fields: fields }),
                storeId:"YEARMONTHDCNO"
            });
            StoreGrid_staffList.removeAll();
            StoreGrid_staffList.load({params:{start:0,limit:iPageSizeNum}});
            var gridPanel_staffList = new Ext.grid.EditorGridPanel
            ({
                cm: cmStaffList,
                id: "StaffListGridPanel",
                columnWidth:.5, 
                store: StoreGrid_staffList,
                frame: false,
                border: true,
                height: 650,
                stripeRows: true,
                columnLines:true,
                trackMouseOver:true,
                sm : new Ext.grid.RowSelectionModel({singleSelect:false}),
                viewConfig:{   
                    columnsText:document.getElementById("hfTipGridColumnsText").value,
                    sortAscText:document.getElementById("hfTipGridSortAscText").value,   
                    sortDescText:document.getElementById("hfTipGridSortDescText").value,   
                    forceFit:true  
                },
                bbar: new Ext.PagingToolbar({
                    id:"pagingStaffList",
                    pageSize: iPageSizeNum,
                    store: StoreGrid_staffList,
                    displayInfo: true,
                    displayMsg: '显示第{0} 条到{1} 条记录，一共{2} 条',
                    emptyMsg: "没有记录"
                })
            });
            this.GridView = gridPanel_staffList;
            
            gridPanel_staffList.addListener('rowclick', showStaffPayItemList);  
        }    
        
        //选择某员工显示其对应的薪资调整项目
        var showStaffPayItemList = function(grid, rowIndex, e){ 
            showOpWaittingDiv();
            var selectionModel = grid.getSelectionModel();
            selectionModel.selectRow(rowIndex);    
            var record = selectionModel.getSelected();
            var varSHCode = record.data['YEARMONTHDCNO'];
            CurYearMonthDcno = varSHCode;
            loadStaffPayItemList(varSHCode);
            hideOpWaittingDiv(); 
        }
        
        //加载薪资调整员工列表信息
        function loadAdjustStaffList(){
            Ext.Ajax.request({
                url: "Ajax/AdjustStaffPayAjax.aspx?param=AdjustStaffListColumnInfo",
                method: "POST",
                success: function(p1, p2) {
                    var de1 = Ext.decode(p1.responseText);
                    datacol_staffList = new MakeDataColumn_staffList();
                    for (var j = 0; j < de1.length; j++) {
                        for (var q in de1[j]) {
                            datacol_staffList.addColumns(q, de1[j][q]);
                        }
                    }
                    if (Ext.getCmp("StaffListGridPanel")) { Ext.getCmp("formMain").remove(Ext.getCmp("StaffListGridPanel")); }
                    var grid_staffList = new MakeGridView_staffList().GridView; 
                    Ext.getCmp("formMain").add(grid_staffList);
                    Ext.getCmp("formMain").doLayout();
                    
                 }
             })
        }
        
        hideOpWaittingDiv(); 
    </script>
    
    <%--薪资调整薪资项目列表信息数据--Grid--%>
    <script type="text/javascript">
        function MakeDataColumn_payItemList() {//加载薪资调整薪资项目列表信息数据列
            this.fields = '';
            this.columns = '';
            this.addColumns = function(name, caption) {
                if (this.fields.length > 0) {
                    this.fields += ',';
                }
                if (this.columns.length > 0) {
                    this.columns += ',';
                }
                this.fields += '{name:"' + name + '"}';
                if (name == 'YEARMONTHDCNO') {//年月员工
                    this.columns += '{header:"' + caption + '",dataIndex:"' + name + '",width:6,sortable:true,hidden:true,css:"background: #9F9;"}';
                }
                else if (name == 'EMPNO') {//员工编号
                    this.columns += '{header:"' + caption + '",dataIndex:"' + name + '",width:8,sortable:true,hidden:false,css:"background: #9F9;"}';
                }
                else if (name == 'YEARMONTH') {//月份
                    this.columns += '{header:"' + caption + '",dataIndex:"' + name + '",width:5,sortable:true,hidden:false,css:"background: #9F9;"}';
                } else if ((name == 'ITEMNAMECHS')) {
                    if(document.getElementById("hfLanguage").value=="zh-cn"){//如果是中文环境则显示中文名
                        this.columns += '{header:"' + caption + '",dataIndex:"' + name + '",width:15,sortable:true,hidden:false,css:"background: #9F9;"}';
                    }else{
                        this.columns += '{header:"' + caption + '",dataIndex:"' + name + '",width:15,sortable:true,hidden:true,css:"background: #9F9;"}';
                    }
                }else if((name=='ITEMNAME')){
                    if(document.getElementById("hfLanguage").value=="zh-cn"){//如果是英文环境则显示英文名
                        this.columns += '{header:"' + caption + '",dataIndex:"' + name + '",width:15,sortable:true,hidden:true,css:"background: #9F9;"}';
                    }else{
                        this.columns += '{header:"' + caption + '",dataIndex:"' + name + '",width:15,sortable:true,hidden:false,css:"background: #9F9;"}';
                    }
                }else if((name=='EMPNAME')||(name=='EMPNAMECHS')){//员工姓名隐藏
                    this.columns += '{header:"' + caption + '",dataIndex:"' + name + '",width:8,sortable:true,hidden:true,css:"background: #9F9;"}';
                }else if(name=='ITEMEVALUE'){//可编辑值
                    this.columns += '{header:"' + caption + '",dataIndex:"' + name + '",width:6,sortable:true,hidden:false,css:"background: #DA70D6;",editor:new Ext.form.TextField({maxLength:9 })}';
                }else if(name=='ITEMCOMM'){//备注可编辑
                    this.columns += '{header:"' + caption + '",dataIndex:"' + name + '",width:10,sortable:true,hidden:false,css:"background: #DA70D6;",editor:new Ext.form.TextArea({maxLength:30})}';
                }else{
                    this.columns += '{header:"' + caption + '",dataIndex:"' + name + '",width:10,sortable:true,hidden:false,css:"background: #9F9;"}';
                }
                 
            }
        }
        
        function MakeGridView_payItemList(YearMonthDcno) {//加载薪资调整薪资项目列表信息数据
            var cm = new Ext.grid.ColumnModel(eval('([' + datacol_payItemList.columns + '])'));
            cm.defaultSortable = true;
            var fields = eval('([' + datacol_payItemList.fields + '])');
            StoreGrid_payItemList = new Ext.data.Store
            ({
                proxy: new Ext.data.HttpProxy({ url: "Ajax/AdjustStaffPayAjax.aspx?param=StaffPayItemListDataInfo&YEARMONTHDCNO=" + YearMonthDcno }),
                reader: new Ext.data.JsonReader({ totalProperty: "StaffPayItemListTotalPorperty", root: "StaffPayItemList", fields: fields }),
                storeId:"itemcode"
            });
            StoreGrid_payItemList.removeAll();
            StoreGrid_payItemList.load({});
            var gridPanel_payItemList = new Ext.grid.EditorGridPanel
            ({
                cm: cm,
                id: "payItemListGridPanel",
                columnWidth:.5, 
                store: StoreGrid_payItemList,
                frame: false,
                border: true,
                height: 650,
                stripeRows: true,
                clicksToEdit:1,
                columnLines:true,
                trackMouseOver:true,
                sm : new Ext.grid.RowSelectionModel({singleSelect:false}),
                viewConfig:{   
                    columnsText:document.getElementById("hfTipGridColumnsText").value,
                    sortAscText:document.getElementById("hfTipGridSortAscText").value,   
                    sortDescText:document.getElementById("hfTipGridSortDescText").value,   
                    forceFit:true  
                }
//                ,
//                bbar: new Ext.PagingToolbar({
////                    pageSize: iPageSizeNum,
//                    store: StoreGrid_payItemList,
//                    displayInfo: true,
//                    displayMsg: '显示第{0} 条到{1} 条记录，一共{2} 条',
//                    emptyMsg: "没有记录"
//                })
            });
            this.GridView = gridPanel_payItemList;
        }    
        
        //加载薪资调整薪资项目列表信息
        function loadStaffPayItemList(YearMonthDcno){
            Ext.Ajax.request({
                url: "Ajax/AdjustStaffPayAjax.aspx?param=StaffPayItemListColumnInfo",
                method: "POST",
                success: function(p1, p2) {
                    var de1 = Ext.decode(p1.responseText);
                    datacol_payItemList = new MakeDataColumn_payItemList();
                    for (var j = 0; j < de1.length; j++) {
                        for (var q in de1[j]) {
                            datacol_payItemList.addColumns(q, de1[j][q]);
                        }
                    }
                    if (Ext.getCmp("payItemListGridPanel")) { Ext.getCmp("formMain").remove(Ext.getCmp("payItemListGridPanel")); }
                    var grid_payItemList = new MakeGridView_payItemList(YearMonthDcno).GridView; 
                    Ext.getCmp("formMain").add(grid_payItemList);
                    Ext.getCmp("formMain").doLayout();
                    
                 }
             })
        }
        
        hideOpWaittingDiv(); 
    </script>

</form>
</body>
</html>
