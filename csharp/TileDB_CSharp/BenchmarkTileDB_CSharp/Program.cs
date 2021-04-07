using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenchmarkTileDB_CSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            //run benchmarks for dense array
            BenchmarkDotNet.Running.BenchmarkRunner.Run<BenchmarkDenseArray>();

            //run benchmarks for sparse array
            BenchmarkDotNet.Running.BenchmarkRunner.Run<BenchmarkSparseArray>();
        }
    }
}
