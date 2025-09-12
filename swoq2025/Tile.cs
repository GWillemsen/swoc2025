using System.Diagnostics;

namespace swoq2025;

[DebuggerDisplay("{Type}")]
public class Tile
{
    public Swoq.Interface.Tile Type { get; set; } = Swoq.Interface.Tile.Unknown;
}
