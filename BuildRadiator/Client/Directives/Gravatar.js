( function() {
  'use strict';

  var module = angular.module( 'BuildRadiator' );
  
  module.directive( 'gravatar', function() {

    function buildGravatarUrl( email, size, type ) {
      var baseUrl = 'https://secure.gravatar.com/avatar/';
      type = type || 'retro';
      size = size || 80;

      if( !email ) {
        return baseUrl + '?d=' + type + '&f=y&s=' + size;
      }

      var hash = md5( email.trim().toLowerCase() );
      return baseUrl + hash + '?d=' + type + '&s=' + size;
    }

    return {
      restrict: 'E',
      scope: {
        email: '=',
        size: '=',
        type: '='
      },
      template: '<img ng-src="{{ gravatarUrl }}" width="{{ size }}" height="{{ size }}" alt="{{ email }}" />',
      link: function( scope ) {
        scope.$watchGroup( ['email', 'size'], function() {
          scope.gravatarUrl = buildGravatarUrl( scope.email, scope.size, scope.type );
        } );
      }
    };
  } );

} )();