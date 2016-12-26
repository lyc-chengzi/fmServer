/*
	@author => 刘一呈
	@create date => 2013年7月22日15:31:53
	@with CSS => lyc.tabs.css
*/
(function ($) {
    var defaultOps = {
        width: 400,
        height: 400,
        tabIndex: 0,
        showType: 'click',  /*可选值：'click','mouseover'*/
        contentOverflow: 'auto' /*可选值：'auto','hidden','scroll','visible'*/
    };

    function iniTabs($tabsDiv, $headerLiList, $contentList, op) {
        $tabsDiv.width(op.width).height(op.height);

        $contentList.css("overflow", op.contentOverflow).height($tabsDiv.height() - $headerLiList.parent().parent().height() - 2); //文本div高度 = 总高度 - 导航高度 - 文本上下边框2px高度
        setCheckTab($tabsDiv, $headerLiList, $contentList, op.tabIndex);
    }

    //设置选中选项卡样式
    function setCheckTab($tabsDiv, $headerLiList, $contentList, tabIndex) {
        $headerLiList.removeClass("current");
        $headerLiList.eq(tabIndex).addClass("current");

        $contentList.hide().eq(tabIndex).show();
    }

    $.fn.extend({
        lycTabs: function (options) {
            //初始值、所选值为最小值；
            var op = $.extend(defaultOps, options || {});

            //定义选项卡导航对象
            var $headerLiList = $("div.header > ul > li", $(this));
            var $contentList = $("div.content", $(this));
            var $tabsDiv = $(this);
            iniTabs($(this), $headerLiList, $contentList, op);

            //给导航绑定单击事件
            $headerLiList.bind(op.showType, function (e) {
                if (e.target == this) {
                    //获取所点击li的下标                    
                    setCheckTab($tabsDiv, $headerLiList, $contentList, $headerLiList.index($(this)));
                }
            });

            return $(this);
        }
    });
})(jQuery);