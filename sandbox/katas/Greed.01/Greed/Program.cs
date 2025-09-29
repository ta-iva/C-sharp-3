using System.Linq;

int CalculateScore(IEnumerable<int> roll)
{
    // toto je vypůjčený nápad :-)
    var counts = roll.GroupBy(d => d).ToDictionary(g => g.Key, g => g.Count());
    int totalScore = 0;

    // triples
    for (int value = 1; value <= 6; value++)
    {
        if (counts.TryGetValue(value, out int count) && count >= 3)
        {

            if (value == 1)
            {
                totalScore = 1000;
            }
            else
            {
                totalScore = value * 100;
            }

            counts[value] -= 3;
        }
    }

    // singles
    if (counts.TryGetValue(1, out int remainingOnes))
    {
        totalScore += remainingOnes * 100;
    }

    if (counts.TryGetValue(5, out int remainingFives))
    {
        totalScore += remainingFives * 50;
    }

    return totalScore;
}

int[] inputRoll = { 1, 1, 1, 5, 1 };
int userScore = CalculateScore(inputRoll);

string strRoll = string.Join(", ", inputRoll);
Console.WriteLine($"The best score based on a given roll ({strRoll}): {userScore}");
