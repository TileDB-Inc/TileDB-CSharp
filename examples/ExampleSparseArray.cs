
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
    public class ExampleSparseArray
    {
        public static void Run()
        {
            Console.WriteLine("Start creating sparse array...");

            // Create a sparse array
            CreateSparseArray();

            // Write the sparse array
            TileDB.QueryStatus status_write = WriteSparseArray();
            if (status_write == TileDB.QueryStatus.TILEDB_FAILED)
            {
                Console.WriteLine("Failed to write the sparse array!");
            }
            else
            {
                Console.WriteLine("Finished writing the sparse array.");
            }

            // Read the sparse array
            TileDB.QueryStatus status_read = ReadSparseArray();
            if (status_read == TileDB.QueryStatus.TILEDB_FAILED)
            {
                Console.WriteLine("Failed to read the sparse array!");
            }
            else
            {
                Console.WriteLine("Finished reading the sparse array.");
            }

            // Remove sparse array
            RemoveSparseArray();

            return;
        }

        private static String array_uri_ = "test_sparse_array";

        #region Sparse Array
        private static void CreateSparseArray()
        {
            TileDB.Context ctx = new TileDB.Context();
            TileDB.Domain dom = new TileDB.Domain(ctx);
            dom.add_int32_dimension("rows", 1, 4, 4);
            dom.add_int32_dimension("cols", 1, 4, 4);

            TileDB.ArraySchema schema = new TileDB.ArraySchema(ctx, TileDB.ArrayType.TILEDB_SPARSE);
            schema.set_domain(dom);
            TileDB.Attribute attr1 = TileDB.Attribute.create_attribute(ctx, "a", TileDB.DataType.TILEDB_INT32);
            schema.add_attribute(attr1);

            //create array
            TileDB.Array.create(array_uri_, schema);

        }//private void CreateSparseArray()

        private static void RemoveSparseArray()
        {
            TileDB.Context ctx = new TileDB.Context();

            // Delete array if it already exists
            TileDB.VFS vfs = new TileDB.VFS(ctx);
            if (vfs.is_dir(array_uri_))
            {
                vfs.remove_dir(array_uri_);
            }
        }//private void RemoveSparseArray()

        private static TileDB.QueryStatus WriteSparseArray()
        {
            TileDB.Context ctx = new TileDB.Context();

            TileDB.VectorInt32 coords_rows = new TileDB.VectorInt32();
            coords_rows.Add(1);
            coords_rows.Add(2);
            coords_rows.Add(2);

            TileDB.VectorInt32 coords_cols = new TileDB.VectorInt32();
            coords_cols.Add(1);
            coords_cols.Add(4);
            coords_cols.Add(3);

            TileDB.VectorInt32 data = new TileDB.VectorInt32();
            for (int i = 1; i <= 3; ++i)
            {
                data.Add(i);
            }


            //open array for write
            TileDB.Array array = new TileDB.Array(ctx, array_uri_, TileDB.QueryType.TILEDB_WRITE);
            TileDB.Query query = new TileDB.Query(ctx, array, TileDB.QueryType.TILEDB_WRITE);
            query.set_layout(TileDB.LayoutType.TILEDB_UNORDERED);
            query.set_int32_vector_buffer("a", data);
            query.set_int32_vector_buffer("rows", coords_rows);
            query.set_int32_vector_buffer("cols", coords_cols);

            TileDB.QueryStatus status = query.submit();
            array.close();

            return status;

        }//private void WriteSparseArray()

        private static TileDB.QueryStatus ReadSparseArray()
        {
            TileDB.Context ctx = new TileDB.Context();


            TileDB.VectorInt32 coords_rows = new TileDB.VectorInt32(3);
            TileDB.VectorInt32 coords_cols = new TileDB.VectorInt32(3);

            TileDB.VectorInt32 data = new TileDB.VectorInt32(3); //hold 3 elements

            TileDB.VectorInt32 subarray = new TileDB.VectorInt32();
            subarray.Add(1);
            subarray.Add(2);
            subarray.Add(2);
            subarray.Add(4);

            //open array for read
            TileDB.Array array = new TileDB.Array(ctx, array_uri_, TileDB.QueryType.TILEDB_READ);

            //query
            TileDB.Query query = new TileDB.Query(ctx, array, TileDB.QueryType.TILEDB_READ);
            query.set_layout(TileDB.LayoutType.TILEDB_ROW_MAJOR);
            query.set_int32_subarray(subarray);
            query.set_int32_vector_buffer("a", data);
            query.set_int32_vector_buffer("rows", coords_rows);
            query.set_int32_vector_buffer("cols", coords_cols);

            TileDB.QueryStatus status = query.submit();
            array.close();

            return status;
        }//private TileDB.QueryStatus ReadSparseArray()

        #endregion


    }
}