using Microsoft.VisualStudio.TestTools.UnitTesting;
using TileDB;

namespace TileDB.CSharp.Test
{
	[TestClass]
	public class ArraySchemaTest
	{
		[TestMethod]
		public void TestArraySchema()
		{
			
			var context = TileDB.Context.GetDefault();

			var bound = new int[] { 1, 10 };
			const int extent = 5;
			var dimension = Dimension.create(context, "test", bound, extent);
			Assert.IsNotNull(dimension);

			var domain = new Domain(context);
			Assert.IsNotNull(domain);

			domain.add_dimension(dimension);

			var array_schema = new ArraySchema(context, ArrayType.TILEDB_DENSE);
			Assert.IsNotNull(array_schema);

			Assert.IsFalse(array_schema.allows_dups());

			var attribute = new Attribute(context, "a1", DataType.TILEDB_INT32);
			Assert.IsNotNull(attribute);

			array_schema.add_attribute(attribute);

			Assert.AreEqual((uint)1, array_schema.attribute_num());

			var attribute_from_index = array_schema.attribute(0);
			Assert.AreEqual("a1", attribute_from_index.name());

			var attribute_from_name = array_schema.attribute("a1");
			Assert.AreEqual("a1", attribute_from_name.name());

			Assert.IsTrue(array_schema.has_attribute("a1"));

			array_schema.set_capacity(100);
			Assert.AreEqual((ulong)100, array_schema.capacity());

			array_schema.set_domain(domain);

			var d = array_schema.domain();
			Assert.IsNotNull(d);

			Assert.AreEqual((ulong)1, d.ndim());
			Assert.AreEqual(DataType.TILEDB_INT32, d.type());

			array_schema.set_cell_order(LayoutType.TILEDB_GLOBAL_ORDER);
			Assert.AreEqual(LayoutType.TILEDB_GLOBAL_ORDER, array_schema.cell_order());

			array_schema.set_tile_order(LayoutType.TILEDB_COL_MAJOR);
			Assert.AreEqual(LayoutType.TILEDB_COL_MAJOR, array_schema.tile_order());

			using var filter = new Filter(context, FilterType.TILEDB_FILTER_BZIP2);
			Assert.AreEqual(FilterType.TILEDB_FILTER_BZIP2, filter.filter_type());

			using var filter_list = new FilterList(context);
			filter_list.add_filter(filter);

			array_schema.set_coords_filter_list(filter_list);
			var filter_list_return = array_schema.coords_filter_list();

			var filter0 = filter_list_return.filter(0);
			Assert.AreEqual(FilterType.TILEDB_FILTER_BZIP2, filter0.filter_type());

			array_schema.set_offsets_filter_list(filter_list);
			filter_list_return = array_schema.offsets_filter_list();

			filter0 = filter_list_return.filter(0);
			Assert.AreEqual(FilterType.TILEDB_FILTER_BZIP2, filter0.filter_type());

			array_schema.check();
		}

		[TestMethod]
		public void TestArraySchemaInt32Hilbert()
		{
			var config = new Config();
			var context = new Context(config);

			var d1 = Dimension.create(context, "d1", new int[] { 0, 100 }, 5);
			Assert.IsNotNull(d1);

			var d2 = Dimension.create(context, "d2", new int[] { 0, 200 }, 5);
			Assert.IsNotNull(d1);

			var domain = new Domain(context);
			Assert.IsNotNull(domain);

			domain.add_dimensions(new Dimension[] { d1, d2 });

			var array_schema = new ArraySchema(context, ArrayType.TILEDB_SPARSE);
			Assert.IsNotNull(array_schema);

			var a1 = new Attribute(context, "a1", DataType.TILEDB_INT32);
			Assert.IsNotNull(a1);

			array_schema.add_attributes(new Attribute[] { a1 });

			array_schema.set_domain(domain);

			array_schema.set_cell_order(LayoutType.TILEDB_HILBERT);
			array_schema.set_capacity(2);
			array_schema.check();
		}

