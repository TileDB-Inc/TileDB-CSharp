using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TileDB.CSharp.Test;

	[TestClass]
	public class ArraySchemaTest
	{
		[TestMethod]
		public void TestArraySchema()
		{
			var context = Context.GetDefault();

			var dimension = Dimension.Create(context, "test", 1, 10, 5);
			Assert.IsNotNull(dimension);

			var domain = new Domain(context);
			Assert.IsNotNull(domain);

			domain.AddDimension(dimension);

			var array_schema = new ArraySchema(context, ArrayType.Dense);
			Assert.IsNotNull(array_schema);

			Assert.IsFalse(array_schema.AllowsDups());

			var attribute = new Attribute(context, "a1", DataType.Int32);
			Assert.IsNotNull(attribute);

			array_schema.AddAttribute(attribute);

			Assert.AreEqual((uint)1, array_schema.AttributeNum());

			var attribute_from_index = array_schema.Attribute(0);
			Assert.AreEqual("a1", attribute_from_index.Name());

			var attribute_from_name = array_schema.Attribute("a1");
			Assert.AreEqual("a1", attribute_from_name.Name());

			Assert.IsTrue(array_schema.HasAttribute("a1"));

			array_schema.SetCapacity(100);
			Assert.AreEqual((ulong)100, array_schema.Capacity());

			array_schema.SetDomain(domain);

			var d = array_schema.Domain();
			Assert.IsNotNull(d);

			Assert.AreEqual((ulong)1, d.NDim());
			Assert.AreEqual(DataType.Int32, d.Type());

			array_schema.SetCellOrder(LayoutType.GlobalOrder);
			Assert.AreEqual(LayoutType.GlobalOrder, array_schema.CellOrder());

			array_schema.SetTileOrder(LayoutType.ColumnMajor);
			Assert.AreEqual(LayoutType.ColumnMajor, array_schema.TileOrder());

			using var filter = new Filter(context, FilterType.Bzip2);
			Assert.AreEqual(FilterType.Bzip2, filter.FilterType());

			using var filter_list = new FilterList(context);
			filter_list.AddFilter(filter);

			array_schema.SetCoordsFilterList(filter_list);
			var filter_list_return = array_schema.CoordsFilterList();

			var filter0 = filter_list_return.Filter(0);
			Assert.AreEqual(FilterType.Bzip2, filter0.FilterType());

        array_schema.SetOffsetsFilterList(filter_list);
        filter_list_return = array_schema.OffsetsFilterList();

        filter0 = filter_list_return.Filter(0);
        Assert.AreEqual(FilterType.Bzip2, filter0.FilterType());

        array_schema.SetValidityFilterList(filter_list);
        filter_list_return = array_schema.ValidityFilterList();

        filter0 = filter_list_return.Filter(0);
        Assert.AreEqual(FilterType.Bzip2, filter0.FilterType());

        array_schema.Check();
		}

		[TestMethod]
		public void TestArraySchemaInt32Simple()
    {
			var context = Context.GetDefault();
			var domain = new Domain(context);
			Assert.IsNotNull(domain);

			var dim_rows = Dimension.Create(context, "rows", 0, 4, 4);
			Assert.IsNotNull(dim_rows);
			domain.AddDimension(dim_rows);

			var dim_cols = Dimension.Create(context, "cols", 0, 4, 4);
			Assert.IsNotNull(dim_cols);
			domain.AddDimension(dim_cols);

			var array_schema = new ArraySchema(context, ArrayType.Dense);
			Assert.IsNotNull(array_schema);

			var attr1 = new Attribute(context, "a", DataType.Int32);
			Assert.IsNotNull(attr1);

			array_schema.AddAttribute(attr1);

			array_schema.SetDomain(domain);

			array_schema.Check();

		}

		[TestMethod]
		public void TestArraySchemaInt32Hilbert()
		{
			var config = new Config();
			var context = new Context(config);

			var d1 = Dimension.Create(context, "d1", 0, 100, 5);
			Assert.IsNotNull(d1);

			var d2 = Dimension.Create(context, "d2", 0, 200, 5);
			Assert.IsNotNull(d1);

			var domain = new Domain(context);
			Assert.IsNotNull(domain);

			domain.AddDimensions(d1, d2);

			var array_schema = new ArraySchema(context, ArrayType.Sparse);
			Assert.IsNotNull(array_schema);

			var a1 = new Attribute(context, "a1", DataType.Int32);
			Assert.IsNotNull(a1);

			array_schema.AddAttributes(a1);

			array_schema.SetDomain(domain);

			array_schema.SetCellOrder(LayoutType.Hilbert);
			array_schema.SetCapacity(2);
			array_schema.Check();
		}

		[TestMethod]
		public void TestArraySchemaInt32DenseHilbert()
		{
			var config = new Config();
			var context = new Context(config);

			var d1 = Dimension.Create(context, "d1", 0, 100, 5);
			Assert.IsNotNull(d1);

			var d2 = Dimension.Create(context, "d2", 0, 200, 5);
			Assert.IsNotNull(d1);

			var domain = new Domain(context);
			Assert.IsNotNull(domain);

			domain.AddDimensions(d1, d2);

			var array_schema = new ArraySchema(context, ArrayType.Dense);
			Assert.IsNotNull(array_schema);

			var a1 = new Attribute(context, "a1", DataType.Int32);
			Assert.IsNotNull(a1);

			array_schema.AddAttributes(a1);

			array_schema.SetDomain(domain);

			
			Assert.ThrowsException<TileDBException>(()=>array_schema.SetCellOrder(LayoutType.Hilbert));
			Assert.ThrowsException<TileDBException>(()=>array_schema.SetTileOrder(LayoutType.Hilbert));
			array_schema.SetCapacity(2);
			array_schema.Check();
		}

		[TestMethod]
		public void TestArraySchemaInt32NegativeDomainHilbert()
		{
			var config = new Config();
			var context = new Context(config);

			var d1 = Dimension.Create(context, "d1", -50, 50, 5);
			Assert.IsNotNull(d1);

			var d2 = Dimension.Create(context, "d2", -100, 100, 5);
			Assert.IsNotNull(d1);

			var domain = new Domain(context);
			Assert.IsNotNull(domain);

			domain.AddDimensions(d1, d2);

			var array_schema = new ArraySchema(context, ArrayType.Sparse);
			Assert.IsNotNull(array_schema);

			var a1 = new Attribute(context, "a1", DataType.Int32);
			Assert.IsNotNull(a1);

			array_schema.AddAttributes(a1);

			array_schema.SetDomain(domain);

			array_schema.SetCellOrder(LayoutType.Hilbert);
			array_schema.SetCapacity(2);
			array_schema.Check();
		}

		[TestMethod]
		public void TestArraySchemaFloat32Hilbert()
		{
			var config = new Config();
			var context = new Context(config);

			var d1 = Dimension.Create(context, "d1", 0.0f, 1.0f, 0.01f);
			Assert.IsNotNull(d1);

			var d2 = Dimension.Create(context, "d2", 0.0f, 2.0f, 0.01f);
			Assert.IsNotNull(d1);

			var domain = new Domain(context);
			Assert.IsNotNull(domain);

			domain.AddDimensions(d1, d2);

			var array_schema = new ArraySchema(context, ArrayType.Sparse);
			Assert.IsNotNull(array_schema);

			var a1 = new Attribute(context, "a1", DataType.Int32);
			Assert.IsNotNull(a1);

			array_schema.AddAttributes(a1);

			array_schema.SetDomain(domain);

			array_schema.SetCellOrder(LayoutType.Hilbert);
			array_schema.SetCapacity(2);
			array_schema.Check();
		}

		[TestMethod]
		public void TestArraySchemaStringHilbert()
		{
			var config = new Config();
			var context = new Context(config);
			
			var d1 = Dimension.CreateString(context, "d1");
			Assert.IsNotNull(d1);
			
			var d2 = Dimension.CreateString(context, "d2");
			Assert.IsNotNull(d1);
			
			var domain = new Domain(context);
			Assert.IsNotNull(domain);
			
			domain.AddDimensions(d1, d2);
			
			var array_schema = new ArraySchema(context, ArrayType.Sparse);
			Assert.IsNotNull(array_schema);
			
			var a1 = new Attribute(context, "a1", DataType.Int32);
			Assert.IsNotNull(a1);
			
			array_schema.AddAttributes(a1);
			
			array_schema.SetDomain(domain);
			
			array_schema.SetCellOrder(LayoutType.Hilbert);
			array_schema.SetCapacity(2);
			array_schema.Check();			
		}
	}
