using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using TileDB.CSharp.Examples;
using TileDB.Interop;

namespace QuickstartSparseString
{
    static class Program
    {
        private static readonly string ArrayPath = ExampleUtil.MakeExamplePath("quickstart-sparse-string");

        static unsafe void CheckError(tiledb_ctx_t* ctx)
        {
            tiledb_error_t* err;
            Methods.tiledb_ctx_get_last_error(ctx, &err);
            if (err != null)
            {
                var err_out = (sbyte**)Marshal.StringToHGlobalAnsi("");
                Methods.tiledb_error_message(err, err_out);

                Console.WriteLine($"{new string(err_out[0])}");
                Marshal.FreeHGlobal((IntPtr)err_out);
            }
        }

        static unsafe void CreateArray(tiledb_ctx_t* ctx)
        {
            // Creating dimensions
            tiledb_dimension_t* d1, d2;

            Int32[] dim_domain = { 1, 4 };
            Int32 dim_extent = 4;

            var attr_name = (sbyte*)Marshal.StringToHGlobalAnsi("a1");
            var rows_name = (sbyte*)Marshal.StringToHGlobalAnsi("rows");
            var cols_name = (sbyte*)Marshal.StringToHGlobalAnsi("cols");

            Methods.tiledb_dimension_alloc(ctx, rows_name, tiledb_datatype_t.TILEDB_STRING_ASCII,
                null, null, &d1
            );
            // Check for errors with helper function
            CheckError(ctx);

            var domain_handle = GCHandle.Alloc(dim_domain, GCHandleType.Pinned);
            var extent_handle = GCHandle.Alloc(dim_extent, GCHandleType.Pinned);
            Methods.tiledb_dimension_alloc(ctx, cols_name, tiledb_datatype_t.TILEDB_INT32,
                domain_handle.AddrOfPinnedObject().ToPointer(), extent_handle.AddrOfPinnedObject().ToPointer(), &d2
            );
            CheckError(ctx);

            // Creating domain
            tiledb_domain_t* domain;
            Methods.tiledb_domain_alloc(ctx, &domain);
            CheckError(ctx);

            Methods.tiledb_domain_add_dimension(ctx, domain, d1);
            CheckError(ctx);
            Methods.tiledb_domain_add_dimension(ctx, domain, d2);
            CheckError(ctx);

            // Creating attribute
            tiledb_attribute_t* a1;

            Methods.tiledb_attribute_alloc(ctx, attr_name, tiledb_datatype_t.TILEDB_INT32, &a1);
            CheckError(ctx);

            // Creating schema
            tiledb_array_schema_t* array_schema;
            Methods.tiledb_array_schema_alloc(ctx, tiledb_array_type_t.TILEDB_SPARSE, &array_schema);
            CheckError(ctx);

            Methods.tiledb_array_schema_set_cell_order(ctx, array_schema, tiledb_layout_t.TILEDB_ROW_MAJOR);
            CheckError(ctx);

            Methods.tiledb_array_schema_set_tile_order(ctx, array_schema, tiledb_layout_t.TILEDB_ROW_MAJOR);
            CheckError(ctx);

            Methods.tiledb_array_schema_set_domain(ctx, array_schema, domain);
            CheckError(ctx);

            Methods.tiledb_array_schema_add_attribute(ctx, array_schema, a1);
            CheckError(ctx);

            var arr_name = Marshal.StringToHGlobalAnsi(ArrayPath);
            Methods.tiledb_array_create(ctx, (sbyte*)arr_name, array_schema);
            CheckError(ctx);

            Methods.tiledb_dimension_free(&d1);
            Methods.tiledb_dimension_free(&d2);
            Methods.tiledb_attribute_free(&a1);
            Methods.tiledb_domain_free(&domain);
            Methods.tiledb_array_schema_free(&array_schema);
            Marshal.FreeHGlobal(arr_name);
            Marshal.FreeHGlobal((IntPtr)attr_name);
            Marshal.FreeHGlobal((IntPtr)rows_name);
            Marshal.FreeHGlobal((IntPtr)cols_name);
            domain_handle.Free();
            extent_handle.Free();
        }

