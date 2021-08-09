
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
        public static void Main(string[] args)
        {
            System.Console.WriteLine("Start to consolidate an array...");

            // Create a config
            TileDB.Config config = new TileDB.Config();

            // Create a context
            TileDB.Context context = new TileDB.Context(config);

            string array_uri = "test_array";

            try
            {
                // Consolidate the array
                TileDB.Array.consolidate(context, array_uri, config);

                // Vacuum the array
                TileDB.Array.vacuum(context, array_uri);
            }
            catch(TileDB.TileDBError tdbe) {
                System.Console.WriteLine(tdbe.Message);
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
            }

            System.Console.WriteLine("Finished consolidating an array!");

            return;
        }

    }

}
