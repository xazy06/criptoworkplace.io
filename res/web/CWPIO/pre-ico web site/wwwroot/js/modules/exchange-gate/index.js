var Controller = (Controller || {}), Gate = function () {
	
	var self = this;
	
	this.strings = {
		ru:{
			
		},
		en:{
			
		}
	};
	
	this.api = {
		shapeshift: 'https://shapeshift.io/'
	};
	
	this.apiKey = '803d1f5df2ed1b1476e4b9e6bcd089e34d8874595dda6a23b67d93c56ea9cc2445e98a6748b219b2b6ad654d9f075f1f1db139abfa93158c04e825db122c14b6';
	//Public key : 08ef330fe264f674ddd4943a5156cfb1ea06f10b95d5db54781afa3d8b108100874083d53b28afa5ce58bf3e834158a3114db725bce5b49da9454ef036753599
	//Private key : d80356c4c5c561bac38c5e451edc2b7a535ad27088b22e46ba8506995edb0d09a70a24c1c1ffa0bc1f6acddbec49870f135897568dfbee5a6d823109bcb9073d
	
	this.poolingId = null;

	this.options = {
		poolingInterval: 3000
	};
	
	this.actions = {
		getMap: function () {
			try{
				Controller.ViewModel.flags.currenciesReady(false);
			}catch (e){}
						
			shapeshift.coins(function (err, coinData) {
				var coinDataAsArray = null;

				coinDataAsArray = _.toArray(coinData);
				
				Controller.ViewModel.currencies(coinDataAsArray);
				Controller.ViewModel.currenciesCache(coinDataAsArray);
				Controller.ViewModel.currenciesMap = coinData;
				
				Controller.ViewModel.flags.currenciesReady(true);

				Controller.actions.getCurrentExchangeSession();

				//console.dir(coinData); // =>
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
		timeremaining: function () {
			
		},
		transactions: function () {
			Controller.ViewModel.obs.transactions([]);
			
			Controller.ViewModel.flags.transactionsGetting(true);

			// shapeshift.transactions('d80356c4c5c561bac38c5e451edc2b7a535ad27088b22e46ba8506995edb0d09a70a24c1c1ffa0bc1f6acddbec49870f135897568dfbee5a6d823109bcb9073d', 
			// 	'0x4B69FADF8B0D13EBD14546CB1406CC02869D7C28', function(r){
			// 	console.log(r);
			//		
			// 	Controller.ViewModel.flags.transactionsGetting(false);
			// });
			Controller.web3js.eth.getTransactionCount("0x4B69FAdf8B0D13ebD14546CB1406CC02869D7c28")
				.then(function(tcount){
					for (var i=0; i <= tcount; i++) {
						Controller.web3js.eth.getBlock(i, function(err, res) {
							Controller.ViewModel.obs.transactions.push(res);
						});
					}

					Controller.ViewModel.flags.transactionsGetting(false);
				});
									
		},
		shiftCoin: function () {
			//var withdrawalAddress = '0x4b69fadf8b0d13ebd14546cb1406cc02869d7c28';
			var withdrawalAddress = Controller.ViewModel.obs.withdrawalAddress();
			
			var pair = Controller.ViewModel.obs.currentCoin.symbol().toLowerCase() +  '_eth';
			//debugger;
			Controller.ViewModel.flags.depositAddrGetting(true);
			Controller.ViewModel.flags.depositAddrGot(false);

			// if something fails
			var options = {
				//BTC return addr 1EiM1ucSsTaLkbTt9QTBt1Z3fKxnP9CKA
				returnAddress: (Controller.ViewModel.obs.returnAddress()),/* || '1EiM1ucSsTaLkbTt9QTBt1Z3fKxnP9CKA')*///'YOUR_CURRENCY_RETURN_ADDRESS'
				apiKey: self.apiKey
			};

			if (window.restoring){
				window.restoring = false;
				
				self.actions.initStatusBang();

				Controller.ViewModel.flags.depositAddrGetting(false);

				Controller.ViewModel.flags.depositAddrGot(true);

				Controller.initCopyPurchaseAddr();

				return;
			}

			if (pair === '_eth') {
				return;
			}

			shapeshift.shift(withdrawalAddress, pair, options, function (err, returnData) {

				// ShapeShift owned BTC address that you send your BTC to
				var depositAddress = returnData.deposit;
				
				Controller.ViewModel.obs.depositAddress(depositAddress);

				// you need to actually then send your BTC to ShapeShift
				// you could use module `spend`: https://www.npmjs.com/package/spend
				// spend(SS_BTC_WIF, depositAddress, shiftAmount, function (err, txId) { /.. ../ })

				// later, you can then check the deposit status
				// shapeshift.status(depositAddress, function (err, status, data) {
				// 	console.log(status) // => should be 'received' or 'complete'
				// });
				
				self.actions.initStatusBang();

				Controller.ViewModel.flags.depositAddrGetting(false);

				Controller.ViewModel.flags.depositAddrGot(true);

				if (returnData.error){
					$.notify(returnData.error);
				}

				Controller.initCopyPurchaseAddr();
			})
		},
		sendamount: function (ammount) {
			//var withdrawalAddress = '0x4b69fadf8b0d13ebd14546cb1406cc02869d7c28';
			var withdrawalAddress = Controller.ViewModel.obs.withdrawalAddress();
			var pair = Controller.ViewModel.obs.currentCoin.symbol().toLowerCase() +  '_eth';
			
			//debugger;
			Controller.ViewModel.flags.depositAddrGetting(true);
			Controller.ViewModel.flags.depositAddrGot(false);

			ammount = ammount.replace(',', '.');
			
			// if something fails
			var options = {
				//BTC return addr 1EiM1ucSsTaLkbTt9QTBt1Z3fKxnP9CKA
				returnAddress: (Controller.ViewModel.obs.returnAddress()),/* || '1EiM1ucSsTaLkbTt9QTBt1Z3fKxnP9CKA')*///'YOUR_CURRENCY_RETURN_ADDRESS'
				apiKey: self.apiKey,
				amount:ammount,
				withdrawal:withdrawalAddress
			};
			
			
			if (window.restoring){
				window.restoring = false;
				self.actions.initStatusBang();

				Controller.ViewModel.flags.depositAddrGetting(false);

				Controller.ViewModel.flags.depositAddrGot(true);

				Controller.initCopyPurchaseAddr();
				
				return;
			}

			if (pair === '_eth') {
				return;
			}

			shapeshift.sendAmount(pair, options, function (err, returnData) {

				// ShapeShift owned BTC address that you send your BTC to
				var depositAddress = returnData.deposit;
				
				Controller.ViewModel.obs.depositAddress(depositAddress);
				Controller.ViewModel.obs.transactionFee(returnData.minerFee);
				
				Controller.ViewModel.obs.fixedAmmount.depositAmount(returnData.depositAmount);
				Controller.ViewModel.obs.fixedAmmount.expiration(returnData.expiration);
				Controller.ViewModel.obs.fixedAmmount.maxLimit(returnData.maxLimit);
				Controller.ViewModel.obs.fixedAmmount.minerFee(returnData.minerFee);
				Controller.ViewModel.obs.fixedAmmount.orderId(returnData.orderId);
				Controller.ViewModel.obs.fixedAmmount.pair(returnData.pair);
				Controller.ViewModel.obs.fixedAmmount.quotedRate(returnData.quotedRate);
				Controller.ViewModel.obs.fixedAmmount.withdrawal(returnData.withdrawal);
				Controller.ViewModel.obs.fixedAmmount.withdrawalAmount(returnData.withdrawalAmount);

				// you need to actually then send your BTC to ShapeShift
				// you could use module `spend`: https://www.npmjs.com/package/spend
				// spend(SS_BTC_WIF, depositAddress, shiftAmount, function (err, txId) { /.. ../ })

				// later, you can then check the deposit status
				// shapeshift.status(depositAddress, function (err, status, data) {
				// 	console.log(status) // => should be 'received' or 'complete'
				// });
				self.actions.initStatusBang();

				Controller.ViewModel.flags.depositAddrGetting(false);

				Controller.ViewModel.flags.depositAddrGot(true);

				if (returnData.error){
					$.notify(returnData.error);
				}
				
				Controller.initCopyPurchaseAddr();
				
			})
		},
		status: function () {
			shapeshift.status(Controller.ViewModel.obs.depositAddress(), function (err, status, data) {
				//console.log(status); // => should be 'received' or 'complete'
				console.log(data);

				if (status === 'failed') {
					self.actions.stopStatusBang();
					
					Controller.actions.sendEmail(data);//TODO
					return;
				}
				
				if (status !== 'complete') {
					return;
				}
				
				Controller.actions.monitor(Controller.ViewModel.obs.cwtCount(), data.transaction);

				self.actions.stopStatusBang();
			})
		},
		stopStatusBang: function () {
			window.clearInterval(self.poolingId);
		},
		initStatusBang: function () {
			try{
				window.clearInterval(self.poolingId);
			}catch (e){}
			
			self.poolingId = window.setInterval(function () {
				self.actions.status();
			},self.options.poolingInterval);
			
		},
		marketInfo: function () {
			var pair;
			if (Controller.ViewModel === undefined) {
				return;
			}
			
			pair = Controller.ViewModel.obs.currentCoin.symbol().toLowerCase() +  '_eth';
			
			if (pair === 'eth_eth') {
				self.actions.ethMarketInfo();
				
				return;
			}
			
			shapeshift.marketInfo(pair, function (err, response, data) {
				console.log(response);
				if (response === undefined) {
					return;
				}
				
				Controller.ViewModel.obs.market.limit(response.limit);
				Controller.ViewModel.obs.market.maxLimit(response.maxLimit);
				Controller.ViewModel.obs.market.minerFee(response.minerFee);
				Controller.ViewModel.obs.market.minimum(response.minimum);
				Controller.ViewModel.obs.market.rate(response.rate);
				Controller.ViewModel.obs.market.pair(response.pair);
			})
		},
		ethMarketInfo: function () {
			var minimumPurchaseCount = 500;

			Controller.ViewModel.flags.depositAddrGot(false);
			
			Controller.actions._calc(minimumPurchaseCount).then(function (response) {
				Controller.ViewModel.obs.market.limit('-');
				Controller.ViewModel.obs.market.maxLimit('-');
				Controller.ViewModel.obs.market.minerFee(0);
				Controller.ViewModel.obs.market.minimum(response.totalAmount);
				Controller.ViewModel.obs.market.pair('eth_eth');
			});

			Controller.actions._calc(1).then(function (response) {
				//TODO total with fee 
				Controller.ViewModel.obs.market.rate(response.totalAmount);
			});

			Controller.ViewModel.flags.depositAddrGot(true);
		},
		validateReturnAddress: function (address, coinSymbol) {
			
			Controller.ViewModel.flags.returnAddressValidating(true);
			$.get(self.api.shapeshift + 'validateAddress/'+ address +'/' + coinSymbol).then(function(response){
				Controller.ViewModel.flags.isValidReturnAddress(response.isvalid);
				Controller.ViewModel.flags.returnAddressValidating(false);
				
				console.log(response);
			}).fail(function () {
				Controller.ViewModel.flags.returnAddressValidating(false);
			});
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
				console.log(e);
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