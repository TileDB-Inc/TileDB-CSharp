using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using TileDB.Interop;

namespace quickstart_sparse_string
{
    class Program
    {
        static unsafe void check_err(tiledb_ctx_t* ctx)
        {
            var err = stackalloc tiledb_error_t*[1];
            Methods.tiledb_ctx_get_last_error(ctx, err);
            if (*err != null)
            {
                var err_out = (sbyte**)Marshal.StringToHGlobalAnsi("");
                Methods.tiledb_error_message(*err, err_out);

                Console.WriteLine($"{new string(err_out[0])}");
                Marshal.FreeHGlobal((IntPtr)err_out);
            }
        }
        
        static unsafe void create_array(tiledb_ctx_t* ctx)
        {
            // Creating dimensions
            var d1 = stackalloc tiledb_dimension_t*[1];
            var d2 = stackalloc tiledb_dimension_t*[1];

            Int32[] dim_domain = { 1, 4 };
            Int32 dim_extent = 4;
            
            var attr_name = (sbyte*)Marshal.StringToHGlobalAnsi("a1");
            var rows_name = (sbyte*)Marshal.StringToHGlobalAnsi("rows");
            var cols_name = (sbyte*)Marshal.StringToHGlobalAnsi("cols");

            Methods.tiledb_dimension_alloc(ctx, rows_name, tiledb_datatype_t.TILEDB_STRING_ASCII,
                null, null, d1
            );
            // Check for errors with helper function
            check_err(ctx);

            var domain_handle = GCHandle.Alloc(dim_domain, GCHandleType.Pinned);
            var extent_handle = GCHandle.Alloc(dim_extent, GCHandleType.Pinned);
            Methods.tiledb_dimension_alloc(ctx, cols_name, tiledb_datatype_t.TILEDB_INT32,
                domain_handle.AddrOfPinnedObject().ToPointer(), extent_handle.AddrOfPinnedObject().ToPointer(), d2
            );
            domain_handle.Free();
            extent_handle.Free();
            check_err(ctx);

            // Creating domain
            tiledb_domain_t* domain;
            Methods.tiledb_domain_alloc(ctx, &domain);
            check_err(ctx);

            Methods.tiledb_domain_add_dimension(ctx, domain, *d1);
            check_err(ctx);
            Methods.tiledb_domain_add_dimension(ctx, domain, *d2);
            check_err(ctx);

            // Creating attribute
            tiledb_attribute_t* a1;

            Methods.tiledb_attribute_alloc(ctx, attr_name, tiledb_datatype_t.TILEDB_INT32, &a1);
            check_err(ctx);

            // Creating schema
            tiledb_array_schema_t* array_schema;
            Methods.tiledb_array_schema_alloc(ctx, tiledb_array_type_t.TILEDB_SPARSE, &array_schema);
            check_err(ctx);

            Methods.tiledb_array_schema_set_cell_order(ctx, array_schema, tiledb_layout_t.TILEDB_ROW_MAJOR);
            check_err(ctx);

            Methods.tiledb_array_schema_set_tile_order(ctx, array_schema, tiledb_layout_t.TILEDB_ROW_MAJOR);
            check_err(ctx);

            Methods.tiledb_array_schema_set_domain(ctx, array_schema, domain);
            check_err(ctx);

            Methods.tiledb_array_schema_add_attribute(ctx, array_schema, a1);
            check_err(ctx);

            var arr_name = Marshal.StringToHGlobalAnsi("quickstart-sparse-string");
            Methods.tiledb_array_create(ctx, (sbyte*)arr_name, array_schema);
            Marshal.FreeHGlobal(arr_name);
            check_err(ctx);

            Marshal.FreeHGlobal((IntPtr)attr_name);
            Marshal.FreeHGlobal((IntPtr)rows_name);
            Marshal.FreeHGlobal((IntPtr)cols_name);
            Methods.tiledb_dimension_free(d1);
            Methods.tiledb_dimension_free(d2);
            Methods.tiledb_attribute_free(&a1);
            Methods.tiledb_domain_free(&domain);
            Methods.tiledb_array_schema_free(&array_schema);
        }

