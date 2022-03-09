using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TileDB.CSharp;
using System.Runtime.InteropServices;
namespace TileDB.CSharp.Test
{
    [TestClass]
    public class QueryTest
    {

        [TestMethod]
        public void TestQuery()
        {
            var context = Context.GetDefault();
            Assert.IsNotNull(context);
            
            var bound = new sbyte[] { 0, 9 };
            const sbyte extent = 2;
            var dimension = Dimension.Create<sbyte>(context, "dim1", bound, extent);
            Assert.IsNotNull(dimension);
            
            var domain = new Domain(context);
            Assert.IsNotNull(domain);
            
            domain.add_dimension(dimension);
            
            var array_schema = new ArraySchema(context, ArrayType.TILEDB_DENSE);
            Assert.IsNotNull(array_schema);

            var a1 = new Attribute(context, "a1", DataType.TILEDB_INT32);
            Assert.IsNotNull(a1);

            var a2 = new Attribute(context, "a2", DataType.TILEDB_FLOAT32);
            Assert.IsNotNull(a2);

            a2.set_cell_val_num((uint)Constants.TILEDB_VAR_NUM);

            array_schema.add_attributes(a1, a2);

            array_schema.set_domain(domain);

            array_schema.Check();
            
            var tmpArrayPath = Path.Join( Path.GetTempPath(), "tiledb_test_array");

            if (Directory.Exists(tmpArrayPath))
            {
                Directory.Delete(tmpArrayPath, true);
            }

            var array = new Array(context, tmpArrayPath);
            Assert.IsNotNull(array);
            
            array.Create(array_schema);
            
            array.Open(QueryType.TILEDB_WRITE);

            var query = new Query(context, array);
            
            query.set_subarray(new sbyte[]{0, 1});
            
            query.set_layout(LayoutType.TILEDB_ROW_MAJOR);

            var a1_data_buffer = new int[2] { 1, 2 };  
     
            query.set_data_buffer<int>("a1", a1_data_buffer);

            var a2_data_buffer = new float[5] { 1.0f, 2.0f, 3.0f, 4.0f, 5.0f };
  
            query.set_data_buffer<float>("a2", a2_data_buffer );

            var a2_offset_buffer = new UInt64[2] { 0, 3 };  
  
            query.set_offsets_buffer("a2", a2_offset_buffer);
            
            query.submit();

            var status = query.status();
            
            Assert.AreEqual(QueryStatus.TILEDB_COMPLETED, status);

            query.finalize();

            array.Close();

            //Start to read 
            var array_read = new Array(context, tmpArrayPath);
            Assert.IsNotNull(array_read);

            array_read.Open(QueryType.TILEDB_READ);

            var query_read = new Query(context, array_read);

            query_read.set_subarray(new sbyte[] { 0, 1 });

            query_read.set_layout(LayoutType.TILEDB_ROW_MAJOR);

            var a1_data_buffer_read = new int[2];
    
            query_read.set_data_buffer<int>("a1", a1_data_buffer_read);

            var a2_data_buffer_read = new float[5];

            query_read.set_data_buffer<float>("a2", a2_data_buffer_read);

            var a2_offset_buffer_read = new UInt64[2];
      
            query_read.set_offsets_buffer("a2", a2_offset_buffer_read);

            query_read.submit();
            var status_read = query_read.status();

            Assert.AreEqual(QueryStatus.TILEDB_COMPLETED, status_read);

            query_read.finalize();

            array_read.Close();

            CollectionAssert.AreEqual(a1_data_buffer, a1_data_buffer_read);
             
            CollectionAssert.AreEqual(a2_data_buffer, a2_data_buffer_read);

            CollectionAssert.AreEqual(a2_offset_buffer, a2_offset_buffer_read);


        }


