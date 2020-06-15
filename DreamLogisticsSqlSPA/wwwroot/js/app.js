var myApp = angular.module("myApp", [
    'ngRoute',
    'myControllers'
]);

myApp.config(['$routeProvider', '$locationProvider', '$httpProvider', function ($routeProvider, $locationProvider, $httpProvider) {

    $locationProvider.hashPrefix('');

    $routeProvider
        .when('/login', {
            templateUrl: '/app/partials/login.html',
            controller: 'LoginController'
        })
        .when('/queryList', {
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
        .when('/create', {
            templateUrl: '/app/partials/createAndEditQuery.html',
            controller: 'CreateEditController',
        })
        .when('/edit/:queryId', {
            templateUrl: '/app/partials/createAndEditQuery.html',
            controller: 'CreateEditController'
        })
        .otherwise({ redirectTo: '/queryList' })

    $httpProvider.interceptors.push('BearerAuthInterceptor');

}])    //Checks if there is a token and redirects accordingly. Sets userName and roleId to the $rootScope.user varible
    .run(['$rootScope', '$location', '$window', 'utilitiesService', function ($rootScope, $location, $window, utilitiesService) {

        $rootScope.$on("$routeChangeStart", function (event, next, current) {
            
            var token = $window.localStorage.getItem('token')
            if (token === null || typeof token === 'undefined' || token === '') {
                if (location.hash.indexOf('login') === -1) {
                    $window.location.href = '/#/login';
                }
            }
            else {
                var user = utilitiesService.getUserInfo(token)
                $window.localStorage.setItem('name', user.unique_name);
                $window.localStorage.setItem('roleId', user.role);
                $rootScope.user = {
                    roleId: $window.localStorage.getItem('roleId'),
                    name: $window.localStorage.getItem('name')
                };
                $window.location.href = '#/queryList';
            }
            
        });
        
    }]);




var myControllers = angular.module('myControllers', []);

myControllers.controller('CreateEditController', ['$scope', '$http', '$routeParams', '$rootScope', '$window','$document',
    function CreateEditController($scope, $http, $routeParams, $rootScope, $window, $document) {
        
        $document.on('keyup', keyboardEvents);

        $scope.$on('$destroy', function () {
            $document.off('keyup', keyboardEvents);
        });
        function keyboardEvents(event) {
            if (event.keyCode == 27 || event.which == 27 || event.key == "Escape") {
                $window.location.href = '#/queryList';
            }

        };

        //This controller is divided into three section 1. Create Parameter Logic || 2. Sql-test modal logic || 3. Http Post/Put logic
        //==============================================================================================================================
        //==============================================================================================================================
        //Create Parameter Logic: Checks Role Id, Checks if its Create or Edit Time by the queryId value
        //Adds and removes QueryParams input
        if ($rootScope.user.roleId != '4') {
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
            }
        }
        $scope.modalBoxEnter = function () {
            isMouseOverModalBox = true;
        }
        $scope.modalBoxLeave = function () {
            isMouseOverModalBox = false;
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
myControllers.controller('QueryListController', ['$scope', '$http', '$window', '$rootScope',
    function QueryListController($scope, $http, $window, $rootScope) {

        window.addEventListener('keydown', keyboardEvents);
        function keyboardEvents(event) {
            if (event.keyCode == 27 || event.which == 27 || event.key == "Escape") {
                const detailsSections = document.getElementsByClassName("details-drop-down-section");
                const deleteSections = document.getElementsByClassName("delete-drop-down-section");

                for (var del of deleteSections) {
                    del.style.display = "none";
                }
                for (var det of detailsSections) {
                    det.style.display = "none";
                }
            }
        };

        $scope.isAdmin = false;

        if ($rootScope.user.roleId === '4') {
            $scope.isAdmin = true;
            $http.get("https://localhost:44313/api/query/QueryParams")
                .then(function (response) {
                    $scope.query = response.data;
                });
        }

        $http.get("https://localhost:44313/api/query")
            .then(function (response) {
                $scope.isAuthorized = true;
                $scope.queries = response.data;

                if ($scope.queries == 0) {
                    $scope.noQueriesMessage = 'Det finns inga rapportmallar att välja på';
                }
            });

       //==============================================================================================================================
        //==============================================================================================================================
        //This section is for IT-role only, check details and delete Query/QueryParams (IT-role = 4)

        $scope.details = function (id) {

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
            if (options.style.display == "table-row") {
                options.style.display = "none"
            }
            else {
                options.style.display = "table-row";
            }

        };

        $scope.closeAll = function (id) {
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
myControllers.controller('SearchController', ['$scope', '$http', '$routeParams', 'utilitiesService', '$window', '$document',
    function SearchController($scope, $http, $routeParams, utilitiesService, $window, $document) {

        $scope.searchCompleted = false;
        $scope.invalidQueryParam = false;


        $document.on('keyup', keyboardEvents);

        $scope.$on('$destroy', function () {
            $document.off('keyup', keyboardEvents);
        });
        function keyboardEvents(event) {
            if (event.keyCode == 27 || event.which == 27 || event.key == "Escape") {
                $window.location.href = '#/queryList';
            }
        };

        $http.get("https://localhost:44313/api/query/List/" + $routeParams.queryId)
            .then(function (response) {
                for (var param of response.data.queryParams) {
                    if (param.typeId == 4) {
                        if (param.sqlLists.length == 0) {

                            $scope.invalidQueryParam = true;
                        }
                        else {
                            console.log(param.sqlLists);
                            param.sqlLists.unshift({
                                displayColumn: '(ej satt)',
                                keyColumn: null
                            });
                        }
                    }

                }
                $scope.searchInfo = response.data;

            });

        //button for downloading the result as an Excel file
        $scope.searchQueryExcel = function () {

            var getObject = getPayloadValues();

            if (getObject.valid) {

                $http.post("https://localhost:44313/api/Search/Excel",
                    getObject.payload, { responseType: 'blob' }
                ).then(function (response) {
                    var file = new Blob([response.data], {
                        type: 'application/octet-stream'
                    });
                    if (file.size != 0) {
                        $scope.noResult = '';

                        var fileUrl = URL.createObjectURL(file);

                        var fileName = utilitiesService.formatFileName($scope.searchInfo.query.groupName);

                        var a = document.createElement('a');
                        a.href = fileUrl;
                        a.target = '_blank';
                        a.download = fileName;
                        document.body.appendChild(a);
                        a.click();
                        document.body.removeChild(a);
                    }
                    else {
                        $scope.noResult = 'Inga resultat med den sökningen';
                    }


                }, function (error) {
                    console.log("error");
                    console.log(error);
                });
            }

        };
        //button for showing the result on the html page
        $scope.searchQuery = function () {

            var getObject = getPayloadValues();

            if (getObject.valid) {
                $http.post("https://localhost:44313/api/Search",
                    getObject.payload
                ).then(function (response) {
                    $scope.searchResult = response.data;
                    $scope.searchCompleted = true;

                    if ($scope.searchResult.columnNames.length == 0) {
                        $scope.noResultView = 'Felaktiga parametrar i Rapportmallen, Kontakta IT';
                    }
                    else if ($scope.searchResult.rowValues.length == 0) {
                        $scope.noResultView = 'Inga resultat med den sökningen';
                    }
                }, function (error) {
                    console.log("error");
                    console.log(error);
                });
            }

        }

        $scope.searchAgain = function () {
            $scope.searchCompleted = false;
        }

        //Logic to get the right format of the inputs and checking if they are valid
        function getPayloadValues() {
            $scope.validInputs = true;

            var payload = {
                QueryId: $scope.searchInfo.query.id,
                Parameters: []
            };

            angular.forEach($scope.searchInfo.queryParams, function (param) {

                if (param.typeId == 2) {
                    param.value = utilitiesService.formatDate(param.value);

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
                    payload.Parameters.push({ Id: param.id, Value: null });
                }

            });
            var returnObject = {
                valid: $scope.validInputs,
                payload
            }
            return returnObject;
        };

    }]);
(function () {
    'use strict';

    angular
        .module('myApp')
        .factory('utilitiesService', ['$http', '$q', function ($http, $q) {
            var service = {};

            //gets the playload data string from the token and decodes it to a json string in the "urlBase64Decode" function,
            //converts the json string and returns a readable object
            service.getUserInfo = function (token) {
                var regex = /\.(.*?)\./g
                var tokenStringFormated = regex.exec(token);
                var jsonToken = service.urlBase64Decode(tokenStringFormated[1])
                var tokenObject = JSON.parse(jsonToken);
                return tokenObject;
            }

            //Decodes the token payload data string and returns a json string
            service.urlBase64Decode = function (str) {

                var output = str.replace(/-/g, '+').replace(/_/g, '/');

                switch (output.length % 4) {
                    case 0:
                        break;
                    case 2:
                        output += '==';
                        break;
                    case 3:
                        output += '=';
                        break;
                    default:
                        throw 'Illegal base64url string';
                }

                return decodeURIComponent(window.escape(window.atob(output)));
            }

            //formats the javascript default full text date string into a "yyyy-MM-dd" date string 
            service.formatDate = function (date) {
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

            //formats the excel file name
            service.formatFileName = function (groupName) {

                var formatedGroupName = groupName.replace(/ /g, "_");

                var today = new Date();

                var date = today.getFullYear() + '-' + ('0' + (today.getMonth() + 1)).slice(-2) + '-' + ('0' + today.getDate()).slice(-2);
                var time = ('0' + today.getHours()).slice(-2) + "_" + ('0' + today.getMinutes()).slice(-2) + "_" + ('0' + today.getSeconds()).slice(-2);
                var dateTime = date + ' ' + time;

                var fileName = formatedGroupName + ' ' + dateTime + '.xlsx';

                return fileName;
            };

            return service;
        }]);
})();
myApp.factory('BearerAuthInterceptor', ['$window', '$q', '$rootScope', function ($window, $q, $rootScope) {
    return {
        request: function (config) {
            config.headers = config.headers || {};
            if ($window.localStorage.getItem('token')) {
                config.headers.Authorization = 'Bearer ' + $window.localStorage.getItem('token');
                
            }

            return config || $q.when(config);
        },
        responseError: function (responseError) {
            if (responseError.status === 401 && $window.localStorage.getItem('token') != '') {
                $window.localStorage.removeItem('token');
                $rootScope.user.name = '';
                $window.location.href = '#/login';
            }
            else if (responseError.status === 401) {
                $window.location.href = '#/login';
            }
            else if (responseError.status === 404) {
                $window.location.href = '/#/queryList';
            }
        },
        response: function (response) {

            return response || $q.when(response);
        }
    };

}]);