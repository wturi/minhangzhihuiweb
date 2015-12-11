var BuniessNumber0 = $("#BuniessNumber4").val();
var BuniessNumber1 = $("#BuniessNumber5").val();
var BuniessNumber2 = $("#BuniessNumber1").val();
var BuniessNumber3 = $("#BuniessNumber2").val();
var BuniessNumber4 = $("#BuniessNumber6").val();
var BuniessNumber5 = $("#BuniessNumber7").val();
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
    var str6 = '';
    for (var i = 1; i <= 5; i++) {
        if (i == 1 && i <= BuniessNumber0) {
            str1 += '<li class="active num"><a href="#">' + i + '</a></li>';
        } else if (i <= BuniessNumber0) {
            str1 += '<li class="num"><a href="#">' + i + '</a></li>';
        }
    }
    for (var i = 1; i <= 5; i++) {
        if (i == 1 && i <= BuniessNumber1) {
            str2 += '<li class="active num"><a href="#">' + i + '</a></li>';
        } else if (i <= BuniessNumber1) {
            str2 += '<li class="num"><a href="#">' + i + '</a></li>';
        }
    }
    for (var i = 1; i <= 5; i++) {
        if (i == 1 && i <= BuniessNumber2) {
            str3 += '<li class="active num"><a href="#">' + i + '</a></li>';
        } else if (i <= BuniessNumber2) {
            str3 += '<li class="num"><a href="#">' + i + '</a></li>';
        }
    }
    for (var i = 1; i <= 5; i++) {
        if (i == 1 && i <= BuniessNumber3) {
            str4 += '<li class="active num"><a href="#">' + i + '</a></li>';
        } else if (i <= BuniessNumber3) {
            str4 += '<li class="num"><a href="#">' + i + '</a></li>';
        }
    }
    for (var i = 1; i <= 5; i++) {
        if (i == 1 && i <= BuniessNumber4) {
            str5 += '<li class="active num"><a href="#">' + i + '</a></li>';
        } else if (i <= BuniessNumber3) {
            str5 += '<li class="num"><a href="#">' + i + '</a></li>';
        }
    }
    for (var i = 1; i <= 5; i++) {
        if (i == 1 && i <= BuniessNumber5) {
            str5 += '<li class="active num"><a href="#">' + i + '</a></li>';
        } else if (i <= BuniessNumber3) {
            str5 += '<li class="num"><a href="#">' + i + '</a></li>';
        }
    }
    $('#paginpre4').parent().after(str1);
    $('#paginpre5').parent().after(str2);
    $('#paginpre1').parent().after(str3);
    $('#paginpre2').parent().after(str4);
    $('#paginpre6').parent().after(str5);
    $('#paginpre7').parent().after(str6);
}


//通过
$(document).on("click", ".tongguo", function () {
    var applyid = $(this).parent().attr("tag");
    $.post("ShenHe", { applyid: applyid, bools: 1 }, function (data, status) {
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

//不通过
$(document).on("click", ".butongguo", function () {
    var NoText = $('#NoText').val();
    var applyid = $('#NoText').attr('tag');

    $.post("ShenHe", { applyid: applyid, content: NoText, bools: 0 }, function (data, status) {
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


//审核意见模态框数据重写
$(document).on('click', '.Notextapplyid', function () {
    var applyid = $(this).parent().attr("tag");
    $('#NoText').attr('tag', applyid);
})


//抓取网上报修单个具体信息数据
$(document).on("click", ".repairtitle", function () {
    var id = $(this).attr("repairid");
    //alert(id);
    $.post("../BusinessApplication/SelectRepairByIdAjax", { id: id }, function (data, status) {
        if (status == "success") {
            // alert(data.RepairContent);

            $("#ModelRepair").find('.myModalLabel').text(data.RepairTitle);
            $("#ModelRepair").find('.modal-body').text(data.RepairContent);
            $("#ModelRepair").modal();
        }
    })
})


//抓取业务单个具体信息数据
$(document).on("click", ".buniesstitle", function () {
    var id = $(this).attr("repairid");
    //alert(id);
    $.post("../BusinessApplication/SelectBuniessByIdAjax", { id: id }, function (data, status) {
        if (status == "success") {
            var strss = data.BuniessContent.split('|+|');
            var strtile = '';
            var strcontent = '';
            if (strss.length == 2) {
                strtile = strss[0];
                strcontent = strss[1];
            } else if (strss.length == 3) {
                strtile = strss[0];
                strcontent = strss[1] + '<br /> 数量：' + strss[2];
            }
            //alert(data.BuniessContent);
            $("#ModelBuniess4").find('.myModalLabel').html(strtile);
            $("#ModelBuniess4").find('.modal-body').html(strcontent);
            $("#ModelBuniess4").modal();
        }
    })
})





