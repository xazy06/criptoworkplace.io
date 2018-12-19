var
	$html,
	$body,
	$partnersSlider,
	$ratingSlider,
	windowHeight,
	windowWidth,
	$header,
	$headerNav,
	$scrollTop = 0,
	lastScrollTop = 0,
	$menuTrigger,
	$mainMenu,
	$mainMenuLink,
	$moreTeamLink,
	$teamList,
	maxRating,
	ratingStar,
	activeRating,
	checkElementMove = false,
	degree = 0.0174532925,
	mediaPoint1 = 1024,
	mediaPoint2 = 768,
	mediaPoint3 = 480,
	mediaPoint4 = 320;

$(document).ready(function ($) {
	lastScrollTop = 0;
	$html = $('html');
	$body = $('body');
	$header = $('.header');
	$headerNav = $('.header_nav');
	$partnersSlider = $('.partners_slider');
	$ratingSlider = $('.rating_list');
	$menuTrigger = $('.menuTrigger');
	$mainMenu = $('.main_menu.header_mod');
	$mainMenuLink =$('.main_menu_link');
	$moreTeamLink = $('.moreTeamLink');
	$teamList = $('.team_list');

	$moreTeamLink.on('click', function (e) {
		// $teamList.toggleClass('visible_all_mod');
		$('.team_hidden_list').slideToggle(600);

		var elementClick = $(this).attr('href');
		var destination = $(elementClick).offset().top;
		if ($(this).hasClass('active_mod')) {
			$html.animate({ scrollTop: destination }, 600);
		}
		
		$(this).toggleClass('active_mod');
	});

	$menuTrigger.on('click', function () {
		if ($body.hasClass('menu_open')) {
			$body.removeClass('menu_open');
			$(this).removeClass('active_mod');
		} else {
			$body.addClass('menu_open');
			$(this).addClass('active_mod');
		}
	});

	svg4everybody();

	$('.rating_block').each(function(index, element) {
		maxRating = $(this).find('.rating_stars_w').data('max');
		ratingStar = $(this).find('.rating_amount').text();
		activeRating = Math.round(ratingStar / maxRating * 5);
		if (activeRating > 0) {
			for (var i = 0; i < activeRating; i++) {
				$(this).find('.icon.rating_mod').eq(i).addClass('active_mod');
			}
		}
	})

	$mainMenuLink.click(function () {
		var elementClick = $(this).attr('href');
		var destination = $(elementClick).offset().top;
		$html.animate({ scrollTop: destination }, 1000);
		$menuTrigger.trigger('click');
	});

	$ratingSlider.slick({
		prevArrow: $('.rating_slider_prev'),
		nextArrow: $('.rating_slider_next'),
		infinite: true,
		slidesToShow: 5,
		slidesToScroll: 1,
		dots: false,
		arrows: true,
		autoplay: false,
		autoplaySpeed: 2000,
		draggable: false,
		swipe: true,
		touchMove: true,
		responsive: [
			{
				breakpoint: 767,
				settings: 'unslick'
			}
		]
	});

	$partnersSlider.slick({
		prevArrow: $('.js_slider_prev'),
		nextArrow: $('.js_slider_next'),
		infinite: true,
		slidesToShow: 5,
		slidesToScroll: 1,
		dots: false,
		arrows: true,
		autoplay: false,
		autoplaySpeed: 2000,
		draggable: false,
		swipe: true,
		touchMove: true,
		responsive: [
			{
				breakpoint: 1024,
				settings: {
					slidesToShow: 3,
					slidesToScroll: 3
				}
			},
			{
				breakpoint: 801,
				settings: {
					slidesToShow: 3,
					slidesToScroll: 3
				}
			},
			{
				breakpoint: 641,
				settings: {
					slidesToShow: 2,
					slidesToScroll: 2
				}
			},
			{
				breakpoint: 481,
				settings: {
					slidesToShow: 2,
					slidesToScroll: 2,
					arrows: false,
					variableWidth: true,
					dots: true,
					appendDots: $('.parnters_slider_w')
				}
			}
		]
	});

	//developer funcitons
	// pageWidget(['index']);
	// getAllClasses('html','.elements_list');

	// video player

	var player = new Plyr('#player');

	$('.video_btn').on('click', function(e) {
		e.preventDefault();

		$('.video_btn').addClass('hidden_mod');
		player.togglePlay();
		$('.video_bg_w').addClass('hidden_mod');
	});

	player.on('ended', function(data) {
		$('.video_btn').removeClass('hidden_mod');
		$('.video_bg_w').removeClass('hidden_mod');
	});

		// lang menu 

		$('.lang_control').on('click',function() {
			$('.lang_menu').toggleClass('active_mod');
		});
	
		$('.lang_item').on('click', function(e) {
			e.preventDefault();
			$('.lang_item').removeClass('active_mod');
			$(this).addClass('active_mod');
			$('.lang_menu').toggleClass('active_mod');
		});
	
		$(document).mouseup(function (e) {
			var container = $('.lang_menu');
			if (container.has(e.target).length === 0){
				$('.lang_menu').removeClass('active_mod');
			}
		});

});

