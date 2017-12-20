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

                decode: function(){
                    var token;
                    token = localStorage.auth_token;
                    var decoded = jwt_decode(token);
                    return decoded.sub;
                }

            };
        }
    ]);



})();