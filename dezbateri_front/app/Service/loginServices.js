(function () {
    var userServices = angular.module('loginServices', []);

    userServices.factory('loginFactory', ['$http', 'API_URL',
        function ($http, API_URL) {

            return {
                login: function (user) {
                    return  $http.post(
                        API_URL + '/login/login',
                        JSON.stringify(user),
                        {
                            headers : {
                                'Content-Type' : 'application/json; charset=UTF-8'
                            }
                        });
                },

                register: function (user) {
                    return $http.post(API_URL + '/login/register', JSON.stringify(user));
                },

                checkVerificationCode: function(content){
                    return $http.post(API_URL + '/login/CheckCode', JSON.stringify(content));
                },

                resendVerificationCode: function(content){
                    return $http.post(API_URL + '/login/ResendCode', JSON.stringify(content));
                },

                decode: function(){
                    var token;
                    token = localStorage.auth_token;
                    var decoded = jwt_decode(token);
                    return decoded.sub;
                },

                checkAdmin: function(token){
                    var role = jwt_decode(token).role;
                    if(role == "Administrator"){
                        return true;
                    }
                    return false;
                }

            };
        }
    ]);



})();