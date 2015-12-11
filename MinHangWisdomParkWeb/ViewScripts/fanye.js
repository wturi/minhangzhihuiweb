//查询点击
$(document).on("click", ".search-btn", function () {
    var id = $(this).attr("tag");
    var str = $.trim($(this).parent().prev().val().replace(/\s+/g, " "));
    var typename = $(this).attr("typename");
    var typenumber = $(this).attr("typenumber");
    select(str, 0, typename, typenumber, 1000000000);
})

//向前翻页
$('.ListPrevious').click(function () {
    var id = $(this).parent().parent().children().eq(0).children().attr('id').replace('paginpre', '');
    var n = $(this).parent().parent().find('.active');
    var maxcount = $("#BuniessNumber" + id).val();
    var str = $("button[typenumber='" + id + "']").parent().prev().val();
    var typename = $("button[typenumber='" + id + "']").attr("typename");
    if (n.text() != '1') {
        if (n.prev().find("span").text != "«") {
            n.removeClass("active");
            n.prev().addClass("active");

            select(str, (Number(n.prev().text()) - 1), typename, id, 10);
        } else if (n.prev().find("span").text == "«") {
            n.parent().find('.num').remove();
            var i = 5;
            var page = '';
            for (var i; i >= 1 ; i--) {
                if (i == 1) {
                    page += '<li class="active num"><a href="#">' + (Number(n.text()) - i) + '</a></li>';

                    select(str, ((Number(n.text()) - i) - 1), typename, id, 10);
                } else {
                    page += '<li class="num"><a href="#">' + (Number(num.text()) - i) + '</a></li>'
                }
            }
            $('#paginpre' + id).parent().after(page);
        }
    }
})

//向后翻页
$(".ListNext").click(function () {
    var id = $(this).parent().parent().children().eq(0).children().attr('id').replace('paginpre', '');
    var n = $(this).parent().parent().find('.active');
    var maxcount = $("#BuniessNumber" + id).val();
    var str = $("button[typenumber='" + id + "']").parent().prev().val();
    var typename = $("button[typenumber='" + id + "']").attr("typename");
    if (n.text() != maxcount) {
        if (n.next().find("span").text != "»") {
            n.removeClass("active");
            n.next().addClass("active");
            select(str, (Number(n.next().text()) - 1), typename, id, 10);
        } else if (n.next().find("span").text == "»") {
            n.parent().find('.num').remove();
            var i = 1;
            var page = '';
            for (var i; i <= 5; i++) {
                if (i == 1) {
                    page += '<li class="active num"><a href="#">' + (Number(n.text()) + i) + '</a></li>';
                    select(str, ((Number(n.text()) + i) - 1), typename, id, 10);
                } else {
                    page += '<li class="num"><a href="#">' + (Number(num.text()) + i) + '</a></li>'
                }
            }
            $('#paginpre' + id).parent().after(page);
        }
    }

})

//点击页码
$(document).on("click", '.num', function () {
    $(this).siblings().removeClass('active');
    $(this).addClass('active');
    var id = $(this).parent().children().eq(0).children().attr('id').replace('paginpre', '');
    var n = $(this).parent().find('.active');
    var maxcount = $("#BuniessNumber" + id).val();
    var str = $("button[typenumber='" + id + "']").parent().prev().val();
    var typename = $("button[typenumber='" + id + "']").attr("typename");
    select(str, (Number($(this).text()) - 1), typename, id, 10);
})

