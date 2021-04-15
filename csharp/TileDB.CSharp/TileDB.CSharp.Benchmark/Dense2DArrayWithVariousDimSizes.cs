using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileDB.CSharp.Benchmark
{
    [BenchmarkDotNet.Attributes.SimpleJob(BenchmarkDotNet.Engines.RunStrategy.ColdStart, targetCount: 1)]
    public class Dense2DArrayWithVariousDimSizes
    {
        private String array_uri_ = "bench_dense_array";
        private int tile_rows_ = 100;
        private int tile_cols_ = 100;

        private TileDB.VectorInt32 data_ = new TileDB.VectorInt32(); //data


        public IEnumerable<object> DimSizes()
        {
            yield return 200;
            yield return 500;
            yield return 1000;
            yield return 2000;
            yield return 3000;
            yield return 4000;
            yield return 5000;
            yield return 6000;
        }

        [BenchmarkDotNet.Attributes.Benchmark]
        [BenchmarkDotNet.Attributes.ArgumentsSource(nameof(DimSizes))]
        public void CreateDense2DArrayWithDimSize(int dimsize)
        {
            TileDB.Context ctx = new TileDB.Context();
            TileDB.Domain dom = new TileDB.Domain(ctx);
            int array_rows = dimsize;
            int array_cols = dimsize;
            dom.add_int32_dimension("rows", 1, array_rows, tile_rows_);
            dom.add_int32_dimension("cols", 1, array_cols, tile_cols_);

            TileDB.ArraySchema schema = new TileDB.ArraySchema(ctx, TileDB.tiledb_array_type_t.TILEDB_DENSE);
            schema.set_domain(dom);

            TileDB.FilterList filterlist = new TileDB.FilterList();
      //      filterlist.add_filter(new TileDB.Filter(ctx, TileDB.tiledb_filter_type_t.TILEDB_FILTER_GZIP));

            TileDB.Attribute attr1 = TileDB.Attribute.create_attribute(ctx, "a", TileDB.tiledb_datatype_t.TILEDB_INT32);
    //        attr1.set_filter_list(filterlist);
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
        [BenchmarkDotNet.Attributes.ArgumentsSource(nameof(DimSizes))]
        public void WriteDense2DArrayWithDimSize(int dimsize)
        {
            TileDB.Context ctx = new TileDB.Context();

  

            data_.Clear();
            int datasize = dimsize * dimsize;
            for (int i = 0; i < datasize; ++i)
            {
                data_.Add(i);
            }


            //open array for write
            TileDB.Array array = new TileDB.Array(ctx, array_uri_, TileDB.tiledb_query_type_t.TILEDB_WRITE);
            TileDB.Query query = new TileDB.Query(ctx, array, TileDB.tiledb_query_type_t.TILEDB_WRITE);
            query.set_layout(TileDB.tiledb_layout_t.TILEDB_ROW_MAJOR);
            query.set_int32_vector_buffer("a", data_);

            TileDB.Query.Status status = query.submit();
            array.close();
        }

        [BenchmarkDotNet.Attributes.Benchmark]
        [BenchmarkDotNet.Attributes.ArgumentsSource(nameof(DimSizes))]
        public void ReadDense2DArrayWithDimSize(int dimsize)
        {
            TileDB.Context ctx = new TileDB.Context();

            TileDB.VectorInt32 subarray = new TileDB.VectorInt32();
            subarray.Add(1);
            subarray.Add(dimsize); //rows 1,dimsize
            subarray.Add(1);
            subarray.Add(dimsize); //cols 1,dimsize

             
            data_ = new TileDB.VectorInt32(dimsize * dimsize);

            //open array for read
            TileDB.Array array = new TileDB.Array(ctx, array_uri_, TileDB.tiledb_query_type_t.TILEDB_READ);

            //query
            TileDB.Query query = new TileDB.Query(ctx, array, TileDB.tiledb_query_type_t.TILEDB_READ);
            query.set_layout(TileDB.tiledb_layout_t.TILEDB_ROW_MAJOR);
            query.set_int32_subarray(subarray);
            query.set_int32_vector_buffer("a", data_);

            TileDB.Query.Status status = query.submit();
            array.close();

        }


    }
}
