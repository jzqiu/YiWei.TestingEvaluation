var _p = $.trim($("#MainContent_hdp").val()) == "" ? 0 : parseInt($.trim($("#MainContent_hdp").val()));
var _c = $.trim($("#MainContent_hdc").val()) == "" ? 0 : parseInt($.trim($("#MainContent_hdc").val()));
var _d = $.trim($("#MainContent_hdd").val()) == "" ? 0 : parseInt($.trim($("#MainContent_hdd").val()));

var data = {
    provinceId: 0,
    provinceName: "",
    cityId: 0,
    cityName: "",
    districtId: 0,
    districtName: ""
};
$().ready(function() {
    getprovince();
});

var getprovince = function() {
    $.ajax({
        type: 'post',
        url: '../ajax/area.ashx',
        data: {
            op: "getprovince",
            date: new Date().getTime()
        },
        success: function(data) {
            var dt = eval("(" + data + ")");
            //$(".left_box").html('<p data-id="0">请选择省份</p>');
            $.each(dt, function(i, v) {
                if (v.Id == _p) {
                    $(".left_box").append('<p data-id="' + v.Id + '" class="selected">' + v.ShortCNName + '</p>');
                    data.provinceId = v.Id;
                    data.provinceName = v.ShortCNName;

                } else {
                    $(".left_box").append('<p data-id="' + v.Id + '">' + v.ShortCNName + '</p>');
                }
            });

            if ($(".left_box").find("p[class='selected']").length) {
                $(".left_box").find("p[class='selected']").click();
            }
        }
    });
}

var getcity = function(id) {
    $.ajax({
        type: 'post',
        url: '../ajax/area.ashx',
        data: {
            op: "getcity",
            provinceId: id,
            date: new Date().getTime()
        },
        success: function(data) {
            var dt = eval("(" + data + ")");

            if (dt.length == 1) {
                $(".center_box").html('<p data-id="' + dt[0].Id + '"  class="selected">' + dt[0].ShortCNName + '</p>');
                data.cityId = dt[0].Id;
                data.cityName = dt[0].ShortCNName;

                //$(".center_box").find("p").eq(0).click();
                $(".center_box").hide();
            } else {
                //$(".center_box").html('<p data-id="0">请选择城市</p>');
                $.each(dt, function(i, v) {
                    if (v.Id == _c) {
                        $(".center_box").append('<p data-id="' + v.Id + '" class="selected">' + v.ShortCNName + '</p>');
                        data.cityId = v.Id;
                        data.cityName = v.ShortCNName;
                    } else {
                        $(".center_box").append('<p data-id="' + v.Id + '">' + v.ShortCNName + '</p>');
                    }
                });
            }
            if ($(".center_box").find("p[class='selected']").length) {
                $(".center_box").find("p[class='selected']").click();
            }
        }
    });
}


var getdistrict = function(id) {
    $.ajax({
        type: 'post',
        url: '../ajax/area.ashx',
        data: {
            op: "getdistrict",
            cityId: id,
            date: new Date().getTime()
        },
        success: function(data) {
            var dt = eval("(" + data + ")");
            if (dt.length == 1) {
                $(".right_box").html('<p data-id="' + dt[0].Id + '"  class="selected">' + dt[0].ShortCNName + '</p>');
                data.districtId = dt[0].Id;
                data.districtName = dt[0].ShortCNName;

                //$(".right_box").find("p").eq(0).click();
            } else {

                //$(".right_box").html('<p data-id="0">请选择区县</p>');
                $.each(dt, function(i, v) {
                    if (v.Id == _d) {
                        $(".right_box").append('<p data-id="' + v.Id + '" class="selected">' + v.ShortCNName + '</p>');
                        data.districtId = v.Id;
                        data.districtName = v.ShortCNName;
                    } else {
                        $(".right_box").append('<p data-id="' + v.Id + '">' + v.ShortCNName + '</p>');
                    }
                });
            }
            if ($(".right_box").find("p[class='selected']").length) {
                $(".right_box").find("p[class='selected']").click();
            }
        }
    });
}

