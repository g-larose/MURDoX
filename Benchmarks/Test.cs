using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmarks
{
    [MemoryDiagnoser]
    public static class Test
    {
        [Benchmark]
        public static int PickRandom(string min, string max)
        {
            var randomNumber = 0;
            if (int.TryParse(min, out int minValue))
            {
                if (int.TryParse(max, out int maxValue))
                {
                    Random rnd = new Random();
                    randomNumber = rnd.Next(minValue, maxValue);
                }
            }

            return randomNumber;
        }
    }
}
