myControllers.controller('SearchController', ['$scope', '$http', '$routeParams', 'utilitiesService', '$window',
    function SearchController($scope, $http, $routeParams, utilitiesService, $window) {

        $scope.searchCompleted = false;
        $scope.invalidQueryParam = false;

        window.addEventListener('keydown', keyboardEvents);
        function keyboardEvents(event) {
            if (event.keyCode == 13 || event.which == 13 || event.key == "Enter") {

                if ($scope.searchCompleted == true) {
                    $scope.searchQueryExcel();
                }
                else {
                    $scope.searchQuery();
                }
            }
            else if (event.keyCode == 27 || event.which == 27 || event.key == "Escape") {
                $window.location.href = '#/queryList';
            }
        };

        $http.get("https://localhost:44313/api/query/List/" + $routeParams.queryId)
            .then(function (response) {
                console.log(response.data.queryParams)
                for (var param of response.data.queryParams) {
                    console.log(param);
                    if (param.typeId == 4 && param.sqlLists.length == 0) {
                        $scope.invalidQueryParam = true;
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
        //resonse.columnNames.length
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
                    payload.Parameters.push({ Id: param.id, Value: 'null' });
                }

            });
            var returnObject = {
                valid: $scope.validInputs,
                payload
            }
            return returnObject;
        };

    }]);