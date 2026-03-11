
//加载省份列表
function BuildProvinceList(){
	setTimeout(function() {
		var url = Const_ServiceUrl + '/NetData/Handler/DataGetter.ashx';
//		var url = '../Handler/DataGetter.ashx';
		mui.ajax(url,{
			data:{
				param:'getprovincelist'
			},
			type:'post',//HTTP请求类型
			timeout:10000,//超时时间设置为10秒；
			success:function(data){
//							alert(data);
				//服务器返回响应，根据响应结果，分析是否登录成功；
                if (data == "") return false;
                var dataobj = eval("(" + data + ")");
				var rows = dataobj.ResultData.length;

				var table = document.getElementById('ulNationDataView');
				
				//再加载列表
	            for (i = 0; i < rows; i++) {
					var li = document.createElement('li');
					
					var DataPackageNo = unescape(dataobj.ResultData[i]["DPNO"]);
					var DataName = unescape(dataobj.ResultData[i]["DataName"]);
					var DataValue = unescape(dataobj.ResultData[i]["DataValue"]);
					var CostPrice = unescape(dataobj.ResultData[i]["CostPrice"]);
					var Discount = unescape(dataobj.ResultData[i]["Discount"]);
					var SalePrice = unescape(dataobj.ResultData[i]["SalePrice"]);
					var ShowDesc = unescape(dataobj.ResultData[i]["ShowDesc"]);
					
					var btnAddId = 'btn_'+DataPackageNo;
//					var tempInnerHtml = '<a class="mui-navigate-right">'
					var tempInnerHtml = ''
					tempInnerHtml = tempInnerHtml + '<span style = "font-weight:bold;font-size:medium;">' + DataName + '</span>'
									+ '<span style = "font-weight:bold;color:green;font-size:small;">&nbsp;&nbsp;&nbsp售价:</span>'
									+ '<span style = "font-weight:bold;color:green;font-size:small;">' + SalePrice + '</span>';
					tempInnerHtml = tempInnerHtml + '<p><span style = "font-size:small;">' + ShowDesc + '</span></p>';
//					tempInnerHtml = tempInnerHtml + '</a>';
					tempInnerHtml = tempInnerHtml
									+ '<button type="button" style="height:28px;font-weight:bold" class="mui-btn mui-btn-success mui-btn-outlined btn-add-data">'
									+ SalePrice + '元</button>';
					
					li.className = 'mui-table-view-cell';	
					li.id = 'li_'+DataName;
					li.name = 'liName_'+DataName;						
					li.innerHTML = tempInnerHtml;
					document.getElementById("lb_NationLoading").style.display="none";
					table.appendChild(li, table.firstChild);
	            }
	            
			},
			error:function(xhr,type,errorThrown){
				mui.toast(GetStringByLanguage('CULTURE_Login_ServerUrlError'));
				//异常处理；
				console.log(type);
			}
		});
	}, 1500);
}

//加载全国流量包
function BuildNationDataList(companyType){
	document.getElementById("lb_NationLoading").innerText = '正在加载中...';
	setTimeout(function() {
		var url = Const_ServiceUrl + '/NetData/Handler/DataGetter.ashx';
//		var url = '../Handler/DataGetter.ashx';
//					alert(url);
		mui.ajax(url,{
			data:{
				param:'querynationdata',
				datascope:'nation',
				companytype:companyType
			},
			type:'post',//HTTP请求类型
			timeout:10000,//超时时间设置为10秒；
			success:function(data){
//							alert(data);
				//服务器返回响应，根据响应结果，分析是否登录成功；
                if (data == "") return false;
                var dataobj = eval("(" + data + ")");
				var rows = dataobj.ResultData.length;

				var table = document.getElementById('ulNationDataView');
				
				//再加载列表
	            for (i = 0; i < rows; i++) {
					var li = document.createElement('li');
					
					var DataPackageNo = unescape(dataobj.ResultData[i]["DPNO"]);
					var DataName = unescape(dataobj.ResultData[i]["DataName"]);
					var DataValue = unescape(dataobj.ResultData[i]["DataValue"]);
					var CostPrice = unescape(dataobj.ResultData[i]["CostPrice"]);
					var Discount = unescape(dataobj.ResultData[i]["Discount"]);
					var SalePrice = unescape(dataobj.ResultData[i]["SalePrice"]);
					var ShowDesc = unescape(dataobj.ResultData[i]["ShowDesc"]);
					
					var btnAddId = 'btn_'+DataPackageNo;
//					var tempInnerHtml = '<a class="mui-navigate-right">'
					var tempInnerHtml = ''
					tempInnerHtml = tempInnerHtml + '<span style = "font-weight:bold;font-size:medium;">' + DataName + '</span>'
									+ '<span style = "font-weight:bold;color:green;font-size:small;">&nbsp;&nbsp;&nbsp售价:</span>'
									+ '<span style = "font-weight:bold;color:green;font-size:small;">' + SalePrice + '</span>';
					tempInnerHtml = tempInnerHtml + '<p><span style = "font-size:small;">' + ShowDesc + '</span></p>';
//					tempInnerHtml = tempInnerHtml + '</a>';
					tempInnerHtml = tempInnerHtml
									+ '<button type="button" style="height:28px;font-weight:bold" class="mui-btn mui-btn-success mui-btn-outlined btn-add-data">'
									+ SalePrice + '元</button>';
					
					li.className = 'mui-table-view-cell';	
					li.id = 'li_'+DataName;
					li.name = 'liName_'+DataName;						
					li.innerHTML = tempInnerHtml;
					document.getElementById("lb_NationLoading").style.display="none";
					table.appendChild(li, table.firstChild);
	            }
	            
			},
			error:function(xhr,type,errorThrown){
				mui.toast(GetStringByLanguage('CULTURE_Login_ServerUrlError'));
				//异常处理；
				console.log(type);
			}
		});
	}, 1500);
}