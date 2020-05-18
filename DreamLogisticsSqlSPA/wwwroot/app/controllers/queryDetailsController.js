myControllers.controller('DetailsController', ['$scope', '$http', '$routeParams',
    function DetailsController($scope, $http, $routeParams) {

        $http.get("https://localhost:44313/api/query/NoList/" + $routeParams.queryId)
            .then(function (response) {

                $scope.queryInfo = response.data;

            });

    }]);