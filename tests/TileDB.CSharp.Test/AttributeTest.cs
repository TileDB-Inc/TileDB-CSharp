using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace TileDB.CSharp.Test;

[TestClass]
public class AttributeTest
{
    [TestMethod]
    public void FillValue()
    {
        var ctx = Context.GetDefault();
        var attr = new Attribute(ctx, "a", DataType.Int32);
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
        using var attribute =  new Attribute(context, attrName, DataType.Int32);
        Assert.AreEqual(attrName, attribute.Name());
        Assert.AreEqual(DataType.Int32, attribute.Type());

        // Set and get compressor
        var gzipFilter = new Filter(context, FilterType.Gzip);
        gzipFilter.SetOption(FilterOption.CompressionLevel, 5);
        var filter_list = new FilterList(context);
        filter_list.AddFilter(gzipFilter);
        attribute.SetFilterList(filter_list);

        var filterListReturn = attribute.FilterList();
        Assert.AreEqual(filter_list.NFilters(), filterListReturn.NFilters());
        var filterReturn = filterListReturn.Filter(0);
        var filterTypeReturn = filterReturn.FilterType();
        Assert.AreEqual(FilterType.Gzip, filterTypeReturn);
        var filterOption = gzipFilter.GetOption<int>(FilterOption.CompressionLevel);
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
        using var attribute = new Attribute(context, attrName, DataType.Int32);
        Assert.AreEqual(attrName, attribute.Name());

        attribute.SetNullable(true);

        Assert.AreEqual(true, attribute.Nullable());
        Assert.AreEqual(DataType.Int32, attribute.Type());

        // Set and get compressor
        var gzipFilter = new Filter(context, FilterType.Gzip);
        gzipFilter.SetOption( FilterOption.CompressionLevel, 5);
        var filter_list = new FilterList(context);
        filter_list.AddFilter(gzipFilter);
        attribute.SetFilterList(filter_list);

        var filterListReturn = attribute.FilterList();
        Assert.AreEqual(filter_list.NFilters(), filterListReturn.NFilters());
        var filterReturn = filterListReturn.Filter(0);
        var filterTypeReturn = filterReturn.FilterType();
        Assert.AreEqual(FilterType.Gzip, filterTypeReturn);
        var filterOption = gzipFilter.GetOption<int>(FilterOption.CompressionLevel);
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
        using var attribute = new Attribute(context, attrName, DataType.StringAscii);
        Assert.AreEqual(attrName, attribute.Name());
        Assert.AreEqual(DataType.StringAscii, attribute.Type());

        attribute.SetFillValue("test_fill");
        var fill_value = attribute.FillValue();
        Assert.AreEqual("test_fill", fill_value);
    }//TestStringAttribute

    [TestMethod]
    public void TestBoolAttribute()
    {
        var context = Context.GetDefault();

        const string attrName = "a";
        using var attribute = new Attribute(context, attrName, DataType.Boolean);
        Assert.AreEqual(attrName, attribute.Name());
        Assert.AreEqual(DataType.Boolean, attribute.Type());

        bool fill_value = true;
        attribute.SetFillValue(fill_value);
        var value_size = attribute.FillValue<bool>();
        Assert.AreEqual(true, value_size[0]);
    }

    [TestMethod]
    public void TestInt32Attribute()
    {
        var context = Context.GetDefault();

        const string attrName = "a";
        using var attribute = new Attribute(context, attrName, DataType.Int32);
        Assert.AreEqual(attrName, attribute.Name());
        Assert.AreEqual(DataType.Int32, attribute.Type());

        int fill_value = 184;
        attribute.SetFillValue(fill_value);
        var value_size = attribute.FillValue<int>();
        Assert.AreEqual(184, value_size[0]);
    }

    [TestMethod]
    public void TestVarLengthAttribute()
    {
        createVarLengthAttributeArray();
        writeVarLengthAttributeArray();
        readVarLengthAttributeArray();
    }

