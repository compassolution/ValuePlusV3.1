<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmAdjustResult.aspx.cs" Inherits="WinForm_FrmAdjustResult" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<title>员工异常考勤信息调整</title>
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

<asp:HiddenField ID="hfTipGridColumnsText" runat ="server" />
<asp:HiddenField ID="hfTipGridSortAscText" runat ="server" />
<asp:HiddenField ID="hfTipMsg" runat ="server" />
<asp:HiddenField ID="hfTipSaveMsg" runat ="server" />
<asp:HiddenField ID="hfTipNoSave" runat ="server" />
<asp:HiddenField ID="hfTipGridSortDescText" runat ="server" />

<asp:HiddenField ID="hfIsCanEditTx" runat ="server" />
<asp:HiddenField ID="hfIsCanEditJb" runat ="server" />
<asp:HiddenField ID="hfIsCanEditIsToNormal" runat ="server" />

<asp:HiddenField ID="hfListNormal" runat ="server" />
<asp:HiddenField ID="hfListUnNormal" runat ="server" />

<asp:HiddenField ID="hfLabelYouCanAdjust" runat ="server" />
<asp:HiddenField ID="hfLabelUnNormalInfo" runat ="server" />
<asp:HiddenField ID="hfLabelToNormalInfo" runat ="server" />
<asp:HiddenField ID="hfBtnSaveAdjust" runat ="server" />
<asp:HiddenField ID="hfLVOVMustInt" runat ="server" />
<asp:HiddenField ID="hfBtnClose" runat ="server" />
<asp:HiddenField ID="hfTipPagingShowMsg" runat ="server" />
<asp:HiddenField ID="hfTipPagingNoRecord" runat ="server" />
    <div id="div1" style="width:100%">
    
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
        //为获取动态生成的列而设置的变量
        var datacol_Result=null;
        //为获取当前表格数据的store
        var StoreGrid_Result=null;
        //异常员工考勤结果表格的列数
        var colCount_Result=0;
        //异常员工考勤结果表格的列数
        var flagToNormal=2;
        
        var curYearMonth = document.getElementById("hfCurYearMonth").value; 
        var IsCanEditTx = document.getElementById("hfIsCanEditTx").value; 
        var IsCanEditJb = document.getElementById("hfIsCanEditJb").value;
        var IsCanEditIsToNormal = document.getElementById("hfIsCanEditIsToNormal").value;
        var varUserType = document.getElementById("hfUserType").value;
        var varIsLock = document.getElementById("hfIsLock").value;
        var varAnalysStatus = document.getElementById("hfAnalysStatus").value;
        
    </script>  
    
    <%--为异常员工考勤结果中的combox数据的准备--Grid--%>
    <script type="text/javascript">
        Ext.data.status = [
//            ['0','正常'],
//            ['1','调整为正常'],
//            ['2', '异常']
            ['1', document.getElementById("hfListNormal").value], //调整为正常
            ['2', document.getElementById("hfListUnNormal").value] //异常
        ];
        var statusDS = new Ext.data.SimpleStore({ //通过字典表获得用户使用状态数据源
            fields: ['code', 'value'],
            data:Ext.data.status //这里对应我在字典表里定义的类型名称
        });
    </script>
    
    <%--ExtJS页面加载及渲染--Grid--%>
    <script type="text/javascript">
        function ready() {
            var formOne = new Ext.Panel({
                id: "formOne",
                renderTo:"div1",
                layout:"form",
                region: 'center', 
                title: document.getElementById("hfLabelYouCanAdjust").value ,
                autoScroll:true,
                loader:loadUnNormalResult(),
                tbar: [
                        new Ext.form.RadioGroup({
                            name:"rdGIsToNormal",
                            width:"300",
                            items:[
                                {
                                    id:'rdUnNormal',
                                    boxLabel:document.getElementById("hfLabelUnNormalInfo").value,
                                    inputValue:'2',
                                    name:'flagUnNormal',
                                    checked:true,
                                    handler:function(){
                                        if(Ext.getCmp("rdUnNormal").checked){
                                            showOpWaittingDiv();
                                            flagToNormal = 2;
                                            loadUnNormalResult();
                                            hideOpWaittingDiv(); 
                                        }
                                    }
                                }, 
                                {
                                    id:'rdToNormal',
                                    boxLabel:document.getElementById("hfLabelToNormalInfo").value,
                                    width:150,
                                    inputValue:'1',
                                    name:'flagUnNormal',
                                    handler:function(){
                                        if(Ext.getCmp("rdToNormal").checked){
                                            showOpWaittingDiv(); 
                                            flagToNormal = 1;
                                            loadUnNormalResult();
                                            hideOpWaittingDiv(); 
                                        }
                                    }
                                } 
                            ]

                        }),
                       new Ext.Button
                            ({id: "btnSaveResult",
                            text: document.getElementById("hfBtnSaveAdjust").value,
                            width:"100",
                            style: "background:#9F9",
                            hidden: "true", //add  by sammen 20120618 即时保存时，批量保存暂时屏蔽
                            handler:function(){
                                showOpWaittingDiv(); 
                                if(StoreGrid_Result==null){
                                    hideOpWaittingDiv(); 
                                    return;
                                }
                                if(Ext.getCmp("StuffResultGridPanel").activeEditor!=null){
                                    Ext.getCmp("StuffResultGridPanel").activeEditor.completeEdit(); 
                                }

                                var results={};
                                var recordArray = StoreGrid_Result.getModifiedRecords();
                                var recordOld;
                                var resultStr = '';
                                for(var i=0;i< recordArray.length;i++){
                                    var strSEQNO = recordArray[i].get("SEQNO");
                                    var strRDate = getDateStr1(recordArray[i].get("r_Date"));
                                    var resultStr1 = '';
                                    
　　                                items = recordArray[i].getChanges();
　　                                for(var key in items){
　　                                    if(items[key]==""){
　　                                        resultStr1 += "^" + key+'*'+'space';
　　                                    }else{
                                            if((key=="STXHOUR")&&(!isPositiveNumber(items[key]))){
                                                Ext.Msg.alert(document.getElementById("hfTipMsg").value, document.getElementById("hfLVOVMustInt").value);
                                                hideOpWaittingDiv(); 
                                                return;
                                            }
                                            if((key=="SJBHOUR")&&(!isPositiveNumber(items[key]))){
                                                Ext.Msg.alert(document.getElementById("hfTipMsg").value, document.getElementById("hfLVOVMustInt").value);
                                                hideOpWaittingDiv(); 
                                                return;
                                            }
　　　　                                    resultStr1 += "^" + key+'*'+items[key].trim().replace('!','').replace('#','').replace('$','').replace('^','').replace('*','');
　　　　                                }
　　                                }
　　　　                            resultStr1 = resultStr1.substring(1,resultStr1.length);
　　                                resultStr += "!" + strSEQNO+'#'+strRDate+"$" +resultStr1;
　　                                resultStr = encodeURI(resultStr);
　　　　                            
　　                                //****拼写字符串的含义*****
　　                                //!   ：记录与记录之间间隔符
　　                                //#   ：员工编号与排班月份间隔符
　　                                //$   ：一条记录key与其修改所有对象的间隔符
　　                                //^   ：同一条记录的多个修改对象之间的间隔符
　　                                //*   ：同一条记录的同一个修改对象中，修改字段及修改后值之间的间隔符
　　                                //****拼写字符串的含义*****
                                };
                                resultStr = resultStr.substring(1,resultStr.length);
//                                alert(resultStr);
                                if (resultStr!=""){
                                    curYearMonth = document.getElementById("hfCurYearMonth").value;
                                    Ext.Ajax.request({
                                        url: "Ajax/AdjustResultAjax.aspx?param=saveUnNormalResultAdjustInfo&resultModifyStr=" + resultStr + "&userId=" + document.getElementById("hfUserId").value,
                                        params: { results: results },
                                        method: "POST",
                                        success: function(p1, p2) {
//                                            var de = Ext.decode(p1.responseText);
                                            Ext.Msg.alert(document.getElementById("hfTipMsg").value, document.getElementById("hfTipSaveMsg").value);
                                            //重新加载
                                            if (Ext.getCmp("StuffResultGridPanel")) { Ext.getCmp("formOne").remove(Ext.getCmp("StuffResultGridPanel")); }
                                            var grid = new MakeGridView_Result().GridView; 
                                            Ext.getCmp("formOne").add(grid);
                                            
                                            Ext.getCmp("formOne").doLayout();
                                            hideOpWaittingDiv(); 
                                        },
                                        failure: function(p1, p2) { }
                                    })
                                }else{
                                    Ext.Msg.alert(document.getElementById("hfTipMsg").value, document.getElementById("hfTipNoSave").value);
                                    hideOpWaittingDiv(); 
                                }
                                
                            }
                            })
                        
                        ]
            });
            
        }  
        Ext.onReady(ready);
    </script>
    
    <%--员工异常考勤信息数据--Grid--%>
    <script type="text/javascript">
        function MakeDataColumn_Result() {//加载员工异常考勤信息数据列
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
                if((name=='EM_NO')||(name=='Ymonth')){
                    this.columns += '{header:"' + caption + '",dataIndex:"' + name + '",width:8,sortable:true,hidden:false}';
                } else if ((name == 'OverTime') || (name == 'LVhrs')) {
                    this.columns += '{header:"' + caption + '",dataIndex:"' + name + '",width:10,sortable:true,hidden:false}';
                } else if ((name == 'FactIn') || (name == 'FactOut') || (name == 'FactIn1') || (name == 'FactOut1')) {
                    this.columns += '{header:"' + caption + '",dataIndex:"' + name + '",width:15,sortable:true,hidden:false}';
                } else if ((name == 'DCNAMEC') || (name == 'EM_NO') || (name == 'r_Date')) {
                    if(document.getElementById("hfLanguage").value=="zh-cn"){//如果是中文环境则显示中文名
                        this.columns += '{header:"' + caption + '",dataIndex:"' + name + '",width:12,sortable:true}';
                    }else{
                        this.columns += '{header:"' + caption + '",dataIndex:"' + name + '",width:12,sortable:true,hidden:true}';
                    }
                } else if ((name == 'DCNAMEE') || (name == 'EM_NO') || (name == 'r_Date')) {
                    if(document.getElementById("hfLanguage").value=="zh-cn"){//如果是英文环境则显示英文名
                        this.columns += '{header:"' + caption + '",dataIndex:"' + name + '",width:10,sortable:true,hidden:true}';
                    }else{
                        this.columns += '{header:"' + caption + '",dataIndex:"' + name + '",width:10,sortable:true}';
                    }
                }else if(name=='STXHOUR'){
                    if((IsCanEditTx==0)||(varIsLock==1)||(varAnalysStatus==3)||((varUserType!=1)&&(varAnalysStatus!=1))){
                        this.columns += '{header:"' + caption + '",dataIndex:"' + name + '",width:10,sortable:true,hidden:false,css:"background: #FFCCFF;"}';
                    }else{
                        this.columns += '{header:"' + caption + '",dataIndex:"' + name + '",width:10,sortable:true,hidden:false,css:"background: #FFCCFF;",editor:new Ext.form.TextField({maxLength:4})}';
                    }
                }else if(name=='SJBHOUR'){
                    if((IsCanEditJb==0)||(varIsLock==1)||(varAnalysStatus==3)||((varUserType!=1)&&(varAnalysStatus!=1))){
                        this.columns += '{header:"' + caption + '",dataIndex:"' + name + '",width:10,sortable:true,hidden:false,css:"background: #FFCCFF;"}';
                    }else{
                        this.columns += '{header:"' + caption + '",dataIndex:"' + name + '",width:10,sortable:true,hidden:false,css:"background: #FFCCFF;",editor:new Ext.form.TextField({maxLength:4})}';
                    }
                }else if(name=='SREMARK'){
                    if((varIsLock==1)||(varAnalysStatus==3)||((varUserType!=1)&&(varAnalysStatus!=1))){
                        this.columns += '{header:"' + caption + '",dataIndex:"' + name + '",width:30,sortable:true,hidden:false,css:"background: #FFCCFF;"}';
                    }else{
                        this.columns += '{header:"' + caption + '",dataIndex:"' + name + '",width:30,sortable:true,hidden:false,css:"background: #FFCCFF;",editor:new Ext.form.TextArea({maxLength:300})}';
                    }
                }else if(name=='BISTONORMAL'){
                    ///增加开关IsCanEditIsToNormal的逻辑add by sammen 20180105
                    ///未锁定或者非审核状态下，人事部审核能编辑，部门人员只有在开关IsCanEditIsToNormal=1时才能编辑 
                    if (((varUserType != 1) && (IsCanEditIsToNormal != 1)) || (varIsLock == 1) || (varAnalysStatus == 3) || ((varUserType != 1) && (varAnalysStatus != 1))) {
                        this.columns += '{header:"' + caption + '",dataIndex:"' + name + '",width:10,sortable:true,hidden:false,css:"background: #FFCCFF;"}';
                    }else{
                        this.columns += '{header:"' + caption + '",dataIndex:"' + name + '",width:10,sortable:true,hidden:false,css:"background: #FFCCFF;",'
                                +'editor:new Ext.form.ComboBox({store: statusDS,valueField :"code",displayField: "value",hiddenName:"code",mode: "local",editable:false,triggerAction:"all"}) }';
                    }
                }else{
                    this.columns += '{header:"' + caption + '",dataIndex:"' + name + '",width:10,sortable:true,hidden:true,css:"background: #FFCCFF;"}';
                }
               
            }
        }
        
        function MakeGridView_Result() {//加载员工异常考勤结果信息数据
            var cm = new Ext.grid.ColumnModel(eval('([' + datacol_Result.columns + '])'));
            cm.defaultSortable = true;
            var fields = eval('([' + datacol_Result.fields + '])');
            curYearMonth = document.getElementById("hfCurYearMonth").value; 
//            alert(curYearMonth);
            StoreGrid_Result = new Ext.data.Store
            ({
                proxy: new Ext.data.HttpProxy({ url: "Ajax/AdjustResultAjax.aspx?param=UnNormalResultDataInfo&yearMonth=" + curYearMonth + "&toNormalFlag=" + flagToNormal + "&userId=" + document.getElementById("hfUserId").value }),
                reader: new Ext.data.JsonReader({ totalProperty: "KQStuffResultTotalPorperty", root: "KQStuffResult", fields: fields }),
                storeId:"SHCODE"
            });
            StoreGrid_Result.removeAll();
            StoreGrid_Result.load({params:{start:0,limit:10}});
            var gridPanel_Result = new Ext.grid.EditorGridPanel
            ({
                cm: cm,
                id: "StuffResultGridPanel",
                columnWidth:.9, 
                store: StoreGrid_Result,
                frame: false,
                border: true,
                region:'south',
                pageSize: 16,
                height: 500,
                stripeRows: true,
                clicksToEdit:1,
                viewConfig: { forceFit: true },
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
                    pageSize: 10,
                    store: StoreGrid_Result,
                    displayInfo: true,
                    displayMsg: document.getElementById("hfTipPagingShowMsg").value,
                    emptyMsg: document.getElementById("hfTipPagingNoRecord").value
                })
            });
            this.GridView = gridPanel_Result;
