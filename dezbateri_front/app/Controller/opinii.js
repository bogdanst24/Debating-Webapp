var index = angular.module('opiniiModule', ['ngRoute', 'angularTrix', 'ngSanitize'])

index.controller('opiniiController', ['navigateFactory', 'debateFactory', 'opiniiFactory', '$location', '$timeout', '$filter', '$sce',
    function (navigateFactory, debateFactory, opiniiFactory, $location, $timeout, $filter, $sce) {

        var _this = this;

        _this.navigateFactory = navigateFactory;
        _this.userLogged = navigateFactory.getUserLogged();
        var date = new Date();
        var month = date.getMonth() +1;
        _this.dateNow = date.getDate() + "."+ month + "." + date.getFullYear();
       

        _this.allOpinions = [];
        _this.myOpinions = [];
        _this.copiedOpinions =[];
        _this.allOpinions.Id = null;
        _this.allOpinions.Subject = null;
        _this.allOpinions.Date_created = null;
        _this.allOpinions.User_email = null;
        _this.allOpinions.User_username = null;
        _this.allOpinions.Picture_url = null;

        _this.pageno = 1; // initialize page no to 1
        _this.total_count = 0;
        _this.itemsPerPage = 9;
        _this.getData = function (pageno) { // This would fetch the data on page change.
            opiniiFactory.getAllOpinions().then(function (response) {
                _this.allOpinions = response.data;
                _this.allOpinions.sort(function (a, b) {
                    return (a["Date_created"] < b["Date_created"] ? 1 : -1);
                });

                _this.opinions = _this.allOpinions;

                angular.copy(_this.allOpinions, _this.copiedOpinions);
                _this.total_count = response.total_count; // total data count.

                for (var i = 0; i < _this.opinions.length; i++) {
                    if (_this.userLogged === _this.opinions[i].User_username) {
                        _this.myOpinions.push(_this.opinions[i]);
                    }
                }
                _this.total_count_myOpinions = _this.myOpinions.totalItems;
            });
        };

        _this.categoriesAll = [];
        debateFactory.getAllCategories().then(function (response) {
            _this.categoriesAll = response.data.map(function(obj){
                var cate = {};
                cate["cat"] = obj.name;
                return cate;
            });
        });

        _this.getData(_this.pageno); // Call the function to fetch initial data on page load.

        _this.currentPage = 1;
        _this.numPerPage = 10;
        _this.paginate = function (value) {
            var begin, end, index;
            begin = (_this.currentPage - 1) * _this.numPerPage;
            end = begin + _this.numPerPage;
            index = _this.copiedOpinions.indexOf(value);
            return (begin <= index && index < end);
        };

        _this.pageno_myOpinions = 1; // initialize page to 1
        _this.total_count_myOpinions = 0;
        _this.itemsPerPage_myOpinions = 6;
        _this.currentPage_myOpinions = 1;
        _this.numPerPage_myOpinions = 1;

        _this.paginate = function (value) {
            var begin, end, index;
            begin = (_this.currentPage_myOpinions - 1) * _this.numPerPage_myOpinions;
            end = begin + _this.numPerPage_myOpinions;
            index = _this.myOpinions.indexOf(value);
            return (begin <= index && index < end);
        };

        _this.sort = function (keyname) {
            console.log(keyname);
            _this.sortKey = keyname;   //set the sortKey to the param passed
            _this.reverse = !_this.reverse; //if true make it false and vice versa
        };

        _this.searchMore = "Cautare avansata";
        _this.advancedSearch = false;
        _this.resetCautare = false;

        _this.showAdvancedSearch = function () {
            _this.advancedSearch = !_this.advancedSearch;
            if (_this.advancedSearch === false) {
                _this.searchMore = "Cautare avansata";
            } else {
                _this.searchMore = "Inchide cautarea";
            }
        };

        _this.categorySearched = null;
        _this.setSelectedCategory = function(){
            _this.categorySearched = _this.categoryselect;
        };

        _this.searchForOpinii = function(){
            _this.showAdvancedSearch();

            var categ = this.categorySearched;
            var user = this.searchedusername;
            var toDelete = [];
            var isfound = false;

            if( categ !== null || user !== null){
               for (var i = 0; i < this.allOpinions.length; i++){
                   var ifcateg =  false;
                   var ifuser = false;
                 
                   if(categ === null || categ === undefined){
                       ifcateg = true;
                   }
                   if(user === null || user === undefined){
                       ifuser = true;
                   }
                  
                   for(var c = 0 ; c < this.allOpinions[i].Category.length; c++){
                       if(this.allOpinions[i].Category[c] === categ){
                           ifcateg = true;
                       }
                   }

                   if(this.allOpinions[i].User_username === user){
                       ifuser = true;
                   }

                   var del = true;
                   if( ifcateg && ifuser){
                       del = false;
                   }
                   if(del === true){
                       toDelete.push(i);
                   }

               }

               var count = 0;
               for(var t = 0;t < toDelete.length;t++){
                   this.allOpinions.splice(toDelete[t] - count,1);
                   count++;
                   isfound = true;
               }
            }
            if(isfound){
                this.resetCautare = true;
            }

        };

        _this.resetOpinionSearch = function(){
            _this.resetCautare = false;
            _this.allOpinions = [];
            angular.copy(_this.copiedOpinions,  _this.allOpinions);
            var element = document.getElementById('categoryselect');
            element.value = "";
            _this.searchedusername = null;
            _this.categorySearched = null;
        };

        _this.myOpinionMessage = null;
        if (navigateFactory.isLoggedIn() === false) {
            _this.myOpinionMessage = "Pentru a vedea opiniile create, te rugam sa te autentifici prima data";
        } else {
            _this.myOpinionMessage = "Nu ai adaugat nicio opinie. Intra acum in prima ta dezbatere!";
        }


        _this.wrong = "Completeaza toate spatiile!";
        _this.addOpinionMessage = "Pentru a adauga o opinie, trebuie sa te autentifici";
        _this.wrongInputVar = false;
        _this.wrongInput = function(){
            return _this.wrongInputVar;
        };

        _this.createOpinion = function (valid) {
            var chosenCategories = [];
            if (valid) {
                var cboxes = document.getElementsByName('var_id[]');
                var len = cboxes.length;
                for (var i = 0; i < len; i++) {
                    if (cboxes[i].checked === true) {
                        chosenCategories.push(_this.categoriesAll[i].cat);
                    }
                }

                var d = new Date();
                var createDate = d.getDate() + "." +(d.getMonth()+1)+"."+d.getFullYear();

                var newOpinion ={
                    Subject: _this.newOpinion_subject,
                    Date_created: createDate,
                    User_email: _this.userLogged,
                    Picture_url:  "http://dezbateri.ro/pic/opinion.jpg",
                    Category: chosenCategories
                };

                opiniiFactory.addNewOpinion(newOpinion).then(function(){
                    _this.copiedOpinions.push(newOpinion);
                    _this.myOpinions.push(newOpinion);
                    navigateFactory.getAllOpinions();
                    window.location.reload();
                });
            } else {
                _this.wrongInputVar = true;
            }

        };

        _this.showCategoryOpinions = false;
        _this.selectedCategory = null;
        _this.opinionsOfCateg = [];
        _this.changethis = function(categ){
            _this.opinionsOfCateg = [];
            _this.selectedCategory = categ;
           

            for(var i=0; i<_this.copiedOpinions.length;i++){
                var thisOpinion = _this.copiedOpinions[i];
                
                for(var c = 0 ; c < thisOpinion.Category.length; c++){
                 
                    if(thisOpinion.Category[c] === categ){
                        _this.opinionsOfCateg.push(thisOpinion);
                    }
                }
            }

         
            _this.showCategoryOpinions = !_this.showCategoryOpinions;
            $("#astama").fadeOut(1000,function() {
                $("#allOpinionsInCategory").fadeIn(1000)
            });

        };

        _this.changeBack = function(){
            $("#allOpinionsInCategory").fadeOut(1000,function() {
                $("#astama").fadeIn(1000)
            });
        }



        /* -----------------------------------------------------------------------------------------
        --------------------------------------------------------------------------------------------
        --------------------------------------------------------------------------------------------
        ----------------------------------------------------------------------------------------- */

        $(".tiptext").mouseover(function () {
            $(_this).children(".Description").show();
        }).mouseout(function () {
            $(_this).children(".Description").hide();
        });


        $('#search').on('keyup', function () {
            var pattern = $(_this).val();
            $('.searchable-container .items').hide();
            $('.searchable-container .items').filter(function () {

                return $(_this).text().match(new RegExp(pattern, 'i'));
            }).show();
        });

        

        $("a.anchor_dezbateri_1").click(function (e) {
            e.preventDefault();
            $("div.anchor_dezbateri_1").scrollintoview({ duration: "slow", viewPadding: { y: 300 } });
        });
        $("a.anchor_dezbateri_2").click(function (e) {
            e.preventDefault();
            $("div.anchor_dezbateri_2").scrollintoview({ duration: "slow", viewPadding: { y: 300 } });
        });
        $("a.anchor_dezbateri_3").click(function (e) {
            e.preventDefault();
            $("div.anchor_dezbateri_3").scrollintoview({ duration: "slow", viewPadding: { y: 300 } });
        });
        $("a.anchor_dezbateri_4").click(function (e) {
            e.preventDefault();
            $("div.anchor_dezbateri_4").scrollintoview({ duration: "slow", viewPadding: { y: 300 } });
        });

    }
]);


