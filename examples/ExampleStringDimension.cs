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
    public class ExampleStringIntDoubleDimensions
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
                Console.WriteLine("Finished reading the array.");
            }

            return;
        }

        private static String array_uri_ = "test_string_dimension_array";

        #region Array
        private static void CreateArray()
        {
            TileDB.Context ctx = new TileDB.Context();
            TileDB.Domain dom = new TileDB.Domain(ctx);
            dom.add_dimension_for_datatype("dim1", TileDB.DataType.TILEDB_STRING_ASCII, "", "", "");
            dom.add_dimension_for_datatype("dim2", TileDB.DataType.TILEDB_INT32, "0", "9", "10");//domain=(0,9),tile_extent=10


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

            TileDB.VectorChar dim1_data = new TileDB.VectorChar(new char[9] { 'a', 'b', 'b', 'c', 'a', 'b', 'b', 'c', 'a' });
            TileDB.VectorUInt64 dim1_offsets = new TileDB.VectorUInt64(new UInt64[6] { 0, 1, 3, 4, 5, 7 });//["a","bb","c","a","bb","ca"]

            TileDB.VectorInt32 dim2_data = new TileDB.VectorInt32(new int[6] { 1, 1, 1, 2, 2, 2 });

            TileDB.VectorInt32 a1_data = new TileDB.VectorInt32(new int[6] { 1, 2, 3, 4, 5, 6 });


            TileDB.QueryStatus status = TileDB.QueryStatus.TILEDB_UNINITIALIZED;
            try
            {
                //open array for write
                TileDB.Array array = new TileDB.Array(ctx, array_uri_, TileDB.QueryType.TILEDB_WRITE);
                TileDB.Query query = new TileDB.Query(ctx, array, TileDB.QueryType.TILEDB_WRITE);
                query.set_layout(TileDB.LayoutType.TILEDB_UNORDERED);
                query.set_int32_vector_buffer("a1", a1_data);
                query.set_char_vector_buffer_with_offsets("dim1", dim1_data, dim1_offsets);
                query.set_int32_vector_buffer("dim2", dim2_data);

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


            TileDB.VectorString dim1_domain_vec = array.non_empty_domain_str_vector_from_name("dim1");
            System.Console.WriteLine("dim1_domain:[{0},{1}]", dim1_domain_vec[0], dim1_domain_vec[1]);

            TileDB.VectorString dim2_domain_vec = array.non_empty_domain_str_vector_from_name("dim2");
            System.Console.WriteLine("dim2_domain:[{0},{1}]", dim2_domain_vec[0], dim2_domain_vec[1]);

            TileDB.QueryStatus status = TileDB.QueryStatus.TILEDB_UNINITIALIZED;
            try
            {
                query.add_range_from_str_vector(0, dim1_domain_vec);
                query.add_range_from_str_vector(1, dim2_domain_vec);

                TileDB.VectorInt32 a1_data = TileDB.VectorInt32.Repeat(0, 2);
                TileDB.VectorChar dim1_data = TileDB.VectorChar.Repeat(' ', 8);
                TileDB.VectorUInt64 dim1_offsets = TileDB.VectorUInt64.Repeat(0, 1);
                TileDB.VectorInt32 dim2_data = TileDB.VectorInt32.Repeat(0, 1);

                query.set_layout(TileDB.LayoutType.TILEDB_ROW_MAJOR);
                query.set_int32_vector_buffer("a1", a1_data);
                query.set_char_vector_buffer_with_offsets("dim1", dim1_data, dim1_offsets);
                query.set_int32_vector_buffer("dim2", dim2_data);

                // the query result will be soreted by dim1 and then by dim2 (row major
                // the dim1 is sorted as "a", "a", "bb","bb", "c","c"
                status = TileDB.QueryStatus.TILEDB_UNINITIALIZED;
                do
                {
                    status = query.submit();
                    System.Console.WriteLine("dim1:{0},dim1_offset:{1},dim2:{2},data:{3}", new string(dim1_data.ToArray()), dim1_offsets[0], dim2_data[0], a1_data[0]);

                } while (status == TileDB.QueryStatus.TILEDB_INCOMPLETE);

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



            return status;
        }//private TileDB.QueryStatus ReadArray()
        #endregion


    }

}
