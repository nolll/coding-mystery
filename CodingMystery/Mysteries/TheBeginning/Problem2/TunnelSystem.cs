using CodingMystery.Common.CoordinateSystems;

namespace CodingMystery.Mysteries.TheBeginning.Problem2;

internal class TunnelSystem
{
    public void Run()
    {
        var mapInput = FileReader.Read("/Mysteries/TheBeginning/Problem2/TunnelMap.txt");
        var matrix = MatrixBuilder.BuildCharMatrix(mapInput);

        var directionsInput = FileReader.ReadLines("/Mysteries/TheBeginning/Problem2/Directions.txt");
        var directions = string.Join(',', directionsInput).Replace(" ", "").Replace(",", "").ToCharArray();

        var hero = new MatrixCoord(3, 21);

        matrix.MoveTo(0, 0);
        const char space = ' ';
        matrix.MoveTo(hero);
        matrix.WriteValue(space);

        foreach (var direction in directions)
        {
            if (direction == 'N')
            {
                matrix.MoveUp();
                if (matrix.ReadValue() != space)
                    matrix.MoveDown();
            }

            if (direction == 'E')
            {
                matrix.MoveRight();
                if (matrix.ReadValue() != space)
                    matrix.MoveLeft();
            }

            if (direction == 'S')
            {
                matrix.MoveDown();
                if (matrix.ReadValue() != space)
                    matrix.MoveUp();
            }

            if (direction == 'W')
            {
                matrix.MoveLeft();
                if (matrix.ReadValue() != space)
                    matrix.MoveRight();
            }
        }

        Console.WriteLine($"({matrix.CurrentCoord.X}, {matrix.CurrentCoord.Y})");
    }
}