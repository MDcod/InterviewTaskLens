namespace FlatsService.Extensions;

public static class RandomExtensions
{
    public static double NextDoubleInRange(this Random random, double start, double end) => random.NextDouble() * (end - start) + start;
}