﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileDB.CSharp.Benchmark
{
 
    [BenchmarkDotNet.Attributes.SimpleJob(BenchmarkDotNet.Engines.RunStrategy.ColdStart,targetCount:1)]
    public class Dense2DArrayWithVariousTileSizes
    {
        private String array_uri_ = "bench_dense_array";
        private int array_rows_ = 1000;
        private int array_cols_ = 1000;
        private TileDB.VectorInt32 data_ = new TileDB.VectorInt32(); //data 

        public IEnumerable<object> TileSizes()
        {
            yield return 10;
            yield return 20;
            yield return 30;
            yield return 40;
            yield return 50;
            yield return 100;
            yield return 500;
            yield return 1000;
        }


        [BenchmarkDotNet.Attributes.Benchmark]
        [BenchmarkDotNet.Attributes.ArgumentsSource(nameof(TileSizes))]
        public void CreateDense2DArrayWithTileSize(int tilesize)
        {
            TileDB.Context ctx = new TileDB.Context();
            TileDB.Domain dom = new TileDB.Domain(ctx);
            int tile_rows = tilesize;
            int tile_cols = tilesize;
            dom.add_int32_dimension("rows", 1, array_rows_, tile_rows);
            dom.add_int32_dimension("cols", 1, array_cols_, tile_cols);

            TileDB.ArraySchema schema = new TileDB.ArraySchema(ctx, TileDB.ArrayType.TILEDB_DENSE);
            schema.set_domain(dom);

            TileDB.FilterList filterlist = new TileDB.FilterList();
     //       filterlist.add_filter(new TileDB.Filter(ctx, TileDB.tiledb_filter_type_t.TILEDB_FILTER_GZIP));

            TileDB.Attribute attr1 = TileDB.Attribute.create_attribute(ctx, "a", TileDB.DataType.TILEDB_INT32);
      //      attr1.set_filter_list(filterlist);
            schema.add_attribute(attr1);

            //delete array if it already exists
            TileDB.VFS vfs = new TileDB.VFS(ctx);
            if (vfs.is_dir(array_uri_))
            {
                vfs.remove_dir(array_uri_);
            }

            //create array
            TileDB.Array.create(array_uri_, schema);
        }

        [BenchmarkDotNet.Attributes.Benchmark]
        [BenchmarkDotNet.Attributes.ArgumentsSource(nameof(TileSizes))]
        public void WriteDense2DArrayWithTileSize(int tilesize)
        {
            TileDB.Context ctx = new TileDB.Context();



            data_.Clear();
            int datasize = array_rows_ * array_cols_;
            for (int i = 0; i < datasize; ++i)
            {
                data_.Add(i);
            }


            //open array for write
            TileDB.Array array = new TileDB.Array(ctx, array_uri_, TileDB.QueryType.TILEDB_WRITE);
            TileDB.Query query = new TileDB.Query(ctx, array, TileDB.QueryType.TILEDB_WRITE);
            query.set_layout(TileDB.LayoutType.TILEDB_ROW_MAJOR);
            query.set_int32_vector_buffer("a", data_);

            TileDB.QueryStatus status = query.submit();
            array.close();
        }

        [BenchmarkDotNet.Attributes.Benchmark]
        [BenchmarkDotNet.Attributes.ArgumentsSource(nameof(TileSizes))]
        public void ReadDense2DArrayWithTileSize(int tilesize)
        {
            TileDB.Context ctx = new TileDB.Context();

            TileDB.VectorInt32 subarray = new TileDB.VectorInt32();
            subarray.Add(1);
            subarray.Add(array_rows_); //rows 1,dimsize
            subarray.Add(1);
            subarray.Add(array_cols_); //cols 1,dimsize


            data_ = new TileDB.VectorInt32(array_rows_ * array_cols_);

            //open array for read
            TileDB.Array array = new TileDB.Array(ctx, array_uri_, TileDB.QueryType.TILEDB_READ);

            //query
            TileDB.Query query = new TileDB.Query(ctx, array, TileDB.QueryType.TILEDB_READ);
            query.set_layout(TileDB.LayoutType.TILEDB_ROW_MAJOR);
            query.set_int32_subarray(subarray);
            query.set_int32_vector_buffer("a", data_);

            TileDB.QueryStatus status = query.submit();
            array.close();

        }


    }//public class Dense2DArrayWithVariousTileSizes
}
