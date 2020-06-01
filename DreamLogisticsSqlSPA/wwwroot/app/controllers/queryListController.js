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

        $scope.isAdmin = true;

        if ($rootScope.user.roleId === '4') {
            $scope.isAdmin = false;
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
        //This section is for IT-role only, check details and delete Query/QueryParams (IT-role = 1)

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