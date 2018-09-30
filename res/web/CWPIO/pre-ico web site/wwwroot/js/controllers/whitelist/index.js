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
				return ViewModel.obs.ercAddress().length === 42;
			})
		},
		
		actions:{
		
		}
		
	};
		
	return this.init();
};

Controller = new Controller();


var p = tsale.addAddressToWhitelist('0x' + rows[0].address);
for (var i = 1; i < rows.length; i++){
	
	(function (i) {
		var currentRow = rows[i];
		p = p.then(() => {
			console.log("Add whitelist: 0x" + currentRow.address);
			
			return tsale.addAddressToWhitelist('0x' + currentRow.address);
	});	
	})(i)
	
}