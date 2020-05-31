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