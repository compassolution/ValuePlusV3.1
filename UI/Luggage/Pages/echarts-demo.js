$(function () {
    var lineChart1 = echarts.init(document.getElementById("echarts-line-chart1"));
    var lineoption1 = {
        title : {
            text: '近30天寄存行李数量(件/日)'
        },
        tooltip : {
            trigger: 'axis'
        },
        legend: {
            data: ['寄存行李数']
        },
        grid:{
            x:40,
            x2:40,
            y2:24
        },
        calculable : true,
        xAxis : [
            {
                type : 'category',
                boundaryGap : false,
                data: ['04-13', '04-14','04-15', '04-16', '04-17', '04-18', '04-19', '04-20', '04-21'
                    , '04-22', '04-23', '04-24', '04-25', '04-26', '04-27', '04-28'
                    , '04-29', '04-30', '05-01', '05-02', '05-03', '05-04', '05-05'
                    ,'05-06', '05-07', '05-08', '05-09', '05-10', '05-11', '05-12']
            }
        ],
        yAxis : [
            {
                type : 'value',
                axisLabel : {
                    formatter: '{value} 件'
                }
            }
        ],
        series : [
            {
                name: '主题公园入口寄存点',
                type:'line',
                data: [100, 210, 100, 210, 250, 230, 320, 130, 400
                    , 100, 210, 250, 230, 320, 130, 400
                    , 100, 210, 250, 230, 320, 130, 400
                    , 100, 210, 250, 230, 320, 130, 400],
                markPoint : {
                    data : [
                        {type : 'max', name: '最大值'},
                        {type : 'min', name: '最小值'}
                    ]
                },
                markLine : {
                    data : [
                        {type : 'average', name: '平均值'}
                    ]
                }
            }
        ]
    };
    lineChart1.setOption(lineoption1);
    $(window).resize(lineChart1.resize);


    var lineChart2 = echarts.init(document.getElementById("echarts-line-chart2"));
    var lineoption2 = {
        title: {
            text: '近12个月行李寄存数量(件/月)'
        },
        tooltip: {
            trigger: 'axis'
        },
        legend: {
            data: ['行李寄存数']
        },
        grid: {
            x: 40,
            x2: 40,
            y2: 24
        },
        calculable: true,
        xAxis: [
            {
                type: 'category',
                boundaryGap: false,
                data: ['201906', '201907', '201908', '201909', '201910', '201911', '201912', '202001', '202002', '202003', '202004', '202005']
            }
        ],
        yAxis: [
            {
                type: 'value',
                axisLabel: {
                    formatter: '{value} 件'
                }
            }
        ],
        series: [
            {
                name: '行李寄存数',
                type: 'line',
                data: [2110, 4200, 2130, 1220, 4330, 5200,3210, 3250, 4230, 2320, 6130, 4400],
                markPoint: {
                    data: [
                        { type: 'max', name: '最大值' },
                        { type: 'min', name: '最小值' }
                    ]
                },
                markLine: {
                    data: [
                        { type: 'average', name: '平均值' }
                    ]
                }
            }
        ]
    };
    lineChart2.setOption(lineoption2);
    $(window).resize(lineChart2.resize);

    var pieChart1 = echarts.init(document.getElementById("echarts-pie-chart1"));
    var pieoption1 = {
        title: {
            text: '寄存点分布',
            //subtext: '纯属虚构',
            x: 'center'
        },
        tooltip: {
            trigger: 'item',
            formatter: "{a} <br/>{b} : {c} ({d}%)"
        },
        legend: {
            orient: 'vertical',
            x: 'left',
            data: ['主题公园入口寄存点', '主停车场寄存点', '地铁站寄存点', '环球大酒店行李房', '诺度假村行李房']
        },
        calculable: true,
        series: [
            {
                name: '部门分布',
                type: 'pie',
                radius: '55%',
                center: ['50%', '60%'],
                data: [
                    { value: 335, name: '主题公园入口寄存点' },
                    { value: 300, name: '主停车场寄存点' },
                    { value: 210, name: '地铁站寄存点' },
                    { value: 110, name: '环球大酒店行李房' },
                    { value: 210, name: '诺度假村行李房' }
                ]
            }
        ]
    };
    pieChart1.setOption(pieoption1);
    $(window).resize(pieChart1.resize);

    var pieChart2 = echarts.init(document.getElementById("echarts-pie-chart2"));
    var pieoption2 = {
        title: {
            text: '用工事项分布',
            //subtext: '纯属虚构',
            x: 'center'
        },
        tooltip: {
            trigger: 'item',
            formatter: "{a} <br/>{b} : {c} ({d}%)"
        },
        legend: {
            orient: 'vertical',
            x: 'left',
            data: ['餐饮服务', '客房打扫','楼道清洁']
        },
        calculable: true,
        series: [
            {
                name: '用工事项分布',
                type: 'pie',
                radius: '55%',
                center: ['50%', '60%'],
                data: [
                    { value: 335, name: '餐饮服务' },
                    { value: 310, name: '客房打扫' },
                    { value: 310, name: '楼道清洁' }
                ]
            }
        ]
    };
    pieChart2.setOption(pieoption2);
    $(window).resize(pieChart2.resize);

});
