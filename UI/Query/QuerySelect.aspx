<%@ Page Language="C#" AutoEventWireup="true" CodeFile="QuerySelect.aspx.cs" Inherits="Query_QuerySelect" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>列表选择页面</title>
<link href="../common/css/main.css" rel="stylesheet" type="text/css" />
<link href="../common/css/button.css" type="text/css" rel="stylesheet" /> 
<link href="../common/css/topStyle.css" rel="stylesheet"  type="text/css" rev="stylesheet" media="all" />
<link href="../common/css/fixAreaStyle.css" type="text/css" rel="stylesheet" /> 
</head>
<script language="javascript">
    function selectRow(keyValue){
        var parentElement = document.getElementById("hdFieldElement").value;
        if((parentElement!=null)&(parentElement!=""))
        {
            if(window.opener!=null){
                var varElement1 = window.opener.window.document.getElementById(parentElement);
                if(varElement1!=null){
                    varElement1.value = keyValue;
                }
                window.close();
            }
        }
    }


    function EnterFilterTextBox() {
        if (event.keyCode == 13) {
            event.keyCode = 9;
            event.returnValue = false;
            document.all["ImageBtnSearch"].click();
        }
    }
</script>
<body>
 <form id="form1" runat="server">
<asp:HiddenField ID="hdFieldElement" runat ="server" />
    <div>
    <table width="100%" border="0" cellpadding="0" cellspacing="0">
          <!-- 操作行 -->
          <tr height="30" align="left">
            <td align ="center" valign="middle">
                <div class="topBox"style="vertical-align:middle; float:left; margin:auto">
                    <asp:TextBox ID="txtCondition" runat="server"  Width = "300"></asp:TextBox>
                    
                    <%--<div style="display:none; float:left; margin:auto">
                        <asp:LinkButton ID="btnFilter" runat="server" CssClass="a_Left" Font-Bold="true" OnClick="Filter_Click">定   位</asp:LinkButton>
                    </div>--%>
                    <asp:imagebutton runat="server" ImageUrl="../common/images/search_big.png" style= "cursor:hand " AlternateText="search"
                            CausesValidation="false" ID="ImageBtnSearch" Height="20" Width="20" OnClick="ImageBtnSearch_Click"></asp:imagebutton>
                    <asp:imagebutton runat="server" ImageUrl="../common/images/close1.png" style= "cursor:hand " AlternateText="close"
                            CausesValidation="false" ID="Imagebutton2" Height="20" Width="20" OnClientClick="window.close();"></asp:imagebutton>
                </div>
            </td>
          </tr>
           
          <tr>
            <td align="center" valign="top" bgcolor="#F7F8F9">
                <table id="changecolor" class="table" style="width:98%">
                    <tr width="100%">
                        <td style ="width:98%; white-space:nowrap">
                            <asp:DataGrid ID="DataGrid1" runat="server" AllowSorting="true"  Width="100%"  
                                AutoGenerateColumns="true"  onHorizontalAlign="Center" onsortcommand="DataGrid1_SortCommand"
                                onitemdatabound="DataGrid1_ItemDataBound"  >
								<ItemStyle CssClass="tableContent"  />
								<HeaderStyle  BackColor="GreenYellow" Font-Size="small" ></HeaderStyle>
                                <Columns>
								</Columns>
                            </asp:DataGrid>
                        </td>
                    </tr>
                 </table>
             </td>
          </tr>
    </table>
    </div>
    
    </form>
</body>
</html>
