
//流程审批中,不同的流程状态使用不同颜色标记,【当然也可以扩展其他非流程的TID模板】
var arrayFLArchive = ['FLOT', 'FLCA', 'FLCR', 'FLER', 'FLLV', 'FLMP', 'FLNS', 'FLOT', 'FLPA', 'FLPC', 'FLSA', 'FLUT'];
//我的全部和全部单据两种状态【这里可扩展其他场景状态】
var arrayFLSence = ['PRS900', 'PRS800']
if ($.inArray(mainTID, arrayFLArchive) >= 0 && $.inArray(mainSID, arrayFLSence) >= 0) {
    $('#DataGrid1 tr').each(function (i) {  
        var objTR = $(this);                 // 遍历 tr
        $(this).children('td').each(function (j) {  // 遍历 tr 的各个 td
            //alert($(this).text());
            var objTD = $(this);
            var textTD = $(this).text();
            switch (textTD) {
                // 【不同项目中的状态名称可能有区别，则需个别修改判断】
                case '草稿': objTD.css("background-color", "#4F94CD"); break;
                case '审核中': objTD.css("background-color", "#DAA520"); break;
                case '已退回': objTD.css("background-color", "#FF0000"); break;
                case '已取消': objTD.css("background-color", "#CDC1C5"); break;
                case '审核失败': objTD.css("background-color", "#FF8247"); break;
                case '审核通过': objTD.css("background-color", "#00CD00"); break;
                case '二次录入': objTD.css("background-color", "#4F94CD"); break;
                case '二次审核中': objTD.css("background-color", "#DAA520"); break;
                case '二次已退回': objTD.css("background-color", "#FF0000"); break;
                case '二次审核失败': objTD.css("background-color", "#FF8247"); break;
                case 'HCD已阅': objTD.css("background-color", "#008B8B"); break;
                case 'PM已阅': objTD.css("background-color", "#00688B"); break;
                case '流程结束': objTD.css("background-color", "#66CD00"); break;
                default: break;
            }
        });
    });
}