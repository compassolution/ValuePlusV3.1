
//PBMS系统中CRM项目管理中的模板明细页面的客户端额外处理
//主要是判断由于某个字段值变化后对其他字段的影响

//CloseDate字段设置只读，否则销售人员会手工输入错误格式
$('#txt_CProject_1_CloseDate').attr('readonly','readonly');

//自动触发销售人员默认选择后的事件触发
$('#ddList_CProject_1_SalesManager').trigger('change');

//取消保存按钮的OnPreSaved事件
$("#aSave").removeAttr('onclick');

//项目阶段的变更对字段的影响
$('#ddList_CProject_1_ProjectStage').change(function () {
    var NoRedStarHtml = '<SPAN></SPAN>';
    var redStarHtml = '<SPAN style="COLOR:red;">*</SPAN>';

    //合同签署时，部分字段必填
    if ($('#ddList_CProject_1_ProjectStage').val() == '050') {
        //客户名称
        $('#label_CProject_1_ProjectClient').next().html(redStarHtml);
        //发货日（开始
        $('#label_CProject_1_FirstDeliveryDate').next().html(redStarHtml);
        //发货日（结束）
        $('#label_CProject_1_LastDeliveryDate').next().html(redStarHtml);
        //项目类型 
        $('#label_CProject_1_PojectType').next().html(redStarHtml);
        //建筑类型 
        $('#label_CProject_1_BuildingType').next().html(redStarHtml);
        //项目信息来源 
        $('#label_CProject_1_ProjectSource').next().html(redStarHtml);
    } else {
        //客户名称
        $('#label_CProject_1_ProjectClient').next().html(NoRedStarHtml);
        //发货日（开始
        $('#label_CProject_1_FirstDeliveryDate').next().html(NoRedStarHtml);
        //发货日（结束）
        $('#label_CProject_1_LastDeliveryDate').next().html(NoRedStarHtml);
        //项目类型 
        $('#label_CProject_1_PojectType').next().html(NoRedStarHtml);
        //建筑类型 
        $('#label_CProject_1_BuildingType').next().html(NoRedStarHtml);
        //项目信息来源 
        $('#label_CProject_1_ProjectSource').next().html(NoRedStarHtml);

        $('#dbTxt_CProject_1_ProjectClient').css('background-color', '');
        $('#txt_CProject_1_FirstDeliveryDate').css('background-color', '');
        $('#txt_CProject_1_LastDeliveryDate').css('background-color', '');
        $('#ddList_CProject_1_PojectType').css('background-color', '');
        $('#ddList_CProject_1_BuildingType').css('background-color', '');
        $('#ddList_CProject_1_ProjectSource').css('background-color', '');
    }

    //项目丢失时填写丢失原因
    if ($('#ddList_CProject_1_ProjectStage').val() == '060') {
        $('#label_CProject_1_LostReason').next().html(redStarHtml);
        $('#label_CProject_1_LostDescription').next().html(redStarHtml);
    } else {
        $('#label_CProject_1_LostReason').next().html(NoRedStarHtml);
        $('#label_CProject_1_LostDescription').next().html(NoRedStarHtml);
        $('#ddList_CProject_1_LostReason').css('background-color', '');
        $('#txt_CProject_1_LostDescription').css('background-color', '');
    }

});

//保存按钮点时需要先校验字段必填性
$("#aSave").click(function (e) {
    var isCanSave = true;
    //合同签署时，部分字段必填
    if ($('#ddList_CProject_1_ProjectStage').val() == '050') {
        if ($('#dbTxt_CProject_1_ProjectClient').val() == '') {
            $('#dbTxt_CProject_1_ProjectClient').css('background-color', 'red');
            isCanSave = false;
        }
        if ($('#txt_CProject_1_FirstDeliveryDate').val() == '') {
            $('#txt_CProject_1_FirstDeliveryDate').css('background-color', 'red');
            isCanSave = false;
        }
        if ($('#txt_CProject_1_LastDeliveryDate').val() == '') {
            $('#txt_CProject_1_LastDeliveryDate').css('background-color', 'red');
            isCanSave = false;
        }
        if ($('#ddList_CProject_1_PojectType').val() == '') {
            $('#ddList_CProject_1_PojectType').css('background-color', 'red');
            isCanSave = false;
        }
        if ($('#ddList_CProject_1_BuildingType').val() == '') {
            $('#ddList_CProject_1_BuildingType').css('background-color', 'red');
            isCanSave = false;
        }
        if ($('#ddList_CProject_1_ProjectSource').val() == '') {
            $('#ddList_CProject_1_ProjectSource').css('background-color', 'red');
            isCanSave = false;
        }
    } else if ($('#ddList_CProject_1_ProjectStage').val() == '060') {
        if ($('#ddList_CProject_1_LostReason').val() == '') {
            $('#ddList_CProject_1_LostReason').css('background-color', 'red');
            isCanSave = false;
        }
        if ($('#txt_CProject_1_LostDescription').val() == '') {
            $('#txt_CProject_1_LostDescription').css('background-color', 'red');
            isCanSave = false;
        }
    }

    if (!isCanSave) {
        alert('红色标记处的字段必填!');
    }

    return isCanSave;
});