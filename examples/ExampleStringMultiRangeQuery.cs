/*
 * The MIT License (MIT)
 *
 * Copyright (c) 2020 TileDB, Inc.
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace TileDB.Example
{
    public class ExampleStringMultiRangeQuery
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Start to create an array...");

            // Create an array
            CreateArray();

            // Write the dense array
            TileDB.QueryStatus status_write = WriteArray();
            if (status_write == TileDB.QueryStatus.TILEDB_FAILED)
            {
                Console.WriteLine("Failed to write the array!");
            }
            else
            {
                Console.WriteLine("Finished writing the array.");
            }

            // Read the dense array
            TileDB.QueryStatus status_read = ReadArray();
            if (status_read == TileDB.QueryStatus.TILEDB_FAILED)
            {
                Console.WriteLine("Failed to read the array!");
            }
            else
            {
                Console.WriteLine("Finished reading the  array.");
            }

            return;
        }

        private static String array_uri_ = "test_query_condition_string_array";

        #region Array
        private static void CreateArray()
        {
            TileDB.Context ctx = new TileDB.Context();
            TileDB.Domain dom = new TileDB.Domain(ctx);
            dom.add_dimension_for_datatype("dim1", TileDB.DataType.TILEDB_STRING_ASCII, "", "", "");
            dom.add_dimension_for_datatype("dim2", TileDB.DataType.TILEDB_INT32, "0", "1000000", "10"); 

            TileDB.ArraySchema schema = new TileDB.ArraySchema(ctx, TileDB.ArrayType.TILEDB_SPARSE);
            schema.set_capacity(1000);

            schema.set_domain(dom);
            TileDB.Attribute attr1 = TileDB.Attribute.create_attribute(ctx, "a1", TileDB.DataType.TILEDB_INT32);
            schema.add_attribute(attr1);
 
            //delete array if it already exists
            TileDB.VFS vfs = new TileDB.VFS(ctx);
            if (vfs.is_dir(array_uri_))
            {
                vfs.remove_dir(array_uri_);
            }

            //create array
            TileDB.Array.create(array_uri_, schema);

        }//private void CreateArray()

        private static TileDB.QueryStatus WriteArray()
        {
            TileDB.Context ctx = new TileDB.Context();



            string[] strarr = new string[32] 
            {
                "a","bb","ccc","dd",
                "eee","f","g","hhh",
                "i","jjj","kk","l",
                "m","n","oo","p",
                "a","bb","ccc","dd",
                "eee","f","g","hhh",
                "i","jjj","kk","l",
                "m","n","oo","p",
            };
            TileDB.TileDBBuffer<string> tdb_str_buffer = new TileDBBuffer<string>();
            tdb_str_buffer.PackStringArray(strarr);

            TileDB.TileDBBuffer<int> tdb_int_buffer = new TileDBBuffer<int>();
            tdb_int_buffer.Init(32, false, false);
            for (int i = 0; i < 32; ++i)
            {
                tdb_int_buffer.Data[i] = i + 1;
            }

            TileDB.TileDBBuffer<int> tdb_a1_buffer = new TileDBBuffer<int>();
            tdb_a1_buffer.Init(32, false, false);
            for (int i = 0; i < 32; ++i)
            {
                tdb_a1_buffer.Data[i] = i + 1;
            }


            TileDB.QueryStatus status = TileDB.QueryStatus.TILEDB_UNINITIALIZED;
            try
            {
                //open array for write
                TileDB.Array array = new TileDB.Array(ctx, array_uri_, TileDB.QueryType.TILEDB_WRITE);
                TileDB.Query query = new TileDB.Query(ctx, array, TileDB.QueryType.TILEDB_WRITE);
                query.set_layout(TileDB.LayoutType.TILEDB_UNORDERED);
                //set int buffer

                query.set_buffer_with_offsets("dim1", tdb_str_buffer.DataIntPtr, tdb_str_buffer.BufferSize, tdb_int_buffer.ElementDataSize, tdb_str_buffer.Offsets);
                query.set_buffer("dim2", tdb_int_buffer.DataIntPtr, tdb_int_buffer.BufferSize, tdb_int_buffer.ElementDataSize);
                query.set_buffer("a1", tdb_a1_buffer.DataIntPtr, tdb_a1_buffer.BufferSize, tdb_a1_buffer.ElementDataSize);

                query.submit();
                query.finalize();
                status = query.query_status();
                array.close();
            }
            catch (TileDB.TileDBError tdbe)
            {
                System.Console.WriteLine(tdbe.Message);
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine(e.Message);
            }

            return status;

        }//private void WriteArray()

        private static TileDB.QueryStatus ReadArray()
        {
            TileDB.Context ctx = new TileDB.Context();

            string[] string_list = new string[10]
            {
                "a","bb","ccc","dd",
                "eee","f","g","hhh",
                "i","jjj"
            };

            //open array for read
            TileDB.Array array = new TileDB.Array(ctx, array_uri_, TileDB.QueryType.TILEDB_READ);

     //       TileDB.VectorInt32 subarray = new TileDB.VectorInt32(new int[4] { 1, 4, 1, 4 });

            //from the subarray, we know the maximum number of elements is 16(4x4), so we initialize the buffer with length of 6  
            TileDB.TileDBBuffer<int> tdb_dim2_buffer = new TileDBBuffer<int>();
            tdb_dim2_buffer.Init(32, false, false);

            TileDB.TileDBBuffer<int> tdb_a1_buffer = new TileDBBuffer<int>();
            tdb_a1_buffer.Init(32, false, false);

            //Here we initialize the string buffer to have maxium number of string as 16, for each string might have several characters
            //so we use 100 as buffer size for char
            TileDB.TileDBBuffer<string> tdb_str_buffer = new TileDBBuffer<string>();
            tdb_str_buffer.Init(32, true, false, 256);



            TileDB.QueryStatus status = TileDB.QueryStatus.TILEDB_UNINITIALIZED;
            TileDB.MapStringVectorUInt64 buffer_elements = new TileDB.MapStringVectorUInt64();

            try
            {
                //query
                TileDB.Query query = new TileDB.Query(ctx, array, TileDB.QueryType.TILEDB_READ);

                query.set_layout(TileDB.LayoutType.TILEDB_ROW_MAJOR);

                query.set_buffer_with_offsets("dim1", tdb_str_buffer.DataIntPtr, tdb_str_buffer.BufferSize, tdb_str_buffer.ElementDataSize, tdb_str_buffer.Offsets);
                query.set_buffer("dim2", tdb_dim2_buffer.DataIntPtr, tdb_dim2_buffer.BufferSize, tdb_dim2_buffer.ElementDataSize);
                query.set_buffer("a1", tdb_a1_buffer.DataIntPtr, tdb_a1_buffer.BufferSize, tdb_a1_buffer.ElementDataSize);
 
                for(int i=0;i<string_list.Length; ++i)
                {
                    query.add_range(0, string_list[i], string_list[i]); //add range for dimension 0
                }
 
                query.submit();
                query.finalize();
                status = query.query_status();
                buffer_elements = query.result_buffer_elements();
            }
            catch (TileDB.TileDBError tdbe)
            {
                System.Console.WriteLine(tdbe.Message);

            }
            catch (System.Exception e)
            {
                System.Console.WriteLine(e.Message);

            }

            //a1 int result
            UInt64 result_el_a1_size = buffer_elements["a1"][1];
            var a1_int = tdb_a1_buffer.Data.Take((int)result_el_a1_size);

            // a2 string query result
            UInt64 result_el_dim1_data_length = buffer_elements["dim1"][0];
            UInt64 result_el_dim1_buffer_size = buffer_elements["dim1"][1];
            string[] dim1_str = tdb_str_buffer.UnPackStringArray((int)result_el_dim1_buffer_size, (int)result_el_dim1_data_length);


            //print out 
            System.Console.WriteLine("query result a1:{0}, dim1:{1}", String.Join(" ", a1_int), String.Join(" ", dim1_str));
            array.close();


            return TileDB.QueryStatus.TILEDB_COMPLETED;
        }//private TileDB.QueryStatus ReadArray()
        #endregion


    }

}
