(function () {
    var userServices = angular.module('userServices', []);

    userServices.factory('userFactory', ['$http', 'API_URL',
        function ($http, API_URL) {

            return {
                getAllAccounts: function () {
                    return $http.get(API_URL + '/accounts/get');
                },

                addNewUser: function (user) {
                    return $http.post(API_URL + '/login/register', JSON.stringify(user));
                },

                updateUser: function (user) {
                    return $http.put(API_URL + '/accounts/', JSON.stringify(user));
                },

                getUserByUsername: function (username) {
                    return $http.get(API_URL + '/users/' + username);
                },

                deleteUserByUsername: function (username) {
                    return $http.delete(API_URL + '/accounts/' + username);
                },

            };
        }
    ]);



})();