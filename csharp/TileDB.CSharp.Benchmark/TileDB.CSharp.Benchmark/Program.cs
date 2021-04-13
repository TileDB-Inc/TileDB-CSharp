using System;

namespace TileDB.CSharp.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            // Console.WriteLine("Hello World!");
            BenchmarkDotNet.Running.BenchmarkRunner.Run<Dense2DArrayWithVariousDimSizes>();


        }
    }
}
