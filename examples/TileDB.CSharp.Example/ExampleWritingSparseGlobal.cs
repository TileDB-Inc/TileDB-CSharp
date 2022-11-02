using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TileDB.CSharp.Examples
{
    public static class ExampleWritingSparseGlobal
    {
        private static readonly string ArrayPath = ExampleUtil.MakeExamplePath("writing-sparse-global");
        private static readonly Context Ctx = Context.GetDefault();

        private static void CreateArray()
        {
            using var domain = new Domain(Ctx);
            // Create a 4x4 array with 2x2 tile dimensions; Total of 16 cells, 4 tiles, 4 cells each tile
            // + Same layout seen here: https://docs.tiledb.com/main/background/key-concepts-and-data-format#indexing
            domain.AddDimension(Dimension.Create(Ctx, "rows", 1, 4, 2));
            domain.AddDimension(Dimension.Create(Ctx, "cols", 1, 4, 2));

            using var schema = new ArraySchema(Ctx, ArrayType.Sparse);
            Console.WriteLine($"Tile order: {schema.TileOrder()}; Cell order: {schema.CellOrder()}");
            schema.SetDomain(domain);
            schema.AddAttribute(Attribute.Create<int>(Ctx, "a1"));

            Array.Create(Ctx, ArrayPath, schema);
        }

        private static async Task WriteArrayAsync()
        {
            using var array = new Array(Ctx, ArrayPath);
            array.Open(QueryType.Write);
            using var queryWrite = new Query(Ctx, array, QueryType.Write);
            queryWrite.SetLayout(LayoutType.GlobalOrder);

            // Coordinates for global order writes must be provided in-order
            // + We may not write to (2, 1) before (1, 1); (2, 1) comes after (1, 1) in global order
            // + We will write 3 values at (1, 1), (2, 1), (2, 4)
            queryWrite.SetDataBuffer("rows", new[] { 1, 2, 2 });
            queryWrite.SetDataBuffer("cols", new[] { 1, 1, 4 });
            queryWrite.SetDataBuffer("a1", new[] { 1, 2, 3 });
            await queryWrite.SubmitAsync();
            Console.WriteLine($"Write query #1 status: {queryWrite.Status()}");

            // Write 3 more value at coordinates (3, 1), (4, 1), and (4, 4)
            queryWrite.SetDataBuffer("rows", new[] { 3, 4, 4 });
            queryWrite.SetDataBuffer("cols", new[] { 1, 1, 4 });
            queryWrite.SetDataBuffer("a1", new[] { 4, 5, 6 });
            await queryWrite.SubmitAsync();
            Console.WriteLine($"Write query #2 status: {queryWrite.Status()}");

            // Important: Global order writes must call Query.FinalizeQuery()
            queryWrite.FinalizeQuery();
            array.Close();
        }

        private static async Task ReadArrayAsync()
        {
            using var array = new Array(Ctx, ArrayPath);
            array.Open(QueryType.Read);
            using var readQuery = new Query(Ctx, array, QueryType.Read);
            readQuery.SetSubarray(new[] { 1, 4, 1, 4 });

            var rowsRead = new int[6];
            var colsRead = new int[6];
            var a1Read = new int[6];
            readQuery.SetDataBuffer("rows", rowsRead);
            readQuery.SetDataBuffer("cols", colsRead);
            readQuery.SetDataBuffer("a1", a1Read);
            await readQuery.SubmitAsync();
            Console.WriteLine($"Read query status: {readQuery.Status()}");
            var coordsRead = rowsRead.Zip(colsRead);
            var dataRead = coordsRead.Zip(a1Read);
            Console.WriteLine($"a1 data: {string.Join(", ", dataRead)}");

            array.Close();
        }

        public static async Task RunAsync()
        {
            if (Directory.Exists(ArrayPath))
            {
                Directory.Delete(ArrayPath, true);
            }

            CreateArray();
            await WriteArrayAsync();
            await ReadArrayAsync();
        }
    }
}
