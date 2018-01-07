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

        var _this = this;
        //************** LOGIN START***********************************************************//
        this.login = function (loginForm) {
      
            var data =  {};
            if(_this.loginemail.toString().match(/[@]/)){
                data = {
                    "username": "",
                    "email": _this.loginemail,
                    "password": _this.loginpassword
                };
            } else {
                data = {
                    "username": _this.loginemail,
                    "email": "",
                    "password": _this.loginpassword
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
           
            _this.wrongCredentialsVar = false;

            var res = response.data;
            if(res["ErrorCode"] !== undefined){
                var code = res["ErrorCode"];
                var wrong = errHandler.handleError(code);
                if(wrong === undefined){
                    wrong = "Error in performing login";
                }
                _this.wrong = wrong;
                 _this.wrongCredentialsVar = true;
            }
            else {
                var tkn = response.data;
                localStorage.setItem('auth_token',tkn);
                navigateFactory.goToAcasa();
            }
        };
        var errorLogin = function (response) {
            _this.wrong = "Date de logare invalide";
            _this.wrongCredentialsVar = true;
            _this.user.password = "";
        };
        //************** LOGIN END ***********************************************************//


        //************** REGISTER START ***********************************************************//
        this.registerAccount = function () {

            var email = this.registerusername

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
            _this.wrongCredentialsVar = false;
            var res = response.data;
            if(res["ErrorCode"] !== undefined){
                var code = res["ErrorCode"];
                var wrong = errHandler.handleError(code);
                if(wrong === undefined){
                    wrong = "Error in performing login";
                }
                _this.wrong = wrong;
                _this.wrongCredentialsVar = true;
            }
            else {
                sweetAlert("Succes", "Contul a fost creat. Va puteti loga", "success");
                _this.showLoginF();
            }


        };
        var errorRegister = function (response) {
            if(response === "birthate_error"){
                _this.wrong = "Selectati data nasterii";
                _this.wrongCredentialsVar = true;
            } else
            if (response.data.message === "dupplicate_email") {
                _this.wrong = "Exista un cont cu aceasta adresa de E-mail";
                _this.wrongCredentialsVar = true;
                _this.registeremail = "";
            } else
            if (response.data.message === "dupplicate_username") {
                _this.wrong = "Exista un cont cu acest username";
                _this.wrongCredentialsVar = true;
                _this.registerusername = "";
            }
        };
        //************** REGISTER END ***********************************************************//

        this.showWindow = function(type){
            if(type == 'login'){
                this.showLogin = true;
                this.showRegister = false;
            } else {
                this.showLogin = false;
                this.showRegister = true;
            }
        }


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

        var path = window.location.href;
        var urlParams = $location.search();
        this.verifyErrorMsg = "";
        if(path.includes("/verify")){
            this.verifyEmail = urlParams.email;
            $("#footer").addClass("hidden");            
        }

        this.checkVerificationCode = function(){
            var content = {
                email: this.verifyEmail,
                code: this.codActivare
            }
            var promise = loginFactory.checkVerificationCode(content);
            promise.then(successVerification, errorVerification);
        }

        var successVerification = function (response) {
            if(response.data == true){
                navigateFactory.goToLogin();
                _this.verifyErrorMsg = "";
                _this.codActivare = "";
                $location.url($location.path());
                sweetAlert("Succes", "Contul a fost verificat. Va puteti loga", "success");
            } else{
                _this.verifyErrorMsg = "Codul nu este valid";
                _this.codActivare = "";
            }
        };
        var errorVerification = function (response) {
            console.log("error");
        };

        this.resendVerificationCode = function(){
            var promise = loginFactory.resendVerificationCode(this.verifyEmail);
            promise.then(function (response) {
                _this.verifyErrorMsg = "Codul de verificare a fost retrimis pe E-mail";
                _this.codActivare = "";
            })
        }

    }]
);
