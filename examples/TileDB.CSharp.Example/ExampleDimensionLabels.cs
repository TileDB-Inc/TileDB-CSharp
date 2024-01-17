using System;
using System.IO;
using System.Linq;

namespace TileDB.CSharp.Examples
{
    public static class ExampleDimensionLabels
    {
        private static readonly string ArrayPath = ExampleUtil.MakeExamplePath("dimension-labels");
        private static readonly Context Ctx = Context.GetDefault();

        private static void CreateArray()
        {
            using var xIndex = Dimension.Create(Ctx, "x_index", 0, 5, 6);
            using var sample = Dimension.Create(Ctx, "sample", 0, 3, 4);

            using var domain = new Domain(Ctx);
            domain.AddDimensions(xIndex, sample);

            using var a = Attribute.Create<short>(Ctx, "a");

            using var schema = new ArraySchema(Ctx, ArrayType.Dense);
            schema.SetCellOrder(LayoutType.RowMajor);
            schema.SetTileOrder(LayoutType.RowMajor);
            schema.SetDomain(domain);
            schema.AddAttribute(a);
            schema.AddDimensionLabel(0, "x", DataOrder.Increasing, DataType.Float64);
            schema.AddDimensionLabel(0, "y", DataOrder.Increasing, DataType.Float64);
            schema.AddDimensionLabel(1, "timestamp", DataOrder.Increasing, DataType.DateTimeSecond);

            Array.Create(Ctx, ArrayPath, schema);
        }

        private static void WriteArrayAndLabels()
        {
            short[] a = Enumerable.Range(1, 24).Select(x => (short)x).ToArray();
            double[] x = { -1.0, -0.6, -0.2, 0.2, 0.6, 1.0 };
            double[] y = { 0.0, 2.0, 4.0, 6.0, 8.0, 10.0 };
            long[] timestamp = { 31943, 32380, 33131, 33228 };

            using Array array = new Array(Ctx, ArrayPath);
            array.Open(QueryType.Write);

            using Query query = new Query(Ctx, array);
            query.SetLayout(LayoutType.RowMajor);
            query.SetDataBuffer("a", a);
            query.SetDataBuffer("x", x);
            query.SetDataBuffer("y", y);
            query.SetDataBuffer("timestamp", timestamp);

            query.Submit();

            if (query.Status() != QueryStatus.Completed)
            {
                throw new Exception("Write query did not complete.");
            }

            array.Close();
        }

        private static void ReadArrayAndLabels()
        {
            Console.WriteLine("Read from main array");

            using var array = new Array(Ctx, ArrayPath);
            array.Open(QueryType.Read);

            using var subarray = new Subarray(array);
            subarray.AddRange(0, 1, 2);
            subarray.AddRange(1, 0, 2);

            short[] a = new short[6];
            double[] x = new double[2];
            double[] y = new double[2];
            long[] timestamp = new long[3];

            using var query = new Query(Ctx, array);
            query.SetLayout(LayoutType.RowMajor);
            query.SetSubarray(subarray);
            query.SetDataBuffer("a", a);
            query.SetDataBuffer("x", x);
            query.SetDataBuffer("y", y);
            query.SetDataBuffer("timestamp", timestamp);

            query.Submit();

            if (query.Status() != QueryStatus.Completed)
            {
                throw new Exception("Read query did not complete.");
            }

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    int x_val = i + 1;
                    int sample_val = j;
                    Console.WriteLine($"Cell ({x_val}, {sample_val})");
                    Console.WriteLine($"    * a({x_val}, {sample_val}) = {a[3 * i + j]}");
                    Console.WriteLine($"    * x({x_val}) = {x[i]}");
                    Console.WriteLine($"    * y({x_val}) = {y[i]}");
                    Console.WriteLine($"    * timestamp({sample_val}) = {TimeSpan.FromSeconds(timestamp[j])}");
                }
            }

            array.Close();
        }

        private static void ReadTimestampData()
        {
            Console.WriteLine("Read from dimension label");

            using var array = new Array(Ctx, ArrayPath);
            array.Open(QueryType.Read);

            using var subarray = new Subarray(array);
            subarray.AddRange(1, 1, 3);

            long[] timestamp = new long[3];

            using var query = new Query(Ctx, array);
            query.SetLayout(LayoutType.RowMajor);
            query.SetSubarray(subarray);
            query.SetDataBuffer("timestamp", timestamp);

            query.Submit();

            if (query.Status() != QueryStatus.Completed)
            {
                throw new Exception("Read query did not complete.");
            }

            for (int i = 0; i < 3; i++)
            {
                int sample_val = i + 1;
                Console.WriteLine($"Cell ({sample_val})");
                Console.WriteLine($"    * timestamp({sample_val}) = {TimeSpan.FromSeconds(timestamp[i])}");
            }

            array.Close();
        }

        private static void ReadArrayByLabel()
        {
            Console.WriteLine("Read array from label ranges");

            using var array = new Array(Ctx, ArrayPath);
            array.Open(QueryType.Read);

            using var subarray = new Subarray(array);
            subarray.AddLabelRange("y", 3.0, 8.0);
            subarray.AddLabelRange<long>("timestamp", 31943, 32380);

            short[] a = new short[6];
            double[] y = new double[3];
            long[] timestamp = new long[2];

            using var query = new Query(Ctx, array);
            query.SetLayout(LayoutType.RowMajor);
            query.SetSubarray(subarray);
            query.SetDataBuffer("y", y);
            query.SetDataBuffer("timestamp", timestamp);
            query.SetDataBuffer("a", a);

            query.Submit();

            if (query.Status() != QueryStatus.Completed)
            {
                throw new Exception("Read query did not complete.");
            }

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    Console.WriteLine($"Cell ({y[i]}, {TimeSpan.FromSeconds(timestamp[j])})");
                    Console.WriteLine($"    * a = {a[2 * i + j]}");
                }
            }

            array.Close();
        }

        public static void Run()
        {
            if (Directory.Exists(ArrayPath))
            {
                Directory.Delete(ArrayPath, true);
            }

            CreateArray();
            WriteArrayAndLabels();
            ReadArrayAndLabels();
            Console.WriteLine();
            ReadTimestampData();
            Console.WriteLine();
            ReadArrayByLabel();
            Console.WriteLine();
        }
    }
}