    private void createVarLengthAttributeArray()
    {
        var tmpArrayPath = TestUtil.MakeTestPath("varlength_attributes_array");
        if (Directory.Exists(tmpArrayPath))
        {
            Directory.Delete(tmpArrayPath, true);
        }

        var context = Context.GetDefault();
        var domain = new Domain(context);
        Assert.IsNotNull(domain);

        var dim_rows = Dimension.Create(context, "rows", 1, 4, 4);
        Assert.IsNotNull(dim_rows);
        domain.AddDimension(dim_rows);

        var dim_cols = Dimension.Create(context, "cols", 1, 4, 4);
        Assert.IsNotNull(dim_cols);
        domain.AddDimension(dim_cols);

        var array_schema = new ArraySchema(context, ArrayType.Dense);
        Assert.IsNotNull(array_schema);

        var attr1 = new Attribute(context, "a1", DataType.StringAscii);
        Assert.IsNotNull(attr1);
        attr1.SetCellValNum(Attribute.VariableSized);
        array_schema.AddAttribute(attr1);

        var attr2 = new Attribute(context, "a2", DataType.Int32);
        Assert.IsNotNull(attr2);
        attr2.SetCellValNum(Attribute.VariableSized);
        array_schema.AddAttribute(attr2);

        array_schema.SetDomain(domain);
        array_schema.SetCellOrder(LayoutType.RowMajor);
        array_schema.SetTileOrder(LayoutType.RowMajor);

        array_schema.Check();

        Array.Create(context, tmpArrayPath, array_schema);
    }

    private void writeVarLengthAttributeArray()
    {
        var tmpArrayPath = TestUtil.MakeTestPath("varlength_attributes_array");
        var context = Context.GetDefault();
        var array_write = new Array(context, tmpArrayPath);
        Assert.IsNotNull(array_write);

        array_write.Open(QueryType.Write);

        string[] a1_str = [ "a","b","c","d", "aa","bb","cc","dd"
            ,"aaa","bbb","ccc","ddd","aaaa","bbbb","cccc","dddd" ];
        var (a1_data, a1_offsets) = CoreUtil.PackStringArray(a1_str);

        int[] a2_data = [1, 1, 2, 2,  3,  4,  5,  6,  6,  7,  7,  8,  8,
                          8, 9, 9, 10, 11, 12, 12, 13, 14, 14, 14, 15, 16 ];
        ulong[] a2_el_off = [0, 2, 4, 5, 6, 7, 9, 11, 14, 16, 17, 18, 20, 21, 24, 25];
        ulong[] a2_off = CoreUtil.ElementOffsetsToByteOffsets(a2_el_off, DataType.Int32);

        var query_write = new Query(array_write);
        query_write.SetLayout(LayoutType.RowMajor);
        // using var subarray = new Subarray(array_write);
        // subarray.SetSubarray(new int[] { 1, 4, 4, 4 });

        query_write.SetDataBuffer("a1", a1_data);
        query_write.SetOffsetsBuffer("a1", a1_offsets);

        query_write.SetDataBuffer("a2", a2_data);
        query_write.SetOffsetsBuffer("a2", a2_off);

        query_write.Submit();

        var status_write = query_write.Status();
        Assert.AreEqual(QueryStatus.Completed, status_write);
        query_write.FinalizeQuery();

        array_write.Close();
    }

    private void readVarLengthAttributeArray()
    {
        var tmpArrayPath = TestUtil.MakeTestPath("varlength_attributes_array");
        var context = Context.GetDefault();
        var array_read = new Array(context, tmpArrayPath);
        Assert.IsNotNull(array_read);
        array_read.Open(QueryType.Read);

        var query_read = new Query(array_read);

        // Slice only rows 1,2 and cols 2,3,4
        using var subarray = new Subarray(array_read);
        subarray.SetSubarray(1, 4, 1, 4);
        query_read.SetSubarray(subarray);

        query_read.SetLayout(LayoutType.RowMajor);

        byte[] a1_data_buffer_read = new byte[128];
        ulong[] a1_offsets_buffer_read = new ulong[128];

        int[] a2_data_buffer_read = new int[128];
        ulong[] a2_offsets_buffer_read = new ulong[128];

        query_read.SetDataBuffer("a1", a1_data_buffer_read);
        query_read.SetOffsetsBuffer("a1", a1_offsets_buffer_read);
        query_read.SetDataBuffer("a2", a2_data_buffer_read);
        query_read.SetOffsetsBuffer("a2", a2_offsets_buffer_read);

        query_read.Submit();
        var status_read = query_read.Status();
        Assert.AreEqual(QueryStatus.Completed, status_read);

        var result_size_a1 = query_read.EstResultSize("a1");
        var result_size_a2 = query_read.EstResultSize("a2");

        Assert.AreEqual<ulong>(40, result_size_a1.DataBytesSize);
        Assert.AreEqual<ulong>(16, result_size_a1.OffsetsSize());
        Assert.AreEqual<ulong>(26, result_size_a2.DataSize(DataType.Int32));
        Assert.AreEqual<ulong>(16, result_size_a2.OffsetsSize());
        array_read.Close();
    }
}
