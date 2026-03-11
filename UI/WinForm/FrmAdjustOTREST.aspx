<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmAdjustOTREST.aspx.cs" Inherits="WinForm_FrmAdjustOTREST" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<title>员工加班调休调整</title>
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
<asp:HiddenField ID="hfUserId" runat ="server" />
<asp:HiddenField ID="hfLanguage" runat ="server" />
<asp:HiddenField ID="hfCurYearMonth" runat ="server" />
<asp:HiddenField ID="hfUserType" runat ="server" />
<asp:HiddenField ID="hfIsLock" runat ="server" />
<asp:HiddenField ID="hfAnalysStatus" runat ="server" />
    
<asp:HiddenField ID="hfListNormal" runat ="server" />
<asp:HiddenField ID="hfListUnNormal" runat ="server" />
<asp:HiddenField ID="hfTitle" runat ="server" />
<asp:HiddenField ID="hfBtnSave" runat ="server" />
<asp:HiddenField ID="hfTipGridColumnsText" runat ="server" />
<asp:HiddenField ID="hfTipGridSortAscText" runat ="server" />
<asp:HiddenField ID="hfTipMsg" runat ="server" />
<asp:HiddenField ID="hfTipSaveMsg" runat ="server" />
<asp:HiddenField ID="hfTipNoSave" runat ="server" />
<asp:HiddenField ID="hfTipGridSortDescText" runat ="server" />

<asp:HiddenField ID="hfTipCurPeriod" runat ="server" />
<asp:HiddenField ID="hfTipTitleTip" runat ="server" />
<asp:HiddenField ID="hfTipFilterNameC" runat ="server" />
<asp:HiddenField ID="hfTipFilterNameE" runat ="server" />
<asp:HiddenField ID="hfTipFilterStaff" runat ="server" />
<asp:HiddenField ID="hfTipPagingShowMsg" runat ="server" />
<asp:HiddenField ID="hfTipPagingNoRecord" runat ="server" />
<asp:HiddenField ID="hfTipOTValidtorMsg" runat ="server" />
<asp:HiddenField ID="hfTipTXValidtorMsg" runat ="server" />

