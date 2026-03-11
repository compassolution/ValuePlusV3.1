    function reloadOpenerPages(msg,moveNextFlag) 
    {
        //moveNextFlag:动作执行完成后，是否跳出本条记录的标志(1为跳出)
        alert(msg);
        //列表页面执行动作的情况
        if(window.opener.document.getElementById("aRefresh")!=null){//档案列表页面的刷新按钮
            window.opener.document.all["aRefresh"].click();
        }
        //明细信息页面执行动作的情况
        if(window.opener!=null){
            if (window.opener.document.getElementById("hfIsOpenAtCurPage") != null) {
                //档案明细页面的隐藏标志位，标志档案明细页面的打开位置
                var varIsOpenAtCurPage = window.opener.document.getElementById("hfIsOpenAtCurPage").value;

                if (moveNextFlag == 0) {//当前记录
                    if (window.opener.document.getElementById("aRefreshDetail") != null)//档案明细页面的刷新按钮
                    {
                        window.opener.document.all["aRefreshDetail"].click();
                    }
                }
                else if (moveNextFlag == 1) {//返回列表
                    if (varIsOpenAtCurPage == '0') {//明细页面为新弹出窗口
                        window.opener.close();
                    } else {
                        if (window.opener.document.getElementById("aBack") != null)//档案明细页面的返回按钮
                        {
                            window.opener.document.all["aBack"].click();
                        }
                    }
                }
                else if (moveNextFlag == 2) {//下条记录
                    if (window.opener.document.getElementById("imgBtnNext") != null)//档案明细页面的下条记录按钮
                    {
                        window.opener.document.all["imgBtnNext"].click();
                    }
                }
                else if (moveNextFlag == 3) {//上条记录
                    if (window.opener.document.getElementById("imgBtnPre") != null)//档案明细页面的上条记录按钮
                    {
                        window.opener.document.all["imgBtnPre"].click();
                    }
                }
                else if (moveNextFlag == 4) {//新增记录
                    if (window.opener.document.getElementById("aAddDetail") != null)//档案明细页面的新增记录按钮
                    {
                        window.opener.document.all["aAddDetail"].click();
                    }
                }
            }
        };
        window.close();
    }