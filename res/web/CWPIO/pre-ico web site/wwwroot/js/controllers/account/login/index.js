var Controller = function () {

	var self = this;

	this.strings = {
		ru:{

		},
		en:{

		}
	};

	this.api = {

	};

	this.actions = {

	};
	
	this.init = function () {
		ko.applyBindings(ViewModel);
		
		return this;
	};


	var ViewModel = {
		obs:{
			password: ko.observables(''),
			passwordConfirmation: ko.observables('')
		},

		flags:{
			
		},

		actions:{
			notify: function(a, val){
				$.notify(a, {
					type: 'success',
					timer: 1000
				});
			},
			validate: function () {
				
			}
		}

	};


	ViewModel.obs.password.subscribe(function (val) {
		ViewModel.actions.validate(val);
	});

	ViewModel.obs.passwordConfirmation.subscribe(function (val) {
		ViewModel.actions.validate(val);
	});
	
	return this.init();
};

Controller = new Controller();