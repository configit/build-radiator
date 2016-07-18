( function() {
  'use strict';

  angular.module( 'BuildRadiator', ['ngMaterial', 'ngMessages'] );

  angular.module( 'BuildRadiator' ).config( ['$httpProvider', '$locationProvider', '$provide', function( $httpProvider, $locationProvider, $provide ) {

    $locationProvider.html5Mode(true);

    $provide.factory( 'httpInterceptor', ['$window', '$location', '$q', function( $window, $location, $q ) {
      return {
        responseError: function( response ) {
          if ( response.status === 401 ) {
            $window.location = 'Login?return=' + encodeURIComponent( $location.absUrl() );
          }
          return $q.reject( response );
        }
      };
    }] );

    $httpProvider.interceptors.push( 'httpInterceptor' );
  }] );
} )();