static int CalculateScore(IEnumerable<int> roll)
{
    // toto je vypůjčený nápad :-)
    var counts = roll.GroupBy(d => d).ToDictionary(g => g.Key, g => g.Count());
    int totalScore = 0;

    // triples
    foreach (var count in counts)
    {
        if (count.Value >= 3)
        {

            if (count.Key == 1)
            {
                totalScore = 1000;
            }
            else
            {
                totalScore = count.Key * 100;
            }

            counts[count.Key] -= 3;
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
