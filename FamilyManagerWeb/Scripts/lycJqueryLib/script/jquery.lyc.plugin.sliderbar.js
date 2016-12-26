/*
	@author => 刘一呈
	@create date => 2013年7月10日15:20:10
	@with CSS => lyc.sliderbar.css
*/
(function ($) {
    var defaultOps = {
        enable: true,
        barWidth: 300,
        maxValue: 100,
        minValue: 0,
        startValue: null,
        showMouseInfo: false,
        barLeft: 0   //已选择的值对应的进度
    };

    /*
    设置初始位置，并绑定拖拽事件
    @$sliderObj => 当前拖动控件对象（即调用插件的jquery对象）
    @options => 参数设置
    */
    function iniSlider($sliderObj, options) {
        if (options.maxValue < options.minValue) {
            alert("参数设置错误，最小值大于最大值！");
        }
        $sliderObj.css("width", options.barWidth);
        //计算初始位置对应拖动按钮的left值
        var barLeft = (options.barWidth / options.maxValue) * (options.startValue > options.maxValue ? options.maxValue : options.startValue);

        $("div.completed", $sliderObj).css("width", barLeft);
        $("div.jqSlider", $sliderObj).css("left", barLeft);
        options.barLeft = barLeft;

    }

    /*
    绑定拖拽事件
    @$sliderObj => 当前拖动控件对象（即调用插件的jquery对象）
    @options => 参数设置
    */
    function moveAction($sliderObj, options) {
        var $slider = $("div.jqSlider", $sliderObj);
        $slider.bind("mousedown", function (eDown) {
            //初始化slider的left值
            var sliderOldLeft = parseInt($slider.css("left"));
            $(window).bind("mousemove", lycSliderMove = function (e) {
                //计算偏移量
                var skewingWidth = e.pageX - eDown.pageX + sliderOldLeft;
                if (skewingWidth > options.barWidth) {
                    skewingWidth = options.barWidth;
                } else if (skewingWidth < 0) {
                    skewingWidth = 0;
                } 

                if (options.showMouseInfo) {
                    //显示鼠标坐标信息
                    $("#sliderContent", $slider).html(function () {
                        return "鼠标按下时坐标：" + eDown.pageX + ";" + "鼠标移动时初始坐标：" + e.pageX + ";" + "原始left值：" + sliderOldLeft + "偏移量：" + skewingWidth;
                    }).show();
                }
                $slider.css("left", skewingWidth);
                $("div.completed", $sliderObj).width(skewingWidth);
            }).bind("mouseup", lycSliderUp = function (e) {
                $(this).unbind("mousemove", lycSliderMove).unbind("mouseup", lycSliderUp);
                $("#sliderContent", $slider).hide();
            });
        });
    }

    $.fn.extend({
        lycSlider: function (options) {
            //初始值、所选值为最小值；
            var op = $.extend(defaultOps, { startValue: defaultOps.minValue, nowValue: defaultOps.minValue }, options || {});
            //创建bar的html元素
            var sliderHtml = "<div class=\"completed\"></div><div class=\"jqSlider\"><div id='sliderContent'></div></div><input type='hidden' class='lycJqSliderNowValue' value=" + options.startValue + ">";
            $(this).removeClass().addClass("lycpg_sliderbar").append(sliderHtml);

            //初始化控件各参数属性
            iniSlider($(this), op);

            if (op.enable) {
                //绑定拖动事件
                moveAction($(this), op);
            }
            return this;
        }
    });
})(jQuery);