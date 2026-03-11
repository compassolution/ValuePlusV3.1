/*------------ DataGrid鼠标事件处理 ------------
功能:用于数据梆定后鼠标事件
参数说明:
obj:对像this
fontColor:字体颜色
backColor:背景颜色
----------------------------------------------*/ 
var objState

//------------鼠标经过时-----------
function OnFoucsMouseOver(obj,fontColor,backColor)
{
 if (obj.rowIndex > 0 )
 {
  obj.style.color = fontColor;
  obj.style.backgroundColor = backColor;
 }
}

//-----------鼠标离开时-----------
function OnFoucsMouseOut(obj,fontColor,backColor)
{
 if ( obj.rowIndex > 0 )
 {
  if ( obj != objState )
  {
   obj.style.color = fontColor;
   obj.style.backgroundColor = backColor;
  }
 }
}

//-----------鼠标单击时-----------
function OnFoucsClick(obj,fontColor,backColor)
{
 if ( obj.rowIndex > 0 )
 {
  if ( objState != null )
  {
   objState.style.color = ;
   objState.style.backgroundColor = ;
  }
  obj.style.color = fontColor;
  obj.style.backgroundColor = backColor;
  
  objState = obj;
 }
}

//-----------鼠标双击时-----------
function OnFoucsDbClick(obj,fontColor,backColor,openUrl)
{
 //参数openUrl为要开的新窗口的地址
 if ( obj.rowIndex > 0 )
 {
  if ( objState != null )
  {
   objState.style.color = ;
   objState.style.backgroundColor = ;
  }
  obj.style.color = fontColor;
  obj.style.backgroundColor = backColor;
  
  objState = obj;
  
  window.open( openUrl );
 }
}

