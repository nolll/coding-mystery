namespace CodingMystery.TheBeginning.Problem1;

internal class PaperShred
{
    private static Dictionary<char, int> Scores = new Dictionary<char, int>
    {
        { 'T', 0 },
        { 'I', 1 },
        { 'M', 2 },
        { 'E', 3 },
        { '-', 4 },
        { 'C', 5 },
        { 'O', 6 },
        { 'R', 7 },
        { 'P', 8 },
        { '▄', 9 },
        { '█', 10 },
        { '▀', 11 }
    };

    public string Text { get; }
    public bool IsBorder { get; }
    public int Indent { get; }
    public int Score { get; }

    public PaperShred(string text)
    {
        var parts = text.Split(' ');

        Text = text;
        IsBorder = text.StartsWith("███████████████████████");
        if (IsBorder)
            return;

        Indent = parts[0].Length;
        Score =  Scores[parts[1].First()];
    }
}