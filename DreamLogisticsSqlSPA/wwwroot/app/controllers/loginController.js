myControllers.controller('LoginController', ['$scope', '$http', '$window',
    function LoginController($scope, $http, $window) {
        
        window.addEventListener('keydown', loginAttemptKey);
        function loginAttemptKey(event) {
            if (event.keyCode == 13 || event.which == 13 || event.key == "Enter") {
                $scope.loginAttempt();
            }
        };

        $scope.login = {
            username: '',
            password: ''
        };

        $scope.loginAttempt = function () {
            $http.post("https://localhost:44313/api/Login/",
                $scope.login
            ).then(function (response) {
                var token = response.data;
                if (token != '') {
                    $window.localStorage.setItem('token', token);

                    $window.location.href = '/#/queryList';
                }
                else {
                    $scope.invalidUser = 'Felaktiga Inloggninsuppgifter';
                }
            }, function (error) {
                console.log("error");
                console.log(error);
            });
        };
    }]);