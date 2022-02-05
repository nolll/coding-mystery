using System.Text;

namespace CodingMystery.Common.CoordinateSystems;

public class Matrix<T> where T : struct
{
    private readonly T _defaultValue;
    private readonly IList<IList<T>> _matrix;
    public MatrixDirection Direction { get; private set; }
    public MatrixCoord CurrentCoord { get; private set; }
    public MatrixCoord StartCoord { get; private set; }
    protected readonly int[] AdjacentDeltas = { -1, 0, 1 };
    public IList<T> Values => _matrix.SelectMany(x => x).ToList();
    public int Height => _matrix.Count;
    public int Width => _matrix.Any() ? _matrix[0].Count : 0;
    public bool IsAtTop => CurrentCoord.Y == 0;
    public bool IsAtRightEdge => CurrentCoord.X == Width - 1;
    public bool IsAtBottom => CurrentCoord.Y == Height - 1;
    public bool IsAtLeftEdge => CurrentCoord.X == 0;
    public MatrixCoord Center => new(Width / 2, Height / 2);

    public Matrix(int width = 1, int height = 1, T defaultValue = default)
    {
        _defaultValue = defaultValue;
        _matrix = BuildMatrix(width, height, defaultValue);
        CurrentCoord = new MatrixCoord(0, 0);
        StartCoord = new MatrixCoord(0, 0);
        Direction = MatrixDirection.Up;
    }

