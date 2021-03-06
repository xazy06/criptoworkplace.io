﻿var app = (function () {
	var App = new function () {

		var self = this;
		
		this.initTimer = function () {
			var timer = new Timer(), updater,
				a = new Date();
			timer.start({countdown: true, startValues: {days: (30 - a.getDate()), hours:(23 - a.getHours()), minutes:(60 - a.getMinutes())}});

			updater = function () {
				try{
					$('.js-day').text(timer.getTimeValues().days);
					$('.js-hour').text(timer.getTimeValues().hours);
					$('.js-min').text(timer.getTimeValues().minutes);
				}catch(e){
					console.log(e);
				}	
			};

			updater();

			$(timer).on('minutesUpdated', function (e) {
				//console.log('update');
				updater();
			});
		};
		
		this.scroll = function () {
			$('.smoothscroll').on('click', function(e) {
				e.preventDefault();
				var target = this.hash;

				$('html, body').stop().animate({
					'scrollTop': $(target).offset().top - 56
				}, 1200);
			});
		};
		
		this.initRoadmap = function () {
			$('.roadmap').owlCarousel({
				loop: false,
				margin: 0,
				autoHeight: true,
				nav: false,
				navText: ['<i class="ion-arrow-left-c"></i>', '<i class="ion-arrow-right-c"></i>']
			});
		};
		
		this.initJIvo =function () {
		
			(function(){ var widget_id = '67sVoKO5GY';var d=document;var w=window;function l(){var s = document.createElement('script'); s.type = 'text/javascript'; s.async = true;s.src = '//code.jivosite.com/script/widget/'+widget_id; var ss = document.getElementsByTagName('script')[0]; ss.parentNode.insertBefore(s, ss);}if(d.readyState=='complete'){l();}else{if(w.attachEvent){w.attachEvent('onload',l);}else{w.addEventListener('load',l,false);}}})();
		
		};
		
		this.public = {
			initTimer: self.initTimer
		};
		
	}();
	
	App.init = function () {
		i18n.init({lng: "en"}, function(err, t) {
			
			$("body").i18n();
		});
		
		App.initTimer();

		App.scroll();

		App.initJIvo();
		
		//App.initRoadmap();
	};
	
	App.init();
	
	return App.public;
	
})();