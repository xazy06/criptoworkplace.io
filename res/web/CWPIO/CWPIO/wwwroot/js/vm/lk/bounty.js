var Bounty = (function () {

	var ViewModel,
		Controller = function () {
		this.actions = {
			list: {
				get: function () {
					
				}
			}
		};	
		
		this.init = function () {
			
			this.actions.list.get();
			
			return this;
		}.bind(this);
		
		
		return this.init();
	};

	ViewModel = {
		flags:{},
		observers:{},
		observables:{
			list: ko.observableArray([])
		},
		actions:{
			activateProgramm: function () {
				
			}
		}
	};
	
	Controller = new Controller();
	
}());