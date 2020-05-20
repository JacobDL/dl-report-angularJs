myControllers.controller('LoginController', ['$scope', '$http', '$window',
    function LoginController($scope, $http, $window) {

        $scope.login = {
            username: '',
            password: ''
        };

        $scope.loginAttempt = function () {

            $http.post("https://localhost:44313/api/Login/",
                $scope.login
            ).then(function (response) {
                $scope.userResult = response.data;
                if ($scope.userResult.username != null) {
                    //$rootscope.loggedInuser = true;
                    $window.location.href = '/#!/queryList';
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