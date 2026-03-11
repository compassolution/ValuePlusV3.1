<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BoundTag.aspx.cs" Inherits="Asset_BoundTag" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>资产标签绑定页面</title>
<link href="../common/css/main.css" rel="stylesheet" type="text/css" />
<link href="../common/css/button.css" type="text/css" rel="stylesheet" /> 
<link href="../common/css/topStyle.css" rel="stylesheet"  type="text/css" rev="stylesheet" media="all" />
<link href="../common/css/fixAreaStyle.css" type="text/css" rel="stylesheet" /> 
<script  src="../common/js/waitProcess.js"></script>
<script  src="../common/js/tableStyle.js"></script>
</head>
<body>
<script type="text/javascript">
    var time1;
    function startTimer(){
        time1 = setInterval("clock()",1000);
    }
    function stopTimer(){
        clearInterval(time1);
    }
    startTimer();
    
    function clock() {
        var t = new Date()
        var responseObj = Asset_BoundTag.GetEpcIdFromMoblieCache();
        if ((responseObj.value != '') && (responseObj.value != document.getElementById('txtGetEPCID').value)) {
            document.getElementById('txtGetEPCID').value = responseObj.value;
            document.getElementById("txtGetEPCID").style.color = 'red';
            
        }
    }

    function test() {
        Asset_BoundTag.test();
    }
    
    function CheckIsNow(isChecked) {
        if (isChecked) {
            document.getElementById("hfIsNow").value = "1";
            this.trEPCID.style.display = "";
            this.divBtnBatch.style.display = "none";
            document.getElementById("CheckAll").style.display = "none";
            startTimer();
        } else {
            document.getElementById("hfIsNow").value = "0";
            this.trEPCID.style.display = "none";
            this.divBtnBatch.style.display = "";
            document.getElementById("CheckAll").style.display = "";
            stopTimer();
        }
    }


    //CheckBox全选And反全选
    function select_deselectAll(chkVal, idVal) {
        if (idVal.indexOf('CheckAll') != -1) {
            var oTb = document.getElementById('changecolor');

            var oSel = oTb.getElementsByTagName('input');
            for (i = 0; i < oSel.length; i++) {
                if (oSel[i].type == "checkbox" && oSel[i].id.indexOf("cBox") != -1) {
                    var checkB = oSel[i];
                    if (chkVal == true) {
                        checkB.checked = true;
                    }
                    else {
                        checkB.checked = false;
                    }
                }
            }
        }

    }

    //表格中的checkbox框单选
    function SetCheckBoxState() {
        if (document.getElementById("hfIsNow").value == '1') {
            var oTb = document.getElementById('changecolor');
            var dom = oTb.getElementsByTagName('input');
            //控制其他单选框取消选中
            var el = event.srcElement;
            if (el.tagName == "INPUT" && el.type.toLowerCase() == "checkbox") {
                for (i = 0; i < dom.length; i++) {
                    if (dom[i].tagName == "INPUT" && dom[i].type.toLowerCase() == "checkbox" && dom[i].id.indexOf("cBox") != -1) {
                        dom[i].checked = false;
                    }
                }
            }
            el.checked = !el.checked;

            var curCBoxID = el.id;
            curCBoxID = curCBoxID.replace('cBox', 'txtBarCode');
            var txtObject = document.getElementById(curCBoxID);
            var txtOldValue = txtObject.value;
            txtObject.value = document.getElementById('txtGetEPCID').value;
            txtObject.style.color = "red";

            if (confirm(document.getElementById('hfConfirmIsBinding').value)) {
                document.getElementById("btnSingleBound").click();
            } else {
                txtObject.value = txtOldValue;
                txtObject.style.color = "black";
            }
        }
        
    }
    
    function FilterEvent(varField) {
        document.getElementById("hfFilterField").value = varField;
        if (varField == '资产编码') {
      
            if (event.keyCode == 13 && document.all["txtFilterACode"].value != "") {
                event.keyCode = 9;
                event.returnValue = false;
                document.all["btnFilter"].click();
            }
        } else if (varField == '标签ID') {
            if (event.keyCode == 13) {
                event.keyCode = 9;
                event.returnValue = false;
                document.all["btnFilter"].click();
            }
        } else if (varField == '资产名称') {
            if (event.keyCode == 13) {
                event.keyCode = 9;
                event.returnValue = false;
                document.all["btnFilter"].click();
            }
        } else if (varField == '资产中文名') {
            if (event.keyCode == 13) {
                event.keyCode = 9;
                event.returnValue = false;
                document.all["btnFilter"].click();
            }
        } else if (varField == '资产型号') {
            if (event.keyCode == 13) {
                event.keyCode = 9;
                event.returnValue = false;
                document.all["btnFilter"].click();
            }
        } else if (varField == '使用部门') {
            if (event.keyCode == 13) {
                event.keyCode = 9;
                event.returnValue = false;
                document.all["btnFilter"].click();
            }
        } else if (varField == '存放地址') {
            if (event.keyCode == 13) {
                event.keyCode = 9;
                event.returnValue = false;
                document.all["btnFilter"].click();
            }
        }
    }

