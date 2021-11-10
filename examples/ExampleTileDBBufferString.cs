
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


namespace TileDB.Example
{
    public class ExampleTileDBBufferString
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Start to create the array...");

            // Create a dense array
            CreateArray();

            // Write the array
            TileDB.QueryStatus status_write = WriteArray();
            if (status_write == TileDB.QueryStatus.TILEDB_FAILED)
            {
                Console.WriteLine("Failed to write the array!");
            }
            else
            {
                Console.WriteLine("Finished writing the array.");
            }

            // Read the array
            TileDB.QueryStatus status_read = ReadArray();
            if (status_read == TileDB.QueryStatus.TILEDB_FAILED)
            {
                Console.WriteLine("Failed to read the array!");
            }
            else
            {
                Console.WriteLine("Finished reading the array.");
            }

            return;
        }

        private static String array_uri_ = "test_tiledb_buffer";

        #region Array
        private static void CreateArray()
        {
            TileDB.Context ctx = new TileDB.Context();
            TileDB.Domain dom = new TileDB.Domain(ctx);
            dom.add_int32_dimension("d", 1, 8, 8);
          
            TileDB.ArraySchema schema = new TileDB.ArraySchema(ctx, TileDB.ArrayType.TILEDB_DENSE);
            schema.set_domain(dom);
            TileDB.Attribute attr1 = TileDB.Attribute.create_attribute(ctx, "a", TileDB.DataType.TILEDB_STRING_ASCII);
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

            TileDB.TileDBBuffer<System.String> strbuffer = new TileDBBuffer<string>();

            string[] strarr = new string[8]
            {
                "a","b","c","d",
                "aa","bb","cc","dd"
            };

            System.Console.WriteLine("start to write the follwing string data:");
            foreach(string v in strarr)
            {
                System.Console.Write(" {0}", v);
            }
            System.Console.WriteLine();

            strbuffer.PackStringArray(strarr);

            //open array for write
            TileDB.Array array = new TileDB.Array(ctx, array_uri_, TileDB.QueryType.TILEDB_WRITE);
            TileDB.Query query = new TileDB.Query(ctx, array, TileDB.QueryType.TILEDB_WRITE);
            query.set_layout(TileDB.LayoutType.TILEDB_ROW_MAJOR);

            query.set_buffer_with_offsets("a", strbuffer.DataIntPtr, strbuffer.BufferSize, strbuffer.ElementDataSize, strbuffer.Offsets);

            TileDB.QueryStatus status = query.submit();
            query.finalize();
            array.close();

            return status;

        }//private void WriteArray()

        private static TileDB.QueryStatus ReadArray()
        {
            TileDB.Context ctx = new TileDB.Context();

            TileDB.VectorInt32 subarray = new TileDB.VectorInt32();
            subarray.Add(1);
            subarray.Add(8); //d1 3,6
 
            TileDB.TileDBBuffer<string> strbuffer = new TileDBBuffer<string>();
            strbuffer.Init(8, true, false, 100);

            //open array for read
            TileDB.Array array = new TileDB.Array(ctx, array_uri_, TileDB.QueryType.TILEDB_READ);

            //query
            TileDB.Query query = new TileDB.Query(ctx, array, TileDB.QueryType.TILEDB_READ);
            query.set_layout(TileDB.LayoutType.TILEDB_ROW_MAJOR);
            query.set_int32_subarray(subarray);

            query.set_buffer_with_offsets("a", strbuffer.DataIntPtr, strbuffer.BufferSize, strbuffer.ElementDataSize, strbuffer.Offsets);

            TileDB.QueryStatus status = query.submit();
            query.finalize();
            TileDB.MapStringVectorUInt64 buffer_elements = query.result_buffer_elements();
            array.close();

            // a query result
            UInt64 result_el_a1_size = buffer_elements["a"][1];

            string[] data = strbuffer.UnPackStringArray((int)result_el_a1_size);
            System.Console.WriteLine();
            System.Console.WriteLine("query result:");
            foreach(string v in data)
            {
                System.Console.Write(" {0}", v);
            }
            System.Console.WriteLine();

            return status;
        }//private TileDB.QueryStatus ReadArray()
        #endregion


    }

}
