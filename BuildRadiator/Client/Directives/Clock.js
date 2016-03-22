( function() {
  'use strict';

  var module = angular.module( 'BuildRadiator' );
  
  module.directive( 'clock', ['$interval', function( $interval ) {
    return {
      restrict: 'E',
      scope: {
        timezone: '='
      },
      template: '<div>{{ time }}</div><div class="small">{{ date }}</div>',
      link: function( scope ) { 
        function refresh() {
          var now = new moment().tz( scope.timezone );
          scope.time = now.format( 'HH:mm' );
          scope.date = now.format( 'DD MMMM YYYY' );
        }

        var timer = $interval( refresh, 1000 );

        scope.$on( '$destroy', function() {
          $interval.cancel( timer );
        } );

        refresh();
      }
    };
  }] );

} )();