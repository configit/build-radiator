namespace BuildRadiator.Model {
  public class ProjectTile: Tile<ProjectTileConfig> {
    public override string Type {
      get { return "project"; }
    }

    public ProjectTile( string caption, string buildName, string branchName ) {
      Caption = caption;
      Config = new ProjectTileConfig {
        BuildName = buildName,
        BranchName = branchName
      };
    }
  }
}