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
    public class ExampleQueryCondition
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

        private static String array_uri_ = "test_query_condition_array";

        #region Array
        private static void CreateArray()
        {
            TileDB.Context ctx = new TileDB.Context();
            TileDB.Domain dom = new TileDB.Domain(ctx);
            dom.add_dimension_for_datatype("rows", TileDB.DataType.TILEDB_INT32, "1", "4", "4");
            dom.add_dimension_for_datatype("cols", TileDB.DataType.TILEDB_INT32, "1", "4", "4");//domain=(,4),tile_extent=4


            TileDB.ArraySchema schema = new TileDB.ArraySchema(ctx, TileDB.ArrayType.TILEDB_DENSE);


            schema.set_domain(dom);
            schema.set_order(TileDB.LayoutType.TILEDB_ROW_MAJOR, TileDB.LayoutType.TILEDB_ROW_MAJOR);

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


            TileDB.VectorInt32 a1_data = new TileDB.VectorInt32(new int[16] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 });


            TileDB.QueryStatus status = TileDB.QueryStatus.TILEDB_UNINITIALIZED;
            try
            {
                //open array for write
                TileDB.Array array = new TileDB.Array(ctx, array_uri_, TileDB.QueryType.TILEDB_WRITE);
                TileDB.Query query = new TileDB.Query(ctx, array, TileDB.QueryType.TILEDB_WRITE);
                query.set_layout(TileDB.LayoutType.TILEDB_ROW_MAJOR);
                query.set_int32_vector_buffer("a1", a1_data);


                status = query.submit();
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

            //open array for read
            TileDB.Array array = new TileDB.Array(ctx, array_uri_, TileDB.QueryType.TILEDB_READ);
            //query
            TileDB.Query query = new TileDB.Query(ctx, array, TileDB.QueryType.TILEDB_READ);

            TileDB.VectorInt32 subarray = new TileDB.VectorInt32(new int[4] { 1, 2, 2, 4 });

            TileDB.VectorInt32 a1_data = TileDB.VectorInt32.Repeat(Int32.MinValue, 6);

            TileDB.QueryStatus status = TileDB.QueryStatus.TILEDB_UNINITIALIZED;
            try
            {
                query.set_int32_subarray(subarray);
                query.set_layout(TileDB.LayoutType.TILEDB_ROW_MAJOR);
                query.set_int32_vector_buffer("a1", a1_data);

                TileDB.QueryCondition qc1 = TileDB.QueryCondition.create_for_datatype(ctx, TileDB.DataType.TILEDB_INT32, "a1", "3", TileDB.QueryConditionOperatorType.TILEDB_GE);
                TileDB.QueryCondition qc2 = TileDB.QueryCondition.create_for_datatype(ctx, TileDB.DataType.TILEDB_INT32, "a1", "7", TileDB.QueryConditionOperatorType.TILEDB_LT);
                TileDB.QueryCondition qc = qc1.combine(qc2, TileDB.QueryConditionCombinationOperatorType.TILEDB_AND);
                //set combined condition >=3 and <7
                query.set_condition(qc);

                status = query.submit();
                array.close();

            }
            catch (TileDB.TileDBError tdbe)
            {
                System.Console.WriteLine(tdbe.Message);
                return status;
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine(e.Message);
                return status;
            }

            System.Console.WriteLine("query result:{0}", String.Join(" ", a1_data));

            return status;
        }//private TileDB.QueryStatus ReadArray()
        #endregion


    }

}
