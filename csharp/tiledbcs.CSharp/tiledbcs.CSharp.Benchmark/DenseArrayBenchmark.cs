using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tiledbcs.CSharp.Benchmark
{
    [BenchmarkDotNet.Attributes.MemoryDiagnoser]
    public class DenseArrayBenchmark
    {
        private String array_uri_ = "bench_array";

        [BenchmarkDotNet.Attributes.Benchmark]
        public void CreateSimpleDenseArray()
        {
            tiledb.Context ctx = new tiledb.Context();
            tiledb.Domain dom = new tiledb.Domain(ctx);
            dom.add_int32_dimension("rows", 1, 4, 4);
            dom.add_int32_dimension("cols", 1, 4, 4);

            tiledb.ArraySchema schema = new tiledb.ArraySchema(ctx, tiledb.tiledb_array_type_t.TILEDB_DENSE);
            schema.set_domain(dom);
            tiledb.Attribute attr1 = tiledb.Attribute.create_attribute(ctx, "a", tiledb.tiledb_datatype_t.TILEDB_INT32);
            schema.add_attribute(attr1);

            //delete array if it already exists
            tiledb.VFS vfs = new tiledb.VFS(ctx);
            if (vfs.is_dir(array_uri_))
            {
                vfs.remove_dir(array_uri_);
            }

            //create array
            tiledb.Array.create(array_uri_, schema);
        }

        [BenchmarkDotNet.Attributes.Benchmark]
        public void WriteSimpleDenseArray()
        {
            tiledb.Context ctx = new tiledb.Context();

            tiledb.VectorInt32 data = new tiledb.VectorInt32();
            for (int i = 1; i <= 16; ++i)
            {
                data.Add(i);
            }


            //open array for write
            tiledb.Array array = new tiledb.Array(ctx, array_uri_, tiledb.tiledb_query_type_t.TILEDB_WRITE);
            tiledb.Query query = new tiledb.Query(ctx, array, tiledb.tiledb_query_type_t.TILEDB_WRITE);
            query.set_layout(tiledb.tiledb_layout_t.TILEDB_ROW_MAJOR);
            query.set_int32_vector_buffer("a", data);

            tiledb.Query.Status status = query.submit();
            array.close();
        }

        [BenchmarkDotNet.Attributes.Benchmark]
        public void ReadSimpleDenseArray()
        {
            tiledb.Context ctx = new tiledb.Context();

            tiledb.VectorInt32 subarray = new tiledb.VectorInt32();
            subarray.Add(1);
            subarray.Add(2); //rows 1,2
            subarray.Add(2);
            subarray.Add(4); //cols 2,3,4

            tiledb.VectorInt32 data = new tiledb.VectorInt32(6); //hold 6 elements

            //open array for read
            tiledb.Array array = new tiledb.Array(ctx, array_uri_, tiledb.tiledb_query_type_t.TILEDB_READ);

            //query
            tiledb.Query query = new tiledb.Query(ctx, array, tiledb.tiledb_query_type_t.TILEDB_READ);
            query.set_layout(tiledb.tiledb_layout_t.TILEDB_ROW_MAJOR);
            query.set_int32_subarray(subarray);
            query.set_int32_vector_buffer("a", data);

            tiledb.Query.Status status = query.submit();
            array.close();

        }



    }
}
