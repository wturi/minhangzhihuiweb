function GetClientIp() {
    var url = 'http://chaxun.1616.net/s.php?type=ip&output=json&callback=?&_=' + Math.random();

    $.getJSON(url, function (data) {
        window.localStorage["ClientIp"] = data.Ip;
    });

}