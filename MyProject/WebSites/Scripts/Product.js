// 全局变量
var grid;
var dlg_Edit;
var dlg_Edit_form;
var searchWin;
var searchForm;
var virpath = ""; //网站的虚拟目录 如：/ShopManager


$(function () {
    grid = $('#grid').datagrid({
        title: '产品管理',
        iconCls: 'icon-save',
        methord: 'post',
        url: '/Admin/LoadProductjson/',
        sortName: 'ID',
        sortOrder: 'desc',
        idField: 'ID',
        pageSize: 20,
        striped: true, //奇偶行是否区分
        frozenColumns: [[
	                { field: 'ck', checkbox: true }

				]],
        columns: [[
					{ field: 'ID', title: '编号', width: 50, sortable: true, rowspan: 2 },
                    { field: 'Image', title: '产品图片', width: 60, rowspan: 2, align: 'center', formatter: function (value, row, index) {
                        
                        return "<img src='" + row.Image + "' alt='" + row.ProductName + "' width='60px',higth='60px' />";
                    }

                    },
                    { field: 'ProductTypeName', title: '所属类型', width: 100, sortable: true, rowspan: 2 },
                    { field: 'ProductName', title: '产品名称', width: 150, sortable: true, rowspan: 2 },
                    { title: '价格(单位：元)', colspan: 2 },
                    { field: 'GetDate', title: '录入/修改时间', width: 120, sortable: true,align: 'center', rowspan: 2 },
                    { field: 'Enable', title: '状态', width: 50, rowspan: 2, formatter: function (value, row, index) {
                        if (row.Enable) {

                            return "出售中";
                        } else {

                            return "已下架";
                        }
                    }, styler: function (value, row, index) {
                        if (!row.Enabled) {
                            return 'background-color:#ffee00;color:red;';
                        }
                    }

                    }

				], [{ field: 'MarketPrice', title: '市场价', width: 80, sortable: true },
                    { field: 'NewPrice', title: '真实价', width: 80, sortable: true}]
                    ],
        //        onLoadSuccess: function () {
        //            var merges = [{
        //                index: 2,
        //                rowspan: 2
        //            }, {
        //                index: 5,
        //                rowspan: 2
        //            }, {
        //                index: 7,
        //                rowspan: 2
        //            }];
        //            for (var i = 0; i < merges.length; i++)
        //                $('#grid').datagrid('mergeCells', {
        //                    index: merges[i].index,
        //                    field: 'GameName',
        //                    rowspan: merges[i].rowspan
        //                });
        //        },
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
            text: '高级搜索',
            iconCls: 'icon-search',
            handler: OpensearchWin
        }, '-', {
            text: '所有',
            iconCls: 'icon-search',
            handler: showAll
        }], onDblClickRow: function (rowIndex, rowData) {  //双击事件
            dlg_Edit.dialog('open');
            dlg_Edit_form.form('clear'); //清除之前面选中的表单
            dlg_Edit.dialog('setTitle', '您正在查看的是：' + rowData.ProductTypeName + "->" + rowData.ProductName );

            dlg_Edit_form.form('load', rowData); //加载到表单的控件上  

            dlg_Edit_form.url = virpath + '/Admin/UpdateProduct?id=' + rowData.ID;

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
    dlg_Edit.dialog('setTitle', '添加产品信息');
    //$("#ProductId2").removeAttr("readonly"); //移除只读   
    $('#Enable').combobox('setValue', true); //默认设置开启

    dlg_Edit_form.url = virpath + '/Admin/CreateProduct/';
    Msgalert("提示", '成功调用', "info");    
}
function edit() {
    var rows = grid.datagrid('getSelections');
    var num = rows.length;
    if (num == 0) {
        Msgshow('请选择一条记录进行操作!');
        return;
    }
    else if (num > 1) {
        Msgfade('您选择了多条记录,只能选择一条记录进行修改!'); //$.messager.alert('提示', '您选择了多条记录,只能选择一条记录进行修改!', 'info');
        return;
    }
    else {
        //alert(rows[0].ProductId);
        dlg_Edit_form.form('clear');
        dlg_Edit.dialog('open');
        dlg_Edit.dialog('setTitle', '您正在修改的是:' + rows[0].ProductTypeName + "->" + rows[0].ProductName );

        dlg_Edit_form.form('load', rows[0]); //加载到表单的控件上 
     
      //  $("#tishi").html("禁止修改");

        dlg_Edit_form.url = virpath + '/Admin/UpdateProduct?id=' + rows[0].ID;

    }
}
function del() {
    var arr = getSelectedArr();
    alert(arr);
    if (arr.length > 0) {
        $.messager.confirm('提示信息', '您确认要删除选中的记录吗?', function (data) {
            if (data) {
                $.ajax({
                    url: virpath + '/Admin/RemeProducts?ids=' + arr2str(arr),
                    type: 'post',
                    error: function () {
                        Msgalert('错误', '删除失败!', 'error');
                        grid.datagrid('clearSelections');
                    },
                    success: function (re) {
                        var data = eval('(' + re + ')');
                        if (data.success) {
                            Msgfade(arr.length + "条记录" + data.msg); //提示消息
                            grid.datagrid('reload');
                            grid.datagrid('clearSelections'); //清除所有选中的元素
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
        success: successCallback
    });
}


function showAll() {
    grid.datagrid({ url: virpath + '/Admin/LoadProductjson/' });
}
function OpensearchWin() {
    searchWin.window('open');
    searchForm.form('clear');
    $('#Enable2').combobox('setValue', true); //默认设置开启搜素
}

function SearchOK() {
    var Name = $("#Name").val();
    var Id = $("#TypeId2").combobox("getValue");   
    var bl = $('#Enable2').combobox('getValue'); //默认设置开启搜素
    var PId = $("#pid").val(); //ID
    var Price = $("#Price").val(); //价格
    //    alert(Name + "==" + Id + "==" + MId + "==" + bl + "==" + CId+"=="+Code);
    searchWin.window('close');
    grid.datagrid({ url: virpath + '/Admin/SeachProductInfo/', queryParams: { ProductName: Name, typeId: Id, ProductId: PId, RealPrice: Price, en: bl} });
    //经过测试queryParams参数可以提交到后台通过FormCollection获取 也可以Request["ProductName"]=?获取
}
function closeSearchWindow() {
    searchWin.window('close');
}