        [TestMethod]
        public void TestSimpleSparseArrayQuery()
        {
            var context = Context.GetDefault();
            Assert.IsNotNull(context);

            // Create array
            int[] dim1_bound = new int[] { 1, 4 };
            int dim1_extent = 4;
            var dim1 = Dimension.Create<int>(context, "rows", dim1_bound, dim1_extent);

            int[] dim2_bound = new int[] { 1, 4 };
            int dim2_extent = 4;
            var dim2 = Dimension.Create<int>(context, "cols", dim2_bound, dim2_extent);

            var domain = new Domain(context);
            Assert.IsNotNull(domain);

            domain.add_dimension(dim1);
            domain.add_dimension(dim2);

            var array_schema = new ArraySchema(context, ArrayType.TILEDB_SPARSE);
            Assert.IsNotNull(array_schema);

            var attr = new Attribute(context, "a", DataType.TILEDB_INT32);
            Assert.IsNotNull(attr);

            array_schema.add_attribute(attr);

            array_schema.set_domain(domain);

            array_schema.Check();

            var tmpArrayPath = Path.Join(Path.GetTempPath(), "tiledb_test_sparse_array");

            if (Directory.Exists(tmpArrayPath))
            {
                Directory.Delete(tmpArrayPath, true);
            }

            Array.Create(context, tmpArrayPath, array_schema);

            //Write array
            var array_write = new Array(context, tmpArrayPath);
            Assert.IsNotNull(array_write);

            array_write.Open(QueryType.TILEDB_WRITE);

            var query_write = new Query(context, array_write);

            var dim1_data_buffer = new int[3] { 1, 2, 3 };
  
            var dim2_data_buffer = new int[3] { 1, 3, 4 };

            var attr_data_buffer = new int[3] { 1, 2, 3 };
 
            query_write.set_layout(LayoutType.TILEDB_UNORDERED);

            query_write.set_data_buffer<int>("rows", dim1_data_buffer);
            query_write.set_data_buffer<int>("cols", dim2_data_buffer);
            query_write.set_data_buffer<int>("a", attr_data_buffer);

            query_write.submit();

            var status = query_write.status();

            Assert.AreEqual(QueryStatus.TILEDB_COMPLETED, status);

            array_write.Close();

            //Read array
            var array_read = new Array(context, tmpArrayPath);
            Assert.IsNotNull(array_read);

            array_read.Open(QueryType.TILEDB_READ);

            var query_read = new Query(context, array_read);

            query_read.set_layout(LayoutType.TILEDB_ROW_MAJOR);

            var dim1_data_buffer_read = new int[3];
    
            query_read.set_data_buffer<int>("rows", dim1_data_buffer_read);

            var dim2_data_buffer_read = new int[3];
  
            query_read.set_data_buffer<int>("cols", dim2_data_buffer_read);

            var attr_data_buffer_read = new int[3];
  
            query_read.set_data_buffer<int>("a", attr_data_buffer_read);

            query_read.submit();
            var status_read = query_read.status();

            Assert.AreEqual(QueryStatus.TILEDB_COMPLETED, status_read);

            array_read.Close();

            CollectionAssert.AreEqual(dim1_data_buffer, dim1_data_buffer_read);

            CollectionAssert.AreEqual(dim2_data_buffer, dim2_data_buffer_read);

            CollectionAssert.AreEqual(attr_data_buffer, attr_data_buffer_read);

        }

