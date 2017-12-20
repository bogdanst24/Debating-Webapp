
var index = angular.module('dezbateriModule', ['ngRoute', 'angularUtils.directives.dirPagination']);

index.run(['$anchorScroll', function ($anchorScroll) {
    $anchorScroll.yOffset = 100;   // always scroll by 50 extra pixels
}]);

index.controller('dezbateriController', ['navigateFactory', 'debateFactory', 
        function (navigateFactory, debateFactory) {

            this.navigateFactory = navigateFactory;
            this.userLogged = navigateFactory.getUserLogged();

            this.categoriesAll = [];
            debateFactory.getAllCategories().then(function (response) {
                console.log(response.data);
                this.categoriesAll = response.data.map(categ => categ.name);
            });

                // [ {cat: "Economie"}, {cat: "Mediu"}, {cat: "Politica internationala"},
                // {cat: "Drepturi si libertati"}, {cat: "Moralitate"}, {cat: "Religie"},
                // {cat: "Politica"}, {cat: "Sport"}, {cat: "Stiinta si tehnologie"},
                // {cat: "Cultura si arta"}, {cat: "Istorie"}, {cat: "Societate"}, {cat: "Conflicte"}];

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
            this.getData = function (pageno) { // This would fetch the data on page change.
                //In practice this should be in a factory.
                this.debates = [];
                debateFactory.getAllDebates().then(function (response) {
                    //ajax request to fetch data into vm.data
                    this.debates = response.data;  // data to be displayed on current page.
                    this.alldebates = this.debates;
                    angular.copy(this.alldebates,  this.copiedDebates);
                    this.total_count = response.total_count; // total data count.
                    for (var i = 0; i < this.debates.length; i++) {
                        if (this.userLogged === this.debates[i].debateRound.pro_username) {
                            this.mydebates.push(this.debates[i]);
                        }
                        if (this.userLogged === this.debates[i].debateRound.con_username) {
                            this.mydebates.push(this.debates[i]);
                        }
                    }
                    this.total_count_mydebates = this.mydebates.totalItems;
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
                       if(status === null){
                           ifstatus = true;
                       }
                       if(categ === null){
                           ifcateg = true;
                       }
                       if(user === null){
                           ifuser = true;
                       }

                       if(this.alldebates[i].debateDescription.state === status){
                           ifstatus = true;
                       }

                       for(var c = 0 ; c < this.alldebates[i].debateDescription.category.length; c++){
                           if(this.alldebates[i].debateDescription.category[c] === categ){
                               ifcateg = true;
                           }
                       }

                       if(this.alldebates[i].debateRound.pro_username === user || this.alldebates[i].debateRound.con_username === user){
                           ifuser = true;
                       }

                       var del = true;
                       if(ifstatus && ifcateg && ifuser){
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
                this.searchedusername = "";

            };

            this.isJoinable = function(id){

                for (var i = 0; i < this.copiedDebates.length; i++) {
                    if(this.copiedDebates[i].debateDescription.debate_id === id){
                        var user = this.copiedDebates[i].debateRound.con_username;
                        var creator = this.copiedDebates[i].debateRound.pro_username;
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
            this.select.debateDescription = null;
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
                        state: "asteptare",
                        description: this.newDebate_description,
                        subject: this.newDebate_subject,
                        debate_id: "empty",
                        date_created: createDate,
                        con_username: "waiting",
                        pro_username:  this.userLogged,
                        category: chosenCategories,
                        round_1: "incepe sa scrii primul discurs...",
                        round_3: "Discursul inca nu a fost publicat",
                        round_2: "Discursul inca nu a fost publicat",
                        round_4: "Discursul inca nu a fost publicat",
                        round_5: "Discursul inca nu a fost publicat",
                        round_6: "Discursul inca nu a fost publicat"
                    };

                    debateFactory.addNewDebate(newdebate).then(function(){
                        this.copiedDebates.push(newdebate);
                        this.mydebates.push(newdebate);
                        navigateFactory.goToDezbateri();
                    });
                } else {
                    this.wrongInputVar = true;
                }

            };

            $(".tiptext").mouseover(function () {
                $(this).children(".description").show();
            }).mouseout(function () {
                $(this).children(".description").hide();
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
                    for(var c = 0 ; c < this.copiedDebates[i].debateDescription.category.length; c++){
                        if(this.copiedDebates[i].debateDescription.category[c] === categ){
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


