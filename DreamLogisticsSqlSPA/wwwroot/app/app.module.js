var myApp = angular.module("myApp", [
    'ngRoute',
    'myControllers'
]);

myApp.config(['$routeProvider', function ($routeProvider) {
    $routeProvider
        .when('/', {
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