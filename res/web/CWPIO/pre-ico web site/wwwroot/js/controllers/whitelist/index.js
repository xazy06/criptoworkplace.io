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

	this.web3js = null;
	
	this.init = function () {
		ko.applyBindings(ViewModel);
		
		return this;
	};
	
	
	var ViewModel = {
		obs:{
			ercAddress:ko.observable('')
		},
		
		flags:{
			continueEnabled:ko.pureComputed(function () {
				return ViewModel.obs.ercAddress().length === 40;
			}),
			checking: ko.observable(false)
		},
		
		actions:{
		 send: function () {
			 ViewModel.flags.checking(true);
			 this.form.submit();
		 }
		}
		
	};
		
	return this.init();
};

Controller = new Controller();