(function () {
    var userServices = angular.module('debateServices', []);

    userServices.factory('debateFactory', ['$http', 'API_URL',
        function ($http, API_URL) {

            return {
                getAllDebates: function () {
                    return $http.get(API_URL + '/debate/GetAllDebates');
                },

                getAllCategories: function(){
                    return $http.get(API_URL + '/category/GetAllCategories')
                },

                getDebate: function(debate_id){
                    return $http.get(API_URL + '/debate/GetDebate/' + debate_id);
                },
                addNewDebate: function (debate) {
                    return $http.post(API_URL + '/debate/addDebate', JSON.stringify(debate));
                },
                deleteDebate: function(content){
                    return $http.post(API_URL + '/debate/deleteDebate', JSON.stringify(content));
                },
                joinDebate: function(content){
                    return $http.post(API_URL + '/debate/JoinDebate', JSON.stringify(content));
                },
                saveDebate: function(debate){
                    return $http.post(API_URL + '/debate/UpdateDebateContent', JSON.stringify(debate));
                },
                goToNextRound: function(debate){
                    return $http.post(API_URL + '/debate/GoToNextRound' , JSON.stringify(debate));
                },
                voteDebate: function(content){
                    return $http.post(API_URL + '/debate/Vote' , JSON.stringify(content));
                },
                getVotes: function(debate_id){
                    return $http.get(API_URL + '/debate/GetDebateVotes/' + debate_id);
                },
                getAllComments: function(debate_id){
                    return $http.get(API_URL + '/debate/GetAllComments/' + debate_id);
                },
                addComment: function(content){
                    return $http.post(API_URL + '/debate/AddComment' , JSON.stringify(content));
                },
            };
        }
    ]);



})();