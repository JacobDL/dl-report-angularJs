(function () {
    'use strict';

    angular
        .module('app')
        .factory('dataService', ['$http', '$q', function ($http, $q) {
            var service = {};

            service.getQueries = function () {
                var deferred = $q.defer();
                $http.get('/Query/GetQueries').then(function (result) {
                    deferred.resolve(result.data);
                }, function () {
                        deferred.reject();
                });
                return deferred.promise;
            }
            return service;
        }]);
})();