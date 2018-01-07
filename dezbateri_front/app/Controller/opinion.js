var index = angular.module('opinionModule', ['ngRoute', 'angularTrix', 'ngSanitize']);

index.controller('opinionController', ['navigateFactory', 'opiniiFactory', '$location', '$timeout', '$filter', '$sce',
    function (navigateFactory, opiniiFactory, $location, $timeout, $filter, $sce) {
        var _this = this;
        
        _this.navigateFactory = navigateFactory;
        _this.userLogged = navigateFactory.getUserLogged();
        var urlParams = $location.search();

        _this.OpinionFull =[];
        _this.OpinionFull.Id = urlParams.opinie_id;;
        _this.OpinionFull.Subject = null;
        _this.OpinionFull.Date_created = null;
        _this.OpinionFull.User_email = null;
        _this.OpinionFull.User_username = null;
        _this.OpinionFull.Picture_url = null;
        _this.OpinionFull.Category = null;
        _this.OpinionFull.Pro_votes = null;
        _this.OpinionFull.Con_votes = null;
        _this.OpinionFull.Arguments =[];
        _this.OpinionFull.ArgumentsPro = [];
        _this.OpinionFull.ArgumentsCon = [];

        var promise = opiniiFactory.getOpinion(_this.OpinionFull.Id).then(function (response) {
            var res = response.data;
            
            _this.OpinionFull.Date_created = res.Date_created;
            _this.OpinionFull.Picture_url = res.Picture_url;
            _this.OpinionFull.Subject = res.Subject; 
            _this.OpinionFull.User_email = res.User_email;
            _this.OpinionFull.User_username = res.User_username; 
            _this.OpinionFull.Picture_url = res.Picture_url; 
            _this.OpinionFull.Pro_votes = res.Pro_votes;
            _this.OpinionFull.Con_votes = res.Con_votes;
            _this.OpinionFull.Category = res.Category;
            _this.OpinionFull.Arguments = response.data.Arguments.map(function(obj){
                var arg = {};
                arg["Content"] = obj.Content;
                arg["Date_created"] = obj.Date_created;
                arg["Side"] = obj.Side;
                arg["User"] = obj.User.Username;
                return arg;
            });
            _this.OpinionFull.Arguments.forEach(element => {
                if(element.Side == "1")
                    _this.OpinionFull.ArgumentsPro.push(element);
                else
                    _this.OpinionFull.ArgumentsCon.push(element);
            });
        });

        _this.vote = function(type){
            var side;
            if(type == "pro") side = "pro";
            else side = "con";

            var content={
                OpinionId:_this.OpinionFull.Id,
                User_email:_this.OpinionFull.User_email,
                Side: side
            }

            opiniiFactory.castVote(content).then(function (response){
                window.location.reload();
            })
        }

        _this.newArgPro = "";
        _this.newArgCon = "";
        
        _this.addArgument = function(type){
            var side;
            var content = "";
            if(type == "pro") {
                side =true;
                content = _this.newArgPro;
            }
            else{
                side = false;
                content = _this.newArgCon;
            } 

            var date = new Date();
            var month = date.getMonth() +1;
            var dateNow = date.getDate() + "."+ month + "." + date.getFullYear();

          

            var content = {
                Syde: side,
                Content: content,
                Date_Created: dateNow,
                Opinion_id: _this.OpinionFull.Id,
                User_email: _this.userLogged
            }

            opiniiFactory.addArgument(content).then(function (response){
                window.location.reload();
            })
          
        }
    }]
);
