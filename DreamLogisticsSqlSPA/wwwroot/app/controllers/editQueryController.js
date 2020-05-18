myControllers.controller('EditController', ['$scope', '$http', '$routeParams',
    function EditController($scope, $http, $routeParams) {
        $scope.queryEdited = false;

        $scope.deletedQueryParams = [];


        $http.get("https://localhost:44313/api/query/NoList/" + $routeParams.queryId)
            .then(function (response) {

                $scope.editInfo = response.data;
                console.log($scope.editInfo);
            });
        
        $scope.addOptions = function (id) {

            const sqlOptions = document.getElementsByClassName("sql-options-" + id);

            if ($scope.editInfo.queryParams[id].typeId == "4") {

                for (const sqlOption of sqlOptions) {
                    sqlOption.style.display = "block"
                }
            }
            else {

                for (const sqlOption of sqlOptions) {
                    sqlOption.style.display = "none"
                }

                $scope.editInfo.queryParams[id].tableName = '';
                $scope.editInfo.queryParams[id].displayColumn = '';
                $scope.editInfo.queryParams[id].keyColumn = '';
            }
        }

        $scope.addRow = function () {

            $scope.editInfo.queryParams.push(
                {
                    name: '',
                    typeId: 0,
                    parameterCode: '',
                    tableName: '',
                    displayColumn: '',
                    keyColumn: '',
                    paramRequired: ''
                }
            );
        };

        $scope.removeRow = function (row) {
            var idToDelete = $scope.editInfo.queryParams[row].id;

            if (idToDelete != undefined) {
                $scope.deletedQueryParams.push(idToDelete)
            }
            $scope.editInfo.queryParams.splice(row, 1);

        };

        $scope.editQuery = function () {

            angular.forEach($scope.editInfo.queryParams, function (param) {
                param.typeId = parseInt(param.typeId)
            });

            $scope.editInfo.queryParamsToDelete = $scope.deletedQueryParams;
            $http({
                method: "PUT",
                url: "https://localhost:44313/api/query/UpdateQuery",
                data: $scope.editInfo
            })
            $scope.queryEdited = true;
        }

    }]);