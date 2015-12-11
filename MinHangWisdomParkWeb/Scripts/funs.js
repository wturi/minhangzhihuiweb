


//分页向前翻页
function qian(count, index) {
    var pageindex = $('.qian').parent().parent().find(".active");
    if (pageindex.text() != '1') {
        if (pageindex.prev().find('span').text() == '«') {
            pageindex.parent().find('.li').remove();
            var i = 0;
            var str = '';
            for (var i; i < index; i++) {
                if (Number(count) - i > 0) {
                    if (i == index - 1 || i == Number(count) - 1) {
                        str += '<li class="active li"><a href="#">' + (Number(pageindex.text()) - Number(index) + Number(i)) + '</a></li>';
                    } else {
                        str += '<li class="li"><a href="#">' + (Number(pageindex.text()) - Number(index) + Number(i)) + '</a></li>';
                    }
                }
            }
            $('.qian').parent().after(str);

        } else {
            pageindex.removeClass('active');
            pageindex.prev().addClass('active');
        }
    }
}


//分页向后翻页
function hou(count, index) {
    var pageindex = $('.hou').parent().parent().find('.active');
    if (pageindex.text() != count) {
        if (pageindex.next().find('span').text() == "»") {
            pageindex.parent().find('.li').remove();
            var i = 1;
            var str = '';
            for (var i; i <= index; i++) {
                if (Number(pageindex.text()) + Number(i) <= count) {
                    if (i == 1) {
                        str += '<li class="active li"><a href="#">' + (Number(pageindex.text()) + Number(i)) + '</a></li>';
                    } else {

                        str += '<li class="li"><a href="#">' + (Number(pageindex.text()) + Number(i)) + '</a></li>';
                    }
                }
            }
            $('.qian').parent().after(str);
        } else {
            pageindex.removeClass('active');
            pageindex.next().addClass('active');
        }

    }
}

//刷新分页数量


//获取url中的参数
function getUrlParam(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); //构造一个含有目标参数的正则表达式对象
    var r = window.location.search.substr(1).match(reg);  //匹配目标参数
    if (r != null) return unescape(r[2]); return null; //返回参数值
}

//时间转换格式
function timeStamp2String(time) {
    var time = eval(time.replace(/\/Date\((\d+)\)\//gi, "new Date($1)"));
    var datetime = new Date();
    datetime.setTime(time);
    var year = datetime.getFullYear();
    var month = datetime.getMonth() + 1 < 10 ? "0" + (datetime.getMonth() + 1) : datetime.getMonth() + 1;
    var date = datetime.getDate() < 10 ? "0" + datetime.getDate() : datetime.getDate();
    var hour = datetime.getHours() < 10 ? "0" + datetime.getHours() : datetime.getHours();
    var minute = datetime.getMinutes() < 10 ? "0" + datetime.getMinutes() : datetime.getMinutes();
    var second = datetime.getSeconds() < 10 ? "0" + datetime.getSeconds() : datetime.getSeconds();
    return year + "-" + month + "-" + date + " " + hour + ":" + minute + ":" + second;
}

//信息发布中心弹出model层数据填充
function modelshuju(a) {
    $.post("../Main/TextAjax", { typename: 'peblish', peblishid: a }, function (data, status) {
        if (status == "success") {
            $('#mymodel1').find('#myModalLabel').text(data.peblish.PeblishTitle);
            var str = '';
            var a = 0;
            var b = 0;
            if (data.files != null) {
                str += '<div id="carousel-example-generic" class="carousel slide" data-ride="carousel">';
                str += '<ol class="carousel-indicators">';
                $.each(data.files, function (i, items) {
                    if (a == 0) {
                        str += ' <li data-target="#carousel-example-generic" data-slide-to="' + a + '" class=" active"></li>';
                    } else {
                        str += ' <li data-target="#carousel-example-generic" data-slide-to="' + a + '"></li>';
                    }
                    a++;
                })
                str += '</ol><div class="carousel-inner" role="listbox">';
                $.each(data.files, function (i, items) {
                    if (b == 0) {
                        str += '<div class="item active"><img src="../..' + items.FilePath + '"></div>';
                    } else {
                        str += '<div class="item"><img src="../..' + items.FilePath + '"></div>';
                    }
                    b++
                })
                str += ' </div>';
                str += '<a class="left carousel-control" href="#carousel-example-generic" role="button" data-slide="prev">'
                + '<span class="glyphicon glyphicon-chevron-left" aria-hidden="true"></span>'
                + ' <span class="sr-only">Previous</span></a>'
                + '<a class="right carousel-control" href="#carousel-example-generic" role="button" data-slide="next">'
                + '<span class="glyphicon glyphicon-chevron-right" aria-hidden="true"></span>'
                + '<span class="sr-only">Next</span>'
                + '</a></div>';
            }
            str += '<div class="jumbotron"><p>' + data.peblish.PeblishContent + '</p></div>'
            $('#mymodel1').find('.modal-body').empty().append(str);
        }
    })



    //$.post('../InformationDelivery/ChaByPeblishId/', { peblishid: a }, function (data, status) {
    //    if (status == 'success') {
    //        $('#mymodel1').find('#myModalLabel').text(data.title);
    //        $('#mymodel1').find('.modal-body').text(data.Content);
    //    }
    //})
}

