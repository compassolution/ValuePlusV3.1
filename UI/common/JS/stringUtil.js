/*
******************************************
                        字符串函数扩充                                 
******************************************
*/

//js把数字转化为字母（A, B.....AA, AB,..）
function createCellPos(n) {
    var ordA = 'A'.charCodeAt(0);
    var ordZ = 'Z'.charCodeAt(0);
    var len = ordZ - ordA + 1;
    var s = "";
    while (n >= 0) {

        s = String.fromCharCode(n % len + ordA) + s;

        n = Math.floor(n / len) - 1;

    }
    return s;
}

//   判断是否是整数 
function   is_int(field){ 
    var   Ret   =   true; 
    var   NumStr= "0123456789 "; 
    var   chr; 

    for   (i=0;i <field.length;++i) 
    { 
        chr=field.charAt(i); 
    if   (NumStr.indexOf(chr,0)==-1) 
    { 
        Ret=false; 
    } 
    } 
    return(Ret); 
}

//   判断是否是数值型 
function isAValue(value) 
{   var bIs = false;
    if(value/1==value){
        bIs = true;
    }
    return bIs;
}
//   判断是否是数值型 (如果有逗号也验证成功)
function isAValueIncludeDot(value) 
{   var bIs = false;
    value = value.toString().replace(',','')
    if(value/1==value){
        bIs = true;
    }
    return bIs;
}

//   判断是否是正数值型 
function isPositiveNumber(value) 
{ 
    var   Ret   =   true; 
    var   NumStr= "0123456789."; 
    var   chr; 

    for   (i=0;i <value.length;++i) 
    { 
        chr=value.charAt(i); 
        if (NumStr.indexOf(chr,0)<0) 
        { 
            Ret=false; 
            break;
        } 
    } 
    return Ret; 
    
}
        
//判断输入的字符是否存在与某个特定字符数组中 
function isFixDayCol(col){ 
    var Ret = true; 
    var fixDayCol =new Array("D1","D2","D3","D4","D5","D6","D7","D8","D9","D10","D11","D12","D13","D14","D15","D16","D17","D18","D19","D20","D21","D22","D23","D24","D25","D26","D27","D28","D29","D30","D31");
    var chr; 

    if(fixDayCol.indexOf(col)< 0) {
        Ret = false;
    } 
    return(Ret); 
}

//根据特定输入的日期判断是否为周末 (暂时不用)
///yearmonth:yymm
///day:dd
function isWeekEnd(yearmonth,day){  
    var Ret = 0; 
    
    var strDate = "20"+yearmonth.substring(0,2)+"/"+yearmonth.substring(2,4)+"/"+day;
    var val=Date.parse(strDate);
    var newDate=new Date(strDate);
//    var newDate = new Date(Date.parse(strDate.replace(/-/g,   "/"))); 
    
//    if(newDate.getDay()== 0) {//周日
//        Ret = true;
//    }else if(newDate.getDay()== 6) {//周六
//        Ret = true;
//    }
    return(newDate.getDay()); 
}


//将时间字符串转化成特定的字符串输出 (YYMMDD)
function getDateStr1(strDate){  
    var retStr = ""; 
    var newDate=new Date(strDate);
//    var newDate = new Date(Date.parse(strDate.replace(/-/g,   "/"))); 
    
    var iMonth = newDate.getMonth() +1;
    retStr = newDate.getFullYear().toString()+"/"+iMonth.toString()+"/"+newDate.getDate().toString();
    return(retStr); 
}

/*
===========================================
//判断输入的整数是奇数还是偶数
===========================================

*/
String.prototype.IsOdevity = function()
{
        var a = prompt("请输入整数：");
        return a % 2;
        
}

/*
===========================================
//去除左边的空格
===========================================

*/
String.prototype.LTrim = function()
{
        return this.replace(/(^\s*)/g, "");
}


/*
===========================================
//去除右边的空格
===========================================
*/
String.prototype.Rtrim = function()
{
        return this.replace(/(\s*$)/g, "");
}

 

/*
===========================================
//去除前后空格
===========================================
*/
String.prototype.Trim = function()
{
        return this.replace(/(^\s*)|(\s*$)/g, "");
}

