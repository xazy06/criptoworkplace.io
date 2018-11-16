var app = (function () {
	var App = new function () {

		var self = this;

		this.getGMT = function() {
			return (new Date().getTimezoneOffset() / 60) * (-1);
		};
		
		
		this.initJIvo =function () {
		
			(function(){ var widget_id = '67sVoKO5GY';var d=document;var w=window;function l(){var s = document.createElement('script'); s.type = 'text/javascript'; s.async = true;s.src = '//code.jivosite.com/script/widget/'+widget_id; var ss = document.getElementsByTagName('script')[0]; ss.parentNode.insertBefore(s, ss);}if(d.readyState=='complete'){l();}else{if(w.attachEvent){w.attachEvent('onload',l);}else{w.addEventListener('load',l,false);}}})();
		};
		
		this.changeLocale = function (locale) {
			try{
				
				i18n.setLng(locale);
				
			}catch (e){
				
			}
		};
				
		this.public = {
			
		};
		
	}();
	
	App.init = function () {
		i18n.init({lng: "en"}, function(err, t) {
			$("body").i18n();
		});
		
		App.initJIvo();
		
	};
	
	App.init();
	
	return App.public;
	
})();