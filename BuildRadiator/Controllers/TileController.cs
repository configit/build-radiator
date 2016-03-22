using System.Collections.Generic;
using System.Web.Http;
using BuildRadiator.Model;

namespace BuildRadiator.Controllers {
  public class TileController: ApiController {
    private static readonly IReadOnlyCollection<Tile> Tiles;

    static TileController() {
      Tiles = new Tile[] {
        new ProjectTile( "Ace Commit", "Ace Commit", "master" ) { ColumnSpan = 2, RowSpan = 2 },
        new MessageTile( "Current Theme", "sprintTheme", "fancy" ) { ColumnSpan = 2 }, 
        new ClockTile( "UK Time", "Europe/London" ),
        new ProjectTile( "Ngyn Commit", "Ngyn Commit", "master" ),
        new ProjectTile( "Vcdb Commit", "Vcdb Commit", "master" ),
        new ProjectTile( "Installer Commit", "Installer Commit", "master" ),
        new ProjectTile( "Grid Commit", "Grid Commit", "master" ),
        new ProjectTile( "Base Commit", "Base Commit", "master" ),
        new ProjectTile( "Ace PRFF", "Ace Commit", "prff" ),
        new ProjectTile( "Ace PRFF Deploy", "Ace Daily Deploy (PRFF)", "prff" ),
        new ProjectTile( "Ace MVFF Deploy", "Ace Daily Deploy (MVFF)", "mvff" ),
        new ProjectTile( "Ace Daily", "Ace Daily Deploy", "master" ),
        new ProjectTile( "Ace End To End", "Ace End To End Test", "master" ),
        new ProjectTile( "Ace Upgrade", "Ace Daily Upgrade", "master" ),
      };
    }

    public IEnumerable<Tile> Get() {
      return Tiles;
    }
  }
}