/*
===========================================
//得到左边的字符串
===========================================
*/
String.prototype.Left = function(len)
{

        if(isNaN(len)||len==null)
        {
                len = this.length;
        }
        else
        {
                if(parseInt(len)<0||parseInt(len)>this.length)
                {
                        len = this.length;
                }
        }
        
        return this.substr(0,len);
}


/*
===========================================
//得到右边的字符串
===========================================
*/
String.prototype.Right = function(len)
{

        if(isNaN(len)||len==null)
        {
                len = this.length;
        }
        else
        {
                if(parseInt(len)<0||parseInt(len)>this.length)
                {
                        len = this.length;
                }
        }
        
        return this.substring(this.length-len,this.length);
}


/*
===========================================
//得到中间的字符串,注意从0开始
===========================================
*/
String.prototype.Mid = function(start,len)
{
        return this.substr(start,len);
}


/*
===========================================
//在字符串里查找另一字符串:位置从0开始
===========================================
*/
String.prototype.InStr = function(str)
{

        if(str==null)
        {
                str = "";
        }
        
        return this.indexOf(str);
}

/*
===========================================
//在字符串里反向查找另一字符串:位置0开始
===========================================
*/
String.prototype.InStrRev = function(str)
{

        if(str==null)
        {
                str = "";
        }
        
        return this.lastIndexOf(str);
}

 

/*
===========================================
//计算字符串打印长度
===========================================
*/
String.prototype.LengthW = function()
{
        return this.replace(/[^\x00-\xff]/g,"**").length;
}

/*
===========================================
//是否是正确的IP地址
===========================================
*/
String.prototype.isIP = function()
{

        var reSpaceCheck = /^(\d+)\.(\d+)\.(\d+)\.(\d+)$/;

        if (reSpaceCheck.test(this))
        {
                this.match(reSpaceCheck);
                if (RegExp.$1 <= 255 && RegExp.$1 >= 0 
                 && RegExp.$2 <= 255 && RegExp.$2 >= 0 
                 && RegExp.$3 <= 255 && RegExp.$3 >= 0 
                 && RegExp.$4 <= 255 && RegExp.$4 >= 0) 
                {
                        return true;     
                }
                else
                {
                        return false;
                }
        }
        else
        {
                return false;
        }
   
}


/*
===========================================
//是否是正确的长日期
===========================================
*/
String.prototype.isLongDate = function()
{
        var r = this.replace(/(^\s*)|(\s*$)/g, "").match(/^(\d{1,4})(-|\/)(\d{1,2})\2(\d{1,2}) (\d{1,2}):(\d{1,2}):(\d{1,2})$/); 
        if(r==null)
        {
                return false; 
        }
        var d = new Date(r[1], r[3]-1,r[4],r[5],r[6],r[7]); 
        return (d.getFullYear()==r[1]&&(d.getMonth()+1)==r[3]&&d.getDate()==r[4]&&d.getHours()==r[5]&&d.getMinutes()==r[6]&&d.getSeconds()==r[7]);

}

/*
===========================================
//是否是正确的短日期
===========================================
*/
String.prototype.isShortDate = function()
{
        var r = this.replace(/(^\s*)|(\s*$)/g, "").match(/^(\d{1,4})(-|\/)(\d{1,2})\2(\d{1,2})$/); 
        if(r==null)
        {
                return false; 
        }
        var d = new Date(r[1], r[3]-1, r[4]); 
        return (d.getFullYear()==r[1]&&(d.getMonth()+1)==r[3]&&d.getDate()==r[4]);
}

/*
===========================================
//是否是正确的日期
===========================================
*/
String.prototype.isDate = function()
{
        return this.isLongDate()||this.isShortDate();
}

/*
===========================================
//是否是手机
===========================================
*/
String.prototype.isMobile = function()
{
        return /^0{0,1}13[0-9]{9}$/.test(this);
}

/*
===========================================
//是否是邮件
===========================================
*/
String.prototype.isEmail = function()
{
        return /^\w+((-\w+)|(\.\w+))*\@[A-Za-z0-9]+((\.|-)[A-Za-z0-9]+)*\.[A-Za-z0-9]+$/.test(this);
}

/*
===========================================
//是否是邮编(中国)
===========================================
*/

String.prototype.isZipCode = function()
{
        return /^[\\d]{6}$/.test(this);
}

/*
===========================================
//是否是有汉字
===========================================
*/
String.prototype.existChinese = function()
{
        //[\u4E00-\u9FA5]為漢字﹐[\uFE30-\uFFA0]為全角符號
        return /^[\x00-\xff]*$/.test(this);
}

