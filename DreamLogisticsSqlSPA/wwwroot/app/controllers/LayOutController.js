myControllers.controller('LayOutController', ['$scope', '$http', '$window', '$rootScope',
    function LayOutController($scope, $http, $window, $rootScope) {

        $scope.logOut = function () {
            $window.localStorage.removeItem('token');
            $rootScope.user.name = '';
            $window.location.href = '#/login';
        }
        $scope.homeBtn = function () {
            if ($window.localStorage.getItem('token')) {

                $window.location.href = '#/queryList';
            }
        }

    }]);