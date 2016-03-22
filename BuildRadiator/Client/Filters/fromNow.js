( function( angular ) {
  'use strict';

  var module = angular.module( 'BuildRadiator' );

  module.filter( 'fromNow', function() {
    return function( input ) {
      return input && moment( input ).fromNow();
    };
  } );

} )( window.angular );