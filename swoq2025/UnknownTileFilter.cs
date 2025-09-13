namespace swoq2025;

public class UnknownTileFilter
{
    private readonly Map map;
    private readonly Router router;

    public UnknownTileFilter(Map map)
    {
        this.map = map;
        router = new Router(map);
    }

    public List<Coord> GetUnknownTiles(Coord playerPosition)
    {
        List<Coord> tiles = [];
        for (int y = 0; y < map.Height; ++y)
        {
            for (int x = 0; x < map.Width; ++x)
            {
                Coord coord = new(x, y);
                if (map[x, y].Type == Swoq.Interface.Tile.Unknown && NeighborHasType(coord, Swoq.Interface.Tile.Empty))
                {
                    tiles.Add(coord);
                }
            }
        }

        // Sort by distance to player
        tiles.Sort((a, b) => {
            int distA = Math.Abs(a.X - playerPosition.X) + Math.Abs(a.Y - playerPosition.Y);
            int distB = Math.Abs(b.X - playerPosition.X) + Math.Abs(b.Y - playerPosition.Y);
            return distA.CompareTo(distB);
        });

        tiles = tiles.Where(i => router.FindPath(playerPosition, i).Count > 0).ToList();

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
