
//加班申请流程中FLOT的模板明细页面的客户端额外处理
//主要是判断由于某个字段值变化后对其他字段的影响


//取消保存按钮的OnPreSaved事件
$("#aSave").removeAttr('onclick');


//加班申请类别的变更对字段的影响
function ShowCtrlByOTApplyType_FLOT() {
    var NoRedStarHtml = '<SPAN></SPAN>';
    var redStarHtml = '<SPAN style="COLOR:red;">*</SPAN>';

    if ($('#ddList_FLOT_1_OTApplyType').val() == '010') {//010	Room Apply	客房申请加班
        //需填写预计当天住房率
        $('#label_FLOT_1_ForecastOCC').next().html(redStarHtml);
        $('#label_FLOT_1_ForecastOCC').show();
        $('#txt_FLOT_1_ForecastOCC').show();

        //不显示活动列表
        $('#tb_FLOT_4').hide();

    } else if ($('#ddList_FLOT_1_OTApplyType').val() == '020') {//020	RBE Apply	宴会申请加班
        //无需填写预计当天住房率
        $('#label_FLOT_1_ForecastOCC').next().html(NoRedStarHtml);
        $('#label_FLOT_1_ForecastOCC').hide();
        $('#txt_FLOT_1_ForecastOCC').hide();

        //显示活动列表
        $('#tb_FLOT_4').show();

    } else if ($('#ddList_FLOT_1_OTApplyType').val() == '099') {//099	Others Apply	其他类别申请加班
        //无需填写预计当天住房率
        $('#label_FLOT_1_ForecastOCC').next().html(NoRedStarHtml);
        $('#label_FLOT_1_ForecastOCC').hide();
        $('#txt_FLOT_1_ForecastOCC').hide();

        //不显示活动列表
        $('#tb_FLOT_4').hide();
    }
}

ShowCtrlByOTApplyType_FLOT();

//切换加班申请类别
$('#ddList_FLOT_1_OTApplyType').change(function () {
    ShowCtrlByOTApplyType_FLOT();
});

//保存按钮点时需要先校验字段必填性
$("#aSave").click(function (e) {
    var isCanSave = true;

    if ($('#ddList_FLOT_1_OTApplyType').val() == '010') {//010	Room Apply	客房申请加班
        if ($('#txt_FLOT_1_ForecastOCC').val() == '') {
            $('#txt_FLOT_1_ForecastOCC').css('background-color', 'red');
            isCanSave = false;
        }

    } else if ($('#ddList_FLOT_1_OTApplyType').val() == '020') {//020	RBE Apply	宴会申请加班
        $('#txt_FLOT_1_ForecastOCC').css('background-color', 'white');
        isCanSave = true;


    } else if ($('#ddList_FLOT_1_OTApplyType').val() == '099') {//099	Others Apply	其他类别申请加班
        $('#txt_FLOT_1_ForecastOCC').css('background-color', 'white');
        isCanSave = true

    }

    if (!isCanSave) {
        alert('红色标记处的字段必填!');
    }

    return isCanSave;
});