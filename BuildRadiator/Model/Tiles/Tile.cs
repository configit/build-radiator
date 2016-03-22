namespace BuildRadiator.Model {
  public abstract class Tile {
    public abstract string Type { get; }

    public string Caption { get; set; }
    public int ColumnSpan { get; set; }
    public int RowSpan { get; set; }

    protected Tile() {
      ColumnSpan = 1;
      RowSpan = 1;
    }
  }

  public abstract class Tile<TConfig>: Tile {
    public TConfig Config { get; set; }
  }
}