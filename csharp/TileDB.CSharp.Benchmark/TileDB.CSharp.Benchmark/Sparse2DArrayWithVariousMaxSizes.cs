using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileDB.CSharp.Benchmark
{
    [BenchmarkDotNet.Attributes.SimpleJob(BenchmarkDotNet.Engines.RunStrategy.ColdStart, targetCount: 1)]
    public class Sparse2DArrayWithVariousMaxSizes
    {
        private String array_uri_ = "bench_sparse_array";
        private int max_rows_ = 1000;
        private int max_cols_ = 1000;
        private int tile_rows_ = 100;
        private int tile_cols_ = 100;

        private UInt64 capacity_ = 100;

        private TileDB.VectorInt32 data_ = new TileDB.VectorInt32(); //data 
        private TileDB.VectorInt32 coords_ = new TileDB.VectorInt32();//coordinates

        public IEnumerable<object> MaxSizes()
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
        [BenchmarkDotNet.Attributes.ArgumentsSource(nameof(MaxSizes))]
        public void CreateSparse2DArrayWithMaxSize(int maxsize)
        {
            TileDB.Context ctx = new TileDB.Context();
            TileDB.Domain dom = new TileDB.Domain(ctx);

            dom.add_int32_dimension("rows", 1, Int32.MaxValue - tile_rows_, tile_rows_);
            dom.add_int32_dimension("cols", 1, Int32.MaxValue - tile_cols_, tile_cols_);

            TileDB.ArraySchema schema = new TileDB.ArraySchema(ctx, TileDB.tiledb_array_type_t.TILEDB_SPARSE);
            schema.set_domain(dom);
            schema.set_capacity(capacity_);

            TileDB.FilterList filterlist = new TileDB.FilterList();
            //       filterlist.add_filter(new TileDB.Filter(ctx, TileDB.tiledb_filter_type_t.TILEDB_FILTER_GZIP));

            TileDB.Attribute attr1 = TileDB.Attribute.create_attribute(ctx, "a", TileDB.tiledb_datatype_t.TILEDB_INT32);
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
        [BenchmarkDotNet.Attributes.ArgumentsSource(nameof(MaxSizes))]
        public void WriteSparse2DArrayWithMaxSize(int maxsize)
        {
            TileDB.Context ctx = new TileDB.Context();

            int max_row = maxsize;
            int max_col = maxsize;

            coords_.Clear();
            for (int i = 1; i <= max_row; i += 2)
            {
                for (int j = 1; j <= max_col; j += 2)
                {
                    coords_.Add(i);
                    coords_.Add(j);
                }
            }

            data_.Clear();
            int datasize = coords_.Count;
            for (int i = 0; i < datasize; ++i)
            {
                data_.Add(i);
            }


            //open array for write
            TileDB.Array array = new TileDB.Array(ctx, array_uri_, TileDB.tiledb_query_type_t.TILEDB_WRITE);
            TileDB.Query query = new TileDB.Query(ctx, array, TileDB.tiledb_query_type_t.TILEDB_WRITE);
            query.set_layout(TileDB.tiledb_layout_t.TILEDB_UNORDERED);
            query.set_int32_vector_buffer("a", data_);
            query.set_int32_coordinates(coords_);


            TileDB.Query.Status status = query.submit();
            array.close();
        }

        [BenchmarkDotNet.Attributes.Benchmark]
        [BenchmarkDotNet.Attributes.ArgumentsSource(nameof(MaxSizes))]
        public void ReadSparse2DArrayWithMaxSize(int maxsize)
        {
            TileDB.Context ctx = new TileDB.Context();

            int max_row = maxsize;
            int max_col = maxsize;

            coords_.Clear();
            for (int i = 1; i <= max_row; i += 2)
            {
                for (int j = 1; j <= max_col; j += 2)
                {
                    coords_.Add(i);
                    coords_.Add(j);
                }
            }


            int datasize = coords_.Count;
            data_ = new TileDB.VectorInt32(datasize);

            //open array for read
            TileDB.Array array = new TileDB.Array(ctx, array_uri_, TileDB.tiledb_query_type_t.TILEDB_READ);

            //query
            TileDB.Query query = new TileDB.Query(ctx, array, TileDB.tiledb_query_type_t.TILEDB_READ);
            query.set_layout(TileDB.tiledb_layout_t.TILEDB_UNORDERED);
            query.set_int32_coordinates(coords_);
            query.set_int32_vector_buffer("a", data_);

            TileDB.Query.Status status = query.submit();
            array.close();

        }



    }//public class Sparse2DArrayWithVariousMaxSizes
}
