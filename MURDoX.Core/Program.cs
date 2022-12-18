using BenchmarkDotNet.Running;
using MURDoX.Core;
using MURDoX.Core.Benchmark;

//class Program
//{
//    static void Main(string[] args)
//    {
//        var nums = BenchmarkRunner.Run<Test>();
//    }
//}

Bot bot = new Bot();
bot.RunAsync().GetAwaiter().GetResult();
