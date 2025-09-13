using System.Diagnostics;

namespace swoq2025;

[DebuggerDisplay("({X}, {Y})")]
public struct Coord : IEquatable<Coord>
{
    public int X;
    public int Y;

    public Coord(int x, int y)
    {
        X = x;
        Y = y;
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
}