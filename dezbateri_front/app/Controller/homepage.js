var index = angular.module('homepageModule', ['ngRoute']);

index.run(['$anchorScroll', function($anchorScroll) {
        $anchorScroll.yOffset = 50;   // always scroll by 50 extra pixels
}]);

index.controller('homepageController', [ 'navigateFactory','$scope',
    function (navigateFactory,$scope) {


            this.navigateFactory = navigateFactory;
            this.userLogged = navigateFactory.getUserLogged();

            var scroll = 0;
            $("a.anchor1").click(function (e) {
                e.preventDefault();
                $("p.anchor1").scrollintoview({ duration: "slow", viewPadding: { y: scroll } });
            });
            $("a.anchor2").click(function (e) {
                e.preventDefault();
                $("p.anchor2").scrollintoview({ duration: "slow", viewPadding: { y: scroll } });
            });
            $("a.anchor3").click(function (e) {
                e.preventDefault();
                $("p.anchor3").scrollintoview({ duration: "slow", viewPadding: { y: scroll } });
            });
            $("a.anchor4").click(function (e) {
                e.preventDefault();
                $("p.anchor4").scrollintoview({ duration: "slow", viewPadding: { y: scroll } });
            });
            $("a.anchor5").click(function (e) {
                e.preventDefault();
                $("p.anchor5").scrollintoview({ duration: "slow", viewPadding: { y: scroll } });
            });



        }]
    
);

