var n0 = $("#BuniessNumber1").val();
var n1 = $("#BuniessNumber2").val();
var num = 10;

$(function () {
    Counts();//初始化页码
})
function Counts() {
    var str1 = '';
    var str2 = '';
    for (var i = 1; i <= 5; i++) {
        if (i == 1 && i <= n0) {
            str1 += '<li class="active num"><a href="#">' + i + '</a></li>';
        } else if (i <= n0) {
            str1 += '<li class="num"><a href="#">' + i + '</a></li>';
        }
    }
    for (var i = 1; i <= 5; i++) {
        if (i == 1 && i <= n1) {
            str2 += '<li class="active num"><a href="#">' + i + '</a></li>';
        } else if (i <= n1) {
            str2 += '<li class="num"><a href="#">' + i + '</a></li>';
        }
    }
    $('#paginpre1').parent().after(str1);
    $('#paginpre2').parent().after(str2);
}

//提取信息填充到Model里
$(document).on("click", '.btn-link', function () {
    var checkid = $(this).attr("applyid");
    var statetype = $(this).attr('statetype');
    $.post("InOrOutXSAjax", { checkid: checkid, type: statetype }, function (data, status) {
        if (status == "success") {
            $('#myModalLabel').html(data.Title)
            var strcontent = '';
            switch (statetype) {
                case 'zhengshi':
                    if (data.Peoples.length != 0 || data.Cars.length != 0 || data.Products.length != 0)
                        strcontent += '<table class="table table-striped table-hover">';
                    if (data.Peoples.length != 0) { //人
                        strcontent += ' <tr><th>类别</th><th>姓名</th><th>电话</th></tr>';
                        $.each(data.Peoples, function (i, item) {
                            strcontent += '<tr><td>人员</td><td>' + item.PeopleName + '</td><td>' + item.PhoneNum + '</td></tr>';
                        })

                    }
                    if (data.Cars.length != 0) { //车
                        strcontent += ' <tr><th>类别</th><th>车牌</th><th>车型</th></tr>';
                        $.each(data.Cars, function (i, item) {
                            strcontent += '<tr><td>车辆</td><td>' + item.CarLicence + '</td><td>' + item.CarType + '</td></tr>';

                        })
                    }
                    if (data.Products.length != 0) { //物
                        strcontent += '<tr><th>类别</th><th>名称</th><th>规格</th></tr>';
                        $.each(data.Products, function (i, item) {
                            strcontent += '<tr><td>物品</td><td>' + item.ProductName + '</td><td>' + item.ProductSpec + '</td></tr>';

                        })
                    }
                    strcontent += '</table>';
                    break;

                case 'lingshi':
                    if (data.Temps.length != 0) {
                        var aa = 0;
                        $.each(data.Temps, function (i, item) {
                            if (item.UseType == '1') {
                                if (aa == 0)
                                    strcontent += ' <table class="table table-striped table-hover"><thead><tr><th>类别</th><th>姓名</th><th>电话</th></tr></thead><tbody>';
                                strcontent += '<tr><td>人员</td><td>' + item.TempDesc.split('|')[0] + '</td><td>' + item.TempDesc.split('|')[1] + '</td></tr>'
                                aa += 1;
                            }
                        })
                        aa = 0;
                        $.each(data.Temps, function (i, item) {
                            if (item.UseType == '2') {
                                if (aa == 0)
                                    strcontent += '<tr><th>类别</th><th>车牌</th><th>车型</th></tr>';
                                strcontent += '<tr><td>车辆</td><td>' + item.TempDesc.split('|')[0] + '</td><td>' + item.TempDesc.split('|')[1] + '</td></tr>'
                                aa += 1;
                            }
                        })
                        aa = 0;
                        $.each(data.Temps, function (i, item) {
                            if (item.UseType == '3') {
                                if (aa == 0)
                                    strcontent += ' <tr><th>类别</th><th>名称</th><th>简述</th></tr>';
                                strcontent += '<tr><td>物品</td><td>' + item.TempDesc.split('|')[0] + '</td><td>' + item.TempDesc.split('|')[1] + '</td></tr>'

                                aa += 1;
                            }
                        })
                        strcontent += '</tbody></table>';
                    }
                    break;
            }
            $('#myModelContent').html(strcontent);
        }
    })
})
