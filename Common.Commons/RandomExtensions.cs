namespace Common.Commons;

public static class RandomExtensions
{
    public static double NextDouble(this Random random, double min, double max)
    {
        if (min >= max)
        {
            throw new ArgumentOutOfRangeException(nameof(min), "minValue cannot be bigger than maxValue");

        }

        double x = random.NextDouble();
        return x * max + (1 - x) * min;
    }
}