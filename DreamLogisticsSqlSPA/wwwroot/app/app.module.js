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
            console.log(token);
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



