namespace BuildRadiator.Model {
  public class ClockTile: Tile<ClockTileConfig> {
    public override string Type {
      get { return "clock"; }
    }

    public ClockTile( string caption, string timezone ) {
      Caption = caption;
      Config = new ClockTileConfig {
        Timezone = timezone
      };
    }
  }
}