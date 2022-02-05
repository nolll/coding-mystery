using CodingMystery.Common.CoordinateSystems;

namespace CodingMystery.Mysteries.TheBeginning.Problem2;

internal class TunnelSystem
{
    public void Run()
    {
        var mapInput = FileReader.Read("/Mysteries/TheBeginning/Problem2/TunnelMap.txt");
        var matrix = MatrixBuilder.BuildCharMatrix(mapInput);

        var officeDoor = new MatrixCoord(0, 7);
        var hero = new MatrixCoord(3, 21);

        matrix.MoveTo(0, 0);
        var wall = matrix.ReadValue();
        const char space = ' ';
        matrix.MoveTo(officeDoor);
        matrix.WriteValue(wall);
        matrix.MoveTo(hero);
        matrix.WriteValue(space);

        Console.WriteLine(matrix.Print());
    }
}