var gradeObj = $("#clkgrade");
gradeObj.find('input[type="radio"]').each(function() {
    var indexNum = gradeObj.find('input[type="radio"]').index(this);
    var newObj = gradeObj.find("a").eq(indexNum);
    if ($(this).prop("checked") == true) {
        newObj.addClass("selected");
    }
    $(newObj).on("click", function() {
        $(this).parent().parent().find("a").removeClass("selected");
        $(this).addClass("selected");
        gradeObj.find('input[type="radio"]').prop("checked", false);
        gradeObj.find('input[type="radio"]').eq(indexNum).prop("checked", true);
        gradeObj.find('input[type="radio"]').eq(indexNum).trigger("click");
    });
});


$(".choose_city").parent().on("click", function() {
    $(".address_wrapper").show();
    $(".page").hide();

    $(".left_box").scrollTop(0);
    $(".center_box").scrollTop(0);
    $(".right_box").scrollTop(0);
    var t = $(".left_box").find("p[class='selected']").offset();
    $(".left_box").scrollTop(t.top);
    t = $(".center_box").find("p[class='selected']").offset();
    $(".center_box").scrollTop(t.top);
    t = $(".right_box").find("p[class='selected']").offset();
    $(".right_box").scrollTop(t.top);
});


$(".left_box").delegate("p", "click", function() {
    $(this).parent().find("p").removeClass("selected");
    $(this).addClass("selected");

    var id = $(this).attr("data-id");
    if (id == 0)
        return false;
    data.provinceId = id;
    data.provinceName = $(this).text();
    data.cityId = 0;
    data.cityName = "";
    data.districtId = 0;
    data.districtName = "";

    $(".center_box").html("").hide();
    $(".right_box").html("").hide();

    $(".center_box").html(getcity(id)).show();
});


//城市
$(".center_box").delegate("p", "click", function() {
    $(this).parent().find("p").removeClass("selected");
    $(this).addClass("selected");

    var id = $(this).attr("data-id");
    if (id == 0)
        return false;
    data.cityId = id;
    data.cityName = $(this).text();
    data.districtId = 0;
    data.districtName = "";

    $(".right_box").html("").hide();

    $(".right_box").html(getdistrict(id)).show();
});


//地区
$(".right_box").delegate("p", "click", function() {
    $(this).parent().find("p").removeClass("selected");
    $(this).addClass("selected");
    var id = $(this).attr("data-id");
    if (id == 0)
        return false;
    data.districtId = id;
    data.districtName = $(this).text();

    if (data.provinceName == data.cityName)
        $(".choose_city").text(data.provinceName + "，" + data.districtName);
    else
        $(".choose_city").text(data.provinceName + "，" + data.cityName + "，" + data.districtName);

    $("#MainContent_hdp").val(data.provinceId);
    $("#MainContent_hdc").val(data.cityId);
    $("#MainContent_hdd").val(data.districtId);

    $(".address_wrapper").hide();
    $(".page").show();
});


function checkSave() {

    if ($("#MainContent_hdp").val() == "" || $("#MainContent_hdp").val() == "0" || $("#MainContent_hdc").val() == "" || $("#MainContent_hdc").val() == "0" || $("#MainContent_hdd").val() == "" || $("#MainContent_hdd").val() == "0") {
        showTipLayer($(".choose_city"), {
            msg: "请选择您的现居住地",
            type: 1,
            showtime: 3
        });
        return false;
    }

    var reg = /^[\u4E00-\u9FA5]+$/;
    var e = $("#MainContent_txtName");
    if ($.trim(e.val()) == "" || !reg.test($.trim(e.val())) || $.trim(e.val()).length < 2) {
        showTipLayer(e, {
            msg: "请输入中文姓名",
            type: 1,
            showtime: 3
        });
        e.focus();
        return false;
    }
    var ischecked = false;
    $("#MainContent_rblChildrenSex input[type='radio']").each(function() {
        if ($(this).prop("checked")) {
            ischecked = true;
            return false;
        }
    });

    if (!ischecked) {
        showTipLayer($(".rule-multi-radio").eq(0), {
            msg: "请选择性别",
            type: 1,
            showtime: 3
        });
        return false;
    }

    ischecked = false;
    $("#MainContent_rblGrade input[type='radio']").each(function() {
        if ($(this).prop("checked")) {
            ischecked = true;
            return false;
        }
    });

    if (!ischecked) {
        showTipLayer($("#clkgrade"), {
            msg: "请选择年级",
            type: 1,
            showtime: 3
        });
        return false;
    }


    loadingR();
}