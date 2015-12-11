var n0 = $("#BuniessNumber1").val();
var n1 = $("#BuniessNumber2").val();
var n2 = $("#BuniessNumber3").val();
var n3 = $("#BuniessNumber4").val();
var n4 = $("#BuniessNumber5").val();
var num = 10;

$(function () {
    Counts();
})

//页数初始化
function Counts() {
    var str1 = '';
    var str2 = '';
    var str3 = '';
    var str4 = '';
    var str5 = '';
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
    for (var i = 1; i <= 5; i++) {
        if (i == 1 && i <= n2) {
            str3 += '<li class="active num"><a href="#">' + i + '</a></li>';
        } else if (i <= n2) {
            str3 += '<li class="num"><a href="#">' + i + '</a></li>';
        }
    }
    for (var i = 1; i <= 5; i++) {
        if (i == 1 && i <= n3) {
            str4 += '<li class="active num"><a href="#">' + i + '</a></li>';
        } else if (i <= n3) {
            str4 += '<li class="num"><a href="#">' + i + '</a></li>';
        }
    }
    for (var i = 1; i <= 5; i++) {
        if (i == 1 && i <= n4) {
            str5 += '<li class="active num"><a href="#">' + i + '</a></li>';
        } else if (i <= n4) {
            str5 += '<li class="num"><a href="#">' + i + '</a></li>';
        }
    }

    $('#paginpre1').parent().after(str1);
    $('#paginpre2').parent().after(str2);
    $('#paginpre3').parent().after(str3);
    $('#paginpre4').parent().after(str4);
    $('#paginpre5').parent().after(str5);
}

//确认审核
$(".queren").click(function () {
    var applyid = $(this).parent().attr("tag");
    var content = $(this).parent().find("textarea").val();
    var bools = $(this).attr("tag");
    $.post("ShenHe", { applyid: applyid, content: content, bools: bools }, function (data, status) {
        if (status == "success") {
            if (data.msg == "OK") {
                alert("审核成功！");
                window.location.reload();

            } else {
                alert("审核失败！");
            }
        }
    })
})

//提取信息填充到Model里
$(document).on("click", ".btn-info", function () {
    var applyid = $(this).attr("applyid");
    var peblishid = $(this).attr("peblishid").trim();
    var typename = $(this).attr("typename");

    $.post("../Main/TextAjax", { typename: typename, peblishid: peblishid }, function (data, status) {
        if (status == "success") {
            $('#myModalLabel').text(data.peblish.PeblishTitle)
            //alert(data.peblish.PeblishTitle);
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
            $('#myModelContent').empty().append(str);
            $('#mymodeltag').attr('tag', applyid);
        }
    })

})


