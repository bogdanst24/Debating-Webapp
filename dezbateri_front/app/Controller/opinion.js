var index = angular.module('opinionModule', ['ngRoute', 'angularTrix', 'ngSanitize']);

index.controller('opinionController', ['navigateFactory', 'debateFactory', '$location', '$timeout', '$filter', '$sce',
    function (navigateFactory, debateFactory, $location, $timeout, $filter, $sce) {
        var _this = this;
        
        _this.navigateFactory = navigateFactory;
        _this.userLogged = navigateFactory.getUserLogged();
        var urlParams = $location.search();

        _this.OpinionFull.Id = urlParams.deb_id;;
        _this.OpinionFull.Subject = null;
        _this.OpinionFull.Date_created = null;
        _this.OpinionFull.User_email = null;
        _this.OpinionFull.User_username = null;
        _this.OpinionFull.Picture_url = null;


    }
]);