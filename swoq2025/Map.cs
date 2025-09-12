using Swoq.Interface;

namespace swoq2025;

public class Map
{
    private readonly List<Tile> _tiles = [];
    private List<bool> _updatedTiles = [];

    public int Width { get; }

    public int Height { get; }

    public Tile this[int x, int y] => _tiles[(y * Width) + x];

    public bool WasUpdated(int x, int y) => _updatedTiles[(y * Width) + x];

    public Map(int width, int height)
    {
        Width = width;
        Height = height;
        _tiles.Capacity = width * height;
        _updatedTiles.Capacity = width * height;
        for (int i = 0; i < width * height; ++i)
        {
            _tiles.Add(new Tile());
            _updatedTiles.Add(false);
        }
    }

    public Map(int width, IEnumerable<Tile> tiles)
    {
        Width = width;
        Height = tiles.Count() / width;
        _tiles = [.. tiles];
        _updatedTiles = Enumerable.Repeat(false, Width * Height).ToList();
    }

    public static Map FromState(int width, State state)
    {
        return new Map(width, state.PlayerState.Surroundings.Select(t => new Tile(t)));
    }

    public void Reset()
    {
        for (int i = 0; i < _tiles.Count; ++i)
        {
            _tiles[i].Type = Swoq.Interface.Tile.Unknown;
            _updatedTiles[i] = false;
        }
    }

    public void MergeMap(Map other, int x, int y)
    {
        for (int i = 0; i < _tiles.Count; ++i)
        {
            _updatedTiles[i] = false;
        }

        // Depend on int truncation
        int startX = x - ((other.Width - 1) / 2);
        int startY = y - ((other.Height - 1) / 2);

        for (int nY = 0; nY < other.Height; ++nY)
        {
            for (int nX = 0; nX < other.Width; ++nX)
            {
                int tX = startX + nX;
                int tY = startY + nY;
                if (tX < 0 || tX >= Width || tY < 0 || tY >= Height)
                {
                    continue;
                }

                Tile otherTile = other[nX, nY];
                int idx = (tY * Width) + tX;

                if (otherTile.Type != Swoq.Interface.Tile.Unknown)
                {
                    this[tX, tY].Type = otherTile.Type;
                    _updatedTiles[idx] = true;
                }
            }
        }
    }

    public List<Coord> FindPath(int startX, int startY, int endX, int endY)
    {
        var openSet = new SortedSet<(int, int, Coord)>(Comparer<(int, int, Coord)>.Create((a, b) => a.Item1 != b.Item1 ? a.Item1 - b.Item1 : a.Item2 - b.Item2));
        var cameFrom = new Dictionary<Coord, Coord>();
        var gScore = new Dictionary<Coord, int>();
        var hScore = new Dictionary<Coord, int>();
        var closedSet = new HashSet<Coord>();

        Coord start = new(startX, startY);
        Coord end = new(endX, endY);

        gScore[start] = 0;
        hScore[start] = Heuristic(start, end);
        openSet.Add((hScore[start], hScore[start], start));

        while (openSet.Count > 0)
        {
            var current = openSet.Min.Item3;
            if (current.X == end.X && current.Y == end.Y)
                return ReconstructPath(cameFrom, current);

            openSet.Remove(openSet.Min);
            closedSet.Add(current);

            foreach (var neighbor in GetNeighbors(current))
            {
                if (closedSet.Contains(neighbor) || IsBlocked(neighbor.X, neighbor.Y))
                    continue;

                int tentativeG = gScore[current] + 1;
                if (!gScore.ContainsKey(neighbor) || tentativeG < gScore[neighbor])
                {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeG;
                    hScore[neighbor] = Heuristic(neighbor, end);
                    openSet.Add((gScore[neighbor] + hScore[neighbor], hScore[neighbor], neighbor));
                }
            }
        }
        return new List<Coord>();
    }

    private int Heuristic(Coord a, Coord b) => Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);

    private List<Coord> ReconstructPath(Dictionary<Coord, Coord> cameFrom, Coord current)
    {
        var path = new List<Coord> { current };
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            path.Add(current);
        }
        path.Reverse();
        return path;
    }

    private IEnumerable<Coord> GetNeighbors(Coord c)
    {
        int[] dx = { 0, 1, 0, -1 };
        int[] dy = { -1, 0, 1, 0 };
        for (int i = 0; i < 4; ++i)
        {
            int nx = c.X + dx[i];
            int ny = c.Y + dy[i];
            if (nx >= 0 && nx < Width && ny >= 0 && ny < Height)
                yield return new Coord(nx, ny);
        }
    }

    private bool IsBlocked(int x, int y)
    {
        var type = this[x, y].Type;
        return type == Swoq.Interface.Tile.Wall || type == Swoq.Interface.Tile.Exit || type == Swoq.Interface.Tile.Player;
    }
}