        static unsafe void WriteArray(tiledb_ctx_t* ctx, tiledb_array_t* array)
        {
            // Open array for writing
            Methods.tiledb_array_open(ctx, array, tiledb_query_type_t.TILEDB_WRITE);
            CheckError(ctx);

            // Allocate data to write into array
            byte[] data_rows = Encoding.ASCII.GetBytes("abbc");
            UInt64 data_rows_size = 4;

            UInt64[] data_rows_offsets = { 0, 1, 3 };
            UInt64 data_rows_offsets_size = (UInt64)data_rows_offsets.Length * sizeof(UInt64);

            Int32[] data_cols_coords = { 1, 4, 3 };
            UInt64 data_cols_coords_size = (UInt64)data_cols_coords.Length * sizeof(Int32);

            Int32[] data = { 5, 6, 7 };
            UInt32 data_size = (UInt32)data.Length * sizeof(Int32);

            var attr_name = (sbyte*)Marshal.StringToHGlobalAnsi("a1");
            var rows_name = (sbyte*)Marshal.StringToHGlobalAnsi("rows");
            var cols_name = (sbyte*)Marshal.StringToHGlobalAnsi("cols");

            // Create write query
            tiledb_query_t* query;
            Methods.tiledb_query_alloc(ctx, array, tiledb_query_type_t.TILEDB_WRITE, &query);
            CheckError(ctx);

            Methods.tiledb_query_set_layout(ctx, query, tiledb_layout_t.TILEDB_UNORDERED);
            CheckError(ctx);

            var data_handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            var data_size_handle = GCHandle.Alloc(data_size, GCHandleType.Pinned);
            Methods.tiledb_query_set_data_buffer(ctx, query, attr_name,
                data_handle.AddrOfPinnedObject().ToPointer(), (ulong*)data_size_handle.AddrOfPinnedObject()
            );
            CheckError(ctx);

            var data_rows_handle = GCHandle.Alloc(data_rows, GCHandleType.Pinned);
            var data_rows_size_handle = GCHandle.Alloc(data_rows_size, GCHandleType.Pinned);
            Methods.tiledb_query_set_data_buffer(ctx, query, rows_name,
                data_rows_handle.AddrOfPinnedObject().ToPointer(), (ulong*)data_rows_size_handle.AddrOfPinnedObject()
            );
            CheckError(ctx);

            var data_rows_offsets_handle = GCHandle.Alloc(data_rows_offsets, GCHandleType.Pinned);
            var data_rows_offsets_size_handle = GCHandle.Alloc(data_rows_offsets_size, GCHandleType.Pinned);
            Methods.tiledb_query_set_offsets_buffer(ctx, query, rows_name,
                (ulong*)data_rows_offsets_handle.AddrOfPinnedObject(), (ulong*)data_rows_offsets_size_handle.AddrOfPinnedObject()
            );
            CheckError(ctx);

            var data_cols_handle = GCHandle.Alloc(data_cols_coords, GCHandleType.Pinned);
            var data_cols_size_handle = GCHandle.Alloc(data_cols_coords_size, GCHandleType.Pinned);
            Methods.tiledb_query_set_data_buffer(ctx, query, cols_name,
                data_cols_handle.AddrOfPinnedObject().ToPointer(), (ulong*)data_cols_size_handle.AddrOfPinnedObject()
            );
            CheckError(ctx);

            Methods.tiledb_query_submit(ctx, query);
            CheckError(ctx);

            Methods.tiledb_array_close(ctx, array);
            CheckError(ctx);
            Methods.tiledb_query_free(&query);
            Marshal.FreeHGlobal((IntPtr)attr_name);
            Marshal.FreeHGlobal((IntPtr)rows_name);
            Marshal.FreeHGlobal((IntPtr)cols_name);
            data_handle.Free();
            data_size_handle.Free();
            data_rows_handle.Free();
            data_rows_size_handle.Free();
            data_rows_offsets_handle.Free();
            data_rows_offsets_size_handle.Free();
            data_cols_handle.Free();
            data_cols_size_handle.Free();
        }

