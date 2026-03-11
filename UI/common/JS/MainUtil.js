/**
 * Edge/Chrome浏览器对showModalDialog兼容处理
 * 该特性已从Web标准中删除
 */
if (!window.showModalDialog) {
    window.showModalDialog = function (uri, args, opts) {
        opts = opts.replace(/:/g, '=')
            .replace(/;/g, ',')
            .replace('dialogWidth', 'width')
            .replace('dialogHeight', 'height')
            .replace('dialogtop', 'top')
            .replace('dialogleft', 'left')
            .replace('scroll', 'scrollbars');
        window.open(uri, '', opts).dialogArguments = args;
    };
}
//获取本页地址中的传递参数值
function GetUrlQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var reg_rewrite = new RegExp("(^|/)" + name + "/([^/]*)(/|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    var q = window.location.pathname.substr(1).match(reg_rewrite);
    if (r != null) {
        return unescape(r[2]);
    } else if (q != null) {
        return unescape(q[2]);
    } else {
        return null;
    }
}

//判断浏览器
function myBrowser() {
    var userAgent = navigator.userAgent; //取得浏览器的userAgent字符串
    var isOpera = userAgent.indexOf("Opera") > -1; //判断是否Opera浏览器
    var isIE = !isOpera &&
        (userAgent.indexOf("compatible") > -1
        || userAgent.indexOf("MSIE") > -1
        || userAgent.indexOf("Trident") > -1); //判断是否IE浏览器
    var isEdge = userAgent.indexOf("Edg") > -1; //判断是否IE的Edge浏览器
    var isFF = userAgent.indexOf("Firefox") > -1; //判断是否Firefox浏览器
    var isSafari = userAgent.indexOf("Safari") > -1
        && userAgent.indexOf("Chrome") == -1; //判断是否Safari浏览器
    var isChrome = userAgent.indexOf("Chrome") > -1
        && userAgent.indexOf("Safari") > -1; //判断Chrome浏览器

    if (isIE) {
        return "IE";
    }
    if (isOpera) {
        return "Opera";
    }
    if (isEdge) {
        return "Edge";
    }
    if (isFF) {
        return "FF";
    }
    if (isSafari) {
        return "Safari";
    }
    if (isChrome) {
        return "Chrome";
    }

}