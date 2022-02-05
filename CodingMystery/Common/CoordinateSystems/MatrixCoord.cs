using System.Diagnostics;

namespace CodingMystery.Common.CoordinateSystems;

[DebuggerDisplay("{X},{Y}")]
public class MatrixCoord : IEquatable<MatrixCoord>
{
    private string? _id;

    public int X { get; }
    public int Y { get; }
    public string Id => _id ??= $"{X},{Y}";

    public MatrixCoord(int x, int y)
    {
        X = x;
        Y = y;
    }

    public int ManhattanDistanceTo(MatrixCoord other)
    {
        var xMax = Math.Max(X, other.X);
        var xMin = Math.Min(X, other.X);
        var xDiff = xMax - xMin;

        var yMax = Math.Max(Y, other.Y);
        var yMin = Math.Min(Y, other.Y);
        var yDiff = yMax - yMin;

        return xDiff + yDiff;
    }

    public bool Equals(MatrixCoord? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return X == other.X && Y == other.Y;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((MatrixCoord)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }
}