$(window).on('load', function () {
	updateSizes();
	loadFunc();
});

$(window).on('resize', function () {
	resizeFunc();
});

$(window).on('scroll', function () {
	scrollFunc();
});

function loadFunc() {
	moveElement();
}

function resizeFunc() {
	updateSizes();
	moveElement();
}

function scrollFunc() {
	$scrollTop = $(window).scrollTop();

	var stored_scrollTop = false;

	if (!stored_scrollTop && lastScrollTop > $scrollTop) {
		stored_scrollTop = lastScrollTop;
	}

	if ($scrollTop === 0 && $header.hasClass('scroll_mod') || stored_scrollTop - 10 > $scrollTop && $header.hasClass('scroll_mod')) {
		$header.removeClass('scroll_mod');
	} else if ($scrollTop > 0 && lastScrollTop < $scrollTop && !$header.hasClass('scroll_mod')) {
		$header.addClass('scroll_mod');
		stored_scrollTop = false;
	}

	if ($scrollTop < 10) {
		$headerNav.removeClass('bg_mod');
	} else {
		$headerNav.addClass('bg_mod');
	}

	lastScrollTop = $scrollTop;
}

function moveElement() {
	if (windowWidth < 768) {
		if(checkElementMove !== true) {
			$mainMenu.detach().insertAfter('.header_nav');
			checkElementMove = true;
		}
	} else if (checkElementMove) {
		$mainMenu.detach().insertAfter('.logo.header_mod');
		checkElementMove = false;
	}
}

// function headerScroll() {
// 	if($scrollTop > 10) {
// 			if (!$header.hasClass('scroll_mod')) {
// 				$header.addClass('scroll_mod');
// 			}
// 		} else if ($scrollTop < 10) {
// 			$header.removeClass('scroll_mod');
// 		}
// }

function updateSizes() {
	windowWidth = window.innerWidth;
	windowHeight = window.innerHeight;
}

if ('objectFit' in document.documentElement.style === false) {
	document.addEventListener('DOMContentLoaded', function () {
		Array.prototype.forEach.call(document.querySelectorAll('img[data-object-fit]'), function (image) {
			(image.runtimeStyle || image.style).background = 'url("' + image.src + '") no-repeat 50%/' + (image.currentStyle ? image.currentStyle['object-fit'] : image.getAttribute('data-object-fit'));

			image.src = 'data:image/svg+xml,%3Csvg xmlns=\'http://www.w3.org/2000/svg\' width=\'' + image.width + '\' height=\'' + image.height + '\'%3E%3C/svg%3E';
		});
	});
}

function getRandomInt(min, max) {
	return Math.floor(Math.random() * (max - min)) + min;
}

function getRandom(min, max) {
	return Math.random() * (max - min) + min;
}
