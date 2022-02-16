/*
SAMPLE OUTPUT:
Welcome to Cassiboggle
Here is your board:
a a q t
x c w e
r l e p
What word would you like to find? (or type EXIT) ace
Searching for [ace]
* a q t
x * w e
r l * p
What word would you like to find? (or type EXIT)
*/

var board = new[]
{
    new[] { 'a', 'a', 'q', 't' },
    new[] { 'x', 'c', 'w', 'e' },
    new[] { 'r', 'l', 'e', 'p' }
};

Console.WriteLine("Welcome to Cassiboggle");
Console.WriteLine("Here is your board:");
DisplayBoard(board);
Console.WriteLine();

while (true)
{
    Console.Write("What word would you like to find? (or type EXIT) ");

    var word = Console.ReadLine();

    if (word.Equals("EXIT", StringComparison.OrdinalIgnoreCase))
    {
        break;
    }
    
    Console.WriteLine("Searching for [{0}]", word);
    DisplayBoard(FindWord(board, word.ToLower()));
    Console.WriteLine();
}

Console.WriteLine("TOODALOO!");

void DisplayBoard(IReadOnlyList<char[]> board)
{
    foreach (var row in board)
    {
        foreach (var item in row)
        {
            Console.Write(item);
            Console.Write(' ');
        }

        Console.WriteLine();
    }
}

char[][] FindWord(IReadOnlyList<char[]> board, string word)
{
    // store the coordinates for each found character
    // this can be used to ensure we never double back
    // as well as to generate the output character map
    var usedPoints = new HashSet<(int Row, int Column)>();
    
    // copy the input to the output
    var output = new char[board.Count][];

    for (var row = 0; row < board.Count; row++)
    {
        output[row] = new char[board[row].Length];
        for (var col = 0; col < board[row].Length; col++)
        {
            output[row][col] = board[row][col];
        }
    }
    
    // begin the great search
    for (var rowIndex = 0; rowIndex < board.Count; rowIndex++)
    {
        for (var colIndex = 0; colIndex < board[rowIndex].Length; colIndex++)
        {
            if (board[rowIndex][colIndex] != word[0])
            {
                continue;
            }

            // set our coordinate data for the first letter
            var currentCoordinate = (rowIndex, colIndex);
            usedPoints.Clear();
            usedPoints.Add(currentCoordinate);
            
            // container for possible matches on the next letter
            var charCoordinates = new Dictionary<int, Stack<(int Row, int Column)>>();
            
            var currentCharIndex = 1;

            while (currentCharIndex > 0 && currentCharIndex < word.Length)
            {
                if (!charCoordinates.ContainsKey(currentCharIndex) || charCoordinates[currentCharIndex].Count == 0)
                {
                    var potentialCoordinates = GetCoordinatesOfCharacter(word[currentCharIndex], 
                        currentCoordinate, 
                        usedPoints, 
                        board);

                    charCoordinates[currentCharIndex] = new Stack<(int Row, int Column)>(potentialCoordinates);
                }
                
                if (charCoordinates[currentCharIndex].Count > 0)
                {
                    currentCoordinate = charCoordinates[currentCharIndex].Pop();
                    usedPoints.Add(currentCoordinate);
                    currentCharIndex++;
                }
                else
                {
                    usedPoints.Remove(currentCoordinate);
                    while (currentCharIndex > 0 && charCoordinates[currentCharIndex].Count == 0)
                    {
                        currentCharIndex--;
                    }
                }
            }

            if (currentCharIndex < word.Length)
            {
                continue;
            }
            
            foreach (var (row, column) in usedPoints)
            {
                output[row][column] = '*';
            }

            break;
        }
    }

    return output.ToArray();
}

IEnumerable<(int Row, int Column)> GetCoordinatesOfCharacter(
        char target,
        (int Row, int Column) startingPoint,
        IEnumerable<(int Row, int Column)> excludePoints,
        IReadOnlyList<IReadOnlyList<char>> board)
{
    var possibles = new List<(int Row, int Column)>();

    for (var x = startingPoint.Row-1; x < startingPoint.Row + 2; x++)
    {
        for (var y = startingPoint.Column - 1; y < startingPoint.Column + 2; y++)
        {
            if (y > -1 && y < board[0].Count && 
                x > -1 && x < board.Count &&
                !excludePoints.Contains((x, y)) &&
                (x != startingPoint.Row || y != startingPoint.Column))
            {
                possibles.Add((x, y));
            }
        }
    }

    foreach (var point in possibles)
    {
        if (board[point.Row][point.Column] != target)
        {
            continue;
        }

        yield return point;
    }
}