        static unsafe void write_array(tiledb_ctx_t* ctx, tiledb_array_t* array)
        {
            // Open array for writing
            Methods.tiledb_array_open(ctx, array, tiledb_query_type_t.TILEDB_WRITE);
            check_err(ctx);

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
            check_err(ctx);
            
            Methods.tiledb_query_set_layout(ctx, query, tiledb_layout_t.TILEDB_UNORDERED);
            check_err(ctx);

            var data_handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            var data_size_handle = GCHandle.Alloc(data_size, GCHandleType.Pinned);
            Methods.tiledb_query_set_data_buffer(ctx, query, attr_name, 
                data_handle.AddrOfPinnedObject().ToPointer(), (ulong*)data_size_handle.AddrOfPinnedObject()
            );
            data_handle.Free();
            data_size_handle.Free();
            check_err(ctx);

            data_handle = GCHandle.Alloc(data_rows, GCHandleType.Pinned);
            data_size_handle = GCHandle.Alloc(data_rows_size, GCHandleType.Pinned);
            Methods.tiledb_query_set_data_buffer(ctx, query, rows_name,
                data_handle.AddrOfPinnedObject().ToPointer(), (ulong*)data_size_handle.AddrOfPinnedObject()
            );
            data_handle.Free();
            data_size_handle.Free();
            check_err(ctx);

            data_handle = GCHandle.Alloc(data_rows_offsets, GCHandleType.Pinned);
            data_size_handle = GCHandle.Alloc(data_rows_offsets_size, GCHandleType.Pinned);
            Methods.tiledb_query_set_offsets_buffer(ctx, query, rows_name,
                (ulong*)data_handle.AddrOfPinnedObject(), (ulong*)data_size_handle.AddrOfPinnedObject()
            );
            data_handle.Free();
            data_size_handle.Free();
            check_err(ctx);

            data_handle = GCHandle.Alloc(data_cols_coords, GCHandleType.Pinned);
            data_size_handle = GCHandle.Alloc(data_cols_coords_size, GCHandleType.Pinned);
            Methods.tiledb_query_set_data_buffer(ctx, query, cols_name,
                data_handle.AddrOfPinnedObject().ToPointer(), (ulong*)data_size_handle.AddrOfPinnedObject()
            );
            data_handle.Free();
            data_size_handle.Free();
            check_err(ctx);

            Methods.tiledb_query_submit(ctx, query);
            check_err(ctx);

            Marshal.FreeHGlobal((IntPtr)attr_name);
            Marshal.FreeHGlobal((IntPtr)rows_name);
            Marshal.FreeHGlobal((IntPtr)cols_name);
            Methods.tiledb_array_close(ctx, array);
            check_err(ctx);
            Methods.tiledb_query_free(&query);
        }

