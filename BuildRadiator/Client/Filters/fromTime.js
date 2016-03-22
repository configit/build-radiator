( function( angular ) {
  'use strict';

  var module = angular.module( 'BuildRadiator' );

  module.filter( 'fromTime', function() {
    return function( input, time ) {
      return moment( input ).from( time, true );
    };
  } );

} )( window.angular );