<asp:HiddenField ID="hfIsCanEditTx" runat ="server" />
<asp:HiddenField ID="hfIsCanEditJb" runat ="server" />

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
        var varCurUserId = document.getElementById("hfUserId").value;
        var varCurYearMonth = document.getElementById("hfCurYearMonth").value;
        
        
        //为获取动态生成员工列表的列而设置的变量
        var datacol_staffList=null;
        //为获取员工列表表格数据的store（当前页）
        var StoreGrid_staffList=null;
        //员工列表表格的ColumnModel
        var cmStaffList = null;
        //为获取员工列表表格所有数据的store
        var StoreGrid_staffList_All=null;
        
        //为获取动态生成加班调休调整日期的列而设置的变量
        var datacol_DateItemList=null;
        //为获取加班调休调整日期表格数据的store
        var StoreGrid_DateItemList=null;
        
        //员工列表每页显示记录数
        var iPageSizeNum = 25;
        
        //当前选择的员工编号
        var varCurStaffNo = "";
        
        var IsCanEditTx = document.getElementById("hfIsCanEditTx").value;
        var IsCanEditJb = document.getElementById("hfIsCanEditJb").value;
        
        var varUserType = document.getElementById("hfUserType").value;
        var varIsLock = document.getElementById("hfIsLock").value;
        var varAnalysStatus = document.getElementById("hfAnalysStatus").value;
    </script>  
    
    <%--为员工考勤结果中的combox数据的准备--Grid--%>
    <script type="text/javascript">
        Ext.data.status = [
        //            ['0','正常'],//正常
            //['1', '调整为正常'], //调整为正常
            //['2', '异常'] //异常
            ['1', document.getElementById("hfListNormal").value], //调整为正常
            ['2', document.getElementById("hfListUnNormal").value] //异常
        ];
        var statusDS = new Ext.data.SimpleStore({ //通过字典表获得用户使用状态数据源
            fields: ['code', 'value'],
            data: Ext.data.status //这里对应我在字典表里定义的类型名称
        });
                
    </script>

    <%--加载所有加班调休调整员工列表信息数据--Grid--%>
    <script type="text/javascript">
        function LoadAllStaffList(strFilterSql){
            strFilterSql = encodeURI(strFilterSql);
            var fields = eval('([' + datacol_staffList.fields + '])');
            StoreGrid_staffList_All = new Ext.data.Store
            ({
                proxy: new Ext.data.HttpProxy({ url: "Ajax/AdjustStaffOtRestAjax.aspx?param=FilterStaffListDataInfo&userId="+varCurUserId+"&yearMonth="+varCurYearMonth+"&filterSql="+strFilterSql}),
                reader: new Ext.data.JsonReader({ totalProperty: "AllStaffListTotalPorperty", root: "AllStaffList", fields: fields }),
                storeId:"STAFFID"
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
                renderTo:"div1",
                layout:"column",
                title: '【'+document.getElementById("hfTipCurPeriod").value+'：'+varCurYearMonth+'】'+document.getElementById("hfTipTitleTip").value,
                autoWidth:false,
                autoScroll:true,
                buttonAlign:"right",
                loader:loadAdjustStaffList(),
                tbar: [
                       new Ext.form.TextField({
                            id:"txtStaffNoFilter",
                            emptyText:document.getElementById("hfTipFilterStaff").value,
                            width:"150",
                            enableKeyEvents: true,
                            listeners:{
                                keyup:function(e){
                                    LoadAllStaffList("STAFFID like '"+Ext.getCmp("txtStaffNoFilter").getValue()+"%'");
                                } 
                            }
                       }),
                       new Ext.Button
                            ({id: "btnTemp1",
                            width:"10",
                            text: "   "
                            }),
                       new Ext.form.TextField({
                            id:"txtStaffEnNameFilter",
                            emptyText:document.getElementById("hfTipFilterNameE").value,
                            width:"150",
                            enableKeyEvents: true,
                            listeners:{
                                keyup:function(e){
                                    LoadAllStaffList("DCNAME like '"+Ext.getCmp("txtStaffEnNameFilter").getValue()+"%'");
                                } 
                            }
                       }),
                       new Ext.form.TextField({
                            id:"txtStaffCnNameFilter",
                            emptyText:document.getElementById("hfTipFilterNameC").value,
                            width:"150",
                            enableKeyEvents: true,
                            listeners:{
                                keyup:function(e){
                                    LoadAllStaffList("DCNAMECHS like '%"+Ext.getCmp("txtStaffCnNameFilter").getValue()+"%'");
                                } 
                            }
                       })                        
                    ]
            });
            
        }  
        Ext.onReady(ready);
    </script>
    
    <%--加班调休调整员工列表信息数据--Grid--%>
    <script type="text/javascript">
        function MakeDataColumn_staffList() {//加载加班调休调整员工列表信息数据列
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
                this.columns += '{header:"' + caption + '",dataIndex:"' + name + '",width:10,sortable:true,hidden:false}';
                 
            }
        }
        
        function MakeGridView_staffList() {//加载加班调休调整员工列表信息数据
            cmStaffList = new Ext.grid.ColumnModel(eval('([' + datacol_staffList.columns + '])'));
            cmStaffList.defaultSortable = true;
            var fields = eval('([' + datacol_staffList.fields + '])');
            StoreGrid_staffList = new Ext.data.Store
            ({
                proxy: new Ext.data.HttpProxy({ url: "Ajax/AdjustStaffOtRestAjax.aspx?param=MultiPageStaffListDataInfo&userId="+varCurUserId+"&yearMonth="+varCurYearMonth }),
                reader: new Ext.data.JsonReader({ totalProperty: "StaffListTotalPorperty", root: "StaffList", fields: fields }),
                storeId:"STAFFID"
            });
            StoreGrid_staffList.removeAll();
            StoreGrid_staffList.load({params:{start:0,limit:iPageSizeNum}});
            var gridPanel_staffList = new Ext.grid.EditorGridPanel
            ({
                cm: cmStaffList,
                id: "StaffListGridPanel",
                columnWidth:.3, 
                store: StoreGrid_staffList,
                frame: false,
                border: true,
                height: 580,
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
                    displayMsg: document.getElementById("hfTipPagingShowMsg").value,
                    emptyMsg: document.getElementById("hfTipPagingNoRecord").value
                })
            });
            this.GridView = gridPanel_staffList;
            
            gridPanel_staffList.addListener('rowclick', showStaffDateItemList);  
        }    
        
        //选择某员工显示其对应的加班调休调整项目
        var showStaffDateItemList = function(grid, rowIndex, e){ 
            showOpWaittingDiv();
            var selectionModel = grid.getSelectionModel();
            selectionModel.selectRow(rowIndex);    
            var record = selectionModel.getSelected();
            var varSHCode = record.data['STAFFID'];
            varCurStaffNo = varSHCode;
            loadStaffDateItemList(varSHCode);
            hideOpWaittingDiv(); 
        }
        
        //加载加班调休调整员工列表信息
        function loadAdjustStaffList(){
            Ext.Ajax.request({
                url: "Ajax/AdjustStaffOtRestAjax.aspx?param=AdjustStaffListColumnInfo&userId="+varCurUserId+"&yearMonth="+varCurYearMonth,
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
    
        
    <%--加班调休调整列表信息数据--Grid--%>
    <script type="text/javascript">
        function MakeDataColumn_DateItemList() {//加载加班调休调整薪资项目列表信息数据列
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
                if (name == 'em_No') {//员工编号
                    this.columns += '{header:"' + caption + '",dataIndex:"' + name + '",width:8,sortable:true,hidden:false}';
                } else if (name == 'DNUM') {//对应天数
                    this.columns += '{header:"' + caption + '",dataIndex:"' + name + '",width:5,sortable:true,hidden:true}';
                } else if (name == 'DATE') {//日期
                    this.columns += '{header:"' + caption + '",dataIndex:"' + name + '",width:8,sortable:true,hidden:false}';
                } else if (name == 'Shift') {//班次
                    this.columns += '{header:"' + caption + '",dataIndex:"' + name + '",width:5,sortable:true,hidden:false}';
                } else if (name == 'SJBHOUR') {//加班小时数，可编辑值
                    if((IsCanEditJb==0)||(varIsLock==1)||(varAnalysStatus==3)||((varUserType!=1)&&(varAnalysStatus!=1))){
                        this.columns += '{header:"' + caption + '",dataIndex:"' + name + '",width:5,sortable:true,hidden:false}';
                    } else {
                        this.columns += '{header:"' + caption + '",dataIndex:"' + name + '",width:5,sortable:true,hidden:false,';
                        this.columns += 'editor:new Ext.form.TextField({maxLength:9  })}';
                    }

                } else if (name == 'STXHOUR') {//调休小时数，可编辑值
                    if((IsCanEditTx==0)||(varIsLock==1)||(varAnalysStatus==3)||((varUserType!=1)&&(varAnalysStatus!=1))){
                        this.columns += '{header:"' + caption + '",dataIndex:"' + name + '",width:5,sortable:true,hidden:false}';
                    } else {
                        this.columns += '{header:"' + caption + '",dataIndex:"' + name + '",width:5,sortable:true,hidden:false,';
                        this.columns += 'editor:new Ext.form.TextField({maxLength:9  })}';
                    }
                } else if (name == 'BISTONORMAL') {//是否调整为正常，可编辑值
                    if ((varIsLock == 1) || (varAnalysStatus == 3) || ((varUserType != 1) && (varAnalysStatus != 1))) {
                        this.columns += '{header:"' + caption + '",dataIndex:"' + name + '",width:8,sortable:true,hidden:false}';
                    } else {
                        this.columns += '{header:"' + caption + '",dataIndex:"' + name + '",width:8,sortable:true,hidden:false,';
                        this.columns += 'editor:new Ext.form.ComboBox({store: statusDS,valueField :"code",displayField: "value",hiddenName:"value",mode: "local",editable:false,triggerAction:"all"}) }';
                    }
                } else if (name == 'ExceptionChs') {//异常类型描述
                    this.columns += '{header:"' + caption + '",dataIndex:"' + name + '",width:15,sortable:true,hidden:false}';
                } else if (name == 'SREMARK') {//备注可编辑
                    if((varIsLock==1)||(varAnalysStatus==3)||((varUserType!=1)&&(varAnalysStatus!=1))){
                        this.columns += '{header:"' + caption + '",dataIndex:"' + name + '",width:20,sortable:true,hidden:false}';
                    }else{
                        this.columns += '{header:"' + caption + '",dataIndex:"' + name + '",width:20,sortable:true,hidden:false,';
                        this.columns += 'editor:new Ext.form.TextArea({maxLength:300  })}';
                    }
                }

            }
        }
        
        function MakeGridView_DateItemList(StaffNo) {//加载加班调休调整薪资项目列表信息数据
            var cm = new Ext.grid.ColumnModel(eval('([' + datacol_DateItemList.columns + '])'));
            cm.defaultSortable = true;
            var fields = eval('([' + datacol_DateItemList.fields + '])');
            StoreGrid_DateItemList = new Ext.data.Store
            ({
                proxy: new Ext.data.HttpProxy({ url: "Ajax/AdjustStaffOtRestAjax.aspx?param=StaffDateItemListDataInfo&userId="+varCurUserId+"&yearMonth="+varCurYearMonth+"&staffNo="+StaffNo }),
                reader: new Ext.data.JsonReader({ totalProperty: "StaffDateItemListTotalPorperty", root: "StaffDateItemList", fields: fields }),
                storeId:"DATE"
            });
            StoreGrid_DateItemList.removeAll();
            StoreGrid_DateItemList.load({});
            var gridPanel_DateItemList = new Ext.grid.EditorGridPanel
            ({
                cm: cm,
                id: "dateItemListGridPanel",
                columnWidth:.7, 
                store: StoreGrid_DateItemList,
                frame: false,
                border: true,
                height: 580,
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
            });
            this.GridView = gridPanel_DateItemList;
            
            
            gridPanel_DateItemList.on('afteredit', afterEdit, this ); 
        }    
        
        function afterEdit(e) {  
            SaveAdjust(e);
        };

        //加载加班调休调整薪资项目列表信息
        function loadStaffDateItemList(StaffNo){
            Ext.Ajax.request({
                url: "Ajax/AdjustStaffOtRestAjax.aspx?param=StaffDateItemListColumnInfo&userId="+varCurUserId+"&yearMonth="+varCurYearMonth+"&staffNo="+StaffNo,
                method: "POST",
                success: function(p1, p2) {
                    var de1 = Ext.decode(p1.responseText);
                    datacol_DateItemList = new MakeDataColumn_DateItemList();
                    for (var j = 0; j < de1.length; j++) {
                        for (var q in de1[j]) {
                            datacol_DateItemList.addColumns(q, de1[j][q]);
                        }
                    }
                    if (Ext.getCmp("dateItemListGridPanel")) { Ext.getCmp("formMain").remove(Ext.getCmp("dateItemListGridPanel")); }
                    var grid_DateItemList = new MakeGridView_DateItemList(StaffNo).GridView; 
                    Ext.getCmp("formMain").add(grid_DateItemList);
                    Ext.getCmp("formMain").doLayout();
                    
                 }
             })
        }
        
        hideOpWaittingDiv(); 
    </script>
    
    <%--即时保存加班调休调整员工数据按钮（单条记录保存）--Grid--%>
    <script type="text/javascript">
        function SaveAdjust(e){
            if(StoreGrid_DateItemList==null){
                return;
            }
            if(Ext.getCmp("dateItemListGridPanel").activeEditor!=null){
                Ext.getCmp("dateItemListGridPanel").activeEditor.completeEdit(); 
            }

            var recordArray = StoreGrid_DateItemList.getModifiedRecords();
            var varAdjustResult = "";
            for(var i=0;i< recordArray.length;i++){
                var strStaffNo = varCurStaffNo;
                var strDate = recordArray[i].get("DATE");
                
                items = recordArray[i].getChanges();
                for(var key in items){
                    if(key=="SJBHOUR"){
                        if(!isPositiveNumber(items[key])){
                            Ext.Msg.alert(document.getElementById("hfTipMsg").value, document.getElementById("hfTipOTValidtorMsg").value);
                        }else{
                            varAdjustResult += "SJBHOUR＠" +items[key].trim();
                        }
                    }else if(key=="STXHOUR"){
                        if(!isPositiveNumber(items[key])){
                            Ext.Msg.alert(document.getElementById("hfTipMsg").value, document.getElementById("hfTipTXValidtorMsg").value);
                        }else{
                            varAdjustResult += "STXHOUR＠" + items[key].trim();
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
                    url: "Ajax/AdjustStaffOtRestAjax.aspx?param=saveAdjustOTRESTResult&staffNo="+strStaffNo+"&date="+strDate+"&staffListModifyStr="+varAdjustResult,
                    method: "POST",
                    success: function(p1, p2) {
                        e.record.commit();  
                    },
                    failure: function(p1, p2) { }
                })
            }
        }
    </script>
    
</form>
</body>
</html>
