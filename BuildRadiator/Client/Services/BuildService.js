( function() {
  'use strict';

  var module = angular.module( 'BuildRadiator' );

  module.factory( 'BuildService', ['$http', '$q', function( $http, $q ) {
    return {
      get: function( buildType, branchName ) {
        var deferred = $q.defer();

        $http.get( 'api/Build?buildType=' + encodeURIComponent( buildType ) + '&branchName=' + encodeURIComponent( branchName ) ).then( function( response ) {
          deferred.resolve( response.data );
        }, function( error ) {
          deferred.reject( error );
        } );

        return deferred.promise;
      },
      getAll: function ( builds ) {
        var deferred = $q.defer();
        var requestBody = builds;

        $http.post( 'api/Build', requestBody ).then(function (response) {
          deferred.resolve(response.data);
        }, function (error) {
          deferred.reject(error);
        });

        return deferred.promise;
      }
    };
  }] );
} )();