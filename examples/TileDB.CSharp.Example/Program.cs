using System;
using System.IO;
using TileDB.CSharp;
namespace TileDB.CSharp.Examples;

static class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("TileDB Core Version: {0}", CoreUtil.GetCoreLibVersion());

        ExampleQuery.Run();
        ExampleIncompleteQuery.Run();
        ExampleIncompleteQueryStringDimensions.Run();
        ExampleIncompleteQueryVariableSize.Run();
        ExampleWritingDenseGlobal.Run();
        ExampleWritingSparseGlobal.Run();
        ExampleDataframe.Run();
        ExampleAggregateQuery.Run();

        ExampleFile.RunLocal();
        // ExampleFile.RunCloud("tiledb_api_token", "tiledb_namespace", "new_cloud_array_name", "s3://bucket/prefix/");

        ExampleGroup.RunLocal();
        // ExampleGroup.RunCloud("tiledb_api_token", "tiledb_namespace", "s3://bucket/prefix");
    }
}