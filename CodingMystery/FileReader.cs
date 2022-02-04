using System.Text;

namespace CodingMystery;

internal static class FileReader
{
    public static string Read(string relativePath)
    {
        var parts = relativePath.TrimStart('/').Split('/').ToList();
        parts.Insert(0, AppDomain.CurrentDomain.BaseDirectory);

        var filePath = Path.Combine(parts.ToArray());
        if (!File.Exists(filePath))
            throw new FileNotFoundException("File not found", filePath);

        return File.ReadAllText(filePath, Encoding.UTF8);
    }

    public static IEnumerable<string> ReadLines(string relativePath)
    {
        var content = Read(relativePath);
        return content.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Where(o => o.Length > 0).ToList();
    }
}