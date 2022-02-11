using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TileDB.CSharp.Test
{
    [TestClass]
    public class AttributeTest
    {
        [TestMethod]
        public void FillValue()
        {
            var ctx = TileDB.Context.GetDefault();
            var attr = new Attribute(ctx, "a", DataType.TILEDB_INT32);
            var fill_value = 100;
            attr.set_fill_value(fill_value);
            var value_size = attr.fill_value<int>();
            Assert.AreEqual(fill_value, value_size[0]);
        }

        [TestMethod]
        public void TestFullAttribute()
        {
    
            var context = TileDB.Context.GetDefault();

            const string attrName = "a";
            using var attribute =  new Attribute(context, attrName, DataType.TILEDB_INT32);
            Assert.AreEqual(attrName, attribute.name());
            Assert.AreEqual(DataType.TILEDB_INT32, attribute.type());

            // Set and get compressor
            var gzipFilter = new Filter(context, FilterType.TILEDB_FILTER_GZIP);
            gzipFilter.set_option(FilterOption.TILEDB_COMPRESSION_LEVEL, 5);
            var filter_list = new FilterList(context);
            filter_list.add_filter(gzipFilter);
            attribute.set_filter_list(filter_list);

            var filterListReturn = attribute.filter_list();
            Assert.AreEqual(filter_list.nfilters(), filterListReturn.nfilters());
            var filterReturn = filterListReturn.filter(0);
            var filterTypeReturn = filterReturn.filter_type();
            Assert.AreEqual(FilterType.TILEDB_FILTER_GZIP, filterTypeReturn);
            var filterOption = gzipFilter.get_option<int>(FilterOption.TILEDB_COMPRESSION_LEVEL);
            Assert.AreEqual(5, filterOption);

            attribute.set_cell_val_num(10);
            var cell_size = attribute.cell_size();
            Assert.AreEqual<ulong>(40, cell_size);

            var cell_val_num = attribute.cell_val_num();
            Assert.AreEqual<ulong>(10, cell_val_num);

            attribute.set_fill_value(12);
            var value_size = attribute.fill_value<int>();
            Assert.AreEqual(12, value_size[0]);
        }

        [TestMethod]
        public void TestNullableAttribute()
        {
            
            var context = TileDB.Context.GetDefault();

            const string attrName = "a";
            using var attribute = new Attribute(context, attrName, DataType.TILEDB_INT32);
            Assert.AreEqual(attrName, attribute.name());

            attribute.set_nullable(true);

            Assert.AreEqual(true, attribute.nullable());
            Assert.AreEqual(DataType.TILEDB_INT32, attribute.type());

            // Set and get compressor
            var gzipFilter = new Filter(context, FilterType.TILEDB_FILTER_GZIP);
            gzipFilter.set_option( FilterOption.TILEDB_COMPRESSION_LEVEL, 5);
            var filter_list = new FilterList(context);
            filter_list.add_filter(gzipFilter);
            attribute.set_filter_list(filter_list);

            var filterListReturn = attribute.filter_list();
            Assert.AreEqual(filter_list.nfilters(), filterListReturn.nfilters());
            var filterReturn = filterListReturn.filter(0);
            var filterTypeReturn = filterReturn.filter_type();
            Assert.AreEqual(FilterType.TILEDB_FILTER_GZIP, filterTypeReturn);
            var filterOption = gzipFilter.get_option<int>(FilterOption.TILEDB_COMPRESSION_LEVEL);
            Assert.AreEqual(5, filterOption);

            attribute.set_cell_val_num(10);
            var cell_size = attribute.cell_size();
            Assert.AreEqual<ulong>(40, cell_size);

            var cell_val_num = attribute.cell_val_num();
            Assert.AreEqual<ulong>(10, cell_val_num);

            attribute.set_fill_value_nullable(12, true);
            var fill_value = attribute.fill_value_nullable<int>();
            Assert.AreEqual(12, fill_value.Item1[0]);
            Assert.AreEqual(true, fill_value.Item2);
        }  
        
        [TestMethod]
        public void TestStringAttribute()
        {
            
            var context = TileDB.Context.GetDefault();

            const string attrName = "a";
            using var attribute = new Attribute(context, attrName, DataType.TILEDB_STRING_ASCII);
            Assert.AreEqual<string>(attrName, attribute.name());
            Assert.AreEqual(DataType.TILEDB_STRING_ASCII, attribute.type());

            attribute.set_fill_value("test_fill");
            var fill_value = attribute.fill_value();
            Assert.AreEqual<string>("test_fill", fill_value);
        }//TestStringAttribute
    }

}//namespace