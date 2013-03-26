// 全局变量
var grid;
var dlg_Edit;
var dlg_Edit_form;
var searchWin;
var searchForm;
var virpath = ""; //网站的虚拟目录 如：/ShopManager


$(function () {
    grid = $('#grid').datagrid({
        title: '产品类型管理',
        iconCls: 'icon-save',
        methord: 'post',
        url: '/Admin/LoadProductTypejson/',
        sortName: 'ID',
        sortOrder: 'desc',
        idField: 'ID',
        pageSize: 30,
        striped:true,
        frozenColumns: [[
	                { field: 'ck', checkbox: true }

				]],
        columns: [[
					{ field: 'ID', title: '产品类型编号', width: 100, sortable: true },
					{ field: 'ProductTypeName', title: '产品类型名称', width: 150, sortable: true },
                    { field: 'Description', title: '描述', width: 150, sortable: true }
                   
				]],
        fit: true,
        pagination: true,
        rownumbers: true,
        fitColumns: true,
        singleSelect: false,
        toolbar: [{
            text: '新增',
            iconCls: 'icon-add',
            handler: add
        }, '-', {
            text: '修改',
            iconCls: 'icon-edit',
            handler: edit
        }, '-', {
            text: '删除',
            iconCls: 'icon-remove',
            handler: del
        }, '-', {
            text: '查找',
            iconCls: 'icon-search',
            handler: OpensearchWin
        }, '-', {
            text: '所有',
            iconCls: 'icon-search',
            handler: showAll
        }], onDblClickRow: function (rowIndex, rowData) {  //双击事件
            dlg_Edit.dialog('open');
            dlg_Edit_form.form('clear');
            dlg_Edit.dialog('setTitle', '您正在修改:' + rowData.ProductTypeName );
            dlg_Edit_form.form('load', rowData); //加载到表单的控件上      

            dlg_Edit_form.url = virpath + '/Admin/UpdateProductType?id=' + rowData.ID;

        }, onHeaderContextMenu: function (e, field) {
            e.preventDefault();
            if (!$('#tmenu').length) {
                createColumnMenu();
            }
            $('#tmenu').menu('show', {
                left: e.pageX,
                top: e.pageY
            });
        }

    });

    dlg_Edit = $('#Dlg-Edit').dialog({
        closed: true,
        modal: true,
        toolbar: [{
            text: '保存',
            iconCls: 'icon-save',
            handler: saveData
        }, '-', {
            text: '关闭',
            iconCls: 'icon-no',
            handler: function () {
                dlg_Edit.dialog('close');
            }
        }]

    });
    dlg_Edit_form = dlg_Edit.find('form');

    $('#btn-search,#btn-search-cancel').linkbutton();
    searchWin = $('#search-window').window({
        iconCls: 'icon-search',
        closed: true,
        modal: true
    });
    searchForm = searchWin.find('form');



    $('body').layout();



});


function createColumnMenu() {
    var tmenu = $('<div id="tmenu" style="width:100px;"></div>').appendTo('body');
    var fields = grid.datagrid('getColumnFields');
    for (var i = 0; i < fields.length; i++) {
        $('<div iconCls="icon-ok"/>').html(fields[i]).appendTo(tmenu);
    }
    tmenu.menu({
        onClick: function (item) {
            if (item.iconCls == 'icon-ok') {
                grid.datagrid('hideColumn', item.text);
                tmenu.menu('setIcon', {
                    target: item.target,
                    iconCls: 'icon-empty'
                });
            } else {
                grid.datagrid('showColumn', item.text);
                tmenu.menu('setIcon', {
                    target: item.target,
                    iconCls: 'icon-ok'
                });
            }
        }
    });
}

function getSelectedArr() {
    var ids = [];
    var rows = grid.datagrid('getSelections');
    for (var i = 0; i < rows.length; i++) {
        ids.push(rows[i].ID);
    }
    return ids;
}
function getSelectedID() {
    var ids = getSelectedArr();
    return ids.join(',');
}
function arr2str(arr) {
    return arr.join(',');
}

function add() {
    dlg_Edit_form.form('clear');
    dlg_Edit.dialog('open');
    dlg_Edit.dialog('setTitle', '添加产品类型');
   
    dlg_Edit_form.url = virpath + '/Admin/CreateProductType/';
    //Msgalert("提示", '成功调用', "info");    
}
function edit() {
    var rows = grid.datagrid('getSelections');
    var num = rows.length;
    if (num == 0) {
        Msgshow('请选择一条记录进行操作!');
        //$.messager.alert('提示', '请选择一条记录进行操作!', 'question');       
        return;
    }
    else if (num > 1) {
        Msgfade('您选择了多条记录,只能选择一条记录进行修改!'); //$.messager.alert('提示', '您选择了多条记录,只能选择一条记录进行修改!', 'info');
        return;
    }
    else {
        dlg_Edit_form.form('clear');
        dlg_Edit.dialog('open');
        dlg_Edit.dialog('setTitle', '您正在修改' + rows[0].ProductTypeName + '游戏');
        dlg_Edit_form.form('load', rows[0]); //加载到表单的控件上       
        
        dlg_Edit_form.url = virpath + '/Admin/UpdateProductType?id=' + rows[0].ID;

    }
}
function del() {
    var arr = getSelectedArr();
    //alert(arr);
    if (arr.length > 0) {
        $.messager.confirm('提示信息', '您确认要删除选中的所有记录吗?', function (data) {
            if (data) {
                $.ajax({
                    url: virpath + '/Admin/RemeProductTypes?ids=' + arr2str(arr),
                    type: 'post',
                    error: function () {
                        Msgalert('错误', '删除失败!', 'error');
                        grid.datagrid('clearSelections');
                    },
                    success: function (Result) {
                        var data = eval('(' + Result + ')');
                        if (data.success) {
                            Msgfade(arr.length+"条记录"+data.msg); //提示消息
                            grid.datagrid('reload');
                            grid.datagrid('clearSelections');
                        } else {
                            Msgalert('错误', data.msg, 'error');
                        }
                    }
                });
            }
        });
    } else {
        Msgshow('请先选择要删除的记录。');

    }
}
function saveData() {
    //alert(dlg_Edit_form.url);
    dlg_Edit_form.form('submit', {
        url: dlg_Edit_form.url,
        onSubmit: function () {
            return $(this).form('validate');
        },
        success: successCallback   //~/Content/JqueryUI/MyEasyUIcommmon.js
    });
}


function showAll() {
    grid.datagrid({ url: virpath + '/Admin/LoadProductTypejson/' });
}
function OpensearchWin() {
    searchWin.window('open');
    searchForm.form('clear');
}

function SearchOK() {
    var ProductTypeName = $("#Name").val();
    searchWin.window('close');
    grid.datagrid({ url: virpath + '/Admin/SeachProductTypeInfo/', queryParams: { Name: ProductTypeName} });
}
function closeSearchWindow() {
    searchWin.window('close');
}