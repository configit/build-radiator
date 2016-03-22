( function() {
  'use strict';

  var module = angular.module( 'BuildRadiator' );
  
  module.directive( 'buildStatusIcon', function() {
    return {
      restrict: 'E',
      scope: {
        status: '='
      },
      template: '<md-icon md-svg-src="Content/icons/{{ status }}.svg" alt="{{ status }}"></md-icon>'
    };
  } );

} )();