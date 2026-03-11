
///使用插件Layer的等待层
var layerLoadWaiting;

function ShowLoadingLevel1() {
    layerLoadWaiting = layer.load('Loading');
}

function HideLoadingLevel1() {
    layer.close(layerLoadWaiting);
}

function ShowLoadingLevel2() {
    layerLoadWaiting = parent.layer.load('Loading');
}

function HideLoadingLevel2() {
    parent.layer.close(layerLoadWaiting);
}

function ShowLoadingLevel3() {
    layerLoadWaiting = window.parent.parent.layer.load('Loading');
}

function HideLoadingLevel3() {
    window.parent.parent.layer.close(layerLoadWaiting);
}