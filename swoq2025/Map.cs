using Swoq.Interface;

namespace swoq2025;

public class Map
{
    private readonly List<Tile> _tiles = [];

    public int Width { get; }

    public int Height { get; }

    public Tile this[int x, int y]
    {
        get
        {
            int index = (y * Width) + x;
            return _tiles[index];
        }
    }

    public Map(int width, int height)
    {
        Width = width;
        Height = height;
        _tiles.Capacity = width * height;
        for (int i = 0; i < width * height; ++i)
        {
            _tiles.Add(new Tile());
        }
    }

    public Map(int width, IEnumerable<Tile> tiles)
    {
        Width = width;
        Height = tiles.Count() / width;
        _tiles = [.. tiles];
    }

    public static Map FromState(int width, State state)
    {
        return new Map(width, state.PlayerState.Surroundings.Select(t => new Tile(t)));
    }

    public void MergeMap(Map other, int x, int y)
    {
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

                if (otherTile.Type != Swoq.Interface.Tile.Unknown)
                {
                    this[tX, tY].Type = otherTile.Type;
                }
            }
        }
    }
}
