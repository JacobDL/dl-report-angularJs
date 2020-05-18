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