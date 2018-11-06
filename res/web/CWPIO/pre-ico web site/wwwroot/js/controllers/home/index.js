var Controller = function () {

	var self = this;

	this.strings = {
		ru:{

		},
		en:{

		}
	};

	this.api = {
		subscribe:'/home/subscribe'
	};

	this.actions = {
		subscribe: function () {
			$.post(self.api.subscribe, {
				email:ViewModel.obs.email()
			}).done(function (result) {
				ViewModel.flags.successSubscribed(true);
				ViewModel.obs.email('');
				
				setTimeout(function () {
					ViewModel.flags.successSubscribed(false);
				},4000)
			});
		}
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
		})();
	};

	this.init = function () {
		ko.applyBindings(ViewModel);

		return this;
	};

	var ViewModel = {
		obs:{
			email: ko.observable('')
		},

		flags:{
			successSubscribed: ko.observable(false),
			sendDisabled: ko.pureComputed(function () {
				var email = ViewModel.obs.email();
				
				return email.length> 0 && email.indexOf('@') > -1;
			})
		},

		actions:{
			subscribe: function () {
				return self.actions.subscribe();
			}
		}

	};
	
	return this.init();
};

Controller = new Controller();