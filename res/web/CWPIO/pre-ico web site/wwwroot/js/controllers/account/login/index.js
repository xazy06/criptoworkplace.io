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
			password: ko.observable(''),
			passwordConfirmation: ko.observable('')
		},

		flags:{
			showValidation: ko.observable(false),
			validation: {
				lowercaseletter: ko.observable(true),
				uppercaseletter: ko.observable(true),
				hasNumber: ko.observable(true),
				hasSymbol: ko.observable(true),
				charLen: ko.observable(true),
				passesMutch: ko.observable(true)
			}
		},

		actions:{
			notify: function(a, val){
				$.notify(a, {
					type: 'success',
					timer: 1000
				});
			},
			validate: function (str) {
				var options = {
					passLength: 8
				};
				
				ViewModel.flags.showValidation(true);
				
				
				ViewModel.flags.validation.charLen(str.length === options.passLength);
				ViewModel.flags.validation.passesMutch(ViewModel.obs.password() === ViewModel.obs.passwordConfirmation());
				ViewModel.flags.validation.hasNumber(/\d/g.test(str));
				ViewModel.flags.validation.uppercaseletter(/\W/g.test(str));
				ViewModel.flags.validation.lowercaseletter(/\w/g.test(str));
				ViewModel.flags.validation.hasSymbol(!(/^[a-zA-Z0-9!@#\$%\^\&*\)\(+=._-]+$/g.test(str)));
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