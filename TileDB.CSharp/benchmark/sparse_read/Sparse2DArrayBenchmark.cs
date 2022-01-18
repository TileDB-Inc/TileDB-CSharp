using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace array_read
{
    public class Sparse2DArrayBenchmark
    {
        public Sparse2DArrayBenchmark()
        {
            _array_uri = "test_sparse_array";
            InitBuffersToWrite();
        }
        public Sparse2DArrayBenchmark(string uri)
        {
            _array_uri = uri;
            InitBuffersToWrite();
        }
 
        protected string _array_uri = "test_sparse_array";

        protected TileDB.TileDBBuffer<int> _x_buffer_write = new TileDB.TileDBBuffer<int>();
        protected TileDB.TileDBBuffer<int> _y_buffer_write = new TileDB.TileDBBuffer<int>();
        protected TileDB.TileDBBuffer<string> _data1_buffer_write = new TileDB.TileDBBuffer<string>();
        protected TileDB.TileDBBuffer<int> _data2_buffer_write = new TileDB.TileDBBuffer<int>();
        protected TileDB.TileDBBuffer<int> _data3_buffer_write = new TileDB.TileDBBuffer<int>();

        protected TileDB.TileDBBuffer<int> _x_buffer_read = new TileDB.TileDBBuffer<int>();
        protected TileDB.TileDBBuffer<int> _y_buffer_read = new TileDB.TileDBBuffer<int>();
        protected TileDB.TileDBBuffer<string> _data1_buffer_read = new TileDB.TileDBBuffer<string>();
        protected TileDB.TileDBBuffer<int> _data2_buffer_read = new TileDB.TileDBBuffer<int>();
        protected TileDB.TileDBBuffer<int> _data3_buffer_read = new TileDB.TileDBBuffer<int>();

        protected TileDB.MapStringVectorUInt64 _resultBufferElements = new TileDB.MapStringVectorUInt64();

        protected void InitBuffersToWrite()
        {
            int num_data = 250*200;
            _x_buffer_write.Init(num_data, false, false);
            _y_buffer_write.Init(num_data, false, false);

            _data2_buffer_write.Init(num_data, false, false);
            _data3_buffer_write.Init(num_data, false, false);

            string[] string_array = new string[num_data];

            const string allowedChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz0123456789";
            Random rng = new Random(2022); //using 2022 as starting feed 
            char[] chars = new char[12];

            int count = 0;
            for(int i = 0; i < 250; ++i)
            {
                int x = 100 + i * 10;
                for(int j=0;j<200; ++j)
                {
                    int y = -3500 + i * 10;

                    _x_buffer_write.Data[count] = x;
                    _y_buffer_write.Data[count] = y;

                    for (int k = 0; k < 12; ++k)
                    {
                        chars[k] = allowedChars[rng.Next(0, allowedChars.Length)];
                    }
                    string_array[count] = new string(chars);

                    _data2_buffer_write.Data[count] = i + j;
                    _data3_buffer_write.Data[count] = i + 2 * j;

                    ++count;
                }
            }

            _data1_buffer_write.PackStringArray(string_array);

        }

        public IEnumerable<object> Compressions()
        {
            yield return "none";
            yield return "gzip";
        }


        [BenchmarkDotNet.Attributes.Benchmark]
        [BenchmarkDotNet.Attributes.ArgumentsSource(nameof(Compressions))]
        public void CreateArray(string compression_method)
        {

            try
            {
                string uri = _array_uri.Replace("test_", "test_" + compression_method + "_");
                System.Console.WriteLine("start to create array:{0}", uri);
                TileDB.Config config = new TileDB.Config();
                config.set("vfs.s3.scheme", "https");
                //     config.set("vfs.s3.region", "us-east-1");
                config.set("vfs.s3.endpoint_override", "");
                config.set("vfs.s3.aws_access_key_id", System.Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID"));
                config.set("vfs.s3.aws_secret_access_key", System.Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY"));
                config.set("vfs.s3.use_virtual_addressing", "True");

                using TileDB.Context ctx = new TileDB.Context(config);

                using TileDB.Domain dom = new TileDB.Domain(ctx);
                dom.add_int32_dimension("x", 10, 5500, 500);
                dom.add_int32_dimension("y", -3650, -10, 500);

                using TileDB.ArraySchema schema = new TileDB.ArraySchema(ctx, TileDB.ArrayType.TILEDB_SPARSE);
                schema.set_domain(dom);
                schema.set_order(TileDB.LayoutType.TILEDB_ROW_MAJOR, TileDB.LayoutType.TILEDB_ROW_MAJOR);
                schema.set_allows_dups(true);

                using TileDB.Attribute attr1 = TileDB.Attribute.create_attribute(ctx, "data1", TileDB.DataType.TILEDB_STRING_ASCII);
                using TileDB.Attribute attr2 = TileDB.Attribute.create_attribute(ctx, "data2", TileDB.DataType.TILEDB_INT32);
                using TileDB.Attribute attr3 = TileDB.Attribute.create_attribute(ctx, "data3", TileDB.DataType.TILEDB_INT32);

                if (compression_method == "gzip")
                {
                    using TileDB.Filter compression = new TileDB.Filter(ctx, TileDB.FilterType.TILEDB_FILTER_GZIP);
                    using TileDB.FilterList filterList = new TileDB.FilterList(ctx);
                    filterList.add_filter(compression);
                    attr1.set_filter_list(filterList);
                }

                schema.add_attribute(attr1);
                schema.add_attribute(attr2);
                schema.add_attribute(attr3);

                //delete array if it already exists
                TileDB.VFS vfs = new TileDB.VFS(ctx);
                if (vfs.is_dir(uri))
                {
                    vfs.remove_dir(uri);
                }


                TileDB.Array.create(uri, schema);

            }
            catch (TileDB.TileDBError tdbe)
            {
                System.Console.WriteLine(tdbe.Message);

            }
            catch (System.Exception e)
            {
                System.Console.WriteLine(e.Message);

            }


        }

        [BenchmarkDotNet.Attributes.Benchmark]
        [BenchmarkDotNet.Attributes.ArgumentsSource(nameof(Compressions))]
        public void WriteArray(string compression_method)
        {
            try
            {
                string uri = _array_uri.Replace("test_", "test_" + compression_method + "_");
                System.Console.WriteLine("start to write array:{0}", uri);
                TileDB.Config config = new TileDB.Config();
                config.set("vfs.s3.scheme", "https");
                config.set("vfs.s3.region", "us-east-1");
                config.set("vfs.s3.endpoint_override", "");
                config.set("vfs.s3.aws_access_key_id", System.Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID"));
                config.set("vfs.s3.aws_secret_access_key", System.Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY"));
                config.set("vfs.s3.use_virtual_addressing", "True");

                using TileDB.Context ctx = new TileDB.Context(config);

                var array_write = new TileDB.Array(ctx, uri, TileDB.QueryType.TILEDB_WRITE);
                var query_write = new TileDB.Query(ctx, array_write, TileDB.QueryType.TILEDB_WRITE);
                query_write.set_layout(TileDB.LayoutType.TILEDB_UNORDERED);
                query_write.set_buffer("x", _x_buffer_write.DataIntPtr, _x_buffer_write.BufferSize, _x_buffer_write.ElementDataSize);
                query_write.set_buffer("y", _y_buffer_write.DataIntPtr, _y_buffer_write.BufferSize, _y_buffer_write.ElementDataSize);
                query_write.set_buffer_with_offsets("data1", _data1_buffer_write.DataIntPtr, _data1_buffer_write.BufferSize, _data1_buffer_write.ElementDataSize, _data1_buffer_write.Offsets);
                query_write.set_buffer("data2", _data2_buffer_write.DataIntPtr, _data2_buffer_write.BufferSize, _data2_buffer_write.ElementDataSize);
                query_write.set_buffer("data3", _data3_buffer_write.DataIntPtr, _data3_buffer_write.BufferSize, _data3_buffer_write.ElementDataSize);

                query_write.submit();
                query_write.finalize();
                array_write.close();

            }
            catch (TileDB.TileDBError tdbe)
            {
                System.Console.WriteLine(tdbe.Message);

            }
            catch (System.Exception e)
            {
                System.Console.WriteLine(e.Message);

            }


        }


        [BenchmarkDotNet.Attributes.Benchmark]
        [BenchmarkDotNet.Attributes.ArgumentsSource(nameof(Compressions))]
        public void ReadArray(string compression_method)
        {
            try
            {
                string uri = _array_uri.Replace("test_", "test_" + compression_method + "_");
                System.Console.WriteLine("start to read array:{0}", uri);
                TileDB.Config config = new TileDB.Config();
                config.set("vfs.s3.scheme", "https");
                config.set("vfs.s3.region", "us-east-1");
                config.set("vfs.s3.endpoint_override", "");
                config.set("vfs.s3.aws_access_key_id", System.Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID"));
                config.set("vfs.s3.aws_secret_access_key", System.Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY"));
                config.set("vfs.s3.use_virtual_addressing", "True");

                using TileDB.Context ctx = new TileDB.Context(config);

                int num_data = 250 * 200;
                _x_buffer_read.Init(num_data, false, false);
                _y_buffer_read.Init(num_data, false, false);
                _data1_buffer_read.Init(num_data, true, false, num_data * 12);
                _data2_buffer_read.Init(num_data, false, false);
                _data3_buffer_read.Init(num_data, false, false);

                var array_read = new TileDB.Array(ctx, uri, TileDB.QueryType.TILEDB_READ);
                var query_read = new TileDB.Query(ctx, array_read, TileDB.QueryType.TILEDB_READ);
                query_read.set_layout(TileDB.LayoutType.TILEDB_UNORDERED);
                query_read.set_buffer("x", _x_buffer_read.DataIntPtr, _x_buffer_read.BufferSize, _x_buffer_read.ElementDataSize);
                query_read.set_buffer("y", _y_buffer_read.DataIntPtr, _y_buffer_read.BufferSize, _y_buffer_read.ElementDataSize);
                query_read.set_buffer_with_offsets("data1", _data1_buffer_read.DataIntPtr, _data1_buffer_read.BufferSize, _data1_buffer_read.ElementDataSize, _data1_buffer_read.Offsets);
                query_read.set_buffer("data2", _data2_buffer_read.DataIntPtr, _data2_buffer_read.BufferSize, _data2_buffer_read.ElementDataSize);
                query_read.set_buffer("data3", _data3_buffer_read.DataIntPtr, _data3_buffer_read.BufferSize, _data3_buffer_read.ElementDataSize);

                query_read.submit();
                query_read.finalize();

                _resultBufferElements = query_read.result_buffer_elements();

                array_read.close();
            }
            catch (TileDB.TileDBError tdbe)
            {
                System.Console.WriteLine(tdbe.Message);

            }
            catch (System.Exception e)
            {
                System.Console.WriteLine(e.Message);

            }

        }

        [BenchmarkDotNet.Attributes.Benchmark]
        [BenchmarkDotNet.Attributes.ArgumentsSource(nameof(Compressions))]
        public void DisplayReadResults(string compression_method)
        {
            System.Console.WriteLine("display results...");
            foreach (var kv in _resultBufferElements)
            {
                System.Console.WriteLine("{0}:{1}", kv.Key, string.Join(" ", kv.Value.ToArray()));
                if(kv.Key=="data1")
                {
                    UInt64 data_length = kv.Value[0];
                    UInt64 buffer_size = kv.Value[1];
                    string[] data1_str = _data1_buffer_read.UnPackStringArray((int)buffer_size, (int)data_length);
                    var data1_display = data1_str.Take(10);
                    System.Console.WriteLine("{0}: {1}", kv.Key, string.Join(" ", data1_display));
                }     
            }

           

        }

 



    }
}
