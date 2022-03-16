using System;
using System.Text;
using System.Collections.Generic;

namespace TileDB.CSharp
{
    public class File
    {

        #region File Helper Functions
        /// <summary>
        /// Save a local file to a tiledb array.
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="array_uri"></param>
        /// <param name="file"></param>
        /// <param name="mime_type"></param>
        /// <param name="mime_coding"></param>
        public static void SaveFileToArray(Context ctx, string array_uri, string file, string mime_type, string mime_coding)
        {
            try
            {
                if (ctx == null)
                {
                    ctx = Context.GetDefault();
                }
                VFS vfs = new VFS(ctx);
                if (vfs.IsDir(array_uri))
                {
                    vfs.RemoveDir(array_uri);
                }

                byte[] contents = System.IO.File.ReadAllBytes(file);
                ulong nbytes = (ulong)contents.Length;


                ulong tile_extent = 1024;
                if (nbytes > ((ulong)1024 * (ulong)1024 * (ulong)1024 * (ulong)10))
                {
                    tile_extent = (ulong)1024 * (ulong)1024 * (ulong)100;
                }
                else if (nbytes > ((ulong)1024 * 1024 * 100))
                {
                    tile_extent = (ulong)1024 * (ulong)1024;
                }
                else if (nbytes > ((ulong)1024 * (ulong)1024))
                {
                    tile_extent = (ulong)1024 * (ulong)256;
                }
                var array_schema = new ArraySchema(ctx, ArrayType.TILEDB_DENSE);

                ulong[] dim_bound = new ulong[] { 0, ulong.MaxValue - tile_extent - 2 };
                var dim = Dimension.Create<ulong>(ctx, Constants.FILE_DIMENSION_NAME, dim_bound, tile_extent);
                var domain = new Domain(ctx);
                domain.AddDimension(dim);
                array_schema.SetDomain(domain);

                var attr = new Attribute(ctx, "contents", DataType.TILEDB_UINT8);
                attr.SetCellValNum(1);
                array_schema.AddAttribute(attr);

                // create the array
                Array.Create(ctx, array_uri, array_schema);

                using (var array_write = new Array(ctx, array_uri))
                {
                    array_write.Open(QueryType.TILEDB_WRITE);

                    var query_write = new Query(ctx, array_write);
                    query_write.SetLayout(LayoutType.TILEDB_ROW_MAJOR);

                    var subarray = new ulong[2] { 0, nbytes - 1 };
                    query_write.SetSubarray<ulong>(subarray);

                    query_write.SetDataBuffer<byte>(Constants.FILE_ATTRIBUTE_NAME, contents);

                    query_write.Submit();
                    query_write.FinalizeQuery();

                    //save metadata
                    array_write.PutMetadata<ulong>(Constants.METADATA_SIZE_KEY, nbytes);
                    array_write.PutMetadata(Constants.METADATA_ORIGINAL_FILE_NAME, file);
                    string mimetype = string.IsNullOrEmpty(mime_type) ? "None" : mime_type;
                    array_write.PutMetadata(Constants.FILE_METADATA_MIME_TYPE_KEY, mimetype);
                    string mimeencoding = string.IsNullOrEmpty(mime_coding) ? "None" : mime_coding;
                    array_write.PutMetadata(Constants.FILE_METADATA_MIME_ENCODING_KEY, mimeencoding);

                    array_write.Close();
                }
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("CoreUtil.SaveFileToArray, caught exception:");
                System.Console.WriteLine(e.Message);
            }


        }

        /// <summary>
        /// Export a file array to a local file.
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="array_uri"></param>
        /// <param name="file"></param>
        public static void ExportArrayToFile(Context ctx, string array_uri, string file)
        {
            try
            {
                if (ctx == null)
                {
                    ctx = Context.GetDefault();
                }
                using (var array_read = new Array(ctx, array_uri))
                {
                    array_read.Open(QueryType.TILEDB_READ);
                    var query_read = new Query(ctx, array_read);
                    query_read.SetLayout(LayoutType.TILEDB_ROW_MAJOR);

                    var file_size_metadata = array_read.Metadata<ulong>(Constants.METADATA_SIZE_KEY);
                    ulong file_size = file_size_metadata.Item3[0];

                    var orig_file_name = array_read.Metadata(Constants.METADATA_ORIGINAL_FILE_NAME);

                    ulong[] subarray = new ulong[2] { 0, file_size - 1 };
                    query_read.SetSubarray<ulong>(subarray);

                    byte[] data_buffer = new byte[file_size];
                    query_read.SetDataBuffer<byte>(Constants.FILE_ATTRIBUTE_NAME, data_buffer);

                    System.IO.BinaryWriter bw = new System.IO.BinaryWriter(System.IO.File.Open(file, System.IO.FileMode.OpenOrCreate));

                    int loop_zero_sum = 0;
                    while (query_read.Status() != QueryStatus.TILEDB_COMPLETED)
                    {
                        query_read.Submit();

                        var result_buffer_elements = query_read.EstResultSize(Constants.FILE_ATTRIBUTE_NAME);
                        ulong read_size = result_buffer_elements.DataBytesSize;

                        if (read_size > 0)
                        {
                            bw.Write(data_buffer, 0, (int)read_size);
                        }

                        ++loop_zero_sum;
                        if (read_size == 0 && loop_zero_sum > 10)
                        {

                            break;
                        }
                        else
                        {
                            loop_zero_sum = 0;
                        }

                    }

                    array_read.Close();
                }

            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("CoreUtil.ExportArrayToFile, caught exception:");
                System.Console.WriteLine(e.Message);
            }
        }
        #endregion File Helper Functions

    }
}