
var mainProjectId;
var mainTID;
var mainRID;
var mainSID;
var mainKEY;
var mainUserId;

//可扩展该页面的一些逻辑 ADD BY SAMMEN 20180504
function ExcuteSpecialScript(projectId, tid, rid, sid, key, userid) {
    mainProjectId = projectId;
    mainTID = tid;
    mainRID = rid;
    mainSID = sid;
    mainKEY = key;
    mainUserId = userid;
    if (projectId == 'VP_HR_SZPP') {
        $.getScript("JS/ArchiveMain_HR_SZPP.js");
    }else if (projectId == 'VP_HR_XMPP') {
        $.getScript("JS/ArchiveMain_HR_XMPP.js");
    } else if (projectId == 'VP_HR_FSGZ') {
        $.getScript("JS/ArchiveMain_VP_HR_FSGZ.js");
    } else if (projectId == 'VP_HR_HKSFT') {
        // ADD BY SAMMEN 20210830
        $.getScript("JS/ArchiveMain_VP_HR_HKSFT.js");
    }
}
