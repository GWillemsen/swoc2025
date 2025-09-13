namespace swoq2025;

public class Router
{
    private readonly Map _map;

    public Router(Map map)
    {
        _map = map;
    }

    public List<Coord> FindPath(Coord from, Coord target)
    {
        int width = _map.Width;
        int height = _map.Height;
        int total = width * height;

        var openSet = new PriorityQueue<Coord, int>();
        var openSetHash = new HashSet<Coord>();
        var closedSet = new HashSet<Coord>();

        // Arrays for fast access
        var gScore = new int[total];
        var fScore = new int[total];
        var cameFrom = new Coord?[total];
        for (int i = 0; i < total; ++i) { gScore[i] = int.MaxValue; fScore[i] = int.MaxValue; cameFrom[i] = null; }

        int Index(Coord c) => c.Y * width + c.X;

        gScore[Index(from)] = 0;
        fScore[Index(from)] = Heuristic(from, target);
        openSet.Enqueue(from, fScore[Index(from)]);
        openSetHash.Add(from);

        while (openSet.Count > 0)
        {
            var current = openSet.Dequeue();
            openSetHash.Remove(current);

            if (current.Equals(target))
            {
                var path = new List<Coord>();
                int idx = Index(current);
                while (cameFrom[idx].HasValue)
                {
                    path.Add(current);
                    current = cameFrom[idx].Value;
                    idx = Index(current);
                }
                path.Reverse();
                return path;
            }

            closedSet.Add(current);

            foreach (var neighbor in GetNeighbors(current))
            {
                int nIdx = Index(neighbor);
                if (closedSet.Contains(neighbor)) continue;
                if (IsBlocked(neighbor.X, neighbor.Y) && !neighbor.Equals(target)) continue;

                int tentativeGScore = gScore[Index(current)] + 1;
                if (tentativeGScore < gScore[nIdx])
                {
                    cameFrom[nIdx] = current;
                    gScore[nIdx] = tentativeGScore;
                    fScore[nIdx] = tentativeGScore + Heuristic(neighbor, target);
                    if (!openSetHash.Contains(neighbor))
                    {
                        openSet.Enqueue(neighbor, fScore[nIdx]);
                        openSetHash.Add(neighbor);
                    }
                }
            }

            // Early exit: if openSet is very large, assume target is unreachable
            if (openSet.Count > 2000) // tune this threshold as needed
            {
                return [];
            }
        }

        return []; // No path found
    }

    private int Heuristic(Coord a, Coord b)
    {
        return a.ManhattanDistance(b);
    }

    private IEnumerable<Coord> GetNeighbors(Coord coord)
    {
        var directions = new[] { (0, 1), (1, 0), (0, -1), (-1, 0) };
        foreach (var (dx, dy) in directions)
        {
            int nx = coord.X + dx;
            int ny = coord.Y + dy;
            if (nx >= 0 && ny >= 0 && nx < _map.Width && ny < _map.Height)
                yield return new Coord(nx, ny);
        }
    }

    private bool IsBlocked(int x, int y)
    {
        var type = _map[new Coord(x, y)].Type;
        return
            type == Swoq.Interface.Tile.Wall || type == Swoq.Interface.Tile.Exit ||
            type == Swoq.Interface.Tile.Player || type == Swoq.Interface.Tile.DoorBlue ||
            type == Swoq.Interface.Tile.DoorGreen || type == Swoq.Interface.Tile.DoorRed;
    }
}