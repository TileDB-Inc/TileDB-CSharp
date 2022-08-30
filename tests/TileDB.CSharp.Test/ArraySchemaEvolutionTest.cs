using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TileDB.CSharp.Test
{
    [TestClass]
    public class ArraySchemaEvolutionTest
    {
        private static readonly string ArrayUri = TestUtil.GetTempPath("test-schema-evolution");

        [TestInitialize]
        public void CreateArray()
        {
            var ctx = Context.GetDefault();

            var rows = Dimension.Create(ctx, "rows", new[] { 1, 4 }, 4);
            var cols = Dimension.Create(ctx, "cols", new[] { 1, 4 }, 4);

            var domain = new Domain(ctx);
            domain.AddDimensions(rows, cols);

            var schema = new ArraySchema(ctx, ArrayType.TILEDB_DENSE);
            schema.SetDomain(domain);

            var nullableAttr = new Attribute(ctx, "a1", DataType.TILEDB_INT32);
            nullableAttr.SetNullable(true);
            schema.AddAttributes(nullableAttr, new Attribute(ctx, "a2", DataType.TILEDB_INT32),
                new Attribute(ctx, "a3", DataType.TILEDB_BOOL));
            schema.Check();
            TestUtil.CreateArray(ctx, ArrayUri, schema);
        }

        [TestMethod]
        public void TestSchemaEvolution()
        {
            var ctx = Context.GetDefault();
            var array = new Array(ctx, ArrayUri);
            TestUtil.PrintLocalSchema(ArrayUri);

            array.Open(QueryType.TILEDB_READ);
            // Select existing attribute from schema to delete
            var delAttr = array.Schema().Attributes()["a3"];
            array.Close();

            var schemaEvolution = new ArraySchemaEvolution(ctx);

            var addAttr = new Attribute(ctx, "a4", DataType.TILEDB_FLOAT32);
            schemaEvolution.AddAttribute(addAttr);
            schemaEvolution.DropAttribute("a2");
            schemaEvolution.DropAttribute(delAttr);
            Console.WriteLine("Removing attributes `a2`, `a3`; Adding new attribute `a4`");
            schemaEvolution.EvolveArray(ArrayUri);

            array.Open(QueryType.TILEDB_READ);
            var schema = array.Schema();
            TestUtil.PrintLocalSchema(ArrayUri);
            Assert.IsTrue(schema.HasAttribute("a1"));
            Assert.IsFalse(schema.HasAttribute("a2"));
            Assert.IsFalse(schema.HasAttribute("a3"));
            Assert.IsTrue(schema.HasAttribute("a4"));
            array.Close();

            schemaEvolution = new ArraySchemaEvolution(ctx);
            schemaEvolution.DropAttribute("a1");
            Console.WriteLine("Removing attribute `a1` using Array.Evolve");
            array.Evolve(ctx, schemaEvolution);

            array.Open(QueryType.TILEDB_READ);
            schema = array.Schema();
            Assert.IsFalse(schema.HasAttribute("a1"));
            TestUtil.PrintLocalSchema(ArrayUri);
            array.Close();
        }
    }
}