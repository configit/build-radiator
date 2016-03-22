( function() {
  'use strict';

  var module = angular.module( 'BuildRadiator' );

  module.controller( 'DashboardController', ['$http', '$scope', '$interval', '$log', 'TileConfiguration', 'BuildService', 'MessageService', function( $http, $scope, $interval, $log, TileConfiguration, BuildService, MessageService ) {
    var self = this;
    var timer = null;
    
    self.committerLimit = 11;

    function refreshProject( tile ) {
      BuildService.get( tile.config.buildName, tile.config.branchName ).then( function( project ) {
        delete tile.error;
        tile.project = project;
      }, function( error ) {
        tile.error = error;
        $log.error( error );
      } );
    }

    function refreshMessage( tile ) {
      MessageService.get( tile.config.messageKey ).then( function( message ) {
        delete tile.error;
        tile.message = message;
      }, function( error ) {
        tile.error = error;
        $log.error( error );
      } );
    }

    function refresh() {
      self.lastUpdated = new Date();
      angular.forEach( self.tiles, function( tile ) {
        switch ( tile.type ) {
          case 'project':
            refreshProject( tile );
            break;
          case 'message':
            refreshMessage( tile );
            break;
        }
      } );
    }

    function startRefreshTimer() {
      refresh();
      timer = $interval( refresh, 10000 );
    }

    $scope.$on( '$destroy', function() {
      $interval.cancel( timer );
    } );

    TileConfiguration.get().then( function( tiles ) {
      self.tiles = tiles;
      startRefreshTimer();
    } );
  }] );

} )();