namespace swoq2025;

public class Game
{
    public int Id { get; }

    public int? Seed { get; }

    public Map Map { get; }

    public Game(int id, Map map, int? seed)
    {
        Id = id;
        Map = map;
        Seed = seed;
    }
}
