( function() {
  'use strict';

  var module = angular.module( 'BuildRadiator' );

  module.factory( 'TileConfiguration', ['$q', '$http', '$location', function( $q, $http, $location ) {
    return {
      get: function () {
        var query = $location.search();
        var url = 'api/tile';
        console.log( query, query.conf );
        if ( query && query["conf"] ) {
          url += '/' + query["conf"];
        }

        var deferred = $q.defer();
        $http.get( url ).then( function( response ) {
          deferred.resolve( response.data );
        }, function( error ) {
          deferred.reject( error );
        } );

        return deferred.promise;
      }
    };
  }] );
} )();