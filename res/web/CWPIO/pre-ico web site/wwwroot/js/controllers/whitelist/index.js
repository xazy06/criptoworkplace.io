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
				return ViewModel.obs.ercAddress().length === 40 && ViewModel.actions.byteLength(ViewModel.obs.ercAddress()) > 20;
			}),
			checking: ko.observable(false)
		},
		
		actions:{
			byteLength: function (str) {
				var s = str.length;
				for (var i=str.length-1; i>=0; i--) {
					var code = str.charCodeAt(i);
					if (code > 0x7f && code <= 0x7ff) s++;
					else if (code > 0x7ff && code <= 0xffff) s+=2;
					if (code >= 0xDC00 && code <= 0xDFFF) i--;
				}
				return s;
		 },
		 send: function () {
			 ViewModel.flags.checking(true);
			 this.form.submit();
		 }
		}
		
	};
		
	return this.init();
};

Controller = new Controller();