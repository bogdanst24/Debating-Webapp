(function () {
    var userServices = angular.module('opiniiServices', []);

    userServices.factory('opiniiFactory', ['$http', 'API_URL',
        function ($http, API_URL) {

            return {
                getAllOpinions: function () {
                    return $http.get(API_URL + '/opinion/GetAllOpinions');
                },

                getAllCategories: function(){
                    return $http.get(API_URL + '/category/GetAllCategories')
                },

                getOpinion: function(id){
                    return $http.get(API_URL + '/opinion/GetOpinion/' + id);
                },

                addNewOpinion: function (opinion) {
                    return $http.post(API_URL + '/opinion/AddOpinion', JSON.stringify(opinion));
                },

                castVote: function(content){
                    return $http.post(API_URL + '/opinion/CastVote' , JSON.stringify(content));
                },

                addArgument: function(content){
                    return $http.post(API_URL + '/opinion/AddArgument', JSON.stringify(content));
                },


                saveDebate: function(debate){
                    return $http.post(API_URL + '/debate/UpdateDebateContent', JSON.stringify(debate));
                },
                goToNextRound: function(debate){
                    return $http.post(API_URL + '/debate/GoToNextRound' , JSON.stringify(debate));
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

                sendEmail: function(content){
                    return $http.post(API_URL + '/debate/SendEmail', JSON.stringify(content));
                }
            };
        }
    ]);



})();