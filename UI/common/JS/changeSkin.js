var arrCSS=[ 
    ["<img src='common/images/ocean.gif' width='10' height='8' class='themes' alt='blue'>","common/css/blueSkin.css"], 
    ["<img src='common/images/tangerine.gif' width='10' height='8' class='themes' alt='green'>","common/css/greenSkin.css"],
    "" 
    ]; 


function v(){ 
		//alert(document.getElementsByTagName("link"));
    return; 
} 

// *** Cookies *** 
function writeCookie(name, value) {  
    exp = new Date();  
    exp.setTime(exp.getTime() + (86400 * 1000 * 30)); 
    document.cookie = name + "=" + escape(value) + "; expires=" + exp.toGMTString() + "; path=/";  
}  
function readCookie(name) {  
    var search;  
    search = name + "=";  
    offset = document.cookie.indexOf(search);  
    if (offset != -1) {  
        offset += search.length;  
        end = document.cookie.indexOf(";", offset);  
        if (end == -1){ 
            end = document.cookie.length; 
        } 
        return unescape(document.cookie.substring(offset, end));  
    }else{ 
        return ""; 
    } 
} 


//////////////////////////////////// 
// StyleSheet 
//////////////////////////////////// 
function writeCSS(){ 
  for(var i=0;i<arrCSS.length;i++){ 
    document.write('<link title="css'+i+'" href="'+arrCSS[i][1]+'" rel="stylesheet" disabled="true" type="text/css" />'); 
    //alert('<link title="css'+i+'" href="'+arrCSS[i][1]+'" rel="stylesheet" disabled="true" type="text/css" />'); 
  } 
    setStyleSheet(readCookie("stylesheet")); 
} 

function writeCSSLinks(){ 
  for(var i=0;i<arrCSS.length-1;i++){ 
    if(i>0) document.write('  '); 
    document.write('<a href="javascript:v()" onclick="setStyleSheet(\'css'+i+'\')">'+arrCSS[i][0]+'</a>'); 
    //alert('<a href="javascript:v()" onclick="setStyleSheet(\'css'+i+'\')">'+arrCSS[i][0]+'</a>'); 
  } 
} 

function setStyleSheet(strCSS){ 
  var objs=document.getElementsByTagName("link"); 
  var intFound=0; 
  for(var i=0;i<objs.length;i++){ 
    if(objs[i].type.indexOf("css")>-1&&objs[i].title){ 
      objs[i].disabled = true; 
      if(objs[i].title==strCSS) intFound=i; 
    	//alert(objs.length+','+objs[i].title+','+objs[i].title);
    } 
  } 
  objs[intFound].disabled = false; 
  writeCookie("stylesheet",objs[intFound].title); 
} 

writeCSS(); 
setStyleSheet(readCookie("stylesheet")); 

// Òþ²ØÏÔÊ¾»»·ô¿ò 
function ShowHideDiv(init) { 
    if(document.getElementById("Sright").style.display == "block"){ 
         document.getElementById("Sright").style.display = "none"; 
  } 
  else{ 
      document.getElementById("Sright").style.display = "block"; 
  } 
} 