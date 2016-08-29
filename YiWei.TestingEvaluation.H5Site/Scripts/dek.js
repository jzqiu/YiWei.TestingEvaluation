$(function () {
    $("body").keypress(function (e) {
        var key = window.event ? e.keyCode : e.which;
        if (key.toString() == "13") {
            return false;
        }
    });
});

function loadingR(t) {
    $("#loadR").remove();
    var newDiv = document.createElement('div');
    newDiv.id = 'loadR';
    newDiv.className = 'loadingBg';
    document.body.appendChild(newDiv);
    if (t != null && t != "undefined" && t != "") {
        newDiv.innerHTML = "<div class='loading loadingMiddle'>" + t + "</div>";
    } else {
        newDiv.innerHTML = "<div class='loading loadingMiddle'></div>";
    }
    $(newDiv).show();
}


function showTipLayer(e, t) {

    if ($(".tipLayer").length) {
        $(".tipLayer").remove();
    }
    var html = "<div class=\"tipLayer\"><div class=\"content\"><i class=\"fa \"></i></div></div>";
    $(document.body).append(html);
    if (t.type == 1) {
        $(".tipLayer .content").addClass("tip-error");
        $(".tipLayer .content i").addClass("fa-times");
    } else if (t.type == 2) {
        $(".tipLayer .content").addClass("tip-warning");
        $(".tipLayer .content i").addClass("fa-exclamation");
    } else if (t.type == 3) {
        $(".tipLayer .content").addClass("tip-success");
        $(".tipLayer .content i").addClass("fa-check");
    } else {
        $(".tipLayer .content i").remove();
    }
    $(".tipLayer .content").append(t.msg);

    var u = $(".tipLayer");
    var r = 100;
    var w = (document.body.clientWidth / 2 - u.outerWidth() / 2);

    if (e != null) {
        r = (e.offset().top - u.outerHeight() - 2);
    } else {
        r = (document.body.clientHeight / 2 - u.outerHeight() / 2);
    }

    u.css({ left: w + "px", bottom: "0px" }).css("opacity", "0.7").show();
    u.animate({ opacity: "show", bottom: "+=100px" }, 300, function () {
        u.attr("style", "left:" + w + "px;" + "top:" + r + "px").show();
    });
    setTimeout(function () {
        u.animate({ opacity: "hide", top: "-=100px" }, 300, function () { u.remove(); });
    }, 1000 * t.showtime);
}