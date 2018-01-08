var index = angular.module('adminModule', ['ngRoute', 'angularTrix', 'ngSanitize','base64']);

index.controller('adminController', ['navigateFactory', 'loginFactory', '$location', 'errHandler', '$timeout', '$filter', '$sce','$base64',
    'userFactory', 'opiniiFactory', 'debateFactory',
    function (navigateFactory, loginFactory, $location, errHandler,$timeout, $filter, $sce,$base64,
        userFactory, opiniiFactory,debateFactory) {
        var _this = this;
        
        if(localStorage.getItem("adlogtryfail")){
          
            var date = new Date().getTime();
            var hours = 720000;
            var then_time = localStorage.getItem("adlogtryfail");
            then_time = parseInt(then_time) + parseInt(hours);
            if(date  <  then_time){
                navigateFactory.goToAcasa();
            } else {
                localStorage.removeItem("adlogtryfail");
                localStorage.removeItem("howmanytimes");
            }

        }
      
        _this.navigateFactory = navigateFactory;
        _this.userLogged = navigateFactory.getUserLogged();
  
        _this.loginemail = null;
        _this.loginpassword = null;
        _this.js_token = null;
        _this.howManyTimesItTried = 0;

        _this.showFirst = true;
        _this.showSecond = false;
        _this.mytkn = "";
        _this.showMain = false;

        if(localStorage.getItem("mytkn") == $base64.encode("logged") && localStorage.getItem("auth_token")){
            
            _this.showSecond = false;
            _this.showFirst = false;
            _this.showMain = true;
        } else {
            localStorage.removeItem("auth_token");
        }

        _this.adminLogin = function (loginForm) {
      
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
                _this.mytkn = tkn;
                var decoded = loginFactory.checkAdmin(tkn);
                if(decoded){
                    _this.generate(defaultKey);
                    _this.showFirst = false;
                    _this.showSecond = true;
                }else{
                    $timeout(function() {
                        navigateFactory.goToAcasa();
                     }, 3000);
                    
                     swal("Interzis", "Nu ai drepturile necesare pentru a accesa pagina", "error");
                }
            }
        };

        var errorLogin = function (response) {
            _this.wrong = "Date de logare invalide";
            _this.wrongCredentialsVar = true;
            _this.user.password = "";
        };
        
        var defaultKey = 'hxdm vjec jjws rb3h wizr 4ifu gftm xboz';
        var key;
        var $ = function (x) {
          return document.querySelector(x);
        };
        
        _this.generate = function(ke) {
          Authenticator.generateKey().then(function (k) {
            var $keyEl = defaultKey;
            if (ke) {
              key = ke;
            }
            else if ($keyEl.value) {
              key = $keyEl.value;
              $keyEl.placeholder = key;
              $keyEl.value = '';
            }
            else {
              key = k;
              $keyEl.placeholder = key;
            }
        
            var companyName ="dezbateri.ro";
            var userAccount = _this.loginemail;
            var algo =  'SHA1';
            var digits =  6;
            var period =  30;
        
            var otpauth = Authenticator.generateTotpUri(key, userAccount, companyName, algo, digits, period);
            var src = 'https://chart.googleapis.com/chart?chs=166x166&chld=L|0&cht=qr&chl=' + encodeURIComponent(otpauth);
        
            $('img.js-qrcode').src = src;
        
            Authenticator.generateToken(key).then(function (token) {
              Authenticator.verifyToken(key, token).then(function (result) {
                Authenticator.verifyToken(key, '000 000').then(function (result) {
                });
              });
            });
          });
        }
        
        _this.verify = function () {
          var token = _this.js_token;
          var checked = false;
        
          if (!/.*\d{3}.*\d{3}.*/.test(token)) {
            window.alert("must have a 6 digit token");
            return;
          }
          
          Authenticator.verifyToken(key, token)
          .then(function (result) {
            var msg;
            if (result) {
                localStorage.removeItem("howmanytimes");
                return true;
            } else {
                if(localStorage.getItem("howmanytimes"))
                    _this.howManyTimesItTried = localStorage.getItem("howmanytimes");
                if(_this.howManyTimesItTried == 3){
                    $timeout(function() {
                        navigateFactory.goToAcasa();
                    }, 5000);
                    
                    swal("Interzis", "Nu ai drepturile necesare pentru a accesa pagina. Accesul va fi oprit pentru 2 ore.", "error");
                    var date = new Date();
                    localStorage.setItem("adlogtryfail", date.getTime());
                } else {
                    _this.howManyTimesItTried ++;
                    localStorage.setItem("howmanytimes", _this.howManyTimesItTried);
                    var incercari = 3 - _this.howManyTimesItTried;
                    swal("Cod gresit", "Mai ai " +  incercari + " incercari", "warning");
                }
            }
          }, function (err) {
            window.alert('[ERROR]:' + err.message);
            window.alert('[ERROR]:' + err.stack);
          })
          .then(function(result){
            if(result){
                localStorage.setItem("mytkn", $base64.encode("logged"));
                localStorage.setItem("auth_token", _this.mytkn);
                window.location.reload();
            }
         });

        
        }     

        _this.menuChosen = "1";

        _this.showMenu = function(type){
            if(type == _this.menuChosen)
                return true;
        }

        _this.changeMenu = function(type){
            _this.menuChosen = type;
        }
        
        _this.allUsers = [];
        
        userFactory.getAllUsers().then(function (response) {
            _this.allUsers = response.data;
        });


        
    }]
);
