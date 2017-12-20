var index = angular.module('loginModule', ['ngRoute']);

index.controller('loginController', ['navigateFactory','loginFactory', 'userFactory', 'errHandler', '$scope', '$location',
    function (navigateFactory, loginFactory, userFactory,errHandler, $scope, $location) {

        this.navigateFactory = navigateFactory;
        this.userLogged = navigateFactory.getUserLogged();

        this.showLogin = true;
        this.showRegister = false;
        this.user = null;
        this.wrongCredentialsVar = false;
        this.wrong = "Date de logare invalide";

        this.mailRegex = /^([a-zA-Z0-9]|[.]|[_])+[@][a-zA-Z0-9]+[.][a-zA-Z0-9]+$/i;
        this.usernameRegex = /^[a-zA-Z0-9]+$/i;

        this.registerusername = null;
        this.registerpassword = null;
        this.registeremail = null;
        this.registerbirthdate = null;

        this.loginemail = "";
        this.loginpassword = "";

        this.showLoginF = function () {
            this.showLogin = true;
            this.showRegister = false;
        };

        this.showRegistration = function () {
            this.showRegister = true;
            this.showLogin = false;
        };

        this.wrongCredentials = function () {
            return this.wrongCredentialsVar;
        };

        this.setValidBithdate = function () {
            this.registrationForm["birthdate"].$setValidity('birthdate', true);
        };


        //************** LOGIN START***********************************************************//
        this.login = function (loginForm) {
      
            var data =  {};
            if(this.loginemail.toString().match(/[@]/)){
                data = {
                    "username": "",
                    "email": this.loginemail,
                    "password": this.loginpassword
                };
            } else {
                data = {
                    "username": this.loginemail,
                    "email": "",
                    "password": this.loginpassword
                };
            }
            if (loginForm.$valid) {

                var promise = loginFactory.login(data);
                promise.then(successLogin, errorLogin);
                
            } else {
                console.log("ELSE");
            }
        };
        var successLogin = function (response) {
           
            this.wrongCredentialsVar = false;

            var res = response.data;
             console.log(response.data);
            if(res["ErrorCode"] !== undefined){
                var code = res["ErrorCode"];
                var wrong = errHandler.handleError(code);
                console.log(code);
                if(wrong === undefined){
                    wrong = "Error in performing login";
                }
                this.wrong = wrong;
                 console.log(this.wrong);
                this.wrongCredentialsVar = true;
                 console.log(this.wrongCredentialsVar);
            }
            else {
                var tkn = response.data;
                localStorage.setItem('auth_token',tkn);
                navigateFactory.goToAcasa();
            }
        };
        var errorLogin = function (response) {
            this.wrong = "Date de logare invalide";
            this.wrongCredentialsVar = true;
            this.user.password = "";
        };
        //************** LOGIN END ***********************************************************//


        //************** REGISTER START ***********************************************************//
        this.registerAccount = function () {

            var birth = $("#datetimepicker9").find("input").val();
            if (birth === "") {
                errorRegister("birthate_error");
            } else {
                var data = {
                    "username": this.registerusername,
                    "password": this.registerpassword,
                    "email": this.registeremail,
                    "accountType": "User",
                    "birthdate": birth
                };

                var promise = loginFactory.register(data);
                promise.then(successRegister, errorRegister);
            }
        };
        var successRegister = function (response) {
            this.wrongCredentialsVar = false;
            var res = response.data;
            if(res["ErrorCode"] !== undefined){
                var code = res["ErrorCode"];
                var wrong = errHandler.handleError(code);
                if(wrong === undefined){
                    wrong = "Error in performing login";
                }
                this.wrong = wrong;
                this.wrongCredentialsVar = true;
            }
            else {
                sweetAlert("Succes", "Contul a fost creat. Va puteti loga", "success");
                this.showLoginF();
            }


        };
        var errorRegister = function (response) {
            if(response === "birthate_error"){
                this.wrong = "Selectati data nasterii";
                this.wrongCredentialsVar = true;
            } else
            if (response.data.message === "dupplicate_email") {
                this.wrong = "Exista un cont cu aceasta adresa de E-mail";
                this.wrongCredentialsVar = true;
                this.registeremail = "";
            } else
            if (response.data.message === "dupplicate_username") {
                this.wrong = "Exista un cont cu acest username";
                this.wrongCredentialsVar = true;
                this.registerusername = "";
            }
        };
        //************** REGISTER END ***********************************************************//




        $('#datetimepicker9').datetimepicker({
            viewMode: 'years',
            format: 'YYYY/MM/DD'
        });

        $(".tiptext").mouseover(function () {
            $(this).children(".description").show();
        }).mouseout(function () {
            $(this).children(".description").hide();
        });

        $('#datePicked').keydown(function () {
            //code to not allow any changes to be made to input field
            return false;
        });


    }]
);
