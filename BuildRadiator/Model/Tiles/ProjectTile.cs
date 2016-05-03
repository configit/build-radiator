namespace BuildRadiator.Model {
  public class ProjectTile: Tile<BuildDefinition> {
    public override string Type {
      get { return "project"; }
    }

    public ProjectTile( string caption, string buildName, string branchName ) {
      Caption = caption;
      Config = new BuildDefinition {
        BuildName = buildName,
        BranchName = branchName
      };
    }
  }
}