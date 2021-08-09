
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
    public class ExampleArraySchema
    {
        public static void Main(string[] args)
        {

            TileDB.Context ctx = new TileDB.Context();//Create context
            TileDB.Domain dom = new TileDB.Domain(ctx);//Create domain
            dom.add_int32_dimension("rows", 1, 4, 4); //Add an int32 dimension
            dom.add_int32_dimension("cols", 1, 4, 4); //Add an int32 dimension

            // Create an dense array schema
            TileDB.ArraySchema schema = new TileDB.ArraySchema(ctx, TileDB.ArrayType.TILEDB_DENSE);
            schema.set_domain(dom);

            // Create and add an int attribute
            TileDB.Attribute attr1 = TileDB.Attribute.create_attribute(ctx, "a", TileDB.DataType.TILEDB_INT32);
            schema.add_attribute(attr1);

            try
            {
                schema.check();
            }
            catch(TileDB.TileDBError tdbe)
            {
                Console.WriteLine(tdbe.Message);
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
            }

            // Dump schema to file
            schema.dump("temp.schema");


            return;
        }
    }

}
