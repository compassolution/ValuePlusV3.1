    //常规分组类多个页眉的显示和隐藏
	function HideRows(rowId)
	{
	    var Arr1 = rowId.split("_");
	    var tbName = "tb_"+Arr1[1]+"_"+Arr1[2];
	    var iRowCount = document.all[tbName].rows.length;
	    var iIndex = Arr1[3];
	    var i=iIndex*1+1;
	    while(i<iRowCount)
	    {
	        var trID = document.all[tbName].rows(i).id;
	        
	        if(trID.indexOf('CH')<0){
	            if(document.all[tbName].rows(i).style.display==""){
	                document.all[tbName].rows(i).style.display = "none";
	            }else{
	                document.all[tbName].rows(i).style.display = "";
	            }
	        }else{
	            break;
	        }
	        i++;
	    }
	}
	
	//选择全选所有分组多选框
	function AllGroupCheck()
	{
	    var oTd = document.getElementById('tdGroupCheckBox');
	    var oSel = oTd.getElementsByTagName('input');
	    var cbAll = document.getElementById('ckb_SelectAll');
	    for( i = 0; i< oSel.length; i++ )
        {
            if(oSel[i].type=="checkbox"){ 
                var cbCtrlId = oSel[i].id;
	            var checkB = document.getElementById(cbCtrlId);
	            if(cbAll.checked){
	                checkB.checked = "checked";
	            }else{
	                checkB.checked = "";
	            }
	            changeGroupCheckBox(cbCtrlId);
	        }
	    }
	}
	
	//选择某特定分组的多选框
	function changeGroupCheckBox(cbCtrlId)
	{
	    var checkB = document.getElementById(cbCtrlId);
	    var vTidGid = cbCtrlId.substring(6);
	    var divGroupId = "gDiv_"+vTidGid;
	    var divGroup = document.getElementById(divGroupId);
	    if(checkB.checked){
	        divGroup.style.display = "";
	    }else{
	        divGroup.style.display = "none";
	    }
	}
	
	//点击分组名称定为到相应区域
	function locationDiv(divArea){
	    var ctrlDiv = document.getElementById(divArea);
	    if(ctrlDiv.style.display == ""){
	        window.location.href="#"+divArea;
	    }
	}

	/**
	* 将数值四舍五入(保留2位小数)后格式化成金额形式
	*
	* @param ctrlId 控件ID
    * @n 四舍五入保留小数位
	* @return 金额格式的字符串,如'1,234,567.45'
	* @type String
    * 调用：formatCurrency("12345.675910", 3)，返回12,345.676 
	*/
    function formatCurrency(ctrlId, n)   
    {  
       var s = document.getElementById(ctrlId).value;
//       alert(s);
       n = n > 0 && n <= 20 ? n : 2;   
       s = parseFloat((s + "").replace(/[^\d\.-]/g, "")).toFixed(n) + "";   
       var l = s.split(".")[0].split("").reverse(),   
       r = s.split(".")[1];   
       t = "";   
       for(i = 0; i < l.length; i ++ )   
       {   
          t += l[i] + ((i + 1) % 3 == 0 && (i + 1) != l.length ? "," : "");   
       }   
       document.getElementById(ctrlId).value = t.split("").reverse().join("") + "." + r;
   }

   //金额还原函数
   function rmoney(s) {
       return parseFloat(s.replace(/[^\d\.-]/g, ""));
   }
   
   
	/*
	===========================================
	//全替换字符串
	===========================================
	*/
	String.prototype.ReplaceAll = function(s1,s2) { 
		return this.replace(new RegExp(s1,"gm"),s2); 
	}