		[TestMethod]
		public void TestArraySchemaInt32DenseHilbert()
		{
			var config = new Config();
			var context = new Context(config);

			var d1 = Dimension.create(context, "d1", new int[] { 0, 100 }, 5);
			Assert.IsNotNull(d1);

			var d2 = Dimension.create(context, "d2", new int[] { 0, 200 }, 5);
			Assert.IsNotNull(d1);

			var domain = new Domain(context);
			Assert.IsNotNull(domain);

			domain.add_dimensions(new Dimension[] { d1, d2 });

			var array_schema = new ArraySchema(context, ArrayType.TILEDB_DENSE);
			Assert.IsNotNull(array_schema);

			var a1 = new Attribute(context, "a1", DataType.TILEDB_INT32);
			Assert.IsNotNull(a1);

			array_schema.add_attributes(new Attribute[] { a1 });

			array_schema.set_domain(domain);

			
			Assert.ThrowsException<System.Exception>(()=>array_schema.set_cell_order(LayoutType.TILEDB_HILBERT));
			Assert.ThrowsException<System.Exception>(()=>array_schema.set_tile_order(LayoutType.TILEDB_HILBERT));
			array_schema.set_capacity(2);
			array_schema.check();
		}

		[TestMethod]
		public void TestArraySchemaInt32NegativeDomainHilbert()
		{
			var config = new Config();
			var context = new Context(config);

			var d1 = Dimension.create(context, "d1", new int[] { -50, 50 }, 5);
			Assert.IsNotNull(d1);

			var d2 = Dimension.create(context, "d2", new int[] { -100, 100 }, 5);
			Assert.IsNotNull(d1);

			var domain = new Domain(context);
			Assert.IsNotNull(domain);

			domain.add_dimensions(new Dimension[] { d1, d2 });

			var array_schema = new ArraySchema(context, ArrayType.TILEDB_SPARSE);
			Assert.IsNotNull(array_schema);

			var a1 = new Attribute(context, "a1", DataType.TILEDB_INT32);
			Assert.IsNotNull(a1);

			array_schema.add_attributes(new Attribute[] { a1 });

			array_schema.set_domain(domain);

			array_schema.set_cell_order(LayoutType.TILEDB_HILBERT);
			array_schema.set_capacity(2);
			array_schema.check();
		}

		[TestMethod]
		public void TestArraySchemaFloat32Hilbert()
		{
			var config = new Config();
			var context = new Context(config);

			var d1 = Dimension.create(context, "d1", new float[] { 0.0f, 1.0f }, 0.01f);
			Assert.IsNotNull(d1);

			var d2 = Dimension.create(context, "d2", new float[] { 0.0f, 2.0f }, 0.01f);
			Assert.IsNotNull(d1);

			var domain = new Domain(context);
			Assert.IsNotNull(domain);

			domain.add_dimensions(new Dimension[] { d1, d2 });

			var array_schema = new ArraySchema(context, ArrayType.TILEDB_SPARSE);
			Assert.IsNotNull(array_schema);

			var a1 = new Attribute(context, "a1", DataType.TILEDB_INT32);
			Assert.IsNotNull(a1);

			array_schema.add_attributes(new Attribute[] { a1 });

			array_schema.set_domain(domain);

			array_schema.set_cell_order(LayoutType.TILEDB_HILBERT);
			array_schema.set_capacity(2);
			array_schema.check();
		}

		[TestMethod]
		public void TestArraySchemaStringHilbert()
		{
			var config = new Config();
			var context = new Context(config);
			
			var d1 = Dimension.create_string(context, "d1");
			Assert.IsNotNull(d1);
			
			var d2 = Dimension.create_string(context, "d2");
			Assert.IsNotNull(d1);
			
			var domain = new Domain(context);
			Assert.IsNotNull(domain);
			
			domain.add_dimensions(new Dimension[] { d1, d2 });
			
			var array_schema = new ArraySchema(context, ArrayType.TILEDB_SPARSE);
			Assert.IsNotNull(array_schema);
			
			var a1 = new Attribute(context, "a1", DataType.TILEDB_INT32);
			Assert.IsNotNull(a1);
			
			array_schema.add_attributes(new Attribute[] { a1 });
			
			array_schema.set_domain(domain);
			
			array_schema.set_cell_order(LayoutType.TILEDB_HILBERT);
			array_schema.set_capacity(2);
			array_schema.check();			
		}
	}
}//namespace