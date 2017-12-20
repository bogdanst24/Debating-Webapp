var index = angular.module('debateModule', ['ngRoute','angularTrix','ngSanitize']);

index.controller('debateController', ['navigateFactory', 'debateFactory', '$location','$timeout',
        function (navigateFactory, debateFactory, $location,$timeout) {

            this.navigateFactory = navigateFactory;
            this.userLogged = navigateFactory.getUserLogged();

            var urlParams = $location.search();
            this.debate_id = urlParams.deb_id;
            this.debateFull = [];

            this.proPlayer = null;
            this.conPlayer = null;
            this.ownDebate = false;

            this.round1 = {
                content: "",
                editable: true
            };
            this.round2 = {
                content: "",
                editable: false
            };
            this.round3 = {
                content: "",
                editable: false
            };
            this.round4 = {
                content: "",
                editable: false
            };
            this.round5 = {
                content: "",
                editable: false
            };
            this.round6 = {
                content: "",
                editable: false
            };

            trixTextSetup();

            debateFactory.getDebate(this.debate_id).then(function (response) {
                this.debateFull = response.data;
                this.category = this.debateFull.category;
                this.date_created = this.debateFull.date_created;
                this.debateMotion = this.debateFull.subject;
                this.motionDescription = this.debateFull.description;
                this.proPlayer = this.debateFull.pro_username;
                this.conPlayer = this.debateFull.con_username;
                this.round1.content = this.debateFull.round_1;
                this.round2.content = this.debateFull.round_2;
                this.round3.content = this.debateFull.round_3;
                this.round4.content = this.debateFull.round_4;
                this.round5.content = this.debateFull.round_5;
                this.round6.content = this.debateFull.round_6;

                this.status = this.debateFull.state;
                this.checkStatus();
                this.getStateOfDebate();


                this.winner = this.proPlayer;

                if(this.userLogged === this.debateFull.pro_username){
                    this.ownDebate = true;
                }

            })
                .catch(function (err) {
                    console.log(err)
                });

            this.endActive = false;
            this.challengeActive = false;
            this.goingActive = false;

            this.checkStatus = function () {
                var el = document.getElementById('status_short');
                if (this.status === "asteptare") {
                    this.statusShort = "Asteapta un competitor";
                    this.statusExplained = "Aceasta dezbatere astapta ca un utilizator sa accepte dezbaterea";
                    this.endActive = false;
                    this.challengeActive = true;
                    this.goingActive = false;
                    el.className += el.className ? ' black-status' : 'black-status';
                } else if (this.status === "incheiat") {
                    this.statusShort = "Dezbatere incheiata";
                    this.statusExplained = "Aceasta dezbatere s-a sfarsit. Utilizatorii pot vota sau comenta dezbaterea";
                    this.endActive = true;
                    this.challengeActive = false;
                    this.goingActive = false;
                    el.className += el.className ? ' red-status' : 'red-status';
                } else if (this.status === "desfasurare") {
                    this.statusShort = "Dezbatere in desfasurare";
                    this.statusExplained = "Aceasta dezbatere este in desfasurare. Urmatorul discurs trebuie publicat in";
                    this.endActive = false;
                    this.challengeActive = false;
                    this.goingActive = true;
                    el.className += el.className ? ' green-status' : 'green-status';
                }
            };
            this.winner = this.proPlayer;

            this.joinDebate = function () {
                var user = this.userLogged;
                var debate_id = this.debate_id;

                debateFactory.joinDebate(user,debate_id).then(function(){
                    navigateFactory.goToDebate(debate_id);
                })
            };

            this.debateState = [];
            this.getStateOfDebate = function(){
                debateFactory.getDebateState(this.debate_id).then(function(response){
                    this.debateState = response.data;
                    setRoundsEditable();
                    setTimeToNextSpeech();
                });
            };


            this.quitDebate = function () {

                debateFactory.deleteDebate(this.debate_id).then(function (response) {
                    swal({
                            title: "Esti sigur?",
                            text: "Nu vei putea sa recuperezi aceasta dezbatere!",
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonColor: "#DD6B55",
                            confirmButtonText: "Da, sterge!",
                            closeOnConfirm: false
                        },
                        function(){
                            swal("Dezbatere stearsa!", "Dezbaterea nu mai exista din acest moment", "success");
                            navigateFactory.goToDezbateri();
                        });
                });



            };

            this.saveSpeech = function(){
                var debateContent ={
                    debate_id: this.debate_id,
                    round_1: this.round1.content,
                    round_2: this.round2.content,
                    round_3: this.round3.content,
                    round_4: this.round4.content,
                    round_5: this.round5.content,
                    round_6: this.round6.content
                };
                debateFactory.saveSpeech(debateContent).then(function(response){
                    swal("Salvare reusita", "Puteti reveni oricand fara a va pierde continutul", "success");
                });
            };

            this.publishSpeech = function(round){
                swal({
                        title: "Esti sigur?",
                        text: "Odata publicat, discursul nu va mai putea fi modificat",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonColor: "#008F95",
                        cancelButtonText: "Renunta",
                        confirmButtonText: "Publica",
                        closeOnConfirm: false
                    },
                    function(){
                        this.saveSpeech();
                        debateFactory.goToNextRound(this.debate_id).then(function(response){
                            navigateFactory.goToDebate(this.debate_id);
                            swal("Publicat!", "Discursul tau a fost publicat, iar dezbaterea a trecut in runda urmatoare.", "success");
                        })

                    });
            };

            function setRoundsEditable(){
                var round = this.debateState.next_round;
                this.round1.editable = false;
                this.round2.editable = false;
                this.round3.editable = false;
                this.round4.editable = false;
                this.round5.editable = false;
                this.round6.editable = false;
                if(this.userLogged === this.proPlayer){
                    var iAmPro = true;
                } else {
                    iAmPro = false;
                }
                if(this.userLogged === this.conPlayer){
                    var iAmCon = true;
                } else {
                    iAmCon = false;
                }
                if(iAmPro || iAmCon){

                    if(round === "1" && iAmPro){
                        if(this.round1.content === "<div><!--block-->Discursul inca nu a fost publicat</div>"){
                            this.round1.content = "";
                        }
                        this.activeRound = 1;
                        this.round1.editable = true;
                    } else if (round === "2" && iAmCon){
                        if(this.round2.content === "<div><!--block-->Discursul inca nu a fost publicat</div>"){
                            this.round2.content = "";
                        }
                        this.round2.editable = true;
                        this.activeRound = 2;
                    } else if (round === "3" && iAmPro){
                        if(this.round3.content === "<div><!--block-->Discursul inca nu a fost publicat</div>"){
                            this.round3.content = "";
                        }
                        this.round3.editable = true;
                        this.activeRound = 3;
                    } else if (round === "4" && iAmCon ){
                        if(this.round4.content === "<div><!--block-->Discursul inca nu a fost publicat</div>"){
                            this.round4.content = "";
                        }
                        this.round4.editable = true;
                        this.activeRound = 4;
                    } else if (round === "5" && iAmPro){
                        if(this.round5.content === "<div><!--block-->Discursul inca nu a fost publicat</div>"){
                            this.round5.content = "";
                        }
                        this.round5.editable = true;
                        this.activeRound = 5;
                    } else if (round === "6" && iAmCon){
                        if(this.round6.content === "<div><!--block-->Discursul inca nu a fost publicat</div>"){
                            this.round6.content = "";
                        }
                        this.round6.editable = true;
                        this.activeRound = 6;
                    }
                }
            }
            this.timeleftMessage = "mesag";
            this.timeleftVisible = false;

            function setTimeToNextSpeech(){
                this.timeToNextSpeech = this.debateState.time_to_next;
                var timerTriggered = false;

                var x = setInterval(function() {

                    // Get todays date and time
                    var now = new Date().getTime();

                    // Find the distance between now an the count down date
                    var distance = this.timeToNextSpeech - now;

                    // Time calculations for days, hours, minutes and seconds
                    var days = Math.floor(distance / (1000 * 60 * 60 * 24));
                    var hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
                    var minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
                    var seconds = Math.floor((distance % (1000 * 60)) / 1000);
                    document.getElementById("timer").innerHTML = days + " zile   " + hours + " ore   "
                        + minutes + " minute   " + seconds + " secunde   ";



                    if(days === 0){
                        if(hours === 1) {
                            $timeout(function() {
                                this.timeleftMessage = "Mai putin de doua ore ramase!";
                                this.timeleftVisible = true;
                            });
                        } else if(hours === 0){
                            $timeout(function() {
                                this.timeleftMessage = "Mai putin de o ora ramasa!";
                                this.timeleftVisible = true;
                            });
                            if(minutes < 10 && minutes > 0){
                                $timeout(function() {
                                    this.timeleftMessage = "Ultimele minute in care poti publica!";
                                    this.timeleftVisible = true;
                                });
                            } else if(minutes === 0 && seconds === 0) {
                                if (this.activeRound === 1) {
                                    this.round1.content = "Discursul nu a fost postat. Dezbaterea continua.";
                                } else if (this.activeRound === 2) {
                                    this.round2.content = "Discursul nu a fost postat. Dezbaterea continua.";
                                } else if (this.activeRound === 3) {
                                    this.round3.content = "Discursul nu a fost postat. Dezbaterea continua.";
                                } else if (this.activeRound === 4) {
                                    this.round4.content = "Discursul nu a fost postat. Dezbaterea continua.";
                                } else if (this.activeRound === 5) {
                                    this.round5.content = "Discursul nu a fost postat. Dezbaterea continua.";
                                } else if (this.activeRound === 6) {
                                    this.round6.content = "Discursul nu a fost postat. Dezbaterea continua.";
                                }
                                var debateContent ={
                                    debate_id: this.debate_id,
                                    round_1: this.round1.content,
                                    round_2: this.round2.content,
                                    round_3: this.round3.content,
                                    round_4: this.round4.content,
                                    round_5: this.round5.content,
                                    round_6: this.round6.content
                                };

                                    debateFactory.saveSpeech(debateContent).then(function (response) {
                                          debateFactory.goToNextRound(this.debate_id).then(function (response) {
                                            navigateFactory.goToDebate(this.debate_id);

                                        })
                                            .catch(function (err) {
                                                console.log(err)
                                            });
                                    })
                                        .catch(function (err) {
                                            console.log(err)
                                        });

                            }
                        }
                    }
                    if(this.debateState.next_round % 2 === 1){
                        this.nextPlayer = this.proPlayer;
                    } else {
                        this.nextPlayer = this.conPlayer;
                    }
                    if(this.userLogged !== this.nextPlayer){
                        $timeout(function() {
                            this.timeleftVisible = false;
                        });
                    }

                }, 1000);

            }

            function trixTextSetup(){
                document.addEventListener("trix-initialize", function(event) {
                    var toolbar, toolbar_id;
                    toolbar_id = event.target.getAttribute('toolbar');
                    toolbar = document.getElementById(toolbar_id);
                    toolbar.style.display = 'none';
                });

                document.addEventListener("trix-focus", function(event) {
                    event.target.toolbarElement.style.display = "block";
                });

                this.trixBlur = function(e, editor) {
                    if(editor.selectionManager.lockCount !== 1){
                        event.target.toolbarElement.style.display = "none";
                    }
                }

            }


        }

    ]
);


