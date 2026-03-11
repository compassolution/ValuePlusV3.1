
  //提交页添加
  window.onbeforeunload=function()  
  {  
      setCookie("ArchiveDetailPage",document.body.scrollTop);  
  }  
  function   setCookie(name,value)  
  {  
      var   Days   =   30;  
      var   exp     =   new   Date();  
      exp.setTime(exp.getTime()   +   Days*24*60*60*1000);  
      document.cookie   =   name   +   "="+   escape   (value)   +   ";expires="   +   exp.toGMTString(); 
  }  

  //目标跳转页面添加
  window.onload=function()  
  {  
      window.scroll(0,getCookie("ArchiveDetailPage"))  
  }  
  function   getCookie(name)  
  {  
      var   arr   =   document.cookie.match(new   RegExp("(^|   )"+name+"=([^;]*)(;|$)"));  
      if(arr   !=   null)   return   unescape(arr[2]);   return   null;  
  }  
