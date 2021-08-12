
/*
 * The MIT License (MIT)
 *
 * Copyright (c) 2021 TileDB, Inc.
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
using System.Linq;
using System.Collections.Generic;

namespace TileDB.Example
{
    public class ExampleDenseArray
    {
        public static void Main(string[] args)
        {
            CreateAzureArray();
            WriteAzureArray();
            ReadAzureArray();
        }

        private static TileDB.Config config() {
            TileDB.Config config = new TileDB.Config();

            /** set the AZURE_STORAGE_{ACCOUNT, KEY} environment variables **/
            /** or configure here:                                         **/
            config.set("vfs.azure.storage_account_name", "");
            config.set("vfs.azure.storage_account_key", "");

            /** set SAS token, if applicable:                              **/
            //config.set("vfs.azure.storage_sas_token", "<SAS token>");

            return config;
        }

        private static String array_uri_ = "azure://<container>/path";

        private static void CreateAzureArray()
        {
            TileDB.Context ctx = new TileDB.Context(config());
            TileDB.Domain dom = new TileDB.Domain(ctx);
            dom.add_int64_dimension("rows", 1, 4, 4);
            dom.add_int64_dimension("cols", 1, 4, 4);

            TileDB.ArraySchema schema = new TileDB.ArraySchema(ctx, TileDB.ArrayType.TILEDB_DENSE);
            schema.set_domain(dom);
            TileDB.Attribute attr1 = TileDB.Attribute.create_attribute(ctx, "a", TileDB.DataType.TILEDB_INT64);
            schema.add_attribute(attr1);

            //delete array if it already exists
            TileDB.VFS vfs = new TileDB.VFS(ctx);
            if (vfs.is_dir(array_uri_))
            {
                vfs.remove_dir(array_uri_);
            }

            //create array
            TileDB.Array.create(array_uri_, schema);
        }

        private static TileDB.QueryStatus WriteAzureArray()
        {
            TileDB.Context ctx = new TileDB.Context(config());

            TileDB.VectorInt64 data = new TileDB.VectorInt64();
            for (int i = 1; i <= 16; ++i)
            {
                data.Add(i);
            }

            //open array for write
            TileDB.Array array = new TileDB.Array(ctx, array_uri_, TileDB.QueryType.TILEDB_WRITE);
            TileDB.Query query = new TileDB.Query(ctx, array, TileDB.QueryType.TILEDB_WRITE);
            query.set_layout(TileDB.LayoutType.TILEDB_ROW_MAJOR);
            query.set_int64_vector_buffer("a", data);

            TileDB.QueryStatus status = query.submit();
            array.close();

            return status;
        }

        private static TileDB.QueryStatus ReadAzureArray()
        {
            TileDB.Context ctx = new TileDB.Context(config());

            TileDB.VectorInt64 subarray = new TileDB.VectorInt64();
            subarray.Add(1);
            subarray.Add(2);
            subarray.Add(2);
            subarray.Add(4);

            TileDB.VectorInt64 data = TileDB.VectorInt64.Repeat(0, 6);

            //open array for read
            TileDB.Array array = new TileDB.Array(ctx, array_uri_, TileDB.QueryType.TILEDB_READ);
            TileDB.QueryStatus status = TileDB.QueryStatus.TILEDB_COMPLETED;

            //query
            try {
                TileDB.Query query = new TileDB.Query(ctx, array, TileDB.QueryType.TILEDB_READ);
                query.set_layout(TileDB.LayoutType.TILEDB_ROW_MAJOR);
                query.set_int64_subarray(subarray);
                query.set_int64_vector_buffer("a", data);
                status = query.submit();
            } catch (TileDBError e) {
                Console.WriteLine("Unexpected error: " + e.Message);
            }

            array.close();

            Console.WriteLine(
                "data: " + String.Join(" ", data.ToArray().Take(data.Count))
            );

            Console.WriteLine("done");

            return status;
        }
    }
}
