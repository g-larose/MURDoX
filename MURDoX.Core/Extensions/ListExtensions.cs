namespace MURDoX.Core.Extensions;

public static class ListExtensions
{
    public static int SumOf(this Dictionary<string, List<int>> source)
    {
        return source.Values.Sum(value => value.Sum());
    }
}