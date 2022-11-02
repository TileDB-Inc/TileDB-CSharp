using System;
using System.Threading.Tasks;

namespace TileDB.CSharp.Examples
{
    static class Program
    {
        static async Task Main()
        {
            Console.WriteLine("TileDB Core Version: {0}", CoreUtil.GetCoreLibVersion());

            await ExampleQuery.RunAsync();
            await ExampleIncompleteQuery.RunAsync();
            await ExampleIncompleteQueryStringDimensions.RunAsync();
            await ExampleWritingDenseGlobal.RunAsync();
            await ExampleWritingSparseGlobal.RunAsync();

            ExampleFile.RunLocal();
            // ExampleFile.RunCloud("tiledb_api_token", "tiledb_namespace", "new_cloud_array_name", "s3://bucket/prefix/");

            ExampleGroup.RunLocal();
            // ExampleGroup.RunCloud("tiledb_api_token", "tiledb_namespace", "s3://bucket/prefix");
        }
    }
}
