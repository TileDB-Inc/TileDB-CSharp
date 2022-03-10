using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TileDB.CSharp.Test
{
    [TestClass]
    public class AttributeTest
    {
        [TestMethod]
        public void FillValue()
        {
            var ctx = Context.GetDefault();
            var attr = new Attribute(ctx, "a", DataType.TILEDB_INT32);
            var fill_value = 100;
            attr.SetFillValue(fill_value);
            var value_size = attr.FillValue<int>();
            Assert.AreEqual(fill_value, value_size[0]);
        }

        [TestMethod]
        public void TestFullAttribute()
        {
    
            var context = Context.GetDefault();

            const string attrName = "a";
            using var attribute =  new Attribute(context, attrName, DataType.TILEDB_INT32);
            Assert.AreEqual(attrName, attribute.Name());
            Assert.AreEqual(DataType.TILEDB_INT32, attribute.Type());

            // Set and get compressor
            var gzipFilter = new Filter(context, FilterType.TILEDB_FILTER_GZIP);
            gzipFilter.SetOption(FilterOption.TILEDB_COMPRESSION_LEVEL, 5);
            var filter_list = new FilterList(context);
            filter_list.AddFilter(gzipFilter);
            attribute.SetFilterList(filter_list);

            var filterListReturn = attribute.FilterList();
            Assert.AreEqual(filter_list.NFilters(), filterListReturn.NFilters());
            var filterReturn = filterListReturn.Filter(0);
            var filterTypeReturn = filterReturn.FilterType();
            Assert.AreEqual(FilterType.TILEDB_FILTER_GZIP, filterTypeReturn);
            var filterOption = gzipFilter.GetOption<int>(FilterOption.TILEDB_COMPRESSION_LEVEL);
            Assert.AreEqual(5, filterOption);

            attribute.SetCellValNum(10);
            var cell_size = attribute.CellSize();
            Assert.AreEqual<ulong>(40, cell_size);

            var cell_val_num = attribute.CellValNum();
            Assert.AreEqual<ulong>(10, cell_val_num);

            attribute.SetFillValue(12);
            var value_size = attribute.FillValue<int>();
            Assert.AreEqual(12, value_size[0]);
        }

        [TestMethod]
        public void TestNullableAttribute()
        {
            
            var context = Context.GetDefault();

            const string attrName = "a";
            using var attribute = new Attribute(context, attrName, DataType.TILEDB_INT32);
            Assert.AreEqual(attrName, attribute.Name());

            attribute.SetNullable(true);

            Assert.AreEqual(true, attribute.Nullable());
            Assert.AreEqual(DataType.TILEDB_INT32, attribute.Type());

            // Set and get compressor
            var gzipFilter = new Filter(context, FilterType.TILEDB_FILTER_GZIP);
            gzipFilter.SetOption( FilterOption.TILEDB_COMPRESSION_LEVEL, 5);
            var filter_list = new FilterList(context);
            filter_list.AddFilter(gzipFilter);
            attribute.SetFilterList(filter_list);

            var filterListReturn = attribute.FilterList();
            Assert.AreEqual(filter_list.NFilters(), filterListReturn.NFilters());
            var filterReturn = filterListReturn.Filter(0);
            var filterTypeReturn = filterReturn.FilterType();
            Assert.AreEqual(FilterType.TILEDB_FILTER_GZIP, filterTypeReturn);
            var filterOption = gzipFilter.GetOption<int>(FilterOption.TILEDB_COMPRESSION_LEVEL);
            Assert.AreEqual(5, filterOption);

            attribute.SetCellValNum(10);
            var cell_size = attribute.CellSize();
            Assert.AreEqual<ulong>(40, cell_size);

            var cell_val_num = attribute.CellValNum();
            Assert.AreEqual<ulong>(10, cell_val_num);

            attribute.SetFillValueNullable(12, true);
            var fill_value = attribute.FillValueNullable<int>();
            Assert.AreEqual(12, fill_value.Item1[0]);
            Assert.AreEqual(true, fill_value.Item2);
        }  
        
        [TestMethod]
        public void TestStringAttribute()
        {
            
            var context = Context.GetDefault();

            const string attrName = "a";
            using var attribute = new Attribute(context, attrName, DataType.TILEDB_STRING_ASCII);
            Assert.AreEqual<string>(attrName, attribute.Name());
            Assert.AreEqual(DataType.TILEDB_STRING_ASCII, attribute.Type());

            attribute.SetFillValue("test_fill");
            var fill_value = attribute.FillValue();
            Assert.AreEqual<string>("test_fill", fill_value);
        }//TestStringAttribute
    }

}//namespace