/*
*刘一呈jquery插件 for 页面弹出框
*依赖文件：
1、jquery核心库
2、lyc_alertMsg.css
*使用方法：LycAlert.warn  Or  LycAlert.error  Or  LycAlert.info  Or LycAlert.success  Or LycAlert.confirm
*for exzample:  LycAlert.error('测试警告',{ title: "系统错误",shadow:true });

LycAlert.confirm("测试确定框",{
buttons: {
okCall: function () {
alert(111);
}
}
});
*方法参数：      
*       1、msg：输出的信息
*       2、optiongs：   title：弹出框标题栏。
*                      shadow：bool类型，是否显示遮罩层。默认为true
*                      closeSelf：bool类型，是否定时关闭，必须与参数closeSecond同时设定。默认false
*                      closeSecond：几秒之后关闭弹出框，单位是毫秒。
*/
(function ($) {
    var LycAlertMsgObj = {
        title: { error: "系统错误", info: "系统提示", warn: "系统警告", success: "成功提示", confirm: "确定提示框" }
    };

    $.LycAlert = {
        boxId: "#alertMsgBox",
        bgId: "#lycAlertMsgdivhidden",
        types: { error: "error", info: "info", warn: "warn", success: "success", confirm: "confirm" },
        //关闭弹出框，判断是否有遮罩层的情况下
        lycAlertClose: function (closeOP) {
            var op = $.extend({ shadow: true, gourl: "", target: "" }, closeOP || {});
            $(this.boxId).slideUp(500, function () {
                if (op.shadow == true) {
                    $($.LycAlert.bgId).fadeOut(200);
                }
                $($.LycAlert.boxId).remove();
                $($.LycAlert.bgId).remove();
            });
            return false;
        },
        lycAlertMsgAccount: null,
        //显示弹出框
        open: function (type, msg, options) {
            var op = $.extend({ title: "", shadow: true, closeSelf: false, closeSecond: 0, buttons: null }, options || {});

            var $divHtml = "";
            $divHtml += "<div id='alertMsgBox' class='lyc_alert'>";
            $divHtml += "<div id='alertMsgBoxHeader' class='lyc_alert_Header_" + type + "'>";
            $divHtml += "<span class='lycAlert_titleSpan'></span>";
            $divHtml += "</div>";
            $divHtml += "<div id='alertMsgBoxContent' class='lyc_alert_Content'>";
            $divHtml += "</div>";
            $divHtml += "<div id='alertMsgBoxFooter' class='lyc_alert_Footer'>";

            //绑定按钮
            for (var key in op.buttons) {
                $divHtml += "<input type='button' class='lyc_new_InputImage'  value='" + key + "' />";
            }
            $divHtml += "<input type='button' class='lyc_new_InputImage' onclick='return $.LycAlert.lycAlertClose({shadow:" + op.shadow + "})' value='关闭' />";

            $divHtml += "</div>";
            $divHtml += "</div>";

            //绑定阴影层
            $divHtml += "<div id='lycAlertMsgdivhidden' class='lyc_divhidden' style='display: none; top: 0px; position: absolute; left: 0px;'>";
            $divHtml += "</div>";
            $($divHtml).appendTo("body");

            //绑定按钮事件
            $("input.lyc_new_InputImage:not(input[value=关闭])", $("#alertMsgBoxFooter")).each(function () {
                $(this).bind("click", op.buttons[$(this).val()]);
            });

            //设定标题文字
            if (op.title != "") {
                $('#alertMsgBoxHeader span.lycAlert_titleSpan').html(op.title);
            } else {
                $('#alertMsgBoxHeader span.lycAlert_titleSpan').html(LycAlertMsgObj.title[type]);
            }
            $('#alertMsgBoxContent').html(msg);
            var alertMsgx = $(window).width() / 2 - $(this.boxId).width() / 2;
            var alertMsgy = $(window).height() / 2 - $(this.boxId).height() / 2;

            //如果用户不需要弹出阴影层
            if (op.shadow == true) {

                $($.LycAlert.bgId).css({ 'width': $(document).width(), 'height': $(document).height() });
                $($.LycAlert.bgId).fadeTo(500, 0.8).show();
            }

            $(this.boxId).css({ left: alertMsgx + "px", top: (alertMsgy - 60) + "px" }).show().animate({ top: (alertMsgy - 10) + "px" }, 500);

            if (this.lycAlertMsgAccount) {
                clearTimeout(this.lycAlertMsgAccount);
                this.lycAlertMsgAccount = null;
            }

            if (op.closeSelf == true && op.closeSecond == 0) {
                alert("自关闭参数设置错误！");
                return false;
            } else if (op.closeSelf == true && op.closeSecond != 0) {
                this.lycAlertMsgAccount = setTimeout(function () { $.LycAlert.lycAlertClose(op.shadow) }, op.closeSecond);
            }

        },
        error: function (msg, options) {
            this.alert(this.types.error, msg, options);
        },
        info: function (msg, options) {
            this.alert(this.types.info, msg, options);
        },
        warn: function (msg, options) {
            this.alert(this.types.warn, msg, options);
        },
        success: function (msg, options) {
            this.alert(this.types.success, msg, options);
        },
        alert: function (type, msg, options) {
            this.open(type, msg, options);
        },
        /**
        * 确定框
        * @param {msg} 显示的提示信息
        * @param {options} {okText, okCall, cancelText, cancelCall}
        */
        confirm: function (msg, options) {
            //var buttonOP = $.extend({ okText: '确定', cancelText: '取消', okCall: null, cancelCall: this.lycAlertClose(), shadow: true }, options.buttons || {});

            this.alert(this.types.confirm, msg, { buttons: options.buttons });
        },
        resizeFun: function () {
            var windowWH = { width: 0, height: 0 };
            if ($.browser.opera) {
                windowWH = { width: window.innerWidth, height: window.innerHeight };
            } else {
                windowWH = { width: $(window).width(), height: $(window).height() };
            }
            var topLocation = windowWH.height / 2 - $($.LycAlert.boxId).height() / 2;

            var leftLocation = windowWH.width / 2 - $($.LycAlert.boxId).width() / 2;

            var newY = $(window).scrollTop() + topLocation + "px";
            var newX = $(window).scrollLeft() + leftLocation + "px";
            $($.LycAlert.boxId).stop().animate({ "top": newY, "left": newX });
            $($.LycAlert.bgId).css({ "width": $(document).width(), "height": $(document).height() });
        }
    };
})(jQuery);

