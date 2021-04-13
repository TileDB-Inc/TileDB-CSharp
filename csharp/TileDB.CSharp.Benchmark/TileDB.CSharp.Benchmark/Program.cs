using System;

namespace TileDB.CSharp.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            
           // System.Console.WriteLine("TileDB version:{0}", TileDB.ArrayUtil.get_tiledb_version());

            Dense2DArrayWithVariousDimSizes test_dense_dimsize = new Dense2DArrayWithVariousDimSizes();
            test_dense_dimsize.CreateDense2DArrayWithDimSize(100);
            test_dense_dimsize.WriteDense2DArrayWithDimSize(100);
            test_dense_dimsize.ReadDense2DArrayWithDimSize(100);
            //BenchmarkDotNet.Running.BenchmarkRunner.Run<Dense2DArrayWithVariousDimSizes>();

            Dense2DArrayWithVariousTileSizes test_dense_tilesize = new Dense2DArrayWithVariousTileSizes();
            test_dense_tilesize.CreateDense2DArrayWithTileSize(10);
            test_dense_tilesize.WriteDense2DArrayWithTileSize(10);
            test_dense_tilesize.ReadDense2DArrayWithTileSize(10);
            //BenchmarkDotNet.Running.BenchmarkRunner.Run<Dense2DArrayWithVariousTileSizes>();

            Sparse2DArrayWithVariousMaxSizes test_sparse_maxsize = new Sparse2DArrayWithVariousMaxSizes();
            test_sparse_maxsize.CreateSparse2DArrayWithMaxSize(1000);
            test_sparse_maxsize.WriteSparse2DArrayWithMaxSize(1000);
            test_sparse_maxsize.ReadSparse2DArrayWithMaxSize(1000);
            //BenchmarkDotNet.Running.BenchmarkRunner.Run<Sparse2DArrayWithVariousMaxSizes>();

            Sparse2DArrayWithVariousCapacities test_sparse_capacity = new Sparse2DArrayWithVariousCapacities();
            test_sparse_capacity.CreateSparse2DArrayWithCapacity(300);
            test_sparse_capacity.WriteSparse2DArrayWithCapacity(300);
            test_sparse_capacity.ReadSparse2DArrayWithCapacity(300);
            //BenchmarkDotNet.Running.BenchmarkRunner.Run<Sparse2DArrayWithVariousCapacities>();


        }
    }
}
