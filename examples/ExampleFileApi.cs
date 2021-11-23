
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
    public class ExampleFileApi
    {
        public static void Main(string[] args)
        {
            if (System.IO.File.Exists("Sample.png"))
            {
                Console.WriteLine("Start to save Sample.png to a tiledb array...");

                SaveLocalFileToTileDBArray();

                Console.WriteLine("Start to export to Sample_exported.png from a tiledb array...");
                ExportLocalTileDBArrayToFile();

                Console.WriteLine("Start to save Sample.png to s3://tiledb-bin/sample_tdb...");
                SaveFileToS3TileDBArray();
            }

            ExportS3TileDBArrayToFile();

            return;
        }


        #region File Array

        private static void SaveLocalFileToTileDBArray()
        {
            TileDB.Context ctx = new TileDB.Context();
            TileDB.ArrayUtil.save_file_from_path("test_file_array", "Sample.png", "", "", ctx);
        }
        private static void ExportLocalTileDBArrayToFile()
        {
            TileDB.Context ctx = new TileDB.Context();
            TileDB.ArrayUtil.export_file_to_path("test_file_array", "Sample_exported.png", 0, ctx);
        }

        private static void SaveFileToS3TileDBArray()
        {
            TileDB.Config cfg = new Config();
            cfg.set("vfs.s3.region", "us-east-2");
            TileDB.Context ctx = new TileDB.Context(cfg);

            String s3_uri_to_save = "s3://tiledb-bin/sample_tdb";

            TileDB.VFS vfs = new VFS(ctx);
            if(vfs.is_dir(s3_uri_to_save))
            {
                vfs.remove_dir(s3_uri_to_save);
            }

            TileDB.ArrayUtil.save_file_from_path(s3_uri_to_save, "Sample.png", "", "", ctx);
        }

        private static void ExportS3TileDBArrayToFile()
        {
            TileDB.Config cfg = new Config();
            cfg.set("vfs.s3.region", "us-east-2");
            TileDB.Context ctx = new TileDB.Context(cfg);
            
            TileDB.ArrayUtil.export_file_to_path("s3://stefan-region-test/image_tdb", "image_exported.tiff", 0, ctx);
        }
 

        #endregion File Array

 
    }

}
