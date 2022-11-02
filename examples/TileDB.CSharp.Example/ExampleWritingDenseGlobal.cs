using System;
using System.IO;
using System.Threading.Tasks;

namespace TileDB.CSharp.Examples
{
    public static class ExampleWritingDenseGlobal
    {
        private static readonly string ArrayPath = ExampleUtil.MakeExamplePath("writing-dense-global");
        private static readonly Context Ctx = Context.GetDefault();

        private static void CreateArray()
        {
            using var domain = new Domain(Ctx);
            // Create a 4x4 array with 2x2 tile dimensions; Total of 16 cells, 4 tiles, 4 cells each tile
            // + Same layout seen here: https://docs.tiledb.com/main/background/key-concepts-and-data-format#indexing
            domain.AddDimension(Dimension.Create(Ctx, "rows", 1, 4, 2));
            domain.AddDimension(Dimension.Create(Ctx, "cols", 1, 4, 2));

            using var schema = new ArraySchema(Ctx, ArrayType.Dense);
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
            // Slice rows 1-4, columns 1-2 for a total of 8 cells
            queryWrite.AddRange("rows", 1, 4);
            queryWrite.AddRange("cols", 1, 2);

            // Write 4 cells
            queryWrite.SetDataBuffer("a1", new[] { 1, 2, 3, 4 });
            await queryWrite.SubmitAsync();
            Console.WriteLine($"Write query #1 status: {queryWrite.Status()}");

            // Write 4 more cells before we finalize the query
            queryWrite.SetDataBuffer("a1", new[] { 5, 6, 7, 8});
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

            var a1Read = new int[16];
            readQuery.SetDataBuffer("a1", a1Read);
            await readQuery.SubmitAsync();
            Console.WriteLine($"Read query status: {readQuery.Status()}");
            Console.WriteLine($"a1 data: {string.Join(", ", a1Read)}");

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
