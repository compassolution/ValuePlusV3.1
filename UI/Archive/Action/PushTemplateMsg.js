
/**
 * 动作执行完成后进行公众号的模版消息推送的处理逻辑
 */
const localHalderUrl = "../../HTML5Handler/WebWX/TemplateMsg/ClientHandler.ashx";
const serverHalderUrl = "HTML5Handler/WebWX/TemplateMsg/ServerHandler.ashx";
var varRemoteServerUrl = "";
var varProjectId = "";

//动作执行完成后准备推送消息
function pushMsgAfterAction(tid, rid, sid, aid, keyValue,userId)
{
    //alert('keyValue:'+keyValue);
    //根据TID/RID/AID获取流程FLFlowConfig_3中对应的下一个岗位的FLUser中配置所需推送提醒的场景及用户及其待办数量
    GetNeedPushUserData(tid, rid, sid, aid, keyValue, userId);
}

//根据TID/RID/AID获取流程FLFlowConfig_3中对应的下一个岗位的FLUser中配置所需推送提醒的场景及用户及其待办数量
function GetNeedPushUserData(tid, rid, sid, aid, keyValue, userId) {
    var urlQuery = "M=" + Math.random() + "&param=getneedpushuserdata&tid=" + tid + "&rid=" + rid + "&sid=" + sid + "&aid=" + aid + "&keyvalue=" + keyValue;
    var url = localHalderUrl + "?" + urlQuery;
    //alert(url);
    $.ajax({
        cache: false,
        url: url,
        type: "POST",//json数据大时，此配置非常重要
        async: false,
        dataType: 'json',
        error: function (request) {
        },
        success: function (data) {
            //alert(data);
            //alert('GetNeedPushUserData.data:' + JSON.stringify(data));
            var dataobj = JSON.parse(JSON.stringify(data));
            //console.log('GetNeedPushUserData.dataobj：', dataobj);
            var returnCode = dataobj.ReturnStatus.ReturnCode
            //alert('GetNeedPushUserData.dataobj.returnCode:' + returnCode);
            if (returnCode == '1') {
                var resultLen = dataobj.ResultData.length
                //alert('GetNeedPushUserData.dataobj.resultLen:' + resultLen);
                if (resultLen == 1) {
                    ///加载客户基本信息
                    //LoadClientInfo();
                    //ResultData的length最多为1，NextPostArray可能有多个【具体组成由localHalderUrl页面生成，可见最底部结构注释】
                    var dataNextPostArray = dataobj.ResultData[0].NextPostArray;

                    varRemoteServerUrl = unescape(dataNextPostArray[0].RemoteServer);
                    varProjectId = unescape(dataNextPostArray[0].ProjectId);

                    //alert('GetNeedPushUserData.varRemoteServerUrl:' + varRemoteServerUrl);
                    //alert('GetNeedPushUserData.varProjectId:' + varProjectId);

                    $.each(dataNextPostArray, function (idx, item) {
                        //alert('GetNeedPushUserData.item:' + JSON.stringify(item));
                        //推送模版数据
                        PushTemplateData(encodeURIComponent(JSON.stringify(item)), keyValue, userId, varProjectId);
                    });

                }
            }

        }
    });
}


//根据目标信息及用户推送模版数据
function PushTemplateData(item, keyValue, userId,projectId) {
    var urlQuery = escape("M=" + Math.random() + "&param=pushtemplatedata&aimdata=" + item + "&keyvalue=" + keyValue + "&usercode=" + userId + "&projectid=" + projectId);
    var url = varRemoteServerUrl + serverHalderUrl + "?" + urlQuery;
    //var url = "../../" + serverHalderUrl + "?" + urlQuery;
    //alert(url);
    $.ajax({
        cache: false,
        url: url,
        type: "POST",//json数据大时，此配置非常重要
        async: false,
        dataType: 'json',
        error: function (request) {
        },
        success: function (data) {
            console.log('PushTemplateData.data', data)


        }
    });
}

//{
//    "ReturnStatus": {
//        "ReturnCode": "1",
//            "ReturnMsg": "根据TID/RID/AID获取流程FLFlowConfig_3中对应的下一个岗位的FLUser中配置所需推送提醒的场景、用户及其待办数量成功"
//    },
//    "ResultData": [{
//        "NextPostArray": [{
//            "FCODE": "FLLV",
//            "FNAME": "Leaves Approval",
//            "FNAMECHS": "休假审批",
//            "PostCode": "PRP0050",
//            "PostName": "T&C Manager",
//            "PostNameChs": "T&C经理",
//            "SceneCode": "PRS030",
//            "SceneName": "My Approve Pending",
//            "SceneNameChs": "我的待审核",
//            "SUSERID": "HRM",
//            "MOBILENO": "18089779994",
//            "STAFFNO": "0004",
//            "PendingQty": "47",
//            "ProjectId": "VP_HR_HKSFT",
//            "RemoteServer": "http://115.29.110.153/"
//        }]
//    }]
//}
