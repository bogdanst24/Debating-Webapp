(function () {
    var userServices = angular.module('debateServices', []);

    userServices.factory('debateFactory', ['$http', 'API_URL',
        function ($http, API_URL) {

            return {
                getAllDebates: function () {
                    return $http.get(API_URL + '/debate/getGeneral');
                },

                getAllCategories: function(){
                    return $http.get(API_URL + '/category/GetAllCategories')
                },

                getDebate: function(debate_id){
                    return $http.get(API_URL + '/debate/getDebate/' + debate_id);
                },
                addNewDebate: function (debate) {
                    return $http.post(API_URL + '/debate/addDebate', JSON.stringify(debate));
                },
                deleteDebate: function(debate_id){
                    return $http.delete(API_URL + '/debate/deleteDebate/' + debate_id);
                },
                joinDebate: function(user,debate_id){
                    return $http.post(API_URL + '/debate/joinDebate/' + debate_id + "/"+user)
                }
            };
        }
    ]);



})();