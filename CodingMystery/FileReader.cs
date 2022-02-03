using System.Text;

namespace CodingMystery;

internal static class FileReader
{
    public static string ReadFile(params string[] pathParts)
    {
        var parts = pathParts.ToList();
        parts.Insert(0, AppDomain.CurrentDomain.BaseDirectory);

        var filePath = Path.Combine(parts.ToArray());
        if (!File.Exists(filePath))
            throw new FileNotFoundException("File not found", filePath);

        return File.ReadAllText(filePath, Encoding.UTF8);
    }
}