//跳转到新页面
function RedirectNewPage(pageId, pageUrl, param) {
	if (mui.os.plus) {
		//跳转页面(打包成App时使用此方案可最大限度提供性能)
		//将参数转化为json数组
		var paramJson = '{paramName:"paramValue"';
		if (param != '') {
			var array1 = param.split('&');
			for (var i = 0; i < array1.length; i++) {
				var array2 = array1[i].split('=');
				paramJson = paramJson + ',' + array2[0] + ':"' + array2[1] + '"';
			}
		}
		paramJson = paramJson + '}';
//		alert(paramJson);
		var paramJsonObj = eval("(" + paramJson + ")");
		mui.openWindow({
			url: pageUrl,
			id: pageId,
			extras: paramJsonObj
		});
	} else {
//		alert(pageUrl + '?' + param);
		location.href = pageUrl + '?m='+Math.random()+'&' + param;
	}
}

// 对Date的扩展，将 Date 转化为指定格式的String 
// 月(M)、日(d)、小时(h)、分(m)、秒(s)、季度(q) 可以用 1-2 个占位符， 
// 年(y)可以用 1-4 个占位符，毫秒(S)只能用 1 个占位符(是 1-3 位的数字) 
// 例子： 
// (new Date()).Format("yyyy-MM-dd hh:mm:ss.S") ==> 2006-07-02 08:09:04.423 
// (new Date()).Format("yyyy-M-d h:m:s.S")      ==> 2006-7-2 8:9:4.18 
Date.prototype.Format = function(fmt) { //author: meizz 
	var o = {
		"M+": this.getMonth() + 1, //月份 
		"d+": this.getDate(), //日 
		"h+": this.getHours(), //小时 
		"m+": this.getMinutes(), //分 
		"s+": this.getSeconds(), //秒 
		"q+": Math.floor((this.getMonth() + 3) / 3), //季度 
		"S": this.getMilliseconds() //毫秒 
	};
	if (/(y+)/.test(fmt))
		fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
	for (var k in o)
		if (new RegExp("(" + k + ")").test(fmt))
			fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
	return fmt;
}

//生成GUID
function NewGuid() {   
	function S4() {      
		return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);   
	}   
	return (S4() + S4() + "-" + S4() + "-" + S4() + "-" + S4() + "-" + S4() + S4() + S4());
}


//针对url中的参数进行加密
function EscapeUrlParam(paramString) {
	return escape(paramString + "&M=" + Math.random());
}

//JS获取URL参数值的函数 
function GetUrlParamValue(name) {
	var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
	//var reg = new RegExp((.*)?(.*|$)); 
	var r = window.location.search.substr(1).match(reg);
	if (r != null) {
		return unescape(r[2]);
	} else {
		return '';
	}
}

//為系統追加replaceAll方法
String.prototype.replaceAll = function(oldStr, newStr) {
	return this.replace(new RegExp(oldStr, "gm"), newStr);
}

//Hashtable的Javascript实现
function HashTable() {
	var size = 0;
	var entry = new Object();

	this.setValue = function(key, value) {
		if (!this.containsKey(key)) {
			size++;
		}
		entry[key] = value;
	}

	this.getValue = function(key) {
		return this.containsKey(key) ? entry[key] : null;
	}

	this.remove = function(key) {
		if (this.containsKey(key) && (delete entry[key])) {
			size--;
		}
	}

	this.containsKey = function(key) {
		return (key in entry);
	}

	this.containsValue = function(value) {
		for (var prop in entry) {
			if (entry[prop] == value) {
				return true;
			}
		}
		return false;
	}

	this.getValues = function() {
		var values = new Array();
		for (var prop in entry) {
			values.push(entry[prop]);
		}
		return values;
	}

	this.getKeys = function() {
		var keys = new Array();
		for (var prop in entry) {
			keys.push(prop);
		}
		return keys;
	}

	this.getSize = function() {
		return size;
	}

	this.clear = function() {
		size = 0;
		entry = new Object();
	}
}