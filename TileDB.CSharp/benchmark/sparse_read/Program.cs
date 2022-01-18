using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace array_read
{
    public class Program
    {
        static void Main(string[] args)
        {

            string base_uri = "test_sparse_array";
           // string base_uri = "s3://username/test/test_sparse_array";
            RunSparse2DArrayBenchmark("none",base_uri);
            RunSparse2DArrayBenchmark("gzip",base_uri);
          

            System.Console.WriteLine("finished!");
        }

        static void RunSparse2DArrayBenchmark(string compression_method, string base_uri= "test_sparse_array")
        {
            Sparse2DArrayBenchmark bench = new Sparse2DArrayBenchmark(base_uri);
            System.DateTime dt0 = System.DateTime.Now;
            bench.CreateArray(compression_method);
            System.DateTime dt1 = System.DateTime.Now;
            bench.WriteArray(compression_method);
            System.DateTime dt2 = System.DateTime.Now;
            bench.ReadArray(compression_method);
            System.DateTime dt3 = System.DateTime.Now;
            bench.DisplayReadResults(compression_method);
            System.DateTime dt4 = System.DateTime.Now;
            System.Console.WriteLine("compression:{0}, create:{1},write:{2},read:{3},display:{4}", 
                compression_method, 
                (dt1 - dt0).TotalMilliseconds, 
                (dt2 - dt1).TotalMilliseconds, 
                (dt3 - dt2).TotalMilliseconds,
                (dt4-dt3).TotalMilliseconds);

        }




    }
}
