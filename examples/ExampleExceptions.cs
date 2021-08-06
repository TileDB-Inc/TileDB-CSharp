
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
    public class ExampleArrayConsolidate
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start to read/consolidate a non-existing array...");
            // Create a config
            TileDB.Config config = new TileDB.Config();

            // Create a context 
            TileDB.Context ctx = new TileDB.Context(config);

            string array_uri = "test_nonexist_array";

            try
            {
                // Consolidate the array
                var arr = new TileDB.Array(ctx, array_uri, TileDB.QueryType.TILEDB_READ);
            }
            catch (Exception e)
            {
                System.Console.WriteLine("got exception when reading:");
                System.Console.WriteLine(e.Message);
            }


            try
            {
                // Consolidate the array
                TileDB.Array.consolidate(ctx, array_uri, config);

                // Vacuum the array
                TileDB.Array.vacuum(ctx, array_uri);
            }
            catch (Exception e)
            {
                System.Console.WriteLine("got exception when consolidating:");
                System.Console.WriteLine(e.Message);
            }

            System.Console.WriteLine("Finished reading and consolidating an array!");

            return;
        }

    }

}