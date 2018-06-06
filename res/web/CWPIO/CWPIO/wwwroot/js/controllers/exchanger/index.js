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
		calc:'/api/v1/exchanger',
		purchase: '/api/v1/exchanger/addr'
	};
	
	this.actions = {
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
				ViewModel.obs.usersettings.Name(response.Name);
				ViewModel.obs.usersettings.LastName(response.LastName);
				ViewModel.obs.usersettings.Country(response.Country);
				ViewModel.obs.usersettings.TelegramNickname(response.TelegramNickname);
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
			});
		},

		calc:function (count) {

			$.ajax({
				contentType: 'application/json',
				url:self.api.calc,
				data:count, 
				method:'GET'
			}).done(function (response) {
				ViewModel.obs.needPay(response);
			});
		},
		purchase: function () {
			$.getJSON(self.api.purchase).done(function (result) {
				//TODO
				web3js.eth.sendTransaction({ 
					from: $('#ethAddr').val(), 
					to: result, 
					value: web3js.utils.toWei($('#toPay').text()) 
				});
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
		
		return this;
	};
	
	
	var ViewModel = {
		obs:{
			page:ko.observable(0),
			usersettings: {
				Name:ko.observable(0),
				LastName:ko.observable(0),
				Country:ko.observable(0),
				TelegramNickname:ko.observable(0),
				ethAddr: ko.observable('')
			},
			sales:{
				cap:ko.observable(0),
				rate:ko.observable(0),
				sold:ko.observable(0),
				step:ko.observable(0),
				ballance:ko.observable(0)
			},
			needPay: ko.observable(0)
		},
		
		flags:{
			check1:ko.observable(false),
			check2:ko.observable(false),
			check3:ko.observable(false),
			check4:ko.observable(false),
			check5:ko.observable(false),
			check6:ko.observable(false),
			nextEnabled: ko.pureComputed(function () {
				return ViewModel.flags.check1() && ViewModel.flags.check2() && ViewModel.obs.usersettings.ethAddr();
			}),
			next2Enabled: ko.pureComputed(function () {
				return ViewModel.flags.check3() && ViewModel.flags.check4() && ViewModel.flags.check5() && ViewModel.flags.check6(); 
			})
		},
		
		actions:{
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
			}
		}
		
	};
	
	return this.init();
};

Controller = new Controller();