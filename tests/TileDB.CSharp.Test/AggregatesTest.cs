using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TileDB.CSharp.Test
{
    [TestClass]
    public class AggregatesTest
    {
        [TestMethod]
        public unsafe void Test()
        {
            var context = Context.GetDefault();

            using var tmpArrayPath = new TemporaryDirectory("aggregate_test");

            using var array = new Array(context, tmpArrayPath);
            Assert.IsNotNull(array);

            using var array_schema = BuildArraySchema(context);
            Assert.IsNotNull(array_schema);

            array.Create(array_schema);

            array.Open(QueryType.Write);

            int[] data = new int[10] { 62, 26, 81, 86, 93, 76, 60, 25, 68, 28 };

            using (var query = new Query(array))
            {
                query.SetLayout(LayoutType.RowMajor);
                query.SetDataBuffer("a1", data);

                query.Submit();
            }
            array.Close();

            array.Open(QueryType.Read);
            int[] dataRead = new int[data.Length];
            ulong count;
            long sum;
            double mean;
            using (var query = new Query(array))
            {
                query.SetLayout(LayoutType.RowMajor);
                using var subarray = new Subarray(array);
                subarray.AddRange(0, 1, 10);
                query.SetSubarray(subarray);

                using var channel = query.GetDefaultChannel();
                channel.ApplyAggregate(AggregateOperation.Count, nameof(count));
                channel.ApplyAggregate(AggregateOperation.Unary(AggregateOperator.Sum, "a1"), nameof(sum));
                channel.ApplyAggregate(AggregateOperation.Unary(AggregateOperator.Mean, "a1"), nameof(mean));

                using var countField = query.GetField(nameof(count));
                Assert.AreEqual(nameof(count), countField.Name);
                Assert.AreEqual(DataType.UInt64, countField.DataType);
                Assert.AreEqual(1u, countField.ValuesPerCell);

                query.SetDataBuffer("a1", dataRead);
                query.SetDataBuffer(nameof(count), &count, 1);
                query.SetDataBuffer(nameof(sum), &sum, 1);
                query.SetDataBuffer(nameof(mean), &mean, 1);

                query.Submit();

                CollectionAssert.AreEqual(data, dataRead);
                Assert.AreEqual((ulong)data.Length, count);
                Assert.AreEqual(605, sum);
                Assert.AreEqual(60.5, mean);
            }
        }

        private static ArraySchema BuildArraySchema(Context context)
        {
            var array_schema = new ArraySchema(context, ArrayType.Dense);
            using var domain = new Domain(context);
            using var dimension = Dimension.Create(context, "dim1", 1, 10, 5);
            domain.AddDimension(dimension);
            array_schema.SetDomain(domain);

            using var a1 = new Attribute(context, "a1", DataType.Int32);
            array_schema.AddAttribute(a1);
            array_schema.Check();
            return array_schema;
        }
    }
}
