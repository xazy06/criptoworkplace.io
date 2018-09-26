var app = (function () {
	var App = new function () {
		this.public = {};
	}();
	
	App.init = function () {
		i18n.init({lng: "en"}, function(err, t) {
			// translate nav
			$("body").i18n();
		});
	};
	
	App.init();
	
	return App.public;
	
})();