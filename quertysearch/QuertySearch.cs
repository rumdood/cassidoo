namespace quertysearch;

public static class QuertySearch
{
    private record Coordinate(int Row, int Column)
    {
        public int Row = Row;
        public int Column = Column;
    }
    
    public static readonly char[][] Keys = {
        new[] { 'q', 'w', 'e', 'r', 't', 'y', 'u', 'i', 'o', 'p' },
        new[] { 'a', 's', 'd', 'f', 'g', 'h', 'j', 'k', 'l'},
        new[] { 'z', 'x', 'c', 'v', 'b', 'n', 'm' }
    };

    private static readonly Dictionary<char, Coordinate> KeyMap = new();

    private const string Down = "down";
    private const string Up = "up";
    private const string Left = "left";
    private const string Right = "right";
    private const string Select = "select";

    public static void Main()
    {
        while (true)
        {
            Console.Write("Enter a word or enter ! to exit: ");
            var word = Console.ReadLine();

            if (word == "!")
            {
                return;
            }

            if (string.IsNullOrEmpty(word) || word.Any(x => !char.IsLetter(x)))
            {
                Console.WriteLine("Dude, just a word with letters please");
                continue;
            }

            Console.WriteLine(GetControlUsingDictionary(word));
        }
    }

    private static string GetControlUsingDictionary(string word)
    {
        var output = new List<string>();
        var current = new Coordinate(0, 0);
        
        if (KeyMap.Count == 0)
        {
            for (var rowIndex = 0; rowIndex < Keys.Length; rowIndex++)
            {
                for (var columnIndex = 0; columnIndex < Keys[rowIndex].Length; columnIndex++)
                {
                    KeyMap[Keys[rowIndex][columnIndex]] = new Coordinate(rowIndex, columnIndex);
                }
            }
        }

        foreach (var c in word.ToLower())
        {
            if (!char.IsLetter(c))
            {
                throw new InvalidOperationException("Gotta be a letter, dude");
            }

            var target = KeyMap[c];
            output.AddRange(GetControlsBetweenCoordinates(current, target));
            current = target;
        }

        return string.Join(", ", output);
    }

    private static IEnumerable<string> GetControlsBetweenCoordinates(Coordinate start, Coordinate end)
    {
        var verticalDifference = end.Row - start.Row;
        var horizontalDifference = end.Column - start.Column;

        // this grouping allows for an exact match of the test case (see readme)
        while (verticalDifference > 0)
        {
            yield return Down;
            verticalDifference--;
        }
        
        while (horizontalDifference < 0)
        {
            yield return Left;
            horizontalDifference++;
        }

        while (verticalDifference < 0)
        {
            yield return Up;
            verticalDifference++;
        }

        while (horizontalDifference > 0)
        {
            yield return Right;
            horizontalDifference--;
        }

        yield return Select;
    }
}