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