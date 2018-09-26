var app = (function () {
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
	};
	
	App.init();
	
	return App.public;
	
})();