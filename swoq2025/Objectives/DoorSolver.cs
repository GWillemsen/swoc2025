namespace swoq2025.Objectives;

using swoq2025;

public class DoorSolver : IObjective
{
    private readonly Map map;
    private readonly Player player;
    private readonly Router router;
    private readonly Swoq.Interface.Tile doorTile;
    private readonly Swoq.Interface.Tile keyTile;

    private List<Coord> path = [];
    private Coord? door = null;
    private Coord? key = null;

    public DoorSolver(Map map, Player player, Swoq.Interface.Tile doorTile, Swoq.Interface.Tile keyTile)
    {
        this.map = map;
        this.player = player;
        router = new Router(map);
        this.doorTile = doorTile;
        this.keyTile = keyTile;
    }

    public int Priority => 3;

    public bool IsCompleted
    {
        get
        {
            if (door.HasValue && key.HasValue)
            {
                return map[door.Value].Type != doorTile && map[key.Value].Type != keyTile;
            }
            return false;
        }
    }

    public bool CanBeSolved => GetDoor().HasValue && GetKey().HasValue && CanReachDoor() && CanReachKey();

    public bool HasToBeSolved => GetDoor().HasValue; // If it can be, it should be.

    public void Reset()
    {
        path = [];
        door = null;
        key = null;
    }

    public bool TryGetNextTarget(out Coord nextTarget, out bool use)
    {
        nextTarget = new(0, 0);
        use = false;

        if (!door.HasValue)
        {
            door = GetDoor();
        }

        if (!key.HasValue)
        {
            key = GetKey();
        }

        if (!door.HasValue || !key.HasValue)
        {
            // There is no door and key
            return false;
        }
        
        if (door.HasValue && !GetDoor().HasValue && key.HasValue && !GetKey().HasValue)
        {
            // We are done
            return false;
        }

        if (path.Count > 0)
        {
            path.RemoveAt(0);
        }

        if (path.Count == 0)
        {
            if (key.HasValue && GetKey().HasValue)
            {
                // Go to the key
                path = router.FindPath(player.Position, key.Value);
            }
            else if (door.HasValue)
            {
                // Go to the door
                path = router.FindPath(player.Position, door.Value);
            }
            else
            {
                return false;
            }
        }

        if (path.Count == 1)
        {
            use = door.HasValue && path[0] == door.Value;
        }

        nextTarget = path[0];
        return true;
    }

    private Coord? GetDoor()
    {
        for (int y = 0; y < map.Height; ++y)
        {
            for (int x = 0; x < map.Width; ++x)
            {
                if (map[x, y].Type == doorTile)
                {
                    return new Coord(x, y);
                }
            }
        }
        return null;
    }

    private Coord? GetKey()
    {
        for (int y = 0; y < map.Height; ++y)
        {
            for (int x = 0; x < map.Width; ++x)
            {
                if (map[x, y].Type == keyTile)
                {
                    return new Coord(x, y);
                }
            }
        }
        return null;
    }

    private bool CanReachDoor()
    {
        Coord? coord = GetDoor();
        if (coord.HasValue)
        {
            var pathToDoor = router.FindPath(player.Position, coord.Value);
            return pathToDoor.Count > 0;
        }
        return false;
    }

    private bool CanReachKey()
    {
        Coord? coord = GetKey();
        if (coord.HasValue)
        {
            var pathToKey = router.FindPath(player.Position, coord.Value);
            return pathToKey.Count > 0;
        }
        return false;
    }
}