namespace CodingMystery.Common.CoordinateSystems;

public static class MatrixBuilder
{
    public static Matrix<char> BuildCharMatrix(string input, char defaultValue = default)
    {
        var matrix = new Matrix<char>(1, 1, defaultValue);
        var rows = input.Trim().Split('\n');
        var y = 0;
        foreach (var row in rows)
        {
            var x = 0;
            var chars = row.Trim().ToCharArray();
            foreach (var c in chars)
            {
                matrix.MoveTo(x, y);
                matrix.WriteValue(c);
                x += 1;
            }

            y += 1;
        }

        return matrix;
    }
}