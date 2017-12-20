/**
 * Created by Admin on 6/5/2017.
 */
var index = angular.module('opiniiModule', ['ngRoute'])

index.controller('opiniiController', ['navigateFactory','$scope','$location',
    function (navigateFactory,$scope) {
        $scope.navigateFactory= navigateFactory;
        $scope.userLogged = navigateFactory.getUserLogged();
    }
        ]
);


