var myApp = angular.module("myApp", [
    'ngRoute',
    'myControllers'
]);

myApp.config(['$routeProvider', function ($routeProvider) {
    $routeProvider
        .when('/', {
            templateUrl: '/app/partials/queryList.html',
            controller: 'QueryListController'
        })
        .when('/search/:queryId', {
            templateUrl: '/app/partials/search.html',
            controller: 'SearchController'
        })
        .when('/result', {
            templateUrl: '/app/partials/result.html',
            controller: 'ResultController'
        })
        
}]);
var isAdmin = true;

if (isAdmin) {


myApp.config(['$routeProvider', function ($routeProvider) {
    $routeProvider
        
        .when('/create', {
            templateUrl: '/app/partials/createQuery.html',
            controller: 'CreateController'
        })
        
        .when('/delete/:queryId', {
            templateUrl: '/app/partials/deleteQuery.html',
            controller: 'DeleteController'
        })
        .when('/edit/:queryId', {
            templateUrl: '/app/partials/editQuery.html',
            controller: 'EditController'
        })
        .when('/details/:queryId', {
            templateUrl: '/app/partials/queryDetails.html',
            controller: 'DetailsController'
        })
}]);
}
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
myControllers.controller('DetailsController', ['$scope', '$http', '$routeParams',
    function DetailsController($scope, $http, $routeParams) {

        $http.get("https://localhost:44313/api/query/NoList/" + $routeParams.queryId)
            .then(function (response) {

                $scope.queryInfo = response.data;

            });

    }]);
myControllers.controller('QueryListController', ['$scope', '$http',
    function QueryListController($scope, $http) {
        $scope.isAdmin = true;

        $http.get("https://localhost:44313/api/query")
            .then(function (response) {
                $scope.queries = response.data;

                if ($scope.queries == 0) {
                    $scope.noQueriesMessage = 'Det finns inga rapportmallar att välja på';
                }
            });

        $scope.details = function (id) {
            $http.get("https://localhost:44313/api/query/NoList/" + id) 
                .then(function (response) {

                    $scope.query = response.data;

                });

            const options = document.getElementById("details-option-" + id);

            if (options.style.display == "table-row") {
                options.style.display = "none"
            }
            else {
                options.style.display = "table-row";
            }
        };

        $scope.delete = function (index) {

            const options = document.getElementById("delete-option-" + index);
            console.log(options.style.display);
            if (options.style.display == "table-row") {
                options.style.display = "none"
            }
            else {
                options.style.display = "table-row";
            }

        };

        $scope.closeAll = function (id) {
            console.log(id);
            const deleteOptions = document.getElementById("delete-option-" + id);
            const deatailsOptions = document.getElementById("details-option-" + id);

            deleteOptions.style.display = "none";
            deatailsOptions.style.display = "none";
        }

        $scope.deleteConfirmed = function (id) {
            
            $http.delete("https://localhost:44313/api/query/DeleteQuery/" + id)

            const deleteOptions = document.getElementById("delete-option-" + id);
            const queryRow = document.getElementById("query-row-" + id);
            const deatailsOptions = document.getElementById("details-option-" + id);

            const deleteMessage = document.getElementById("delete-message-top");
            const deleteConfirmed = document.getElementById("delete-confirmed-" + id);

            queryRow.style.display = "none";
            deatailsOptions.style.display = "none";
            deleteOptions.style.display = "none";

            deleteConfirmed.style.display = "table-row";
            deleteMessage.style.display = "block";
        };

        $scope.closeDeleteConfirmed = function (id) {
            const deleteConfirmed = document.getElementById("delete-confirmed-" + id);
            const deleteMessage = document.getElementById("delete-message-top");

            deleteConfirmed.style.display = "none";
            deleteMessage.style.display = "none";
        }



    }]);
myControllers.controller('SearchController', ['$scope', '$http', '$routeParams',
    function SearchController($scope, $http, $routeParams) {

        $scope.searchCompleted = false;

        $http.get("https://localhost:44313/api/query/List/" + $routeParams.queryId)
            .then(function (response) {

                $scope.searchInfo = response.data;

            });
      
        $scope.searchQueryExcel = function () {
            
            var getObject = getPayloadValues();

            if (getObject.valid) {

                $http.post("https://localhost:44313/api/Search/Excel",
                    getObject.payload, { responseType: 'blob' }
                ).then(function (response) {
                    
                    var file = new Blob([response.data], {
                        type: 'application/octet-stream'
                    });

                    var fileUrl = URL.createObjectURL(file);

                    var fileName = formatFileName($scope.searchInfo.query.groupName);

                    var a = document.createElement('a');
                    a.href = fileUrl;
                    a.target = '_blank';
                    a.download = fileName;
                    document.body.appendChild(a);
                    a.click();
                    document.body.removeChild(a);

                }, function (error) {
                    console.log("error");
                    console.log(error);
                });
            }
        };

        $scope.searchQuery = function () {

            var getObject = getPayloadValues();

            if (getObject.valid) {

                $http.post("https://localhost:44313/api/Search/",
                    getObject.payload
                ).then(function (response) {
                    $scope.searchResult = response.data;
                    $scope.searchCompleted = true;
                    console.log($scope.searchResult);
                }, function (error) {
                    console.log("error");
                    console.log(error);
                });
            }

        }
        function getPayloadValues() {
            $scope.validInputs = true;
            
            var payload = {
                QueryId: $scope.searchInfo.query.id,
                Parameters: []
            };

            angular.forEach($scope.searchInfo.queryParams, function (param) {

                if (param.typeId == 2) {
                    param.value = formatDate(param.value);

                    var regex = /^[0-9]{4}-[0-9]{2}-[0-9]{2}/;

                    if (!regex.test(param.value)) {
                        $scope.dateInputMessage = "Ogiltligt Datum";
                        $scope.validInputs = false;
                    }
                }
                if (param.value != null) {
                    payload.Parameters.push({ Id: param.id, Value: param.value.toString() });
                }
                else {
                    payload.Parameters.push({ Id: param.id, Value: 'null' });
                }
                
            });
            var returnObject = {
                valid: $scope.validInputs,
                payload
            }
            return returnObject;
        };

        function formatDate(date) {
            var d = new Date(date),
                month = '' + (d.getMonth() + 1),
                day = '' + d.getDate(),
                year = d.getFullYear();

            if (month.length < 2)
                month = '0' + month;
            if (day.length < 2)
                day = '0' + day;

            return [year, month, day].join('-');
        };

        function formatFileName(groupName) {

            var formatedGroupName = groupName.replace(/ /g, "_");

            var today = new Date();
            
            var date = today.getFullYear() + '-' + ('0' + (today.getMonth() + 1)).slice(-2) + '-' + ('0' + today.getDate()).slice(-2);
            var time = ('0' + today.getHours()).slice(-2) + "_" + ('0' + today.getMinutes()).slice(-2) + "_" + ('0' + today.getSeconds()).slice(-2);
            var dateTime = date + ' ' + time;

            var fileName = formatedGroupName + ' ' + dateTime + '.xlsx';

            return fileName;
        };

    }]);