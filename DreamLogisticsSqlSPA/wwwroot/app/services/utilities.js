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