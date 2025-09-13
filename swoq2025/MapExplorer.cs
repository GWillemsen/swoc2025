namespace swoq2025;

public class MapExplorer
{
    private readonly Map map;
    private readonly Game game;
    private readonly Router router;
    private Coord? target;
    private List<Coord> targetPath = [];

    public MapExplorer(Map map, Game game)
    {
        this.map = map;
        this.game = game;
        router = new Router(map);
        target = null;
    }

    public bool TryGetNextTarget(out Coord nextTarget)
    {
        nextTarget = new(0, 0);
        if (target.HasValue)
        {
            if (!game.PlayerPosition.IsNeighbor(target.Value))
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
            targetPath = router.FindPath(game.PlayerPosition, target.Value);
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
            int distA = a.ManhattanDistance(game.PlayerPosition);
            int distB = b.ManhattanDistance(game.PlayerPosition);
            return distA.CompareTo(distB);
        });

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
