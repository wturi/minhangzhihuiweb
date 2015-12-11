//获取url参数
function getUrlParam(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); //构造一个含有目标参数的正则表达式对象
    var r = window.location.search.substr(1).match(reg);  //匹配目标参数
    if (r != null) return unescape(r[2]); return null; //返回参数值
}

//点击添加按钮 弹出模态框 》》》 填写临时人员信息
$(".tianjia").click(function () {
    var tag = $(this).attr('tag');
    var id = $(this).attr('data-target');
    $(id).find('input').val('');
})

//填写信息 并 添加到表单
$(document).on('click', '.xixniqueren', function () {
    var tag = $(this).attr('tag');
    if (tag == '1') {
        PeopleInsert();
    } else if (tag == '2') {
        CarInsert();
    } else if (tag == '3') {
        ProductInsert();
    }
})

//删除已添加信息
$(document).on('click', '.shanchu', function () {
    var children = $(this).parent().parent().children();
    var input = $(this).parent().parent().parent().parent().parent().parent().parent().find('input[type="hidden"]');
    var str = '';
    for (var i = 0; i < children.length - 1; i++) {
        str += children.eq(i).text() + '|';
    }
    str = str.substring(0, str.length - 1);
    input.val($.trim(input.val().replace(str, '')));
    $(this).parent().parent().remove();

})

//临时人员资料 并 添加到页面
function PeopleInsert() {
    var id = $('#myModalPeople');
    var name = id.find('#PeopleName').val();
    var phone = id.find('#PeoplePhone').val();
    var PeopleCertificates = id.find('#PeopleCertificates').val();
    //var zhengjian = id.find('#zhengjian').is(':checked');
    var zhengjian = id.find('#PeopleCertificates').val();
    $('#Peopleid').val($('#Peopleid').val() + name + '|' + phone + '|' + zhengjian + ' ');
    var table = $('#Peopleid').parent().find('.table');
    var str = '<tr><td>' + name + '</td><td>' + phone + '</td><td>' + PeopleCertificates + '</td><td><a class="btn btn-warning shanchu">删除</a></td></tr>';
    table.append(str);
    id.find('input').val('');
}

//临时车辆资料 并 添加到页面
function CarInsert() {
    var id = $('#myModalCar');
    var licence = id.find('#CarLicence').val();
    var type = id.find('#CarType').val();
    $('#Carid').val($('#Carid').val() + licence + '|' + type + ' ');
    var table = $('#Carid').parent().find('.table');
    var str = '<tr><td>' + licence + '</td><td>' + type + '</td><td><a class="btn btn-warning shanchu">删除</a></td></tr>'
    table.append(str);
    id.find('input').val('');
}

//临时物品资料 并 添加到页面
function ProductInsert() {
    var id = $('#myModalProduct');
    var name = id.find('#ProductName').val();
    var genera = id.find('#ProductGenera').val();
    var number = id.find('#ProdutNumber').val();
    $('#Productid').val($('#Productid').val() + name + '|' + genera + '|' + number + ' ');
    var table = $('#Productid').parent().find('.table');
    var str = '<tr><td>' + name + '</td><td>' + genera + '</td><td>' + number + '</td><td><a class="btn btn-warning shanchu">删除</a></td></tr>'
    table.append(str);
    id.find('input').val('');
}

//弹出批准人选择框
$('#PeblishPeople').click(function () {
    $('#shou').parent().nextAll().remove();
    PiZhunRen(0)

})

//刷新部门 id:部门上级id
function PiZhunRen(id) {
    $('.bumen').empty();
    $('.renyuan').empty();
    $.post('PiZhunRensbumen/', { id: id }, function (data, status) {
        if (status == 'success') {
            var stra = '';
            $.each(data.BuMens, function (i, item) {
                stra += '<button class="col-lg-2 btn btn-info bumenbt" tag=" ' + item.BuMenId + ' "   >' + item.BuMenName + '</button>'
            })
            $('.bumen').append(stra);

            var strb = '';
            $.each(data.RenYuans, function (i, item) {
                strb += '<button class="col-lg-2 btn btn-info renyuanbt" tag=" ' + item.RenYuanId + ' ">' + item.RenYuanName + '</button>'
            })
            $('.renyuan').append(strb);
        }
    })
}

//点击部门刷新
$(document).on('click', '.bumenbt', function () {
    var tag = $(this).attr('tag');
    var name = $(this).text();
    PiZhunRen(tag);
    $('.pizhunrumb').append(' <li><a href="#" tag="' + tag + '" class="bmt">' + name + '</a></li>')
})

//点击路径
$(document).on('click', '.bmt', function () {
    var tag = $(this).attr('tag');
    PiZhunRen(tag);
    $(this).parent().nextAll().remove();
})


//选择人员
$(document).on('click', '.renyuanbt', function () {
    var tag = $(this).attr('tag');
    var name = $(this).text();
    $('#PeblishPeople').val(name);
    $('#PeblishPeople').attr('tag', tag);
    $('#myModalPiZhun').modal('hide');
})

//批准人搜索
$(document).on('click', '.pinzhunrensuosou', function () {
    var tag = $(this).attr('tag');
    var str = $(this).parent().prev().val();
    if (tag == '1') {
        cha(str, '');
    } else if (tag == '2') {
        cha('', str);
    }
})

//批准人搜索ajax
function cha(owner, user) {
    $.post('PiZhunRenStr/', { owner: owner, user: user }, function (data, status) {
        if (status == 'success') {
            $('#shou').parent().nextAll().remove();
            PiZhunRen(data.ownerid);
        }
    })
}

//提交数据
$('#btnzong').click(function () {
    var RfidCodeid = $('#RfidCodeid').val();
    var title = $('#PeblishTitle').val();
    var content = $('#PublishContent').val();
    var Peopleid = $('#Peopleid').val().trim();
    var Carid = $('#Carid').val().trim();
    var Productid = $('#Productid').val().trim();
    //var pizhunren = $('#PeblishPeople').attr('tag').trim();

    if (title == '' || title == null) {
        alert('必须填写标题!');
        return false;
    } else if (content == '' || content == null) {
        alert('必须填写内容!');
        return false;
    } else if (Peopleid.length < 3 || Peopleid == null) {
        alert('最少必须选择一个人员!');
        return false;
    }

    $.post('LinShiAjax/', { RfidCodeid: RfidCodeid, title: title, content: content, Peopleid: Peopleid, Carid: Carid, Productid: Productid, pizhunren: "1" }, function (data, status) {
        if (status == 'success') {
            if (data.msg == 'OK') {
                alert('操作成功')
            }
        }

    })

})



