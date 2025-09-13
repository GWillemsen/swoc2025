using System.Collections;
using System.Text;
using Swoq.Interface;

namespace swoq2025;

public class Map : IEnumerable<Tile>
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

                if (otherTile.Type != Swoq.Interface.Tile.Unknown && otherTile.Type != this[tX, tY].Type)
                {
                    this[tX, tY].Type = otherTile.Type;
                    _updatedTiles[idx] = true;
                }
            }
        }
    }

    public string Dump()
    {
        StringBuilder builder = new();
        builder.Append("   ");
        for (int x = 0; x < Width; ++x)
        {
            builder.Append($"{x}".ToString().PadLeft(3, ' '));
        }
        builder.AppendLine();
        for (int y = 0; y < Height; ++y)
        {
            builder.Append($"{y}".ToString().PadLeft(3, ' '));
            for (int x = 0; x < Width; ++x)
            {
                char c;
                switch (this[x, y].Type)
                {
                    case Swoq.Interface.Tile.Unknown:
                        c = '?';
                        break;
                    case Swoq.Interface.Tile.Empty:
                        c = '.';
                        break;
                    case Swoq.Interface.Tile.Wall:
                        c = '#';
                        break;
                    case Swoq.Interface.Tile.Player:
                        c = 'P';
                        break;
                    case Swoq.Interface.Tile.Exit:
                        c = 'E';
                        break;
                    case Swoq.Interface.Tile.DoorBlue:
                        c = 'B';
                        break;
                    case Swoq.Interface.Tile.DoorGreen:
                        c = 'G';
                        break;
                    case Swoq.Interface.Tile.DoorRed:
                        c = 'R';
                        break;
                    case Swoq.Interface.Tile.KeyBlue:
                        c = 'b';
                        break;
                    case Swoq.Interface.Tile.KeyGreen:
                        c = 'g';
                        break;
                    case Swoq.Interface.Tile.KeyRed:
                        c = 'r';
                        break;
                    default:
                        c = ' ';
                        break;
                }
                builder.Append(c.ToString().PadLeft(3, ' '));
            }
            builder.AppendLine();
        }
        return builder.ToString();
    }

    public IEnumerator<Tile> GetEnumerator()
    {
        return _tiles.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
