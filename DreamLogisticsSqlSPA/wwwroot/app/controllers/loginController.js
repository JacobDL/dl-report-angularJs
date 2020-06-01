myControllers.controller('LoginController', ['$scope', '$http', '$window', '$document',
    function LoginController($scope, $http, $window, $document) {

        $scope.login = {
            username: '',
            password: ''
        };

        $document.on('keyup', loginAttemptKey);

        $scope.$on('$destroy', function () {
            $document.off('keyup', loginAttemptKey);
        });
        function loginAttemptKey(event) {
            if (event.keyCode == 13 || event.which == 13 || event.key == "Enter") {
                $scope.loginAttempt();
            }
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