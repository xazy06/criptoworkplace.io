var app = (function () {
	var App = new function () {

		var self = this;

		this.getGMT = function() {
			return (new Date().getTimezoneOffset() / 60) * (-1);
		};
		
		this.initTimer = function () {
			// var timer = new Timer(), updater,
			// a = new Date();
			// timer.start({
			// 	countdown: true, 
			// 	startValues: {
			// 		days: (30 - a.getDate()), 
			// 		hours:(21 - (a.getHours() + self.getGMT())), 
			// 		minutes:(57 - a.getMinutes())
			// 	}
			// });
			//
			// $(timer).on('targetAchieved', function (e) {
			// 	//TODO
			// });
			//
			// updater = function () {
			// 	try{
			// 		$('.js-day').text(timer.getTimeValues().days);
			// 		$('.js-hour').text(timer.getTimeValues().hours);
			// 		$('.js-min').text(timer.getTimeValues().minutes);
			// 	}catch(e){
			// 		console.log(e);
			// 	}	
			// };
			//
			// updater();
			//
			// $(timer).on('minutesUpdated', function (e) {
			// 	//console.log('update');
			// 	updater();
			// });
			
			initializeTimer();

		};
		
		this.scroll = function () {
			$('.smoothscroll').on('click', function(e) {
				e.preventDefault();
				var target = this.hash;

				$($('#collapse').data('target')).removeClass('in');
				
				$('html, body').stop().animate({
					'scrollTop': $(target).offset().top - 56
				}, 1200);
			});
		};
		
		this.initYoutube =function () {
			$('.js-youtube-popup').YouTubePopUp();
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
		
		this.initMobileToggler = function () {
			var $toggler = $('#collapse');

			function toggler() {
				$($toggler.data('target')).toggleClass('in');	
			}
			
			$toggler.on('click', toggler);
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

		App.initYoutube();
		
		App.initMobileToggler();
		
		//App.initRoadmap();
	};
	
	App.init();
	
	return App.public;
	
})();