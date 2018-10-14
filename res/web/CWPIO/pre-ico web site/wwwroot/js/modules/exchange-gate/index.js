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

				Controller.ViewModel.currencies(_.toArray(coinData));
				Controller.ViewModel.currenciesMap = coinData;
								
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
			var withdrawalAddress = '0x4b69fadf8b0d13ebd14546cb1406cc02869d7c28';
			var pair = Controller.ViewModel.obs.selectedCurrency().toLowerCase() +  '_eth';

			Controller.ViewModel.flags.depositAddrGetting(true);
			Controller.ViewModel.flags.depositAddrGot(false);

			// if something fails
			var options = {
				//BTC return addr 1EiM1ucSsTaLkbTt9QTBt1Z3fKxnP9CKA
				returnAddress: (Controller.ViewModel.obs.returnAddress() || '1EiM1ucSsTaLkbTt9QTBt1Z3fKxnP9CKA'), //'YOUR_CURRENCY_RETURN_ADDRESS'
				apiKey: "803d1f5df2ed1b1476e4b9e6bcd089e34d8874595dda6a23b67d93c56ea9cc2445e98a6748b219b2b6ad654d9f075f1f1db139abfa93158c04e825db122c14b7"
			};

			shapeshift.shift(withdrawalAddress, pair, options, function (err, returnData) {

				// ShapeShift owned BTC address that you send your BTC to
				var depositAddress = returnData.deposit;
				
				Controller.ViewModel.obs.depositAddress(depositAddress);

				// you need to actually then send your BTC to ShapeShift
				// you could use module `spend`: https://www.npmjs.com/package/spend
				// spend(SS_BTC_WIF, depositAddress, shiftAmount, function (err, txId) { /.. ../ })

				// later, you can then check the deposit status
				shapeshift.status(depositAddress, function (err, status, data) {
					console.log(status) // => should be 'received' or 'complete'
				});

				Controller.ViewModel.flags.depositAddrGetting(false);

				Controller.ViewModel.flags.depositAddrGot(true);
			})
		},
		status: function () {
			shapeshift.status(Controller.ViewModel.obs.depositAddress(), function (err, status, data) {
				console.log(status) // => should be 'received' or 'complete'
			})
		},
		marketInfo: function () {
			var pair;
			if (Controller.ViewModel === undefined) {
				return;
			}
			
			pair = Controller.ViewModel.obs.selectedCurrency().toLowerCase() +  '_eth';
			
			shapeshift.marketInfo(pair, function (err, response, data) {
				console.log(response);
				Controller.ViewModel.obs.market.limit(response.limit);
				Controller.ViewModel.obs.market.maxLimit(response.maxLimit);
				Controller.ViewModel.obs.market.minerFee(response.minerFee);
				Controller.ViewModel.obs.market.minimum(response.minimum);
				Controller.ViewModel.obs.market.rate(response.rate);
				Controller.ViewModel.obs.market.pair(response.pair);
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