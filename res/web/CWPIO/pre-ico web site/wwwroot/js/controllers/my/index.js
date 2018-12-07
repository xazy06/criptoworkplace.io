var app = (function () {
	var App = new function () {

		var self = this;
		
		this.options = {
			flagsPath: '/assets/app/media/img/flags/'
		};

		this.getGMT = function() {
			return (new Date().getTimezoneOffset() / 60) * (-1);
		};
		
		
		this.initJIvo =function () {
		
			(function(){ var widget_id = '67sVoKO5GY';var d=document;var w=window;function l(){var s = document.createElement('script'); s.type = 'text/javascript'; s.async = true;s.src = '//code.jivosite.com/script/widget/'+widget_id; var ss = document.getElementsByTagName('script')[0]; ss.parentNode.insertBefore(s, ss);}if(d.readyState=='complete'){l();}else{if(w.attachEvent){w.attachEvent('onload',l);}else{w.addEventListener('load',l,false);}}})();
		};
		
		this.changeLocale = function (locale) {
			
			locale = locale || $(this).data('lang'); 
			
			console.log('locale changing to ', locale);

			self.storeLang(locale);
				
			try{
				
				i18n.setLng(locale).then(function(){
					$("body").i18n();
				});

				console.log('locale changed');

				$('.js-current-lang').prop('src', [self.options.flagsPath, locale, '.svg'].join(''));
				
			}catch (e){
				
			}
		};

		this.storeLang = function (locale) {
			window.localStorage.setItem('activeLang', locale);
		};
		
		this.getStoredLang = function () {
			var activeLang = window.localStorage.getItem('activeLang');
			
			self.changeLocale(activeLang);
		};
		
		this.addHandlers = function () {
			$('.js-change-lang').on('click.changeLocale', function(){
				self.changeLocale($(this).data('lang'));
			});
		};
				
		this.public = {
			
		};
		
	}();
	
	App.init = function () {
		i18n.init({lng: "en"}, function(err, t) {
			$("body").i18n();
		});
		
		App.initJIvo();
		
		App.addHandlers();
		
		App.getStoredLang();
		
	};
	
	App.init();
	
	return App.public;
	
})();