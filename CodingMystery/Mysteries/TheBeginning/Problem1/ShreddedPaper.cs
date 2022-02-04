namespace CodingMystery.Mysteries.TheBeginning.Problem1;

internal class ShreddedPaper
{
    public void Run()
    {
        var lines = FileReader.ReadLines("/Mysteries/TheBeginning/Problem1/ShreddedPaper.txt");
        var shreds = lines.Select(o => new PaperShred(o));
        var contentShreds = shreds.Where(o => !o.IsBorder); 
        var sortedShreds = contentShreds.OrderBy(o => o.Indent).ThenBy(o => o.Score).ToList();

        var result = string.Join("\r\n", sortedShreds.Select(o => o.Text));

        //Console.WriteLine(result);
        Console.WriteLine($"Code: 143670892");
    }
}
