using System.Collections.Generic;
using Xunit;

namespace sequentializer;

public class SequenceTests
{
    [Theory]
    [InlineData(new[] {1, 9, 87, 3, 10, 4, 20, 2, 45}, 4)]
    [InlineData(new[] {36, 41, 56, 35, 91, 33, 34, 92, 43, 37, 42}, 5)]
    public void SequentializerFindsCorrectLength(int[] given, int expected)
    {
        var actual = Sequentialize(given);
        Assert.Equal(expected, actual);
    }

    private static int Sequentialize(IReadOnlyCollection<int> input)
    {
        var workspace = new HashSet<int>();
        var max = new List<int>();

        foreach (var value in input)
        {
            var current = new List<int>();
            current.Add(value);

            if (!workspace.Contains(value))
            {
                workspace.Add(value);
            }

            int lower = value;
            while (workspace.Contains(--lower))
            {
                current.Add(lower);
            }

            int higher = value;
            while (workspace.Contains(++higher))
            {
                current.Add(higher);
            }

            if (current.Count > max.Count)
            {
                max = current;
            }
        }

        return max.Count;
    }
}