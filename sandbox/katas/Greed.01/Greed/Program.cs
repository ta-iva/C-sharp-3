// Total score based on basic rules
static int CalculateScore(IEnumerable<int> roll)
{
    // zgrupování je vypůjčený nápad :-)
    var counts = roll.GroupBy(d => d).ToDictionary(g => g.Key, g => g.Count());
    int totalScore = 0;

    // triple
    var triple = counts.FirstOrDefault(x => x.Value >= 3);

    if (triple.Value >= 3)
    {
        totalScore = (triple.Key == 1) ? 1000 : triple.Key * 100;
        counts[triple.Key] -= 3;
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



// Total score based on extra credit rules
static int CalculateScoreExtra(IEnumerable<int> roll)
{
    var counts = roll.GroupBy(d => d).ToDictionary(g => g.Key, g => g.Count());
    int totalScore = 0;

    // straight
    if (counts.Count == 6)
    {
        totalScore = 1200;
    }

    // three pairs
    else if ((counts.Count == 3) && counts.All(x => x.Value == 2))
    {
        totalScore = 800;
    }

    // two triples
    else if (counts.Count == 2 && counts.All(x => x.Value == 3))
    {
        totalScore = counts.Sum(i => i.Key == 1 ? i.Key * 1000 : i.Key * 100);
    }

    else
    {
        // triple+
        var triple = counts.FirstOrDefault(x => x.Value >= 3);

        if (triple.Value >= 3)
        {
            int interScore = (triple.Key == 1) ? 1000 : triple.Key * 100;
            totalScore = interScore * (int)Math.Pow(2, triple.Value - 3);
            counts[triple.Key] = 0;
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
    }

    return totalScore;
}


// input w/o validation (5 values in case of basic rules, 1-6 values for extra credit)
int[] inputRoll = { 1, 1, 1, 5, 1 };
int userScore = CalculateScoreExtra(inputRoll);

string strRoll = string.Join(", ", inputRoll);
Console.WriteLine($"The best score based on a given roll ({strRoll}): {userScore}");
