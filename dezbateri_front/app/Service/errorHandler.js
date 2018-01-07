(function () {
    var userServices = angular.module('errorHandler', []);

    userServices.factory('errHandler', [
        function () {

            return {
                handleError: function(code){
                    switch(code) {
                        case 101:
                            return "Error connecting to database";
                        case 102:
                            return "Error authenticating";
                        case 120:
                            return "No header provided";
                        case 121:
                            return "No token provided";
                        case 122:
                            return "No access provided";
                        case 123:
                            return "Session expired";
                        case 124:
                            return "Operation denied";
                        case 133:
                            return "Adresa de E-mail nu este verificata"
                                +"\n Pentru a te putea autentifica, urmeaza pasii primiti pe E-mail";
                        case 400:
                            return "Success";
                        case 700:
                            return "Error no password provided";
                        case 701:
                            return "Error no username or email provided"
                        case 702:
                            return "Error accessing login services";
                        case 703:
                            return "Error no birthdate provided";
                        case 704:
                            return "Username already exists";
                        case 705:
                            return "Email already exists";
                        case 706:
                            return "No body provided";
                        case 800:
                            return "Error adding user to db";
                        case 801:
                            return "Error getting all users from db";
                        case 802:
                            return "No users in db";
                        case 803:
                            return "No user in database with that name";
                        case 804:
                            return "Error getting user from database";
                        case 805:
                            return "Error updating user from database";
                        case 806:
                            return "Error deleting user from database";
                        case 807:
                            return "Duplicate entry";
                        case 810:
                            return "Error adding category to db";
                        case 811:
                            return
                        case 812:
                            return
                        case 813:
                            return
                        case 814:
                            return
                        case 815:
                            return
                        case 816:
                            return

                    }
                }

            };
        }
    ]);



})();