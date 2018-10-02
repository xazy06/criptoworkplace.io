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
		},
		shiftCoin: function () {
			var withdrawalAddress = 'YOUR_LTC_ADDRESS';
			var pair = 'btc_eth';

			// if something fails
			var options = {
				returnAddress: 'YOUR_BTC_RETURN_ADDRESS'
			};

			shapeshift.shift(withdrawalAddress, pair, options, function (err, returnData) {

				// ShapeShift owned BTC address that you send your BTC to
				var depositAddress = returnData.deposit

				// you need to actually then send your BTC to ShapeShift
				// you could use module `spend`: https://www.npmjs.com/package/spend
				// spend(SS_BTC_WIF, depositAddress, shiftAmount, function (err, txId) { /.. ../ })

				// later, you can then check the deposit status
				shapeshift.status(depositAddress, function (err, status, data) {
					console.log(status) // => should be 'received' or 'complete'
				})
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