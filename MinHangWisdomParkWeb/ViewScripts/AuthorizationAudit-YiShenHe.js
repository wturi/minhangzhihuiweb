var n0 = $("#BuniessNumber1").val();
var num = 10;

$(function(){
    Counts();
})

function Counts() {
    var str1 = '';
    for (var i = 1; i <= 5; i++) {
        if (i == 1 && i <= n0) {
            str1 += '<li class="active num"><a href="#">' + i + '</a></li>';
        } else if (i <= n0) {
            str1 += '<li class="num"><a href="#">' + i + '</a></li>';
        }
    }
    $('#paginpre1').parent().after(str1);
}

//取消审核
$(".queren").click(function () {
    var applyid = $("#mymodeltag").attr("tag");
    var content = $(this).parent().find("textarea").val();
    var bools = $(this).attr("tag");

    $.post("quxiaoshenhe", { applyid: applyid, content: content, bools: bools }, function (data, status) {
        if (status == "success") {
            alert(data.msg);
            window.location.reload();

        }
    })
})

//提取信息填充到Model里
$(document).on("click", ".btn-info", function () {
    var applyid = $(this).attr("applyid");
    var peblishid = $(this).attr("peblishid").trim();
    var typename = $(this).attr("typename");


    $.post("../Main/TextAjax", { typename: typename, peblishid: applyid }, function (data, status) {
        if (status == "success") {
            $('#myModalLabel').text(data.peblish.PeblishTitle)
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

            //alert(str);
            $('#myModelContent').empty().append(str);
            $('#mymodeltag').attr('tag', peblishid);
        }
    })



})