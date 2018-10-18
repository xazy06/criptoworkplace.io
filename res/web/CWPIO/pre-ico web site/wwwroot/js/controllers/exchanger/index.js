var Controller = function () {
	
	var self = this;
	
	this.strings = {
		ru:{
			
		},
		en:{
			
		}
	};
	
	this.api = {
		usersettings:'/api/v1/usersettings',
		exchanger:'/api/v1/exchanger',
		calc:'/api/v1/exchanger/calc/',
		purchase: '/api/v1/exchanger/addr',
		initPurchasing: '/api/v1/exchanger/initPurchasing'
	};
	
	this.actions = {
		getContractAddr: function () {
			$.getJSON(self.api.purchase).done(function (result) {
				ViewModel.obs.contractAddress(result);
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
					
				});
				
			}

			$.ajax({
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

				if (ViewModel.obs.usersettings.ethAddress()){
					ViewModel.obs.page(2);
				}
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
		})();
	};
	
	this.init = function () {
		ko.applyBindings(ViewModel);

		self.initWeb3Js();
		
		self.actions.usersettings();

		self.actions.exchanger();

		self.actions.getContractAddr();

		var copy = new ClipboardJS('#copy');
		
		return this;
	};
	
	var ViewModel = {
		currencies: ko.observableArray([]),
		currenciesCache: ko.observableArray([]),
		
		currenciesMap: null,
		
		obs:{
			market:{
				limit:ko.observable(''),
				maxLimit:ko.observable(''),
				minerFee:ko.observable(''),
				minimum:ko.observable(''),
				rate:ko.observable(''),
				pair:ko.observable('')
			},
			currentCoin: {
				symbol:ko.observable(''),
				name:ko.observable(''),
				image:ko.observable(''),
				imageSmall:ko.observable(''),
				status:ko.observable(''),
				minerFee:ko.observable('')
			},
			searchInput: ko.observable(''),
			depositAddress: ko.observable(''),
			selectedCurrency:ko.observable(''),
			returnAddress:ko.observable(''),
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
			needPay: ko.observable(0)
		},
		
		flags:{
			whiteListLess: ko.observable(false),
			whiteListAskFormNotReady: ko.observable(false),
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
			setOptionDisable:  function(option, item) {
				ko.applyBindingsToNode(option, {disable: (item.status === 'unavailable')}, item);
			}, 
			gate: {
				shiftCoin: Gate.actions.shiftCoin
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
				self.actions.initPurchasing(this.obs.cwtCount(), this.obs.needPay());

				try{
					yaCounter50462326.reachGoal('Purchase');
					ga('send', 'event', 'forms', 'purchase');
				}catch (e){					
				}
			},
			initGate: function () {
				ViewModel.obs.currentCoin.symbol(this.symbol);
				ViewModel.obs.currentCoin.name(this.name);
				ViewModel.obs.currentCoin.image(this.image);
				ViewModel.obs.currentCoin.imageSmall(this.imageSmall);
				ViewModel.obs.currentCoin.status(this.status);
				ViewModel.obs.currentCoin.minerFee(this.minerFee);
				
				ViewModel.obs.selectedCurrency(this.symbol);//TODO redunant ~
				
				ViewModel.flags.gateOperating(true);

				ViewModel.actions.gate.shiftCoin();

			},
			offGate: function () {
				ViewModel.flags.gateOperating(false);
			}
		}
		
	};

	
	ViewModel.obs.cwtCount.subscribe(function (val) {
		self.actions.calc(val);
	});

	ViewModel.obs.selectedCurrency.subscribe(function (val) {
		Gate.actions.marketInfo();
	});

	ViewModel.obs.searchInput.subscribe(function (val) {
		var filtered;

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
		
	});
		
	
	ViewModel.obs.cwtCount(500);
	
	self.ViewModel = ViewModel;
	
	return this.init();
};

Controller = new Controller();