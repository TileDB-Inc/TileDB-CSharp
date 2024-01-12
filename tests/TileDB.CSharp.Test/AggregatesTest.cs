using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TileDB.CSharp.Test;

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

        int[] data = [62, 26, 81, 86, 93, 0, 60, 25, 68, 28];
        byte[] validity = [1, 1, 1, 1, 1, 0, 1, 1, 1, 1];

        using (var query = new Query(array))
        {
            query.SetLayout(LayoutType.RowMajor);
            query.SetDataBuffer("a1", data);
            query.SetValidityBuffer("a1", validity);

            query.Submit();
        }
        array.Close();

        array.Open(QueryType.Read);
        int[] dataRead = new int[data.Length];
        byte[] validityRead = new byte[data.Length];
        ulong count, nullCount;
        long sum;
        double mean;
        int min, max;
        byte sumValidity, meanValidity, minValidity, maxValidity;
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
            channel.ApplyAggregate(AggregateOperation.Unary(AggregateOperator.Min, "a1"), nameof(min));
            channel.ApplyAggregate(AggregateOperation.Unary(AggregateOperator.Max, "a1"), nameof(max));
            channel.ApplyAggregate(AggregateOperation.Unary(AggregateOperator.NullCount, "a1"), nameof(nullCount));

            using var countField = query.GetField(nameof(count));
            Assert.AreEqual(nameof(count), countField.Name);
            Assert.AreEqual(DataType.UInt64, countField.DataType);
            Assert.AreEqual(1u, countField.ValuesPerCell);
            Assert.AreEqual(QueryFieldOrigin.Aggregate, countField.Origin);

            query.SetDataBuffer("a1", dataRead);
            query.SetValidityBuffer("a1", validityRead);
            query.SetDataBuffer(nameof(count), &count, 1);
            query.SetDataBuffer(nameof(sum), &sum, 1);
            query.SetValidityBuffer(nameof(sum), &sumValidity, 1);
            query.SetDataBuffer(nameof(mean), &mean, 1);
            query.SetValidityBuffer(nameof(mean), &meanValidity, 1);
            query.SetDataBuffer(nameof(min), &min, 1);
            query.SetValidityBuffer(nameof(min), &minValidity, 1);
            query.SetDataBuffer(nameof(max), &max, 1);
            query.SetValidityBuffer(nameof(max), &maxValidity, 1);
            query.SetDataBuffer(nameof(nullCount), &nullCount, 1);

            query.Submit();

            CollectionAssert.AreEqual(data, dataRead);
            CollectionAssert.AreEqual(validity, validityRead);
            Assert.AreEqual((ulong)data.Length, count);
            Assert.AreEqual(529, sum);
            Assert.AreEqual(58.8, mean, 0.1);
            Assert.AreEqual(25, min);
            Assert.AreEqual(93, max);
            Assert.AreEqual(1, sumValidity);
            Assert.AreEqual(1, meanValidity);
            Assert.AreEqual(1, minValidity);
            Assert.AreEqual(1, maxValidity);
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
        a1.SetNullable(true);
        array_schema.AddAttribute(a1);
        array_schema.Check();
        return array_schema;
    }
}
