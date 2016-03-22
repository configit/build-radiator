( function() {
  'use strict';

  var module = angular.module( 'BuildRadiator' );
  
  module.factory( 'MessageService', ['$http', '$q', '$sce', function( $http, $q, $sce ) {
    return {
      get: function( messageKey ) {
        var deferred = $q.defer();

        $http.get( 'api/message/' + messageKey ).then( function( response ) {
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