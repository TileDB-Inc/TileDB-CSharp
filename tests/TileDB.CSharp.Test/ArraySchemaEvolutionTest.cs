using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TileDB.CSharp.Test;

[TestClass]
public class ArraySchemaEvolutionTest
{
    private static readonly string ArrayUri = TestUtil.MakeTestPath("test-schema-evolution");

    [TestInitialize]
    public void CreateArray()
    {
        var ctx = Context.GetDefault();

        var rows = Dimension.Create(ctx, "rows", 1, 4, 4);
        var cols = Dimension.Create(ctx, "cols", 1, 4, 4);

        var domain = new Domain(ctx);
        domain.AddDimensions(rows, cols);

        var schema = new ArraySchema(ctx, ArrayType.Dense);
        schema.SetDomain(domain);

        var nullableAttr = new Attribute(ctx, "a1", DataType.Int32);
        nullableAttr.SetNullable(true);
        schema.AddAttributes(nullableAttr, new Attribute(ctx, "a2", DataType.Int32),
            new Attribute(ctx, "a3", DataType.Boolean));
        schema.Check();
        TestUtil.CreateArray(ctx, ArrayUri, schema);
    }

    [TestMethod]
    public void TestSchemaEvolution()
    {
        var ctx = Context.GetDefault();
        var array = new Array(ctx, ArrayUri);
        TestUtil.PrintLocalSchema(ArrayUri);

        array.Open(QueryType.Read);
        // Select existing attribute from schema to delete
        var delAttr = array.Schema().Attributes()["a3"];
        array.Close();

        var schemaEvolution = new ArraySchemaEvolution(ctx);

        var addAttr = new Attribute(ctx, "a4", DataType.Float32);
        schemaEvolution.AddAttribute(addAttr);
        schemaEvolution.DropAttribute("a2");
        schemaEvolution.DropAttribute(delAttr);
        Console.WriteLine("Removing attributes `a2`, `a3`; Adding new attribute `a4`");
        schemaEvolution.EvolveArray(ArrayUri);

        array.Open(QueryType.Read);
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
        array.Evolve(schemaEvolution);

        array.Open(QueryType.Read);
        schema = array.Schema();
        Assert.IsFalse(schema.HasAttribute("a1"));
        TestUtil.PrintLocalSchema(ArrayUri);
        array.Close();
    }
}
