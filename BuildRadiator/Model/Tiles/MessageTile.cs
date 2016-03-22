namespace BuildRadiator.Model {
  public class MessageTile: Tile<MessageTileConfig> {
    public override string Type {
      get { return "message"; }
    }

    public MessageTile( string caption, string messageKey, params string[] classes ) {
      Caption = caption;
      Config = new MessageTileConfig {
        MessageKey = messageKey,
        Classes = classes
      };
    }
  }
}