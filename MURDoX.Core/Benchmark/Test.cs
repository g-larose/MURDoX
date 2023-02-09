using BenchmarkDotNet.Attributes;


namespace MURDoX.Core.Benchmark
{
    [MemoryDiagnoser]
    public class Test
    {
        [Benchmark]
        public Dictionary<int, List<int>> DoRoll()
        {
            var numDice = 6;
            var sides = 6;
            Dictionary<int, List<int>> result = new Dictionary<int, List<int>>();
            List<int> numbers = new List<int>();
            var rnd = new Random();

            for (int i = 0; i < sides; i++)
            {
                numbers.Add(rnd.Next(1, sides));
            }
            result.Add(numDice, numbers);
            return result;
        }

        [Benchmark]
        public (int, List<int>) Roll()
        {
            var numDice = 6;
            var sides = 6;
            var nums = new List<int>();
            var result = (numDice, nums);
            var rnd = new Random((int)Environment.TickCount);
            for (int i = 0; i < numDice; i++)
            {
                result.Item2.Add(rnd.Next(1, sides + 1));
            }
            return result;
        }
                 
    }


}
