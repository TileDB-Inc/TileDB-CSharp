using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tiledbcs.CSharp.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            
            //run benchmarks for dense array
            BenchmarkDotNet.Running.BenchmarkRunner.Run<DenseArrayBenchmark>();

            //run benchmarks for sparse array
            BenchmarkDotNet.Running.BenchmarkRunner.Run<SparseArrayBenchmark>();
        }
    }
}
