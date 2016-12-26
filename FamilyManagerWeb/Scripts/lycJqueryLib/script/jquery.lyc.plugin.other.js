(function ($) {
	$.LycOtherFun = {
		//遮罩层Div类
		BG_Div:{
			ID:"LycBgDiv_PageShadow",
			createBGDiv: function () {
				var $html = "";
				$html += "<div id=\"" + this.ID + "\" style=\"z-index: 800; left: 0; position: absolute; top: 0; display: none; background:#e8e8e8;  text-align:center; padding-top:200px; \">";
				$html += "<span class=\"content\" style=\"font-size:12px; \"></span>";
				$html += "</div>";

				$("body").append($html);
				return this;
			},
			show: function (txt) {
				var isCreate = $("body").data("lycBGDiv_Create");
				//如果没有遮罩层对象，生成对象
				if (typeof isCreate == "undefined") {					
					this.createBGDiv();
				} else if (isCreate == false) {
					this.createBGDiv();
				}

				var $shadow = $("#" + this.ID);
				if ($shadow) {
					this.hide();
					$shadow.width($(document).width()).height($(document).height()).fadeTo('normal', 0.6).show();
				}
				$("body").data("lycBGDiv_Create", true);

				if (txt && txt!=null && txt !== "") {
					$("span.content", $shadow).html(txt);
				}
				else if (!txt || txt==null || txt==="") {
					$("span.content", $shadow).html("");
				}
			},
			hide: function () {
				$("#" + this.ID).hide();
			}
		}
		
	};
})(jQuery);