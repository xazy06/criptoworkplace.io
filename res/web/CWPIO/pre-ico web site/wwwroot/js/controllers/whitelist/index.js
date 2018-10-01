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

	this.initWeb3Js = function () {

		(function () {

			// Checking if Web3 has been injected by the browser (Mist/MetaMask)
			if (typeof web3 !== 'undefined') {
				// Use Mist/MetaMask's provider
				self.web3js = new Web3(web3.currentProvider);
				console.log('web3 js success inited')
			} else {
				console.log('No web3? You should consider trying MetaMask!');
				// fallback - use your fallback strategy (local node / hosted node + in-dapp id mgmt / fail)
				self.web3js = new Web3(new Web3.providers.HttpProvider("http://localhost:8545"));
			}
			
			try{
				ViewModel.obs.hasMetamask(web3.currentProvider.isMetaMask === true);
			}catch (e){
			}
		})();
	};
	
	this.init = function () {
		self.initWeb3Js();
		
		ko.applyBindings(ViewModel);
		
		return this;
	};
	
	
	var ViewModel = {
		obs:{
			ercAddress:ko.observable(''),
			hasMetamask: ko.observable(false)
		},
		
		flags:{
			continueEnabled:ko.pureComputed(function () {
				return ViewModel.obs.ercAddress().length === 42 && ViewModel.actions.byteLength(ViewModel.obs.ercAddress()) > 20;
			}),
			checking: ko.observable(false),
			lockFill: ko.observable(false)
		},
		
		actions:{
			getndFillAddressFromMetamask: function () {
				ViewModel.flags.lockFill(true);
				self.web3js.eth.getAccounts().then(function(r){
					ViewModel.flags.lockFill(false);
					try{
						r = r && r[0] || '';
						ViewModel.obs.ercAddress(r);
					}catch (e){}
				})
			},
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