//            gridPanel_Result.render();
            //            gridPanel_Result.getView().refresh();

            gridPanel_Result.on('afteredit', afterEdit, this); //add  by sammen 20120618 即时保存
        }

        function afterEdit(e) {//add  by sammen 20120618 即时保存
            SaveAdjustRealTime(e);
        };
        
        //加载异常员工考勤结果调整信息
        function loadUnNormalResult(){
            Ext.Ajax.request({
                url: "Ajax/AdjustResultAjax.aspx?param=UnNormalResultColumnInfo" + "&userId=" + document.getElementById("hfUserId").value,
                method: "POST",
                success: function(p1, p2) {
                    var de1 = Ext.decode(p1.responseText);
                    datacol_Result = new MakeDataColumn_Result();
                    colCount_Result = de1.length;
                    for (var j = 0; j < de1.length; j++) {
                        for (var q in de1[j]) {
                            datacol_Result.addColumns(q, de1[j][q]);
                        }
                    }
                    if (Ext.getCmp("StuffResultGridPanel")) { Ext.getCmp("formOne").remove(Ext.getCmp("StuffResultGridPanel")); }
                    if (Ext.getCmp("btnSaveResult")) { Ext.getCmp("formOne").remove(Ext.getCmp("btnSaveResult")); }
                    var grid_Result = new MakeGridView_Result().GridView; 
//                    var btn_SaveResult = new MakeBtn_saveResult(Ext.getCmp("StuffResultGridPanel")).btnSaveResult;
                    
//                    Ext.getCmp("formOne").add(btn_SaveResult);
                    Ext.getCmp("formOne").add(grid_Result);
                    Ext.getCmp("formOne").doLayout();
                    
                 }
             })
        }
        
        hideOpWaittingDiv(); 
    </script>
    
    <%--即时保存异常考勤结果调整数据按钮（单条记录保存）--Grid--%>
    <script type="text/javascript">
        function SaveAdjustRealTime(e) {
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
                    if (key == "STXHOUR"){
                        if (!isPositiveNumber(items[key])) {
                            Ext.Msg.alert(document.getElementById("hfTipMsg").value, document.getElementById("hfLVOVMustInt").value);
                        } else {
                            varAdjustResult += "STXHOUR＠" + items[key].trim();
                        }
                    } else if (key == "SJBHOUR") {
                        if (!isPositiveNumber(items[key])) {
                            Ext.Msg.alert(document.getElementById("hfTipMsg").value, document.getElementById("hfLVOVMustInt").value);
                        }else{
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
                    url: "Ajax/AdjustResultAjax.aspx?param=saveAdjustResultRealTime&seqNo=" + strSeqNo + "&date=" + strDate + "&resultModifyStr=" + varAdjustResult + "&userId=" + document.getElementById("hfUserId").value,
                    method: "POST",
                    success: function(p1, p2) {
                        e.record.commit(); 
//                        //重新加载
//                        if (Ext.getCmp("StuffResultGridPanel")) { Ext.getCmp("formOne").remove(Ext.getCmp("StuffResultGridPanel")); }
//                        var grid = new MakeGridView_Result().GridView;
//                        Ext.getCmp("formOne").add(grid);

//                        Ext.getCmp("formOne").doLayout();
//                        hideOpWaittingDiv(); 
                    },
                    failure: function(p1, p2) { }
                })
            }
        }
    </script>
</form>
</body>
</html>
