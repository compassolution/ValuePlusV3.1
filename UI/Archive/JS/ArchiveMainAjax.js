    //纯脚本的全选功能
    function checkAll(ctrlId) 
    {
	    var cbSelectAll = document.getElementById(ctrlId);
	    //var oTd = document.getElementById('tdGrid');
	    //var oSel = oTd.getElementsByTagName('input');
	    //for( i = 0; i< oSel.length; i++ )
     //   {
     //       if(oSel[i].type=="checkbox"){ 
	    //        var checkB = oSel[i];
     //           var cbCtrlId = oSel[i].id;
     //           if(cbCtrlId.indexOf("cbSelect")>0)
     //           {
                      //单项全选模式
	    //            if(checkB.checked){
	    //                checkB.checked = "";
	    //            }else{
	    //                checkB.checked = "checked";
	    //            }
	    //        }
	    //    }
	    //}
	    //if(cbSelectAll.checked){
     //       cbSelectAll.checked = "";
     //   }else{
     //       cbSelectAll.checked = "checked";
     //   }

        //alert(ctrlId);
        //alert($('input[id*="cbAll"]').attr('id'));
        var ctrlId = $('input[id*="cbAll"]').attr('id');
        //修改成全选和全不选模式
        $('input[id*="cbSelect"]').each(function () {
            $(this).attr("checked", $('#' + ctrlId).is(':checked'));

        }) 
        $('#' + ctrlId).attr('checked', isAllChecked)
    } 
    
    //保存复选框的是否被选择状态
    function SaveCheckBoxValue() 
    {
	    var oTd = document.getElementById('tdGrid');
	    var oSel = oTd.getElementsByTagName('input');
	    var allValue = "";
	    var j = 0;
	    for( i = 1; i< oSel.length; i++ )
        {
            if (oSel[i].type == "checkbox") {
	            var checkB = oSel[i];
                var cbCtrlId = oSel[i].id;
                if (cbCtrlId.indexOf("cbSelect") > 0) {
                    if(j ==0){
                        if (checkB.checked) {
                            allValue = cbCtrlId+":true";
	                    }else{
                            allValue = cbCtrlId+":false";
	                    }
                    }else{
	                    if(checkB.checked){
                            allValue = allValue+"*"+cbCtrlId+":true";
	                    }else{
                            allValue = allValue+"*"+cbCtrlId+":false";
	                    }
	                }
                }
                j++;
	        }
        }
	    if(allValue!=""){
	        Archive_ArchiveMainAjax.SaveArchiveMainCheckBox(allValue);
	    }
    } 
    