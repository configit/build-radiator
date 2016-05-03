(function () {
  'use strict';

  var module = angular.module('BuildRadiator');

  module.controller('DashboardController', ['$http', '$scope', '$interval', '$log', 'TileConfiguration', 'BuildService', 'MessageService', function ($http, $scope, $interval, $log, TileConfiguration, BuildService, MessageService) {
    var self = this;
    var timer = null;

    self.committerLimit = 11;

    function refreshProject(err, tile, build) {
      if (err) {
        tile.error = err;
        $log.error(err);
        return;
      }

      delete tile.error;
      tile.project = build;
    }

    function refreshMessage(tile) {
      MessageService.get(tile.config.messageKey).then(function (message) {
        delete tile.error;
        tile.message = message;
      }, function (err) {
        tile.error = err;
        $log.error(err);
      });
    }

    function findBuild( builds, tile ) {
      return builds.find(function (b) {
        return tile.config.buildName === b.name && tile.config.branchName === b.branchName;
      });
    }

    function refreshAllProjects(err, builds, projectTiles) {
      if (err) {
        return projectTiles.forEach(function (tile) {
          refreshProject(err, tile);
        });
      }

      return projectTiles.forEach(function (tile) {
        var build = findBuild(builds, tile);

        if (build) {
          refreshProject(null, tile, build);
        }
        else {
          refreshProject({ message: "could not find build" }, tile);
        }
      });
    }

    function refresh() {
      self.lastUpdated = new Date();

      var projectTiles = self.tiles.filter(function (t) { return t.type === 'project'; });
      var projectConfigurations = projectTiles.map(function (t) { return t.config; });
      BuildService.getAll(projectConfigurations).then(function (builds) {
        refreshAllProjects(null, builds, projectTiles);
      }, function (err) {
        refreshAllProjects(err, null, projectTiles);
      }
      );

      var messageTiles = self.tiles.filter(function (t) { return t.type === 'message'; });
      messageTiles.forEach(function (messageTile) {
        refreshMessage(messageTile);
      });
    }

    function startRefreshTimer() {
      refresh();
      timer = $interval(refresh, 30000);
    }

    $scope.$on('$destroy', function () {
      $interval.cancel(timer);
    });

    TileConfiguration.get().then(function (tiles) {
      self.tiles = tiles;
      startRefreshTimer();
    });
  }]);

})();