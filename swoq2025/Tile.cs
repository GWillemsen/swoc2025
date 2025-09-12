using System.Diagnostics;

namespace swoq2025;

[DebuggerDisplay("{Type}")]
public class Tile
{
    public TileType Type { get; set; } = TileType.Unknown;
}
