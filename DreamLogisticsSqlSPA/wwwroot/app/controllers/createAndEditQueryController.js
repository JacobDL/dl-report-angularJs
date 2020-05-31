var myControllers = angular.module('myControllers', []);

myControllers.controller('CreateEditController', ['$scope', '$http', '$routeParams', '$rootScope', '$window',
    function CreateEditController($scope, $http, $routeParams, $rootScope, $window) {
        
        window.addEventListener('keydown', keyboardEvents);
        function keyboardEvents(event) {
            console.log(event);
            if (event.keyCode == 27 || event.which == 27 || event.key == "Escape") {
                $window.location.href = '#/queryList';
            }

        };

        //This controller is divided into three section 1. Create Parameter Logic || 2. Sql-test modal logic || 3. Http Post/Put logic
        //==============================================================================================================================
        //==============================================================================================================================
        //Create Parameter Logic: Checks Role Id, Checks if its Create or Edit Time by the queryId value
        //Adds and removes QueryParams input
        if ($rootScope.user.roleId == 2) {
            $window.location.href = '#/queryList';
        }

        $scope.queryEdited = false;
        $scope.create = false;
        $scope.deletedQueryParams = [];

        if ($routeParams.queryId === undefined) {

            $scope.create = true;
            $scope.queryData = {
                groupName: '',
                sql: ''
            };

            $scope.queryData.queryParams = [{
                name: '',
                typeId: 1,
                parameterCode: '',
                tableName: '',
                displayColumn: '',
                keyColumn: '',
                paramOptional: ''
            }];

            $scope.title = 'Skapa Rapportmall';
        }
        else {

            $http.get("https://localhost:44313/api/query/NoList/" + $routeParams.queryId)
                .then(function (response) {

                    $scope.queryData = response.data;
                    $scope.title = 'Redigera Rapportmall';

                });
        }

        $scope.addOptions = function (id) {

            const sqlOptions = document.getElementsByClassName("sql-options-" + id);

            if ($scope.queryData.queryParams[id].typeId == "4") {

                for (const sqlOption of sqlOptions) {
                    sqlOption.style.display = "block"
                }
            }
            else {

                for (const sqlOption of sqlOptions) {
                    sqlOption.style.display = "none"
                }

                $scope.queryData.queryParams[id].tableName = '';
                $scope.queryData.queryParams[id].displayColumn = '';
                $scope.queryData.queryParams[id].keyColumn = '';
            }
        }

        $scope.addRow = function () {

            $scope.queryData.queryParams.push(
                {
                    name: '',
                    typeId: 1,
                    parameterCode: '',
                    tableName: '',
                    displayColumn: '',
                    keyColumn: '',
                    paramRequired: ''
                }
            );
        };

        $scope.removeRow = function (row) {
            var idToDelete = $scope.queryData.queryParams[row].id;

            if (idToDelete != undefined) {
                $scope.deletedQueryParams.push(idToDelete)
            }
            $scope.queryData.queryParams.splice(row, 1);

        };

        //==============================================================================================================================
        //==============================================================================================================================
        //Sql Test Box Logic (Includes being able to close the modal box with just a click outside its div)
        var isMouseOverModalBox = true;
        window.addEventListener("click", closeModalBox);

        $scope.testSqlValues = function (queryParam) {
            var sqlSearchParam = {
                tableName: queryParam.tableName,
                displayColumn: queryParam.displayColumn,
                keyColumn: queryParam.keyColumn
            };

            $http.post("https://localhost:44313/api/query/SqlList", sqlSearchParam)
                .then(function (response) {
                    $scope.sqlList = response.data;
                })
            const sqlBox = document.getElementById("text");
            sqlBox.style.display = "block";

        };
        $scope.setBoolFalse = function () {
            isMouseOverModalBox = false;
        }
        $scope.setBoolTrue = function () {
            isMouseOverModalBox = true;
        }
        function closeModalBox() {
            if (isMouseOverModalBox == false) {
                const sqlBox = document.getElementById("text");
                sqlBox.style.display = "none";
                isMouseOverModalBox = true;
                console.log(isMouseOverModalBox)
            }
        }
        $scope.modalBoxEnter = function () {
            isMouseOverModalBox = true;
            console.log(isMouseOverModalBox)
        }
        $scope.modalBoxLeave = function () {
            isMouseOverModalBox = false;
            console.log(isMouseOverModalBox)
        }

        $scope.close = function () {
            const sqlBox = document.getElementById("text");
            sqlBox.style.display = "none";
            isMouseOverModalBox = true;
        }

        //==============================================================================================================================
        //==============================================================================================================================
        //Save button Logic and http post/put
        $scope.submitQuery = function () {
            if ($scope.create) {
                createQuery();
            }
            else if (!$scope.create) {
                editQuery();
            }
        }
        editQuery = function () {
            $scope.completeMessage = 'Rapportmall Uppdaterad!'
            angular.forEach($scope.queryData.queryParams, function (param) {
                param.typeId = parseInt(param.typeId)
            });

            $scope.queryData.queryParamsToDelete = $scope.deletedQueryParams;
            $http({
                method: "PUT",
                url: "https://localhost:44313/api/query/UpdateQuery",
                data: $scope.queryData
            })
            $scope.queryEdited = true;
        }

        createQuery = function () {
            $scope.completeMessage = 'Rapportmall Skapad!'
            angular.forEach($scope.queryData.queryParams, function (param) {
                param.typeId = parseInt(param.typeId)
                if (param.paramOptional == 0) {
                    param.paramOptional = false;
                }
                else {
                    param.paramOptional = true;
                }
            });
            $http({
                method: "POST",
                url: "https://localhost:44313/api/query/Create",
                data: $scope.queryData
            })
            $scope.queryEdited = true;
        }

    }]);