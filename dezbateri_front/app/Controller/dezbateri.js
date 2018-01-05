
var index = angular.module('dezbateriModule', ['ngRoute', 'angularUtils.directives.dirPagination']);

index.run(['$anchorScroll', function ($anchorScroll) {
    $anchorScroll.yOffset = 100;   // always scroll by 50 extra pixels
}]);

index.controller('dezbateriController', ['navigateFactory', 'debateFactory', 
        function (navigateFactory, debateFactory) {
            console.log("loaded");
            this.navigateFactory = navigateFactory;
            this.userLogged = navigateFactory.getUserLogged();
         
            this.categoriesAll = [];
            _this = this;
            debateFactory.getAllCategories().then(function (response) {
                _this.categoriesAll = response.data.map(function(obj){
                    var cate = {};
                    cate["cat"] = obj.name;
                    return cate;
                });
            });

       
            this.statusAll = [ {sts: "In asteptare"}, {sts: "In desfasurare"}, {sts: "Incheiata"}];

            $("a.anchor_dezbateri_1").click(function (e) {
                e.preventDefault();
                $("div.anchor_dezbateri_1").scrollintoview({duration: "slow", viewPadding: {y: 300}});
            });
            $("a.anchor_dezbateri_2").click(function (e) {
                e.preventDefault();
                $("div.anchor_dezbateri_2").scrollintoview({duration: "slow", viewPadding: {y: 300}});
            });
            $("a.anchor_dezbateri_3").click(function (e) {
                e.preventDefault();
                $("div.anchor_dezbateri_3").scrollintoview({duration: "slow", viewPadding: {y: 300}});
            });
            $("a.anchor_dezbateri_4").click(function (e) {
                e.preventDefault();
                $("div.anchor_dezbateri_4").scrollintoview({duration: "slow", viewPadding: {y: 300}});
            });

            this.debates = null;
            this.alldebates = null;
            this.mydebates = [];
            this.pageno = 1; // initialize page no to 1
            this.total_count = 0;
            this.itemsPerPage = 5;
            this.debates = [];
            _this = this;
           
            this.getData = function (pageno) { // This would fetch the data on page change.
               
                debateFactory.getAllDebates().then(function (response) {
                  
                    _this.debates = response.data;  
                    _this.alldebates = _this.debates;
                    angular.copy(_this.alldebates,  _this.copiedDebates);
                    _this.total_count = response.total_count; // total data count.
                    for (var i = 0; i < _this.debates.length; i++) {
                        if (_this.userLogged === _this.debates[i].Pro_username) {
                            _this.mydebates.push(_this.debates[i]);
                        }
                        if (_this.userLogged === _this.debates[i].Con_username) {
                            _this.mydebates.push(_this.debates[i]);
                        }
                    }
                    _this.total_count_mydebates = _this.mydebates.totalItems;
                })
                    .catch(function (err) {
                        console.log(err)
                    });
            };

            this.categorySearched = null;
            this.setSelectedCategory = function(){
                this.categorySearched = this.categoryselect;
            };

            this.statusSearched = null;
            this.setSelectedStatus = function(){
                this.statusSearched = this.statusselect;
            };
            this.searchedusername = null;
            this.copiedDebates = [];
            this.resetCautare = false;

            this.searchForDebates = function(){
                this.showAdvancedSearch();

                var status = null;
                if(this.statusSearched === "In asteptare"){
                    status = "asteptare";
                } else if(this.statusSearched === "In desfasurare"){
                    status = "desfasurare";
                } else if(this.statusSearched === "Incheiata"){
                    status = "incheiat";
                }
                var categ = this.categorySearched;
                var user = this.searchedusername;
                var toDelete = [];
                var isfound = false;

                if(status !== null || categ !== null || user !== null){
                   for (var i = 0; i < this.alldebates.length; i++){
                       var ifstatus = false;
                       var ifcateg =  false;
                       var ifuser = false;
                       if(status === null || status === undefined){
                           ifstatus = true;
                       }
                       if(categ === null || categ === undefined){
                           ifcateg = true;
                       }
                       if(user === null || user === undefined){
                           ifuser = true;
                       }

                       if(this.alldebates[i].State === status){
                           ifstatus = true;
                       }

                       for(var c = 0 ; c < this.alldebates[i].Category.length; c++){
                           if(this.alldebates[i].Category[c] === categ){
                               ifcateg = true;
                           }
                       }

                       if(this.alldebates[i].Pro_username === user || this.alldebates[i].Con_username === user){
                           ifuser = true;
                       }

                       var del = true;
                       if((ifstatus && ifcateg) && ifuser){
                           del = false;
                       }
                       if(del === true){
                           toDelete.push(i);
                       }

                   }

                   var count = 0;
                   for(var t = 0;t < toDelete.length;t++){
                       this.alldebates.splice(toDelete[t] - count,1);
                       count++;
                       isfound = true;
                   }
                }
                if(isfound){
                    this.resetCautare = true;
                }

            };

            this.resetDebates = function(){
                this.resetCautare = false;
                this.alldebates = [];
                angular.copy(this.copiedDebates,  this.alldebates);
                var element = document.getElementById('categoryselect');
                element.value = "";
                element = document.getElementById('statusselect');
                element.value = "";
                this.searchedusername = null;
                this.categorySearched = null;
                this.statusSearched = null; 
            };

            this.isJoinable = function(id){

                for (var i = 0; i < this.copiedDebates.length; i++) {
                    if(this.copiedDebates[i].DebateId === id){
                        var user = this.copiedDebates[i].Con_username;
                        var creator = this.copiedDebates[i].Pro_username;
                        if(user === 'waiting'){

                            if(creator === this.userLogged){
                                return false;
                            } else {
                                return true;
                            }
                        } else {
                            return false;
                        }
                    }
                }
            };

            this.getData(this.pageno); // Call the function to fetch initial data on page load.

            this.currentPage = 1;
            this.numPerPage = 10;
            this.paginate = function (value) {
                var begin, end, index;
                begin = (this.currentPage - 1) * this.numPerPage;
                end = begin + this.numPerPage;
                index = this.copiedDebates.indexOf(value);
                return (begin <= index && index < end);
            };


            this.pageno_mydebates = 1; // initialize page no to 1
            this.total_count_mydebates = 0;
            this.itemsPerPage_mydebates = 5;
            this.currentPage_mydebates = 1;
            this.numPerPage_mydebates = 1;

            this.paginate = function (value) {
                var begin, end, index;
                begin = (this.currentPage_mydebates - 1) * this.numPerPage_mydebates;
                end = begin + this.numPerPage_mydebates;
                index = this.mydebates.indexOf(value);
                return (begin <= index && index < end);
            };

            this.sort = function (keyname) {
                this.sortKey = keyname;   //set the sortKey to the param passed
                this.reverse = !this.reverse; //if true make it false and vice versa
            };

            this.select = {};
            this.select.idcard = null;
            this.select.name = null;
            this.select.dateOfBirth = null;
            this.select.address = null;
            this.advancedSearch = false;

            this.searchMore = "Cautare avansata";

            this.showAdvancedSearch = function () {
                this.advancedSearch =  !this.advancedSearch;
                if(this.advancedSearch === false){
                    this.searchMore = "Cautare avansata";
                } else {
                    this.searchMore = "Inchide cautarea";
                }
            };


            this.myDebatesMessage = null;
            if (navigateFactory.isLoggedIn() === false) {
                this.myDebatesMessage = "Pentru a vedea dezbaterile in care participi, te rugam sa te autentifici prima data";
            } else {
                this.myDebatesMessage = "Nu ai participat in nicio dezbatere. Intra acum in prima ta dezbatere!";
            }

            this.wrong = "Completeaza toate spatiile!";
            this.addDebateMessage = "Pentru a adauga o dezbatere, trebuie sa te autentifici";

            this.wrongInputVar = false;
            this.wrongInput = function(){
                return this.wrongInputVar;
            };

            this.createDebate = function (valid) {
                var chosenCategories = [];
                if (valid) {
                    var cboxes = document.getElementsByName('var_id[]');
                    var len = cboxes.length;
                    for (var i = 0; i < len; i++) {
                        if (cboxes[i].checked === true) {
                            chosenCategories.push(this.categoriesAll[i].cat);
                        }
                    }

                    var d = new Date();
                    var createDate = d.getDate() + "/" +(d.getMonth()+1)+"/"+d.getFullYear();

                    var newdebate ={
                        State: "asteptare",
                        Description: this.newDebate_description,
                        Subject: this.newDebate_subject,
                        DebateId: "empty",
                        Date_created: createDate,
                        Con_username: "waiting",
                        Pro_username:  this.userLogged,
                        Category: chosenCategories,
                        Round_1: "incepe sa scrii primul discurs...",
                        Round_3: "Discursul inca nu a fost publicat",
                        Round_2: "Discursul inca nu a fost publicat",
                        Round_4: "Discursul inca nu a fost publicat",
                        Round_5: "Discursul inca nu a fost publicat",
                        Round_6: "Discursul inca nu a fost publicat"
                    };

                    

                    _this = this;
                    debateFactory.addNewDebate(newdebate).then(function(){
                        _this.copiedDebates.push(newdebate);
                        _this.mydebates.push(newdebate);
                        navigateFactory.goToDezbateri();
                        window.location.reload();
                    });
                } else {
                    this.wrongInputVar = true;
                }

            };

            $(".tiptext").mouseover(function () {
                $(this).children(".Description").show();
            }).mouseout(function () {
                $(this).children(".Description").hide();
            });


            $('#search').on('keyup', function () {
                var pattern = $(this).val();
                $('.searchable-container .items').hide();
                $('.searchable-container .items').filter(function () {

                    return $(this).text().match(new RegExp(pattern, 'i'));
                }).show();
            });

            this.showCategoryDebates = false;
            this.selectedCategory = null;
            this.debatesOfCateg = [];
            this.changethis = function(categ){
                this.debatesOfCateg = [];
                this.selectedCategory = categ;
                for(var i=0; i<this.copiedDebates.length;i++){
                    for(var c = 0 ; c < this.copiedDebates[i].Category.length; c++){
                        if(this.copiedDebates[i].Category[c] === categ){
                            this.debatesOfCateg.push(this.copiedDebates[i]);
                        }
                    }
                }

                this.showCategoryDebates = !this.showCategoryDebates;
                $("#astama").fadeOut(1000,function() {
                    $("#allDebatesInCategory").fadeIn(1000)
                });

            };

            this.changeBack = function(){
                $("#allDebatesInCategory").fadeOut(1000,function() {
                    $("#astama").fadeIn(1000)
                });
            }


        }

    ]
);