        static unsafe void read_array(tiledb_ctx_t* ctx, tiledb_array_t* array)
        {
            Methods.tiledb_array_open(ctx, array, tiledb_query_type_t.TILEDB_READ);
            check_err(ctx);

            byte[] read_rows = new byte[4];
            UInt64 read_rows_size = 4;
            
            UInt64[] read_rows_offsets = new UInt64[3];
            UInt64 read_rows_offsets_size = (UInt64)read_rows_offsets.Length * sizeof(UInt64);
            
            Int32[] read_cols = new Int32[3];
            UInt64 read_cols_size = (UInt64)3 * sizeof(Int32);
            
            Int32[] read_data = new Int32[3];
            UInt64 read_data_size = (UInt64)3 * sizeof(Int32);
            
            using var attr_name = new MarshaledString("a1");
            using var rows_name = new MarshaledString("rows");
            using var cols_name = new MarshaledString("cols");
            
            tiledb_query_t* read_query;
            Methods.tiledb_query_alloc(ctx, array, tiledb_query_type_t.TILEDB_READ, &read_query);
            check_err(ctx);

            byte[] row_start = Encoding.ASCII.GetBytes("a");
            byte[] row_end = Encoding.ASCII.GetBytes("c");
            var start_handle = GCHandle.Alloc(row_start, GCHandleType.Pinned);
            var end_handle = GCHandle.Alloc(row_end, GCHandleType.Pinned);
            Methods.tiledb_query_add_range_var_by_name(ctx, read_query, rows_name,
            start_handle.AddrOfPinnedObject().ToPointer(), (ulong)row_start.Length,
            end_handle.AddrOfPinnedObject().ToPointer(), (ulong)row_end.Length
            );
            check_err(ctx);

            int cols_start = 2, cols_end = 4;
            Methods.tiledb_query_add_range(ctx, read_query, 1, &cols_start, &cols_end, null);
            check_err(ctx);

            Methods.tiledb_query_set_layout(ctx, read_query, tiledb_layout_t.TILEDB_ROW_MAJOR);
            check_err(ctx);

            var read_data_handle = GCHandle.Alloc(read_data, GCHandleType.Pinned);
            var read_data_size_handle = GCHandle.Alloc(read_data_size, GCHandleType.Pinned);
            Methods.tiledb_query_set_data_buffer(ctx, read_query, attr_name,
                read_data_handle.AddrOfPinnedObject().ToPointer(), (ulong*)read_data_size_handle.AddrOfPinnedObject()
            );
            read_data_handle.Free();
            read_data_size_handle.Free();
            check_err(ctx);

            read_data_handle = GCHandle.Alloc(read_rows, GCHandleType.Pinned);
            read_data_size_handle = GCHandle.Alloc(read_rows_size, GCHandleType.Pinned);
            Methods.tiledb_query_set_data_buffer(ctx, read_query, rows_name,
                read_data_handle.AddrOfPinnedObject().ToPointer(), (ulong*)read_data_size_handle.AddrOfPinnedObject()
            );
            read_data_handle.Free();
            read_data_size_handle.Free();
            check_err(ctx);

            read_data_handle = GCHandle.Alloc(read_rows_offsets, GCHandleType.Pinned);
            read_data_size_handle = GCHandle.Alloc(read_rows_offsets_size, GCHandleType.Pinned);
            Methods.tiledb_query_set_offsets_buffer(ctx, read_query, rows_name,
                (ulong*)read_data_handle.AddrOfPinnedObject(), (ulong*)read_data_size_handle.AddrOfPinnedObject()
            );
            read_data_handle.Free();
            read_data_size_handle.Free();
            check_err(ctx);

            read_data_handle = GCHandle.Alloc(read_cols, GCHandleType.Pinned);
            read_data_size_handle = GCHandle.Alloc(read_cols_size, GCHandleType.Pinned);
            Methods.tiledb_query_set_data_buffer(ctx, read_query, cols_name,
                read_data_handle.AddrOfPinnedObject().ToPointer(), (ulong*)read_data_size_handle.AddrOfPinnedObject()
            );
            read_data_handle.Free();
            read_data_size_handle.Free();
            check_err(ctx);

            Methods.tiledb_query_submit(ctx, read_query);
            check_err(ctx);

            for (int i = 0; i < read_data.Length; i++)
            {
                // No more data to print if offset is 0 after first iteration
                if (i > 0 && read_rows_offsets[i] == 0) break;
                
                int start = (int)read_rows_offsets[i];
                // Take all characters after start position
                string cell_row = new string(Encoding.ASCII.GetChars(read_rows))[start..];
                // Check if next offset position is available
                if (i < read_rows_offsets.Length - 1 && read_rows_offsets[i + 1] > 0)
                {
                    // Take all characters up to next offset position
                    cell_row = cell_row[..(int)read_rows_offsets[i + 1]];
                }
                Console.WriteLine( $"Cell ({cell_row}, {read_cols[i]}) has data: {read_data[i]}");
            }

            Methods.tiledb_array_close(ctx, array);
            check_err(ctx);
            Methods.tiledb_query_free(&read_query);
        }

        static void Main(string[] args)
        {
            using var arr_name = new MarshaledString("quickstart-sparse-string");
            unsafe
            {
                // Check TileDB Version
                int major = 0, minor = 0, rev = 0;
                Methods.tiledb_version(&major, &minor, &rev);
                Console.WriteLine($"TileDB Version: {major}.{minor}.{rev}");

                // Create context
                tiledb_ctx_t* ctx;
                Methods.tiledb_ctx_alloc(null, &ctx);
                check_err(ctx);

                // Allocate object to check if URI exists
                tiledb_object_t o = new();
                Methods.tiledb_object_type(ctx, arr_name, &o);
                check_err(ctx);
                if (o != tiledb_object_t.TILEDB_INVALID)
                {
                    Directory.Delete(new string(arr_name), recursive: true);
                }
                
                create_array(ctx);

                // Allocate array for read / write
                tiledb_array_t* array;
                Methods.tiledb_array_alloc(ctx, arr_name, &array);
                check_err(ctx);

                write_array(ctx, array);
                read_array(ctx, array);

                Methods.tiledb_array_free(&array);
                Methods.tiledb_ctx_free(&ctx);
            }
        }
    }
}
