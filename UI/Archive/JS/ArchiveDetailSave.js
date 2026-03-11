
	//保存档案信息的数据之前的客户端处理
	function onPreSaved()
	{
	    var oTr = document.getElementById('trAchiveContent');
	    
	    //获取所有select对象
	    var oSelect = oTr.getElementsByTagName('select');
	    for( i = 0; i< oSelect.length; i++ )
        {
            var txtCrlId = oSelect[i].id;
            var txtCtrl = document.getElementById(txtCrlId);
            //去掉下拉框控件的disabled属性
            txtCtrl.removeAttribute("disabled");
	    }
	    ShowWaitingDiv();
	    return true;
	}