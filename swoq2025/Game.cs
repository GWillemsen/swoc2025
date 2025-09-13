using System.Drawing;
using Swoq.Interface;

namespace swoq2025;

public class Game
{
    public string Id { get; }

    public int? Seed { get; }

    public Map Map { get; }

    public Player Player { get; set; } = new(new Coord(0, 0), new Swoq.Interface.Inventory());

    public int ViewDistance { get; }

    public GameStatus Status { get; set; } = GameStatus.Active;

    public int Level { get; set; } = 0;

    public int Tick { get; set; } = 0;

    public Game(string id, Map map, int? seed, int viewDistance)
    {
        Id = id;
        Map = map;
        Seed = seed;
        ViewDistance = viewDistance;
    }
}