//页面重置
function select(str, pageindex, typename, typenumber, nums) {
    $.post("../AuthorizationAudit/ShenHeFenYe", { str: str, pageindex: pageindex, typename: typename, typenumber: typenumber, number: nums }, function (data, status) {
        if (status == 'success') {
            var table = $("table[id='table" + typenumber + "']");
            var nav = $("a[id='paginpre" + typenumber + "']");
            var strs = '';
            var str2 = '';
            var c = 1;
            if (typename == 'buniess') {
                $.each(data, function (i, item) {
                    if (c > 10) {
                        return false;
                    }
                    if (item.length != 0) {
                        if (item.Time != null) {
                            var time = timeStamp2String(item.Time);
                        } else {
                            var time = "";
                        }
                        strs += '<tr><td><button type="button" class="btn btn-link buniesstitle" data-toggle="modal" data-target="#ModelPeople" repairid="' + item.Content + '">' + item.Title.split("|+|")[0] + '</button></td>';
                        strs += '<td>' + item.Updater + '</td><td>' + item.ownerid + '</td>';
                        if (typenumber == "4") {
                            strs += '<td>' + item.Content.split("|+|")[2] + '</td>';
                        }
                        strs += '<td>' + time + '</td><td><div class="btn-group" role="group" tag="' + item.ApplyID + '"><button type="button" class="btn btn-info tongguo">通过</button><button type="button" class="btn btn-danger butongguo">不通过</button></div></td>';
                        c++;
                    }
                })
            } else if (typename == 'repair') {
                $.each(data, function (i, item) {
                    if (c > 10) {
                        return false;
                    }
                    if (item.length != 0) {
                        if (item.Time != null) {
                            var time = timeStamp2String(item.Time);
                        } else {
                            var time = "";
                        }
                        strs += '<tr><td><button type="button" class="btn btn-link" data-toggle="modal" data-target="#ModelPeople">' + item.Title + '</button></td>';
                        strs += '<td>' + item.Updater + '</td><td>' + item.ownerid + '</td>';
                        strs += '<td>' + time + '</td><td><div class="btn-group" role="group" tag="' + item.ApplyID + '"><button type="button" class="btn btn-info tongguo">通过</button><button type="button" class="btn btn-danger butongguo">不通过</button></div></td>';
                        c++;
                    }
                })
            } else if (typename == "peblish") {
                $.each(data, function (i, item) {
                    if (c > 10) {
                        return false;
                    }
                    if (item.length != 0) {
                        var name;
                        switch (typenumber) {
                            case "1": name = '新闻'; break;
                            case "2": name = '园区'; break;
                            case "3": name = '广告'; break;
                            case "4": name = '政策'; break;
                            case "5": name = '企业'; break;
                        }
                        if (item.Time != null) {
                            var time = timeStamp2String(item.Time);
                        } else {
                            var time = "";
                        }
                        strs += '<tr><td>' + item.Title + '</td><td>' + time + '</td><td>' + item.Updater + '</td><td>' + item.ownerid + '</td>'
                        + '<td><button type="button" class="btn btn-info" data-toggle="modal" data-target="#modelxiangxi" typename="' + name + '" applyid="' + item.ApplyID + '" peblishid="' + item.Content + '">查看详情</button></td></tr>';

                    }
                })
            } else if (typename == "check") {
                $.each(data, function (i, item) {
                    if (c > 10) {
                        return false;
                    }
                    if (item.length != 0) {
                        if (item.Time != null) {
                            var time = timeStamp2String(item.Time);
                        } else {
                            var time = "";
                        }
                        strs += '<tr>'
                        + '<td><button type="button" class="btn btn-link" data-toggle="modal" data-target="#modelxiangxi" typename="check" applyid="' + item.CheckID + '">' + item.Title + '</button></td>'
                        + '<td>' + time + '</td><td>' + item.Updater + '</td> </tr>';
                    }
                })
            } else if (typename == "yishenhe") {
                $.each(data, function (i, item) {
                    if (c > 10) {
                        return false;
                    }
                    if (item.length != 0) {
                        if (item.Time != null) {
                            var time = timeStamp2String(item.Time);
                        } else {
                            var time = "";
                        }
                        strs += '<tr><td>' + item.typeid + '</td><td>' + item.Title + '</td><td>' + time + '</td><td>' + item.Updater + '</td><td>' + item.ownerid + '</td>'
                             + '<td><button type="button" class="btn btn-info" data-toggle="modal" data-target="#modelxiangxi" typename="' + item.typeid + '" applyid="' + item.Content + '" peblishid="' + item.ApplyID + '">查看详情</button></td></tr>';
                    }
                })
            }
            if (nums == '1000000000') {
                var datacount = Math.ceil(data.length / 10);
                for (var i = 1; i <= 5; i++) {
                    if (i == 1 && i <= datacount) {
                        str2 += '<li class="active num"><a href="#">' + i + '</a></li>';
                    } else if (i <= datacount) {
                        str2 += '<li class="num"><a href="#">' + i + '</a></li>';
                    }
                }
                $("#BuniessNumber" + typenumber).val(data.length);
                nav.parent().parent().find('.num').remove();
                nav.parent().parent().find('.li').remove();
                nav.parent().after(str2);
            }
            table.find('tbody').empty().append(strs);
        }
    })
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



