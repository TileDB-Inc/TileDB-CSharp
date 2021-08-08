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
        public static void Main()
        {
            // This test runs all examples, to exercise them as unit test

            var args = new string[0];
            var exclusions = new string[]{
              // List any test classes which should be exluded here
              // disabled due to ch9421: example aborts due to error
                "ExampleArrayConsolidate"
            };

            // Get list of classes in 'TileDB.Example' namespace
            string target_ns = "TileDB.Example";
            var q = from t in Assembly.GetExecutingAssembly().GetTypes()
                    where t.IsClass && t.Namespace == target_ns && !(exclusions.Any(t.Name.Contains))
                    select t;

            // Run all Main methods for classes in 'TileDB.Example' namespace
            //q.ToList().ForEach(t => t.GetMethod("Main").Invoke(t, new object[] { args }));

            // Run all Run methods for classes in 'TileDB.Example' namespace
            q.ToList().ForEach(t => t.GetMethod("Run").Invoke(t));

        }
    }
}