/*
===========================================
//是否是合法的文件名/目录名
===========================================
*/
String.prototype.isFileName = function()
{
        return !/[\\\/\*\?\|:"<>]/g.test(this);
}

/*
===========================================
//是否是有效链接
===========================================
*/
String.prototype.isUrl = function()
{
        return /^http[s]?:\/\/([\w-]+\.)+[\w-]+([\w-./?%&=]*)?$/i.test(this);
}


/*
===========================================
//是否是有效的身份证(中国)
===========================================
*/
String.prototype.isIDCard = function()
{
        var iSum=0;
        var info="";
        var sId = this;

//        var aCity={11:"北京",12:"天津",13:"河北",14:"山西",15:"内蒙古",21:"辽宁",22:"吉林",23:"黑龙江",31:"上海",32:"江苏",33:"浙江",34:"安徽",35:"福建",36:"江西",37:"山东",41:"河南",42:"湖北",43:"湖南",44:"广东",45:"广西",46:"海南",50:"重庆",51:"四川",52:"贵州",53:"云南",54:"西藏",61:"陕西",62:"甘肃",63:"青海",64:"宁夏",65:"新疆",71:"台湾",81:"香港",82:"澳门",91:"国外"};
        var aCity={};
        if(!/^\d{17}(\d|x)$/i.test(sId))
        {
                return false;
        }
        sId=sId.replace(/x$/i,"a");
        //非法地区
        if(aCity[parseInt(sId.substr(0,2))]==null)
        {
                return false;
        }

        var sBirthday=sId.substr(6,4)+"-"+Number(sId.substr(10,2))+"-"+Number(sId.substr(12,2));

        var d=new Date(sBirthday.replace(/-/g,"/"))
        
        //非法生日
        if(sBirthday!=(d.getFullYear()+"-"+ (d.getMonth()+1) + "-" + d.getDate()))
        {
                return false;
        }
        for(var i = 17;i>=0;i--) 
        {
                iSum += (Math.pow(2,i) % 11) * parseInt(sId.charAt(17 - i),11);
        }

        if(iSum%11!=1)
        {
                return false;
        }
        return true;

}

/*
===========================================
//是否是有效的电话号码(中国)
===========================================
*/
String.prototype.isPhoneCall = function()
{
        return /(^[0-9]{3,4}\-[0-9]{3,8}$)|(^[0-9]{3,8}$)|(^\([0-9]{3,4}\)[0-9]{3,8}$)|(^0{0,1}13[0-9]{9}$)/.test(this);
}


/*
===========================================
//是否是数字
===========================================
*/
String.prototype.isNumeric = function(flag)
{
        //验证是否是数字
        if(isNaN(this))
        {

                return false;
        }

        switch(flag)
        {

                case null:        //数字
                case "":
                        return true;
                case "+":        //正数
                        return                /(^\+?|^\d?)\d*\.?\d+$/.test(this);
                case "-":        //负数
                        return                /^-\d*\.?\d+$/.test(this);
                case "i":        //整数
                        return                /(^-?|^\+?|\d)\d+$/.test(this);
                case "+i":        //正整数
                        return                /(^\d+$)|(^\+?\d+$)/.test(this);                        
                case "-i":        //负整数
                        return                /^[-]\d+$/.test(this);
                case "f":        //浮点数
                        return                /(^-?|^\+?|^\d?)\d*\.\d+$/.test(this);
                case "+f":        //正浮点数
                        return                /(^\+?|^\d?)\d*\.\d+$/.test(this);                        
                case "-f":        //负浮点数
                        return                /^[-]\d*\.\d$/.test(this);                
                default:        //缺省
                        return true;                        
        }
}

/*
===========================================
//是否是颜色(#FFFFFF形式)
===========================================
*/
String.prototype.IsColor = function()
{
        var temp        = this;
        if (temp=="") return true;
        if (temp.length!=7) return false;
        return (temp.search(/\#[a-fA-F0-9]{6}/) != -1);
}

/*
===========================================
//转换成全角
===========================================
*/
String.prototype.toCase = function()
{
        var tmp = "";
        for(var i=0;i<this.length;i++)
        {
                if(this.charCodeAt(i)>0&&this.charCodeAt(i)<255)
                {
                        tmp += String.fromCharCode(this.charCodeAt(i)+65248);
                }
                else
                {
                        tmp += String.fromCharCode(this.charCodeAt(i));
                }
        }
        return tmp
}

/*
===========================================
//对字符串进行Html编码
===========================================
*/
String.prototype.toHtmlEncode = function()
{
        var str = this;

        str=str.replace(/&/g,"&amp;");
        str=str.replace(/</g,"&lt;");
        str=str.replace(/>/g,"&gt;");
        str=str.replace(/\'/g,"&apos;");
        str=str.replace(/\"/g,"&quot;");
        str=str.replace(/\n/g,"<br>");
        str=str.replace(/\ /g,"&nbsp;");
        str=str.replace(/\t/g,"&nbsp;&nbsp;&nbsp;&nbsp;");

        return str;
}

/*
===========================================
//转换成日期
===========================================
*/
String.prototype.toDate = function()
{
        try
        {
                return new Date(this.replace(/-/g, "\/"));
        }
        catch(e)
        {
                return null;
        }
}


/*
===========================================
//全替换字符串
===========================================
*/
String.prototype.ReplaceAll = function(s1,s2) { 
    return this.replace(new RegExp(s1,"gm"),s2); 
}


//日期加上天数后的新日期.
///输出格式yyyy-MM-dd
function AddDays(date, days) {
    //var temp = new Date(date).format("yyyy/MM/dd");
    //alert(typeof date);
    var nd;
    if (typeof date == 'string') {
        nd = new Date(date.replace(/[-]/g, "/"));
    } else {
        nd = new Date(date);
    }
    nd = nd.valueOf();
    nd = nd + days * 24 * 60 * 60 * 1000;
    nd = new Date(nd);
    //alert(nd.getFullYear() + "年" + (nd.getMonth() + 1) + "月" + nd.getDate() + "日");
    var y = nd.getFullYear();
    var m = nd.getMonth() + 1;
    var d = nd.getDate();
    if (m <= 9) m = "0" + m;
    if (d <= 9) d = "0" + d;
    var cdate = y + "-" + m + "-" + d;
    return cdate;
}

//日期加上月数后的新日期.
///输出格式yyyy-MM-dd
function AddMonths(date, months) {
    var nd;
    if (typeof date == 'string') {
        nd = new Date(date.replace(/[-]/g, "/"));
    } else {
        nd = new Date(date);
    }
    nd.setMonth(nd.getMonth() + months);
    //alert(nd.getFullYear() + "年" + (nd.getMonth() + 1) + "月" + nd.getDate() + "日");
    var y = nd.getFullYear();
    var m = nd.getMonth() + 1;
    var d = nd.getDate();
    if (m <= 9) m = "0" + m;
    if (d <= 9) d = "0" + d;
    var cdate = y + "-" + m + "-" + d;
    return cdate;
}

//日期加上年数后的新日期.
///输出格式yyyy-MM-dd
function AddYears(date, years) {
    var nd;
    if (typeof date == 'string') {
        nd = new Date(date.replace(/[-]/g, "/"));
    } else {
        nd = new Date(date);
    }
    nd.setFullYear(nd.getFullYear() + years);
    //alert(nd.getFullYear() + "年" + (nd.getMonth() + 1) + "月" + nd.getDate() + "日");
    var y = nd.getFullYear();
    var m = nd.getMonth() + 1;
    var d = nd.getDate();
    if (m <= 9) m = "0" + m;
    if (d <= 9) d = "0" + d;
    var cdate = y + "-" + m + "-" + d;

    //nd.setFullYear(nd.getFullYear() + years);
    //nd.setDate(nd.getDate() - 1);
    //var cdate = nd.format("yyyy-MM-dd");

    return cdate;
}

Date.prototype.format = function (fmt) {
    var o = {
        "M+": this.getMonth() + 1,                 //月份 
        "d+": this.getDate(),                    //日 
        "h+": this.getHours(),                   //小时 
        "m+": this.getMinutes(),                 //分 
        "s+": this.getSeconds(),                 //秒 
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度 
        "S": this.getMilliseconds()             //毫秒 
    };
    if (/(y+)/.test(fmt)) {
        fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    }
    for (var k in o) {
        if (new RegExp("(" + k + ")").test(fmt)) {
            fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
        }
    }
    return fmt;
}

//生成guid
function NewClientGuid() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
}