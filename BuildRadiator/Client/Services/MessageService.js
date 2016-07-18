( function() {
  'use strict';

  var module = angular.module( 'BuildRadiator' );

  module.factory( 'MessageService', ['$http', '$location', '$q', '$sce', function( $http, $location, $q, $sce ) {
    return {
      get: function( messageKey ) {
        var url = 'api/message/' + messageKey;

        var conf = $location.search();
        if ( conf && conf.conf ) {
          url += '?confId=' + conf.conf;
        }

        var deferred = $q.defer();

        $http.get( url ).then( function( response ) {
          var message = response.data;
          message.contentHtml = $sce.trustAsHtml( message.content );
          deferred.resolve( message );
        }, function( error ) {
          deferred.reject( error );
        } );

        return deferred.promise;
      }
    };
  }] );
} )();