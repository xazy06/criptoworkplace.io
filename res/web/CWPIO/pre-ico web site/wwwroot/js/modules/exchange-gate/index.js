var Gate = function () {
	
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
		getMap: function () {
			shapeshift.coins(function (err, coinData) {

				Controller.ViewModel.currencies = coinData;
				
				console.dir(coinData) // =>
				/*
					{ BTC:
					 { name: 'Bitcoin',
						 symbol: 'BTC',
						 image: 'https://shapeshift.io/images/coins/bitcoin.png',
						 status: 'available' },
			
						 ...
			
					VRC:
					 { name: 'Vericoin',
						 symbol: 'VRC',
						 image: 'https://shapeshift.io/images/coins/vericoin.png',
						 status: 'available' } }
				*/
			})
		}
	};

	this.shapeshift = window.shapeshift || {};

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
		self.actions.getMap();
		
		return this;
	};
	
	
	var ViewModel = {
				
		obs:{
			
		},
		
		flags:{
			
		},
		
		actions:{
			
		}
		
	};
		
	return this.init();
};

Gate = new Gate();