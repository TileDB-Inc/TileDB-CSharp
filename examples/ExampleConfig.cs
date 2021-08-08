
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
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace TileDB.Example
{
    public class ExampleConfig
    {
        public static void Run()
        {
            const string tempConfig = "temp.cfg";

            TileDB.Config config = new TileDB.Config();

            // Set values
            config.set("sm.memory_budget", "10737418240");
            config.set("vfs.s3.connect_timeout_ms", "5000");
            config.set("vfs.s3.endpoint_override", "localhost:8888");

            // Get values
            string memory_budget = config.get("sm.memory_budget");
            Console.WriteLine("memory_budget:{0}", memory_budget);

            // Save to a file
            config.save_to_file(tempConfig);

            // Assign a config object to a context
            TileDB.Context ctx = new TileDB.Context(config);

            File.Delete(tempConfig);

            return;
        }
    }

}
