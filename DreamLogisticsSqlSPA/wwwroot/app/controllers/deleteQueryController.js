myControllers.controller('DeleteController', ['$scope', '$http', '$routeParams',
    function DeleteController($scope, $http, $routeParams) {
        $scope.queryDeleted = false;
        $http.get("https://localhost:44313/api/query/NoList/" + $routeParams.queryId)
            .then(function (response) {

                $scope.deleteInfo = response.data;

            });
        $scope.deleteQuery = function () {
            $http.delete("https://localhost:44313/api/query/DeleteQuery/" + $routeParams.queryId)
            $scope.queryDeleted = true;
        }

    }]);