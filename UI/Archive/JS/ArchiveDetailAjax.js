//主控字段下拉框控件ONCHANGE事件/数据列表控件onpropertychange事件
    function onChangeCtrlValue(ctrlId,keyValue) 
    {
        var ddList = document.getElementById(ctrlId);
        try {
            Archive_Detail_ArchiveDetailAjax.GetBeMastControlList(ctrlId, ddList.value, keyValue, get_CtrlDataSet_CallBack);
        } catch (e) {
        alert(e.ToString());
            throw e;
        }
    }
    
    //主控字段变化值后返回数据集并填充所影响控件的显示
    function get_CtrlDataSet_CallBack(response)
    {
        if (response.value != null)
        { 　　 
            var arrList = response.value;
            if(arrList != null && typeof(arrList) == "object" && arrList.length>0)
            {
                for(var i=0;i<arrList.length;i++)
                {
                    var ObjectPMast = arrList[i];
                    if(ObjectPMast!=null)
                    {
                        var tid = ObjectPMast.TID;
                        var gid = ObjectPMast.GID;
                        var ctrlType = ObjectPMast.CtrlType;
                        var ctrlId = ObjectPMast.CtrlId;
                        var mastValue = ObjectPMast.MastValue;
                        var dt = ObjectPMast.DtResult;
                        if(dt != null)
                        {
                            if(document.all(ctrlId)!=null)
                            {
                                if((ctrlType=="0")||(ctrlType=="3")||(ctrlType=="6")||(ctrlType=="9"))//文本框(包括多行，宽行，密码等)
                                {
                                    for (var j = 0; j < dt.Rows.length; j++) 
                                    {
                                        var name = dt.Rows[j].CDESC;
　　                                    var id=dt.Rows[j].CID;
　　                                    document.all(ctrlId).value = name;
　                                  }
　                              }else if(ctrlType=="1")//下拉框列表
                                {
　                                  Archive_Detail_ArchiveDetailAjax.SetSeverCtrlMastValue(ctrlId,mastValue);
　　                                document.all(ctrlId).length = 0;
                                    document.all(ctrlId).options.add(new Option("", ""));
                                    for(var j=0; j<dt.Rows.length; j++)
　                                  {
                                        var name = dt.Rows[j].CDESC;
　　                                    var id=dt.Rows[j].CID;
　　                                    document.all(ctrlId).options.add(new Option(name,id));
　                                  }
//　                                  document.all(ctrlId).options[0].selected=true;
　                              }else if(ctrlType=="2")//DB LIST数据列表
                                {
                                    //document.all(ctrlId).value = "";
                                    //document.getElementById(ctrlId).setAttribute("curValue", "");
　　                                if(document.all("img_"+ctrlId)!=null)
　　                                {
                                        document.all("img_" + ctrlId).onclick = function () {
                                            var txtCtrlId = this.id.replace("img_", "");
                                            var curValue = document.getElementById(txtCtrlId).getAttribute("curValue");

                                            document.all(this.id).alt = mastValue;
                                            var sParams = document.all(this.id).getAttribute("Params");
                                            var sParams = sParams + "&MASTVALUE=" + mastValue + "&KEYVALUE=" + curValue; //不加密
                                            //兼容Edge浏览器的参数添加
                                            sParams = sParams + "&TextCtrlId=" + ctrlId;//
                                            var url = "ArchiveCtrlDBList.aspx?" + sParams;

                                            var varKeyValue = document.all(txtCtrlId).value;
                                            curValue = document.getElementById(txtCtrlId).getAttribute("curValue");
                                            url = url.replace("&KEYVALUE=" + curValue, "&KEYVALUE=" +varKeyValue);
                                            document.getElementById(txtCtrlId).setAttribute("curValue", varKeyValue);
                                            window.open(url, 'newwindow', 'width=900,height=500,top=100,left=250, center:1, toolbar=no, menubar=no, scrollbars=yes,resizable=no,location=no, status=no');
                                            return false;
                                        };
                                        //delete by sammen 20231228 禁止控件可人工输入，仅提供放大镜选择,则需屏蔽掉如下一段代码
                                        //restore by sammen 20240105 恢复可人工输入
                                        document.all(ctrlId).onblur = function () {
                                            var txtCtrlId = this.id;
                                            var curValue = document.getElementById(txtCtrlId).getAttribute("curValue");

                                            document.all("img_" + this.id).alt = mastValue;
                                            var sParams = document.all("img_" + this.id).getAttribute("Params");
                                            var sParams = sParams + "&MASTVALUE=" + mastValue + "&KEYVALUE=" + curValue; //不加密
                                            var url = "ArchiveCtrlDBList.aspx?" + sParams;

                                            var varKeyValue = document.all(txtCtrlId).value;
                                            curValue = document.getElementById(txtCtrlId).getAttribute("curValue");
                                            url = url.replace("&KEYVALUE=" + curValue, "&KEYVALUE=" + varKeyValue);
                                            document.getElementById(txtCtrlId).setAttribute("curValue", varKeyValue);
                                            window.open(url + "&ISOK=1", 'newwindow', 'width=900,height=500,top=100,left=250, center:1, toolbar=no, menubar=no, scrollbars=yes,resizable=no,location=no, status=no');
                                            return false;
                                        };
　　                                }
　                              }
　                          }
　                      }
　                  }
　              }
　          }
         }
         
    }
    
    //删除模板列表类型分组中某条记录
    function DeleteGridDetail(tid,gid,key,keyvalue,gridkey,gridkeyvalue){
        if(confirm('Delete it,Are you sure?')){
            var responseObj = Archive_Detail_ArchiveDetailAjax.DeleteGridOneRecord(tid,gid,key,keyvalue,gridkey,gridkeyvalue);
            var msg = responseObj.value;
            if (document.getElementById("aRefreshDetail") != null) {
                document.getElementById("aRefreshDetail").click();
            }
            alert(msg);
        }
        return false;
    }
    
    
    