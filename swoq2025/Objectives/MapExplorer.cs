namespace swoq2025.Objectives;

public class MapExplorer : IObjective
{
    private readonly Map map;
    private readonly Player player;
    private readonly Router router;
    private Coord? target;
    private List<Coord> targetPath = [];
    

    public bool IsCompleted => GetUnknownTiles().Count == 0;

    public bool CanBeSolved => !IsCompleted;

    public bool HasToBeSolved => false;

    public int Priority => 0; // Lowest priority

    public MapExplorer(Map map, Player player)
    {
        this.map = map;
        this.player = player;
        router = new Router(map);
        target = null;
    }

    public bool TryGetNextTarget(out Coord nextTarget)
    {
        nextTarget = new(0, 0);
        if (target.HasValue)
        {
            if (!player.Position.IsNeighbor(target.Value))
            {
                targetPath = [];
            }
        }

        if (targetPath.Count != 0 && MapHasUpdatesOnPath(targetPath))
        {
            targetPath = [];
        }

        if (targetPath.Count != 0)
        {
            nextTarget = targetPath[0];
            targetPath.RemoveAt(0);
            return true;
        }

        var tiles = GetUnknownTiles();
        if (tiles.Count != 0)
        {
            target = tiles.First();
            targetPath = router.FindPath(player.Position, target.Value);
            if (targetPath.Count > 0)
            {
                nextTarget = targetPath[0];
                targetPath.RemoveAt(0);
                return true;
            }
            else
            {
                target = null;
                return false;
            }
        }
        else
        {
            target = null;
            return false;
        }
    }

    private bool MapHasUpdatesOnPath(IEnumerable<Coord> path)
    {
        foreach (var coord in path)
        {
            if (map.WasUpdated(coord.X, coord.Y))
            {
                return true;
            }
        }
        return false;
    }

    private List<Coord> GetUnknownTiles()
    {
        var tiles = new List<Coord>();
        for (int y = 0; y < map.Height; ++y)
        {
            for (int x = 0; x < map.Width; ++x)
            {
                Coord pos = new(x, y);
                if (map[x, y].Type == Swoq.Interface.Tile.Unknown && NeighborHasType(pos, Swoq.Interface.Tile.Empty))
                {
                    tiles.Add(pos);
                }
            }
        }

        tiles.Sort((a, b) =>
        {
            int distA = a.ManhattanDistance(player.Position);
            int distB = b.ManhattanDistance(player.Position);
            return distA.CompareTo(distB);
        });

        tiles = tiles.Where(i => router.FindPath(player.Position, i).Count > 0).ToList();

        return tiles;
    }

    private bool NeighborHasType(Coord pos, Swoq.Interface.Tile type)
    {
        if (pos.Y + 1 < map.Height && map[pos.X, pos.Y + 1].Type == type)
        {
            return true;
        }
        if (pos.X + 1 < map.Width && map[pos.X + 1, pos.Y].Type == type)
        {
            return true;
        }
        if (pos.Y - 1 >= 0 && map[pos.X, pos.Y - 1].Type == type)
        {
            return true;
        }
        if (pos.X - 1 >= 0 && map[pos.X - 1, pos.Y].Type == type)
        {
            return true;
        }
        return false;
    }
}
