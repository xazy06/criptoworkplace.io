'use strict';

var App = {
	locale: 'en'
};

App.extensions = {
	bindingHandlers: {

		popup: ko.bindingHandlers.popup = {
			init: function (element, valueAccessor) {

				$(element).magnificPopup(ko.unwrap(valueAccessor()));
				
			}
		}
		
	}
};

App.helpers = {
	modal: function (text, type) {
		var classs = type || 'white-popup';
		
		$.magnificPopup.open({
			items: {
				src: ['<div class="', classs, '">', text, '</div>'].join(''),
				type: 'inline'
			}
		});
	},
	ge: function ge(id){
		return document.getElementById(id);
	}
};

App._resources = {
	homeFormSendSuccess: {
		ru: 'Заявка на участие получена, спасибо!',
		en: 'Participation request successfully received!'
	},
	homeFormSendFail: {
		ru: 'Извините произошла ошибка, попробуйте позже.',
		en: 'Server Error please try again later'
	}
};

App.resources = function (res) {
	return App._resources[res][App.locale];
};

window.hel = App.helpers;