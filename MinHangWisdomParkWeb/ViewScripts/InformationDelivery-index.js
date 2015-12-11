
$(document).ready(function () {
    $('.summernote').summernote({
        height: 300, // set editor height
        minHeight: 300, // set minimum height of editor
        maxHeight: 300, // set maximum height of editor
        focus: true, // set focus to editable area after initializing summernote
    });
});

//上传图片
$("#imgUpload").click(function () {
    if ($("#imgzhanshi").children().length >= 3) {
        alert("上传图片数量最大3张！")
        return false;
    }
    $("#imgFlie").click();
    var stm1 = setInterval(function () {
        var imgstr = $("#imgFlie").val();
        if (imgstr != "") {
            clearInterval(stm1);
            $("#ImgForm input[type='submit']").click();
        }
    }, 500);
    return false;
});

$('#ImgForm').ajaxForm({
    beforeSend: function () {

    },
    success: function (data) {
        if (data.imgUrl != "") {
            var str = '<div class="alert col-xs-6 col-md-3" role="alert" imgid="' + data.imgId + '"><button type="button" onclick="xxx(\'' + data.imgUrl + '\')" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button><a href="#" class="thumbnail"><img src="' + data.imgUrl + '" alt="..."></a></div>';
            $('#imgzhanshi').append(str);
        } else {
            //alerTips(data.ErrorInfo);
        }

    }, complete: function (xhr) {
        $("#imgFlie").val("");
    }
});


function xxx(e) {
    $.post("DeleteFiles", { url: e }, function (data, stauts) {
    })
};

//提交数据
$("#btnzong").click(function () {
    var PeblishType = $("#countid").val();
    var PeblishTitle = $("#PeblishTitle").val();
    var PublishContent = $('.summernote').code();
    var PublishImg = [];
    var DateTimeNew = $("#DateTime").val();
    var DateTimeOld = $("#DateTime").attr("DateTimeOld");

    //alert(PublishContent);
    $("#imgzhanshi").children().each(function () {
        var id = $(this).attr("imgid");
        PublishImg.push(id);
    })

    if (PeblishType == "" || PeblishTitle == "" || PublishContent == "") {
        alert("标题 内容必须填写！");
        return false;
    }
    PublishContent = encodeURI(PublishContent);

    $.post("InsertPeblishApply", { PeblishType: PeblishType, PeblishTitle: PeblishTitle, PublishContent: PublishContent, PublishImg: { 0: PublishImg[0], 1: PublishImg[1], 2: PublishImg[2] } }, function (data, stauts) {
        if (stauts == "success") {
            if (data.msg == "time") {
                alert("不能选择之前时间段！")
            } else if (data.msg == "ok") {
                alert("提交成功");
                window.location.reload();
            } else {
                alert("提交失败！");
            }
        }
    }).error(function () {
        //alert("error");
    })
})

//多条件搜索
$('#search-btn').click(function () {
    var str = $.trim($(this).parent().prev().val().replace(/\s+/g, " "));
    fenyeajaxcha(str, 1, getUrlParam("CodeID"), 1000000000);
})

//点击换页
$('.li').click(function () {
    $(this).siblings().removeClass('active');
    $(this).addClass('active');
    var str = $.trim($('#search-btn').parent().prev().val().replace(/\s+/g, " "));
    fenyeajaxcha(str, $(this).text(), getUrlParam("CodeID"), 10);

})

//分页前翻
$('.qian').click(function () {
    qian(pagecount, 5);
    var pageindex = $(this).parent().parent().find('.active').text();
    var str = $.trim($('#search-btn').parent().prev().val().replace(/\s+/g, " "));
    fenyeajaxcha(str, pageindex, getUrlParam("CodeID"), 10);
})


//分页后翻
$('.hou').click(function () {
    hou(pagecount, 5);
    var pageindex = $(this).parent().parent().find('.active').text();
    var str = $.trim($('#search-btn').parent().prev().val().replace(/\s+/g, " "));
    fenyeajaxcha(str, pageindex, getUrlParam("CodeID"), 10);
})


//有查询条件的分页
function fenyeajaxcha(str, pageindex, codeid, pagesize) {
    $.post("DuoTiaoJianChaXun/", { str: str, PageIndex: pageindex, CodeID: codeid, pagesize: pagesize }, function (data, status) {
        if (status == 'success') {
            var s = '';
            var str2 = '';
            var c = 1;
            $("#shuju").find("tbody").empty();
            $.each(data, function (i, item) {
                if (c > 10) {
                    return false;
                }
                if (item.length != 0) {
                    var time = timeStamp2String(item.Time);
                    str += '<tr>' +
                        ' <td><button type="button" class="btn btn-link title" data-toggle="modal" data-target="#mymodel1" tag="' + item.PeblishID + '">' + item.Title + '</button></td>' +
                        '<td>' + time + '</td>' +
                        '<td>' + item.StateName + '</td>' +
                        '<td>' + item.ConfirmerName + '</td>' +
                        '</tr>';
                }
                c++;
            })
            if (pagesize == '1000000000') {
                var datacount = Math.ceil(data.length / 10);
                for (var i = 1; i <= 5; i++) {
                    if (i == 1 && i <= datacount) {
                        str2 += '<li class="active li"><a href="#">' + i + '</a></li>';
                    } else if (i <= datacount) {
                        str2 += '<li class="li"><a href="#">' + i + '</a></li>';
                    }
                }
                $('#count').val(data.length);
                $('.qian').parent().parent().find('.li').remove();
                $('.qian').parent().after(str2);
            }
            $("#shuju").find("tbody").append(str);

        }
    })
}

//弹出modal层数据填充
$(document).on('click', '.title', function () {

    var peblishid = $(this).attr("tag");
    var typename = $(this).attr("typename");
    var ConfirmeMemo = $(this).attr("confirmeMemo");

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
            $('#mymodeltag').find('small').empty().append(ConfirmeMemo != "" ? ConfirmeMemo : "无");
        }
    })
})
