using System.Diagnostics;

namespace swoq2025;

[DebuggerDisplay("({X}, {Y})")]
public struct Coord(int x, int y) : IEquatable<Coord>
{
    public int X = x;
    public int Y = y;

    public override string ToString()
    {
        return $"({X}, {Y})";
    }

    public bool Equals(Coord other)
    {
        return X == other.X && Y == other.Y;
    }

    public override bool Equals(object? obj)
    {
        return obj is Coord other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }

    public static bool operator ==(Coord left, Coord right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Coord left, Coord right)
    {
        return !(left == right);
    }

    public bool IsNeighbor(Coord test)
    {
        return Math.Abs(X - test.X) == 1 || Math.Abs(Y - test.Y) == 1;
    }

    public int ManhattanDistance(Coord other)
    {
        return Math.Abs(X - other.X) + Math.Abs(Y - other.Y);
    }
}