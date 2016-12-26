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
    $.fn.extend({
        testValidit: function (options) {
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