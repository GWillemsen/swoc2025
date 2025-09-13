namespace swoq2025.Objectives;

public class ExitFinder : IObjective
{
    private readonly Map map;
    private readonly Player player;
    private readonly Router router;
    private Coord? exit;

    public bool IsCompleted => exit.HasValue && !map.Any(t => t.Type == Swoq.Interface.Tile.Exit);

    public bool CanBeSolved => map.Any(t => t.Type == Swoq.Interface.Tile.Exit);

    public bool HasToBeSolved => true;

    public int Priority => 1; // Higher priority than exploration

    public ExitFinder(Map map, Player player)
    {
        this.map = map;
        this.player = player;
        router = new Router(map);
        exit = null;
    }

    public bool TryGetNextTarget(out Coord target, out bool use)
    {
        target = new(0, 0);
        use = false;
        if (exit.HasValue)
        {
            target = exit.Value;
            return true;
        }

        // Scan the map for an exit
        for (int y = 0; y < map.Height; ++y)
        {
            for (int x = 0; x < map.Width; ++x)
            {
                if (map[x, y].Type == Swoq.Interface.Tile.Exit)
                {
                    exit = new Coord(x, y);
                    break;
                }
            }
        }

        if (exit.HasValue)
        {
            var path = router.FindPath(player.Position, exit.Value);
            if (path.Count == 0)
            {
                // Exit is not reachable
                exit = null;
                return false;
            }
            target = path.First();
            return true;
        }

        return false;
    }

    public void Reset()
    {
        exit = null;
    }
}