</script>

<form id="form1" runat="server">
<asp:HiddenField ID="hfIsNow" runat="server" />
<asp:HiddenField ID="hfFilterField"  runat="server"/>
<asp:HiddenField ID="hfConfirmIsBinding"  runat="server"/>
<script type="text/javascript">
    document.getElementById("hfIsNow").value = '1';
</script>
     <table width="100%" border="0" cellpadding="0" cellspacing="0" class="tableNoHover">
    
        <tr align="center" height = "30"  id = "trEPCID" style="display:" runat="server">
            <td align="left" valign="top" bgcolor="#F7F8F9" width = "35%">
                <asp:TextBox ID="txtGetEPCID" runat="server" Height="30" Width="98%" Font-Size="X-Large" ForeColor=Gray  ReadOnly="true" Text="等待手持设备准备中...."></asp:TextBox>
            </td>
            <td align="left" valign="top" bgcolor="#F7F8F9">
                <asp:Label ID="lbNowTip1" runat="server" Text="提示：“即时获取手持设备”选项可将手持读写器在写入标签时向本页面传递对应标签的编码，同时显示在左边的文本框中" CssClass="tableTitle"></asp:Label> 
                <br>
                <asp:Label ID="lbNowTip2" runat="server" Text="操作：点击第一列的复选框进行绑定操作！" CssClass="tableTitle"></asp:Label> 
            </td>
        </tr>
        
          <tr id = "trCountArea" align="center" runat="server" bgcolor="#F7F8F9">
            <td align="left" valign="top" bgcolor="#F7F8F9">
                <asp:CheckBox ID="cbIsNow" runat="server" Checked="true" onclick = "CheckIsNow(this.checked);" Text="即时获取手持设备"/>
                <asp:LinkButton ID="LinkButton1" runat="server" CssClass="a_Left" Font-Bold="true" OnClientClick = "test();return false;">test</asp:LinkButton>
                <asp:LinkButton ID="btnClose" runat="server" CssClass="a_Left" Font-Bold="true" OnClientClick = "javascript:window.close();">关闭</asp:LinkButton>
                <asp:CheckBox ID="cbIsMulti" runat="server" Checked="true" onclick = "CheckIsNow(this.checked);" Text="批量绑定"/>
                <span id="divBtnBatch" style="display:none" runat="server">
                    <asp:LinkButton ID="btnBatchBound" runat="server" CssClass="a_Left" Font-Bold="true" OnClick="BatchBoundClick">批量保存</asp:LinkButton>
                </span>
                <div id="divSingleSave" style="display:none" runat="server">
                    <asp:LinkButton ID="btnSingleBound" runat="server" CssClass="a_Left" Font-Bold="true" OnClick="SingleBoundClick">单个保存</asp:LinkButton>
                </div>
                <div id="Span1" style="display:none">
                    <asp:LinkButton ID="btnFilter" runat="server" CssClass="a_Left" Font-Bold="true" OnClick = "FilterClick">点击过滤</asp:LinkButton>
                </div>
                
            </td>
            <td align ="right">
                <asp:Label ID="lbCountLabel" runat="server" Text="本次查询结果数为：" CssClass="left_ts"></asp:Label> 
                <asp:Label ID="lbCount" runat="server" Text="0" CssClass="left_ts"></asp:Label> 
                <asp:Label ID="lbMaxCount" runat="server" Text="(仅显示前20条)" CssClass="left_ts" Visible=false></asp:Label> 
            </td>
          </tr>
          <!-- 操作行 -->
          <tr height="35" align="left">
            <td align="center" valign="top" bgcolor="#F7F8F9" colspan = "2">
                <table id="Table1" width="100%">
                    <tr width="100%">
                        <TD align ="center" valign="middle" Width="5%">
                            <asp:Label ID="Label2" runat="server" Width="100%" Text="Filter"></asp:Label>                        
                        </TD>
                        <TD align ="center" valign="middle" Width="10%">
                            <asp:TextBox ID="txtFilterACode" runat="server" Width="95%" ToolTip = "通过资产编号过滤"></asp:TextBox>	
                        </TD>
                        <TD align ="center" valign="middle" Width="15%">
                            <asp:TextBox ID="txtFilterBarCode" runat="server" Width="95%" ToolTip = "通过标签ID过滤"></asp:TextBox>		
                        </TD>
                        <TD align ="center" valign="middle" Width="20%" style = "display:none">
                            <asp:TextBox ID="txtFilterName" runat="server" Width="95%" ToolTip = "通过资产英文名称过滤"></asp:TextBox>		
                        </TD>
                        <TD align ="center" valign="middle" Width="20%">
                            <asp:TextBox ID="txtFilterNameChs" runat="server" Width="95%" ToolTip = "通过资产中文名称过滤"></asp:TextBox>		
                        </TD>
                        <TD align ="center" valign="middle" Width="15%">
                            <asp:TextBox ID="txtFilterModel" runat="server" Width="95%" ToolTip = "通过资产型号过滤"></asp:TextBox>		
                        </TD>
                        <TD align ="center" valign="middle" Width="10%">
                            <asp:TextBox ID="txtFilterDept" runat="server" Width="95%" ToolTip = "通过使用部门过滤"></asp:TextBox>		
                        </TD>
                        <TD align ="center" valign="middle" Width="10%">
                            <asp:TextBox ID="txtFilterLocationChs" runat="server" Width="95%" ToolTip = "通过存放地址过滤"></asp:TextBox>		
                        </TD>
                        <TD align ="center" valign="middle" Width="20%">
                            <asp:TextBox ID="txtFilterRemark" runat="server" Width="95%" ToolTip = "通过备注过滤"></asp:TextBox>		
                        </TD>
                    </tr>
                 </table>
             </td>
          </tr>
          <!-- 结果列表部分 --> 
          <tr>
            <td align="center" valign="top" bgcolor="#F7F8F9" colspan = "2">
                <table id="changecolor" width="100%">
                    <tr width="100%">
                        <td style ="width:98%; white-space:nowrap">
                            <asp:DataGrid ID="DataGrid1" runat="server" AllowSorting="true"  Width="100%" DataKeyField="资产编码"  OnItemDataBound="DataGrid1_ItemDataBound"
                                AutoGenerateColumns="false"  onHorizontalAlign="Center" onsortcommand="DataGrid1_SortCommand">
                                <HeaderStyle CssClass="tableTitle"></HeaderStyle>
					            <ItemStyle CssClass="tableContent"  />
					            <Columns>
						            <asp:TemplateColumn HeaderText="<input   type='checkbox'   id='CheckAll' style='display:none'  onclick='return select_deselectAll(this.checked, this.id);'>">
							            <HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle> 
						                <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
							            <ItemTemplate>
                                            <asp:CheckBox ID="cBox" runat="server"/>								            
							            </ItemTemplate>
						            </asp:TemplateColumn>
						            <asp:BoundColumn DataField="资产编码" SortExpression="资产编码"  HeaderText="资产编码">
							            <HeaderStyle HorizontalAlign ="Center" Width="10%"></HeaderStyle> 
						                <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
						            </asp:BoundColumn>
						            <asp:TemplateColumn SortExpression="标签ID" HeaderText="标签ID">
							            <HeaderStyle HorizontalAlign="Center" Width="15%"></HeaderStyle> 
						                <ItemStyle HorizontalAlign="Center" ></ItemStyle>
							            <ItemTemplate>
                                            <asp:TextBox ID="txtBarCode" runat="server" Width="95%"></asp:TextBox>									            
							            </ItemTemplate>
						            </asp:TemplateColumn>
						            <asp:BoundColumn DataField="标签ID" SortExpression="标签ID" HeaderText="标签ID" Visible="false">
							            <HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle> 
						                <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
						            </asp:BoundColumn>
						            <asp:BoundColumn DataField="资产名称" SortExpression="资产名称" HeaderText="资产英文名" Visible =false >
							            <HeaderStyle HorizontalAlign="Center" Width="20%"></HeaderStyle> 
						                <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
						            </asp:BoundColumn>
						            <asp:BoundColumn DataField="资产中文名" SortExpression="资产中文名" HeaderText="资产中文名">
							            <HeaderStyle HorizontalAlign="Center" Width="20%"></HeaderStyle> 
						                <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
						            </asp:BoundColumn>
						            <asp:BoundColumn DataField="资产型号" SortExpression="资产型号" HeaderText="资产DD型号">
							            <HeaderStyle HorizontalAlign="Center" Width="15%"></HeaderStyle> 
						                <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
						            </asp:BoundColumn>
						            <asp:BoundColumn DataField="使用部门" SortExpression="使用部门" HeaderText="使用部门">
							            <HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle> 
						                <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
						            </asp:BoundColumn>
						            <asp:BoundColumn DataField="存放地址" SortExpression="存放地址" HeaderText="存放地址">
							            <HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle> 
						                <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
						            </asp:BoundColumn>
						            <asp:BoundColumn DataField="备注" SortExpression="备注" HeaderText="备注">
							            <HeaderStyle HorizontalAlign="Center" Width="20%"></HeaderStyle> 
						                <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
						            </asp:BoundColumn>
					            </Columns>
                            </asp:DataGrid>
                        </td>
                    </tr>
                 </table>
             </td>
          </tr>	    
    </table>
</form>
</body>
</html>

<script language="javascript">
    //初始化结果表格
    DefineTableCssNoCursorOver("changecolor");
</script>
