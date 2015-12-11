$(document).ready(function () {
    $('.summernote').summernote({
        height: 300, // set editor height
        minHeight: 300, // set minimum height of editor
        focus: false, // set focus to editable area after initializing summernote
        toolbar: [
        //[groupname, [button list]]
        ]
    });
});


$(document).on('click', '#btnzong', function () {
    var title = $('#SEtext').val();
    var content = $('.summernote').code();

    $.post("guashiajax", { content: content, clientip: window.localStorage["ClientIp"] }, function (data, status) {
        if (data.msg == "OK") {
            alert("提交成功！");
            $("#PublishContent").val('');
        } else {
            alert("提交失败！");
        }

    })

})