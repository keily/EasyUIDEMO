function Msgshow(msg) {
    $.messager.show({
        title: '提示',
        msg: msg,
        showType: 'show'
    });
}
function Msgslide(msg) {
    $.messager.show({
        title: '提示',
        msg: msg,
        timeout: 3000,
        showType: 'slide'
    });
}
function Msgfade(msg) {
    $.messager.show({
        title: '提示',
        msg: msg,
        timeout: 3000,
        showType: 'fade'
    });
}
//弹出信息窗口 title:标题 msgString:提示信息 msgType:信息类型 [error,info,question,warning]
function Msgalert(title, msgString, msgType) {
    $.messager.alert(title, msgString, msgType);
}
///当提交信息的触发
var successCallback = function (result) {
    //result为请求处理后的返回值    
    var result = eval('(' + result + ')');
    if (result.success) {
        $.messager.show({
            title: '成功',
            msg: result.msg,
            timeout: 2000,
            showType: 'fade'
        });
        grid.datagrid('reload'); //grid变量要与其他文件中的一致
        dlg_Edit.dialog('close'); //dlg_Edit与其他文件中的一致  才能公共调用 
        
        
    } else {
        $.messager.show({
            title: '错误提示',
            msg: result.msg
        });
    }
}