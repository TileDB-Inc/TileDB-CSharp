using System;
using Xunit;
using TileDB;
using tdb = TileDB;

using System.Reflection;
using System.Linq;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace test
{
    public class UnitTestExamples
    {
        [Fact]
        public static void Main() {
            // run all examples as a unit test

            var args = new string[0];
            var exclusions = new string[]{
                // disabled due to ch9421: example aborts due to error
                "ExampleArrayConsolidate"
            };
            Console.WriteLine(exclusions.Any("ExampleVersion".Contains));

            string nspace = "TileDB.Example";
            var q = from t in Assembly.GetExecutingAssembly().GetTypes()
                    where t.IsClass && t.Namespace == nspace && !(exclusions.Any(t.Name.Contains))
                    select t;
            q.ToList().ForEach(t => t.GetMethod("Main").Invoke(t, new object[]{args}));

            // disabled due to ch9421: example aborts due to error
            //TileDB.Example.ExampleArrayConsolidate.Main(args);
            //TileDB.Example.ExampleArraySchema.Main(args);
            //TileDB.Example.ExampleConfig.Main(args);
            //TileDB.Example.ExampleDenseArray.Main(args);
            //TileDB.Example.ExampleSparseArray.Main(args);
            //TileDB.Example.ExampleVersion.Main(args);
        }
    }
}
