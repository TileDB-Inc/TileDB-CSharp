# Obsoletion warnings in `TileDB.CSharp`

## <span id="TILEDB0001">`TILEDB0001` - Enum value names that start with `TILEDB_` were replaced with C#-friendly names.</span>

Prior to version 5.3.0, the library's various enum values had names identical with the C API, meaning that they were in all-caps and started with `TILEDB_`.

Version 5.3.0 introduced new values with shorter names that comply with .NET's naming guidelines. The old names were obsoleted and hidden from IntelliSense, and future enum values will not be available in the old naming scheme. Switching to the new names is as simple as a find-replace operation. The obsoletion messages will point you to the correct replacement.

> Some already obsolete enum values (like `DataType.TILEDB_ANY`) did not receive new names. Users are strongly encouraged to move away from them.

### Existing code

```csharp
var dim1_data_buffer = new int[3] { 1, 2, 3 };
var dim2_data_buffer = new int[3] { 1, 3, 4 };
var attr_data_buffer = new int[3] { 1, 2, 3 };

using (Array array_write = new Array(Context.GetDefault(), "my_array"))
{
    array_write.Open(QueryType.TILEDB_WRITE);
    var query_write = new Query(Ctx, array_write);
    query_write.SetLayout(LayoutType.TILEDB_UNORDERED);
    query_write.SetDataBuffer<int>("rows", dim1_data_buffer);
    query_write.SetDataBuffer<int>("cols", dim2_data_buffer);
    query_write.SetDataBuffer<int>("a", attr_data_buffer);
    query_write.Submit();
    array_write.Close();
}
```

### New code

```csharp
var dim1_data_buffer = new int[3] { 1, 2, 3 };
var dim2_data_buffer = new int[3] { 1, 3, 4 };
var attr_data_buffer = new int[3] { 1, 2, 3 };

using (Array array_write = new Array(Context.GetDefault(), "my_array"))
{
    array_write.Open(QueryType.Write);
    var query_write = new Query(Ctx, array_write);
    query_write.SetLayout(LayoutType.Unordered);
    query_write.SetDataBuffer<int>("rows", dim1_data_buffer);
    query_write.SetDataBuffer<int>("cols", dim2_data_buffer);
    query_write.SetDataBuffer<int>("a", attr_data_buffer);
    query_write.Submit();
    array_write.Close();
}
```

## <span id="TILEDB0002">`TILEDB0002` - Data type is obsolete.</span>

Some members of the `DataType` enum are deprecated and should not be used. This table lists them, along with recommended replacements:

|Obsolete type|Replacement|Reason|
|-------------|-----------|------|
|`StringUcs2`|`StringUtf16`|UCS-2 does not support characters outside the Basic Multilingual Plane.|
|`StringUcs4`|`StringUtf32`|UTF-32 is a more established synonym of UCS-4.|
|`Char`|`Byte` or `StringAscii`|The meaning of `Char` is ambiguous.|
|`TILEDB_ANY`|_a more specific type, depending on the situation_||
