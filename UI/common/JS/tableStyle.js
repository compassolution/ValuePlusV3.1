//设置table表格样式("表格名称","奇数行背景","偶数行背景","鼠标经过背景","点击后背景","列名行背景");
function biuldTableCss(o,a,b,c,d,e){
    if(document.getElementById(o)){
         var t=document.getElementById(o).getElementsByTagName("tr");
         if(t.length>1){
             t[1].style.backgroundColor= e;//第一行（列名）
             for(var i=2;i<t.length;i++){
                t[i].style.backgroundColor=(t[i].sectionRowIndex%2==0)?a:b;
                t[i].onclick=function(){
                    if(this.x!="1"){
                        this.x="1";
                        this.style.backgroundColor=d;
                    }else{
                        this.x="0";
                        this.style.backgroundColor=(this.sectionRowIndex%2==0)?a:b;
                    }
                }
                t[i].onmouseover=function(){
                    if(this.x!="1"){
                        this.style.backgroundColor=c;
                        this.style.cursor='hand';
                    }
                }
                t[i].onmouseout=function(){
                    if(this.x!="1")this.style.backgroundColor=(this.sectionRowIndex%2==0)?a:b;
                }
             }
         }
    }     
}

//设置没有列名行的table表格样式("表格名称","奇数行背景","偶数行背景","鼠标经过背景","点击后背景");
function biuldNoTitleTableCss(o,a,b,c,d){
    if(document.getElementById(o)){
         var t=document.getElementById(o).getElementsByTagName("tr");
         if(t.length>1){
             for(var i=0;i<t.length;i++){
                t[i].style.backgroundColor=(t[i].sectionRowIndex%2==0)?a:b;
                t[i].onclick=function(){
                    if(this.x!="1"){
                        this.x="1";
                        this.style.backgroundColor=d;
                    }else{
                        this.x="0";
                        this.style.backgroundColor=(this.sectionRowIndex%2==0)?a:b;
                    }
                }
                t[i].onmouseover=function(){
                    if(this.x!="1"){
                        this.style.backgroundColor=c;
                        this.style.cursor='hand';
                    }
                }
                t[i].onmouseout=function(){
                    if(this.x!="1")this.style.backgroundColor=(this.sectionRowIndex%2==0)?a:b;
                }
             }
         }
    }     
}

//设置table表格样式("表格名称","奇数行背景","偶数行背景","点击后背景","列名行背景");【不执行"鼠标经过背景"】
function biuldTableCssNoCursorOver(o,a,b,d,e){
    if(document.getElementById(o)){
         var t=document.getElementById(o).getElementsByTagName("tr");
         if(t.length>1){
             t[1].style.backgroundColor= e;//第一行（列名）
             for(var i=2;i<t.length;i++){
                t[i].style.backgroundColor=(t[i].sectionRowIndex%2==0)?a:b;
                t[i].onclick=function(){
                    if(this.x!="1"){
                        this.x="1";
                        this.style.backgroundColor=d;
                    }else{
                        this.x="0";
                        this.style.backgroundColor=(this.sectionRowIndex%2==0)?a:b;
                    }
                }
             }
         }
    }     
}

//设置存在列名的table表格样式("表格名称","奇数行背景","偶数行背景","鼠标经过背景","点击后背景","列名行背景");
function DefineTableCss(tableName){
    biuldTableCss(tableName,"#ffffff","#f0f0f0","#E3EEFD","#C2DAF1","#CAE1FF");
}

//设置没有列名的table表格样式("表格名称","奇数行背景","偶数行背景","鼠标经过背景","点击后背景");
function DefineNoTitleTableCss(tableName){
    biuldNoTitleTableCss(tableName,"#ffffff","#f0f0f0","#E3EEFD","#C2DAF1");
}

//设置存在列名的table表格样式("表格名称","奇数行背景","偶数行背景","点击后背景","列名行背景");
function DefineTableCssNoCursorOver(tableName){
    biuldTableCssNoCursorOver(tableName,"#ffffff","#f0f0f0","#C2DAF1","#CAE1FF");
}