
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
    public class ExampleNullableAttribute
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Start to create a simple dense array...");

            // Create a dense array
            CreateArray();

            // Write the array
            TileDB.QueryStatus status_write = WriteArray();
            if (status_write == TileDB.QueryStatus.TILEDB_FAILED)
            {
                Console.WriteLine("Failed to write array!");
            }
            else
            {
                Console.WriteLine("Finished writing array.");
            }

            // Read the array
            TileDB.QueryStatus status_read = ReadArray();
            if (status_read == TileDB.QueryStatus.TILEDB_FAILED)
            {
                Console.WriteLine("Failed to read array!");
            }
            else
            {
                Console.WriteLine("Finished reading array.");
            }

            return;
        }

        private static String array_uri_ = "nullable_attributes_array";

        #region Array
        private static void CreateArray()
        {
            TileDB.Context ctx = new TileDB.Context();
            TileDB.Domain dom = new TileDB.Domain(ctx);
            // The array will be 2x2 with dimensions "rows" and "cols", with domain [1,2]
            dom.add_int32_dimension("rows", 1, 2, 2);
            dom.add_int32_dimension("cols", 1, 2, 2);

            TileDB.ArraySchema schema = new TileDB.ArraySchema(ctx, TileDB.ArrayType.TILEDB_DENSE);
            schema.set_domain(dom);
            schema.set_order(TileDB.LayoutType.TILEDB_ROW_MAJOR, TileDB.LayoutType.TILEDB_ROW_MAJOR);

            TileDB.Attribute attr1 = TileDB.Attribute.create_attribute(ctx, "a1", TileDB.DataType.TILEDB_INT32);
            attr1.set_nullable(true);
            schema.add_attribute(attr1);

            //vector of int, var-sized
            TileDB.Attribute attr2 = TileDB.Attribute.create_vector_attribute(ctx, "a2", TileDB.DataType.TILEDB_INT32);
            attr2.set_nullable(true);
            schema.add_attribute(attr2);

            TileDB.Attribute attr3 = TileDB.Attribute.create_attribute(ctx, "a3", TileDB.DataType.TILEDB_STRING_ASCII);
            attr3.set_nullable(true);
            schema.add_attribute(attr3);

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

            TileDB.VectorInt32 a1_data = new VectorInt32(new int[4] { 100, 200, 300, 400 });
            TileDB.VectorInt32 a2_data = new VectorInt32(new int[8] { 10, 10, 20, 30, 30, 30, 40, 40 });
            TileDB.VectorUInt64 a2_el_off = new VectorUInt64(new UInt64[4] { 0, 2, 3, 6 });
            TileDB.VectorUInt64 a2_off = new VectorUInt64();
            for (int i = 0; i < a2_el_off.Count; ++i) {
                a2_off.Add(a2_el_off[i] * TileDB.EnumUtil.datatype_size(TileDB.DataType.TILEDB_INT32));
            }

            TileDB.VectorChar a3_data = new VectorChar(new char[9] { 'a', 'b', 'c', 'd', 'e', 'w', 'x', 'y', 'z' });
            TileDB.VectorUInt64 a3_el_off = new VectorUInt64(new UInt64[4] { 0, 3, 4, 5 });
            TileDB.VectorUInt64 a3_off = new VectorUInt64();
            for (int i = 0; i < a3_el_off.Count; ++i) {
                //donot use sizeof(char), since char in csharp is two bytes for UTF
                a3_off.Add(a3_el_off[i] * TileDB.EnumUtil.datatype_size(TileDB.DataType.TILEDB_CHAR));
            }

            //validity buffer
            TileDB.VectorUInt8 a1_validity = new VectorUInt8(new byte[4] { 1, 0, 0, 1 });
            TileDB.VectorUInt8 a2_validity = new VectorUInt8(new byte[4] { 0, 1, 1, 0 });
            TileDB.VectorUInt8 a3_validity = new VectorUInt8(new byte[4] { 1, 0, 0, 1 });

            TileDB.QueryStatus status = TileDB.QueryStatus.TILEDB_UNINITIALIZED;

            try
            {
                //open array for write
                TileDB.Array array = new TileDB.Array(ctx, array_uri_, TileDB.QueryType.TILEDB_WRITE);
                TileDB.Query query = new TileDB.Query(ctx, array, TileDB.QueryType.TILEDB_WRITE);
                query.set_layout(TileDB.LayoutType.TILEDB_ROW_MAJOR);

                //set query buffer
                query.set_int32_vector_buffer_with_validity("a1", a1_data, a1_validity);
                query.set_int32_vector_buffer_with_offsets_validity("a2", a2_data, a2_off, a2_validity);
                query.set_char_vector_buffer_with_offsets_validity("a3", a3_data, a3_off, a3_validity);

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

            // prepar the vector that will hold the results
            TileDB.VectorInt32 a1_data = TileDB.VectorInt32.Repeat(0, 4);
            TileDB.VectorUInt8 a1_validity = TileDB.VectorUInt8.Repeat(0, 4);

            TileDB.VectorInt32 a2_data = TileDB.VectorInt32.Repeat(0, 8);
            TileDB.VectorUInt64 a2_off = TileDB.VectorUInt64.Repeat(0, 4);
            TileDB.VectorUInt8 a2_validity = TileDB.VectorUInt8.Repeat(0, 4);

            TileDB.VectorChar a3_data = TileDB.VectorChar.Repeat(' ', 1000);
            TileDB.VectorUInt64 a3_off = TileDB.VectorUInt64.Repeat(0, 10);
            TileDB.VectorUInt8 a3_validity = TileDB.VectorUInt8.Repeat(0, 10);


            TileDB.VectorInt32 subarray = new TileDB.VectorInt32();
            subarray.Add(1);
            subarray.Add(2); //rows 1,2 all rows
            subarray.Add(1);
            subarray.Add(2); //cols 1,2 all cols

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
                query.set_int32_vector_buffer_with_validity("a1", a1_data, a1_validity);
                query.set_int32_vector_buffer_with_offsets_validity("a2", a2_data, a2_off, a2_validity);
                query.set_char_vector_buffer_with_offsets_validity("a3", a3_data, a3_off, a3_validity);

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
            System.Console.WriteLine("a1_validity:{0}", String.Join("", a1_validity));
            System.Console.WriteLine("a1_buffer_elements:{0}", String.Join(" ", buffer_elements["a1"]));
            System.Console.WriteLine("a1_data:{0}",String.Join(" ", a1_data));

            //output a2. the validitiy is {0,1,1,0}, the output is { NULL },{20},{30,30,30},{ NULL}
            System.Console.WriteLine("a2_validity:{0}", String.Join("", a2_validity));
            System.Console.WriteLine("a2_buffer_elements:{0}", String.Join(" ", buffer_elements["a2"]));
            System.Console.Write("a2_data_masked_with_validity:");
            for (int i = 0; i < a2_validity.Count; ++i) {
                if (i > 0) { System.Console.Write(","); }
                if (a2_validity[i] > 0) {
   
                    UInt64 startidx = a2_off[i]/ TileDB.EnumUtil.datatype_size(TileDB.DataType.TILEDB_INT32);
                    UInt64 endidx = (UInt64)a2_data.Count/ TileDB.EnumUtil.datatype_size(TileDB.DataType.TILEDB_INT32);
                    if (i < (a2_validity.Count-1)) {
                        endidx = a2_off[i + 1]/ TileDB.EnumUtil.datatype_size(TileDB.DataType.TILEDB_INT32);
                    }
                    System.Console.Write("{");
                    System.Console.Write(String.Join(",", a2_data.GetRange((int)startidx, (int)(endidx - startidx))));
                    System.Console.Write("}");
                }
                else
                {
                    System.Console.Write("{ NULL }");
                }
            }
            System.Console.WriteLine();

            //output a3
            System.Console.WriteLine("a3_validity:{0}", String.Join("", a3_validity));
            System.Console.WriteLine("a3_buffer_elements:{0}", String.Join(" ", buffer_elements["a3"]));
            UInt64 a3_offsets_num = buffer_elements["a3"][0];
            UInt64 a3_data_num = buffer_elements["a3"][1];
            System.Console.Write("a3_data:");
            for (UInt64 i=0; i<a3_offsets_num; ++i)
            {
                if (i > 0) { System.Console.Write(","); }
                UInt64 startidx = a3_off[(int)i] / TileDB.EnumUtil.datatype_size(TileDB.DataType.TILEDB_CHAR);
                UInt64 endidx = a3_data_num;
                if (i < (a3_offsets_num-1))
                {
                    endidx = a3_off[(int)(i + 1)] / TileDB.EnumUtil.datatype_size(TileDB.DataType.TILEDB_CHAR);
                }
                string s = new string(a3_data.GetRange((int)startidx, (int)(endidx - startidx)).ToArray());
                System.Console.Write(s);

            }
            System.Console.WriteLine();
            return status;
        }//private TileDB.QueryStatus ReadArray()
        #endregion


    }

}
