namespace swoq2025.Objectives;

public class MapExplorer : IObjective
{
    private readonly Map map;
    private readonly Player player;
    private readonly Router router;
    private readonly UnknownTileFilter unknownTileFilter;
    private Coord? target;
    private List<Coord> targetPath = [];
    

    public bool IsCompleted => unknownTileFilter.GetUnknownTiles(player.Position).Count == 0;

    public bool CanBeSolved => !IsCompleted;

    public bool HasToBeSolved => false;

    public int Priority => 0; // Lowest priority

    public MapExplorer(Map map, Player player)
    {
        this.map = map;
        this.player = player;
        router = new Router(map);
        target = null;
        unknownTileFilter = new UnknownTileFilter(map);
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

        var tiles = unknownTileFilter.GetUnknownTiles(player.Position);
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

    public void Reset()
    {
        target = null;
        targetPath = [];
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

}
