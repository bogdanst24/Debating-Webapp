$(document).ready(function () {

    'use strict';
    var btn = $('#menu-button');
    var mainmenu = $("#menu-header");

    btn.click (function(){
        var height = mainmenu.height();
        if(height === 0){
            mainmenu.animate({height : "220px"},300);
        } else {
            mainmenu.animate({height : "0"},300);
        }
    });

    var c, currentScrollTop = 0;

    $(window).scroll(function () {
        var navbar = $('#header-main-landing');
        var img = $('#nav-img');
        var second = $('#header-secondary-landing');
        var content = $('#main-content');
        var a = $(window).scrollTop();
        var b = img.outerHeight() + navbar.outerHeight() +100;

       currentScrollTop = a;

        if (c < currentScrollTop && a > b ) {

            navbar.addClass("scrollUp");
            second.addClass("fixit-top");
            content.addClass("margin-top-main-content");
        } else if (c > currentScrollTop && !(a <= b)) {
            navbar.removeClass("scrollUp");
            second.removeClass("fixit-top");
            second.removeClass("fixit");
            content.removeClass("margin-top-main-content");
        }

        if(currentScrollTop >=  img.outerHeight() - navbar.outerHeight() +100){
            second.addClass("fixit");
            content.addClass("margin-top-main-content");
        } else {
            second.removeClass("fixit");
            content.removeClass("margin-top-main-content");
        }


        c = currentScrollTop;


    });

});