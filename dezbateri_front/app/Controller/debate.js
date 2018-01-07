var index = angular.module('debateModule', ['ngRoute', 'angularTrix', 'ngSanitize']);

index.controller('debateController', ['navigateFactory', 'debateFactory', '$location', '$timeout', '$filter', '$sce',
    function (navigateFactory, debateFactory, $location, $timeout, $filter, $sce) {

        if (sessionStorage.getItem("speechpublished")) {
            swal("Discursul a fost publicat cu succes!", {
                buttons: {
                    catch: {
                        text: "Inchide",
                        value: "catch",
                    }
                },
                closeOnClickOutside: false
            })
                .then((value) => {
                    switch (value) {
                        case "catch":
                            sessionStorage.removeItem("speechpublished");
                            break;
                        default:
                            sessionStorage.removeItem("speechpublished");
                            break;
                    }
                });
        }

        this.navigateFactory = navigateFactory;
        this.userLogged = navigateFactory.getUserLogged();
        this.timeleftMessage = "mesag";
        this.timeleftVisible = false;
        this.endActive = false;
        this.challengeActive = false;
        this.goingActive = false;
        this.clockInterval = null;
        this.proVotes = 0;
        this.conVotes = 0;
        this.myVote = [];
        this.myVote.voted = false;
        this.myVote.pro = false;
        this.myVote.con = false;
        this.allComments = [];

        this.newComment = "";

        var urlParams = $location.search();

        this.debateFull = [];

        this.debateFull.debate_id = urlParams.deb_id;
        this.debateFull.proPlayer = null;
        this.debateFull.conPlayer = null;
        this.debateFull.ownDebate = false;
        this.debateFull.next_round = null;

        this.inDebate = false;

        this.debateFull.round1 = {
            content: "",
            editable: true,
            minimized: false
        };
        this.debateFull.round2 = {
            content: "",
            editable: false,
            minimized: false
        };
        this.debateFull.round3 = {
            content: "",
            editable: false,
            minimized: false
        };
        this.debateFull.round4 = {
            content: "",
            editable: false,
            minimized: false
        };
        this.debateFull.round5 = {
            content: "",
            editable: false,
            minimized: false
        };
        this.debateFull.round6 = {
            content: "",
            editable: false,
            minimized: false
        };

        this.winner = "egalitate";


        trixTextSetup();

        var _this = this;
        var promise = debateFactory.getDebate(this.debateFull.debate_id).then(function (response) {
            var debateResponse = response.data;

            _this.debateFull.category = debateResponse.Category;
            _this.debateFull.date_created = debateResponse.Date_created;
            _this.debateFull.debateMotion = debateResponse.Subject;
            _this.debateFull.motionDescription = debateResponse.Description;
            _this.debateFull.proPlayer = debateResponse.Pro_username;
            _this.debateFull.conPlayer = debateResponse.Con_username;
            _this.debateFull.round1.content = debateResponse.Round_1;
            _this.debateFull.round2.content = debateResponse.Round_2;
            _this.debateFull.round3.content = debateResponse.Round_3;
            _this.debateFull.round4.content = debateResponse.Round_4;
            _this.debateFull.round5.content = debateResponse.Round_5;
            _this.debateFull.round6.content = debateResponse.Round_6;

            _this.debateFull.status = debateResponse.State;
            _this.checkStatus();
            _this.setRoundsEditable(debateResponse.Next_round);
            _this.setTimeToNextSpeech(debateResponse.Time_to_next);

            if (_this.userLogged === _this.debateFull.proPlayer) 
                _this.debateFull.ownDebate = true;
            
            if(_this.userLogged === _this.debateFull.conPlayer || _this.userLogged === _this.debateFull.proPlayer)
                _this.inDebate = true;


        })
            .catch(function (err) {
                console.log("ERRROR")
                console.log(err)
            });

        promise.then( function(payload){
            debateFactory.getVotes(_this.debateFull.debate_id).then(function(response){
                var allVotes = response.data;
                allVotes.forEach(vote => {
                    if(vote.vote_pro) {
                        _this.proVotes++;
                    } else {
                        _this.conVotes++;
                    }
    
                    if(vote.user_username == _this.userLogged)
                        _this.myVote.voted = true;
                        if(vote.vote_pro) {
                            _this.myVote.pro = true;
                            _this.myVote.con = false;
                        } else {
                            _this.myVote.pro = false;
                            _this.myVote.con = true;
                        }
                });
    
                if(_this.proVotes > _this.conVotes)
                    _this.winner = _this.debateFull.proPlayer;
                else if (_this.proVotes < _this.conVotes)
                    _this.winner = _this.debateFull.conPlayer;
                else
                    _this.winner = "egalitate";
            });
            if(_this.endActive)
                debateFactory.getAllComments(_this.debateFull.debate_id).then(function(response){
                    _this.allComments = response.data;
                    
                    _this.allComments.forEach(element => {
                        element.Comment = _this.replaceBackslash(element.Comment);
                    });

                })
        });



        this.checkStatus = function () {
            var el = document.getElementById('status_short');
            switch (this.debateFull.status) {
                case ("asteptare"):
                    this.statusShort = "Asteapta un competitor";
                    this.statusExplained = "Aceasta dezbatere astapta ca un utilizator sa accepte dezbaterea";
                    this.endActive = false;
                    this.challengeActive = true;
                    this.goingActive = false;
                    el.className += el.className ? ' black-status' : 'black-status';
                    break;
                case ("incheiat"):
                    this.statusShort = "Dezbatere incheiata";
                    this.statusExplained = "Aceasta dezbatere s-a sfarsit. Utilizatorii pot vota sau comenta dezbaterea";
                    this.endActive = true;
                    this.challengeActive = false;
                    this.goingActive = false;
                    el.className += el.className ? ' red-status' : 'red-status';
                    break;
                case ("desfasurare"):
                    this.statusShort = "Dezbatere in desfasurare";
                    this.statusExplained = "Aceasta dezbatere este in desfasurare. Urmatorul discurs trebuie publicat in";
                    this.endActive = false;
                    this.challengeActive = false;
                    this.goingActive = true;
                    el.className += el.className ? ' green-status' : 'green-status';
                    break;
                default: break;
            }

        };

        this.setRoundsEditable = function (debateRound) {
            this.debateFull.next_round = debateRound;
            this.debateFull.round1.editable = false;
            this.debateFull.round2.editable = false;
            this.debateFull.round3.editable = false;
            this.debateFull.round4.editable = false;
            this.debateFull.round5.editable = false;
            this.debateFull.round6.editable = false;

            if (this.userLogged === this.debateFull.proPlayer) {
                var iAmPro = true;
            } else {
                iAmPro = false;
            }
            if (this.userLogged === this.debateFull.conPlayer) {
                var iAmCon = true;
            } else {
                iAmCon = false;
            }

            if ((iAmPro || iAmCon) && !this.endActive) {

                if (this.debateFull.next_round === "1" && iAmPro) {
                    if (this.debateFull.round1.content === "<div><!--block-->Discursul inca nu a fost publicat</div>") {
                        this.debateFull.round1.content = "";
                    }
                    this.debateFull.next_round = 1;
                    this.debateFull.round1.editable = true;
                } else if (this.debateFull.next_round === "2" && iAmCon) {
                    if (this.debateFull.round2.content === "<div><!--block-->Discursul inca nu a fost publicat</div>") {
                        this.debateFull.round2.content = "";
                    }
                    this.debateFull.round2.editable = true;
                    this.debateFull.next_round = 2;
                } else if (this.debateFull.next_round === "3" && iAmPro) {
                    if (this.debateFull.round3.content === "<div><!--block-->Discursul inca nu a fost publicat</div>") {
                        this.debateFull.round3.content = "";
                    }
                    this.debateFull.round3.editable = true;
                    this.debateFull.next_round = 3;
                } else if (this.debateFull.next_round === "4" && iAmCon) {
                    if (this.debateFull.round4.content === "<div><!--block-->Discursul inca nu a fost publicat</div>") {
                        this.debateFull.round4.content = "";
                    }
                    this.debateFull.round4.editable = true;
                    this.debateFull.next_round = 4;
                } else if (this.debateFull.next_round === "5" && iAmPro) {
                    if (this.debateFull.round5.content === "<div><!--block-->Discursul inca nu a fost publicat</div>") {
                        this.debateFull.round5.content = "";
                    }
                    this.debateFull.round5.editable = true;
                    this.debateFull.next_round = 5;
                } else if (this.debateFull.next_round === "6" && iAmCon) {
                    if (this.debateFull.round6.content === "<div><!--block-->Discursul inca nu a fost publicat</div>") {
                        this.debateFull.round6.content = "";
                    }
                    this.debateFull.round6.editable = true;
                    this.debateFull.next_round = 6;
                }
            }
        }

        this.setTimeToNextSpeech = function (Time_to_next) {

            _this.debateFull.timeToNextSpeech = Time_to_next;
            var timerTriggered = false;
            if (_this.goingActive) {
                _this.clockInterval = setInterval(function () {

                    // Get todays date and time
                    var now = new Date().getTime();

                    // Find the distance between now an the count down date
                    var distance = Time_to_next - now;

                    // Time calculations for days, hours, minutes and seconds
                    var days = Math.floor(distance / (1000 * 60 * 60 * 24));
                    var hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
                    var minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
                    var seconds = Math.floor((distance % (1000 * 60)) / 1000);
                    document.getElementById("timer").innerHTML = days + " zile   " + hours + " ore   "
                        + minutes + " minute   " + seconds + " secunde   ";

                    // if(days === 1 && hours === 0 && minutes === 0 && seconds === 0){
                    //     var content = {
                    //         type: "reminder",
                    //         email: _this.userLogged
                    //     }
                    //     debateFactory.sendEmail(content);
                    // }

                    if (days === 0 || days < 0) {
                        if (hours === 1) {
                            $timeout(function () {
                                _this.timeleftMessage = "Mai putin de doua ore ramase!";
                                _this.timeleftVisible = true;
                            });
                        } else if (hours === 0 || hours < 0) {
                            $timeout(function () {
                                _this.timeleftMessage = "Mai putin de o ora ramasa!";
                                _this.timeleftVisible = true;
                            });
                            if (minutes < 10 && minutes > 0) {
                                $timeout(function () {
                                    _this.timeleftMessage = "Ultimele minute in care poti publica!";
                                    _this.timeleftVisible = true;
                                });
                            } else if ((minutes === 0 && seconds === 0) || (minutes < 0 && seconds < 0)) {
                                if (_this.debateFull.next_round === 1) {
                                    _this.debateFull.round1.content = "Discursul nu a fost postat. Dezbaterea continua.";
                                } else if (_this.debateFull.next_round === 2) {
                                    _this.debateFull.round2.content = "Discursul nu a fost postat. Dezbaterea continua.";
                                } else if (_this.debateFull.next_round === 3) {
                                    _this.debateFull.round3.content = "Discursul nu a fost postat. Dezbaterea continua.";
                                } else if (_this.debateFull.next_round === 4) {
                                    _this.debateFull.round4.content = "Discursul nu a fost postat. Dezbaterea continua.";
                                } else if (_this.debateFull.next_round === 5) {
                                    _this.debateFull.round5.content = "Discursul nu a fost postat. Dezbaterea continua.";
                                } else if (_this.debateFull.next_round === 6) {
                                    _this.debateFull.round6.content = "Discursul nu a fost postat. Dezbaterea s-a incheiat.";
                                }
                                var debateContent = {
                                    debate_id: _this.debateFull.debate_id,
                                    round_1: _this.debateFull.round1.content,
                                    round_2: _this.debateFull.round2.content,
                                    round_3: _this.debateFull.round3.content,
                                    round_4: _this.debateFull.round4.content,
                                    round_5: _this.debateFull.round5.content,
                                    round_6: _this.debateFull.round6.content
                                };
                                var nextRound = {
                                    debate_id: _this.debateFull.debate_id
                                }
                                debateFactory.saveDebate(debateContent).then(function (response) {
                                })
                                    .catch(function (err) {
                                        console.log(err)
                                    });



                                debateFactory.goToNextRound(nextRound).then(function (response) {
                                    clearInterval(_this.clockInterval);
                                    navigateFactory.goToDebate(_this.debateFull.debate_id);
                                })
                                    .catch(function (err) {
                                        console.log(err)
                                    });

                            }
                        }
                    }

                    if (this.round % 2 === 1) {
                        this.nextPlayer = this.proPlayer;
                    } else {
                        this.nextPlayer = this.conPlayer;
                    }
                    if (this.userLogged !== this.nextPlayer) {
                        $timeout(function () {
                            this.timeleftVisible = false;
                        });
                    }

                }, 1000);
            }
        }

        this.joinDebate = function () {
            var content = {
                debate_id: this.debateFull.debate_id,
                user_id: this.userLogged
            }
            var _this = this;
            debateFactory.joinDebate(content).then(function () {
                clearInterval(_this.clockInterval);
                navigateFactory.goToDebate(_this.debateFull.debate_id);
                window.location.reload();
            })
        };

        this.quitDebate = function () {
            var content = {
                debate_id: this.debateFull.debate_id
            }
            swal("Esti sigur? Acesta dezbatere va fi stearsa definitiv!", {
                buttons: {
                    renunta: {
                        text: "Inapoi",
                        value: "cancel",
                    },
                    catch: {
                        text: "Renunta la dezbatere",
                        value: "catch",
                    }
                },
                closeOnClickOutside: false
            })
                .then((value) => {
                    switch (value) {
                        case "cancel":
                            break;
                        case "catch":
                            debateFactory.deleteDebate(content).then(function (response) {
                                clearInterval(this.clockInterval);
                                navigateFactory.goToDezbateri();
                            })
                            break;
                        default:
                            break;
                    }
                });
        };

        this.saveSpeech = function () {
            var debateContent = {
                debate_id: this.debateFull.debate_id,
                round_1: this.debateFull.round1.content,
                round_2: this.debateFull.round2.content,
                round_3: this.debateFull.round3.content,
                round_4: this.debateFull.round4.content,
                round_5: this.debateFull.round5.content,
                round_6: this.debateFull.round6.content
            };
            debateFactory.saveDebate(debateContent).then(function (response) {
                swal("Salvare reusita", "Puteti reveni oricand fara a va pierde continutul", "success");
            });
        };

        this.publishSpeech = function () {
            var _this = this;
            swal("Odata publicat, discursul nu va mai putea fi modificat", {
                buttons: {
                    renunta: {
                        text: "Renunta",
                        value: "cancel",
                    },
                    catch: {
                        text: "Publica",
                        value: "catch",
                    }
                },
                closeOnClickOutside: false
            })
                .then((value) => {
                    switch (value) {
                        case "cancel":
                            break;
                        case "catch":
                            _this.saveSpeech();
                            var nextRound = {
                                debate_id: _this.debateFull.debate_id
                            }
                            debateFactory.goToNextRound(nextRound).then(function (response) {
                                clearInterval(_this.debateFull.clockInterval);
                                navigateFactory.goToDebate(_this.debateFull.debate_id);
                                sessionStorage.setItem("speechpublished", "yes");
                                window.location.reload();
                            })
                            break;
                        default:
                            break;
                    }
                });

        };

        function trixTextSetup() {
            document.addEventListener("trix-initialize", function (event) {
                var toolbar, toolbar_id;
                toolbar_id = event.target.getAttribute('toolbar');
                toolbar = document.getElementById(toolbar_id);
                toolbar.style.display = 'none';
            });

            document.addEventListener("trix-focus", function (event) {
                event.target.toolbarElement.style.display = "block";
            });

            this.trixBlur = function (e, editor) {
                if (editor.selectionManager.lockCount !== 1) {
                    event.target.toolbarElement.style.display = "none";
                }
            }

        }

        this.navigate = function (type) {
            clearInterval(this.clockInterval);
            switch (type) {
                case ("home"):
                    navigateFactory.goToAcasa();
                    break;
                case ("dezbateri"):
                    navigateFactory.goToDezbateri();
                    break;
                case ("opinii"):
                    navigateFactory.goToOpinii();
                    break;
                case ("login"):
                    navigateFactory.goToLogin();
                    break;
                case ("logout"):
                    navigateFactory.logout();
                    break;
            }
        }

        this.vote = function(side){
            var userToVote;
            var debateToVote = this.debateFull.debate_id;
            var userVoting = this.userLogged;

            switch (side){
                case 'pro':
                userToVote = "pro";
                break;
                case 'con':
                userToVote = "con";
                break;
            }

            var content = {
                debate_id: debateToVote,
                userVoting: userVoting,
                voteCasted : userToVote
            };
            
            debateFactory.voteDebate(content).then(function (response) {
                window.location.reload();
            });
        }

        this.addComment = function(){
            var comm = _this.replaceBackslash(_this.newComment);
            var today = new Date();
            var formattedDate = $filter('date')(today, 'yyyy.MM.dd hh:mm:ss');
            var content = {
                Debate_id: _this.debateFull.debate_id,
                Comment: comm,
                Date_created: formattedDate,
                User_username: _this.userLogged
            }
            debateFactory.addComment(content).then(function(response){
                _this.allComments.push(response.data);
                _this.newComment = "";
            });
        }

        this.minimizeSpeech = function (number, ifmin) {
            switch (number) {
                case (1):
                    if (ifmin)
                        this.debateFull.round1.minimized = true;
                    else
                        this.debateFull.round1.minimized = false;
                    break;
                case (2):
                    if (ifmin)
                        this.debateFull.round2.minimized = true;
                    else
                        this.debateFull.round2.minimized = false;
                    break;
                case (3):
                    if (ifmin)
                        this.debateFull.round3.minimized = true;
                    else
                        this.debateFull.round3.minimized = false;
                    break;
                    break;
                case (4):
                    if (ifmin)
                        this.debateFull.round4.minimized = true;
                    else
                        this.debateFull.round4.minimized = false;
                    break;
                    break;
                case (5):
                    if (ifmin)
                        this.debateFull.round5.minimized = true;
                    else
                        this.debateFull.round5.minimized = false;
                    break;
                    break;
                case (6):
                    if (ifmin)
                        this.debateFull.round6.minimized = true;
                    else
                        this.debateFull.round6.minimized = false;
                    break;
                    break;
            }
        }

        this.trustedHtml = function (plainText) {
            return $sce.trustAsHtml(plainText);
        }

        this.replaceBackslash = function(text){
            var res = text.replace(/\n/g, "<br/>");
            return res;
        }

        $("span.anchor_debate").click(function (e) {
            e.preventDefault();
            $("div.anchor_debate").scrollintoview({duration: "fast", viewPadding: {y: 300}});
        });


    }

]
);