        [TestMethod]
        public void TestNullableAttributeArrayQuery() 
        {
            var context = Context.GetDefault();
            Assert.IsNotNull(context);

            // Create array
            int[] dim1_bound = new int[] { 1, 2 };
            int dim1_extent = 2;
            var dim1 = Dimension.Create<int>(context, "rows", dim1_bound, dim1_extent);

            int[] dim2_bound = new int[] { 1, 2 };
            int dim2_extent = 2;
            var dim2 = Dimension.Create<int>(context, "cols", dim2_bound, dim2_extent);

            var domain = new Domain(context);
            Assert.IsNotNull(domain);

            domain.add_dimension(dim1);
            domain.add_dimension(dim2);

            var array_schema = new ArraySchema(context, ArrayType.TILEDB_DENSE);
            Assert.IsNotNull(array_schema);

            var attr1 = new Attribute(context, "a1", DataType.TILEDB_INT32);
            Assert.IsNotNull(attr1);
            attr1.set_nullable(true);
            array_schema.add_attribute(attr1);

            var attr2 = new Attribute(context, "a2", DataType.TILEDB_INT32);
            Assert.IsNotNull(attr2);
            attr2.set_nullable(true);
            attr2.set_cell_val_num((uint)Constants.TILEDB_VAR_NUM);
            array_schema.add_attribute(attr2);

            var attr3 = new Attribute(context, "a3", DataType.TILEDB_STRING_ASCII);
            Assert.IsNotNull(attr3);
            attr3.set_nullable(true);
            array_schema.add_attribute(attr3);

            array_schema.set_domain(domain);
            array_schema.set_tile_order(LayoutType.TILEDB_ROW_MAJOR);
            array_schema.set_cell_order(LayoutType.TILEDB_ROW_MAJOR);

            array_schema.Check();

            var tmpArrayPath = Path.Join(Path.GetTempPath(), "tiledb_test_nullable_array");

            if (Directory.Exists(tmpArrayPath))
            {
                Directory.Delete(tmpArrayPath, true);
            }

            Array.Create(context, tmpArrayPath, array_schema);

            // Write array
            var array_write = new Array(context, tmpArrayPath);
            Assert.IsNotNull(array_write);

            array_write.Open(QueryType.TILEDB_WRITE);

            var query_write = new Query(context, array_write);
            query_write.set_layout(LayoutType.TILEDB_ROW_MAJOR);

            int[] a1_data = new int[4] { 100, 200, 300, 400 };
            int[] a2_data = new int[8] { 10, 10, 20, 30, 30, 30, 40, 40 };
            ulong[] a2_el_off = new ulong[4] { 0, 2, 3, 6 };
            ulong[] a2_off = new ulong[4];
            for (int i = 0; i < a2_el_off.Length; ++i)
            {
                a2_off[i] = a2_el_off[i] * EnumUtil.datatype_size(DataType.TILEDB_INT32);
            }

            byte[] a3_data = new byte[9] { Convert.ToByte('a'), Convert.ToByte('b'), Convert.ToByte('c'), Convert.ToByte('d'), Convert.ToByte('e'), Convert.ToByte('w'), Convert.ToByte('x'), Convert.ToByte('y'), Convert.ToByte('z') };
            ulong[] a3_el_off = new ulong[4] { 0, 3, 4, 5 };
            ulong[] a3_off = new ulong[4];
            for (int i = 0; i < a3_el_off.Length; ++i)
            {
                a3_off[i] = a3_el_off[i] * EnumUtil.datatype_size(DataType.TILEDB_CHAR);
            }

            byte[] a1_validity = new byte[4] { 1, 0, 0, 1 };
            byte[] a2_validity = new byte[4] { 0, 1, 1, 0 };
            byte[] a3_validity = new byte[4] { 1, 0, 0, 1 };

            query_write.set_data_buffer<int>("a1", a1_data);
            query_write.set_validity_buffer("a1", a1_validity);

            query_write.set_data_buffer<int>("a2", a2_data);
            query_write.set_offsets_buffer("a2", a2_off);
            query_write.set_validity_buffer("a2", a2_validity);

            query_write.set_data_buffer<byte>("a3", a3_data);
            query_write.set_offsets_buffer("a3", a3_off);
            query_write.set_validity_buffer("a3", a3_validity);

            query_write.submit();

            var status = query_write.status();

            Assert.AreEqual(QueryStatus.TILEDB_COMPLETED, status);

            array_write.Close();

            // Read array
            var array_read = new Array(context, tmpArrayPath);
            Assert.IsNotNull(array_read);

            array_read.Open(QueryType.TILEDB_READ);

            var query_read = new Query(context, array_read);

            query_read.set_layout(LayoutType.TILEDB_ROW_MAJOR);

            int[] a1_data_read = new int[4];
            byte[] a1_validity_read = new byte[4];

            int[] a2_data_read = new int[8];
            ulong[] a2_off_read = new ulong[4];
            byte[] a2_validity_read = new byte[4];

            byte[] a3_data_read = new byte[9];
            ulong[] a3_off_read = new ulong[4];
            byte[] a3_validity_read = new byte[4];

            var subarray = new int[4] { 1, 2, 1, 2 };
            query_read.set_subarray<int>(subarray);

            query_read.set_data_buffer<int>("a1", a1_data_read);
            query_read.set_validity_buffer("a1", a1_validity_read);

            query_read.set_data_buffer<int>("a2", a2_data_read);
            query_read.set_offsets_buffer("a2", a2_off_read);
            query_read.set_validity_buffer("a2", a2_validity_read);

            query_read.set_data_buffer<byte>("a3", a3_data_read);
            query_read.set_offsets_buffer("a3", a3_off_read);
            query_read.set_validity_buffer("a3", a3_validity_read);

            query_read.submit();
            var status_read = query_read.status();

            Assert.AreEqual(QueryStatus.TILEDB_COMPLETED, status_read);

            array_read.Close();

            CollectionAssert.AreEqual(a1_data, a1_data_read);
            CollectionAssert.AreEqual(a1_validity, a1_validity_read);

            CollectionAssert.AreEqual(a2_data, a2_data_read);
            CollectionAssert.AreEqual(a2_validity, a2_validity_read);
            CollectionAssert.AreEqual(a2_off, a2_off_read);

            CollectionAssert.AreEqual(a3_data, a3_data_read);
            CollectionAssert.AreEqual(a3_validity, a3_validity_read);
            CollectionAssert.AreEqual(a3_off, a3_off_read);

        }// public void TestNullableAttributeArrayQuery

    }//class
}
