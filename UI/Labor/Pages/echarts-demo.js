$(function () {
    var lineChart1 = echarts.init(document.getElementById("echarts-line-chart1"));
    var lineoption1 = {
        title : {
            text: '近一周外包工数量(人/日)'
        },
        tooltip : {
            trigger: 'axis'
        },
        legend: {
            data:['餐饮部','客房部']
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
                data: ['05-06', '05-07', '05-08', '05-09', '05-10', '05-11', '05-12']
            }
        ],
        yAxis : [
            {
                type : 'value',
                axisLabel : {
                    formatter: '{value} 人'
                }
            }
        ],
        series : [
            {
                name: '餐饮部',
                type:'line',
                data:[10, 21, 25, 23, 32, 13, 40],
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
            },{
                name: '客房部',
                type:'line',
                data:[20, 11, 20, 13, 22, 33, 20],
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
            text: '近6个月外包工数量(人/月)'
        },
        tooltip: {
            trigger: 'axis'
        },
        legend: {
            data: ['餐饮部', '客房部']
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
                data: ['201912', '202001', '202002', '202003', '202004', '202005']
            }
        ],
        yAxis: [
            {
                type: 'value',
                axisLabel: {
                    formatter: '{value} 人'
                }
            }
        ],
        series: [
            {
                name: '餐饮部',
                type: 'line',
                data: [321, 325, 423, 232, 613, 440],
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
            }, {
                name: '客房部',
                type: 'line',
                data: [211, 420, 213, 122, 433, 520],
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
            text: '用工部门分布',
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
            data: ['餐饮部', '客房部']
        },
        calculable: true,
        series: [
            {
                name: '部门分布',
                type: 'pie',
                radius: '55%',
                center: ['50%', '60%'],
                data: [
                    { value: 335, name: '餐饮部' },
                    { value: 310, name: '客房部' }
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
