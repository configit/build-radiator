( function() {
  'use strict';

  var module = angular.module( 'BuildRadiator' );
  
  module.factory( 'TileConfiguration', ['$q', '$http', function( $q, $http ) {
    return {
      get: function() {
        var deferred = $q.defer();

        $http.get( 'api/tile' ).then( function( response ) {
          deferred.resolve( response.data );
        }, function( error ) {
          deferred.reject( error );
        } );

        return deferred.promise;
      }
    };
  }] );
} )();