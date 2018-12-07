var Controller = function () {

	var self = this;

	this.options = {
		flagsPath: '/assets/app/media/img/flags/'
	};

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

	this.changeLocale = function (locale) {
		
		console.log('locale changing to ', locale);

		ViewModel.actions.storeLang(locale);
		
		try{

			i18n.setLng(locale).then(function(){
				$("body").i18n();
			});

			console.log('locale changed');
			
		}catch (e){

		}
	};

	this.init = function () {
		ko.applyBindings(ViewModel);

		ViewModel.actions.getStoredLang();
		
		return this;
	};

	var ViewModel = {
		obs:{
			email: ko.observable(''),
			activeLang: ko.observable('en')
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
			},
			changeLang: function () {
				return self.changeLocale(''+this);
			},
			storeLang: function (locale) {
				window.localStorage.setItem('activeLang', locale);
				
			},
			getStoredLang: function () {
				var activeLang = window.localStorage.getItem('activeLang');
				
				ViewModel.obs.activeLang(activeLang);
				
				ViewModel.actions.changeLang.call(activeLang);
			}
		}

	};
	
	return this.init();
};

Controller = new Controller();