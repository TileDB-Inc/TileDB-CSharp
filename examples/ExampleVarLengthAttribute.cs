
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
    public class ExampleVarLengthAttribute
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Start to create an array...");

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

        private static String array_uri_ = "varlength_attributes_array";

        #region Array
        private static void CreateArray()
        {
            TileDB.Context ctx = new TileDB.Context();
            TileDB.Domain dom = new TileDB.Domain(ctx);
            // The array will be 4x4 with dimensions "rows" and "cols", with domain [1,4]
            dom.add_int32_dimension("rows", 1, 4, 4);
            dom.add_int32_dimension("cols", 1, 4, 4);

            TileDB.ArraySchema schema = new TileDB.ArraySchema(ctx, TileDB.ArrayType.TILEDB_DENSE);
            schema.set_domain(dom);
            schema.set_order(TileDB.LayoutType.TILEDB_ROW_MAJOR, TileDB.LayoutType.TILEDB_ROW_MAJOR);

            TileDB.Attribute attr1 = TileDB.Attribute.create_attribute(ctx, "a1", TileDB.DataType.TILEDB_STRING_ASCII);
            schema.add_attribute(attr1);

            //vector of int, var-sized
            TileDB.Attribute attr2 = TileDB.Attribute.create_vector_attribute(ctx, "a2", TileDB.DataType.TILEDB_INT32);
            schema.add_attribute(attr2);

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

            string a1_str = "abbcccddeeefghhhijjjkklmnoop";

            TileDB.VectorChar a1_data = new TileDB.VectorChar(a1_str.ToCharArray());
            TileDB.VectorUInt64 a1_off = new VectorUInt64(new UInt64[16] { 0, 1, 3, 6, 8, 11, 12, 13, 16, 17, 20, 22, 23, 24, 25, 27 });

 
            TileDB.VectorInt32 a2_data = new VectorInt32(new int[26] {1, 1, 2, 2,  3,  4,  5,  6,  6,  7,  7,  8,  8,
                              8, 9, 9, 10, 11, 12, 12, 13, 14, 14, 14, 15, 16 });
            TileDB.VectorUInt64 a2_el_off = new VectorUInt64(new UInt64[16] { 0, 2, 4, 5, 6, 7, 9, 11, 14, 16, 17, 18, 20, 21, 24, 25 });
            TileDB.VectorUInt64 a2_off = new VectorUInt64();
            for (int i = 0; i < a2_el_off.Count; ++i) {
                a2_off.Add(a2_el_off[i] * TileDB.EnumUtil.datatype_size(TileDB.DataType.TILEDB_INT32));
            }
 

            TileDB.QueryStatus status = TileDB.QueryStatus.TILEDB_UNINITIALIZED;
            try
            {
                //open array for write
                TileDB.Array array = new TileDB.Array(ctx, array_uri_, TileDB.QueryType.TILEDB_WRITE);
                TileDB.Query query = new TileDB.Query(ctx, array, TileDB.QueryType.TILEDB_WRITE);
                query.set_layout(TileDB.LayoutType.TILEDB_ROW_MAJOR);

                //set query buffer
                query.set_char_vector_buffer_with_offsets("a1", a1_data, a1_off);
                query.set_int32_vector_buffer_with_offsets("a2", a2_data, a2_off);
               
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

            // Slice only rows 1,2 and cols 2,3,4
            TileDB.VectorInt32 subarray = new TileDB.VectorInt32(new int[4] { 1, 2, 2, 4 });

            TileDB.VectorUInt64 a1_off = TileDB.VectorUInt64.Repeat(0, 12);
            TileDB.VectorChar a1_data = TileDB.VectorChar.Repeat(' ', 32);

            TileDB.VectorUInt64 a2_off = TileDB.VectorUInt64.Repeat(0, 12);
            TileDB.VectorInt32 a2_data = TileDB.VectorInt32.Repeat(0, 32);
 

            TileDB.QueryStatus status = TileDB.QueryStatus.TILEDB_UNINITIALIZED;
            TileDB.MapStringVectorUInt64 buffer_elements = new TileDB.MapStringVectorUInt64();
            try
            {
                //open array for read
                TileDB.Array array = new TileDB.Array(ctx, array_uri_, TileDB.QueryType.TILEDB_READ);

                //query
                TileDB.Query query = new TileDB.Query(ctx, array, TileDB.QueryType.TILEDB_READ);
                query.set_layout(TileDB.LayoutType.TILEDB_ROW_MAJOR);
                query.set_int32_subarray(subarray);

                //set query buffer
                query.set_char_vector_buffer_with_offsets("a1", a1_data, a1_off);
                query.set_int32_vector_buffer_with_offsets("a2", a2_data, a2_off); 

                status = query.submit();
                buffer_elements= query.result_buffer_elements();
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

            System.Console.WriteLine("query result:");

            // a1 query result
            UInt64 result_el_a1_off = buffer_elements["a1"][0];
            UInt64 result_el_a1_size = buffer_elements["a1"][1];
            TileDB.VectorUInt64 a1_sizes = new TileDB.VectorUInt64();
            for (int i = 0; i < ((int)result_el_a1_off - 1); ++i)
            {
                a1_sizes.Add(a1_off[i + 1] - a1_off[i]);
            }
            a1_sizes.Add(result_el_a1_size * TileDB.EnumUtil.datatype_size(TileDB.DataType.TILEDB_CHAR) - a1_off[(int)result_el_a1_off - 1]);

            List<string> a1_str = new List<string>();
            for (int i = 0; i < (int)result_el_a1_off; ++i)
            {
                string temp = new string(a1_data.GetRange((int)a1_off[i], (int)a1_sizes[i]).ToArray());
                a1_str.Add(temp);
            }

            //a2 query result
            UInt64 result_el_a2_off = buffer_elements["a2"][0];
            UInt64 result_el_a2_size = buffer_elements["a2"][1];

            TileDB.VectorUInt64 a2_el_off = new TileDB.VectorUInt64();
            UInt64 a2_el_size = TileDB.EnumUtil.datatype_size(TileDB.DataType.TILEDB_INT32);
            for (int i = 0; i < (int)result_el_a2_off; ++i)
            {
                a2_el_off.Add(a2_off[i] / a2_el_size);
            }

            //Get the number of elements per cell value
            TileDB.VectorUInt64 a2_cell_el = new TileDB.VectorUInt64();
            for (int i = 0; i < ((int)result_el_a2_off - 1); ++i)
            {
                a2_cell_el.Add(a2_el_off[i + 1] - a2_el_off[i]);
            }
            a2_cell_el.Add(result_el_a2_size - a2_el_off[(int)result_el_a2_off - 1]);

            //Print
            for (int i = 0; i < (int)result_el_a1_off; ++i)
            {
                System.Console.Write("a1:{0},a2:", a1_str[i]);
                for (int j = 0; j < (int)a2_cell_el[i]; ++j)
                {
                    System.Console.Write("{0} ", a2_data[(int)a2_el_off[i] + j]);
                }
                System.Console.WriteLine();
            }
 
            System.Console.WriteLine();
            return status;
        }//private TileDB.QueryStatus ReadArray()
        #endregion


    }

}
