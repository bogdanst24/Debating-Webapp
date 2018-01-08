var app = angular.module("dezbateriApp", [
    'ngRoute',
    'ngSanitize',

    'homepageModule',
    'debateModule',
    'opiniiModule',
    'opinionModule',
    'dezbateriModule',
    'loginModule',
    'adminModule',

    'userServices',
    'navigateServices',
    'debateServices',
    'loginServices',
    'opiniiServices',

    'errorHandler'
]);

app.constant('API_URL', 'http://localhost:18586/api');

app.factory('authInterceptor', function($q,$location){
    return{
        request: function(config){
            config.headers = config.headers || {};
            if(localStorage.auth_token){
                config.headers.token = localStorage.auth_token;
            }
            return config;
        },
        responseError: function(response){
            if(response.status === 401){
                localStorage.removeItem('auth_token');
                $location.path('/login');
            }
            return $q.reject(response);
        }
    }
});


app.config(function($httpProvider){
    $httpProvider.interceptors.push('authInterceptor');
});


app.config(function ($routeProvider) {

    $routeProvider
    .when('/', {
        templateUrl: 'app/View/homepage.html'
    })
    .when('/dezbateri',{
        templateUrl: 'app/View/dezbateri.html'
    })
    .when('/opinii',{
        templateUrl: 'app/View/opinii.html'
    })
    .when('/login',{
        templateUrl: 'app/View/login.html'
    })
    .when('/verify', {
        templateUrl: 'app/View/verify.html'
    })
    .when('/debate',{
        templateUrl: 'app/View/debate.html'
    })
    .when('/opinie',{
        templateUrl: 'app/View/opinie.html'
    })
    .when('/manage',{
        templateUrl: 'app/View/admin.html'
    })

});