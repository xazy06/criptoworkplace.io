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
			contonueEnabled:ko.pureComputed(function () {
				return ViewModel.obs.ercAddress().length === 40;
			})
		},
		
		actions:{
		
		}
		
	};
		
	return this.init();
};

Controller = new Controller();