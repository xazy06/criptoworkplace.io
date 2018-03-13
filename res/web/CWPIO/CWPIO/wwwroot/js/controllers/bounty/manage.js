/*jshint esversion: 6, node:true, unused:false */
'use strict';

import CWP from 'cwp';

export default CWP.Controller.extend({

	programms: null,

	actions: {
		create: function() {
			
		},
		
		update: function () {
			
		},

		remove: function () {

		}
	},

	init: function(){
		
	}

});

(function() {
	function Controller() {
		'use strict';

		var controller = (function(){

			return {
				
			};

		}());

		return { 'default': controller };
	}

	define('bounty-manage', [], Controller);
})();
