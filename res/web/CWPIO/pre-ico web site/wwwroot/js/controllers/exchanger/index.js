var Controller = function () {
	
	var self = this;
	
	this.locale = 'en';
	
	this.strings = {
		ru:{
			metamaskAuthLess:'Please login in MetaMask',
			refundRequested: 'Refund requested',
			balanceIncrese:'Your CWT-P balance has been changed',
			confirmDidTransaction: 'Thank you for your purchase, please wait a moment',
			whiteListLess: 'You need to set up ETH Address to receive CWT-P and be able to continue purchasing',
			whiteListAddressFieldSave: {
				fail: 'Look`s like some thing is wrong!',
				successSaved:'Your ETH address successfully saved, now you can continue purchase',
				success: 'Success!'
			}
		},
		en:{
			metamaskAuthLess:'Please login in MetaMask',
			refundRequested: 'Refund requested',
			balanceIncrese:'Your CWT-P balance has been changed',
			confirmDidTransaction: 'Thank you for your purchase, please wait a moment',
			whiteListLess: 'You need to set up ETH Address to receive CWT-P and be able to continue purchasing',
			whiteListAddressFieldSave: {
				fail: 'Look`s like some thing is wrong!',
				successSaved:'Your ETH address successfully saved, now you can continue purchase',
				success: 'Success!'
			}
		}
	};
	
	this.api = {
		withdrawalAddress: '/api/v1/exchanger/changerAddr',
		wl: '/api/v1/exchanger/whiteList',
		usersettings:'/api/v1/usersettings',
		exchanger:'/api/v1/exchanger',
		calc:'/api/v1/exchanger/calc/',
		calcExchange: '/api/v1/exchanger/calcExchange/',
		purchase: '/api/v1/exchanger/addr',
		initPurchasing: '/api/v1/exchanger/initPurchasing',
		monitor: '/api/v1/exchanger/monitor/',
		contractabi: '/api/v1/exchanger/contractabi/',
		sendMail: '/api/v1/sendMail/',
		ethStatus: '/api/v1/exchanger/status/'
	};
	
	this.actions = {
		refund: function () {
			
			$.get(self.api.contractabi).done(function (contractabi) {
				var contract;
				
				try {
					contract = new self.web3js.eth.Contract(
						contractabi,
						ViewModel.obs.contractAddress(),
						{
							from: ViewModel.obs.usersettings.ethAddress() 
						});
					
					console.log(ViewModel.obs.usersettings.ethAddress());
					
					contract.methods.withdrawPayments()
						.send().on('transactionHash', function (hash) {
							console.log(hash);
							
						}).on('receipt', function (receipt) {
							console.log(receipt);
							
						}).on('confirmation', function (confirmationNumber, receipt) {
							console.log('confNumber: ' + confirmationNumber + ', receipt: ' + receipt);
							
						}).on('error', console.log);
					
				} catch (e) {
					console.log(e.stack);
					
				}
				
			});
		},
		metrics:{
			purchasing: function () {
				try{
					window.yaCounter50462326 && window.yaCounter50462326.reachGoal('Purchase');
					ga('send', 'event', 'forms', 'purchase');
				}catch (e){
					console.log(e);
				}
			}
		},
		initBallanceUpdateEvent: function (interf) {
			var contractInt;
			
			if (ViewModel.obs.contractAddress() === '' || ViewModel.obs.withdrawalAddress() === '') {
				console.log('adresses less can`t attach events handling');
				
				return;
			}
			
			contractInt = new self.web3js.eth.Contract(interf, ViewModel.obs.contractAddress());

            contractInt.events.TokenPurchase({ filter: { beneficiary: ViewModel.obs.usersettings.ethAddress() } })
				.on('data', function(event){
					console.log(event);

                    self.actions.exchanger();

					self.actions.getPastEvents();

					ViewModel.flags.userDidTransaction(false);
					
                    $.notify(self.strings[self.locale].balanceIncrese);

                    self.ViewModel.actions.offGate();
			})
		},
		contractabi: function (callback) {
			return self.actions.getContractAddr()
				.then(function(response){
					self.actions.withdrawalAddress().then(function () {
						$.get(self.api.contractabi).done(function (response) {
							//console.log(response);
							
							if (callback){
								return callback(response);
							}
							
							self.actions.initBallanceUpdateEvent(response);
						})
					})
			});
		},
		
		getPastEvents: function () {
			
			return self.actions.contractabi(self.actions.fetchTransactions);
		},

		fetchTransactions: function (contractInterface) {
			var contractInt;

			if (ViewModel.obs.contractAddress() === '' || ViewModel.obs.withdrawalAddress() === '') {
				console.log('adresses less can`t continue');
				
				return;
			}

			contractInt = new self.web3js.eth.Contract(contractInterface, ViewModel.obs.contractAddress());

			contractInt.getPastEvents('TokenPurchase', {
				fromBlock: 0,
				toBlock: 'latest', 
                filter: {
                    beneficiary: ViewModel.obs.usersettings.ethAddress()
				}
			}).then(function(events){
				
				console.log('getPastEvents', events);
				
				_.each(events, function (item) {
					item.timestamp = ko.observable('');
					item.status = ko.observable('');
				});

				ViewModel.obs.transactions(events);
				
				ViewModel.flags.transactionsGetting(false);
			});
		},
		
		sendEmail: function (data, type) {
			$.post(self.api.sendMail, {body: data, type:type}).done(function (response) {
				console.log(response);
			}).fail(function (response) {
				console.log(response);
			});
		},
		getCurrentExchangeSession: function () {
			var exchangerSession = window.sessionStorage.getItem('exchangerSession');
			
			if (exchangerSession === null) {
				return false;
			}

			window.restoring = true;
			
			exchangerSession = JSON.parse(exchangerSession);
						
			ViewModel.actions.continueGateProceccing(exchangerSession);

			window.sessionStorage.removeItem('exchangerSession');
			
		},
		saveCurrentExchangeSession: function () {
			var gateOperation;

			if (ViewModel.flags.gateOperating() === false) {
				return;
			}
			
			gateOperation= {
				restoring: true,
				userDidTransaction: ViewModel.flags.userDidTransaction(),
				cwtCount: ViewModel.obs.cwtCount(),
				symbol: ViewModel.obs.currentCoin.symbol(),
				name:ViewModel.obs.currentCoin.name(),
				image:ViewModel.obs.currentCoin.image(),
				imageSmall:ViewModel.obs.currentCoin.imageSmall(),
				status:ViewModel.obs.currentCoin.status(),
				minerFee:ViewModel.obs.currentCoin.minerFee(),
				withdrawalAddress: ViewModel.obs.withdrawalAddress(),
				depositAddress: ViewModel.obs.depositAddress(),
				transactionFee: ViewModel.obs.transactionFee(),
				fixedAmmount: {
					depositAmount:ViewModel.obs.fixedAmmount.depositAmount(),
					expiration:ViewModel.obs.fixedAmmount.expiration(),
					maxLimit: ViewModel.obs.fixedAmmount.maxLimit(),
					minerFee: ViewModel.obs.fixedAmmount.minerFee(),
					orderId:ViewModel.obs.fixedAmmount.orderId(),
					pair:ViewModel.obs.fixedAmmount.pair(),
					quotedRate:ViewModel.obs.fixedAmmount.quotedRate(),
					withdrawal: ViewModel.obs.fixedAmmount.withdrawal(),
					withdrawalAmount:ViewModel.obs.fixedAmmount.withdrawalAmount()
				}
			};
			
			window.sessionStorage.setItem('exchangerSession', JSON.stringify(gateOperation));
						
		},
		monitor: function (count, txId) {
			return $.ajax({
				contentType: 'application/json',
				url: self.api.monitor,
				data: JSON.stringify({count: count, tx: txId}),
				method:'POST'
			}).done(function (response) {
				console.log(response);

			}).fail(function (error) {
				console.log(error);
				
				//debugger;
			});
		},
		withdrawalAddress: function () {
			return $.get(self.api.withdrawalAddress).done(function (response) {
				if (!response) {
					console.log('withdrawalAddress get error');
				}
				
				ViewModel.obs.withdrawalAddress(response);
			}).fail(function (response) {
				console.log(response);
								
				$.notify(response.statusText);
			});
		},
		whiteListAddressFieldSave: function(address){

			return $.ajax({
				contentType: 'application/json',
				url: self.api.wl,
				data: JSON.stringify({ercAddress: address}),
				method:'POST'
			}).done(function (response) {
				console.log(response);

				$.notify(self.strings[self.locale].whiteListAddressFieldSave.success);
				
				ViewModel.flags.whiteListAddressProcessing(false);
				ViewModel.flags.whiteListInputReady(false);
				ViewModel.obs.askFormText(self.strings[self.locale].whiteListAddressFieldSave.successSaved);
				ViewModel.flags.whiteListAddressProcessing(true);
				
				setTimeout(function () {
					ViewModel.actions.initGate.call(ko.toJS(ViewModel.obs.currentCoin));
					ViewModel.flags.whiteListAddressProcessing(false);
				},3000);
				
			}).fail(function (error) {
				console.log(error);
				
				$.notify(self.strings[self.locale].whiteListAddressFieldSave.fail + ' ' + error.statusText);
				
				ViewModel.flags.whiteListAddressProcessing(false);

				ViewModel.flags.whiteListInputReady(false);
				ViewModel.obs.askFormText(self.strings[self.locale].whiteListAddressFieldSave.fail);
				ViewModel.flags.whiteListAddressProcessing(true);
				ViewModel.obs.whiteListAddressField('');
				
				setTimeout(function () {
					ViewModel.actions.initGate.call(ko.toJS(ViewModel.obs.currentCoin));
					ViewModel.flags.whiteListAddressProcessing(false);
				},3000);
				
			});
			
		},
		
		getContractAddr: function () {
			
			return $.getJSON(self.api.purchase).done(function (result) {
				var copy4;
				
				ViewModel.obs.contractAddress(result);

				copy4 = new ClipboardJS('#cwpcontract');
				
			}).fail(function (response) {
				
				console.log(response);
			});
		},
		initPurchasing: function (count, amount) {
			
			return $.ajax({
				contentType: 'application/json',
                url: self.api.initPurchasing,
				data: JSON.stringify({count:count}),
				method:'POST'
			}).done(function (response) {
								
				ViewModel.flags.purchasingIsInitializing(false);
				
				//$('#m_modal_4').modal('show');
				
				try{
					ViewModel.obs.freezed(response.fixRate.rate);
				}catch (e){
					console.log(e);
				}
				
			}).fail(function (resp, e) {
				ViewModel.flags.purchasingIsInitializing(false);
				ViewModel.actions.notify(resp.statusText, 'danger');
				
			});
		},
		usersettings: function (put, data) {
			
			if (put){
				
				return $.ajax({
					contentType: 'application/json',
					url:self.api.usersettings,
					data: data, 
					method:'PUT'
				}).done(function (response) {
					console.log(response);
				});
				
			}

			return $.ajax({
				contentType: 'application/json',
				url:self.api.usersettings,
				method:'GET'
			}).done(function (response) {
				
				if (response.ethAddress === undefined) {
					console.log('response.ethAddress === undefined');	
				}
				
				ViewModel.obs.usersettings.name(response.name);
				ViewModel.obs.usersettings.lastName(response.lastName);
				ViewModel.obs.usersettings.ethAddress(response.ethAddress);
				ViewModel.obs.usersettings.country(response.country);
				ViewModel.obs.usersettings.telegramNickname(response.telegramNickname);
				
			}).fail(function (response) {
				$.notify(response.statusText);
				
			});
		},
		
		exchanger:function (params) {
			
		  $.ajax({
				contentType: 'application/json',
				url:self.api.exchanger,
				method:'GET'
			}).done(function (response) {
				
				ViewModel.obs.sales.cap(response.cap);
				ViewModel.obs.sales.rate(response.rate);
				ViewModel.obs.sales.sold(response.sold);
				ViewModel.obs.sales.step(response.step);
				ViewModel.obs.sales.ballance(response.ballance);
			  ViewModel.obs.sales.refund(response.refund);
			});
		},

		initialFixedAmmountShift: function () {
			console.log('initialFixedAmmountShift');
						
			return self.actions.calcWithFee(500)
		},

		_calc: function (count) {
			count = count || 0;

			return $.ajax({
				url:self.api.calc + count,
				method:'GET'
			});
		},
		
		calcWithFee: function (count) {
			count = count || 0;

			return $.ajax({
				url:self.api.calcExchange + count,
				method:'GET'
			}).done(function (response) {
				ViewModel.obs.needPay(response.totalAmount);
			});
		},
		
		calc:function (count) {
			count = count || 0;
			
			$.ajax({
				url:self.api.calc + count,
				method:'GET'
			}).done(function (response) {
				ViewModel.obs.needPay(response.totalAmount);
				
				ViewModel.obs.transactionFee(response.fee);
				ViewModel.obs.fixedAmmount.depositAmount(response.totalAmount);
								
				//ViewModel.actions.initPurchasingThrottled()();
				//self.actions.monitor(ViewModel.obs.cwtCount(), -1);
			});
		},
		purchase: function () {
			$.getJSON(self.api.purchase).done(function (result) {
				
				try{
				self.web3js.eth.sendTransaction({ 
					from: ViewModel.obs.usersettings.ethAddress(), 
					to: ViewModel.obs.depositAddress(), 
					value: self.web3js.utils.toWei(ViewModel.obs.needPay().replace(',', '.')) 
				});
				}catch (e){
					$.notify(e.stack, {z_index: 1001031});
					console.log(e.stack);
				}
				
				self.actions.exchanger();
			});
		}
	};

	this.web3js = null;
	
	this.initWeb3Js = function () {
				
		(function () {

			self.web3CWP = new Web3(new Web3.providers.HttpProvider(window.nodeUrl));
			
			// Checking if Web3 has been injected by the browser (Mist/MetaMask)
			if (typeof web3 !== 'undefined') {
				// Use Mist/MetaMask's provider
				self.web3js = new Web3(web3.currentProvider);
				console.log('web3 js success inited')
			} else {
				console.log('No web3? You should consider trying MetaMask!');
				// fallback - use your fallback strategy (local node / hosted node + in-dapp id mgmt / fail)
				self.web3js = new Web3(new Web3.providers.HttpProvider(window.nodeUrl));
			}

			try{
				ViewModel.flags.hasMetamask(web3.currentProvider.isMetaMask === true);
			}catch (e){
				console.log(e);
			}
			
		})();
	};
	
	this.initCopyPurchaseAddr = function (id) {
		var copy2 = new ClipboardJS(id || '#copy2');
		var copy3 = new ClipboardJS('#deposit-ammount');
		var copy5 =  new ClipboardJS('#purchaseAddr');
	};
		
	this.initUnloadingWindowProtocol = function () {
		window.addEventListener("beforeunload", function (event) {
			
			// Cancel the event as stated by the standard.
			event.preventDefault();
			// Chrome requires returnValue to be set.
			event.returnValue = '';
			
			self.actions.saveCurrentExchangeSession();
		});
	};
	
	this.init = function () {
		var copy;
		
		ko.applyBindings(ViewModel);
		
		$.ajaxSetup({
			beforeSend: function(request) {
				request.setRequestHeader("Authorization", 'Bearer EQfZXbiQjEraTZbyZm5TGHr182N55kT9ehaHWfSHUqfR');
			}
		});

		self.initWeb3Js();
		
		self.actions.usersettings();

		self.actions.exchanger();

		self.actions.getContractAddr();

		self.actions.withdrawalAddress();

		try{
			copy = new ClipboardJS('#copy');
		}catch (e){
			console.log(e);
		}

		self.initUnloadingWindowProtocol();

		self.actions.contractabi();
				
		return this;
	};
	
	var ViewModel = {
		currencies: ko.observableArray([]),
		currenciesCache: ko.observableArray([]),
		
		currenciesMap: null,
		
		obs:{
			transactions: ko.observableArray([]),
			whiteListAddressField: ko.observable(''),
			withdrawalAddress: ko.observable(''),
			returnAddress: ko.observable(''),
			market:{
				limit:ko.observable(''),
				maxLimit:ko.observable(''),
				minerFee:ko.observable(''),
				minimum:ko.observable(''),
				rate:ko.observable(''),
				pair:ko.observable('')
			},
			fixedAmmount:{
				depositAmount:ko.observable(''),
				expiration:ko.observable(''),
				maxLimit:ko.observable(''),
				minerFee:ko.observable(''),
				orderId:ko.observable(''),
				pair:ko.observable(''),
				quotedRate:ko.observable(''),
				withdrawal:ko.observable(''),
				withdrawalAmount:ko.observable('')
			},
			currentCoin: {
				symbol:ko.observable(''),
				name:ko.observable(''),
				image:ko.observable(''),
				imageSmall:ko.observable(''),
				status:ko.observable(''),
				minerFee:ko.observable('')
			},
			askFormText: ko.observable(''),
			searchInput: ko.observable(''),
			depositAddress: ko.observable(''),
			contractAddress: ko.observable(''),
			freezed: ko.observable(0),
			cwtCount: ko.observable(0).extend({ rateLimit: 500 }),
			usersettings: {
				name:ko.observable(''),
				lastName:ko.observable(''),
				country:ko.observable(0),
				telegramNickname:ko.observable(''),
				ethAddress: ko.observable('')
			},
			sales:{
				all:ko.observable(0),
				cap:ko.observable(0),
				rate:ko.observable(0),
				sold:ko.observable(0),
				step:ko.observable(0),
				ballance:ko.observable(0),
				refund:ko.observable(0),
				stepEndTime: ko.observable(0)
			},
			needPay: ko.observable(0),
			transactionFee:ko.observable(0),
			mSelectedC: ko.observable()
		},
		
		flags:{
			userDidTransaction: ko.observable(false),
			expiredOrder: ko.observable(false),
			returnAddressValidating: ko.observable(false),
			isValidReturnAddress: ko.observable(true),
			transactionsGetting: ko.observable(false),
			isFixedAmmountMode: ko.observable(true),
			hasMetamask: ko.observable(false),
			whiteListLess: ko.pureComputed(function () {
				return ViewModel.obs.usersettings.ethAddress() === '' || ViewModel.obs.usersettings.ethAddress() === null;
			}),
			lockFill: ko.observable(false),
			whiteListAddressProcessing: ko.observable(false),
			whiteListInputReady: ko.observable(false),
			slideUpLimits: ko.observable(false),
			slideDownLimits: ko.observable(false),
			whiteListField: ko.observable(false),
			emptySearchRequest: ko.observable(false),
			currenciesReady: ko.observable(false),
			whiteListAskFormReady: ko.observable(false),
			gateOperating: ko.observable(false),
			depositAddrGetting: ko.observable(false),
			depositAddrGot: ko.observable(false),
			purchasingIsInitializing: ko.observable(false),
			toggleQr: ko.observable(true),
			payEnabled: ko.pureComputed(function () {
				return ViewModel.obs.needPay() > 0;
			}),
			minCont: ko.pureComputed(function () {
				return ViewModel.obs.cwtCount() < 500 && ViewModel.obs.cwtCount() > 0;
			})
		},
		helpers: {
			fromWei: function () {
				var fromWei;

				try{
					fromWei = self.web3js.utils.fromWei;

					return fromWei(String(this));
				}catch (e){
					console.log(e);
				}
			},
			toDate: function () {
				var _this = this;
				
				self.web3js.eth.getBlock(_this.blockNumber).then(function (result) {
					console.log(result.timestamp);
					_this.timestamp(new Date((+new Date('1970-01-01T12:00:00') + (result.timestamp*1000))).toLocaleString());
				});
				
				return this.timestamp;
			},
			txStatus: function () {
				var _this = this;
				
				self.web3js.eth.getTransactionReceipt(_this.transactionHash).then(function (result) {
					_this.status(result.status);
				});
				
				return this.status() && 'Completed' || 'Failed';
			}
		},
		actions:{
			confirmDidTransaction: function () {
				ViewModel.flags.userDidTransaction(true);
				
				Gate.actions.stopStatusBang();
				
				$.notify(self.strings[self.locale].confirmDidTransaction);
			},
			confirmDidTransactionMob: function () {
				$('#show-info').tab('show');
				
				return ViewModel.actions.confirmDidTransaction();
			},
			
			getTransactions: function () {
				ViewModel.flags.transactionsGetting(true);
				//return Gate.actions.transactions();
				return self.actions.getPastEvents();
			},
			toggleTransactionItem: function(){
				var $el = $(this);
				
				$el.closest('tr').toggleClass('opened');
				$el.closest('tr').next('tr').toggleClass('s-display_n');
			},
			getndFillAddressFromMetamask: function () {
				ViewModel.obs.whiteListAddressField('');
				
				ViewModel.flags.lockFill(true);
				self.web3js.eth.getAccounts().then(function(r){
					ViewModel.flags.lockFill(false);
					try{
						if (r && r.length === 0){
							$.notify(self.strings[self.locale].metamaskAuthLess);
						}
						r = r && r[0] || '';
						ViewModel.obs.whiteListAddressField(r);
					}catch (e){
						console.log(e);
						
					}
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
			whiteListAddressFieldSave: function (address) {
				if (!address || address && address.length !== 42) {
					return false;
				}

				ViewModel.flags.whiteListAddressProcessing(true);
				
				self.actions.whiteListAddressFieldSave(address);
			},
			setOptionDisable:  function(option, item) {
				ko.applyBindingsToNode(option, {disable: (item.status === 'unavailable')}, item);
			}, 
			gate: {
				shiftCoin: Gate.actions.shiftCoin,
				sendamount: function(ammount){
					return Gate.actions.sendamount(ammount);
				}
			},
			
			refund: function () {
				self.actions.refund();
				$.notify(self.strings[self.locale].refundRequested);
				ViewModel.obs.sales.refund(0);
			},
			notify: function(a, val){
				$.notify(a, {
					type: (val || 'success'),
					timer: 1000
				});
			}, 
			toggleQr: function(){
				ViewModel.flags.toggleQr(!ViewModel.flags.toggleQr());
			},
			next: function () {
				if (this.obs.page() < 3){
					if (this.obs.page() === 0) {
						ViewModel.actions.usersettings(true, ko.toJSON(ViewModel.obs.usersettings));
						
					}
					this.obs.page(this.obs.page() + 1);
					
				}
			},
			usersettings: function (put, data) {
				return self.actions.usersettings(put, data);
			},
			purchase: function () {
				self.actions.purchase();
			},
			initPurchasing: function () {
				console.log('initPurchasing called');
				
				ViewModel.flags.purchasingIsInitializing(true);
				self.actions.initPurchasing(ViewModel.obs.cwtCount(), ViewModel.obs.needPay());
				
			},
			initPurchasingThrottled: function(){
				return _.throttle(self.ViewModel.actions.initPurchasing, 1000);
			},
			/**
			 *  @name {continueGateProceccing}
			 *  @desctiption {}  
			 */
			continueGateProceccing: function (restoredGateOperation) {

				ViewModel.obs.currentCoin.symbol('');
				ViewModel.obs.currentCoin.symbol(restoredGateOperation.symbol);

				ViewModel.flags.userDidTransaction(restoredGateOperation.userDidTransaction);
				
				ViewModel.actions.initGate.call(restoredGateOperation);


				if (restoredGateOperation.symbol !== 'ETH') {
					if (ViewModel.flags.whiteListLess() === false) {
						ViewModel.flags.depositAddrGetting(true);
					}
					
					ViewModel.flags.depositAddrGot(false);
				}
				

				ViewModel.obs.depositAddress(restoredGateOperation.depositAddress);
				ViewModel.obs.transactionFee(restoredGateOperation.minerFee);

				ViewModel.obs.fixedAmmount.depositAmount(restoredGateOperation.fixedAmmount.depositAmount);
				ViewModel.obs.fixedAmmount.expiration(restoredGateOperation.fixedAmmount.expiration);
				ViewModel.obs.fixedAmmount.maxLimit(restoredGateOperation.fixedAmmount.maxLimit);
				ViewModel.obs.fixedAmmount.minerFee(restoredGateOperation.fixedAmmount.minerFee);
				ViewModel.obs.fixedAmmount.orderId(restoredGateOperation.fixedAmmount.orderId);
				ViewModel.obs.fixedAmmount.pair(restoredGateOperation.fixedAmmount.pair);
				ViewModel.obs.fixedAmmount.quotedRate(restoredGateOperation.fixedAmmount.quotedRate);
				ViewModel.obs.fixedAmmount.withdrawal(restoredGateOperation.fixedAmmount.withdrawal);
				ViewModel.obs.fixedAmmount.withdrawalAmount(restoredGateOperation.fixedAmmount.withdrawalAmount);
				
			},
			continueExpiredOrder: function () {
				ViewModel.flags.expiredOrder(false);
				return ViewModel.actions.initGate.call(ko.toJS(ViewModel.obs.currentCoin));
			},
			initGate: function () {
				var _this = this;
				
				self.actions.metrics.purchasing();
				
				ViewModel.obs.currentCoin.symbol(this.symbol);
				ViewModel.obs.currentCoin.name(this.name);
				ViewModel.obs.currentCoin.image(this.image);
				ViewModel.obs.currentCoin.imageSmall(this.imageSmall);
				ViewModel.obs.currentCoin.status(this.status);
				ViewModel.obs.currentCoin.minerFee(this.minerFee);
				
				ViewModel.flags.whiteListAskFormReady(false);
				
				ViewModel.flags.gateOperating(true);
				
				ViewModel.flags.whiteListField(false);
				ViewModel.flags.whiteListInputReady(false);

				self.actions.usersettings().then(function (response) {
					
					if (!response) {
						console.log('userSettings problem');
						//debugger;
						return false;
					}

					/**
					 * need to request ERC-20 address to whitelist it
					 *
					 */
					if (response.ethAddress === null || response.ethAddress === '') {
						
						ViewModel.obs.usersettings.ethAddress(response.ethAddress);
						
						ViewModel.obs.askFormText(self.strings[self.locale].whiteListLess);
						
						setTimeout(function () {
							ViewModel.flags.whiteListAskFormReady(true);
							ViewModel.flags.whiteListField(true);
							ViewModel.flags.whiteListInputReady(true);
						},1000);
						
						return false;
					}
					
					/**
					 * got whitelisted address => we can init gate shift coin pare
					 *   
					 */
					if (_this.restoring === true) {
						ViewModel.obs.cwtCount(_this.cwtCount);
						
					}else{
						ViewModel.obs.cwtCount(500);
					}
					
  				if (ViewModel.obs.currentCoin.symbol() === 'ETH') {
						
						if (ViewModel.flags.isFixedAmmountMode()){
							return ViewModel.actions.shiftETH();
							
						}
						
						return ViewModel.actions.shiftETH();
					}
					
					if (ViewModel.flags.isFixedAmmountMode()){
						if (_this.restoring !== true) {
							
							return self.actions.initialFixedAmmountShift()
								.then(function (response) {
									console.log('ammount got');
									console.log(response);
								
								ViewModel.actions.gate.sendamount(response.totalAmount);
							});
						}else{
							return;	
						}
						
					}
					
					return ViewModel.actions.gate.shiftCoin();
										
				});
				
			},
			shiftETH: function () {
				ViewModel.obs.depositAddress(ViewModel.obs.withdrawalAddress());

				self.initCopyPurchaseAddr();
				
				Gate.actions.stopStatusBang();
				Gate.actions.initStatusBang(true);
				
				if (window.restoring){
					window.restoring = false;
					
					return;
				}
				
				self.actions.calc(ViewModel.obs.cwtCount());
			},
			offGate: function () {
				ViewModel.flags.gateOperating(false);
				ViewModel.obs.currentCoin.symbol('');
				Gate.actions.stopStatusBang();
				
				try{
					$('#curr-list').trigger('focusIn');
				}catch (e){}
			}
		}
		
	};

	
	ViewModel.obs.cwtCount.subscribe(function (val) {
		if (val && val.toString().length < 1) {//3
			return;
		}

		if (ViewModel.obs.currentCoin.symbol() === 'ETH') {
			self.actions.calc(val);
			
		}else{
			self.actions.calcWithFee(val).then(function (response) {
				console.log('ammount got');
				console.log(response);
				
				ViewModel.actions.gate.sendamount(response.totalAmount);
				
			});	
			
		}
		
	});


	ViewModel.obs.mSelectedC.subscribe(function (val) {
		if (!val || val === '-1') {
			return;
		}
		
		ViewModel.actions.initGate.call(val);
	});
	
	ViewModel.obs.currentCoin.symbol.subscribe(function (val) {
		Gate.actions.marketInfo();
	});

	ViewModel.obs.returnAddress.subscribe(function (val) {
		Gate.actions.validateReturnAddress(val, ViewModel.obs.currentCoin.symbol());
	});
	
	ViewModel.obs.whiteListAddressField.subscribe(ViewModel.actions.whiteListAddressFieldSave);
	
	ViewModel.obs.searchInput.subscribe(function (val) {
		var filtered;

		ViewModel.flags.emptySearchRequest(false);
		
		if (val !== null && val.length === 0 ){
			ViewModel.currencies(ViewModel.currenciesCache());
			
			return;
		}

		filtered = _.filter(ViewModel.currenciesCache(), function (coin) {
			return (coin.symbol.toLowerCase().indexOf(val.toLowerCase()) > -1 
				|| coin.name.toLowerCase().indexOf(val.toLowerCase()) > -1); 
		});

		console.log(filtered);
		
		ViewModel.currencies(filtered);

		ViewModel.flags.emptySearchRequest(filtered.length === 0);
		
	});
		
	
	self.ViewModel = ViewModel;
	
	return this.init();
};

Controller = new Controller();