    public IList<MatrixCoord> Coords
    {
        get
        {
            var coords = new List<MatrixCoord>();
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    coords.Add(new MatrixCoord(x, y));
                }
            }
            return coords;
        }
    }

    public bool TryMoveTo(MatrixCoord coord)
    {
        return MoveTo(coord, false);
    }

    public bool MoveTo(MatrixCoord coord)
    {
        return MoveTo(coord, true);
    }

    private bool MoveTo(MatrixCoord coord, bool extend)
    {
        if (IsOutOfRange(coord))
        {
            if (extend)
                ExtendMatrix(coord);
            else
                return false;
        }

        var x = coord.X > 0 ? coord.X : 0;
        var y = coord.Y > 0 ? coord.Y : 0;
        CurrentCoord = new MatrixCoord(x, y);
        return true;
    }

    public bool TryMoveTo(int x, int y)
    {
        return MoveTo(new MatrixCoord(x, y), false);
    }

    public bool MoveTo(int x, int y)
    {
        return MoveTo(new MatrixCoord(x, y), true);
    }

    public bool TryMoveForward()
    {
        return MoveForward(false);
    }

    public bool MoveForward()
    {
        return MoveForward(true);
    }

    private bool MoveForward(bool extend)
    {
        return MoveTo(new MatrixCoord(CurrentCoord.X + Direction.X, CurrentCoord.Y + Direction.Y), extend);
    }

    public bool TryMoveBackward()
    {
        return MoveBackward(false);
    }

    public bool MoveBackward()
    {
        return MoveBackward(true);
    }

    private bool MoveBackward(bool extend)
    {
        return MoveTo(new MatrixCoord(CurrentCoord.X - Direction.X, CurrentCoord.Y - Direction.Y), extend);
    }

    public bool TryMoveUp(int steps = 1)
    {
        return MoveUp(steps, false);
    }

    public bool MoveUp(int steps = 1)
    {
        return MoveUp(steps, true);
    }

    private bool MoveUp(int steps, bool extend)
    {
        return MoveTo(new MatrixCoord(CurrentCoord.X, CurrentCoord.Y - steps), extend);
    }

    public bool TryMoveRight(int steps = 1)
    {
        return MoveRight(steps, false);
    }

    public bool MoveRight(int steps = 1)
    {
        return MoveRight(steps, true);
    }

    private bool MoveRight(int steps, bool extend)
    {
        return MoveTo(new MatrixCoord(CurrentCoord.X + steps, CurrentCoord.Y), extend);
    }

    public bool TryMoveDown(int steps = 1)
    {
        return MoveDown(steps, false);
    }

    public bool MoveDown(int steps = 1)
    {
        return MoveDown(steps, true);
    }

    private bool MoveDown(int steps, bool extend)
    {
        return MoveTo(new MatrixCoord(CurrentCoord.X, CurrentCoord.Y + steps), extend);
    }

    public bool TryMoveLeft(int steps = 1)
    {
        return MoveLeft(steps, false);
    }

    public bool MoveLeft(int steps = 1)
    {
        return MoveLeft(steps, true);
    }

    private bool MoveLeft(int steps, bool extend)
    {
        return MoveTo(new MatrixCoord(CurrentCoord.X - steps, CurrentCoord.Y), extend);
    }

    public MatrixDirection TurnLeft()
    {
        if (Direction.Equals(MatrixDirection.Up))
            return TurnTo(MatrixDirection.Left);
        if (Direction.Equals(MatrixDirection.Right))
            return TurnTo(MatrixDirection.Up);
        if (Direction.Equals(MatrixDirection.Down))
            return TurnTo(MatrixDirection.Right);
        return TurnTo(MatrixDirection.Down);
    }

    public MatrixDirection TurnRight()
    {
        if (Direction.Equals(MatrixDirection.Up))
            return TurnTo(MatrixDirection.Right);
        if (Direction.Equals(MatrixDirection.Right))
            return TurnTo(MatrixDirection.Down);
        if (Direction.Equals(MatrixDirection.Down))
            return TurnTo(MatrixDirection.Left);
        return TurnTo(MatrixDirection.Up);
    }

    public MatrixDirection TurnTo(MatrixDirection direction)
    {
        Direction = direction;
        return direction;
    }

    public string Print(bool markCurrentAddress = false, bool markStartAddress = false, T currentAddressMarker = default, T startAddressMarker = default, bool spacing = false)
    {
        var sb = new StringBuilder();
        var y = 0;
        foreach (var row in _matrix)
        {
            var x = 0;
            foreach (var o in row)
            {
                if (markCurrentAddress && x == CurrentCoord.X && y == CurrentCoord.Y)
                    sb.Append('D');
                else if (markStartAddress && x == StartCoord.X && y == StartCoord.Y)
                    sb.Append('S');
                else
                    sb.Append(o);

                if (spacing)
                    sb.Append(' ');

                x += 1;
            }

            sb.AppendLine();
            y += 1;
        }

        return sb.ToString().Trim();
    }

    public T ReadValue()
    {
        return ReadValueAt(CurrentCoord.X, CurrentCoord.Y);
    }

    public T ReadValueAt(MatrixCoord coord)
    {
        return ReadValueAt(coord.X, coord.Y);
    }

    public T ReadValueAt(int x, int y)
    {
        return _matrix[y][x];
    }

    public void WriteValue(T value)
    {
        _matrix[CurrentCoord.Y][CurrentCoord.X] = value;
    }

    public IList<MatrixCoord> FindAddresses(T value)
    {
        var addresses = new List<MatrixCoord>();
        for (var y = 0; y < _matrix.Count; y++)
        {
            for (var x = 0; x < _matrix.Count; x++)
            {
                var address = new MatrixCoord(x, y);
                MoveTo(address);
                var val = ReadValue();
                if (val.Equals(value))
                    addresses.Add(address);
            }
        }

        return addresses;
    }

    public bool IsOutOfRange(MatrixCoord coord)
    {
        return coord.Y >= Height ||
               coord.Y < 0 ||
               coord.X >= Width ||
               coord.X < 0;
    }

    public IList<T> PerpendicularAdjacentValues => PerpendicularAdjacentCoords.Select(ReadValueAt).ToList();
    public IList<MatrixCoord> PerpendicularAdjacentCoords => PossiblePerpendicularAdjacentCoords.Where(o => !IsOutOfRange(o)).ToList();

    private IEnumerable<MatrixCoord> PossiblePerpendicularAdjacentCoords =>
        new List<MatrixCoord>
        {
            new MatrixCoord(CurrentCoord.X, CurrentCoord.Y - 1),
            new MatrixCoord(CurrentCoord.X + 1, CurrentCoord.Y),
            new MatrixCoord(CurrentCoord.X, CurrentCoord.Y + 1),
            new MatrixCoord(CurrentCoord.X - 1, CurrentCoord.Y)
        };

    public IList<T> AllAdjacentValues => AllAdjacentCoords.Select(ReadValueAt).ToList();
    public IList<MatrixCoord> AllAdjacentCoords => AllPossibleAdjacentCoords.Where(o => !IsOutOfRange(o)).ToList();

    private IEnumerable<MatrixCoord> AllPossibleAdjacentCoords
    {
        get
        {
            foreach (var dy in AdjacentDeltas)
            {
                foreach (var dx in AdjacentDeltas)
                {
                    var coord = new MatrixCoord(CurrentCoord.X + dx, CurrentCoord.Y - dy);
                    if (!coord.Equals(CurrentCoord))
                        yield return coord;
                }
            }
        }
    }

    public Matrix<T> Copy()
    {
        var matrix = new Matrix<T>();
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                matrix.MoveTo(x, y);
                matrix.WriteValue(ReadValueAt(x, y));
            }
        }

        matrix.MoveTo(CurrentCoord);
        return matrix;
    }

    public Matrix<T> RotateLeft()
    {
        var newMatrix = new Matrix<T>(Height, Width, _defaultValue);
        var oy = 0;
        for (var ox = Width - 1; ox >= 0; ox--)
        {
            var nx = 0;
            for (var ny = 0; ny < Height; ny++)
            {
                MoveTo(ox, ny);
                newMatrix.MoveTo(nx, oy);
                newMatrix.WriteValue(ReadValue());
                nx++;
            }
            oy++;
        }
        return newMatrix;
    }

    public Matrix<T> RotateRight()
    {
        return RotateLeft().RotateLeft().RotateLeft();
    }

    public Matrix<T> Slice(MatrixCoord? from = null, MatrixCoord? to = null)
    {
        from ??= new MatrixCoord(0, 0);
        to ??= new MatrixCoord(Width - 1, Height - 1);
        var xNew = 0;
        var yNew = 0;
        var newMatrix = new Matrix<T>(defaultValue: _defaultValue);
        for (var y = from.Y; y <= to.Y; y++)
        {
            for (var x = from.X; x <= to.X; x++)
            {
                newMatrix.MoveTo(xNew, yNew);
                newMatrix.WriteValue(ReadValueAt(x, y));

                xNew++;
            }

            xNew = 0;
            yNew++;
        }
        return newMatrix;
    }

    public Matrix<T> Slice(MatrixCoord from, int width, int height)
    {
        var to = new MatrixCoord(from.X + width, from.Y + height);
        return Slice(from, to);
    }

    public Matrix<T> FlipVertical()
    {
        var width = Width;
        var height = Height;
        var newMatrix = new Matrix<T>(width, height, _defaultValue);
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                var ny = height - y - 1;
                newMatrix.MoveTo(x, ny);
                newMatrix.WriteValue(ReadValueAt(x, y));
            }
        }
        return newMatrix;
    }

    public Matrix<T> FlipHorizontal()
    {
        var width = Width;
        var height = Height;
        var newMatrix = new Matrix<T>(width, height, _defaultValue);
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                var nx = width - x - 1;
                newMatrix.MoveTo(nx, y);
                newMatrix.WriteValue(ReadValueAt(x, y));
            }
        }
        return newMatrix;
    }

    private IList<IList<T>> BuildMatrix(int width, int height, T defaultValue)
    {
        var matrix = new List<IList<T>>();
        for (var y = 0; y < height; y++)
        {
            var row = new List<T>();
            for (var x = 0; x < width; x++)
            {
                row.Add(defaultValue);
            }
            matrix.Add(row);
        }
        return matrix;
    }

    private void ExtendMatrix(MatrixCoord coord)
    {
        ExtendX(coord);
        ExtendY(coord);
    }

    private void ExtendX(MatrixCoord coord)
    {
        if (coord.X < 0)
            ExtendLeft(coord);
        ExtendRight(coord);
    }

    private void ExtendLeft(MatrixCoord coord)
    {
        AddCols(-coord.X, MatrixAddMode.Prepend);
        StartCoord = new MatrixCoord(StartCoord.X - coord.X, StartCoord.Y);
    }

    private void ExtendRight(MatrixCoord coord)
    {
        var extendBy = coord.X - (Width - 1);
        if (extendBy > 0)
            AddCols(extendBy, MatrixAddMode.Append);
    }

    private void ExtendY(MatrixCoord coord)
    {
        if (coord.Y < 0)
            ExtendTop(coord);
        ExtendBottom(coord);
    }

    private void ExtendTop(MatrixCoord coord)
    {
        AddRows(-coord.Y, MatrixAddMode.Prepend);
        StartCoord = new MatrixCoord(StartCoord.X, StartCoord.Y - coord.Y);
    }

    private void ExtendBottom(MatrixCoord coord)
    {
        var extendBy = coord.Y - (Height - 1);
        if (extendBy > 0)
            AddRows(extendBy, MatrixAddMode.Append);
    }

    private void AddRows(int numberOfRows, MatrixAddMode addMode)
    {
        var width = Width;
        for (var y = 0; y < numberOfRows; y++)
        {
            var row = new List<T>();
            for (var x = 0; x < width; x++)
            {
                row.Add(_defaultValue);
            }

            if (addMode == MatrixAddMode.Prepend)
                _matrix.Insert(0, row);
            else
                _matrix.Add(row);
        }
    }

    private void AddCols(int numberOfRows, MatrixAddMode addMode)
    {
        var height = Height;
        for (var y = 0; y < height; y++)
        {
            var row = _matrix[y];
            for (var x = 0; x < numberOfRows; x++)
            {
                if (addMode == MatrixAddMode.Prepend)
                    row.Insert(0, _defaultValue);
                else
                    row.Add(_defaultValue);
            }
        }
    }

    public void ExtendAllDirections(int steps = 1)
    {
        ExtendUp(steps);
        ExtendRight(steps);
        ExtendDown(steps);
        ExtendLeft(steps);
    }

    public void ExtendUp(int steps = 1)
    {
        AddRows(steps, MatrixAddMode.Prepend);
    }

    public void ExtendRight(int steps = 1)
    {
        AddCols(steps, MatrixAddMode.Append);
    }

    public void ExtendDown(int steps = 1)
    {
        AddRows(steps, MatrixAddMode.Append);
    }

    public void ExtendLeft(int steps = 1)
    {
        AddCols(steps, MatrixAddMode.Prepend);
    }
}