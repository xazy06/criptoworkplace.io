var app = (function () {
	var App = new function () {

		var self = this;
		
		this.initTimer = function () {
			var timer = new Timer();
			timer.start({countdown: true, startValues: {days: 4, hours:2, minutes:59}});

			try{
				$('.js-day').text(timer.getTimeValues().days);
				$('.js-hour').text(timer.getTimeValues().hours);
				$('.js-min').text(timer.getTimeValues().minutes);
			}catch(e){console.log(e)}
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
	};
	
	App.init();
	
	return App.public;
	
})();