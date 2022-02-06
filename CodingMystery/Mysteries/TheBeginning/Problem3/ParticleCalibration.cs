using CodingMystery.Common.CoordinateSystems;

namespace CodingMystery.Mysteries.TheBeginning.Problem3;

internal class ParticleCalibration
{
    public void Run()
    {
        var input = FileReader.Read("/Mysteries/TheBeginning/Problem3/ParticleGrid.txt");
        var matrix = MatrixBuilder.BuildCharMatrix(input);

        var center = new MatrixCoord(51, 26);
        var totalDistance = 0;
        for (var y = 0; y < matrix.Height; y++)
        {
            for (var x = 0; x < matrix.Width; x++)
            {
                var coord = new MatrixCoord(x, y);
                matrix.MoveTo(coord);
                var v = matrix.ReadValue();
                if (v == '•')
                {
                    totalDistance += coord.ManhattanDistanceTo(center);
                }
            }
        }

        Console.WriteLine(totalDistance);
    }
}