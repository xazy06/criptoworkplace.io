var Bounty = (function () {

	var ViewModel,
		Controller = function () {
		
		var self = this;
		
		this.api = {
			list: '/api/v1/bounty/'
		}; 
		this.actions = {
			list: {
				get: function () {
					$.get(self.api.list, function (response) {
						console.log(response);
						ViewModel.observables.list(response)
					})
				},
				join: function (id) {
					$.post(self.api.list + id + '/join', function (response) {
						console.log(response);	
					})
				},
				leave: function (id) {
					$.ajax({
						url: self.api.list + id,
						type: 'DELETE',
						dataType: 'application/json'
					}).done(function (response) {
						console.log(response);
					});
				}
			}
		};	
		
		this.init = function () {
			ko.applyBindings(ViewModel);
			
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
			join: function () {
				Controller.actions.list.join(this.id);
			},
			leave: function () {
				Controller.actions.list.leave(this.id);
			}
		}
	};
	
	Controller = new Controller();
	
}());