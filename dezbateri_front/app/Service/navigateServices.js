(function () {
    var userServices = angular.module('navigateServices', []);

    userServices.factory('navigateFactory', ['$location',
        function ($location) {

            return {

                goToDezbateri: function () {
                    $location.path('/dezbateri');
                    // window.location.reload();
                },

                goToOpinii: function () {
                    $location.path('/opinii');
                    // window.location.reload();
                },

                goToAcasa: function () {
                    $location.path('/');
                    // window.location.reload();
                },

                goToLogin: function () {
                    $location.path('/login');
                    // window.location.reload();
                },

                goToDebate: function (debate_id) {
                    $location.path('/debate').search({deb_id: debate_id});
                    // window.location.reload();
                },

                isLoggedIn: function () {
                    var value = !!(localStorage.getItem('auth_token'));
                    return (value);
                },

                getUserLogged: function () {
                    var value = !!(localStorage.getItem('auth_token'));
                    if (value === true) {
                        var token = localStorage.auth_token;
                        var decoded = jwt_decode(token);
                        var userLogged = decoded.iss;
                    }
                    return (userLogged);
                },

                logout: function () {
                    localStorage.removeItem('auth_token');
                    $location.path('/');
                    window.location.reload();
                }

            }
        }
    ]);


})();