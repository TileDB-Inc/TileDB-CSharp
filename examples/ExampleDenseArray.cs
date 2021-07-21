
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
    public class ExampleArray
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start to create a simple sparse array...");

            // Create a dense array
            CreateDenseSimpleArray();
            
            // Write the sparse array
            TileDB.Query.Status status_write = WriteDenseSimpleArray();
            if (status_write == TileDB.Query.Status.FAILED)
            {
                Console.WriteLine("Failed to write the sparse array!");
            }
            else
            {
                Console.WriteLine("Finished writing the sparse array.");
            }

            // Read the sparse array
            TileDB.Query.Status status_read = ReadDenseSimpleArray();
            if (status_read == TileDB.Query.Status.FAILED)
            {
                Console.WriteLine("Failed to read the sparse array!");
            }
            else
            {
                Console.WriteLine("Finished reading the sparse array.");
            }

            return;
        }

        private static String array_uri_ = "test_dense_array";

        #region Simple Dense Array
        private static void CreateDenseSimpleArray()
        {
            TileDB.Context ctx = new TileDB.Context();
            TileDB.Domain dom = new TileDB.Domain(ctx);
            dom.add_int32_dimension("rows", 1, 4, 4);
            dom.add_int32_dimension("cols", 1, 4, 4);

            TileDB.ArraySchema schema = new TileDB.ArraySchema(ctx, TileDB.tiledb_array_type_t.TILEDB_DENSE);
            schema.set_domain(dom);
            TileDB.Attribute attr1 = TileDB.Attribute.create_attribute(ctx, "a", TileDB.tiledb_datatype_t.TILEDB_INT32);
            schema.add_attribute(attr1);

            //delete array if it already exists
            TileDB.VFS vfs = new TileDB.VFS(ctx);
            if (vfs.is_dir(array_uri_))
            {
                vfs.remove_dir(array_uri_);
            }

            //create array
            TileDB.Array.create(array_uri_, schema);

        }//private void CreateDenseSimpleArray()

        private static TileDB.Query.Status WriteDenseSimpleArray()
        {
            TileDB.Context ctx = new TileDB.Context();

            TileDB.VectorInt32 data = new TileDB.VectorInt32();
            for (int i = 1; i <= 16; ++i)
            {
                data.Add(i);
            }


            //open array for write
            TileDB.Array array = new TileDB.Array(ctx, array_uri_, TileDB.tiledb_query_type_t.TILEDB_WRITE);
            TileDB.Query query = new TileDB.Query(ctx, array, TileDB.tiledb_query_type_t.TILEDB_WRITE);
            query.set_layout(TileDB.tiledb_layout_t.TILEDB_ROW_MAJOR);
            query.set_int32_vector_buffer("a", data);

            TileDB.Query.Status status = query.submit();
            array.close();

            return status;

        }//private void WriteDenseSimpleArray()

        private static TileDB.Query.Status ReadDenseSimpleArray()
        {
            TileDB.Context ctx = new TileDB.Context();

            TileDB.VectorInt32 subarray = new TileDB.VectorInt32();
            subarray.Add(1);
            subarray.Add(2); //rows 1,2
            subarray.Add(2);
            subarray.Add(4); //cols 2,3,4

            TileDB.VectorInt32 data = new TileDB.VectorInt32(6); //hold 6 elements

            //open array for read
            TileDB.Array array = new TileDB.Array(ctx, array_uri_, TileDB.tiledb_query_type_t.TILEDB_READ);

            //query
            TileDB.Query query = new TileDB.Query(ctx, array, TileDB.tiledb_query_type_t.TILEDB_READ);
            query.set_layout(TileDB.tiledb_layout_t.TILEDB_ROW_MAJOR);
            query.set_int32_subarray(subarray);
            query.set_int32_vector_buffer("a", data);

            TileDB.Query.Status status = query.submit();
            array.close();

            return status;
        }//private TileDB.Query.Status ReadDenseSimpleArray()
        #endregion


    }

}
 