        static unsafe void ReadArray(tiledb_ctx_t* ctx, tiledb_array_t* array)
        {
            Methods.tiledb_array_open(ctx, array, tiledb_query_type_t.TILEDB_READ);
            CheckError(ctx);

            var attr_name = (sbyte*)Marshal.StringToHGlobalAnsi("a1");
            var rows_name = (sbyte*)Marshal.StringToHGlobalAnsi("rows");
            var cols_name = (sbyte*)Marshal.StringToHGlobalAnsi("cols");

            tiledb_query_t* read_query;
            Methods.tiledb_query_alloc(ctx, array, tiledb_query_type_t.TILEDB_READ, &read_query);
            CheckError(ctx);

            byte[] row_start = Encoding.ASCII.GetBytes("a");
            byte[] row_end = Encoding.ASCII.GetBytes("c");
            var start_handle = GCHandle.Alloc(row_start, GCHandleType.Pinned);
            var end_handle = GCHandle.Alloc(row_end, GCHandleType.Pinned);
            Methods.tiledb_query_add_range_var_by_name(ctx, read_query, rows_name,
                start_handle.AddrOfPinnedObject().ToPointer(), (ulong)row_start.Length,
                end_handle.AddrOfPinnedObject().ToPointer(), (ulong)row_end.Length
            );
            CheckError(ctx);

            int cols_start = 2, cols_end = 4;
            Methods.tiledb_query_add_range(ctx, read_query, 1, &cols_start, &cols_end, null);
            CheckError(ctx);

            Methods.tiledb_query_set_layout(ctx, read_query, tiledb_layout_t.TILEDB_ROW_MAJOR);
            CheckError(ctx);

            Int32[] read_data = new Int32[3];
            UInt64 read_data_size = (UInt64)3 * sizeof(Int32);
            var read_data_handle = GCHandle.Alloc(read_data, GCHandleType.Pinned);
            var read_data_size_handle = GCHandle.Alloc(read_data_size, GCHandleType.Pinned);
            Methods.tiledb_query_set_data_buffer(ctx, read_query, attr_name,
                read_data_handle.AddrOfPinnedObject().ToPointer(), (ulong*)read_data_size_handle.AddrOfPinnedObject()
            );
            CheckError(ctx);

            byte[] read_rows = new byte[4];
            UInt64 read_rows_size = 4;
            var read_rows_handle = GCHandle.Alloc(read_rows, GCHandleType.Pinned);
            var read_rows_size_handle = GCHandle.Alloc(read_rows_size, GCHandleType.Pinned);
            Methods.tiledb_query_set_data_buffer(ctx, read_query, rows_name,
                read_rows_handle.AddrOfPinnedObject().ToPointer(),
                (ulong*)read_rows_size_handle.AddrOfPinnedObject()
            );
            CheckError(ctx);

            UInt64[] read_rows_offsets = new UInt64[3];
            UInt64 read_rows_offsets_size = (UInt64)read_rows_offsets.Length * sizeof(UInt64);
            var read_rows_offsets_handle = GCHandle.Alloc(read_rows_offsets, GCHandleType.Pinned);
            var read_rows_offsets_size_handle = GCHandle.Alloc(read_rows_offsets_size, GCHandleType.Pinned);
            Methods.tiledb_query_set_offsets_buffer(ctx, read_query, rows_name,
                (ulong*)read_rows_offsets_handle.AddrOfPinnedObject(),
                (ulong*)read_rows_offsets_size_handle.AddrOfPinnedObject()
            );
            CheckError(ctx);

            Int32[] read_cols = new Int32[3];
            UInt64 read_cols_size = (UInt64)3 * sizeof(Int32);
            var read_cols_handle = GCHandle.Alloc(read_cols, GCHandleType.Pinned);
            var read_cols_size_handle = GCHandle.Alloc(read_cols_size, GCHandleType.Pinned);
            Methods.tiledb_query_set_data_buffer(ctx, read_query, cols_name,
                read_cols_handle.AddrOfPinnedObject().ToPointer(), (ulong*)read_cols_size_handle.AddrOfPinnedObject()
            );
            CheckError(ctx);

            tiledb_query_status_t status;
            Methods.tiledb_query_submit(ctx, read_query);
            CheckError(ctx);
            Methods.tiledb_query_get_status(ctx, read_query, &status);
            if (status == tiledb_query_status_t.TILEDB_COMPLETED)
            {
                // Divide read data size by size of read data type
                var result_num = (int)((UInt64)read_data_size_handle.Target! / sizeof(Int32));
                for (int i = 0; i < result_num; i++)
                {
                    int start = (int)read_rows_offsets[i];
                    // Find size of current cell using distance to next offset
                    // + Final cell_size found by length of read data - final offset position
                    int cell_size = (int)(i == result_num - 1
                        ? (ulong)read_rows_size_handle.Target! - read_rows_offsets[i]
                        : read_rows_offsets[i + 1] - read_rows_offsets[i]);

                    // Take all characters after start position
                    string cell_row = new string(Encoding.ASCII.GetChars(read_rows))[start..];
                    // Take characters up to current cell_size
                    cell_row = cell_row[..cell_size];
                    Console.WriteLine($"Cell ({cell_row}, {read_cols[i]}) has data: {read_data[i]}");
                }
            }
            else
            {
                throw new InvalidOperationException($"Invalid query status: {status}");
            }

            Methods.tiledb_array_close(ctx, array);
            CheckError(ctx);
            Methods.tiledb_query_free(&read_query);
            Marshal.FreeHGlobal((IntPtr)attr_name);
            Marshal.FreeHGlobal((IntPtr)rows_name);
            Marshal.FreeHGlobal((IntPtr)cols_name);
            start_handle.Free();
            end_handle.Free();
            read_data_handle.Free();
            read_data_size_handle.Free();
            read_rows_handle.Free();
            read_rows_size_handle.Free();
            read_rows_offsets_handle.Free();
            read_rows_offsets_size_handle.Free();
            read_cols_handle.Free();
            read_cols_size_handle.Free();
        }

        static unsafe void Main()
        {
            var arr_name = (sbyte*)Marshal.StringToHGlobalAnsi(ArrayPath);
            // Check TileDB Version
            int major = 0, minor = 0, rev = 0;
            Methods.tiledb_version(&major, &minor, &rev);
            Console.WriteLine($"TileDB Version: {major}.{minor}.{rev}");

            // Create context
            tiledb_ctx_t* ctx;
            Methods.tiledb_ctx_alloc(null, &ctx);
            CheckError(ctx);

            // Allocate object to check if URI exists
            tiledb_object_t o = new();
            Methods.tiledb_object_type(ctx, arr_name, &o);
            CheckError(ctx);
            if (o != tiledb_object_t.TILEDB_INVALID)
            {
                Directory.Delete(new string(arr_name), recursive: true);
            }

            CreateArray(ctx);

            // Allocate array for read / write
            tiledb_array_t* array;
            Methods.tiledb_array_alloc(ctx, arr_name, &array);
            CheckError(ctx);

            WriteArray(ctx, array);
            ReadArray(ctx, array);

            Methods.tiledb_array_free(&array);
            Methods.tiledb_ctx_free(&ctx);
            Marshal.FreeHGlobal((IntPtr)arr_name);
        }
    }
}
