

//设置某变量的本地存储
function SetLocalStorage(item,value){
	localStorage.setItem(item,value);
}

//获取本地存储中某变量的值
function GetLocalStorage(item){
	if(localStorage.getItem(item)){
		return localStorage.getItem(item);
	}else{
		return ''
	}
}

//移除本地存储中某变量
function RemoveLocalStorage(item){
	localStorage.removeItem(item);
}

//清除本地缓存数据（主要是localStorage）
function ClearLocalStorage(){
	localStorage.clear();
}

///////*****特定参数操作


