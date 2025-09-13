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
        var openSet = new PriorityQueue<Coord, int>();
        var cameFrom = new Dictionary<Coord, Coord>();
        var gScore = new Dictionary<Coord, int>();
        var fScore = new Dictionary<Coord, int>();

        gScore[from] = 0;
        fScore[from] = Heuristic(from, target);
        openSet.Enqueue(from, fScore[from]);

        while (openSet.Count > 0)
        {
            var current = openSet.Dequeue();

            if (current.Equals(target))
            {
                var path = new List<Coord>();
                while (cameFrom.ContainsKey(current))
                {
                    path.Add(current);
                    current = cameFrom[current];
                }
                path.Reverse();
                return path;
            }

            foreach (var neighbor in GetNeighbors(current))
            {
                if (IsBlocked(neighbor.X, neighbor.Y) && !neighbor.Equals(target))
                    continue;

                int tentativeGScore = gScore[current] + 1;
                if (!gScore.ContainsKey(neighbor) || tentativeGScore < gScore[neighbor])
                {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = tentativeGScore + Heuristic(neighbor, target);
                    if (!openSet.UnorderedItems.Any(x => x.Element.Equals(neighbor)))
                        openSet.Enqueue(neighbor, fScore[neighbor]);
                }
            }
        }

        return []; // No path found
    }

    private int Heuristic(Coord a, Coord b)
    {
        // Manhattan distance
        return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
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