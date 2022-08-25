using System;
using System.IO;
using TileDB.CSharp;
namespace TileDB.CSharp.Examples
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("TileDB Core Version: {0}", CoreUtil.GetCoreLibVersion());

            ExampleQuery.Run();
            // ExampleIncompleteQuery.Run();
            // ExampleIncompleteQueryStringDimensions.Run();

            //ExampleFile.RunLocal();
            //ExampleFile.RunCloud();

            //ExampleGroup.RunLocal();
            //ExampleGroup.RunCloud();
        }
    }
}