var myControllers = angular.module('myControllers', []);
var isAdmin = true;
if (isAdmin) {


myControllers.controller('CreateController', ['$scope', '$http',
    function CreateController($scope, $http) {
        $scope.queryCreated = false;

        $scope.formData = {
            groupName: '',
            sql: ''
        };

        $scope.formData.rows = [{
            name: '',
            typeId: 0,
            parameterCode: '',
            tableName: '',
            displayColumn: '',
            keyColumn: '',
            paramOptional:''
        }];

        $scope.addRow = function () {

            $scope.formData.rows.push(
                {
                    name: '',
                    typeId: 0,
                    parameterCode: '',
                    tableName: '',
                    displayColumn: '',
                    keyColumn: '',
                    paramOptional: ''
                }
            );
            console.log($scope.formData.rows);
        };

        $scope.removeRow = function (row) {
            $scope.formData.rows.splice(row, 1);

        };

        $scope.addOptions = function (id) {

            const sqlOptions = document.getElementsByClassName("sql-options-" + id);

            if ($scope.formData.rows[id].typeId == "4") {

                for (const sqlOption of sqlOptions) {
                    sqlOption.style.display = "block"
                }
            }
            else {

                for (const sqlOption of sqlOptions) {
                    sqlOption.style.display = "none"
                }

                $scope.formData.rows[id].tableName = '';
                $scope.formData.rows[id].displayColumn = '';
                $scope.formData.rows[id].keyColumn = '';
            }
        }

        $scope.createQuery = function () {
            console.log("Hallå?!?!?!?")
            angular.forEach($scope.formData.rows, function (row) {
                row.typeId = parseInt(row.typeId)
                if (row.paramOptional == 0) {
                    row.paramOptional = false;
                }
                else {
                    row.paramOptional = true;
                }
            });
            $http({
                method: "POST",
                url: "https://localhost:44313/api/query/Create",
                data: $scope.formData
            })
            $scope.queryCreated = true;
        }

    }]);
}