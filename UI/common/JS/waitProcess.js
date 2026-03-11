    //暂时不用
    function disableBodyArea(win,f){
        var objWindow = null;
        var objArea = null;
        if((win=='self')&&(window.document.getElementById("processPage"))!=null){
            //alert('3');
            objWindow = window;
            objArea = objWindow.document.getElementById("processPage");
        }else if ((win=='parent')&&(objArea==null)&&(window.parent!=null)&&(window.parent.document.getElementById("processPage")!=null)){
            //alert('2');
            objWindow = window.parent;
            objArea = objWindow.document.getElementById("processPage");
        }else if ((objArea==null)&&(window.document.getElementById("processPage")!=null)){
            //alert('1');
            objWindow = window;
            objArea = objWindow.document.getElementById("processPage");
        }
        
        if (objArea!=null){
            objArea.style.display = f? '':'none';
            objArea.style.height = window.document.body.scrollHeight;
            objArea.style.width = window.document.body.scrollWidth;
        }
    }
    
    //暂时不用
    function showWaittingProcess(win){
        //alert(win);
        disableBodyArea(win,true);
    }
    
    window.onbeforeunload=function (){
        var n = window.event.screenX -window.screenLeft;         
        var b = n > document.documentElement.scrollWidth-20;        
        if(b && window.event.clientY < 0 || window.event.altKey)         
        {         
            //alert("是关闭而非刷新");         
            //window.event.returnValue = "是否关闭？";      
        }else{      
            //alert("是刷新而非关闭");         
        
            var objWindow = null;
            var objArea = null;
            if(window.document.getElementById("processPage")!=null){
                //alert('3');
                objWindow = window;
                objArea = objWindow.document.getElementById("processPage");
            }else if ((objArea==null)&&(window.parent!=null)&&(window.parent.document.getElementById("processPage")!=null)){
                //alert('2');
                objWindow = window.parent;
                objArea = objWindow.document.getElementById("processPage");
            }else if ((objArea==null)&&(window.parent.parent!=null)&&(window.parent.parent.document.getElementById("processPage")!=null)){
                //alert('1');
                objWindow = window.parent.parent;
                objArea = objWindow.document.getElementById("processPage");
            } 
            
            if (objArea!=null){
                objArea.style.height = objWindow.document.body.scrollHeight;
                objArea.style.width = objWindow.document.body.scrollWidth;
                objArea.style.display = '';
            }
       }
    }
