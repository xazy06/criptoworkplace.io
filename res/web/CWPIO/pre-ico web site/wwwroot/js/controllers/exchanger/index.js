var Controller = function () {
	
	var self = this;
	
	this.locale = 'en';
	
	this.strings = {
		ru:{
			whiteListLess:'',
			whiteListAddressFieldSave: {
				fail: '',
				successSaved:'',
				success: ''
			}
		},
		en:{
			whiteListLess: 'You need to set up ETH Address to continue purchasing',
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
		calc:'/api/v1/exchanger/calcExchange/',
		purchase: '/api/v1/exchanger/addr',
		initPurchasing: '/api/v1/exchanger/initPurchasing',
		monitor: '/api/v1/exchanger/monitor/'
	};
	
	this.actions = {
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
				
				debugger;
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
				ViewModel.obs.askFormText(self.strings[self.locale].whiteListAddressFieldSave.successSaved);
				ViewModel.flags.whiteListAddressProcessing(true);
				ViewModel.obs.whiteListAddressField('');
				
				setTimeout(function () {
					//window.addr = '11';//TODO for test
					ViewModel.actions.initGate.call(ko.toJS(ViewModel.obs.currentCoin));
					ViewModel.flags.whiteListAddressProcessing(false);
				},3000);
				
			});
			
		},
		
		getContractAddr: function () {
			$.getJSON(self.api.purchase).done(function (result) {
				ViewModel.obs.contractAddress(result);
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
				
				$('#m_modal_4').modal('show');
				
				try{
					ViewModel.obs.freezed(response.fixRate.rate);
				}catch (e){
					console.log(e);
				}
				
			}).fail(function (resp, e) {
				ViewModel.flags.purchasingIsInitializing(false);
				debugger;
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
					console.log()	
				}
				
				ViewModel.obs.usersettings.name(response.name);
				ViewModel.obs.usersettings.lastName(response.lastName);
				ViewModel.obs.usersettings.ethAddress(response.ethAddress);
				ViewModel.obs.usersettings.country(response.country);
				ViewModel.obs.usersettings.telegramNickname(response.telegramNickname);

				//redunant
				// if (ViewModel.obs.usersettings.ethAddress()){
				// 	ViewModel.obs.page(2);
				// }
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

		initialFixedAmmountShift: function (currencyName, rate) {
			console.log('initialFixedAmmountShift');
			console.log(currencyName);
			console.log(rate);
			
			return self.actions.calcWithFee(500, currencyName, rate)
		},

		_calc: function (count) {
			count = count || 0;

			return $.ajax({
				url:self.api.calc + count,
				method:'GET'
			});
		},
		
		calcWithFee: function (count, currencyName, rate) {
			count = count || 0;

			return $.ajax({
				url:self.api.calc + count,
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
				ViewModel.obs.needPay(response);
			});
		},
		purchase: function () {
			$.getJSON(self.api.purchase).done(function (result) {
				
				try{
				self.web3js.eth.sendTransaction({ 
					from: ViewModel.obs.usersettings.ethAddress(), 
					to: result, 
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
				ViewModel.flags.hasMetamask(web3.currentProvider.isMetaMask === true);
			}catch (e){
				console.log(e);
			}
			
		})();
	};
	
	this.initCopyPurchaseAddr = function (id) {
		var copy2 = new ClipboardJS(id || '#copy2');
	};
	
	this.init = function () {
		ko.applyBindings(ViewModel);

		self.initWeb3Js();
		
		self.actions.usersettings();

		self.actions.exchanger();

		self.actions.getContractAddr();

		self.actions.withdrawalAddress();

		var copy = new ClipboardJS('#copy');
		
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
			page:ko.observable(0),
			cwtCount: ko.observable(0),
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
			transactionFee:ko.observable(0)
		},
		
		flags:{
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
			toggleQr: ko.observable(false),
			check1:ko.observable(false),
			check2:ko.observable(false),
			check3:ko.observable(false),
			check4:ko.observable(false),
			check5:ko.observable(false),
			check6:ko.observable(false),
			nextEnabled: ko.pureComputed(function () {
				return ViewModel.flags.check1() && ViewModel.flags.check2() && ViewModel.obs.usersettings.ethAddress();
			}),
			next2Enabled: ko.pureComputed(function () {
				return ViewModel.flags.check3() && ViewModel.flags.check4() && ViewModel.flags.check5() && ViewModel.flags.check6(); 
			}),
			payEnabled: ko.pureComputed(function () {
				return ViewModel.obs.needPay() > 0;
			}),
			minCont: ko.pureComputed(function () {
				return ViewModel.obs.cwtCount() < 500;
			})
		},
		
		actions:{
			getTransactions: function () {
				return Gate.actions.transactions();
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
				ViewModel.flags.purchasingIsInitializing(true);
				self.actions.initPurchasing(ViewModel.obs.cwtCount(), ViewModel.obs.needPay());

				try{
					yaCounter50462326.reachGoal('Purchase');
					ga('send', 'event', 'forms', 'purchase');
				}catch (e){
					console.log(e);
				}
			},
			initGate: function () {
								
				ViewModel.obs.currentCoin.symbol(this.symbol);
				ViewModel.obs.currentCoin.name(this.name);
				ViewModel.obs.currentCoin.image(this.image);
				ViewModel.obs.currentCoin.imageSmall(this.imageSmall);
				ViewModel.obs.currentCoin.status(this.status);
				ViewModel.obs.currentCoin.minerFee(this.minerFee);
				
				ViewModel.flags.whiteListAskFormReady(false);
				
				ViewModel.flags.gateOperating(true);

				//ViewModel.flags.slideUpLimits(false);
				ViewModel.flags.whiteListField(false);
				ViewModel.flags.whiteListInputReady(false);

				self.actions.usersettings().then(function (response) {

					//TEST
					//response.ethAddress = window.addr || '';
					//response.ethAddress = window.addr || response.ethAddress;
					
					if (!response) {
						console.log('userSettings problem');
						debugger;
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
							//ViewModel.flags.slideUpLimits(true);
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
					ViewModel.obs.cwtCount(500);
					
  				if (ViewModel.obs.currentCoin.symbol() === 'ETH') {
						
						if (ViewModel.flags.isFixedAmmountMode()){
							return ViewModel.actions.shiftETH();
						}
						
						return ViewModel.actions.shiftETH();
					}
					
					if (ViewModel.flags.isFixedAmmountMode()){
						return self.actions.initialFixedAmmountShift(ViewModel.obs.currentCoin.symbol(), ViewModel.obs.market.rate())
							.then(function (response) {
								console.log('ammount got');
								console.log(response);
							
							ViewModel.actions.gate.sendamount(response.totalAmount);
						});	
					}
					
					return ViewModel.actions.gate.shiftCoin();
										
				});
				
			},
			shiftETH: function () {
				ViewModel.obs.depositAddress(ViewModel.obs.contractAddress());
				ViewModel.actions.initPurchasing();
			},
			offGate: function () {
				ViewModel.flags.gateOperating(false);
			}
		}
		
	};

	
	ViewModel.obs.cwtCount.subscribe(function (val) {
		if (val && val.toString().length < 3) {
			return;
		}
		
		self.actions.calcWithFee(val).then(function (response) {
			console.log('ammount got');
			console.log(response);

			ViewModel.actions.gate.sendamount(response.totalAmount);
		});
	});

	ViewModel.obs.currentCoin.symbol.subscribe(function (val) {
		Gate.actions.marketInfo();
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